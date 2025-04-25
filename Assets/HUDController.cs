using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
  public static HUDController instance;
    

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] TMP_Text interactionText;
    
    public void EnableInteractionText(string text)
    {
        interactionText.text = text +" (E)";
        interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText() {
        interactionText.gameObject.SetActive(false);

    }
    public void ShowResult(int score)
    {
        resultText.text = "Puanýnýz: " + score + " / 5";
        resultPanel.SetActive(true);
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
    public void HidePrescriptionPanel()
    {
        //
    }
    public void ShowStartInstruction()
    {
        //
    }
}
