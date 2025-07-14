using NUnit.Framework.Constraints;
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
    public Image leftButton;
    public Image rightButton;

    [Header("Texts")]
    public TextMeshProUGUI yatayCizgiDeger;
    public TextMeshProUGUI yatayCizgiDeger1;
    public TextMeshProUGUI DBLtext;
    public TextMeshProUGUI xTxt;
    public TextMeshProUGUI yText;
    public TextMeshProUGUI errorText; 
    public TextMeshProUGUI hintText; 

    [Header("Input Fields")]
    public TMP_InputField dikeyInputField;
    public TMP_InputField yatayInputField;

    private Prescription prescription;
    private bool tracerPressed = false;
    public bool olcumYapildi=false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null)//  if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
        else
            Debug.LogError("Prescription null!");

       // RandomizeYatayNoktaPosition(); random olmayacak
       getYatayNoktaPosition();

        dikeyCizgiButton.onClick.AddListener(() =>
        {
            if (!tracerPressed)
            {
                ShowError("Önce tracer butonuna basýnýz");
                HUDController.instance.DecreaseScore(5);
                return;
            }
            OnDikeyButtonClicked();
        });

        yatayCizgiButton.onClick.AddListener(() =>
        {
            if (!tracerPressed)
            {
                ShowError("Önce tracer butonuna basýnýz");
                HUDController.instance.DecreaseScore(5);
                return;
            }
            OnYatayButtonClicked();
        });

        tracerButton.onClick.AddListener(OnTracerButtonClicked);

        errorText.text = "";
    }
    private void CheckCorrectPosition()
    {
        float tolerance = 5f;
        Vector3 dikeyWorldX = dikeyCizgi.transform.position;
        Vector3 yatayWorldY = yatayCizgi.transform.position;
        Vector3 ortaWorld = ortaNokta.transform.position;
        Vector2 intersection = new Vector2(dikeyWorldX.x, yatayWorldY.y);
        float distance = Vector2.Distance(intersection, ortaWorld);

        if (distance <= tolerance)
        {
            olcumYapildi = true;
            errorText.text = "Doðru noktadasýn. Sonraki makineye geç!";
        }
        else
        {
            olcumYapildi = false;
            errorText.text = "Henüz doðru noktayý bulamadýn.";
        }
    }
  /*  private void RandomizeYatayNoktaPosition()
    {
        float x = Random.Range(15f, 300f);
        float y = Random.Range(-260f, 270f);
        yatayNoktalar.anchoredPosition = new Vector2(x, y);
      
    } random yapmak yerine her receteye uygun bir pozisyon atamayý düsünüyorum
  */
  private void getYatayNoktaPosition()
    {
       
        yatayNoktalar.anchoredPosition = new Vector2(prescription.pd*3, prescription.plus*4);
    }
    

    private void OnDikeyButtonClicked()
    {
        dikeyInputField.gameObject.SetActive(true);
        yatayInputField.gameObject.SetActive(false);
        dikeyInputField.onEndEdit.AddListener(SetDikeyCizgiPosition);
    }

    private void OnYatayButtonClicked()
    {
        yatayInputField.gameObject.SetActive(true);
        dikeyInputField.gameObject.SetActive(false);
        yatayInputField.onEndEdit.AddListener(SetYatayCizgiPosition);
    }

    private void SetDikeyCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            result = Mathf.Clamp(result, 0f, 100f);
            float movementResult = result * 3f;
            dikeyCizgi.anchoredPosition = new Vector2(movementResult, dikeyCizgi.anchoredPosition.y);

            xTxt.text = result.ToString("F2");
            dikeyCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = result.ToString("F2");
        }
        dikeyInputField.text = "";
        dikeyInputField.gameObject.SetActive(false);
        dikeyInputField.onEndEdit.RemoveAllListeners();

        CheckCorrectPosition();
    }

    private void SetYatayCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            result = Mathf.Clamp(result, -10f, 10f);
            float movementResult = result * 30f;
            yatayCizgi.anchoredPosition = new Vector2(yatayCizgi.anchoredPosition.x, movementResult);
            yText.text = result.ToString("+0.0;-0.0");
            yatayCizgiDeger.text = yText.text;
            yatayCizgiDeger1.text = yText.text;
        }
        yatayInputField.text = "";
        yatayInputField.gameObject.SetActive(false);
        yatayInputField.onEndEdit.RemoveAllListeners();
        CheckCorrectPosition();
    }

    private void OnTracerButtonClicked()
    {
        tracerPressed = true;
        errorText.text = "";
        tracerButton.interactable = false;
        hintText.text = $"Ýpucu: PD: {prescription.pd}";
        dikeyCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = "66,10";
        DBLtext.text = "15,40";
        yatayCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = "+2,0";
        yatayCizgiDeger.text = "+2,0";

       
        if (prescription != null)
        {
            if (prescription.leftRight)
            {
                leftButton.color= Color.red;
            }
            else
            {
                rightButton.color = Color.yellow;
            }
        }
    }

    private void ShowError(string message)
    {
        errorText.text = message;
        CancelInvoke(nameof(ClearError));
        Invoke(nameof(ClearError), 2f); 
    }

    private void ClearError()
    {
        errorText.text = "";
    }
}
