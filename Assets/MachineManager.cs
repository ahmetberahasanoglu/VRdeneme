using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;
    public GameObject highlightPrefab; 
    private GameObject currentHighlight;
    public List<Machine> machines = new List<Machine>();
    private int currentMachineIndex = 0;


    private void Awake()
    {
        Instance = this;
    }
    public Machine GetCurrentMachine()
    {
        if (currentMachineIndex >= 0 && currentMachineIndex < machines.Count)
        {
            return machines[currentMachineIndex];
        }
        else
        {
            Debug.LogError($"Geçersiz makine indeksi: {currentMachineIndex}");
            return null;
        }
    }
    public void StartMachineSequence(List<Machine> machineList)
    {
        machines = machineList;
        currentMachineIndex = 0;
        HighlightCurrentMachine();
    }

    public void NextMachine()
    {
        currentMachineIndex++;

        if (currentMachineIndex < machines.Count)
        {
            HighlightCurrentMachine();
        }
        else
        {
            Debug.Log("All machines completed!");
            GameManager.Instance.FinishGame();
        }
    }

    public void HighlightCurrentMachine()
    {
        Debug.Log($"Next machine: {machines[currentMachineIndex].machineName}");

        // Önceki highlight’ý sil
        if (currentHighlight != null)
            Destroy(currentHighlight);

        // Yeni highlight instantiate et
        Transform highlightPoint = machines[currentMachineIndex].transform.Find("HighlightPoint");
        if (highlightPoint != null)
        {
            currentHighlight = Instantiate(highlightPrefab, highlightPoint.position, Quaternion.identity);
            currentHighlight.transform.SetParent(machines[currentMachineIndex].transform); // Takip etsin
        }
        else
        {
            Debug.LogWarning("Makinede 'HighlightPoint' bulunamadý.");
        }
    }

    public bool CanInteractWith(Machine machine)
    {
        return machines[currentMachineIndex] == machine;
    }
}
