using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    [SerializeField] private string gasURL;
    [SerializeField] private string prompt;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SendDatToGAS());
        }
    }
    private IEnumerator SendDatToGAS()
    {
        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);
        UnityWebRequest www= UnityWebRequest.Post(gasURL, form);
        yield return www.SendWebRequest();
        string response = " ";
        if (www.result == UnityWebRequest.Result.Success)
        {
            response = www.downloadHandler.text; 
        }
        else
        {
            response = "Hata var!";
        }
        Debug.Log(response);
    }


}
