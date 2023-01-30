using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GlobalsNS;
using System.Text.RegularExpressions;
using graphicsCommonNS;
using gameStatisticsNS;
using System.IO;
using System;
using MenuCustomNS;
using GeometryCommonNS;
using UnityEngine.Analytics;
using TMPro;
using AudioManagerNS;

/*TOCOMMENT*/
//using UnityEngine.Profiling;

public class BonusShootToSmallGoal : MonoBehaviour
{
    private Rigidbody rb;
    public setTextures setTextureScript;
    public gamePausedMenu gamePausedScript;
    protected Rigidbody ballRb;
    bool running;
    private Animator animator;
    private Camera m_MainCamera;
    private FixedJoint fixedJoint;
    public joystick1 joystick;
    private RectTransform joystickBG;
    public GameObject joystickBgGameObject;
    private RectTransform joystickButton;
    private float joystickScreenOffset;
    private Vector3 specialButtonsScreenOffset;
    public buttonVolley volleyButton;
    public buttonLob lobButton;
    public buttonCamera cameraButton;
    private GameObject volleyButtonTextGameObject;
    private GameObject lobButtonTextGameObject;
    private GameObject cameraButtonTextGameObject;
    private float lastDistFromMidLine;
    //public buttonOverHead overheadButton;
    /*Canvas game objects */
    //private GameObject overheadButtonGameObject;
    private GameObject pauseCanvas;
    public GameObject lobButtonGameObject;
    public GameObject volleyButtonGameObject;
    public GameObject cameraButtonGameObject;
    public GameObject timePanelGameObject;

    public GameObject timeImageGameObject;
    public GameObject timeToShotBallImageGameObject;

    public GameObject mainTimeTextGameObject;
    private GameObject flagPanelGameObject;
    public GameObject joystickGameObject;

    private GameObject matchIntroPanel;
    private GameObject matchIntroFlagPanel;
    private GameObject matchIntroPanelBackground;
    private GameObject matchIntroPanelHeaderTop;
    private GameObject matchIntroPanelHeaderDown;
    private TextMeshProUGUI matchIntroWinCoinsNumText;
    private TextMeshProUGUI matchIntroTieNumCoinText;



    private float BALL_RADIUS = 0.35f;
    private float BALL_NEW_RADIUS = 0.2585f;
    private float DEFAULT_CAMERA_Y_POS = 4.4f;
    private GameObject shotBarGameObject;
    private GameObject shotBarIconGameObject;
    public GameObject shotBarBackground;
    public GameObject refereeBackground;

    private GameObject matchStatisticsPanel;
    private GameObject matchStatisticsFlagPanel;
    private GameObject matchStatisticsPanelBackground;
    private GameObject matchStatisticsPanelHeaderTop;
    private GameObject matchStatisticsPanelHeaderDown;
    private GameObject matchStatisticsNext;
    private TextMeshProUGUI matchStatisticsCoinsRewareded;
    private TextMeshProUGUI matchStatisticsDiamondsRewareded;
    private GameObject traningPanel;
    public Image flashBackgroundImage;
    private bool ballPositionLock = false;
    private GameObject teamAgoalsText;
    private GameObject teamBgoalsText;
    private GameObject teamAshotsText;
    private GameObject teamBshotsText;
    private GameObject teamAshotsOnTargetText;
    private GameObject teamBshotsOnTargetText;
    private GameObject teamAsavesText;
    private GameObject teamBsavesText;
    private GameObject teamAballPossessionText;
    private GameObject teamBballPossessionText;

    private GameObject stadiumPeople;
    private GameObject stadium;

    public gkMoveUpButton gkMoveUpButton;
    public gkMoveDownButton gkMoveDownButton;
    public gkSideRightButton gkSideRightButton;
    public gkSideLeftButton gkSideLeftButton;
    public RectTransform gkMoveUpButtonRectTrans;
    public RectTransform gkMoveDownButtonRectTrans;
    public RectTransform gkSideRightButtonRectTrans;
    public RectTransform gkSideLeftButtonRectTrans;
    private float passedShotFlyTime = 0.0f;
    private bool preShotActive = false;
    private bool isCurvePressed = false;
    private bool shotActive = false;
    private bool isMovingCurve = false;
    private bool initShot = false;
    private Vector2 startPos, midPos, endPos;
    private Vector3 startPos3, midPos3, localMidPos3, endPos3;
    private Vector3 curveStartPos3, curveMidPos3, curveEndPos3, endPosOrg;
    //private Vector2 prevMidVec;
    private Vector2 lastTouch;
    private Vector2 prevMovedPos;
    //private float maxMovedDot;
    private Vector2 shotDirection2D;
    private Vector3 shotDirection3D;
    private Quaternion lookOnLook;
    private float baseBallSpeed;
    private float addionalBallSpeed;
    //private float ballSpeed;
    private Vector3 ballInitPos;
    Vector3 shotParabolaDirect1 = Vector3.zero;
    Vector3 shotParabolaDirect2 = Vector3.zero;
    Vector2 startNorm;
    Vector2 endNorm;
    Vector2[] positions;
    Vector3 curveVel;
    Vector3 locVel = Vector3.zero;
    private bool isShootingDirectionSet = false;
    private int touchCount = 0;
    private bool activeShot = false;
    private bool shotLock;
    private int width, height;
    private int screenWidth, screenHeight;
    float drawDistance = 0.0f;
    float drawTimeStart;
    float drawTimeEnd;
    private bool touchLocked;
    private float delayAfterGoal;
    public ballMovement ball;
    private bool goalJustScored;
    private bool gkAnimPlayed = false;
    private List<string> AllAnimationsNames;
    private List<string> RunAnimationsNames;
    private List<string> shotTypesNames;
    private Vector3 playerDirection;
    private readonly int MIN_TOUCH = 2;
    private enum PlayerOnBall { NEUTRAL = 0, ISGK = 1, ONBALL = 2 };
    private PlayerOnBall onBall = PlayerOnBall.NEUTRAL;
    private bool isAnyAnimationPlaying = false;
    private bool animationPlaying = false;
    private bool animationStarted = false;
    private string shotType = "3D_shot_right_foot";
    private Vector3 velocityShotSmooth = Vector3.zero;
    private Vector3 m1Smooth = Vector3.zero;
    private Vector3 m2Smooth = Vector3.zero;
    private AudioSource audioSource;
    public float audioVolume = 1.0f;
    private AudioManager audioManager;
    private float angularVelocityFactor = 1.0f;
    private bool gkTouchDone = false;
    private float lastTimeAnimationPlayed = 0.0f;
    private Quaternion cameraStartPos;
    private float ballOutOfGameTime = 0.0f;
    private bool isBallInGame = true;
    private Vector3 lastSaveRotationPosition = Vector3.zero;
    private LineRenderer touchLine;
    private Vector3 lineStartPos;
    private Vector3 lineEndPos;
    private float minDistToBallShot = 0.8f;
    private float minDistToOnBAll = 0.8f;
    private float maxYdistToBallShot = 0.3f;
    private float gkMinDistToWalls = 2.2f;
    private Vector3 gkTouchPosRotatedRbWS;
    private Vector3 gkTouchPosRbWS;
    private Image gkHelperImage;
    public GameObject gkHelperImageGameObject;
    private float gkHelperImageWidth;
    private float gkHelpeImageHeight;
    private RectTransform gkHelperRectTransform;
    private Image gkClickHelper;
    private RectTransform rectTransformGkClickHelper;
    public GameObject gkClickHelperGameObject;
    private GameObject tournamentGroupObj;
    //private GroupTable tournamentGroup;
    private float timeOfGameInSec = 0;
    private float stoppageTime = 0f;
    private float currentTimeOfGame = 0;
    private TextMeshProUGUI score1Text;
    private TextMeshProUGUI score2Text;
    private TextMeshProUGUI mainTimeText;
    private TextMeshProUGUI timeToShotText;
    //private Text gameEventMsgText;
    private TextMeshProUGUI gameEventMsgText;
    private bool initDisplayEventInfo = false;
    private int teamHostID = 1;
    //private GameObject gameInfoImageGameObject;
    //private RawImage gameInfoImage;
    private float gameEventLastMsgPrintTime;
    private Image shotBar;
    //private Text speedShotText;
    private TextMeshProUGUI speedShotText;
    private TrailRenderer trailRenderer;
    private GameObject trailShoot;
    public GameObject drawPrefabShotTrail;
    private bool isBallTrailRendererInit = false;
    /*POSSIBLE TO REMOVE*/
    private Texture aTexture;
    //private Vector3 startVectorDraw;
    private Vector3 endVectorDraw;
    private float shotCurveMinDist = 80.0f;
    public GameObject cpuGoalUp;
    private Plane goalUpPlane;
    private Plane goalDownPlane;
    private Plane middleLinePlane;
    private float timeofBallFly;
    private float shotSpeed;
    private float ShotSpeedMax = 1300.0f;
    private float ShotSpeedMin = 420.0f;
    /*private float ShotCurveSpeedMinTime = 650.0f;
    private float ShotCurveSpeedMaxTime = 1500.0f;*/
    private float MIN_SHOT_SPEED = 50.0f;
    private bool isLobButtonPressed = false;
    private bool isLobActive = false;
    //private float animationOffsetTime;
    private GameObject rbRightFoot;
    private GameObject rbRightToeBase;
    private GameObject rbLeftToeBase;
    private GameObject rbHead;
    private float ballUpStartYVelocity = 0.14f;
    private IDictionary<string, float> animationOffsetTime;
    private bool initPreShot;
    private bool initVolleyShot;
    private bool isBallOut = false;
    private Vector3 outShotStart, outShotMid, outShotEnd, outShotBallVelocity;
    public SHOTVARIANT shotvariant;

    private float CURVE_SHOT_MAX_DIST = 4.0f;
    private float SKILLS_MAX_VALUE = 100.0f;
    private float LEVEL_MIN = 1f;
    private float LEVEL_MAX = 5f;
    private float SPEED_RUN_MIN = 2.0f;
    private float SPEED_RUN_MAX = 10.0f;
    private float MAX_PLAYER_SPEED = 12.0f;
    private float MIN_ANIM_GK_PLAYER_SPEED = 3.1f;
    private float MAX_ANIM_GK_PLAYER_SPEED = 4.0f;
    private float GK_SIDE_MOVE_MIN_TIME = 0.3f;
    private Vector3 BALL_MIN_VAL =
        new Vector3(0f, 0.270f, 0f);

    /*this is time the maximum time is needed to GK 
     * with slowest animation execution to save a ball*/
    private float MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE = 0.270f;
    private bool shotRet = false;
    private float cpuGkLastMovement = 0.0f;
    private int counter = 0;
    private string lastGkAnimName = "";
    private float lastTimeGkAnimPlayed = 0f;
    private GameObject cpuLeftPalm;
    private GameObject cpuRightPalm;
    private GameObject leftPalm;
    private GameObject rightPalm;
    private GameObject leftHand;
    private GameObject rightHand;
    private float lastGkDistX;
    bool matchTargetActive1 = false;
    bool matchTargetActive2 = false;
    private Vector3 gkStartPos;
    private Transform gkStartTransform;
    private float gkTimeToCorrectPos;
    private float BALL_MAX_SPEED = 50.0f;
    private float FRAME_RATE = 30.0f;
    private bool initCpuAdjustAnimSpeed = false;
    private bool initGkDeleyLevel = false;
    private float levelDelay = 0.0f;
    private string initAnimName = "";
    private float cpuGkAnimAdjustSpeed = 2.2f;
    private float prevZballPos;
    private float timeToShot;
    /*TOCHANGE TO 10 BACK */
    private float maxTimeToShot = 10f;
    private int timeLoops = 0;
    //private float rateScreenWidth;
    //private float rateScreenHeight;
    private Vector3 prevRbPos;
    /*Pitch is 42 width. This value represents half of pitch width */
    private float PITCH_WIDTH_HALF = 20.0f;
    /* Pitch is 28 height. This value represents half of pitch height*/
    public float PITCH_HEIGHT_HALF = 14.0f;
    public float COMPUTER_MIN_Z_TO_GET_BALL = 0.35f;
    private float goalXOffset = 4.8f;
    private float goalYOffset = 2.9f;
    private float goalZOffset = -14.0f;

    private float goalXOffsetCpu = 5.25f;
    private float goalYOffsetCpu = 3.5f;
    private float goalZOffsetCpu = 14.0f;

    private Vector3 goalSizes = new Vector3(4.7f, 3.1f, -14.0f);
    private Vector3 goalSizesCpu = new Vector3(5.25f, 3.5f, 14.0f);

    private float MAX_GK_OFFSET = 6.8f;
    private float MAX_GK_OFFSET_CPU = 6.8f;
    private float MAX_GK_HAND_REACH_CPU = 2.7f;
    private float MIN_GK_STRAIGHT_DIST_CPU = 0.45f;

    private Vector3 gkCornerPoints;

    int attackSkillsPlayer = 40;
    int attackSkillsCpu = 40;
    int defenseSkillsPlayer = 40;
    int defenseSkillsCpu = 40;
    int cumulativeStrengthPlayer = 80;
    int cumulativeStrengthCpu = 80;

    private string gkAction;
    private float gkTimeLastCatch;
    public RawImage teamAflagIntro;
    public RawImage teamBflagIntro;
    //public Text teamAIntroText;
    //public Text teamBIntroText;
    public TextMeshProUGUI teamAIntroText;
    public TextMeshProUGUI teamBIntroText;

    public RawImage teamAflagStatisticsImage;
    public RawImage teamBflagStatisticsImage;
    //public Text teamAStatisticsText;
    //public Text teamBStatisticsText;
    public TextMeshProUGUI teamAStatisticsText;
    public TextMeshProUGUI teamBStatisticsText;

    public float realTime = 0.0f;
    private bool gameStarted = false;
    private bool gameStartedInit = false;
    private bool gameEnded = false;
    private bool gameEndedAnimations = false;
    private bool isTouchBegin = false;

    private MatchStatistics matchStatistics;
    private Vector3 ballPrevPosition;

    private int winCoinsRewarded;
    private int tieCoinsRewarded;

    private GraphicsCommon graphics = new GraphicsCommon();
    private GeometryCommon geometry = new GeometryCommon();
    private float MAX_RB_VELOCITY = 10.0f;

    public GameObject[] fansFlag;
    public GameObject[] fansFlagSticks;
    private List<RectTransform> joystickButtons = new List<RectTransform>();
    private static int FANS_FLAG_MAX = 6;

    private Vector3[] fansFlagAngles = new Vector3[FANS_FLAG_MAX];
    private Vector3[] fansFlagDirections = new Vector3[FANS_FLAG_MAX];
    private bool[] isFansFlagActive = new bool[FANS_FLAG_MAX];
    private Vector3 stepSideAnimOffset = Vector3.zero;
    private bool gkLobPointReached = false;
    private bool gkRunPosReached = false;
    private float initDistX = -1f;
    //private PlayerUpRigidBody playerUpRigidBody;
    private Vector3 lastBallVelocity;
    private int level = Globals.level;
    private float speedMultiplayer = 80.0f;
    private float runningSpeed;
    private float ballRadius = 0.4f;
    private static int MID_MAX_POS = 100;
    private Vector2[] midTouchPos;
    private int midTouchPosIdx = 0;
    private int touchFingerId = -1;
    private bool isTrainingActive = Globals.isTrainingActive;

    private float joystickButtonWidth = 80f;
    private float joystickButtonHeight = 80f;
    private Vector3 INCORRECT_VECTOR = new Vector3(float.MaxValue / 3f,
                                                   float.MaxValue / 3f,
                                                   float.MaxValue / 3f);
    private bool gkLock = false;
    private Vector3 ballBeforeLookPos;
    private float MIN_DIST_REAL_CLICKED = 1.0f;
    private float gkDistRealClicked = 0;
    private float INCORRECT_DIST = float.MinValue;
    private GameObject rotatedRbToBall;
    private GameObject tmpRotatedRbToBall;

    private bool isUpdateBallPosActive = false;
    private Vector3 updateBallPos;
    private string updateBallPosName = "";

    private bool timeToShotExceeded = false;

    private GameObject dummyTouchRotatedGO;
    private RectTransform cameraRectTrans;
    /*not main camera. it's position of camera to change game view angles*/
    private Vector3 cameraPos;
    public Canvas UICanvas;

    /*TODELETE*/
    private Vector3[,] lastPlayerMovePos;
    private int lastPlayerMovePosHead = 0;
    private int lastPlayerMovePosTail = 0;
    private int lastPlayerMovePosNumEl = 0;

    private Vector3 matchSavePos;
    private bool matchInitSavePos;
    private float WALLS_MIN_OFFSET = 3f;

    private int isFixedUpdate = 0;

    private GameObject wallUpLeftTop;
    private GameObject wallUpRightTop;
    private GameObject wallUpLeft1;
    private GameObject wallUpLeft2;
    private GameObject wallUpRight1;
    private GameObject wallUpRight2;

    private string leagueName;

    //private bool gkHelperImageErased = true;
    //private GameObject MarkerBasic;
    void setupGlobals()
    {
        Globals.joystickSide = "LEFT";
    }

    void Awake()
    {
        //TOREMOVE
        setupGlobals();

        leagueName = Globals.leagueName;
        Globals.initSkills(ref attackSkillsPlayer,
                           ref attackSkillsCpu,
                           ref defenseSkillsPlayer,
                           ref defenseSkillsCpu,
                           ref cumulativeStrengthPlayer,
                           ref cumulativeStrengthCpu,
                           Globals.playerPlayAway);

        //print("#DBGSKILLS123 PLAYER attack " + attackSkillsPlayer + " defenseSkillsPlayer "
        //    + defenseSkillsPlayer
        //    + " cumulative " +
        //    cumulativeStrengthPlayer);

        //print("#DBGSKILLS123 CPU attack " + attackSkillsCpu + " defenseSkillsPlayer "
        //        + defenseSkillsCpu
        //        + " cumulative " +
        //        cumulativeStrengthCpu);

        if (leagueName.Equals("WORLDCUP") ||
            leagueName.Equals("EUROCUP") ||
            leagueName.Equals("COPAAMERICA"))
        {
            teamHostID = UnityEngine.Random.Range(1, 3);
        } else
        {
            teamHostID = 1;
        }

        //teamHostID = UnityEngine.Random.Range(1, 3);
        //if (Globals.playerPlayAway)
        //{
        //    teamHostID = 1;
        //}
    }

