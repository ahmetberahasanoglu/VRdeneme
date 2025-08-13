using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject gameOverPanel;
    private void LateUpdate()
    {
        gameOverPanel.transform.LookAt(playerCamera);
        gameOverPanel.transform.Rotate(0, 180, 0);
    }
}
