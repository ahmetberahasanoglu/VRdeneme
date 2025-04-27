using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;
    public float sensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;
    private bool isMouseLookLocked = false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LockCursor(); 
    }

    void Update()
    {
        if (isMouseLookLocked) return; 

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void LockMouseLooking()
    {
        isMouseLookLocked = true;
        UnlockCursor(); 
    }

    public void UnlockMouseLooking()
    {
        isMouseLookLocked = false;
        LockCursor(); 
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
