using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalsNS;
using AudioManagerNS;
using graphicsCommonNS;
using UnityEngine.SceneManagement;

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

public class Powers : MonoBehaviour
{
    private int MAX_POWERS = 10;
    private int MAX_POWERS_IN_USE = 3;
    private controllerRigid playerMainScript;
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
    private bool[] powerChoosen;
    private bool cpuPowerLock = false;
    private bool mainPlayerLock = false;
    private bool isPlayerGoalEnlarge = false;
    private bool isCpuGoalEnlarge = false;

    public GameObject[] goalUpFlare;
    public GameObject[] goalDownFlare;

    private bool isPowerEnable = true;

    private int isGoalUpHandicap = 0;
    private int isGoalDownHandicap = 0;

    private float[] powerMaxTime;
    public ParticleSystem[] flareParticle;
    private GraphicsCommon graphics;

    public delegate bool DelegateFunc(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2);
    private List<DelegateFunc> funcList  = new List<DelegateFunc>();
    private List<DelegateFunc> listPowerChoosen = new List<DelegateFunc>();
    public GameObject[] extraGrounds;
    public Renderer[] playerRenderer;
    public Renderer[] hairRenderer;
    public Renderer ballRenderer;
    public ParticleSystem[] invisibility;
    public ParticleSystem[] snowParticle;
    private bool[] isPlayerSlowDown;
    private bool[] isFlareTurn;
    private int numOfPowersUsedCpu = 0;
    private bool isInvisibleUpActive = false;
    private bool isInvisibleDownActive = false;
    int powerIdx = 0;
  
