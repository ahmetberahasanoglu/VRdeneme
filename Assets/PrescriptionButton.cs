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

    public void OnClick()
    {
        GameManager.Instance.StartGame(prescription);
        Debug.Log("Se�ilen re�ete: " + prescription.prescriptionName);

        // UI'de bir sonraki ad�ma ge�ilebilir, �rne�in:
        HUDController.instance.HidePrescriptionPanel();
        HUDController.instance.ShowStartInstruction();
    }
}
