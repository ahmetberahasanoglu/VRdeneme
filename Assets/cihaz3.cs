using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cihaz3 : MonoBehaviour
{
    public static cihaz3 instance;

    [Header("UI Elements")]
    public RectTransform dikeyCizgi;
    public RectTransform yatayCizgi;
    public RectTransform ortaNokta;
    public RectTransform yatayNoktalar;
    public Button tracerButton;
    public Button dikeyCizgiButton; // pd
    public Button yatayCizgiButton; // +-

    [Header("Texts")]
    public TextMeshProUGUI yatayCizgiDeger;
    public TextMeshProUGUI DBLtext;
    public TextMeshProUGUI xTxt; // XText = dikeyCizgiButton.text olck
    public TextMeshProUGUI yText; // yText = yatayCizgiButton.text

    [Header("Input Fields")]
    public TMP_InputField dikeyInputField;
    public TMP_InputField yatayInputField;

    private Prescription prescription;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.selectedPrescription;
     

        RandomizeYatayNoktaPosition();

        dikeyCizgiButton.onClick.AddListener(OnDikeyButtonClicked);
        yatayCizgiButton.onClick.AddListener(OnYatayButtonClicked);
        tracerButton.onClick.AddListener(OnTracerButtonClicked);
    }

    private void RandomizeYatayNoktaPosition()
    {
        float x = Random.Range(15f, 300f);
        float y = Random.Range(-260f, 270f);
        yatayNoktalar.anchoredPosition = new Vector2(x, y);
        UpdateOrtaNokta();
    }

    private void UpdateOrtaNokta()
    {
        ortaNokta.anchoredPosition = yatayNoktalar.anchoredPosition;
    }

    private void OnDikeyButtonClicked()
    {
        dikeyInputField.gameObject.SetActive(true);
        dikeyInputField.onEndEdit.AddListener(SetDikeyCizgiPosition);
    }

    private void OnYatayButtonClicked()
    {
        yatayInputField.gameObject.SetActive(true);
        yatayInputField.onEndEdit.AddListener(SetYatayCizgiPosition);
    }

    private void SetDikeyCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            result = Mathf.Clamp(result, 0f, 100f);
            dikeyCizgi.anchoredPosition = new Vector2(result, dikeyCizgi.anchoredPosition.y);
            xTxt.text = result.ToString("F2");
        }
        dikeyInputField.text = "";
        dikeyInputField.gameObject.SetActive(false);
        dikeyInputField.onEndEdit.RemoveAllListeners();
    }

    private void SetYatayCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            result = Mathf.Clamp(result, -10f, 10f);
            yatayCizgi.anchoredPosition = new Vector2(yatayCizgi.anchoredPosition.x, result);
            yText.text = result.ToString("+0.0;-0.0");
            yatayCizgiDeger.text = yText.text;
        }
        yatayInputField.text = "";
        yatayInputField.gameObject.SetActive(false);
        yatayInputField.onEndEdit.RemoveAllListeners();
    }

    private void OnTracerButtonClicked()
    {
       
        dikeyCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = "66.10";
        DBLtext.text = "15.40";
        yatayCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = "+2.0";
        yatayCizgiDeger.text = "+2.0";

        dikeyCizgi.anchoredPosition = new Vector2(ortaNokta.anchoredPosition.x, dikeyCizgi.anchoredPosition.y);
        yatayCizgi.anchoredPosition = new Vector2(yatayCizgi.anchoredPosition.x, ortaNokta.anchoredPosition.y);
    }
}
