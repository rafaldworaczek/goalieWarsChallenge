using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using Com.Osystems.GoalieStrikerFootball;

public class PredictCollision : MonoBehaviour
{
    // Start is called before the first frame update
   // private Animator animatorUp;
    //private Animator animatorDown;
    static public Animator animator;

    //public GameObject playerDown;
    //public Rigidbody playerDownRb;
    static private string lastGkAnimPlayed = "";
    static public GameObject ball;
    static public Rigidbody ballRb;
    static public Rigidbody ballRb2;
    private static GameObject ballRbLeftSide;
    private static GameObject ballRbRightSide;
    private float ShotSpeedMax = 1300.0f;
    private float ShotSpeedMin = 420.0f;
    private float MAX_SHOT_SPEED_UNITY_UNITS = 34.5f;
    static private bool animatorIKExecuted = false;

    private Vector3 lastBallPos;
    static private GameObject leftHand;
    static private GameObject rightHand;
    static private GameObject leftPalm;
    static private GameObject rightPalm;

    private float passedShotFlyTime = 0.0f;
    public float ballShotVelocity = 20f;
    private float timeofBallFly = 1000f;
    private bool shotRet = false;
    static private bool shotActive = true;
    static public BallRb ballRbScript;
    static public BallRb ballRbScript2;
    private float shotCurrentPlace = 0f;
    static public GameObject player;
    static public Rigidbody playerRb;
    static float lastGkDistX;
    static bool isAnimPlaying = false;
    static private playerControllerMultiplayer player1MainScript;
    private playerControllerMultiplayer player2MainScript;
    private static bool updatePlayerPos = false;
    private static Vector3 playerUpdatedPos = Vector3.zero;
    private static Quaternion playerUpdatedPosRot;
    static private bool isOnAnimatorActive = false;
    public static string predictedAnimName = "";
    private static Vector3 rightHandOrgPos;
    private static Vector3 leftHandOrgPos;
    private static Vector3 ballPrevPos;
    private static bool ballVelocitySet = false;

    void Awake()
    {
        ballRbLeftSide = new GameObject();
        ballRbRightSide = new GameObject();

        ballRbLeftSide = GameObject.Find("virtualballLeftSide");
        ballRbRightSide = GameObject.Find("virtualballRightSide");
        animator = GetComponent<Animator>();

        //playerDownRb = playerDown.GetComponent<Rigidbody>();
        player = GameObject.Find("virtualPlayer");
        playerRb = GameObject.Find("virtualPlayer").GetComponent<Rigidbody>();
  
        ballRb = GameObject.Find("virtualBall").GetComponent<Rigidbody>();
        ballRb2 = GameObject.Find("virtualBall2").GetComponent<Rigidbody>();

        ballRbScript = GameObject.Find("virtualBall").GetComponent<BallRb>();
        ballRbScript2 = GameObject.Find("virtualBall2").GetComponent<BallRb>();

        ballRb.velocity = Vector3.zero;
    }

    void Start()
    {
        player1MainScript = Globals.player1MainScript;

        leftHand =
            player1MainScript.getChildWithName(player, "virtualPlayer1LeftHand");
        rightHand =
            player1MainScript.getChildWithName(player, "virtualPlayer1RightHand");
        leftPalm =
            player1MainScript.getChildWithName(player, "virtualPlayer1LeftPalm");
        rightPalm =
            player1MainScript.getChildWithName(player, "virtualPlayer1RightPalm");
    }

    // Update is called once per frame
    //void Update()
    //{
    //doesBallColliding();predictGkCollision
    //}s

    static public float getBallVelocity()
    {
        return ballVelocity;
    }


    static Vector3 wallVelocity = new Vector3(-10000f, -10000f, -100000f);



    static public bool doesItCollideWithPlayer(Collider[] hitColliders)
    {
        //print("DEBUGGK1045 check if collide " + hitColliders.Length);
        foreach (Collider collider in hitColliders)
        {
            //print("DEBUGGK1045 COLLISION hit collider " + collider.name);
            if (collider.name.Contains("virtualPlayer") ||
                collider.name.Contains("Spine"))
            {
                return true;            
            }
        }

        return false;
    }

    public static Vector3 ballAnimatorStartPos = Vector3.zero;
    public static Vector3 ballAnimatorVirtualStartPos = Vector3.zero;
    static int ballStartIdx;
    static int offsetDown = 0;
    static int offsetUp = 0;
    static int loopNumber = 0;
    static float ballVelocity = 0f;
    static int ballPoMainIndex = 0;
    static bool isBall2Collided = false;



    static public Vector3 predictionGkCollisionOutput(Vector3 outShotStart,
                                                      Vector3 outShotMid,
                                                      Vector3 outShotEnd,
                                                      //Rigidbody ballRbTODELETE,
                                                      Rigidbody playerRigidBody,
                                                      GameObject rotatedRbToBallTmp,
                                                      ref bool initialization,
                                                      string animName,
                                                      float animOffset,
                                                      float gkSideAnimPlayOffset,
                                                      float timeOfBallFly,
                                                      ref float curvePercentHit,
                                                      float lastGkDist,
                                                      ref Vector3 localGkStartPos,
                                                      Vector3[,] prepareShotPos,
                                                      int prepareShotMaxIdx,
                                                      int predictGkCollisionStartJ,
                                                      ref bool isCollisionWithPlayer,
                                                      ref Vector3 ballPosWhenCollision)

