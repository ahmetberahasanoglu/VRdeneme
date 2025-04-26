using TMPro;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    public TMP_Text instructionText;
    void OnEnable()
    {
        PrescriptionButton.OnPrescriptionSelected += HandlePrescriptionSelected;
    }

    void OnDisable()
    {
        PrescriptionButton.OnPrescriptionSelected -= HandlePrescriptionSelected;
    }

    void HandlePrescriptionSelected()
    {
        instructionText.text = "";
        gameObject.SetActive(false);
    }
}
