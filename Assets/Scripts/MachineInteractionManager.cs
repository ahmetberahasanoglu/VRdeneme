using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MachineInteractionManager : MonoBehaviour
{
    public static MachineInteractionManager Instance;

    [Header("VR Player Settings")]
    public Transform vrPlayer; // XR Origin
    public Camera vrCamera;

    [Header("Global UI (Fallback)")]
    public GameObject globalInteractionUI;
    public TMPro.TextMeshProUGUI globalInteractionText;

    [Header("Error Display Settings")]
    public float errorDisplayDuration = 3f;
    public AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private void Awake()
    {
        Instance = this;

        // VR kamerası yoksa bulun
        if (vrCamera == null)
        {
            vrCamera = FindObjectOfType<Camera>();
        }
    }

    public void TryInteractWith(Machine machine)
    {
        if (GameManager.Instance.currentState != GameManager.GameState.InProgress)
        {
            ShowMachineError(machine, "Önce bir reçete seçmelisin!");
            return;
        }

        if (machine != MachineManager.Instance.GetCurrentMachine())
        {
            ShowMachineError(machine, $"Şu anda {machine.machineName} kullanılamaz.");
            return;
        }

        // VR'da player teleportation yerine pozisyon ayarlama
        PositionPlayerForMachine(machine);
        machine.Interact();
    }

    public void ShowMachineError(Machine machine, string message)
    {
        
        MachineErrorPanel machinePanel = machine.GetComponent<MachineErrorPanel>();

        if (machinePanel != null)
        {
            machinePanel.ShowError(message, errorDisplayDuration);
        }
        else
        {
            // Fallback: Global UI kullan
            ShowGlobalError(message);
            Debug.LogWarning($"Machine {machine.name} has no MachineErrorPanel component. Using global UI.");
        }

        // Ses efekti çal
        AudioManager.Instance.PlaySound(AudioManager.Instance.warningSound, 0.9f);
    }

    private void ShowGlobalError(string message)
    {
        if (globalInteractionUI != null && globalInteractionText != null)
        {
            globalInteractionUI.SetActive(true);
            globalInteractionText.text = message;
            CancelInvoke(nameof(HideGlobalError));
            Invoke(nameof(HideGlobalError), errorDisplayDuration);
        }
    }

    private void HideGlobalError()
    {
        if (globalInteractionUI != null)
        {
            globalInteractionUI.SetActive(false);
        }
    }

    private void PositionPlayerForMachine(Machine machine)
    {
        Transform focusPoint = machine.transform.Find("FocusPoint");

        if (focusPoint && vrPlayer)
        {
            // VR'da smooth teleportation
            StartCoroutine(SmoothTeleportToPosition(focusPoint.position, focusPoint.rotation));
        }
    }

    
    private IEnumerator SmoothTeleportToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPosition = vrPlayer.position;
        Quaternion startRotation = vrPlayer.rotation;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            vrPlayer.position = Vector3.Lerp(startPosition, targetPosition, t);
            vrPlayer.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        vrPlayer.position = targetPosition;
        vrPlayer.rotation = targetRotation;
    }




}