    {
        int ballPosOffsetIdx = 0;
        //int ballPoMainIndex = 0;
        updatePlayerPos = true;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        predictedAnimName = animName;

        //print("DBG342344COL_NEWDBG34 initialization val " + initialization + " isOnAnimatorActive " + isOnAnimatorActive
        //    + " prepareShotMaxIdx " + prepareShotMaxIdx);
        localGkStartPos.y = Mathf.Max(0.03f, playerRigidBody.transform.position.y);

        if (!initialization)
        {
            ballVelocitySet = false;
            isCollisionWithPlayer = false;
            isOnAnimatorActive = true;
            animatorIKExecuted = false;
            isBall2Collided = false;

            //print("DEBUGGK1045#### leftHand animatorIKExecuted " + animatorIKExecuted);

            loopNumber = 1;
            //this is second ball that should hit wall - do it in parallel  using 2 ball
            ballRbScript2.setBallCollided(false);
            ballRbScript2.setBallGoalCollisionStatus(false);
            ballPosOffsetIdx = 0;

            /*Debug.Log("DBGWALL ballVelocity " + velocity + " 2: " +
                prepareShotPos[prepareShotMaxIdx - 2, 0]
                + " 3: "
                +
                prepareShotPos[prepareShotMaxIdx - 3, 0]);*/
            int basePosWall = 1;
            for (int i = 1; i <= 5; i++)
            {
                if (prepareShotMaxIdx - i < 0)
                    break;

                if (Mathf.Abs(prepareShotPos[prepareShotMaxIdx - i, 0].z) <
                    (player1MainScript.PITCH_HEIGHT_HALF - player1MainScript.BALL_NEW_RADIUS))
                {
                    basePosWall = i;
                    break;
                }
            }


            float velocity =
                     Vector3.Distance(prepareShotPos[prepareShotMaxIdx - basePosWall, 0],
                                      prepareShotPos[prepareShotMaxIdx - (basePosWall - 1), 0]) / Time.deltaTime;
            ballVelocity = velocity;

            ballRb2.transform.position =
                    prepareShotPos[prepareShotMaxIdx - basePosWall, 0];
            
            ballRb2.velocity =
              (prepareShotPos[prepareShotMaxIdx - (basePosWall - 1), 0] - prepareShotPos[prepareShotMaxIdx - basePosWall, 0]).normalized * velocity;

            /*print("DBGWALL BALLPOS " + ballRb2.transform.position
                + " ballPosWall " + basePosWall
                + " velocity " + velocity
                + " prepareShotPos[prepareShotMaxIdx - (basePosWall - 1) "
                + (prepareShotPos[prepareShotMaxIdx - (basePosWall - 1), 0])
                + " prepareShotPos[prepareShotMaxIdx - basePosWall, 0]) "
                + prepareShotPos[prepareShotMaxIdx - basePosWall, 0]);*/

            ballRb2.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            wallVelocity = new Vector3(-10000f, -10000f, -100000f);
            //print("collisionOutput ball2 position init " + ballRb2.transform.position + " vel " + ballRb2.velocity);

            ballRbScript.setBallCollided(false);
            ballRbScript.setBallGoalCollisionStatus(false);
            ballRb.velocity = Vector3.zero;
            float timeBack = (timeOfBallFly * curvePercentHit);
            //- (Time.deltaTime * 1000f);
            curvePercentHit = Mathf.InverseLerp(0.0f, timeOfBallFly, timeBack);

            /*ballRb.transform.position = BezierCuve.bezierCurve3DGetPoint(outShotStart,
                                                                         outShotMid,
                                                                         outShotEnd,
                                                                         curvePercentHit,
                                                                         true);*/
          


            //ballPrevPos = ballRb.transform.position;
            //print("DBG342344COL INIT curvePercentHIT " + curvePercentHit);
            //print("DBG342344COL INIT ball2Pos " + curvePercentHit);

            //playerRb.transform.position = playerRigidBody.transform.position;
            //playerRb.transform.rotation = playerRigidBody.transform.rotation;          
            /* print("#DBGCOLLISIONCALC1024DB virtualPlayerPos playerRb.transform BEFORE" + playerRb.transform.position +
                 " playerRotation " + playerRb.transform.eulerAngles +
                 " animName " + animName +
                 " animOffset " + animOffset + " animOffset " + animOffset
                 + " timeOfBallFly " + timeOfBallFlycurvePercentHit
                 + " curvePercentHit " + curvePercentHit
                 + " lastGkDist " + lastGkDist
                 + " playerRigidBody.transform.position BEOFER ADD LOCAL " + playerRigidBody.transform.position
                 + " localGkStartPos " + localGkStartPos);*/

            if (!animName.Equals("EMPTY"))
            {
                //print("DBGCOLLISIONCALC1024DB BEFORE pos playerRb.transform.position " + playerRb.transform.position +
                //    " localGkStartPos NOT EMTPY gkEndPos " + localGkStartPos);
                playerRb.transform.position = localGkStartPos;
                playerRb.transform.rotation = playerRigidBody.transform.rotation;

                ///print("DEBUGGK1045#### leftHand anim before## " + leftHand.transform.position
                //    + " rightHand " + rightHand.transform.position);
             

                animator.speed = 0.0001f;
                animator.Play(animName, 0, gkSideAnimPlayOffset);

                /*print("DEBUGGK1045#### leftHand anim after## " + leftHand.transform.position
            + " rightHand " + rightHand.transform.position);*/

                localGkStartPos = new Vector3(localGkStartPos.x,
                                              Mathf.Max(0.03f, playerRb.transform.position.y),
                                              localGkStartPos.z);
                playerRb.transform.position = localGkStartPos;

                //print("DBG342344COL AFTER NOT EMPTY pos playerRb.transform.position " + playerRb.transform.position +
                //    " localGkStartPos " + localGkStartPos);

                isAnimPlaying = true;
                lastGkAnimPlayed = animName;
                lastGkDistX = lastGkDist;
            }
            else
            {

                playerRb.transform.position = playerRigidBody.transform.position;
                playerRb.transform.rotation = playerRigidBody.transform.rotation;
                    //playerRigidBody.transform.rotation;
                //print("DBG342344COL EMPTY BEFORE pos playerRb.transform.position " + playerRb.transform.position +
                //  " localGkStartPos " + localGkStartPos);

                animator.speed = 0.0001f;
                animator.Play("3D_GK_stand_still", 0, 0);
                animator.Update(0f);

                playerRb.transform.position = playerRigidBody.transform.position;
                playerRb.transform.rotation = playerRigidBody.transform.rotation;

                //print("DBGCOLLISIONCALC1024DB EMPTY AFTER pos playerRb.transform.position " + playerRb.transform.position +
                //   " localGkStartPos " + localGkStartPos);

                isAnimPlaying = true;
                lastGkAnimPlayed = "3D_GK_stand_still";
                lastGkDistX = 0f;
            }

            //print("player1MainScript.peerPlayer.prepareShotMaxIdx " +
            //    player1MainScript.peerPlayer.prepareShotMaxIdx);
            /*ballRb.transform.position = player1MainScript.TransformPointUnscaled(playerRb.transform,
                                                          BezierCuve.bezierCurvePlaneInterPoint(
                                                          playerRb,
                                                          player1MainScript.peerPlayer.prepareShotPos,
                                                          0,
                                                          player1MainScript.peerPlayer.prepareShotMaxIdx - 1,
                                                          false));*/

            //rotated
            ballStartIdx = BezierCuve.bezierCurvePlaneInterPoint(
                                                       playerRb,
                                                       prepareShotPos, 
                                                       0,
                                                       prepareShotMaxIdx - 1);

            /*print("DEBUGGK1045#### ballStartIdx ballPlayerPos " + Globals.InverseTransformPointUnscaled(
                    playerRb.transform, prepareShotPos[ballStartIdx, 0])
                + " playerRb pos " + playerRb.transform
                + " prepareShotPos[ballStartIdx, 0] " + prepareShotPos[ballStartIdx, 0]
                + " ballStartIdx " + ballStartIdx);*/
            ballPoMainIndex = ballStartIdx - 1;

            if ((ballPoMainIndex + 1) == prepareShotMaxIdx)
                ballPoMainIndex--;

            //to solve when player in movement and no animation
            ballPoMainIndex = Mathf.Clamp(ballPoMainIndex, 0, prepareShotMaxIdx - 1);

            //ballRb.transform.position = prepareShotPos[ballPoMainIndex, 0];


            ballAnimatorStartPos = prepareShotPos[ballStartIdx, 0];
            ballAnimatorVirtualStartPos = prepareShotPos[ballPoMainIndex, 0];
            //prepareShotPos[ballPoMainIndex, 0];
            //prepareShotPos[ballStartIdx, 0];
            animator.Update(0f);

            offsetDown = 0;
            offsetUp = 0;
            for (int i = 0; i <= 10; i++)
            {
                if ((ballStartIdx - i) < 0)
                    break;

                if (Globals.InverseTransformPointUnscaled(
                    playerRb.transform, prepareShotPos[ballStartIdx - i, 0]).z < 2.5f) 
                {
                    offsetDown = i;
                } else
                {
                    break;
                }
            }

            for (int i = 0; i <= 10; i++)
            {
                if ((ballStartIdx + i) == prepareShotMaxIdx)
                    break;

                if (Globals.InverseTransformPointUnscaled(
                    playerRb.transform, prepareShotPos[ballStartIdx + i, 0]).z > -2.5f)
                {
                    offsetUp = i;
                } else
                {
                    break;
                }
            }

            leftHandOrgPos = leftHand.transform.position;
            rightHandOrgPos = rightHand.transform.position;

            /*onAniatorIkManual(animator,
                    ballAnimatorStartPos,
                    shotActive,
                    false,
                    2f,
                    playerRb,
                    leftHand,
                    rightHand,
                    predictedAnimName,
                    false);*/


            /*player1MainScript.onAniatorIk(
                        animator,
                        ballAnimatorStartPos,
                        shotActive,
                        false,
                        lastGkDistX,
                        playerRb,
                        leftHand,
                        rightHand,
                        predictedAnimName,
                        false,
                        true);*/
            //print("DEBUGGK1045#### leftHand after " + leftHand.transform.position + " rightHand " + rightHand.transform.position);

            //print("DEBUGGK1045#### ballStartIdx " + ballStartIdx + " startDown " +
            //    (ballStartIdx - offsetDown) + " " +
            //    " ballStartIdx + offsetUp) " + (ballStartIdx + offsetUp));
            /* ballPoMainIndex = BezierCuve.bezierCurvePlaneInterPoint(
                                  playerRb.transform.position,
                                  prepareShotPos,
                                  ballStartIdx - offsetDown,
                                  ballStartIdx,
                                  ballStartIdx + offsetUp,
                                  leftHand,
                                  rightHand,
                                  playerRb);

             ballRb.transform.position = prepareShotPos[ballPoMainIndex, 0];

             print("DEBUGGK1045 ballStartIdx ballPlayerPos " + Globals.InverseTransformPointUnscaled(
                  playerRb.transform, prepareShotPos[ballPoMainIndex, 0]));

             ballVelocity =
                    Vector3.Distance(prepareShotPos[ballPoMainIndex, 0],
                                     prepareShotPos[ballPoMainIndex + 1, 0]) / Time.deltaTime;
             ballRb.velocity =
            (prepareShotPos[ballPoMainIndex + 1, 0] - prepareShotPos[ballPoMainIndex, 0]).normalized * ballVelocity;
             ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

             print("DEBUGGK1045 ballVelocity " + ballVelocity + " " +
                 " ballRb.transform.position " + ballRb.transform.position
                 + " prepareShotPos[ballPoMainIndex, 0] " + prepareShotPos[ballPoMainIndex, 0]
                 + " prepareShotPos[ballPoMainIndex + 1, 0] " + prepareShotPos[ballPoMainIndex + 1, 0]
                 + " ballRb.velocity " + ballRb.velocity);*/


            /*onAniatorIkManual(animator,
                             ballAnimatorStartPos,
                             shotActive,
                             false,
                             lastGkDistX,
                             playerRb, 
                             leftHand,
                             rightHand,
                             predictedAnimName,
                             false);*/

         
            /*print("DEBUGGK1045#### ballVelocity " + ballVelocity + " " +
                " ballRb.transform.position " + ballRb.transform.position
                + " prepareShotPos[ballPoMainIndex, 0] " + prepareShotPos[ballPoMainIndex, 0]
                + " prepareShotPos[ballPoMainIndex + 1, 0] " + prepareShotPos[ballPoMainIndex + 1, 0]
                + " ballRb.velocity " + ballRb.velocity);
                */
            initialization = true;
            /*print("#DBG342344COL virtualPlayerPos playerRb.transform " + playerRb.transform.position +
                " playerRotation " + playerRb.transform.eulerAngles +
                " animName " + animName +
                " animOffset " + animOffset + " animOffset " + animOffset
                + " timeOfBallFly " + timeOfBallFly
                + " curvePercentHit " + curvePercentHit
                + " lastGkDist " + lastGkDist
                + " playerRigidBody.transform.position BEOFER ADD LOCAL " + playerRigidBody.transform.position
                + "  ballRb.transform.position " + ballRb.transform.position);*/


            /*float timeBack = (timeOfBallFly * curvePercentHit) - (Time.deltaTime * 1000f);
            float curvePercentHitBack = Mathf.InverseLerp(0.0f, timeOfBallFly, timeBack);

            Vector3 prevBallPos = BezierCuve.bezierCurve3DGetPoint(outShotStart,
                                                                   outShotMid,
                                                                   outShotEnd,
                                                                   curvePercentHitBack,
                                                                   false);

            Vector3 currentBallPos = BezierCuve.bezierCurve3DGetPoint(outShotStart,
                                                                      outShotMid,
                                                                      outShotEnd,
                                                                      curvePercentHit,
                                                                      false);

            ballRb.transform.position = prevBallPos;
            ballRb.velocity = (currentBallPos - prevBallPos) / Time.deltaTime;
            ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            ballRbScript.setBallCollided(false);

            initialization = true;

            print("DBG342344COL ballRb.velocity " + ballRb.velocity + " prevBallPos " + prevBallPos
                + " ballRb position " + ballRb.transform.position
                + " currentBallPos " + currentBallPos
                + " curvePercentHit " + curvePercentHit
                + " outShotEnd " + outShotEnd
                + " outShotMid " + outShotMid
                + " outShotEnd " + outShotEnd
                + " timeBack " + timeBack
                + " curvePercentHitBack " + curvePercentHitBack
                + " ballRbScript.getBallGoalCollisionStatus() " + ballRbScript.getBallGoalCollisionStatus());*/
        } else
        {
            loopNumber++;
            ballPosOffsetIdx = 1;
        }


        if (!animName.Equals("EMPTY"))
        {
           
            localGkStartPos = new Vector3(localGkStartPos.x,
                                          Mathf.Max(0.03f, playerRb.transform.position.y),
                                          localGkStartPos.z);

            playerRb.transform.position = localGkStartPos;
            playerRb.transform.rotation = playerRigidBody.transform.rotation;        
        }
        else
        {            
            playerRb.transform.position = playerRigidBody.transform.position;
            playerRb.transform.rotation = playerRigidBody.transform.rotation;
           
        }

        playerUpdatedPos = playerRb.transform.position;
        playerUpdatedPosRot = playerRb.transform.rotation;

        /*print("#DBGWALL ballRb2.velocity " + ballRb2.velocity + " ballPos " + ballRb2.transform.position);*/
 
        /*        player1MainScript.onAniatorIk(
                    animator,
                    ballAnimatorStartPos,
                    shotActive,
                    false,
                    lastGkDistX,
                    playerRb,
                    leftHand,
                    rightHand,
                    predictedAnimName,
                    false,
                    true);*/


        // if (loopNumber == 1)
        // {
        /*  ballPoMainIndex = BezierCuve.bezierCurvePlaneInterPoint(
                              playerRb.transform.position,
                              prepareShotPos,
                              ballStartIdx - offsetDown,
                              ballStartIdx,
                              ballStartIdx + offsetUp,
                              leftHand,
                              rightHand,
                              playerRb);*/

        /*     ballRb.transform.position = prepareShotPos[ballPoMainIndex, 0];

             print("DEBUGGK1045 ballStartIdx ballPlayerPos " + Globals.InverseTransformPointUnscaled(
                  playerRb.transform, prepareShotPos[ballPoMainIndex, 0])
                 + " mainIdx " + ballPoMainIndex);

             ballVelocity =
                    Vector3.Distance(prepareShotPos[ballPoMainIndex, 0],
                                     prepareShotPos[ballPoMainIndex + 1, 0]) / Time.deltaTime;

             ballRb.velocity =
                 (prepareShotPos[ballPoMainIndex + 1, 0] - prepareShotPos[ballPoMainIndex, 0]).normalized * ballVelocity;
             ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

             ballVelocitySet = true;
             print("DEBUGGK1045 ballVelocity " + ballVelocity + " " +
                 " ballRb.transform.position " + ballRb.transform.position
                 + " prepareShotPos[ballPoMainIndex, 0] " + prepareShotPos[ballPoMainIndex, 0]
                 + " prepareShotPos[ballPoMainIndex + 1, 0] " + prepareShotPos[ballPoMainIndex + 1, 0]
                 + " ballRb.velocity " + ballRb.velocity);
         }*/


        ballPoMainIndex = ballStartIdx - 1;
        if ((ballPoMainIndex + 1) == prepareShotMaxIdx)
            ballPoMainIndex--;

        //print("DEBUGGK1045#### loopNumber " + loopNumber);
        if (animatorIKExecuted)
        {
            ballPoMainIndex = ballStartIdx - 1;
            if ((ballPoMainIndex + 1) == prepareShotMaxIdx)
                ballPoMainIndex--;
            ballPoMainIndex = Mathf.Clamp(ballPoMainIndex, 0, prepareShotMaxIdx - 1);

            ballRb.transform.position = prepareShotPos[ballPoMainIndex, 0];

            /*print("DEBUGGK1045 ballStartIdx ballPlayerPos " + Globals.InverseTransformPointUnscaled(
                 playerRb.transform, prepareShotPos[ballPoMainIndex, 0])
                + " mainIdx " + ballPoMainIndex);*/

            ballVelocity =
                   Vector3.Distance(prepareShotPos[ballPoMainIndex, 0],
                                    prepareShotPos[ballPoMainIndex + 1, 0]) / Time.deltaTime;

            ballRb.velocity =
                (prepareShotPos[ballPoMainIndex + 1, 0] - prepareShotPos[ballPoMainIndex, 0]).normalized * ballVelocity;
            ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

            ballVelocitySet = true;
            //print("DEBUGGK1045#### leftHand  onanimatorIK true");
            animatorIKExecuted = false;
        }

        /*print("DEBUGGK1045#### leftHand " + leftHand.transform.position
            + " rightHand " + rightHand.transform.position
            + " animatorIKExecuted " + animatorIKExecuted
            + " rigthHandBall" + Globals.InverseTransformPointUnscaled(
                leftHand.transform, ballRb.transform.position)
            + " rightHandBall " + Globals.InverseTransformPointUnscaled(
                rightHand.transform, ballRb.transform.position)
            + " playerRbBall " + Globals.InverseTransformPointUnscaled(
                playerRb.transform, ballRb.transform.position)
            + " ballAnimatorStartPos " + ballAnimatorStartPos
            + " LeftHandToballAnimatorStartPosDist " 
            + Vector3.Distance(leftHand.transform.position, ballAnimatorStartPos)
            + " RightHandToLeftHandToballAnimatorStartPosDist" +
                Vector3.Distance(rightHand.transform.position, ballAnimatorStartPos)
            + " LeftHandToballRb.transform.positionDist" +
                 +Vector3.Distance(leftHand.transform.position, ballRb.transform.position)
            + " RightHandballRb.transform.positionDist " +
                Vector3.Distance(rightHand.transform.position, ballRb.transform.position)
            + " ballRb " + ballRb.transform.position
            + " ballVelocity " + ballRb.velocity
            + " ballStartIdx " + ballStartIdx
            + " prepareShotPos[ballPoMainIndex, 0] " + prepareShotPos[ballPoMainIndex, 0]
            + " prepareShotPos[ballPoMainIndex + 1, 0]) "
            + prepareShotPos[ballPoMainIndex + 1, 0]
            );*/


        //float timeBack = (timeOfBallFly * curvePercentHit) - (Time.deltaTime * 3000f);
        //float curvePercentHitBack = Mathf.InverseLerp(0.0f, timeOfBallFly, timeBack);

        /*ballRb.transform.position = BezierCuve.bezierCurve3DGetPoint(outShotStart,
                                                                     outShotMid,
                                                                     outShotEnd,
                                                                     curvePercentHit,
                                                                     true);*/

        //Vector3 ballPos = BezierCuve.bezierCurvePlaneInterPoint(
        //                               playerRb,
        //                               player1MainScript.peerPlayer.prepareShotPos,
        //                              0,
        //                              player1MainScript.peerPlayer.prepareShotMaxIdx - 1,
        //                             true);


        /*int ballPoMainIdx = BezierCuve.bezierCurvePlaneInterPoint(
                                    playerRb,
                                    player1MainScript.peerPlayer.prepareShotPos,
                                    0,
                                    player1MainScript.peerPlayer.prepareShotMaxIdx - 1);*/
        //ballPoMainIdx += ballPosOffsetIdx;
        //if (ballPoMainIdx == player1MainScript.peerPlayer.prepareShotMaxIdx)
        //{
        //    ballPoMainIdx--;
        //}
        //*ballRb.transform.position = player1MainScript.peerPlayer.prepareShotPos[ballPoMainIdx, 0];
        /* float ballVelocity =
            Vector3.Distance(prepareShotPos[ballPoMainIdx + 1, 0],
                             prepareShotPos[ballPoMainIdx, 0]) / Time.deltaTime;
         print("DEBUGGK1045 ballVelocity " + ballVelocity);
         ballRb.velocity =
         (prepareShotPos[ballPoMainIdx + 1, 0] - prepareShotPos[ballPoMainIdx, 0]).normalized * ballVelocity;
         ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

         */
        //print("player1MainScript.peerPlayer.prepareShotMaxIdx " + player1MainScript.peerPlayer.prepareShotMaxIdx);
        //##

        /*int ballPoMainIdx = BezierCuve.bezierCurvePlaneInterPoint(
                                       playerRb,
                                       player1MainScript.peerPlayer.prepareShotPos,
                                       0,
                                       player1MainScript.peerPlayer.prepareShotMaxIdx - 1);
        ballPoMainIdx += ballPosOffsetIdx;
        if (ballPoMainIdx == player1MainScript.peerPlayer.prepareShotMaxIdx)
        {
            ballPoMainIdx--;
        }
        //ballRb.transform.position = player1MainScript.peerPlayer.prepareShotPos[ballPoMainIdx, 0];
        float ballVelocity =
           Vector3.Distance(prepareShotPos[ballPoMainIdx - 1, 0],
                            prepareShotPos[ballPoMainIdx, 0]) / Time.deltaTime;
        print("DEBUGGK1045 ballVelocity " + ballVelocity);
        ballRb.velocity = 
        (prepareShotPos[ballPoMainIdx, 0] - prepareShotPos[ballPoMainIdx - 1, 0]).normalized * ballVelocity;
        ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
        */

        //print("#DEBUGGK1045 ballPosition local to player " +
        //    Globals.InverseTransformPointUnscaled(playerRb.transform, ballRb.transform.position)
        //    + " playerPos " + playerRb.transform.position 
        //    + " ballPos " + ballRb.transform.position
        //    + " ballVel " + ballRb.velocity);


        //Vector3 currentBallPos = BezierCuve.bezierCurve3DGetPoint(outShotStart,
        //                                                          outShotMid,
        //                                                          outShotEnd,
        //                                                         curvePercentHit,
        //                                                        true);


        //ballRb.velocity = new Vector3((currentBallPos.x - ballPrevPos.x) / Time.deltaTime,
        //                              (currentBallPos.y - ballPrevPos.y) / Time.deltaTime,
        //                              (currentBallPos.z - ballPrevPos.z) / Time.deltaTime);

        //print("COLLISIONDBG123 Ball velocity collision " + ballRb.velocity + " BALL pOS" + ballRb.transform.position +
        //    "  curvePercentHit " + curvePercentHit
        //    + " currentBallPos " + currentBallPos);

        //print("DBGVEL1CALC ballRb2.pos " + ballRb2.transform.position + " vel " + ballRb2.velocity);

        ///ballRb.transform.position = currentBallPos;
        //ballPrevPos = currentBallPos;
        ///ballRb.transform.position = ballPos;
        ballPrevPos = ballRb.transform.position;

        //ballRb.velocity = Vector3.zero;
    
        /*print("DEBUGGK1045 simulation position "
              + " playerRbPos " + playerRb.transform.position
              + " ballPos " + ballRb.transform.position
              + " ballRbPos " + Globals.InverseTransformPointUnscaled(playerRb.transform, ballRb.transform.position)
              + " leftHandLocalBall " + Globals.InverseTransformPointUnscaled(leftHand.transform, ballRb.transform.position)
              + " rightHandLocalBAll " + Globals.InverseTransformPointUnscaled(rightHand.transform, ballRb.transform.position)
              + " leftHand Global " + leftHand.transform.position
              + " rightHand Global " + rightHand.transform.position
              + " LeftHandLocalRb " + Globals.InverseTransformPointUnscaled(playerRb.transform, leftHand.transform.position)
              + " RightHandLocalRb " + Globals.InverseTransformPointUnscaled(playerRb.transform, rightHand.transform.position));*/


        //curvePercentHit = Mathf.InverseLerp(0.0f, 
        //                                    timeOfBallFly, 
        //                                    (timeOfBallFly * curvePercentHit) - (Time.deltaTime * 2000f));  

        /*print("DBG342344COL AFTER SET BAL ball velocity " + ballRb.velocity + " ballRb " + ballRb.transform.position
             + " outShotSTart " + outShotStart
             + " outShotMid " + outShotMid
             + " outShotEnd " + outShotEnd
             + " curvePercentHit " + curvePercentHit
             + " ballRbScript.getBallCollided() " + ballRbScript.getBallCollided());


        print("DEBUGGK1045 playerRb.transform.position " + playerRb.transform.position
        + " rotation " + playerRb.transform.eulerAngles
        + " ballRb " + ballRb.transform.position
        + " localGkStartPos " + localGkStartPos
        + " leftHand " + Globals.player1MainScript.getChildWithName(player, "virtualPlayer1LeftHand").transform.position
        + " rightHand " + Globals.player1MainScript.getChildWithName(player, "virtualPlayer1RightHand").transform.position
        + " leftPalm " + Globals.player1MainScript.getChildWithName(player, "virtualPlayer1LeftPalm").transform.position
        + " rightPalm " + Globals.player1MainScript.getChildWithName(player, "virtualPlayer1RightPalm").transform.position);

        print("DBG342344COL setIsOnAniamtor to " + isOnAnimatorActive);*/


        //ballRb.transform.position = prevBallPos;
        //ballRb.velocity = (currentBallPos - prevBallPos) / Time.deltaTime;

        if (ballRbScript2.getBallCollided() &&
            !isBall2Collided)
        {
            //print("collisionOutput collided with wall ballPos " + ballRb2.transform.position +
            //    " vel " + ballRb2.velocity);
            wallVelocity = ballRbScript2.getBallCollidingSpeed();
            ballPosWhenCollision = ballRb2.transform.position;

            //print("DBGWALL wallVelocity " + wallVelocity + " ballPosWhenCollision " + ballPosWhenCollision);
            ballRb2.transform.position = new Vector3(1000f, 1000f, 1000f);
            ballRb2.velocity = Vector3.zero;
            ballRb2.angularVelocity = Vector3.zero;
            isBall2Collided = true;
        }

        if (!ballRbScript.getBallGoalCollisionStatus() &&
            !ballRbScript.getBallCollided() &&
            !ballRbScript2.getBallCollided())
            return Globals.INCORRECT_VECTOR;

        if (ballRbScript2.getBallCollided() && 
            !ballRbScript.getBallCollided() && 
            predictGkCollisionStartJ <= 1)
        {
            return Globals.INCORRECT_VECTOR;
        }

        //print("collisionOutput ballRbScript Speeed " + ballRbScript.getBallCollidingSpeed());

        //ballRb.velocity = new Vector3(9.1f, 6.6f, -28f);
        //print("#DBG Velocity "
        //    + ballRb.velocity
        //    + " currentBAllPos "
        //    + " Time.deltaTime " + Time.deltaTime);

        //shotRet = true;
        //ballRb.transform.position = new Vector3(1.1f, 3.3f, -11.9f);
        //shotRet = true;
        if (ballRbScript.getBallGoalCollisionStatus() ||
            ballRbScript2.getBallGoalCollisionStatus())
        {
            //print("DBGCOLLISIONCALC1024D GOAL!!!!! ");
            ballRbScript.setBallGoalCollisionStatus(false);
            ballRbScript2.setBallGoalCollisionStatus(false);

            return Globals.INCORRECT_VECTOR_2;
        }

        ballRb.transform.position = new Vector3(1000f, 1000f, 1000f);
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        animatorIKExecuted = false;
        ballRb2.transform.position = new Vector3(0, 100f, 0);
        ballRb2.velocity = Vector3.zero;
        ballRb2.angularVelocity = Vector3.zero;
        isOnAnimatorActive = false;
        animator.speed = 0.0f;

        if (ballRbScript.getBallCollided())
        {
            ballRbScript.setBallCollided(false);
            isCollisionWithPlayer = true;
            ballPosWhenCollision = ballRb.transform.position;

            return ballRbScript.getBallCollidingSpeed();
        }
        else
        {
            ballRbScript2.setBallCollided(false);
            float minVal = 1000000f;
            int minValIdx = 0;
            for (int i = 1; i <= 3; i++)
            {
                float currVal =
                    0.5f - (14f - Mathf.Abs(prepareShotPos[prepareShotMaxIdx - i, 0].z));
                if (currVal < minVal)
                {
                    minVal = currVal;
                    minValIdx = i;
                }
            }

            //curvePercentHit = prepareShotPos[prepareShotMaxIdx - minValIdx, 1].x - 0.001f;
            curvePercentHit = prepareShotPos[prepareShotMaxIdx - 2, 1].x;
            isCollisionWithPlayer = false;
            //print("DBGWALL collision predicted wall " + wallVelocity + " curvePercentHit " + curvePercentHit
            //    + "ballPosWhenCollision "+ ballPosWhenCollision);
            return wallVelocity;
        }    
    }

