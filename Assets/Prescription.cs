using UnityEngine;

[CreateAssetMenu(fileName = "New Prescription", menuName = "Prescription")]
public class Prescription : ScriptableObject
{
    public string odaklama;
    public string cam;
    public string frameType;
    public string mod;
    public string prescriptionName;
    public float sphere;
    public float cylinder;
    public int axis;
   
    public bool leftRight;
 
    public bool polisaj;
    public bool capak;
  
    public float x;
    public float y;
    public float pd;
    public float plus;
    
}
