using UnityEngine;

[CreateAssetMenu(fileName = "New Prescription", menuName = "Prescription")]
public class Prescription : ScriptableObject
{
    public string prescriptionName;
    public float sphere;
    public float cylinder;
    public int axis;
   
    public bool leftRight;
    public string cam;
    public string frameType;
    public string mod;
    public bool polisaj;
    public bool capak;
    public string odaklama;
}
