using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalsNS;
using Photon.Pun;
using AudioManagerMultiNS;



public enum POWERSMODE
{
    EXTRA_GOALS = 1,
    GOAL_WALLS = 2,
    ENLARGE_GOAL,

}
public class PowersMulti : MonoBehaviour
{
    public enum POWER
    {
        TWO_EXTRA_GOAL = 1,
        CUT_GOAL_BY_HALF,
        ENLARGE_GOAL,
        SILVER_BALL,
        SHAKE_CAMERA,
        INVISABLE_PLAYER,
        GOAL_WALL,
        BAD_CONDITIONS,
        ENABLE_FLARE,
        GOLDEN_BALL
    }

    private int MAX_POWERS = 3;
    private bool[] RPC_confirmation;
    private float[] powerMaxTime;

    private playerControllerMultiplayer playerMainScript;
    private GameObject wallUpLeftTop;
    private GameObject wallUpRightTop;
    private GameObject wallUpLeft1;
    private GameObject wallUpLeft2;
    private GameObject wallUpRight1;
    private GameObject wallUpRight2;
        
    private GameObject wallDownLeft1;
    private GameObject wallDownLeft2;
    private GameObject wallDownRight1;
    private GameObject wallDownRight2;

    private GameObject wallDownLeftGround1;
    private GameObject wallDownLeftGround2;
    private GameObject wallDownRightGround1;
    private GameObject wallDownRightGround2;

    public GameObject[] smallGoal;
    public GameObject[] powerButtons;
    public GameObject[] goalDownObstacles;
    public GameObject[] goalUpObstacles;
    public Image[] powerButtonImg;
    private float ADD_SMALL_GOALS_TIME = 2.5f;
    private float ENLARGE_OPPONENT_GOAL_TIME = 2.5f;
    private float FREEZE_PLAYER_TIME = 2.5f;
    private float GOALS_OBSTACLES_TIME = 2f;
    private AudioManager audioManager;
    public GameObject goalUp;
    public GameObject goalDown;
    public GameObject goalDownBigger;
    public GameObject goalUpBigger;
    public Cloth goalDownCloth;
    public Cloth goalUpCloth;
    public GameObject goalDownStandardColliders;
    public GameObject goalDownExtraGoalsColliders;
    public GameObject goalDownEnlargeColliders;
    public GameObject goalUpExtraGoalsColliders;
    public GameObject goalUpEnlargeColliders;
    public GameObject goalUpStandardColliders;
    private bool[] cpuPowersUsed;
    private int[] playerPowerLock;
    private bool cpuPowerLock = false;
    private bool mainPlayerLock = false;
    private bool isPlayerGoalEnlarge = false;
    private bool isCpuGoalEnlarge = false;
    private float[] rpc_shotPercent;

    public GameObject[] goalUpFlare;
    public GameObject[] goalDownFlare;

    public bool[] rpc_powers_state;


    private bool isPowerEnable = true;
    PhotonView photonView;
    private bool isMaster = false;

    public ParticleSystem[] snowParticle;
    private bool[] isPlayerSlowDown;

    [PunRPC]
    void RPC_extraPower(bool shotActive,
                        float shotPercent,
                        byte idx,
                        PhotonMessageInfo info)
    {
        float lagDelay = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
        rpc_shotPercent[idx] = shotPercent;

        photonView.RPC("RPC_PACKET_ACK",
                       RpcTarget.Others,
                       idx);

        if (Globals.PITCHTYPE.Equals("STREET"))
        {


            if (idx == (int) POWER.GOAL_WALL)
            {
                if (!shotActive)
                {
                    goalObstacles(
                        isMaster,
                        GOALS_OBSTACLES_TIME - lagDelay,
                        false);
                    return;
                }
                else
                {
                    rpc_powers_state[(int)POWERSMODE.GOAL_WALLS] = true;
                }
            }
            return;
        }

        //print("DBGEXTRAPOWERS lagDelay " + lagDelay + " idx " + idx);
        if (idx == (int) POWERSMODE.EXTRA_GOALS ||
            idx == (int) POWERSMODE.ENLARGE_GOAL)
        {
            if (!isPowerEnable)
                return;

            if ((idx == (int) POWERSMODE.ENLARGE_GOAL))
            {
                if (!shotActive)
                {
                    resizeOpponentGoal(isMaster,
                                       new Vector3(2.223f, 1.922f, 1f),
                                       new Vector3(8f, 4.6f, 14f),
                                       ENLARGE_OPPONENT_GOAL_TIME - lagDelay,
                                       false);
                }
                else
                {
                    rpc_powers_state[(int)POWERSMODE.ENLARGE_GOAL] = true;
                }
                return;
            } 

            if ((idx == (int) POWERSMODE.EXTRA_GOALS))               
            {
                if (!shotActive)
                {
                    twoExtraGoals(
                    isMaster,
                    ADD_SMALL_GOALS_TIME - lagDelay,
                    false);
                } else
                {
                    rpc_powers_state[(int)POWERSMODE.EXTRA_GOALS] = true;
                }
                return;
            }
        }


        if (idx == (int) POWERSMODE.GOAL_WALLS) 
        {
            if (!shotActive)
            {
                goalObstacles(
                    isMaster,
                    GOALS_OBSTACLES_TIME - lagDelay,
                    false);
                return;
            } else
            {
                rpc_powers_state[(int) POWERSMODE.GOAL_WALLS] = true;
            }
        } 
    }

    void Start()
    {
        RPC_confirmation = new bool[MAX_POWERS + 1];
        rpc_powers_state = new bool[MAX_POWERS + 1];
        rpc_shotPercent = new float[MAX_POWERS + 1];

        for (int i = 0; i < rpc_powers_state.Length; i++)
        {
            RPC_confirmation[i] = false;
            rpc_powers_state[i] = false;
            rpc_shotPercent[i] = 0f;
        }
        photonView = GetComponent<PhotonView>();

        if (Globals.powersStr.Equals("NO"))
        {
            isPowerEnable = false;
            return;
        }

        isPlayerSlowDown = new bool[2];
        initPowerTimes();

        cpuPowersUsed = new bool[MAX_POWERS + 1];
        playerPowerLock = new int[MAX_POWERS + 1];
        for (int i = 0; i < cpuPowersUsed.Length; i++)
            cpuPowersUsed[i] = false;

        for (int i = 0; i < playerPowerLock.Length; i++)       
            playerPowerLock[i] = 1;
        
        audioManager = FindObjectOfType<AudioManager>();


        playerMainScript = Globals.player1MainScript;
        if (PhotonNetwork.IsMasterClient)
        {
            isMaster = true;
        }
        else
        {
            isMaster = false;
        }

        if (Globals.PITCHTYPE.Equals("STREET"))
            return;

        wallUpLeft1 = GameObject.Find("wallUpLeft1");
        wallUpRight1 = GameObject.Find("wallUpRight1");
        wallUpLeft2 = GameObject.Find("wallUpLeft2");
        wallUpRight2 = GameObject.Find("wallUpRight2");

        if (Globals.stadiumNumber != 1)
        {
            wallDownLeft1 = GameObject.Find("wallDownLeft1");
            wallDownLeft2 = GameObject.Find("wallDownLeft2");
            wallDownRight1 = GameObject.Find("wallDownRight1");
            wallDownRight2 = GameObject.Find("wallDownRight2");

            wallDownLeftGround1 = GameObject.Find("wallDownLeftGround1");
            wallDownLeftGround2 = GameObject.Find("wallDownLeftGround2");
            wallDownRightGround1 = GameObject.Find("wallDownRightGround1");
            wallDownRightGround2 = GameObject.Find("wallDownRightGround2");
        }   
    }

