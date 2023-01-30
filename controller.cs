using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class controller: MonoBehaviour
{
    CharacterController characterController;
    private Animator animator;
    bool running;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        running = false;
        ///prevPos = transform.position;
    }

    void Update()
    {
       if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKeyUp("1"))
                running = false;


            if (Input.GetKeyDown("1"))
                running = true;

            if (running)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("3D_run") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
                { 
                    animator.Play("3D_run", 0, 0.0f);
                }
            }



           // Vector3 lookPos = transform.position - prevPos;
           // Quaternion rotation = Quaternion.LookRotation(lookPos);

        }

        print("MOVE DIRECTION " + moveDirection);
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}