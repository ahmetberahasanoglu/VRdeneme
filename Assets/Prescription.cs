using UnityEngine;

[CreateAssetMenu(fileName = "New Prescription", menuName = "Prescription")]
public class Prescription : ScriptableObject
{
    public string prescriptionName;
    public float sphere;
    public float cylinder;
    public int axis;
    public string lensType;
    public string frameType;
    public string coatingType;
    public string nosePadType;
    public string screwType;
}
