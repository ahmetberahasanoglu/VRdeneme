using TMPro;
using UnityEngine;

public class labPass : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text instructionText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
            instructionText.text = "Bir re�ete se� ve g�zl��� yapmaya ba�la!";
        }
    }
}
