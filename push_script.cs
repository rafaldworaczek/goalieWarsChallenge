using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push_script : MonoBehaviour {
	public float pushPower = 2.0F;

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		// no rigidbody
		if (body == null || body.isKinematic)
			return;


		//print("Hit Collider EXECUTED");
		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f)
			return;


		//print ("HIT\n");
		// Calculate push direction from move direction,
		// we only push objects to the sides never up and d *own
		//print("System controller velocity. x " + hit.controller.velocity.x);
		//print("System controller velocity. y " + hit.controller.velocity.y);
		//print("System controller velocity. z " + hit.controller.velocity.z);


		Vector3 pushDir = new Vector3(hit.moveDirection.x * hit.controller.velocity.x, 
							        			  0, 
											      hit.moveDirection.z * hit.controller.velocity.z);

		//print ("Speed X " + hit.controller.velocity.x);
		//print("Speed Z" + hit.controller.velocity.z);



		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// Apply the push
		body.velocity = pushDir * 1.3F;

		//print("PUSH HERE " + pushDir * 1.3F);

	}
}
