using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cihaz4 : MonoBehaviour
{
    public static Cihaz4 instance;
    public RectTransform modelCagir;
    private Prescription prescription;
    public bool islemTamamlandi=false;
    public Slider islemSlider;
    public float stepDelay = 0.5f;
    public TextMeshProUGUI kesimState;

    public TextMeshProUGUI dikeyCizgiDeger;
    public TextMeshProUGUI yatayCizgiDeger;
    public TextMeshProUGUI yatayCizgiDeger2;
    public TMP_Dropdown Cam;
    public TMP_Dropdown Cerceve;
    public TMP_Dropdown Mod;
    public TMP_Dropdown Polisaj;
    public TMP_Dropdown Capak;
    public TMP_Dropdown Odaklama;

    private bool isMeasuring = false;
    public bool kesimYapildi = false;
    private void Awake()
    {
        instance = this;
    }
    void rotateModelIfLeft()
    {
        modelCagir.rotation= new Quaternion(0,-180 ,0,0);
    }
    public void onPressedZero()
    {
        modelCagir.gameObject.SetActive(true);
        //cihaz3'de bitirirken sahip oldugumuz yataycizgidegeri ve dikeyCizgiDegerini al ve
        // buradaki yataycizgideger ve dikeycizgidegerlerine eþitle
    }
    private void Start()
    {
        if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.selectedPrescription;
        else
            Debug.LogError("Prescription null!");
    }
    public void BaslatOlcum()
    {
        if (isMeasuring || kesimYapildi) return;

        StartCoroutine(OlcumRutini());
    }

    private IEnumerator OlcumRutini()
    {
        isMeasuring = true;
        islemSlider.value = 0f;
        kesimState.text = "Kesim Baþlýyor...";

        yield return new WaitForSeconds(stepDelay);
        islemSlider.value = 0.33f;
        kesimState.text = "Merceðe Su döküldü...";

        yield return new WaitForSeconds(stepDelay);
        islemSlider.value = 0.66f;
        kesimState.text = "Kenarlar temizlendi";

        yield return new WaitForSeconds(stepDelay);
        islemSlider.value = 0.99f;

        yield return new WaitForSeconds(stepDelay);
        islemSlider.value = 1f;
        kesimState.text = "Ölçüm Tamamlandý";

        kesimYapildi = true;
        isMeasuring = false;
    }
}
