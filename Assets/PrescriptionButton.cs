using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrescriptionButton : MonoBehaviour
{
    public TextMeshProUGUI prescriptionNameText;
    public TextMeshProUGUI prescriptionSphere;
    public TextMeshProUGUI prescriptionAxis;
    public TextMeshProUGUI prescriptionCylinder;
    public TextMeshProUGUI cam;
    public TextMeshProUGUI frametype;
    public TextMeshProUGUI mod;
    public TextMeshProUGUI odaklama;
    private Prescription prescription;
   

    public void Setup(Prescription _prescription)
    {
        prescription = _prescription;
        if (cam == null) Debug.LogError("cam referans� null!");
        if (mod == null) Debug.LogError("mod referans� null!");
        if (frametype == null) Debug.LogError("frametype referans� null!");
        if (odaklama == null) Debug.LogError("odaklama referans� null!");
        prescriptionNameText.text = prescription.prescriptionName;
        cam.text = prescription.cam;
        mod.text = prescription.mod;
        frametype.text = prescription.frameType;
        odaklama.text = prescription.odaklama;
        prescriptionSphere.text = prescription.sphere.ToString("F2");
        prescriptionAxis.text = prescription.axis.ToString("F0");
        prescriptionCylinder.text = prescription.cylinder.ToString("F2");
       // cam.text = prescription.cam;
       // mod.text = prescription.mod;
       // frametype.text = prescription.frameType;
       // odaklama.text = prescription.odaklama;
    }

    public static event System.Action OnPrescriptionSelected;
    public void OnClick()
    {
        GameManager.Instance.StartGame(prescription);
        Debug.Log("Se�ilen re�ete: " + prescription.prescriptionName);
        OnPrescriptionSelected?.Invoke();
        HUDController.instance.HidefokoPanel();
        //HUDController.instance.ShowStartInstruction();
    }
}
