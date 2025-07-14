using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class labPass : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text instructionText;
    [SerializeField] Image Panel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ShowInstruction());

           
        }
    }
    private IEnumerator ShowInstruction()
    {
        canvas.SetActive(true);
        Panel.gameObject.SetActive(true);
        instructionText.text = "Bir reçete seç ve gözlüðü yapmaya baþla!";

        yield return new WaitForSeconds(2);
        Panel.gameObject.SetActive(false);
        GameObject.Destroy(this);

    }
}
