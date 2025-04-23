using UnityEngine;

public class PrescriptionUIManager : MonoBehaviour
{
    public Prescription[] availablePrescriptions;
    public GameObject buttonPrefab;
    public Transform contentParent;

    void Start()
    {
        foreach (Prescription p in availablePrescriptions)
        {
            GameObject btn = Instantiate(buttonPrefab, contentParent);
            btn.GetComponent<PrescriptionButton>().Setup(p);
        }
    }
}
