using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class fokometre : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text sphereText;
    public TMP_Text cylinderText;
    public TMP_Text axisText;
    public TMP_Text markingStatusText;

    [Header("Target Prescription Values")]
    private float targetSphere;
    private float targetCylinder;
    private int targetAxis;

    private float currentSphere = 0f;
    private float currentCylinder = 0f;
    private int currentAxis = 0;

    private bool sphereSet = false;
    private bool cylinderSet = false;
    private bool markingOK = false;

    private Prescription prescription;
    private void Start()
    {
        if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.selectedPrescription;
        else
            Debug.LogError("Prescription null!");
    }




    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            HandleAxisInput();
            CheckMarkingStatus();
        }
    }

    private void HandleAxisInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentAxis--;
            if (currentAxis < 0) currentAxis = 180;
            UpdateAxisText();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentAxis++;
            if (currentAxis > 180) currentAxis = 0;
            UpdateAxisText();
        }
    }

    public void ConfirmSphereCylinder()
    {
        currentSphere = targetSphere;
        currentCylinder = targetCylinder;

        sphereSet = true;
        cylinderSet = true;
    }

    private void UpdateSphereCylinderText()
    {
        sphereText.text =  currentSphere.ToString("F2");
        cylinderText.text = currentCylinder.ToString("F2");
    }

    private void UpdateAxisText()
    {
        axisText.text = "Axis: " + currentAxis.ToString();
    }

    private void CheckMarkingStatus()
    {
        if (currentAxis == targetAxis)
        {
            markingStatusText.text = "Marking OK";
            markingOK = true;
        }
        else
        {
            markingStatusText.text = "Adjusting...";
            markingOK = false;
        }

        CheckIfTaskCompleted();
    }

    private void CheckIfTaskCompleted()
    {
        if (sphereSet && cylinderSet && markingOK)
        {
            HUDController.instance.CompleteCurrentTask();
        }
    }
    
    public void SetupPrescription()
    {
        targetSphere = prescription.sphere;
        targetCylinder = prescription.cylinder;
        targetAxis = prescription.axis;

        ResetValues();
    }

    private void ResetValues()
    {
        currentSphere = 0f;
        currentCylinder = 0f;
        currentAxis = 0;
        sphereSet = false;
        cylinderSet = false;
        markingOK = false;

        sphereText.text = "Sphere: --";
        cylinderText.text = "Cylinder: --";
        axisText.text = "Axis: 0";
        markingStatusText.text = "Adjusting...";
    }
}
