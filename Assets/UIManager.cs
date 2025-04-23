using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
