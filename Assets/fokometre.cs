using UnityEngine;

public class fokometre : MonoBehaviour
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
