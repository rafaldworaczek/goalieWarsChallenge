using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalsNS;
using UnityEngine.SceneManagement;
using graphicsCommonNS;
using TMPro;
using UnityEngine.Analytics;
using LANGUAGE_NS;
using AudioManagerNS;

public enum BONUS_EVENT
{
    STRAIGHT_SHOT = 3,
    CURVE_SHOT = 5,
    LOB_SHOT = 9,
    VOLLEY_SHOT = 11,
    VOLLEY_LOB_SHOT = 13,
    GRASS_SHOT = 15,
    CROSSBAR_SHOT = 16,
    SHOT_10_SECONDS = 18
}

public class BonusRounds : MonoBehaviour
{
    public int SHOT_RANGE_MIN = 3;
    public int SHOT_RANGE_MAX = 22;
    public int GK_RANGE_MIN = 21;
    public int GK_RANGE_MAX = 40;
    private int MIN_GK_SAVES = 5;
    private float MAX_LEVEL_TIME = 5.0f;
    private AudioManager audioManager;
    private int gkSavesCounter = 0;    
    private int numberOfShotCounter = 0;
    private int NUMBER_OF_SHOT_MAX = 5;
    public Vector3 PLAYER_DEFUALT_POS = new Vector3(0f, 0f, -5f);
    public Vector3 GK_DEFUALT_POS = new Vector3(0f, 0f, -12.5f);
    public Vector3 BALL_DEFUALT_POS = new Vector3(0f, 0f, -4.5f);
    public Text trainingDialogText;
    private controllerRigid playerMainScript;
    private bool trainingPaused = true;
    private bool runPaused = true;
    private bool touchPaused = true;
    private bool rotationLocked = false;
    private bool shotTrainingActive = false;
    private bool shotBonusActive = false;
    private bool gkTrainingActive = false;
    private bool waitForEvent = false;
    private bool shotActive = false;
    private bool wasShotActive = false;
    private bool coroutineLocked = false;
    private bool nextTaskButtonOnClick = false;
    public GameObject traningPanel;
    public GameObject skipTraningPanel;
    public RawImage trainingIconLeft;
    private GameObject trailShoot;
    private Vector3 trailShootStartPos;
    private Vector3 trailShootMidPos;
    private Vector3 trailShootEndPos;
    private bool[] preEventInit;
    delegate bool TrainFuncDelegate();
    TrainFuncDelegate[] trainDialogFunc;
    delegate bool BonusFuncInitDelegate();
    BonusFuncInitDelegate[] bonusFuncInitDialogFunc;
    public GameObject drawPrefabShotTrail;
    public float curveTime = 0f;
    private bool ballPositionLock = false;
    private GraphicsCommon graphics = new GraphicsCommon();
    private bool wasTrainingActive = false;
    private float timeToFinishLevel = 0f;
    private GameObject trainingSkipButton;
    private GameObject trainingSkipButtonYes;
    private GameObject trainingSkipButtonNo;
    private float maxTimeToShot;
    private float shotTimer = 0f;
    private GameObject playerUp;
    private GameObject camera_button;
    public GameObject[] bonusDiamonds;
    public GameObject[] coinsDiamonds;
    private GameObject admobGameObject;
    private bool waitingForAdsEvent = true;
    private int numCoinsAward = 0;
    private int numDiamondsAward = 0;
    private admobAdsScript admobAdsScript;
    public GameObject bonusPanel;
    public TextMeshProUGUI bonusDescText;
    public TextMeshProUGUI tryAgainButtonText;
    public TextMeshProUGUI bonusHeaderText;
    public Button tryAgainButton;
    public RawImage bonusImg;
    public GameObject rewardAdsButton;
    public GameObject rewardAdsImg;
    private bool updateWaiting = true;
    //public GameObject wallGoalDown;
    //public GameObject wallGoalDownTop;
    public GameObject goalDown;
    private bool waitingForRewardAdEvent;
    public GameObject admobCanvas;
    public GameObject shopNotificationCanvas;
    public TextMeshProUGUI shopNotificationHeaderText;
    public TextMeshProUGUI shopNotificationText;
    public RawImage shopNotificationImage;
    public gamePausedMenu gamePausedScript;

