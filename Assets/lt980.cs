using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lt980 : MonoBehaviour
{
    public static lt980 Instance;

    [Header("UI")]
    public Button olcuAl;               
    public Slider olcuSlider;           
    public TextMeshProUGUI olcumState;  

    [Header("Ayarlar")]
    public float stepDelay = 0.5f;      

    [HideInInspector]
    public bool olcumYapıldı = false;

    private bool isMeasuring = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        olcuAl.onClick.AddListener(BaslatOlcum);
        ResetPanel();
    }

    public void BaslatOlcum()
    {
        if (isMeasuring || olcumYapıldı) return;

        StartCoroutine(OlcumRutini());
    }

    private IEnumerator OlcumRutini()
    {
        isMeasuring = true;
        olcuSlider.value = 0f;
        olcumState.text = "Ölçüm Başlıyor...";

        yield return new WaitForSeconds(stepDelay);
        olcuSlider.value = 0.33f;
        olcumState.text = "Ölçüm Devam Ediyor...";

        yield return new WaitForSeconds(stepDelay);
        olcuSlider.value = 0.66f;

        yield return new WaitForSeconds(stepDelay);
        olcuSlider.value = 0.99f;

        yield return new WaitForSeconds(stepDelay);
        olcuSlider.value = 1f;
        olcumState.text = "Ölçüm Tamamlandı ✅";

        olcumYapıldı = true;
        isMeasuring = false;
    }

    public void ResetPanel()
    {
        olcuSlider.value = 0f;
        olcumState.text = "Hazır.";
        olcumYapıldı = false;
        isMeasuring = false;
    }


}
