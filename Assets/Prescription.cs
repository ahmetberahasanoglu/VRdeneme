using UnityEngine;

[CreateAssetMenu(fileName = "New Prescription", menuName = "Prescription")]
public class Prescription : ScriptableObject
{
    public string prescriptionName;
    public float sphere;
    public float cylinder;
    public int axis;
    public string lensType;
    public bool leftRight;
    public string frameType;
    public bool polisaj;
    public string nosePadType;
    public string screwType;
}