    /*public Vector3 predictBallPosDuringShot(string animName, float animOffset)
    {
        animator.Play(animName, 0, animOffset);
        animator.Update(0f);
        animator.speed = 0f;
    }*/

    static public void setBallVelocitySet(bool val)
    {
        ballVelocitySet = val;
    }

    static public bool getBallVelocitySet()
    {
        return ballVelocitySet;
    }

    static public void setOnAnimatorAcitve(bool val)
    {
        //print("DBG342344COL setOnAnimatorACtive " + val);
        isOnAnimatorActive = val;
    }

    static public bool getOnAnimatorAcitve()
    {
        return isOnAnimatorActive;
    }

    void updatePlayePosition()
    {
        if (updatePlayerPos)
        {
            playerRb.transform.position = playerUpdatedPos;
            playerRb.transform.rotation = playerUpdatedPosRot;
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        updatePlayePosition();
        updatePlayerPos = false;
        if (playerRb.transform.position.y < 0.03f)
            playerRb.transform.position =
                new Vector3(playerRb.transform.position.x, 0.03f, playerRb.transform.position.z);
    }

    void FixedUpdate()
    {
        updatePlayePosition();
    }

    /*void FixedUpdate()
    {


        float speedPerc = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly);
        setBallShotVel(speedPerc * MAX_SHOT_SPEED_UNITY_UNITS);

        if (!shotRet && shotActive)
        {
            float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);

            shotRet = shot3New(outShotStart,
                               outShotMid,
                               outShotEnd,                      
                               normalizeTime);
            shotCurrentPlace = normalizeTime;
            passedShotFlyTime = passedShotFlyTime + (Time.deltaTime * 1000.0f);
            print("NORMALIZETIME " + normalizeTime + " passedShotFlyTime " + passedShotFlyTime +
                " timeofBallFly " + timeofBallFly + " shotRet " + shotRet + "" +
                " ballRbVelocity " + ballRb.velocity);
        }
    }*/

    public void setBallShotVel(float speed)
    {
        ballShotVelocity = speed;
    }

    public string getLastAnimPlayed()
    {
        return lastGkAnimPlayed;
    }

    public float getShotCurrentPlace()
    {
        return shotCurrentPlace;
    }

    public string getLastGkAnimPlayed()
    {
        return lastGkAnimPlayed;
    }

    public Vector3 getPlayerPosition()
    {
        return player.transform.position;
    }

    public float getBallShotVel()
    {
        return ballShotVelocity;
    }

    public Transform getRbTransform()
    {
        return player.transform;
    }

    public Vector3 predictColissionAndSolve(Animator animator, 
                                            GameObject ball)
    {

        return Vector3.zero;
    }

    //take it from controllerRigid

    /*public bool doesBallColliding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
                ballRb.transform.position, BALL_NEW_RADIUS,
                Physics.AllLayers,
                QueryTriggerInteraction.Collide);

        foreach (Collider collider in hitColliders)
        {
            print("#DBG COLLISION " + collider.name);

            if (collider.name.Contains("Hand") ||
                collider.name.Contains("ForeArm") ||
                collider.name.Contains("Shoulder") ||
                collider.name.Contains("Head") ||
                collider.name.Contains("Spine") ||
                collider.name.Contains("Area"))
                print("#DBGHITCOLLIDERS CPU MAIN " + collider.name);
        }

        return true;

        return false;
    }*/

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

    void OnAnimatorIK(int layerIndex)
    {
        if (///(Globals.peersReady < Globals.MAX_PLAYERS) ||
             !player1MainScript.arePeersPlayerSet())
             //!player1MainScript.photonView.IsMine)
        {
            return;
        }

        if (isOnAnimatorActive)
        {
            animatorIKExecuted = true;

            //print("DEBUGGK1045#### leftHand onAnimatorIK executed in simulation " + isOnAnimatorActive);
            player1MainScript.onAniatorIk(
                animator,
                //ballAnimatorStartPos,
                ballAnimatorVirtualStartPos,
                true,
                false,
                lastGkDistX,
                playerRb,
                leftHand,
                rightHand,
                predictedAnimName,
                false,
                true);
        }

        ///onAnimIK();
    }


    static public void onAnimIK()
    {
        if (isOnAnimatorActive)
        {
            //print("onAnimatorIK executed " + isOnAnimatorActive);
            player1MainScript.onAniatorIk(
                animator,
                ballAnimatorStartPos,
                shotActive,
                false,
                lastGkDistX,
                playerRb,
                leftHand,
                rightHand,
                predictedAnimName,
                false,
                true);
        }
    }

    static public void onAniatorIkManual(Animator animator,
                           Vector3 ballPosition,
                           bool shotActive,
                           bool isLobActive,
                           float distX,
                           Rigidbody rb,
                           GameObject leftPalm,
                           GameObject rightPalm,
                           string anim,
                           bool isCpu)
    {

        ///print("DBG342344COL_NEWDBG34 onAnimatorIK executed beging POS BAll ###" + ballRb.transform.position + " isOnAnimatorActive " +
        //    isOnAnimatorActive);
        if (Globals.player1MainScript == null ||
            !isOnAnimatorActive)
            return;

        if ((Globals.player1MainScript.gkDistRealClicked > 
            (Globals.player1MainScript.MIN_DIST_REAL_CLICKED + 1.0f)))
        {

            /*print("DEBUGGK1045#### " + " Globals.player1MainScript.gkDistRealClicked " +
                Globals.player1MainScript.gkDistRealClicked
                + " (Globals.player1MainScript.MIN_DIST_REAL_CLICKED " +
                (Globals.player1MainScript.MIN_DIST_REAL_CLICKED));*/

            //  if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            //      (shotActive))
            //          print("#DBGANIMATOR gkDistRealClicked dist too big ####");
            return;
        }


        //float reach = 0.12f;
        //if (anim.Contains("punch"))
        //    reach = 0.16f;
        Vector3 handPos = Vector3.zero;
        float reach = 0.3f;
        float reachX = 0.5f;
        float reachY = 0.6f;
        float reachZ = 0.3f;
        if (anim.Contains("punch")) {
           reachX = 0.5f;
           reachY = 0.6f;
           reachZ = 0.3f;
        }

        //Vector3 ballInLocalRb =
        //    player1MainScript.InverseTransformPointUnscaled(rb.transform, ballPosition);

        //print("DEBUGGK1045 DBG342344COL_NEWDBG34 onAnimatorIK executed  ballRb" + ballRb.transform.position + " ballInLocalRb " + ballInLocalRb);

        //if (ballInLocalRb.z < 0f) return;

        bool isGktStraightPlaying = predictedAnimName.Contains("straight");
        //player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "straight");

        if (isGktStraightPlaying &&
            predictedAnimName.Contains("chest"))
            ///player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "chest"))
            return;

        //Vector3 ballPosition = ballRb.transform.position;
        if (isGktStraightPlaying &&
            ballPosition.y > 0.8f)
        {

            Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);

            //if (ballInLocalRb.z < 0.0f) return;

            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
            //animator.SetIKPosition(AvatarIKGoal.RightHand, ballRb.transform.position);
            leftHand.transform.position = leftHandOrgPos + 
                ((ballPosition - leftHand.transform.position).normalized * reach);
            rightHand.transform.position = rightHandOrgPos +
                ((ballPosition - rightHand.transform.position).normalized * reach);


            //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
            //animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb.transform.position);
            ///print("DBG342344COL_NEWDBG34 onAnimatorIK executed straight " + ballRb.transform.position);

            return;
        }

  
        if (predictedAnimName.Contains("3D_GK_sidecatch") &&
            //player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            (shotActive))
        {

            //print("DEBUGGK1045 DBG342344COL_NEWDBG34 onAnimatorIK executed predictedAnimName " + predictedAnimName);

            Vector3 ballPos = ballPosition;
            Vector3 whereBallHit = ballPos;
            Vector3 whereBallHitUp =
                new Vector3(ballPos.x, ballPos.y + Globals.BALL_NEW_RADIUS, ballPos.z);
            Vector3 whereBallHitDown =
                new Vector3(ballPos.x, Mathf.Max(0f, ballPos.y - Globals.BALL_NEW_RADIUS), ballPos.z);
            //print("DEBUGGK1045 balPos " + ballPos + " whereBallHit " + whereBallHit
            //    + " whereBallHitDown " + whereBallHitDown
            //    + " whereBallHitUp " + whereBallHitUp);
            float distance = 0f;
            float distanceY = 0f;
            ballRbRightSide.transform.position = whereBallHitUp;
            ballRbLeftSide.transform.position = whereBallHitDown;

            if ((anim.Contains("right") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX >= 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {

                //Quaternion rightHandRotation =
                //    Quaternion.LookRotation(ballPosition - rightHand.transform.position);

//                print("DEBUGGK1045 onAnimatorIK executed right " + predictedAnimName);

                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        //animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitUp);


  //                      print("DEBUGGK1045 rightHand.transform.position BEOFRE pos " + rightHand.transform.position);

                        /*print("DEBUGGK1045 onAnimatorIK executed before rightHand " + rightHand.transform.position
                         + " rightHandOrgPos " + rightHandOrgPos + " animName " + anim
                         + " whereBallHitUp " + whereBallHitUp);*/

                        distance = 
                            Mathf.Abs(rightHand.transform.position.x - whereBallHitUp.x);
                        handPos.x = rightHandOrgPos.x +
                            Globals.getSign(whereBallHitUp.x - rightHand.transform.position.x) * 
                            Mathf.Min(distance, reachX);

                        distance =
                          Mathf.Abs(rightHand.transform.position.y - whereBallHitUp.y);
                        handPos.y = rightHandOrgPos.y +
                            Globals.getSign(whereBallHitUp.y - rightHand.transform.position.y) *
                            Mathf.Min(distance, reachY);

                        distance =
                            Mathf.Abs(rightHand.transform.position.z - whereBallHitUp.z);
                        handPos.z = rightHandOrgPos.z +
                                Globals.getSign(whereBallHitUp.z - rightHand.transform.position.z) *
                                Mathf.Min(distance, reachZ);

                        rightHand.transform.position = handPos;
                        //rightHand.transform.position = rightHandOrgPos +
                        //    ((whereBallHitUp - rightHand.transform.position).normalized * 
                        //    Mathf.Min(distance, reach));

                       /* print("DEBUGGK1045 onAnimatorIK executed after rightHand " + rightHand.transform.position
                        + " rightHandOrgPos " + rightHandOrgPos + " animName " + anim
                        + " whereBallHitUp " + whereBallHitUp + " dist " + distance);

                        print("DEBUGGK1045 rightHand.transform.position AFTER pos " + rightHand.transform.position
                            + " whereBallHitUp  " 
                            + whereBallHitUp
                            + " (whereBallHitUp - rightHand.transform.position).normalized " 
                            + ((whereBallHitUp - rightHand.transform.position).normalized)
                            + " whereBallHitUp - rightHand.transform.position).normalized * reach)"
                            + ((whereBallHitUp - rightHand.transform.position).normalized * reach));*/
//

  //                  print("DBG342344COL_NEWDBG34 onAnimatorIK executed right ballHitUP " + whereBallHitUp);

                       // rightHandRotation =
                       //     Quaternion.LookRotation(whereBallHitUp - rightHand.transform.position);
                    }
                    else
                    {
                        //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        //animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitDown);

                        /*print("DEBUGGK1045 onAnimatorIK executed before rightHand " + rightHand.transform.position
                         + " rightHandOrgPos " + rightHandOrgPos + " animName " + anim
                         + " whereBallHitDown " + whereBallHitDown);*/

                        distance =
                          Mathf.Abs(rightHand.transform.position.x - whereBallHitDown.x);
                        handPos.x = rightHandOrgPos.x +
                            Globals.getSign(whereBallHitDown.x - rightHand.transform.position.x) *
                            Mathf.Min(distance, reachX);

                        distance =
                          Mathf.Abs(rightHand.transform.position.y - whereBallHitDown.y);
                        handPos.y = rightHandOrgPos.y +
                            Globals.getSign(whereBallHitDown.y - rightHand.transform.position.y) *
                            Mathf.Min(distance, reachY);

                        distance =
                            Mathf.Abs(rightHand.transform.position.z - whereBallHitDown.z);
                        handPos.z = rightHandOrgPos.z +
                                Globals.getSign(whereBallHitDown.z - rightHand.transform.position.z) *
                                Mathf.Min(distance, reachZ);

                        rightHand.transform.position = handPos;



                        /*print("DEBUGGK1045 onAnimatorIK executed after rightHand " + rightHand.transform.position
                         + " rightHandOrgPos " + rightHandOrgPos + " animName " + anim
                         + " whereBallHitDown " + whereBallHitDown + " distance " + distance);
                        print("DEBUGGK1045 onAnimatorIK executed right ballHitDown " + whereBallHitDown);*/
   
                        //rightHandRotation =
                       //  Quaternion.LookRotation(whereBallHitDown - rightHand.transform.position);
                    }
                }
                else
                {
                    //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                    //animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHit);
                    ///print("DEBUGGK1045 onAnimatorIK executed before rightHand " + rightHand.transform.position
                   ///       + " rightHandOrgPos " + rightHandOrgPos + " animName " + anim);


                    distance =
                        Mathf.Abs(rightHand.transform.position.x - whereBallHit.x);
                    handPos.x = rightHandOrgPos.x +
                        Globals.getSign(whereBallHit.x - rightHand.transform.position.x) *
                        Mathf.Min(distance, reachX);

                    distance =
                      Mathf.Abs(rightHand.transform.position.y - whereBallHit.y);
                    handPos.y = rightHandOrgPos.y +
                        Globals.getSign(whereBallHit.y - rightHand.transform.position.y) *
                        Mathf.Min(distance, reachY);

                    distance =
                        Mathf.Abs(rightHand.transform.position.z - whereBallHit.z);
                    handPos.z = rightHandOrgPos.z +
                            Globals.getSign(whereBallHit.z - rightHand.transform.position.z) *
                            Mathf.Min(distance, reachZ);

                    rightHand.transform.position = handPos;


                    //print("DEBUGGK1045 onAnimatorIK executed after rightHand " + rightHand.transform.position
          //+ " rightHandOrgPos " + rightHandOrgPos + " animName " + anim + " whereBallHit " + whereBallHit
          //+ " distance " + distance);
                }

               // rightHand.transform.rotation = rightHandRotation;
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
                //animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
            }

            if ((anim.Contains("left") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX < 0.0f) ||
                 (!anim.Contains("punch") && !anim.Contains("straight")))
            {
                //print("DEBUGGK1045 onAnimatorIK executed left  " + predictedAnimName);

             //   Quaternion leftHandRotation =
             //       Quaternion.LookRotation(ballPosition - leftHand.transform.position);
                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        //animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitDown);

                        //print("DEBUGGK1045 onAnimatorIK executed before leftHandPos " + leftHand.transform.position
                        //   + " leftHandOrgPos " + leftHandOrgPos + " animName " + anim);

                        distance =
                            Mathf.Abs(leftHand.transform.position.x - whereBallHitDown.x);
                        handPos.x = leftHandOrgPos.x +
                            Globals.getSign(whereBallHitDown.x - leftHand.transform.position.x) *
                            Mathf.Min(distance, reachX);

                        distance =
                          Mathf.Abs(leftHand.transform.position.y - whereBallHitDown.y);
                        handPos.y = leftHandOrgPos.y +
                            Globals.getSign(whereBallHitDown.y - leftHand.transform.position.y) *
                            Mathf.Min(distance, reachY);

                        distance =
                            Mathf.Abs(leftHand.transform.position.z - whereBallHitDown.z);
                        handPos.z = leftHandOrgPos.z +
                                Globals.getSign(whereBallHitDown.z - leftHand.transform.position.z) *
                                Mathf.Min(distance, reachZ);
                        leftHand.transform.position = handPos;

                        //print("DEBUGGK1045 onAnimatorIK executed after leftHandPos " + leftHand.transform.position
                        //    + " leftHandOrgPos " + leftHandOrgPos + " distance " + distance);
                        //print("DEBUGGK1045 onAnimatorIK executed left ballHitLeftDown " + whereBallHitDown);

                        //leftHandRotation =
                        //    Quaternion.LookRotation(whereBallHitDown - leftHand.transform.position);
                    }
                    else
                    {
                        ///animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        ///animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitUp);
                        //print("DEBUGGK1045 onAnimatorIK executed before leftHandPos " + leftHand.transform.position
                        //  + " leftHandOrgPos " + leftHandOrgPos + " animName " + anim);

                        distance =
                             Mathf.Abs(leftHand.transform.position.x - whereBallHitUp.x);
                        handPos.x = leftHandOrgPos.x +
                            Globals.getSign(whereBallHitUp.x - leftHand.transform.position.x) *
                            Mathf.Min(distance, reachX);

                        distance =
                          Mathf.Abs(leftHand.transform.position.y - whereBallHitUp.y);
                        handPos.y = leftHandOrgPos.y +
                            Globals.getSign(whereBallHitUp.y - leftHand.transform.position.y) *
                            Mathf.Min(distance, reachY);

                        distance =
                            Mathf.Abs(leftHand.transform.position.z - whereBallHitUp.z);
                        handPos.z = leftHandOrgPos.z +
                                Globals.getSign(whereBallHitUp.z - leftHand.transform.position.z) *
                                Mathf.Min(distance, reachZ);
                        leftHand.transform.position = handPos;

                        //print("DEBUGGK1045 onAnimatorIK executed after leftHandPos " + leftHand.transform.position
                        //  + " leftHandOrgPos " + leftHandOrgPos);
                        //print("DEBUGGK1045 onAnimatorIK executed left ballHitLeftUp " + whereBallHitUp);


                        ///leftHandRotation =
                         //   Quaternion.LookRotation(whereBallHitUp - leftHand.transform.position);
                    }
                }
                else
                {
                    ///animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                    ///animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHit);
                    ///
                    //print("DEBUGGK1045 onAnimatorIK executed before leftHandPos " + leftHand.transform.position
                    //   + " leftHandOrgPos " + leftHandOrgPos + " animName " + anim);

                    distance =
                          Mathf.Abs(leftHand.transform.position.x - whereBallHit.x);
                    handPos.x = leftHandOrgPos.x +
                            Globals.getSign(whereBallHit.x - leftHand.transform.position.x) *
                            Mathf.Min(distance, reachX);

                    distance =
                      Mathf.Abs(leftHand.transform.position.y - whereBallHit.y);
                    handPos.y = leftHandOrgPos.y +
                            Globals.getSign(whereBallHit.y - leftHand.transform.position.y) *
                            Mathf.Min(distance, reachY);

                    distance =
                        Mathf.Abs(leftHand.transform.position.z - whereBallHit.z);
                    handPos.z = leftHandOrgPos.z +
                            Globals.getSign(whereBallHit.z - leftHand.transform.position.z) *
                            Mathf.Min(distance, reachZ);
                    leftHand.transform.position = handPos;

                    //print("DEBUGGK1045 onAnimatorIK executed after leftHand " + leftHand.transform.position
          //+ " rightHandOrgPos " + leftHandOrgPos + " animName " + anim + " whereBallHit " + whereBallHit
          //+ " distance " + distance);

            //    print("DEBUGGK1045 onAnimatorIK executed LeftHand ### " + whereBallHit);

                }

                //leftHand.transform.rotation = leftHandRotation;

                //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                //animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            }
            return;
        }

        return;
    }


