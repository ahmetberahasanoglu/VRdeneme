using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class fokometre : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static fokometre Instance;

    [Header("UI References")]
    public TMP_Text sphereText;
    public TMP_Text cylinderText;
    public TMP_Text axisText;
    public TMP_Text markingStatusText;
    public RectTransform crosshair;
    public RectTransform targetCenter;

    [Header("Crosshair Constraints")]
    public float crosshairMinX = -150f;
    public float crosshairMaxX = 150f;
    public float crosshairMinY = -150f;
    public float crosshairMaxY = 150f;

    [Header("VR Input References")]
    public XRRayInteractor rightHandRayInteractor;
    public XRRayInteractor leftHandRayInteractor;

    [Header("VR Settings")]
    public float crosshairSensitivity = 100f;
    public bool useVRMode = true;

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

    // VR Input Actions
    private InputDevice rightHandDevice;
    private InputDevice leftHandDevice;
    private bool wasRightTriggerPressed = false;
    private bool wasLeftTriggerPressed = false;

    private Prescription prescription;
    private Canvas parentCanvas;

    private void Awake()
    {
        Instance = this;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
        else
            Debug.LogError("Prescription null!");

        RandomizeCrosshairPosition();
        DetectVRMode();
        InitializeVRInput();
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            axisInputTimer += Time.deltaTime;

            if (useVRMode)
            {
                HandleVRInput();
            }
            else
            {
                HandlePCInput();
            }

            CheckMarkingStatus();
        }
    }

    private void DetectVRMode()
    {
        useVRMode = XRSettings.isDeviceActive;
    }

    private void InitializeVRInput()
    {
        if (useVRMode)
        {
            var rightHandDevices = new List<InputDevice>();
            var leftHandDevices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
            if (rightHandDevices.Count > 0)
                rightHandDevice = rightHandDevices[0];
            if (leftHandDevices.Count > 0)
                leftHandDevice = leftHandDevices[0];
        }
    }

    #region VR Input Handling

    private void HandleVRInput()
    {
        HandleVRAxisInput();
        HandleVRTriggerInput();
    }

    private void HandleVRAxisInput()
    {
        // Sol el ile axis kontrolü
        if (leftHandDevice.isValid)
        {
            // Primary2DAxis (joystick) ile axis kontrolü
            if (leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftJoystick))
            {
                if (Mathf.Abs(leftJoystick.x) > 0.3f && axisInputTimer >= axisInputDelay)
                {
                    if (leftJoystick.x < -0.3f)
                    {
                        currentAxis--;
                        if (currentAxis < 0) currentAxis = 180;
                        axisInputTimer = 0f;
                        SendHapticFeedback(leftHandDevice, 0.2f, 0.1f);
                        UpdateAxisText();
                    }
                    else if (leftJoystick.x > 0.3f)
                    {
                        currentAxis++;
                        if (currentAxis > 180) currentAxis = 0;
                        axisInputTimer = 0f;
                        SendHapticFeedback(leftHandDevice, 0.2f, 0.1f);
                        UpdateAxisText();
                    }
                }
            }
        }
    }

    private void HandleVRTriggerInput()
    { 
        if (rightHandDevice.isValid)
        {
            if (rightHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerPressed))
            {
                if (rightTriggerPressed && !wasRightTriggerPressed)
                {
                    OnVRDragStart();
                }
                else if (!rightTriggerPressed && wasRightTriggerPressed)
                {
                    OnVRDragEnd();
                }
                else if (rightTriggerPressed && isDragging)
                {
                    OnVRDrag();
                }

                wasRightTriggerPressed = rightTriggerPressed;
            }
        }
        if (leftHandDevice.isValid)
        {
            if (leftHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerPressed))
            {
                if (leftTriggerPressed && !wasLeftTriggerPressed && !sphereSet)
                {
                    ConfirmSphereCylinder();
                    SendHapticFeedback(leftHandDevice, 0.4f, 0.15f);
                }

                wasLeftTriggerPressed = leftTriggerPressed;
            }
        }
    }

    private void OnVRDragStart()
    {
        isDragging = true;
        SendHapticFeedback(rightHandDevice, 0.3f, 0.1f);
        Debug.Log("VR Drag Started");
    }

    private void OnVRDrag()
    {
        if (rightHandRayInteractor != null && rightHandRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(hit.point);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                crosshair.parent as RectTransform,
                screenPoint,
                parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                out Vector2 localPoint))
            {
                Vector2 clampedPos = ClampCrosshairPosition(localPoint);
                crosshair.anchoredPosition = clampedPos;

                
                SendHapticFeedback(rightHandDevice, 0.05f, 0.02f);
            }
        }
    }

    private void OnVRDragEnd()
    {
        isDragging = false;
        SendHapticFeedback(rightHandDevice, 0.2f, 0.08f);
        Debug.Log("VR Drag Ended");
    }

    private void SendHapticFeedback(InputDevice device, float intensity, float duration)
    {
        if (device.isValid)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, intensity, duration);
            }
        }
    }

    #endregion

    #region PC Input Handling & UI Event System

    private void HandlePCInput()
    {
        HandleAxisInput();
        HandleCrosshairDrag();
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

            axisInputTimer = 0f;
            UpdateAxisText();
        }
    }

    private void HandleCrosshairDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                crosshair.parent as RectTransform,
                Input.mousePosition,
                parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                out localMousePosition))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(crosshair, Input.mousePosition,
                    parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main))
                {
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                crosshair.parent as RectTransform,
                Input.mousePosition,
                parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                out localMousePosition))
            {
                Vector2 clampedPosition = ClampCrosshairPosition(localMousePosition);
                crosshair.anchoredPosition = clampedPosition;
            }
        }
    }

    // Unity Event System interface implementations (UI drag için)
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                crosshair.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
            {
                Vector2 clampedPos = ClampCrosshairPosition(localPoint);
                crosshair.anchoredPosition = clampedPos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    #endregion

    #region Utility Methods

    private Vector2 ClampCrosshairPosition(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, crosshairMinX, crosshairMaxX);
        float clampedY = Mathf.Clamp(position.y, crosshairMinY, crosshairMaxY);
        return new Vector2(clampedX, clampedY);
    }

    public void arttirAxisButton()
    {
        currentAxis++;
        if (currentAxis > 180) currentAxis = 0;
        UpdateAxisText();

        if (useVRMode && rightHandDevice.isValid)
        {
            SendHapticFeedback(rightHandDevice, 0.2f, 0.1f);
        }
    }

    public void azaltAxisButton()
    {
        currentAxis--;
        if (currentAxis < 0) currentAxis = 180;
        UpdateAxisText();

        if (useVRMode && rightHandDevice.isValid)
        {
            SendHapticFeedback(rightHandDevice, 0.2f, 0.1f);
        }
    }

    private void UpdateAxisText()
    {
        axisText.text = "Axis: " + currentAxis.ToString();
    }

    #endregion

    #region Game Logic

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
        else if (sphereSet && cylinderSet && markingOK)
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
        else if (markingOK && isCrosshairCentered)
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
        Debug.Log("Sphere/Cylinder confirmed!");
    }

    private void UpdateSphereCylinderText()
    {
        sphereText.text = currentSphere.ToString("F2");
        cylinderText.text = currentCylinder.ToString("F2");
    }

    private void RandomizeCrosshairPosition()
    {
        float x = Random.Range(crosshairMinX, crosshairMaxX);
        float y = Random.Range(crosshairMinY, crosshairMaxY);
        crosshair.anchoredPosition = new Vector2(x, y);
    }

    #endregion
}