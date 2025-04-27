using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("Interaction")]
    [SerializeField] TMP_Text interactionText;

    [Header("Prescription Panel")]
    [SerializeField] GameObject prescriptionPanel;
    [SerializeField] TMP_Text prescriptionText;
    [SerializeField] GameObject warningMessage;
    GraphicRaycaster raycaster;

    private bool isTaskCompleted = false;

    private void Awake()
    {
        instance = this;
        raycaster = GetComponent<GraphicRaycaster>();   
    }

    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void ShowPrescriptionPanel()
    {
        isTaskCompleted = false; 
        prescriptionPanel.SetActive(true);
        raycaster.enabled = true;
        LockPlayerControls();
        Prescription prescription = GameManager.Instance.selectedPrescription;
        prescriptionText.text = $"Reçete: SPH: {prescription.sphere} CYL: {prescription.cylinder} AXIS: {prescription.axis}";
    }

    public void CompleteCurrentTask()
    {
        isTaskCompleted = true;
        HidePrescriptionPanel();
        MachineManager.Instance.CompleteCurrentMachine();
    }

    public void TryHidePrescriptionPanel()
    {
        if (isTaskCompleted)
        {
            HidePrescriptionPanel();
        }
        else
        {
            ShowWarningMessage("Ýþlem tamamlanmadan çýkamazsýn!");
        }
    }

    public void HidePrescriptionPanel()
    {
        prescriptionPanel.SetActive(false);
        raycaster.enabled = false;
        UnlockPlayerControls();
    }

    private void ShowWarningMessage(string message)
    {
        warningMessage.SetActive(true);
        warningMessage.GetComponentInChildren<TMP_Text>().text = message;
        // warning message'i belirli saniyelik gösterme yapýlabilr sonra
    }
    private void LockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.LockControls();
        MouseLook.instance.LockMouseLooking();
    }

    private void UnlockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.instance.UnlockControls();
        MouseLook.instance.UnlockMouseLooking();
    }
}
