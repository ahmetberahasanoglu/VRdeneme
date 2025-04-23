using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public static MachineManager Instance;

    public List<Machine> machines = new List<Machine>();
    private int currentMachineIndex = 0;

    private void Awake()
    {
        Instance = this;
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
        // Ýstersen burada bir ýþýk, efekt veya UI ile belirt
    }

    public bool CanInteractWith(Machine machine)
    {
        return machines[currentMachineIndex] == machine;
    }
}
