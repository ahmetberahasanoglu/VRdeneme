using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;
public class GameManager : MonoBehaviour
{
    public PlayerSelection currentPlayerSelection;
    public enum GameState
    {
        WaitingForPrescription, // Reçete alýnmasý
        InProgress,             // Gözlük yapým süreci
        Finished                // Gözlük tamamlandý, puanlama
    }
    public static GameManager Instance;

    public GameState currentState = GameState.WaitingForPrescription;

    public Prescription selectedPrescription;

    void Awake()
    {
        // Singleton pattern
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
        Debug.Log("Oyun baþladý. Seçilen reçete: " + prescription.name);
        List<Machine> machineList = new List<Machine>(FindObjectsOfType<Machine>());
        machineList.Sort((a, b) => a.name.CompareTo(b.name)); // sýraya göre diz
        MachineManager.Instance.StartMachineSequence(machineList);
    }

    public void FinishGame()
    {
        currentState = GameState.Finished;


        int score = ScoreManager.Instance.CalculateScore(selectedPrescription, currentPlayerSelection);

        Debug.Log("Oyuncu puaný: " + score + "/5");

        UIManager.Instance.ShowResult(score);
    }
}
