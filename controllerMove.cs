using UnityEngine;
using System.Collections;


// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class controllerMove : MonoBehaviour
{

	CharacterController characterController;
	Rigidbody player1;
	public joystick1 joystick;
	public ballMovement ball;
	private Animator animator;
 	private float walking;
	private float playerrun;
	private bool running;
	
	public float speed = 15.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	private Vector3 prevPos;
	private Vector3 currPos;
	private Vector4 deltaPos;

	private Vector3 moveDirection = Vector3.zero;

	void Start()
	{
   		characterController = GetComponent<CharacterController>();
		player1 = GetComponent<Rigidbody>();
	//	.rotation = Quaternion.identity;
        animator = GetComponent<Animator>();
		joystick = GetComponent<joystick1>();
		playerrun = 0.0f;
		running = false;
		prevPos = transform.position;
		print ("PREVPOS " + prevPos);
		print ("ROTATION " + transform.rotation);
	}

	void FixedUpdate()
	{
		walking = joystick.Horizontal();
		//shoot = Input.GetButtonDown("Fire1");
		//playerrun = Input.GetKeyDown(KeyCode.K);
		playerrun = joystick.Vertical();
			

		//shoot = 1.0f;
	
		if (characterController.isGrounded)
		{
			// We are grounded, so recalculate
			// move direction directly from axes
//			print ("RUNNING " + running);

			//moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		        /*if (playerrun == 0.0f)
            			animator.SetFloat("playerrun", playerrun);
			else {
//				print("playerrun " + playerrun);
				animator.SetFloat("playerrun", playerrun);
			}	*/

			if (Input.GetKeyDown(KeyCode.K)) {
   			  animator.SetTrigger("volley");
			}

			if (Input.GetKeyDown(KeyCode.L)) {
   			  animator.SetTrigger("punchdown");
			}

			if (Input.GetKeyUp("1")) 
			    running = false;
			

			if (Input.GetKeyDown("1")) 
		         running = true;


			if (joystick.Horizontal() != 0.0 || joystick.Horizontal() != 0.0) {
			   running = true;
			}

			if (joystick.Horizontal() == 0.0 && joystick.Horizontal() == 0.0) {
			   running = false;
			}

			if (Input.GetKeyDown("3")) {
			   animator.Play("3D_step_s2", 0, 0.0f); 
			}

			if (running) {
			   if (!animator.GetCurrentAnimatorStateInfo(0).IsName("3D_run") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f) { //||  {
 			     animator.Play("3D_run", 0, 0.0f); 
			   }
			}

//			animator.SetFloat("walking", walking);

			moveDirection = new Vector3(joystick.Horizontal(), 0.0f, joystick.Vertical());
			moveDirection *= speed;

			//print ("Rotation " + player1.rotation);
			//Vector3 rot = new Vector3(0, 0.1f, 0);
			//player1.rotation = rot;


			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		} 

		// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
		// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
		// as an acceleration (ms^-2)
		moveDirection.y -= gravity * Time.deltaTime;

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);


		print ("PREVIOUS POS " + prevPos);
		print ("NEW POS " + transform.position);

	//	Vector3 deltaPosition = transform.position - prevPos;
		Vector3 deltaPosition = transform.position - prevPos;

		print ("DELTA POS " + deltaPosition);
		print ("VECTOR3 POS " + Vector3.zero);

		//if (deltaPosition != Vector3.zero) {
			//Quaternion rotation = Quaternion.LookRotation(deltaPosition, Vector3.up);
       		 	//transform.rotation = rotation;
			Vector3 lookPos = transform.position - prevPos;

			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
		////	transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

			print("ENTERED");
	//	}

	//	if (deltaPosition != Vector3.zero) {
         //   		transform.forward = deltaPosition;
        //	}	

		prevPos = transform.position;
	}
}
