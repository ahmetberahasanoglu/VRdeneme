using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

namespace GoogleSpeechToText.Scripts
{
    public class SpeechToTextManager : MonoBehaviour
    {
        // [SerializeField] private string audioUri = "gs://cloud-samples-tests/speech/brooklyn.flac"; // Audio file URI in Google Cloud Storage
        [Header("Google Cloud API Password")]
        [SerializeField] private string apiKey; // Replace with your API key
        [Header("Gemini Manager Prefab")]
        public UnityAndGeminiV3 geminiManager;
                
        private AudioClip clip;
        private byte[] bytes;
        private bool recording = false;
        private float maxRecordingTime = 10f;
        private Coroutine recordingCoroutine;
        public Button micButton; 
        public TextMeshProUGUI buttonText;
        public Color idleColor = Color.green;   
        public Color recordingColor = Color.red;
        public void OnMicButtonPressed()
        {
            if (!recording)
            {
                StartRecording();
               
                recordingCoroutine = StartCoroutine(StopRecordingAfterDelay(maxRecordingTime));
            }
            else
            {
                StopRecording();
                if (recordingCoroutine != null)
                {
                    StopCoroutine(recordingCoroutine);
                    recordingCoroutine = null;
                }
            }
            UpdateButtonText();
        }
        void UpdateButtonText()
        {
            buttonText.text = recording ? "Sorun bittiginde, tekrar bas" : "Soru Sor";
            ColorBlock colors = micButton.colors;
            colors.normalColor = recording ? recordingColor : idleColor;
            colors.highlightedColor = recording ? recordingColor : idleColor;
            micButton.colors = colors;
        }
        private IEnumerator StopRecordingAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (recording) { 
            StopRecording();
            UpdateButtonText();
        }
        }
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !recording)
        {
            StartRecording();
            recording = true;
        } 
        
        if (Input.GetKeyUp(KeyCode.Space) && recording )
        
        {
            StopRecording();
            recording = false;
        }

    }

    private void StartRecording()
    {
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

        private void StopRecording()
        {
            var position = Microphone.GetPosition(null);
            Microphone.End(null);

            var samples = new float[position * clip.channels];
            clip.GetData(samples, 0);
            bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
            recording = false;

            GoogleCloudSpeechToText.SendSpeechToTextRequest(bytes, apiKey,
                (response) => {
                    Debug.Log("Speech-to-Text Response: " + response);

             
                    var speechResponse = JsonUtility.FromJson<SpeechToTextResponse>(response);

                    if (speechResponse != null &&
                        speechResponse.results != null &&
                        speechResponse.results.Length > 0 &&
                        speechResponse.results[0].alternatives != null &&
                        speechResponse.results[0].alternatives.Length > 0)
                    {
                        var transcript = speechResponse.results[0].alternatives[0].transcript;
                        Debug.Log("Söylediðimiz þey: " + transcript);

                        geminiManager.SendChat(transcript);
                    }
                    else
                    {
                        Debug.LogWarning("Speech-to-Text: Geçerli bir metin bulunamadý.");
                    }
                },
                (error) => {
                    Debug.LogError("Error: " + error.error.message);
                });
        }

    }
}
