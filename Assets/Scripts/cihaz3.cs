using System.Collections;
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

    [Header("UI References")]
    public GameObject numpadPanel;
    public TextMeshProUGUI displayText;
    public Button[] numberButtons;
    public Button dotButton;
    public Button deleteButton;
    public Button confirmButton;
    public Button cancelButton;
    public Button eksiButton;

    [Header("Input Settings")]
    public int maxDigits = 6;
    public bool allowDecimals = true;

    private string currentInput = "";
    private System.Action<float> onValueConfirmed;
    private System.Action onValueCancelled;
    private bool isInputTypeVertical = true; 

    [Header("Texts")]
    public TextMeshProUGUI yatayCizgiDeger;
    public TextMeshProUGUI yatayCizgiDeger1;
    public TextMeshProUGUI DBLtext;
    public TextMeshProUGUI xTxt;
    public TextMeshProUGUI yText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI hintText;

    [Header("Input Fields")]
   // public TMP_InputField dikeyInputField;
   // public TMP_InputField yatayInputField;

    private Prescription prescription;
    private bool tracerPressed = false;
    public bool olcumYapildi = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
        else
            Debug.LogError("Prescription null!");

        getYatayNoktaPosition();
        SetupNumpadButtons();
        numpadPanel.SetActive(false);

        dikeyCizgiButton.onClick.AddListener(() =>
        {
            if (!tracerPressed)
            {
                ShowError("Önce tracer butonuna basýnýz");
                HUDController.instance.DecreaseScore(10);
                return;
            }
            ShowNumpadForDikey();
        });

        yatayCizgiButton.onClick.AddListener(() =>
        {
            if (!tracerPressed)
            {
                ShowError("Önce tracer butonuna basýnýz");
                HUDController.instance.DecreaseScore(10);
                return;
            }
            ShowNumpadForYatay();
        });

        tracerButton.onClick.AddListener(OnTracerButtonClicked);

        errorText.text = "";
    }

    private void SetupNumpadButtons()
    {
       
        for (int i = 0; i < numberButtons.Length && i < 10; i++)
        {
            int number = i;
            numberButtons[i].onClick.AddListener(() => OnNumberPressed(number.ToString()));
        }

    
        if (dotButton != null)
            dotButton.onClick.AddListener(() => OnNumberPressed(","));

        if (deleteButton != null)
            deleteButton.onClick.AddListener(OnDeletePressed);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmPressed);
        if (eksiButton != null)
            eksiButton.onClick.AddListener(OnMinusPressed);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelPressed);
    }

 
    public void ShowNumpadForDikey()
    {
        isInputTypeVertical = true;
        ShowNumpad((value) => {
            SetDikeyCizgiPosition(value);
        }, () => {
            Debug.Log("Dikey input cancelled");
        });
    }


    public void ShowNumpadForYatay()
    {
        isInputTypeVertical = false;
        ShowNumpad((value) => {
            SetYatayCizgiPosition(value);
        }, () => {
            Debug.Log("Yatay input cancelled");
        });
    }

    public void ShowNumpad(System.Action<float> onConfirm, System.Action onCancel = null)
    {
        numpadPanel.SetActive(true);
        currentInput = "";
        UpdateDisplay();

        onValueConfirmed = onConfirm;
        onValueCancelled = onCancel;
    }

    public void HideNumpad()
    {
        numpadPanel.SetActive(false);
        currentInput = "";
        onValueConfirmed = null;
        onValueCancelled = null;
    }

    private void OnNumberPressed(string number)
    {
      
        if (currentInput.Length >= maxDigits) return;

        if (number == ".")
        {
            if (!allowDecimals || currentInput.Contains(".")) return;
            if (currentInput == "") currentInput = "0";
        }
        if (currentInput == "-" && number != ".")
        {
            currentInput += number;
        }
        else
        {
            currentInput += number;
        }
        
        UpdateDisplay();
    }
    private void OnMinusPressed()
    {
        if (currentInput.Length == 0)
        {
            // Eðer hiç karakter yoksa - ekle
            currentInput = "-";
        }
        else if (currentInput == "-")
        {
            
            currentInput = "";
        }
        else if (currentInput.StartsWith("-"))
        {
            // Eðer - ile baþlýyorsa kaldýr
            currentInput = currentInput.Substring(1);
        }
        else
        {
           
            currentInput = "-" + currentInput;
        }

        UpdateDisplay();
    }
    private void OnDeletePressed()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    private void OnConfirmPressed()
    {
        if (string.IsNullOrEmpty(currentInput)) return;

        if (float.TryParse(currentInput, out float result))
        {
            onValueConfirmed?.Invoke(result);
            HideNumpad();
        }
        else
        {
            StartCoroutine(ShowErrorFeedback());
        }
    }
    private void OnEksiPressed()
    {

    }
    private void OnCancelPressed()
    {
        onValueCancelled?.Invoke();
        HideNumpad();
    }

    private void UpdateDisplay()
    {
        displayText.text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;
    }

    private IEnumerator ShowErrorFeedback()
    {
        Color originalColor = displayText.color;
        displayText.color = Color.red;
        displayText.text = "HATA!";

        yield return new WaitForSeconds(1f);

        displayText.color = originalColor;
        UpdateDisplay();
    }

    private void CheckCorrectPosition()
    {
        float tolerance = 5f;

        Vector2 dikeyLocal = ((RectTransform)dikeyCizgi).anchoredPosition;
        Vector2 yatayLocal = ((RectTransform)yatayCizgi).anchoredPosition;

        Vector2 ortaForLocal = ((RectTransform)yatayNoktalar).anchoredPosition;

        bool xDogru = Mathf.Abs(dikeyLocal.x - ortaForLocal.x) <= tolerance;
        bool yDogru = Mathf.Abs(yatayLocal.y - ortaForLocal.y) <= tolerance;

        if (xDogru && yDogru)
        {
            olcumYapildi = true;
            errorText.text = "Doðru noktadasýn. Sonraki makineye geç!";
        }
        else
        {
            olcumYapildi = false;
            errorText.text = "Henüz doðru noktayý bulamadýn.";
        }

        Debug.Log("Dikey pos" + dikeyLocal);
        Debug.Log("orta pos" + ortaForLocal);
        Debug.Log($"Dikey fark: {Mathf.Abs(dikeyLocal.x - ortaForLocal.x)} | Yatay fark: {Mathf.Abs(dikeyLocal.y - ortaForLocal.y)}");

    }

    private void getYatayNoktaPosition()
    {
        if (prescription != null)
        {
            yatayNoktalar.anchoredPosition = new Vector2(prescription.pd * 3, prescription.plus * 30);//3 4
        }
        else
        {
            Debug.LogError("Prescription null! Yatay nokta pozisyonu ayarlanamadý.");
        }
    }

    private void SetDikeyCizgiPosition(float value)
    {
        value = Mathf.Clamp(value, 0f, 100f);
        float movementResult = value * 3f;

        dikeyCizgi.anchoredPosition = new Vector2(movementResult, dikeyCizgi.anchoredPosition.y);

        xTxt.text = value.ToString("F2");
        dikeyCizgiButton.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString("F2");

        CheckCorrectPosition();
    }

    private void SetYatayCizgiPosition(float value)
    {
        value = Mathf.Clamp(value, -10f, 10f);
        float movementResult = value * 30f;

        yatayCizgi.anchoredPosition = new Vector2(yatayCizgi.anchoredPosition.x, movementResult);

        yText.text = value.ToString("+0.0;-0.0");
        yatayCizgiDeger.text = yText.text;
        yatayCizgiDeger1.text = yText.text;

        CheckCorrectPosition();
    }

    // Eski metotlar (inputfield ile çalýþan) - geriye dönük uyumluluk için
    /*
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

    // InputField için overload metotlar
    private void SetDikeyCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            SetDikeyCizgiPosition(result);
        }

        dikeyInputField.text = "";
        dikeyInputField.gameObject.SetActive(false);
        dikeyInputField.onEndEdit.RemoveAllListeners();
    }

    private void SetYatayCizgiPosition(string value)
    {
        if (float.TryParse(value, out float result))
        {
            SetYatayCizgiPosition(result);
        }

        yatayInputField.text = "";
        yatayInputField.gameObject.SetActive(false);
        yatayInputField.onEndEdit.RemoveAllListeners();
    }
    */
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
                leftButton.color = Color.red;
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
        AudioManager.Instance.PlaySound(AudioManager.Instance.warningSound, 0.9f);
        CancelInvoke(nameof(ClearError));
        Invoke(nameof(ClearError), 2f);
    }

    private void ClearError()
    {
        errorText.text = "";
    }
}

[System.Serializable]
public class NumpadButton
{
    public Button button;
    public string value;
}