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
        prescriptionSphere.text = prescription.sphere.ToString();
        prescriptionAxis.text = prescription.axis.ToString();
        prescriptionCylinder.text = prescription.cylinder.ToString();
    }

    public void OnClick()
    {
        GameManager.Instance.StartGame(prescription);
        Debug.Log("Seçilen reçete: " + myPrescription.prescriptionName);

        // UI'de bir sonraki adýma geçilebilir, örneðin:
        UIManager.Instance.HidePrescriptionPanel();
        UIManager.Instance.ShowStartInstruction();
    }
}
