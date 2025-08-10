using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HoldRestartButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float holdTime = 1.5f;
    private float timer;
    private bool holding;

    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
        timer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
    }

    void Update()
    {
        if (holding)
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