    private string[] bonusImgNames =
    {
         //0
         "coinsDiamonds",          
         //1
         "goalCrossBar",
         //"goalCrossBar"
    };

    private int[] maxTimeToFinishLevel =
    {
         5,         
         5,
         50
    };

    private string[] bonusDialogMessages =
        {
         //0
         "if you hit either coin or diamond by ball you get extra bonus!",
         //1
         "Hit the crossbar by ball and extra bonus is your!"       
        };

    private int bonusTaskIdx = 0;

    void Awake()
    {
        //if (!Globals.isTrainingActive)
        //{
        //    skipTraningPanel.SetActive(false);
        //    return;
        //}

        //trainingSkipButton = GameObject.Find("trainingSkipButton");
        //trainingSkipButtonYes = GameObject.Find("trainingSkipButtonYes");
        //trainingSkipButtonNo = GameObject.Find("trainingSkipButtonNo");
  
        //wallGoalDown.SetActive(true);
        //wallGoalDownTop.SetActive(true);
        //goalDown.SetActive(false);


        //bonusTaskIdx = 2;

        //if (bonusTaskIdx == 2)
        //{
        //    playerMainScript.setNumberOfBalls(5);
        //}
    }

    void Start()
    {
        //print("#DBG ISBONUSACTIVE " + Globals.isBonusActive);

        if (!Globals.isBonusActive)
        {
            return;
        }

        adInit();
        audioManager = FindObjectOfType<AudioManager>();
        playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
        bonusTaskIdx =
            UnityEngine.Random.Range(0, bonusImgNames.Length);

        //print("BONUSTASKIDX " + bonusTaskIdx + " len " + bonusImgNames.Length);

        tryAgainButton.onClick.RemoveAllListeners();
        tryAgainButton.onClick.AddListener(
                delegate { onClickFirstPlay(); });

        //bonusTaskIdx = 2;

        playerUp = GameObject.Find("playerUp");
        //camera_button = GameObject.Find("camera_button");

        playerUp.SetActive(false);
        //camera_button.SetActive(false);
        //goalDown = GameObject.Find("goalDown");
        //wasTrainingActive = Globals.isTrainingActive;
        //if (!wasTrainingActive)
        //    return;

//        print("TRAINING1");

        maxTimeToShot = playerMainScript.getMaxTimeToShot();

        //trainingSkipButtonYes.SetActive(false);
        //trainingSkipButtonNo.SetActive(false);

        //initTrailShot();

        //print("TRAINING2");

        //playerMainScript.deactivateCanvasElements();
        //if (!traningPanel.activeSelf)
        //    traningPanel.SetActive(true);

        //print("TRAINING3");

        trainDialogFunc = new TrainFuncDelegate[bonusDialogMessages.Length];
        trainDialogFunc[0] = isHitMoney;
        trainDialogFunc[1] = isHitUpCrossBar;
       
        bonusFuncInitDialogFunc = new BonusFuncInitDelegate[bonusDialogMessages.Length];
        bonusFuncInitDialogFunc[0] = hitMoneyInit;
        bonusFuncInitDialogFunc[1] = hitCrossBarInit;

        preEventInit = new bool[bonusDialogMessages.Length];

        rewardAdsImg.SetActive(false);
        bonusPanel.SetActive(true);
        bonusDescText.text =
            Languages.getTranslate(
                bonusDialogMessages[bonusTaskIdx]);       
        bonusImg.texture =
                Resources.Load<Texture2D>("BonusRounds/" + bonusImgNames[bonusTaskIdx]);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!wasTrainingActive)
        //    return;
        adsCheck();

        if (updateWaiting)
            return;

        timeToFinishLevel += Time.deltaTime;
        int timeToFinish = (int) ((float) maxTimeToFinishLevel[bonusTaskIdx] - timeToFinishLevel);
        trainDialogFunc[bonusTaskIdx]();
        playerMainScript.setTimeToShotText(Mathf.Max(0,timeToFinish).ToString());
        //if (trainDialogFunc[bonusTaskIdx]() &&
        //    waitForEvent)
        //{
        /*Button has not been really clicked. it is kinda asynchronous callback*/
        //nextTaskButtonClicked();
        //}

