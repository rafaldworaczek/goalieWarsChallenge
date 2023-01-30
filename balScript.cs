using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balScript : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rb.transform.position, 0.2585f);
    }

    public void OnCollisionEnter(Collision other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(
                    rb.transform.position, 0.2585f,
                    Physics.AllLayers,
                    QueryTriggerInteraction.Collide);
        foreach (Collider collider in hitColliders)
        {
            if (!other.collider.name.Equals("floor"))
                print("#DBGHITCOLLIDERS TEST " + collider.name);
        }
    }
}
