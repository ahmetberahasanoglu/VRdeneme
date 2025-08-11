using System.Collections;
using UnityEngine;

[System.Serializable]
public class MachineErrorPanel : MonoBehaviour
{
    [Header("Error Panel UI")]
    public GameObject errorPanel;
    public TMPro.TextMeshProUGUI errorText;
    public CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    public float animationDuration = 0.3f;
    public Vector3 showScale = Vector3.one;
    public Vector3 hideScale = new Vector3(0.8f, 0.8f, 0.8f);

    [Header("World Space UI Settings")]
    public bool faceCamera = true;
   

    private Coroutine currentErrorCoroutine;
    private Camera vrCamera;
    
    private void Start()
    {  
        vrCamera = FindObjectOfType<Camera>();
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
        SetupWorldSpaceCanvas();
    }

    private void Update()
    {
    
        if (faceCamera && vrCamera != null && errorPanel.activeInHierarchy)
        {
            Vector3 directionToCamera = vrCamera.transform.position - errorPanel.transform.position;
            directionToCamera.y = 0;
            if (directionToCamera != Vector3.zero)
            {
                errorPanel.transform.rotation = Quaternion.LookRotation(-directionToCamera);
            }
        }
    }

    public void ShowError(string message, float duration)
    {
        if (errorPanel == null || errorText == null) return;     
        if (currentErrorCoroutine != null)
        {
            StopCoroutine(currentErrorCoroutine);
        }

        currentErrorCoroutine = StartCoroutine(ShowErrorCoroutine(message, duration));
    }
    
    private IEnumerator ShowErrorCoroutine(string message, float duration)
    {
 
        errorText.text = message;
        errorPanel.SetActive(true);

        yield return StartCoroutine(FadePanel(0f, 1f, animationDuration));

 
        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadePanel(1f, 0f, animationDuration));

        errorPanel.SetActive(false);
        currentErrorCoroutine = null;
    }

    private IEnumerator FadePanel(float fromAlpha, float toAlpha, float duration)
    {
        Vector3 fromScale = fromAlpha > 0.5f ? showScale : hideScale;
        Vector3 toScale = toAlpha > 0.5f ? showScale : hideScale;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            }

           
            errorPanel.transform.localScale = Vector3.Lerp(fromScale, toScale, t);

            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = toAlpha;
        }
        errorPanel.transform.localScale = toScale;
    }

    private void SetupWorldSpaceCanvas()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = vrCamera;

            
        }
    }
}

