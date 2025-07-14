    using UnityEngine;

    [CreateAssetMenu(fileName = "New Prescription", menuName = "Prescription")]
    public class Prescription : ScriptableObject
    {
        public string odaklama;
        public string cam;
        public string frameType;
        public string mod;
        public string prescriptionName;
        public float sphere;//-7 - +7 arasý olmalý rastgele
        public float cylinder;//-6 +6 arasý olmalý rastgele
        public int axis; //0- 180 arasý rastgele deðer olmalý
   
        public bool leftRight;
 
        public bool polisaj;
        public bool capak;
  
        public float x;
        public float y;
        public float pd;
        public float plus;

    public void GenerateRandomValues()
    {
        sphere = Random.Range(-28, 29) * 0.25f;  
        cylinder = Random.Range(-24, 25) * 0.25f; 
        axis = Random.Range(0, 20);//180 yap
        System.Random rand = new System.Random();
        pd = rand.Next(0, 7001) / 100f;
        System.Random rand1 = new System.Random();
        plus = rand1.Next(-800, 801) / 100f;


    }


}
