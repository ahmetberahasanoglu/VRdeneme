using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;
    public GameObject highlightPrefab; 
    private GameObject currentHighlight;
    public List<Machine> machines = new List<Machine>();
    public int currentMachineIndex = 0;
    [SerializeField] private TextMeshProUGUI hocaInstruction;


    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {/*
        if(currentMachineIndex >= 0) {
            hocaInstruction.text = machines[currentMachineIndex].gameObject.name;
        }*/
       
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
    public void StartMachineSequence()//List<Machine> machineList
    {
        //machines = machineList;
        currentMachineIndex = 0;
        hocaInstruction.text = machines[currentMachineIndex].gameObject.name;
        HighlightCurrentMachine();
    }

    public void NextMachine()
    {
        currentMachineIndex++;
        if (currentMachineIndex != 5) { 
            hocaInstruction.text = machines[currentMachineIndex].gameObject.name; 
        }
       
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
    public void CompleteCurrentMachine()
    {
        currentMachineIndex++;
        if (currentMachineIndex < machines.Count)
        {
          //  StartMachine(machines[currentMachineIndex]);
        }
        else
        {
            GameManager.Instance.FinishGame();
        }
    }


    public void HighlightCurrentMachine()
    {
        Debug.Log($"Next machine: {machines[currentMachineIndex].machineName}");

        if (currentHighlight != null)
            Destroy(currentHighlight);

        Transform highlightPoint = machines[currentMachineIndex].transform.Find("HighlightPoint");
        if (highlightPoint != null)
        {
            currentHighlight = Instantiate(highlightPrefab, highlightPoint.position, Quaternion.identity);
            currentHighlight.transform.SetParent(machines[currentMachineIndex].transform);
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
