using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPos : MonoBehaviour
{
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        distance = 3.0f;
        //player = GameObject.Find("player1").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Find might be slow */
        //Vector3 playerPos = GameObject.Find("player1").transform.transform.position;
        //transform.position = new Vector3(playerPos.x, 3.0f + (Mathf.Abs(playerPos.z / 3.0f)), playerPos.z - (distance + Mathf.Abs(playerPos.z / 2.5f)));

        /*transform.position.z = target.position.z - distance;
        transform.position.y = target.position.y;
        transform.position.x = target.position.x;*/
    }   
}

