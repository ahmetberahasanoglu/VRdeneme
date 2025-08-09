using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class OculusInteractor : XRBaseInteractor
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayerMask = -1;

    [Header("UI References")]
   // public GameObject interactionUI;
  //  public TextMeshProUGUI interactionText;


    private Interactable currentInteractable;
    private Camera xrCamera;
    private XRController controller;

    protected override void Awake()
    {
        base.Awake();

        controller = GetComponent<XRController>();


        if (xrCamera == null)
        {
            xrCamera = FindFirstObjectByType<Camera>();
        }

        
    }

    void Update()
    {
        CheckInteraction();
        HandleInput();
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // Line Renderer için baþlangýç noktasý
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        Vector3 rayEndPoint = rayOrigin + rayDirection * interactionDistance;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayerMask))
        {
            rayEndPoint = hit.point;

            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }

   
    }

    private void HandleInput()
    {
        // XR Controller input kontrolü
        if (controller != null)
        {
            // Trigger butonuna basýldýðýnda etkileþim
            if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerPressed))
            {
                if (triggerPressed && currentInteractable != null)
                {
                    currentInteractable.Interact();
                }
            }
        }

        // Alternatif input metodu (Input System kullanýyorsanýz)
        /*
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
        */
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();

        // HUD Controller varsa kullan
        if (HUDController.instance != null)
        {
            HUDController.instance.EnableInteractionText(currentInteractable.message);
        }

        // Haptic feedback ver
        if (controller != null)
        {
            controller.SendHapticImpulse(0.2f, 0.1f);
        }
    }

    private void DisableCurrentInteractable()
    {
        if (HUDController.instance != null)
        {
            HUDController.instance.DisableInteractionText();
        }

        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }

  

    

    // XR Interaction Toolkit için gerekli override'lar
    public override void GetValidTargets(List<IXRInteractable> targets)
    {
        targets.Clear();

        if (currentInteractable != null)
        {
            var xrInteractable = currentInteractable.GetComponent<IXRInteractable>();
            if (xrInteractable != null)
            {
                targets.Add(xrInteractable);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DisableCurrentInteractable();
    }
}