        static public void onAniatorIk(Animator animator,
                           bool shotActive,
                           bool isLobActive,
                           float distX,
                           Rigidbody rb,
                           GameObject leftPalm,
                           GameObject rightPalm,
                           string anim,
                           bool isCpu)
    {

            ///print("DBG342344COL_NEWDBG34 onAnimatorIK executed beging POS BAll ###" + ballRb.transform.position + " isOnAnimatorActive " +
            //    isOnAnimatorActive);
            if (Globals.player1MainScript == null ||
                !isOnAnimatorActive)
                return;


            //TOUNCOMMENT
            ///if (!isCpu && (player1MainScript.gkDistRealClicked > (player1MainScript.MIN_DIST_REAL_CLICKED + 1.0f)))
            //{
            //    return;
            //}

            Vector3 ballInLocalRb =
                player1MainScript.InverseTransformPointUnscaled(rb.transform, ballRb.transform.position);

            //print("DBG342344COL_NEWDBG34 onAnimatorIK executed  ballRb" + ballRb.transform.position + " ballInLocalRb " + ballInLocalRb);
               
            if (ballInLocalRb.z < 0.0f) return;
      
            bool isGktStraightPlaying = predictedAnimName.Contains("straight");
                //player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "straight");

            if (isGktStraightPlaying &&
                predictedAnimName.Contains("chest"))
                ///player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "chest"))
                return;

            if (isGktStraightPlaying &&
                ballRb.transform.position.y > 0.8f)
            {
                float reach = 1.0f;

                Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
                Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
                //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);

                //if (ballInLocalRb.z < 0.0f) return;

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                animator.SetIKPosition(AvatarIKGoal.RightHand, ballRb.transform.position);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb.transform.position);
                //print("DBG342344COL_NEWDBG34 onAnimatorIK executed straight " + ballRb.transform.position);

                return;
            }


