using UnityEngine;
using UnityEngine.Events;

public class MachineInteractionManager : MonoBehaviour
{
    public static MachineInteractionManager Instance;

    public Transform player;
    public CharacterController characterController;
    public GameObject interactionUI;
    public TMPro.TextMeshProUGUI interactionText;

    private bool isLocked = false;

    private void Awake()
    {
        Instance = this;
    }

    public void TryInteractWith(Machine machine)
    {
        if (machine != MachineManager.Instance.GetCurrentMachine())
        {
            ShowError($"Şu anda {machine.machineName} kullanılamaz.");
            return;
        }

        // Etkileşim başarılı → pozisyona geç
        LockPlayerMovement();
        MovePlayerToMachine(machine);
        machine.Interact();
    }

    void ShowError(string msg)
    {
        interactionUI.SetActive(true);
        interactionText.text = msg;
        Invoke(nameof(HideError), 2f); // 2 saniye sonra kapat
    }

    void HideError()
    {
        interactionUI.SetActive(false);
    }

    void MovePlayerToMachine(Machine machine)
    {
        Transform focusPoint = machine.transform.Find("FocusPoint"); // Makineye önceden boş bir Transform ekle
        if (focusPoint)
        {
            characterController.enabled = false; // FPS controller için geçici disable
            player.position = focusPoint.position;
            characterController.enabled = true;
        }
        else
        {
            Debug.LogWarning("Makine üzerinde 'FocusPoint' yok!");
        }
    }

    public void LockPlayerMovement()
    {
        isLocked = true;
        PlayerMovement.instance.LockControls(); 
    }

    public void UnlockPlayerMovement()
    {
        isLocked = false;
        PlayerMovement.instance.UnlockControls(); // örnek fonksiyon
    }
}
