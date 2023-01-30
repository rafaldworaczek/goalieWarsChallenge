using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
	float speed;
	// Use this for initialization
	void Start () {
		speed = 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//print (Input.GetAxis("Horizontal"));
		transform.Translate (speed * Input.GetAxis("Horizontal")*Time.deltaTime,
							 0.0f,
						     speed * Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
