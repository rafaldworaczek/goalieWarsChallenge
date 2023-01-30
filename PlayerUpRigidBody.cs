using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpRigidBody : MonoBehaviour
{
    private bool collisionWithWall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        /*if (other.collider.name.Contains("ball"))
        {
            print("DEBUG2345ANIMPLAY COLLISION PLAYERUP " + other.collider.name);
        }*/

        if (other.collider.name.Contains("wall") ||
            other.collider.name.Contains("goal"))
        {
            //Physics.IgnoreCollision(other.collider, GetComponent<Collider>());

            collisionWithWall = true;
            //print("COLLSION WITH WALL ");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        string name = other.GetComponent<Collider>().name;

        //print("OTHERCOLLIDERNAMES PLAYERUP TRIGGER " + name);

        if (name.Contains("wall") ||
            name.Contains("goal"))
        {
            collisionWithWall = true;
            //print("COLLSION WITH WALL TRIGGER ");
        }
    }

    public bool isCollisionWithWall()
    {
        return collisionWithWall;
    }

    public void setIsCollisionWithWall(bool value)
    {
        collisionWithWall = value;
    }

}
