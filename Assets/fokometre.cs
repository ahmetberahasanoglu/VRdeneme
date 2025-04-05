using UnityEngine;

public class fokometre : MonoBehaviour, IInteractable
{
    public GameObject fokoPanel;
    public string GetDescription()
    {
        return "Fokometreyi kullan";
    }

    public void Interact()
    {
        fokoPanel.SetActive(true);
    }



}