    void Start()
    {
        //print("##trainig " + Globals.isTrainingActive + " bonus " + Globals.isBonusActive);

        if (Globals.powersStr.Equals("NO") ||
            Globals.isTrainingActive ||
            Globals.isBonusActive)
        {
            isPowerEnable = false;
            return;
        }

        graphics = new GraphicsCommon();

        cpuPowersUsed = new bool[MAX_POWERS + 1];
        playerPowerLock = new int[MAX_POWERS + 1];
        isPlayerSlowDown = new bool[2];
        isFlareTurn = new bool[2];

        initPowerTimes();
        initPowerChosen();

        for (int i = 0; i < cpuPowersUsed.Length; i++)
            cpuPowersUsed[i] = false;

        for (int i = 0; i < playerPowerLock.Length; i++)       
            playerPowerLock[i] = 1;

        for (int i = 0; i < isPlayerSlowDown.Length; i++)
        {
            isPlayerSlowDown[i] = false;
            isFlareTurn[i] = false;
        }

        audioManager = FindObjectOfType<AudioManager>();
        playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();


        if (Globals.PITCHTYPE.Equals("STREET") ||
            (Globals.stadiumNumber == 2))
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

    // Update is called once per frame
    void Update()
    {
        if (Globals.powersStr.Equals("NO") ||
            Globals.isTrainingActive ||
            Globals.isBonusActive)
        {
            isPowerEnable = false;
            return;
        }

        if ((playerMainScript != null) && 
            (playerMainScript.getShotActive()))
        {
            enablePlayerInvisibility(false);
            /*if (!playerRenderer[0].enabled)
            {
                playerRenderer[0].enabled = true;
                hairRenderer[0].enabled = true;
                ballRenderer.enabled = true;
            }*/
        }

        if ((playerMainScript != null) &&
            (playerMainScript.cpuPlayer.getShotActive()))
        {
            enablePlayerInvisibility(true);
/*            if (!playerRenderer[1].enabled)
            {
                playerRenderer[1].enabled = true;
                hairRenderer[1].enabled = true;
                ballRenderer.enabled = true;
            }*/
        }
    }

    public void button1Action()
    {
        if (!isPowerEnable ||
            !playerMainScript.doesGameStarted())
            return;

        //if (Globals.stadiumNumber == 0)
            executePower(0, false);
       // else
       //     twoExtraGoals(false, 0, Vector3.zero, Vector3.zero);

    }

    public void button2Action()
    {
        if (!isPowerEnable ||
           !playerMainScript.doesGameStarted())
            return;
        //goalObstacles(false);
        //if (Globals.stadiumNumber == 0)
        //{
            executePower(1, false);
        //} else
       // {
       //     cutGoalByHalf(false, 1, Vector3.zero, Vector3.zero);
       // }
        //listPowerChoosen[1](false, Vector3.zero, Vector3.zero);
    }

    public void button3Action()
    {
        if (!isPowerEnable ||
            !playerMainScript.doesGameStarted())
            return;


        //if (Globals.stadiumNumber == 0) {
            executePower(2, false);
        //}
        //else
        //{
        //    resizeOpponentGoal(false,
        //                       3,
        //                       new Vector3(3f, 2.3f, 5.34f),
        //                       new Vector3(8f, 4.6f, 14f));
        //}

        /* if (Globals.stadiumNumber != 1)
         {

             listPowerChoosen[2](false, Vector3.zero, Vector3.zero);

             resizeOpponentGoal(false,
                                new Vector3(2.223f, 1.922f, 1f),
                                new Vector3(8f, 4.6f, 14f));
         }
         else
         {
             resizeOpponentGoal(false,
                        new Vector3(3f, 2.3f, 5.34f),
                        new Vector3(8f, 4.6f, 14f));
         }*/
    }

    private void executePower(int idx, bool isCpu)
    {
        if (listPowerChoosen[idx].Method.Name.Equals("resizeOpponentGoal"))
        {
            if (Globals.stadiumNumber != 1)
            {

                listPowerChoosen[idx](isCpu,
                                   idx,
                                   new Vector3(2.223f, 1.922f, 1f),
                                   new Vector3(8f, 4.6f, 14f));
            }
            else
            {
                listPowerChoosen[idx](isCpu,
                           idx,
                           new Vector3(3f, 2.3f, 5.34f),
                           new Vector3(8f, 4.6f, 14f));
            }
        }
        else if (listPowerChoosen[idx].Method.Name.Equals("cutGoalByHalf"))
        {
            if (Globals.stadiumNumber != 1)
            {
                if (!isCpu)
                {
                    listPowerChoosen[idx](isCpu,
                                          idx,
                                          new Vector3(1.32f, 1.32f / 1.5f, 1f),
                                          new Vector3(4.7f, 3.1f / 1.5f, 14f));
                }
                else
                {
                    listPowerChoosen[idx](isCpu,
                                          idx,
                                          new Vector3(1.48f, 1.44f / 1.5f, 1f),
                                          new Vector3(5.25f, 3.5f / 1.5f, 14.0f));
                }
            }
            else
            {
                if (!isCpu)
                {
                    listPowerChoosen[idx](isCpu,
                                     idx,
                                     new Vector3(3f, 1.558f / 1.5f, 3.13412f),
                                     new Vector3(4.7f, 3.1f / 1.5f, 14.0f));
                }
                else
                {
                    listPowerChoosen[idx](isCpu,
                                          idx,
                                          new Vector3(3f, 1.758f / 1.5f, 3.498f),
                                          new Vector3(5.25f, 3.5f / 1.5f, 14.0f));
                }
            }
        } else
        {
            listPowerChoosen[idx] (isCpu, 
                                  idx, 
                                  Vector3.zero, 
                                  Vector3.zero);
        }        
    }

    public bool twoExtraGoals(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 2)
            return false;

        if (Globals.stadiumNumber != 1)
        {
            if (!isCpu)
            {
                if (playerPowerLock[(int) POWER.TWO_EXTRA_GOAL] < 1 ||
                    mainPlayerLock)
                    return false;

                wallUpRight1.transform.localScale =
                wallUpLeft2.transform.localScale =
                       new Vector3(4.06f, 1.3f, 0.15f);

                wallUpRight1.transform.position =
                    new Vector3(10.728f, 0.65f, 14.075f);
                wallUpLeft2.transform.position =
                    new Vector3(-10.728f, 0.65f, 14.075f);

                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");
                playerMainScript.cpuPlayer.setExtraGoals(true, new Vector3(13f, 3.5f, 14f));
                smallGoal[0].SetActive(true);
                smallGoal[1].SetActive(true);
                playerPowerLock[(int) POWER.TWO_EXTRA_GOAL]--;
                mainPlayerLock = true;

                //goalUpStandardColliders.SetActive(false);
                //goalUpExtraGoalsColliders.SetActive(true);

                StartCoroutine(removeSmallGoals(ADD_SMALL_GOALS_TIME, isCpu));
                StartCoroutine(fillAmountImg(ADD_SMALL_GOALS_TIME, 
                                             powerButtonImg[powerIdx], 
                                             powerButtons[powerIdx]));
            }
            else
            {
                if (cpuPowersUsed[(int) POWER.TWO_EXTRA_GOAL] ||
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                numOfPowersUsedCpu++;
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

                cpuPowerLock = true;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");

                cpuPowersUsed[(int) POWER.TWO_EXTRA_GOAL] = true;
                smallGoal[2].SetActive(true);
                smallGoal[3].SetActive(true);
                playerMainScript.setExtraGoals(true);


                StartCoroutine(removeSmallGoals(ADD_SMALL_GOALS_TIME, isCpu));
            }
        } else
        //sport hall
        {
            if (!isCpu)
            {
                if (playerPowerLock[(int) POWER.TWO_EXTRA_GOAL] < 1 ||
                    mainPlayerLock)
                    return false;

                print("playerPowerLock");

                wallUpRight1.transform.localScale =
                wallUpLeft2.transform.localScale =
                       new Vector3(4.06f, 1.3f, 0.15f);

                //wallUpRight2.transform.localScale =
                //wallUpLeft1.transform.localScale =
                //      new Vector3(8.5f, 1.3f, 0.15f);

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

                playerMainScript.cpuPlayer.setExtraGoals(true, new Vector3(13f, 3.5f, 14f));
                smallGoal[0].SetActive(true);
                smallGoal[1].SetActive(true);
                playerPowerLock[(int) POWER.TWO_EXTRA_GOAL]--;
                mainPlayerLock = true;

                goalUpStandardColliders.SetActive(false);
                goalUpExtraGoalsColliders.SetActive(true);

                StartCoroutine(removeSmallGoals(ADD_SMALL_GOALS_TIME, isCpu));
                StartCoroutine(fillAmountImg(ADD_SMALL_GOALS_TIME, 
                                             powerButtonImg[powerIdx], 
                                             powerButtons[powerIdx]));
            }
            else
            {
                if (cpuPowersUsed[(int) POWER.TWO_EXTRA_GOAL] ||
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                numOfPowersUsedCpu++;
                cpuPowerLock = true;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("twoextragoals");

                cpuPowersUsed[(int)POWER.TWO_EXTRA_GOAL] = true;
                smallGoal[2].SetActive(true);
                smallGoal[3].SetActive(true);
                goalDownStandardColliders.SetActive(false);
                goalDownExtraGoalsColliders.SetActive(true);
                playerMainScript.setExtraGoals(true);
                StartCoroutine(removeSmallGoals(ADD_SMALL_GOALS_TIME, isCpu));
            }

            return true;
        }

        return true;
    }

    public bool goalObstacles(bool isCpu, 
                              int powerIdx, 
                              Vector3 arg1, 
                              Vector3 arg2)
    {
        if (!isPowerEnable)
            return false;

        if (!isCpu)
        {
            if (playerPowerLock[(int) POWER.GOAL_WALL] < 1 ||
                mainPlayerLock)
                return false;

            goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.GOAL_WALL]--;
            mainPlayerLock = true;
            StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalDownObstacles[0], isCpu));
            StartCoroutine(fillAmountImg(GOALS_OBSTACLES_TIME, 
                                         powerButtonImg[powerIdx], 
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.GOAL_WALL] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int) POWER.GOAL_WALL] = true;
            goalUpObstacles[0].SetActive(true);

            StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }

    /*public bool freezePlayer(bool isCpu)
    {
        if (!isPowerEnable)
            return false;

        if (!isCpu)
        {
            if (playerPowerLock[2] < 1 ||
                mainPlayerLock)
                return false;

            playerMainScript.cpuPlayer.playerFreeze(true);
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
 */

    public bool resizeOpponentGoal(bool isCpu, 
                                   int powerIdx,
                                   Vector3 localScale, 
                                   Vector3 realScale)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 2)
            return false;

        if (Globals.stadiumNumber != 1)
        {
            if (!isCpu)
            {
                if (playerPowerLock[(int) POWER.ENLARGE_GOAL] < 1 ||
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
                    new Vector3(11.9f, 1.3f, 0.15f);

                goalUp.transform.localScale = localScale;
                goalUp.transform.localScale = localScale;
                playerMainScript.setGoalSizeCpu(realScale);
                playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

                //goalUpStandardColliders.SetActive(false);
                //goalUpEnlargeColliders.SetActive(true);
                playerPowerLock[(int)POWER.ENLARGE_GOAL]--;
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
                    rollbackEnlargeGoal(ENLARGE_OPPONENT_GOAL_TIME,
                                        goalUp,
                                        new Vector3(1.48f, 1.44f, 1f),
                                        new Vector3(5.25f, 3.5f, 14.0f),
                                        false));
                StartCoroutine(fillAmountImg(ENLARGE_OPPONENT_GOAL_TIME, 
                                             powerButtonImg[powerIdx], 
                                             powerButtons[powerIdx]));
            }
            else
            {
                if (cpuPowersUsed[(int) POWER.ENLARGE_GOAL] ||
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                numOfPowersUsedCpu++;
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
                cpuPowersUsed[(int)POWER.ENLARGE_GOAL] = true;
                isPlayerGoalEnlarge = true;
                audioManager.Play("extraPowerEnable");
                audioManager.Commentator_extraPower("enlargeGoal");


                goalDown.SetActive(false);
                goalDownBigger.SetActive(true);
                goalDownFlare[0].transform.position = new Vector3(goalDownFlare[0].transform.position.x,
                                                                  3.64f,
                                                                  goalDownFlare[0].transform.position.z);
                goalDownFlare[1].transform.position = new Vector3(goalDownFlare[1].transform.position.x,
                                                                  3.64f,
                                                                  goalDownFlare[1].transform.position.z);

                StartCoroutine(
                  rollbackEnlargeGoal(ENLARGE_OPPONENT_GOAL_TIME,
                                      goalDown,
                                      new Vector3(1.32f, 1.32f, 1f),
                                      new Vector3(4.7f, 3.1f, 14.0f),
                                      true));
            }
        }
        else
        {
            if (!isCpu)
            {
                if (playerPowerLock[(int)POWER.ENLARGE_GOAL] < 1 ||
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
                playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

                goalUpStandardColliders.SetActive(false);
                goalUpEnlargeColliders.SetActive(true);
                playerPowerLock[(int) POWER.ENLARGE_GOAL]--;
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
                    rollbackEnlargeGoal(ENLARGE_OPPONENT_GOAL_TIME,
                                        goalUp,
                                        new Vector3(3f, 1.758f, 3.498f),
                                        new Vector3(5.25f, 3.5f, 14.0f),
                                        false));
                StartCoroutine(fillAmountImg(ENLARGE_OPPONENT_GOAL_TIME, 
                                             powerButtonImg[powerIdx], 
                                             powerButtons[powerIdx]));
            }
            else
            {
                if (cpuPowersUsed[(int)POWER.ENLARGE_GOAL] ||
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                //print("EXTRAGOALS enlargegoals ");

                numOfPowersUsedCpu++;
                cpuPowerLock = true;
                cpuPowersUsed[(int)POWER.ENLARGE_GOAL] = true;
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
                  rollbackEnlargeGoal(ENLARGE_OPPONENT_GOAL_TIME,
                                      goalDown,
                                      new Vector3(3f, 1.558f, 3.13412f),
                                      new Vector3(4.7f, 3.1f, 14.0f),
                                      true));
            }
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
                                    bool isCpu)
    {
        yield return new WaitForSeconds(delay);
    
        if (!isCpu)
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

            recoverAdsOriginalSize(isCpu);
            if (Globals.stadiumNumber == 1)
            {
                goalUpStandardColliders.SetActive(true);
                goalUpEnlargeColliders.SetActive(false);
            }

            wallUpLeft1.SetActive(true);
            wallUpRight1.SetActive(true);
     
            goalUpFlare[0].transform.position = new Vector3(goalUpFlare[0].transform.position.x,
                                                            3.656f,
                                                            goalUpFlare[0].transform.position.z);
            goalUpFlare[1].transform.position = new Vector3(goalUpFlare[1].transform.position.x,
                                                            3.656f,
                                                            goalUpFlare[1].transform.position.z);

            playerMainScript.cpuPlayer.recalculateCornerPoints(playerMainScript.getGoalSizePlr2());

            mainPlayerLock = false;
            isCpuGoalEnlarge = false;

            goal.transform.localScale = localScale;
            playerMainScript.setGoalSizeCpu(realScale);
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
        }
    }

    private void recoverAdsOriginalSize(bool isCpu)
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

        if (!isCpu)
        {
            wallUpLeft1.transform.position =
                new Vector3(-16.45f, 0.65f, 14.075f);
            wallUpLeft2.transform.position =
                new Vector3(-9.05f, 0.65f, 14.075f);

            wallUpRight1.transform.position =
                new Vector3(9.05f, 0.65f, 14.075f);
            wallUpRight2.transform.position =
                new Vector3(16.45f, 0.65f, 14.075f);

            wallUpLeft1.transform.position =
                new Vector3(-16.381f, 0.65f, 14.075f);

            wallUpLeft1.transform.localScale =
            wallUpRight1.transform.localScale =
            wallUpLeft2.transform.localScale =
            wallUpRight2.transform.localScale =
                new Vector3(7.4f, 1.3f, 0.15f);

            //goalUpStandardColliders.SetActive(true);
            //goalUpExtraGoalsColliders.SetActive(false);
        }
        else
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
  
    }


    IEnumerator removeSmallGoals(float delay, bool isCpu)
    {
        yield return new WaitForSeconds(delay);

        if (!isCpu)
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

            smallGoal[0].SetActive(false);
            smallGoal[1].SetActive(false);
            recoverAdsOriginalSize(isCpu);
            playerMainScript.cpuPlayer.setExtraGoals(false, playerMainScript.getGoalSizePlr2());
            mainPlayerLock = false;
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
        }
    }

    IEnumerator unFreezePlayer(float delay, bool isCpu)
    {

        yield return new WaitForSeconds(delay);
        if (!isCpu)
        {
            playerMainScript.cpuPlayer.playerFreeze(false);
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
        if (!playerMainScript.cpuPlayer.getShotActive())
            return false;

        playerMainScript.cpuPlayer.setShotActive(false);
        playerMainScript.setGkHelperImageVal(false);
        playerMainScript.setBallVelocity(Vector3.zero, Vector3.zero);
        audioManager.Play("extraPowerEnable");
        return true;
        //StartCoroutine(unFreezePlayer(FREEZE_PLAYER_TIME));
    }

    public bool getPlayerGoalEnlarge()
    {
        if (!isPowerEnable) 
            return false;

        return isPlayerGoalEnlarge;
    }

    public bool getCpuGoalEnlarge()
    {
        if (!isPowerEnable)
            return false;

        return isCpuGoalEnlarge;
    }


    public int getGoalDownHandical() 
    {
        if (!isPowerEnable)
            return 0;
        return isGoalDownHandicap;
    } 

    public int getGoalUpHandicap()
    {        
         if (!isPowerEnable)
               return 0;

         return isGoalUpHandicap;
    }


    IEnumerator setBallTexture(bool isCpu,
                               float delay,
                               Renderer ballRenderer,
                               int goalSide,
                               int goalVal,
                               string ballTexName)
    {
        Texture ballTex =
            ballRenderer.material.mainTexture;

        Texture2D ballTexture =
                           graphics.getTexture(ballTexName);
        ballRenderer.material.SetTexture("_MainTex", ballTexture);
        if (goalSide == 1)
            isGoalUpHandicap = goalVal;
        else
            isGoalDownHandicap = goalVal;

        yield return new WaitForSeconds(delay);

        if (goalSide == 1)
            isGoalUpHandicap = 0;
        else
            isGoalDownHandicap = 0;

        //recover original ball Texture
        ballRenderer.material.SetTexture("_MainTex", ballTex);
        if (!isCpu)
            mainPlayerLock = false;
        else
            cpuPowerLock = false;
    }

    private void initPowerTimes()
    {
        powerMaxTime = new float[MAX_POWERS + 1];

        powerMaxTime[(int) POWER.CUT_GOAL_BY_HALF] = 3f;
        powerMaxTime[(int) POWER.SILVER_BALL] = 4f;
        powerMaxTime[(int) POWER.SHAKE_CAMERA] = 1.5f;
        powerMaxTime[(int) POWER.INVISABLE_PLAYER] = 4f;
        powerMaxTime[(int) POWER.ENABLE_FLARE] = 4f;
        powerMaxTime[(int) POWER.BAD_CONDITIONS] = 5f;
        powerMaxTime[(int) POWER.GOLDEN_BALL] = 4f;
    }

    private void initPowerChosen()
    {
        powerChoosen = new bool[MAX_POWERS + 1];

        if (Globals.PITCHTYPE.Equals("STREET") ||
            (Globals.stadiumNumber == 2))
        {
            funcList.Add(twoExtraGoals);
            funcList.Add(cutGoalByHalf);
            funcList.Add(resizeOpponentGoal);
            funcList.Add(silverBall);
            funcList.Add(shakeCamera);
            funcList.Add(goalObstacles);
            funcList.Add(invisiblePlayer);
            funcList.Add(badConditions);
            funcList.Add(enableFlares);
            funcList.Add(goldenBall);
        } else
        {
            funcList.Add(twoExtraGoals);
            funcList.Add(cutGoalByHalf);
            funcList.Add(resizeOpponentGoal);
            funcList.Add(silverBall);
            funcList.Add(shakeCamera);
            funcList.Add(goalObstacles);
            funcList.Add(invisiblePlayer);
            funcList.Add(badConditions);
            funcList.Add(enableFlares);
            funcList.Add(goldenBall);
        }

        for (int i = 0; i < powerChoosen.Length; i++)
        {
            powerChoosen[i] = false;
        }

        if ((Globals.stadiumNumber == 0) &&
            (!Globals.isMultiplayer) &&
            (!Globals.PITCHTYPE.Equals("STREET")))
        {

            string selectedPowers = PlayerPrefs.GetString("POWERS_SELECTED");
            //print("selected powers " + selectedPowers);
            string[] selectedPowerState = selectedPowers.Split('_');

            for (int i = 0; i < Globals.MAX_POWERS; i++)
            {
                if (selectedPowerState[i].Equals("1"))
                {
                    powerChoosen[i] = true;
                }
                else
                {
                    powerChoosen[i] = false;
                }
            }
        }
        else
        //its sport hall or multiplayer
        {
            if (!Globals.PITCHTYPE.Equals("STREET") &&
                (Globals.stadiumNumber != 2))
            {
                powerChoosen[0] = true;
                powerChoosen[1] = true;
                powerChoosen[5] = true;
            } else
            {
                powerChoosen[1] = true;
                powerChoosen[5] = true;
                powerChoosen[7] = true;
            }
        }



            //powerChoosen[6] = true;
            //powerChoosen[7] = true;
            //powerChoosen[8] = true;

        powerIdx = 0;
        for (int i = 0; i < funcList.Count; i++) {
            if (powerChoosen[i])
            {
                listPowerChoosen.Add(funcList[i]);
                //print("powerFileNames " + Globals.powerFileNames[i]);
                powerButtons[powerIdx].GetComponent<Image>().sprite = 
                    Resources.Load<Sprite>(Globals.powerFileNames[i]);

                powerButtons[powerIdx].SetActive(true);
                powerIdx++;
                if (powerIdx >= MAX_POWERS_IN_USE)
                    break;
            }
        }

        //button.GetComponent<Image>().sprite

    }

    public int getNumberOfActivePowers()
    {
        if (!isPowerEnable)
            return 0;

        return powerIdx;
    }

    //silver ball - goal count x3
    public bool goldenBall(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 2)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int) POWER.GOLDEN_BALL] < 1) ||
                mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.GOLDEN_BALL]--;
            mainPlayerLock = true;
 
            StartCoroutine(setBallTexture(
                    isCpu,
                    powerMaxTime[(int) POWER.GOLDEN_BALL], 
                    ballRenderer, 
                    1, 
                    3,
                    "ball/textures/st_ball_gold"));
            StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.GOLDEN_BALL], 
                                         powerButtonImg[powerIdx], 
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.GOLDEN_BALL] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int) POWER.GOLDEN_BALL] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(setBallTexture(
                isCpu,
                powerMaxTime[(int)POWER.GOLDEN_BALL], 
                ballRenderer, 
                2, 
                3,
                "ball/textures/st_ball_gold"));
            /*StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.GOLDEN_BALL], 
                                         powerButtonImg[powerIdx], 
                                         powerButtons[powerIdx]));*/
            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }

    public bool enableFlares(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable || 
            Globals.stadiumNumber == 1 || 
            Globals.stadiumNumber == 2)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int) POWER.ENABLE_FLARE] < 1) ||
                mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);
            
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.ENABLE_FLARE]--;
            mainPlayerLock = true;

            StartCoroutine(turnOnFlare(
                    isCpu,
                    powerMaxTime[(int) POWER.ENABLE_FLARE]));            
            StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.ENABLE_FLARE],
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.ENABLE_FLARE] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int) POWER.ENABLE_FLARE] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(turnOnFlare(
                     isCpu,
                     powerMaxTime[(int) POWER.ENABLE_FLARE]));
            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }


    IEnumerator turnOnFlare(bool isCpu, float delay)
    {
        if (isCpu)
        {
            flareParticle[0].Play();
            isFlareTurn[0] = true;
            audioManager.PlayAtTheSameTime("fireworks1");
        } else
        {
            isFlareTurn[1] = true;
            audioManager.PlayAtTheSameTime("fireworks1");
        }

        /*yield return new WaitForSeconds(delay / 2f);
        if (isCpu)
        {
            flareParticle[0].Stop();
        }
        else
        {
            flareParticle[1].Stop();
        }*/

        yield return new WaitForSeconds(delay);

        if (isCpu)
        {
            isFlareTurn[0] = false;

            //flareParticle[0].Stop();
            //flareParticle[1].transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isFlareTurn[1] = false;
        }


        if (!isCpu)
            mainPlayerLock = false;
        else
            cpuPowerLock = false;

        audioManager.Stop("fireworks1");
    }

    public bool getIsFlareUpEnable()
    {
        if (!isPowerEnable)
            return false;

        return isFlareTurn[1];
    }

    public bool getIsFlareDownEnable()
    {
        if (!isPowerEnable)
            return false;

        return isFlareTurn[0];
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
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                numOfPowersUsedCpu++;

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
                    roolbackChangeGoal(powerMaxTime[(int) POWER.CUT_GOAL_BY_HALF],
                                       goalUp,
                                       new Vector3(1.48f, 1.44f, 1f),
                                       new Vector3(5.25f, 3.5f, 14.0f),
                                       isCpu));               
            }
            else
            {
                if (playerPowerLock[(int) POWER.CUT_GOAL_BY_HALF] < 1 ||
                    mainPlayerLock)
                    return false;

                goalDown.transform.localScale = localScale;
                playerMainScript.setGoalSize(realScale);

                playerPowerLock[(int) POWER.CUT_GOAL_BY_HALF]--;
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
                  roolbackChangeGoal(powerMaxTime[(int) POWER.CUT_GOAL_BY_HALF],
                                      goalDown,
                                      new Vector3(1.32f, 1.32f, 1f),
                                      new Vector3(4.7f, 3.1f, 14.0f),
                                      isCpu));
                StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.CUT_GOAL_BY_HALF],
                                            powerButtonImg[powerIdx],
                                            powerButtons[powerIdx]));
            }
        }
        else
        {
            if (isCpu)
            {
                if (cpuPowersUsed[(int)POWER.CUT_GOAL_BY_HALF] ||
                    cpuPowerLock ||
                    (numOfPowersUsedCpu >= 3))
                    return false;

                numOfPowersUsedCpu++;
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

                if (playerPowerLock[(int) POWER.CUT_GOAL_BY_HALF] < 1 ||
                    mainPlayerLock)
                    return false;

                //print("EXTRAGOALS enlargegoals ");

                goalDown.transform.localScale = localScale;
                playerMainScript.setGoalSize(realScale);

                playerPowerLock[(int) POWER.CUT_GOAL_BY_HALF]--;
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

    public bool silverBall(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 2)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int)POWER.SILVER_BALL] < 1) ||
                mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int)POWER.SILVER_BALL]--;
            mainPlayerLock = true;

            StartCoroutine(setBallTexture(
                    isCpu,
                    powerMaxTime[(int)POWER.SILVER_BALL],
                    ballRenderer,
                    1,
                    2,
                    "ball/textures/st_ball_silver"));
            StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.SILVER_BALL],
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.SILVER_BALL] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;


            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int)POWER.SILVER_BALL] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(setBallTexture(
                isCpu,
                powerMaxTime[(int)POWER.SILVER_BALL],
                ballRenderer,
                2,
                2,
                "ball/textures/st_ball_silver"));
            /*StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.SILVER_BALL],
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));*/

            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }


    public bool badConditions(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 1)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int) POWER.BAD_CONDITIONS] < 1) ||
                 mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.BAD_CONDITIONS]--;
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
            if (cpuPowersUsed[(int) POWER.BAD_CONDITIONS] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");

            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int) POWER.BAD_CONDITIONS] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(enableBadCondition(
                isCpu,
                powerMaxTime[(int) POWER.BAD_CONDITIONS]));
         
         /*   StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.BAD_CONDITIONS], 
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));*/
            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }

    IEnumerator enableBadCondition(bool isCpu, float delay)
    {

        /*if (!isCpu)
        {
            extraGrounds[1].SetActive(true);
        } else
        {
            extraGrounds[2].SetActive(true);
        }

        rainParticle.Play();
        audioManager.PlayAtTheSameTime("wind1");

        yield return new WaitForSeconds(delay);

        if (!isCpu)
        {
            extraGrounds[1].SetActive(false);
        }
        else
        {
            extraGrounds[2].SetActive(false);
        }

        rainParticle.Stop();
        audioManager.Stop("wind1");*/

        if (!isCpu)
        {
            isPlayerSlowDown[1] = true;
            snowParticle[1].Play();
        } else
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

    public bool isPlayerDownSlowDown()
    {
        if (!isPowerEnable)
            return false;

        return isPlayerSlowDown[0];
    }

    public bool isPlayerUpSlowDown()
    {
        if (!isPowerEnable)
            return false;

        return isPlayerSlowDown[1];
    }

    private bool isCameraUpShakeActive = false;
    private bool isCameraDownShakeActive = false;

    public bool shakeCamera(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 2)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int) POWER.SHAKE_CAMERA] < 1) ||
                mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.SHAKE_CAMERA]--;
            mainPlayerLock = true;

            StartCoroutine(setCameraShake(
                    isCpu,
                    1,
                    powerMaxTime[(int)POWER.SHAKE_CAMERA]));                   
            StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.SHAKE_CAMERA], 
                                         powerButtonImg[powerIdx], 
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.SHAKE_CAMERA] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            Handheld.Vibrate();
            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int)POWER.SHAKE_CAMERA] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(setCameraShake(
                           isCpu,
                           2,
                           powerMaxTime[(int)POWER.SHAKE_CAMERA]));                          
            /*StartCoroutine(fillAmountImg(powerMaxTime[(int)POWER.SHAKE_CAMERA], 
                                         powerButtonImg[powerIdx], 
                                         powerButtons[powerIdx]));*/
            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }

   

    private bool isPlayerDownInvisible = false;
    private bool isPlayerUpInvisible = false;

    public bool getIsPlayerDownInvisible()
    {
        if (!isPowerEnable)
            return false;

        return isPlayerDownInvisible;
    }

    public bool getIsPlayerUpInvisible()
    {
        if (!isPowerEnable)
            return false;

        return isPlayerUpInvisible;
    }
    IEnumerator makePlayerInvisible(bool isCpu, float delay)
    {
        if (!isCpu)
        {
            invisibility[0].Play();
            yield return new WaitForSeconds(0.5f);
            audioManager.Play("shortSound");
            yield return new WaitForSeconds(1.2f);
            isPlayerDownInvisible = true;

            //playerRenderer[0].enabled = false;
            //ballRenderer.enabled = false;
        }
        else
        {
            invisibility[1].Play();
            yield return new WaitForSeconds(0.4f);
            audioManager.Play("shortSound");
            //yield return new WaitForSeconds(1.2f);
            isPlayerUpInvisible = true;
            playerRenderer[1].enabled = false;
            hairRenderer[1].enabled = false;
            ballRenderer.enabled = false;
        }


        yield return new WaitForSeconds(delay);


        enablePlayerInvisibility(isCpu);
       /* if (!isCpu)
        {
            if (!isPlayerDownInvisible)
                yield break;

            playerRenderer[0].enabled = true;
            invisibility[0].Play();
            ballRenderer.enabled = true;
            isPlayerDownInvisible = false;
            mainPlayerLock = false;
        }
        else
        {
            if (!isPlayerUpInvisible)
                yield break;

            playerRenderer[1].enabled = true;
            invisibility[1].Play();
            ballRenderer.enabled = true;
            hairRenderer[1].enabled = true;
            isPlayerUpInvisible = false;
            cpuPowerLock = false;
        }*/

        audioManager.Play("shortSound");
    }

   
    private void enablePlayerInvisibility(bool isCpu)
    {
        if (!isCpu)
        {
            if (!isPlayerDownInvisible)
                return;

            playerRenderer[0].enabled = true;
            invisibility[0].Play();
            ballRenderer.enabled = true;
            isPlayerDownInvisible = false;
            mainPlayerLock = false;
        }
        else
        {
            if (!isPlayerUpInvisible)
                return;

            playerRenderer[1].enabled = true;
            invisibility[1].Play();
            ballRenderer.enabled = true;
            hairRenderer[1].enabled = true;
            isPlayerUpInvisible = false;
            cpuPowerLock = false;
        }

        audioManager.Play("shortSound");
    }

    public bool invisiblePlayer(bool isCpu, int powerIdx, Vector3 arg1, Vector3 arg2)
    {
        if (!isPowerEnable ||
            Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 2)
            return false;

        if (!isCpu)
        {
            if ((playerPowerLock[(int) POWER.INVISABLE_PLAYER] < 1) ||
                mainPlayerLock)
                return false;

            //goalDownObstacles[0].SetActive(true);

            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //TOCHECK
            /////audioManager.Commentator_extraPower("wall");

            playerPowerLock[(int) POWER.INVISABLE_PLAYER]--;
            mainPlayerLock = true;

            StartCoroutine(makePlayerInvisible(
                    isCpu,                    
                    powerMaxTime[(int) POWER.INVISABLE_PLAYER]));
            StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.INVISABLE_PLAYER],
                                         powerButtonImg[powerIdx],
                                         powerButtons[powerIdx]));
        }
        else
        {
            if (cpuPowersUsed[(int) POWER.INVISABLE_PLAYER] ||
                cpuPowerLock ||
                (numOfPowersUsedCpu >= 3))
                return false;

            numOfPowersUsedCpu++;
            cpuPowerLock = true;
            audioManager.Play("extraPowerEnable");
            audioManager.Commentator_extraPower("com_extraPowerUsed_1");
            //audioManager.Commentator_extraPower("wall");

            cpuPowersUsed[(int) POWER.INVISABLE_PLAYER] = true;
            //goalUpObstacles[0].SetActive(true);

            StartCoroutine(makePlayerInvisible(
                   isCpu,
                   powerMaxTime[(int) POWER.INVISABLE_PLAYER]));
          //  StartCoroutine(fillAmountImg(powerMaxTime[(int) POWER.INVISABLE_PLAYER],
          //                               powerButtonImg[powerIdx],
          //                               powerButtons[powerIdx]));
            //StartCoroutine(deactiveGameObject(GOALS_OBSTACLES_TIME, goalUpObstacles[0], isCpu));
        }

        return true;
    }

    public bool getIsCameraUpShakeActive()
    {
        if (!isPowerEnable)
            return false;

        return isCameraUpShakeActive;
    }

    public bool getIsCameraDownShakeActive()
    {
        if (!isPowerEnable)
            return false;

        return isCameraDownShakeActive;
    }

    IEnumerator setCameraShake(bool isCpu, int cameraNum, float delay)
    {
        if (cameraNum == 1)
        {
            isCameraUpShakeActive = true;
        }
        else
        {
            isCameraDownShakeActive = true;
        }

        yield return new WaitForSeconds(delay);

        if (cameraNum == 1)
        {
            isCameraUpShakeActive = false;
        } else
        {
            isCameraDownShakeActive = false;
        }

        if (!isCpu)
            mainPlayerLock = false;
        else
            cpuPowerLock = false;
    }
}
