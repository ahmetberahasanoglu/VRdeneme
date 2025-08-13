using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Prescription basePrescription; 
    public Prescription currentPrescription;


    public enum GameState
    {
        WaitingForPrescription,
        InProgress,
        Finished
    }

    public static GameManager Instance;

    public GameState currentState = GameState.WaitingForPrescription;

    [Header("Score & Timer Settings")]
    public int baseScore = 100;              
    public float examTime = 300f;        
    private float timer;                 
    private int currentScore;            

    private void Start()
    {
        if (currentPrescription != null)
        {
            currentPrescription.GenerateRandomValues();
        }
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject); yeni sahne eklemedikce gerek yok
        }
        else
        {
            Destroy(gameObject);
        }
    }
  
    public void StartGame(Prescription prescription)    
    {
        currentPrescription = prescription;
        currentState = GameState.InProgress;
        currentScore = baseScore; // baþlangýç puaný
        timer = examTime;         // süreyi baþlat

        HUDController.instance.UpdateScore(currentScore);
        HUDController.instance.UpdateTimer(timer);

        StartCoroutine(ExamTimer());
        //  currentPrescription.GenerateRandomValues();

        // Debug.Log("Oyun baþladý. Seçilen reçete: " + prescription.prescriptionName);

        //  List<Machine> machineList = new List<Machine>(FindObjectsOfType<Machine>());
        //  machineList.Sort((a, b) => a.name.CompareTo(b.name));//bu kodu sonra degistirebilirim

        MachineManager.Instance.StartMachineSequence();
    }

    public void AddPenalty(int scorePenalty)
    {
        currentScore -= scorePenalty;
        HUDController.instance.UpdateScore(currentScore);

        if (currentScore <= 0)
        {
            FinishGame();
        }
    }

    IEnumerator ExamTimer()
    {
        while (timer > 0 && currentState == GameState.InProgress)
        {
            timer -= Time.deltaTime;
            HUDController.instance.UpdateTimer(timer);
            yield return null;
        }

        // süre biterse oyun bitsin
        if (timer <= 0 && currentState == GameState.InProgress)
        {
            FinishGame();
        }
    }
    public void FinishGame()
    {
        currentState = GameState.Finished;
        int timeBonus = Mathf.RoundToInt(timer);
        int finalScore = Mathf.Max(0, currentScore);

        //int score = ScoreManager.Instance.CalculateScore(selectedPrescription, currentPlayerSelection);
        //Debug.Log("Oyuncu puaný: " + score + "/5");
        HUDController.instance.EndGame(finalScore, examTime - timer, baseScore - currentScore);
        Time.timeScale = 0f;
    }
}