        //drawShotTrail();
    }

    private void adInit()
    {
        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();
        admobAdsScript.setAdsClosed(false);
        admobAdsScript.setAdsFailed(false);
        admobAdsScript.setAdsRewardEarn(false);
        admobAdsScript.hideBanner();
    }

    private void adsCheck()
    {
        /*print("ADSDEBUG waitingForAddEvent " + waitingForRewardAdEvent
           + " admobAdsScript.getAdsClosed() " + admobAdsScript.getAdsClosed()
           + " admobAdsScript.getAdsFailed() " + admobAdsScript.getAdsFailed()
           + " admobAdsScript.getAdsRewardEarn() " + admobAdsScript.getAdsRewardEarn());*/

        if (waitingForRewardAdEvent &&
           (admobAdsScript.getAdsClosed() ||
            admobAdsScript.getAdsFailed() ||
            admobAdsScript.getAdsRewardEarn()))
        {
            waitingForRewardAdEvent = false;

            if (admobAdsScript.getAdsRewardEarn())
            {
                //start game again
                StartCoroutine(startLevel(1.5f));
            }
            else
            {
                //reward ad  failed
                shopNotificationCanvas.SetActive(true);
                shopNotificationHeaderText.text =
                    Languages.getTranslate("Ads failed");
                shopNotificationText.text = "";
                shopNotificationImage.texture =
                         Resources.Load<Texture2D>("Shop/showNotificationAdsFailed");
            }

            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsRewardEarn(false);
            admobCanvas.SetActive(false);
            gamePausedScript.ResumeButton();
            //initPauseCanvas();
        }
    }

    private IEnumerator startLevel(float delayTime)
    {
        //give time to prepare
        bonusFuncInitDialogFunc[bonusTaskIdx]();
        bonusPanel.SetActive(false);
        updateWaiting = true;
        runPaused = true;
        timeToFinishLevel = 0f;
        touchPaused = true;
        wasShotActive = false;
        shotActive = false;
        timeToFinishLevel = 0f;
        coroutineLocked = false;
        rotationLocked = false;
        preEventInit[bonusTaskIdx] = false;

        playerMainScript.resetDrawOnScreen();

        yield return new WaitForSeconds(delayTime);
        audioManager.PlayNoCheck("whislestart1");
        updateWaiting = false;
    }

    private void drawShotTrail()
    {
        if ((bonusTaskIdx == (int)BONUS_EVENT.CURVE_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.LOB_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.STRAIGHT_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.VOLLEY_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.GRASS_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.CROSSBAR_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.VOLLEY_LOB_SHOT ||
             bonusTaskIdx == (int)BONUS_EVENT.SHOT_10_SECONDS) &&
             waitForEvent &&
             !coroutineLocked &&
             !playerMainScript.isShotActive() &&
             !playerMainScript.isPreShotActive() &&
             !traningPanel.activeSelf)
        {
            if (curveTime == 0f)
            {
                initTrailShot();
                trailShootStartPos = playerMainScript.getPlayerPosition();
                trailShootEndPos =
                    new Vector3(-4.5f, 2f, playerMainScript.PITCH_HEIGHT_HALF);

                if (bonusTaskIdx == (int)BONUS_EVENT.GRASS_SHOT)
                    trailShootEndPos =
                        new Vector3(-4.5f, 0f, playerMainScript.PITCH_HEIGHT_HALF - 3f);

                if (bonusTaskIdx == (int)BONUS_EVENT.CROSSBAR_SHOT)
                    trailShootEndPos =
                        new Vector3(4.5f, 3.0f, playerMainScript.PITCH_HEIGHT_HALF);

                /*STRAIGHT SHOT */
                trailShootMidPos = new Vector3(
                    ((trailShootStartPos.x + trailShootEndPos.x) / 2f),
                    ((trailShootEndPos.y + trailShootEndPos.y) / 2f),
                    ((trailShootStartPos.z + trailShootEndPos.z) / 2f));

                if (bonusTaskIdx == (int)BONUS_EVENT.CURVE_SHOT)
                {
                    trailShootMidPos =
                        new Vector3(-10f, 1f, playerMainScript.PITCH_HEIGHT_HALF / 2.0f);
                }
            }

            drawTrailShootCurve(trailShootStartPos,
                                trailShootMidPos,
                                trailShootEndPos,
                                ref curveTime);
        }
        else
        {
            curveTime = 0f;
        }
    }

    private void clearVariables()
    {
        curveTime = 0f;
        shotActive = false;
        touchPaused = false;
        trainingPaused = false;
        if (!shotTrainingActive)
            runPaused = false;
        wasShotActive = false;
        coroutineLocked = false;
        traningPanel.SetActive(false);
        playerMainScript.setTimeToShot(0f);
        gkSavesCounter = 0;
    }
    
    private bool levelTimeExceeded()
    {
        return (timeToFinishLevel > maxTimeToFinishLevel[bonusTaskIdx]) ? true : false;
    }

    private bool levelTimeExceeded(float max)
    {
        return (timeToFinishLevel > max) ? true : false;
    }

    
    public bool noOperation()
    {
        return true;
    }

    public bool isTraningPaused()
    {
        return trainingPaused;
    }

    public bool isRunPaused()
    {
        return runPaused;
    }

    public bool isTouchPaused()
    {
        return touchPaused;
    }

    public bool isRotationLocked()
    {
        return rotationLocked;
    }

    private void setPlayerPos(Vector3 pos)
    {
        playerMainScript.setPlayerPos(pos);
    }

    private void showTimeToShot()
    {
        playerMainScript.timePanelGameObject.SetActive(true);
        Color fadeColor = Color.white;
        fadeColor.a = 0f;
        playerMainScript.timePanelGameObject.GetComponent<Image>().color = fadeColor;
        playerMainScript.mainTimeTextGameObject.SetActive(false);
        playerMainScript.timeToShotBallImageGameObject.SetActive(false);
    }

    private void hideTimePanel()
    {
        playerMainScript.timePanelGameObject.SetActive(false);
    }

    IEnumerator posChangeFlash(float delayTime)
    {
        float offset = 0.2f;

        //print("POSCHANGEFLASH ");
        ballPositionLock = true;

        //Color fadeColor = new Color();
        //ColorUtility.TryParseHtmlString("#777E75", out fadeColor);
        Color fadeColor = new Color();
        ColorUtility.TryParseHtmlString("#C36868", out fadeColor);
        //Color fadeColor = Color.black;
        fadeColor.a = 0.6f;
        for (int i = 0; i < 1; i++)
        {
            fadeColor.a += 0.1f;
            playerMainScript.flashBackgroundImage.color = fadeColor;
            yield return new WaitForSeconds(delayTime);
        }

        //setPlayerWithBallDefaultPos();
        fadeColor.a = 0.0f;
        playerMainScript.flashBackgroundImage.color = fadeColor;

        ballPositionLock = false;
    }

    private bool hitMoneyInit()
    {
        float randPosZ = UnityEngine.Random.Range(6f, 8f);
        float randPosX = UnityEngine.Random.Range(
            -(playerMainScript.getPitchWidth() - 8), (playerMainScript.getPitchWidth() - 8));
        setPlayerPos(new Vector3(randPosX, 0f, -randPosZ));

        playerMainScript.RblookAtWSPoint(playerMainScript.getRb(),
                                         new Vector3(0f, 0f, playerMainScript.PITCH_HEIGHT_HALF),
                                         1f);

        playerMainScript.ball[1].setBallGoalCollisionStatus(false);
        playerMainScript.stopBallVel();
        playerMainScript.setUpdateBallPosActive(false);
        playerMainScript.setBallPosition(
            playerMainScript.getPlayerPosition() +
            (playerMainScript.getRb().transform.forward / 1.4f));


       


        Vector3 getGoalSizePlr2 =
            playerMainScript.getGoalSizePlr2();

        for (int i = 0; i < bonusDiamonds.Length; i++)
        {
            bonusDiamonds[i].SetActive(true);
            bonusDiamonds[i].transform.position = new Vector3(
                 UnityEngine.Random.Range(
                     -(getGoalSizePlr2.x - 1f), (getGoalSizePlr2.x - 1f)),
                 UnityEngine.Random.Range(1f, (getGoalSizePlr2.y - 1f)),
                playerMainScript.PITCH_HEIGHT_HALF);
        }

        for (int i = 0; i < coinsDiamonds.Length; i++)
        {
            coinsDiamonds[i].SetActive(true);
            coinsDiamonds[i].transform.position = new Vector3(
               UnityEngine.Random.Range(
                 -Mathf.Sign(bonusDiamonds[i].transform.position.x) * (getGoalSizePlr2.x - 1f), 0f),
                 UnityEngine.Random.Range(1f, (getGoalSizePlr2.y - 1f)),
                 playerMainScript.PITCH_HEIGHT_HALF);
        }

        //camera_button.GetComponent<buttonCamera>().setCameraIdx(2);

        //                 new Vector3(0f, 0f, Mathf.Abs(playerMainScript.getPitchHeight())));

        //playerMainScript.setBallPosition(
        //  new Vector3(0f, 0.3f, -(randPos - 0.5f)));

        if (playerUp.activeSelf)
            playerUp.SetActive(false);

        playerMainScript.ball[1].setBonusDiamonHit(false);
        playerMainScript.ball[1].setBonusCoinHit(false);

        return true;
    }

    private bool hitCrossBarInit()
    {
        float randPosZ = UnityEngine.Random.Range(6f, 8f);
        float randPosX = UnityEngine.Random.Range(
            -(playerMainScript.getPitchWidth() - 8), (playerMainScript.getPitchWidth() - 8));
        setPlayerPos(new Vector3(randPosX, 0f, -randPosZ));

        playerMainScript.RblookAtWSPoint(playerMainScript.getRb(),
                                              new Vector3(0f, 0f, playerMainScript.PITCH_HEIGHT_HALF));

        playerMainScript.ball[1].setBallGoalCollisionStatus(false);

        playerMainScript.stopBallVel();
        playerMainScript.setUpdateBallPosActive(false);
        playerMainScript.setBallPosition(
            playerMainScript.getPlayerPosition() +
            (playerMainScript.getRb().transform.forward / 1.4f));        

        touchPaused = false;
        rotationLocked = true;

        Vector3 getGoalSizePlr2 =
            playerMainScript.getGoalSizePlr2();

        //camera_button.GetComponent<buttonCamera>().setCameraIdx(2);

        //                 new Vector3(0f, 0f, Mathf.Abs(playerMainScript.getPitchHeight())));

        //playerMainScript.setBallPosition(
        //  new Vector3(0f, 0.3f, -(randPos - 0.5f)));

        if (playerUp.activeSelf)
            playerUp.SetActive(false);

        playerMainScript.ball[1].setGoalUpCrossBarJustHit(false);
        //goalDown.SetActive(false);

        return true;
    }

    private void setPlayerWithBallDefaultPos()
    {
        playerMainScript.setBallInFrontOfRb(
            playerMainScript.getPlayerRb(),
            BALL_DEFUALT_POS);
        playerMainScript.setUpdateBallPosActive(false);
        playerMainScript.stopBallVel();
        setPlayerDefaultRotation();
        setPlayerDefaultPos();
    }

    private void setGkWithBallDefaultPos()
    {
        setPlayerDefaultRotation();
        setPlayerPos(GK_DEFUALT_POS);
        playerMainScript.stopBallVel();
        playerMainScript.setBallPosition(
                           new Vector3(UnityEngine.Random.Range(-17, 17f),
                           0f,
                           UnityEngine.Random.Range(3, 9f)));
        playerMainScript.setUpdateBallPosActive(false);
    }

    private void setPlayerDefaultRotation()
    {
        playerMainScript.setPlayerRotation(Vector3.zero);
    }

    private void setPlayerDefaultPos()
    {
        playerMainScript.setPlayerPos(PLAYER_DEFUALT_POS);
    }
   
    public bool isHitMoney()
    {
        //different distance different number of points

        //print("TRAINDIALOG EXEC SHOT DONE ");

        bool isShotActive = playerMainScript.isShotActive();

        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (playerMainScript.isBallOutOfPitch())
        {
            isShotActive = false;
            isPreShotActive = false;
        }

        bool crossBarHited = false;

        if (!preEventInit[bonusTaskIdx])
        {

            //goalDown.SetActive(false);
            touchPaused = false;
            rotationLocked = true;
            preEventInit[bonusTaskIdx] = true;
            showTimeToShot();
            goalDown.SetActive(false);
        }

        //print("isHitMoney 1");

        if (isShotActive)
            wasShotActive = true;

        if (isShotActive || isPreShotActive)
        {
            rotationLocked = false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        if (coroutineLocked)
            return false;
       
        if ((playerMainScript.ball[1].getBonusDiamondHit() ||
             playerMainScript.ball[1].getBonusCoinHit()))
        {
            if (playerMainScript.ball[1].getBonusDiamondHit() &&
                playerMainScript.ball[1].getBonusCoinHit())
            {
                bonusDiamonds[0].SetActive(false);
                coinsDiamonds[0].SetActive(false);
                bonusDescText.text = Languages.getTranslate(
                    "Congratulations!\nYou just earned 20 coins + 20 diamonds!");
            }
            else if (playerMainScript.ball[1].getBonusDiamondHit())
            {
                bonusDiamonds[0].SetActive(false);
                bonusDescText.text = 
                    Languages.getTranslate("Congratulations!\nYou just earned 20 diamonds!");
            } else if (playerMainScript.ball[1].getBonusCoinHit())
            {
                coinsDiamonds[0].SetActive(false);
                bonusDescText.text = 
                    Languages.getTranslate("Congratulations!\nYou just earned 20 coins!");
            }

            if (wasShotActive && !isShotActive)
            {
                if (playerMainScript.ball[1].getBonusDiamondHit() &&
                    playerMainScript.ball[1].getBonusCoinHit())
                {                   
                    Globals.addDiamonds(20);
                    Globals.addCoins(20);
                }
                else if (playerMainScript.ball[1].getBonusDiamondHit())
                {
                    Globals.addDiamonds(20);
                }
                else if (playerMainScript.ball[1].getBonusCoinHit())
                {
                    Globals.addCoins(20);
                }

                StartCoroutine(levelDone(bonusTaskIdx, 1f));
            }
            return false;
        }

        //if (isShotActive ||
        //    isPreShotActive)
        //    return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        //print("timeExceeded " + timeExceeded);

        if (wasShotActive ||
            timeExceeded)
        {
            StartCoroutine(levelNotDone(bonusTaskIdx,
                "", delay));
        }

        return false;
    }

    public bool isHitUpCrossBar()
    {
        //print("TRAINDIALOG EXEC SHOT DONE ");

        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (playerMainScript.isBallOutOfPitch())
        {
            isShotActive = false;
            isPreShotActive = false;
        }

        if (!preEventInit[bonusTaskIdx])
        {

            //goalDown.SetActive(false);
            touchPaused = false;
            rotationLocked = true;
            preEventInit[bonusTaskIdx] = true;
            showTimeToShot();
            goalDown.SetActive(false);
        }

        if (isShotActive)
            wasShotActive = true;

        if (isShotActive || isPreShotActive)
        {
            rotationLocked = false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        if (coroutineLocked)
            return false;

        //print("getGoalUpCrossBarJustHit " + playerMainScript.ball[1].getGoalUpCrossBarJustHit());
        if (playerMainScript.ball[1].getGoalUpCrossBarJustHit())
        {
            if (wasShotActive && !isShotActive)
            {
                Globals.addDiamonds(15);
                Globals.addCoins(15);
                bonusDescText.text = 
                    Languages.getTranslate("Congratulations!\nYou just earned 15 coins + 15 diamonds!");
                StartCoroutine(levelDone(bonusTaskIdx, 1f));
            }

            return false;
        }

        //if (isShotActive ||
        //    isPreShotActive)
        //    return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        //print("timeExceeded " + timeExceeded);

        if (wasShotActive ||
            timeExceeded)
        {
            StartCoroutine(levelNotDone(bonusTaskIdx,
                "", delay));
        }

        return false;


    }

    void LateUpdate()
    {

    }
            
    IEnumerator levelDone(int index, float delayTime)
    {
        coroutineLocked = true;
        touchPaused = true;
        yield return new WaitForSeconds(delayTime);
        preEventInit[index] = false;
        updateWaiting = true;
        //bonusDescText.text = "Congratulations! \nYou just earned 20 coins + 20 diamonds!";
        bonusHeaderText.text = Languages.getTranslate("Congratulations!");
        showBonusPanel();
        audioManager.PlayNoCheck("moneySound");
        //trainDialogFunc[index] = noOperation;
        preEventInit[index] = false;
        wasShotActive = false;
        shotActive = false;
        timeToFinishLevel = 0f;
        coroutineLocked = false;
        rotationLocked = false;
        touchPaused = false;
        playerMainScript.setTimeToShot(0f);

        //endOfBonus();
    }

    IEnumerator levelNotDone(int index, string text, float delayTime)
    {
        coroutineLocked = true;
        touchPaused = true;
        yield return new WaitForSeconds(delayTime);
        updateWaiting = true;

        bonusDescText.text = Languages.getTranslate("SORRY\nNO EXTRA COINS THIS TIME...\n" +
            "TRY AGAIN! GOOD LUCK!");
        showBonusPanel();

        preEventInit[index] = false;
        wasShotActive = false;
        shotActive = false;
        timeToFinishLevel = 0f;
        coroutineLocked = false;
        rotationLocked = false;
        touchPaused = false;
        playerMainScript.setTimeToShot(0f);
    }
      
    private void showBonusPanel() {
        bonusPanel.SetActive(true);
        tryAgainButtonText.text = 
            Languages.getTranslate("TRY AGAIN");

        tryAgainButton.onClick.RemoveAllListeners();
        tryAgainButton.onClick.AddListener(
                delegate { onClickWatchRewardAdButton(); });

        rewardAdsImg.SetActive(true);
        hideTimePanel();
    }

    private void showPanelWithText(string text)
    {
        playerMainScript.deactivateCanvasElements();

        if (!traningPanel.activeSelf)
            traningPanel.SetActive(true);
        trainingDialogText.text = 
            Languages.getTranslate(text);
        touchPaused = true;
        runPaused = true;
        playerMainScript.joystick.zeroPosition();
    }

    public void initTrailShot()
    {
        trailShoot =
           (GameObject)Instantiate(drawPrefabShotTrail, Vector3.zero, Quaternion.identity);
        trailShoot.GetComponent<TrailRenderer>().sortingOrder = 1;
    }

    public void initTrailShot(Vector3 startPos, Vector3 endPos)
    {
        trailShoot =
            (GameObject)Instantiate(drawPrefabShotTrail, Vector3.zero, Quaternion.identity);
        trailShoot.GetComponent<TrailRenderer>().sortingOrder = 1;
        //trailShootStartPos = startPos;
        //trailShootEndPos = endPos;
        //trailShoot.transform.position = trailShootStartPos;
    }

    public bool isShotBonusActive()
    {
        return shotBonusActive;
    }

    public bool isGkTraining()
    {
        return gkTrainingActive;
    }

    public void drawATrailShootInit()
    {

    }

    public void drawATrailShootStraight()
    {
        //print("CAMERAXYz Camera.main.transform.forward * -1 " + Camera.main.transform.forward * -1);
        //print("CAMERAXYz eulerAngles " + Camera.main.transform.eulerAngles);

        Vector3 trailEndDirection =
            (trailShootEndPos - trailShoot.transform.position).normalized * 0.2f;

        Vector3 trailPos = convertWStoScreenPlane(
            trailShoot.transform.position + trailEndDirection);

        if (trailPos != Vector3.zero)
            trailShoot.transform.position = trailPos;

    }

    private void drawTrailShootCurve(Vector3 startPos,
                                     Vector3 midPos,
                                     Vector3 endPos,
                                     ref float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
        Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
        Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

        Vector3 trailPos = convertWStoScreenPlane(currPos);

        if (trailPos != Vector3.zero)
            trailShoot.transform.position = trailPos;

        //print("CURVE trailShoot.transform.position " + trailShoot.transform.position 
        //    + " TIME " + currentTime);
        currentTime += 0.01f;
        if (currentTime >= 1.0f)
        {
            currentTime = 0f;
            initTrailShot();
        }
    }

    private Vector3 convertWStoScreenPlane(Vector3 posWS)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(posWS);

        Plane cameraPlane = new Plane(
                    Camera.main.transform.forward * -1, trailShoot.transform.position);
        Ray mRay = Camera.main.ScreenPointToRay(screenPos);
        float rayDistance;
        if (cameraPlane.Raycast(mRay, out rayDistance))
        {
            Vector3 lineEndPos = mRay.GetPoint(rayDistance);
            return lineEndPos;
            //print("TRAILSHOTPOS22 " + trailShoot.transform.position);
        }

        return Vector3.zero;
    }

    public void onClickClose()
    {
        endOfBonus();
    }

    public void onClickFirstPlay()
    {
        StartCoroutine(startLevel(0.8f));
    }

    private bool endOfBonus()
    {
        Globals.recoverOriginalResolution();
        Globals.isTrainingActive = false;

        //Globals.isBonusFinished = true;
        //Globals.isBonusActive = false;

        if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return false;
        }

        //if (Globals.isFriendly)
            SceneManager.LoadScene("gameLoader");
        //else
        //{
        //    SceneManager.LoadScene("Leagues");
        //}

        return false;
    }

    public void skipTrainingButton()
    {
        //print("SKIPTRAININGBUTTON CLICKED");
        showSkipTrainingButtons();
    }

    public void skipTrainingButtonYes()
    {
        endOfBonus();
    }

    public void skipTrainingButtonNo()
    {
        deactivateTrainingSkipButtons();
    }

    private void activateTrainingSkipButtons()
    {
        /*trainingSkipButton.SetActive(false);
        trainingSkipButtonYes.SetActive(true);
        trainingSkipButtonNo.SetActive(true);*/
    }

    private void deactivateTrainingSkipButtons()
    {
        /*trainingSkipButtonYes.SetActive(false);
        trainingSkipButtonNo.SetActive(false);
        trainingSkipButton.SetActive(true);*/
    }

    private void showSkipTrainingButtons()
    {
        activateTrainingSkipButtons();
    }

    public void onClickWatchRewardAdButton()
    {
        //print("ADONCLICK");

        //waitingForRewardAdEvent = true;
        //admobAdsScript.setAdsRewardEarn(true);

        //return;

        if (admobAdsScript.showRewardAd())
        {
            waitingForRewardAdEvent = true;           
            admobCanvas.SetActive(true);
        }
        else
        {
            waitingForRewardAdEvent = false;
            admobCanvas.SetActive(false);

            if (Globals.isAnalyticsEnable)
            {
                AnalyticsResult analyticsResult =
                    Analytics.CustomEvent("REWARDS_ADS_ONLCICK_EVENT_BONUS_LOAD_FAILED", new Dictionary<string, object>
                {
                    { "REWARDS_ADS_BUTTON_BONUS_CLICKED_FAILED", true},
                });
            }
        }

        admobAdsScript.setAdsClosed(false);
        admobAdsScript.setAdsFailed(false);
        admobAdsScript.setAdsRewardEarn(false);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("REWARDS_ADS_ONLCICK_EVENT_BONUS", new Dictionary<string, object>
            {
                    { "REWARDS_ADS_BUTTON_BONUS_CLICKED", true},
            });
        }
    }

    public void onClickShopNotificationClose()   
    {
        shopNotificationCanvas.SetActive(false);
    }
}

