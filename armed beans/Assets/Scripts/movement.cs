using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class movement : NetworkBehaviour
{
    // Start is called before the first frame update

    CharacterController characterController;
    
    [SerializeField]
    float MoveSpeed = 100f;

    [SerializeField]
    float Senstivity = 3f;

    [SerializeField]
    Transform cameraTransform;
    float pitch;
    Vector3 velocity;

    [SerializeField] 
    float gravity = 9.81f;
    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    float JumpHeight = 10f;
    
    [SerializeField]
    GameObject cameraChild;

    void Start()
    {
        if (IsLocalPlayer)
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            cameraChild.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer) 
        {
            MovePlayerAndCamera();
        }
    }

    void MovePlayerAndCamera()
    {
        // movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * Time.deltaTime * MoveSpeed);
        // gravity

        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }   
        velocity.y += gravity * Time.deltaTime ;
        characterController.Move(velocity * Time.deltaTime);

        // camera
        float mouseX = Input.GetAxis("Mouse X") * Senstivity;
        transform.Rotate(0, mouseX, 0);
        pitch -= Input.GetAxis("Mouse Y") * Senstivity;
        pitch = Mathf.Clamp(pitch, -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(pitch,0,0);
    }
}
