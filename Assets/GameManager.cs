using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Prescription selectedPrescription;
    public PlayerSelections currentPlayerSelection;


    public enum GameState
    {
        WaitingForPrescription,
        InProgress,
        Finished
    }

    public static GameManager Instance;

    public GameState currentState = GameState.WaitingForPrescription;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(Prescription prescription)
    {
        selectedPrescription = prescription;
        currentState = GameState.InProgress;

        Debug.Log("Oyun baþladý. Seçilen reçete: " + prescription.prescriptionName);

        List<Machine> machineList = new List<Machine>(FindObjectsOfType<Machine>());
        machineList.Sort((a, b) => a.name.CompareTo(b.name));

        MachineManager.Instance.StartMachineSequence(machineList);
    }

    public void FinishGame()
    {
        currentState = GameState.Finished;

        int score = ScoreManager.Instance.CalculateScore(selectedPrescription, currentPlayerSelection);
        Debug.Log("Oyuncu puaný: " + score + "/5");

        HUDController.instance.ShowResult(score);
    }
}