    void FixedUpdate()
    {
        for (int i = 1; i <= MAX_POWERS; i++)
        {
            if (!rpc_powers_state[i])
                continue;

            if ((i == (int) POWERSMODE.GOAL_WALLS) &&
                (playerMainScript.getShotPercent() >= rpc_shotPercent[i])) {
                goalObstacles(
                    isMaster,
                    GOALS_OBSTACLES_TIME,
                    false);            }

            if ((i == (int) POWERSMODE.EXTRA_GOALS) &&
                (playerMainScript.peerPlayer.getShotPercent() >= rpc_shotPercent[i]))
            {
                twoExtraGoals(
                            isMaster,
                            ADD_SMALL_GOALS_TIME,
                            false);
            }

            if ((i == (int)POWERSMODE.ENLARGE_GOAL) &&
                (playerMainScript.peerPlayer.getShotPercent() >= rpc_shotPercent[i]))
            {
                resizeOpponentGoal(isMaster,
                                   new Vector3(2.223f, 1.922f, 1f),
                                   new Vector3(8f, 4.6f, 14f),
                                   ENLARGE_OPPONENT_GOAL_TIME,
                                   false);
            }

            rpc_powers_state[i] = false;
        }
    }

    void rpc_send(byte idx)
    {
        if ((idx == (int) POWERSMODE.GOAL_WALLS))
        {
            photonView.RPC("RPC_extraPower",
                           RpcTarget.Others,
                           playerMainScript.peerPlayer.getShotActive(),
                           playerMainScript.peerPlayer.getShotPercent(),
                           idx);
            StartCoroutine(
                RPC_extraPowerResend(playerMainScript.peerPlayer.getShotActive(),
                                     playerMainScript.peerPlayer.getShotPercent(),
                                     idx));
        }
        else
        {
            photonView.RPC("RPC_extraPower",
                            RpcTarget.Others,
                            playerMainScript.getShotActive(),
                            playerMainScript.getShotPercent(),
                            idx);
            StartCoroutine(
                RPC_extraPowerResend(playerMainScript.getShotActive(),
                                     playerMainScript.getShotPercent(),
                                     idx));
        }

        PhotonNetwork.SendAllOutgoingCommands();
    }
    IEnumerator RPC_extraPowerResend(bool shotActive,
                                     float shotPercent,
                                     byte idx)
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                photonView.RPC("RPC_extraPower",
                                RpcTarget.Others,
                                shotActive,
                                shotPercent,
                                idx);
                yield return null;

            }
        }

        RPC_confirmation[idx] = false;

    }

    [PunRPC]
    void RPC_PACKET_ACK(byte idx)
    {
        RPC_confirmation[idx] = true;
        //Debug.Log("DBG342344COL get ack confirm idx " + idx + " Time " + Time.time);
    }

    public void button1Action()
    {
        if (!isPowerEnable)
            return;

        if (Globals.PITCHTYPE.Equals("STREET"))
            cutGoalByHalf(!isMaster,
                          0,
                          new Vector3(1.32f, 1.32f / 1.5f, 1f),
                          new Vector3(4.7f, 3.1f / 1.5f, 14f));

        else
            twoExtraGoals(isMaster, ADD_SMALL_GOALS_TIME, true);
        rpc_send(1);        
    }

    public void button2Action()
    {
        if (!isPowerEnable)
            return;

        goalObstacles(!isMaster, GOALS_OBSTACLES_TIME, true);
        rpc_send(2);
    }

    public void button3Action()
    {
        if (!isPowerEnable)
            return;

        if (Globals.PITCHTYPE.Equals("STREET"))
        {
            badConditions(true,
                         2,
                         Vector3.zero,
                         Vector3.zero);
            return;
        }

        if (Globals.stadiumNumber != 1)
        {
            resizeOpponentGoal(isMaster,
                               new Vector3(2.223f, 1.922f, 1f),
                               new Vector3(8f, 4.6f, 14f),
                               ENLARGE_OPPONENT_GOAL_TIME,
                               true);
        }
        else
        {
            resizeOpponentGoal(isMaster,
                               new Vector3(3f, 2.3f, 5.34f),
                               new Vector3(8f, 4.6f, 14f),
                               ENLARGE_OPPONENT_GOAL_TIME,
                               true);
        }

        rpc_send(3);
    }

    public bool twoExtraGoals(bool isMaster, float delay, bool removePower)
    {
        if (!isPowerEnable)
            return false;

        if (Globals.stadiumNumber != 1)
        {
            //if (!isCpu)
            //{
                if (removePower && 
                    (playerPowerLock[1] < 1 ||
                     mainPlayerLock))
                    return false;
                  
            if (isMaster)
            {
                if (removePower)
                {
                    wallUpRight1.transform.position =
                        new Vector3(10.3f, 0.65f, 14.075f);
                    wallUpLeft2.transform.position =
                        new Vector3(-10.3f, 0.65f, 14.075f);
                    wallUpRight1.transform.localScale =
                        wallUpLeft2.transform.localScale =
                        new Vector3(4.4f, 1.3f, 0.15f);
                }
                else
                {
                    wallDownRight1.transform.localScale =
                    wallDownLeft2.transform.localScale =
                    wallDownRightGround1.transform.localScale =
                    wallDownLeftGround2.transform.localScale =
                        new Vector3(4.4f, 1.3f, 0.15f);

                    wallDownRightGround1.transform.position =
                       new Vector3(10.3f, 0.07f, -14.7575f);
                    wallDownRight1.transform.position =
                        new Vector3(10.3f, 0.65f, -14.075f);

                    wallDownLeftGround2.transform.position =
                        new Vector3(-10.3f, 0.07f, -14.7575f);
                    wallDownLeft2.transform.position =
                        new Vector3(-10.3f, 0.65f, -14.075f);
                }
            } 
            else
            //because it's rotated 180 degrees
            {
                if (removePower)
                {
                    wallUpRight1.transform.position =
                        new Vector3(10.3f, 0.65f, -14.075f);
                    wallUpLeft2.transform.position =
                        new Vector3(-10.3f, 0.65f, -14.075f);
                    wallUpRight1.transform.localScale =
                        wallUpLeft2.transform.localScale =
                        new Vector3(4.4f, 1.3f, 0.15f);
                }
                else
                {

                    wallDownRight1.transform.localScale =
                    wallDownLeft2.transform.localScale =
                    wallDownRightGround1.transform.localScale =
                    wallDownLeftGround2.transform.localScale =
                        new Vector3(4.4f, 1.3f, 0.15f);

                    wallDownRightGround1.transform.position =
                       new Vector3(10.3f, 0.07f, 14.7575f);
                    wallDownRight1.transform.position =
                        new Vector3(10.3f, 0.65f, 14.075f);

                    wallDownLeftGround2.transform.position =
                        new Vector3(-10.3f, 0.07f, 14.7575f);
                    wallDownLeft2.transform.position =
                        new Vector3(-10.3f, 0.65f, 14.075f);
                }
            }
            
                //print("wallUpRight1 pos " + wallUpRight1.transform.position);
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");

                if (isMaster)
                {
                    if (removePower)
                    {
                        smallGoal[0].SetActive(true);
                        smallGoal[1].SetActive(true);
                        playerMainScript.peerPlayer.setExtraGoals(true, new Vector3(13f, 3.5f, 14f));
                    } else
                    {
                        smallGoal[2].SetActive(true);
                        smallGoal[3].SetActive(true);
                        playerMainScript.setExtraGoals(true);
                    }
                }
                else
                {
                    if (removePower) {
                        smallGoal[2].SetActive(true);
                        smallGoal[3].SetActive(true);
                        playerMainScript.setExtraGoals(true);                    
                    }
                    else
                    {
                        smallGoal[0].SetActive(true);
                        smallGoal[1].SetActive(true);
                        playerMainScript.peerPlayer.setExtraGoals(true, new Vector3(13f, 3.5f, 14f));
                    }
                }

                if (removePower)
                {
                    playerPowerLock[1]--;
                    mainPlayerLock = true;
                }

                //goalUpStandardColliders.SetActive(false);
                //goalUpExtraGoalsColliders.SetActive(true);
                
                StartCoroutine(removeSmallGoals(delay, isMaster, removePower));
                ///if (removePower)
                //    StartCoroutine(fillAmountImg(delay, powerButtonImg[0], powerButtons[0]));
            //*}
            /* else
             {
                 if (cpuPowersUsed[1] ||
                     cpuPowerLock)
                     return false;

                 wallDownRight1.transform.localScale =
                 wallDownLeft2.transform.localScale =
                 wallDownRightGround1.transform.localScale =
                 wallDownLeftGround2.transform.localScale =
                        new Vector3(4.4f, 1.3f, 0.15f);

                 wallDownRight1.transform.position =
                     new Vector3(10.3f, 0.65f, -14.075f);
                 wallDownLeft2.transform.position =
                     new Vector3(-10.3f, 0.65f, -14.075f);

                 wallDownRightGround1.transform.position =
                    new Vector3(10.3f, 0.07f, -14.7575f);


                 wallDownLeftGround2.transform.position =
                     new Vector3(-10.3f, 0.07f, -14.7575f);

                 cpuPowerLock = true;
                 audioManager.Play("extraPowerEnable");
                 audioManager.Commentator_extraPower("twoextragoals");

                 cpuPowersUsed[1] = true;
                 smallGoal[2].SetActive(true);
                 smallGoal[3].SetActive(true);
                 playerMainScript.setExtraGoals(true);

                 StartCoroutine(removeSmallGoals(delay, isCpu));*/
            ///}

            if (removePower)
                StartCoroutine(fillAmountImg(delay, powerButtonImg[0], powerButtons[0]));
        }
        else
        //sport hall
        {
            if (!isMaster)
            {
                if (playerPowerLock[1] < 1 ||
                    mainPlayerLock)
                    return false;

                //print("playerPowerLock");

                wallUpRight1.transform.localScale =
                wallUpLeft2.transform.localScale =
                       new Vector3(4.06f, 1.3f, 0.15f);

                wallUpRight1.transform.position =
                    new Vector3(10.728f, 0.65f, 14f);
                wallUpLeft2.transform.position =
                    new Vector3(-10.728f, 0.65f, 14f);

                //wallUpLeft1.transform.position =
                new Vector3(-16.381f, 0.65f, 14.075f);

                //wallUpRight2.transform.position =
                //      new Vector3(15.835f, 0.65f, 14.075f);

                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");

                playerMainScript.peerPlayer.setExtraGoals(true, new Vector3(13f, 3.5f, 14f));
                smallGoal[0].SetActive(true);
                smallGoal[1].SetActive(true);
                playerPowerLock[1]--;
                mainPlayerLock = true;

                goalUpStandardColliders.SetActive(false);
                goalUpExtraGoalsColliders.SetActive(true);

                StartCoroutine(removeSmallGoals(delay, isMaster, removePower));
                StartCoroutine(fillAmountImg(delay, powerButtonImg[0], powerButtons[0]));
            }
            else
            {
                if (cpuPowersUsed[1] ||
                    cpuPowerLock)
                    return false;

                cpuPowerLock = true;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");

                cpuPowersUsed[1] = true;
                smallGoal[2].SetActive(true);
                smallGoal[3].SetActive(true);
                goalDownStandardColliders.SetActive(false);
                goalDownExtraGoalsColliders.SetActive(true);
                playerMainScript.setExtraGoals(true);
                StartCoroutine(removeSmallGoals(delay, isMaster, removePower));
            }

            return true;
        }

        return true;
    }

    public bool goalObstacles(bool isCpu, float delay, bool removePower)
    {
        if (!isPowerEnable)
            return false;

        if (!isCpu)
        {
            if (playerPowerLock[2] < 1 ||
                mainPlayerLock)
                return false;

            goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("wall");

            playerPowerLock[2]--;
            mainPlayerLock = true;
            StartCoroutine(deactiveGameObject(delay, goalDownObstacles[0], isCpu));
        }
        else
        {
            if (cpuPowersUsed[2] ||
                cpuPowerLock)
                return false;

            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[2] = true;
            goalUpObstacles[0].SetActive(true);

            StartCoroutine(deactiveGameObject(delay, goalUpObstacles[0], isCpu));
        }

        if (removePower)
            StartCoroutine(fillAmountImg(delay, powerButtonImg[1], powerButtons[1]));

        return true;
    }

    public bool freezePlayer(bool isCpu)
    {
        if (!isPowerEnable)
            return false;

        if (!isCpu)
        {
            if (playerPowerLock[2] < 1 ||
                mainPlayerLock)
                return false;

            playerMainScript.peerPlayer.playerFreeze(true);
            audioManager.Play("extraPowerEnable");
            playerPowerLock[2]--;
            mainPlayerLock = true;
            StartCoroutine(unFreezePlayer(FREEZE_PLAYER_TIME, false));
            StartCoroutine(fillAmountImg(FREEZE_PLAYER_TIME, powerButtonImg[1], powerButtons[1]));
        }
        else
        {
            if (cpuPowersUsed[2] ||
                cpuPowerLock)
                return false;

            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            cpuPowersUsed[2] = true;

            playerMainScript.playerFreeze(true);
            StartCoroutine(unFreezePlayer(FREEZE_PLAYER_TIME, true));
        }

        return true;
    }
 
    public bool resizeOpponentGoal(bool isMaster, 
                                   Vector3 localScale, 
                                   Vector3 realScale,
                                   float delay,
                                   bool removePower)
    {
        if (!isPowerEnable)
            return false;

        if (Globals.stadiumNumber != 1)
        {
            //if (!isCpu || true)
            //{
                if (removePower &&
                    (playerPowerLock[3] < 1 ||
                    mainPlayerLock))
                    return false;

                if (isMaster)
                {
                    if (!removePower)
                    {
                        wallDownLeft2.transform.position =
                             new Vector3(-13.16f, 0.65f, -14.075f);
                        wallDownRight2.transform.position =
                            new Vector3(13.16f, 0.65f, -14.075f);
                        wallDownLeftGround2.transform.position =
                            new Vector3(-13.16f, 0.07f, -14.075f);
                        wallDownRightGround2.transform.position =
                           new Vector3(13.16f, 0.07f, -14.075f);
                        wallDownLeft2.transform.localScale =
                        wallDownRight2.transform.localScale =
                            new Vector3(13.95f, 1.3f, 0.15f);
                        wallDownLeft1.SetActive(false);
                        wallDownLeftGround1.SetActive(false);
                        wallDownRight1.SetActive(false);
                        wallDownRightGround1.SetActive(false);
                } else
                    {
                        wallUpLeft1.SetActive(false);
                        wallUpRight1.SetActive(false);

                        wallUpLeft2.transform.position =
                            new Vector3(-13.16f, 0.65f, 14.075f);
                        wallUpRight2.transform.position =
                            new Vector3(13.16f, 0.65f, 14.075f);
                        wallUpLeft2.transform.localScale =
                        wallUpRight2.transform.localScale =
                            new Vector3(13.95f, 1.3f, 0.15f);
                        //wallUpLeft1.SetActive(false);
                        //wallUpRight1.SetActive(false);
                    }                    
                }
                else
                {
                    if (!removePower)
                    {
                        wallDownLeft2.transform.position =
                            new Vector3(-13.16f, 0.65f, 14.075f);
                        wallDownRight2.transform.position =
                            new Vector3(13.16f, 0.65f, 14.075f);
                        wallDownLeftGround2.transform.position =
                        new Vector3(-13.16f, 0.07f, 14.075f);
                        wallDownRightGround2.transform.position =
                            new Vector3(13.16f, 0.07f, 14.075f);

                        wallDownLeft2.transform.localScale =
                        wallDownRight2.transform.localScale =
                            new Vector3(13.95f, 1.3f, 0.15f);
                        //print("wallDown changed removePower " + removePower);
                        wallDownLeft1.SetActive(false);
                        wallDownLeftGround1.SetActive(false);

                        wallDownRight1.SetActive(false);
                        wallDownRightGround1.SetActive(false);
                } else {
                        wallUpLeft1.SetActive(false);
                        wallUpRight1.SetActive(false);

                        wallUpLeft2.transform.position =
                            new Vector3(-13.16f, 0.65f, -14.075f);
                        wallUpRight2.transform.position =
                            new Vector3(13.16f, 0.65f, -14.075f);
                        wallUpLeft2.transform.localScale =
                        wallUpRight2.transform.localScale =
                            new Vector3(13.95f, 1.3f, 0.15f);

                        //wallUpLeft1.SetActive(false);
                        //wallUpRight1.SetActive(false);
                        playerMainScript.setGoalSizeCpu(realScale);
                        playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
                    }
                }

            //if (isMaster)
            //{
            //    playerMainScript.setGoalSizeCpu(realScale);
            //    playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
            //}
            //else
            //{
            //    playerMainScript.setGoalSize(realScale);
            //    playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
            //}


                if (removePower)
                {
                    playerPowerLock[3]--;
                    mainPlayerLock = true;
                }
                //isCpuGoalEnlarge = true;

                if (isMaster)
                {
                    if (removePower)
                    {
                        goalUp.SetActive(false);
                        goalUpBigger.SetActive(true);
                        isCpuGoalEnlarge = true;
                        playerMainScript.setGoalSizeCpu(realScale);
                        playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
                    } else
                    {
                        goalDown.SetActive(false);
                        goalDownBigger.SetActive(true);
                        isPlayerGoalEnlarge = true;
                        playerMainScript.setGoalSize(realScale);
                        playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
                    }
                }
                else
                {
                    if (removePower)
                    {
                        goalDown.SetActive(false);
                        goalDownBigger.SetActive(true);
                        isPlayerGoalEnlarge = true;
                        playerMainScript.setGoalSize(realScale);
                        playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
                    } else
                    {
                        goalUp.SetActive(false);
                        goalUpBigger.SetActive(true);
                        isCpuGoalEnlarge = true;
                        playerMainScript.setGoalSizeCpu(realScale);
                        playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
                    }
                }

                if (isMaster)
                {
                    if (removePower)
                    {
                        goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                    4.791f,
                                                                    goalUpFlare[0].transform.position.z);
                        goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                    4.791f,
                                                                    goalUpFlare[1].transform.position.z);
                    } else
                    {
                        goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                 4.791f,
                                                                 goalDownFlare[0].transform.position.z);
                        goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                      4.791f,
                                                                      goalDownFlare[1].transform.position.z);
                    }
                } else
                {
                    if (!removePower)
                    {
                        goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                4.791f,
                                                                goalUpFlare[0].transform.position.z);
                        goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                4.791f,
                                                                goalUpFlare[1].transform.position.z);
                    }
                    else
                    {
                        goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                             4.791f,
                                                             goalDownFlare[0].transform.position.z);
                        goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  4.791f,
                                                                  goalDownFlare[1].transform.position.z);
                    }
            }

                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("enlargeGoal");

                if (isMaster)
                {
                    if (removePower)
                    {
                        StartCoroutine(
                            rollbackEnlargeGoal(delay,
                                        goalUp,
                                        new Vector3(1.32f, 1.32f, 1f),
                                        new Vector3(4.7f, 3.1f, 14.0f),
                                        isMaster,
                                        removePower));
                    } else
                    {
                        StartCoroutine(
                             rollbackEnlargeGoal(delay,
                                             goalDown,
                                             new Vector3(1.32f, 1.32f, 1f),
                                             new Vector3(4.7f, 3.1f, 14.0f),
                                             isMaster,
                                             removePower));
                    }
                }
                else
                {
                    if (removePower)
                    {
                        StartCoroutine(
                            rollbackEnlargeGoal(delay,
                                    goalDown,
                                    new Vector3(1.32f, 1.32f, 1f),
                                    new Vector3(4.7f, 3.1f, 14.0f),
                                    isMaster,
                                    removePower));
                    }
                    else
                    {
                        StartCoroutine(
                             rollbackEnlargeGoal(delay,
                                         goalUp,
                                         new Vector3(1.32f, 1.32f, 1f),
                                         new Vector3(4.7f, 3.1f, 14.0f),
                                         isMaster,
                                         removePower));
                    }
            }

                //StartCoroutine(fillAmountImg(delay, powerButtonImg[2], powerButtons[2]));
                if (removePower)
                    StartCoroutine(fillAmountImg(delay, powerButtonImg[2], powerButtons[2]));
            //}
            /*   else
               {
                   if (cpuPowersUsed[3] ||
                       cpuPowerLock)
                       return false;

                   wallDownLeft1.SetActive(false);
                   wallDownLeftGround1.SetActive(false);
                   wallDownRight1.SetActive(false);
                   wallDownRightGround1.SetActive(false);

                   wallDownLeft2.transform.position =
                      new Vector3(-13.16f, 0.65f, -14.075f);
                   wallDownRight2.transform.position =
                      new Vector3(13.16f, 0.65f, -14.075f);

                   wallDownLeftGround2.transform.position =
                      new Vector3(-13.16f, 0.07f, -14.075f);
                   wallDownRightGround2.transform.position =
                      new Vector3(13.16f, 0.07f, -14.075f);

                   wallDownLeft2.transform.localScale =
                   wallDownRight2.transform.localScale =
                       new Vector3(13.95f, 1.3f, 0.15f);

                   wallDownLeftGround2.transform.localScale =
                   wallDownRightGround2.transform.localScale =
                       new Vector3(13.95f, 1.3f, 0.15f);

                   cpuPowerLock = true;
                   cpuPowersUsed[3] = true;
                   isPlayerGoalEnlarge = true;
                   audioManager.Play("extraPowerEnable");
                   audioManager.Commentator_extraPower("enlargeGoal");


                   goalDown.SetActive(false);
                   goalDownBigger.SetActive(true);


                   playerMainScript.setGoalSize(realScale);
                   playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());

                   goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                     4.791f,
                                                                     goalDownFlare[0].transform.position.z);
                   goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                     4.791f,
                                                                     goalDownFlare[1].transform.position.z);


                   StartCoroutine(
                     rollbackEnlargeGoal(delay,
                                         goalDown,
                                         new Vector3(1.32f, 1.32f, 1f),
                                         new Vector3(4.7f, 3.1f, 14.0f),
                                         true));
               }

               if (removePower)
                   StartCoroutine(fillAmountImg(delay, powerButtonImg[2], powerButtons[2]));
           }
           else
           {
               if (!isMaster)
               {
                   if (playerPowerLock[3] < 1 ||
                       mainPlayerLock)
                       return false;

                   wallUpLeft1.SetActive(false);
                   wallUpRight1.SetActive(false);

                   wallUpLeft2.transform.position =
                       new Vector3(-14.22f, 0.65f, 14.075f);

                   wallUpRight2.transform.position =
                      new Vector3(14.22f, 0.65f, 14.075f);

                   wallUpLeft2.transform.localScale =
                   wallUpRight2.transform.localScale =
                       new Vector3(11.5f, 1.3f, 0.15f);

                   goalUp.transform.localScale = localScale;
                   playerMainScript.setGoalSizeCpu(realScale);
                   playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

                   goalUpStandardColliders.SetActive(false);
                   goalUpEnlargeColliders.SetActive(true);
                   playerPowerLock[3]--;
                   mainPlayerLock = true;
                   isCpuGoalEnlarge = true;

                   goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                   4.791f,
                                                                   goalUpFlare[0].transform.position.z);
                   goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                   4.791f,
                                                                   goalUpFlare[1].transform.position.z);

                   //goalUpCloth.enabled = false;
                   audioManager.Play("extraPowerEnable");
                   audioManager.Commentator_extraPower("enlargeGoal");

                   StartCoroutine(
                       rollbackEnlargeGoal(delay,
                                           goalUp,
                                           new Vector3(3f, 1.758f, 3.498f),
                                           new Vector3(5.25f, 3.5f, 14.0f),
                                           false));
               }
               else
               {
                   if (cpuPowersUsed[3] ||
                       cpuPowerLock)
                       return false;

                   //print("EXTRAGOALS enlargegoals ");

                   cpuPowerLock = true;
                   cpuPowersUsed[3] = true;
                   isPlayerGoalEnlarge = true;
                   audioManager.Play("extraPowerEnable");
                   audioManager.Commentator_extraPower("enlargeGoal");

                   ////goalDownCloth.enabled = false;
                   // goalDown.transform.localScale = localScale;
                   //playerMainScript.setGoalSize(realScale);

                   goalDown.SetActive(false);
                   goalDownBigger.SetActive(true);
                   goalDownStandardColliders.SetActive(false);
                   goalDownEnlargeColliders.SetActive(true);

                   goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                     3.64f,
                                                                     goalDownFlare[0].transform.position.z);
                   goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                     3.64f,
                                                                     goalDownFlare[1].transform.position.z);

                   StartCoroutine(
                     rollbackEnlargeGoal(delay,
                                         goalDown,
                                         new Vector3(3f, 1.558f, 3.13412f),
                                         new Vector3(4.7f, 3.1f, 14.0f),
                                         true));
               }*/
        }

        return true;
    }

    IEnumerator deactiveGameObject(float delay,
                                   GameObject gameObj,
                                   bool isCpu)                                    
    {
        yield return new WaitForSeconds(delay);

        if (isCpu)
        {
            for (int i = 0; i < 10; i++)
            {
                if ((playerMainScript.isShotActive() &&
                     (playerMainScript.getBallPos().z > 13.2f)) ||
                     playerMainScript.ball[1].getBallGoalCollisionStatus())
                {
                    if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                        yield return new WaitForSeconds(0.35f);
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
        } else
        {
            for (int i = 0; i < 10; i++)
            {
                if ((playerMainScript.peerPlayer.getShotActive() &&
                    (playerMainScript.getBallPos().z < -13.2f)) ||
                    playerMainScript.ball[1].getBallGoalCollisionStatus())
                {
                    if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                        yield return new WaitForSeconds(0.35f);
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
        }

        gameObj.SetActive(false);
        if (!isCpu) 
            mainPlayerLock = false;
        else
            cpuPowerLock = false;
    }

    IEnumerator rollbackEnlargeGoal(
                                    float delay,
                                    GameObject goal, 
                                    Vector3 localScale, 
                                    Vector3 realScale,
                                    bool isMaster,
                                    bool removePower)
    {
        yield return new WaitForSeconds(delay);
    
        //if (!isCpu)
        
        //{
            for (int i = 0; i < 10; i++)
            {
                if (isMaster)
                {
                    if ((playerMainScript.isShotActive() &&
                        (playerMainScript.getBallPos().z > 13.2f)) ||
                         playerMainScript.ball[1].getBallGoalCollisionStatus())
                    {
                        if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                            yield return new WaitForSeconds(0.35f);
                        else
                            yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {

                    if ((playerMainScript.peerPlayer.getShotActive() &&
                        (playerMainScript.getBallPos().z < -13.2f)) ||
                         playerMainScript.ball[1].getBallGoalCollisionStatus())
                    {
                        if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                            yield return new WaitForSeconds(0.35f);
                        else
                            yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            recoverAdsOriginalSize(isMaster, removePower);
            if (Globals.stadiumNumber == 1)
            {
                goalUpStandardColliders.SetActive(true);
                goalUpEnlargeColliders.SetActive(false);
            }

            if (isMaster) {
                if (removePower)
                {
                    goalUp.SetActive(true);
                    goalUpBigger.SetActive(false);
                    isCpuGoalEnlarge = false;
                    wallUpLeft1.SetActive(true);
                    wallUpRight1.SetActive(true);
                } else {
                    goalDown.SetActive(true);
                    goalDownBigger.SetActive(false);
                    isPlayerGoalEnlarge = false;    
                    wallDownLeft1.SetActive(true);
                    wallDownRight1.SetActive(true);
                    wallDownLeftGround1.SetActive(true);
                    wallDownRightGround1.SetActive(true);
                    ///print("execute goalDown change");
                }
            }            
            else            
            {
                if (!removePower)
                {
                    goalUp.SetActive(true);
                    goalUpBigger.SetActive(false);
                    isCpuGoalEnlarge = false; 
                    wallDownLeft1.SetActive(true);
                    wallDownRight1.SetActive(true);
                    wallDownLeftGround1.SetActive(true);
                    wallDownRightGround1.SetActive(true);
                 
                }
                else
                {
                    goalDown.SetActive(true);
                    goalDownBigger.SetActive(false);
                    isPlayerGoalEnlarge = false;
                    //print("goalDown executed ");
                    wallUpLeft1.SetActive(true);
                    wallUpRight1.SetActive(true);
                }
            }

            if (isMaster)
            {
                if (removePower)
                {
                    goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                3.237f,
                                                                    goalUpFlare[0].transform.position.z);
                    goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                3.237f,
                                                                goalUpFlare[1].transform.position.z);
                    playerMainScript.setGoalSizeCpu(realScale);
                    playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
                } else
                {
                    goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[0].transform.position.z);
                    goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[1].transform.position.z);
                    playerMainScript.setGoalSize(realScale);
                    playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
                }
            } else
                {
                    if (removePower) {
                        goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[0].transform.position.z);
                    goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[1].transform.position.z);
                    playerMainScript.setGoalSize(realScale);
                    playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
                    }
                    else
                    {
                        goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                          3.237f,
                                                          goalUpFlare[0].transform.position.z);
                        goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                            3.237f,
                                                            goalUpFlare[1].transform.position.z);
                        playerMainScript.setGoalSizeCpu(realScale);
                        playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
                    }
        }

        /*if (isMaster) {
            playerMainScript.setGoalSizeCpu(realScale);
            playerMainScript.peerPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());
        }
        else
        {
            playerMainScript.setGoalSize(realScale);
            playerMainScript.recalculateCornerPoints(playerMainScript.getGoalSizePlr1());
        }*/

            if (removePower)
            {
                mainPlayerLock = false;
            }
            isCpuGoalEnlarge = false;

        //goal.transform.localScale = localScale;
     
            //goalUpCloth.enabled = true;
            //}
            /*else
            {
                for (int i = 0; i < 10; i++)
                {
                    if ((playerMainScript.peerPlayer.getShotActive() &&
                        (playerMainScript.getBallPos().z < -13.2f)) ||
                         playerMainScript.ball[1].getBallGoalCollisionStatus())
                    {
                        if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                            yield return new WaitForSeconds(0.35f);
                        else
                            yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }

                goalDown.SetActive(true);
                goalDownBigger.SetActive(false);

                goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[0].transform.position.z);
                goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  3.237f,
                                                                  goalDownFlare[1].transform.position.z);


                recoverAdsOriginalSize(isCpu);

                if (Globals.stadiumNumber == 1)
                {
                    goalDownStandardColliders.SetActive(true);
                    goalDownEnlargeColliders.SetActive(false);
                }

                if (Globals.stadiumNumber != 1)
                {
                    wallDownLeft1.SetActive(true);
                    wallDownRight1.SetActive(true);
                    wallDownLeftGround1.SetActive(true);
                    wallDownRightGround1.SetActive(true);
                }
                //goalDownCloth.enabled = true;
                cpuPowerLock = false;
                isPlayerGoalEnlarge = false;
            }*/
        }

    private void recoverAdsOriginalSize(bool isMaster, bool removePower)
    {
        if (Globals.stadiumNumber == 1)
        {
            wallUpLeft1.transform.position =
                new Vector3(-16.381f, 0.65f, 14.075f);
            wallUpLeft2.transform.position =
                new Vector3(-9.146f, 0.65f, 14.075f);

            wallUpRight1.transform.position =
                new Vector3(9.146f, 0.65f, 14.075f);
            wallUpRight2.transform.position =
                new Vector3(16.381f, 0.65f, 14.075f);

            wallUpLeft1.transform.position =
                new Vector3(-16.381f, 0.65f, 14.075f);

            wallUpLeft1.transform.localScale =
            wallUpRight1.transform.localScale =
            wallUpLeft2.transform.localScale =
            wallUpRight2.transform.localScale =
                new Vector3(7.235f, 1.3f, 0.15f);

            goalUpStandardColliders.SetActive(true);
            goalUpExtraGoalsColliders.SetActive(false);
            return;
        }


        if (isMaster)
        {
            if (!removePower)
            {
                wallDownLeft1.transform.position =
                    new Vector3(-16.325f, 0.65f, -14.075f);
                wallDownLeftGround1.transform.position =
                    new Vector3(-16.325f, 0.07f, -14.7575f);
                wallDownLeft2.transform.position =
                    new Vector3(-8.7f, 0.65f, -14.075f);
                wallDownLeftGround2.transform.position =
                    new Vector3(-8.7f, 0.07f, -14.7575f);
                wallDownRight1.transform.position =
                    new Vector3(8.7f, 0.65f, -14.075f);
                wallDownRightGround1.transform.position =
                    new Vector3(8.7f, 0.07f, -14.7575f);
                wallDownRight2.transform.position =
                    new Vector3(16.325f, 0.65f, -14.075f);
                wallDownRightGround2.transform.position =
                    new Vector3(16.325f, 0.07f, -14.7575f);
                wallDownLeft1.transform.localScale =
                wallDownRight1.transform.localScale =
                wallDownLeft2.transform.localScale =
                wallDownRight2.transform.localScale =
                    new Vector3(7.65f, 1.3f, 0.15f);
                wallDownLeftGround1.transform.localScale =
                wallDownRightGround1.transform.localScale =
                wallDownLeftGround2.transform.localScale =
                wallDownRightGround2.transform.localScale =
                    new Vector3(7.65f, 1.3f, 0.15f);
            }
            else
            {

                wallUpLeft1.transform.position =
                    new Vector3(-16.325f, 0.65f, 14.075f);
                wallUpLeft2.transform.position =
                    new Vector3(-8.7f, 0.65f, 14.075f);
                wallUpRight1.transform.position =
                    new Vector3(8.7f, 0.65f, 14.075f);
                wallUpRight2.transform.position =
                    new Vector3(16.325f, 0.65f, 14.075f);
                wallUpLeft1.transform.localScale =
                wallUpRight1.transform.localScale =
                wallUpLeft2.transform.localScale =
                wallUpRight2.transform.localScale =
                   new Vector3(7.65f, 1.3f, 0.15f);
            }
        }
        else
        {
            if (!removePower)
            {
                wallDownLeft1.transform.position =
                    new Vector3(-16.325f, 0.65f, 14.075f);
                wallDownLeftGround1.transform.position =
                    new Vector3(-16.325f, 0.07f, 14.7575f);
                wallDownLeft2.transform.position =
                    new Vector3(-8.7f, 0.65f, 14.075f);
                wallDownLeftGround2.transform.position =
                    new Vector3(-8.7f, 0.07f, 14.7575f);
                wallDownRight1.transform.position =
                    new Vector3(8.7f, 0.65f, 14.075f);
                wallDownRightGround1.transform.position =
                    new Vector3(8.7f, 0.07f, 14.7575f);
                wallDownRight2.transform.position =
                    new Vector3(16.325f, 0.65f, 14.075f);
                wallDownRightGround2.transform.position =
                    new Vector3(16.325f, 0.07f, 14.7575f);
                wallDownLeft1.transform.localScale =
                wallDownRight1.transform.localScale =
                wallDownLeft2.transform.localScale =
                wallDownRight2.transform.localScale =
                    new Vector3(7.65f, 1.3f, 0.15f);
                wallDownLeftGround1.transform.localScale =
                wallDownRightGround1.transform.localScale = 
                wallDownLeftGround2.transform.localScale =
                wallDownRightGround2.transform.localScale =
                    new Vector3(7.65f, 1.3f, 0.15f);
            }
            else
            {
                wallUpLeft1.transform.position =
                new Vector3(-16.325f, 0.65f, -14.075f);
                wallUpLeft2.transform.position =
                new Vector3(-8.7f, 0.65f, -14.075f);
                wallUpRight1.transform.position =
                new Vector3(8.7f, 0.65f, -14.075f);
                wallUpRight2.transform.position =
                new Vector3(16.325f, 0.65f, -14.075f);
                wallUpLeft1.transform.localScale =
                wallUpRight1.transform.localScale =
                wallUpLeft2.transform.localScale =
                wallUpRight2.transform.localScale =
                new Vector3(7.65f, 1.3f, 0.15f);
            }
        }
    
       

            //print("execwalluP " + wallUpRight2.transform.position);
            //goalUpStandardColliders.SetActive(true);
            //goalUpExtraGoalsColliders.SetActive(false);
        //}
        /*else
        {
            wallDownLeft1.transform.position =
                new Vector3(-16.325f, 0.65f, -14.075f);
            wallDownLeftGround1.transform.position =
                new Vector3(-16.325f, 0.07f, -14.7575f);

            wallDownLeft2.transform.position =
                new Vector3(-8.7f, 0.65f, -14.075f);
            wallDownLeftGround2.transform.position =
                new Vector3(-8.7f, 0.07f, -14.7575f);

            wallDownRight1.transform.position =
                new Vector3(8.7f, 0.65f, -14.075f);
            wallDownRightGround1.transform.position =
                new Vector3(8.7f, 0.07f, -14.7575f);

            wallDownRight2.transform.position =
                new Vector3(16.325f, 0.65f, -14.075f);
            wallDownRightGround2.transform.position =
                new Vector3(16.325f, 0.07f, -14.7575f);

            wallDownLeft1.transform.localScale =
            wallDownRight1.transform.localScale =
            wallDownLeft2.transform.localScale =
            wallDownRight2.transform.localScale =
                new Vector3(7.65f, 1.3f, 0.15f);

            wallDownLeftGround1.transform.localScale =
            wallDownRightGround1.transform.localScale =
            wallDownLeftGround2.transform.localScale =
            wallDownRightGround2.transform.localScale =
                new Vector3(7.65f, 1.3f, 0.15f);
        }*/
  
    }


    IEnumerator removeSmallGoals(float delay, bool isMaster, bool removePower)
    {
        yield return new WaitForSeconds(delay);
               
            for (int i = 0; i < 10; i++)
            {
                if (!isMaster)
                {
                    if ((playerMainScript.isShotActive() &&
                        (playerMainScript.getBallPos().z > 13.2f)) ||
                        playerMainScript.ball[1].getBallGoalCollisionStatus())
                    {
                        if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                            yield return new WaitForSeconds(0.35f);
                        else
                            yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if ((playerMainScript.peerPlayer.getShotActive() &&
                            (playerMainScript.getBallPos().z < -13.2f)) ||
                            playerMainScript.ball[1].getBallGoalCollisionStatus())
                    {
                        if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                            yield return new WaitForSeconds(0.35f);
                        else
                            yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }
            }
 
            if (isMaster) {           
                if (removePower)
                {
                    smallGoal[0].SetActive(false);
                    smallGoal[1].SetActive(false);
                    playerMainScript.peerPlayer.setExtraGoals(false, playerMainScript.getGoalSizePlr2());
                }
                else
                {
                    smallGoal[2].SetActive(false);
                    smallGoal[3].SetActive(false);
                    playerMainScript.setExtraGoals(false);
                }
            }
            else
            {
                if (removePower)
                {
                    smallGoal[2].SetActive(false);
                    smallGoal[3].SetActive(false);
                    playerMainScript.setExtraGoals(false);
                } else
                {
                    smallGoal[0].SetActive(false);
                    smallGoal[1].SetActive(false);
                    playerMainScript.peerPlayer.setExtraGoals(false, playerMainScript.getGoalSizePlr2());
                }
            }
            
            //smallGoal[0].SetActive(false);
            //smallGoal[1].SetActive(false);
            recoverAdsOriginalSize(isMaster, removePower);
        //if (isMaster)
        //    playerMainScript.peerPlayer.setExtraGoals(false, playerMainScript.getGoalSizePlr2());
        //else
        //    playerMainScript.setExtraGoals(false);
            if (removePower)
            {
                mainPlayerLock = false;
            }
        /*}
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if ((playerMainScript.peerPlayer.getShotActive() &&
                    (playerMainScript.getBallPos().z < -13.2f)) ||
                    playerMainScript.ball[1].getBallGoalCollisionStatus())
                {
                    if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                        yield return new WaitForSeconds(0.35f);
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }

            smallGoal[2].SetActive(false);
            smallGoal[3].SetActive(false);

            recoverAdsOriginalSize(isCpu);

            if (Globals.stadiumNumber == 1)
            {
                goalDownStandardColliders.SetActive(true);
                goalDownExtraGoalsColliders.SetActive(false);
            }

            cpuPowerLock = false;
            playerMainScript.setExtraGoals(false);

            //playerMainScript.cpuPlayer.setExtraGoals(false, playerMainScript.getGoalSizePlr1());
        }*/
    }

    IEnumerator unFreezePlayer(float delay, bool isCpu)
    {

        yield return new WaitForSeconds(delay);
        if (!isCpu)
        {
            playerMainScript.peerPlayer.playerFreeze(false);
            mainPlayerLock = false;
        }
        else
        {
            playerMainScript.playerFreeze(false);
            cpuPowerLock = false;
        }
    }

    IEnumerator fillAmountImg(float delay, Image img, GameObject gObject)
    {
        float startTime = Time.time;

        while (true)
        {
            float diffTime = Time.time - startTime;
            img.fillAmount = Mathf.Max(0f, 1.0f - Mathf.InverseLerp(0f, delay, diffTime));
            yield return new WaitForSeconds(0.05f);            

            if (diffTime >= delay)
            {
                break;
            }
        }

        gObject.SetActive(false);
    }

    private bool stopBallVelocity()
    {
        if (!playerMainScript.peerPlayer.getShotActive())
            return false;

        playerMainScript.peerPlayer.setShotActive(false);
        playerMainScript.setGkHelperImageVal(false);
        playerMainScript.setBallVelocity(Vector3.zero, Vector3.zero);
        audioManager.Play("extraPowerEnable");
        return true;
        //StartCoroutine(unFreezePlayer(FREEZE_PLAYER_TIME));
    }

    public bool isGoalDownEnlarge()
    {
        return isPlayerGoalEnlarge;
    }

    public bool isGoalUpEnlarge()
    {
        return isCpuGoalEnlarge;
    }

    public bool cutGoalByHalf(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable)
            return false;

        Vector3 localScale = arg1;
        Vector3 realScale = arg2;

        if (Globals.stadiumNumber != 1)
        {
            if (isCpu)
            {
                if (cpuPowersUsed[(int) POWER.CUT_GOAL_BY_HALF] ||
                    cpuPowerLock)                    
                    return false;


                goalUp.transform.localScale = localScale;
                playerMainScript.setGoalSizeCpu(realScale);
                ///playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

                //goalUpStandardColliders.SetActive(false);
                //goalUpEnlargeColliders.SetActive(true);
                cpuPowerLock = true;
                cpuPowersUsed[(int)POWER.CUT_GOAL_BY_HALF] = true;

                goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                localScale.y + 0.2f,
                                                                goalUpFlare[0].transform.position.z);
                goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                localScale.y + 0.2f,
                                                                goalUpFlare[1].transform.position.z);

                //goalUpCloth.enabled = false;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("com_extraPowerUsed_1");

                StartCoroutine(
                    roolbackChangeGoal(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                       goalUp,
                                       new Vector3(1.48f, 1.44f, 1f),
                                       new Vector3(5.25f, 3.5f, 14.0f),
                                       isCpu));
            }
            else
            {
                if (playerPowerLock[(int)POWER.CUT_GOAL_BY_HALF] < 1 ||
                    mainPlayerLock)
                    return false;

                goalDown.transform.localScale = localScale;
                playerMainScript.setGoalSize(realScale);

                playerPowerLock[(int)POWER.CUT_GOAL_BY_HALF]--;
                mainPlayerLock = true;

                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("com_extraPowerUsed_1");


                goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  localScale.y + 0.2f,
                                                                  goalDownFlare[0].transform.position.z);
                goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  localScale.y + 0.2f,
                                                                  goalDownFlare[1].transform.position.z);

                StartCoroutine(
                  roolbackChangeGoal(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                      goalDown,
                                      new Vector3(1.32f, 1.32f, 1f),
                                      new Vector3(4.7f, 3.1f, 14.0f),
                                      isCpu));
                StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                            powerButtonImg[powerIdx],
                                            powerButtons[powerIdx]));
            }
        }
        else
        {
            if (isCpu)
            {
                if (cpuPowersUsed[(int)POWER.CUT_GOAL_BY_HALF] ||
                    cpuPowerLock)                    
                    return false;

                cpuPowerLock = true;
                cpuPowersUsed[(int)POWER.CUT_GOAL_BY_HALF] = true;

                goalUp.transform.localScale = localScale;
                playerMainScript.setGoalSizeCpu(realScale);

                //playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

                // goalUpStandardColliders.SetActive(false);
                // goalUpEnlargeColliders.SetActive(true);



                goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                                localScale.y + 0.2f,
                                                                goalUpFlare[0].transform.position.z);
                goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                                localScale.y + 0.2f,
                                                                goalUpFlare[1].transform.position.z);

                //goalUpCloth.enabled = false;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("com_extraPowerUsed_1");

                StartCoroutine(
                    roolbackChangeGoal(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                        goalUp,
                                        new Vector3(3f, 1.758f, 3.498f),
                                        new Vector3(5.25f, 3.5f, 14.0f),
                                        isCpu));
            }
            else
            {

                if (playerPowerLock[(int)POWER.CUT_GOAL_BY_HALF] < 1 ||
                    mainPlayerLock)
                    return false;

                //print("EXTRAGOALS enlargegoals ");

                goalDown.transform.localScale = localScale;
                playerMainScript.setGoalSize(realScale);

                playerPowerLock[(int)POWER.CUT_GOAL_BY_HALF]--;
                mainPlayerLock = true;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("com_extraPowerUsed_1");

                ////goalDownCloth.enabled = false;
                // goalDown.transform.localScale = localScale;
                //playerMainScript.setGoalSize(realScale);

                ///goalDownStandardColliders.SetActive(false);
                ///goalDownEnlargeColliders.SetActive(true);

                goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  localScale.y + 0.2f,
                                                                  goalDownFlare[0].transform.position.z);
                goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  localScale.y + 0.2f,
                                                                  goalDownFlare[1].transform.position.z);

                StartCoroutine(
                  roolbackChangeGoal(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                      goalDown,
                                      new Vector3(3f, 1.558f, 3.13412f),
                                      new Vector3(4.7f, 3.1f, 14.0f),
                                      isCpu));
                StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF],
                                powerButtonImg[powerIdx],
                                powerButtons[powerIdx]));
            }
        }

        return true;
    }

    IEnumerator roolbackChangeGoal(float delay,
                                   GameObject goal,
                                   Vector3 localScale,
                                   Vector3 realScale,
                                   bool isCpu)
    {
        yield return new WaitForSeconds(delay);

        if (isCpu)
        {
            for (int i = 0; i < 10; i++)
            {
                if ((playerMainScript.isShotActive() &&
                    (playerMainScript.getBallPos().z > 13.2f)) ||
                    playerMainScript.ball[1].getBallGoalCollisionStatus())
                {
                    if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                        yield return new WaitForSeconds(0.35f);
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }

            //if (Globals.stadiumNumber == 1)
            //{
            //    goalUpStandardColliders.SetActive(true);
            //    goalUpEnlargeColliders.SetActive(false);
            // }

            goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                            3.656f,
                                                            goalUpFlare[0].transform.position.z);
            goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                            3.656f,
                                                            goalUpFlare[1].transform.position.z);

            //playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());


            goal.transform.localScale = localScale;
            playerMainScript.setGoalSizeCpu(realScale);
            cpuPowerLock = false;

            //goalUpCloth.enabled = true;

        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if ((playerMainScript.cpuPlayer.getShotActive() &&
                    (playerMainScript.getBallPos().z < -13.2f)) ||
                     playerMainScript.ball[1].getBallGoalCollisionStatus())
                {
                    if (playerMainScript.ball[1].getBallGoalCollisionStatus())
                        yield return new WaitForSeconds(0.35f);
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }

            goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                              3.237f,
                                                              goalDownFlare[0].transform.position.z);
            goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                              3.237f,
                                                              goalDownFlare[1].transform.position.z);

            //if (Globals.stadiumNumber == 1)
            ///{
            ///     goalDownStandardColliders.SetActive(true);
            //    goalDownEnlargeColliders.SetActive(false);
            //}

            goal.transform.localScale = localScale;
            playerMainScript.setGoalSize(realScale);

            mainPlayerLock = false;

            //goalDownCloth.enabled = true;
            //isPlayerGoalEnlarge = false;
        }
    }

    public bool badConditions(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int)POWER.BAD_CONDITIONS] < 1) ||
                 mainPlayerLock)
                return false;

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK

            playerPowerLock[(int)POWER.BAD_CONDITIONS]--;
            mainPlayerLock = true;

            StartCoroutine(enableBadCondition(
                                              isCpu,
                                              powerMaxTime[(int)POWER.BAD_CONDITIONS]));
            StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.BAD_CONDITIONS],
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int)POWER.BAD_CONDITIONS] ||
                cpuPowerLock)               
                return false;

            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");

            cpuPowersUsed[(int)POWER.BAD_CONDITIONS] = true;

            StartCoroutine(enableBadCondition(
                isCpu,
                powerMaxTime[(int)POWER.BAD_CONDITIONS]));            
        }

        return true;
    }

    IEnumerator enableBadCondition(bool isCpu, float delay)
    {
        if (!isCpu)
        {
            isPlayerSlowDown[1] = true;
            snowParticle[1].Play();
        }
        else
        {
            isPlayerSlowDown[0] = true;
            snowParticle[0].Play();
        }

        //audioManager.PlayAtTheSameTime("wind1");

        yield return new WaitForSeconds(delay);

        if (!isCpu)
        {
            isPlayerSlowDown[1] = false;
            snowParticle[1].Stop();
        }
        else
        {
            isPlayerSlowDown[0] = false;
            snowParticle[0].Stop();
        }

        //audioManager.Stop("wind1");

        if (!isCpu)
            mainPlayerLock = false;
        else
            cpuPowerLock = false;

    }


    private void initPowerTimes()
    {
        powerMaxTime = new float[MAX_POWERS + 1];

        powerMaxTime[(int)POWER.CUT_GOAL_BY_HALF] = 3f;
        powerMaxTime[(int)POWER.SILVER_BALL] = 4f;
        powerMaxTime[(int)POWER.SHAKE_CAMERA] = 1.5f;
        powerMaxTime[(int)POWER.INVISABLE_PLAYER] = 4f;
        powerMaxTime[(int)POWER.ENABLE_FLARE] = 4f;
        powerMaxTime[(int)POWER.BAD_CONDITIONS] = 5f;
        powerMaxTime[(int)POWER.GOLDEN_BALL] = 4f;
    }
}
