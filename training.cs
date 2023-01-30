using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalsNS;
using UnityEngine.SceneManagement;
using graphicsCommonNS;
using LANGUAGE_NS;

public enum TRAINING_EVENT
{  
    STRAIGHT_SHOT = 3,
    CURVE_SHOT = 5,
    LOB_SHOT = 9,
    VOLLEY_SHOT = 11 ,
    VOLLEY_LOB_SHOT = 13,
    GRASS_SHOT = 15,
    CROSSBAR_SHOT = 16,
    SHOT_10_SECONDS = 18
}

public class training : MonoBehaviour
{
    public int SHOT_RANGE_MIN = 3;
    public int SHOT_RANGE_MAX = 22;
    public int GK_RANGE_MIN = 21;
    public int GK_RANGE_MAX = 40;
    private int MIN_GK_SAVES = 3;
    private float MAX_LEVEL_TIME = 8.5f;
    private int gkSavesCounter = 0;
    public Vector3 PLAYER_DEFUALT_POS = new Vector3(0f, 0f, -5f);
    public Vector3 GK_DEFUALT_POS = new Vector3(0f, 0f, -12.5f);
    public Vector3 BALL_DEFUALT_POS = new Vector3(0f, 0f, -4.5f);
    public Text trainingDialogText;
    private controllerRigid playerMainScript;
    private bool trainingPaused = true;
    private bool runPaused = true;
    private bool touchPaused = true;
    private bool shotTrainingActive = false;
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
    delegate bool TrainFuncDelegate();
    TrainFuncDelegate[] trainDialogFunc;
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
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    private bool waitingForInterstitialAddEvent = false;
    public GameObject admobCanvas;
    public GameObject pauseCanvas;
    public gamePausedMenu GamePauseScript;
    private string[] trainingDialogMessages =
        {
         //0
         "Welcome to the training mode. I am going to show you how to play and win!",          
         //1
         "You move, score and defend from your own half. You cannot cross the halfway line",
         //2
         "Run to the ball now. Use the joystick in the bottom corner",
         //3
         "Brilliant! When you have the ball, you can shoot. Draw a straight line as shown",
         //4
         "Great! Good shot!",
         //5
         "Now try to curl a shot by drawing a curved line",
         //6
         "Perfect shot!. The more you curve the line, the more the shot will curve",
         //7
         "The faster you draw, the higher the ball speed",            
          //This is shown in the top-right corner",
         //8       
         "There are 2 buttons in the bottom corner - V (volley) and L (lob) shot",
         //9
         "Click the (L) lob button and draw a line",
         //10          
         "Perfect shot!",
         //11
         "Now click the (V) volley button and draw a line",
         //12
         "Great volley!",
         //13
         "You can use volley and lob together, take a curve shot or shoot straight at the goal",
         //14
         "Brilliant! ",
         //15
         "To shoot along the ground, draw a line on the grass as shown",
         //16
         "The higher you draw, the higher the ball will go. Draw a line close to the crossbar",
         //17
         "Great shot! You are a fantastic striker!",
         //18
         "You have 8 seconds to take a shot. You can see the timer at the top",
         //19 
         "You have to take a shot within 8 seconds. Otherwise your opponent gets the ball",
         //20
         "Excellent! You know how to shoot. I am now going to teach you how to defend",
         //21
         "When the ball is heading towards your goal, click the circle to try to save it",
         //22
         "You can adjust your goalkeeper's position using the buttons next to the joystick",
         //23
         "Now try a two minute training match",
         //24
         "Well done!. That is the end of the training. Good luck in the tournaments!",
         //25
         "Well done!. That is the end of the training. Good luck in the tournaments!"
        };

    private int trainingTaskIdx = 0;

    void Awake()
    {
        if (!Globals.isTrainingActive)
        {
            skipTraningPanel.SetActive(false);
            return;
        }

        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

        trainingSkipButton = GameObject.Find("trainingSkipButton");
        trainingSkipButtonYes = GameObject.Find("trainingSkipButtonYes");
        trainingSkipButtonNo = GameObject.Find("trainingSkipButtonNo");
    }

