using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
 
    [Header("Interaction")]
    [SerializeField] TMP_Text interactionText;

    [Header("FokoPanel")]
    [SerializeField] GameObject fokoPanel;
    
    [SerializeField] TMP_Text prescriptionText;
    [SerializeField] GameObject warningMessage;

    [Header("Cihaz2Panel")]
    [SerializeField] GameObject cihaz2Panel;
    [SerializeField] TMP_Text simpleText;

    GraphicRaycaster raycaster;

    [SerializeField] GameObject cihaz3Panel;
    [SerializeField] GameObject cihaz4Panel;
    [SerializeField] GameObject hocaPanel;

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    private Prescription prescription;
    private int score = 100;


    private bool isTaskCompleted = false;
    public float warningDisplayDuration = 2f;
    public AudioSource audioSource;
    public AudioClip failClip;
    public AudioClip positiveClip;
    public AudioClip fireClip;

    private Coroutine warningCoroutine;

    private void Awake()
    {
        instance = this;
        raycaster = GetComponent<GraphicRaycaster>();   
    }
    private void Start()
    {
        if (GameManager.Instance.currentPrescription != null)// if (GameManager.Instance.selectedPrescription != null)
            prescription = GameManager.Instance.currentPrescription;
    }
    public void DecreaseScore(int amount)
    {
        score -= amount;
        scoreText.text = "Ba�ar� Notu: " + score;
        audioSource.PlayOneShot(failClip);
        if (score < 0)
        {
            EndGame(); 
        }
    }
    public void onIsiticiInteracted()
    {
        audioSource.PlayOneShot(fireClip);
    }
    public void EndGame()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Notunuz: " + Mathf.Max(score, 0);
        Time.timeScale = 0f; 
    }
    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void ShowfokoPanel()
    {
        Debug.Log("panel goster.");
        isTaskCompleted = false; 
     
        fokoPanel.SetActive(true);
        raycaster.enabled = true;
        LockPlayerControls();
        Prescription prescription = GameManager.Instance.currentPrescription;
        prescriptionText.text = $"Re�ete: SPH: {prescription.sphere} CYL: {prescription.cylinder} AXIS: {prescription.axis}";
       
    }
    public void ShowCihaz3Panel()
    {
        Debug.Log("panel goster.");
        isTaskCompleted = false;

        cihaz3Panel.SetActive(true);
        raycaster.enabled = true;
        LockPlayerControls();
        
    }
    public void ShowCihaz4Panel()
    {     
        isTaskCompleted = false;

        cihaz4Panel.SetActive(true);
        raycaster.enabled = true;
        LockPlayerControls();

    }
    public void ShowHocaPanel()
    {
        hocaPanel.SetActive(true);
        raycaster.enabled = true;
        LockPlayerControls();

    }
    public void ShowGlassPanel()
    {
        isTaskCompleted=false;
        cihaz2Panel.SetActive(true);
   
        raycaster.enabled=true;
        LockPlayerControls();
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
            ShowWarningMessage("T�m de�erleri do�ru girmeden i�lemi tamamlayamazs�n!");
        }
    }
    public void TryHideLtPanel()
    {
        if (lt980.Instance.olcumYap�ld�)
        {
            isTaskCompleted = true;
            cihaz2Panel.SetActive(false);
            raycaster.enabled = false;
            UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }
       
    }
    public void TryHideCihaz3Panel()
    {
        if (cihaz3.instance.olcumYapildi)
        {
            isTaskCompleted = true;
            cihaz3Panel.SetActive(false);
            raycaster.enabled = false;
            UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }

    }
    public void TryHideCihaz4Panel()
    {
        if (Cihaz4.instance.islemTamamlandi)
        {
            isTaskCompleted = true;
            cihaz4Panel.SetActive(false);
            raycaster.enabled = false;
            UnlockPlayerControls();
            audioSource.PlayOneShot(positiveClip);
            MachineManager.Instance.NextMachine();
        }

    }

    public void HidefokoPanel()
    {
        fokoPanel.SetActive(false);
        raycaster.enabled = false;
        UnlockPlayerControls();
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
    }
}
