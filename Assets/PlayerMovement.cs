using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public static PlayerMovement instance;  

    public float speed = 12f;
    public float gravity = -9.81f;
   // public TextMeshProUGUI instructionText;

   Vector3 velocity;

    public Transform groundCheck;
    public float distance=0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,distance,groundMask);
        
        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move= transform.right*x+transform.forward*z;
        controller.Move(move*speed*Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void LockControls()
    {
        controller.enabled = false;
    }

    public void UnlockControls()
    {
        controller.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "giris")
        {
            //instructionText.IsActive(true);
            //Üste yazý gelecek: Bir reçete al ve gözlüðü yapmaya baþla!.
        }
    }
}
   