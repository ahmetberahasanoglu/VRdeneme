using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*sensivity*Time.deltaTime;   
        float mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation=Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.Rotate(Vector3.up * mouseX);
       transform.localRotation=Quaternion.Euler(xRotation,0f,0f);
    }
}
