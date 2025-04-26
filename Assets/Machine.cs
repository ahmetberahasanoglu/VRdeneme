using UnityEngine;

public class Machine : MonoBehaviour
{
    public string machineName;
    public bool isCompleted = false;

    public virtual void Interact()
    {
        Debug.Log($"{machineName} interacted.");

        CompleteMachine();
    }

    public void CompleteMachine()
    {
        isCompleted = true;
        MachineManager.Instance.NextMachine();
    }
}