            float distLeftHand = 0f;
            float distRightHand = 0f;


        if (predictedAnimName.Contains("3D_GK_sidecatch") &&
            //player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            (shotActive))
        {

            float reach = 1.00f;
  
//            print("DBG342344COL_NEWDBG34 onAnimatorIK executed predictedAnimName " + predictedAnimName);

            Vector3 ballPos = ballRb.transform.position;
            Vector3 whereBallHit = ballPos;
            Vector3 whereBallHitUp =
                new Vector3(ballPos.x, ballPos.y + Globals.BALL_NEW_RADIUS, ballPos.z);
            Vector3 whereBallHitDown =
                new Vector3(ballPos.x, Mathf.Max(0f, ballPos.y - Globals.BALL_NEW_RADIUS), ballPos.z);

            ballRbRightSide.transform.position = whereBallHitUp;
            ballRbLeftSide.transform.position = whereBallHitDown;

            if ((anim.Contains("right") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX >= 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {

                Quaternion rightHandRotation =
                    Quaternion.LookRotation(ballRb.transform.position - rightHand.transform.position);

                //print("DBG342344COL_NEWDBG34 onAnimatorIK executed right " + predictedAnimName);

                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitUp);

                        //print("DBG342344COL_NEWDBG34 onAnimatorIK executed right ballHitUP " + whereBallHitUp);

                        rightHandRotation =
                            Quaternion.LookRotation(whereBallHitUp - rightHand.transform.position);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitDown);

                        //print("DBG342344COL_NEWDBG34 onAnimatorIK executed right ballHitDown " + whereBallHitDown);

                        /*if (isCpu)
                            print("DISTHANDFROMBALL_# CPU 3D_GK_sidecatch_right_ "
                                + cpuRightHand.transform.position
                                + " whereBallHitDown "
                                + whereBallHitDown);
                        else
                            print("DISTHANDFROMBALL_# PLAYER 3D_GK_sidecatch_right_ " +
                                rightHand.transform.position
                                + " whereBallHitDown " +
                                whereBallHitDown);*/


                        rightHandRotation =
                         Quaternion.LookRotation(whereBallHitDown - rightHand.transform.position);
                    }
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHit);
                }

                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
            }

            if ((anim.Contains("left") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX < 0.0f) ||
                 (!anim.Contains("punch") && !anim.Contains("straight")))
            {

                //print("DBG342344COL_NEWDBG34 onAnimatorIK executed left  " + predictedAnimName);

                Quaternion leftHandRotation =
                    Quaternion.LookRotation(ballRb.transform.position - leftHand.transform.position);
                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitDown);

                  //      print("DBG342344COL_NEWDBG34 onAnimatorIK executed left ballHitLeftDown " + whereBallHitDown);



                        leftHandRotation =
                            Quaternion.LookRotation(whereBallHitDown - leftHand.transform.position);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitUp);

                    //    print("DBG342344COL_NEWDBG34 onAnimatorIK executed left ballHitLeftUp " + whereBallHitUp);


                        leftHandRotation =
                            Quaternion.LookRotation(whereBallHitUp - leftHand.transform.position);
                    }
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHit);

                   // print("DBG342344COL_NEWDBG34 onAnimatorIK executed LeftHand ### " + whereBallHit);

                }


                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            }
            return;
        }

        return;
    
       /* bool isGktStraightPlaying = false;
      
        if (Globals.player1MainScript == null)
            return;

        if (player1MainScript.checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch"))          
        {
            float reach = 1.00f;
            if (!isCpu)
            {
                reach = 1.0f;     
            }

            Vector3 ballInLocalRb = InverseTransformPointUnscaled(rb.transform, ballRb.transform.position);

            if (ballInLocalRb.z < 0.0f) return;
        

            if ((anim.Contains("right") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX >= 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                animator.SetIKPosition(AvatarIKGoal.RightHand, ballRb.transform.position);

                Quaternion rightHandRotation =
                    Quaternion.LookRotation(ballRb.transform.position - rightHand.transform.position);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
            }

            if ((anim.Contains("left") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX < 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {                
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb.transform.position);
                Quaternion leftHandRotation =
                    Quaternion.LookRotation(ballRb.transform.position - leftHand.transform.position);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            }

            return;
        }

        return;*/
    }

    public void OnCollisionEnter(Collision other)
    {
        if (PredictCollision.getBallVelocitySet())
        {
            //print("DEBUGGK1045#### collisionOutput ######## OnCollisionEnter " + other.collider.name
            //    + " ballRb.transform.position " + ballRb.transform.position);
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (PredictCollision.getBallVelocitySet())
        {
            //print("DEBUGGK1045#### collisionOutput ######## OnCollisionStay " + other.collider.name + " ballRb.transform.position " +
            //    ballRb.transform.position);
        }
    }

    public Vector3 InverseTransformPointUnscaled(Transform transform, Vector3 position)
    {
        Matrix4x4 worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
        return worldToLocalMatrix.MultiplyPoint3x4(position);
    }

    public Vector3 TransformPointUnscaled(Transform transform, Vector3 position)
    {
        Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        return localToWorldMatrix.MultiplyPoint3x4(position);
    }    
}

