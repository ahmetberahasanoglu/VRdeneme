using TMPro;
using UnityEngine;



public class Interactor : MonoBehaviour
{
    public Camera mainCamera;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;



    void Update()
    {
        InteractionRay();
    }
    private bool wasHitSomething = false;

    void InteractionRay()
    {
        Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2);
        RaycastHit hit;

        bool hitsomethin = false;
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                hitsomethin = true;
                interactionText.text = interactable.GetDescription();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }

        if (hitsomethin != wasHitSomething)
        {
            interactionUI.SetActive(hitsomethin);
            wasHitSomething = hitsomethin;
        }
    }
}