    void Start()
    {
        playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();

        wasTrainingActive = Globals.isTrainingActive;
        if (!wasTrainingActive)
            return;

        maxTimeToShot = playerMainScript.getMaxTimeToShot();

        trainingSkipButtonYes.SetActive(false);
        trainingSkipButtonNo.SetActive(false);

        initTrailShot();

        playerMainScript.deactivateCanvasElements();
        if (!traningPanel.activeSelf)
            traningPanel.SetActive(true);

        trainDialogFunc = new TrainFuncDelegate[trainingDialogMessages.Length];
        trainDialogFunc[0] = trainDialogFunc[1] = noOperation;
        trainDialogFunc[2] = isPlayerOnBall;
        trainDialogFunc[3] = isStraightShotDone;
        trainDialogFunc[4] = noOperation;
        trainDialogFunc[5] = isCurveShotDone;
        trainDialogFunc[6] = noOperation;
        trainDialogFunc[7] = noOperation;
        trainDialogFunc[8] = noOperation;
        trainDialogFunc[9] = isLobShotDone;
        trainDialogFunc[10] = noOperation;
        trainDialogFunc[11] = isVolleyShotDone;
        trainDialogFunc[12] = noOperation;
        trainDialogFunc[13] = isVolleyLobShotDone;
        trainDialogFunc[14] = noOperation;
        trainDialogFunc[15] = isGrassShotDone;
        trainDialogFunc[16] = isCrossBarShotDone;
        trainDialogFunc[17] = noOperation;
        trainDialogFunc[18] = timeToShot;
        trainDialogFunc[19] = noOperation;
        trainDialogFunc[20] = noOperation;
        trainDialogFunc[21] = gkTraning;
        trainDialogFunc[22] = gkTraning;
        trainDialogFunc[23] = shotTraningAfter;
        trainDialogFunc[24] = noOperation;
        trainDialogFunc[25] = endOfTraining;

        trainingDialogText.text =
            Languages.getTranslate(trainingDialogMessages[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!wasTrainingActive)
            return;

        timeToFinishLevel += Time.deltaTime;
        if (trainDialogFunc[trainingTaskIdx]() &&
            waitForEvent)
        {
            /*Button has not been really clicked. it is kinda asynchronous callback*/
            nextTaskButtonClicked();
        }

        if (waitingForInterstitialAddEvent &&
           (admobAdsScript.getAdsClosed() ||
            admobAdsScript.getAdsFailed()))
        {
            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsClosed(false);
            admobCanvas.SetActive(false);
            waitingForInterstitialAddEvent = false;
            GamePauseScript.ResumeButton();
        }

        drawShotTrail();
    }

    private void drawShotTrail()
    {
        if ((trainingTaskIdx == (int)TRAINING_EVENT.CURVE_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.LOB_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.STRAIGHT_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.VOLLEY_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.GRASS_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.CROSSBAR_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.VOLLEY_LOB_SHOT ||
             trainingTaskIdx == (int)TRAINING_EVENT.SHOT_10_SECONDS) &&
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

                if (trainingTaskIdx == (int)TRAINING_EVENT.GRASS_SHOT)
                    trailShootEndPos =
                        new Vector3(-4.5f, 0f, playerMainScript.PITCH_HEIGHT_HALF - 3f);

                if (trainingTaskIdx == (int)TRAINING_EVENT.CROSSBAR_SHOT)
                    trailShootEndPos =
                        new Vector3(4.5f, 3.0f, playerMainScript.PITCH_HEIGHT_HALF);

                /*STRAIGHT SHOT */
                trailShootMidPos = new Vector3(
                    ((trailShootStartPos.x + trailShootEndPos.x) / 2f),
                    ((trailShootEndPos.y + trailShootEndPos.y) / 2f),
                    ((trailShootStartPos.z + trailShootEndPos.z) / 2f));

                if (trainingTaskIdx == (int)TRAINING_EVENT.CURVE_SHOT)
                {
                    trailShootMidPos =
                        new Vector3(-10f, 1f, playerMainScript.PITCH_HEIGHT_HALF / 2.0f);
                }
            }

            drawTrailShootCurve(trailShootStartPos,
                                trailShootMidPos,
                                trailShootEndPos,
                                ref curveTime);
        } else
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

    public void nextLevel()
    {
        trainingTaskIdx++;

        if (trainingTaskIdx >= SHOT_RANGE_MIN)
            shotTrainingActive = true;

        if (trainingTaskIdx == GK_RANGE_MIN)
        {
            gkTrainingActive = true;
        }

        if (trainingTaskIdx >= 24)
            playerMainScript.setGameEnded(true);

        if (shotTrainingActive)
        {
            setPlayerWithBallDefaultPos();
        }

        if (gkTrainingActive)
        {
            setGkWithBallDefaultPos();
        }

        showPanelWithText(
            Languages.getTranslate(trainingDialogMessages[trainingTaskIdx]));
        changeTrainingIcon();
        nextTaskButtonOnClick = false;
        gkSavesCounter = 0;
    }

    private bool levelTimeExceeded()
    {
        return (timeToFinishLevel > MAX_LEVEL_TIME) ? true : false;
    }

    private bool levelTimeExceeded(float max)
    {
        return (timeToFinishLevel > max) ? true : false;
    }

    public void nextTaskButtonClicked()
    {
        timeToFinishLevel = 0f;
        nextTaskButtonOnClick = true;

        if (trainDialogFunc[trainingTaskIdx]())
        {
            nextLevel();
            waitForEvent = false;
        }
        else
        {
            clearVariables();

            if (traningPanel.activeSelf)
                traningPanel.SetActive(false);

            trainingActivateCanvasElement();
            waitForEvent = true;
        }

        //print("timeToFinishLevel BEFORE playerOnBall BUTTON CLICKED " + timeToFinishLevel);
        //print("timeToFinishLevel AFTER playerOnBall BUTTON CLICKED " + timeToFinishLevel);
    }

    private void changeTrainingIcon()
    {
        switch (trainingTaskIdx)
        {
            case 2:
                trainingIconLeft.texture =
                     graphics.getTexture("Training/runIcon");
                break;

            case 3:
                trainingIconLeft.texture =
                     graphics.getTexture("Training/timeToShotImage");
                break;

            case 20:
                trainingIconLeft.texture =
                  graphics.getTexture("Training/glove1");
                break;

            case 23:
                trainingIconLeft.texture =
                  graphics.getTexture("Training/tshirt5");
                break;

            default:
                break;
        }
    }

    private void trainingActivateCanvasElement()
    {
        if (trainingTaskIdx == 2 ||
            trainingTaskIdx >= 20)
        {
            playerMainScript.joystickGameObject.SetActive(true);
            playerMainScript.joystickBgGameObject.SetActive(true);
            playerMainScript.gkClickHelperGameObject.SetActive(true);
            playerMainScript.gkHelperImageGameObject.SetActive(true);
            playerMainScript.joystick.zeroPosition();
        }

        if (trainingTaskIdx == 9) {
            playerMainScript.joystickGameObject.SetActive(true);
            playerMainScript.gkClickHelperGameObject.SetActive(false);
            playerMainScript.gkHelperImageGameObject.SetActive(false);
            playerMainScript.joystickBgGameObject.SetActive(false);
            playerMainScript.volleyButtonGameObject.SetActive(false);
            playerMainScript.lobButtonGameObject.SetActive(true);
        }

        if (trainingTaskIdx == 11)
        {
            playerMainScript.joystickGameObject.SetActive(true);
            playerMainScript.gkClickHelperGameObject.SetActive(false);
            playerMainScript.gkHelperImageGameObject.SetActive(false);
            playerMainScript.joystickBgGameObject.SetActive(false);
            playerMainScript.lobButtonGameObject.SetActive(false);
            playerMainScript.volleyButtonGameObject.SetActive(true);
        }

        if (trainingTaskIdx == 13)
        {
            playerMainScript.joystickGameObject.SetActive(true);
            playerMainScript.gkClickHelperGameObject.SetActive(false);
            playerMainScript.gkHelperImageGameObject.SetActive(false);
            playerMainScript.joystickBgGameObject.SetActive(false);
            playerMainScript.lobButtonGameObject.SetActive(true);
            playerMainScript.volleyButtonGameObject.SetActive(true);
        }

        //if (trainingTaskIdx >= 7 &&
        //    trainingTaskIdx <= 20)
        //    playerMainScript.shotBarBackground.SetActive(true);

        if (trainingTaskIdx == 18)
        {
            showTimeToShot();
        }

        if (trainingTaskIdx >= 23) {
            playerMainScript.activateCanvasElements();
            traningPanel.SetActive(false);
            playerMainScript.joystick.zeroPosition();    
            showTimeToShot();
        }
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
        //playerMainScript.timeImageGameObject.SetActive(false);
        playerMainScript.mainTimeTextGameObject.SetActive(false);
        playerMainScript.timeToShotBallImageGameObject.SetActive(false);
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

    public bool isPlayerOnBall()
    {  
        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        //print("timeToFinishLevel isPlayerOnBall " 
        //    + nextTaskButtonOnClick + " LOCKED " + coroutineLocked);

        if (!playerMainScript.joystickGameObject.activeSelf)
            playerMainScript.joystickGameObject.SetActive(true);

        runPaused = false;
        if (playerMainScript.isPlayerOnBall())
            return true;
  
        if (levelTimeExceeded())
        {
            StartCoroutine(playerOnBallNotDone(trainingTaskIdx,
                Languages.getTranslate("Hey footballer! Go to the ball. Use joystick in the bottom corner"), 
                0f));
        }

        return false;

    }

    public bool isStraightShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            if (playerMainScript.getShotVariant() == SHOTVARIANT.STRAIGHT ||
                playerMainScript.shotLastDistFromMidLine() < 2.5f)
            {
                shotActive = true;
            }

            wasShotActive = true;
        }
                  
        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        //if ((!isShotActive && !isPreShotActive && wasShotActive) ||
        //    (levelTimeExceeded() && !isShotActive && !isPreShotActive))
        float delay = 2.0f;
        bool timeExceeded = levelTimeExceeded();
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("Try once again. Draw a straight line as shown"), delay));
        }

        return false;
    }

    public bool isCurveShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive) {
            if (playerMainScript.getShotVariant() == SHOTVARIANT.CURVE)
            {
                shotActive = true;
            }

            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("Try once again. Try to curl more"), delay));       
        }

        return false;
    }

    public bool isVolleyShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            if (playerMainScript.getShotType().Contains("volley"))
            {
                shotActive = true;
            }
            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;            
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {            
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("Click V button in the bottom corner"), delay));
        }

        return false;
    }
 
    public bool isLobShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            if (playerMainScript.isLobShotActive())
            {
                shotActive = true;
            }

            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;           
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {          
            StartCoroutine(levelNotDone(trainingTaskIdx,
              Languages.getTranslate("Click L button in the bottom corner before shot"), delay));
        }

        return false;
    }

    public bool isVolleyLobShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            if (playerMainScript.getShotType().Contains("volley") &&
                playerMainScript.isLobShotActive())
            {
                shotActive = true;
            }
            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {           
            StartCoroutine(levelNotDone(trainingTaskIdx,
              Languages.getTranslate("Click V and L buttons in the bottom corner before shot"), delay));           
        }

        return false;
    }

    public bool isGrassShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            //print("playerMainScript.getEndPosOrg() " 
            //    + playerMainScript.getEndPosOrg());

            if (playerMainScript.getEndPosOrg().y < 0.65f)
            {
                shotActive = true;
            }
            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("Draw a line on the grass exactly as shown"), delay));
        }

        return false;
    }

    public bool isCrossBarShotDone()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
            return false;

        if (isShotActive)
        {
            if (playerMainScript.getEndPosOrg().y > 1.65f)
            {
                shotActive = true;
            }
            wasShotActive = true;
        }

        if (!isShotActive && shotActive)
        {
            StartCoroutine(levelDone(trainingTaskIdx, 2f));
            return false;
        }

        if (isShotActive ||
            isPreShotActive)
            return false;

        bool timeExceeded = levelTimeExceeded();
        float delay = 2.0f;
        if (timeExceeded && !wasShotActive)
            delay = 0f;

        if (wasShotActive ||
            timeExceeded)
        {         
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("Draw a line closer to the crossbar"), delay));        
        }

        return false;
    }

    private bool timeToShot()
    {
        bool isShotActive = playerMainScript.isShotActive();
        bool isPreShotActive = playerMainScript.isPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
        {
            return false;
        }
  
        if (isShotActive)
        {
            shotActive = true;
        }

        if (!isShotActive && shotActive && timeToFinishLevel < 10.0f)
        {
            StartCoroutine(timeToShotDone(trainingTaskIdx, 3f));
            return false;
        }

        if (levelTimeExceeded(playerMainScript.getMaxTimeToShot()) && 
            !isShotActive && 
            !isPreShotActive)
        {
            StartCoroutine(levelNotDone(trainingTaskIdx,
                Languages.getTranslate("You have to take a shot within 8 seconds. Otherwise your opponent gets the ball"), 0f));
        }

        return false;
    }

    private bool shotTraningAfter()
    {   
        if (!nextTaskButtonOnClick || 
            coroutineLocked)
        {
            return false;
        }
  
        Globals.isTrainingActive = false;
        shotTrainingActive = false;
        gkTrainingActive = false;
        runPaused = false;
        touchPaused = false;
       
        StartCoroutine(shotTraningAfterDone(trainingTaskIdx, 60f));

        //StartCoroutine(shotTraningAfterDone(trainingTaskIdx, 120f));

        return false;
    }

    private bool gkTraning()
    {
        bool cpuShotActive = playerMainScript.cpuPlayer.getShotActive();
        bool preShotActive = playerMainScript.cpuPlayer.getPreShotActive();

        if (!nextTaskButtonOnClick ||
            coroutineLocked)
        {
            return false;
        }

//        print("GK TRAINING 1 " + gkTrainingActive);

        if (gkSavesCounter > MIN_GK_SAVES)
            return true;

        touchPaused = false;
        runPaused = false;
        shotTrainingActive = false;
        gkTrainingActive = true;

        if (cpuShotActive)
        {    
            wasShotActive = true;
        }
     
        if (!cpuShotActive && wasShotActive)
        {            
            StartCoroutine(gkOneSave(1.5f, 0.01f));
            gkSavesCounter++;
            return false;
        }

        if (!cpuShotActive &&
            !preShotActive &&
            levelTimeExceeded(playerMainScript.getMaxTimeToShot()))
        {
            StartCoroutine(gkOneSave(1.5f, 0.01f));
        }

        return false;
    }
  
    IEnumerator gkOneSave(float delayTime, float flashDelay)
    {
        coroutineLocked = true;
        //shotTrainingActive = true;
 
        yield return new WaitForSeconds(delayTime);
        //screen flash
        yield return StartCoroutine(posChangeFlash(flashDelay));

        setGkWithBallDefaultPos();
        shotTrainingActive = false;
        wasShotActive = false;
        coroutineLocked = false;
        timeToFinishLevel = 0f;
    }


    IEnumerator genericDone(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        playerMainScript.timePanelGameObject.SetActive(false);
        trainDialogFunc[index] = noOperation;
        runPaused = true;
    }

    IEnumerator shotTraningAfterDone(int index, float delayTime)
    {
        coroutineLocked = true;


        if (Globals.adsEnable)
        {
            if (admobAdsScript.showInterstitialAd())
            {
                admobCanvas.SetActive(true);
                waitingForInterstitialAddEvent = true;
            }
            //else
            //{
            //    rewardAdsNextCanvasButtonGameObj.SetActive(true);
            //}
        }

        while (waitingForInterstitialAddEvent)
            yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            if (playerMainScript.isShotActive() ||
                playerMainScript.isPreShotActive() ||
                playerMainScript.cpuPlayer.getShotActive() ||
                playerMainScript.cpuPlayer.getPreShotActive())
            yield return new WaitForSeconds(0.2f);

            if (shotTimer > 10f)
            {
                break;
            }

            shotTimer += Time.deltaTime;
        }

        playerMainScript.timePanelGameObject.SetActive(false);
        trainDialogFunc[index] = noOperation;
        coroutineLocked = false;
        runPaused = true;
    }

    IEnumerator timeToShotDone(int index, float delayTime)
    {
        coroutineLocked = true;
        yield return new WaitForSeconds(delayTime);
        playerMainScript.timePanelGameObject.SetActive(false);
        trainDialogFunc[index] = noOperation;
        coroutineLocked = false;
        timeToFinishLevel = 0f;
        nextTaskButtonOnClick = false;
    }

    IEnumerator levelDone(int index, float delayTime)
    {
        coroutineLocked = true;
        touchPaused = true;
        yield return new WaitForSeconds(delayTime);
        trainDialogFunc[index] = noOperation;
        wasShotActive = false;
        shotActive = false;
        coroutineLocked = false;
        timeToFinishLevel = 0f;
        nextTaskButtonOnClick = false;
        touchPaused = false;
    }

    IEnumerator levelNotDone(int index, string text, float delayTime)
    {
        coroutineLocked = true;
        touchPaused = true;
        yield return new WaitForSeconds(delayTime);
        wasShotActive = false;
        shotActive = false;
        showPanelWithText(text);  
        setPlayerWithBallDefaultPos();
        timeToFinishLevel = 0f;
        coroutineLocked = false;
        nextTaskButtonOnClick = false;
        playerMainScript.setTimeToShot(0f);
    }

    IEnumerator playerOnBallNotDone(int index, string text, float delayTime)
    {
        coroutineLocked = true;
        yield return new WaitForSeconds(delayTime); 
        showPanelWithText(text);

        if (playerMainScript.getBallPosition().z > 0f)
        {
            playerMainScript.stopBallVel();
            playerMainScript.setBallPosition(
                new Vector3(0f, 0f, -4f));
            playerMainScript.setUpdateBallPosActive(false);
        }

        if (playerMainScript.getPlayerPosition().z > 0f)
        {
            setPlayerDefaultRotation();
            playerMainScript.setPlayerPos(
                new Vector3(0f, 0f, -11f));
        }

        playerMainScript.joystickGameObject.SetActive(false);
        nextTaskButtonOnClick = false;
        timeToFinishLevel = 0f;
        coroutineLocked = false;
    }

    private void showPanelWithText(string text)
    {
        playerMainScript.deactivateCanvasElements();
        
        if (!traningPanel.activeSelf)
            traningPanel.SetActive(true);
        trainingDialogText.text = text;      
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

    public bool isShotTraining()
    {
        return shotTrainingActive;
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

    private bool endOfTraining()
    {
        Globals.recoverOriginalResolution();
        Globals.isTrainingActive = false;

        if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return false;
        }

        if (Globals.isFriendly)
            SceneManager.LoadScene("gameLoader");
        else
        {
            SceneManager.LoadScene("Leagues");
        }

        return false;
    }

    public void skipTrainingButton()
    {
        //print("SKIPTRAININGBUTTON CLICKED");
        showSkipTrainingButtons();
    }

    public void skipTrainingButtonYes()
    {
        endOfTraining();
    }

    public void skipTrainingButtonNo()
    {
        deactivateTrainingSkipButtons();
    }

    private void activateTrainingSkipButtons()
    {
        trainingSkipButton.SetActive(false);
        trainingSkipButtonYes.SetActive(true);
        trainingSkipButtonNo.SetActive(true);
    }

    private void deactivateTrainingSkipButtons()
    {
        trainingSkipButtonYes.SetActive(false);
        trainingSkipButtonNo.SetActive(false);
        trainingSkipButton.SetActive(true);
    }

    private void showSkipTrainingButtons()
    {
        activateTrainingSkipButtons();
    }
}
