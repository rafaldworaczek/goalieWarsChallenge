using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestScene : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody rb;
    PhotonView photonView;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        if (!animator)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
        {
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetBool("Idle", false);
            rb.velocity = new Vector3(h, 0, v) * 10.0f;
        } else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("Idle", true);
        }

        print("Speed " + (h * h + v * v));
    }

}
