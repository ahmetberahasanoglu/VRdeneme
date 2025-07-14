using UnityEngine;
using UnityEngine.Events;

public class MachineInteractionManager : MonoBehaviour
{
    public static MachineInteractionManager Instance;

    public Transform player;
    public CharacterController characterController;
    public GameObject interactionUI;
    public TMPro.TextMeshProUGUI interactionText;


    private void Awake()
    {
        Instance = this;
    }

    public void TryInteractWith(Machine machine)
    {
        if (GameManager.Instance.currentState != GameManager.GameState.InProgress)
        {
            ShowError("Önce bir reçete seçmelisiniz!");
            return;
        }
        if (machine != MachineManager.Instance.GetCurrentMachine())
        {
            ShowError($"Şu anda {machine.machineName} kullanılamaz.");
            return;
        }

        LockPlayerMovement();
        Debug.Log("LockMovementCalısmalı");
        MovePlayerToMachine(machine);
        machine.Interact();
    }

    void ShowError(string msg)
    {
        interactionUI.SetActive(true);
        interactionText.text = msg;
        Invoke(nameof(HideError), 2f); 
    }

    void HideError()
    {
        interactionUI.SetActive(false);
    }

    void MovePlayerToMachine(Machine machine)
    {
        Transform focusPoint = machine.transform.Find("FocusPoint"); 
        if (focusPoint)
        {
            characterController.enabled = false;
            player.position = focusPoint.position;
            characterController.enabled = true;
        }
    }

    public void LockPlayerMovement()
    {
        PlayerMovement.instance.LockControls();
        Debug.Log("Kontroller kilitlendi");
    }

    public void UnlockPlayerMovement()
    {
        PlayerMovement.instance.UnlockControls();
        Debug.Log("Kontroller acıldı");
    }
}
