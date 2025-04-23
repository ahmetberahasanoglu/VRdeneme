using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera mainCamera;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    private bool wasHitSomething = false;

    void Update()
    {
        InteractionRay();
    }

    void InteractionRay()
    {
        Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2); // ekranýn ortasý
        RaycastHit hit;

        bool hitsomething = false;
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                hitsomething = true;
                interactionText.text = interactable.GetDescription();

                // Deðiþiklik: Mouse sol týk kontrolü
                if (Input.GetMouseButtonDown(0)) // 0 = Left Click
                {
                    interactable.Interact();
                }
            }
        }

        if (hitsomething != wasHitSomething)
        {
            interactionUI.SetActive(hitsomething);
            wasHitSomething = hitsomething;
        }
    }
}
