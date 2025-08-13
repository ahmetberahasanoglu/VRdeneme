using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
 
    //[Header("Interaction")]
    //[SerializeField] TMP_Text interactionText;

    [Header("FokoPanel")]
    [SerializeField] GameObject fokoPanel;
    
    [SerializeField] TextMeshProUGUI prescriptionText;
    [SerializeField] GameObject warningMessage;

    [Header("Cihaz2Panel")]
    [SerializeField] GameObject cihaz2Panel;
    [SerializeField] TextMeshProUGUI simpleText;

    //GraphicRaycaster raycaster;
    
    [SerializeField] GameObject cihaz3Panel;
    [SerializeField] GameObject cihaz4Panel;
    [SerializeField] GameObject hocaPanel;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public Transform playerCamera;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI performanceText;
    private Prescription prescription;
    private int score = 100;


    private bool isTaskCompleted = false;
    public float warningDisplayDuration = 2f;
    public AudioSource audioSource;
    public AudioClip failClip;
    public AudioClip positiveClip;
    public AudioClip fireClip;
    public AudioClip buttonClip;

    private Coroutine warningCoroutine;


    public GameObject replyPanel;

    private void Awake()
    {
        instance = this;
     //  raycaster = GetComponent<GraphicRaycaster>();   
    }
    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null) //if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            replyPanel.SetActive(true);
        }
    }
    public void DecreaseScore(int amount)
    {
        GameManager.Instance.AddPenalty(amount);
        audioSource.PlayOneShot(failClip);
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "Baþarý Notu: " + score;
    }
    public void UpdateTimer(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    void LateUpdate()
    {
        if (gameOverPanel.activeSelf)
        {
            gameOverPanel.transform.LookAt(playerCamera);
            gameOverPanel.transform.Rotate(0, 180, 0);
        }
    }
    public void onIsiticiInteracted()
    {
        audioSource.PlayOneShot(fireClip);
    }
    public void EndGame(int finalScore, float elapsedTime, int totalPenalties)
    {
        gameOverPanel.SetActive(true);

        gameOverPanel.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1f;
        gameOverPanel.transform.LookAt(playerCamera.transform);
        gameOverPanel.transform.Rotate(0, 180, 0);

        finalScoreText.text = $"Notunuz: {finalScore}";
        performanceText.text = $"Geçen Süre: {elapsedTime:F0} sn\n" +
                                $"Hata Sayýsý: {totalPenalties}\n" +
                                $"{(finalScore >= 50 ? "<color=green>Geçtiniz</color>" : "<color=red>Kaldýnýz</color>")}";
    }
    /*  public void EnableInteractionText(string text)
      {
          interactionText.text = text + " (E)";
          interactionText.gameObject.SetActive(true);
      }

      public void DisableInteractionText()
      {
          interactionText.gameObject.SetActive(false);
      }
    */
    public void ShowfokoPanel()
    {
        isTaskCompleted = false; 
     
        fokoPanel.SetActive(true);
        //raycaster.enabled = true;
      //  LockPlayerControls();
        Prescription prescription = GameManager.Instance.currentPrescription;
        prescriptionText.text = $"Reçete: SPH: {prescription.sphere} CYL: {prescription.cylinder} AXIS: {prescription.axis}";
       
    }
    public void ShowCihaz3Panel()
    {
        Debug.Log("panel goster.");
        isTaskCompleted = false;

        cihaz3Panel.SetActive(true);
       // raycaster.enabled = true;
       // LockPlayerControls();
        
    }
    public void ShowCihaz4Panel()
    {     
        isTaskCompleted = false;

        cihaz4Panel.SetActive(true);
     //   raycaster.enabled = true;
       // LockPlayerControls();

    }
    public void ShowHocaPanel()
    {
        hocaPanel.SetActive(true);
    //   raycaster.enabled = true;
       // LockPlayerControls();

    }
    public void HideHocaPanel()
    {
        hocaPanel.SetActive(false);
      // raycaster.enabled = false;
      //  UnlockPlayerControls();
    }
    public void ShowGlassPanel()
    {
        isTaskCompleted=false;
        cihaz2Panel.SetActive(true);
   
    //   raycaster.enabled=true;
      //  LockPlayerControls();
    }
    public void ButtonSound()
    {
        audioSource.PlayOneShot(buttonClip);
    }
    public void CompleteCurrentTask()
    {
        isTaskCompleted = true;
        audioSource.PlayOneShot(positiveClip);
        HidefokoPanel();
        MachineManager.Instance.NextMachine();
    }
    public void ChangeGlass()
    {
        prescription.leftRight = false;
    }

    public void TryHidefokoPanel()
    {
        if (fokometre.Instance.IsAllConditionsMet())
        {
            CompleteCurrentTask();
        }
        else
        {
            ShowWarningMessage("Tüm deðerleri doðru girmeden iþlemi tamamlayamazsýn!");
        }
    }
    public void TryHideLtPanel()
    {
        if (lt980.Instance.olcumYapýldý)
        {
            isTaskCompleted = true;
            cihaz2Panel.SetActive(false);
        //  raycaster.enabled = false;
        //    UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }
       
    }
    public void TryHideCihaz3Panel()
    {
        if (cihaz3.instance.olcumYapildi)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.stickSound, 0.9f);
            isTaskCompleted = true;
            cihaz3Panel.SetActive(false);
      //   raycaster.enabled = false;
       //     UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }
        else
        {
           audioSource.PlayOneShot(failClip);
        }

    }
    public void TryHideCihaz4Panel()
    {
        if (Cihaz4.instance.islemTamamlandi)
        {
            isTaskCompleted = true;
            cihaz4Panel.SetActive(false);
          // raycaster.enabled = false;
        //    UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }

    }

    public void HidefokoPanel()
    {
        fokoPanel.SetActive(false);
     //  raycaster.enabled = false;
     //   UnlockPlayerControls();
    }

    private void ShowWarningMessage(string message)
    {
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine); 
        }

        warningCoroutine = StartCoroutine(ShowWarningRoutine(message));
    }
    private IEnumerator ShowWarningRoutine(string message)
    {
        warningMessage.SetActive(true);
        warningMessage.GetComponentInChildren<TMP_Text>().text = message;
        DecreaseScore(10);

        yield return new WaitForSeconds(warningDisplayDuration); 

        warningMessage.SetActive(false);
    }
    public int GetCurrentScore()
    {
        return score;
    }
    /*
  private void LockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.LockControls();
        MouseLook.instance.LockMouseLooking();
    }

    private void UnlockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.instance.UnlockControls();
        MouseLook.instance.UnlockMouseLooking();
    }*/
}
