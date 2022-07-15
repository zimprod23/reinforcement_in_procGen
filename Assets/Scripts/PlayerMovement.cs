using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Transform cam;
    public float speed = 12f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    // Start is called before the first frame update
    public Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float SmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool isGrounded;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }
        float x  = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Jump") && isGrounded){
           velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
        Vector3 move = new Vector3(x,0f,z).normalized;

        if(move.magnitude >= 0.1f){
               float targetAngle = Mathf.Atan2(move.x,move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
               float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnSmoothVelocity,SmoothTime);
               transform.rotation = Quaternion.Euler(0f,angle,0f);
               Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
               controller.Move(moveDir.normalized * speed * Time.deltaTime);
               velocity.y += gravity * Time.deltaTime;
         controller.Move(velocity * Time.deltaTime);
        }
       // transform.Translate(move * speed * Time.deltaTime);
         velocity.y += gravity * Time.deltaTime;
         controller.Move(velocity * Time.deltaTime);
    }
}