    void Start()
    {
        //print("DBG12345 Globals.level " + Globals.level);

        if (Globals.level == Globals.MIN_LEVEL)
            maxTimeToShot = 20;

        goalResize(true);
        //Application.targetFrameRate = 30;
        //QualitySettings.vSyncCount = 0;
        //QualitySettings.antiAliasing = 0;

        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        gkCornerPoints = new Vector3(PITCH_WIDTH_HALF, 0f, -PITCH_HEIGHT_HALF);
        dummyTouchRotatedGO = new GameObject("touchRotatedGO");

        Globals.numMatchesInThisSession++;

        /*moved to setTextures*/
        //updateTrainingSettings();

        screenWidth = Screen.width;
        screenHeight = Screen.height;
        //rateScreenWidth = Globals.OrginalScreenWidth / (float) (Screen.width);
        //rateScreenHeight = Globals.OrignalScreenHeight / (float) (Screen.height);

        //print("RESOLUTIONS SETUP WIDTH "
        //    + Screen.width + "x" + Screen.height);

        setupLevelDependentVariables();
        midTouchPos = new Vector2[MID_MAX_POS];

        lastPlayerMovePos = new Vector3[20, 2];
        /*#if UNITY_EDITOR
            Debug.logger.logEnabled = true;
        #else
            Debug.logger.logEnabled = false;
        #endif*/

        //print(" Application.targetFrameRate " + Application.targetFrameRate);
        //Application.targetFrameRate = 30;

        //print("START MAIN SCENE");

        gkHelperImageGameObject = GameObject.Find("gkHelper");
        gkClickHelperGameObject = GameObject.Find("gkClickHelper");

        gkHelperImage = GameObject.Find("gkHelper").GetComponent<Image>();
        gkHelperRectTransform = GameObject.Find("gkHelper").GetComponent<RectTransform>();
        gkHelperImageWidth = Screen.width * 0.055f;
        gkHelpeImageHeight = gkHelperImageWidth;
        gkHelperRectTransform.sizeDelta = new Vector2(gkHelperImageWidth,
                                                      gkHelpeImageHeight);

        //print("DEBUGSCREEN1x2SCREEN SCREEN WIDTH " + Screen.width);
        //print("DEBUGSCREEN1x2SCREEN HEIGHT " + Screen.height);
        //print("DEBUGSCREEN1x2SCREEN gkHelperRectTransform size" + gkHelperRectTransform.sizeDelta);

        //playerUpRigidBody = GameObject.Find("playerUp").GetComponent<PlayerUpRigidBody>();
        //MarkerBasic = GameObject.Find("Marker.Basic");
        //MarkerBasic.SetActive(false);

        setupCanvasGameObjects();
        setJoystickPosition();
        setSpecialButtonsPosition();

        rotatedRbToBall = new GameObject();
        tmpRotatedRbToBall = new GameObject();

        Globals.score1 = 0;
        Globals.score2 = 0;
        score1Text = GameObject.Find("scoreText_1").GetComponent<TextMeshProUGUI>();
        score2Text = GameObject.Find("scoreText_2").GetComponent<TextMeshProUGUI>();

        mainTimeText = GameObject.Find("mainTimeText").GetComponent<TextMeshProUGUI>();
        timeToShotText = GameObject.Find("timeToShotText").GetComponent<TextMeshProUGUI>();

        gameEventMsgText = GameObject.Find("gameEventMsgText").GetComponent<TextMeshProUGUI>();
        shotBar = GameObject.Find("shotBar").GetComponent<Image>();

        speedShotText = GameObject.Find("speedShotText").GetComponent<TextMeshProUGUI>();

        rbRightFoot = GameObject.FindWithTag("playerDownRightLeg");
        rbRightToeBase = GameObject.FindWithTag("playerDownRightToeBase");
        rbLeftToeBase = GameObject.FindWithTag("playerDownLeftToeBase");
        rbHead = GameObject.FindWithTag("playerDownHead");

        cpuLeftPalm = GameObject.FindWithTag("playerUpLeftPalm");
        cpuRightPalm = GameObject.FindWithTag("playerUpRightPalm");


        leftPalm = GameObject.FindWithTag("playerDownLeftPalm");
        rightPalm = GameObject.FindWithTag("playerDownRightPalm");

        leftHand = GameObject.FindWithTag("playerDownLeftHand");
        rightHand = GameObject.FindWithTag("playerDownRightHand");
        matchStatistics = new MatchStatistics();
       
        /*string[] flagsNames = new string[] { Globals.teamAname,
                                             Globals.teamBname};
        RawImage flagGameImage;
        for (int i = 0; i < flagsNames.Length; i++)
        {            
            flagGameImage = 
                GameObject.Find("flagImage_" + (i + 1).ToString()).GetComponent<RawImage>();
            
            graphics.setFlagRawImage(flagGameImage,
                                     graphics.getFlagPath(flagsNames[i]));
        }*/

        /*graphics.setFlagRawImage(teamAflagIntro,
            graphics.getFlagPath(Globals.teamAname));

        graphics.setFlagRawImage(teamBflagIntro,
           graphics.getFlagPath(Globals.teamBname));

        teamAIntroText.text = Globals.teamAname;
        teamBIntroText.text = Globals.teamBname;*/

        //calculateAndSetWinTieMatchIntroValues(cumulativeStrengthPlayer,
        //                                      cumulativeStrengthCpu);


        /*graphics.setFlagRawImage(teamAflagStatisticsImage,
                                 graphics.getFlagPath(Globals.teamAname));

        graphics.setFlagRawImage(teamBflagStatisticsImage,
                                 graphics.getFlagPath(Globals.teamBname));

        teamAStatisticsText.text = Globals.teamAname;
        teamBStatisticsText.text = Globals.teamBname;*/

        //updateStadiumTextures();
        ///initFlagPositions();

        setScoresText();
        setTimesText();

        animationOffsetTime = new Dictionary<string, float>();
        animationOffsetTime.Add("3D_shot_left_foot", 0.33f);
        animationOffsetTime.Add("3D_shot_right_foot", 0.33f);

        animationOffsetTime.Add("3D_volley", 0.45f);


        initPreShot = false;
        initVolleyShot = false;

        rb = GetComponent<Rigidbody>();
        touchLine = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        ballRb = GameObject.Find("ball").GetComponent<Rigidbody>();
        gkClickHelper = GameObject.Find("gkClickHelper").GetComponent<Image>();

        rectTransformGkClickHelper = gkClickHelper.GetComponent<RectTransform>();

        goalUpPlane = new Plane(
                                new Vector3(0, 0, -1),
                                new Vector3(0.0f, 0.0f, 14.0f));

        goalDownPlane = new Plane(new Vector3(0, 0, 1),
                                  new Vector3(0.0f, 0.0f, -14.0f));

        middleLinePlane = new Plane(
                                new Vector3(0, 0, -1),
                                new Vector3(0.0f, 0.0f, 14.0f));

        /*Fill time of a game */
        float timeFactor = 0.0f;
        if (Globals.matchTime.Contains("SECONDS"))
        {
            timeFactor = 1.0f;
        } else
        {
            timeFactor = 60.0f;
        }



        //string timeOfGame = Regex.Replace(Globals.matchTime, "[^0-9]", "");
        //timeOfGameInSec = float.Parse(timeOfGame) * timeFactor;
        timeOfGameInSec = 99999999999999;

        stoppageTime = UnityEngine.Random.Range(4, 10);

       
        animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;

        passedShotFlyTime = 0.0f;
        startPos = endPos = Vector2.zero;
        baseBallSpeed = 25.0f;
        initShot = isBallTrailRendererInit = false;
        endPos = Vector2.zero;
        positions = new Vector2[1000];
        touchCount = 0;
        shotLock = false;
        width = Screen.width;
        height = Screen.height;
        drawDistance = 0.0f;
        delayAfterGoal = 0.0f;
        goalJustScored = false;
        gkAnimPlayed = false;
        AllAnimationsNames = new List<string>();
        RunAnimationsNames = new List<string>();
        shotTypesNames = new List<string>();
        gkTimeToCorrectPos = 0.119f;

        initAnimationList();
        initRunAnimationList();
        initShotsList();

        audioManager = FindObjectOfType<AudioManager>();
        touchLocked = false;

        ballRb.maxAngularVelocity = 1000.0f;
        cameraStartPos = m_MainCamera.transform.rotation;

        gkHelperImage.enabled = false;
        gkClickHelper.enabled = false;
        prevZballPos = ballRb.transform.position.z;
        ballPrevPosition = ballRb.transform.position;
     
        //if (!isTrainingActive)
        //{
        //    audioManager.Play("fanschantBackground2");
        //} else
        //{
            audioManager.Play("training1");
        //}

        deactivateCanvasElements();

        /*Draw who starts */
        int whoStarts = UnityEngine.Random.Range(0, 2);
        if (whoStarts == 0)
        {
            ballRb.transform.position = new Vector3(0, 0, 2);
        } else
        {
            ballRb.transform.position = new Vector3(0, 0, -2);
        }

        if (isTrainingActive)
        {
            gameStarted = true;
            initCameraMatchStartPos();
            //activateCanvasElements();
            ballRb.transform.position = new Vector3(0, 0, -4);
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        prevRbPos = rb.transform.position;
        matchSavePos = rb.transform.position;
        matchInitSavePos = false;
        //print("maxTimeToShotDBG2 START" + maxTimeToShot);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult = Analytics.CustomEvent("Game_Started", new Dictionary<string, object>
            {
                { "level", Globals.level},
                { "is_Friendly", Globals.isFriendly},
                { "teamA_name", Globals.teamAname },
                { "teamB_name", Globals.teamBname },
                { "match_Time", Globals.matchTime },
                { "joystickSide", Globals.joystickSide},
                { "numGameOpened", Globals.numGameOpened},
                { "numMatchesInThisSession", Globals.numMatchesInThisSession},
                { "coins", Globals.coins},
                { "isTrainingActive", Globals.isTrainingActive},
                { "isGoalEnlarge", Globals.cpuGoalSize}
             });
            //Debug.Log("Analytics result " + analyticsResult);
        }
    }

    //int deltaIterator = 1;
    void Update()
    {
        //print("DEBUGCAMERA UPDATE DELTA TIME " + deltaIterator + " Time.deltaTime " + Time.deltaTime);

        //print("maxTimeToShotDBG2 FIXEDUPDATE" + maxTimeToShot);

        /*in traning mode start game immediately */
        //if (isTrainingActive)
        //    gameStarted = true;
        if (isGamePaused())
        {
            return;
        }
        clearGeneralInformtionText();

        if (!isTrainingActive)
            updateFlagsPositions();

        realTime += Time.deltaTime;
        if (gameEnded)
        {
            rb.velocity = Vector3.zero;
            displayStatisticsPanel();
            gamEndAnimations();
            if (realTime < 1f && Globals.score1 >= Globals.score2)
                if (!isTrainingActive)
                    audioManager.Play("crowdOvation1Short");
            return;
        }

        /*if (!gameStarted)
        {
            if (realTime > 13.0f)
            {
                audioManager.Play("whislestart1");
                gameStarted = true;
            }

            float matchIntroPanelFillAmount = (realTime - 1.0f) / 2.0f;
            if (realTime >= 1.0f && realTime < 3.0f)
            {
                int chantRandom = UnityEngine.Random.Range(3, 5);
                if (!gameStartedInit)
                    audioManager.Play("fanschant" + chantRandom.ToString());
                gameStartedInit = true;
                matchIntroPanel.SetActive(true);
                matchIntroPanel.GetComponent<Image>().fillAmount = matchIntroPanelFillAmount;
            } else
            {
                matchIntroPanel.GetComponent<Image>().fillAmount = 1.0f;
            }

            if (realTime > 3.2f)
            {
                matchIntroPanelHeaderDown.SetActive(true);
            }

            if (matchIntroPanelFillAmount > 0.1f && realTime <= 4.5f)
            {
                matchIntroPanelHeaderTop.SetActive(true);
            }

            //if (matchIntroPanelFillAmount >= 0.95f && realTime <= 4.5f)
            //{
            // matchIntroPanelHeaderDown.SetActive(true);
            //}

            if (realTime > 3.7f)
            {
                matchIntroFlagPanel.SetActive(true);
                matchIntroPanelBackground.SetActive(true);
            }

            if (realTime > 7.0f)
            {
                matchIntroPanelBackground.SetActive(false);
                matchIntroPanel.SetActive(false);
            }

            if (realTime > 9.0f)
            {
                initCameraMatchStartPos();
                activateCanvasElements();
            }

            return;
        }*/

        /*if updateGameTime is true - begining of match end */
        if (!isTrainingActive)
        {
            if (updateGameTime())
            {
                addCoins();
                audioManager.Play("whisleFinal1");
                realTime = 0.0f;
                gameEnded = true;
                rb.velocity = Vector3.zero;      
            }
        }

        timeLoops++;

        matchTarget(animator, 
                    rb, 
                    ref gkStartPos, 
                    gkTimeToCorrectPos, 
                    stepSideAnimOffset,
                    ref matchSavePos,
                    ref matchInitSavePos,
                    false);

        /*if (!isTrainingActive)
        {

            if (setTextureScript.isFlareEnable())
            {
                int randChant = UnityEngine.Random.Range(3, 5);
                if (!audioManager.isPlayingByName("fanschant3") &&
                    !audioManager.isPlayingByName("fanschant4"))
                    audioManager.Play("fanschant" + randChant.ToString());
            }
            else
            {
                if (timeLoops % 1000 == 0)
                {
                    int randChant = UnityEngine.Random.Range(0, 4) + 1;
                    if (randChant == 2)
                        randChant = 4;

                    audioManager.Play("fanschant" + randChant.ToString());
                }
            }

            if ((timeLoops % 1400) == 0)
            {
                audioManager.Play("crowdBassDrum");
            }
        }*/

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying && shotActive == false)
        {
            touchLocked = false;
        }

        float distance = Vector3.Distance(rb.transform.position,
                                         (ballRb.transform.position));

        if ((isPlayerOnBall()) &&
            !isTouchPaused()) {

            if (          
                isPlayerOnBall())
                updateTouch();
        }

        clearTouch();

        //if (onBall == PlayerOnBall.ISGK && !shotActive && !preShotActive)       
    }

    void FixedUpdate()
    {
        //print("RBVEL " + rb.velocity);
        if (isGamePaused())
            return;

        /*string name = nameAnimationPlaying(animator, 1.0f);
        bool isBackRunPlaying = isPlaying(animator, "3D_back_run_cpu", 1.0f);
        bool isRunPlaying = isPlaying(animator, "3D_run", 1.00f);

        if (name == string.Empty)
        {
            if (isBackRunPlaying)
                name = "3D_back_run_cpu";
            if (isRunPlaying)
                //name = "3D_run_cpu";
                name = "3D_run";
        }

        if (name != string.Empty)
        {
            //if (isRunPlaying)
            //    name = "3D_back_run_cpu";

            print("DEBUG2345ANIMPLAY PLAYINGANIMATIONOW " + name + "  "
                + animator.GetCurrentAnimatorStateInfo(0).IsName(name) + " ANIMNORMTIME "
                + animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                + " BALLPOS " + ballRb.transform.position
                + " RB POS " + rb.transform.position);
        }*/
        

        //print("ENTERPOSITION CLEAN END OF FIXEDUPATE START RB POS " 
        //    + rb.transform.position + " isUpdateBallPosActive " + isUpdateBallPosActive);

        /*this should stay here because fixedUpdate might execute more than once?? */
        correctWhenOutOfPitch(animator,
                              preShotActive,
                              shotActive,
                              rb,
                              rbLeftToeBase,
                              rbRightToeBase,
                              ballRb,
                              shotType,
                              ref prevRbPos,
                              false);

        corectBallPositionOnBall(rb,
                                 animator,
                                 rbRightToeBase,
                                 rbRightFoot,
                                 ref isUpdateBallPosActive,
                                 updateBallPos,
                                 updateBallPosName,
                                 false);

        Vector2 ballPos = new Vector2(ballRb.transform.position.x, ballRb.transform.position.z);

        recoverAnimatorSpeed();

        //if (!gameStarted)
        //{
        //    if (realTime > 2.0f)
        //    {
        //        cameraMovementIntro();
        //    }
        //    return;
        //}

        isFixedUpdate++;

       // cameraMovement(false);

        /*TOCLEAN*/
        /*if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch"))
        {
            gkLock = false;
        }*/

        if (!gameEnded)
            //|| (gameEnded && !gameEndedAnimations))
            /*TODELETE*/
            if (!gkLock)
                playerRun();

        if (gameEnded)
        {
            correctWhenOutOfPitch(animator,
                             preShotActive,
                             shotActive,
                             rb,
                             rbLeftToeBase,
                             rbRightToeBase,
                             ballRb,
                             shotType,
                             ref prevRbPos,
                             false);

            return;
        }

        if (!isTrainingActive &&
            !timeToShotExceeded)
            playMissGoalSound();

        /*Second condition will help when you shot and ball will not cross half*/
        if (ball.getBallCollided() ||
            (shotActive &&
            (Mathf.Abs(ballRb.transform.position.y) < 0.45f) &&
            (Mathf.Abs(ballRb.velocity.x) < 0.05f) &&
            (Mathf.Abs(ballRb.velocity.y) < 0.05f) &&
            (Mathf.Abs(ballRb.velocity.z) < 0.05f))) {

            /*print("TOUCHPOSITION getBallCollided " + ball.getBallCollided() + " X " + Mathf.Abs(rb.velocity.x) +
               " Y " + Mathf.Abs(rb.velocity.y) + " Z " + Mathf.Abs(rb.velocity.z));*/
            //print("GKDEBUG800 PLAYINGANIMATIONOW CLEAR SHOT ACTIVE VELOCITY" + ballRb.velocity.ToString("F10"));
            //print("GKDEBUG800 " + ball.getBallCollided());

            clearAfterBallCollision();
            ball.setBallCollided(false);

            //print("GKDEBUG1TEST1 SHOTACTIVE " + shotActive + " PRESHOT " + preShotActive);

            if (ball.getBallCpuPlayerStatus() == false)
            {
                //print("POINT TO GO COMPUTER 1");
                //cpuPlayer.setShotActive(false);
            } else
            {
                ball.setBallCpuPlayerCollision(false);
            }
        }

        //print("GKDEBUG7 BallRBVELOCITY VELOCITY BEFORE CPU FIXED UPDATE " + ballRb.velocity + " SHOT " + shotActive);

        //print("GKDEBUG7 BallRBVELOCITY VELOCITY AFTER CPU FIXED UPDATE " + ballRb.velocity + " SHOT " + shotActive);

        //prepareFrameClean();

        //print("UNLOCK TOUCH COUNT " + touchCount);

        timeToShotExceeded = updateTimeToShot(ref prevZballPos, ref timeToShot);
        if (isBallOutOfPitch() ||
            ball.getBallGoalCollisionStatus() ||
            (timeToShotExceeded && !isTrainingActive))
            isBallOut = true;

        /*print("Executed setBallPositionFlash AFTER DELAY IS BALL OUT " 
            + isBallOutOfPitch() + " BALLCOLLISION " + ball.getBallGoalCollisionStatus());*/

        if (isBallOut)
        {
            if (delayAfterGoal <= 3.0f)
            {
                delayAfterGoal += Time.fixedDeltaTime;
                if (!initDisplayEventInfo)
                {
                    displayEventInfo();
                    initDisplayEventInfo = true;
                }

                isBallInGame = false;
                goalJustScored = true;
                onBall = PlayerOnBall.NEUTRAL;
                setScoresText();
                setGkHelperImageVal(false);
                enableShotButtons();
                //preShotActive = false;
                //shotActive = false;

                //print("GOALDEBEBUG1 " + ball.getBallGoalCollisionStatus() + " TIMEDIFF " + (Time.time - gkTimeLastCatch) + " ACTION " + gkAction);

                isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
                if (!isAnyAnimationPlaying)
                {
                    //if (delayAfterGoal < 1.8f)
                    //{

                    if (isPlaying(animator, "3D_run", 1.0f) ||
                        playerDirection != Vector3.zero)
                    {
                        //print("LOOKATHEREOUT1ENDPOSORG PLAYERDIRECTION" + playerDirection);
                        //isRunRotationActiveOut = true;
                        RblookAtDirection(rb, playerDirection, 10f);
                        //print("DEBUG2345ANIMPLAY PLAYERDIRECTION LOOK AT DIRECTION OF RUN ");
                    } else
                    {
                        //print("DEBUG2345ANIMPLAY PLAYERDIRECTION OUT ####### " + playerDirection);

                        if (preShotActive || shotActive)
                        {
                            endPosOrg = updateEndShotPos();
                            RblookAtWSPoint(rb, endPosOrg);
                        }
                        else
                        {
                            RblookAtWSPoint(rb, new Vector3(0f, 0f, PITCH_HEIGHT_HALF));
                        }
                        //print("LOOKATHEREOUT1ENDPOSORG " + endPosOrg + " ENDPOS " + endPos);
                    }                
                }

                correctWhenOutOfPitch(animator,
                           preShotActive,
                           shotActive,
                           rb,
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           shotType,
                           ref prevRbPos,
                           false);

                //playerOutOfPitch(animator, rb, ref prevRbPos, rbLeftToeBase, rbRightToeBase, isPlayerOnBall(), false);
            }
            else
            {
                //clearVariables();
                if (Globals.isTrainingActive)
                {
                    clearVariables();
                    ballPrevPosition = ballRb.transform.position;
                    ball.setBallGoalCollisionStatus(false);
                }
                else
                {
                    if (timeToShotExceeded)
                    {
                        if (ball.transform.position.z <= 0)
                            ball.setwhoTouchBallLast(1);
                        else
                            ball.setwhoTouchBallLast(2);

                        //print("whoTouchBallLast TIMETOSHOTEXCEEDED" + ball.whoTouchBallLast());
                        timeToShot = 0.0f;
                    }

                    if (!ballPositionLock)
                        StartCoroutine(setBallPositionFlash(0.01f));
                }

                preShotActive = false;
                shotActive = false;
                initDisplayEventInfo = false;
            }

            return;
        }

        updateBallPossessionMatchStatistics();

        //print("Executed setBallPositionFlash AFTER");
        float distance = Vector3.Distance(rb.transform.position,
                                         (ballRb.transform.position));

        //print("GKDEBUG1 DISTANCEHERE " + distance + " preShotActive " + preShotActive + " ShoTACTIVE " + shotActive + 
        //    " RBTRANSFOMR " + rb.transform.position);

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!preShotActive && !shotActive && !isAnyAnimationPlaying)
        {
            //if (distance < minDistToBallShot && !cpuPlayer.getShotActive() && !shotActive && !preShotActive)
            bool isOnBall = isPlayerOnBall(rbLeftToeBase, rbRightToeBase, ballRb, rb, "move", false);
            //print("CPUMOVEDEBUG123X_NOCPU PLAYERONBALL " + isOnBall);
            if (isOnBall)
               
            //!shotActive && 
            //!preShotActive)
            {
                //print("Distance " + distance);
                if (touchCount > 1)
                {
                    int shotFoot = UnityEngine.Random.Range(0, 2);
                    if (shotFoot == 1)
                        shotType = "3D_shot_left_";
                    else
                        shotType = "3D_shot_right_";
                    shotType += "foot";

                    if (volleyButton.getButtonState())
                    {
                        shotType = "3D_volley_before";
                    }

                    if (lobButton.getButtonState())
                    {
                        isLobActive = true;
                    }

                    /*Striker moves*/
                    preShotActive = true;
                    gkTouchDone = false;
                    touchCount = 0;

                    /*TOCHECK*/
                    isUpdateBallPosActive = false;

                    //print("GKDEBUG1TEST1 PRESHOT REACXTIVE " + preShotActive + " SHOT " 
                    //    + shotActive + " isAnyAnimationPlaying " + isAnyAnimationPlaying);
                    // print("GKDEBUG1 REFRESH!!");

                }
               
                onBall = PlayerOnBall.ONBALL;
                gkTouchDone = false;
            }
            else
            {
                /*GK moves*/
                if (gkTouchDone == true && ballRb.velocity.z < 0.0f)
                {
                    onBall = PlayerOnBall.ISGK;
                    //print("GKDEBUG1 EXECUTED");
                }
            }

            if (!isOnBall)
            {
                //print("ONBALL NEUTRAL ");
                onBall = PlayerOnBall.NEUTRAL;
            }
        }

        //print("GKDEBUG7 BallRBVELOCITY VELOCITY AFTER CPU TOUCH " + ballRb.velocity + " SHOTACTIVE " + shotActive);

        if (preShotActive && !shotActive)
        {
            shotActive = prepareShot(animator,
                                     ref shotType,
                                     rb,
                                     ballRb,
                                     rbRightFoot,
                                     rbRightToeBase,
                                     rbLeftToeBase,
                                     goalUpPlane,
                                     ref initPreShot, ref initVolleyShot,
                                     endPos,
                                     endPosOrg,
                                     ref isUpdateBallPosActive,
                                     ref updateBallPos,
                                     ref updateBallPosName,
                                     false);

            if (shotActive == true)
            {
                if (isShotOnTarget(endPosOrg, goalSizesCpu))
                {
                    matchStatistics.setShotOnTarget("teamA");
                }
            }
        }
 
        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (preShotActive ||
            shotActive ||
            (!isAnyAnimationPlaying &&
             !checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 1.00f, "3D_run_")))
        {
            /*TODELETE*/
            if (!gkLock)
            {

                if ((!isPlaying(animator, "3D_run", 1.0f) &&
                     playerDirection == Vector3.zero) &&
                    !preShotActive &&
                    !shotActive &&
                    !isAnyAnimationPlaying &&
                    (ballRb.transform.position.z > 0.0f))
                {
                    Vector3 lookPoint =
                            calcGkCorrectRotationToBall(
                                //cpuPlayer.getBallInit(),
                                Vector3.zero,
                                rb,
                                ref rotatedRbToBall,
                                gkCornerPoints);

                
                        if (ballRb.transform.position.z > 0.0f)
                        {
                            lookPoint =
                            calcGkCorrectRotationToBall(
                                ball.transform.position,
                                rb,
                                ref rotatedRbToBall,
                                gkCornerPoints);
                            RblookAtWSPoint(rb, lookPoint);
                        }                    
                }
                else
                {
                    /*volley has a separeate rotation*/
                    if (!isPlaying(animator, "3D_volley", 1.0f) &&
                        !isPlaying(animator, "3D_volley_before", 1.0f))
                    {
                        RblookAt(rb,
                                 onBall,
                                 playerDirection,
                                 animator,
                                 preShotActive | shotActive,
                                 Vector3.zero,
                                 false,
                                 shotType);
                    }
                }
            }
        }

        if (shotActive)
        {
            //print("GKDEBUG5 SHOTACTIVE RET " + shotRet);

            touchLocked = true;
            touchCount = 0;
            isTouchBegin = false;

            /*print("Distance MOVING");
            print("DRAWTIME END " + drawTimeEnd + " DRAWTIME START " + drawTimeStart + " DELTA " + (drawTimeEnd - drawTimeStart));
            print("DRAWTIMEOFBALL TIMEOFBALL " + timeofBallFly);
            print("VECTORT START " + startPos);
            print("VECTORT END " + endPos);*/

            //print("GKDEBUG7 GKDEBUG5 INITSHOT " + initShot);
            if (!initShot)
            {
                /*RESOULTIONCHANGE*/
                float height = drawDistance / 80.0f;

                //print("DEBUG123CDALOCERROR ballrb.position " + ballRb.position
                //    + " transform " + ballRb.transform.position);

                ballInitPos = ballRb.transform.position;
                initShot = true;

                float deltaTime = drawTimeEnd - drawTimeStart;

                /*Speed of ball beetween 0 and ShotSpeedMax */
                timeofBallFly = (drawDistance / deltaTime) / 1.25f;
                //print("DEBUG1234 TIMEOFBALL SPEED " + timeofBallFly);

                /*TODO LEVELS SPEED */
                /*CUT MAX SPEED TOO SOMETHING LOWER THAN SHOT SPEED MAX FOR WEAKER PLAYERS*/
                timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
                timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);

                //print("DEBUG1234 TIMEOFBALL SPEED AFTER CUT " + timeofBallFly);


                if (isLobActive)
                {
                    //timeofBallFly = 1200f;
                    //timeofBallFly = 1600f;
                    timeofBallFly = UnityEngine.Random.Range(500.0f, 700.0f);
                    //endPosOrg.x = getRandFloat(-3.0f, 3.0f);
                    //endPosOrg.y = getRandFloat(0.0f, 2.2f);
                    //endPosOrg.z = 14.0f;
                }

                //print("TIMOFBALLXX BEFORE " + timeofBallFly + " DRAWDISTANCE " + drawDistance + " DELTATIME " + deltaTime);

                /*Than higher timeofBallFly than ball fly slower */
                //print("TIMOFBALLXX AFTER " + timeofBallFly + " shotSpeed " + shotSpeed);

                /*max speed should be 120 in km/h */
                /*speed multiplayer should be beetween [0-80] */
                shotSpeed = MIN_SHOT_SPEED + Mathf.InverseLerp(ShotSpeedMin, ShotSpeedMax, timeofBallFly) * speedMultiplayer;
                shotBar.fillAmount = 1.0f - Mathf.InverseLerp(120f, 0f, shotSpeed);

                speedShotText.text = ((int)shotSpeed).ToString() + " km/h";

                //Invoke("eraseShotBar", 1.3f);
                passedShotFlyTime = 0.0f;
                //print("TIMEOFBALLFLYXZ BEFORE " + timeofBallFly);
                //timeofBallFly = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly) * 1000.0f;
                //print("TIMEOFBALLFLYXZ2 BEFORE " + timeofBallFly);

                //timeofBallFly = Mathf.Min(timeofBallFly, ShotCurveSpeedMaxTime);
                //timeofBallFly = Mathf.Max(timeofBallFly, ShotCurveSpeedMinTime);

                matchStatistics.setShot("teamA");
                //print("GET SHOT " + matchStatistics.getShot("teamA"));

                //print("DEBUG1234 TIMEOFBALL BEFORE " + timeofBallFly);
                timeofBallFly = ShotSpeedMin + (ShotSpeedMax - timeofBallFly);
                //print("DEBUG1234 TIMEOFBALL AFTER FLY " + timeofBallFly);

                timeofBallFly =
                    timeOfBallFlyBasedOnPosition(rb.transform.position, timeofBallFly);
                //print("DEBUG1234 TIMEOFBALL AFTER " + timeofBallFly);

                //print("TIMEOFBALLFLYXZ AFTER " + timeofBallFly);


                //print("OUTBALLVELBEFORE " + outShotEnd + " SHOTV " + shotvariant);
                //print("CURVESTARTPOS3XYZ " + curveStartPos3 + " MID " + curveMidPos3 + " curveEndPos3 " + curveEndPos3);
                preShotCalc(curveStartPos3,
                            curveMidPos3,
                            curveEndPos3,
                            endPosOrg,
                            height,
                            ballInitPos,
                            shotSpeed,
                            isLobActive,
                            ref outShotBallVelocity,
                            ref outShotStart,
                            ref outShotMid,
                            ref outShotEnd,
                            ref shotvariant,
                            false);


                //print("GKDEBUG7 BallRBVELOCITY VELOCITY AFTER PREPARE SHOT CALC " + ballRb.velocity + " SHOTACTIVE " + shotActive);

                //print("GKDEBUG7 OUT VELOCITY " + outShotBallVelocity + " OUTSHOTSTART "
                //    + outShotStart + " OUTSHOTMID " + outShotMid + " OUTSHOTEND " + outShotEnd + " SHOTVARIANT " + shotvariant);
            }

            //print("POSITIONVECTOR" + startPos + " MID " + midPos + " END " + endPos);

            /*TOCHECK*/
            //print("GKDEBUG7 GKDEBUG5 SHOTRET BEFORE " + shotRet);
            if (!shotRet) {
                float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);
                //print("factorTimeFlyNorm " + timeofBallFly + " TIME " + passedShotFlyTime + " NORMALIZE TIME " + normalizeTime);
                //print("NORMALIZE TIME " + normalizeTime);
                shotRet = shot3New(outShotStart,
                                   outShotMid,
                                   outShotEnd,
                                   outShotBallVelocity,
                                   ref lastBallVelocity,
                                   shotvariant,
                                   normalizeTime);

                //print("GKDEBUG7 BallRBVELOCITY VELOCITY AFTER PREPARE SHOT ACTIVE " + ballRb.velocity + " SHOTACTIVE " + shotActive);
                //print("GKDEBUG7 GKDEBUG5 SHOTRET " + shotRet);
                //print("DEBUG1 STARTHR timeofBallFlyNORM " + timeofBallFly);
                if (!isBallTrailRendererInit)
                //&& shotSpeed > 72.0f)
                {
                    //print("SHOTSPEED " + shotSpeed);

                    if (timeOfBallFlyBasedOnPositionReverse(
                            rb.transform.position, timeofBallFly) <
                       (ShotSpeedMin + 50.0f))
                    {
                        ball.ballTrailRendererInit();
                        audioManager.Play("ballFastSpeed");
                    }

                    //else {
                    audioManager.Play("kick2");
                    //}

                    isBallTrailRendererInit = true;
                }
            }

            passedShotFlyTime = passedShotFlyTime + (Time.deltaTime * 1000.0f);
            //print("TIMEEXEC " + passedShotFlyTime);
        }

    }

    private void displayEventInfo()
    {
        if (ball.getBallGoalCollisionStatus())
        {
            if (ball.whoScored() == 2)
            {
                //if (((Time.time - gkTimeLastCatch) < 1.0f))
                //    printGameEventsInfo(gkAction);
                printGameEventsInfo("GOAL!");
            }
            else
            {
                printGameEventsInfo("GOAL!");
            }
        }
        else
        {
            if (timeToShotExceeded)
            {
                printGameEventsInfo("TIME'S UP!");
                audioManager.Play("whistle1");
            }

            if (isBallOutOfPitch())
            {
                printGameEventsInfo("OUT!");
                //    audioManager.Play("whistle1");
            }
        }
    }

    public IEnumerator flashBackground(float delayTime)
    {
        float offset = 0.2f;
        Color fadeColor = new Color();
        ColorUtility.TryParseHtmlString("#C36868", out fadeColor);

        fadeColor.a = 0.6f;
        for (int i = 0; i < 1; i++)
        {
            fadeColor.a += 0.1f;
            flashBackgroundImage.color = fadeColor;
            yield return new WaitForSeconds(delayTime);
        }

        fadeColor.a = 0.0f;
        flashBackgroundImage.color = fadeColor;
    }

    IEnumerator setBallPositionFlash(float delayTime)
    {
        float offset = 0.2f;
        //print("Executed setBallPositionFlash " + ballPositionLock);

        ballPositionLock = true;
        Color fadeColor = new Color();
        ColorUtility.TryParseHtmlString("#C36868", out fadeColor);

        //Color fadeColor = Color.white;
        fadeColor.a = 0.6f;
        for (int i = 0; i < 1; i++) {
            fadeColor.a += 0.1f;
            flashBackgroundImage.color = fadeColor;
            yield return new WaitForSeconds(delayTime);
        }

        clearVariables();
        setBallPosition();
        fadeColor.a = 0.0f;
        flashBackgroundImage.color = fadeColor;

        ballPrevPosition = ballRb.transform.position;
        ball.setBallGoalCollisionStatus(false);
        ballPositionLock = false;

        //print("Executed setBallPositionFlash END COURUTINE" + ballPositionLock);

    }


    private void rotateHead(Rigidbody rb,
                            GameObject head,
                            Vector3 pointToLookAt,
                            bool isShotActive,
                            float rotationSpeed)
    {
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        Vector3 lookDirection = Vector3.zero;
        float maxHeadAngle = 110.0f;

        //print("DEBUGLAKILUCK334XYYZ head euler angle " + head.transform.rotation.eulerAngles);
        if (!isShotActive &&
            head.transform.rotation.eulerAngles.y < 1.0f)
            return;

        if (isShotActive)
        {
            lookDirection = (pointToLookAt - head.transform.position).normalized;
            lookDirection.y = 0.0f;

            lookOnLook = Quaternion.LookRotation(lookDirection);
            float angle = Quaternion.Angle(head.transform.rotation, lookOnLook);
            //print("DEBUGLAKILUCK334XYYZ ANGLE BEETWEEN QUATERIONS " + angle);
            if (angle < 2.0f)
                return;

        } else
        {
            lookOnLook = Quaternion.Euler(0f,
                                          rb.transform.rotation.eulerAngles.y,
                                          0f);
        }

        head.transform.rotation =
            Quaternion.Slerp(head.transform.rotation, lookOnLook, Time.deltaTime * rotationSpeed);
    }

    public float shotLastDistFromMidLine()
    {
        return lastDistFromMidLine;
    }

    private void goBackwardToPoint(Rigidbody rb,
                                   Animator animator,
                                   Vector3 pointToGo,
                                   float speed)
    {
        string animName = "3D_back_run_cpu";

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName) ||
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.00f)
        {
            animator.Play(animName, 0, 0.0f);
            //print("DEBUG1234AAAC PLAYANIMBACK " + animName);
            animator.Update(0f);
        }

        Vector3 towardsNewPos = new Vector3(pointToGo.x - rb.position.x,
                                            0.0f,
                                            pointToGo.z - rb.position.z);

        towardsNewPos = towardsNewPos.normalized;
        interruptSideAnimation(animator, rb);
        rb.velocity = towardsNewPos * speed;
        //print("DEBUG1234AAAC RB VEL " + rb.velocity);
    }

    public Vector3 getOutShotStart()
    {
        return outShotStart;
    }

    public Vector3 getOutShotMid()
    {
        return outShotMid;
    }

    public Vector3 getOutShotEnd()
    {
        return outShotEnd;
    }

    public float getTimeOfBallFly()
    {
        return timeofBallFly;
    }

    public float getPassedTime()
    {
        return passedShotFlyTime;
    }


    public Vector3 getEndPosOrg()
    {
        return endPosOrg;
    }

    public bool isLobShotActive()
    {
        return isLobActive;
    }

    private void playerRun()
    {

        float horizontalMovement = joystick.Horizontal();
        float verticalMovement = joystick.Vertical();

        if (isRunPaused())
        {
            horizontalMovement = 0f;
            verticalMovement = 0f;
        }

        /* setup speed of Run animation depending on a joystick position */
        //float runSpeed = Mathf.Max(Mathf.Abs(horizontalMovement),
        //                           Mathf.Abs(verticalMovement));

            playerDirection = new Vector3(horizontalMovement, 0.0f, verticalMovement);

            isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
            if (!isAnyAnimationPlaying)
            {
                bool isOnBall = isPlayerOnBall(
                              rbLeftToeBase,
                              rbRightToeBase,
                              ballRb,
                              rb,
                              "move",
                              false);
   
                    playerOnBallMoves(rb,
                                      isOnBall, 
                                      ref playerDirection,
                                      runningSpeed); 
            }
    }

    private void setupRunningAnimSpeed()
    {
        float runSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x),
                                   Mathf.Abs(rb.velocity.z));
        runSpeed = Mathf.InverseLerp(0.0f, MAX_PLAYER_SPEED, runSpeed);

        float runSpeedVal = Mathf.Clamp((1.25f + Mathf.Abs(runSpeed)), 1.25f, 2.2f);
        animator.SetFloat("3d_run_speed", runSpeedVal);

        //print("runSpeed " + runSpeedVal + " RB VELOCITY " + rb.velocity);
    }

    private void setupTurnSpeedAnim()
    {

        float runSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x),
                                   Mathf.Abs(rb.velocity.z));
        runSpeed = Mathf.InverseLerp(0.0f, MAX_PLAYER_SPEED, runSpeed);

        float runSpeedVal = Mathf.Clamp((1.15f + Mathf.Abs(runSpeed / 1.5f)), 1.15f, 1.65f);
        animator.SetFloat("3d_run_turn_speed", runSpeedVal);
        //print("runSpeed turn " + runSpeedVal);
    }

    private void playerOnBallMoves(Rigidbody rb,
                                   bool isOnBall, 
                                   ref Vector3 runDirection,
                                   float runSpeed)
    {
        if (!isOnBall ||
            isBallInGame == false)
        {
            //if (!isPlaying(animator, "3D_run", 1f) &&
            if (!isPlaying(animator, "3D_run", 1f) &&                
                 runDirection != Vector3.zero)   
            {                
                animator.Play("3D_run", 0, 0.0f);
                audioManager.Play("run2");                
            }

            float ballRbDist =
                Vector2.Distance(new Vector2(ballRb.transform.position.x, ballRb.transform.position.z),
                                 new Vector2(rb.transform.position.x, rb.transform.position.z));

            if (runDirection != Vector3.zero &&
                isBallInGame &&
                ballRbDist < 2f &&
                ballRb.transform.position.y < 1.0f)
            {
                runDirection = (ballRb.transform.position - rb.transform.position).normalized;
                runDirection.y = 0;
            }

            rb.velocity = runDirection * runSpeed;
            setupRunningAnimSpeed();

            lastPlayerMovePosHead = 0;
            lastPlayerMovePosTail = 0;
            return;
        }

        bool isTurnPossible = false;
        int listHead = lastPlayerMovePosHead;
        int listTail = lastPlayerMovePosTail;
        string animName = "3D_run";
        string lastAnimTurnPlayed = "empty";
        //print("DEBUGXYKJA HEAD IDX " + lastPlayerMovePosHead + " TAIL IDX " + lastPlayerMovePosTail);
        //print("DEBUGXYKJA START");

        float maxAngle = 0;
        float minAngle = 0;
        float angle;
        int angleIdx = 0;

        while (listHead != listTail)              
        {
            angle = Vector2.SignedAngle(new Vector2(runDirection.x, runDirection.z),
                                        new Vector2(lastPlayerMovePos[listHead, 0].x,
                                                    lastPlayerMovePos[listHead, 0].z));

            /*print("DEBUGXYKJA lastPlayerMovePos " + ": " + listHead 
                + lastPlayerMovePos[listHead, 0].ToString("F4") 
                + " TIME " + lastPlayerMovePos[listHead, 1].ToString("F4")
                + " PLYAERDIR " + runDirection.ToString("F4") + " ANGLE " + angle.ToString("F4"));*/

            if (angle > maxAngle)
            {
                maxAngle = angle;
                angleIdx = listHead;
            }

            if (angle < minAngle)
            {
                minAngle = angle;
                angleIdx = listHead;
            }

        
            maxAngle = Mathf.Max(angle, maxAngle);
            listHead++;
            listHead %= lastPlayerMovePos.GetLength(0);
        }

        if (maxAngle > Mathf.Abs(minAngle))
        {
            angle = maxAngle;
        } else
            angle = minAngle;

        bool isAnyTurnAnimPlaying = 
            checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 0.99f, "3D_run_");

        //print("DEBUG2NONCPU345ANIMPLAY isAnyRunAnimPlaying " + isAnyRunAnimPlaying);
        if (!isAnyTurnAnimPlaying)
        {
            animName = 
                convertAngleToAnimNameRunOnBall(rb, angle, false);           

            if (animName.Contains("3D_run_") &&
                (Time.time - lastPlayerMovePos[angleIdx, 1].x > 0.1f))
            {
                /*print("DEBUGXYKJA PLAY ### " + animName + " TIME " + Time.time.ToString("F4") +
                    " angleIDX " + angleIdx + " CURRTIME " + Time.time + 
                    " ANGLETIME " + lastPlayerMovePos[angleIdx, 1].x);*/

                //float animSpeed = Mathf.Max(Mathf.Abs(lastPlayerMovePos[angleIdx, 0].x),
                //                            Mathf.Abs(lastPlayerMovePos[angleIdx, 0].z));



                //print("anim name played " + animName);
                lastAnimTurnPlayed = animName;
                animator.Play(animName, 0, 0.0f);
                isAnyTurnAnimPlaying = true;                
                animator.Update(0f);
                audioManager.Play("run2");
                lastPlayerMovePosHead = 0;
                lastPlayerMovePosTail = 0;
            }
            else
            {
                if (!isPlaying(animator, "3D_run", 1.0f) &&
                   (playerDirection.x != 0f || playerDirection.y != 0f))
                {
                    animator.Play("3D_run", 0, 0.0f);
                    audioManager.Play("run2");
                }
            }
        }         
    
        /*TODELETE*/
        /*print("playerDirection rb.transform.forward "
            + rb.transform.forward.ToString("F4") + " Time " + Time.time + " PLAYERDIRECTIN "
            + runDirection.ToString("F4"));*/

        //if (runDirection.x != 0f || runDirection.z != 0f) {
        //if (!isAnyRunAnimPlaying)
        //{
            lastPlayerMovePos[lastPlayerMovePosTail, 0] = runDirection;
            lastPlayerMovePos[lastPlayerMovePosTail, 1] = new Vector3(Time.time, 0f, 0f);
            //lastPlayerMovePos[lastPlayerMovePosTail, 2] = rb.velocity;


        /*    print("DEBUGXYKJA ADD : " + lastPlayerMovePosTail + " RUNDIRECTION " + runDirection.ToString("F4")
        + " lastPlayerMovePosTail " + lastPlayerMovePosTail); */
            lastPlayerMovePosTail++;
            lastPlayerMovePosTail %= lastPlayerMovePos.GetLength(0);
           

        //print("playerDirection lastPlayerMovePosTailX BEFORE " + lastPlayerMovePosTail);
        //print("playerDirection lastPlayerMovePosTailX AFTER " + lastPlayerMovePosTail
        //    + " lastPlayerMovePos.Length " + lastPlayerMovePos.GetLength(0));

        if (lastPlayerMovePosHead == lastPlayerMovePosTail)
        {
            lastPlayerMovePosHead++;
            lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
        }

        isUpdateBallPosActive = true;
        updateBallPosName = "bodyMain";

        if (Time.time - lastPlayerMovePos[lastPlayerMovePosHead, 1].x > 0.10f)// &&
            //lastPlayerMovePosHead != lastPlayerMovePosTail)
        {
            //print("DEBUGXYKJA VELOCITY TAKEN " + lastPlayerMovePos[lastPlayerMovePosHead, 0]);
         
            //rb.velocity = lastPlayerMovePos[lastPlayerMovePosHead, 0] * runSpeed;
            if (isAnyTurnAnimPlaying)
            {
                string turnAnim = lastAnimTurnPlayed;
                if (lastAnimTurnPlayed.Equals("empty"))
                {
                    turnAnim = nameAnimationPlaying(animator, 1.0f, 2);
                }

                //90turn
                //float minNormTime = 0.78f;
                float minNormTime = 0.60f;
                if (turnAnim.Contains("180turn"))
                {
                    minNormTime = 0.85f;
                } else if (turnAnim.Contains("135turn"))
                {
                    minNormTime = 0.80f;
                }
               
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= minNormTime)    
                {
                    rb.velocity = lastPlayerMovePos[lastPlayerMovePosHead, 0] * runSpeed;
                    rb.velocity /= 1.15f;
                    //print("RUN animator.GetCurrentAnimatorStateInfo(0).normalizedTime "
                    //    + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                }
                setupTurnSpeedAnim();
            }
            else
            {
                rb.velocity = lastPlayerMovePos[lastPlayerMovePosHead, 0] * runSpeed;
                rb.velocity /= 1.15f;
                setupRunningAnimSpeed();
            }

            lastPlayerMovePosHead++;
            lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
        }
    }

    private string convertAngleToAnimNameRunOnBall(Rigidbody rb, 
                                                   float angle,
                                                   bool isCpu)
    {
        bool rbRestSpeed = false;
        string animName = "3D_run";
        bool isOutAnim = false;
        int randOutAnim = UnityEngine.Random.Range(0, 2);

        if (randOutAnim == 1)
        {
            isOutAnim = true;
        }

        if (isCpu)
            animName = "3D_run";

        if (Mathf.Abs(rb.velocity.x) < 3f &&
            Mathf.Abs(rb.velocity.z) < 3f)
        {
            rbRestSpeed = true;
        }
    
        if (Mathf.Abs(angle) > 125f && 
            Mathf.Abs(angle) < 145f)
            //&&
            //isCpu)
        {
            animName = "3D_run_135turn_in_";
            //if (isCpu && isOutAnim)
            if (isOutAnim)
                    animName = "3D_run_135turn_out_";
        }

        if (Mathf.Abs(angle) > 70f && Mathf.Abs(angle) < 110f)
        {
            animName = "3D_run_90turn_in_";
            if (isOutAnim)
                animName = "3D_run_90turn_out_";
        }

        if (Mathf.Abs(angle) > 40f && 
            Mathf.Abs(angle) < 50f &&
            isCpu)
        {
            animName = "3D_run_45turn_in_";
            if (isOutAnim)
                animName = "3D_run_45turn_out_";    
        }

        if (Mathf.Abs(angle) > 170f &&
            isCpu)
        {
            animName = "3D_run_180turn_out_";
        }

        if (!animName.Equals("3D_run"))
        {
            if (angle < 0)
            {
                animName += "left";
            }
            else
                animName += "right";
        }
     
        return animName;
    }

    private void setupCanvasGameObjects()
    {
        //overheadButtonGameObject = GameObject.Find("overhead_button");
        lobButtonGameObject = GameObject.Find("lob_button");
        volleyButtonGameObject = GameObject.Find("volley_button");
        cameraButtonGameObject = GameObject.Find("camera_button");
        cameraRectTrans = cameraButtonGameObject.GetComponent<RectTransform>();
        /*
         * https://answers.unity.com/questions/931514/canvas-scaler-math-behind-scale-with-screen-size.html
         */
        cameraPos = new Vector3(cameraRectTrans.position.x - (screenWidth * 0.03f),
                                0f,
                                0f);
        /*WORKAROUD*/
        /*camera is 100 by 100 on canvas with reference resolution 1920x1080*/
        cameraPos.x -= 
            (cameraRectTrans.rect.width * UICanvas.scaleFactor) / 2;

        ///print("UPDATTOUCH145  cameraRectTrans.rect.xMin ## " 
       //     + cameraRectTrans.rect.width + " UICanvas.scaleFactor " + UICanvas.scaleFactor);

        lobButtonTextGameObject = GameObject.Find("lobButtonText");
        volleyButtonTextGameObject = GameObject.Find("volleyButtonText");
        cameraButtonTextGameObject = GameObject.Find("cameraButtonText");

        timePanelGameObject = GameObject.Find("timePanel");
        timeImageGameObject = GameObject.Find("timeToShotBackgroundImage");
        mainTimeTextGameObject = GameObject.Find("mainTimeText");
        timeToShotBallImageGameObject = GameObject.Find("mainTimeBackgroundImage");
        flashBackgroundImage = GameObject.Find("flashBackground").GetComponent<Image>();
        flagPanelGameObject = GameObject.Find("flagPanel");
        joystickGameObject = GameObject.Find("joystick");
        //matchIntroPanel = GameObject.Find("matchIntroPanel");
        //matchIntroFlagPanel = GameObject.Find("matchIntroFlagPanel");
        //matchIntroPanelBackground = GameObject.Find("matchIntroPanelBackground");
        //matchIntroPanelHeaderTop = GameObject.Find("matchIntroPanelHeaderTop");
        //matchIntroPanelHeaderDown = GameObject.Find("matchIntroPanelHeaderDown");
        //matchIntroWinCoinsNumText = GameObject.Find("winCoinsNumText").GetComponent<TextMeshProUGUI>();
        //matchIntroTieNumCoinText = GameObject.Find("tieNumCoinText").GetComponent<TextMeshProUGUI>();
        pauseCanvas = GameObject.Find("pauseCanvas");
        //stadiumPeople = GameObject.Find("st_040_people");
        stadium = GameObject.Find("st_040_stadium");

        shotBarBackground = GameObject.Find("shotBarBackground");
        refereeBackground = GameObject.Find("refereeBackground");

        try
        {
            matchStatisticsPanel = GameObject.Find("matchStatisticsPanel");
        } catch (NullReferenceException ex)
        {
            Debug.Log("matchStatisticsPanel is null");
        }

        try
        {
            matchStatisticsFlagPanel = GameObject.Find("matchStatisticsFlagPanel");
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("matchStatisticsFlagPanel is null");
        }

        try
        {
            matchStatisticsPanelBackground = GameObject.Find("matchStatisticsPanelBackground");
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("matchStatisticsPanelBackground is null");
        }

        try
        {
            matchStatisticsPanelHeaderTop = GameObject.Find("matchStatisticsPanelHeaderTop");
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("matchStatisticsPanelHeaderTop is null");
        }

        try
        {
            matchStatisticsPanelHeaderDown = GameObject.Find("matchStatisticsPanelHeaderDown");
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("matchStatisticsPanelHeaderDown is null");
        }

        teamAgoalsText = GameObject.Find("teamAgoalsText");
        teamBgoalsText = GameObject.Find("teamBgoalsText");
        teamAshotsText = GameObject.Find("teamAshotsText");
        teamBshotsText = GameObject.Find("teamBshotsText");
        teamAshotsOnTargetText = GameObject.Find("teamAshotsOnTargetText");
        teamBshotsOnTargetText = GameObject.Find("teamBshotsOnTargetText");
        teamAsavesText = GameObject.Find("teamAsavesText");
        teamBsavesText = GameObject.Find("teamBsavesText");
        teamAballPossessionText = GameObject.Find("teamAballPossessionText");
        teamBballPossessionText = GameObject.Find("teamBballPossessionText");

        //matchStatisticsNext = GameObject.Find("matchStatisticsNext");
        //matchStatisticsCoinsRewareded = 
        //    GameObject.Find("matchStatisticsCoinsRewarded").GetComponent<TextMeshProUGUI>();
        //matchStatisticsDiamondsRewareded = 
        //    GameObject.Find("matchStatisticsDiamondsRewarded").GetComponent<TextMeshProUGUI>();

        traningPanel = GameObject.Find("traningPanel");
        //shotBarGameObject = GameObject.Find("shotBar");
        //shotBarIconGameObject = GameObject.Find("shotBarIcon");
    }

    public Vector3 getGoalSizePlr1()
    {
        return goalSizes;
    }

    public Vector3 getGoalSizePlr2()
    {
        return goalSizesCpu;
    }

    public void activateCanvasElements()
    {
        volleyButtonGameObject.SetActive(true);
        //overheadButtonGameObject.SetActive(true);
        lobButtonGameObject.SetActive(true);
        cameraButtonGameObject.SetActive(true);

        if (!isTrainingActive)
            timePanelGameObject.SetActive(true);
        if (!isTrainingActive)
            flagPanelGameObject.SetActive(true);
        joystickGameObject.SetActive(true);
        joystickBgGameObject.SetActive(true);
        gkClickHelperGameObject.SetActive(true);
        gkHelperImageGameObject.SetActive(true);
        joystickBgGameObject.SetActive(true);

        //shotBarGameObject.SetActive(true);
        //shotBarIconGameObject.SetActive(true);
        shotBarBackground.SetActive(true);
        refereeBackground.SetActive(true);
        if (isTrainingActive)
            traningPanel.SetActive(true);
    }

    public void deactivateCanvasElements()
    {
        //overheadButtonGameObject.SetActive(false);
        //lobButtonGameObject.SetActive(false);
        //cameraButtonGameObject.SetActive(false);
        //volleyButtonGameObject.SetActive(false);
        //timePanelGameObject.SetActive(false);
        //flagPanelGameObject.SetActive(false);
        //joystickGameObject.SetActive(false);
        //matchIntroPanelHeaderTop.SetActive(false);
        //matchIntroPanelHeaderDown.SetActive(false);
        //matchIntroFlagPanel.SetActive(false);
        //matchIntroPanel.SetActive(false);
        //matchIntroPanelBackground.SetActive(false);

        //matchStatisticsNext.SetActive(false);
        //matchStatisticsPanel.SetActive(false);
        //matchStatisticsFlagPanel.SetActive(false);
        //matchStatisticsPanelBackground.SetActive(false);
        //matchStatisticsPanelHeaderTop.SetActive(false);
        //matchStatisticsPanelHeaderDown.SetActive(false);
    
        //shotBarBackground.SetActive(false);
        //refereeBackground.SetActive(false);
        //traningPanel.SetActive(false);
        pauseCanvas.SetActive(false);
        //if (isTrainingActive)
        //{
        //    foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        //    {
        //        if (allStadiumPeople.name.Contains("crowdAnimated"))
        //        {
        //            allStadiumPeople.SetActive(false);
        //        }
        //    }
        //}
    }

    private void addCoins()
    {
        if ((Globals.score1 > Globals.score2 && !Globals. playerPlayAway) ||
            (Globals.score2 > Globals.score1 && Globals.playerPlayAway))
        {
            Globals.addCoins(winCoinsRewarded);
            Globals.addDiamonds(winCoinsRewarded);

            matchStatisticsCoinsRewareded.text = "+" + winCoinsRewarded.ToString();
            matchStatisticsDiamondsRewareded.text = "+" + winCoinsRewarded.ToString();
        }
        else
        {
            if (Globals.score1 == Globals.score2) {
                Globals.addCoins(tieCoinsRewarded);
                Globals.addDiamonds(tieCoinsRewarded);
                matchStatisticsCoinsRewareded.text = "+" + tieCoinsRewarded.ToString();
                matchStatisticsDiamondsRewareded.text = "+" + tieCoinsRewarded.ToString();
            }
            else
            {
                matchStatisticsCoinsRewareded.text = "+0";
                matchStatisticsDiamondsRewareded.text = "+0";
            }
        }
    }

    private bool isTooCloseToWalls(Vector3 pos, Vector3 minDist)
    {
        if ((PITCH_WIDTH_HALF - Math.Abs(pos.x) <= minDist.x) ||
            (PITCH_HEIGHT_HALF - Mathf.Abs(pos.z) <= minDist.z) ||
            (pos.z <= minDist.z))
            return true;
        return false;    
    }

    /* This function check if player is onBall */
    private bool isPlayerOnBall(GameObject rbLeftToeBase,
                                GameObject rbRightToeBase,
                                Rigidbody ballRb,
                                Rigidbody rb,
                                string shotType,
                                bool isCpu)
    {
        float distance = 0.0f;
        float minDistance = minDistToBallShot;
        //float maxYDist = maxYdistToBallShot;
        /*CRITICAL CHANGE*/
        float maxYDist = BALL_RADIUS;

        Vector2 ball2D = new Vector2(ballRb.transform.position.x, ballRb.transform.position.z);
        Vector2 rbLeftToe2D = new Vector2(rbLeftToeBase.transform.position.x, rbLeftToeBase.transform.position.z);
        Vector2 rbRightToe2D = new Vector2(rbRightToeBase.transform.position.x, rbRightToeBase.transform.position.z);
        Vector2 midPos2D = new Vector2(rb.transform.position.x, rb.transform.position.z);

        /*you cannot take ball that is on opponet half*/
        if (!isBallReachable(ballRb.transform.position, isCpu))
            return false;

        /*Check if ball is in front of player. Convert both rb and ballRb to local and check z coordinate of ball */
        if (!checkRelativeBallRigidBodyPos(rb, ballRb))
            return false;

        Vector3 pos = rb.transform.position;
        /*For a normal move take mid of rigidbody*/
        distance = Vector2.Distance(midPos2D,
                                    ball2D);

        if (shotType.Contains("3D_shot_left_foot")) {
            minDistance += 0.2f;
            distance = Vector2.Distance(rbLeftToe2D,
                                        ball2D);
            //if (!isCpu && !shotActive && preShotActive)
            //       print("DBGNORMAL LEFT FOOT DISTANCE " + distance);

            pos = rbLeftToeBase.transform.position;
        } else if (shotType.Contains("3D_shot_right_foot") || shotType.Contains("3D_volley")) {
            distance = Vector2.Distance(rbRightToe2D,
                                        ball2D);

            if (shotType.Contains("3D_shot_right_foot")) {
                minDistance += 0.2f;
            }

            //if (shotType.Contains("3D_shot_right_foot") && !isCpu && !shotActive && preShotActive)
            //{
            //    print("DBGNORMAL RIGHT FOOT DISTANCE " + distance);
            //}

            pos = rbRightToeBase.transform.position;
            if (shotType.Contains("3D_volley"))
            {
                //minDistance += 1.3f;
                //maxYDist += 0.2f;
                /*special case*/
                float zMin = 1.1f;
                if (isTooCloseToWalls(rb.transform.position, new Vector3(1.25f, 0f, 1.25f)))
                    zMin += 0.4f;

                Vector3 localRbRightToeBase =
                    getLocalRigidBodyObjectPos(rb, rbRightToeBase.transform.position);

                Vector3 localBallPos =
                   getLocalRigidBodyObjectPos(rb, ballRb.transform.position);

                /*if (!isCpu)
                {
                    if (preShotActive && !shotActive)
                        print("CPUMOVEDEBUG123X_NOCPU ONBALL " + distance + " BALLVEL " + ballRb.velocity + " RB TRANSFORM "
                          + rb.transform.position + " MINDISTANCE " + minDistance
                          + " DISTANCE " + distance
                          + " localBallPos "
                          + localBallPos.ToString("F4")
                          + " localRbRightToeBase "
                          + localRbRightToeBase.ToString("F4")
                          + " LOCAL X DIFF " + (Mathf.Abs(localBallPos.x - localRbRightToeBase.x))
                          + " LOCAL Z DIFF " + (Mathf.Abs(localBallPos.z - localRbRightToeBase.z))
                          + " maxYDist " + maxYDist + " YDIST "
                          + Mathf.Abs(pos.y - ballRb.transform.position.y)
                          + " BALLRB " + ballRb.transform.position
                          + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                          + " animator.normalizedTime "
                          + animator.GetCurrentAnimatorStateInfo(0).normalizedTime); ;
                } 
                else
                {
                    if (cpuPlayer.getPreShotActive() && !cpuPlayer.getShotActive())
                        print("CPUMOVEDEBUG123X_CPU ONBALL " + distance + " BALLVEL " + ballRb.velocity + " RB TRANSFORM "
                          + rb.transform.position + " MINDISTANCE " + minDistance
                          + " DISTANCE " + distance
                          + " localBallPos "
                          + localBallPos.ToString("F4")
                          + " localRbRightToeBase "
                          + localRbRightToeBase.ToString("F4")
                          + " LOCAL X DIFF " + (Mathf.Abs(localBallPos.x - localRbRightToeBase.x))
                          + " LOCAL Z DIFF " + (Mathf.Abs(localBallPos.z - localRbRightToeBase.z))
                          + " maxYDist " + maxYDist + " YDIST "
                          + Mathf.Abs(pos.y - ballRb.transform.position.y)
                          + " BALLRB " + ballRb.transform.position
                          + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                          + " animator.normalizedTime "
                          + cpuPlayer.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime);
                }*/

                if (Mathf.Abs(pos.y - ballRb.transform.position.y) > 0.6f ||
                    Mathf.Abs(localBallPos.x - localRbRightToeBase.x) > 0.5f ||
                    Mathf.Abs(localBallPos.z - localRbRightToeBase.z) > zMin)
                    return false;
                else
                    return true;
            }
        }

        if (shotType.Contains("move"))
        {
            if (PITCH_WIDTH_HALF - Mathf.Abs(ballRb.transform.position.x) < 1.5f ||
                PITCH_HEIGHT_HALF - Mathf.Abs(ballRb.transform.position.z) < 1.5f ||
                Mathf.Abs(ballRb.transform.position.z) < 1.5f)
            {
                //minDistance += 0.7f;
                minDistance += BALL_NEW_RADIUS;
            }
        }

        /*if (shotType.Contains("volley"))
            if (isCpu)
                print("CPUMOVEDEBUGVOLLEY123XCPU ONBALL " + distance
                    + " BALL2d " + ball2D.ToString("F10")
                    + " RB TRANSFORM " + midPos2D.ToString("F10") + " MINDISTANCE " + minDistance
                    + " DISTANCE " + distance + " MIDPOS " + midPos2D
                    + " maxYDist " + maxYDist + " YDIST " + " ballRbPOS " + ballRb.position +
                    +Mathf.Abs(pos.y - ballRb.transform.position.y)
                    + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position + " POS " + pos);
            else
                if (preShotActive && !shotActive)
                print("CPUMOVEDEBUG123X_NOCPU ONBALL " + distance + " BALLVEL " + ballRb.velocity + " RB TRANSFORM "
                  + rb.transform.position + " MINDISTANCE " + minDistance
                  + " DISTANCE " + distance + " MIDPOS " + midPos2D
                  + " maxYDist " + maxYDist + " YDIST "
                  + Mathf.Abs(pos.y - ballRb.transform.position.y)
                  + " BALLRB " + ballRb.transform.position
                  + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                  + " animator.normalizedTime " 
                  + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);*/

        /*if (isCpu)
        print("SHOTTYPEXYZ " + distance + " BALL2d " + ball2D + " rbRightToe2D " + rbRightToe2D + " RB TRANSFORM " 
            + rb.transform.position +                    
            " Y " + Mathf.Abs(pos.y - ballRb.transform.position.y)  + " MID POS MAIN "
            + rb.transform.position + " SHOTTYPE " + shotType + " BALL POS " + ballRb.transform.position 
            + " minDistance " + minDistance + " maxYDist " + maxYDist + " RIGHT FOOT DIST " + 
            Vector2.Distance(new Vector2(
                              rbRightFoot.transform.position.x, rbRightFoot.transform.position.z),
                              ball2D));*/

                //if (isCpu)
                //    print("");
                //print("checkRelativeBallRigidBodyPos(rb, ballRb) CPU DIST " + distance + " Y DIST " + Mathf.Abs(pos.y - ballRb.transform.position.y));
                //else
                //    if (v)
                /*if (!isCpu && isPlaying(animator, "3D_volley", 1.0f))
                    print("VOLLEYDEBUGXY67 NO CPU DIST " + distance + " MINDIST " 
                        + minDistance + " Y DIST " + Mathf.Abs(pos.y - ballRb.transform.position.y) 
                        + " MAXYDIST " + maxYDist + " rbRightToeBase.transform.position " 
                        + rbRightToeBase.transform.position 
                        + " rballRb.transform.position " + ballRb.transform.position);*/

        if ((distance < minDistance) &&
            (Mathf.Abs(pos.y - ballRb.transform.position.y) < maxYDist))
        {
            return true;
        }

        return false;
    }

    private bool isBallReachable(Vector3 ballPos, bool isCpu)
    {
        if (isCpu)
        {
            if (ballPos.z < 0f)
                return false;
        }
        else
        {
            if (ballPos.z > 0f)
                return false;
        }

        return true;
    }

    private Vector3 getLocalRigidBodyObjectPos(Rigidbody rb, Vector3 objectWorldPos)
    {
        return InverseTransformPointUnscaled(rb.transform, objectWorldPos);

    }

    /*If the ball is in "front" return true. Otherwise false */
    private bool checkRelativeBallRigidBodyPos(Rigidbody rb, Rigidbody ballRb)
    {
        /*CHANGED SCALE */
        //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb.transform.position);
        Vector3 ballInLocalRb = InverseTransformPointUnscaled(rb.transform, ballRb.transform.position);

        //print("TRANSFORMHANDBALL " + ballInLocalRb);
        //print("GKDEBUG2 BALLLOCALPOS " + ballInLocalRb.z);

        if (ballInLocalRb.z < -0.3f)
            return false;

        return true;
    }

    private void clearTouch()
    {
        for (int i = 0; i < shotTypesNames.Count; i++)
        {
            if (isPlaying(animator, shotTypesNames[i], 1.0f))
            {
                touchCount = 0;
                gkTouchDone = false;
            }
        }

        if (isPlaying(animator, "3D_volley", 1.0f))
        {
            touchCount = 0;
            gkTouchDone = false;
        }
    }

    private void setJoystickPosition()
    {
        joystick.zeroPosition();

        joystickBgGameObject = GameObject.Find("joystickBG");
        joystickBG = GameObject.Find("joystickBG").GetComponent<RectTransform>();
        joystickButton = GameObject.Find("joystickButton").GetComponent<RectTransform>();

        gkMoveUpButtonRectTrans = GameObject.Find("gkMoveUpButton").GetComponent<RectTransform>();
        gkMoveDownButtonRectTrans = GameObject.Find("gkMoveDownButton").GetComponent<RectTransform>();
        gkSideRightButtonRectTrans = GameObject.Find("gkSideRightButton").GetComponent<RectTransform>();
        gkSideLeftButtonRectTrans = GameObject.Find("gkSideLeftButton").GetComponent<RectTransform>();

        joystickButtons.Add(gkMoveUpButtonRectTrans);
        joystickButtons.Add(gkMoveDownButtonRectTrans);
        joystickButtons.Add(gkSideRightButtonRectTrans);
        joystickButtons.Add(gkSideLeftButtonRectTrans);

        //float sizeOfJoystickCircleInside = Screen.width * 110.0f;
        /*Jopystick button occupies 7 % of width */
        //float joystickButtonWidth = Screen.width * 0.07f;
        float joystickButtonWidth = Screen.width * 0.0615f;
        float sizeOfJoystickCircleOutside = joystickButtonWidth * 2.0f;
        float joystickMoveButtonsWidth = joystickButtonWidth / 1.3f;

        if (Globals.joystickSide.Contains("LEFT")) {
            joystickBG.anchorMin = new Vector2(0, 0);
            joystickBG.anchorMax = new Vector2(0, 0);
            joystickBG.pivot = new Vector2(1, 0);
        }

        //float joyStickW = sizeOfJoystickCircleOutside;
        //float joyStickH = sizeOfJoystickCircleOutside;

        //print("RESOLUTIONS Camera.aspect " + Camera.main.aspect);

        /*print("JOYSTICKWIDTH " + joystickButtonWidth
            + " HEIGHT "
            + joystickButtonHeight
            + " RATESCREEN ");*/


        float joystickOffsetW = (sizeOfJoystickCircleOutside / 2.0f)
            + (joystickMoveButtonsWidth / 2.1f);

        joystickScreenOffset = sizeOfJoystickCircleOutside + (joystickMoveButtonsWidth * 2f);

        //print("DEBUTOUCHXYZ JOYSTICKOFFSET " + joystickScreenOffset);

        //float joystickOffsetH = (joyStickW / 2.0f) + (joystickButtonWidth / 2.1f);
        joystickButton.sizeDelta = new Vector2(joystickButtonWidth,
                                               joystickButtonWidth);

        joystickBG.sizeDelta = new Vector2(sizeOfJoystickCircleOutside,
                                           sizeOfJoystickCircleOutside);

        if (Globals.joystickSide.Contains("RIGHT"))
        {
            joystickBG.anchoredPosition = new Vector2(-joystickMoveButtonsWidth,
                                                       joystickMoveButtonsWidth);
        }
        else
        {
            joystickBG.anchoredPosition = new Vector2(joystickMoveButtonsWidth + sizeOfJoystickCircleOutside,
                                                      joystickMoveButtonsWidth);
        }

        //print(" SCREEN WIDTH " + Screen.width + " SCREEN HEIGHT " + Screen.height + " RATE WIDHT "
        //    + rateScreenWidth + " rateScreenHeight " + rateScreenHeight + 
        //    " SIZE IN PIXEL WIDTH " + (sizeOfJoystickCircleOutside / rateScreenWidth) +
        //    " SIZE IN PIXEL HEIGHT " + sizeOfJoystickCircleInside / rateScreenWidth);



        gkSideLeftButtonRectTrans.sizeDelta = new Vector2(joystickMoveButtonsWidth,
                                                          joystickMoveButtonsWidth);
        gkSideLeftButtonRectTrans.anchoredPosition = new Vector2(-joystickOffsetW,
                                                                  0.0f);

        gkSideRightButtonRectTrans.sizeDelta = new Vector2(joystickMoveButtonsWidth,
                                                           joystickMoveButtonsWidth);
        gkSideRightButtonRectTrans.anchoredPosition = new Vector2(joystickOffsetW,
                                                                  0.0f);

        gkMoveUpButtonRectTrans.sizeDelta = new Vector2(joystickMoveButtonsWidth,
                                                        joystickMoveButtonsWidth);
        gkMoveUpButtonRectTrans.anchoredPosition = new Vector2(0.0f,
                                                               joystickOffsetW);

        gkMoveDownButtonRectTrans.sizeDelta = new Vector2(joystickMoveButtonsWidth,
                                                          joystickMoveButtonsWidth);
        gkMoveDownButtonRectTrans.anchoredPosition = new Vector2(0.0f,
                                                                 -joystickOffsetW);     
    }

    private void setSpecialButtonsPosition()
    {
        float buttonSize = screenWidth * 0.067f;
        RectTransform lobRectTrans = lobButtonGameObject.GetComponent<RectTransform>();
        RectTransform volleyRectTrans = volleyButtonGameObject.GetComponent<RectTransform>();
        RectTransform lobTextRectTrans = lobButtonTextGameObject.GetComponent<RectTransform>();
        RectTransform volleyTextRectTrans = volleyButtonTextGameObject.GetComponent<RectTransform>();
        RectTransform cameraTextRectTrans = cameraButtonTextGameObject.GetComponent<RectTransform>();

        if (Globals.joystickSide.Contains("LEFT"))
        {
            /*anchor bottom, right*/
            lobRectTrans.anchorMin = new Vector2(1, 0);
            lobRectTrans.anchorMax = new Vector2(1, 0);
            lobRectTrans.pivot = new Vector2(1, 0);
            volleyRectTrans.anchorMin = new Vector2(1, 0);
            volleyRectTrans.anchorMax = new Vector2(1, 0);
            volleyRectTrans.pivot = new Vector2(1, 0);
        }

        /*Anchor is in left bottom when joystick right or right bottom when joystick left*/
        if (Globals.joystickSide.Contains("RIGHT"))
        {
            lobRectTrans.anchoredPosition = new Vector2(buttonSize / 2.1f,
                                                        buttonSize / 8f);
        }
        else
        {
            lobRectTrans.anchoredPosition = new Vector2(-(buttonSize / 2.1f),
                                                          buttonSize / 8f);
        }
       
        lobRectTrans.sizeDelta = new Vector2(buttonSize,
                                             buttonSize);

        volleyRectTrans.anchoredPosition = new Vector2(
            lobRectTrans.anchoredPosition.x * 1.2f,
            lobRectTrans.anchoredPosition.y + (buttonSize * 1.1f));
        volleyRectTrans.sizeDelta = new Vector2(buttonSize,
                                                buttonSize);

        //cameraRectTrans.anchoredPosition = 
        //    new Vector2(lobRectTrans.anchoredPosition.x + (buttonSize * 1.1f),
        //                lobRectTrans.anchoredPosition.y + (buttonSize / 2.0f));
        //cameraRectTrans.sizeDelta = new Vector2(buttonSize,
        //                                        buttonSize);

        lobTextRectTrans.sizeDelta = new Vector2(buttonSize,
                                                 buttonSize);
        volleyTextRectTrans.sizeDelta = new Vector2(buttonSize,
                                                   buttonSize);
        //cameraTextRectTrans.sizeDelta = new Vector2(buttonSize,
        //                                            buttonSize);

        int fontSize = Math.Max(10, (int) (buttonSize / 2.5f));
        volleyButtonTextGameObject.GetComponent<Text>().fontSize = fontSize;
        lobButtonTextGameObject.GetComponent<Text>().fontSize = fontSize;
        //cameraButtonTextGameObject.GetComponent<Text>().fontSize = fontSize;

        /*BUG*/
        //specialButtonsScreenOffset = cameraRectTrans.anchoredPosition.x + (buttonSize * 1.1f);

        specialButtonsScreenOffset.x = Mathf.Abs(volleyRectTrans.anchoredPosition.x) +
            buttonSize;

        specialButtonsScreenOffset.y = volleyRectTrans.anchoredPosition.y +
            (buttonSize * 1.6f);

        /*print("UPDATTOUCH145 specialButtonsScreenOffset " + specialButtonsScreenOffset 
            + " LOB ANCHOR " + lobRectTrans.anchoredPosition
            + " CAMERA POS " + cameraRectTrans.position
            + "volleyRectTrans.anchoredPosition "
            + volleyRectTrans.anchoredPosition + " buttonSize " + buttonSize);*/
    }

    public bool isPlayerOnBall()
    {
        return (onBall == PlayerOnBall.ONBALL) ? true : false;
    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("ONANIMATOROIK ");
        /*onAniatorIk(animator,
                    cpuPlayer.getShotActive(),
                    cpuPlayer.isLobShotActive(),
                    lastGkDistX, rb, leftHand,
                    rightHand,
                    lastGkAnimName,
                    false);*/
    }

    public void onAniatorIk(Animator animator,
                            bool shotActive,
                            bool isLobActive,
                            float distX,
                            Rigidbody rb,
                            GameObject leftPalm,
                            GameObject rightPalm,
                            string anim,
                            bool isCpu)
    {

        if (!isCpu && gkDistRealClicked > (MIN_DIST_REAL_CLICKED + 1.0f))
        {
            return;
        }

        bool isGktStraightPlaying =
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "straight");

        if (isGktStraightPlaying &&
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "chest"))
            return;

        if (isGktStraightPlaying &&
            ballRb.transform.position.y > 0.8f)           
        {
            float reach = 1.0f;

            Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb.transform.position);

            if (ballInLocalRb.z < 0.0f) return;

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
            animator.SetIKPosition(AvatarIKGoal.RightHand, ballRb.transform.position);
       

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb.transform.position);
       

            return;
        }

        /*if (!isCpu)
            print("GKDEBUG800 PLAYINGANIMATIONOW isShotActive " + shotActive + " ISPLAYING "
                + checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch"));*/
        /*if (isPlaying(animator, "3D_volley", 1.0f))
        {
            float reach = 1.00f;

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, reach);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, ballRb.transform.position);
            
            return;
        }*/

        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") && shotActive)
        {
            float reach = 1.00f;
            if (!isCpu)
            {
                //reach = 0.15f;
                reach = 1.0f;
                /*TODELETE*/
                //return;
            }
            //print("TRANSFORMHANDLEFT " + rb.transform.InverseTransformPoint(leftPalm.transform.position));
            //print("TRANSFORMHANDRIGHT " + rb.transform.InverseTransformPoint(rightPalm.transform.position));

            //print("TRANSFORMHANDLEFTGLOBAL " + leftPalm.transform.position);
            //print("TRANSFORMHANDRIGHTGLOBAL " + rightPalm.transform.position);

            //Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            //Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb.transform.position);
            Vector3 ballInLocalRb = InverseTransformPointUnscaled(rb.transform, ballRb.transform.position);

            //print("TRANSFORMHANDBALL " + ballInLocalRb);

            //if (ballInLocalRb.z < -0.3f) return;

            //print("GKDEBUG800 ballINLOCALRB " + ballInLocalRb);

            if (ballInLocalRb.z < 0.0f) return;


            //if (rightHandLocalPos.z > 0.3f)
            //{
            //if (ballInLocalRb.z  0.3f)

            if ((anim.Contains("right") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX >= 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {

                //if (isCpu)
                //{
                //    print("GKDEBUG800 PLAYINGANIMATIONOW ON ANIMATOR IK LEFT RIGHT " + distX);
                //}
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
                //print("IKPUNCHTEST1LEFT");
                //if (isCpu)
                //{
                //    print("GKDEBUG800 PLAYINGANIMATIONOW ON ANIMATOR IK LEFT " + distX);
                //}

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb.transform.position);
                Quaternion leftHandRotation = Quaternion.LookRotation(ballRb.transform.position - leftHand.transform.position);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            }

            return;
        }

        return;
    }

    private void drawHelperGrid()
    {
        for (int z = -14; z < 14; z++)
            DrawLine(new Vector3(-21.0f, 0.05f, (float)z), new Vector3(21.0f, 0.05f, (float)z), Color.white, 2000.0f, 0.05f);

        for (int x = -21; x < 21; x++)
        {
            if (x != 5 && x != 0)
                DrawLine(new Vector3(x, 0.05f, 14.0f), new Vector3(x, 0.05f, -14.0f), Color.white, 2000.0f, 0.05f);
            else
            {
                DrawLine(new Vector3(x, 0.05f, 14.0f), new Vector3(x, 0.05f, -14.0f), Color.yellow, 2000.0f, 0.15f);

            }
        }
    }

    private void setupLevelDependentVariables()
    {
        float offsetBase = 3.0f;

        //float shotSkillsInter = 
        //    getSkillInterpolationReverse(Globals.teamAAttackStrength);

        //float offset;
        //offset = offsetBase + 
        //    (Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, Globals.teamAGkStrength) * 3.0f);

        float shotSkillsInter = 
            getSkillInterpolationReverse(attackSkillsPlayer);

        float offset;
        offset = offsetBase + 
            (Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, defenseSkillsPlayer) * 3.0f);

        //print("DEBUG111X #defenseSkillsPlayer " + defenseSkillsPlayer);

        MAX_GK_OFFSET = offset;
        MAX_GK_OFFSET = Mathf.Clamp(MAX_GK_OFFSET, 4.2f, 6.0f);

        //float skillsInter = 
        //    getSkillInterpolation(Globals.teamBGkStrength);

        float skillsInter = 
            getSkillInterpolation(defenseSkillsCpu);

        float levelInter = getLevelInterpolation();
        float avgWeight = (skillsInter + levelInter) / 2.0f;
        MAX_GK_OFFSET_CPU = offsetBase + (avgWeight * 2.0f);
        MAX_GK_OFFSET_CPU = Mathf.Clamp(MAX_GK_OFFSET_CPU, 3.8f, 5.0f);

        //print("DEBUG111X MAX_GK_OFFSET " + MAX_GK_OFFSET + " MAX_GK_OFFSET_CPU " + MAX_GK_OFFSET_CPU
        //    + " shotSkillsInter " + shotSkillsInter + " max shot value " + (80.0f - Mathf.Lerp(0, 18f, shotSkillsInter)));


        //runningSpeed = calcRunLevelSpeed(Globals.level,
        //                                 Globals.teamAcumulativeStrength,
        //                                 false) + 2.0f;
        runningSpeed = calcRunLevelSpeed(Globals.level,
                                         cumulativeStrengthPlayer,
                                         false) + 2.0f;


        speedMultiplayer = 70.0f - Mathf.Lerp(0, 30f, shotSkillsInter);
    }

    private void gamEndAnimations()
    {
        string animName = "3D_end_celebration";
        string cpuAnimName = "3D_end_celebration";

        if (realTime < 1.0f) return;

        gameEndedAnimations = true;

        if (realTime > 20.0f)
        {
            animName = cpuAnimName = "3D_stand_end";
        }
        else
        {

            //if (Globals.score1 > Globals.score2)
            if (Globals.didPlayerWon(Globals.score1, Globals.score2))
            {
                cpuAnimName = "3D_stand_end";
            }
            else
            {
                //if (Globals.score1 == Globals.score2)
                if (Globals.didPlayerDraw(Globals.score1, Globals.score2))
                {
                    cpuAnimName = "3D_stand_end";
                }

                animName = "3D_stand_end";
            }
        }

        //print("ANIMNAME " + animName + " CPUANIM " + cpuAnimName);

        if (!checkIfAnyAnimationPlaying(animator, 1.0f) &&
            !isPlaying(animator, animName, 1.0f))
        {
            animator.Play(animName, 0, 0.0f);
        }

        //Animator cpuAnimator = cpuPlayer.getAnimator();
        //if (!checkIfAnyAnimationPlaying(cpuAnimator, 1.0f) &&
        //    !isPlaying(cpuAnimator, cpuAnimName, 1.0f))
       // {
        //    cpuAnimator.Play(cpuAnimName, 0, 0.0f);
        //}
    }

    private void recoverAnimatorSpeed()
    {
        if (!checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_"))
        {
            animator.speed = 1.0f;
        }

        //if (!checkIfAnyAnimationPlayingContain(cpuPlayer.getAnimator(), 1.0f, "3D_GK_sidecatch_"))
       // {
        //    cpuPlayer.setAnimatorSpeed(1.0f);
       // }
    }

    public string getLastGkAnimPlayed()
    {
        return lastGkAnimName;
    }

    void LateUpdate()
    {
      
        //print("DEBUGCAMERA LateUpdate  DELTA TIME " + deltaIterator + " Time.deltaTime " + Time.deltaTime);
        //deltaIterator++;

        if (!ball.getBallCollided() &&
            shotActive &&
            !shotRet &&
            lastBallVelocity != Vector3.zero &&
            isBallInGame)
        {
            //print("GKDEBUG7 LATE BEFORE " + ballRb.velocity);
            ballRb.velocity = lastBallVelocity;
            //print("GKDEBUG7 LATE AFTER " + ballRb.velocity);
        }


        correctWhenOutOfPitch(animator,
                        preShotActive,
                        shotActive,
                        rb,
                        rbLeftToeBase,
                        rbRightToeBase,
                        ballRb,
                        shotType,
                        ref prevRbPos,
                        false);

        corectBallPositionOnBall(rb,
                                 animator,
                                 rbRightToeBase,
                                 rbRightFoot,
                                 ref isUpdateBallPosActive,
                                 updateBallPos,
                                 updateBallPosName,
                                 false);

        //cpuPlayer.lateUpdate();
            

        //print("LATE RB POSITION " + rb.position);
        /*don't allow to goes down over floor after some time */
        if (rb.transform.position.y < 0.03f)
            rb.transform.position =
                new Vector3(rb.transform.position.x, 0.03f, rb.transform.position.z);



        //for (int i = 0; i < isFixedUpdate; i++)
        //{       
        //    cameraMovement(false);        
        //}

        isFixedUpdate = 0;

        //print("ENTERPOSITION CLEAN END OF LATEUPDATE RB POS " + rb.transform.position);
    }

    private void slowDownGkAnimation(Animator animator, string contain, float start, float end, float slowSpeed, float orgSpeed)
    {
        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, contain))
        {
            float normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (normTime >= start &&
                normTime <= end)
            {
                animator.speed = slowSpeed;
                //print("CONTAIN MID " + contain + " TIME %" + normTime);
            }
            else
            {
                animator.speed = orgSpeed;
            }
        }
    }

    public void setBallAngularVelocity(Rigidbody rb)
    {
        Vector3 rbRoundVector = new Vector3(
                                           Mathf.Round(rb.velocity.x),
                                           Mathf.Round(rb.velocity.y),
                                           Mathf.Round(rb.velocity.z));

        Vector3 BallRoundAngularVelVector = new Vector3(
                                           Mathf.Round(ballRb.angularVelocity.x),
                                           Mathf.Round(ballRb.angularVelocity.y),
                                           Mathf.Round(ballRb.angularVelocity.z));

        if (rbRoundVector == Vector3.zero)
        {
            if (BallRoundAngularVelVector == Vector3.zero)
            {
                ballRb.angularVelocity = Vector3.zero;
                ballRb.velocity = Vector3.zero;
            }
            else
            {
                ballRb.angularVelocity /= 1.05f;
                ballRb.velocity /= 1.05f;
            }
        }
        else
        {
            ballRb.angularVelocity = new Vector3(10.0f, 10.0f, 10.0f);
        }
    }

    /*public void ballMovePosition(Rigidbody rb, Vector3 movementDir)
    {
        ballRb.MovePosition(rb.position + (movementDir / 1.2f));

        Vector3 rbRoundVector = new Vector3(
                                         Mathf.Round(rb.velocity.x),
                                         Mathf.Round(rb.velocity.y),
                                         Mathf.Round(rb.velocity.z));

        Vector3 BallRoundAngularVelVector = new Vector3(
                                         Mathf.Round(ballRb.angularVelocity.x),
                                         Mathf.Round(ballRb.angularVelocity.y),
                                         Mathf.Round(ballRb.angularVelocity.z));

        if (rbRoundVector == Vector3.zero)
        {
            if (BallRoundAngularVelVector == Vector3.zero)
            {
                ballRb.angularVelocity = Vector3.zero;
                ballRb.velocity = Vector3.zero;
            }
            else
            {
                ballRb.angularVelocity /= 1.05f;
                ballRb.velocity /= 1.05f;
            }
            //print("ballMovePosition CLEARED HERE 2");
        } else
        {
            ballRb.angularVelocity = new Vector3(10.0f, 10.0f, 10.0f);
        }

       
    }
*/
    public void setBallPosition()
    {     
        //if (ball.getBallGoalCollisionStatus())
        //{
        if (Globals.isTrainingActive) {
            /*if (trainingScript.isGkTraining())
            {
                ballRb.position =
                    new Vector3(UnityEngine.Random.Range(-13, 13f),
                                0f,
                                UnityEngine.Random.Range(3, 8f));
            }*/

            return;
        }

        //ballRb.position = new Vector3(PITCH_WIDTH_HALF - 0.23f, 0, -(PITCH_HEIGHT_HALF - 0.23f));
        //return;


        bool ballGoal = ball.getBallGoalCollisionStatus();
        Vector3 ballPosPlayer2 = new Vector3(UnityEngine.Random.Range(-15, 15),
                                             0,
                                             UnityEngine.Random.Range(3, 10));
        Vector3 ballPosPlayer1 = ballPosPlayer2;
        ballPosPlayer1.z *= -1;

        if (ballGoal)
        {
            if (ball.whoScored() == 1)
            {
                ballRb.transform.position = ballPosPlayer2;
            } else
            {
                ballRb.transform.position = ballPosPlayer1;
            }

            return;
        }

        //print("whoTouchBallLastSETBALL " + ball.whoScored() + " WHOTOUCHLAST " + ball.whoTouchBallLast());
        if (ball.whoTouchBallLast() == 1)
        {
            //ballRb.position = new Vector3(0, 0, 4);
            ballRb.transform.position = ballPosPlayer2;
            return;
        } else
        {
            ballRb.transform.position = ballPosPlayer1;
            return;
        }

        /*shouldn't be reach */
        ballRb.transform.position = ballPosPlayer1;

        return;
    }

    public void setBallPosition(Vector3 ballPos)
    {
        ballRb.transform.position = ballPos;
    }

    public void goToPos(Animator animator,
                        string animName,
                        Rigidbody rb,
                        float speed,
                        Vector3 towardsNewPos)
    {
        towardsNewPos = towardsNewPos.normalized;

        /*bool collisionWithWall = parentRb.playerUpRigidBody.isCollisionWithWall();
        if (collisionWithWall ||
           (Time.time - lastTimeCollisionWithWall) < 0.15f)
        {
            towardsNewPos = new Vector3(0.0f - cpuPlayerRb.position.x,
                                        0.0f,
                                        10.0f - cpuPlayerRb.position.z);
            if (collisionWithWall)
            {
                lastTimeCollisionWithWall = Time.time;
                parentRb.playerUpRigidBody.setIsCollisionWithWall(false);
            }

            print("DEBUGWALLCOLLISION" + collisionWithWall);
        }*/

        interruptSideAnimation(animator, rb);

        //print("DEBUGTOOFAR VELOCITY BEFORE " + rb.velocity + " SPEED " + speed);
        rb.velocity = towardsNewPos * speed;

        //print("DEBUGTOOFAR VELOCITY " + rb.velocity + " SPEED " + speed);

        //print("GKDEBUG7 INTERRUPT IN GO TO POSITION VELOCITY BEFORE " + cpuPlayerRb.velocity);

        //print("GKDEBUG7 INTERRUPT IN GO TO POSITION VELOCITY AFTER " + cpuPlayerRb.velocity);

        //print("TOWARD NEW POS 4 " + cpuPlayerRb.velocity);

        //print("PLAYANIMATION BEFORE");animName
        if (!isPlaying(animator, animName, 0.95f))
        {
            animator.Play(animName, 0, 0.0f);
            //print("DEBUGTOOFAR PLAY ANIM");
        }
    }

    private bool prepareShot(Animator animator,
                             ref string shotType,
                             Rigidbody playerRb,
                             Rigidbody ballRb,
                             GameObject rbRightFoot,
                             GameObject rbRightToeBase,
                             GameObject rbLeftToeBase,
                             Plane plane,
                             ref bool initShot,
                             ref bool initVolleyShot,
                             Vector2 endPos,
                             Vector3 endPosOrg,
                             ref bool isUpdateBallPosActive,
                             ref Vector3 updateBallPos,
                             ref string updateBallPosName,
                             bool isCpu)
    {
        bool shotActive = false;
        float clipOffsetTime = 1.0f;

        if (!initShot)
        {
            /*Don't block touch here */
            animator.Play(shotType, 0, 0.0f);
            animator.Update(0.0f);
            initShot = true;
            if (shotType.Equals("3D_volley_before"))
            {
                shotType = "3D_volley";
            }
        }

        clipOffsetTime = animationOffsetTime[shotType];
        //print("GKDEBUG1 " + shotType + " OFFSET TIME " + clipOffsetTime + " noramlized time " +
        //    animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " VALUE " + animator.GetCurrentAnimatorStateInfo(0).IsName(shotType));

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(shotType) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > clipOffsetTime)
        {
            bool onBall = isPlayerOnBall(rbLeftToeBase,
                                         rbRightToeBase,
                                         ballRb,
                                         playerRb,
                                         shotType,
                                         isCpu);

            //float ballDist = Vector3.Distance(playerRb.transform.position,
            //                                 (ballRb.transform.position));

            //print("GKDEBUG1 ballDIST INSIDE " + ballDist);
            //print("BALLDISTANCE " + ballDist + " SHOTTYPE " + shotType + " ISCPU " + isCpu);
            if (shotType.Equals("3D_volley"))
            {
            
                onBall = isPlayerOnBall(rbLeftToeBase,
                                        rbRightToeBase,
                                        ballRb,
                                        playerRb,
                                        shotType,
                                        isCpu);

                //if (!isCpu)
                //    print("CPUMOVEDEBUG123X_NOCPU ONBALL RETURN " + onBall);
                //else
                //    print("CPUMOVEDEBUG123X_CPU " + onBall);
            }

            if (onBall)
            {
                /*Start shoting */
                shotActive = true;
                initShot = false;
                initVolleyShot = false;
                //audioManager.Play("kick3");
                if (!isCpu)
                {
                    setShotButtonsInactive();
                    updateShotPos();
                }

                return true;
            }
            else
            {
                if (!isCpu)
                {
                    clearPreShotVariables();
                    isTouchBegin = false;

                    //print("BALLDISTANCE CLEARPRESHOTVARIABLESPLAYER");
                    //touchCount = 0;
                    //isTouchBegin = false;
                }
                else
                {
                    //print("BALLDISTANCE CLEARPRESHOTVARIABLESCPU");
                    //cpuPlayer.clearPreShotVariables();
                }

                //clearVars = true;
                return false;
            }

            //print("ENTERED ISMOVING STRAING");
        }
        else
        {   
            if (shotType.Contains("3D_volley"))
            {
                Vector3 ballPos;
                ballRb.angularVelocity = new Vector3(5.0f, 5.0f, 5.0f);

                if (!isPlaying(animator, "3D_volley", 1.0f))
                {
                    volleyShotUpdateBallPos(playerRb, rbRightToeBase, rbRightFoot, isCpu);
                    isUpdateBallPosActive = true;
                    updateBallPosName = "volleyShot";
                }
                else
                {
                    if (!initVolleyShot)
                    {                   
                            if (!isCpu)
                                touchLocked = true;                
                           
                            float zOffset = 1.65f;
                            if (isCpu)
                                zOffset = 1.73f;
                            Vector3 pointInFrointOfRightToe =
                                 TransformPointUnscaled(playerRb.transform, new Vector3(1.15f, 0f, 1.65f));
                           
                            float dist = Vector2.Distance(
                                new Vector2(ballRb.transform.position.x, ballRb.transform.position.z),
                                new Vector2(pointInFrointOfRightToe.x, pointInFrointOfRightToe.z));
                            
                            Vector3 pointInFrointOfRightToeDir =
                                (pointInFrointOfRightToe - ballRb.transform.position).normalized;

                            /*animation frame 34 */
                            if (!isCpu)
                            {
                                ballRb.velocity = new Vector3(
                                    pointInFrointOfRightToeDir.x * dist,
                                    7.5f,
                                    pointInFrointOfRightToeDir.z * dist);
                            } else
                            {
                                ballRb.velocity = new Vector3(
                                     pointInFrointOfRightToeDir.x * dist,
                                     7.58f,
                                     pointInFrointOfRightToeDir.z * dist);
                            }


                            /*ball goes up*/
                            //ballRb.velocity = new Vector3(shotEndPos.x * 1.35f, 7.5f, shotEndPos.z * 1.35f);

                            /*if (!isCpu)
                            {
                                print("CPUMOVEDEBUG123X_NOCPU ballRb.velocity "
                                    + ballRb.velocity
                                    + " BALLPOS "
                                    + ballRb.transform.position
                                    + " DISTANCE "
                                    + dist
                                    + " playerRb.transform "
                                    + playerRb.transform.position
                                    + " pointInFrointOfRightToe "
                                    + pointInFrointOfRightToe);
                            } else
                            {
                                print("CPUMOVEDEBUG123X_CPU ballRb.velocity "
                                   + ballRb.velocity
                                   + " BALLPOS "
                                   + ballRb.transform.position
                                   + " DISTANCE "
                                   + dist
                                   + " playerRb.transform "
                                   + playerRb.transform.position
                                   + " pointInFrointOfRightToe "
                                   + pointInFrointOfRightToe);
                            }*/
                        //}
                        //else
                        //{
                        //    shotEndPos = endPosOrg - ballRb.transform.position;
                        //    shotEndPos = shotEndPos.normalized;

                            /*ball goes up*/
                            //ballRb.velocity = new Vector3(shotEndPos.x * 1.2f, 7.10f, shotEndPos.z * 1.2f);
                        //    ballRb.velocity = new Vector3(shotEndPos.x * 1.35f, 7.7f, shotEndPos.z * 1.35f);

                        //}

                        initVolleyShot = true;
                    }
                }
            }
            else
            {
                normalShotUpdateBallPos(playerRb, shotType, animator);
                updateBallPosName = shotType;
                isUpdateBallPosActive = true;

                ballRb.angularVelocity = new Vector3(10.0f, 10.0f, 10.0f);
                if (playerRb.velocity == Vector3.zero)
                {
                    ballRb.angularVelocity = Vector3.zero;
                    ballRb.velocity = Vector3.zero;
                }
            }
        }

        return shotActive;
    }

    private void normalShotUpdateBallPos(Rigidbody playerRb, 
                                         string shotType, 
                                         Animator animator)
    {
        //Vector3 localRbPosition =
        //       InverseTransformPointUnscaled(playerRb.transform, playerRb.transform.position);
        //Vector3 localPosForward =
        //     InverseTransformPointUnscaled(playerRb.transform, playerRb.transform.forward);

        //print("DBGNORMAL SHOT UPDATE localRbPosition "
        //    + localRbPosition + " localPosForward " + localPosForward 
        //    + " playerRb.transform.forward " + playerRb.transform.forward);

        float normTime = 1.0f;
        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_shot_"))
        {
            normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            normTime %= 1;
        }

        Vector3 localBallLocStart = new Vector3(0, 0, 0.714f);
        Vector3 localBallPosDestionation = new Vector3(0f, 0f, 1.0f);
        if (shotType.Contains("3D_shot_left_foot"))
        {
            localBallPosDestionation.x = -0.3f;
        }
        else
        {
            localBallPosDestionation.x = 0.3f;
        }

        float maxAnimTime = 0.220f;
        normTime = Mathf.InverseLerp(0, maxAnimTime, Mathf.Min(maxAnimTime, normTime));
        Vector3 interpolateBallPos = Vector3.Lerp(localBallLocStart,
                                                  localBallPosDestionation,
                                                  normTime);
  
        //print("DBGNORMAL start ball pos "
        //    + interpolateBallPos.ToString("F4") 
        //    + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
          
        //if (Vector3.Distance(localBallPosDestionation, newBallPos) > 0.02f) {
        //    localBallPosDestionation = newBallPos;
        //}


        /*if (preShotActive && !shotActive)
        {
            print("DBGNORMAL RIGHTTOE "
            + InverseTransformPointUnscaled(playerRb.transform, rbRightToeBase.transform.position).ToString("F4")
            + " LEFTTOE "
            + InverseTransformPointUnscaled(playerRb.transform, rbLeftToeBase.transform.position).ToString("F4")
            + " BALL RB "
            + InverseTransformPointUnscaled(playerRb.transform, ballRb.transform.position).ToString("F4")
            + " interpolateBallPos "
            + interpolateBallPos.ToString("F4"));           
        }*/

        //Vector3 ballMovePos = TransformPointUnscaled(playerRb.transform, localRbPosition) +
        //                      TransformPointUnscaled(playerRb.transform, localPosForward);

        //print("DBGNORMAL SHOT UPDATE localRbPosition "
        //    + localRbPosition + " localPosForward " 
        //    + localPosForward + " ballMovePos " + ballMovePos 
        //    + " FORWARD TRANSFORM BACK " + TransformPointUnscaled(playerRb.transform, localPosForward));

        Vector3 ballWorldSpace =
             TransformPointUnscaled(playerRb.transform, interpolateBallPos);

        ballRb.transform.position = 
            new Vector3(ballWorldSpace.x, BALL_MIN_VAL.y, ballWorldSpace.z);
    }

    private void volleyShotUpdateBallPos(Rigidbody rb,
                                         GameObject rbRightToeBase,
                                         GameObject rbRightFoot,                                        
                                         bool isCpu)
    {
        Vector3 ballPos;

        ballPos = rbRightToeBase.transform.position + (rbRightFoot.transform.forward * 0.3f);
        ballRb.transform.position = ballPos;

        if (!isCpu)
        {
            Vector3 shotGoalEndPos = updateEndShotPos();
            RblookAtWSPoint(rb, shotGoalEndPos);

            /*TOREMOVE*/
            /*GameObject lookPointRot = new GameObject();
            Vector3 lookDirection = Vector3.zero;
            Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
            lookDirection = (shotGoalEndPos - rb.transform.position).normalized;
            lookDirection.y = 0.0f;
            lookOnLook = Quaternion.LookRotation(lookDirection);
            lookPointRot.transform.rotation =
                Quaternion.Slerp(rb.transform.rotation, lookOnLook, 1f);

            float angle = Quaternion.Angle(rb.transform.rotation, lookPointRot.transform.rotation);
            print("DEBUG2345ANIMPLAY NEW ROTATION !!!! ANGLE " + angle + 
                " RB EULER ANGLES " 
                + rb.transform.eulerAngles 
                + " ENDPOSORG " + shotGoalEndPos);*/
        }
        //ballRb.MovePosition(ballPos);
    }

    private void gkStandStill(Animator animator, Rigidbody rb)
    {
        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying)
        {
            /*if (Mathf.Abs(rb.velocity.x) < 0.05f &&
                Mathf.Abs(rb.velocity.y) < 0.05f &&
                Mathf.Abs(rb.velocity.z) < 0.05f)*/

            if (rb.velocity == Vector3.zero && !isPlaying(animator, "3D_GK_stand_still", 1.0f))
            {
                //animator.Play("3D_GK_stand_still", 0, 0.0f);
            }
        }
    }

    public bool isOnBall()
    {
        return isPlayerOnBall(rbLeftToeBase, rbRightToeBase, ballRb, rb, "move", false);
    }

    /*private void gkIdle(Animator animator)
    {
        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying)
        {            
            if (ballRb.transform.position.z > 0.0f &&
                !preShotActive &&
                !shotActive &&
                !cpuPlayer.getShotActive() &&
                goalJustScored == false &&
                isBallInGame)
            {
                Vector3 lookTowardBall = new Vector3(ballRb.transform.position.x - rb.position.x,
                                                     0.0f,
                                                     ballRb.transform.position.z - rb.position.z);

                lookTowardBall = lookTowardBall.normalized;

                if (isPlaying(animator, "3D_run", 1.0f) == false)
                {
                    RblookAt(rb,
                             onBall,
                             lookTowardBall,
                             animator,
                             preShotActive,
                             Vector3.zero,
                             false,
                             "");
        
                    lastSaveRotationPosition = lookTowardBall;                
                }
            }
            else
            {
                if (cpuPlayer.getShotActive() && (isPlaying(animator, "3D_run", 1.0f) == false))
                {
                    RblookAt(rb,
                             onBall,
                             lastSaveRotationPosition,
                             animator,
                             preShotActive,
                             Vector3.zero,
                             false,
                             "");
                    //print("DEBUGLASTTOUCHLUCKXYU RB LOOK IDLE PREVIOUS " + lastSaveRotationPosition);
                }
            }
        }
    }*/

    private void playerOutOfPitch(Animator animator,
                                  Rigidbody rb,
                                  ref Vector3 prevRbPos,
                                  GameObject rbLeftToeBase,
                                  GameObject rbRightToeBase,
                                  bool isOnBall,
                                  bool isCpu)

    {
        //if (!isCpu)
        //    print("DEBUG2345ANIMPLAY CLEAN OUT " + rb.transform.position 
        //        +  " isOn BAll " + isOnBall + " ballRB.transform" + ballRb.transform.position);

        //if (!isCpu)
        //    print("DEBUG2345ANIMPLAY CLEAN START RB TRANAFORM ### "
        //        + rb.transform.position.ToString("F5") + " prevRbPos " + prevRbPos.ToString("F5"));

        bool isPlayerOut = false;
        bool isAnimPlaying = isAnimatorPlaying(animator);
            //checkIfAnyAnimationPlaying(animator, 1.0f);            

        //checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch");
        float goalXOff = goalXOffset;
        if (isCpu)
        {
            goalXOff = goalXOffsetCpu;
        }

        float minXDistance = minDistToOnBAll - 0.1f;
        float minZDistance = minDistToOnBAll - 0.1f;

        /*ball should never be on higher position than PITCH_WIDTH - BALL_RADIUS */
        if (!isCpu)
        {
            minXDistance += (BALL_NEW_RADIUS - 0.1f);
        }
   
        //if (isOnBall || isAnimPlaying)        
        if (isAnimPlaying &&
            !isPlaying(animator, "3D_volley", 1f))
        {
            minXDistance += 0.42f;
            minZDistance += 0.42f;
        }

        /*Add goal position exception*/
        if ((Mathf.Abs(rb.transform.position.x) <= goalXOff) &&
            (Mathf.Abs(rb.transform.position.z) > (PITCH_HEIGHT_HALF - 2.5f)))
        {
            //minZDistance = -1.0f;
            /*when the ball is on PITCH_HEIGT + 0.30f - no goal 0.31 - it's a goal
             * to reach a ball we need to be 0.8f from it. it means that if we setup 0.45 we can always 
             * reach a ball that is behind a line in goal but still it's a goal 0.45 + 0.30 = 0.75f */
            minZDistance = 0.2f;
        }

        if (Mathf.Abs(rb.transform.position.x) >= (PITCH_WIDTH_HALF - minXDistance) ||
            Mathf.Abs(rb.transform.position.z) >= (PITCH_HEIGHT_HALF - minZDistance))
        {
            //if (!isCpu)
            //    print("DEBUG2345ANIMPLAY CLEAN OUT ISPLAYEROUT ENTERED "
            //         + minZDistance.ToString("F5") 
            //        + " PITCH_WIDTH_HALF - minXDistance " + (PITCH_WIDTH_HALF - minXDistance).ToString("F5"));
            isPlayerOut = true;
        }

        //if (!isCpu)
        //print("DEBUG2345ANIMPLAY CLEAN CALC VALUES " + isPlayerOut
        //    + " PITCH_HEIGHT_HALF - minZDistance " + (PITCH_HEIGHT_HALF - minZDistance).ToString("F4")
        //    + " MINDISTANCe " + minZDistance);

        if ((isCpu && (rb.transform.position.z <= minZDistance)) ||
            (!isCpu && (rb.transform.position.z >= -minZDistance)) ||
            (isPlayerOut && (!isCpu || isAnimPlaying))) 
        {
            if (!isCpu)
            {
                if (rb.transform.position.z < -(PITCH_HEIGHT_HALF - minZDistance))
                {
                    rb.transform.position =
                        new Vector3(rb.transform.position.x, 0f, -(PITCH_HEIGHT_HALF - minZDistance));
                } else
                {
                    if (rb.transform.position.z > -minZDistance)
                    {
                        rb.transform.position =
                            new Vector3(rb.transform.position.x, 0f, -minZDistance);
                    }
                }

                if (Mathf.Abs(rb.transform.position.x) > (PITCH_WIDTH_HALF - minXDistance))
                {
                    float newXPos = PITCH_WIDTH_HALF - minXDistance;
                    if (rb.transform.position.x < 0f)
                        newXPos = -newXPos;

                    rb.transform.position =
                        new Vector3(newXPos, 0f, rb.transform.position.z);
                }                               
            }
            else
            {
                rb.transform.position = prevRbPos;
            }
        }

        prevRbPos = rb.transform.position;
    }

    private void calculateAndSetWinTieMatchIntroValues(int teamAstrength, int teamBstrength)
    {
        //float levelFactor = 0.2f + (Globals.level * 0.15f);
        float levelFactor = 0;
        int winPoints = 50;
        int tiePoints = 20;
        float level = (float) Globals.level;

        if (Globals.level <= 2)
            levelFactor = 0.05f + (level * 0.07f);
        else
        {
            if (Globals.level == 3)
            {
                levelFactor = 0.27f;
            } else if (Globals.level == 4)
            {
                levelFactor = 0.50f;
            } else if (Globals.level == 5)
            {
                levelFactor = 0.70f;
            }
        }

        teamAstrength /= 2;
        teamBstrength /= 2;

        int winPointsDiff = teamBstrength - teamAstrength;
        int tiePointsDiff = teamBstrength - teamAstrength;
        //print("DEBUGCALC1 WINDIFF " + winPointsDiff);
        //print("DEBUGCALC1 TIEDIFF " + tiePointsDiff);
        winPoints += winPointsDiff;
        tiePoints += tiePointsDiff;

        //print("DEBUGCALC1 AFTER WINDIFF " + winPoints);
        //print("DEBUGCALC1 AFTER TIEDIFF " + tiePoints);

        winPoints = Mathf.Clamp(winPoints, 7, 130);
        tiePoints = Mathf.Clamp(tiePoints, 3, 130);

        winPoints = (int)((float) winPoints * levelFactor);
        tiePoints = (int)((float) tiePoints * levelFactor);

        /*values should be beetween 0 and 100 */
        winPoints = Mathf.Clamp(winPoints, 7, 130);
        tiePoints = Mathf.Clamp(tiePoints, 4, 130);
       
        if (Globals.level == Globals.MAX_LEVEL)
        {
            winPoints = Mathf.Clamp(winPoints, 24, 130);
            tiePoints = Mathf.Clamp(tiePoints, 14, 130);
        } else if (Globals.level == 4)
        {
            winPoints = Mathf.Clamp(winPoints, 20, 130);
            tiePoints = Mathf.Clamp(tiePoints, 13, 130);
        }

        if (winPoints == tiePoints)
            winPoints += 5;

        matchIntroWinCoinsNumText.text = "+" + winPoints.ToString();
        matchIntroTieNumCoinText.text = "+" + tiePoints.ToString();

        winCoinsRewarded = winPoints;
        tieCoinsRewarded = tiePoints;
    }

    private void clearPreShotVariables()
    {
        preShotActive = false;
        shotActive = false;
        touchCount = 0;
        initPreShot = false;
        initShot = false;
        initVolleyShot = false;
        setShotButtonsInactive();
    }

    private void setShotButtonsInactive()
    {
        volleyButton.setButtonState(false);
        lobButton.setButtonState(false);
    }

    public void gkMoveUpAnim()
    {

        /*if ((ballRb.transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && Globals.isTrainingActive))
            return;

        if (isAnimatorPlaying(animator)) return;
   
   
            animator.Play("3D_walk", 0, 0.0f);*/
    }

    public void gkMoveDownAnim()
    {
        /*if ((ballRb.transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && Globals.isTrainingActive))
            return;

        if (isAnimatorPlaying(animator)) return;     
        animator.Play("3D_back_run", 0, 0.0f); */  
    }

    public void gkSideLeftAnim()
    {
       /* if ((ballRb.transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && Globals.isTrainingActive))
            return;


        if (isAnimatorPlaying(animator)) return;
      
            animator.Play("3D_GK_step_left", 0, 0.0f);*/
    }

    public void gkSideRightAnim()
    {
       /* if ((ballRb.transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && Globals.isTrainingActive))
            return;

        if (isAnimatorPlaying(animator)) return;
     
            animator.Play("3D_GK_step_right", 0, 0.0f);*/
        
    }

    public bool isShotOnTarget(Vector3 endPosOrg, Vector3 goal)
    {
        /*Assume that z position is always fine*/
        if (Mathf.Abs(endPosOrg.x) <= Math.Abs(goal.x) &&
            Mathf.Abs(endPosOrg.y) <= Math.Abs(goal.y))
            return true;

        return false;
    }

    public void setUpdateBallPosActive(bool val)
    {
        isUpdateBallPosActive = false;
    }

    public void setBallInFrontOfRb(Rigidbody rb, float div)
    {
        ballRb.transform.position = rb.transform.position + (rb.transform.forward / div);
        //ballRb.MovePosition(rb.transform.position + (rb.transform.forward / div));
    }

    public void setBallInFrontOfRb(Rigidbody rb, Vector3 pos)
    {
        ballRb.transform.position = pos;
        //ballRb.MovePosition(pos);
    }

    public bool isTouchPaused()
    {
        if (Globals.isTrainingActive)
            //&&
           //trainingScript.isTouchPaused())
            return true;

        return false;
    }

    public bool isRunPaused()
    {
        if (Globals.isTrainingActive)
            //trainingScript.isRunPaused())
            return true;

        return false;
    }

    public bool isGamePaused()
    {
        //if (isTrainingActive &&
        //    trainingScript.isTraningPaused())
        //    return true;
        if (gamePausedScript.isGamePaused())
            return true;

        return false;
    }

    public bool isShotTrainingActive() {
        if (Globals.isTrainingActive) //&&
            //trainingScript.isShotTraining())
            return true;
        return false;
    }

    public bool isGkTrainingActive()
    {
        if (Globals.isTrainingActive)
            //&&
            //trainingScript.isGkTraining())
            return true;
        return false;
    }

    private Vector3 updateEndShotPos()
    {
        float dist = 0.0f;
        RaycastHit hit;
        Vector3 hitPoint = new Vector3(0.0f, 0.0f, 14.0f);
        //Vector3 endShotPos = Vector3.zero;
        Vector3 endShotPos = INCORRECT_VECTOR;

        Ray ray = m_MainCamera.ScreenPointToRay(endPos);
        if (goalUpPlane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            if (hitPoint.y < 0)
                hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POSENDUPDATE " + hitPoint);
            endShotPos = hitPoint;
        }

        return endShotPos;
    }

    private Vector2 updateMidTouchPos(Vector2 startPos, Vector2 endPos)
    {
        Vector2 midPos = startPos;
        Vector2 lineStartEnd = LineEquation(startPos, endPos);
        float distMidFromLine = 0;
        float maxDistFromLine = -float.MaxValue;

        //print("DEBUG12345XA START POS "
        //    + startPos.ToString("F4") + " ENDPOS " + endPos.ToString("F4") +
        //    " midTouchPosIdx " + midTouchPosIdx + " lineStartEnd " + lineStartEnd.ToString("F4"));

        for (int i = 0; i < midTouchPosIdx; i++)
        {
            distMidFromLine = pointDistanceFromTheLine(lineStartEnd, midTouchPos[i]);
            //print("DEBUG12345XA dist from line " + distMidFromLine + " TOUCH POINT "
            //    + midTouchPos[i] + " maxDistFromLine " + maxDistFromLine);

            if (maxDistFromLine <= distMidFromLine)
            {
                midPos = midTouchPos[i];
                maxDistFromLine = distMidFromLine;
            }
        }

        //print("DEBUG12345XA FINAL MID POS " + midPos + " DISTFROMLINE " + distMidFromLine);

        return midPos;
    }

    private void updateShotPos()
    {
        float dist = 0.0f;
        RaycastHit hit;
        Vector3 hitPoint = new Vector3(0.0f, 0.0f, 14.0f);

        //print("DEBUG1 STARTHRNEW UPDATE SHOT START " + startPos + " MID " + midPos + " END " + endPos);

        midPos = updateMidTouchPos(startPos, endPos);
        //print("DEBUG12345XA MIDPOS " + midPos);
        startPos3 = ballRb.transform.position;
        endPosOrg = Vector3.zero;
        midPos3 = Vector3.zero;

        Ray ray = m_MainCamera.ScreenPointToRay(startPos);
        if (goalUpPlane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            //if (hitPoint.y < 0)
            //    hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POS " + hitPoint);
            curveStartPos3 = hitPoint;
        }

        ray = m_MainCamera.ScreenPointToRay(midPos);
        if (goalUpPlane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            //if (hitPoint.y < 0)
            //    hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POSNEW " + hitPoint);
            curveMidPos3 = hitPoint;
        } else
        {
            //print("SHOTVECTOR3POSNEW NO RAY!!");
        }

        ray = m_MainCamera.ScreenPointToRay(endPos);
        if (goalUpPlane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            //if (hitPoint.y < 0)
            //    hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POS " + hitPoint);
            curveEndPos3 = hitPoint;
        }

        ray = m_MainCamera.ScreenPointToRay(endPos);
        if (goalUpPlane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            if (hitPoint.y < BALL_RADIUS)
                hitPoint.y = BALL_RADIUS;

            //print("SHOTVECTOR3POS " + hitPoint);
            endPosOrg = hitPoint;
        }

        /*print("UPDATECURVEPOSTNEW " + curveStartPos3 + " MID " + curveMidPos3 + " END " + curveEndPos3);
        print("UPDATEPOS START " + startPos + " STARTPOS " + startPos3);
        print("UPDATEPOS MID " + midPos + " MIDPOS3 " + midPos3);
        print("UPDATEPOS END " + endPos + " ENDPOS3 " + endPos3);
        print("BALLINITPOSLERPNEW HERE UPDATE");*/


        //Vector3 directionBallGoal = new Vector3(hitPoint - startPos3).normalized;


        /*Vector2 directionPlayer = new Vector2(goalDownX - cpuPlayerRb.position.x,
                                                -13.0f - cpuPlayerRb.position.z);*/
        //directionBallGoal = directionBallGoal.normalized;

        //endPos3 = new Vector3(startPos.x + (directionBallGoal.x * randDist), startPos.y + (directionBallGoal.y * randDist));


    }

    private float getTimeOfShot()
    {
        return timeToShot;
    }

    public float getMaxTimeToShot()
    {
        return maxTimeToShot;
    }

    public void setTimeToShot(float val)
    {
        timeToShot = val;
    }

    private void printGameEventsInfo(string info)
    {
        /*if (info.Equals("TOOEARLY"))
        {
            gameInfoImage.texture = graphics.getTexture(
                "others/gameInfoEventTooEarly");
        } else if (info.Equals("TOOLATE"))
        {
            gameInfoImage.texture = graphics.getTexture(
                "others/gameInfoEventTooLate");
        } else if (info.Equals("OUT"))
        {
            gameInfoImage.texture = graphics.getTexture(
                "others/gameInfoEventOut");
        } else if (info.Equals("TIMEISUP"))
        {
            gameInfoImage.texture = graphics.getTexture(
                "others/gameInfoEventTimeIsUp");
        }*/

        gameEventMsgText.text = info;
        //gameInfoImageGameObject.SetActive(true);
        gameEventLastMsgPrintTime = Time.time;
    }

    private void clearGeneralInformtionText()
    {
        if ((Time.time - gameEventLastMsgPrintTime) > 2.2f)
        {
            gameEventMsgText.text = "";
            //gameInfoImageGameObject.SetActive(false);
        }
    }

    private bool updateTimeToShot(ref float prevZballPos, ref float timeToShot)
    {
        if ((ballRb.transform.position.z > 0.0f && prevZballPos < 0.0f) ||
            (ballRb.transform.position.z < 0.0f && prevZballPos > 0.0f) ||
            (!isBallInGame && timeToShot < maxTimeToShot))
        {
            timeToShot = 0.0f;
        }

        //print("timeToShot " + timeToShot + " maxTimeToShot " + maxTimeToShot);

        if (timeToShot > maxTimeToShot)
        {
            /*add if not training!!*/
            /*Do something with ball*/
            //printGameEventsInfo("Time to shoot up");
            /*if shot already started then it's fine :-) */
            if (ballRb.transform.position.z > 0.0f)
            {
                //if (cpuPlayer.getShotActive() ||
                //    cpuPlayer.getPreShotActive())
                //{
                //    return false;
                //}
            } else {
                if (touchCount > 0 ||
                    shotActive ||
                    preShotActive)
                    return false;
            }

            return true;
        }

        timeToShot += Time.deltaTime;
        prevZballPos = ballRb.transform.position.z;

        timeToShotText.text = ((int)timeToShot).ToString();
        if ((maxTimeToShot - timeToShot) <= 3.0f)
        {
            if ((timeLoops % 8) < 4)
                timeToShotText.color = Color.grey;
            else
                timeToShotText.color = Color.white;
        }
        else
        {
            timeToShotText.color = Color.white;
        }

        return false;
    }

    private void updateBallPossessionMatchStatistics()
    {
        float deltaTime = Time.deltaTime;

        if (ballRb.transform.position.z < 0.0f && ballPrevPosition.z < 0.0f)
            matchStatistics.setBallPossession("teamA", deltaTime);
        else
        {
            if (ballRb.transform.position.z > 0.0f && ballPrevPosition.z > 0.0f)
            {
                matchStatistics.setBallPossession("teamB", deltaTime);
            }
        }

        ballPrevPosition = ballRb.transform.position;
    }

    private bool updateGameTime()
    {
        float virtualTimeSeconds = currentTimeOfGame * (90.0f * 60.0f / timeOfGameInSec);
        int minutes = (int)Math.Ceiling(virtualTimeSeconds / 60);
        int seconds = (int)Math.Ceiling(virtualTimeSeconds % 60);
        seconds = Mathf.Clamp(seconds, 0, 59);

        //if (Globals.gameInGroup ||
        //    Globals.isFriendly ||
        //    (!Globals.gameInGroup && Globals.score1 == Globals.score2))
        //{
        if (minutes >= 90)
        {
            minutes = 90;
            seconds = 0;

            if ((timeLoops % 32) < 16)
                mainTimeText.color = Color.grey;
            else
                mainTimeText.color = Color.white;
        }
        //}

        string minutesTime = "", secondsTime = "";
        if (minutes < 10)
            minutesTime = "0";

        if (seconds < 10)
            secondsTime = "0";

        minutesTime = minutesTime + minutes.ToString();
        secondsTime = secondsTime + seconds.ToString();
        mainTimeText.text = minutesTime + ":" + secondsTime;
        currentTimeOfGame += Time.deltaTime;

        //print("CURRENT TIME OF GAME " + currentTimeOfGame + " MAX " + timeOfGameInSec + 
        //   " virtualTimeSeconds " + virtualTimeSeconds);

        if (isShotActive() ||
            isPreShotActive())            
            return false;

        if (currentTimeOfGame >= (timeOfGameInSec + stoppageTime))
        {
            if (Globals.gameInGroup ||
                Globals.isFriendly ||
               (!Globals.gameInGroup && Globals.score1 != Globals.score2))
            {
                mainTimeText.color = Color.white;
                return true;
            }
        }

        return false;
    }

    private void setScoresText()
    {
        //if (!Globals.playerPlayAway)
        //{
            score1Text.text = Globals.score1.ToString();
            score2Text.text = Globals.score2.ToString();
        //} else
       // {
            //score1Text.text = Globals.score2.ToString();
           // score2Text.text = Globals.score1.ToString();
        //}
    }

    private void setTimesText()
    {
        mainTimeText.text = "00:00";
        timeToShotText.text = "0";
    }

    private void correctWhenOutOfPitch(Animator animator,
                                       bool preShotActive,
                                       bool shotActive,
                                       Rigidbody rb,
                                       GameObject rbLeftToeBase,
                                       GameObject rbRightToeBase,
                                       Rigidbody ballRb,
                                       string shotType,
                                       ref Vector3 prevRbPos,
                                       bool isCpu)
    {
        bool isOnBall = false;
        if (preShotActive || shotActive)
        {
            isOnBall = isPlayerOnBall(
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           rb,
                           shotType,
                           isCpu);
        }
        else
        {
            isOnBall = isPlayerOnBall(
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           rb,
                           "move",
                           isCpu);
        }

        playerOutOfPitch(animator,
                       rb,
                       ref prevRbPos,
                       rbLeftToeBase,
                       rbRightToeBase,
                       isOnBall,
                       isCpu);
    }

    private void corectBallPositionOnBall(Rigidbody rb,
                                          Animator animator,
                                          GameObject rbRightToeBase,
                                          GameObject rbRightFoot,
                                          ref bool isUpdateBallPosActive,
                                          Vector3 updateBallPos,
                                          string actionName,
                                          bool isCpu)
    {

        /*don't execute when shot active*/

        if (!isUpdateBallPosActive)
            return;

        setBallAngularVelocity(rb);
        switch (actionName)
        {
            case "volleyShot":
                if (!isPlaying(animator, "3D_volley", 1.0f))
                    volleyShotUpdateBallPos(rb, rbRightToeBase, rbRightFoot, isCpu);
                break;
            case "3D_shot_left_foot":
            case "3D_shot_right_foot":
                normalShotUpdateBallPos(rb, actionName, animator);
                break;
            case "bodyMain":
                /*TOCHECK*/
                //ballRb.position = rb.transform.position + updateBallPos;
                if (Mathf.Abs(rb.transform.position.z) >= 13.50f) {
                    ballRb.transform.position = rb.transform.position +
                        (rb.transform.forward / 2.2f);
                    //new Vector3(rb.transform.forward.x / 2.2f,
                    //            BALL_MIN_VAL,y,
                     //           rb.transform.forward.z / 2.2f);
                } else {
                    ballRb.transform.position = rb.transform.position +
                        (rb.transform.forward / 1.4f);
                    //new Vector3(rb.transform.forward.x / 1.4f,
                    //            BALL_NEW_RADIUS,
                    //            rb.transform.forward.z / 1.4f);
                }

                //if (rbRightToeBase.transform.position.z < 0)
                /*print("ENTERPOSITION CLEAN BODY MAIN UPDATE " 
                    + ballRb.position + " rb.transform.position " 
                    + rb.transform.position.ToString("F4")
                    + " rb.transform.forward "  
                    + rb.transform.forward
                    + " (rb.transform.forward / 1.4f) " 
                    + (rb.transform.forward / 1.4f)
                    + " DISTANCE " + 
                        Vector2.Distance(
                        new Vector2(rb.transform.position.x, rb.transform.position.z),
                        new Vector2(ballRb.position.x, ballRb.position.z)));*/
                break;

            default:
                break;
        }

        if (ballRb.transform.position.y < BALL_MIN_VAL.y)
            ballRb.transform.position = new Vector3(ballRb.transform.position.x,
                                                    BALL_MIN_VAL.y,
                                                    ballRb.transform.position.z);

        isUpdateBallPosActive = false;
    }


    /*
       * [0] Field of view
       * [1] distance
       * [2] X rotation angle
       * [3] minZ distance
       * [4] maxZ distance
       * [5] maxZ Pos
       * [6] rb transform div
       * [7] minY distance
       * [8] maxY distance
       */
    float[][] cameraSettings = new float[][] {
            new float[] {50f, 5.6f, 17.5f, 5.6f, 9.0f, -20.90f, 3.0f, 4.0f, 5.5f},
            new float[] {60f, 3.0f, 18.5f, 5.6f, 8.0f, -19.5f, 2.5f, 4.7f, 5.0f},
            new float[] {40f, 7.95f, 15.5f, 7.95f, 12.0f, -23.7f, 2.5f, 4.8f, 6.3f},
            new float[] {50f, 5.6f, 17.5f, 5.6f, 9.0f, -20.90f, 3.0f, 4.0f, 5.5f}};

    public void cameraChanged(bool noLerpMove)
    {
        int cameraIdx = cameraButton.getCameraIdx();

        m_MainCamera.transform.eulerAngles =
              new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
        m_MainCamera.GetComponent<Camera>().fieldOfView =
               cameraSettings[cameraIdx][0];
        cameraMovement(noLerpMove);
    }

    ///Vector3[] cameraVel = new Vector3[10];

    Vector3 cameraVel = Vector3.zero;

    public void cameraMovement(bool noLerpMove)
    {
        int cameraIdx = cameraButton.getCameraIdx();

        /*
         * [0] Field of view
         * [1] distance
         * [2] X rotation angle
         * [3] minZ distance
         * [4] maxZ distance
         * [5] maxZ Pos
         * [6] rb transform div
         * [7] minY distance
         * [8] maxY distance
         */

        float fps = 30.0f;
        float yDist = Mathf.InverseLerp(0, -PITCH_HEIGHT_HALF, rb.transform.position.z);
        if (rb.transform.position.z <= -PITCH_HEIGHT_HALF)
            yDist = 1.0f;

        //print("DBGPOSIION " + rb.transform.position.z);
        //TOCHECK
        if (cameraIdx == 0)
        {
            yDist = Mathf.InverseLerp(0f, -10f, rb.transform.position.z);
            yDist = yDist - ((1f - yDist) * 0.85f);
            //print("#DBGYDIST " + yDist + " rb.transform.position.z " + rb.transform.position.z);
            yDist = Mathf.Clamp(yDist, 0f, 1f);

            if (Mathf.Abs(rb.transform.position.z) > 10f)
                yDist = 1f;
        }

        //if (Mathf.Abs(rb.transform.position.z) < 5.5f)
         //   yDist = 0f;
        //ENDTOCHECK

        yDist = Mathf.Lerp(cameraSettings[cameraIdx][7],
                           cameraSettings[cameraIdx][8],
                           yDist);
        float zDist =
            cameraSettings[cameraIdx][1] +
            Mathf.Abs(rb.transform.position.z / (cameraSettings[cameraIdx][6]));


        zDist = Mathf.Max(zDist, cameraSettings[cameraIdx][3]);
        zDist = Mathf.Min(zDist, cameraSettings[cameraIdx][4]);

        if (noLerpMove)
        {
            m_MainCamera.transform.position =
                new Vector3(
                    rb.transform.position.x,
                    yDist,
                    rb.transform.position.z - zDist);
        }
        else
        {

            /*Vector3 newCameraPos = new Vector3(
            Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.12f),
            Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
            Mathf.Lerp(m_MainCamera.transform.position.z,
                       rb.transform.position.z - zDist,
                       0.15f));*/

            /*Vector3 newCameraPos = new Vector3
                (Mathf.SmoothDamp(m_MainCamera.transform.position.x, rb.transform.position.x, ref cameraVels.x, 0.03f),
                 Mathf.SmoothDamp(m_MainCamera.transform.position.y, yDist, ref cameraVels.y, 0.04f),
                 Mathf.SmoothDamp(m_MainCamera.transform.position.z,
                    rb.transform.position.z - zDist, ref cameraVels.z,
                    0.02f));*/
            /*    Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.12f),
            Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
            Mathf.Lerp(m_MainCamera.transform.position.z,
                       rb.transform.position.z - zDist,
                       0.15f)); */

            /* x = lerp(x, target, rate)
               https://forum.unity.com/threads/camera-stutters-in-a-simple-rotation.389899/
               lerp(x, target, 1 - pow(1 - rate, 60 * dt)) 
               frame independent*/

             Vector3 newCameraPos = m_MainCamera.transform.position =
                         new Vector3(
                     Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f),
                                Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
                                Mathf.Lerp(m_MainCamera.transform.position.z,
                                rb.transform.position.z - zDist,
                                0.2f));

            /*float xVal =
                Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f);

            if (Mathf.Abs(m_MainCamera.transform.position.x - rb.transform.position.x) < 0.15f)
            {
                xVal = rb.transform.position.x;
            }*/


            /*Camera was moved from FixedUpdate to LateUpdate. When system is under pressure on the 
             * very high graphics settings FixedUpdate can be executed a few times but Update may be dropped due a high load.
             * if you don't use isFixedUpdate loop (you only execute once) camera will be more and more behind player 
             * , because position player was updated for instance twice but camera movement only once. 
             */
            //for (int i = 0; i < isFixedUpdate; i++) {
                /*Vector3 newCameraPos =
                         new Vector3(
                                Mathf.Lerp(
                                m_MainCamera.transform.position.x, rb.transform.position.x, 0.12f),
                                Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
                                Mathf.Lerp(m_MainCamera.transform.position.z,
                                   rb.transform.position.z - zDist,
                                   0.2f));*/


                //print("#DEBUGCAMERA m_MainCamera.transform.position X DIFF "
                //    + Mathf.Abs(m_MainCamera.transform.position.x - rb.transform.position.x));

                //Vector3 newCameraPos = new Vector3(
                /* Mathf.Lerp(
                     m_MainCamera.transform.position.x, rb.transform.position.x, 1f - Mathf.Pow(1f - 0.16f, fps * Time.deltaTime)),
                 Mathf.Lerp(
                     m_MainCamera.transform.position.y, yDist, 1f - Mathf.Pow(1f - 0.1f, fps * Time.deltaTime)),
                 Mathf.Lerp(
                     m_MainCamera.transform.position.z,
                            rb.transform.position.z - zDist,
                            1f - Mathf.Pow(1f - 0.15f, fps * Time.deltaTime)));*/


                //float xMoved = Mathf.Abs(m_MainCamera.transform.position.x - newCameraPos.x);
                //print("#DEBUGCAMERA XMOVED " + xMoved);

               /* Vector3 newCameraPos = new Vector3(
                  rb.transform.position.x,
                  yDist,
                  rb.transform.position.z - zDist);

                m_MainCamera.transform.position =
                    Vector3.SmoothDamp(m_MainCamera.transform.position, newCameraPos, ref cameraVel, 0.12f);*/
                           
                m_MainCamera.transform.position =
                        newCameraPos;
            //}
            //m_MainCamera.transform.position =
            // Vector3.SmoothDamp(m_MainCamera.transform.position, newCameraPos2, ref cameraVel, 0.05f);
        }

        /*max z camera position */
        if (m_MainCamera.transform.position.z < cameraSettings[cameraIdx][5])
        {
            m_MainCamera.transform.position =
                new Vector3(m_MainCamera.transform.position.x,
                            m_MainCamera.transform.position.y,
                            cameraSettings[cameraIdx][5]);
        }

        /*print("#DEBUGCAMERA m_MainCamera.transform.position "
            + " X "
            + " MOVEON X " 
            + (1f - Mathf.Pow(1f - 0.16f, fps * Time.deltaTime))
            + " TIMEDELTA " + Time.deltaTime
            + " CAMERA "
            + m_MainCamera.transform.position.ToString("F5")
            + " RB TRANSFORM " + rb.transform.position.ToString("F5")
            + " RB " + rb.position.ToString("F5")
            );
        */

        return;
    }

    private void cameraMovementIntro()
    {
        float cameraSpeed = 0.008f;
        float distance = 3.0f;

        m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                      m_MainCamera.transform.position.y + (cameraSpeed / 2.0f),
                                                      m_MainCamera.transform.position.z + (cameraSpeed * 2.0f));
        m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.04f, 0.0f);
    }

    private Vector2 getScreenResolution()
    {
        return new Vector2(width, height);
    }

    public bool isShotActive()
    {
        /*return when shot is being prepared*/
        return shotActive;
    }

    public bool isPreShotActive()
    {
        return preShotActive;
    }

    public void stopBallVel()
    {
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }

    private void RblookAtBall(Rigidbody rb,                               
                              float speed)

    {
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        Vector3 lookDirection =
             new Vector3(ballRb.transform.position.x - rb.transform.position.x,
                         0.0f,
                         ballRb.transform.position.z - rb.transform.position.z);

        lookOnLook = Quaternion.LookRotation(lookDirection);
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * speed);
    }

    private void RblookAtDirection(Rigidbody rb,
                                   Vector3 lookDirection,
                                   float speed)

    {
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);

        lookOnLook = Quaternion.LookRotation(lookDirection);
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * speed);
    }

    private void RblookAtWSPoint(Rigidbody rb,
                                 Vector3 lookPoint)

    {
        Vector3 lookDirection = Vector3.zero;
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);

        lookDirection = (lookPoint - rb.transform.position).normalized;
        lookDirection.y = 0.0f;

        lookOnLook = Quaternion.LookRotation(lookDirection);
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);
    }

    private void rotateGameObjectTowardPoint(ref GameObject gObject,
                                             Vector3 lookPoint,
                                             float slerpT)
    {
        Vector3 lookDirection = Vector3.zero;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.up);

        lookDirection = (lookPoint - gObject.transform.position).normalized;
        lookDirection.y = 0.0f;

        lookAt = Quaternion.LookRotation(lookDirection);

        gObject.transform.rotation =
            Quaternion.Slerp(gObject.transform.rotation, lookAt, slerpT);
    }

    private float getAngleBeetweenObjects(Rigidbody rb,
                                          GameObject rotatedRbToBall)
    {
        return Quaternion.Angle(rb.transform.rotation, 
                                rotatedRbToBall.transform.rotation);
    }

    private bool RblookAtDirection(Rigidbody rb,
                                   GameObject rotatedRbToBall,
                                   //Vector3 lookPoint,
                                   float stopAngle,
                                   float rotationSpeed)

    {

        float angle = Quaternion.Angle(rb.transform.rotation, rotatedRbToBall.transform.rotation);
        if (angle < stopAngle)
            return false;
      
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation,
                             rotatedRbToBall.transform.rotation,
                             Time.deltaTime * rotationSpeed);

        return true;
    }

    private void RblookAt(Rigidbody rb,
                          PlayerOnBall onBAll,
                          Vector3 playerDirection,
                          Animator animator,
                          bool preShotActive,
                          Vector3 endPosOrg,
                          bool isCpu,
                          string shotType)
    {
        bool isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);

        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        Vector3 shotDirection3D = Vector3.zero;

        if (((!preShotActive || onBAll != PlayerOnBall.ONBALL) &&
            !isAnyAnimationPlaying) ||
            (!isCpu && isPlaying(animator, "3D_run", 1.0f)))
        {
            if (!isCpu)
                if (playerDirection == Vector3.zero)
                    playerDirection = new Vector3(0f, 0f, 1f);

            lookOnLook = Quaternion.LookRotation(playerDirection);                 
        }
        else
        {
            if (!isCpu)
                endPosOrg = updateEndShotPos();
    
            shotDirection3D = (endPosOrg - rb.transform.position).normalized;
            shotDirection3D.y = 0.0f;

            /*print("DEBUG2345ANIMPLAY ROTATION ### ENDPOSROG  " 
                + endPosOrg
                + " SHOT DIRECTION " 
                + shotDirection3D
                + " EULERANGLES RB "
                + rb.transform.eulerAngles);*/

            lookOnLook = Quaternion.LookRotation(shotDirection3D);
        }

        rb.transform.rotation =
                   Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);

    }

    public void setPlayerRotation(Vector3 eulerRot)
    {
        rb.transform.eulerAngles = eulerRot;
    }

    public void setPlayerPos(Vector3 pos)
    {
        rb.transform.position = pos;
    }

    public Transform getRbTransform()
    {
        return rb.transform;
    }

    public Vector3 getPlayerPosition()
    {
        return rb.transform.position;
    }

    public Vector3 getBallPosition()
    {
        return ball.transform.position;
    }

    public Rigidbody getPlayerRb()
    {
        return rb;
    }

    public Vector3 getGkCornerPoints()
    {
        return gkCornerPoints;
    }

    public GameObject getRotatedRbToBall()
    {
        return rotatedRbToBall;
    }

    public ref GameObject getRotatedRbToBallRef()
    {
        return ref rotatedRbToBall;
    }

    public string getShotType()
    {
        return shotType;
    }

    public bool isInFront(GameObject gObject, Vector3 point)
    {
        Vector3 localSpace = InverseTransformPointUnscaled(gObject.transform, point);
        return (localSpace.z > 0f) ? true : false;
    }

    /*This function try to rotate object to look in direction as close to ball as 
     * possible by checking all points that are on left and right side of ball. 
     * It takes too corner points and check if that points are in a back rotated
     * object.*/
    public Vector3 calcGkCorrectRotationToBall(Vector3 ballPos,
                                               Rigidbody rb,
                                               ref GameObject rotatedRbToBall,
                                               Vector3 cornerPoints) {
        float pointX = cornerPoints.x;
        float pointZ = cornerPoints.z;

        float step = 0f;
        float stepOffset = 1.0f;
    
        Vector3 cornerPointLeft = new Vector3(-pointX, 0.0f, pointZ);
        Vector3 cornerPointRight = new Vector3(pointX, 0.0f, pointZ);
        Vector3 tmpBall = ballPos;

        rotatedRbToBall.transform.position = rb.transform.position;
        while (true) {

            /*Check left side of ball */
            tmpBall = ballPos;
            tmpBall.x += step;
            if (Mathf.Abs(tmpBall.x) < PITCH_WIDTH_HALF)
            {
                rotateGameObjectTowardPoint(ref rotatedRbToBall, tmpBall, 1.0f);
                if (!isInFront(rotatedRbToBall, cornerPointLeft) &&
                    !isInFront(rotatedRbToBall, cornerPointRight))
                {
                    return tmpBall;
                }
            }

            tmpBall = ballPos;
            tmpBall.x -= step;
            if (Mathf.Abs(tmpBall.x) < PITCH_WIDTH_HALF)
            {
                rotateGameObjectTowardPoint(ref rotatedRbToBall, tmpBall, 1.0f);
                if (!isInFront(rotatedRbToBall, cornerPointLeft) &&
                    !isInFront(rotatedRbToBall, cornerPointRight))
                {
                    return tmpBall;
                }
            }

            step += 1.0f;
            if (Mathf.Abs(ballPos.x - step) > PITCH_WIDTH_HALF &&
                Mathf.Abs(ballPos.x + step) > PITCH_WIDTH_HALF)
            {
                //print("DEBUGLASTTOUCHLUCKXYU NOT FOUND!!!!!!!");
                /*if no correct towards ball rotation 
                 * was not found (what may happen when you are close to goal) don't rotate them at all*/
                rotatedRbToBall.transform.eulerAngles = Vector3.zero;
                return Vector3.zero;
            }
        }
    }

    /*This function try to rotate object to look in direction as close to ball as 
     * possible by checking all points that are on left and right side of ball. 
     * It takes too corner points and check if that points are in a back rotated
     * object.*/
    public Vector3 calcGkCorrectRotationToBall(Vector3 ballPos,
                                               Rigidbody rb,
                                               ref GameObject rotatedRbToBall,
                                               float pointX,
                                               float pointZ)
    {
        float step = 0f;
        float stepOffset = 1.0f;

        Vector3 cornerPoint = new Vector3(pointX, 0.0f, pointZ);
        Vector3 tmpBall = ballPos;

        rotatedRbToBall.transform.position = rb.transform.position;
        while (true)
        {
            /*Check left side of ball */
            tmpBall = ballPos;
            tmpBall.x += step;
            if (Mathf.Abs(tmpBall.x) < PITCH_WIDTH_HALF)
            {
                rotateGameObjectTowardPoint(ref rotatedRbToBall, tmpBall, 1.0f);
                if (!isInFront(rotatedRbToBall, cornerPoint))
                {
                    return tmpBall;
                }
            }

            tmpBall = ballPos;
            tmpBall.x -= step;
            if (Mathf.Abs(tmpBall.x) < PITCH_WIDTH_HALF)
            {
                rotateGameObjectTowardPoint(ref rotatedRbToBall, tmpBall, 1.0f);
                if (!isInFront(rotatedRbToBall, cornerPoint))
                  
                {
                    return tmpBall;
                }
            }

            step += 1.0f;
            if (Mathf.Abs(ballPos.x - step) > PITCH_WIDTH_HALF &&
                Mathf.Abs(ballPos.x + step) > PITCH_WIDTH_HALF)
            {
                /*if no correct towards ball rotation 
                * was not found (what may happen when you are close to goal) don't rotate them at all*/
                rotatedRbToBall.transform.eulerAngles = Vector3.zero;

                return Vector3.zero;
            }
        }
    }

    public GameObject getRotatedRbToBall(Vector3 pos,
                                         Rigidbody rb,
                                         ref GameObject rotatedRbToBall,
                                         Vector3 cornerPoints)
    {
        Vector3 outLookPos = calcGkCorrectRotationToBall(pos,
                                                         rb,
                                                         ref rotatedRbToBall,
                                                         cornerPoints);
        return rotatedRbToBall;
    }

    private bool isRotationPossible(Animator animator,
                                    string lastGkAnimName,
                                    float lastTimeGkAnimPlayed) {

        if (isPlaying(animator, "3D_run", 1.0f))
        {
            return true;
        }

        if (lastGkAnimName.Contains("chest") && 
           (Time.time - lastTimeGkAnimPlayed) < 2.0f)
        {
            return false;
        }

        return true;
    }

    public bool isLeadingByHighScore(int teamID)
    {
        int score1 = Globals.score1;
        int scoreHandicap = Math.Min(3, (Globals.MAX_LEVEL - Globals.level));
        int score2 = Globals.score2 + scoreHandicap;

        if (teamID == 1 && (score1 > score2))
            return true;

        score1 = Globals.score1 + scoreHandicap;
        score2 = Globals.score2;

        if (teamID == 2 && (score2 > score1))
            return true;

        return false;
    }

    /*Shot using vector 3*/
    private bool preShotCalc(Vector3 startPos,
                             Vector3 midPos,
                             Vector3 endPos,
                             Vector3 endPosOrg,
                             float height,
                             Vector3 ballInitPos,
                             float shotSpeed,
                             bool isLobActive,
                             ref Vector3 ballVelocity,
                             ref Vector3 outStartPos,
                             ref Vector3 outMidPos,
                             ref Vector3 outEndPos,
                             ref SHOTVARIANT type,
                             bool isCpu)
    {
        /* Z is always the same. */
        Vector2 endPosV2 = new Vector2(endPos.x, endPos.y);
        Vector2 startPosV2 = new Vector2(startPos.x, startPos.y);
        Vector2 midPosV2 = new Vector2(midPos.x, midPos.y);
        bool isOverLine = false;

        Vector2 lineStartEnd = LineEquation(startPosV2, endPosV2);

        Vector3 shotParabolaDirect1 = Vector3.zero;
        Vector3 shotParabolaDirect2 = Vector3.zero;
        float distMidFromLine = pointDistanceFromTheLine(lineStartEnd, midPosV2);

        /*if (!isCpu)
        {
            print("DEBUG123CDALOCERROR BALLINITPOS " + ballInitPos.ToString("F5"));
        }

        if (isCpu)
        {
            print("DEBUG123CDALOCERROR preShotCalc DIsTANCE " + distMidFromLine + " lineStartEnd " + lineStartEnd);
            print("DEBUG123CDALOCERROR preShotCalc STARTPOS " + startPos + " MIDPOS " + midPos + " ENDPOS " + endPos);
        }*/

        /*used by training*/
        if (!isCpu)
        {
            lastDistFromMidLine = distMidFromLine;
        }
        /*if (!isCpu)
            print("DEBUG2112XXF STARTHRNEW " + startPos + " MIDPOS " + midPos + " ENDPOS " + endPos + " lineSTARTEND " 
                + lineStartEnd + " DISTMIDFROMLINE " + distMidFromLine + " midPosV2 " + midPosV2
                + " LOCAL3SHOT " + localMidPos3);*/

        if (endPosOrg.y < ballRadius)
            endPosOrg.y = ballRadius;

        /* Shot straight */ /*CHANGE TO 0.5f back */
        if (distMidFromLine <= 1.0f && !isLobActive)
        {
            Vector3 shotDirection3D = (endPosOrg - ballInitPos).normalized;
            float velocity = 25.0f;
            if (shotSpeed != 0.0f)
                velocity = Mathf.Min(shotSpeed / 3.5f, 34.5f);

            /*TODELETE*/
            if (!isCpu)
            {
                //shotDirection3D = (new Vector3(-3.1f, 0.7f, 13.5f) - ballInitPos).normalized;
                //shotDirection3D.y = 0.5f;

            }

            ballVelocity = shotDirection3D * velocity;
            type = SHOTVARIANT.STRAIGHT;
            //print("GKMOVESEXEC3 TIMETOHITX BALLVELOCITY PRE" + ballVelocity);

            //ballRb.velocity = shotDirection3D * (30.0f / (maxTime / 1.15f));
            //ballRb.AddForce(shotDirection3D * velocity);
            //print("BALLRB VELOCITY" + ballRb.velocity);
            return true;
        }
        else
        {
            string side = "left";

            if (isLobActive && distMidFromLine <= 1.0f)
            {
                distMidFromLine = 0.0f;
            }

            dummyTouchRotatedGO.transform.position =
                new Vector3(startPos.x, 0f, startPos.y);
            rotateGameObjectTowardPoint(
                ref dummyTouchRotatedGO, new Vector3(endPos.x, 0f, endPos.y), 1f);

            Vector3 midPos3 = new Vector3(midPos.x, 0f, midPos.y);
            Vector3 midPos3Local =
                InverseTransformPointUnscaled(dummyTouchRotatedGO.transform,
                        midPos3);

            //if (!isCpu)
            //{
            if (midPos3Local.x < 0f)
            {
                side = "left";
            }
            else
            {
                if (midPos3Local.x > 0f)
                {
                    side = "right";
                }
                else
                {
                    side = "center";
                }
            }
            //}

            dummyTouchRotatedGO.transform.position =
                new Vector3(ballInitPos.x, 0f, ballInitPos.z);
            rotateGameObjectTowardPoint(
                ref dummyTouchRotatedGO, new Vector3(endPosOrg.x, 0f, endPosOrg.z), 1f);

            /*if (isCpu)
            {
                print("DEBUG123CDALOC " + side + " MIDPOSLOCAL" + midPos3Local);
            }*/

            distMidFromLine = Mathf.Min(distMidFromLine, CURVE_SHOT_MAX_DIST);
            if (side.Equals("right"))
                midPos3Local.x = distMidFromLine;
            else
            {
                if (side.Equals("left"))
                {
                    midPos3Local.x = -distMidFromLine;
                }
            }

            midPos3Local.y = 0f;
            midPos3Local.z = Vector2.Distance(new Vector2(endPosOrg.x, endPosOrg.z),
                                              new Vector2(ballInitPos.x, ballInitPos.z)) / 2f;

            /*if (isCpu)
            {
                print("DEBUG123CDALOC Z DISTANCE " + midPos3Local.z);
            }*/

            outMidPos = TransformPointUnscaled(dummyTouchRotatedGO.transform, midPos3Local);
            outMidPos.y = endPosOrg.y / 2.0f;
            /*TOCHECK*/
            outMidPos.y = Math.Max(BALL_RADIUS, outMidPos.y);

            if (isLobActive)
            {
                if (!isCpu)
                    outMidPos.y = 11.0f;                
            }

            outStartPos = ballInitPos;
            outEndPos = endPosOrg;

            type = SHOTVARIANT.CURVE;
            //if (!isCpu)
            //    print("DEBUG123CDALOCERROR " + outStartPos + " MIDPOS " + outMidPos + " OUTENDPOS " + outEndPos);

            return false;

            //int curveSide2D = geometry.checkSidePointLine2D(lineStartEnd, midPosV2);

            //print("DEBUG123CDALOC curvsSide2D line " + lineStartEnd 
            //    + " MIDPOSV2 " + midPosV2 + " RESULT: " + curveSide2D);
            //startPosV2 = new Vector2(ballInitPos.x, ballInitPos.z);
            //midPosV2 = new Vector2(midPos.x, midPos.y);
            //endPosV2 = new Vector2(endPosOrg.x, endPosOrg.z);        

            /* user Bézier curve to curve shots */
            //distMidFromLine = Mathf.Min(distMidFromLine, CURVE_SHOT_MAX_DIST);
            //if (isLobActive && distMidFromLine <= 1.0f)
            //{
            //    distMidFromLine = 0.0f;
            //}

            //Vector2 middlePoint = new Vector2((endPosV2.x + startPosV2.x) / 2.0f, (endPosV2.y + startPosV2.y) / 2.0f);
            //if (isLobActive)
            //{
            /*This formula only works for even numbers div by 8*/
            //   middlePoint = new Vector2((middlePoint.x + startPosV2.x) / 2.0f, (middlePoint.y + startPosV2.y) / 2.0f);
            //   middlePoint = new Vector2((middlePoint.x + startPosV2.x) / 2.0f, (middlePoint.y + startPosV2.y) / 2.0f);
            //??
            //middlePoint = new Vector2((middlePoint.x + startPosV2.x) / 2.0f, (middlePoint.y + startPosV2.y) / 2.0f);
            //}

            //lineStartEnd = LineEquation(startPosV2, endPosV2);
            //print("DEBUG11 CURVE STARTPOS " + startPosV2 + " ENDPOSV2 " + endPosV2 + " linestartend " 
            //    + lineStartEnd + " MIDDLE POINT " + middlePoint);
            //Vector2 perpLineCooeficients = perpendicularLineEquation(
            //    lineStartEnd,
            //    middlePoint);

            //Vector2 otherPoint;
            //midPos = midPosV2;

            //float x = 2.0f * (float) width;
            //otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);

            //int curveSide3D = geometry.checkSidePointLine2D(lineStartEnd, otherPoint);
            //print("DEBUG12345XA curvsSide3D_1 line " + lineStartEnd
            //        + " MIDPOSV2 " + otherPoint + " RESULT: " + curveSide3D);

            //if (curveSide2D != curveSide3D)
            //{
            //    x = (float) -width;
            //    otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);

            //    print("DEBUG12345XA curvsSide3D_2 line " + lineStartEnd
            //     + " MIDPOSV2 " + otherPoint + " RESULT: " + curveSide3D);
            //}

            ////float x = 2.0f * (float) width;
            ////otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);    
            /*Check on which side of line midPos lies */
            ////if (!((midPos.y >= ((midPos.x * lineStartEnd.x) + lineStartEnd.y) &&
            ////      otherPoint.y >= ((otherPoint.x * lineStartEnd.x) + lineStartEnd.y)) ||
            ////     (midPos.y <= ((midPos.x * lineStartEnd.x) + lineStartEnd.y) &&
            ////      otherPoint.y <= ((otherPoint.x * lineStartEnd.x) + lineStartEnd.y))))
            ////{
            ////    x = (float) -width;
            ////     otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);
            ////}

            //print("EQUS PREV MID POINT " + midPos.x + " Y " + midPos.y);
            //print("TESTXX " + startPos + " ENDPOS " + endPos);

            /* Find direction to otherPoint from middlePoint - this is used to find a another point away used to curve a shot*/
            //Vector2 directionOther = (otherPoint - middlePoint).normalized;          
            //Vector2 midPosDistAway = middlePoint + (directionOther * distMidFromLine);

            //print("DEBUG11 MIDLE POOINT STARTPOS " + startPosV2 + " ENDPOS " + endPosV2 + " MIDPOS " + midPosDistAway + " MIDPOSDIR " + midPosV2
            //    + " lineSTARTEND " + lineStartEnd + " DISTMIDFROMLINE " + distMidFromLine + " LOCAL3SHOT " + localMidPos3);

            //if (Mathf.Abs(endPosOrg.y) < 0.1f)
            //    endPosOrg.y = 0.62f;

            /*Vector3 midPosDistAwayV3 = new Vector3(midPosDistAway.x,
                                                   endPosOrg.y / 2.0f,
                                                   midPosDistAway.y);
            
            if (isLobActive)
            {
                if (!isCpu)
                    midPosDistAwayV3.y = 11.0f;
                else
                    //midPosDistAwayV3.y = 7.0f;
            }

            outStartPos = ballInitPos;
            outMidPos = midPosDistAwayV3;
            outEndPos = endPosOrg;
            type = SHOTVARIANT.CURVE;
            */

            //print("DEBUG12345XA preSHOTCALCFINALVALUES OUTBALLVEL SHOT VARIANT CURVE DIST " + distMidFromLine + " OUTSTART " 
            //   + outStartPos + " OUTMID " + outMidPos + " outEndPos " + outEndPos);


            //return false;  
        }

        return false;
    }

    private Vector3 getCurveShotPos(Vector3 startPos,
                                    Vector3 midPos,
                                    Vector3 endPos,
                                    float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
        Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
        Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

        return currPos;
    }

    private Vector3 getCurveShotPosLocal(GameObject gObject,
                                         Vector3 startPos,
                                         Vector3 midPos,
                                         Vector3 endPos,
                                         float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
        Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
        Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

        Vector3 localSpace = InverseTransformPointUnscaled(gObject.transform, currPos);
        return localSpace;
    }

    private Vector3 getCurveShotPosLocal(Rigidbody gObject,
                                         Vector3 startPos,
                                         Vector3 midPos,
                                         Vector3 endPos,
                                         float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
        Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
        Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

        Vector3 localSpace = InverseTransformPointUnscaled(gObject.transform, currPos);
        return localSpace;
    }

    /*Shot using vector 3*/
    private bool shot3New(Vector3 startPos,
                          Vector3 midPos,
                          Vector3 endPos,
                          Vector3 ballVelocity,
                          ref Vector3 lastBallVelocity,
                          SHOTVARIANT type,
                          float currentTime)
    {
        if (type == SHOTVARIANT.STRAIGHT)
        {
            ballRb.velocity = ballVelocity;
            //print("DEBUG111X BallRBVELOCITY STRIAGHT VELOCITY SETACTIVE " + ballRb.velocity);
            lastBallVelocity = ballRb.velocity;

            //print("CURRENTIME STRAIGHT");
            return false;
        } else
        {
            //print("GKDEBUG7 CURRENTIME ENTERED " + currentTime);
            if (currentTime >= 1.0f)
                return true;

            Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
            Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
            Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);
            float delta = 1.0f - currentTime;

            ballRb.velocity = new Vector3((currPos.x - ballRb.transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb.transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb.transform.position.z) / Time.deltaTime);

            ballRb.angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            lastBallVelocity = ballRb.velocity;

            //print("DEBUG111X BallRBVELOCITY CURVE VELOCITY SETACTIVE " + ballRb.velocity);
            //print("currentTime " + currentTime);
            return false;
        }
    }

    private Vector3 shot(Vector2 startPos,
                         Vector2 endPos,
                         Vector2 midPos,
                         float height,
                         Vector3 ballInitPos,
                         float currentTime,
                         float maxTime)
    {
        Vector2 lineStartEnd = LineEquation(startPos, endPos);

        Vector3 shotParabolaDirect1 = Vector3.zero;
        Vector3 shotParabolaDirect2 = Vector3.zero;
        float distMidFromLine = pointDistanceFromTheLine(lineStartEnd, midPos);

        //print("DISTFROMLINEXXX " + distMidFromLine);

        /* Shot straight */
        if (distMidFromLine <= 20.0f)
        {
            Vector2 shotDirection2D = (endPos - startPos).normalized;
            Vector3 direction3DShot = new Vector3(shotDirection2D.x, height / 30.0f, shotDirection2D.y);
            ballRb.velocity = direction3DShot * (30.0f / maxTime);
            //ballRb.AddForce(direction3DShot * (30.0f / maxTime) * 1.8f);
            //print("BALLRB VELOCITY" + ballRb.velocity);
        }
        else
        {
            //print("MIDFROMLINE " + distMidFromLine);
            float dist = Vector2.Distance(startPos, endPos);
            //print("DistanceSTARTEND " + dist);
            if (dist < shotCurveMinDist)
            {
                //print("DISTANCECURVE BEFORE " + dist + "startPOs" + startPos + " MIDPOS " + midPos + " ENDPOS " + endPos);
                Vector2 endDirection = (endPos - startPos).normalized;
                /*if the distance beetween startPos and endPos is too small, extend it. Curve will look much better*/
                endPos = startPos + (endDirection * shotCurveMinDist);
                //print("DISTANCECURVE AFTER " + Vector2.Distance(startPos, endPos));
            }

            /* user Bézier curve to curve shots */
            //print("CURVE BALL");
            Vector2 middlePoint = new Vector2((endPos.x + startPos.x) / 2.0f, (endPos.y + startPos.y) / 2.0f);
            Vector2 perpLineCooeficients = perpendicularLineEquation(
                lineStartEnd,
                middlePoint);

            Vector2 otherPoint;

            float x = 2.0f * (float)width;
            otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);
            //print("EQUS OTHER POINT X " + otherPoint.x + " otherPOINT Y " + otherPoint.y);
            //print("TESTXXX " + middlePoint + " OTHERPOINT " + otherPoint);
            /*Check on which side of line midPos lies */
            if (!((midPos.y >= ((midPos.x * lineStartEnd.x) + lineStartEnd.y) &&
                  otherPoint.y >= ((otherPoint.x * lineStartEnd.x) + lineStartEnd.y)) ||
                 (midPos.y <= ((midPos.x * lineStartEnd.x) + lineStartEnd.y) &&
                  otherPoint.y <= ((otherPoint.x * lineStartEnd.x) + lineStartEnd.y))))
            {
                x = (float)-width;
                otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);
                //print("EQUS OTHER POINT OVERWRITTEN " + otherPoint.x + " otherPOINT Y " + otherPoint.y);
            }

            //print("EQUS PREV MID POINT " + midPos.x + " Y " + midPos.y);
            //print("TESTXX " + startPos + " ENDPOS " + endPos);

            /* Find direction to otherPoint from middlePoint - this is used to find a another point away used to curve a shot*/
            Vector2 direction = (otherPoint - middlePoint).normalized;

            //print("DISTancemidpointFROMLINE " + distMidFromLine);
            distMidFromLine = Mathf.Min(distMidFromLine, 200.0f);
            float curveOffsetNorm = Mathf.InverseLerp(0.0f, 200.0f, distMidFromLine);

            //print("DISTancemidpoint OFFSET" + curveOffsetNorm);

            //curveOffset = Mathf.Max(curveOffset, 0.0f) * 60.0f;
            curveOffsetNorm = curveOffsetNorm * 110.0f;
            //print("DISTancemidpoint AFTER" + curveOffsetNorm);

            Vector2 midPosDistAway = middlePoint + (direction * curveOffsetNorm);

            /*print("EQUS START " + startPos + " END " + endPos);
            print("EQUS LINE X " + lineStartEnd.x);
            print("EQUS LINE Y " + lineStartEnd.y);
            print("EQUS MIDDLE POINT X " + middlePoint.x + " Y " + middlePoint.y);
            print("EQUS PERP LINE X " + perpLineCooeficients.x + " PERP LINE Y " + perpLineCooeficients.y);
            print("EQUS OTHER POINT X " + otherPoint.x + " otherPOINT Y " + otherPoint.y);
            print("EQUS MID POINT CALCULATED X " + midPosDistAway.x + " Y " + midPosDistAway.y);*/

            float curvepPointDistAway = 11.0f;


            //print("DRAWDISTANCE " + drawDistance);

            startNorm = (midPosDistAway - startPos).normalized;
            shotParabolaDirect1 = new Vector3(
                    (startNorm.x * curvepPointDistAway) + ballInitPos.x,
                    height,
                    (startNorm.y * curvepPointDistAway) + ballInitPos.z);

            /*Added */
            /*startNorm = (midPosDistAway - startPos);
            shotParabolaDirect1 = new Vector3(
                    startNorm.x + ballInitPos.x,
                    height,
                    startNorm.y + ballInitPos.z);

            print("SHOTPARABOL1 " + shotParabolaDirect1);*/


            endNorm = (endPos - midPosDistAway).normalized;
            shotParabolaDirect2 = new Vector3(
                (endNorm.x * curvepPointDistAway) + shotParabolaDirect1.x,
                Mathf.Max(height - 3.0f, 0),
                (endNorm.y * curvepPointDistAway) + shotParabolaDirect1.z);

            //print("TESTXXX " + ballInitPos + " shot parabola " + shotParabolaDirect1 + " shotParabola );
            //print("SHOTPARABOL2 " + shotParabolaDirect2);

            //currentTime += 0.07f;
            Vector3 m1 = Vector3.Lerp(ballInitPos, shotParabolaDirect1, currentTime);
            Vector3 m2 = Vector3.Lerp(shotParabolaDirect1, shotParabolaDirect2, currentTime);


            //Vector3 m1 = Vector3.SmoothDamp(ballInitPos, shotParabolaDirect1, ref m1Smooth, 1.0f);
            //Vector3 m2 = Vector3.SmoothDamp(shotParabolaDirect1, shotParabolaDirect2, ref m2Smooth, 1.0f);
            //Vector3 currPos = Vector3.SmoothDamp(m1, m2, ref velocityShotSmooth, 2.0f);
            //ballRb.transform.position = currPos;
            Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

            /*ballRb.velocity = new Vector3((currPos.x - ballRb.transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb.transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb.transform.position.z) / Time.deltaTime);*/

            float delta = 1.0f - currentTime;
            //Vector3 currPos = (delta * delta * ballInitPos) + (2 * delta * currentTime * shotParabolaDirect1) + (currentTime * currentTime * shotParabolaDirect2);
            //print("VELOCITYCURRPOST " + currPos);

            ballRb.velocity = new Vector3((currPos.x - ballRb.transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb.transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb.transform.position.z) / Time.deltaTime);




            //print("BallRBVELOCITY " + ballRb.velocity);
            //print("currentTime " + currentTime);
            //Mathf.Lerp(start, end, Mathf.SmoothStep(0.0, 1.0, t));

            //Vector3 vel = new Vector3(1, 2, 2);

            //Vector3 velocity = Vector3.zero;
            //ballRb.transform.position = Vector3.SmoothDamp(ballRb.transform.position, shotParabolaDirect1, ref velocity, 0.2f);
            //print("BALL TRANSFORM POSITION " + ballRb.transform.position);
            //print("smoothdamp " + Vector3.SmoothDamp(ballInitPos, shotParabolaDirect2, ref velocity, 1.5f));
            //print("BALLPOSVEL " + ballRb.velocity + " ballRBMAGINTUE " + ballRb.velocity.magnitude);
        }


        //ballRb.transform.Rotate((Vector3(0.0f, 0.0f, -10.0f) * Time.deltaTime), Space.World);
        /*llRb.transform.rotation = new Vector3(ballRb.transform.rotation.x, 
                                                ballRb.transform.rotation.y + 1, 
                                                ballRb.transform.rotation.z);*/
        return Vector3.zero;
    }

    private void initFlagPositions()
    {
        int randomValue = 1;
        for (int i = 0; i < FANS_FLAG_MAX; i++)
        {
            /*No flag when traning mode */
            if (isTrainingActive)
            {
                isFansFlagActive[i] = false;
                fansFlagSticks[i].SetActive(false);
                continue;
            }

            randomValue = UnityEngine.Random.Range(1, 11);

            if (randomValue > 8)
            {
                isFansFlagActive[i] = false;
                fansFlagSticks[i].SetActive(false);
                //isFansFlagActive[i] = true;
                //fansFlagSticks[i].SetActive(true);
                continue;
            } else
            {
                isFansFlagActive[i] = true;
            }

            fansFlagAngles[i] = fansFlagSticks[i].transform.eulerAngles;
            if (i % 2 == 0)
            {
                fansFlagDirections[i].x = 0.15f;
                fansFlagDirections[i].y = 0.15f;
                fansFlagDirections[i].z = 0.10f;
            }
            else
            {
                fansFlagDirections[i].x = 0.20f;
                fansFlagDirections[i].y = 0.15f;
                fansFlagDirections[i].z = -0.05f;
            }
        }
    }

    private void updateFlagsPositions()
    {

        for (int i = 0; i < FANS_FLAG_MAX; i++)
        {
            if (!isFansFlagActive[i]) continue;

            if (fansFlagAngles[i].x > 30.0f || fansFlagAngles[i].x < -1.0f)
                fansFlagDirections[i].x = -fansFlagDirections[i].x;

            if (fansFlagAngles[i].y > -10f || fansFlagAngles[i].y < -50.0f)
                fansFlagDirections[i].y = -fansFlagDirections[i].y;

            if (fansFlagAngles[i].z > 5.0f || fansFlagAngles[i].z < -4.0f)
                fansFlagDirections[i].z = -fansFlagDirections[i].z;

            fansFlagAngles[i].x += fansFlagDirections[i].x;
            fansFlagAngles[i].y += fansFlagDirections[i].y;
            fansFlagAngles[i].z += fansFlagDirections[i].z;

            //print(" FANSFLAGANGLES " + i + " " + fansFlagAngles[i]);

            fansFlagSticks[i].transform.eulerAngles = fansFlagAngles[i];
        }
    }

    private void updateStadiumTextures()
    {
        //int teamColorChoosen = Globals.stadiumColorTeamA;
        string teamColorChoosen = Globals.stadiumColorTeamA;
        if (teamHostID == 2)
            teamColorChoosen = Globals.stadiumColorTeamB;
        //teamColorChoosen = Globals.stadiumColorTeamB;

        //print("#DBGFANSCOLOR " + teamColorChoosen);

        string[] stadiumColors = teamColorChoosen.Split('|');

        string fansColor = stadiumColors[0];
        string bannerColor = stadiumColors[1];
        string fansFlagName = stadiumColors[2];

        //print("#DBGFANSCOLOR " + fansColor + " BANNERCOLOR " + bannerColor);

        Texture2D texturePeople =
            graphics.getTexture("stadium/fans_" + fansColor);
        //"stadium/st_040_people_" + teamColorChoosen.ToString());

        Texture2D textureBanner =
            graphics.getTexture("stadium/banner_" + bannerColor);
            //"stadium/st_040_banner_" + teamColorChoosen.ToString());

        Material[] materials = stadium.GetComponent<Renderer>().materials;
        materials[2].mainTexture = textureBanner;
        stadium.GetComponent<Renderer>().materials = materials;

        foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (allStadiumPeople.name.Contains("crowdAnimated"))
            {                
                allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);
            }
        }

        /*try
        {
            stadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);
        }
        catch (NullReferenceException ex)
        {
            print("People texture not loaded");
        }*/

        //string teamName = Globals.teamAname;
        //if (teamHostID == 2)
        //{
        //    teamName = Globals.teamBname;
        //}
        //teamName = Regex.Replace(teamName, "\\s+", "");

        //Texture2D flagTextureFans = graphics.getTexture(
        //"FlagsFans/" + teamName.ToLower());

        int flagRand = UnityEngine.Random.Range(1, 5);
        Texture2D flagTextureFans;
        for (int i = 0; i < FANS_FLAG_MAX; i++)
        {
            //Texture2D flagTextureFans = graphics.getTexture(
            //   "FlagsFans/" + fansFlagName + "_" + flagRand.ToString());
            flagTextureFans = graphics.getTexture(
                        "FlagsFans/" + fansFlagName);

            //print("#DBGFANSFLAGS " + ("FlagsFans/" + fansFlagName) +
            //    " Globals.leagueName " + Globals.leagueName);

            if (Globals.isPlayerCardLeague(Globals.leagueName))
            {
                flagTextureFans = graphics.getTexture(
                    "FlagsFans/" + fansFlagName + "_" + flagRand);
            }
           
            fansFlag[i].GetComponent<Renderer>().material.SetTexture("_MainTex", flagTextureFans);
        }
    }

    public int getTeamHostId()
    {
        return teamHostID;
    }

    public void OnCollisionEnter(Collision other)
    {   
    }

    private bool isPlaying(Animator animator, string stateName, float end)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                (animator.GetCurrentAnimatorStateInfo(0).normalizedTime) <= end)
            return true;
        else
            return false;
    }

    private string getShotNameByIndex(int idx)
    {
        if (idx > shotTypesNames.Count - 1)
            return "3D_shot_left_foot";

        return shotTypesNames.ElementAt(idx);
    }

    private void initShotsList()
    {
        shotTypesNames.Add("3D_shot_left_foot");
        shotTypesNames.Add("3D_shot_right_foot");
        /*3D_volley_before must be executed first */
        shotTypesNames.Add("3D_volley_before");
    }

    private void initAnimationList()
    {
        /*TODELETE*/

        /*straight are not sidecatch but they are rename to keep compability*/
        AllAnimationsNames.Add("3D_GK_sidecatch_straight_up");
        AllAnimationsNames.Add("3D_GK_sidecatch_straight_mid");
        AllAnimationsNames.Add("3D_GK_sidecatch_straight_down");
        AllAnimationsNames.Add("3D_GK_sidecatch_straight_chest_mid");

        AllAnimationsNames.Add("3D_GK_sidecatch_left_down");
        AllAnimationsNames.Add("3D_GK_sidecatch_left_mid");
        AllAnimationsNames.Add("3D_GK_sidecatch_left_up");
        AllAnimationsNames.Add("3D_GK_sidecatch_right_down");
        AllAnimationsNames.Add("3D_GK_sidecatch_right_mid");
        AllAnimationsNames.Add("3D_GK_sidecatch_right_up");

        AllAnimationsNames.Add("3D_GK_sidecatch_leftpunch_down");
        AllAnimationsNames.Add("3D_GK_sidecatch_leftpunch_mid");
        AllAnimationsNames.Add("3D_GK_sidecatch_leftpunch_up");
        AllAnimationsNames.Add("3D_GK_sidecatch_rightpunch_down");
        AllAnimationsNames.Add("3D_GK_sidecatch_rightpunch_mid");
        AllAnimationsNames.Add("3D_GK_sidecatch_rightpunch_up");

        AllAnimationsNames.Add("3D_GK_step_left");
        AllAnimationsNames.Add("3D_GK_step_right");

        AllAnimationsNames.Add("3D_GK_step_left_no_offset");
        AllAnimationsNames.Add("3D_GK_step_right_no_offset");

        AllAnimationsNames.Add("3D_GK_step_left_no_offset_nocpu");
        AllAnimationsNames.Add("3D_GK_step_right_no_offset_nocpu");

        AllAnimationsNames.Add("3D_back_run");
        AllAnimationsNames.Add("3D_walk");

        //AllAnimationsNames.Add("3D_GK_step");
        //AllAnimationsNames.Add("3D_GK_bear");
        //AllAnimationsNames.Add("3D_guard");
        AllAnimationsNames.Add("3D_shot_right_foot");
        AllAnimationsNames.Add("3D_shot_left_foot");

        //AllAnimationsNames.Add("3D_trick1");
        //AllAnimationsNames.Add("3D_trick1");

        //AllAnimationsNames.Add("3D_GK_step_left");
        //AllAnimationsNames.Add("3D_GK_step_right");
        AllAnimationsNames.Add("3D_volley_before");
        AllAnimationsNames.Add("3D_volley");
        AllAnimationsNames.Add("3D_overhead");
    }

    private void initRunAnimationList()
    {
        RunAnimationsNames.Add("3D_run_45turn_out_left");
        RunAnimationsNames.Add("3D_run_45turn_out_right");
        RunAnimationsNames.Add("3D_run_90turn_out_left");
        RunAnimationsNames.Add("3D_run_90turn_out_right");
        RunAnimationsNames.Add("3D_run_135turn_out_left");
        RunAnimationsNames.Add("3D_run_135turn_out_right");
        RunAnimationsNames.Add("3D_run_180turn_out_left");
        RunAnimationsNames.Add("3D_run_180turn_out_right");

        RunAnimationsNames.Add("3D_run_45turn_in_left");
        RunAnimationsNames.Add("3D_run_45turn_in_right");
        RunAnimationsNames.Add("3D_run_90turn_in_left");
        RunAnimationsNames.Add("3D_run_90turn_in_right");
        RunAnimationsNames.Add("3D_run_135turn_in_left");
        RunAnimationsNames.Add("3D_run_135turn_in_right");
    }


    /*NOT MINE CODE */
    private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 2.0f, float width = 0.15f)
    {
        //print("VECTORXASD " + start + " end " + end);
        //start.z = -3.0f;
        //end.z = -3.0f;
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer touchLine = myLine.GetComponent<LineRenderer>();
        touchLine.SetVertexCount(2);
        //touchLine.SetColors(color, color);
        touchLine.material = new Material(Shader.Find("Sprites/Default"));
        touchLine.material.SetColor("_TintColor", new Color(1, 1, 1, 0.5f));

        Color c1 = color;
        Color c2 = new Color(1, 1, 1, 0.5f);
        touchLine.SetColors(c2, c2);

        //touchLine.SetWidth(0, 3);
        touchLine.SetPosition(0, start);
        touchLine.SetPosition(1, end);
        touchLine.SetWidth(width, width);
        touchLine.startWidth = width;
        touchLine.endWidth = width;
        //touchLine.useWorldSpace = false;
        GameObject.Destroy(myLine, duration);
        //print("DRAWLINE FROM " + start + " END " + end);
    }

    /* This functions return coefficients of a and b as a 
    * Vector2 of y = ax + b function */
    private Vector2 LineEquation(Vector2 p1, Vector2 p2)
    {
        float a = (float)p2.x - (float)p1.x;
        float b;
        float y = (float)p2.y - (float)p1.y;

        //print("EQUS DIFF X " + a);
        //print("EQUS DIFF Y " + y);

        if (a == 0) a = 0.0001f;

        if (a != 0.0)
            a = y / a;
        else
            a = float.MaxValue;

        if (a == float.MaxValue)
            b = float.MaxValue;
        else
        {
            b = p1.y - (a * p1.x);
        }

        //print("EQUS " + a + " b " + b);

        return new Vector2(a, b);
    }

    private Vector2[] pointsDistAwayOnPerpendicularLine(Vector2 startPosV2,
                                                      Vector2 endPosV2,
                                                      Vector2 linePerpendicularPoint,
                                                      float dist)
    {
        Vector2[] points = new Vector2[2];
        Vector2 directionOther;
        Vector2 midPosDistAway;

        Vector2 lineStartEnd = LineEquation(startPosV2, endPosV2);
        Vector2 perpLineCooeficients = perpendicularLineEquation(
            lineStartEnd,
            linePerpendicularPoint);

        Vector2 otherPoint;
        float x = 2.0f * (float)width;
        otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);
        /* Find direction to otherPoint from middlePoint - this is used to find a another point away used to curve a shot*/
        directionOther = (otherPoint - linePerpendicularPoint).normalized;
        midPosDistAway = linePerpendicularPoint + (directionOther * dist);
        points[0] = midPosDistAway;

        x = (float)-width;
        otherPoint = new Vector2(x, (x * perpLineCooeficients.x) + perpLineCooeficients.y);
        directionOther = (otherPoint - linePerpendicularPoint).normalized;
        midPosDistAway = linePerpendicularPoint + (directionOther * dist);
        points[1] = midPosDistAway;

        return points;
    }

    /* This return a equation of perpendicular line in point*/
    private Vector2 perpendicularLineEquation(Vector2 line, Vector2 point)
    {
        float a = -(1.0f / line.x);
        float b = point.y - (a * point.x);
        return new Vector2(a, b);
    }

    private float pointDistanceFromTheLine(Vector2 line, Vector2 point)
    {
        float a = line.x;
        float b = -1;
        float c = line.y;

        return Mathf.Abs((a * point.x) + (b * point.y) + c) / (Mathf.Sqrt((a * a) + (b * b)));
    }

    public bool compareSign(float val1, float val2)
    {
        return Mathf.Sign(val1) == Mathf.Sign(val2);
    }

    /*Time is return as z value is z overwritee = true*/
    private Vector3 bezierCurvePlaneInterPoint(float s,
                                               float e,
                                               GameObject gObject,
                                               Vector3 startPos,
                                               Vector3 midPos,
                                               Vector3 endPos,
                                               bool overwritteZ)
    {
        float time = (s + e) / 2.0f;
        Vector3 midPoint = getCurveShotPosLocal(gObject, startPos, midPos, endPos, time);

        /*to delete endPoint */
        Vector3 startPointTmp = getCurveShotPosLocal(gObject, startPos, midPos, endPos, s);
        Vector3 endPointTmp = getCurveShotPosLocal(gObject, startPos, midPos, endPos, e);

        //print("DEBUGLASTTOUCHLUCKXYU " + " S " + startPointTmp + " M " + midPoint + " E " + endPointTmp 
        //    + " TIME S " + s + " MID " + time  + " END " + e);

        /*This conditions should allow to execute loop more than 12 times */
        if ((e - s) <= 0.0005f ||
            (Mathf.Abs(midPoint.z) <= 0.1f) ||
            (e <= s))
        {
            /*Use z vector cord as time */
            if (overwritteZ)
                midPoint.z = time;
            return midPoint;
        }

        Vector3 endPoint = getCurveShotPosLocal(gObject, startPos, midPos, endPos, e);
        if (!compareSign(midPoint.z, endPoint.z))
            return bezierCurvePlaneInterPoint(time, e, gObject, startPos, midPos, endPos, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(s, time, gObject, startPos, midPos, endPos, overwritteZ);
    }


    /*Time is return as z value is z overwritee = true*/
    private Vector3 bezierCurvePlaneInterPoint(float s,
                                               float e,
                                               Rigidbody rb,
                                               Vector3 startPos,
                                               Vector3 midPos,
                                               Vector3 endPos,
                                               bool overwritteZ)
    {
        float time = (s + e) / 2.0f;
        Vector3 midPoint = getCurveShotPosLocal(rb, startPos, midPos, endPos, time);

        //print("BEZIERCURE " + " S " + s + " E " + e + " TIME " + time + " POINT " + point);

        if ((e - s) <= 0.0005f ||
            (Mathf.Abs(midPoint.z) <= 0.1f) ||
            (e <= s))
        {
            /*Use z vector cord as time */
            if (overwritteZ)
                midPoint.z = time;
            return midPoint;
        }

        Vector3 endPoint = getCurveShotPosLocal(rb, startPos, midPos, endPos, e);
        if (!compareSign(midPoint.z, endPoint.z))
            return bezierCurvePlaneInterPoint(time, e, rb, startPos, midPos, endPos, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(s, time, rb, startPos, midPos, endPos, overwritteZ);

        /*Vector3 startPoint = getCurveShotPosLocal(rb, startPos, midPos, endPos, s);   
        /*if (point.z > 0)
        {
            return bezierCurvePlaneInterPoint(time, e, rb, startPos, midPos, endPos, overwritteZ);
        } else
        {
            return bezierCurvePlaneInterPoint(s, time, rb, startPos, midPos, endPos, overwritteZ);
        }*/
    }

    public bool getTimeToShotExceeded()
    {
        return timeToShotExceeded;
    }

    public bool getGameEnded()
    {
        return gameEnded;
    }

    public void setGameEnded(bool value)
    {
        gameEnded = value;
    }

    public Vector3 getBallInit()
    {
        return ballInitPos;
    }

    float calculatedTimeToStartPos = 0.227f;

    /* Goalkeeper movement - dist is only related to cpu player*/
    private void gkMoves(Animator animator,
                         Rigidbody rb,
                         bool isCpuPlayer,
                         ref string lastAnimName,
                         ref float lastTimeGkAnimPlayed,
                         ref float distXcord,
                         ref Vector3 gkStartPos,
                         ref Transform gkStartTransform,
                         ref float gkTimeToCorrectPos,
                         ref bool initCpuAdjustAnimSpeed,
                         ref bool initGkDeleyLevel,
                         ref float levelDelay,
                         ref string initAnimName,
                         ref float cpuAnimAdjustSpeed,
                         ref string gkAction,
                         ref float gkTimeLastCatch,
                         bool isLobActive,
                         ref Vector3 stepSideAnimOffset,
                         ref bool gkLobPointReached,
                         ref bool gkRunPosReached,
                         ref float initDistX,
                         SHOTVARIANT shotvariant,
                         Vector3 outShotStart,
                         Vector3 outShotMid,
                         Vector3 outShotEnd,
                         Vector3 endPosOrg,
                         float timeofBallFly,
                         float passedShotFlyTime,
                         ref bool gkLock,
                         ref GameObject rotatedRbToBall,
                         Vector3 cornerPoints)
    {
        bool negativeRun = false;
        float timeToHitX = float.MaxValue;
        float timeToHitZ = float.MaxValue;
        float timeToHitY = float.MaxValue;

        if (!isCpuPlayer && gkTouchDone == false)
        {
            //print("ANGLEKACZY EXECUTED GKMOVES EXECUTED 1 " + gkTouchDone);
            return;
        }

        //print("GKMOVESEXEC2");

        //if (checkIfAnyAnimationPlaying(animator, 0.8f) && !isCpuPlayer)
        string[] excluded = new string[] { "3D_back_run_cpu",
                                           "3D_GK_step_left_no_offset",
                                           "3D_GK_step_right_no_offset" };

        if (!isCpuPlayer)
            excluded = new string[] { "3D_back_run_cpu",
                                      "3D_GK_step_left_no_offset_nocpu",
                                      "3D_GK_step_right_no_offset_nocpu" };

        //print("GKDEBUG800 isAnimationPlaying" + (checkIfAnyAnimationPlaying(animator, 1.0f, excluded)));
        if (checkIfAnyAnimationPlaying(animator, 1.0f, excluded)) {
            //if (checkIfAnyAnimationPlaying(animator, 1.0f)) {       
            return;
        }

        //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb.transform.position);
        //Vector3 ballInLocalRb = InverseTransformPointUnscaled(
        //rb.transform, ballRb.transform.position);


        /*VERIFY THAT CONDITION*/
        //if (isCpuPlayer && ballInLocalRb.z < -1.5f)
        //{
        //    return;
        //}

        /*Workaround to checkIfAnyAnimation Playing */
        //if (isCpuPlayer && (Time.time - gkTimeLastCatch < 0.10f))
        //{
            //print("GKMOVESEXEC3 REMOVED ANIMATION DELTA " + (Time.time - gkTimeLastCatch));
            //return;
        //}

        //print("GKMOVESEXEC3");

        //print("ANGLEKACZY EXECUTED GKMOVES EXECUTED 2");

        float angle = Mathf.Infinity;

        //print("DISTDRAWHERE " + dist);

        /* Sign is not important from this place on */
        Vector3 localSpace = Vector3.zero;
        float distX = 0.0f;
        float distY = 0.0f;
        string animName = "";
        float maxTimingCorrection = 0.230f;
        Vector3 hitPointWorld = Vector3.zero;

        //if (rb.transform.eulerAngles.y > 60.0f && 
        //     rb.transform.eulerAngles.y < 240.0f && 

        if (!isCpuPlayer &&
            gkTouchPosRotatedRbWS == INCORRECT_VECTOR)
        {
            gkLock = false;
            //print("DEBUGLASTTOUCHLAKI LASTOUCH INCORRECT NO HIT");
            return;
        }

        Vector3 realHitPlaceLocal = Vector3.zero;

        if (!isCpuPlayer)
        {
            bool userHitCorrectPoint = false;
            Vector3 clickedRbRotatedLS = InverseTransformPointUnscaled(
                rotatedRbToBall.transform,
                gkTouchPosRotatedRbWS);

            Vector3 clickedRbLS = InverseTransformPointUnscaled(
                rb.transform,
                gkTouchPosRbWS);

            /*is that correct*/
            clickedRbRotatedLS.z = clickedRbLS.z = 0.0f;

            //print("DEBUGLASTTOUCH IN SPACE GK MOVES " + gkTouchPosRotatedRbWS);

            if (shotvariant == SHOTVARIANT.CURVE)
            {
                realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                               1.0f,
                                                               rotatedRbToBall,
                                                               outShotStart,
                                                               outShotMid,
                                                               outShotEnd,
                                                               true);

                timeToHitZ = ((realHitPlaceLocal.z * timeofBallFly) - passedShotFlyTime) / 1000.0f;
                /*print("GKRBPOS realHitPlaceLocal "
                    + realHitPlaceLocal
                    + " timeToHitZ "
                    + timeToHitZ
                    + " rotatedRbToBall " + rotatedRbToBall.transform.position
                    + " rb.transform " + rb.transform.position
                    + " gkTouchPosRbWS " + gkTouchPosRbWS
                    + " gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS);*/
                /*now don't overwritte z */
                realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                               1.0f,
                                                               rotatedRbToBall,
                                                               outShotStart,
                                                               outShotMid,
                                                               outShotEnd,
                                                               false);
                if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
                {
                    realHitPlaceLocal = INCORRECT_VECTOR;
                    timeToHitZ = 0f;
                }

                //print("DEBUGLASTTOUCH CURVED HIT POINT " + TransformPointUnscaled(rotatedRbToBall.transform, 
                //                                                                  realHitPlaceLocal)); 

                gkDistRealClicked = Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal);
                if (gkDistRealClicked <= MIN_DIST_REAL_CLICKED &&
                    realHitPlaceLocal != INCORRECT_VECTOR)
                {
                    localSpace = realHitPlaceLocal;
                    //userHitCorrectPoint = true;
                }
                else
                {
                    //localSpace = clickedRbRotatedLS;
                    localSpace = clickedRbRotatedLS;
                    //if (gkTouchPosRbWS == INCORRECT_VECTOR)
                    //    localSpace = clickedRbRotatedLS;
                    if (realHitPlaceLocal != INCORRECT_VECTOR)
                    {
                        Vector3 tmpLocalSpace = localSpace;
                        tmpLocalSpace += (realHitPlaceLocal - clickedRbRotatedLS).normalized;
                        if (compareSign(tmpLocalSpace.x, localSpace.x))
                            localSpace = tmpLocalSpace;
                    }
                }

                localSpace.z = 0;

                /*used by lob */
                //if (userHitCorrectPoint)
                // hitPointWorld = TransformPointUnscaled(rotatedRbToBall.transform, localSpace);

                /*print("DEBUGLASTTOUCHLAKI DISTANCE GK CURVED " + 
                    Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal) + " LOCALSPACECLICKED "
                   + clickedRbRotatedLS + " REALHITPLACE " + realHitPlaceLocal
                   + " LOCALSPACE " + localSpace + " gkTouchPosRbWS " + gkTouchPosRbWS
                    + " gkTouchPosRotated " + gkTouchPosRotatedRbWS);  */
            }
            else
            {
                Vector3 cpuPlayerForwardVector = rotatedRbToBall.transform.forward;
                Plane cpuPlayerXLocalPlane = new Plane(
                   cpuPlayerForwardVector,
                   rotatedRbToBall.transform.position + (rotatedRbToBall.transform.forward * 0.6f));

                Vector3 ballAway = new Vector3(ballRb.transform.position.x + (ballRb.velocity.x * 10.0f),
                                               ballRb.transform.position.y + (ballRb.velocity.y * 10.0f),
                                               ballRb.transform.position.z + (ballRb.velocity.z * 10.0f));

                float distHit = float.MaxValue;
                Ray rayBall = new Ray(
                   ballRb.transform.position,
                   (ballAway - ballRb.transform.position).normalized);

                realHitPlaceLocal = INCORRECT_VECTOR;
                if (cpuPlayerXLocalPlane.Raycast(rayBall, out distHit))
                {
                    hitPointWorld = rayBall.GetPoint(distHit);
                    realHitPlaceLocal = InverseTransformPointUnscaled(rotatedRbToBall.transform, hitPointWorld);
                }

                /*print("GKRBPOS realHitPlaceLocal NONCURVED hitPointWorld "
                    + hitPointWorld
                    + " realHitPlaceLocal "
                    + realHitPlaceLocal
                    + " rotatedRbToBall " + rotatedRbToBall.transform.position
                    + " rb.transform " + rb.transform.position
                    + " gkTouchPosRbWS " + gkTouchPosRbWS
                    + " gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
                + (rotatedRbToBall.transform.position + (rotatedRbToBall.transform.forward * 0.6f)));*/

                /*This is used to adjust real ball hit position and what player clicked */
                gkDistRealClicked = Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal);

                //print("DEBUGLASTTOUCH NOT CURVE HIT POINT " + hitPointWorld);

                if (gkDistRealClicked <= MIN_DIST_REAL_CLICKED &&
                    realHitPlaceLocal != INCORRECT_VECTOR)
                {
                    localSpace = realHitPlaceLocal;
                    userHitCorrectPoint = true;
                }
                else
                {
                    /*localSpace = clickedRbRotatedLS;
                    if (realHitPlaceLocal != INCORRECT_VECTOR)
                        localSpace += (realHitPlaceLocal - clickedRbRotatedLS).normalized;*/
                    localSpace = clickedRbRotatedLS;
                    if (realHitPlaceLocal != INCORRECT_VECTOR)
                    {
                        Vector3 tmpLocalSpace = localSpace;
                        tmpLocalSpace += (realHitPlaceLocal - clickedRbRotatedLS).normalized;
                        if (compareSign(tmpLocalSpace.x, localSpace.x))
                            localSpace = tmpLocalSpace;
                    }
                    //if (gkTouchPosRbWS == INCORRECT_VECTOR)
                    //    localSpace = clickedRbRotatedLS;
                }

                /*print("DEBUGLASTTOUCHLAKI DISTANCE GK NOT CURVED " + Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal) + " LOCALSPACECLICKED " 
                    + clickedRbRotatedLS + " REALHITPLACE " + realHitPlaceLocal 
                    + " LOCALSPACE " + localSpace + " gkTouchPosRbWS " + gkTouchPosRbWS 
                    + " gkTouchPosRotated " + gkTouchPosRotatedRbWS);*/
                //correctLocalOffsetMax(ref localSpace);

                //print("DEBUGPOSGK1 REAL BALL HIT" + localSpace);

                if (hitPointWorld != Vector3.zero)
                {
                    timeToHitX = Mathf.Abs(hitPointWorld.x - ballRb.transform.position.x) / Mathf.Abs(ballRb.velocity.x);
                    timeToHitY = Mathf.Abs(hitPointWorld.y - ballRb.transform.position.y) / Mathf.Abs(ballRb.velocity.y);
                    timeToHitZ = Mathf.Abs(hitPointWorld.z - ballRb.transform.position.z) / Mathf.Abs(ballRb.velocity.z);
                } else
                {
                    timeToHitX = timeToHitY = timeToHitZ = 0f;
                }

                localSpace.z = 0;
                //correctLocalOffsetMax(ref localSpace, shotvariant, isCpuPlayer);
            }


            //if (!gkLock)
            //    ballBeforeLookPos = cpuPlayer.getBallInit();

            gkLock = true;
            if (RblookAtDirection(rb,
                                  rotatedRbToBall,
                                  //ballBeforeLookPos,
                                  2.0f,
                                  20.0f))
            {
                return;
            }

            gkLock = false;

            /*TODELETE*/
            distX = Mathf.Abs(localSpace.x);
            //print("DEBUGSTEPXYZsiDE " + timeToHitZ);

            if (timeToHitZ > MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
            {
                //print("TIMETOTHITSTEP " + timeToHitZ);
                if (distX < 0.25f) return;

                //print("DEBUGSTEPXYZsiDE STEPSIDE ");

                if (localSpace.x < 0.0f)
                {
                    stepSide(animator,
                             rb,
                             Mathf.Max(-2.4f, localSpace.x),
                             "3D_GK_step_left_no_offset_nocpu",
                             ref stepSideAnimOffset);
                    //print("GKDEBUG7 STEP SIDETEST LEFT RES " + localSpace.x);
                }
                else
                {
                    stepSide(animator,
                             rb,
                             Mathf.Min(2.4f, localSpace.x),
                             "3D_GK_step_right_no_offset_nocpu",
                             ref stepSideAnimOffset);
                    //print("GKDEBUG7 STEP SIDETEST RIGHT RES " + localSpace.x);
                }

                //if (timeToHitZ > 1.0f)
                return;
            }
        }       

        /*if (isCpuPlayer)
            print("DEBUG2345ANIMPLAY CORRECT BEFORE " + localSpace + " DISTX " + distX + " localSpace " + localSpace);*/
        correctLocalOffsetMax(ref localSpace, shotvariant, isCpuPlayer);


        //if (!isCpuPlayer)
        //    print("GKRBPOS " + rb.transform.position + "DISTX " + localSpace);

        //print("DEBUGLASTTOUCHLAKI CORRECT AFTER " + localSpace);
        distX = Mathf.Abs(localSpace.x);
        distY = Mathf.Abs(localSpace.y);

        if (localSpace.x < -0.45f) {
            animName = "left";
        } else
        {
            if (localSpace.x > 0.45f)
                animName = "right";
            else
                animName = "straight";
        }

        //if (isCpuPlayer)
        //    print("DEBUG2345ANIMPLAY animName BEFORE " +
        //        animName + " localSpace " + localSpace + " distX " + distX + " distY " + distY);

        //float randDistX = getRandFloat(0.0f, 1.0f);
        //print("RANDDISTX " + randDistX);

        if (animName.Contains("straight")) {
            //print("DEBUG2345ANIMPLAY entered straight " + animName + " contains " + animName.Contains("straight"));

            if (distY < 0.80f)
            {
                animName += "_down";
            }
            else
            {
                if (distY < 2.9f)
                {
                    if (distX < 0.25f &&
                        distY > 1.3f && 
                        distY < 2.7f &&
                        isLobActive &&
                        rb.transform.eulerAngles.y >= 140f &&
                        rb.transform.eulerAngles.y <= 220f)
                    {
                        animName += "_chest_mid";
                        //print("DEBUG2345ANIMPLAY CHEST MID ENERED!!");
                    }
                    else
                        animName += "_mid";
                }
                else
                {
                    animName += "_up";
                }
            }
        } else {
            //if (isCpuPlayer)
            //    print("DEBUG2345ANIMPLAY entered NONSTRAIGHT " + animName + " contains " + animName.Contains("straight"));


            if (distY < 0.8f)
            {
                if (distX <= 4.0f)
                    animName += "_down";
                else
                    animName += "punch_down";
            }
            else
            {
                if (distY < 1.6f)
                {
                    if (distX <= 4.5f)
                        animName += "_mid";
                    else
                        animName += "punch_mid";
                }
                else
                {
                    if (distX >= 2.0f && distX <= 4.0f)
                        animName += "_up";
                    else
                        animName += "punch_up";
                }
            }
        }

        //if (isCpuPlayer)
        //    print("DEBUG2345ANIMPLAY animName AFTER " +
        //        animName + " localSpace " + localSpace + " distX " + distX + " distY " + distY);

        //if (isCpuPlayer)
        //{
        if (isCpuPlayer &&
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset"))
        {
            animator.speed = 0.0f;
        }

        if (initCpuAdjustAnimSpeed && isCpuPlayer)
        {
            animName = initAnimName;
        }

        if (!initCpuAdjustAnimSpeed)
        {
            if (isCpuPlayer) {
                cpuAnimAdjustSpeed = calcGkAnimSpeedBaseOnVelocity(ballRb,
                                                           shotvariant,
                                                           ShotSpeedMin,
                                                           ShotSpeedMax,
                                                           timeofBallFly,
                                                           isCpuPlayer,
                                                           animName,
                                                           localSpace);
            } else {
                /* when player clicked to late, adjust 
                 * cpuAnimAdjustSpeed until MAX_ANIM_GK_PLAYER_SPEED*/

                cpuAnimAdjustSpeed = calcGkAnimSpeedBaseOnTimeToHit(ref animName,
                                           timeToHitZ,
                                           ballRb,
                                           ref initGkDeleyLevel,
                                           ref levelDelay,
                                           ref calculatedTimeToStartPos,
                                           localSpace,
                                           isCpuPlayer,
                                           0.2f,
                                           MIN_ANIM_GK_PLAYER_SPEED,
                                           MAX_ANIM_GK_PLAYER_SPEED,
                                           shotvariant);
                //print("PLAYER ANIMSPEED " + cpuAnimAdjustSpeed);
            }

            initAnimName = animName;
            initCpuAdjustAnimSpeed = true;
            //print("GKDEBUG800 CPUADMINADJUST " + cpuAnimAdjustSpeed + " isCpuPlayer " + isCpuPlayer);
            //if (!isCpuPlayer)
            //    print("TESTPOINT121 CPU ANIM SPEED CALCULATED " + cpuAnimAdjustSpeed  + " VEL " + ballRb.velocity.z);
        }

        if (isCpuPlayer)
        {
            if (calculateTimeToGkAction(ref animName,
                                        timeToHitZ,
                                        ballRb,
                                        cpuAnimAdjustSpeed,
                                        ref initGkDeleyLevel,
                                        ref levelDelay,
                                        ref calculatedTimeToStartPos,
                                        localSpace,
                                        shotvariant,
                                        isCpuPlayer))
                return;
        }
        else
        {
            //print("CALCULATED TIME NOCPU BEFORE " + calculatedTimeToStartPos + " REALTIMETOHIT " + timeToHitZ);

            if (calculateTimeToGkAction(ref animName,
                                        timeToHitZ,
                                        ballRb,
                                        cpuAnimAdjustSpeed,
                                        ref initGkDeleyLevel,
                                        ref levelDelay,
                                        ref calculatedTimeToStartPos,
                                        localSpace,
                                        shotvariant,
                                        isCpuPlayer) &&
                (Mathf.Abs(calculatedTimeToStartPos - timeToHitZ) < maxTimingCorrection) &&
                calculatedTimeToStartPos < timeToHitZ)
            {
                //print("CALCULATED TIME NOCPU " + calculatedTimeToStartPos + " REALTIMETOHIT "
                //     + timeToHitZ + " LOCAL " + localSpace + " cpuAnimAdjustSpeed " + cpuAnimAdjustSpeed);
                return;
            }

            //print("CALCULATED TIME NOCPU " + calculatedTimeToStartPos + " REALTIMETOHIT "
            //        + timeToHitZ + " LOCAL " + localSpace + " cpuAnimAdjustSpeed " + cpuAnimAdjustSpeed);
            //print("CALCULATED TIME NOCPU EXIT " + calculatedTimeToStartPos + " REALTIMETOHIT " + timeToHitZ);
        }

        if (!isCpuPlayer)
        {
            if (Mathf.Abs(calculatedTimeToStartPos - timeToHitZ) > maxTimingCorrection)
                gkAction = "TOOEARLY";
            else if ((timeToHitZ - calculatedTimeToStartPos) < 0.0f)
            {
                gkAction = "TOOLATE";
            }
            else
            {
                gkAction = "";
            }

            /*CHANGED SCALE*/
            //Vector3 ballInLocalToRb = rb.transform.InverseTransformPoint(ballRb.transform.position);
            Vector3 ballInLocalToRb = InverseTransformPointUnscaled(rb.transform, ballRb.transform.position);
            if (ballInLocalToRb.z < 0.0f)
            {
                gkAction = "TOOLATE";
            }
        }

        //if (!isCpuPlayer)
        //    print("CALCULATED TIME NOCPU EXIT AFTER  " + calculatedTimeToStartPos + " REALTIMETOHIT " + timeToHitZ +  " GKACTION " + gkAction);

        //print("ANIMNAME BEFORE " + animName);      
        //print("CURRENTPOSITION GKMOVESEXEC ANIMNAME " + animName + " LOCAL SPACE " + localSpace);

        /*???*/
        /*Overwrite to default value */
        calculatedTimeToStartPos = 0.227f;
        float offset;
        //if (isCpuPlayer)

        //if (isCpuPlayer || !isCpuPlayer)
        //{
        offset = 1.15f;
        if (animName.Contains("up"))
        {
            offset = 1.80f;
            if (animName.Contains("punch"))
                offset = 2.30f;
        } else if (animName.Contains("mid")) {
            offset = 1.40f;
            if (animName.Contains("punch"))
                offset = 2.30f;
        } else if (animName.Contains("down"))
        {
            offset = 1.30f;
            if (animName.Contains("punch"))
                offset = 2.30f;
        }
        /*} else {            
             offset = 1.15f;
             if (animName.Contains("up"))
             {
                 offset = 1.65f;
                 if (animName.Contains("punch"))
                     offset = 2.3f;
             }
             else if (animName.Contains("mid"))
             {
                 offset = 1.15f;
                 if (animName.Contains("punch"))
                     offset = 2.30f;
             }
             else if (animName.Contains("down"))
             {
                 offset = 0.70f;
                 if (animName.Contains("punch"))
                     offset = 2.3f;
             }
         }*/

        //if (isCpuPlayer)
        //    print("DEBUG123XYGKAA LOCALSPACE " + localSpace);

        distXcord = localSpace.x;

        //print("DISTXCORD " + distXcord);
        //animator.Play("3D_GK_sidecatch_" + animName, 0, 0.0f);
        if (animName.Contains("down"))
        {
            if (localSpace.x >= 0.0f)
            {
                localSpace.x += -offset;
            } else
            {
                localSpace.x += offset;
            }
        }

        if (animName.Contains("mid"))
        {
            if (localSpace.x >= 0.0f)
            {
                localSpace.x += -offset;
            }
            else
            {
                localSpace.x += offset;
            }
        }

        if (animName.Contains("up"))
        {
            if (localSpace.x >= 0.0f)
            {
                localSpace.x += -offset;
            }
            else
            {
                localSpace.x += offset;
            }
        }

        //print("ANIMNAMETEST1 " + animName + " DISTX " + distX);
        gkTimeToCorrectPos = calculateMatchParentGkTimePos(animName);

        /*sidecatch* animations interrupts step_right/step_left */

        //if (isCpuPlayer)
        //{
        interruptSideAnimation(animator, rb);
        //}

        animator.speed = cpuAnimAdjustSpeed;
        //print("DEBUG2345ANIMPLAY PLAY IN GKMOVES NAME: 3D_GK_sidecatch_" + animName);


        animator.Play("3D_GK_sidecatch_" + animName, 0, 0.0f);

        //if (isCpuPlayer)
        //{
        //    print("GKDEBUG800 LASTANIM " + lastAnimName);
        //}

        /*if (!isCpuPlayer)
        {
            animator.speed = 3.5f;
            if (animName.Contains("punch"))
            {
                animator.speed = 4.0f;
            }
        }*/
        //else
        //{
        //}

        animator.Update(0f);

        //gkStartPos = rb.transform.position;

        Vector3 localGkStartPos = InverseTransformPointUnscaled(rb.transform, rb.transform.position);
        localGkStartPos.x += localSpace.x;

        gkStartPos = TransformPointUnscaled(rb.transform, localGkStartPos);
        //print("GLOBAL LOCAL " + rb.transform.position + " LOCAL " + localGkStartPos + " LOCALSPACE OFF " + localSpace.x + 
        //    " GSTARTPOS " + gkStartPos + " DISTREAL " + distXcord + " ANIMNAME " + animName + " OFFSET " + offset);

        lastAnimName = animName;
        lastTimeGkAnimPlayed = Time.time;
        gkStartTransform = rb.transform;

        //print("SAVE GK START POS " + gkStartPos);
        //print("GKMOVESEXEC3 TIMETOHITX GKMOVESEXEC ANIMNAME " + animName + " LOCAL SPACE " + localSpace);

        if (!isCpuPlayer)
        {
            touchLocked = true;
            touchCount = 0;
            animationPlaying = true;
            gkTouchDone = false;
        }

        gkTimeLastCatch = Time.time;
        gkLobPointReached = false;
        gkRunPosReached = false;
        initDistX = -1;

        return;
    }

    private bool curveShotHitPoint(GameObject rotatedRb, 
                                   Vector3 curveStart,
                                   Vector3 curveMid,
                                   Vector3 curveEnd,
                                   Vector3 endPosOrg,
                                   float timeofBallFly,
                                   float passedShotFlyTime,
                                   ref float timeToHitZ,
                                   ref Vector3 hitPointWorld,
                                   ref Vector3 localSpace)                      
    {

        Vector3 realHitPlaceLocal = Vector3.zero;
        bool ret = true;
     
        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rotatedRb,
                                                       curveStart,
                                                       curveMid,
                                                       curveEnd,
                                                       true);

        /* realHitPlaceLocal.z = time. 
        * it keeps time after rotatedRbToBall and curve intersect */
        timeToHitZ = ((realHitPlaceLocal.z* timeofBallFly) - passedShotFlyTime) / 1000f;

        /*now don't overwritte z */
        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rotatedRb,
                                                       curveStart,
                                                       curveMid,
                                                       curveEnd,
                                                       false);

        /*no intersection beetween bezier curve and shot*/
        /*print("GKDEBUG800ERROR1?? (Mathf.Abs(realHitPlaceLocal.z) "
                    + Mathf.Abs(realHitPlaceLocal.z) +
                    " timeofBallFly " + timeofBallFly + " passedShotFlyTime " + passedShotFlyTime);*/

         /*Rb and curve don't intersect. Rb might be rotated such way that intersection points 
         * is out of curve end.
         * This should happen only in a case of lob shot*/
        if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
        {
             //print("GKDEBUG800ERROR1 NO INTERSECTION FIRS TIME "
             //           + realHitPlaceLocal + " ISLOB + " + isLobActive);
            /*z will be overwritten after all that point might be behind*/
            realHitPlaceLocal =
                        InverseTransformPointUnscaled(rotatedRb.transform, endPosOrg);
            timeToHitZ = (timeofBallFly - passedShotFlyTime) / 1000f;

            //print("GKDEBUG800ERROR1 NO INTERSECTION FIRS TIME " + timeToHitZ);

            //realHitPlaceLocal = INCORRECT_VECTOR;
            //timeToHitZ = 0f;
            ret = false;
        }

        localSpace = realHitPlaceLocal;
        hitPointWorld = TransformPointUnscaled(rotatedRb.transform, localSpace);
        localSpace.z = 0;

        return ret;
    }

    private bool straightShotHitPoint(GameObject rotatedRb,
                                      ref Vector3 hitPointLocal,
                                      ref Vector3 hitPointWorld,
                                      ref float timeToHitZ)                                      
    {
        bool ret = false;
        Vector3 realHitPlaceLocal = Vector3.zero;

        Plane localPlaneRot = new Plane(
           rotatedRb.transform.forward,
           rotatedRb.transform.position + (rotatedRb.transform.forward * 0.6f));

        Vector3 ballAway = new Vector3(ballRb.transform.position.x + (ballRb.velocity.x * 10.0f),
                                       ballRb.transform.position.y + (ballRb.velocity.y * 10.0f),
                                       ballRb.transform.position.z + (ballRb.velocity.z * 10.0f));

        float distHit = float.MaxValue;
        Ray rayBall = new Ray(
           ballRb.transform.position,
           (ballAway - ballRb.transform.position).normalized);

        realHitPlaceLocal = INCORRECT_VECTOR;
        if (localPlaneRot.Raycast(rayBall, out distHit))
        {
            hitPointWorld = rayBall.GetPoint(distHit);
            realHitPlaceLocal = InverseTransformPointUnscaled(
                                rotatedRb.transform, hitPointWorld);
            ret = true;
        }

        hitPointLocal = realHitPlaceLocal;
        if (hitPointWorld != Vector3.zero)
        {
            timeToHitZ = Mathf.Abs(hitPointWorld.z - ballRb.transform.position.z) / Mathf.Abs(ballRb.velocity.z);
        }
        else
        {
            timeToHitZ = 0f;
        }
        hitPointLocal.z = 0;

        return ret;
    }

    private void straightShotHitPoint(GameObject rotatedRb,
                                      Vector3 ballPos,
                                      Vector3 endGoalPos,
                                      ref Vector3 hitPointLocal,
                                      ref Vector3 hitPointWorld,
                                      ref float timeToHitZ)
    {
        Vector3 realHitPlaceLocal = Vector3.zero;

        Plane localPlaneRot = new Plane(
           rotatedRb.transform.forward,
           rotatedRb.transform.position + (rotatedRb.transform.forward * 0.6f));

        float distHit = float.MaxValue;
        Ray rayBall = new Ray(
           ballPos,
           (endGoalPos - ballPos).normalized);

        realHitPlaceLocal = INCORRECT_VECTOR;
        if (localPlaneRot.Raycast(rayBall, out distHit))
        {
            hitPointWorld = rayBall.GetPoint(distHit);
            realHitPlaceLocal = InverseTransformPointUnscaled(
                                rotatedRb.transform, hitPointWorld);
        }

        hitPointLocal = realHitPlaceLocal;
        if (hitPointWorld != Vector3.zero)
        {
            timeToHitZ = Mathf.Abs(hitPointWorld.z - ballPos.z) / Mathf.Abs(ballPos.z);
        }
        else
        {
            timeToHitZ = 0f;
        }

        hitPointLocal.z = 0;
    }

    /*this function tries to find a correct position too catch a lob shot*/
    private bool gkSaveLobShot(Rigidbody rb,
                               GameObject rotatedRbToBall,
                               Animator animator,
                               Vector3 ballHitPoint,
                               Vector3 shotEndPos,
                               ref bool gkLobPointReached,
                               float distX,
                               float initDistX,
                               float timeToHitZ,
                               bool isGkTooFarToCatch,
                               float speed,
                               float backRunSpeed)                              
    {
        bool isGoFotwardToBallActive =
         (isGkTooFarToCatch || (initDistX > 8f && distX > 4f && timeToHitZ > 0.35f));

        /*print("DEBUG2345ANIMPLAY isGoFotwardToBallActive "
           + isGoFotwardToBallActive + " gkLobPointReached " + gkLobPointReached
           + " timeToHitZ " + timeToHitZ + " angle " + " rb rotation " + rb.transform.eulerAngles 
           +  " rotatedBall " + rotatedRbToBall.transform.eulerAngles 
           + " "
           + getAngleBeetweenObjects(rb, rotatedRbToBall));*/


        if (!isGoFotwardToBallActive) {
            if (timeToHitZ <= MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
            {
                //if (!gkLobPointReached)
                //    rb.velocity = Vector3.zero;
                //gkLobPointReached = true;
                return false;
            }

            if (timeToHitZ < 0.350f ||
                gkLobPointReached)
            {           
                bool isRotated = RblookAtDirection(rb,
                                              rotatedRbToBall,
                                              2.0f,
                                              20.0f);
                
                gkLobPointReached = true;
                rb.velocity = Vector3.zero;

                if (!isRotated)
                    return false;
                return true;
            }           
        }
     
        /*print("DEBUG2345ANIMPLAY isGoFotwardToBallActive " 
            + isGoFotwardToBallActive + " gkLobPointReached " + gkLobPointReached
            + " timeToHitZ " + timeToHitZ + " angle " 
            + getAngleBeetweenObjects(rb, rotatedRbToBall));*/
      
        /*if (!isGoFotwardToBallActive && 
            timeToHitZ < 0.35f)
        {
            gkLobPointReached = true;
            rb.velocity = Vector3.zero;
            return false;            
        }

        if (!isGoFotwardToBallActive)
            timeToHitZ <= MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
        {
            gkLobPointReached = true;
            rb.velocity = Vector3.zero;
            return false;
        }*/

        //if (!isGoFotwardToBallActive &&
            //(gkLobPointReached ||
        //     timeToHitZ < 0.35f)  
       // {
            
           /* gkLobPointReached = true;
            return true;
            
            bool isRotationNeeded = RblookAtDirection(rb,
                                                      rotatedRbToBall,
                                                      2.0f,
                                                      20.0f);
            if (!isRotationNeeded &&
                gkLobPointReached)
            {
                return false;
            }

            if (!gkLobPointReached &&
                timeToHitZ < MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE &&
                !isRotationNeeded)
            {
                gkLobPointReached = true;
                rb.velocity = Vector3.zero;
                return false;
            }
                
            return true;
        }*/

        Vector3 lookAtBall =
              new Vector3(ballRb.transform.position.x - rb.transform.position.x,
                          0.0f,
                          ballRb.transform.position.z - rb.transform.position.z);

        Vector3 lobPointToGo = moveOutFromWall(
                               ballRb.transform.position, shotEndPos, 1.2f);

        float dist = Vector3.Distance(rb.transform.position,
                                      lobPointToGo);


      
        /*print("DEBUG2345ANIMPLAY INSIDE LOB  " + lobPointToGo + " dist " + dist
        + " HITPOINT" + ballHitPoint
        + " gkLobPointReached " + gkLobPointReached + " distX " + distX
        + " ballRb.transform.position " + ballRb.transform.position
        + " shotEndPos " + shotEndPos);*/
        
       
        if (dist <= 0.1f ||
           (ballHitPoint.y <= MAX_GK_HAND_REACH_CPU &&
            distX <= MIN_GK_STRAIGHT_DIST_CPU))
        {
            //print("DEBUG2345ANIMPLAY INSIDE LOB 2 " + lobPointToGo + " dist " + dist);

            gkLobPointReached = true;
            rb.velocity = Vector3.zero;
            return false;
        }

        //print("DEBUG2345ANIMPLAY INSIDE LOB 3 " + lobPointToGo + " dist " + dist);

        if (isGoFotwardToBallActive)
        {
            goAndRotateTowardRun(rb,
                                animator,
                                //"3D_run_cpu",
                                "3D_run",
                                lobPointToGo,
                                speed,
                                7f);
            return true;
        }

        
        RblookAtDirection(rb,
                         lookAtBall,
                         10.0f);

        if (Mathf.Abs(rb.transform.position.z) >= 10.0f)
            backRunSpeed = getBackSpeed(Globals.level, speed);

        goBackwardToPoint(rb,
                          animator,
                          lobPointToGo,
                          Mathf.Max(SPEED_RUN_MIN, backRunSpeed));

        return true;
    }

    private bool isAnimatorPlaying(Animator animator)
    {
        bool isGkShotAnimationPlaying = 
            checkIfAnyAnimationPlaying(animator, 1.0f);
        bool isRunTurnAnimationPlaying = 
            checkIfAnyAnimationPlayingContain(
                            RunAnimationsNames, animator, 1f, "3D_run_");
        return isGkShotAnimationPlaying || isRunTurnAnimationPlaying;
    }

    private void goAndRotateTowardRun(Rigidbody rb, 
                                      Animator animator,
                                      string animName,
                                      Vector3 goPoint, 
                                      float runSpeed,
                                      float rotationSpeed)
    {
        Vector3 goTowardPos =
                    new Vector3(goPoint.x - rb.transform.position.x,
                                0.0f,
                                goPoint.z - rb.transform.position.z);

        /*print("DEBUGTOOFAR FROM BALL HITPOINT " + goPoint
            + " TIMETOTHITZ lobPointToGo "
            + runSpeed);*/

        interruptSideAnimation(animator, rb);
        goToPos(animator,
                animName,
                rb,
                runSpeed,
                goTowardPos);

        RblookAtDirection(rb,
                          goTowardPos.normalized,
                          rotationSpeed);
    }


    /*new function*/
    private void getGkShotIntersection(SHOTVARIANT shotvariant,
                                       GameObject rotatedRbToBall,
                                       Vector3 outShotStart,
                                       Vector3 outShotMid,
                                       Vector3 outShotEnd,
                                       ref float timeToHitZ,
                                       ref Vector3 realHitPlaceLocal)
    {
        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rotatedRbToBall,
                                                       outShotStart,
                                                       outShotMid,
                                                       outShotEnd,
                                                       true);

        timeToHitZ = 
            ((realHitPlaceLocal.z * timeofBallFly) - passedShotFlyTime) / 1000.0f;

        /*now don't overwritte z */
        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rotatedRbToBall,
                                                       outShotStart,
                                                       outShotMid,
                                                       outShotEnd,
                                                       false);
        if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
        {
            realHitPlaceLocal = INCORRECT_VECTOR;
            timeToHitZ = 0f;
        }

        //print("DEBUGLASTTOUCH CURVED HIT POINT " + TransformPointUnscaled(rotatedRbToBall.transform, 
        //                                                                  realHitPlaceLocal)); 
     
        //localSpace.z = 0;
    }

    private Vector3 moveOutFromWall(Vector3 lookAt, 
                                    Vector3 wallPoint, 
                                    float moveDist)
    {
        Vector3 lookAtV2 = 
            new Vector3(lookAt.x, 0f, lookAt.z);
        Vector3 pointOnWallV2 = 
            new Vector3(wallPoint.x, 0f, wallPoint.z);
        Vector3 pointAway = 
            Vector3.zero;

        pointAway = pointOnWallV2 +
                 (lookAtV2 - pointOnWallV2).normalized * moveDist;
        pointAway.y = 0f;

        return pointAway;
    }

    private void initCameraMatchStartPos()
    {
        cameraChanged(true);
    }

    public bool setShotSaveStatistics(string teamName)
    {        
        return matchStatistics.setSaves(teamName);
    }

    public void decSavesStatistics(string teamName, int val)
    {
        matchStatistics.decSaves(teamName, val);
    }

    private void interruptSideAnimation(Animator animator, Rigidbody rb)
    {
        /*print("GKDEBUG7 PREV INTERRUPT" + rb.transform.position + 
            " animator.isMatchingTarge " + animator.isMatchingTarget + 
            " ISPLAYING OFFSET " + checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset"));*/

        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset") &&
            animator.isMatchingTarget)
        {
            animator.InterruptMatchTarget(false);
            //print("GKDEBUG7 MATCHING TARGET INTERRUPT");
        }
        //print("GKDEBUG7 DEBUG25 AFTER " + rb.transform.position);
    }

    public SHOTVARIANT getShotVariant()
    {
        return shotvariant;
    }

    public float getGkLastDistXCord()
    {
        return lastGkDistX;
    }

    private void correctLocalOffsetMax(ref Vector3 localPos, SHOTVARIANT shotvariant, bool isCpu)
    {
        /*LEVEL DEPENDENT*/
        float maxOffset = MAX_GK_OFFSET;
        if (isCpu)
        {
             maxOffset = MAX_GK_OFFSET_CPU;
            /*TOCHANGE*/
            if (shotvariant == SHOTVARIANT.CURVE &&
                Globals.level >= 4)
            {
                maxOffset = MAX_GK_OFFSET_CPU + 1.0f;
            }
        }

        if (Mathf.Abs(localPos.x) > maxOffset)
        {
            if (localPos.x < 0.0f)
            {
                localPos.x = -maxOffset;
            } else
            {
                localPos.x = maxOffset;
            }
        }  
    }

    private bool isShotPossible(Rigidbody rb)
    {
       if (Mathf.Abs(rb.transform.position.z) > WALLS_MIN_OFFSET &&
          (Mathf.Abs(rb.transform.position.z) < (PITCH_HEIGHT_HALF - WALLS_MIN_OFFSET)) &&
          (Mathf.Abs(rb.transform.position.x) < (PITCH_WIDTH_HALF - WALLS_MIN_OFFSET)))
        {
            return true;
        }

        return false;
    }

    private bool isTurnAnimPossible(Vector3 pos)
    {
        if ((Mathf.Abs(pos.x) >= (PITCH_WIDTH_HALF - 3.25f)) ||
            (Mathf.Abs(pos.z) <= 3.25f) ||
            (Mathf.Abs(pos.z) >= (PITCH_HEIGHT_HALF - 3.25f)))
            return false;
  
       return true;
    }

    private bool gkTooFarToCatch(float distX, Vector3 ballInLocalRb, float minXVal)
    {
        if (distX > minXVal ||
            ballInLocalRb.z < -1.5f)
            return true;
        return false;
    }

    private float calcGkAnimSpeedBaseOnTimeToHit(
                     ref string animName,
                     float timeToHitZ,
                     Rigidbody ball,
                     ref bool initGkDeleyLevel,
                     ref float levelDelay,
                     ref float timeToStartPos,
                     Vector3 localSpace,
                     bool isCpu,
                     float step,
                     float minAnimSpeed,
                     float maxAnimSpeed,
                     SHOTVARIANT shotvariant)
    {
        float cpuAnimAdjustSpeed = minAnimSpeed;
        bool isEnoughTime = false;
        int loops = 0;

        while (cpuAnimAdjustSpeed <= maxAnimSpeed)
        {
            isEnoughTime = calculateTimeToGkAction(ref animName,
                                 timeToHitZ,
                                 ballRb,
                                 cpuAnimAdjustSpeed,
                                 ref initGkDeleyLevel,
                                 ref levelDelay,
                                 ref calculatedTimeToStartPos,
                                 localSpace,
                                 shotvariant,
                                 isCpu);

            /*When it's already too late then calculate*/
            if (!isEnoughTime)
            {
                cpuAnimAdjustSpeed += step;
            }
            else
            {
                break;
            }

            loops++;
            if (loops > 30)
            {
                cpuAnimAdjustSpeed = MAX_ANIM_GK_PLAYER_SPEED;
                break;
            }
        }

        return Mathf.Min(cpuAnimAdjustSpeed, MAX_ANIM_GK_PLAYER_SPEED);
    }


    private float calcGkAnimSpeedBaseOnVelocity(Rigidbody ball, 
                                                SHOTVARIANT shotVariant,
                                                float ShotCurveSpeedMinTime,
                                                float ShotCurveSpeedMaxTime,
                                                float timeofBallFly,
                                                bool isCpuPlayer,
                                                string animName,
                                                Vector3 localHitPoint)
    {
        float baseAnimSpeed;

        //if (!isCpuPlayer)
        //    return 4.0f;

        if (isCpuPlayer)
        {
            /*delay depending on player skills*/
            float levelNorm = getLevelInterpolationReverse();
            //float skillNorm = getSkillInterpolationReverse(Globals.teamBGkStrength);
            float skillNorm = getSkillInterpolationReverse(defenseSkillsCpu);
            float animatorDelay = (levelNorm + skillNorm) / 2.0f;

            baseAnimSpeed = 2.0f;
            if (animName.Contains("punch"))
            {
                baseAnimSpeed = 3.0f;
                animatorDelay = Mathf.Lerp(0.0f, 1.0f, animatorDelay);
            }

            /*if (Globals.level > 3 && localHitPoint.x > 3.0f)
            {
                float mul = 0.3f;
                if (animName.Contains("punch"))
                    mul = 0.5f;
                animatorDelay = ((LEVEL_MAX - Globals.level + 1) * mul);
            }*/
                       
            if (Globals.level < 3)
                baseAnimSpeed -= animatorDelay;  
            
            baseAnimSpeed = Mathf.Max(1.5f, baseAnimSpeed);
            if (Globals.level < 3)
                baseAnimSpeed = Mathf.Min(3.0f, baseAnimSpeed);

            //print("DEBUGZ1237 baseAnimSpeed " + baseAnimSpeed + " animName " + animName);
        }
        else
        {
            baseAnimSpeed = 2.8f;
        }

        if (shotVariant == SHOTVARIANT.CURVE)
        {
            //print("CURVEANIMDBASE " + Mathf.InverseLerp(ShotCurveSpeedMinTime, ShotCurveSpeedMaxTime, timeofBallFly) + " MIN " +
            //    ShotCurveSpeedMinTime + " MAX " + ShotCurveSpeedMaxTime + " TIMEOFBALL " + timeofBallFly);

            return baseAnimSpeed + Mathf.InverseLerp(ShotCurveSpeedMaxTime, ShotCurveSpeedMinTime, timeofBallFly);
        }

        return baseAnimSpeed + Mathf.InverseLerp(0.0f, BALL_MAX_SPEED, Mathf.Abs(ball.velocity.z));
    }

    private float getRunSpeed()
    {
        return runningSpeed;
    }
  
    private void disableShotButtons()
    {
        /*lobButtonGameObject.GetComponent<Button>().interactable
           = false;
        volleyButtonGameObject.GetComponent<Button>().interactable
            = false;
        overheadButtonGameObject.GetComponent<Button>().interactable
            = false;*/
        if (volleyButton.getNonClickedColorAlpha() > 0.95f) {       
            volleyButton.changeNonClickedColorAlpha(0.4f);
            lobButton.changeNonClickedColorAlpha(0.4f);
            //overheadButton.changeNonClickedColorAlpha(0.4f);
        }

        if (volleyButton.getClickedColorAlpha() > 0.95f) {
            volleyButton.changeClickedColorAlpha(0.4f);
            lobButton.changeClickedColorAlpha(0.4f);
        }
    }

    private float dist2Dvector3(Vector3 v1, Vector3 v2)
    {
        Vector2 pos1 = new Vector2(v1.x, v1.z);
        Vector2 pos2 = new Vector2(v2.x, v2.z);

        return Vector2.Distance(pos1, pos2);
    }

    private void enableShotButtons()
    {
        if (volleyButton.getNonClickedColorAlpha() < 0.95f)
        {
            volleyButton.changeNonClickedColorAlpha(1f);
            lobButton.changeNonClickedColorAlpha(1f);
            //overheadButton.changeNonClickedColorAlpha(1f);
        }

        if (volleyButton.getClickedColorAlpha() < 0.95f)
        {
            volleyButton.changeClickedColorAlpha(1f);
            lobButton.changeClickedColorAlpha(1f);
        }

        /*lobButtonGameObject.GetComponent<Button>().interactable 
            = true;
        volleyButtonGameObject.GetComponent<Button>().interactable 
            = true;
        overheadButtonGameObject.GetComponent<Button>().interactable 
            = true;*/
    }

    private float calculateGkActionDelay(Vector3 localHitPoint, 
                                         bool isCpu, 
                                         string animName,
                                         SHOTVARIANT shotvariant)
    {
        float levelNormRev = getLevelInterpolationReverse();
        //float skillsNorm = getSkillInterpolation(Globals.teamAGkStrength);
        //float skillsNormRev = getSkillInterpolationReverse(Globals.teamAGkStrength);
        float skillsNorm = getSkillInterpolation(defenseSkillsPlayer);
        float skillsNormRev = getSkillInterpolationReverse(defenseSkillsPlayer);

        float endDelay = 0.20f;
        if (isCpu)
        {
            //skillsNorm = getSkillInterpolation(Globals.teamBGkStrength);
            //skillsNormRev = getSkillInterpolationReverse(Globals.teamBGkStrength);
            skillsNorm = getSkillInterpolation(defenseSkillsCpu);
            skillsNormRev = getSkillInterpolationReverse(defenseSkillsCpu);
        }

        /*Higher value means that gk will be more difficult to beat */
        float skillsDelay = Mathf.Lerp(0, 0.3f, skillsNorm);
        float skillsProb = Mathf.Lerp(0, 10f, skillsNormRev);


        //if (!animName.Contains("punch") && !animName.Contains("straight"))
        //{
        //    endDelay = 0.25f;
        //}

        float delayTime = Mathf.Lerp(0.08f, endDelay, levelNormRev);

        float minVel = 26.0f;
        float minOffset = 3.0f;

        /*its probabillity (from 0 to 100) of event happen (gk will be delayed) */
        /*for instance 20 means that even will happen 20% of time */
        int prob = 20;
        switch (Globals.level)
        {
            case 1:
                //minVel = 12.0f;
                //minOffset = 1.2f;
                //delayTime = 0.20f;
                minVel = 10.0f;
                minOffset = 0.8f;
                delayTime = 0.20f;
                prob = 100;
                break;
            case 2:
                minVel = 16.0f;
                minOffset = 1.5f;
                prob = 90;
                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    prob = 60;
                    minOffset = 3.2f;
                }
                delayTime = 0.17f;
                break;         
            case 3:
                /*minVel = 19.0f;
                minOffset = 2.1f;
                prob = 70;
                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    prob = 50;
                    minOffset = 4.0f;
                }
                delayTime = 0.12f;*/
                minVel = UnityEngine.Random.Range(16.0f, 19f);
                minOffset = UnityEngine.Random.Range(1.5f, 2.1f);
                prob = UnityEngine.Random.Range(70, 85);

                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    prob = 50;
                    minOffset = UnityEngine.Random.Range(3.6f, 4.0f);
                }

                delayTime = UnityEngine.Random.Range(0.12f, 0.16f);
                break;
            case 4:
                minVel = 22.0f;
                minOffset = 2.8f;
                prob = 60;
                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    prob = 40;
                    minOffset = 4.2f;
                }

                delayTime = 0.10f;
                break;
            case 5:
                minVel = 23.0f;
                minOffset = 3.3f;               
                delayTime = 0.08f;
                prob = 50;
                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    prob = 25;
                    minOffset = 4.45f;
                }

                break;
        }

        int randProb = UnityEngine.Random.Range(1, 101);
        minOffset += skillsDelay;
        minVel += (skillsDelay * 5.0f);
        prob += (int) Mathf.Round(skillsProb);

        if ((Mathf.Abs(localHitPoint.x) < minOffset ||  
             Mathf.Abs(ballRb.velocity.z) < minVel) ||
             prob < randProb)
        {
            delayTime = 0.0f;
            //print("DEBUGZ1237 " + delayTime + " zeros");
        }

        /*print("DEBUG123XYGKA delay time " + delayTime + " localHitPoint " 
            + localHitPoint + " minOffset " + minOffset 
            + " minVel " + minVel + " REALVEL " + Mathf.Abs(ballRb.velocity.z)
            + " randProb " + randProb 
            + " prob " + prob + " skillsProb " + skillsProb + " skillsNormRev " + skillsNormRev 
            + " Globals.teamBGkStrength " + Globals.teamBGkStrength);*/

        return delayTime;
    }

    private void correctTimeToStartPosByDelay(ref float timeToStartPos, float levelDelay)
    {
        //print("GKDEBUG800 TIME TO START DELAY BEFORE " + timeToStartPos);
        timeToStartPos -= levelDelay;
        timeToStartPos = Mathf.Max(0.0f, timeToStartPos);
        //print("GKDEBUG800 TIME TO START DELAY AFTER " + timeToStartPos);
    }

    private bool calculateTimeToGkAction(ref string animName, 
                                         float timeToHitZ, 
                                         Rigidbody ball, 
                                         float animSpeed,
                                         ref bool initGkDeleyLevel,
                                         ref float levelDelay,
                                         ref float timeToStartPos,
                                         Vector3 localHitPoint,
                                         SHOTVARIANT shotvariant,
                                         bool isCpu)                                         
    {
        //float timeToStartPos = 0.227f;

        /*DEFAULT 30 FPS*/
        float animFrameRate = FRAME_RATE;
        float frame;
        //print("ANIMSPEEDCALCULATED " + animSpeed + " BALL VEL Z " + ball.velocity.z);
        if (!isCpu)
            levelDelay = 0.0f;
        else
        {
            if (isCpu && !initGkDeleyLevel)
            {
                levelDelay = calculateGkActionDelay(localHitPoint, 
                                                    isCpu, 
                                                    animName,
                                                    shotvariant);
                initGkDeleyLevel = true;
                //print("GKDEBUG800 LEVELDELAY " + levelDelay);
            }
        }

        if (animName.Contains("down"))
        {
            /*227ms - frame 15 */
            /*FRAME DEPENDEND 0.20f? */
            frame = 15.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 DOWN " + timeToStartPos + " ANIM SPEED " + animSpeed 
                + " timeToHitZ " + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/
            if (!animName.Contains("punch") && (timeToHitZ - timeToStartPos) > 0.040f)
            {
                return true;
            }

            /*FRAME DEPENDEND 0.20f? */
            /*303ms - frame 25 */
            frame = 25.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 DOWN PUNCH " + timeToStartPos + " ANIM SPEED " + animSpeed 
                + " timeToHitZ " + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/

            if (animName.Contains("punch") && ((timeToHitZ - timeToStartPos) > 0.020f))
            {
                return true;
            }
        }

        if (animName.Contains("mid"))
        {
            /*212 ms Frame 13 */
            frame = 13.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 MID " + timeToStartPos + " ANIM SPEED " + animSpeed + " timeToHitZ " 
                + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/

            if (!animName.Contains("punch") && ((timeToHitZ - timeToStartPos) > 0.040f))
            {
                return true;
            }

            /*303ms - frame 20 */
            frame = 19.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 MID PUNCH " + timeToStartPos + " ANIM SPEED " 
                + animSpeed + " timeToHitZ " + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/

            if (animName.Contains("punch") && ((timeToHitZ - timeToStartPos) > 0.020f))
            {
                return true;
            }
        }

        if (animName.Contains("up"))
        {
            /* 213ms - frame 14 */
            frame = 14.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 UP " + timeToStartPos + " ANIM SPEED " + animSpeed 
                + " timeToHitZ " + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/

            if (!animName.Contains("punch") && ((timeToHitZ - timeToStartPos) > 0.040f))
            {
                return true;
            }

            /*frame 25 */
            frame = 25.0f;
            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            /*print("GKDEBUG800 UP PUNCH " + timeToStartPos + " ANIM SPEED " + animSpeed + " timeToHitZ " 
                + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);*/

            if (animName.Contains("punch") && ((timeToHitZ - timeToStartPos) > 0.020f))
            {
                return true;
            }
        }

        if (animName.Contains("straight"))
        {
            frame = 14.0f;
            /* 213ms - frame 14 */
            if (animName.Contains("down"))
            {
                frame = 8.0f;
            } else if (animName.Contains("mid"))
            {
                if (animName.Contains("chest"))
                    frame = 8f;
                else
                    frame = 20.0f;
            }
            else if (animName.Contains("up"))
            {
                //frame = 14.0f;
                frame = 25.0f;
            }

            //print("GKDEBUG800 FRAME straight" + frame);

            timeToStartPos = (frame * 1000.0f) / (animSpeed * animFrameRate) / 1000.0f;
            correctTimeToStartPosByDelay(ref timeToStartPos, levelDelay);

            //print("GKDEBUG800 STRAIGHT PUNCH " + timeToStartPos + " ANIM SPEED " + animSpeed 
            //    + " timeToHitZ " + timeToHitZ + " ANIMNAME " + animName + " localHitPoint " + localHitPoint);

            if ((timeToHitZ - timeToStartPos) > 0.020f)
            {
                return true;
            }
        }

        /*This might happen when first animName will be without punch. 
         * Time will pass and offset will be change by _no_offset and then punch will be taken. it will mean that we will not have
         * enought time to execute whole animation */
      
       /*if (isCpu &&
          ((timeToHitZ - timeToStartPos) < 0f) && 
           animName.Contains("punch"))
       {
            print("GKDEBUG800 CORRECTION !!!!!!! time " + (timeToHitZ - timeToStartPos) + " animName " + animName);
            animName = animName.Replace("punch", "");
            print("GKDEBUG800 CORRECTION !!!!!!! anim Name " + animName);
       }*/
       
        return false;
    }

    float calculateMatchParentGkTimePos(string animName)
    {

        float timeToCorrectPos = 0.119f;

        if (animName.Contains("down"))
        {
            //timeToCorrectPos = 0.130f;
            timeToCorrectPos = 0.110f;
            if (animName.Contains("punch"))
            {
                //timeToCorrectPos = 0.175f;
                timeToCorrectPos = 0.150f;
            }
        }
        else
        {
            if (animName.Contains("mid"))
            {
                //timeToCorrectPos = 0.126f;
                timeToCorrectPos = 0.106f;
                if (animName.Contains("punch"))
                {
                    //timeToCorrectPos = 0.161f;
                    timeToCorrectPos = 0.150f;
                }
            }
            else
            {
                if (animName.Contains("up"))
                {
                    //timeToCorrectPos = 0.128f;
                    timeToCorrectPos = 0.115f;
                    if (animName.Contains("punch"))
                    {
                        timeToCorrectPos = 0.145f;
                        //timeToCorrectPos = 0.175f;
                    }
                }
                else
                {
                    if (animName.Contains("straight"))
                    {
                        //timeToCorrectPos = 0.175f;
                    }
                }
            }
        }

        return timeToCorrectPos;
    }

    private void playMissGoalSound()
    {
        if (isShotActive())
        {
            float goalXOff = goalXOffset + 0.3f;
            
            if ((Mathf.Abs(ballRb.transform.position.x) > goalXOff &&
                 Mathf.Abs(ballRb.transform.position.x) < (goalXOff + 5.0f)) &&
                (Mathf.Abs(ballRb.transform.position.z) > PITCH_HEIGHT_HALF &&
                 Mathf.Abs(ballRb.transform.position.z) < (PITCH_HEIGHT_HALF + 3.0f)))

                if (!audioManager.isPlayingByName("crowdMiss1"))
                {
                    audioManager.Play("crowdMiss1");
                }
        }
    }

    private void fillMatchStatisticsToText()
    {
        teamAgoalsText.GetComponent<Text>().text = Globals.score1.ToString();
        teamBgoalsText.GetComponent<Text>().text = Globals.score2.ToString();

        if (Globals.playerPlayAway)
        {            
            teamBshotsText.GetComponent<Text>().text = matchStatistics.getShot("teamA").ToString();
            teamAshotsText.GetComponent<Text>().text = matchStatistics.getShot("teamB").ToString();
            teamBshotsOnTargetText.GetComponent<Text>().text = matchStatistics.getShotOnTarget("teamA").ToString();
            teamAshotsOnTargetText.GetComponent<Text>().text = matchStatistics.getShotOnTarget("teamB").ToString();
            teamBsavesText.GetComponent<Text>().text = matchStatistics.getSaves("teamA").ToString();
            teamAsavesText.GetComponent<Text>().text = matchStatistics.getSaves("teamB").ToString();

            int ballPossessionPercentage = matchStatistics.getBallPossessionPercentage("teamA");
            teamBballPossessionText.GetComponent<Text>().text = ballPossessionPercentage.ToString() + " %";
            teamAballPossessionText.GetComponent<Text>().text = (100 - ballPossessionPercentage).ToString() + " %";
        }
        else
        {        
            //teamAgoalsText.GetComponent<Text>().text = Globals.score1.ToString();
            //teamBgoalsText.GetComponent<Text>().text = Globals.score2.ToString();
            teamAshotsText.GetComponent<Text>().text = matchStatistics.getShot("teamA").ToString();
            teamBshotsText.GetComponent<Text>().text = matchStatistics.getShot("teamB").ToString();
            teamAshotsOnTargetText.GetComponent<Text>().text = matchStatistics.getShotOnTarget("teamA").ToString();
            teamBshotsOnTargetText.GetComponent<Text>().text = matchStatistics.getShotOnTarget("teamB").ToString();
            teamAsavesText.GetComponent<Text>().text = matchStatistics.getSaves("teamA").ToString();
            teamBsavesText.GetComponent<Text>().text = matchStatistics.getSaves("teamB").ToString();

            int ballPossessionPercentage = matchStatistics.getBallPossessionPercentage("teamA");
            teamAballPossessionText.GetComponent<Text>().text = ballPossessionPercentage.ToString() + " %";
            teamBballPossessionText.GetComponent<Text>().text = (100 - ballPossessionPercentage).ToString() + " %";         
        }
    }

    private void displayStatisticsPanel()
    {
       if (realTime > 1.5f && realTime < 2.5f)
       {
            deactivateCanvasElements();
            fillMatchStatisticsToText();
       }
        
       float matchStatisticsPanelFillAmount = (realTime - 3.0f) / 2.0f;
       if (realTime >= 3.0f && realTime < 5.0f)
       {
              //int chantRandom = UnityEngine.Random.Range(3, 5);
             //audioManager.Play("fanschant" + chantRandom.ToString());
           matchStatisticsPanel.SetActive(true);
           matchStatisticsPanel.GetComponent<Image>().fillAmount = matchStatisticsPanelFillAmount;
       } else
       {
           matchStatisticsPanel.GetComponent<Image>().fillAmount = 1.0f;
       }

       if (matchStatisticsPanelFillAmount > 0.1f && realTime <= 4.5f)
       {
           matchStatisticsPanelHeaderTop.SetActive(true);
       }

       if (realTime > 5.5f)
       {
           matchStatisticsFlagPanel.SetActive(true);
       }

       if (realTime > 5.2f)
       {
            matchStatisticsPanelHeaderDown.SetActive(true);
       }

       if (realTime > 7.0)
            matchStatisticsNext.SetActive(true);

        if (realTime > 9.0f)
       {
            audioManager.Stop("fanschantBackground2");
            audioManager.Play("music2");
        }
    }

    public void matchEndedOnClick()
    {
        //NationalTeams teams = new NationalTeams();
        Teams teams = new Teams("NATIONALS");

        Globals.recoverOriginalResolution();

        if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return;
        }

        if (teams.isAnyNewTeamUnclocked())
        {
            SceneManager.LoadScene("rewardNewTeam");
        }
        else {
            if (Globals.isFriendly)
            {
                SceneManager.LoadScene("menu");
            }
            else
            {
                //SceneManager.LoadScene("Leagues");               
                Globals.loadSceneWithBarLoader("Leagues");

            }
        }
    }

    /*WORKAROUND CHANGE IT*/
    //Vector3 savePos;
    //bool initSavePos = false;

    /*TODO SAVEPOS CANNOT BE GLOBAL */
    private void matchTarget(Animator animator, 
                             Rigidbody rb, 
                             ref Vector3 gkEndPos,
                             float timeToCorrectPos,
                             Vector3 stepSideAnimOffset,
                             ref Vector3 matchSavePos,
                             ref bool matchInitSavePos,
                             bool isCpu)
                        
    {
        float normTime = 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    
        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_") &&
            !checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_straight_"))
        {           
            if (normTime <= timeToCorrectPos)
            {
                matchInitSavePos = false;

                if (animator.isMatchingTarget || animator.IsInTransition(0))
                    return;

                animator.MatchTarget(new Vector3(gkEndPos.x, rb.transform.position.y, gkEndPos.z), 
                                                 Quaternion.identity, AvatarTarget.Root,
                                                 new MatchTargetWeightMask(Vector3.one, 0f), 0.0f, timeToCorrectPos);
            }
            else
            {
                if (!matchInitSavePos)
                {
                    if (animator.isMatchingTarget)
                        animator.InterruptMatchTarget(false);
                    matchSavePos = rb.transform.position;
                    matchInitSavePos = true;
                }

                rb.transform.position = matchSavePos;         
            }
        } else
        {
            matchInitSavePos = false;
        }
  
        /* Cpu step side */
        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset")) {
            if (animator.isMatchingTarget || animator.IsInTransition(0))
                return;
           
            animator.MatchTarget(stepSideAnimOffset,                                 
                                 Quaternion.identity, 
                                 AvatarTarget.Root,
                                 new MatchTargetWeightMask(Vector3.one, 0f), 
                                 0.0f, 
                                 1.0f);

            //print("GKDEBUG18 matchTarget3 " + stepSideAnimOffset);
        }

        return;     
    }

    /*SOURCE https://answers.unity.com/questions/1238142/version-of-transformtransformpoint-which-is-unaffe.html */
    public Vector3 TransformPointUnscaled(Transform transform, Vector3 position)
    {
        Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        return localToWorldMatrix.MultiplyPoint3x4(position);
    }

    public Vector3 InverseTransformPointUnscaled(Transform transform, Vector3 position)
    {
        Matrix4x4 worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
        return worldToLocalMatrix.MultiplyPoint3x4(position);
    }

    private int gkAnimationNumber(string animName, float distX)
    {
        int animInt = 1;
        if (animName.Contains("down"))
        {
            animInt = (int) ((distX - 1.3f) / 0.45f) + 1;
            if (distX < 1.3f)
                animInt = 1;
        }

        if (animName.Contains("mid"))
        {
            //animInt = (int) (distX / 0.55f) + 1;
            if (distX <= 1.0f)
            {
                animInt = 1;
            }
            else
            {
                animInt = (int)((distX - 1.0f) / 0.45f) + 1;
            }

        }

        if (animName.Contains("up"))
        {
            if (distX <= 1.0f)
            {
                animInt = 1;
            } else
            {
                animInt = (int)((distX - 1.0f) / 0.45f) + 1;              
            }         
        }

        if (animInt > 11)
            animInt = 11;

        return animInt;
    }

    private bool checkRectPointIntersection(Vector2 point, 
                                            Vector2 rectCenter,
                                            float size)
    {
        float width = size / 2.0f;
  
        if (point.x >= (rectCenter.x - width) &&
            point.x <= (rectCenter.x + width) &&
            point.y >= (rectCenter.y - width) &&
            point.y <= (rectCenter.y + width))
            return true;
        
        return false;
    }

    private bool checkTouchInterJoystickButtons(Vector2 touch)
    {
        float width = joystickButtonWidth / 2.0f;

        foreach (RectTransform buttonPos in joystickButtons)
        {
            if (touch.x >= (buttonPos.position.x - width) &&
                touch.x <= (buttonPos.position.x + width) &&
                touch.y >= (buttonPos.position.y - width) &&
                touch.y <= (buttonPos.position.y + width))
                return true;
        }
   
        return false;
    }

    private void updateTouch()
    {
        Vector3 shotDrawDirection;
        int gameAreaIdx = -1;
        Touch touch;
        bool isCpuShotActive = false;
    
        if (Input.touchCount <= 0  ||
            touchLocked)
        return;

        /*for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);
            print("UPDATTOUCH145 ENTERED " + touchLocked + " INPU COUNT "
                + Input.touchCount
                + " touch.fingerId " + touch.fingerId
                + " TOUCHPOSITION " + touch.position
                + " touch.phase " + touch.phase);
        }*/

        touch = Input.GetTouch(0);

        //print("UPDATTOUCH145 shot cpu active " + touch.position + " TouchPhase");

        if (isCpuShotActive)
        {    
            /*print("UPDATTOUCH145 START Input.touchCount ##### " + Input.touchCount 
                + " joystick.getPointerId()" + joystick.getPointerId());*/
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (joystick.getPointerId() == i) 
                    continue;

                touch = Input.GetTouch(i);
                bool intersectWithHelper = checkRectPointIntersection(
                                                touch.position,
                                                gkHelperRectTransform.position,
                                                gkHelperImageWidth);

                /*if click point is inside a Helper or is neither on a buttons 
                 * nor on joystick (check above) we should be safe */
                if (intersectWithHelper ||
                    !checkTouchInterJoystickButtons(touch.position)) {                   
                    gameAreaIdx = 0;
                    break;
                }                   
            }
        }
        else
        {         
            float xRangeTouchMax = (screenWidth - (joystickScreenOffset + (screenWidth * 0.03f)));
            float xRangeTouchMin = specialButtonsScreenOffset.x + (screenWidth * 0.03f);
            float yRangeTouchMin = joystickScreenOffset + (screenWidth * 0.03f);
            
            if (Globals.joystickSide.Contains("LEFT"))
            {
                xRangeTouchMin = joystickScreenOffset + (screenWidth * 0.03f);
                xRangeTouchMax = (screenWidth - (specialButtonsScreenOffset.x + (screenWidth * 0.03f)));
            }

            /*print("UPDATTOUCH145 specialButtonsScreenOffset " + specialButtonsScreenOffset +
                  " xRangeTouchMax " + xRangeTouchMax +
                  " xRangeTouchMin " + xRangeTouchMin +
                  " yRangeTouchMin " + yRangeTouchMin +
                  " cameraPos " + cameraPos);*/

            //print("DBGTOUCH GLOBAL " + Globals.joystickSide);
            if (isPlayerOnBall())
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    touch = Input.GetTouch(i);
                    if (Globals.joystickSide.Contains("RIGHT"))
                    {

                        //print("DBGTOUCH RIGHT " + " specialButtonsScreenOffset " + specialButtonsScreenOffset
                        //   + " xRangeTouchMin " + xRangeTouchMin
                        //   + " joystickScreenOffset " + joystickScreenOffset
                        //   + xRangeTouchMin + " xRangeTouchMax " + xRangeTouchMax
                        //     + " TOUCHPOS " + touch.position + " YRANGETOUCHMIN " + yRangeTouchMin + " cameraPOs " + cameraPos);

                        if ((touch.position.x > xRangeTouchMin && touch.position.x < xRangeTouchMax && !isTouchBegin) ||
                            (touch.position.x <= xRangeTouchMin && touch.position.y > specialButtonsScreenOffset.y && !isTouchBegin) ||
                            (touch.position.x < cameraPos.x && (touch.position.y >= yRangeTouchMin && !isTouchBegin)) ||
                            (isTouchBegin && touchFingerId == touch.fingerId))
                        {
                            gameAreaIdx = i;
                            break;
                        }
                    } else {
                        //print("DBGTOUCH LEFT " + " specialButtonsScreenOffset " + specialButtonsScreenOffset
                        //    + " xRangeTouchMin " + xRangeTouchMin
                        //    + " joystickScreenOffset " + joystickScreenOffset 
                        //    + xRangeTouchMin + " xRangeTouchMax " + xRangeTouchMax
                        //      + " TOUCHPOS " + touch.position + " YRANGETOUCHMIN " + yRangeTouchMin + " cameraPOs " + cameraPos);

                        if ((touch.position.x > xRangeTouchMin && touch.position.x < xRangeTouchMax && !isTouchBegin) ||
                            (touch.position.x >= xRangeTouchMax && touch.position.x < cameraPos.x && touch.position.y > specialButtonsScreenOffset.y && !isTouchBegin) ||
                            (touch.position.x < cameraPos.x && (touch.position.y >= yRangeTouchMin && !isTouchBegin)) ||
                            (isTouchBegin && touchFingerId == touch.fingerId))
                        {
                            //print("DBGTOUCH ENTERED");
                            gameAreaIdx = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                gameAreaIdx = -1;
            }
        }
     
        //print("TOUCHPOSITIONBEFORE  " + gameAreaIdx + " TOUCH POS " + touch.position.y + " touch.phase " + touch.phase + " TOUCHLOCKE " + touchLocked + "touchCount" + touchCount);

        if (gameAreaIdx == -1)           
        {
            return;
        }
      
            lastTouch.x = touch.position.x;
            lastTouch.y = touch.position.y;

            if (touch.phase == TouchPhase.Moved)
            {
                    if (!isTouchBegin ||
                       (isTouchBegin && (touch.fingerId != touchFingerId)))
                        return;
               
                    touchCount++;
                    shotDirection2D = (touch.position - startPos).normalized;
                    isShootingDirectionSet = true;                   
                    drawDistance += Vector2.Distance(touch.position, prevMovedPos);
          
                    /*draw a touch line*/
                    if (lineEndPos != Vector3.zero)
                    {
                        lineStartPos = lineEndPos;
                    }
               
                    if (isPlayerOnBall()) {
                        Plane objPlane = new Plane(
                            Camera.main.transform.forward * -1, trailShoot.transform.position);                             
                        Ray mRay = Camera.main.ScreenPointToRay(touch.position);
                        float rayDistance;
                        if (objPlane.Raycast(mRay, out rayDistance))
                        {
                            lineEndPos = mRay.GetPoint(rayDistance);
                            trailShoot.transform.position = lineEndPos;
                            //print("TRAILSHOTPOS " + trailShoot.transform.position);
                        }
                    }
     
                    prevMovedPos = touch.position;
     
                    /*Overwritee endPos too, in case user not finish */
                    endPos = touch.position;
                    drawTimeEnd = Time.time;
               
                    if (midTouchPosIdx < MID_MAX_POS)
                        midTouchPos[midTouchPosIdx++] = touch.position;

                    //print("UPDATTOUCH145 MOVED" + touch.position
                    //    + " touchFingerId " + touchFingerId + " ORG TOUCH ID " + touch.fingerId);
            }

            if (touch.phase == TouchPhase.Began)
            {
                if (!isTouchBegin && !isCpuShotActive)
                    touchFingerId = touch.fingerId;

                /*get only one point*/
                if (isCpuShotActive)
                {
                    updateLastGkTouchPos(touch);
                    touchLocked = true;
                    gkTouchDone = true;
                    gkClickHelper.enabled = true;
                    rectTransformGkClickHelper.position = touch.position;    
                    return;
                } else
                {
                    gkTouchDone = false; 
                }

                //print("UPDATTOUCH145 BEGIN" + touch.position + " touchFingerId " + touchFingerId 
                //    + " ORG TOUCH ID " + touch.fingerId
                //    + " isTouchBegin " + isTouchBegin + " !isCpuShotActive " + !isCpuShotActive);

                //if (isTouchBegin && 
                //    (touch.fingerId != touchFingerId))
                //    return;

                isTouchBegin = true;
                startPos = touch.position;
                midPos = touch.position;
                prevMovedPos = touch.position;
                touchCount = 1;
                drawTimeStart = Time.time;
                drawTimeEnd = 0.0f;
                drawDistance = 0.0f;
                endPos = Vector2.zero;

                midTouchPosIdx = 0;
                    
               // gkTouchDone = false;                           
                lineEndPos = Vector3.zero;

                trailShoot = (GameObject) Instantiate(drawPrefabShotTrail, Vector3.zero, Quaternion.identity);
                trailShoot.GetComponent<TrailRenderer>().sortingOrder = 1;

                Plane objPlane = new Plane(Camera.main.transform.forward * -1, trailShoot.transform.position);
                Ray mRay = Camera.main.ScreenPointToRay(touch.position);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    lineEndPos = mRay.GetPoint(rayDistance);
                    trailShoot.transform.position = lineEndPos;
                    //print("TRAILSHOTPOS " + trailShoot.transform.position);    
                }
        }

            if (touch.phase == TouchPhase.Ended)
            {
                if (!isTouchBegin ||
                    (isTouchBegin && (touch.fingerId != touchFingerId)))
                    return;

                //print("UPDATTOUCH145 END" + touch.position
                //+ " touchFingerId " + touchFingerId
                //+ " ORG TOUCH ID " + touch.fingerId);

                endPos = touch.position;
                drawDistance += Vector2.Distance(touch.position, prevMovedPos);
                drawTimeEnd = Time.time;
                touchCount++;
                touchLocked = true;
                touchFingerId = -1;
                isTouchBegin = false;

            //print("LASTTOUCH POS IN WORLDSPACE BEFORE " + gkTouchPosRotatedRbWS);
            //correctPosIfOutOfPitch(ref gkTouchPosRotatedRbWS);

            //if (correctPosIfOutOfPitch(ref gkTouchPosRotatedRbWS))
            //{
                gkTouchDone = true;
                //}

                //print("DEBUGLASTTOUCHLAKI AFTER CORRECT LAST TOUCH" + gkTouchPosRotatedRbWS);
                        
                if (lineStartPos != Vector3.zero)
                {
                    //DrawLine(lineStartPos, lineEndPos, Color.white, 0.8f);
                }

                if (isPlayerOnBall())
                {
                    Plane objPlane = new Plane(Camera.main.transform.forward * -1, trailShoot.transform.position);
                    Ray mRay = Camera.main.ScreenPointToRay(touch.position);
                    float rayDistance;
                    if (objPlane.Raycast(mRay, out rayDistance))
                    {
                        lineEndPos = mRay.GetPoint(rayDistance);
                        trailShoot.transform.position = lineEndPos;
                        //print("TRAILSHOTPOS " + trailShoot.transform.position);
                }
            }
            }
    }

    public bool getGkTouchDone()
    {
        return gkTouchDone;
    }

    private void updateLastGkTouchPos(Touch touch)
    {
        gkTouchPosRotatedRbWS = INCORRECT_VECTOR;
        gkTouchPosRbWS = INCORRECT_VECTOR;

        Ray ray = m_MainCamera.ScreenPointToRay(touch.position);

        Plane rbRotatedPlane = new Plane(
            rotatedRbToBall.transform.forward,
            rotatedRbToBall.transform.position);

        Plane rbPlane = new Plane(
            rb.transform.forward,
            rb.transform.position);

        //print("DEBUGLASTTOUCHLUCKXYU RB TRANS" + rb.transform.position + " RB FORWARD "
        //    + rb.transform.forward + " ROTATED " + rotatedRbToBall.transform.position + " ROT FORWARD "
        //    + rotatedRbToBall.transform.forward);

        //DrawPlane(rotatedRbToBall.transform.positi//on, rotatedRbToBall.transform.forward);
        //DrawPlane(rb.transform.position, rb.transform.forward);

        //DrawLine(rotatedRbToBall.transform.position, cpuPlayer.getBallInit(), Color.white, 2.0f);

        float dist = 0.0f;
        if (rbRotatedPlane.Raycast(ray, out dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            gkTouchPosRotatedRbWS = hitPoint;
            //DrawLine(ray.origin, hitPoint, Color.green, 3.0f);
        }

        dist = 0.0f;
        if (rbPlane.Raycast(ray, out dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            gkTouchPosRbWS = hitPoint;
            //DrawLine(ray.origin, hitPoint, Color.green, 3.0f);
        }
    }

    private bool gkAnimOutOfPitch(Vector3 pos)
    {
        float maxPosX = PITCH_WIDTH_HALF - 1.0f;
        float maxPosZ = PITCH_HEIGHT_HALF - 1.0f;
        float midPointOff = 1.0f;

        /*player goal */
        if (Mathf.Abs(pos.x) <= goalXOffset)
            return false;

        if (Mathf.Abs(pos.x) > maxPosX ||
            Mathf.Abs(pos.z) > maxPosZ ||
            Mathf.Abs(pos.z) < midPointOff)
        {
            //print("DEBUG4XY OF PITCH YES " + pos);
            return true;
        }

        return false;
    }

    private bool correctPosIfOutOfPitch(ref Vector3 pos)
    {
       float maxPosX = PITCH_WIDTH_HALF - gkMinDistToWalls;
       float maxPosZ = PITCH_HEIGHT_HALF - gkMinDistToWalls;

       if (gkAnimOutOfPitch(pos))
            return false;

       if (Mathf.Abs(pos.x) > maxPosX)
       {
            if (pos.x < 0.0f)
                pos.x = -maxPosX;
            else
                pos.x = maxPosX;
       }

       if (Mathf.Abs(pos.z) > maxPosZ)
       {
            if (pos.z < 0.0f)
                pos.z = -maxPosZ;
            else
                pos.z = maxPosZ;
       }

       if (Mathf.Abs(pos.z) < 1.0f)
       {
            pos.z = 1.0f;
       }

       return true;
    }

    private bool checkIfAnyAnimationPlaying(Animator animator, float end)
    {
        //animator.Update(0f);

        for (int i = 0; i < AllAnimationsNames.Count; i++)
        {
            if (isPlaying(animator, AllAnimationsNames[i], end))
                return true;
        }

        return false;
    }

    private bool checkIfAnyAnimationPlaying(Animator animator, float end, string[] excluded)
    {
        //animator.Update(0f);

        for (int i = 0; i < AllAnimationsNames.Count; i++)
        {
            if (excluded.Any(AllAnimationsNames[i].Equals)) continue;

            if (isPlaying(animator, AllAnimationsNames[i], end))
                return true;
        }

        return false;
    }

    private bool checkIfAnyAnimationPlayingContain(List<string> list,
                                                   Animator animator, 
                                                   float end, 
                                                   string contain)
    {

        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Contains(contain))
                continue;

            if (isPlaying(animator, list[i], end))
                return true;
        }

        return false;
    }


    private bool checkIfAnyAnimationPlayingContain(Animator animator, float end, string contain)
    {

        //animator.Update(0f);
        for (int i = 0; i < AllAnimationsNames.Count; i++)
        {
            if (!AllAnimationsNames[i].Contains(contain)) 
                continue;

            if (isPlaying(animator, AllAnimationsNames[i], end))
                return true;
        }

        return false;
    }

    public string nameAnimationPlaying(Animator animator, float end)
    {
        //animator.Update(0f);
        for (int i = 0; i < AllAnimationsNames.Count; i++)
        {      
            if (isPlaying(animator, AllAnimationsNames[i], end))
                return AllAnimationsNames[i];
        }

        for (int i = 0; i < RunAnimationsNames.Count; i++)
        {
            if (isPlaying(animator, RunAnimationsNames[i], end))
                return RunAnimationsNames[i];
        }

        return string.Empty;
    }

    public string nameAnimationPlaying(Animator animator, float end, int option)
    {
        switch (option)
        {
            case 1:
                for (int i = 0; i < AllAnimationsNames.Count; i++)
                {
                    if (isPlaying(animator, AllAnimationsNames[i], end))
                        return AllAnimationsNames[i];
                }
                break;
            case 2:
                for (int i = 0; i < RunAnimationsNames.Count; i++)
                {
                    if (isPlaying(animator, RunAnimationsNames[i], end))
                        return RunAnimationsNames[i];
                }
                break;
        }
             
        return string.Empty;
    }

    private bool isBallOutOfPitch()
    {
        if (Mathf.Abs(ballRb.transform.position.z) > 15.0f ||
            Mathf.Abs(ballRb.transform.position.x) > 21.5f)
        {
            return true;
        }


        return false;
    }

    private bool isBallinGame()
    {
        return isBallInGame;
    }

    private void clearVariables()
    {
        //joystick.setDefaultColorButton();

        delayAfterGoal = 0.0f;
        //print("VELOCITY CLEARED HERE 4");

        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

        clearAfterBallCollision();
        isBallInGame = true;
        goalJustScored = false;
        isBallOut = false;
        shotRet = false;
        onBall = PlayerOnBall.NEUTRAL;
        //isUpdateBallPosActive = false; ???
    }

    /*private void clearAfterShot()
    {
        initShot = false;
        isLobActive = false;
        isBallTrailRendererInit = false;
        //parentRb.ball.ballTrailRendererStop();
        maxMovedDot = float.MaxValue;
        endPos = Vector2.zero;
        touchCount = 0;
        drawDistance = 0.0f;
        touchLocked = false;
        onBall = PlayerOnBall.NEUTRAL;
    }*/

    private void prepareFrameClean()
    {
        onBall = PlayerOnBall.NEUTRAL;        
    }

    private void clearAfterBallCollision()
    {
        passedShotFlyTime = 0.0f;
        preShotActive = false;
        shotActive = false;
        shotRet = false;
        initPreShot = false;
        initShot = false;
        isLobActive = false;
        isBallTrailRendererInit = false;
        touchCount = 0;
        drawDistance = 0.0f;
        touchLocked = false;   
        gkTouchDone = false;  
        initCpuAdjustAnimSpeed = false;
        initGkDeleyLevel = false;
        levelDelay = 0.0f;

        isTouchBegin = false;
        touchFingerId = -1;

        //maxMovedDot = float.MaxValue;
        /*is it needed??*/
        //endPos = Vector2.zero;
    }

    protected void LookRotationRb(Rigidbody rb, Vector3 direction)
    {
        Quaternion lookOnLook;

        lookOnLook = Quaternion.LookRotation(direction);

        rb.transform.rotation =
                Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);
    }

    private int getRandInt(int min, int max)
    {
        //Random rand = new System.Random();

        //Unity.Mathematics.Random Random;

        return UnityEngine.Random.Range(min, max);

        /** (max - min) + min;*/
    }

    private float getRandFloat(float min, float max)
    {
        //Random rand = new System.Random();

        //Unity.Mathematics.Random Random;

        return UnityEngine.Random.Range(min, max);

        /** (max - min) + min;*/
    }

    private Vector3 ballTrajectory()
    {

        return Vector3.zero;
    }

    private float distance(Rigidbody rb1, Rigidbody rb2)
    {
        float distance = Vector3.Distance(rb1.transform.position,
                                          rb2.transform.position);

        return distance;

    }

    private void moveInLineInLocalSpace(Rigidbody rb, float dist)
    {
        Vector3 locPos = rb.transform.InverseTransformDirection(rb.position);
        locPos.x += dist;
        rb.position = rb.transform.TransformDirection(locPos);
    }
    
    public void stepUpDown(Animator animator, Rigidbody rb, float dist, string animName)
    {
        Vector3 locPos = rb.transform.InverseTransformPoint(rb.position);
        locPos.z += dist;
        rb.position = rb.transform.TransformPoint(locPos);

        //if (!checkIfAnyAnimationPlayingContain(animator, 0.8f, animName))
        if (!isPlaying(animator, animName, 1.0f))
        {
            animator.Play(animName, 0, 0.0f);
        }
    }

    public void stepSide(Animator animator, 
                         Rigidbody rb, 
                         float dist, 
                         string animName, 
                         ref Vector3 stepSideAnimOffset)
    {

        //if (!isPlaying(animator, animName, 1.0f))
        if (!checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset"))
        {
            Vector3 locPos = InverseTransformPointUnscaled(rb.transform, rb.transform.position);
            locPos.x += dist;

            animator.Play(animName, 0, 0.0f);
            //print("STEP SIDE " + animName);
            stepSideAnimOffset = TransformPointUnscaled(rb.transform, locPos);

            //rb.position = TransformPointUnscaled(rb.transform, locPos);
        }
    }

    private Vector3 getBallHitRbPoint(GameObject gameObjectRbRotated,
                                      SHOTVARIANT shotvariant,
                                      Vector3 outShotStart,
                                      Vector3 outShotMid,
                                      Vector3 outShotEnd)       
    {
        Vector3 realHitPlace = INCORRECT_VECTOR;

        if (shotvariant == SHOTVARIANT.CURVE)
        {
            realHitPlace = bezierCurvePlaneInterPoint(0.0f, 
                                                      1.0f,
                                                      gameObjectRbRotated, 
                                                      outShotStart, 
                                                      outShotMid, 
                                                      outShotEnd, 
                                                      false);

            /*print("DEBUGLASTTOUCHLUCKXYU #### realHitPlace LOCALSPACE "
                + realHitPlace + " gameObjectRbRotated " + gameObjectRbRotated.transform.position);*/


            if (Mathf.Abs(realHitPlace.z) > 0.3f)
                return INCORRECT_VECTOR;

            realHitPlace.z = 0.0f;
            realHitPlace = TransformPointUnscaled(gameObjectRbRotated.transform, realHitPlace);

            /*print("DEBUGLASTTOUCHLUCKXYU #### realHitPlace WORLD CURVED " + realHitPlace +
              " outShotStart " + outShotStart + " outShotMid " + outShotMid + " outShotEnd " + outShotEnd);*/
        }
        else
        {
            Vector3 cpuPlayerForwardVector = gameObjectRbRotated.transform.forward;
            Plane cpuPlayerXLocalPlane = new Plane(
               cpuPlayerForwardVector,
               gameObjectRbRotated.transform.position + gameObjectRbRotated.transform.forward * 0.6f);

            Vector3 ballAway = new Vector3(ballRb.transform.position.x + (ballRb.velocity.x * 10.0f),
                                           ballRb.transform.position.y + (ballRb.velocity.y * 10.0f),
                                           ballRb.transform.position.z + (ballRb.velocity.z * 10.0f));

            float distHit = float.MaxValue;
            Ray rayBall = new Ray(
               ballRb.transform.position,
               (ballAway - ballRb.transform.position).normalized);

            if (cpuPlayerXLocalPlane.Raycast(rayBall, out distHit))
            {
                realHitPlace = rayBall.GetPoint(distHit);
            }

            /*print("DEBUGLASTTOUCHLUCKXYU #### realHitPlace WORLD NOT CURVED " + realHitPlace +
              " outShotStart " + outShotStart + " outShotMid " + outShotMid + " outShotEnd " + outShotEnd);*/
        }

        return realHitPlace;
    }

    private void drawGkHelperCircle(
                                    GameObject gameObjectRbRotated,
                                    SHOTVARIANT shotvariant,
                                    Vector3 outShotStart,
                                    Vector3 outShotMid,
                                    Vector3 outShotEnd)
    {
        /*Vector3 direction = endPosOrg - ballRb.position;
        Plane playerXLocalPlane = new Plane(
                                            rb.transform.forward,
                                            rb.transform.position);*/

        /*print("DEBUGLASTTOUCHLUCKXYU DRAW HELPER CIRCLE START SHOTV " + shotvariant +
             " OUTSHOTS " + outShotStart + " MID " + outShotMid + " OUTSHOTEND " + outShotEnd);*/
        Vector3 hitPointWS = getBallHitRbPoint(gameObjectRbRotated, shotvariant, outShotStart, outShotMid, outShotEnd);
        //print("DEBUGLASTTOUCH HELPER IMAGE " + hitPointWS + " shotvariant " + shotvariant);
        if (hitPointWS != INCORRECT_VECTOR) {
            gkHelperRectTransform.position = m_MainCamera.WorldToScreenPoint(hitPointWS);           
            gkHelperImage.enabled = true;
       
        }
  

        return;


        /*float enter = 0.0f;

        Ray rayBall = new Ray(
                              ballRb.position,
                              direction.normalized);


        if (playerXLocalPlane.Raycast(rayBall, out enter))
        {
            lineStartPos = rayBall.GetPoint(enter);         
            RectTransform rectTransform = gkHelperImage.GetComponent<RectTransform>();    
            
            rectTransform.position = m_MainCamera.WorldToScreenPoint(lineStartPos);

            gkHelperImage.enabled = true;           
        }*/
    }

    /*void eraseShotBar()
    {
        shotBar.fillAmount = 0.0f;
        speedShotText.text = "";
    }*/

    /*public bool getGkImageHelperErase()
    {
        return gkHelperImageErased;
    }*/

    public bool getGkHelperImageVal()
    {
        return gkHelperImage.enabled;
    }

    public void setGkHelperImageVal(bool val)
    {
        gkHelperImage.enabled = val;
        //MarkerBasic.SetActive(val);
        if (!val)
            gkClickHelper.enabled = val;
    }

    /*void eraseGkHelperImage()
    {
        gkHelperImage.enabled = false;
    }*/

    /*CODE FROM UNITY FORUM*/
    void DrawPlane(Vector3 position, Vector3 normal)
    {

        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        Vector3 corner0 = position + v3;
        Vector3 corner2 = position - v3;
        Quaternion q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        Vector3 corner1 = position + v3;
        Vector3 corner3 = position - v3;

        DrawLine(corner0, corner2, Color.red);
        DrawLine(corner1, corner3, Color.red);
        DrawLine(corner0, corner1, Color.red);
        DrawLine(corner1, corner2, Color.red);
        DrawLine(corner2, corner3, Color.red);
        DrawLine(corner3, corner0, Color.red);
        //DrawLine(position, normal, Color.red);
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public float timeOfBallFlyBasedOnPosition(Vector3 pos, float timeOfFlyShot)
    {
        float defaultDist = PITCH_HEIGHT_HALF;
        float additionalDist = Mathf.Abs(pos.z);

        return (timeOfFlyShot * (defaultDist + additionalDist)) / defaultDist;
    }

    public float timeOfBallFlyBasedOnPositionReverse(Vector3 pos, float timeOfFlyShotDist)
    {
        float defaultDist = PITCH_HEIGHT_HALF;
        float additionalDist = Mathf.Abs(pos.z);

        return (timeOfFlyShotDist / (defaultDist + additionalDist)) * defaultDist;
    }

    public int convertLevelStringToInteger(string level)
    {
        switch (level)
        {
            case "KID":
                return 1;
            case "EASY":
                return 2;
            case "NORMAL":
                return 3;
            case "HARD":
                return 4;
            case "EXPERT":
                return 5;
            case "TEST_NO_LIMITS":
                return 0;
            default: 
                return 3;
        }
    }

    private void goalResize(bool isCpu)
    {
        if (isCpu)
        {
            cpuGoalUp = GameObject.Find("goalUp");
            wallUpLeft1 = GameObject.Find("wallUpLeft1");
            wallUpRight1 = GameObject.Find("wallUpRight1");
            wallUpLeft2 = GameObject.Find("wallUpLeft2");
            wallUpRight2 = GameObject.Find("wallUpRight2");
            wallUpLeftTop = GameObject.Find("wallUpLeftTop");
            wallUpRightTop = GameObject.Find("wallUpRightTop");

            if (!Globals.cpuGoalSize.Equals("STANDARD"))
            {
                wallUpLeft1.SetActive(false);
                wallUpRight1.SetActive(false);

                wallUpLeft2.transform.position =
                    new Vector3(-13.8f, 0.6f, 14.25f);       
                wallUpLeftTop.transform.position =
                    new Vector3(-13.8f, 1.25f, 14.25f);

                wallUpRight2.transform.position =
                   new Vector3(13.8f, 0.6f, 14.25f);
                wallUpRightTop.transform.position =
                    new Vector3(13.8f, 1.25f, 14.25f);

                wallUpLeft2.transform.localScale =
                wallUpRight2.transform.localScale =
                    new Vector3(12.4f, 1.2f, 0.5f);

                wallUpLeftTop.transform.localScale =
                wallUpRightTop.transform.localScale =
                   new Vector3(12.4f, 0.1f, 0.5f);

                cpuGoalUp.transform.localScale = new Vector3(3.4f, 2.9f, 1f);
                goalSizesCpu = new Vector3(7.5f, 4.15f, 14.0f);
                /*CHANGE A GOAL ONLY ON ON MATCH*/
                Globals.cpuGoalSize = "STANDARD";
            }
        }
    }

    /*LEVEL DEPENDENT*/
    private float calcRunLevelSpeed(int level, int skill, bool isCpu)
    {
        /*range 0 to 100 */
        float avgSkill = (float) skill / 2f;
        float skillsInter = Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, avgSkill);
        float levelInter = Mathf.InverseLerp(LEVEL_MIN, LEVEL_MAX, (float) level);
        float speedNorm = skillsInter; 

        if (isCpu)
            speedNorm = (skillsInter + levelInter) / 2.0f;

        //print("DBGTEAMSSPEED speed NORM " + speedNorm + " skillsInter " + skillsInter
        //    + " levelInter " + levelInter);
        /*print("calcRunLevelSpeed skillsInter " + " ISCPU " + isCpu + " skillsInter " + skillsInter + " AVG "
            + avgSkill + " SKILLS " + skill + " levelInter " + levelInter + " speedNorm " + speedNorm + " ADD " +
            (speedNorm * (SPEED_RUN_MAX - SPEED_RUN_MIN)));

        print("calcRunLevelSpeed final " + (SPEED_RUN_MIN +
                (speedNorm * (SPEED_RUN_MAX - SPEED_RUN_MIN))));

        print("DEBUG125125 running speed " + isCpu + " " + (SPEED_RUN_MIN +
                               (speedNorm * (SPEED_RUN_MAX - SPEED_RUN_MIN))));*/

        float ret = SPEED_RUN_MIN +
            (speedNorm * (SPEED_RUN_MAX - SPEED_RUN_MIN));

        //print("DBGTEAMSSPEED RET " + ret);

        if (!isCpu)
        {
            ret = Mathf.Max(7.0f, ret);
        } else
        {
            ret = Mathf.Max(6.5f, ret);
        }

        return ret;
    }
  
    private float getSkillInterpolation(float skillVal)
    {
        return Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, skillVal);
    }

    private float getSkillInterpolationReverse(float skillVal)
    {
        return Mathf.InverseLerp(SKILLS_MAX_VALUE, 0.0f, skillVal);
    }

    private float getLevelInterpolation()
    {
        return Mathf.InverseLerp(LEVEL_MIN, LEVEL_MAX, (float) Globals.level);
    }

    private float getLevelInterpolationReverse()
    {
        return Mathf.InverseLerp(LEVEL_MAX, LEVEL_MIN, (float) Globals.level);
    }
    
    private bool goAndTakeActiveShotBall(float maxSpeed,
                                         Vector3 targetSize,
                                         Vector3 endPosOrg)
    {
        bool isSpeedBelow = isShotSpeedBelow(maxSpeed);

        if (!isShotOnTarget(endPosOrg, targetSize) &&
            isSpeedBelow)
            return true;

        return false;
    }

    private bool isShotSpeedBelow(float speed)
    {
        if (Mathf.Abs(ballRb.velocity.x) < speed &&
            Mathf.Abs(ballRb.velocity.z) < speed)
            return true;
        return false;
    }


    private Vector3 getGkBallInterPoint(GameObject rotatedRbToBall,
                                        SHOTVARIANT shotvariant,
                                        Vector3 outShotStart,
                                        Vector3 outShotMid,
                                        Vector3 outShotEnd,
                                        Vector3 endPosOrg,
                                        float timeofBallFly,
                                        float passedShotFlyTime,
                                        Vector3 ballPos)
    {

        //Profiler.BeginSample("getGkBallInterPoint cpu fixed update");


        float timeToHitZ = float.MaxValue;
        Vector3 hitPointLocal = INCORRECT_VECTOR;
        Vector3 hitPointWorld = INCORRECT_VECTOR;

        if (shotvariant == SHOTVARIANT.CURVE)
        {
            curveShotHitPoint(rotatedRbToBall,
                              outShotStart,
                              outShotMid,
                              outShotEnd,
                              endPosOrg,
                              timeofBallFly,
                              passedShotFlyTime,
                              ref timeToHitZ,
                              ref hitPointWorld,
                              ref hitPointLocal);
        }
        else
        {

            straightShotHitPoint(rotatedRbToBall,
                                 ballPos,
                                 endPosOrg,
                                 ref hitPointLocal,
                                 ref hitPointWorld,
                                 ref timeToHitZ);
        }

        //print("DEBUGCHANCETOSHOOT getGkBallInterPoint hitPointWorld " + hitPointWorld + " hitPointLocal " + hitPointLocal);

        //Profiler.EndSample();

        return hitPointLocal;       
    }

    /*check where we should shoot*/
    private Vector3 getTheBestgoalEnd(Rigidbody rb,
                                      Vector3 ballPos,
                                      Vector3 cornerPoints,
                                      float playerRunSpeed,
                                      Vector3 goalSize,
                                      bool isCpu)
    {
        Vector3 realHitPlaceLocal = INCORRECT_VECTOR;
        Vector3 hitPointWorld = INCORRECT_VECTOR;
        Vector3 ballAway = Vector3.zero;
        float distHit = float.MaxValue;

        //GameObject rotatedRbToBall = new GameObject();
        Ray rayBall;

        //Profiler.BeginSample("getTheBestgoalEnd cpu fixed update");

        getRotatedRbToBall(ballPos,
                           rb,
                           ref tmpRotatedRbToBall,
                           cornerPoints);

        Vector3 playerForwardVector =
            tmpRotatedRbToBall.transform.forward;

        Plane playerXLocalPlane = new Plane(
           playerForwardVector,
           tmpRotatedRbToBall.transform.position + (tmpRotatedRbToBall.transform.forward * 0.6f));

        Vector3 endGoalPos = new Vector3(0, 0, -PITCH_HEIGHT_HALF);

        float maxHitX = 0f;
        Vector3 maxGoalEnd = INCORRECT_VECTOR;
        for (int i = 0; i < 3; i++)
        {
            realHitPlaceLocal = INCORRECT_VECTOR;

            if (i == 1)
                endGoalPos.x = goalSize.x;
            if (i == 2)
                endGoalPos.x = -goalSize.x;

            distHit = float.MaxValue;
            rayBall = new Ray(
                ballPos,
                (endGoalPos - ballPos).normalized);

            if (playerXLocalPlane.Raycast(rayBall, out distHit))
            {
                hitPointWorld = rayBall.GetPoint(distHit);
                realHitPlaceLocal =
                    InverseTransformPointUnscaled(tmpRotatedRbToBall.transform,
                                                  hitPointWorld);
            }

            /*shot anim execute around 700 ms before ball starts fly*/
            /*add ball fly to hit point time*/
            /*print("DEBUGCHANCETOSHOOT getTheBestgoalEnd endGoalPos "
                + endGoalPos + " realHitPlaceLocal.x "
                + realHitPlaceLocal.x + " hitPointWorld "
                + hitPointWorld
                + " playerRunSpeed * 0.7f " + playerRunSpeed * 0.7f);*/

            if (realHitPlaceLocal != INCORRECT_VECTOR &&
               (Mathf.Abs(realHitPlaceLocal.x) > maxHitX))
            {
                maxHitX = Mathf.Abs(realHitPlaceLocal.x);
                maxGoalEnd = endGoalPos;
            }
        }

        //Profiler.EndSample();
        return maxGoalEnd;
    }

    private bool isChanceToShoot(Rigidbody rb,
                                 Vector3 ballPos,
                                 Vector3 cornerPoints,
                                 float playerRunSpeed,
                                 Vector3 goalSize,
                                 ref Vector3 endPosOrgPredefined,
                                 bool isCpu)
    {

        Vector3 realHitPlaceLocal = INCORRECT_VECTOR;
        Vector3 hitPointWorld = INCORRECT_VECTOR;
        Vector3 ballAway = Vector3.zero;
        float distHit = float.MaxValue;
        //GameObject rotatedRbToBall = new GameObject();
        Ray rayBall;

        //Profiler.BeginSample("isChanceToShoot cpu fixed update");

        getRotatedRbToBall(ballPos,
                           rb,
                           ref tmpRotatedRbToBall,
                           cornerPoints);
        //Profiler.EndSample();

        Vector3 playerForwardVector =
            tmpRotatedRbToBall.transform.forward;

        Plane playerXLocalPlane = new Plane(
           playerForwardVector,
           tmpRotatedRbToBall.transform.position + (tmpRotatedRbToBall.transform.forward * 0.6f));
  
        Vector3 endGoalPos = new Vector3(0, 0, -PITCH_HEIGHT_HALF);

        for (int i = 0; i < 3; i++)
        {
            realHitPlaceLocal = INCORRECT_VECTOR;

            if (i == 1)
                endGoalPos.x = goalSize.x;
            if (i == 2)
                endGoalPos.x = -goalSize.x;

            distHit = float.MaxValue;
            rayBall = new Ray(
                ballPos,
                (endGoalPos - ballPos).normalized);

            if (playerXLocalPlane.Raycast(rayBall, out distHit))
            {
                hitPointWorld = rayBall.GetPoint(distHit);
                realHitPlaceLocal =
                    InverseTransformPointUnscaled(tmpRotatedRbToBall.transform,
                                                  hitPointWorld);
            }

            /*shot anim execute around 700 ms before ball starts fly*/
            /*add ball fly to hit point time*/
            /*print("DEBUGCHANCETOSHOOT ISCHANCETOSHOT endGoalPos " 
                + endGoalPos + " realHitPlaceLocal.x " 
                + realHitPlaceLocal.x + " hitPointWorld " 
                + hitPointWorld 
                + " playerRunSpeed * 0.7f " + (playerRunSpeed * 0.7f)
                + " Mathf.Abs(realHitPlaceLocal.x) " 
                + Mathf.Abs(realHitPlaceLocal.x)
                + " PLAYERRUN " 
                + playerRunSpeed
                + " Mathf.Abs(realHitPlaceLocal.x) > (playerRunSpeed * 0.7f) "
                + (Mathf.Abs(realHitPlaceLocal.x) > (playerRunSpeed * 0.7f)));*/


            if (realHitPlaceLocal != INCORRECT_VECTOR &&
               (Mathf.Abs(realHitPlaceLocal.x) > (playerRunSpeed * 0.7f)))
            {
                //print("DEBUGCHANCETOSHOOT ISCHANCETOSHOT ENTERED " + realHitPlaceLocal);
                endPosOrgPredefined = endGoalPos;
                //Profiler.EndSample();
                return true;
            }
        }

        //Profiler.EndSample();

        return false;
    }

    private float getSpeed(int level, float speed)
    {
        float calcSpeed = speed;

        switch (Globals.level)
        {
            case 5:
                calcSpeed = Mathf.Clamp(speed, 7.0f, SPEED_RUN_MAX);
                break;
            case 4:
                calcSpeed = Mathf.Clamp(speed, 6.7f, 9.0f);
                break;
            case 3:
                calcSpeed = Mathf.Clamp(speed, 6.5f, 8.0f);
                break;
            case 2:
                calcSpeed = Mathf.Clamp(speed, 6.5f, 7.0f);
                break;
            default:
                calcSpeed = speed;
                break;
        }

        //print("DEBUGBACKSPEED NEW BACK SPEED " + calcSpeed);
        return calcSpeed;
    }

    private float getBackSpeed(int level, float speed)
    {
        float backSpeed = speed;

        /*back speed should be similar on each level. Since the setup on a pitch is different*/
        switch (Globals.level)
        {
            case 5:
                backSpeed = Mathf.Max(8.5f, speed);
                break;
            case 4:
                backSpeed = Mathf.Max(8.0f, speed);
                break;
            case 3:
                backSpeed = Mathf.Max(7.5f, speed);
                break;
            case 2:
                backSpeed = Mathf.Max(6.5f, speed);
                break;     
            default:
                backSpeed = speed;
                break;
        }

        //print("DEBUGBACKSPEED NEW BACK SPEED " + backSpeed);

        return backSpeed;
    }
   
}