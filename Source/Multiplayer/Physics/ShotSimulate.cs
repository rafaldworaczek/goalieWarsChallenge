using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using Com.Osystems.GoalieStrikerFootball;

public class ShotSimulate : MonoBehaviour
{
    // Start is called before the first frame update
   // private Animator animatorUp;
    //private Animator animatorDown;
    static public Animator animator;

    static private GameObject leftHand;
    static private GameObject rightHand;
    static private GameObject leftPalm;
    static private GameObject rightPalm;
    static private GameObject rbRightFoot;
    static private GameObject rbRightToeBase;
    static private GameObject rbLeftToeBase;
    static private GameObject playerVirtual;
    static Rigidbody playerRb;
    static playerControllerMultiplayer player1MainScript;

    void Awake()
    {       
        animator = GetComponent<Animator>();
        playerVirtual = GameObject.Find("virtualPlayer");
        playerRb = GameObject.Find("virtualPlayer").GetComponent<Rigidbody>();
    }

    void Start()
    {
        player1MainScript = Globals.player1MainScript;

        leftHand =
            player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1LeftHand");
        rightHand =
            player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1RightHand");
        leftPalm =
            player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1LeftPalm");
        rightPalm =
            player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1RightPalm");

        rbRightFoot = player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1RightFoot");
        rbRightToeBase = player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1RightToeBase");
        rbLeftToeBase = player1MainScript.getChildWithName(playerVirtual, "virtualPlayer1LeftToeBase");
    }

    // Update is called once per frame
    //void Update()
    //{
    //doesBallColliding();predictGkCollision
    //}s

    static public Vector3 predictShotPos(Vector3 mainPlayerPos,
                                         Quaternion mainPlayerRot,
                                         string shotType,
                                         Vector3 outShotEnd,
                                         ref Vector3 prepareShotJustBeforeShotPos,
                                         ref Quaternion playerRot)
    {
        playerVirtual.transform.position = mainPlayerPos;
        playerVirtual.transform.rotation = mainPlayerRot;

        player1MainScript.rotateGameObjectTowardPoint(ref playerVirtual, outShotEnd, 1f);

        playerRot = playerVirtual.transform.rotation;

        /*Vector3 localBallPosDestionation = new Vector3(0f, 0f, 2.1f);
        if (shotType.Contains("volley"))
        {
            localBallPosDestionation = new Vector3(0f, 0f, 0.41091f);
        }

        if (Mathf.Abs(playerPos.z) < 3f)
            localBallPosDestionation = new Vector3(0f, 0f, 1.5f);
        else if (Mathf.Abs(playerPos.z) < 2.5f)
        {
            localBallPosDestionation = new Vector3(0f, 0f, 1.0f);
        } else if (Mathf.Abs(playerPos.z) < 2.0f)
        {
            localBallPosDestionation = new Vector3(0f, 0f, 0.0f);
               
        }*/
        Vector3 localJustBeforeShotPos = new Vector3(-1f, 0f, 1.0f);
        if (shotType.Contains("left"))
            localJustBeforeShotPos = new Vector3(1f, 0f, 1.0f);

        print("DBGSHOT playerVirtual.transform.position " + playerVirtual.transform.position);

        if (Mathf.Abs(playerVirtual.transform.position.z) <= 4f) {
            if (Mathf.Abs(playerVirtual.transform.position.z) <= 3f)
                localJustBeforeShotPos.z = 0f;
            else
                localJustBeforeShotPos.z = 0.8f;
        }

        if (shotType.Contains("volley"))
        {
            localJustBeforeShotPos = Vector3.zero;
            ///localJustBeforeShotPos = new Vector3(0.2598f, 0f, 0.665732f);
        }

        playerVirtual.transform.position = 
            player1MainScript.TransformPointUnscaled(playerVirtual.transform, localJustBeforeShotPos);
        prepareShotJustBeforeShotPos = playerVirtual.transform.position;

        print("DBGSHOT calc localJustBeforeShotPos " + prepareShotJustBeforeShotPos);


        ///print("#DBG120 playerRb.transform after " + playerVirtual.transform.position
        ///    + " outShotEnd " + outShotEnd);

        Vector3 localBallPosDestionation = new Vector3(0f, player1MainScript.BALL_MIN_VAL.y, 1.0f);
        if (shotType.Contains("3D_shot_left_foot"))
        {
            localBallPosDestionation.x = -0.3f;
        }
        else
        {
            localBallPosDestionation.x = 0.3f;
        }

        if (shotType.Contains("volley"))
        {
            //localBallPosDestionation = new Vector3(1.1f, 1.45f, 1.65f);
            localBallPosDestionation = new Vector3(0.5f, 1.45f, 1f);
            //localBallPosDestionation = Vector3.zero;
        }

        //playerRb.transform.position =
        //    player1MainScript.TransformPointUnscaledw(playerRb.transform, localBallPosDestionation);

        //print("#DBG120 playerRb.transform before " + playerVirtual.transform.position
        //     + " playerVirtual.rotation " + playerVirtual.transform.eulerAngles);
        float animOffset =
            player1MainScript.getAnimationOffsetTime(shotType);
        //print("shotType " + shotType + " animOffset " + animOffset);
        /*animator.speed = 0f;
        animator.Play(shotType, 0, animOffset);
        animator.Update(0.0f);*/

        //return playerRb.transform.position;
        ///        return playerRb.transform.position;

        print("DBGSHOT calc ballPos " + player1MainScript.TransformPointUnscaled(playerVirtual.transform, localBallPosDestionation));


        return player1MainScript.TransformPointUnscaled(playerVirtual.transform, localBallPosDestionation);
    }

    ///static public Vector3 predictShotBallPos(GameObject playerRb,                        
     //                                        string shotType,
     //                                        Vector3 outShotEnd)
                                      
    //{
        //player.transform.position = playerRb.transform.position;
        ///player1MainScript.rotateGameObjectTowardPoint(ref player, outShotEnd, 1f);
        //playerRb.transform.rotation = player.transform.rotation;
        //animator.speed = 0f;
        //animator.Play(shotType, 0, animPercent);
        //animator.Update(0f);

        //Vector3 localBallLocStart = new Vector3(0, 0, 0.714f);
        //Value 2.1 is value from animator
      /*  Vector3 localBallPosDestionation = new Vector3(0f, player1MainScript.BALL_MIN_VAL.y, 1.0f);
        if (shotType.Contains("3D_shot_left_foot"))
        {
            localBallPosDestionation.x = -0.3f;
        }
        else
        {
            localBallPosDestionation.x = 0.3f;
        }
       
        if (shotType.Contains("volley"))
            localBallPosDestionation = new Vector3(1.15f, 1.45f, 1.65f);

        return player1MainScript.TransformPointUnscaled(playerRb.transform, localBallPosDestionation);
        
    }*/
     
   
    public Vector3 getPlayerPosition()
    {
        return playerVirtual.transform.position;
    }

   
    public Transform getRbTransform()
    {
        return playerVirtual.transform;
    }

    public Vector3 predictColissionAndSolve(Animator animator, 
                                            GameObject ball)
    {

        return Vector3.zero;
    }
  
    public GameObject getLeftPalm()
    {
        return leftPalm;
    }

    public GameObject getRightPalm()
    {
        return rightPalm;
    }

    public GameObject getLeftHand()
    {
        return leftHand;
    }

    public GameObject getRightHand()
    {
        return rightHand;
    }

    //TODO
    public bool isLobShotActive()
    {
        return false;
    }
           
}

