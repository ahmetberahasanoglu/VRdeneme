using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrescriptionButton : MonoBehaviour
{
    public TextMeshProUGUI prescriptionNameText;
    public TextMeshProUGUI prescriptionSphere;
    public TextMeshProUGUI prescriptionAxis;
    public TextMeshProUGUI prescriptionCylinder;
    private Prescription prescription;
   

    public void Setup(Prescription _prescription)
    {
        prescription = _prescription;
        prescriptionNameText.text = prescription.prescriptionName;
        prescriptionSphere.text = prescription.sphere.ToString("F2");
        prescriptionAxis.text = prescription.axis.ToString("F0");
        prescriptionCylinder.text = prescription.cylinder.ToString("F2");
    }
    public static event System.Action OnPrescriptionSelected;
    public void OnClick()
    {
        GameManager.Instance.StartGame(prescription);
        Debug.Log("Seçilen reçete: " + prescription.prescriptionName);
        OnPrescriptionSelected?.Invoke();
        HUDController.instance.HidefokoPanel();
        //HUDController.instance.ShowStartInstruction();
    }
}
