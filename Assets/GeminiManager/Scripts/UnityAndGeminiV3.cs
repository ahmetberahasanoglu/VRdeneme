using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using GoogleTextToSpeech.Scripts.Data;
using GoogleTextToSpeech.Scripts;
using TMPro;


[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}

[System.Serializable]
public class Response
{
    public Candidate[] candidates;
}

public class ChatRequest
{
    public Content[] contents;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public string role; 
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}


public class UnityAndGeminiV3: MonoBehaviour
{
    [Header("Gemini API Password")]
    public string apiKey; 
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent"; // Edit it and choose your prefer model
    public TextMeshProUGUI text;
    public float delay = 0.05f;

    [Header("NPC Function")]
    [SerializeField] private TextToSpeechManager googleServices;
    private Content[] chatHistory;
    private Coroutine typingCoroutine;

    void Start()
    {
        chatHistory = new Content[]
      {
            new Content
            {
                role = "user",
                parts = new Part[]
                {
                    new Part
                    {
                        text = "Sen bir optisyenlik laboratuvarýndaki Mehmet adýndaki öðretmen rolündesin. " +
        "Görevin öðrencilerin reçeteye uygun þekilde gözlük yapmalarýný öðretmek. " +
        "Laboratuvarda þu cihazlar var: Fokometre, Merkezleme cihazý, Çerçeve izleyici, Cam kesici ve Isýtýcý. " +
        "Sorulara kýsa ve anlaþýlýr cevap ver. Ýnsan gibi konuþ, listeleme veya madde iþaretleri kullanma. " +
        "Ýlgisiz bir soru sorulursa sadece 'Bu konu laboratuvar çalýþmamýzla ilgili deðil.' de."
                    }
                }
            }
      };
    }

    // Functions for sending a new prompt, or a chat to Gemini
    private IEnumerator SendPromptRequestToGemini(string promptText)
    {
        string url = $"{apiEndpoint}?key={apiKey}";
     
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"{" + promptText + "}\"}]}]}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST")){
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError(www.error);
            } else {
                Debug.Log("Request complete!");
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                    {
                        //This is the response to your request
                        string text = response.candidates[0].content.parts[0].text;
                        Debug.Log(text);
                    }
                else
                {
                    Debug.Log("No text found.");
                }
            }
        }
    }

    public void SendChat(string userMessage)
    {
        // string userMessage = inputField.text;
        StartCoroutine( SendChatRequestToGemini(userMessage));
     
    }

  
    private IEnumerator SendChatRequestToGemini(string newMessage)
    {

        string url = $"{apiEndpoint}?key={apiKey}";
     
        Content userContent = new Content
        {
            role = "user",
            parts = new Part[]
            {
                new Part { text = newMessage }
            }
        };

        List<Content> contentsList = new List<Content>(chatHistory);
        contentsList.Add(userContent);
        chatHistory = contentsList.ToArray(); 

        ChatRequest chatRequest = new ChatRequest { contents = chatHistory };

        string jsonData = JsonUtility.ToJson(chatRequest);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST")){
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError(www.error);
            } else {
                Debug.Log("Request complete!");
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                    {
                        //This is the response to your request
                        string reply = response.candidates[0].content.parts[0].text;
                        Content botContent = new Content
                        {
                            role = "model",
                            parts = new Part[]
                            {
                                new Part { text = reply }
                            }
                        };

                        Debug.Log(reply);
                   
                  
                    googleServices.SendTextToGoogle(reply);
                    if (typingCoroutine != null)
                    {
                        StopCoroutine(typingCoroutine);
                    }
                    typingCoroutine = StartCoroutine(TypeReply(reply));
                   
                    //This part shows the text in the Canvas
                    // uiText.text = reply;
                    //This part adds the response to the chat history, for your next message
                    contentsList.Add(botContent);
                        chatHistory = contentsList.ToArray();
                      
                    }
                else
                {
                    Debug.Log("No text found.");
                }
             }
        }  
    }
    private IEnumerator TypeReply(string reply)
    {
        text.text = "";
        foreach (char c in reply)
        {
            text.text += c;
            yield return new WaitForSeconds(delay);
        }
    }



}


