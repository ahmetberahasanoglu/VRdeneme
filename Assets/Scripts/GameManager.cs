using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        if (currentPrescription != null)
        {
            currentPrescription.GenerateRandomValues();
        }
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
      //  currentPrescription.GenerateRandomValues();

       // Debug.Log("Oyun baþladý. Seçilen reçete: " + prescription.prescriptionName);

      //  List<Machine> machineList = new List<Machine>(FindObjectsOfType<Machine>());
      //  machineList.Sort((a, b) => a.name.CompareTo(b.name));//bu kodu sonra degistirebilirim

        MachineManager.Instance.StartMachineSequence();
    }

    public void FinishGame()
    {
        currentState = GameState.Finished;

        //int score = ScoreManager.Instance.CalculateScore(selectedPrescription, currentPlayerSelection);
        //Debug.Log("Oyuncu puaný: " + score + "/5");

        HUDController.instance.EndGame();
    }
}
