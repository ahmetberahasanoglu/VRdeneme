using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class fokometre : MonoBehaviour
{
    public static fokometre Instance;
    [Header("UI References")]
    public TMP_Text sphereText;
    public TMP_Text cylinderText;
    public TMP_Text axisText;
    public TMP_Text markingStatusText;
    public RectTransform crosshair;
    public RectTransform targetCenter;

    [Header("Target Prescription Values")]
    [SerializeField] private float targetSphere;
    [SerializeField] private float targetCylinder;
    private int targetAxis;

    private float currentSphere = 0f;
    private float currentCylinder = 0f;
    private int currentAxis = 0;

    private bool sphereSet = false;
    private bool cylinderSet = false;
    private bool markingOK = false;
    private bool isCrosshairCentered = false;

    private bool isDragging = false;
    private float axisInputDelay = 0.1f; 
    private float axisInputTimer = 0f;

    private Prescription prescription;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
        else
            Debug.LogError("Prescription null!");

        RandomizeCrosshairPosition();
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            axisInputTimer += Time.deltaTime;
            HandleAxisInput();
            HandleCrosshairDrag();
            CheckMarkingStatus();
            UpdateAxisText();
        }
    }

    private void HandleAxisInput()
    {
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && axisInputTimer >= axisInputDelay)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                currentAxis--;
                if (currentAxis < 0) currentAxis = 180;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                currentAxis++;
                if (currentAxis > 180) currentAxis = 0;
            }

            UpdateAxisText();
            axisInputTimer = 0f; 
        }
    }

    private void HandleCrosshairDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse tiklandi");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == crosshair.transform)
                {
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse birakildi");
            isDragging = false;
        }

        if (isDragging)
        {
            Debug.Log("surukluoz");
            Plane plane = new Plane(Vector3.forward, crosshair.position); 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                crosshair.position = worldPos;
            }
        }
    }
    public bool IsAllConditionsMet()
    {
        return sphereSet && cylinderSet && markingOK && isCrosshairCentered;
    }
    private void CheckMarkingStatus()
    {
        if (currentAxis == prescription.axis)
        {

            markingOK = true;
        }
        else
        {
            markingStatusText.text = "Adjusting...";
            markingOK = false;
        }

        CheckCrosshairCentered();
        CheckIfTaskCompleted();
    }

    private void CheckCrosshairCentered()
    {
        float distance = Vector2.Distance(crosshair.anchoredPosition, targetCenter.anchoredPosition);
        isCrosshairCentered = distance < 10f; 
    }

    private void CheckIfTaskCompleted()
    {
        
        if (sphereSet && cylinderSet && markingOK && isCrosshairCentered)
        {
            markingStatusText.text = "Marking OK";
        }
        else if (sphereSet && cylinderSet &&markingOK)
        {
            markingStatusText.text = "Need Alignment";
        }
        else if (isCrosshairCentered)
        {
            markingStatusText.text = "Alignment OK";
        }
        else if (markingOK)
        {
            markingStatusText.text = "Axis OK";
        }
        else if(markingOK&& isCrosshairCentered)
        {
            markingStatusText.text = "Press +- Button";
        }
       
        else
        {
            markingStatusText.text = "Adjusting...";
        }
    }

    public void ConfirmSphereCylinder()
    {
        currentSphere = prescription.sphere;
        currentCylinder = prescription.cylinder;

        sphereSet = true;
        cylinderSet = true;

        UpdateSphereCylinderText();
    }

    private void UpdateSphereCylinderText()
    {
        sphereText.text = currentSphere.ToString("F2");
        cylinderText.text = currentCylinder.ToString("F2");
    }

    private void UpdateAxisText()
    {
        axisText.text = "Axis: " + currentAxis.ToString();
    }

 

    private void RandomizeCrosshairPosition()
    {
        float x = Random.Range(-150f, 150f);
        float y = Random.Range(-150f, 150f);
        crosshair.anchoredPosition = new Vector2(x, y);
    }
}
