using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GlobalsNS;
using System.Text.RegularExpressions;
using graphicsCommonNS;
using gameStatisticsNS;
using System.IO;
using System;
using System.Linq;

using MenuCustomNS;
using GeometryCommonNS;
using UnityEngine.Analytics;
using TMPro;
using AudioManagerNS;
using Photon.Pun;
using System.Text;

using LANGUAGE_NS;


/*TOCOMMENT*/
//using UnityEngine.Profiling;

public enum SHOTVARIANT
{
    STRAIGHT = 1,
    CURVE = 2
}

public class controllerRigid : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject playerUp;
    public GameObject playerUpGameObject;
    public setTextures setTextureScript;
    public gamePausedMenu gamePausedScript;
    protected Rigidbody[] ballRb;
    protected GameObject[] ballRbLeftSide;
    protected GameObject[] ballRbRightSide;
    bool running;
    private Animator animator;
    private Camera m_MainCamera;
    private Camera cameraComp;
    private FixedJoint fixedJoint;
    public joystick1 joystick;
    private RectTransform joystickBG;
    public GameObject joystickBgGameObject;
    private RectTransform joystickButton;
    private GameObject joystickButtonGameObject;
    private float joystickScreenOffset;
    private Vector3 specialButtonsScreenOffset;
    private Vector2 specialPowersScreenOffset;
    public Powers powersScript;

    public buttonVolley volleyButton;
    public buttonLob lobButton;
    public buttonCamera cameraButton;
    private GameObject volleyButtonTextGameObject;
    private GameObject lobButtonTextGameObject;
    private GameObject cameraButtonTextGameObject;
    public training trainingScript;
    public BonusRounds bonusScript;
    private float lastDistFromMidLine;
    private bool isPlayerFreeze = false;
    private bool isExtraGoals = false;
    //public buttonOverHead overheadButton;
    /*Canvas game objects */
    //private GameObject overheadButtonGameObject;
    private GameObject pauseCanvas;
    public GameObject lobButtonGameObject;
    public GameObject volleyButtonGameObject;
    public GameObject cameraButtonGameObject;
    public GameObject timePanelGameObject;

    public GameObject powerButton1GameObject;
    public GameObject powerButton2GameObject;
    public GameObject powerButton3GameObject;

    public GameObject timeImageGameObject;
    public GameObject timeToShotBallImageGameObject;

    private bool isPowerEnable = true;

    public GameObject mainTimeTextGameObject;
    private GameObject flagPanelGameObject;
    public GameObject joystickGameObject;
    private GameObject matchIntroPanel;
    private GameObject matchIntroFlagPanel;
    private GameObject matchIntroPanelBackground;
    private GameObject matchIntroPanelHeaderTop;
    private GameObject matchIntroPanelHeaderDown;
    //private Text matchIntroWinCoinsNumText;
    //private Text matchIntroTieNumCoinText;
    private TextMeshProUGUI matchIntroWinCoinsNumText;
    private TextMeshProUGUI matchIntroTieNumCoinText;
    private float BALL_RADIUS = 0.35f;
    private float BALL_NEW_RADIUS = 0.2585f;
    private float DEFAULT_CAMERA_Y_POS = 4.4f;
    private GameObject shotBarGameObject;
    private GameObject shotBarIconGameObject;
    public GameObject shotBarBackground;
    //public GameObject refereeBackground;
    private float lastTimePlayerOnBall = -1f;
    private bool isPlayerNowOnBall = false;
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
    private Vector3 extraShotVec = Vector3.zero;
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
    private bool secondShotLineDone = false;
    private bool updateTouchEndFinished = false;
    private bool activeShot = false;
    private bool shotLock;
    private int width, height;
    private int screenWidth, screenHeight;
    float drawDistance = 0.0f;
    float drawTimeStart;
    float drawTimeEnd;
    private bool touchLocked;
    private float delayAfterGoal;
    public ballMovement[] ball;
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
    public CpuPlayer cpuPlayer;
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
    private RawImage gkHelperImage;
    private Texture gkHelperImageOutlineTexture;
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
    private float timeofBallFlyOrg = 500f;
    private float shotSpeed;
    private float ShotSpeedMax = 1300.0f;
    private float ShotSpeedMin = 420.0f;
    /*private float ShotCurveSpeedMinTime = 650.0f;
    private float ShotCurveSpeedMaxTime = 1500.0f;*/
    private float MIN_SHOT_SPEED = 50.0f;
    private float MAX_SHOT_SPEED = 120f;
    private float MAX_SHOT_SPEED_UNITY_UNITS = 34.5f;
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
    private int minuteOfMatch = 0;

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
    /*private GameObject playerUpLeftHandFingers;
    private GameObject playerUpRightHandFingers;
    private GameObject playerDownLeftHandFingers;
    private GameObject playerDownRightHandFingers;*/
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject cpuLeftHand;
    private GameObject cpuRightHand;

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
    private float maxTimeToShot = 8f;
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
    private float lastTimeBallWasOut = -1f;
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
    private bool introAnimPlayed = false;

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
    public int FANS_FLAG_MAX = 6;
    private Vector3[] fansFlagAngles;
    private Vector3[] fansFlagDirections;
    private bool[] isFansFlagActive;
    private Vector3 stepSideAnimOffset = Vector3.zero;
    private bool gkLobPointReached = false;
    private bool gkRunPosReached = false;
    private float initDistX = -1f;
    public PlayerUpRigidBody playerUpRigidBody;
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
    private GameObject tmpRotGO;

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

    private int NUMBER_OF_BALLS = 1;
    private int MAX_NUMBER_OF_BALLS = 1;
    private int activeBall = 1;

    private int numberOfCorrectGKClick = 0;
    private float numberOfCorrectGkclickLastTimeUpdate = 0;
    private bool isGkHelperImageRecovered = false;

    private bool isBonusActive = false;
    private float shotRotationDelay = 0f;
    private bool initIntroNewCameraPos = false;

    public float ballShotVelocity = 20f;

    public Vector3 playerPrevPos;
    public float playerPrevPosTime;
    public float playerVelocity = 0f;
    //private bool gkHelperImageErased = true;
    //private GameObject MarkerBasic;
    public audienceReactions audienceReactionsScript;
    private float curveShotFlyPercent = 0f;
    private float prepareShotDelay = 0.35f;
    private bool isPrepareShotDelay = false;

    StringBuilder minutesTime = new StringBuilder("");
    StringBuilder secondsTime = new StringBuilder("");
    StringBuilder mainTimeStr = new StringBuilder("");

    private bool autoMode_gkRunPosReached = false;
    private Vector3 autoModGkPos;
    Material secondShotLineMaterial;

    public GameObject levelEndPanel;
    public TextMeshProUGUI levelEndHeaderText;
    public RawImage levelEndImage;
    private bool showlevelEndPanelDone = false;

    void Awake()
    {
        if (!Globals.isLevelMode)
            currentTimeOfGame = 0;
        else
            currentTimeOfGame = Globals.levelModeTimeOffset;

        if (levelEndPanel != null)
            levelEndPanel.SetActive(false);

        autoModGkPos = new Vector3(0f, 0f, -PITCH_HEIGHT_HALF + 0.2f);

        m_MainCamera = Camera.main;
        cameraComp = m_MainCamera.GetComponent<Camera>();

        if (Globals.isMultiplayer)
        {
            GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>().stop();
        }

        //print("GETQUALITY RESULTS " + QualitySettings.GetQualityLevel());
        multiplayerSettings();
        levelsSettings();

        numberOfCorrectGkclickLastTimeUpdate = Time.time;

        fansFlagAngles = new Vector3[FANS_FLAG_MAX];
        fansFlagDirections = new Vector3[FANS_FLAG_MAX];
        isFansFlagActive = new bool[FANS_FLAG_MAX];

        initBonuses();
        initPowers();

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
        }
        else
        {
            teamHostID = 1;
        }

        //teamHostID = UnityEngine.Random.Range(1, 3);
        //if (Globals.playerPlayAway)
        //{
        //    teamHostID = 1;
        //}

        ballRb = new Rigidbody[NUMBER_OF_BALLS + 1];
        ballRbLeftSide = new GameObject[NUMBER_OF_BALLS + 1];
        ballRbRightSide = new GameObject[NUMBER_OF_BALLS + 1];

        ball = new ballMovement[NUMBER_OF_BALLS + 1];

        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {
            ballRb[i] = GameObject.Find("ball" + i.ToString()).GetComponent<Rigidbody>();
            ball[i] = GameObject.Find("ball" + i.ToString()).GetComponent<ballMovement>();
            ballRbLeftSide[i] = GameObject.Find("ball" + i.ToString() + "LeftSide");
            ballRbRightSide[i] = GameObject.Find("ball" + i.ToString() + "RightSide");
        }

        for (int i = NUMBER_OF_BALLS + 1; i <= MAX_NUMBER_OF_BALLS; i++)
        {
            GameObject.Find("ball" + i.ToString()).SetActive(false);
        }

        cpuLeftPalm = GameObject.FindWithTag("playerUpLeftPalm");
        cpuRightPalm = GameObject.FindWithTag("playerUpRightPalm");


        /*playerUpLeftHandFingers = GameObject.Find("playerUpLeftHandFingers");
        playerUpRightHandFingers = GameObject.Find("playerUpRightHandFingers");
        playerDownLeftHandFingers = GameObject.Find("playerDownLeftHandFingers");
        playerDownRightHandFingers = GameObject.Find("playerDownRightHandFingers");*/

        cpuPlayer = new CpuPlayer(this, ball[1]);

        setupCanvasGameObjects();

    }


    void Start()
    {
        //print("DBG12345 Globals.level " + Globals.level);

        if (!Globals.isMultiplayer)
        {
            maxTimeToShot =
                float.Parse(Regex.Replace(Globals.maxTimeToShotStr, @"[^\d]", ""));

            if (Globals.level == Globals.MIN_LEVEL && !Globals.isLevelMode)
                maxTimeToShot = 20;
        }

        goalResize(true);

        extraShotVec = new Vector3(
            UnityEngine.Random.Range(4.3f, 4.4f),
            UnityEngine.Random.Range(2.7f, 2.8f),
            -PITCH_HEIGHT_HALF);

        //Application.targetFrameRate = 30;
        //QualitySettings.vSyncCount = 0;
        //QualitySettings.antiAliasing = 0;

        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        gkCornerPoints = new Vector3(PITCH_WIDTH_HALF, 0f, -PITCH_HEIGHT_HALF);
        dummyTouchRotatedGO = new GameObject("touchRotatedGO");

        if (!isBonusActive)
            Globals.numMatchesInThisSession++;

        /*moved to setTextures*/
        //updateTrainingSettings();

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        //print("SCREENWIDTH " + Screen.width + " ScreenHeight " + Screen.height);

        //rateScreenWidth = Globals.OrginalScreenWidth / (float) (Screen.width);
        //rateScreenHeight = Globals.OrignalScreenHeight / (float) (Screen.height);

        //print("RESOLUTIONS SETUP WIDTH "
        //    + Screen.width + "x" + Screen.height);

        setupLevelDependentVariables();
        midTouchPos = new Vector2[MID_MAX_POS];

        lastPlayerMovePos = new Vector3[40, 2];
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

        gkHelperImage = GameObject.Find("gkHelper").GetComponent<RawImage>();

        //gkHelperImageOutlineTexture= new Texture();
        gkHelperImageOutlineTexture =
            Resources.Load<Texture2D>("GkImages/circle-inside-outline");

        //print("Globals.numMainMenuOpened " + PlayerPrefs.GetInt("numGameOpen")
        //    + " Globals.numMatchesInThisSession " + Globals.numMatchesInThisSession);

        if ((PlayerPrefs.GetInt("numGameOpen") <= 2) &&
            (Globals.numMatchesInThisSession <= 2))
        {
            Color temp = gkHelperImage.color;
            temp.a = 0.7f;
            gkHelperImage.color = temp;

            gkHelperImage.texture = Resources.Load<Texture2D>("GkImages/circleTurtorial");
        }

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

        setJoystickPosition();
        setSpecialButtonsPosition();

        rotatedRbToBall = new GameObject();
        tmpRotatedRbToBall = new GameObject();
        tmpRotGO = new GameObject();


        if (!Globals.isLevelMode)
        {
            Globals.score1 = 0;
            Globals.score2 = 0;
        }

        score1Text = GameObject.Find("scoreText_1").GetComponent<TextMeshProUGUI>();
        score2Text = GameObject.Find("scoreText_2").GetComponent<TextMeshProUGUI>();

        mainTimeText = GameObject.Find("mainTimeText").GetComponent<TextMeshProUGUI>();
        timeToShotText = GameObject.Find("timeToShotText").GetComponent<TextMeshProUGUI>();

        //gameEventMsgText = GameObject.Find("gameEventMsgText").GetComponent<TextMeshProUGUI>();

        //gameInfoImageGameObject = GameObject.Find("gameInfoImage");
        //gameInfoImage = gameInfoImageGameObject.GetComponent<RawImage>();
        shotBar = GameObject.Find("shotBar").GetComponent<Image>();

        speedShotText = GameObject.Find("speedShotText").GetComponent<TextMeshProUGUI>();

        rbRightFoot = GameObject.FindWithTag("playerDownRightLeg");
        rbRightToeBase = GameObject.FindWithTag("playerDownRightToeBase");
        rbLeftToeBase = GameObject.FindWithTag("playerDownLeftToeBase");
        rbHead = GameObject.FindWithTag("playerDownHead");


        leftPalm = GameObject.FindWithTag("playerDownLeftPalm");
        rightPalm = GameObject.FindWithTag("playerDownRightPalm");

        leftHand = GameObject.FindWithTag("playerDownLeftHand");
        rightHand = GameObject.FindWithTag("playerDownRightHand");

        cpuLeftHand = GameObject.FindWithTag("playerUpLeftHand");
        cpuRightHand = GameObject.FindWithTag("playerUpRightHand");

        //Color goalDownNetColor = GameObject.Find("goalDownNet").GetComponent<Renderer>().material.color;
        //goalDownNetColor.a = 0.3f;

        matchStatistics = new MatchStatistics();

        //GraphicsCommon graphics = new GraphicsCommon();

        //string[] flagsNames = new string[] { Regex.Replace(Globals.teamAname, "\\s+", "").ToLower(),
        //                                     Regex.Replace(Globals.teamBname, "\\s+", "").ToLower()};


        Debug.Log("Globals.teamAname " + Globals.teamAname + " teamB " + Globals.teamBname);


        string[] flagsNames = new string[] { Globals.teamAname,
                                             Globals.teamBname};
        RawImage flagGameImage;
        for (int i = 0; i < flagsNames.Length; i++)
        {
            //GameObject.Find("flagImage_" + (i + 1).ToString()).GetComponent<RawImage>().texture =
            //    graphics.getTexture("Flags/" + graphics.getFlagPath(leagueName, flagsNames[i]));
            flagGameImage =
                GameObject.Find("flagImage_" + (i + 1).ToString()).GetComponent<RawImage>();

            graphics.setFlagRawImage(flagGameImage,
                                     graphics.getFlagPath(flagsNames[i]));
        }

        graphics.setFlagRawImage(teamAflagIntro,
            graphics.getFlagPath(Globals.teamAname));

        //graphics.getFlagPath(leagueName, Globals.teamAname));

        graphics.setFlagRawImage(teamBflagIntro,
           graphics.getFlagPath(Globals.teamBname));

        //graphics.getFlagPath(leagueName, Globals.teamBname));

        teamAIntroText.text = Globals.teamAname;
        teamBIntroText.text = Globals.teamBname;

        //calculateAndSetWinTieMatchIntroValues(Globals.teamAcumulativeStrength,
        //                                      Globals.teamBcumulativeStrength);
        calculateAndSetWinTieMatchIntroValues(cumulativeStrengthPlayer,
                                              cumulativeStrengthCpu);


        //print("DEGUBX1 teamA name cumulative " + Globals.teamAname + " value " + Globals.teamAcumulativeStrength);
        //print("DEBUGX1 teamB name cumulative " + Globals.teamBname + " value " + Globals.teamBcumulativeStrength);


        graphics.setFlagRawImage(teamAflagStatisticsImage,
                                 graphics.getFlagPath(Globals.teamAname));

        //graphics.getFlagPath(leagueName, Globals.teamAname));
        graphics.setFlagRawImage(teamBflagStatisticsImage,
                                 graphics.getFlagPath(Globals.teamBname));


        secondShotLineMaterial =
               graphics.getMaterial("others/Material/second_shot_line");

        //graphics.getFlagPath(leagueName, Globals.teamBname));

        teamAStatisticsText.text = Globals.teamAname;
        teamBStatisticsText.text = Globals.teamBname;

        updateStadiumTextures();

        if (Globals.stadiumNumber != 2)
            initFlagPositions();

        setScoresText();
        setTimesText();
        //trailRenderer = GameObject.Find("shotDrawLine").GetComponent<TrailRenderer>();
        //trail = GameObject.Find("shotDrawLine");

        animationOffsetTime = new Dictionary<string, float>();
        animationOffsetTime.Add("3D_shot_left_foot", 0.33f);
        animationOffsetTime.Add("3D_shot_right_foot", 0.33f);
        //animationOffsetTime.Add("3D_volley", 0.43f);

        animationOffsetTime.Add("3D_volley", 0.45f);


        initPreShot = false;
        initVolleyShot = false;

        rb = GetComponent<Rigidbody>();
        touchLine = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        playerPrevPosTime = Time.time;
        playerPrevPos = rb.transform.position;

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
        }
        else
        {
            //timeFactor = 60.0f;
            timeFactor = 38.0f;
        }

        string timeOfGame = Regex.Replace(Globals.matchTime, "[^0-9]", "");
        timeOfGameInSec = float.Parse(timeOfGame) * timeFactor;
        //if (Globals.matchTime.Contains("MINUTES"))
        //{

        //}

        //stoppageTime = UnityEngine.Random.Range(4, 10);
        stoppageTime = UnityEngine.Random.Range(1, 4);

        //TODELETE
        //timeOfGameInSec = 10000f;
        //print("TIMEOFGAME " + timeOfGameInSec);
        //animator = GameObject.Find("mainPlayer1").GetComponent<Animator>();
        animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;

        if (Globals.stadiumNumber == 0)
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y,
                                                          m_MainCamera.transform.position.z);
        //speed = 9.0f;

        passedShotFlyTime = 0.0f;
        startPos = endPos = Vector2.zero;
        baseBallSpeed = 25.0f;
        //addionalBallSpeed = 0;
        initShot = isBallTrailRendererInit = false;
        //maxMovedDot = float.MaxValue;       
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

        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
            ballRb[i].maxAngularVelocity = 1000.0f;

        cameraStartPos = m_MainCamera.transform.rotation;

        gkHelperImage.enabled = false;
        gkClickHelper.enabled = false;
        prevZballPos = ballRb[activeBall].transform.position.z;
        ballPrevPosition = ballRb[activeBall].transform.position;

        //transform.parent = GameObject.Find("mainPlayer1").transform;

        //DrawLine(new Vector3(-22.0f, 0.01f, -1.0f), new Vector3(22.0f, 0.01f, -1.0f), Color.white, 300000.0f, 0.01f);
        /*POSSIBLE TO REMOVE*/

        //DRAW HELPER LINE//        
        //drawHelperGrid();

        //print("ISBONUSACTIVE " + isBonusActive);

        if (isTrainingActive || isBonusActive)
        {
            if (Globals.PITCHTYPE.Equals("GRASS"))
            {
                audioManager.Play("training1");
            }

            if (Globals.stadiumNumber == 2)
                audioManager.PlayNoCheck("training1");
        }
        else
        {
            if (Globals.stadiumNumber == 2)
                audioManager.PlayNoCheck("training1");
            else
                audioManager.Play("fanschantBackground2", 0.3f);
            //if (Globals.stadiumNumber == 1)
            audienceReactionsScript.playApplause1();
        }

        deactivateCanvasElements();

        /*Draw who starts */
        int whoStarts = UnityEngine.Random.Range(0, 2);
        if (whoStarts == 0)
        {
            ballRb[activeBall].transform.position = new Vector3(0, 0, 2);
        }
        else
        {
            ballRb[activeBall].transform.position = new Vector3(0, 0, -2);
        }

        if (isTrainingActive || isBonusActive || Globals.isLevelMode)
        {
            gameStarted = true;
            initCameraMatchStartPos();
            if (Globals.isLevelMode)
                activateCanvasElements();

            //activateCanvasElements();
            
            ballRb[activeBall].transform.position = new Vector3(0, 0, -4);
            if (Globals.isLevelMode)
            {
                int levelNumber = Globals.levelNumber;
                int level_pos_offset = levelNumber % 5;
                float level_pos_offset_x = level_pos_offset;
                if (levelNumber % 3 == 0 || levelNumber % 5 == 0)
                    level_pos_offset_x = -level_pos_offset_x;

                rb.transform.position = new Vector3(level_pos_offset_x * 1.3f, 0, Mathf.Clamp(-12.5f + level_pos_offset * 1.3f, -12.5f, -4.0f));
                //Debug.Log("#rbtransformposition " + rb.transform.position + " levelNuymber " + Globals.levelNumber + " value " + (-12.5f + level_pos_offset * 1.3f));
                cpuPlayer.setRbPosition(new Vector3(-level_pos_offset_x * 0.8f, 0f, Mathf.Clamp(level_pos_offset * 3, 3f, 12.5f)));

                if (levelNumber % 2 == 0 || levelNumber % 5 == 0)
                    ballRb[activeBall].transform.position = new Vector3(level_pos_offset_x * 1.1f, 0, Mathf.Clamp(-level_pos_offset * 1.3f, -10f, -1f));
                else
                    ballRb[activeBall].transform.position = new Vector3(level_pos_offset_x * 1.1f, 0, Mathf.Clamp(level_pos_offset * 1.3f, 1f, 10f));


            }
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        prevRbPos = rb.transform.position;
        matchSavePos = rb.transform.position;
        matchInitSavePos = false;
        //print("maxTimeToShotDBG2 START" + maxTimeToShot);

        Analytics.enabled = true;

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

        //playerUpGameObject = GameObject.Find("playerUp");
        //Debug.Log("playerUpGameObject " + playerUpGameObject);
        //if (playerUpGameObject != null)
        //    playerUpGameObject.transform.position = new Vector3(0f, 0.03f, -100f);
    }

    //int deltaIterator = 1;

    void Update()
    {

        if (Globals.isPhotoModeEnable)
        {
            if (Input.GetKeyDown("space"))
            {
                int rand = UnityEngine.Random.Range(0, 100000000);
                ScreenCapture.CaptureScreenshot("screenshot/screenshot" + rand.ToString() + ".png", 4);
                Debug.Log("A screenshot was taken!");
            }
        }

        if (Globals.isMultiplayer &&
            !PhotonNetwork.IsConnected)
            return;
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


        if (!isTrainingActive && !isBonusActive)
            updateFlagsPositions();

        realTime += Time.deltaTime;
        if (gameEnded)
        {
            rb.velocity = Vector3.zero;
            if (!Globals.isLevelMode)
                displayStatisticsPanel();
            else
            {
                if (!showlevelEndPanelDone)
                    StartCoroutine(showLevelEndPanel(2.5f));
                showlevelEndPanelDone = true;

            }

            gamEndAnimations();
            if (realTime < 1f && Globals.score1 >= Globals.score2)
                if (!isTrainingActive && !isBonusActive)
                {
                    audioManager.Play("crowdOvation1Short");

                    ///if (Globals.stadiumNumber == 1)
                    audienceReactionsScript.playCelebration1();
                }
            return;
        }

        if (!gameStarted)
        {
            //if (realTime > 1f)
            //{
            cameraMovementIntro();
            //}

            if (realTime > 10f)
            {
                playerUp.transform.position = new Vector3(0f, 0.03f, 11f);
                audioManager.PlayNoCheck("whislestart1");
                int RandWhistleCom = UnityEngine.Random.Range(1, 3);
                if (!Globals.commentatorStr.Equals("NO"))
                    audioManager.Play("com_firstwhistle" + RandWhistleCom.ToString());
                gameStarted = true;
                lastTimeBallWasOut = Time.time;
            }

            float matchIntroPanelFillAmount = (realTime - 1.0f) / 2.0f;
            if ((realTime >= 1.0f && realTime < 3.0f) || Globals.isLevelMode)
            {
                int chantRandom = UnityEngine.Random.Range(3, 5);
                if (!gameStartedInit)
                {
                    if (!Globals.isLevelMode)
                        audioManager.Play("fanschant" + chantRandom.ToString(), 0.26f);
                    else
                        audioManager.Play("fanschant" + chantRandom.ToString(), 0.26f);

                    int randIntrCom = UnityEngine.Random.Range(1, 4);
                    if (!Globals.commentatorStr.Equals("NO"))
                        audioManager.Play("com_intro" + randIntrCom.ToString());
                }
                //if (Globals.stadiumNumber == 1)
                // {
                //     if (chantRandom == 3) audienceReactionsScript.playApplause2();
                //    if (chantRandom == 4) audienceReactionsScript.playCelebration2();
                // }

                gameStartedInit = true;
                matchIntroPanel.SetActive(true);
                matchIntroPanel.GetComponent<Image>().fillAmount = matchIntroPanelFillAmount;
            }
            else
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

            if (realTime > 8.0f)
            {
                initCameraMatchStartPos();
                activateCanvasElements();
                rb.transform.position = new Vector3(0f, 0.03f, -12.5f);
                animator.speed = 1f;
            }

            return;
        }

        /*if updateGameTime is true - begining of match end */
        if (!isTrainingActive && !isBonusActive)
        {
            if (updateGameTime())
            {
                addCoins();
                audioManager.PlayNoCheck("whisleFinal1");
                realTime = 0.0f;
                gameEnded = true;
                rb.velocity = Vector3.zero;
                if (Globals.isAnalyticsEnable)
                {

                    Analytics.CustomEvent("Game_Ended", new Dictionary<string, object>
                    {
                        { "level", Globals.level},
                        { "teamAscore", Globals.score1},
                        { "teamBscore", Globals.score2 }
                    });
                }
            }
        }

        timeLoops++;

        cpuPlayer.update();
        matchTarget(animator,
                    rb,
                    ref gkStartPos,
                    gkTimeToCorrectPos,
                    stepSideAnimOffset,
                    ref matchSavePos,
                    ref matchInitSavePos,
                    false);

        //drawSecondShotHelperLine();

        if (!isTrainingActive && !isBonusActive)
        {
            /*Special case when you are winning with high score */
            if (setTextureScript.isFlareEnable())
            {
                int randChant = UnityEngine.Random.Range(3, 5);
                if (!audioManager.isPlayingByName("fanschant3") &&
                    !audioManager.isPlayingByName("fanschant4"))
                {
                    audioManager.Play("fanschant" + randChant.ToString(), 0.32f);
                }
                //audioManager.Play("crowdBassDrum");
            }
            else
            {
                if (timeLoops % 1000 == 0)
                {
                    int randChant = UnityEngine.Random.Range(0, 4) + 1;
                    if (randChant == 2)
                        randChant = 4;

                    audioManager.Play("fanschant" + randChant.ToString(), 0.30f);
                    //if (Globals.stadiumNumber == 1)
                    //{
                    if (randChant == 3) audienceReactionsScript.playOneOfApplause(10f, 1.8f);
                    if (randChant == 4) audienceReactionsScript.playCelebration2();
                    //audioManager.Commentator_FantasticFansAtmosphere();
                    //}
                }
            }

            if ((timeLoops % 1400) == 0)
            {
                audioManager.Play("crowdBassDrum");
            }
        }

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying && shotActive == false)
        {
            touchLocked = false;
        }

        float distance = Vector3.Distance(rb.transform.position,
                                         (ballRb[activeBall].transform.position));
        /*Touch should be catch in Update*/
        //if (isBallInGame && goalJustScored == false)
        //{
        //if (distance < 1.5f || cpuPlayer.getShotActive())
        /*that's not 100% true cpuPlayer.getShotActive(!) */
        /*What if ball bounce off a player without shot active? */
        /*TO CHANGE*/

        if ((isPlayerOnBall() || cpuPlayer.getShotActive()) &&
            !isTouchPaused())
        {

            if ((!Globals.isTrainingActive) ||
                (trainingScript.isGkTraining() && cpuPlayer.getShotActive()) ||
                (!trainingScript.isGkTraining() && isPlayerOnBall()))
                updateTouch();
        }

        if (updateTouchEndFinished && !secondShotLineDone)
        {
            drawSecondShotHelperLine();
            secondShotLineDone = true;
        }

        clearTouch();
        if ((numberOfCorrectGKClick >= 5) &&
            !isGkHelperImageRecovered &&
            (gkHelperImage.enabled == false))
        {
            //gkHelperImage.texture =
            //    Resources.Load<Texture2D>("GkImages/circle-inside-outline");
            gkHelperImage.texture = gkHelperImageOutlineTexture;
            Color temp = gkHelperImage.color;
            temp.a = 0.352f;
            gkHelperImage.color = temp;
            isGkHelperImageRecovered = true;
        }


        //rotateHead(rb, rbHead, ball.transform.position, cpuPlayer.getShotActive(), 5.0f);

        //if (onBall == PlayerOnBall.ISGK && !shotActive && !preShotActive)
        if (cpuPlayer.getShotActive() && gkTouchDone == true && !shotActive && !preShotActive)
        {
            //print("DEBUG4XYZ GKMOVES EXECUTED");

            gkMoves(animator,
                    rb,
                    false,
                    ref lastGkAnimName,
                    ref lastTimeGkAnimPlayed,
                    ref lastGkDistX,
                    ref gkStartPos,
                    ref gkStartTransform,
                    ref gkTimeToCorrectPos,
                    ref initCpuAdjustAnimSpeed,
                    ref initGkDeleyLevel,
                    ref levelDelay,
                    ref initAnimName,
                    ref cpuGkAnimAdjustSpeed,
                    ref gkAction,
                    ref gkTimeLastCatch,
                    cpuPlayer.isLobShotActive(),
                    ref stepSideAnimOffset,
                    ref gkLobPointReached,
                    ref gkRunPosReached,
                    ref initDistX,
                    cpuPlayer.getShotVariant(),
                    cpuPlayer.getOutShotStart(),
                    cpuPlayer.getOutShotMid(),
                    cpuPlayer.getOutShotEnd(),
                    cpuPlayer.getEndPosOrg(),
                    cpuPlayer.getTimeOfBallFly(),
                    cpuPlayer.getPassedTime(),
                    ref gkLock,
                    ref rotatedRbToBall,
                    gkCornerPoints,
                    isExtraGoals);
        }
        
    }

    void drawSecondShotHelperLine()
    {
        //if (!shotActive || !initShot)
        //    return;

        float currentTime = 0.05f;
        Vector3 m1, m2, currPos;
        ColorUtility.TryParseHtmlString("#89CFF0", out Color lineColor);
        //ColorUtility.TryParseHtmlString("#89CFF0", out Color lineColor);
        lineColor.a = 0.0f;
        updateShotPos();

        preShotCalc(curveStartPos3,
                    curveMidPos3,
                    curveEndPos3,
                    endPosOrg,
                    height,
                    ballRb[activeBall].transform.position,
                    shotSpeed,
                    isLobActive,
                    ref outShotBallVelocity,
                    ref outShotStart,
                    ref outShotMid,
                    ref outShotEnd,
                    ref shotvariant,
                    false);
        Vector3 prevPos = outShotStart;
        Debug.Log("TouchCount curveStartPos3 " + outShotStart
            + " curveMidPos3 " + outShotMid
            + " curveEndPos3 " + outShotEnd);


        while (currentTime < 1.0f)
        {
            currentTime += 0.033f;
            //m1 = Vector3.Lerp(outShotStart, outShotMid, currentTime);
            //m2 = Vector3.Lerp(outShotMid, outShotEnd, currentTime);
            //currPos = Vector3.Lerp(m1, m2, currentTime);
            m1 = Vector3.Lerp(outShotStart, outShotMid, currentTime);
            m2 = Vector3.Lerp(outShotMid, outShotEnd, currentTime);
            currPos = Vector3.Lerp(m1, m2, currentTime);

            DrawLine(prevPos, currPos, ref secondShotLineMaterial, lineColor, 0.8f, 0.2f);
            prevPos = currPos;
        }
    }

    private void photo_3DVolley()
    {
        ballRb[activeBall].transform.position = new Vector3(0f, 1.5f, -4f);
        if (!isPlaying(animator, "3D_volley", 1f))
        {
            animator.speed = 0.001f;
            animator.Play("3D_volley", 0, 0.45f);
            animator.Update(0f);
            rb.transform.position = new Vector3(-3f, 0f, -6f);
            ballRb[activeBall].velocity = Vector3.zero;

            RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            RblookAtDirection(cpuPlayer.getPlayerRb(),
                              rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
                              100f);
        }
    }

    private void photo_save1()
    {
        ballRb[activeBall].transform.position = new Vector3(-4f, 2.5f, -8f);
        ballRb[activeBall].velocity = Vector3.zero;

        if (!isPlaying(animator, "3D_GK_sidecatch_left_up", 1f))
        {
            animator.speed = 0.001f;
            animator.Play("3D_GK_sidecatch_left_up", 0, 0.05f);
            animator.Update(0f);
            rb.transform.position = new Vector3(0f, 0f, -10.5f);
            rb.velocity = Vector3.zero;

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            //RblookAtDirection(cpuPlayer.getPlayerRb(),
            //                  rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
            //                  100f);

            RblookAtDirection(cpuPlayer.getPlayerRb(),
                              rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
                              100f);
        }

        Animator cpuAnim = cpuPlayer.getAnimator();
        cpuPlayer.setAnimatorSpeed(0.001f);
        if (!isPlaying(cpuAnim, "3D_volley", 1f))
        {
            cpuAnim.speed = 0.001f;
            cpuAnim.Play("3D_volley", 0, 0.45f);
            cpuAnim.Update(0f);
            cpuPlayer.getPlayerRb().transform.position = new Vector3(-3f, 0f, 5f);

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            RblookAtDirection(cpuPlayer.getPlayerRb(),
                              rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
                              100f);
        }
    }

    private void photo_save2()
    {
        ballRb[activeBall].transform.position = new Vector3(-3f, 3.1f, 10f);
        ballRb[activeBall].velocity = Vector3.zero;
        Animator cpuAnimator = cpuPlayer.getAnimator();

        if (!isPlaying(animator, "3D_GK_sidecatch_rightpunch_up", 1f))
        {
            cpuAnimator.speed = 0.001f;
            cpuAnimator.Play("3D_GK_sidecatch_rightpunch_up", 0, 0.16f);
            cpuAnimator.Update(0f);
            cpuPlayer.getPlayerRb().transform.position = new Vector3(0f, 0f, 10.5f);

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            //RblookAtDirection(cpuPlayer.getPlayerRb(),
            //                  rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
            //                  100f);

            RblookAtDirection(cpuPlayer.getPlayerRb(),
                              rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
                              100f);
        }

        if (!isPlaying(animator, "3D_shot_right_foot", 1f))
        {
            animator.speed = 0.001f;
            animator.Play("3D_shot_right_foot", 0, 0.40f);
            animator.Update(0f);
            rb.transform.position = new Vector3(5f, 0f, -8f);

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            RblookAtDirection(rb,
                              cpuPlayer.getPlayerRb().transform.position - rb.transform.position,
                              100f);
        }
    }

    private void photo_save3()
    {
        ballRb[activeBall].transform.position = new Vector3(-4.5f, 0.3f, -12f);
        ballRb[activeBall].velocity = Vector3.zero;
        Animator cpuAnimator = cpuPlayer.getAnimator();

        if (!isPlaying(animator, "3D_GK_sidecatch_leftpunch_down", 1f))
        {
            animator.speed = 0.001f;
            animator.Play("3D_GK_sidecatch_leftpunch_down", 0, 0.14f);
            animator.Update(0f);
            rb.transform.position = new Vector3(0f, 0f, -13.5f);

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            //RblookAtDirection(cpuPlayer.getPlayerRb(),
            //                  rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
            //                  100f);

            //RblookAtDirection(cpuPlayer.getPlayerRb(),
            //                  rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
            //                  100f);
        }

        if (!isPlaying(animator, "3D_shot_right_foot", 1f))
        {
            cpuAnimator.speed = 0.001f;
            cpuAnimator.Play("3D_shot_right_foot", 0, 0.40f);
            cpuAnimator.Update(0f);
            cpuPlayer.getPlayerRb().transform.position = new Vector3(-5f, 0f, 5f);

            //RblookAtDirection(rb, new Vector3(0f, 0f, 14f) - rb.transform.position, 100f);
            RblookAtDirection(cpuPlayer.getPlayerRb(),
                              rb.transform.position - cpuPlayer.getPlayerRb().transform.position,
                              100f);
        }
    }

    // every 2 seconds perform the print()
    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            drawSecondShotHelperLine();
        }
    }


    void FixedUpdate()
    {
        /*if (Globals.isPhotoModeEnable) {
            if (!gameStarted)
            {
                return;
            }

            //photo_3DVolley();
            //photo_save1();
            //photo_save2();
            //photo_save3();

            //cpuPlayer.fixedUpdate();
            //return;
        }*/


        curveStartPos3 = new Vector3(0, 0, -14f);
        curveMidPos3 = new Vector3(0, 3f, 0f);
        curveEndPos3 = new Vector3(0, 0, 14f);
        endPosOrg = new Vector3(0, 0, 14f);



        //StartCoroutine(WaitAndPrint(5.0f));


        if (Globals.isMultiplayer &&
            !PhotonNetwork.IsConnected)
            return;
        //print("ActiveBall " + activeBall);

        //print("ballRb[activeBall]POSITION " + ballRb[activeBall].transform.position.ToString("F6"));

        //print("RBVEL " + rb.velocity);
        if (isGamePaused())
            return;

        updatePlayerVelocity();

        //print("isBallInGame " + isBallInGame + " isballOUt " + isBallOut);

        //string name = nameAnimationPlaying(animator, 1.0f);
        //bool isBackRunPlaying = isPlaying(animator, "3D_back_run_cpu", 1.0f);
        /// bool isRunPlaying = isPlaying(animator, "3D_run", 1.00f);

        /*if (name == string.Empty)
        {
            if (isBackRunPlaying)
                name = "3D_back_run_cpu";
            if (isRunPlaying)
                //name = "3D_run_cpu";
                name = "3D_run";
        }

        if (name != string.Empty)
        {
        if (isRunPlaying)
            name = "3D_back_run_cpu";

          if (shotActive || preShotActive)
           {
               print("DEBUG2345ANIMPLAY PLAYINGANIMATIONOW " + name + "  "
                   + animator.GetCurrentAnimatorStateInfo(0).IsName(name) + " ANIMNORMTIME "
                   + animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                   + " BALLPOS " + ballRb[activeBall].transform.position
                   + " RB POS " + rb.transform.position
                   + " preShotActive " + preShotActive
                   + " isShotActive " + shotActive
                   + " cpuPlayer.getRbPosition() " + cpuPlayer.getRbPosition());
           }
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
                              ballRb[activeBall],
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

        Vector2 ballPos = new Vector2(ballRb[activeBall].transform.position.x, ballRb[activeBall].transform.position.z);
        Vector2 cpuPos = new Vector2(cpuPlayer.getRbPosition().x,
                                     cpuPlayer.getRbPosition().z);

        if (!cpuPlayer.getShotActive())
            gkLock = false;

        recoverAnimatorSpeed();

        if (!gameStarted)
        {
            return;
        }

        isFixedUpdate++;

        /*matchTarget(animator,
                  rb,
                  ref gkStartPos,
                  gkTimeToCorrectPos,
                  stepSideAnimOffset,
                  ref matchSavePos,
                  ref matchInitSavePos,
                  false);*/

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
                             ballRb[activeBall],
                             shotType,
                             ref prevRbPos,
                             false);

            return;
        }

        if ((!isTrainingActive && !isBonusActive) &&
            !timeToShotExceeded)
            playMissGoalSound();

        /*Second condition will help when you shot and ball will not cross half*/

        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {

            if (ball[i].getBallCollided() ||
                (shotActive &&
                (Mathf.Abs(ballRb[activeBall].transform.position.y) < 0.45f) &&
                (Mathf.Abs(ballRb[activeBall].velocity.x) < 0.05f) &&
                (Mathf.Abs(ballRb[activeBall].velocity.y) < 0.05f) &&
                (Mathf.Abs(ballRb[activeBall].velocity.z) < 0.05f)))
            {

                /*print("TOUCHPOSITION getBallCollided " + ball.getBallCollided() + " X " + Mathf.Abs(rb.velocity.x) +
                   " Y " + Mathf.Abs(rb.velocity.y) + " Z " + Mathf.Abs(rb.velocity.z));*/
                //print("GKDEBUG800 PLAYINGANIMATIONOW CLEAR SHOT ACTIVE VELOCITY" + ballRb[activeBall].velocity.ToString("F10"));
                //print("GKDEBUG800 " + ball.getBallCollided());

                clearAfterBallCollision();
                ball[i].setBallCollided(false);

                //print("GKDEBUG1TEST1 SHOTACTIVE " + shotActive + " PRESHOT " + preShotActive);

                if (ball[i].getBallCpuPlayerStatus() == false)
                {
                    //print("POINT TO GO COMPUTER 1");
                    cpuPlayer.setShotActive(false);
                }
                else
                {
                    ball[i].setBallCpuPlayerCollision(false);
                }
            }
        }

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY BEFORE CPU FIXED UPDATE " + ballRb[activeBall].velocity + " SHOT " + shotActive);

        cpuPlayer.fixedUpdate();
        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER CPU FIXED UPDATE " + ballRb[activeBall].velocity + " SHOT " + shotActive);

        //prepareFrameClean();

        //print("UNLOCK TOUCH COUNT " + touchCount);

        if (!isBonusActive)
            timeToShotExceeded = updateTimeToShot(ref prevZballPos, ref timeToShot);

        if (!isBonusActive &&
            (isBallOutOfPitch() ||
            ball[1].getBallGoalCollisionStatus() ||
            (timeToShotExceeded && !isTrainingActive)))
            isBallOut = true;

        /*print("Executed setBallPositionFlash AFTER DELAY IS BALL OUT " 
            + isBallOutOfPitch() + " BALLCOLLISION " + ball.getBallGoalCollisionStatus());*/

        if (isBallOut)
        {
            if (delayAfterGoal <= 1.8f)
            //if (delayAfterGoal <= 2.5f)
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
                    }
                    else
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
                           ballRb[activeBall],
                           shotType,
                           ref prevRbPos,
                           false);

                //playerOutOfPitch(animator, rb, ref prevRbPos, rbLeftToeBase, rbRightToeBase, isPlayerOnBall(), false);
            }
            else
            {
                //clearVariables();
                if (Globals.isTrainingActive || isBonusActive)
                {
                    clearVariables();
                    ballPrevPosition = ballRb[activeBall].transform.position;
                    ball[1].setBallGoalCollisionStatus(false);
                }
                else
                {
                    if (timeToShotExceeded)
                    {
                        if (ball[1].transform.position.z <= 0)
                            ball[1].setwhoTouchBallLast(1);
                        else
                            ball[1].setwhoTouchBallLast(2);

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
                                         (ballRb[activeBall].transform.position));

        //print("GKDEBUG1 DISTANCEHERE " + distance + " preShotActive " + preShotActive + " ShoTACTIVE " + shotActive + 
        //    " RBTRANSFOMR " + rb.transform.position);

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!preShotActive && !shotActive && !isAnyAnimationPlaying)
        {
            //if (distance < minDistToBallShot && !cpuPlayer.getShotActive() && !shotActive && !preShotActive)

            bool isOnBall =
                isPlayerOnBall(rbLeftToeBase, rbRightToeBase, ballRb[activeBall], rb, "move", false, ref activeBall);

            //print("ISONBALL " + isOnBall + " activeBall " + activeBall);

            //print("CPUMOVEDEBUG123X_NOCPU PLAYERONBALL " + isOnBall);
            if (isOnBall &&
               !cpuPlayer.getShotActive())
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

                    if (Globals.isMultiplayer)
                        StartCoroutine(prepShotDelay(prepareShotDelay));

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
                if (gkTouchDone == true && ballRb[activeBall].velocity.z < 0.0f)
                {
                    onBall = PlayerOnBall.ISGK;
                    //print("GKDEBUG1 EXECUTED");
                }
            }

            if (!isOnBall && !cpuPlayer.getShotActive())
            {
                //print("ONBALL NEUTRAL ");
                onBall = PlayerOnBall.NEUTRAL;
            }
        }

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER CPU TOUCH " + ballRb[activeBall].velocity + " SHOTACTIVE " + shotActive);

        if (preShotActive && !shotActive)
        {
            shotActive = prepareShot(animator,
                                     ref shotType,
                                     rb,
                                     ballRb[activeBall],
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
                                     ref shotRotationDelay,
                                     false,
                                     isPrepareShotDelay);

            if (shotActive == true)
            {
                if (isShotOnTarget(endPosOrg, goalSizesCpu))
                {
                    matchStatistics.setShotOnTarget("teamA");
                }
            }
        }

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER PREPARE SHOT " + ballRb[activeBall].velocity + " SHOTACTIVE " + shotActive);

        /*isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (overheadButton.getButtonState() && !isAnyAnimationPlaying)
        {
            //animator.Play("3D_overhead", 0, 0.0f);
            if (!isPlaying(animator, "3D_trick1", 1.0f))
                animator.Play("3D_trick1", 0, 0.0f);
            overheadButton.setButtonState(false);
        }*/

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
                    (cpuPlayer.getShotActive() || ballRb[activeBall].transform.position.z > 0.0f))
                {
                    Vector3 lookPoint =
                            calcGkCorrectRotationToBall(
                                cpuPlayer.getBallInit(),
                                rb,
                                ref rotatedRbToBall,
                                gkCornerPoints);

                    if (cpuPlayer.getShotActive())
                    {
                        RblookAtWSPoint(rb, lookPoint);
                    }
                    else
                    {
                        if (ballRb[activeBall].transform.position.z > 0.0f)
                        {
                            if (powersScript.getIsPlayerUpInvisible() ||
                                powersScript.getIsFlareDownEnable())
                            {
                                lookPoint =
                                    calcGkCorrectRotationToBall(
                                  new Vector3(0f, 0f, PITCH_HEIGHT_HALF),
                                  rb,
                                  ref rotatedRbToBall,
                                  gkCornerPoints);
                            } else
                            {
                                lookPoint =
                                calcGkCorrectRotationToBall(
                                    ball[1].transform.position,
                                    rb,
                                    ref rotatedRbToBall,
                                    gkCornerPoints);
                            }


                            RblookAtWSPoint(rb, lookPoint);
                        }
                    }
                }
                else
                {
                    /*volley has a separeate rotation*/
                    if (!isPlaying(animator, "3D_volley", 1.0f) &&
                        !isPlaying(animator, "3D_volley_before", 1.0f))
                    {
                        //
                        //if (!isBonusActive)
                        //{
                        if (!bonusScript.isRotationLocked())
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
                        //}
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

                //print("DEBUG123CDALOCERROR ballRb[activeBall].position " + ballRb[activeBall].position
                //    + " transform " + ballRb[activeBall].transform.position);

                ballInitPos = ballRb[activeBall].transform.position;
                initShot = true;

                float deltaTime = drawTimeEnd - drawTimeStart;

                /*Speed of ball beetween 0 and ShotSpeedMax */
                timeofBallFly = (drawDistance / deltaTime) / 0.85f;
                //print("DEBUG1234 TIMEOFBALL SPEED " + timeofBallFly);

                //TOUNCOMENTT
                //timeofBallFly = ShotSpeedMax;



                /*TODO LEVELS SPEED */
                /*CUT MAX SPEED TOO SOMETHING LOWER THAN SHOT SPEED MAX FOR WEAKER PLAYERS*/
                timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
                timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);


                //temporary solution
                timeofBallFly = ShotSpeedMax;

                //print("DEBUG1234 TIMEOFBALL SPEED AFTER CUT " + timeofBallFly);

                if (isLobActive)
                {
                    timeofBallFly = UnityEngine.Random.Range(550.0f, 630.0f);
                }

                //print("TIMOFBALLXX BEFORE " + timeofBallFly + " DRAWDISTANCE " + drawDistance + " DELTATIME " + deltaTime);

                /*Than higher timeofBallFly than ball fly slower */
                //print("TIMOFBALLXX AFTER " + timeofBallFly + " shotSpeed " + shotSpeed);

                /*max speed should be 120 in km/h */
                /*speed multiplayer should be beetween [0-80] */
                shotSpeed = MIN_SHOT_SPEED +
                    Mathf.InverseLerp(ShotSpeedMin, ShotSpeedMax, timeofBallFly) * speedMultiplayer;
                if (Globals.isMultiplayer)
                    shotSpeed = Mathf.Min(118f, shotSpeed);

                passedShotFlyTime = 0.0f;

                matchStatistics.setShot("teamA");
                //print("GET SHOT " + matchStatistics.getShot("teamA"));

                //print("DEBUG1234 TIMEOFBALL BEFORE " + timeofBallFly);
                timeofBallFly = ShotSpeedMin + (ShotSpeedMax - timeofBallFly);
                //print("DEBUG1234 TIMEOFBALL AFTER FLY " + timeofBallFly);

                //timeofBallFly =
                //    timeOfBallFlyBasedOnPosition(rb.transform.position, timeofBallFly);
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


                float shotDistanceToTravel = calcShotDistance(outShotStart,
                                                              outShotMid,
                                                              outShotEnd,
                                                              shotvariant);

                timeofBallFly -= 20f;
                timeofBallFlyOrg = timeofBallFly;

                timeofBallFly =
                    timeOfBallFlyBasedOnPosition(rb.transform.position, timeofBallFly, shotDistanceToTravel);

                //this depends on player skills
                float extraTimeOfBallFly =
                    Mathf.Lerp(100f, 0, Mathf.InverseLerp(MIN_SHOT_SPEED, MAX_SHOT_SPEED, shotSpeed));

                timeofBallFly = timeofBallFly + (timeofBallFly * extraTimeOfBallFly) / (ShotSpeedMin - 20f);
                timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
                timeofBallFly = Mathf.Max(ShotSpeedMin - 20f, timeofBallFly);


                if (shotvariant == SHOTVARIANT.CURVE)
                {
                    float speedPerc = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly);
                    setBallShotVel(speedPerc * MAX_SHOT_SPEED_UNITY_UNITS);
                }


                if (!secondShotLineDone)
                    drawSecondShotHelperLine();

                updateTouchEndFinished = false;
                secondShotLineDone = false;

                //drawSecondShotHelperLine();
                //consider players skills
                //timeofBallFly = timeofBallFly +
                //    Mathf.Lerp(100f, 0, Mathf.InverseLerp(MIN_SHOT_SPEED, MAX_SHOT_SPEED, shotSpeed));

                //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER PREPARE SHOT CALC " + ballRb[activeBall].velocity + " SHOTACTIVE " + shotActive);

                //print("GKDEBUG7 OUT VELOCITY " + outShotBallVelocity + " OUTSHOTSTART "
                //    + outShotStart + " OUTSHOTMID " + outShotMid + " OUTSHOTEND " + outShotEnd + " SHOTVARIANT " + shotvariant);
            }

            //print("POSITIONVECTOR" + startPos + " MID " + midPos + " END " + endPos);

            /*TOCHECK*/
            //print("GKDEBUG7 GKDEBUG5 SHOTRET BEFORE " + shotRet);
            if (!shotRet)
            {
                float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);
                curveShotFlyPercent = normalizeTime;
                //print("factorTimeFlyNorm " + timeofBallFly + " TIME " + passedShotFlyTime + " NORMALIZE TIME " + normalizeTime);
                //print("NORMALIZE TIME " + normalizeTime);
                shotRet = shot3New(outShotStart,
                                   outShotMid,
                                   outShotEnd,
                                   outShotBallVelocity,
                                   ref lastBallVelocity,
                                   shotvariant,
                                   normalizeTime);

                //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER PREPARE SHOT ACTIVE " + ballRb[activeBall].velocity + " SHOTACTIVE " + shotActive);
                //print("GKDEBUG7 GKDEBUG5 SHOTRET " + shotRet);
                //print("DEBUG1 STARTHR timeofBallFlyNORM " + timeofBallFly);
                //if (!isBallTrailRendererInit)
                //&& shotSpeed > 72.0f)
                //{
                //print("SHOTSPEED " + shotSpeed);

                /*    float shotDistanceToTravel = calcShotDistance(outShotStart,
                                                            outShotMid,
                                                            outShotEnd,
                                                            shotvariant);*/

                ///if (//timeOfBallFlyBasedOnPositionReverse(
                //     rb.transform.position, timeofBallFly, shotDistanceToTravel) <
                //(ShotSpeedMin + 50.0f))
                if (!isBallTrailRendererInit)
                {
                    //if (/timeofBallFlyOrg < 550f))
                    if (powersScript.getGoalUpHandicap() != 0)
                    {
                        ball[1].ballTrailRendererInit();
                        audioManager.PlayNoCheck("ballFastSpeed");
                    }
                    else
                    {
                        audioManager.PlayNoCheck("kick2");
                    }

                    isBallTrailRendererInit = true;
                }

                //    isBallTrailRendererInit = true;
                //}
            }

            passedShotFlyTime = passedShotFlyTime + (Time.deltaTime * 1000.0f);
            //print("TIMEEXEC " + passedShotFlyTime);
        }
    }

    public void resetDrawOnScreen()
    {
        gkTouchDone = false;
        touchCount = 0;
    }

    private void writeToInfoBar(string text, float delay)
    {
        StartCoroutine(writeInfo(text, delay));
    }

    public float getLastTimeBallWasOut()
    {
        return lastTimeBallWasOut;
    }

    IEnumerator writeInfo(string text, float delayTime)
    {
        float fillAmountVal = 0f;
        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(0.025f);
            shotBar.fillAmount = (float)fillAmountVal;
            fillAmountVal += 0.05f;
        }

        speedShotText.text = text;

        yield return new WaitForSeconds(delayTime);

        fillAmountVal = 0f;
        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(0.025f);
            shotBar.fillAmount = 1f - (float)fillAmountVal;
            fillAmountVal += 0.05f;
        }
    }

    public void setBallVelocity(Vector3 ballVel, Vector3 angularVel)
    {
        ballRb[activeBall].velocity = ballVel;
        ballRb[activeBall].angularVelocity = angularVel;
    }

    private void displayEventInfo()
    {
        if (ball[1].getBallGoalCollisionStatus())
        {
            if (ball[1].whoScored() == 2)
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
                audioManager.PlayNoCheck("whistle1");
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

    IEnumerator showLevelEndPanel(float delayTime)
    { 
        if (Globals.score1 > Globals.score2)
        {
            levelEndImage.texture =
                    Resources.Load<Texture2D>("error/ok");
            levelEndHeaderText.text = Languages.getTranslate("Congratulations! Level completed");
        } else
        {
            levelEndHeaderText.text = Languages.getTranslate("Level failed. Try again");
            levelEndImage.texture =
                Resources.Load<Texture2D>("error/error");
        }

        yield return new WaitForSeconds(delayTime);
        if (Globals.score1 > Globals.score2)
            audioManager.Play("levelcompleted");
        levelEndPanel.SetActive(true);
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
        for (int i = 0; i < 1; i++)
        {
            fadeColor.a += 0.1f;
            flashBackgroundImage.color = fadeColor;
            yield return new WaitForSeconds(delayTime);
        }

        clearVariables();
        setBallPosition();
        fadeColor.a = 0.0f;
        flashBackgroundImage.color = fadeColor;

        ballPrevPosition = ballRb[activeBall].transform.position;
        ball[1].setBallGoalCollisionStatus(false);
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

        }
        else
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
        //if (isFixedUpdate > 1)
        //    return;

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

        if (!cpuPlayer.getShotActive() && 
            !cpuPlayer.getPreShotActive())
        //if (!cpuPlayer.getShotActive() && ballRb[activeBall].transform.position.z < 0f)
        {
            autoMode_gkRunPosReached = false;
            joystickButtonGameObject.SetActive(true);
        }

        playerDirection = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        //just simulate joystick
        if (!Globals.PRO_MODE && !isTrainingActive && !isBonusActive) {
            //if (ballRb[activeBall].transform.position.z > 0f || cpuPlayer.getShotActive())
            if (cpuPlayer.getShotActive() ||
                cpuPlayer.getPreShotActive())
            {
                playerDirection =
                    (autoModGkPos - new Vector3(rb.transform.position.x, 0f, rb.transform.position.z)).normalized;
                if (Vector2.Distance(new Vector2(autoModGkPos.x, autoModGkPos.z),
                                     new Vector2(rb.transform.position.x, rb.transform.position.z)) < 0.2f)
                    autoMode_gkRunPosReached = true;

                if (autoMode_gkRunPosReached)
                    playerDirection = Vector3.zero;
                if (joystickButtonGameObject.activeSelf)
                    joystickButtonGameObject.SetActive(false);
            }
        }

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying)
        {
            bool isOnBall = isPlayerOnBall(
                          rbLeftToeBase,
                          rbRightToeBase,
                          ballRb[activeBall],
                          rb,
                          "move",
                          false,
                          ref activeBall);

            if (!isOnBall)
            {
                lastTimePlayerOnBall = -1f;
            }

            if ((isPlayerNowOnBall == false) &&
                isOnBall)
            {
                lastTimePlayerOnBall = Time.time;
            }

            isPlayerNowOnBall = isOnBall;

            if (powersScript.isPlayerDownSlowDown())
            {
                playerOnBallMoves(rb,
                                  isOnBall,
                                  ref playerDirection,
                                  runningSpeed / 2f);
            } else
            {
                playerOnBallMoves(rb,
                                  isOnBall,
                                  ref playerDirection,
                                  runningSpeed);
            }
        }
    }

    public float getLastTimePlayerOnBall()
    {
        return lastTimePlayerOnBall;
    }

    public bool getIsPlayerOnTheBallNow()
    {
        return isPlayerNowOnBall;
    }
    
    public float getplayerVelocity()
    {
        return playerVelocity;
    }

    private void setupRunningAnimSpeed()
    {
        //float runSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x),
        //                           Mathf.Abs(rb.velocity.z));
        float runSpeed = playerVelocity;
        runSpeed = Mathf.InverseLerp(0.0f, MAX_PLAYER_SPEED, runSpeed);

        float runSpeedVal = Mathf.Clamp((1.15f + Mathf.Abs(runSpeed)), 1.15f, 2.2f);
        animator.SetFloat("3d_run_speed", runSpeedVal);

        //print("runSpeed " + runSpeedVal + " RB VELOCITY " + rb.velocity);
    }

    private void setupTurnSpeedAnim()
    {

        //float runSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x),
        //                           Mathf.Abs(rb.velocity.z));
        float runSpeed = playerVelocity;

        runSpeed = Mathf.InverseLerp(0.0f, MAX_PLAYER_SPEED, runSpeed);
        //animator.SetFloat("3d_run_turn_speed", 1.2f);


        float runSpeedVal = Mathf.Clamp((1.15f + Mathf.Abs(runSpeed / 1.5f)), 1.15f, 1.65f);
        animator.SetFloat("3d_run_turn_speed", runSpeedVal);
    }


    Vector3 prevRbVel = Vector3.zero;
    private void playerOnBallMoves(Rigidbody rb,
                                   bool isOnBall,
                                   ref Vector3 runDirection,
                                   float runSpeed)
    {
        //if (runDirection != Vector3.zero)
        //    print("runDirection " + runDirection.ToString("F4"));

        if (!isOnBall ||
            isBallInGame == false)
        {
            //if (!isPlaying(animator, "3D_run", 1f) &&
            if (!isPlaying(animator, "3D_run", 1f) &&
                 runDirection != Vector3.zero)
            {
                animator.Play("3D_run", 0, 0.0f);
                audioManager.PlayNoCheck("run2");
            }

            float ballRbDist =
                Vector2.Distance(new Vector2(ballRb[activeBall].transform.position.x, ballRb[activeBall].transform.position.z),
                                 new Vector2(rb.transform.position.x, rb.transform.position.z));

            if (runDirection != Vector3.zero &&
                isBallInGame &&
                ballRbDist < 2f &&
                ballRb[activeBall].transform.position.y < 1.0f)
            {
                runDirection = (ballRb[activeBall].transform.position - rb.transform.position).normalized;
                runDirection.y = 0;
            }

            rb.velocity = runDirection * runSpeed;
            setupRunningAnimSpeed();

            lastPlayerMovePosHead = 0;
            lastPlayerMovePosTail = 0;
            return;
        }

        bool isAnyTurnAnimPlaying =
               checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 0.80f, "3D_run_");

        //print("#DBGISFIXED " + isFixedUpdate + " prevRbVel " + prevRbVel);
        if (isFixedUpdate > 1)
        {
            rb.velocity = prevRbVel;

            if (!isPlaying(animator, "3D_run", 1.0f) &&
                prevRbVel != Vector3.zero &&
                !isAnyTurnAnimPlaying)
            {
                animator.Play("3D_run", 0, 0.0f);
                audioManager.PlayNoCheck("run2");
            }

            isUpdateBallPosActive = true;
            updateBallPosName = "bodyMain";
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
        float angle = 0f;
        int minAngleIdx = 0;
        int maxAngleIdx = 0;
        int angleIdx = 0;



        if (!isAnyTurnAnimPlaying)
        {
            //print("ANIMPLAY start " + animName);
            while (listHead != listTail)
            {
                if ((Time.time - lastPlayerMovePos[listHead, 1].x) < 0.1f)
                    break;

                angle = Vector2.SignedAngle(new Vector2(runDirection.x, runDirection.z),
                                            new Vector2(lastPlayerMovePos[listHead, 0].x,
                                                        lastPlayerMovePos[listHead, 0].z));

                /*if (angle != 0f)
                {
                    print("DEBUGXYKJA lastPlayerMovePos ANGLE " + angle.ToString("F4") + " : " + listHead
                        + lastPlayerMovePos[listHead, 0].ToString("F6")
                        + " TIME " + lastPlayerMovePos[listHead, 1].ToString("F6")
                        + " PLYAERDIR " + runDirection.ToString("F6"));
                }*/

                if (angle > maxAngle)
                {
                    maxAngle = angle;
                    maxAngleIdx = listHead;
                }

                if (angle < minAngle)
                {
                    minAngle = angle;
                    minAngleIdx = listHead;
                }

                maxAngle = Mathf.Max(angle, maxAngle);
                listHead++;
                listHead %= lastPlayerMovePos.GetLength(0);
            }
            //print("DEBUGXYKJA END");

            if (maxAngle > Mathf.Abs(minAngle))
            {
                angle = maxAngle;
                angleIdx = maxAngleIdx;
            }
            else
            {
                angle = minAngle;
                angleIdx = minAngleIdx;
            }

            //print("DEBUGXYKJA ANGLE " + angle);
   
            animName =
                convertAngleToAnimNameRunOnBall(rb, angle, false);

            if (animName.Contains("3D_run_"))
                //&&
                //(Time.time - lastPlayerMovePos[angleIdx, 1].x > 0.1f))
            {                
                lastAnimTurnPlayed = animName;

                //print("ANIMNAME " + animName);
                animator.Play(animName, 0, 0.0f);    
                setupTurnSpeedAnim();
                isAnyTurnAnimPlaying = true;
                rb.velocity = Vector3.zero;
                animator.Update(0f);
                audioManager.PlayNoCheck("run2");
                lastPlayerMovePosHead = 0;
                lastPlayerMovePosTail = 0;
            }
            
            //else
            //{
            //   
            //    }
            //}      
        } 
                
        if (isAnyTurnAnimPlaying)
        {

      
            //if (//turnAnimPlayeMovePosTail != lastPlayerMovePosHead &&
            //     lastPlayerMovePosHead != lastPlayerMovePosTail)
            //{
          
            //rb.velocity = lastPlayerMovePos[lastPlayerMovePosHead, 0] * runSpeed;
            rb.velocity *= 1.15f;
            float minNormTime = 0.7f;
            if (isPlayingTurnSpecialCase())
            {
                minNormTime = 0.3f;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= minNormTime)
            {
                lastPlayerMovePosHead = 0;
                lastPlayerMovePosTail = 0;
            }
                //lastPlayerMovePosHead++;
                //lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
            //}

            //etupTurnSpeedAnim();

            //isUpdateBallPosActive = true;
            //updateBallPosName = "bodyMain";
        }


        //if (!isAnyTurnAnimPlaying || (
        //    (isAnyTurnAnimPlaying && 
        //     animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.f)))
        //{
        isUpdateBallPosActive = true;
        updateBallPosName = "bodyMain";

 
            lastPlayerMovePos[lastPlayerMovePosTail, 0] = runDirection;
            lastPlayerMovePos[lastPlayerMovePosTail, 1] = new Vector3(Time.time, 0f, 0f);
            lastPlayerMovePosTail++;
            lastPlayerMovePosTail %= lastPlayerMovePos.GetLength(0);

            if (lastPlayerMovePosHead == lastPlayerMovePosTail)
            {
                lastPlayerMovePosHead++;
                lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
            }
        //}
             
        bool conditionalEnterOfLoop = false;
        if (isAnyTurnAnimPlaying &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f &&
            isPlayingTurnSpecialCase())
        {
            conditionalEnterOfLoop = true;
        }

        float timeDiff = Time.time - lastPlayerMovePos[lastPlayerMovePosHead, 1].x;
        if (!isAnyTurnAnimPlaying ||
            conditionalEnterOfLoop)
            //&&
            //lastPlayerMovePosHead != lastPlayerMovePosTail)
        {
            if (timeDiff > 0.1f)
            {
                Vector3 vel = lastPlayerMovePos[lastPlayerMovePosHead, 0];
                if (!isPlaying(animator, "3D_run", 1.0f) &&
                    vel != Vector3.zero &&
                    !animName.Contains("3D_run_"))
                {
                    animator.Play("3D_run", 0, 0.0f);
                    audioManager.PlayNoCheck("run2");
                }

                rb.velocity = vel * runSpeed;
                rb.velocity /= 1.15f;
                setupRunningAnimSpeed();

                prevRbVel = rb.velocity;

                lastPlayerMovePosHead++;
                lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
            }          
        }

        //if ((lastPlayerMovePosHead != lastPlayerMovePosTail)) 
            //&& 
             //(timeDiff > 0.10f  ||
             // isAnyTurnAnimPlaying))
        //{           
        //    if (!isAnyTurnAnimPlaying)
        //    {
                /*string turnAnim = lastAnimTurnPlayed;
                if (lastAnimTurnPlayed.Equals("empty"))
                {
                    turnAnim = nameAnimationPlaying(animator, 1.0f, 2);
                }

                float minNormTime = 0.60f;
                if (turnAnim.Contains("180turn"))
                {
                    minNormTime = 0.85f;
                }
                else if (turnAnim.Contains("135turn"))
                {
                    minNormTime = 0.80f;
                }
                */
                //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= minNormTime)
                //{
                //if (lastPlayerMovePosHead != lastPlayerMovePosTail)
                //{
                //    rb.velocity = lastPlayerMovePos[lastPlayerMovePosHead, 0] * runSpeed;
                //    rb.velocity /= 1.15f;
                //    setupTurnSpeedAnim();
                //    lastPlayerMovePosHead++;
                 //   lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
                //}
                //}
            //}
            //else
            //{
            
            //}
        //}
    }

    private bool isPlayingTurnSpecialCase()
    {
        if (isPlaying(animator, "3D_run_90turn_out_left", 1.0f) ||
            isPlaying(animator, "3D_run_90turn_in_left", 1.0f))
            return true;
        return false;
    }

    /*public GameObject getCpuLeftHandFingers()
    {
        return playerUpLeftHandFingers;
    }

    public GameObject getCpuRightHandFingers()
    {
        return playerUpRightHandFingers;
    }

    public GameObject getLeftHandFingers()
    {
        return playerDownLeftHandFingers;
    }

    public GameObject getRightHandFingers()
    {
        return playerDownRightHandFingers;
    }
    */
    public GameObject getLeftHand()
    {
        return leftHand;
    }

    public GameObject getRightHand()
    {
        return rightHand;
    }

    public GameObject getCpuLeftHand()
    {
        return cpuLeftHand;
    }

    public GameObject getCpuRightHand()
    {
        return cpuRightHand;
    }

    public GameObject getLeftPalm()
    {
        return leftPalm;
    }

    public GameObject getRightPalm()
    {
        return rightPalm;
    }

    public GameObject getCpuLeftPalm()
    {
        return cpuLeftPalm;
    }

    public GameObject getCpuRightPalm()
    {
        return cpuRightPalm;
    }

    public float getBallRadius()
    {
        return BALL_NEW_RADIUS;
    }

    public Vector3 getBallPos()
    {
        return ballRb[activeBall].transform.position;
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

        if (Mathf.Abs(angle) > 170f)
            //&&
            //isCpu)
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

        powerButton1GameObject = GameObject.Find("powerButton_1");
        powerButton2GameObject = GameObject.Find("powerButton_2");
        powerButton3GameObject = GameObject.Find("powerButton_3");

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
        matchIntroPanel = GameObject.Find("matchIntroPanel");
        matchIntroFlagPanel = GameObject.Find("matchIntroFlagPanel");
        matchIntroPanelBackground = GameObject.Find("matchIntroPanelBackground");
        matchIntroPanelHeaderTop = GameObject.Find("matchIntroPanelHeaderTop");
        matchIntroPanelHeaderDown = GameObject.Find("matchIntroPanelHeaderDown");
        matchIntroWinCoinsNumText = GameObject.Find("winCoinsNumText").GetComponent<TextMeshProUGUI>();
        matchIntroTieNumCoinText = GameObject.Find("tieNumCoinText").GetComponent<TextMeshProUGUI>();
        pauseCanvas = GameObject.Find("pauseCanvas");
        //stadiumPeople = GameObject.Find("st_040_people");
        stadium = GameObject.Find("st_040_stadium");

        shotBarBackground = GameObject.Find("shotBarBackground");
        //refereeBackground = GameObject.Find("refereeBackground");

        try
        {
            matchStatisticsPanel = GameObject.Find("matchStatisticsPanel");
        }
        catch (NullReferenceException ex)
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

        matchStatisticsNext = GameObject.Find("matchStatisticsNext");
        matchStatisticsCoinsRewareded =
            GameObject.Find("matchStatisticsCoinsRewarded").GetComponent<TextMeshProUGUI>();
        matchStatisticsDiamondsRewareded =
            GameObject.Find("matchStatisticsDiamondsRewarded").GetComponent<TextMeshProUGUI>();

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

    public void setGoalSize(Vector3 goalSize)
    {
        goalSizes = goalSize;
    }

    public void setGoalSizeCpu(Vector3 goalSize)
    {
        goalSizesCpu = goalSize;
    }

    public void activateCanvasElements()
    {
        volleyButtonGameObject.SetActive(true);
        //overheadButtonGameObject.SetActive(true);
        lobButtonGameObject.SetActive(true);

        powerButton1GameObject.SetActive(false);
        powerButton2GameObject.SetActive(false);
        powerButton3GameObject.SetActive(false);

        if (!isPowerEnable || 
            (Globals.stadiumNumber == 2 &&
             Globals.isMultiplayer))
        {
            powerButton1GameObject.SetActive(false);
            powerButton2GameObject.SetActive(false);
            powerButton3GameObject.SetActive(false);
        } else
        {
            if (powersScript.getNumberOfActivePowers() >= 1)
                powerButton1GameObject.SetActive(true);
            if (powersScript.getNumberOfActivePowers() >= 2)
                powerButton2GameObject.SetActive(true);
            if (powersScript.getNumberOfActivePowers() >= 3)
                powerButton3GameObject.SetActive(true);
        }

        cameraButtonGameObject.SetActive(true);

        if (!isTrainingActive && !isBonusActive)
            timePanelGameObject.SetActive(true);
        if (!isTrainingActive && !isBonusActive)
            flagPanelGameObject.SetActive(true);
        joystickGameObject.SetActive(true);
        joystickBgGameObject.SetActive(true);
        gkClickHelperGameObject.SetActive(true);
        gkHelperImageGameObject.SetActive(true);
        joystickBgGameObject.SetActive(true);

        //shotBarGameObject.SetActive(true);
        //shotBarIconGameObject.SetActive(true);
        shotBarBackground.SetActive(true);
        //refereeBackground.SetActive(true);
        if (isTrainingActive)
            traningPanel.SetActive(true);

    }

    public IEnumerator prepShotDelay(float delay)
    {
        isPrepareShotDelay = true;
        yield return new WaitForSeconds(delay);
        isPrepareShotDelay = false;
    }

    public void deactivateCanvasElements()
    {
        //overheadButtonGameObject.SetActive(false);
        lobButtonGameObject.SetActive(false);
        cameraButtonGameObject.SetActive(false);
        volleyButtonGameObject.SetActive(false);
        powerButton1GameObject.SetActive(false);
        powerButton2GameObject.SetActive(false);
        powerButton3GameObject.SetActive(false);
        timePanelGameObject.SetActive(false);
        flagPanelGameObject.SetActive(false);
        joystickGameObject.SetActive(false);
        matchIntroPanelHeaderTop.SetActive(false);
        matchIntroPanelHeaderDown.SetActive(false);
        matchIntroFlagPanel.SetActive(false);
        matchIntroPanel.SetActive(false);
        matchIntroPanelBackground.SetActive(false);

        matchStatisticsNext.SetActive(false);
        matchStatisticsPanel.SetActive(false);
        matchStatisticsFlagPanel.SetActive(false);
        matchStatisticsPanelBackground.SetActive(false);
        matchStatisticsPanelHeaderTop.SetActive(false);
        matchStatisticsPanelHeaderDown.SetActive(false);
        //gameInfoImageGameObject.SetActive(false);

        //shotBarGameObject.SetActive(false);
        //shotBarIconGameObject.SetActive(false);
        shotBarBackground.SetActive(false);
        //refereeBackground.SetActive(false);
        traningPanel.SetActive(false);
        pauseCanvas.SetActive(false);

        if ((Globals.stadiumNumber != 2) &&
            (isTrainingActive ||
            isBonusActive))
        {
            //stadiumPeople.SetActive(false);
            foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (allStadiumPeople.name.Contains("crowdAnimated"))
                {
                    allStadiumPeople.SetActive(false);
                }
            }
        }
    }

    private void addCoins()
    {
        if ((Globals.score1 > Globals.score2 && !Globals.playerPlayAway) ||
            (Globals.score2 > Globals.score1 && Globals.playerPlayAway))
        {
            Globals.addCoins(winCoinsRewarded);
            Globals.addDiamonds(winCoinsRewarded);

            matchStatisticsCoinsRewareded.text = "+" + winCoinsRewarded.ToString();
            matchStatisticsDiamondsRewareded.text = "+" + winCoinsRewarded.ToString();
        }
        else
        {
            if (Globals.score1 == Globals.score2)
            {
                Globals.addCoins(tieCoinsRewarded);
                Globals.addDiamonds(tieCoinsRewarded);
                matchStatisticsCoinsRewareded.text = "+" + tieCoinsRewarded.ToString();
                matchStatisticsDiamondsRewareded.text = "+" + tieCoinsRewarded.ToString();
            }
            else
            {
                Globals.addCoins(0);
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
                                Rigidbody ballRbToDelete,
                                Rigidbody rb,
                                string shotType,
                                bool isCpu,
                                ref int activeBall)
    {
        float distance = 0.0f;
        float minDistance = minDistToBallShot;
        //float maxYDist = maxYdistToBallShot;
        /*CRITICAL CHANGE*/
        float maxYDist = BALL_RADIUS;


        //TODO
        int i = 1;
        //for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        //{
        Vector2 ball2D = new Vector2(ballRb[i].transform.position.x, ballRb[i].transform.position.z);
        Vector2 rbLeftToe2D = new Vector2(rbLeftToeBase.transform.position.x, rbLeftToeBase.transform.position.z);
        Vector2 rbRightToe2D = new Vector2(rbRightToeBase.transform.position.x, rbRightToeBase.transform.position.z);
        Vector2 midPos2D = new Vector2(rb.transform.position.x, rb.transform.position.z);

        //print("BALLPOS " + ballRb[i].transform.position);

        /*you cannot take ball that is on opponet half*/
        if (!isBallReachable(ballRb[i].transform.position, isCpu))
        {
            //continue;
            return false;
        }

        /*Check if ball is in front of player. Convert both rb and ballRb[activeBall] to local and check z coordinate of ball */
        if (!checkRelativeBallRigidBodyPos(rb, ballRb[i]))
        {
            //continue;
            return false;
        }

        Vector3 pos = rb.transform.position;
        /*For a normal move take mid of rigidbody*/
        distance = Vector2.Distance(midPos2D,
                                    ball2D);

        if (shotType.Contains("3D_shot_left_foot"))
        {
            minDistance += 0.2f;
            distance = Vector2.Distance(rbLeftToe2D,
                                        ball2D);
            //if (!isCpu && !shotActive && preShotActive)
            //       print("DBGNORMAL LEFT FOOT DISTANCE " + distance);

            pos = rbLeftToeBase.transform.position;
        }
        else if (shotType.Contains("3D_shot_right_foot") || shotType.Contains("3D_volley"))
        {
            distance = Vector2.Distance(rbRightToe2D,
                                        ball2D);

            if (shotType.Contains("3D_shot_right_foot"))
            {
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
                   getLocalRigidBodyObjectPos(rb, ballRb[i].transform.position);

                /*if (!isCpu)
                {
                    if (preShotActive && !shotActive)
                        print("CPUMOVEDEBUG123X_NOCPU ONBALL " + distance + " BALLVEL " + ballRb[activeBall].velocity + " RB TRANSFORM "
                          + rb.transform.position + " MINDISTANCE " + minDistance
                          + " DISTANCE " + distance
                          + " localBallPos "
                          + localBallPos.ToString("F4")
                          + " localRbRightToeBase "
                          + localRbRightToeBase.ToString("F4")
                          + " LOCAL X DIFF " + (Mathf.Abs(localBallPos.x - localRbRightToeBase.x))
                          + " LOCAL Z DIFF " + (Mathf.Abs(localBallPos.z - localRbRightToeBase.z))
                          + " maxYDist " + maxYDist + " YDIST "
                          + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y)
                          + " ballRb[activeBall] " + ballRb[activeBall].transform.position
                          + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                          + " animator.normalizedTime "
                          + animator.GetCurrentAnimatorStateInfo(0).normalizedTime); ;
                } 
                else
                {
                    if (cpuPlayer.getPreShotActive() && !cpuPlayer.getShotActive())
                        print("CPUMOVEDEBUG123X_CPU ONBALL " + distance + " BALLVEL " + ballRb[activeBall].velocity + " RB TRANSFORM "
                          + rb.transform.position + " MINDISTANCE " + minDistance
                          + " DISTANCE " + distance
                          + " localBallPos "
                          + localBallPos.ToString("F4")
                          + " localRbRightToeBase "
                          + localRbRightToeBase.ToString("F4")
                          + " LOCAL X DIFF " + (Mathf.Abs(localBallPos.x - localRbRightToeBase.x))
                          + " LOCAL Z DIFF " + (Mathf.Abs(localBallPos.z - localRbRightToeBase.z))
                          + " maxYDist " + maxYDist + " YDIST "
                          + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y)
                          + " ballRb[activeBall] " + ballRb[activeBall].transform.position
                          + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                          + " animator.normalizedTime "
                          + cpuPlayer.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime);
                }*/

                if (Mathf.Abs(pos.y - ballRb[i].transform.position.y) > 0.6f ||
                    Mathf.Abs(localBallPos.x - localRbRightToeBase.x) > 0.5f ||
                    Mathf.Abs(localBallPos.z - localRbRightToeBase.z) > zMin)
                {
                    //continue;
                    return false;
                }
                else
                {
                    activeBall = i;
                    //print("activeBall set " + i);


                    return true;
                }
            }
        }

        if (shotType.Contains("move"))
        {
            if (PITCH_WIDTH_HALF - Mathf.Abs(ballRb[i].transform.position.x) < 1.5f ||
                PITCH_HEIGHT_HALF - Mathf.Abs(ballRb[i].transform.position.z) < 1.5f ||
                Mathf.Abs(ballRb[i].transform.position.z) < 1.5f)
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
                    + " maxYDist " + maxYDist + " YDIST " + " ballRb[activeBall]POS " + ballRb[activeBall].position +
                    +Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y)
                    + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position + " POS " + pos);
            else
                if (preShotActive && !shotActive)
                print("CPUMOVEDEBUG123X_NOCPU ONBALL " + distance + " BALLVEL " + ballRb[activeBall].velocity + " RB TRANSFORM "
                  + rb.transform.position + " MINDISTANCE " + minDistance
                  + " DISTANCE " + distance + " MIDPOS " + midPos2D
                  + " maxYDist " + maxYDist + " YDIST "
                  + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y)
                  + " ballRb[activeBall] " + ballRb[activeBall].transform.position
                  + " rbRightToeBase.transform.position " + rbRightToeBase.transform.position
                  + " animator.normalizedTime " 
                  + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);*/

        /*if (isCpu)
        print("SHOTTYPEXYZ " + distance + " BALL2d " + ball2D + " rbRightToe2D " + rbRightToe2D + " RB TRANSFORM " 
            + rb.transform.position +                    
            " Y " + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y)  + " MID POS MAIN "
            + rb.transform.position + " SHOTTYPE " + shotType + " BALL POS " + ballRb[activeBall].transform.position 
            + " minDistance " + minDistance + " maxYDist " + maxYDist + " RIGHT FOOT DIST " + 
            Vector2.Distance(new Vector2(
                              rbRightFoot.transform.position.x, rbRightFoot.transform.position.z),
                              ball2D));*/

        //if (isCpu)
        //    print("");
        //print("checkRelativeBallRigidBodyPos(rb, ballRb[activeBall]) CPU DIST " + distance + " Y DIST " + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y));
        //else
        //    if (v)
        /*if (!isCpu && isPlaying(animator, "3D_volley", 1.0f))
            print("VOLLEYDEBUGXY67 NO CPU DIST " + distance + " MINDIST " 
                + minDistance + " Y DIST " + Mathf.Abs(pos.y - ballRb[activeBall].transform.position.y) 
                + " MAXYDIST " + maxYDist + " rbRightToeBase.transform.position " 
                + rbRightToeBase.transform.position 
                + " rballRb[activeBall].transform.position " + ballRb[activeBall].transform.position);*/

        if ((distance < minDistance) &&
            (Mathf.Abs(pos.y - ballRb[i].transform.position.y) < maxYDist))
        {
            activeBall = i;
            //print("activeBall set " + i);
            return true;
        }
        //}

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
        //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);
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
        joystickButtonGameObject = GameObject.Find("joystickButton");

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

        if (Globals.joystickSide.Contains("LEFT"))
        {
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
        RectTransform lobTextRectTrans = lobButtonTextGameObject.GetComponent<RectTransform>();
        RectTransform volleyRectTrans = volleyButtonGameObject.GetComponent<RectTransform>();
        RectTransform powerButton1RectTrans = powerButton1GameObject.GetComponent<RectTransform>();
        RectTransform powerButton2RectTrans = powerButton2GameObject.GetComponent<RectTransform>();
        RectTransform powerButton3RectTrans = powerButton3GameObject.GetComponent<RectTransform>();

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

        /*anchor power button top, right*/
        powerButton1RectTrans.anchorMin = new Vector2(1, 1);
        powerButton1RectTrans.anchorMax = new Vector2(1, 1);
        powerButton1RectTrans.pivot = new Vector2(1, 1);
        powerButton2RectTrans.anchorMin = new Vector2(1, 1);
        powerButton2RectTrans.anchorMax = new Vector2(1, 1);
        powerButton2RectTrans.pivot = new Vector2(1, 1);
        powerButton3RectTrans.anchorMin = new Vector2(1, 1);
        powerButton3RectTrans.anchorMax = new Vector2(1, 1);
        powerButton3RectTrans.pivot = new Vector2(1, 1);

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

        powerButton1RectTrans.anchoredPosition = new Vector2(-(buttonSize / 2.1f),
                                                             -(buttonSize / 8f));

        powerButton2RectTrans.anchoredPosition = new Vector2(
            -(buttonSize / 2.1f),
             powerButton1RectTrans.anchoredPosition.y - (buttonSize * 1.1f));

        powerButton3RectTrans.anchoredPosition =
            new Vector2(-buttonSize + (powerButton1RectTrans.anchoredPosition.x * 1.2f),
                        (powerButton1RectTrans.anchoredPosition.y +
                         powerButton2RectTrans.anchoredPosition.y) / 2f);
        powerButton1RectTrans.sizeDelta = new Vector2(buttonSize,
                                                      buttonSize);
        powerButton2RectTrans.sizeDelta = new Vector2(buttonSize,
                                                      buttonSize);
        powerButton3RectTrans.sizeDelta = new Vector2(buttonSize,
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

        int fontSize = Math.Max(10, (int)(buttonSize / 2.9f));
        volleyButtonTextGameObject.GetComponent<TextMeshProUGUI>().fontSize = fontSize;
        lobButtonTextGameObject.GetComponent<TextMeshProUGUI>().fontSize = fontSize;
        //cameraButtonTextGameObject.GetComponent<Text>().fontSize = fontSize;

        /*BUG*/
        //specialButtonsScreenOffset = cameraRectTrans.anchoredPosition.x + (buttonSize * 1.1f);

        specialButtonsScreenOffset.x = Mathf.Abs(volleyRectTrans.anchoredPosition.x) +
            buttonSize;

        specialButtonsScreenOffset.y = volleyRectTrans.anchoredPosition.y +
            (buttonSize * 1.6f);

        specialPowersScreenOffset.x =
            screenWidth - (Mathf.Abs(powerButton3RectTrans.anchoredPosition.x) + (buttonSize * 1.2f));

        specialPowersScreenOffset.y = screenHeight -
            (Mathf.Abs(powerButton2RectTrans.anchoredPosition.y) + (buttonSize * 1.2f));

        //print("#UPDATE1234 specialButtonsOffsetPowers  " + specialPowersScreenOffset + " specialButtonsScreenOffset " +
        //    specialButtonsScreenOffset + " powerButton2RectTrans.anchoredPosition.y " 
        //    + powerButton2RectTrans.anchoredPosition);

        /*print("UPDATTOUCH145 specialButtonsScreenOffset " + specialButtonsScreenOffset 
            + " LOB ANCHOR " + lobRectTrans.anchoredPosition
            + " CAMERA POS " + cameraRectTrans.position
            + "volleyRectTrans.anchoredPosition "
            + volleyRectTrans.anchoredPosition + " buttonSize " + buttonSize);*/
        powerButton1GameObject.SetActive(false);
        powerButton2GameObject.SetActive(false);
        powerButton3GameObject.SetActive(false);
    }

    public bool isPlayerOnBall()
    {
        return (onBall == PlayerOnBall.ONBALL) ? true : false;
    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("ONANIMATOROIK ");
        onAniatorIk(animator,
                    cpuPlayer.getShotActive(),
                    cpuPlayer.isLobShotActive(),
                    lastGkDistX, rb, leftHand,
                    rightHand,
                    lastGkAnimName,
                    false);
        if (!isBonusActive)
            cpuPlayer.onAniatorIk();
    }

    private float lastTimeShotActivePlayer;
    private float lastTimeShotActiveCpu;

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


        /*if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            (shotActive))
        {
            if (!isCpu)
            {
                print("#DBGANIMATOR gkDistRealClicked " + gkDistRealClicked + " MIN_DIST_REAL_CLICKED + 1.0f " +
                    +(MIN_DIST_REAL_CLICKED + 1.0) + " isBigger " + (gkDistRealClicked > (MIN_DIST_REAL_CLICKED + 1.0f)));
            }
        }*/


        if (!isCpu && (gkDistRealClicked > (MIN_DIST_REAL_CLICKED + 1.0f)))
        {

          //  if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
          //      (shotActive))
          //          print("#DBGANIMATOR gkDistRealClicked dist too big ####");
            return;
        }

        Vector3 ballInLocalRb =
            InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position);
        if (ballInLocalRb.z < 0.0f) return;

        bool isGktStraightPlaying =
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "straight");

        if (isGktStraightPlaying &&
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "chest"))
            return;

        if (isGktStraightPlaying &&
            ballRb[activeBall].transform.position.y > 0.8f)
        {
            float reach = 1.0f;

            Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);

            //if (ballInLocalRb.z < 0.0f) return;

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
            animator.SetIKPosition(AvatarIKGoal.RightHand, ballRb[activeBall].transform.position);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRb[activeBall].transform.position);

            return;
        }

        /*if (!isCpu)
            print("GKDEBUG800 PLAYINGANIMATIONOW isShotActive " + shotActive + " ISPLAYING "
                + checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch"));*/
        /*if (isPlaying(animator, "3D_volley", 1.0f))
        {
            float reach = 1.00f;

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, reach);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, ballRb[activeBall].transform.position);
            
            return;
        }*/




        //if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") && shotActive)
        float distLeftHand = 0f;
        float distRightHand = 0f;



        //shotActive is actually opposite
        if (shotActive)
        {
            if (!isCpu)
            {
                lastTimeShotActiveCpu = Time.time;
            }
            else
            {
                lastTimeShotActivePlayer = Time.time;
            }
        }

        if (!isCpu)
        {
            distLeftHand = Vector3.Distance(
                   getLeftHand().transform.position,
                   rb.transform.position);

            distRightHand =
               Vector3.Distance(
                   getRightHand().transform.position,
                   rb.transform.position);
        }
        else
        {
            distLeftHand = Vector3.Distance(
                   getCpuLeftHand().transform.position,
                   rb.transform.position);

            distRightHand =
                Vector3.Distance(
                    getCpuRightHand().transform.position,
                    rb.transform.position);
        }

        /*print("DISTHANDFROMBALL_22 isCpu " 
           + isCpu
           + " lastTimeShotActivePlayer DIFF " + (Time.time - lastTimeShotActivePlayer)
           + " lastTimeShotActiveCPU DIFF " + (Time.time - lastTimeShotActiveCpu)
           + " shotActive " + shotActive
           + " distLeftHand " + distLeftHand
           + " distRightHand " + distRightHand);*/

        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            (shotActive))
        //||             
        //(((isCpu && ((Time.time - lastTimeShotActivePlayer) < 0.06f)) ||
        // (!isCpu && ((Time.time - lastTimeShotActiveCpu) < 0.06f))) && 
        // (distLeftHand < 0.8f) && 
        // (distRightHand < 0.8f))))
        {
            //print("DISTHANDFROMBALL_22 isCpu " + isCpu + " ENTERED ");

            /*print("#DBGANIMATOR normTime " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime +
                " distLeftHand " + Vector3.Distance(
                                                getLeftHand().transform.position,
                                                ballRb[activeBall].transform.position) +
                " distRightHand " + Vector3.Distance(
                                    getRightHand().transform.position,
                                    ballRb[activeBall].transform.position));*/
               
            float reach = 1.00f;
            if (!isCpu)
            {
                reach = 1f;
            }
            else
            {
                reach = 1f;
            }
            //print("TRANSFORMHANDLEFT " + rb.transform.InverseTransformPoint(leftPalm.transform.position));
            //print("TRANSFORMHANDRIGHT " + rb.transform.InverseTransformPoint(rightPalm.transform.position));

            //print("TRANSFORMHANDLEFTGLOBAL " + leftPalm.transform.position);
            //print("TRANSFORMHANDRIGHTGLOBAL " + rightPalm.transform.position);

            //Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            //Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);

            //print("TRANSFORMHANDBALL " + ballInLocalRb);

            //if (ballInLocalRb.z < -0.3f) return;

            //print("GKDEBUG800 ballINLOCALRB " + ballInLocalRb);

            Vector3 ballPos = ballRb[activeBall].transform.position;
            Vector3 whereBallHit = ballPos;
            Vector3 whereBallHitUp =
                new Vector3(ballPos.x, ballPos.y + BALL_NEW_RADIUS, ballPos.z);
            Vector3 whereBallHitDown =
                new Vector3(ballPos.x, Mathf.Max(0f, ballPos.y - BALL_NEW_RADIUS), ballPos.z);

            ballRbRightSide[activeBall].transform.position = whereBallHitUp;
            ballRbLeftSide[activeBall].transform.position = whereBallHitDown;

            /*print("WHEREBALLHITS " + whereBallHit);
            print("DISTHANDFROMBALL_# whereBallHit " + whereBallHit + " whereBallHitUp " + whereBallHitUp
                + " whereBallHitDown " + whereBallHitDown
                + " ballRbRightSide[activeBall].transform.position "
                + ballRbRightSide[activeBall].transform.position
                + " ballRbLeftSide[activeBall].transform.position "
                + ballRbLeftSide[activeBall].transform.position);*/

            /* if (!isCpu) {
                 print(
                     "DISTHANDFROMBALL_# whereBallHit leftHand " + getLeftHand().transform.position + 
                     " rightHand " + getRightHand().transform.position);
             }
             else
             {
                 print(
                    "DISTHANDFROMBALL_# whereBallHit leftHand " + getCpuLeftHand().transform.position + 
                    " rightHand " + getCpuRightHand().transform.position);
             }*/

            //if (rightHandLocalPos.z > 0.3f)
            //{
            //if (ballInLocalRb.z  0.3f)
            /*Vector3 higherBallSidePos = ballRbLeftSide[activeBall].transform.position;
            Vector3 lowerBallSidePos = ballRbRightSide[activeBall].transform.position;
            if (ballRbRightSide[activeBall].transform.position.y >
                ballRbLeftSide[activeBall].transform.position.y)
            {
                higherBallSidePos = ballRbRightSide[activeBall].transform.position;
                lowerBallSidePos = ballRbLeftSide[activeBall].transform.position;
            }*/


            if ((anim.Contains("right") && anim.Contains("punch")) ||
                (anim.Contains("straight") && distX >= 0.0f) ||
                (!anim.Contains("punch") && !anim.Contains("straight")))
            {

                //if (isCpu)
                //{
                //    print("GKDEBUG800 PLAYINGANIMATIONOW ON ANIMATOR IK LEFT RIGHT " + distX);
                //}
                Quaternion rightHandRotation =
                    Quaternion.LookRotation(ballRb[activeBall].transform.position - rightHand.transform.position);

                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitUp);

                        /*if (isCpu)
                            print("DISTHANDFROMBALL_# IK CPU 3D_GK_sidecatch_left_ "
                                + cpuRightHand.transform.position
                                + " whereBallHitUp "
                                + whereBallHitUp);
                        else
                            print("DISTHANDFROMBALL_# IK PLAYER 3D_GK_sidecatch_left_ " +
                                rightHand.transform.position
                                + " whereBallHitUp " +
                                whereBallHitUp);*/

                        rightHandRotation =
                            Quaternion.LookRotation(whereBallHitUp - rightHand.transform.position);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, whereBallHitDown);

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
                //print("IKPUNCHTEST1LEFT");
                //if (isCpu)
                //{
                //    print("GKDEBUG800 PLAYINGANIMATIONOW ON ANIMATOR IK LEFT " + distX);
                //}

                Quaternion leftHandRotation =
                    Quaternion.LookRotation(ballRb[activeBall].transform.position - leftHand.transform.position);
                if (anim.Contains("left_") ||
                    anim.Contains("right_"))
                {
                    if (anim.Contains("left_"))
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitDown);

                        /*if (isCpu)
                            print("DISTHANDFROMBALL_# CPU 3D_GK_sidecatch_left_ "
                                + cpuLeftHand.transform.position
                                + " whereBallHitDown "
                                + whereBallHitDown);
                        else
                            print("DISTHANDFROMBALL_# PLAYER 3D_GK_sidecatch_left_ " +
                                leftHand.transform.position
                                + " whereBallHitDown " +
                                whereBallHitDown);*/

                        leftHandRotation =
                            Quaternion.LookRotation(whereBallHitDown - leftHand.transform.position);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHitUp);

                        /* if (isCpu)
                             print("DISTHANDFROMBALL_# CPU 3D_GK_sidecatch_right_ "
                                 + cpuLeftHand.transform.position
                                 + " whereBallHitUp "
                                 + whereBallHitUp);
                         else
                             print("DISTHANDFROMBALL_# PLAYER 3D_GK_sidecatch_right_ " +
                                 leftHand.transform.position
                                 + " whereBallHitUp " +
                                 whereBallHitUp);*/


                        leftHandRotation =
                            Quaternion.LookRotation(whereBallHitUp - leftHand.transform.position);
                    }
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, whereBallHit);
                }


                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            }

            return;
        }

        return;
    }

    /*
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
    }*/

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

        Animator cpuAnimator = cpuPlayer.getAnimator();
        if (!checkIfAnyAnimationPlaying(cpuAnimator, 1.0f) &&
            !isPlaying(cpuAnimator, cpuAnimName, 1.0f))
        {
            //print("ANIMNAME CPUPLAYED " + cpuAnimName);

            cpuAnimator.Play(cpuAnimName, 0, 0.0f);
        }
    }

    private void recoverAnimatorSpeed()
    {
        if (!checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_"))
        {
            animator.speed = 1.0f;
        }

        if (!checkIfAnyAnimationPlayingContain(cpuPlayer.getAnimator(), 1.0f, "3D_GK_sidecatch_"))
        {
            cpuPlayer.setAnimatorSpeed(1.0f);
        }
    }

    public string getLastGkAnimPlayed()
    {
        return lastGkAnimName;
    }

    void LateUpdate()
    {

        if (Globals.isMultiplayer &&
            !PhotonNetwork.IsConnected)
            return;


            //print("DEBUGCAMERA LateUpdate  DELTA TIME " + deltaIterator + " Time.deltaTime " + Time.deltaTime);
            //deltaIterator++;

            //ballRb[activeBall].angularVelocity = Vector3.zero;
            //ballRb[activeBall].angularVelocity = new Vector3(0.1f, 0.1f, 0.1f);

            if (!ball[1].getBallCollided() &&
            shotActive &&
            !shotRet &&
            lastBallVelocity != Vector3.zero &&
            isBallInGame)
        {
            //print("GKDEBUG7 LATE BEFORE " + ballRb[activeBall].velocity);
            //print("DEBUG2345ANIMPLAY update ball velocit lastVel " + lastBallVelocity);
            ballRb[activeBall].velocity = lastBallVelocity;
            //print("GKDEBUG7 LATE AFTER " + ballRb[activeBall].velocity);
        }


        correctWhenOutOfPitch(animator,
                        preShotActive,
                        shotActive,
                        rb,
                        rbLeftToeBase,
                        rbRightToeBase,
                        ballRb[activeBall],
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

        cpuPlayer.lateUpdate();


        //print("LATE RB POSITION " + rb.position);
        /*don't allow to goes down over floor after some time */
        if (rb.transform.position.y < 0.03f)
            rb.transform.position =
                new Vector3(rb.transform.position.x, 0.03f, rb.transform.position.z);



        for (int i = 0; i < isFixedUpdate; i++)
        {
            cameraMovement(false, -1);
        }
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
                                           Mathf.Round(ballRb[activeBall].angularVelocity.x),
                                           Mathf.Round(ballRb[activeBall].angularVelocity.y),
                                           Mathf.Round(ballRb[activeBall].angularVelocity.z));

        if (rbRoundVector == Vector3.zero)
        {
            if (BallRoundAngularVelVector == Vector3.zero)
            {
                ballRb[activeBall].angularVelocity = Vector3.zero;
                ballRb[activeBall].velocity = Vector3.zero;
            }
            else
            {
                ballRb[activeBall].angularVelocity /= 1.05f;
                ballRb[activeBall].velocity /= 1.05f;
            }
        }
        else
        {
            ballRb[activeBall].angularVelocity = new Vector3(10.0f, 10.0f, 10.0f);
        }
    }

    /*public void ballMovePosition(Rigidbody rb, Vector3 movementDir)
    {
        ballRb[activeBall].MovePosition(rb.position + (movementDir / 1.2f));

        Vector3 rbRoundVector = new Vector3(
                                         Mathf.Round(rb.velocity.x),
                                         Mathf.Round(rb.velocity.y),
                                         Mathf.Round(rb.velocity.z));

        Vector3 BallRoundAngularVelVector = new Vector3(
                                         Mathf.Round(ballRb[activeBall].angularVelocity.x),
                                         Mathf.Round(ballRb[activeBall].angularVelocity.y),
                                         Mathf.Round(ballRb[activeBall].angularVelocity.z));

        if (rbRoundVector == Vector3.zero)
        {
            if (BallRoundAngularVelVector == Vector3.zero)
            {
                ballRb[activeBall].angularVelocity = Vector3.zero;
                ballRb[activeBall].velocity = Vector3.zero;
            }
            else
            {
                ballRb[activeBall].angularVelocity /= 1.05f;
                ballRb[activeBall].velocity /= 1.05f;
            }
            //print("ballMovePosition CLEARED HERE 2");
        } else
        {
            ballRb[activeBall].angularVelocity = new Vector3(10.0f, 10.0f, 10.0f);
        }

       
    }
*/
    public void setBallPosition()
    {
        //if (ball.getBallGoalCollisionStatus())
        //{
        if (Globals.isTrainingActive ||
            isBonusActive)
        {
            /*if (trainingScript.isGkTraining())
            {
                ballRb[activeBall].position =
                    new Vector3(UnityEngine.Random.Range(-13, 13f),
                                0f,
                                UnityEngine.Random.Range(3, 8f));
            }*/

            return;
        }

        //ballRb[activeBall].position = new Vector3(PITCH_WIDTH_HALF - 0.23f, 0, -(PITCH_HEIGHT_HALF - 0.23f));
        //return;


        bool ballGoal = ball[1].getBallGoalCollisionStatus();
        Vector3 ballPosPlayer2 = new Vector3(UnityEngine.Random.Range(-8, 8),
                                             ballRadius,
                                             UnityEngine.Random.Range(5, 10));
        Vector3 ballPosPlayer1 = ballPosPlayer2;
        ballPosPlayer1.z *= -1;

        //TODELETE

        /*Vector3 ballPlayer1NewPos =
                 rb.transform.position +
                 (rb.transform.forward * UnityEngine.Random.Range(1.5f, 3.5f));
        
       if ((Mathf.Abs(ballPlayer1NewPos.x) < (PITCH_WIDTH_HALF - 4)) &&
           (Mathf.Abs(ballPlayer1NewPos.z) > 4f) &&
           (Mathf.Abs(ballPlayer1NewPos.z) < 12.5f))
       {
            ballPlayer1NewPos.y = ballRadius;
            ballPosPlayer1 = ballPlayer1NewPos;
       }


        Vector3 ballPlayer2NewPos =
            cpuPlayer.getRbTransform().position +
                    (cpuPlayer.getRbTransform().forward * UnityEngine.Random.Range(1.5f, 3.5f));
        if (((ballPlayer2NewPos.x) < (PITCH_WIDTH_HALF - 4)) &&
            (ballPlayer2NewPos.z > 4f) &&
            (ballPlayer2NewPos.z < 12.5f))
        {
            ballPlayer2NewPos.y = ballRadius;
            ballPosPlayer2 = ballPlayer2NewPos;
        }*/

        if (ballGoal)
        {
            if (ball[1].whoScored() == 1)
            {
                ballRb[activeBall].transform.position = ballPosPlayer2;
            }
            else
            {
                ballRb[activeBall].transform.position = ballPosPlayer1;
            }

            return;
        }

        //print("whoTouchBallLastSETBALL " + ball.whoScored() + " WHOTOUCHLAST " + ball.whoTouchBallLast());
        if (ball[1].whoTouchBallLast() == 1)
        {
            //ballRb[activeBall].position = new Vector3(0, 0, 4);
            ballRb[activeBall].transform.position = ballPosPlayer2;
            return;
        }
        else
        {
            ballRb[activeBall].transform.position = ballPosPlayer1;
            return;
        }

        /*shouldn't be reach */
        ballRb[activeBall].transform.position = ballPosPlayer1;

        return;
    }

    public void setBallPosition(Vector3 ballPos)
    {
        ballRb[activeBall].transform.position = ballPos;
    }

    public void goToPos(Animator animator,
                        string animName,
                        Rigidbody rb,
                        float speed,
                        Vector3 towardsNewPos)
    {
        towardsNewPos = towardsNewPos.normalized;
     
        interruptSideAnimation(animator, rb);
        rb.velocity = towardsNewPos * speed;
      
        if (!isPlaying(animator, animName, 0.95f))
        {
            animator.Play(animName, 0, 0.0f);
        }
    }

    public void showBalls(int num)
    {
        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {
            ballRb[i] = GameObject.Find("ball" + i.ToString()).GetComponent<Rigidbody>();
            ball[i] = GameObject.Find("ball" + i.ToString()).GetComponent<ballMovement>();
        }

        for (int i = NUMBER_OF_BALLS + 1; i <= MAX_NUMBER_OF_BALLS; i++)
        {
            GameObject.Find("ball" + i.ToString()).SetActive(false);
        }
    }

    public string getShotType()
    {
        return shotType;
    }

    private int getGoldSilverRandFreq()
    {
        int rand = UnityEngine.Random.Range(0, 30);
        if (Globals.level == 3)
        {
            rand = UnityEngine.Random.Range(0, 20);
        }
        else if (Globals.level == 4)
        {
            rand = UnityEngine.Random.Range(0, 12);
        }
        else if (Globals.level == 5)
        {
            rand = UnityEngine.Random.Range(2, 6);
        }

        return rand;
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
                             ref float shotRotationDelay,
                             bool isCpu,
                             bool isDelay)
    {
        bool shotActive = false;
        float clipOffsetTime = 1f;

        /*if (Globals.isMultiplayer)
        {
            if (isDelay)
            {
                isUpdateBallPosActive = true;
                updateBallPosName = "bodyMain";
                return false;
            }
        }*/

        if (!initShot)
        {
            //if (Globals.isMultiplayer &&
            //    !isCpu)
            //{
            //    touchLocked = true;
            //    touchCount = 0;
            //    isTouchBegin = false;
            //}

            /*Don't block touch here */
            animator.Play(shotType, 0, 0.0f);
            animator.Update(0.0f);
            initShot = true;
            //it is used to make rotation slower to bluff off opponent
            shotRotationDelay = 0f;

            if (shotType.Equals("3D_volley_before"))
            {
                shotType = "3D_volley";
            }

            if (isCpu && !shotType.Contains("volley") && UnityEngine.Random.Range(0, 2) == 1)
                shotRotationDelay = animationOffsetTime[shotType] * UnityEngine.Random.Range(0.4f, 0.6f);

            if (isCpu &&
                (Globals.stadiumNumber == 0) &&
                (!Globals.isMultiplayer))
            {

                int rand = getGoldSilverRandFreq();

                if ((rand == 5) &&
                     (Globals.stadiumNumber != 2))
                {
                    if (UnityEngine.Random.Range(0, 3) == 1)
                       powersScript.goldenBall(true, (int)
                            POWER.GOLDEN_BALL, Vector3.zero, Vector3.zero);
                    else 
                       powersScript.silverBall(true, (int)
                            POWER.SILVER_BALL, Vector3.zero, Vector3.zero);
                }
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
                                         isCpu,
                                         ref activeBall);

            //float ballDist = Vector3.Distance(playerRb.transform.position,
            //                                 (ballRb[activeBall].transform.position));

            //print("GKDEBUG1 ballDIST INSIDE " + ballDist);
            //print("BALLDISTANCE " + ballDist + " SHOTTYPE " + shotType + " ISCPU " + isCpu);
            if (shotType.Equals("3D_volley"))
            {

                onBall = isPlayerOnBall(rbLeftToeBase,
                                        rbRightToeBase,
                                        ballRb,
                                        playerRb,
                                        shotType,
                                        isCpu,
                                        ref activeBall);

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

                    //if (curveMidPos3.y > curveEndPos3.y)
                    //    isLobActive = true;
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
                    cpuPlayer.clearPreShotVariables();
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
                        }
                        else
                        {
                            ballRb.velocity = new Vector3(
                                 pointInFrointOfRightToeDir.x * dist,
                                 7.58f,
                                 pointInFrointOfRightToeDir.z * dist);
                        }


                        /*ball goes up*/
                        //ballRb[activeBall].velocity = new Vector3(shotEndPos.x * 1.35f, 7.5f, shotEndPos.z * 1.35f);

                        /*if (!isCpu)
                        {
                            print("CPUMOVEDEBUG123X_NOCPU ballRb[activeBall].velocity "
                                + ballRb[activeBall].velocity
                                + " BALLPOS "
                                + ballRb[activeBall].transform.position
                                + " DISTANCE "
                                + dist
                                + " playerRb.transform "
                                + playerRb.transform.position
                                + " pointInFrointOfRightToe "
                                + pointInFrointOfRightToe);
                        } else
                        {
                            print("CPUMOVEDEBUG123X_CPU ballRb[activeBall].velocity "
                               + ballRb[activeBall].velocity
                               + " BALLPOS "
                               + ballRb[activeBall].transform.position
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
                        //    shotEndPos = endPosOrg - ballRb[activeBall].transform.position;
                        //    shotEndPos = shotEndPos.normalized;

                        /*ball goes up*/
                        //ballRb[activeBall].velocity = new Vector3(shotEndPos.x * 1.2f, 7.10f, shotEndPos.z * 1.2f);
                        //    ballRb[activeBall].velocity = new Vector3(shotEndPos.x * 1.35f, 7.7f, shotEndPos.z * 1.35f);

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
            + InverseTransformPointUnscaled(playerRb.transform, ballRb[activeBall].transform.position).ToString("F4")
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

        ballRb[activeBall].transform.position =
            new Vector3(ballWorldSpace.x, BALL_MIN_VAL.y, ballWorldSpace.z);
    }

    private void volleyShotUpdateBallPos(Rigidbody rb,
                                         GameObject rbRightToeBase,
                                         GameObject rbRightFoot,
                                         bool isCpu)
    {
        Vector3 ballPos;

        ballPos = rbRightToeBase.transform.position + (rbRightFoot.transform.forward * 0.3f);
        ballRb[activeBall].transform.position = ballPos;

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
        //ballRb[activeBall].MovePosition(ballPos);
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
        return isPlayerOnBall(
            rbLeftToeBase, rbRightToeBase, ballRb[activeBall], rb, "move", false, ref activeBall);
    }

    /*private void gkIdle(Animator animator)
    {
        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!isAnyAnimationPlaying)
        {            
            if (ballRb[activeBall].transform.position.z > 0.0f &&
                !preShotActive &&
                !shotActive &&
                !cpuPlayer.getShotActive() &&
                goalJustScored == false &&
                isBallInGame)
            {
                Vector3 lookTowardBall = new Vector3(ballRb[activeBall].transform.position.x - rb.position.x,
                                                     0.0f,
                                                     ballRb[activeBall].transform.position.z - rb.position.z);

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
        //        +  " isOn BAll " + isOnBall + " ballRb[activeBall].transform" + ballRb[activeBall].transform.position);

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
            minZDistance = 0.20f;
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
                }
                else
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
        float level = (float)Globals.level;

        if (Globals.level <= 2)
            levelFactor = 0.05f + (level * 0.25f);
        else
        {
            if (Globals.level == 3)
            {
                levelFactor = 0.70f;
            }
            else if (Globals.level == 4)
            {
                levelFactor = 0.90f;
            }
            else if (Globals.level == 5)
            {
                levelFactor = 1.1f;
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

        winPoints = (int)((float)winPoints * levelFactor);
        tiePoints = (int)((float)tiePoints * levelFactor);

        /*values should be beetween 0 and 100 */
        winPoints = Mathf.Clamp(winPoints, 7, 130);
        tiePoints = Mathf.Clamp(tiePoints, 4, 130);

        if (Globals.level == Globals.MAX_LEVEL)
        {
            winPoints = Mathf.Clamp(winPoints, 24, 130);
            tiePoints = Mathf.Clamp(tiePoints, 14, 130);
        }
        else if (Globals.level == 4)
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
        if ((ballRb[activeBall].transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            cpuPlayer.getShotActive() ||
            playerDirection != Vector3.zero)
            return;

        if (isAnimatorPlaying(animator)) return;

        //stepUpDown(animator, rb, 0.05f, "3D_run");
        //if (!isPlaying(animator, "3D_walk", 1.0f))
        //{
        animator.Play("3D_walk", 0, 0.0f);
        //}

    }

    public void gkMoveDownAnim()
    {
        if ((ballRb[activeBall].transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            cpuPlayer.getShotActive() ||
            playerDirection != Vector3.zero)
            return;

        if (isAnimatorPlaying(animator)) return;

        //isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        //if (isAnyAnimationPlaying) return;

        animator.Play("3D_back_run", 0, 0.0f);
    }

    public void gkSideLeftAnim()
    {
        if ((ballRb[activeBall].transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            cpuPlayer.getShotActive() ||
            playerDirection != Vector3.zero)
            return;

        //if (isTrainingActive &&
        //    trainingScript.isTraningPaused())
        //    return;
        if (isAnimatorPlaying(animator)) return;
        //isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);

        //if (!isAnyAnimationPlaying)
        //    stepSide(animator, rb, -0.05f, "3D_GK_step_left");
        //if (!isAnyAnimationPlaying)
        //{
        animator.Play("3D_GK_step_left", 0, 0.0f);
        //}
    }

    public void gkSideRightAnim()
    {
        if ((ballRb[activeBall].transform.position.z < 0f && !cpuPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            cpuPlayer.getShotActive() ||
            playerDirection != Vector3.zero)
            return;

        //isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (isAnimatorPlaying(animator)) return;
        //if (!isAnyAnimationPlaying)
        //    stepSide(animator, rb, 0.05f, "3D_GK_step_right");
        //if (!isAnyAnimationPlaying)
        //{
        animator.Play("3D_GK_step_right", 0, 0.0f);
        //}
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
        ballRb[activeBall].transform.position = rb.transform.position + (rb.transform.forward / div);
        //ballRb[activeBall].MovePosition(rb.transform.position + (rb.transform.forward / div));
    }

    public void setBallInFrontOfRb(Rigidbody rb, Vector3 pos)
    {
        ballRb[activeBall].transform.position = pos;
        //ballRb[activeBall].MovePosition(pos);
    }

    public bool isTouchPaused()
    {
        if ((Globals.isTrainingActive && trainingScript.isTouchPaused()) ||
             (isBonusActive && bonusScript.isTouchPaused()))
            return true;

        return false;
    }

    public bool isRunPaused()
    {
        if ((Globals.isTrainingActive && trainingScript.isRunPaused()) ||
            (isBonusActive && bonusScript.isRunPaused()))
            return true;

        return false;
    }

    private void levelsSettings()
    {
        if (Globals.isLevelMode)
        {
            Globals.playerPlayAway = false;
            Globals.isTrainingActive = false;
            Globals.isBonusActive = false;
            Globals.onlyTrainingActive = false;
        }
    }

    private void multiplayerSettings()
    {
        if (!Globals.isMultiplayer)
            return;

        Globals.matchTime = "55 SECONDS";
        Globals.playerPlayAway = false;
        Globals.level = UnityEngine.Random.Range(2, Globals.MAX_LEVEL + 1);
        Globals.isTrainingActive = false;
        Globals.isBonusActive = false;
        Globals.onlyTrainingActive = false;
        ShotSpeedMax = 900f;
        ShotSpeedMin = 480f;
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

    public bool isShotTrainingActive()
    {
        if (Globals.isTrainingActive &&
            trainingScript.isShotTraining())
            return true;
        return false;
    }

    public bool isShotBonusActive()
    {
        if (isBonusActive &&
            bonusScript.isShotBonusActive())
            return true;
        return false;
    }

    public bool isGkTrainingActive()
    {
        if (Globals.isTrainingActive &&
            trainingScript.isGkTraining())
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

    private void updatePlayerVelocity()
    {
        float timeDiff = Time.time - playerPrevPosTime;
        float distDiff =
            Vector2.Distance(
            new Vector2(rb.transform.position.x, rb.transform.position.z),
            new Vector2(playerPrevPos.x, playerPrevPos.z));

        playerVelocity = distDiff / Mathf.Max(0.000000000001f, timeDiff);

        //print("PLAYERVELOCITY " + playerVelocity + " timeDiff " + timeDiff + " distDiff " + distDiff);

        playerPrevPos = rb.transform.position;
        playerPrevPosTime = Time.time;

    }

    private void updateShotPos()
    {
        float dist = 0.0f;
        RaycastHit hit;
        Vector3 hitPoint = new Vector3(0.0f, 0.0f, 14.0f);

        //print("DEBUG1 STARTHRNEW UPDATE SHOT START " + startPos + " MID " + midPos + " END " + endPos);

        midPos = updateMidTouchPos(startPos, endPos);
        //print("DEBUG12345XA MIDPOS " + midPos);
        startPos3 = ballRb[activeBall].transform.position;
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
        }
        else
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

    public float getTimeOfShot()
    {
        return timeToShot;
    }

    public float getMaxTimeToShot()
    {
        return maxTimeToShot;
    }

    public bool doesGameStarted()
    {
        return gameStarted;
    }

    public bool doesGameEnded()
    {
        return gameEnded;
    }

    public bool isTrainingEnable()
    {
        return isTrainingActive;
    }

    public bool isBonusEnable()
    {
        return isBonusActive;
    }
 
    public void setTimeToShot(float val)
    {
        timeToShot = val;
    }

    private void printGameEventsInfo(string info)
    {
        writeToInfoBar(info, 2f);

        //speedShotText.text = info;
        //shotBar.fillAmount = 1f;
        gameEventLastMsgPrintTime = Time.time;
    }

    private void clearGeneralInformtionText()
    {
        if ((Time.time - gameEventLastMsgPrintTime) > 2.2f)
        {
            speedShotText.text = "";
        }
    }

    private bool updateTimeToShot(ref float prevZballPos, ref float timeToShot)
    {
        if ((ballRb[activeBall].transform.position.z > 0.0f && prevZballPos < 0.0f) ||
            (ballRb[activeBall].transform.position.z < 0.0f && prevZballPos > 0.0f) ||
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
            if (ballRb[activeBall].transform.position.z > 0.0f)
            {
                if (cpuPlayer.getShotActive() ||
                    cpuPlayer.getPreShotActive())
                {
                    return false;
                }
            }
            else
            {
                if (touchCount > 0 ||
                    shotActive ||
                    preShotActive)
                    return false;
            }

            return true;
        }

        timeToShot += Time.deltaTime;
        prevZballPos = ballRb[activeBall].transform.position.z;

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

        if (ballRb[activeBall].transform.position.z < 0.0f && ballPrevPosition.z < 0.0f)
            matchStatistics.setBallPossession("teamA", deltaTime);
        else
        {
            if (ballRb[activeBall].transform.position.z > 0.0f && ballPrevPosition.z > 0.0f)
            {
                matchStatistics.setBallPossession("teamB", deltaTime);
            }
        }

        ballPrevPosition = ballRb[activeBall].transform.position;
    }

    public int getMatchTimeMinute()
    {
        return minuteOfMatch;
    }

    private bool updateGameTime()
    {
        float virtualTimeSeconds = currentTimeOfGame * (90.0f * 60.0f / timeOfGameInSec);
        int minutes = (int) Math.Ceiling(virtualTimeSeconds / 60);
        int seconds = (int) Math.Ceiling(virtualTimeSeconds % 60);
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

        //minutesTime.Clear();
        //secondsTime.Clear();
        //mainTimeStr.Clear();
        string minutesTime = "", secondsTime = "";
        if (minutes < 10)
            minutesTime = "0";
        ///minutesTime.Append("0");

        if (seconds < 10)
            secondsTime = "0";
            //secondsTime.Append("0");
        //mainTimeStr.Append(minutesTime);
        //mainTimeStr.Append(":");
        // mainTimeStr.Append(secondsTime);

        minutesTime = minutesTime + minutes.ToString();
        secondsTime = secondsTime + seconds.ToString();
        //mainTimeStr.ToString();
        mainTimeText.text = minutesTime + ":" + secondsTime;
        minuteOfMatch = minutes;
        currentTimeOfGame += Time.deltaTime;

        //print("CURRENT TIME OF GAME " + currentTimeOfGame + " MAX " + timeOfGameInSec + 
        //   " virtualTimeSeconds " + virtualTimeSeconds);

        if (isShotActive() ||
            isPreShotActive() ||
            cpuPlayer.getShotActive() ||
            cpuPlayer.getPreShotActive())
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
        score1Text.text = Globals.score1.ToString();
        score2Text.text = Globals.score2.ToString();     
    }

    private void setTimesText()
    {
        mainTimeText.text = "00:00";
        timeToShotText.text = "0";
    }

    public void setTimeToShotText(string text)
    {
        timeToShotText.text = text;
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
                           isCpu,
                           ref activeBall);
        }
        else
        {
            isOnBall = isPlayerOnBall(
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           rb,
                           "move",
                           isCpu,
                           ref activeBall);
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
                //ballRb[activeBall].position = rb.transform.position + updateBallPos;
                if (Mathf.Abs(rb.transform.position.z) >= 13.50f)
                {
                    ballRb[activeBall].transform.position = rb.transform.position +
                        (rb.transform.forward / 2.2f);
                    //new Vector3(rb.transform.forward.x / 2.2f,
                    //            BALL_MIN_VAL,y,
                    //           rb.transform.forward.z / 2.2f);
                }
                else
                {
                    ballRb[activeBall].transform.position = rb.transform.position +
                        (rb.transform.forward / 1.4f);
                    //new Vector3(rb.transform.forward.x / 1.4f,
                    //            BALL_NEW_RADIUS,
                    //            rb.transform.forward.z / 1.4f);
                }

                //if (rbRightToeBase.transform.position.z < 0)
                /*print("ENTERPOSITION CLEAN BODY MAIN UPDATE " 
                    + ballRb[activeBall].position + " rb.transform.position " 
                    + rb.transform.position.ToString("F4")
                    + " rb.transform.forward "  
                    + rb.transform.forward
                    + " (rb.transform.forward / 1.4f) " 
                    + (rb.transform.forward / 1.4f)
                    + " DISTANCE " + 
                        Vector2.Distance(
                        new Vector2(rb.transform.position.x, rb.transform.position.z),
                        new Vector2(ballRb[activeBall].position.x, ballRb[activeBall].position.z)));*/
                break;

            default:
                break;
        }

        if (ballRb[activeBall].transform.position.y < BALL_MIN_VAL.y)
            ballRb[activeBall].transform.position = new Vector3(ballRb[activeBall].transform.position.x,
                                                    BALL_MIN_VAL.y,
                                                    ballRb[activeBall].transform.position.z);

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
             new float[] {50f, 5.6f, 17.0f, 5.6f, 9.0f, -20.95f, 3.0f, 3.7f, 5.5f},
             new float[] {45f, 4.0f, 15.0f, 4.0f, 4.5f, -17.2f, 3.0f, 2.6f, 2.8f}};

    /*float[][] cameraSettings = new float[][] {
             new float[] {50f, 5.6f, 17.0f, 5.6f, 9.0f, -21.55f, 3.0f, 3.7f, 5.5f},
             new float[] {50f, 5.6f, 17.5f, 5.6f, 9.0f, -20.90f, 3.0f, 4.0f, 5.5f},
             new float[] {45f, 4.0f, 15.0f, 4.0f, 4.5f, -17.2f, 3.0f, 2.6f, 2.8f},
             //new float[] {60f, 3.0f, 18.5f, 5.6f, 8.0f, -19.5f, 2.5f, 4.7f, 5.0f},
             new float[] {40f, 7.95f, 15.5f, 7.95f, 12.0f, -23.7f, 2.5f, 4.8f, 6.3f},
             new float[] {50f, 5.6f, 17.5f, 5.6f, 9.0f, -20.90f, 3.0f, 4.0f, 5.5f}};
    */
  
    public void cameraChanged(bool noLerpMove)
    {
        int cameraIdx = cameraButton.getCameraIdx();

        m_MainCamera.transform.eulerAngles =
              new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
        cameraComp.fieldOfView =
               cameraSettings[cameraIdx][0];
        cameraMovement(noLerpMove, -1);
    }

    public void cameraChanged(bool noLerpMove, int cameraIdx)
    {
        m_MainCamera.transform.eulerAngles =
              new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
        cameraComp.fieldOfView =
               cameraSettings[cameraIdx][0];
        cameraMovement(noLerpMove, cameraIdx);
    }

    ///Vector3[] cameraVel = new Vector3[10];

    Vector3 cameraVel = Vector3.zero;

    public void cameraMovement(bool noLerpMove, int camIdx)
    {

        //cameraMovementPortrait(noLerpMove, camIdx);
        ///return;

        int cameraIdx = cameraButton.getCameraIdx();
        if (camIdx != -1)
            cameraIdx = camIdx;

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

            m_MainCamera.transform.eulerAngles =
                new Vector3(
                    cameraSettings[cameraIdx][2] - (Mathf.InverseLerp(-10, 0f, rb.transform.position.z) * 4f), 0.0f, 0.0f);

            if (Mathf.Abs(rb.transform.position.z) > 10f)
            {
                yDist = 1f;
                m_MainCamera.transform.eulerAngles =
                    new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
            }


            cameraComp.fieldOfView =
                Mathf.Min(47f, 41f + (Mathf.Abs(rb.transform.position.z)));

            //cameraComp.GetComponent<Camera>().fieldOfView =
            //    Mathf.Min(47f, 41f + (Mathf.Abs(rb.transform.position.z)));
        }

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

            float shakeX = 0f;
            float shakeY = 0f;
            float shakeZ = 0f;
            if (gameStarted &&
                powersScript.getIsCameraDownShakeActive())
            {

                shakeX = UnityEngine.Random.Range(-1f, 1f) * 0.6f;
                shakeY = UnityEngine.Random.Range(-1f, 1f) * 0.6f;
                shakeZ = UnityEngine.Random.Range(-1f, 1f) * 0.6f;
                m_MainCamera.transform.eulerAngles = new Vector3(
                                                     m_MainCamera.transform.eulerAngles.x + (shakeX * 3.5f),
                                                     m_MainCamera.transform.eulerAngles.y + (shakeY * 3.5f),
                                                     m_MainCamera.transform.eulerAngles.z + (shakeZ * 3.5f));

            }

            Vector3 newCameraPos = m_MainCamera.transform.position =
                        new Vector3(
                    Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f) +
                                shakeX,
                               Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f) + shakeY,
                               Mathf.Lerp(m_MainCamera.transform.position.z,
                               rb.transform.position.z - zDist,
                               0.2f));

            //float yAngle = 0f;
            //if (Mathf.Abs(rb.transform.position.x) > 15f)
            //{
            //    yAngle = 10f;
            //    m_MainCamera.transform.eulerAngles = new Vector3(0f, yAngle, 0f);
            //}


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

    float[][] cameraSettingsPortrait = new float[][] {
            new float[] {50f, 20.6f, 20.0f, 20f, 24.0f, -35.00f, 3.0f, 20f, 20f, 0f}
    };
    
    public void cameraMovementPortrait(bool noLerpMove, int camIdx)
    {       
        int cameraIdx = cameraButton.getCameraIdx();
        if (camIdx != -1)
            cameraIdx = camIdx;

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

            m_MainCamera.transform.eulerAngles =
                new Vector3(
                    cameraSettingsPortrait[cameraIdx][2] - (Mathf.InverseLerp(-10, 0f, rb.transform.position.z) * 4f), cameraSettingsPortrait[cameraIdx][9], 0.0f);

            if (Mathf.Abs(rb.transform.position.z) > 10f)
            {
                yDist = 1f;
                m_MainCamera.transform.eulerAngles =
                    new Vector3(cameraSettingsPortrait[cameraIdx][2], cameraSettingsPortrait[cameraIdx][9], 0.0f);
            }

            //m_MainCamera.GetComponent<Camera>().fieldOfView =
            cameraComp.fieldOfView =
                Mathf.Min(47f, 41f + (Mathf.Abs(rb.transform.position.z)));
        }

        //if (Mathf.Abs(rb.transform.position.z) < 5.5f)
        //   yDist = 0f;
        //ENDTOCHECK

        yDist = Mathf.Lerp(cameraSettingsPortrait[cameraIdx][7],
                           cameraSettingsPortrait[cameraIdx][8],
                           yDist);
        float zDist =
            cameraSettingsPortrait[cameraIdx][1] +
            Mathf.Abs(rb.transform.position.z / (cameraSettingsPortrait[cameraIdx][6]));

        zDist = Mathf.Max(zDist, cameraSettingsPortrait[cameraIdx][3]);
        zDist = Mathf.Min(zDist, cameraSettingsPortrait[cameraIdx][4]);

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

            /* x = lerp(x, target, rate)
               https://forum.unity.com/threads/camera-stutters-in-a-simple-rotation.389899/
               lerp(x, target, 1 - pow(1 - rate, 60 * dt)) 
               frame independent*/

            //zDist = 20;
            Vector3 newCameraPos = m_MainCamera.transform.position =
                        new Vector3(
                    Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f),
                               Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
                               Mathf.Lerp(m_MainCamera.transform.position.z,
                               rb.transform.position.z - zDist,
                               0.2f));

            /*Camera was moved from FixedUpdate to LateUpdate. When system is under pressure on the 
             * very high graphics settings FixedUpdate can be executed a few times but Update may be dropped due a high load.
             * if you don't use isFixedUpdate loop (you only execute once) camera will be more and more behind player 
             * , because position player was updated for instance twice but camera movement only once. 
             */


            m_MainCamera.transform.position =
                    newCameraPos;
        }

        /*max z camera position */
        if (m_MainCamera.transform.position.z < cameraSettingsPortrait[cameraIdx][5])
        {
            m_MainCamera.transform.position =
                new Vector3(m_MainCamera.transform.position.x,
                            m_MainCamera.transform.position.y,
                            cameraSettingsPortrait[cameraIdx][5]);
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


    //float cameraSpeedYRot = 0.005f;
    private void cameraMovementIntro()
    {
        //float cameraSpeed = 0.005f;
        float cameraSpeed = 0.007f;
        /*m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y + (cameraSpeed / 3.0f),
                                                          m_MainCamera.transform.position.z - 0.006f);
        m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - 
            new Vector3(0.0f, m_MainCamera.transform.eulerAngles.y - cameraSpeedYRot, 0.0f);
        cameraSpeedYRot = cameraSpeedYRot * 1.015f;*/
        //print("cameraSpeed " + cameraSpeedYRot);
        if (realTime < 0.5f)
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y,
                                                          m_MainCamera.transform.position.z - 0.00001f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.025f, 0.0f);

            return;
        }


        if (realTime > 2.0f && realTime < 7f)
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y,
                                                          m_MainCamera.transform.position.z - 0.00006f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.02f, 0.0f);
        }
        else if (realTime > 7f)
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                        m_MainCamera.transform.position.y,
                                                        m_MainCamera.transform.position.z - 0.00008f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.03f, 0.0f);
        }
        else
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y,
                                                          m_MainCamera.transform.position.z - 0.00004f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.0024f, 0.0f);
        }
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

    public float getShotSpeedMin()
    {
        return ShotSpeedMin;
    }

    public bool isPreShotActive()
    {
        return preShotActive;
    }

    public void stopBallVel()
    {
        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;
    }

    private void RblookAtBall(Rigidbody rb,
                              float speed)

    {
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        Vector3 lookDirection =
             new Vector3(ballRb[activeBall].transform.position.x - rb.transform.position.x,
                         0.0f,
                         ballRb[activeBall].transform.position.z - rb.transform.position.z);

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

    public void RblookAtWSPoint(Rigidbody rb,
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

    public void RblookAtWSPoint(Rigidbody rb,
                                Vector3 lookPoint,
                                float speed)

    {
        Vector3 lookDirection = Vector3.zero;
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);

        lookDirection = (lookPoint - rb.transform.position).normalized;
        lookDirection.y = 0.0f;

        lookOnLook = Quaternion.LookRotation(lookDirection);
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation, lookOnLook, speed);
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

            //if (isCpu)
            //    print("#DBG134 endPosORG " + endPosOrg);

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

    public Rigidbody getRb()
    {
        return rb;
    }

    public Vector3 getBallPosition()
    {
        return ball[1].transform.position;
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
                                               Vector3 cornerPoints)
    {
        float pointX = cornerPoints.x;
        float pointZ = cornerPoints.z;

        float step = 0f;
        float stepOffset = 1.0f;

        Vector3 cornerPointLeft = new Vector3(-pointX, 0.0f, pointZ);
        Vector3 cornerPointRight = new Vector3(pointX, 0.0f, pointZ);
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
                //workaround for cpu
                if (rb.transform.position.z > 0f)
                    rotatedRbToBall.transform.eulerAngles = new Vector3(0f, 180f, 0f);

                return Vector3.zero;
            }
        }
    }

    /*This function try to rotate object to look in direction as close to ball as 
     * possible by checking all points that are on left and right side of ball. 
     * It takes too corner points and check if that points are in a back rotated
     * object.
     * is that method working ?! */
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
                                    float lastTimeGkAnimPlayed)
    {

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

        if (isLobActive && !isCpu)
        {
            //Workaround to make it easier for cpu to save lob shoot
            //if (UnityEngine.Random.Range(0, 5) <= 3)
            //{
            //Debug.Log("GKLOGPOSITION before endPosOrg " + endPosOrg);
                if (endPosOrg.y >= 2.8f && endPosOrg.y <= 3.3f)
                { 
                    endPosOrg.y = UnityEngine.Random.Range(2.5f, 2.6f);
                }
                else if (endPosOrg.y >= 2.50f && endPosOrg.y < 2.8f)
                {
                    endPosOrg.y = UnityEngine.Random.Range(2.4f, 2.5f);
                }
            //Debug.Log("GKLOGPOSITION after endPosOrg " + endPosOrg);

            //}

            //if (endPosOrg.y > 2.0f)
            //    endPosOrg.y = 2.0f;
        }

        /* Shot straight */ /*CHANGE TO 0.5f back */
        if (distMidFromLine <= 1.0f && !isLobActive)
        {
            Vector3 shotDirection3D = (endPosOrg - ballInitPos).normalized;
            float velocity = 25.0f;
            if (shotSpeed != 0.0f)
                velocity = Mathf.Min(shotSpeed / 3.5f, 34.5f);

            ballShotVelocity = velocity;
            ballVelocity = shotDirection3D * velocity;
            outStartPos = ballInitPos;
            outEndPos = endPosOrg;
            outMidPos = outStartPos + ((outEndPos - outStartPos) / 2f);
            type = SHOTVARIANT.STRAIGHT;
            return true;
        }
        else
        {
            string side = "left";
            if (isCpu)
            {
                distMidFromLine = UnityEngine.Random.Range(3, CURVE_SHOT_MAX_DIST + 1);
            }

            if (endPosOrg.y < 0.65f)
            {
                distMidFromLine = UnityEngine.Random.Range(0.5f, 2f);
            }

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

            float distanceToGoal = 14f;
            if (isCpu)
            {
                distanceToGoal = Vector2.Distance(
                    new Vector2(ballInitPos.x, ballInitPos.z), new Vector2(0f, -PITCH_HEIGHT_HALF));
            }
            else
            {
                distanceToGoal = Vector2.Distance(
                    new Vector2(ballInitPos.x, ballInitPos.z), new Vector2(0f, PITCH_HEIGHT_HALF));
            }

            midPos3Local.y = 0f;
            midPos3Local.z = Vector2.Distance(new Vector2(endPosOrg.x, endPosOrg.z),
                             new Vector2(ballInitPos.x, ballInitPos.z)) / (UnityEngine.Random.Range(2f, 3f));

            if (distanceToGoal < 18f ||
                isLobActive)
            {
                midPos3Local.z = Vector2.Distance(new Vector2(endPosOrg.x, endPosOrg.z),
                                                  new Vector2(ballInitPos.x, ballInitPos.z)) / 2f;
            }

            outMidPos = TransformPointUnscaled(dummyTouchRotatedGO.transform, midPos3Local);
            outMidPos.y = endPosOrg.y / 2.0f;

            //TOCHECK
            //outMidPos.y = endPosOrg.y / UnityEngine.Random.Range(0.7f, 1f);
            //outMidPos.y = endPosOrg.y / 0.75f;
            //outMidPos.y = endPosOrg.y / 0.85f;

            //ball is half height in the middle of way
            //if (UnityEngine.Random.Range(0, 2) == 0)
            //    outMidPos.y = endPosOrg.y / 2f;
            //else
            //outMidPos.y = endPosOrg.y / UnityEngine.Random.Range(0.55f, 0.75f);
            //0.7 div 4 - ok
            //0.8 div 3 - ok
            //0.7 div 3 - ok
            //0.6 div 3 - ok
            //0.5 div 3 - ok but long distance only
            //0.8 div 2 - ok
            //0.7 div 2 - ok
            //0.6 div 2  - ok

            //outMidPos.y = endPosOrg.y / UnityEngine.Random.Range(0.75f, 0.85f);
            float minDiv = 0.55f;


            if (!isCpu)
            {
                if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.3f))
                {
                    minDiv = 0.78f;
                }
                else if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.5f))
                {
                    minDiv = 0.65f;
                }
            }
            else
            {
                if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.3f))
                {
                    minDiv = 0.70f;
                }
                else if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.4f))
                {
                    minDiv = 0.65f;
                }
            }

            outMidPos.y = endPosOrg.y / UnityEngine.Random.Range(minDiv, 0.85f);

            if (isCpu &&
                UnityEngine.Random.Range(0, 4) > 2)
            {
                outMidPos.y = endPosOrg.y / 2f;
            }

            //Old straight shoot
            //if (distMidFromLine <= 1f)
            //{
            //    midPos3Local.z = Vector2.Distance(new Vector2(endPosOrg.x, endPosOrg.z),
            //                                       new Vector2(ballInitPos.x, ballInitPos.z)) / 2f;
            //    outMidPos.y = endPosOrg.y / 2f;
            //}

            //UnityEngine.Random.Range(0.7ff);

            /*TOCHECK*/
            outMidPos.y = Math.Max(BALL_RADIUS, outMidPos.y);

            if (isLobActive)
            {

                if (!isCpu)
                    outMidPos.y = 11.0f;
                else
                {
                    outMidPos.y = 6.0f;
                    float playersDistZ = Mathf.Abs(getPlayerPosition().z) +
                                         Mathf.Abs(cpuPlayer.getRbPosition().z);

                    if (playersDistZ < 9f)
                    {
                        outMidPos.y += 1.5f;
                        if (playersDistZ < 6f)
                            outMidPos.y += 1f;
                    }
                }
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
    private float calcShotDistance(Vector3 startPos,
                                   Vector3 midPos,
                                   Vector3 endPos,
                                   SHOTVARIANT type)
    {
        if (type == SHOTVARIANT.STRAIGHT)
        {
            //print("CURRENTIME STRAIGHT");
            return Vector3.Distance(startPos, endPos);
        }
        else
        {
            //print("GKDEBUG7 CURRENTIME ENTERED " + currentTime);
            float currentTime = 0f;
            float step = 0.01f;
            Vector3 m1, m2, currPos, prevPos = startPos;
            float dist = 0;

            while (currentTime < 1.0f)
            {
                currentTime += step;
                m1 = Vector3.Lerp(startPos, midPos, currentTime);
                m2 = Vector3.Lerp(midPos, endPos, currentTime);
                currPos = Vector3.Lerp(m1, m2, currentTime);
                dist += Vector3.Distance(prevPos, currPos);
                prevPos = currPos;
            }

            return dist;
        }
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
            ballRb[activeBall].velocity = ballVelocity;
            //print("DEBUG111X ballRb[activeBall]VELOCITY STRIAGHT VELOCITY SETACTIVE " + ballRb[activeBall].velocity);
            lastBallVelocity = ballRb[activeBall].velocity;

            //print("CURRENTIME STRAIGHT");
            return false;
        }
        else
        {
            //print("GKDEBUG7 CURRENTIME ENTERED " + currentTime);
            if (currentTime >= 1.0f)
                return true;

            Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
            Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
            Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

            float delta = 1.0f - currentTime;

            ballRb[activeBall].velocity = new Vector3((currPos.x - ballRb[activeBall].transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb[activeBall].transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb[activeBall].transform.position.z) / Time.deltaTime);

            ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            lastBallVelocity = ballRb[activeBall].velocity;

            //print("DEBUG111X ballRb[activeBall]VELOCITY CURVE VELOCITY SETACTIVE " + ballRb[activeBall].velocity);
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
            ballRb[activeBall].velocity = direction3DShot * (30.0f / maxTime);
            //ballRb[activeBall].AddForce(direction3DShot * (30.0f / maxTime) * 1.8f);
            //print("ballRb[activeBall] VELOCITY" + ballRb[activeBall].velocity);
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
            //ballRb[activeBall].transform.position = currPos;
            Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

            /*ballRb[activeBall].velocity = new Vector3((currPos.x - ballRb[activeBall].transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb[activeBall].transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb[activeBall].transform.position.z) / Time.deltaTime);*/

            float delta = 1.0f - currentTime;
            //Vector3 currPos = (delta * delta * ballInitPos) + (2 * delta * currentTime * shotParabolaDirect1) + (currentTime * currentTime * shotParabolaDirect2);
            //print("VELOCITYCURRPOST " + currPos);

            ballRb[activeBall].velocity = new Vector3((currPos.x - ballRb[activeBall].transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb[activeBall].transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb[activeBall].transform.position.z) / Time.deltaTime);




            //print("ballRb[activeBall]VELOCITY " + ballRb[activeBall].velocity);
            //print("currentTime " + currentTime);
            //Mathf.Lerp(start, end, Mathf.SmoothStep(0.0, 1.0, t));

            //Vector3 vel = new Vector3(1, 2, 2);

            //Vector3 velocity = Vector3.zero;
            //ballRb[activeBall].transform.position = Vector3.SmoothDamp(ballRb[activeBall].transform.position, shotParabolaDirect1, ref velocity, 0.2f);
            //print("BALL TRANSFORM POSITION " + ballRb[activeBall].transform.position);
            //print("smoothdamp " + Vector3.SmoothDamp(ballInitPos, shotParabolaDirect2, ref velocity, 1.5f));
            //print("BALLPOSVEL " + ballRb[activeBall].velocity + " ballRb[activeBall]MAGINTUE " + ballRb[activeBall].velocity.magnitude);
        }


        //ballRb[activeBall].transform.Rotate((Vector3(0.0f, 0.0f, -10.0f) * Time.deltaTime), Space.World);
        /*llRb.transform.rotation = new Vector3(ballRb[activeBall].transform.rotation.x, 
                                                ballRb[activeBall].transform.rotation.y + 1, 
                                                ballRb[activeBall].transform.rotation.z);*/
        return Vector3.zero;
    }

    private void initFlagPositions()
    {
        int randomValue = 1;
        for (int i = 0; i < FANS_FLAG_MAX; i++)
        {
            /*No flag when traning mode */
            if (isTrainingActive ||
                isBonusActive)
            {
                isFansFlagActive[i] = false;
                fansFlagSticks[i].SetActive(false);
                continue;
            }

            randomValue = UnityEngine.Random.Range(1, 11);

            //if (randomValue > 8)
            //{
            //    isFansFlagActive[i] = false;
            //    fansFlagSticks[i].SetActive(false);            
            //    continue;
            //} else
            // {
            isFansFlagActive[i] = true;
            //}

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
        if (Globals.stadiumNumber == 2)
            return;


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
        if (Globals.stadiumNumber == 2)
            return;

        //int teamColorChoosen = Globals.stadiumColorTeamA;
        string teamColorChoosen = Globals.stadiumColorTeamA;
        if (teamHostID == 2)
            teamColorChoosen = Globals.stadiumColorTeamB;
        //teamColorChoosen = Globals.stadiumColorTeamB;

        string[] stadiumColors = teamColorChoosen.Split('|');

        string fansColor = stadiumColors[0];
        string bannerColor = stadiumColors[1];
        string fansFlagName = stadiumColors[2];

        /*if (Globals.stadiumNumber == 0)
        {
            Texture2D texturePeople =
                graphics.getTexture("stadium/fans_" + fansColor);

            Texture2D textureBanner =
                graphics.getTexture("stadium/banner_" + bannerColor);

            Material[] materials = stadium.GetComponent<Renderer>().materials;
            materials[2].mainTexture = textureBanner;
            stadium.GetComponent<Renderer>().materials = materials;

            foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (allStadiumPeople.name.Contains("crowdAnimated"))
                {
                    allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);
                }

                //if (allStadiumPeople.name.Contains("wallAround"))
                //{
                //    allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", textureBanner);
                //allStadiumPeople.GetComponent<Renderer>().materials = materials;
                //}
                if (allStadiumPeople.name.Contains("flagWall"))
                {
                    string idx = allStadiumPeople.name.Split('_')[1];
                    Texture2D textureflag =
                            graphics.getTexture("stadium/wallsFlag/banner_" + bannerColor + "_" + idx);
                    allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", textureflag);
                }
            }

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
        else */
        if (Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 0)
        {
            int FANS_FLAG_MAX = 3;
            int numOfFansActive = 32;
            int nunOfStaticFansActive = 100;
            int currentFansActive = 0;
            int currentStaticFansActive = 0;
            if (Globals.graphicsQuality.Equals("LOW"))
            {
                numOfFansActive = 23;
                nunOfStaticFansActive = 50;
            }
            else if (Globals.graphicsQuality.Equals("VERY LOW"))
            {
                numOfFansActive = 0;
                nunOfStaticFansActive = 0;
            }
  
              int randMaterial_fans = UnityEngine.Random.Range(0, 13);
              Material fansMaterial_static;
              foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
              {
                    if (allStadiumPeople.name.Contains("fan_static_"))
                    {

                        if (isBonusActive ||
                            isTrainingActive)
                        {   
                            allStadiumPeople.SetActive(false);
                            continue;
                        }

                        if ((Globals.graphicsQuality.Equals("LOW") || Globals.graphicsQuality.Equals("VERY LOW"))
                            && (currentStaticFansActive >= nunOfStaticFansActive))
                        {
                            allStadiumPeople.transform.parent.gameObject.SetActive(false);
                            continue;
                        }
                
                        currentStaticFansActive++;
                        randMaterial_fans = randMaterial_fans % 12;
                 
                        fansMaterial_static = graphics.getMaterial("stadium/fans/materials/audienceMaterial" + randMaterial_fans.ToString());
                        randMaterial_fans++;                        
                        allStadiumPeople.GetComponent<Renderer>().material = fansMaterial_static;
                        continue;
                    }
    
                    if (allStadiumPeople.name.Contains("fan_"))
                    {
                        if (currentFansActive >= numOfFansActive ||
                        isBonusActive ||
                        isTrainingActive)
                    {
                        allStadiumPeople.transform.parent.gameObject.SetActive(false);
                        continue;
                    }
                 
                    if (Globals.graphicsQuality.Equals("LOW"))
                    {
                        int randFans = UnityEngine.Random.Range(0, 3);
                        if (randFans == 0)
                        {
                            allStadiumPeople.transform.parent.gameObject.SetActive(false);
                            continue;
                        }
                        else
                            currentFansActive++;
                    }
                    else if (Globals.graphicsQuality.Equals("VERY LOW"))
                    {
                        int randFans = UnityEngine.Random.Range(0, 2);
                        if (randFans == 0)
                        {
                            allStadiumPeople.transform.parent.gameObject.SetActive(false);
                            continue;
                        }
                        else
                            currentFansActive++;
                    }

                    //string randMaterial = UnityEngine.Random.Range(1, 3).ToString();
                    string randMaterial = "1";
                    Material fansMaterial =
                          graphics.getMaterial("stadium/fans/materials/audienceMaterial" + randMaterial);

                    Texture2D texturePeople =
                         graphics.getTexture("stadium/fans/" + fansColor + "_" + randMaterial);

                    allStadiumPeople.GetComponent<Renderer>().material = fansMaterial;
                    allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);

                    //Material[] materials = stadium.GetComponent<Renderer>().materials;
                    //materials[2].mainTexture = textureBanner;
                    //stadium.GetComponent<Renderer>().materials = materials;
                }

                //print("BANNERCOLOR " + teamColorChoosen);
                if (Globals.stadiumNumber == 1)
                {
                    if (allStadiumPeople.name.Contains("wallAround_"))
                    {
                        string idx = allStadiumPeople.name.Split('_')[1];                      
                        Texture2D texturePeople =
                                graphics.getTexture("stadium/wallsFlag/banner_" + bannerColor + "_" + idx);
                        //print("PATH " + ("stadium/wallsFlag/banner_" + bannerColor + "_" + idx));
                        allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);
                    }
                } else
                {
                    if (allStadiumPeople.name.Contains("flagWall"))
                    {
                        string idx = allStadiumPeople.name.Split('_')[1];
                        if (bannerColor.Contains("lightblue") ||
                            bannerColor.Contains("orange"))
                        {
                            allStadiumPeople.GetComponent<Renderer>().material.SetFloat("_Metallic", 0.8f);
                        }

                        Texture2D textureflag =
                                graphics.getTexture("stadium/wallsFlag/banner_" + bannerColor + "_" + idx);
                        allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", textureflag);
                    }
                }

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
        }
    }

    public int getTeamHostId()
    {
        return teamHostID;
    }

    public void OnCollisionEnter(Collision other)       
    {
        //Debug.Log("Collision " + other.collider.name);
    }

    public float getPitchWidth()
    {
        return PITCH_WIDTH_HALF;
    }

    public float getPitchHeight()
    {
        return PITCH_HEIGHT_HALF;
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
    /* private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 2.0f, float width = 0.15f)
     {
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
     }*/

    private void DrawLine(Vector3 start, Vector3 end, ref Material material, Color color, float duration = 2.0f, float width = 0.15f)
    {
        //print("VECTORXASD " + start + " end " + end);
        //start.z = -3.0f;
        //end.z = -3.0f;
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer touchLine = myLine.GetComponent<LineRenderer>();
        touchLine.SetVertexCount(2);
        touchLine.receiveShadows = false;
        touchLine.castShadows = false;

        //touchLine.SetColors(color, color);
        //touchLine.material = new Material(Shader.Find("Sprites/Default"));
        //touchLine.material.SetColor("_TintColor", new Color(1, 1, 1, 0.5f));


        //touchLine.material = new Material(Shader.Find("Standard"));
        //touchLine.material.SetOverrideTag("RenderType", "Transparent");
        Debug.Log("drawLine material " + material);
        touchLine.material = material;
        // touchLine.material.SetColor("_TintColor", new Color(1, 1, 1, 0.5f));

        Color c1 = color;
        Color c2 = new Color(0, 0, 1, 0.1f);
        //touchLine.SetColors(color, color);

        touchLine.SetPosition(0, start);
        touchLine.SetPosition(1, end);
        touchLine.SetWidth(width, width);
        touchLine.startWidth = width;
        touchLine.endWidth = width;
        //touchLine.useWorldSpace = false;
        GameObject.Destroy(myLine, duration);
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


    public Vector3 getBallRbLeftSide(int ballIdx)
    {
        return ballRbLeftSide[ballIdx].transform.position;
    }

    public Vector3 getBallRbRightSide(int ballIdx)
    {
        return ballRbRightSide[ballIdx].transform.position;
    }


    //this might be unintentional click
    private bool checkIfClickTooFar(Touch touch) {
         Vector3 gkPlayerTouch = INCORRECT_VECTOR;
         Vector3 realHitPlaceLocal;

         Ray ray = m_MainCamera.ScreenPointToRay(touch.position);

         getRotatedRbToBall(cpuPlayer.getBallInit(),
                            rb,
                            ref tmpRotGO,
                            getGkCornerPoints());
                            
        Plane tmpRotPlane = new Plane(
            tmpRotGO.transform.forward,
            tmpRotGO.transform.position);
            
        float dist = 0.0f;
        if (tmpRotPlane.Raycast(ray, out dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            gkPlayerTouch = hitPoint;
        } else {
            return false;
        }

        Vector3 clickedRbRotatedLS = InverseTransformPointUnscaled(
                            tmpRotGO.transform,
                            gkPlayerTouch);

        /*Debug.Log("DBGclick forward global pos " + 
            InverseTransformPointUnscaled(tmpRotGO.transform, tmpRotGO.transform.forward)
            + " forward " + tmpRotGO.transform.forward);*/



        clickedRbRotatedLS.z =  0.0f;
	   
        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       tmpRotGO,
                                                       cpuPlayer.getOutShotStart(),
                                                       cpuPlayer.getOutShotMid(),
                                                       cpuPlayer.getOutShotEnd(),
                                                       false);

         if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
         {
            realHitPlaceLocal = INCORRECT_VECTOR;
         }
          
         gkDistRealClicked = 
            Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal);

         /*Debug.Log("#DBGclick " + clickedRbRotatedLS + " realHit " + realHitPlaceLocal
         + " dist " + (Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal)
         + " rb.transPos " + rb.transform.position + " rbRot " + rb.transform.eulerAngles
         + " tmpRotGOPos " + tmpRotGO.transform.position
         + " tmpRotGoRot" + tmpRotGO.transform.eulerAngles)
         + " gkPlayerTouch " + gkPlayerTouch
         + " touch.position " + touch.position);*/

         if ((gkDistRealClicked > 7f) &&
             (realHitPlaceLocal != INCORRECT_VECTOR))
         {
            return true;
         }

         return false;
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
                         Vector3 cornerPoints,
                         bool isExtraGoals)
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
        if (checkIfAnyAnimationPlaying(animator, 1.0f, excluded))
        {
            //if (checkIfAnyAnimationPlaying(animator, 1.0f)) {       
            return;
        }

        //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);
        //Vector3 ballInLocalRb = InverseTransformPointUnscaled(
        //rb.transform, ballRb[activeBall].transform.position);


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
                    if ((Time.time - numberOfCorrectGkclickLastTimeUpdate) >= 5f)
                    {
                        numberOfCorrectGkclickLastTimeUpdate = Time.time;
                        numberOfCorrectGKClick++;
                    }
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

                //whereBallHit =
                //    TransformPointUnscaled(rotatedRbToBall.transform, localSpace);
                //whereBallHitDown =
                //    TransformPointUnscaled(rotatedRbToBall.transform,
                //                 new Vector3(localSpace.x, Mathf.Max(localSpace.y - 0.4f, 0f), 0f));
                //whereBallHitUp =
                //    TransformPointUnscaled(rotatedRbToBall.transform,
                //              new Vector3(localSpace.x, localSpace.y + 0.4f, 0f));

                //print("DISTHANDFROMBALL_#  CURVE PLAYER whereBallHit " + whereBallHit +
                //    " whereBallHitDown " + whereBallHitDown
                //    + " whereBallHitUp " + whereBallHitUp);

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

                Vector3 ballAway = new Vector3(ballRb[activeBall].transform.position.x + (ballRb[activeBall].velocity.x * 10.0f),
                                               ballRb[activeBall].transform.position.y + (ballRb[activeBall].velocity.y * 10.0f),
                                               ballRb[activeBall].transform.position.z + (ballRb[activeBall].velocity.z * 10.0f));

                float distHit = float.MaxValue;
                Ray rayBall = new Ray(
                   ballRb[activeBall].transform.position,
                   (ballAway - ballRb[activeBall].transform.position).normalized);

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
                    timeToHitX = Mathf.Abs(hitPointWorld.x - ballRb[activeBall].transform.position.x) / Mathf.Abs(ballRb[activeBall].velocity.x);
                    timeToHitY = Mathf.Abs(hitPointWorld.y - ballRb[activeBall].transform.position.y) / Mathf.Abs(ballRb[activeBall].velocity.y);
                    timeToHitZ = Mathf.Abs(hitPointWorld.z - ballRb[activeBall].transform.position.z) / Mathf.Abs(ballRb[activeBall].velocity.z);
                }
                else
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
        else
        {

            //ISCPU
            //if (ballRb[activeBall].transform.position.z > Mathf.Abs(PITCH_HEIGHT_HALF))
            //    return;

            /*##############CPU IMPLEMENTATION ############################*/
            /*if (!isLobActive)
            {
                rotatedRbToBall = getRotatedRbToBall(getBallInit(),
                                                     rb,
                                                     ref rotatedRbToBall,
                                                     cornerPoints);
            } else
            {
                rotatedRbToBall.transform.position = rb.transform.position;
                rotateGameObjectTowardPoint(ref rotatedRbToBall,
                                            ballRb[activeBall].transform.position,
                                            1f);
            }*/

            //if (!isLobActive)
            //{

            if (!isLobActive)
            {
                rotatedRbToBall = getRotatedRbToBall(getBallInit(),
                                                     rb,
                                                     ref rotatedRbToBall,
                                                     cornerPoints);
            }
            else
            {
                /*if you already found correct position doesn't rotate any longer*/
                if (!gkLobPointReached)
                {
                    /*calcGkCorrectRotationToBall(//ballRb[activeBall].transform.position,
                                                getBallInit(),
                                                rb,
                                                ref rotatedRbToBall,
                                                endPosOrg.x,
                                                endPosOrg.z);*/
                    rotatedRbToBall = getRotatedRbToBall(getBallInit(),
                                                rb,
                                                ref rotatedRbToBall,
                                                cornerPoints);
                }
            }

            //if (gkLobPointReached)
            //    DrawPlane(rotatedRbToBall.transform.position, 
            //              rotatedRbToBall.transform.forward);

            //            bool ret = false;

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
                                        ref localSpace);


                //if (!ret)
                //    return;
            }
            else
            {

                straightShotHitPoint(rotatedRbToBall,
                                     ref localSpace,
                                     ref hitPointWorld,
                                     ref timeToHitZ);


                /*if no correct intersection found. Stop it*/
                //if (!ret)
                //    return;
            }

            Vector3 ballInLocalRb = InverseTransformPointUnscaled(
                                    rotatedRbToBall.transform, ballRb[activeBall].transform.position);

            //if (isLobActive)
            //ballInLocalRb = InverseTransformPointUnscaled(
            //                rb.transform, ballRb[activeBall].transform.position);

            distX = Mathf.Abs(localSpace.x);
            distY = Mathf.Abs(localSpace.y);

            if (initDistX == -1)
                initDistX = distX;

            /*if distance too ball is too big so no chance to catch a ball,
             * try to run toward hit point*/

            //Vector3 lobPointToGo = endPosOrg;
            //bool isLobTooHighToCatch = false;
            bool isGkTooFarToCatch = false;
            if (distX > 9f && !isExtraGoals)
                isGkTooFarToCatch = true;

            if (isLobActive)
            {
                if (rb.position.z > 9f &&
                    !isShotOnTarget(endPosOrg, goalSizesCpu * 1.2f))
                {
                    rb.velocity = Vector3.zero;
                    RblookAtBall(rb, 10f);
                    return;
                }

                if (gkSaveLobShot(rb,
                                  rotatedRbToBall,
                                  animator,
                                  hitPointWorld,
                                  endPosOrg,
                                  ref gkLobPointReached,
                                  distX,
                                  initDistX,
                                  timeToHitZ,
                                  isGkTooFarToCatch,
                                  cpuPlayer.speed,
                                  cpuPlayer.backSpeed))
                    //timeToHitZ > MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
                    return;
            }
            else
            {
                /*print("DEBUG2345ANIMPLAY init dist X "
                   + initDistX + " distX " + distX + " isGkTooFarToCatch "
                   + isGkTooFarToCatch + " TIMETOHITZ " + timeToHitZ
                   + " hitPointWorld " + hitPointWorld + " rb.eulerangles " + rb.transform.eulerAngles);*/

                if (!gkRunPosReached &&
                   (isGkTooFarToCatch ||
                   (initDistX > 8f && distX > 3f && timeToHitZ > 0.35f)))
                {
                    //print("DEBUG2345ANIMPLAY GOTOPOS!!! ");
                    goAndRotateTowardRun(rb,
                                         animator,
                                         //"3D_run_cpu",
                                         "3D_run",
                                         hitPointWorld,
                                         cpuPlayer.speed,
                                         20f);

                    return;
                }

                gkRunPosReached = true;
            }


            //Debug.Log("GKLOGPOSITION " + localSpace + " shotvariant " + shotvariant);
            /*print("DEBUG2345ANIMPLAY gkRunPosReached " + gkRunPosReached
                + " HITPOINT "
                + hitPointWorld + " TIMEZ " + timeToHitZ
                + " RB TRANSFORM EULERANGLES " + rb.transform.eulerAngles);

            print("DEBUGTOOFAR timeToHitZ " + timeToHitZ
                + " isLobActive " + isLobActive
                + " hitPointWorld Y "
                + hitPointWorld.y + " LOCALsPACE " + Mathf.Abs(localSpace.x));*/


            //if (isCpuPlayer)
            //print("DEBUG123XYGKA BEFORE LOCAL SPACE " + localSpace + " MAXGK " + MAX_GK_OFFSET_CPU);
            //correctLocalOffsetMax(ref localSpace, shotvariant, isCpuPlayer);
            //print("DEBUG123XYGKA AFTER LOCAL SPACE " + localSpace);


            /*if (isCpuPlayer) {
                print("DEBUG2345ANIMPLAY #### TIMETOHITZ " + timeToHitZ + " gkLobPointReached "
               + gkLobPointReached + " RB POS " + rb.transform.position
               + timeToHitX + " TIMETOHITZ " + timeToHitZ + " TIMETOHITY " + timeToHitY +
               " DIFF " + Mathf.Abs(localSpace.x - ballRb[activeBall].position.x) + " VELOCITY " + ballRb[activeBall].velocity + " HITPOINT " + localSpace +
               " ballRb[activeBall].position " + ballRb[activeBall].position + " ballRb[activeBall].velocity.z + " + ballRb[activeBall].velocity.z + " ANIMTIME " +
                 animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " cpuLeftPalm pos"
                 + cpuLeftPalm.transform.position
                 + " cpuRightPalm pos " + cpuRightPalm.transform.position
                 + " ballRb[activeBall].position " + ballRb[activeBall].position + " DISTX " + distX
                 + " hitPointWorld " + hitPointWorld);
            }*/

            if (!gkLock)
                ballBeforeLookPos = getBallInit();

            gkLock = true;
            if (!isLobActive &&
                RblookAtDirection(rb,
                                  rotatedRbToBall,
                                  //ballBeforeLookPos, 
                                  2.0f,
                                  20.0f))
            {
                //print("DEBUG2345ANIMPLAY ROTATION timeToHitZ " + timeToHitZ);
                return;
            }
            gkLock = false;

            if ((!gkLobPointReached || !isLobActive) &&
                timeToHitZ > GK_SIDE_MOVE_MIN_TIME)
            {
                if (distX < 0.25f) return;

                if (localSpace.x < 0.0f)
                {
                    stepSide(animator, rb, Mathf.Max(-3.0f, localSpace.x),
                             "3D_GK_step_left_no_offset", ref stepSideAnimOffset);
                }
                else
                {
                    stepSide(animator, rb, Mathf.Min(3.0f, localSpace.x),
                             "3D_GK_step_right_no_offset", ref stepSideAnimOffset);
                }

                return;
            }

            //print("DEBUGTOOFAR timeToHitZ AFTER ALL " + timeToHitZ);

            /* do nothing - ball is too far to catch */
            if (!isExtraGoals &&
                gkTooFarToCatch(distX, ballInLocalRb, MAX_GK_OFFSET_CPU + 4.0f))
            {
                return;
            }
        }

        /*if (isCpuPlayer)
            print("DEBUG2345ANIMPLAY CORRECT BEFORE " + localSpace + " DISTX " + distX + " localSpace " + localSpace);*/
        correctLocalOffsetMax(ref localSpace, shotvariant, isCpuPlayer);


        //if (!isCpuPlayer)
        //    print("GKRBPOS " + rb.transform.position + "DISTX " + localSpace);

        //print("DEBUGLASTTOUCHLAKI CORRECT AFTER " + localSpace);

        /*if (isCpuPlayer)
            print("#DBGCPUPLAYERCOL " + localSpace);
        else
            print("#DBGNOCPUPLAYERCOL " + localSpace);*/


        distX = Mathf.Abs(localSpace.x);
        distY = Mathf.Abs(localSpace.y);

        if (localSpace.x < -0.45f)
        {
            animName = "left";
        }
        else
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

        if (animName.Contains("straight"))
        {
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
        }
        else
        {
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
                    if (distX >= 2.0f && distX <= 2.5f && distY < 2.5f)
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
            if (isCpuPlayer)
            {
                cpuAnimAdjustSpeed = calcGkAnimSpeedBaseOnVelocity(ballRb[activeBall],
                                                           shotvariant,
                                                           ShotSpeedMin,
                                                           ShotSpeedMax,
                                                           timeofBallFly,
                                                           isCpuPlayer,
                                                           animName,
                                                           localSpace);
            }
            else
            {
                /* when player clicked to late, adjust 
                 * cpuAnimAdjustSpeed until MAX_ANIM_GK_PLAYER_SPEED*/

                cpuAnimAdjustSpeed = calcGkAnimSpeedBaseOnTimeToHit(ref animName,
                                           timeToHitZ,
                                           ballRb[activeBall],
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
            //    print("TESTPOINT121 CPU ANIM SPEED CALCULATED " + cpuAnimAdjustSpeed  + " VEL " + ballRb[activeBall].velocity.z);
        }

        if (isCpuPlayer)
        {
            if (calculateTimeToGkAction(ref animName,
                                        timeToHitZ,
                                        ballRb[activeBall],
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
                                        ballRb[activeBall],
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
            //Vector3 ballInLocalToRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);
            Vector3 ballInLocalToRb = InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position);
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
        }
        else if (animName.Contains("mid"))
        {
            offset = 1.40f;
            //it was 2.30 before
            if (animName.Contains("punch"))
                offset = 2.75f;
        }
        else if (animName.Contains("down"))
        {
            offset = 1.30f;
            if (animName.Contains("punch"))
                offset = 2.50f;
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
            }
            else
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
        timeToHitZ = ((realHitPlaceLocal.z * timeofBallFly) - passedShotFlyTime) / 1000f;

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
        //Debug.Log("GKLOGPOSITION realHitPosition calculated local " + realHitPlaceLocal
        //       + " rotatedRb " + rotatedRb.transform.eulerAngles
        //       + " rotatedpos " + rotatedRb.transform.position);

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

    public int getActiveBall()
    {
        return activeBall;
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

        Vector3 ballAway = new Vector3(ballRb[activeBall].transform.position.x + (ballRb[activeBall].velocity.x * 10.0f),
                                       ballRb[activeBall].transform.position.y + (ballRb[activeBall].velocity.y * 10.0f),
                                       ballRb[activeBall].transform.position.z + (ballRb[activeBall].velocity.z * 10.0f));

        float distHit = float.MaxValue;
        Ray rayBall = new Ray(
           ballRb[activeBall].transform.position,
           (ballAway - ballRb[activeBall].transform.position).normalized);

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
            timeToHitZ = Mathf.Abs(hitPointWorld.z - ballRb[activeBall].transform.position.z) / Mathf.Abs(ballRb[activeBall].velocity.z);
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
           + " ballHitPoint "
           + ballHitPoint
           + getAngleBeetweenObjects(rb, rotatedRbToBall));*/


        //print("DEBUG2345ANIMPLAY insde lob save gkLobPointReached " + gkLobPointReached + " distX " + distX
        //    + " initDistX  " + initDistX + " shotEndPos " + shotEndPos + " isGkTooFarToCatch " +
        //    isGkTooFarToCatch + " timeToHitZ " + timeToHitZ + " isGoFotwardToBallActive " + isGoFotwardToBallActive);

        if (!isGoFotwardToBallActive)
        {
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
              new Vector3(ballRb[activeBall].transform.position.x - rb.transform.position.x,
                          0.0f,
                          ballRb[activeBall].transform.position.z - rb.transform.position.z);

        Vector3 lobPointToGo = moveOutFromWall(
                               ballRb[activeBall].transform.position, shotEndPos, 0.3f);

        float dist = Vector3.Distance(rb.transform.position,
                                      lobPointToGo);


        /*print("DEBUG2345ANIMPLAY INSIDE LOB  " + lobPointToGo + " dist " + dist
        + " HITPOINT" + ballHitPoint
        + " gkLobPointReached " + gkLobPointReached + " distX " + distX
        + " ballRb[activeBall].transform.position " + ballRb[activeBall].transform.position
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

        //if (Mathf.Abs(rb.transform.position.z) >= 10.0f)
        backRunSpeed = getBackSpeed(Globals.level, speed);
        //Debug.Log("GKLOGPOSITION getbackspeed " + backRunSpeed);


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

    public bool isShotCurve()
    {
        if (shotvariant == SHOTVARIANT.CURVE)
            return true;
        return false;
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
            }
            else
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
                                 ballRb[activeBall],
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

            //TODO fix it
            /*if (Globals.level == 2)
            {
                Globals.level = 3;
                levelNorm = getLevelInterpolationReverse();
                skillNorm = getSkillInterpolationReverse(defenseSkillsCpu);
                animatorDelay = (levelNorm + skillNorm) / 2.0f;
                Globals.level = 2;
            }*/


            //baseAnimSpeed = 2.0f;
            baseAnimSpeed = 1.5f;
            if (animName.Contains("punch"))
            {
                //baseAnimSpeed = 3.0f;
                baseAnimSpeed = 2.5f;

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
        if (volleyButton.getNonClickedColorAlpha() > 0.95f)
        {
            volleyButton.changeNonClickedColorAlpha(0.4f);
            lobButton.changeNonClickedColorAlpha(0.4f);
            //overheadButton.changeNonClickedColorAlpha(0.4f);
        }

        if (volleyButton.getClickedColorAlpha() > 0.95f)
        {
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
        prob += (int)Mathf.Round(skillsProb);

        if ((Mathf.Abs(localHitPoint.x) < minOffset ||
             Mathf.Abs(ballRb[activeBall].velocity.z) < minVel) ||
             prob < randProb)
        {
            delayTime = 0.0f;
            //print("DEBUGZ1237 " + delayTime + " zeros");
        }

        /*print("DEBUG123XYGKA delay time " + delayTime + " localHitPoint " 
            + localHitPoint + " minOffset " + minOffset 
            + " minVel " + minVel + " REALVEL " + Mathf.Abs(ballRb[activeBall].velocity.z)
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
            }
            else if (animName.Contains("mid"))
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
        if (isShotActive() || cpuPlayer.getShotActive())
        {
            float goalXOff = goalXOffset + 0.3f;
            if (cpuPlayer.getShotActive())
                goalXOff = goalXOffsetCpu + 0.3f;

            if ((Mathf.Abs(ballRb[activeBall].transform.position.x) > goalXOff &&
                 Mathf.Abs(ballRb[activeBall].transform.position.x) < (goalXOff + 5.0f)) &&
                (Mathf.Abs(ballRb[activeBall].transform.position.z) > PITCH_HEIGHT_HALF &&
                 Mathf.Abs(ballRb[activeBall].transform.position.z) < (PITCH_HEIGHT_HALF + 3.0f)))

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
            int chantRandom = UnityEngine.Random.Range(3, 5);
            //audioManager.Play("fanschant" + chantRandom.ToString());

            ///if (Globals.stadiumNumber == 1)
            //{
                if (chantRandom == 3) audienceReactionsScript.playOneOfApplause(10f, 1.8f);
                if (chantRandom == 4) audienceReactionsScript.playCelebration3();
            //}

            matchStatisticsPanel.SetActive(true);
            matchStatisticsPanel.GetComponent<Image>().fillAmount = matchStatisticsPanelFillAmount;
        }
        else
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
            matchStatisticsNext.SetActive(true);
        }

        //if (realTime > 7.0)
        //    matchStatisticsNext.SetActive(true);

        if (realTime > 9.0f)
        {
            audioManager.Stop("fanschantBackground2");
            audioManager.Play("music2");
        }
    }

    public void matchEndedOnClick()
    {
        //NationalTeams teams = new NationalTeams();

        if (Globals.isMultiplayer)
        {
            Globals.dontCheckOnlineUpdate = false;
            Globals.loadSceneWithBarLoader("multiplayerMenu");
            return;
        }

        if (Globals.isLevelMode)
        {
            if (Globals.score1 > Globals.score2)
            {
                Globals.levelNumber++;
                PlayerPrefs.SetInt("levelNumber", Globals.levelNumber);
            }
            Globals.loadSceneWithBarLoader("levelsMenu");
            return;
        }

        Teams teams = new Teams("NATIONALS");

        Globals.recoverOriginalResolution();

        if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return;
        }

        if (teams.isAnyNewTeamUnclocked() ||
            ((Globals.numMatchesInThisSession % 2) == 0) ||
            (Globals.numMainMenuOpened == 1 &&
             Globals.numMatchesInThisSession <= 3))
        {
            SceneManager.LoadScene("rewardNewTeam");
        }
        else
        {
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


    public void setBallShotVel(float speed)
    {
        ballShotVelocity = speed;
    }

    public float getBallShotVel()
    {
        return ballShotVelocity;
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

            /*if (!isCpu)
                print("DBGNOCPUPLAYERCOL matchTarget prep normTime " + normTime + " gkEndPos " + gkEndPos +
                    " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);
            else
                print("DBGCPUPLAYERCOL matchTarget prep normTime " + normTime + " gkEndPos " + gkEndPos +
                   " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);*/

            if (normTime <= timeToCorrectPos)
            {
                matchInitSavePos = false;

                if (animator.isMatchingTarget || animator.IsInTransition(0))
                    return;
             
                animator.MatchTarget(new Vector3(gkEndPos.x, rb.transform.position.y, gkEndPos.z),
                                                 Quaternion.identity, AvatarTarget.Root,
                                                 new MatchTargetWeightMask(Vector3.one, 0f), 0.0f, timeToCorrectPos);

                /*animator.MatchTarget(new Vector3(gkEndPos.x, rb.transform.position.y, rb.transform.position.z), 
                                                   Quaternion.identity, AvatarTarget.Root,
                                                   new MatchTargetWeightMask(Vector3.one, 0f), 0.0f, timeToCorrectPos);*/


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

                /*if (!isCpu)
                    print("DBGNOCPUPLAYERCOL matchTarget after normTime " + normTime + " gkEndPos " + gkEndPos +
                        " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);
                else
                    print("DBGCPUPLAYERCOL matchTarget after normTime " + normTime + " gkEndPos " + gkEndPos +
                      " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);
                */


            }
        }
        else
        {
            matchInitSavePos = false;
        }

        /* Cpu step side */
        if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset"))
        {
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
            animInt = (int)((distX - 1.3f) / 0.45f) + 1;
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
            }
            else
            {
                animInt = (int)((distX - 1.0f) / 0.45f) + 1;
            }
        }

        if (animInt > 11)
            animInt = 11;

        return animInt;
    }

    private void straightShotCurves(ref Vector3 curveStartPosV3,
                                    ref Vector3 curveMidPosV3,
                                    ref Vector3 curveEndPosV3)
    {
        curveStartPosV3 =
                   new Vector3(0f, 0f, PITCH_HEIGHT_HALF);
        curveMidPosV3 =
              new Vector3(screenWidth / 2f, screenWidth / 2f, -PITCH_HEIGHT_HALF);
        curveEndPosV3 =
                 new Vector3(screenWidth, screenWidth, -PITCH_HEIGHT_HALF);
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
        bool isCpuShotActive = cpuPlayer.getShotActive();

        //if (Input.GetMouseButtonDown(0))
        //    print("#UPDATE1234 Input.GetMouseButtonDown(0) " + Input.mousePosition);

        if (Input.touchCount <= 0 ||
            touchLocked)
            return;

        /*for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);
            print("UPDATTOUCH145 ENTERED " + touchLocked + " INPU COUNT "
                + Input.touchCount
                + " touch.fingerId " + touch.fingerId
                + " TOUCHPOSITION " + touch.position
                + " touch.phase " + touche.phase);
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
                //Debug.Log("#DBGTOUCH ID " + i + " VALUE " + touch.position + " Id " + joystick.getPointerId());

                if (isTouchInsidePowerButtons(touch.position))
                    continue;

                if (checkIfClickTooFar(touch))
                    continue;

                bool intersectWithHelper = checkRectPointIntersection(
                                                touch.position,
                                                gkHelperRectTransform.position,
                                                gkHelperImageWidth);

                /*if click point is inside a Helper or is neither on a buttons 
                 * nor on joystick (check above) we should be safe */
                if (intersectWithHelper ||
                    !checkTouchInterJoystickButtons(touch.position))
                {
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

                    if (isTouchInsidePowerButtons(touch.position))
                        continue;

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
                    }
                    else
                    {
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
               (isTouchBegin && (touch.fingerId != touchFingerId)) ||
               isCpuShotActive)
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

            if (isPlayerOnBall())
            {
                Plane objPlane = new Plane(
                    Camera.main.transform.forward * -1, trailShoot.transform.position);
                Ray mRay = Camera.main.ScreenPointToRay(touch.position);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    lineEndPos = mRay.GetPoint(rayDistance);
                    trailShoot.transform.position = lineEndPos;
                }
            }

            prevMovedPos = touch.position;

            /*Overwritee endPos too, in case user not finish */
            endPos = touch.position;
            drawTimeEnd = Time.time;

            if (midTouchPosIdx < MID_MAX_POS)
                midTouchPos[midTouchPosIdx++] = touch.position;
        }

        if (touch.phase == TouchPhase.Began)
        {
            if (!isTouchBegin && !isCpuShotActive)
                touchFingerId = touch.fingerId;

            /*get only one point*/
            if (isCpuShotActive)
            {
                //touchFingerId = touch.fingerId;
                //isTouchBegin = true;
                //touchCount = 1;
                /*updateLastGkTouchPos(touch);
                touchLocked = true;
                gkTouchDone = true;
                gkClickHelper.enabled = true;
                rectTransformGkClickHelper.position = touch.position;*/
                return;
            }
            else
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

            trailShoot = (GameObject)Instantiate(drawPrefabShotTrail, Vector3.zero, Quaternion.identity);
            trailShoot.GetComponent<TrailRenderer>().sortingOrder = 1;

            Plane objPlane = new Plane(Camera.main.transform.forward * -1, trailShoot.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(touch.position);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                lineEndPos = mRay.GetPoint(rayDistance);
                trailShoot.transform.position = lineEndPos;
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {

            if (isCpuShotActive)
            {
                updateLastGkTouchPos(touch);
                touchLocked = true;
                gkTouchDone = true;
                gkClickHelper.enabled = true;
                rectTransformGkClickHelper.position = touch.position;
                touchFingerId = -1;
                isTouchBegin = false;
                return;
            }

            if (!isTouchBegin ||
                (isTouchBegin && (touch.fingerId != touchFingerId)))
                return;

            endPos = touch.position;
            drawDistance += Vector2.Distance(touch.position, prevMovedPos);
            drawTimeEnd = Time.time;
            touchCount++;
            touchLocked = true;
            touchFingerId = -1;
            isTouchBegin = false;
            updateTouchEndFinished = true;
            gkTouchDone = true;

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
                }
            }
        }
    }

    private bool isTouchInsidePowerButtons(Vector2 touch)
    {
        if ((touch.x > specialPowersScreenOffset.x) &&
            (touch.y > specialPowersScreenOffset.y))
            return true;

        return false;
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
        bool excludedAnim = false;
        for (int i = 0; i < AllAnimationsNames.Count; i++)
        {
            for (int j = 0; j < excluded.Length; j++)
            {
                //if (excluded.Any(AllAnimationsNames[i].Equals)) continue;
                if (AllAnimationsNames[i].Equals(excluded[j]))
                {
                    excludedAnim = true;
                    break;
                }
            }

            if (excludedAnim) continue;

            if (isPlaying(animator, AllAnimationsNames[i], end))
                return true;
        }

        return false;

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

    public bool isBallOutOfPitch()
    {
        if (Mathf.Abs(ballRb[activeBall].transform.position.z) > 15.0f ||
            Mathf.Abs(ballRb[activeBall].transform.position.x) > 21.5f)
        {
            lastTimeBallWasOut = Time.time;
            return true;
        }


        return false;
    }

    public bool isBallinGame()
    {
        return isBallInGame;
    }

    public float getDelayAfterGoal()
    {
        return delayAfterGoal;
    }

    private void clearVariables()
    {
        //joystick.setDefaultColorButton();
        cpuPlayer.setShotActive(false);

        delayAfterGoal = 0.0f;
        //print("VELOCITY CLEARED HERE 4");

        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;

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

            Vector3 ballAway = new Vector3(ballRb[activeBall].transform.position.x + (ballRb[activeBall].velocity.x * 10.0f),
                                           ballRb[activeBall].transform.position.y + (ballRb[activeBall].velocity.y * 10.0f),
                                           ballRb[activeBall].transform.position.z + (ballRb[activeBall].velocity.z * 10.0f));

            float distHit = float.MaxValue;
            Ray rayBall = new Ray(
               ballRb[activeBall].transform.position,
               (ballAway - ballRb[activeBall].transform.position).normalized);

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
        /*Vector3 direction = endPosOrg - ballRb[activeBall].position;
        Plane playerXLocalPlane = new Plane(
                                            rb.transform.forward,
                                            rb.transform.position);*/

        /*print("DEBUGLASTTOUCHLUCKXYU DRAW HELPER CIRCLE START SHOTV " + shotvariant +
             " OUTSHOTS " + outShotStart + " MID " + outShotMid + " OUTSHOTEND " + outShotEnd);*/
        Vector3 hitPointWS = 
            getBallHitRbPoint(gameObjectRbRotated, shotvariant, outShotStart, outShotMid, outShotEnd);
        //print("DEBUGLASTTOUCH HELPER IMAGE " + hitPointWS + " shotvariant " + shotvariant);
        if (hitPointWS != INCORRECT_VECTOR)
        {
            gkHelperRectTransform.position = m_MainCamera.WorldToScreenPoint(hitPointWS);
            gkHelperImage.enabled = true;
        }


        return;


        /*float enter = 0.0f;

        Ray rayBall = new Ray(
                              ballRb[activeBall].position,
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

        /*DrawLine(corner0, corner2, Color.red);
        DrawLine(corner1, corner3, Color.red);
        DrawLine(corner0, corner1, Color.red);
        DrawLine(corner1, corner2, Color.red);
        DrawLine(corner2, corner3, Color.red);
        DrawLine(corner3, corner0, Color.red);*/
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public float timeOfBallFlyBasedOnPosition(Vector3 pos,
                                              float timeOfFlyShot,
                                              float shotDistanceToTravel)
    {
        float defaultDist = PITCH_HEIGHT_HALF;
        float additionalDist = Mathf.Abs(pos.z);

        //return (timeOfFlyShot * (defaultDist + additionalDist)) / defaultDist;
        return Mathf.Min(ShotSpeedMax, (timeOfFlyShot * (shotDistanceToTravel / defaultDist)));
    }

    public float timeOfBallFlyBasedOnPosition(Vector3 pos, float timeOfFlyShot)
    {
        float defaultDist = PITCH_HEIGHT_HALF;
        float additionalDist = Mathf.Abs(pos.z);

        return (timeOfFlyShot * (defaultDist + additionalDist)) / defaultDist;
    }


    public float timeOfBallFlyBasedOnPositionReverse(Vector3 pos, float timeOfFlyShotDist, float shotDistanceToTravel)
    {
        float defaultDist = PITCH_HEIGHT_HALF;
        float additionalDist = Mathf.Abs(pos.z);

        //return (timeOfFlyShotDist / (defaultDist + additionalDist)) * defaultDist;
        return (timeOfFlyShotDist / (shotDistanceToTravel)) * defaultDist;
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
            //wallUpLeftTop = GameObject.Find("wallUpLeftTop");
            //wallUpRightTop = GameObject.Find("wallUpRightTop");

            if (!Globals.cpuGoalSize.Equals("STANDARD"))
            {
                wallUpLeft1.SetActive(false);
                wallUpRight1.SetActive(false);

                wallUpLeft2.transform.position =
                    new Vector3(-13.8f, 0.65f, 14.075f);
                //wallUpLeftTop.transform.position =
                //    new Vector3(-13.8f, 1.25f, 14.25f);

                wallUpRight2.transform.position =
                   new Vector3(13.8f, 0.65f, 14.075f);
                //wallUpRightTop.transform.position =
                //    new Vector3(13.8f, 1.25f, 14.25f);

                wallUpLeft2.transform.localScale =
                wallUpRight2.transform.localScale =
                    new Vector3(12.4f, 1.3f, 0.15f);

                //wallUpLeftTop.transform.localScale =
                //wallUpRightTop.transform.localScale =
                //   new Vector3(12.4f, 0.1f, 0.5f);

                cpuGoalUp.transform.localScale = new Vector3(2.06f, 1.7f, 1f);
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
        float avgSkill = (float)skill / 2f;
        float skillsInter = Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, avgSkill);
        float levelInter = Mathf.InverseLerp(LEVEL_MIN, LEVEL_MAX, (float)level);
        float speedNorm = skillsInter;

        if (isCpu)
            speedNorm = (skillsInter + levelInter) / 1.6f;

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
        }
        else
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
        return Mathf.InverseLerp(LEVEL_MIN, LEVEL_MAX, (float)Globals.level);
    }

    private float getLevelInterpolationReverse()
    {
        return Mathf.InverseLerp(LEVEL_MAX, LEVEL_MIN, (float)Globals.level);
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
        if (Mathf.Abs(ballRb[activeBall].velocity.x) < speed &&
            Mathf.Abs(ballRb[activeBall].velocity.z) < speed)
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

    private bool isExtraShot(ref Vector3 endPosOrg)
    {

        if (Globals.level >= 3)
        {
            int isExtraShotEnable = UnityEngine.Random.Range(1, 101);
            if (isExtraShotEnable > (100 - (Globals.level * Globals.level)))
            {
                int randomHeight = UnityEngine.Random.Range(0, 4);
                int randomSide = UnityEngine.Random.Range(0, 2);
                endPosOrg = extraShotVec;

                if (randomSide == 1)
                {
                    endPosOrg.x *= -1;
                }

                if (randomHeight == 2)
                {
                    endPosOrg.y = UnityEngine.Random.Range(1f, 1.5f);
                }

                if (randomHeight == 3)
                {
                    endPosOrg.y = UnityEngine.Random.Range(0.3f, 0.5f);
                }
        
                return true;
            }
        }

        return false;
    }

    private void findGoalEndPoint(ref Vector3 endPosOrg, bool isCpu)
    {
        float levelNorm =
            getLevelInterpolationReverse();

        float skillsNorm =
            getSkillInterpolationReverse(attackSkillsCpu);
        if (!isCpu)
            skillsNorm = getSkillInterpolationReverse(attackSkillsPlayer);

        float goalEndMax = 5.15f;
        if (Globals.level >= 3)
        {
            goalEndMax += (skillsNorm / 1.5f);
        }

        float leftGoalDownX =
              getRandFloat(-(goalEndMax + (levelNorm * 1.5f)), -4.0f);
        float rightGoalDownX =
              getRandFloat(4.0f, goalEndMax + (levelNorm * 1.5f));
        float middleGoalDownX =
              getRandFloat(-2.5f, 2.5f);

        float heightMaxInter = levelNorm;
        float goalDownHeight = getRandFloat(0.0f, 3.0f + heightMaxInter);

        if (isCpu)
        {
            int randSide = UnityEngine.Random.Range(1, 4);
            endPosOrg = new Vector3(leftGoalDownX, goalDownHeight, -PITCH_HEIGHT_HALF);
            if (randSide == 2)
            {
                endPosOrg = new Vector3(rightGoalDownX, goalDownHeight, -PITCH_HEIGHT_HALF);
            }
            else if (randSide == 3)
            {
                endPosOrg = new Vector3(middleGoalDownX, goalDownHeight, -PITCH_HEIGHT_HALF);
            }
        }
    }


    /*check where we should shoot*/
    private bool getTheBestgoalEnd(Rigidbody rb,
                                   Vector3 ballPos,
                                   Vector3 cornerPoints,
                                   float playerRunSpeed,
                                   Vector3 goalSize,
                                   ref int typeOfShot,
                                   Vector3 endPosOrg,
                                   Vector3 curveStartPos,
                                   Vector3 curveMidPos,
                                   Vector3 curveEndPos,
                                   ref Vector3 localInterDistGK,
                                   float minDist,
                                   bool isCpu)
    {
        Vector3 realHitPlaceLocal = INCORRECT_VECTOR;
        Vector3 hitPointWorld = INCORRECT_VECTOR;
        Vector3 ballAway = Vector3.zero;
        float distHit = float.MaxValue;


        //GameObject rotatedRbToBall = new GameObject();
        //Ray rayBall;

        //Profiler.BeginSample("getTheBestgoalEnd cpu fixed update");

        getRotatedRbToBall(ballPos,
                           rb,
                           ref tmpRotatedRbToBall,
                           cornerPoints);

        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       tmpRotatedRbToBall,
                                                       curveStartPos,
                                                       curveMidPos,
                                                       curveEndPos,
                                                       false);


        //intersection not found
        if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
        {
            localInterDistGK = Vector3.zero;
            return false;
        }

        if (Mathf.Abs(realHitPlaceLocal.x) > minDist ||
           (realHitPlaceLocal.y >= (goalSizes.y + 0.5f) ||
           (realHitPlaceLocal.y >= goalSizes.y)))
        {
            //endPosOrg = curveEndPos;
            /*print("FOUNDGOOD SHOOT YUPPII local "
                + realHitPlaceLocal + " rb.transofr.post + " + rb.transform.position
                + " sTart " + curveStartPos
                + " mid " + curveMidPos
                + " ballrb " + ballPos
                + " endGoalPosOrg " + endPosOrg);*/
            localInterDistGK = realHitPlaceLocal;
            return true;
        }

        localInterDistGK = realHitPlaceLocal;

        /*print("FOUNDGOOD SHOOT NOT FOUND "
           + realHitPlaceLocal + " rb.transofr.post + " + rb.transform.position
           + " sTart " + curveStartPos
           + " mid " + curveMidPos
           + " ballrb " + ballPos
           + " endGoalPosOrg " + endPosOrg);*/

        return false;
        //Vector3 playerForwardVector =
        //     tmpRotatedRbToBall.transform.forward;

        //Plane playerXLocalPlane = new Plane(
        //   playerForwardVector,
        //   tmpRotatedRbToBall.transform.position + (tmpRotatedRbToBall.transform.forward * 0.6f));

        //Vector3 endGoalPos = new Vector3(0, 0, -PITCH_HEIGHT_HALF);

        //float maxHitX = 0f;
        //Vector3 maxGoalEnd = INCORRECT_VECTOR;
        //for (int i = 0; i < 3; i++)
        //{
        //    realHitPlaceLocal = INCORRECT_VECTOR;

        /*if (i == 1)
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
        }*/

        /*shot anim execute around 700 ms before ball starts fly*/
        /*add ball fly to hit point time*/
        /*print("DEBUGCHANCETOSHOOT getTheBestgoalEnd endGoalPos "
            + endGoalPos + " realHitPlaceLocal.x "
            + realHitPlaceLocal.x + " hitPointWorld "
            + hitPointWorld
            + " playerRunSpeed * 0.7f " + playerRunSpeed * 0.7f);*/

        //if (realHitPlaceLocal != INCORRECT_VECTOR &&
        //   (Mathf.Abs(realHitPlaceLocal.x) > maxHitX))
        //{
        //    maxHitX = Mathf.Abs(realHitPlaceLocal.x);
        //    maxGoalEnd = endGoalPos;
        //}
        //}

        //Profiler.EndSample();
        //return maxGoalEnd;
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
                Mathf.Abs(realHitPlaceLocal.x) > 8f)
            //(Mathf.Abs(realHitPlaceLocal.x) > (playerRunSpeed * 0.7f)))
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
    public float getCurveShotFlyPercent()
    {
        return curveShotFlyPercent;
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
        int rand = UnityEngine.Random.Range(0, 2);

        /*back speed should be similar on each level. Since the setup on a pitch is different*/
        switch (Globals.level)
        {
            case 5:
                //backSpeed = UnityEngine.Random.Range(8.5f, Mathf.Max(8.6f, speed));
                if (rand != 0)
                    backSpeed = Mathf.Max(8.8f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.7f, Mathf.Max(7.75f, speed * 0.8f));

                break;
            case 4:
                if (rand != 0)
                    backSpeed = Mathf.Max(8.5f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.5f, Mathf.Max(7.55f, speed * 0.8f));
                break;
            case 3:
                if (rand != 0)
                    backSpeed = Mathf.Max(8.3f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.40f, Mathf.Max(7.45f, speed * 0.8f));
                break;
            case 2:
                if (rand != 0)
                    backSpeed = Mathf.Max(8.1f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.3f, Mathf.Max(7.35f, speed * 0.8f));
                break;
            default:
                backSpeed = UnityEngine.Random.Range(4.0f, Mathf.Max(4.1f, speed * 0.8f));
                //backSpeed = UnityEngine.Random.Range(4.5f, Mathf.Max(4.6f, speed));
                //backSpeed = speed;
                break;
        }

        return backSpeed;
    }

    public int foundActiveBall(bool isCpu)
    {
        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {
            if (isBallReachable(ballRb[i].transform.position, isCpu))
            {
                return i;
            }
        }

        return 1;
    }

    public void setNumberOfBalls(int num)
    {
        NUMBER_OF_BALLS = num;
    }

    public void setMaxNumberOfBalls(int maxBalls)
    {
        MAX_NUMBER_OF_BALLS = maxBalls;
    }

    public GameObject getPlayerUp()
    {
        return playerUp;
    }

    private void initPowers()
    {
        //extrapowers disabled
        if (Globals.powersStr.Equals("NO"))
            isPowerEnable = false;
    }

    private void initBonuses()
    {
        if ((Globals.isTrainingActive == false)
            && (Globals.numMatchesInThisSession % 4 == 3))
        {
            if (!Globals.isBonusActive)
            {
                Globals.isBonusActive = true;
                isBonusActive = true;
            }
            else
            {
                Globals.isBonusActive = false;
                isBonusActive = false;
            }
        }
        else
        {
            Globals.isBonusActive = false;
        }

        if (Globals.isMultiplayer || Globals.isLevelMode)
        {
            Globals.isBonusActive = false;
            isBonusActive = false;
        }

        //print("#DBGBOUNSACTIVE " + isBonusActive);
    }

    public void playerFreeze(bool freezePlayer)
    {
        isPlayerFreeze = freezePlayer;
    }

    public void setExtraGoals(bool val)
    {
        isExtraGoals = val;
    }

    public bool getShotActive()
    {
        return shotActive;
    }

    public class CpuPlayer : MonoBehaviour
    {
        private Rigidbody cpuPlayerRb;
        private Rigidbody[] ballRb;
        private Vector3 towardsNewPos;
        public float speed, backSpeed, orgSpeed;
        private controllerRigid parentRb;
        public Animator animator;
        private GameObject cpuPlayer;
        private bool isShotActive;
        private Vector2 startPos, endPos;
        private float passedTime = 0.0f;
        private Vector3 ballInitPos;
        private PlayerOnBall onBall = PlayerOnBall.NEUTRAL;
        private float height = 0.0f;
        private float timeOfShot = 1.2f;
        private float timeOfShotOrg = 500f;
        private float goalDownX = 2.0f;
        private bool enter = false;
        private bool isAnyAnimationPlaying = false;
        private Vector3 goalieBasePoint;
        private Vector3 pointToGo;
        private ballMovement ball;
        private float goPassedTime = 0.0f;
        private int typeOfShot;
        private int curveShotDirection;
        private bool shotPreperationActive = false;
        private bool isBallTrailRendererInit = false;
        private GameObject rbRightFoot;
        private GameObject rbRightToeBase;
        private GameObject rbLeftToeBase;
        private bool initPreShot;
        private bool initVolleyShot;
        private bool preShotActive;
        private string shotType;
        private bool prepareCpuOptions = false;
        private Vector3 endPosOrg;
        private float drawGKHelperTime;
        //private Vector3 midPosv3;
        private Vector3 outShotStart, outShotMid, outShotEnd, outShotBallVelocity;
        private Vector3 outShotMidPredef;
        public SHOTVARIANT shotvariant;
        private bool shotRet;
        private Vector3 pitchMiddlePoint;
        private GameObject leftPalm;
        private GameObject rightPalm;
        private GameObject leftHand;
        private GameObject rightHand;
        private float lastGkDistX;
        private bool gkPositionReach = false;
        private bool goToPosDelay = false;
        private bool isGoToPosDelayActive = false;
        private bool isGoToPosAfterOut = false;
        private bool goToPosAfterOut = false;

        private Vector3 gkStartPos;
        private Transform gkStartTransform;
        private float gkTimeToCorrectPos;
        private string lastGkAnimName = "";
        private float lastTimeGkAnimPlayed = 0f;
        private float MAX_DISTANCE_FROM_MIDDLE = 12.0f;
        private Rect shotZone;
        private Rect dontTouchBallZone;
        private Vector3 pointInsideShotZone;
        private bool pointInsideShotZoneDone = false;
        private float randDistanceShotZone;
        private bool initCpuAdjustAnimSpeed = false;
        private bool initGkDeleyLevel = false;
        private float levelDelay = 0.0f;
        private string initAnimName = "";
        private float cpuGkAnimAdjustSpeed = 2.2f;
        private Vector3 prevRbPos;
        private float prevTrickTime;
        private string gkAction;
        private float gkTimeLastCatch;
        private bool isLobActive = false;
        private Vector3 stepSideAnimOffset;
        private bool gkLobPointReached = false;
        private bool gkRunPosReached = false;
        private float initDistX = -1f;
        private float lastTimeCollisionWithWall = 0.0f;
        private Vector3 lastBallVelocity;
        private float MAX_RB_CPU_VELOCITY = 12.0f;
        private Vector3 goalSizesCpuTakeAction;
        private GameObject parentRotatedRbToBall;
        private bool isUpdateBallPosActive = false;
        private Vector3 updateBallPos;
        private string updateBallPosName = "";
        private Vector3 extraShotVec;
        private Vector3 lastBallPos = Vector3.zero;
        private bool comeToBallReached = false;
        private bool gkLock = false;
        private GameObject rotatedRbToBall;
        private GameObject tmpRotatedRbToBall;
        private Vector3 gkCornerPoints;
        private int moveChangeDirectionCounter = 0;
        private int maxMoveChangePos = 2;
        private float lastTimeTurnAnimPlayed = Time.time;
        private float lastTimeTurnCounter = 0;
        private Vector3 matchSavePos;
        private bool matchInitSavePos;
        private Vector3 endPosOrgPred;
        private bool goalieBasePointChanged = false;
        private bool isRandTimeToShot = false;
        private int randTimeToShot = 5;
        private int activeBall = 1;
        private bool activeBallFound = false;
        private bool isExtraShotActive = false;
        private float shotRotationDelay = 0f;
        private float shotSpeed = 50f;
        private bool isPlayerFreeze = false;
        private bool isExtraGoals = false;
        private float curveShotFlyPercent = 0f;
        private float lastTimePlayerOnBall = -1f;
        private bool isPlayerNowOnBall = false;
        private bool isPlayerStandStill = false;
        private int[] randPower;
        private float[] randPowerMatchTime;
        private float prepareShotDelay = 0.35f;
        private bool isPrepareShotDelay = false;

        public CpuPlayer(controllerRigid parentRigid, ballMovement ball)
        {

            randPower = new int[Globals.MAX_POWERS + 1];
            randPowerMatchTime = new float[Globals.MAX_POWERS + 1];

            for (int i = 0; i < randPower.Length; i++)
            {
                randPower[i] = UnityEngine.Random.Range(0, Globals.MAX_POWERS + 1);
                randPowerMatchTime[i] = UnityEngine.Random.Range(0f, 60f);
            }

            if (Globals.isMultiplayer ||
                (Globals.stadiumNumber != 0) ||                
                (Globals.stadiumNumber == 2))
            {
                for (int i = 0; i < randPower.Length; i++)
                    randPower[i] = 0;

                if (Globals.stadiumNumber == 2)
                {
                    randPower[2] = 5;
                    randPower[7] = 5;
                    randPower[8] = 5;
                } else
                {
                    randPower[0] = 5;
                    randPower[1] = 5;
                    randPower[2] = 5;
                }
            }

            //clearPreShotVariables();
            rotatedRbToBall = new GameObject();
            tmpRotatedRbToBall = new GameObject();

            this.parentRb = parentRigid;
            extraShotVec = new Vector3(4.3f, 2.7f, -parentRb.PITCH_HEIGHT_HALF);

            cpuPlayer = parentRb.getPlayerUp();
            cpuPlayerRb = cpuPlayer.GetComponent<Rigidbody>();

            ballRb = new Rigidbody[parentRb.NUMBER_OF_BALLS + 1];

            for (int i = 1; i <= parentRb.NUMBER_OF_BALLS; i++)
                ballRb[i] = GameObject.Find("ball" + i.ToString()).GetComponent<Rigidbody>();

            //speed = parentRb.calcRunLevelSpeed(Globals.level,
            //                                   Globals.teamBcumulativeStrength,
            //                                   true);
            speed = parentRb.calcRunLevelSpeed(Globals.level,
                                               parentRb.cumulativeStrengthCpu,
                                               true);

            speed = parentRb.getSpeed(Globals.level, speed);
            backSpeed = speed;

            orgSpeed = speed;

            //print("#DBGTEAMSSPEED DEBUGBACKSPEED speed " + speed + " backSpeed " + backSpeed + " " +
            //             " parentRb.cumulativeStrengthCpu " +
            //             parentRb.cumulativeStrengthCpu
            //             + " level " + Globals.level);

            this.ball = ball;
            animator = cpuPlayer.GetComponent<Animator>();
            //print("ANIMATOR " + animator);
            isShotActive = false;
            /*TOUNCHECK*/
            //goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.getRandFloat(10.0f, 13.5f));
            goalieBasePoint = new Vector3(0.0f, 0.0f, 7.0f);
            initMultiplayerSettings();

            if (parentRb.trainingScript.isShotTraining())
                goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.PITCH_HEIGHT_HALF - 3.0f);

            typeOfShot = 0;
            curveShotDirection = 1;
            shotPreperationActive = false;
            initPreShot = false;
            initVolleyShot = false;
            preShotActive = false;
            passedTime = 0.0f;
            drawGKHelperTime = 0.0f;
            pitchMiddlePoint = new Vector3(0.0f, 0.0f, 0.0f);

            rbRightFoot = GameObject.FindWithTag("playerUpRightLeg");
            rbRightToeBase = GameObject.FindWithTag("playerUpRightToeBase");
            rbLeftToeBase = GameObject.FindWithTag("playerUpLeftToeBase");

            leftPalm = GameObject.FindWithTag("playerUpLeftPalm");
            rightPalm = GameObject.FindWithTag("playerUpRightPalm");

            leftHand = GameObject.FindWithTag("playerUpLeftHand");
            rightHand = GameObject.FindWithTag("playerUpRightHand");

            float levelInter = parentRb.getLevelInterpolationReverse();
            //float skillsInter = 
            //    parentRb.getSkillInterpolationReverse(Globals.teamBAttackStrength);
            float skillsInter =
                  parentRb.getSkillInterpolationReverse(parentRb.attackSkillsCpu);

            float avgInter = (levelInter + skillsInter) / 2.0f;
            float levelOffset = Mathf.Lerp(0, -4.0f, avgInter);
            //print("DEBUGZ1237 zone offset " + levelOffset + " levelInter " +
            //                                  levelInter + " skillsInter " + skillsInter + " avgInter " + avgInter);

            /*shotZone = new Rect(-12f + levelOffset,
                                2.5f,
                                Mathf.Abs((-12f + levelOffset) * 2),
                                5f + Mathf.Abs(levelOffset));*/

            shotZone = new Rect(-12f + levelOffset,
                                4.5f,
                                Mathf.Abs((-12f + levelOffset) * 2),
                                4f + Mathf.Abs(levelOffset / 2f));
            /*if (Globals.level == 2)
            {
                shotZone = new Rect(-12f + levelOffset,
                               4.5f,
                               Mathf.Abs((-12f + levelOffset) * 2),
                               UnityEngine.Random.Range(4f, 12f));
            }*/

            float dontTouchXOffset = parentRb.getGoalSizePlr2().x + 2;
            dontTouchBallZone =
                    new Rect(
                        -dontTouchXOffset,
                        parentRb.PITCH_HEIGHT_HALF - 4,
                        dontTouchXOffset * 2f,
                        parentRb.PITCH_HEIGHT_HALF + 2);

            //print("DEBUGZ1237 SHOTZONE " + shotZone);

            pointInsideShotZone = getPointInsideShotZone(shotZone);
            pointInsideShotZoneDone = false;
            /*LEVEL DEPENDENT*/
            //randDistanceShotZone = parentRb.getRandFloat(5.0f, 10.0f);
            randDistanceShotZone = 2f;

            goalSizesCpuTakeAction = new Vector3(parentRb.goalSizesCpu.x,
                                                 parentRb.goalSizesCpu.y,
                                                 parentRb.goalSizesCpu.z);

            /*LEVEL DEPENDENT*/
            goalSizesCpuTakeAction.x *= (1.5f + levelInter);
            goalSizesCpuTakeAction.y *= (1.3f + levelInter);


            gkCornerPoints = new Vector3(
                goalSizesCpuTakeAction.x, 0f, parentRb.PITCH_HEIGHT_HALF);

            prevRbPos = cpuPlayerRb.transform.position;
            matchSavePos = cpuPlayerRb.transform.position;
            matchInitSavePos = false;

            endPosOrgPred = parentRb.INCORRECT_VECTOR;
        }

        public void update()
        {
            /*if (!parentRb.checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_"))
            {
                animator.speed = 1.0f;
            }*/

            if (Globals.stadiumNumber == 0)
            {

                if (isShotActive &&
                   (curveShotFlyPercent < 0.3f))
                {

                    if ((randPower[(int) POWER.TWO_EXTRA_GOAL] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.TWO_EXTRA_GOAL]))
                    {
                        parentRb.powersScript.twoExtraGoals(true,
                                                           (int) POWER.TWO_EXTRA_GOAL,
                                                            Vector3.zero,
                                                            Vector3.zero);
                    }
                }

                if (preShotActive || 
                    isShotActive)
                {
                    if ((Globals.level >= 3) &&
                        (randPower[(int)POWER.SHAKE_CAMERA] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.SHAKE_CAMERA]))
                    {                     
                        parentRb.powersScript.shakeCamera(
                                        true,
                                        (int) POWER.SHAKE_CAMERA,
                                        Vector3.zero,
                                        Vector3.zero);
                    }

                    if ((randPower[(int) POWER.BAD_CONDITIONS] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.BAD_CONDITIONS]))
                    {
                        parentRb.powersScript.badConditions(true, 
                                                            (int) POWER.BAD_CONDITIONS, 
                                                            Vector3.zero, 
                                                            Vector3.zero);
                    }

                    if (isShotActive &&
                        (randPower[(int) POWER.ENLARGE_GOAL] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.ENLARGE_GOAL]))
                    {

                        parentRb.powersScript.resizeOpponentGoal(true,
                                                              (int) POWER.ENLARGE_GOAL,
                                                              new Vector3(1.66f, 1.47f, 4f),
                                                              new Vector3(6f, 3.5f, 14f));
                    }
                }
            } else
            {
                if (parentRb.getShotActive() &&
                    (randPower[(int) POWER.GOAL_WALL] == 5) &&
                    (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.GOAL_WALL]))
                {

                    parentRb.powersScript.goalObstacles(true, 
                                                        (int) POWER.GOAL_WALL, 
                                                        Vector3.zero, 
                                                        Vector3.zero);
                }

                if (isShotActive &&
                    (curveShotFlyPercent < 0.3f))
                {

                    if ((randPower[(int) POWER.TWO_EXTRA_GOAL] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.TWO_EXTRA_GOAL]))
                    {
                        parentRb.powersScript.twoExtraGoals(true,
                                                           (int)POWER.TWO_EXTRA_GOAL,
                                                            Vector3.zero,
                                                            Vector3.zero);
                    }
                }

            }
  
            parentRb.matchTarget(animator,
                                 cpuPlayerRb,
                                 ref gkStartPos,
                                 gkTimeToCorrectPos,
                                 stepSideAnimOffset,
                                 ref matchSavePos,
                                 ref matchInitSavePos,
                                 true);

            //print("cpu velocity " + cpuPlayerRb.velocity);
            float runSpeed = Mathf.Max(Mathf.Abs(cpuPlayerRb.velocity.x),
                                       Mathf.Abs(cpuPlayerRb.velocity.z));

            if (parentRb.powersScript.isPlayerUpSlowDown())
            {
                speed = orgSpeed / 2f;
                backSpeed = speed;
                runSpeed = Mathf.Max(Mathf.Abs(cpuPlayerRb.velocity.x),
                                      Mathf.Abs(cpuPlayerRb.velocity.z)) / 2f;
            }
            else
            {
                speed = orgSpeed;
                backSpeed = speed;
                runSpeed = Mathf.Max(Mathf.Abs(cpuPlayerRb.velocity.x),
                                      Mathf.Abs(cpuPlayerRb.velocity.z));
            }

            runSpeed = Mathf.InverseLerp(0.0f, MAX_RB_CPU_VELOCITY, runSpeed);
            animator.SetFloat("3d_run_speed_cpu", 1.1f + runSpeed);
            animator.SetFloat("3d_run_speedBack_cpu", 0.6f + runSpeed);
        }

        public void fixedUpdate()
        {

            
            isPlayerStandStill = false;
            //if (!activeBallFound)
            //{
            //    activeBall = parentRb.foundActiveBall(true);
            //    activeBallFound = true;
            //}
            /*   parentRb.matchTarget(animator,
                                   cpuPlayerRb,
                                   ref gkStartPos,
                                   gkTimeToCorrectPos,
                                   stepSideAnimOffset,
                                   ref matchSavePos,
                                   ref matchInitSavePos,
                                   true);*/

            //if (parentRb.getMatchTimeMinute() == 3)
            //{
            ///    parentRb.powersScript.enableFlares(t
            ///    rue,
             //                                          (int) POWER.ENABLE_FLARE,
             //                                          Vector3.zero,
             //                                          Vector3.zero);
            //}


            if (!Globals.isTrainingActive && !parentRb.isBonusActive)
            {
                Vector3 ballInLocalRb =
                        parentRb.InverseTransformPointUnscaled(cpuPlayerRb.transform, ballRb[activeBall].transform.position);
                if (parentRb.isShotActive() &&
                    (ballInLocalRb.z < 0f) &&
                    ((Globals.level > 3 && (UnityEngine.Random.Range(0, 4) == 0)) ||
                     (Globals.level <= 3 && (UnityEngine.Random.Range(0, 5) == 0))))
                {

                    if (parentRb.getShotActive() &&
                        (Globals.stadiumNumber == 0) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.CUT_GOAL_BY_HALF]) &&                        
                        ((randPower[(int) POWER.CUT_GOAL_BY_HALF] == 5) ||
                        (randPower[(int) POWER.CUT_GOAL_BY_HALF] == 6)))
                   {
                        parentRb.powersScript.cutGoalByHalf(true,
                                                        (int) POWER.CUT_GOAL_BY_HALF,
                                                        new Vector3(1.48f, 1.44f / 1.5f, 1f),
                                                        new Vector3(5.25f, 3.5f / 1.5f, 14.0f));
                    }

                    if (parentRb.getShotActive() &&
                        (Globals.stadiumNumber == 0) &&
                        (randPower[(int) POWER.GOAL_WALL] == 5) &&
                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.GOAL_WALL]))
                    {
                        parentRb.powersScript.goalObstacles(true, 
                                                           (int) POWER.GOAL_WALL, 
                                                            Vector3.zero, 
                                                            Vector3.zero);
                    }
                }
            }

            if (!parentRb.isShotActive())
                gkLock = false;

            if (ballRb[activeBall].transform.position.z < parentRb.COMPUTER_MIN_Z_TO_GET_BALL)
                comeToBallReached = false;

            parentRb.correctWhenOutOfPitch(animator,
                                           preShotActive,
                                           isShotActive,
                                           cpuPlayerRb,
                                           rbLeftToeBase,
                                           rbRightToeBase,
                                           ballRb[activeBall],
                                           shotType,
                                           ref prevRbPos,
                                           true);

            parentRb.corectBallPositionOnBall(cpuPlayerRb,
                                        animator,
                                        rbRightToeBase,
                                        rbRightFoot,
                                        ref isUpdateBallPosActive,
                                        updateBallPos,
                                        updateBallPosName,
                                        true);

            /*TODELETE */
            /*string name = parentRb.nameAnimationPlaying(animator, 1.0f);
            bool isBackRunPlaying = parentRb.isPlaying(animator, "3D_back_run_cpu", 1.0f);
            bool isRunPlaying = parentRb.isPlaying(animator, "3D_run", 0.95f);
            
            if (name == string.Empty)
            {
                if (isBackRunPlaying)
                    name = "3D_back_run_cpu";
                if (isRunPlaying)
                    name = "3D_run";           
            }

            if (name != string.Empty)               
            {
                //if (isRunPlaying)
                //    name = "3D_back_run_cpu";

                print("DEBUG2345ANIMPLAY PLAYINGANIMATIONOW " + name + "  "
                    + animator.GetCurrentAnimatorStateInfo(0).IsName(name) + " ANIMNORMTIME "
                    + animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                    + " BALLPOS " + ballRb[activeBall].transform.position
                    + " RB POS " + cpuPlayerRb.transform.position
                    + " PRESHOTACTIVE " + preShotActive
                    + " SHOTACTIVE " + isShotActive
                    + " PLAYER RBPOSITION " + parentRb.getRbTransform().position);                   
            }*/

            /*&print("DEBUGFLASHINGGKDEBUG800 BALLPOS cpu RIGID "
                   + parentRb.InverseTransformPointUnscaled(cpuPlayerRb.transform, ballRb[activeBall].transform.position)
                   + " BALLPOS " + ballRb[activeBall].transform.position);*/

            /*TODELETE*/
            float distTOMID = Vector3.Distance(cpuPlayerRb.transform.position,
                                               pitchMiddlePoint);

            //print("CPUMOVEDEBUG123XCPU###  BEFORE MOVE ISONBALL ");

            Vector2 ballPos = new Vector2(ballRb[activeBall].transform.position.x,
                                          ballRb[activeBall].transform.position.z);
            Vector2 cpuPos = new Vector2(cpuPlayerRb.position.x,
                                         cpuPlayerRb.position.z); ;


            float distToBall = Vector2.Distance(new Vector2(cpuPlayerRb.transform.position.x,
                                                             cpuPlayerRb.transform.position.z),
                                                new Vector2(ballRb[activeBall].transform.position.x,
                                                            ballRb[activeBall].transform.position.z));
            if ((ballRb[activeBall].transform.position.z > 0) &&
                !isShotActive &&
                !preShotActive &&
                parentRb.isBallinGame() &&
                !Globals.isTrainingActive &&
                !parentRb.isBonusActive &&
                (distToBall < 2f) &&
                (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.INVISABLE_PLAYER]) &&
                (randPower[(int)POWER.INVISABLE_PLAYER] == 5))
            {
                if (!pointInsideShotZoneDone)
                {
                    pointInsideShotZone = getPointInsideShotZone(shotZone);
                    pointInsideShotZoneDone = true;
                }

                parentRb.powersScript.invisiblePlayer(true,
                                                 (int) POWER.INVISABLE_PLAYER,
                                                  Vector3.zero,
                                                  Vector3.zero);
            }

            bool isOnBall = parentRb.isPlayerOnBall(
                                rbLeftToeBase,
                                rbRightToeBase,
                                ballRb[activeBall],
                                cpuPlayerRb,
                                "move",
                                true,
                                ref activeBall);

            if (isOnBall &&
                (Globals.stadiumNumber == 0))
            {
                if ((parentRb.getMatchTimeMinute() > randPowerMatchTime[(int) POWER.ENABLE_FLARE]) &&
                    (randPower[(int) POWER.ENABLE_FLARE] == 5))
                {
                    parentRb.powersScript.enableFlares(true,
                                                      (int) POWER.ENABLE_FLARE,
                                                       Vector3.zero,
                                                       Vector3.zero);
                }

               // if (parentRb.getMatchTimeMinute() > 2f)
                    //((randPower[(int) POWER.INVISABLE_PLAYER] == 5) &&
                   //(parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.INVISABLE_PLAYER]))
                //{
                 
                //}
            }

            if (!isOnBall)
            {
                lastTimePlayerOnBall = -1f;
            }
           
            if ((isPlayerNowOnBall == false) &&
                isOnBall)
            {
                lastTimePlayerOnBall = Time.time;
            }

            isPlayerNowOnBall = isOnBall;
         
            //print("CPUMOVEDEBUG123XCPU AFTER MOVE ISONBALL " + isOnBall);
            //print("DEBUGFLASHINGGKDEBUG800  isOnBall " + isOnBall);

            //bool isOpponentWrongPos = false;
            bool isChanceToShoot = false;
            bool isLobShotPossible = false;

            Vector3 opponentPos =
                parentRb.getPlayerPosition();

            if (isOnBall &&
                !parentRb.isShotTrainingActive() &&
                !parentRb.isShotBonusActive() &&
                !parentRb.isShotActive() &&
                !isShotActive &&
                !preShotActive &&
                !parentRb.isAnimatorPlaying(animator))
            {
                bool isShotPossible = false;
                endPosOrgPred = parentRb.INCORRECT_VECTOR;

                //isOpponentWrongPos = 
                //    opponentWrongPosition(opponentPos);

                isShotPossible =
                    parentRb.isShotPossible(cpuPlayerRb);

                if (isShotPossible)
                {
                    isChanceToShoot =
                          parentRb.isChanceToShoot(parentRb.getPlayerRb(),
                                                   ballRb[activeBall].transform.position,
                                                   parentRb.getGkCornerPoints(),
                                                   parentRb.getRunSpeed(),
                                                   parentRb.getGoalSizePlr1(),
                                                   ref endPosOrgPred,
                                                   true);

                    //print("DEBUGCHANCETOSHOOT ISCHANCE TO SHOT "
                    //    + isChanceToShoot + " endPosOrgPred " + endPosOrgPred);
                    if (!isChanceToShoot)
                    {
                        endPosOrgPred = parentRb.INCORRECT_VECTOR;
                    }

                    if (isChanceToShoot &&                        
                        (Globals.stadiumNumber != 2))
                    {
                        int rand = UnityEngine.Random.Range(0, 1);
                        if ((Globals.stadiumNumber == 0) &&
                            ((randPower[(int)POWER.GOLDEN_BALL] == 1) ||
                             (randPower[(int)POWER.GOLDEN_BALL] == 2) ||
                             (randPower[(int)POWER.SILVER_BALL] == 1) ||
                             (randPower[(int)POWER.SILVER_BALL] == 2)))
                        {
                            if (rand == 0)
                                parentRb.powersScript.goldenBall(true, (int)
                                    POWER.GOLDEN_BALL, Vector3.zero, Vector3.zero);
                            else
                                parentRb.powersScript.silverBall(true, (int)
                                    POWER.SILVER_BALL, Vector3.zero, Vector3.zero);
                        }
                    }
                }

                isLobShotPossible = isLobPossible(opponentPos,
                                                  cpuPlayerRb.transform.position);


                if (!isRandTimeToShot)
                {
                    randTimeToShot = UnityEngine.Random.Range(4, 7);
                    if (Globals.isMultiplayer)
                    {
                        randTimeToShot = UnityEngine.Random.Range(3, 7);
                    }
                    isRandTimeToShot = true;
                }

                //print("randTimeToShot " + randTimeToShot);
                if (isShotPossible &&
                    (isChanceToShoot ||
                    ((parentRb.maxTimeToShot - parentRb.getTimeOfShot()) < randTimeToShot) ||
                     isLobShotPossible ||
                     isFinalMovePointReached()))
                {
                    // print("DEBUGCHANCETOSHOOT EXEC CHNCETOSHOT "
                    //     + isChanceToShoot + " TIMEDIFF "
                    //     + (parentRb.maxTimeToShot - parentRb.getTimeOfShot())
                    //     + " ISLOBPOSSIBLE " + (isLobPossible(opponentPos,
                    //                 cpuPlayerRb.transform.position))                 
                    //     + " isFinalMovePointReached "
                    //     + isFinalMovePointReached());

                    preShotActive = true;
                    initMultiplayerSettings();
                    isRandTimeToShot = false;
                    // print("SHOTACTIVE CPUYPLAYER POS " + cpuPlayerRb.transform.position);
                }

                /*overwrite when training mode */
                if ((Globals.isTrainingActive || parentRb.isBonusActive) &&
                    !parentRb.trainingScript.isGkTraining())
                {
                    preShotActive = false;
                }
            }

            bool goalStatus = parentRb.ball[1].getBallGoalCollisionStatus();
            if (goalStatus == true || !parentRb.isBallinGame())
            {
                clearPreShotVariables();
            }

            if (preShotActive && !isShotActive)
            {
                /*shot*/
                if (!prepareCpuOptions)
                {
                    int numShots = parentRb.shotTypesNames.Count;

                    if (Globals.isMultiplayer)
                        parentRb.StartCoroutine(prepShotDelayCpu(prepareShotDelay));

                    if (//isLobShotPossible ||
                        isChanceToShoot ||
                        (UnityEngine.Random.Range(1, 11) < 6))
                        numShots -= 1;

                    shotType = parentRb.getShotNameByIndex(
                        UnityEngine.Random.Range(0, numShots));

                    /*TODELETE*/
                    //shotType = parentRb.getShotNameByIndex(2);
                    prepareShotCpuOptions();
                    prepareCpuOptions = true;

                    //print("DBG1434 timeOfShot " + timeOfShot);
                    //TO DELETE
                    //typeOfShot = 2;

                    //if (preShotActive && !isShotActive)
                    //{




                    /*Init shot values*/
                    //if (isShotActive)

                    float screenWidth = (float)parentRb.screenWidth;
                    //Vector3 curveStartPosV3 = 
                    //    new Vector3(0f, 0f, -parentRb.PITCH_HEIGHT_HALF);
                    //Vector3 curveMidPosV3 =
                    //    new Vector3(screenWidth / 2f, screenWidth / 2f, -parentRb.PITCH_HEIGHT_HALF);

                    /*Curve shoot */
                    //if (typeOfShot == 2)
                    //{
                    //   int randCurveSide = UnityEngine.Random.Range(0, 2);
                    /*Left curve */
                    // if (randCurveSide == 0)
                    //{
                    //curveMidPosV3 =
                    //       new Vector3(0, screenWidth, -parentRb.PITCH_HEIGHT_HALF);
                    //} else
                    //{
                    /*Right curve*/
                    //curveMidPosV3 = 
                    //    new Vector3(screenWidth, 0, -parentRb.PITCH_HEIGHT_HALF);
                    // }
                    //}

                    //Vector3 curveEndPosV3 = 
                    //    new Vector3(screenWidth, screenWidth, -parentRb.PITCH_HEIGHT_HALF);



                    
                        /*LEVEL DEPENDENT*/
                        float levelNorm = parentRb.getLevelInterpolation();
                        //float skillNorm = parentRb.getSkillInterpolation(Globals.teamBAttackStrength);
                        float skillNorm =
                            parentRb.getSkillInterpolation(parentRb.attackSkillsCpu);

                        float avg = (levelNorm + skillNorm) / 2.0f;

                    if (!Globals.isMultiplayer)
                    {
                        float speedOffset = Mathf.Lerp(-10f, 30f, avg);
                        shotSpeed = parentRb.getRandFloat(65.0f + speedOffset,
                                                          90.0f + speedOffset);

                        if (Globals.level >= 4)
                        {
                            shotSpeed = Mathf.Max(103f, shotSpeed);
                            if (Globals.level == Globals.MAX_LEVEL)
                            {
                                speedOffset = Mathf.Lerp(0f, 6f, skillNorm);
                                shotSpeed = parentRb.getRandFloat(106f + speedOffset,
                                                                  118f);
                                //shotSpeed = Mathf.Max(105f, shotSpeed);

                            }
                        }

                        if (Globals.level == 3)
                        {
                            shotSpeed = Mathf.Max(85f, shotSpeed);
                        }

                        //print("SHOTSPEED " + shotSpeed + " skills norm " + skillNorm);
                    } else
                    {
                        shotSpeed = 100f + (skillNorm * 22f);
                    }

                    shotSpeed = Mathf.Min(118f, shotSpeed);
                    isLobActive = false;

                    /*LEVEL DEPENDENT*/

                    /*print("DEBUGCHANCETOSHOOT JUST BEFORE SHOOT " 
                        + " isLobPossible "
                        + isLobPossible(opponentPos,
                                        cpuPlayerRb.transform.position)
                        + " volley "
                        + !shotType.Contains("volley") 
                        + " cpuPlayerRb.transform.position " 
                        + cpuPlayerRb.transform.position
                        + " OPPOENT " 
                        + parentRb.getPlayerPosition()); */

                    /*first calculate without consider lobActive*/


                    Vector3 curveStartPosV3 =
                            new Vector3(0f, 0f, -parentRb.PITCH_HEIGHT_HALF);
                    Vector3 curveMidPosV3 =
                        new Vector3(screenWidth / 2f, screenWidth / 2f, -parentRb.PITCH_HEIGHT_HALF);

                    Vector3 curveEndPosV3 =
                                   new Vector3(screenWidth, screenWidth, -parentRb.PITCH_HEIGHT_HALF);


                    Vector3 localInterDistGK = Vector3.zero;
                    Vector3 theBestInterLocalDistGK = new Vector3(-100f, 0f, 0f);
                    Vector3 savedOutShotStart = Vector3.zero;
                    Vector3 savedOutShotEnd = Vector3.zero;
                    Vector3 savedOutShotMid = Vector3.zero;
                    Vector3 savedEndPosOrg = Vector3.zero;
                    SHOTVARIANT savedShotvariant = SHOTVARIANT.STRAIGHT;
                    int savedTypeOfShot = 1;
                    Vector3 savedOutShotBallVelocity = Vector3.zero;

                    bool theBestFound = false;

                    //print("FOUNDGOOD SHOOT ISEXTRAACTIVE: " + isExtraShotActive + " endPosOrg " + endPosOrg + isExtraShotActive);


                    float minDist = UnityEngine.Random.Range(
                        3.8f + Mathf.Lerp(0f, 0.3f, avg), 4.5f);

                    //print("#DGG11SHOT START minDist " + minDist);

                    for (int i = 1; i <= 25; i++)
                    {
                        typeOfShot = UnityEngine.Random.Range(1, 4);
                        //CURVE SHOT
                        if (typeOfShot > 1 && !isChanceToShoot)
                        {
                            int randCurveSide = UnityEngine.Random.Range(0, 2);
                            /*Left curve */
                            if (randCurveSide == 0)
                            {
                                curveMidPosV3 =
                                       new Vector3(0, screenWidth, -parentRb.PITCH_HEIGHT_HALF);
                            }
                            else
                            {
                                /*Right curve*/
                                curveMidPosV3 =
                                   new Vector3(screenWidth, 0, -parentRb.PITCH_HEIGHT_HALF);
                            }
                        }
                        else
                        { //STRAIGHT SHOT
                            curveStartPosV3 =
                                   new Vector3(0f, 0f, -parentRb.PITCH_HEIGHT_HALF);
                            curveMidPosV3 =
                                  new Vector3(screenWidth / 2f, screenWidth / 2f, -parentRb.PITCH_HEIGHT_HALF);
                            curveEndPosV3 =
                                     new Vector3(screenWidth, screenWidth, -parentRb.PITCH_HEIGHT_HALF);
                        }

                        //print("typeOfShot " + typeOfShot);

                        if (!isExtraShotActive &&
                            !isChanceToShoot)
                        {
                            parentRb.findGoalEndPoint(ref endPosOrg, true);
                            //print("#DGG11SHOT endPosORG I: " + i.ToString() + " " + endPosOrg);
                            //endPosOrg = new Vector3(0, 1.3f, -14f);
                        }

                        parentRb.preShotCalc(curveStartPosV3,
                                             curveMidPosV3,
                                             curveEndPosV3,
                                             endPosOrg,
                                             height,
                                             ballRb[activeBall].transform.position,
                                             shotSpeed,
                                             isLobActive,
                                             ref outShotBallVelocity,
                                             ref outShotStart,
                                             ref outShotMid,
                                             ref outShotEnd,
                                             ref shotvariant,
                                             true);

                        //print("FOUNDGOOD SHOOT YUPPII I: " + i + " endPosOrg " + endPosOrg + " shotVariant " + shotvariant);
                        theBestFound = parentRb.getTheBestgoalEnd(
                                                                parentRb.getPlayerRb(),
                                                                ballRb[activeBall].transform.position,
                                                                parentRb.getGkCornerPoints(),
                                                                parentRb.getRunSpeed(),
                                                                parentRb.getGoalSizePlr1(),
                                                                ref typeOfShot,
                                                                endPosOrg,
                                                                outShotStart,
                                                                outShotMid,
                                                                outShotEnd,
                                                                ref localInterDistGK,
                                                                minDist,
                                                                true);
                        //print("#DGG11SHOT endPosORG dist I: " + i.ToString() + " " + localInterDistGK.x);
                        if (Mathf.Abs(localInterDistGK.x) > theBestInterLocalDistGK.x)
                        {
                            theBestInterLocalDistGK.x = Mathf.Abs(localInterDistGK.x);
                            savedOutShotStart = outShotStart;
                            savedOutShotMid = outShotMid;
                            savedOutShotEnd = outShotEnd;
                            savedEndPosOrg = endPosOrg;
                            savedTypeOfShot = typeOfShot;
                            savedShotvariant = shotvariant;
                            savedOutShotBallVelocity = outShotBallVelocity;
                        }
                        
                        if (theBestFound)
                            break;
                    }

                    //print("#DGG11SHOT END "  + shotvariant + " isExtraShotActive " + isExtraShotActive
                    //    + " isChanceToShoot " + isChanceToShoot + " endPosOrg " + endPosOrg);

                    if (!theBestFound)
                    {
                        outShotStart = savedOutShotStart;
                        outShotMid = savedOutShotMid;
                        outShotEnd = savedOutShotEnd;
                        endPosOrg = savedEndPosOrg;
                        typeOfShot = savedTypeOfShot;
                        shotvariant = savedShotvariant;
                        outShotBallVelocity = savedOutShotBallVelocity;
                    }

                    //print("#DBG134 before shot " + endPosOrg + " endPosOrgPred "
                    //    + endPosOrgPred + " isChanceTEDPOSORGTESAToShoot " + isChanceToShoot);

                    //print("FINDL OUTSHOTSTART " + outShotStart + " MID " + outShotMid + " outShotEnd " + outShotEnd);

                    //Special case to take a lob 
                    //Profiler.BeginSample("cpu lob shoot");

                    float zDist = Mathf.Abs(cpuPlayerRb.transform.position.z) +
                                  Mathf.Abs(parentRb.getPlayerPosition().z);

                    //GameObject rotatedRbToBall = new GameObject();

                    parentRb.getRotatedRbToBall(ballRb[activeBall].transform.position,
                                                parentRb.getPlayerRb(),
                                                ref tmpRotatedRbToBall,
                                                parentRb.getGkCornerPoints());

                    //float shotDistanceToTravel = parentRb.calcShotDistance(outShotStart,
                    //                                                       outShotMid,
                    //                                                       outShotEnd,
                    //                                                       shotvariant);

                    //print("DBG1434 shotDistanceToTravel " + shotDistanceToTravel);

                    //timeOfShot =
                    //     parentRb.timeOfBallFlyBasedOnPosition(
                    //         cpuPlayerRb.transform.position, timeOfShot, shotDistanceToTravel);
                    if (zDist < 9f ||
                        (Mathf.Abs(opponentPos.z) < 7f))
                    {

                        Vector3 hitDist = parentRb.getGkBallInterPoint(
                                            tmpRotatedRbToBall,
                                            shotvariant,
                                            outShotStart,
                                            outShotMid,
                                            outShotEnd,
                                            endPosOrg,
                                            timeOfShot,
                                            0f,
                                            ballRb[activeBall].transform.position);
                      
                        if (Mathf.Abs(hitDist.x) < UnityEngine.Random.Range(5f, 8f))
                        {
                            isLobActive = true;

                            float timeOfShotOffset =
                               (Mathf.InverseLerp(1f, parentRb.LEVEL_MAX, Globals.level) * 80f);
                            timeOfShot = UnityEngine.Random.Range(900f, 1000f - timeOfShotOffset);
                            /*print("DEBUGCHANCETOSHOOT LOB ENTERED timeOfShot " + timeOfShot
                                + " Mathf.Lerp "
                                + Mathf.InverseLerp(1f, parentRb.LEVEL_MAX, Globals.level)
                                + " Mathf.Lerp2 " + Mathf.InverseLerp(1f, parentRb.LEVEL_MAX, Globals.level) * 200f);*/

                            //timeOfShot =
                            // parentRb.timeOfBallFlyBasedOnPosition(
                            //     cpuPlayerRb.transform.position, timeOfShot, shotDistanceToTravel);

                            parentRb.preShotCalc(curveStartPosV3,
                                                 curveMidPosV3,
                                                 curveEndPosV3,
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
                                                 true);
                     
                        }
                    }

                    parentRb.matchStatistics.setShot("teamB");
                    if (parentRb.isShotOnTarget(endPosOrg, parentRb.goalSizes))
                    {
                        parentRb.matchStatistics.setShotOnTarget("teamB");
                    }
                }

                //Profiler.EndSample();

                if (preShotActive && !isShotActive)
                {
                    isShotActive = parentRb.prepareShot(animator,
                                                 ref shotType,
                                                 cpuPlayerRb,
                                                 ballRb[activeBall],
                                                 rbRightFoot,
                                                 rbRightToeBase,
                                                 rbLeftToeBase,
                                                 parentRb.goalDownPlane,
                                                 ref initPreShot,
                                                 ref initVolleyShot,
                                                 endPos,
                                                 endPosOrg,
                                                 ref isUpdateBallPosActive,
                                                 ref updateBallPos,
                                                 ref updateBallPosName,
                                                 ref shotRotationDelay,
                                                 true,
                                                 isPrepareShotDelay);
                    if (isShotActive)
                    {
                        /*parentRb.preShotCalc(curveStartPosV3,
                                             curveMidPosV3,
                                             curveEndPosV3,
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
                                             true);*/

                        outShotStart = ballInitPos = ballRb[activeBall].transform.position;
                        timeOfShotOrg = timeOfShot;

                        if (shotvariant == SHOTVARIANT.STRAIGHT)
                        {
                            //ballInit position has changed - 
                            //calculate new velocity (outShotBallVelocity)for straight shot
                            Vector3 curveStartPosV3 = Vector3.zero;
                            Vector3 curveMidPosV3 = Vector3.zero;
                            Vector3 curveEndPosV3 = Vector3.zero;

                            parentRb.straightShotCurves(ref curveStartPosV3,
                                                        ref curveMidPosV3,
                                                        ref curveEndPosV3);

                            parentRb.preShotCalc(curveStartPosV3,
                                                 curveMidPosV3,
                                                 curveEndPosV3,
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
                                                 true);
                        }
                        else
                        {
                            //CURVE
                            float shotDistanceToTravel = parentRb.calcShotDistance(outShotStart,
                                                                                   outShotMid,
                                                                                   outShotEnd,
                                                                                   shotvariant);

                            //print("DBG1434XXX shotDistanceToTravel " + shotDistanceToTravel + " timeOfShot before " + timeOfShot
                            //   );

                            if (shotvariant == SHOTVARIANT.CURVE)
                            {
                                float speedPerc = Mathf.InverseLerp(parentRb.ShotSpeedMax, parentRb.ShotSpeedMin, timeOfShot);
                                parentRb.setBallShotVel(speedPerc * parentRb.MAX_SHOT_SPEED_UNITY_UNITS);
                            }

                            if (!isLobActive)
                            {
                                timeOfShot = parentRb.timeOfBallFlyBasedOnPosition(
                                    cpuPlayerRb.transform.position, timeOfShot, shotDistanceToTravel);
                            }
                            else
                            {
                                timeOfShot =
                                    parentRb.timeOfBallFlyBasedOnPosition(
                                        cpuPlayerRb.transform.position, timeOfShot);
                            }

                           
                            //print("DBG1434XXX scale " + timeOfShot);
                        }

                        //print("shotSpeed " + shotSpeed + " shotvariant " + shotvariant
                        //       + " timeOfShot " + timeOfShot + " islobActive " + isLobActive + " timeOfShotOrg " +
                        //       timeOfShotOrg);

                        /*print("#DBG1434XXX outShotStart " + outShotStart + " outMId " + outShotMid + " outShotEnd " +
                        outShotEnd + " ischanceToShoot " + isChanceToShoot + " shotvariant " + shotvariant
                        + " outShotBallVelocity " + outShotBallVelocity + " shotSpeed " + shotSpeed + " " +
                        " timeOfShot " + timeOfShot + " isExtraShotActive " + isExtraShotActive);*/



                        parentRb.setGkHelperImageVal(true);
                        drawHelperImage();
                        //midPosv3 = (ballInitPos + endPosOrg) / 2.0f;
                        drawGKHelperTime = 0.0f;
                        parentRb.audioManager.PlayNoCheck("kick3");
                    }
                }
                //print("#DBG134 after shot " + endPosOrg);
            }
            else
            {
                /* NON SHOT IMPLEMENTATION */           
                /*Move with ball*/
                isAnyAnimationPlaying = parentRb.checkIfAnyAnimationPlaying(animator, 1.0f);
                if (isOnBall &&
                    !isShotActive &&
                    !preShotActive &&
                     parentRb.isBallinGame() &&
                    !parentRb.isShotTrainingActive() &&
                    !parentRb.isShotBonusActive())
                {
                    towardsNewPos = moveWithBall(isAnyAnimationPlaying);
                }
                else
                {
                    string[] excluded = new string[] { "3D_back_run_cpu",
                                                       "3D_GK_step_left_no_offset",
                                                       "3D_GK_step_right_no_offset" };
                    isAnyAnimationPlaying =
                        parentRb.checkIfAnyAnimationPlaying(animator, 1.0f, excluded);

                    //print("DEBUG2345ANIMPLAY INTRO");

                    if (!isAnyAnimationPlaying &&
                        !isShotActive &&
                        !preShotActive)
                    {
                        /*print("DFAXCEBUG2345ANIMPLAY ISGOTOBALLPOSSIBLE " +
                             isGoToBallPossible(ball.transform.position,
                                                cpuPlayerRb,
                                                parentRb.isShotActive(),
                                                parentRb.isLobShotActive(),
                                                ref comeToBallReached,
                                                true));*/

                        /* ball is on your half or ball taken possible, go to ball */
                        if (!parentRb.isShotTrainingActive() &&
                            !parentRb.isShotBonusActive() &&
                             parentRb.isBallinGame() &&
                             isGoToBallPossible(ballRb[activeBall].transform.position,
                                                cpuPlayerRb,
                                                parentRb.isShotActive(),
                                                parentRb.isLobShotActive(),
                                                ref comeToBallReached,
                                                true))
                        {
                            towardsNewPos = new Vector3(ballRb[activeBall].transform.position.x - cpuPlayerRb.position.x,
                                                        0.0f,
                                                        ballRb[activeBall].transform.position.z - cpuPlayerRb.position.z);

                            if (Globals.isMultiplayer &&
                                goToPosDelay)                               
                            {
                                if (!isGoToPosDelayActive) {
                                    isGoToPosDelayActive = true;
                                    parentRb.StartCoroutine(goToBallDelay(UnityEngine.Random.Range(0.5f, 0.7f)));
                                }
                            } else
                            {
                                goToPos(towardsNewPos);
                            }
                        }
                        else
                        {
                            /*goalkeeper implementation*/
                            bool isShotOnTarget = parentRb.isShotOnTarget(
                                parentRb.getEndPosOrg(), goalSizesCpuTakeAction);

                            /*print("DEBUG2345ANIMPLAY goalie save before isShotOnTarget " +
                                isShotOnTarget +
                                " isBallInGame " + parentRb.isBallinGame()
                                + " isShotActive " + parentRb.isShotActive());*/

                            if (parentRb.isShotActive() &&
                                parentRb.isBallinGame() &&
                                (isShotOnTarget ||
                                isExtraGoals))
                            {
                                //print("DEBUGTOOFAR GOALIESAVE ");
                                //Profiler.BeginSample("goalie save cpu");
                                goalieSave();
                                //Profiler.EndSample();
                                return;
                            }
                            else
                            {
                                //Debug.Log("parentRb " + parentRb);
                                //Debug.Log("parentRb.trainingScrip  " + parentRb.trainingScript);

                                /*GO TO A INIT POSITION, MOVE GK TO FIT OPPONENT MOVES */
                                if (parentRb.trainingScript.isShotTraining())
                                    goalieBasePoint = new Vector3(0.0f,
                                                                  0.0f,
                                                                  parentRb.PITCH_HEIGHT_HALF - 3.0f);

                               
                                float xOffset = Mathf.InverseLerp(
                                    parentRb.PITCH_HEIGHT_HALF,
                                    0,
                                    Mathf.Abs(goalieBasePoint.z)) * 2.0f;

                                if (parentRb.ballRb[parentRb.getActiveBall()].transform.position.x < 0)
                                    xOffset = -xOffset;

                                float pointToGoX = goalieBasePoint.x + (parentRb.ballRb[parentRb.getActiveBall()].transform.position.x / 3.5f)
                                            + xOffset;

                                if (parentRb.isShotBonusActive())
                                {
                                    pointToGoX = goalieBasePoint.x + (parentRb.getPlayerPosition().x / 3.5f)
                                         + xOffset;
                                }

                              
                                if (!parentRb.trainingScript.isShotTraining() &&
                                     // !parentRb.bonusScript.isShotBonusActive() &&
                                     (Globals.level >= 3 || parentRb.bonusScript.isShotBonusActive()))
                                {

                                    if (Mathf.Abs(opponentPos.z) <= 8f)
                                    {
                                        float zOffset = Mathf.Min(12.5f, 5.5f + (8f - Mathf.Abs(opponentPos.z)));
                                        goalieBasePoint.z = Mathf.Max(zOffset, goalieBasePoint.z);
                                        goalieBasePointChanged = true;
                                    }
                                    else
                                    {
                                        if (goalieBasePointChanged)
                                        {
                                            calcNewGoalieBasePoint();
                                            goalieBasePointChanged = false;
                                        }
                                    }
                                }

                                pointToGo = new Vector3(
                                    pointToGoX,
                                    0.0f,
                                    goalieBasePoint.z);

                                if (parentRb.isBallinGame() == false)
                                {
                                    pointToGo.x = 0.0f;
                                }

                                if (parentRb.powersScript.getIsPlayerDownInvisible() ||
                                    parentRb.powersScript.getIsFlareUpEnable())
                                {
                                    pointToGo = new Vector3(
                                        UnityEngine.Random.Range(-1f, 1f),
                                        0f,
                                        UnityEngine.Random.Range(10f, 12.5f));
                                }

                                float dist = Vector3.Distance(cpuPlayerRb.transform.position,
                                                              pointToGo);

                                if (dist <= 0.50f)
                                {
                                    gkPositionReach = true;
                                }
                                else
                                {
                                    if (dist > 1.75f)
                                        gkPositionReach = false;
                                }

                                //print("POINT TO GO " + pointToGo + " ballRb[activeBall] " + ballRb[activeBall].position + " cpuPlayerRb.transform.position "
                                //  + cpuPlayerRb.transform.position + " dist " + dist + " gkPositionReach " + gkPositionReach);

                                //print("DistHERE" + dist);
                                if (!gkPositionReach &&
                                    //(!parentRb.isShotActive() ||
                                    //(ball.transform.position.z < 0f ||
                                    (parentRb.ballRb[parentRb.getActiveBall()].position.z < 0f ||
                                    !parentRb.isBallinGame() ||
                                     parentRb.isShotBonusActive()))
                                {

                                    float currentSpeed = speed;
                                    parentRb.interruptSideAnimation(animator, cpuPlayerRb);
                                    towardsNewPos = new Vector3(pointToGo.x - cpuPlayerRb.position.x,
                                                                0.0f,
                                                                pointToGo.z - cpuPlayerRb.position.z);

                                    towardsNewPos = towardsNewPos.normalized;

                                    if (Globals.isMultiplayer &&
                                        goToPosAfterOut &&
                                        !parentRb.isBallinGame())
                                    {
                                        currentSpeed = 0f;

                                        if (!isGoToPosAfterOut)
                                        {
                                            isGoToPosAfterOut = true;
                                            parentRb.StartCoroutine(
                                                goToPosAfterBallOut(UnityEngine.Random.Range(0.3f, 0.6f)));
                                        }
                                    }
                               
                                    cpuPlayerRb.velocity = towardsNewPos * currentSpeed;
                                    
                                    //Vector3 opponentPos =
                                    //    parentRb.getPlayerPosition();
                                    float playerDistToBall =
                                        Vector2.Distance(
                                            new Vector2(opponentPos.x, opponentPos.z),
                                            new Vector2(
                                                parentRb.ballRb[activeBall].transform.position.x,
                                                parentRb.ballRb[activeBall].transform.position.z));

                                    if (parentRb.isShotBonusActive())
                                    {
                                        playerDistToBall = Vector2.Distance(
                                            new Vector2(opponentPos.x, opponentPos.z),
                                            new Vector2(
                                                parentRb.getPlayerPosition().x,
                                                parentRb.getPlayerPosition().z));
                                    }

                                    //string animName = "3D_run_cpu";
                                    string animName = "3D_run";

                                    /*print("DEBUGTOOFAR DIST " + dist
                                        + " playerDistToBall " + playerDistToBall 
                                        + " isShotActive " + parentRb.isShotActive() 
                                        + " ANIM NORM " 
                                        + parentRb.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime 
                                        + " pointToGo " + pointToGo + " EULER " 
                                        + cpuPlayerRb.transform.eulerAngles);*/

                                    if (parentRb.isBallinGame() &&
                                        dist < 5.0f)
                                    {
                                        towardsNewPos = new Vector3(
                                            parentRb.ballRb[parentRb.getActiveBall()].transform.position.x - cpuPlayerRb.position.x,
                                            0.0f,
                                            parentRb.ballRb[parentRb.getActiveBall()].transform.position.z - cpuPlayerRb.position.z);

                                        if (parentRb.isShotBonusActive())
                                        {
                                            towardsNewPos = new Vector3(
                                                           parentRb.getPlayerPosition().x - cpuPlayerRb.position.x,
                                                           0.0f,
                                                           parentRb.getPlayerPosition().z - cpuPlayerRb.position.z);
                                        }


                                        currentSpeed = backSpeed;
                                        //animName = "3D_back_run_cpu";
                                        //animName = "3D_run_cpu";
                                        animName = "3D_run";
                                    }

                                    //Vector3 pointToGoLocal = parentRb.InverseTransformPointUnscaled(
                                    //                                 cpuPlayer.transform, pointToGo);

                                    /*if (pointToGoLocal.z > 0)
                                    {
                                        animName = "3D_run_cpu";
                                    } else
                                    {
                                        currentSpeed = backSpeed;
                                        animName = "3D_back_run_cpu";
                                    }*/

                                    //print("DEBUGGOTOBALLINIT CPU VELOCITY " + cpuPlayerRb.velocity);
                                    if (!parentRb.isPlaying(animator, animName, 0.95f))
                                    {
                                        animator.Play(animName, 0, 0.0f);
                                        //print("DEBUGGOTOBALLINIT ANIMNAME " + animName);
                                        animator.Update(0f);
                                    }
                                }
                                else
                                {
                                    //print("POINT TO GO STAND STILL");
                                    /*Stand still and wait */
                                    /*Look at ball */

                                    towardsNewPos = new Vector3(parentRb.ballRb[parentRb.getActiveBall()].transform.position.x - cpuPlayerRb.position.x,
                                                                0.0f,
                                                                parentRb.ballRb[parentRb.getActiveBall()].transform.position.z - cpuPlayerRb.position.z);

                                    if (parentRb.isShotBonusActive())
                                    {
                                        towardsNewPos = new Vector3(parentRb.getPlayerPosition().x - cpuPlayerRb.position.x,
                                                                    0.0f,
                                                                    parentRb.getPlayerPosition().z - cpuPlayerRb.position.z);
                                    }


                                    if (parentRb.isBallinGame() == false)
                                    {
                                        towardsNewPos = new Vector3(0.0f - cpuPlayerRb.position.x,
                                                                    0.0f,
                                                                    0.0f - cpuPlayerRb.position.z);                                        
                                    }

                                    towardsNewPos = towardsNewPos.normalized;
                                    parentRb.interruptSideAnimation(animator, cpuPlayerRb);
                                    parentRb.gkStandStill(animator, cpuPlayerRb);

                                    //print("TOWARD NEW POS NOT EXECUTED 5 " + cpuPlayerRb.velocity);
                                    cpuPlayerRb.velocity = Vector3.zero;
                                    isPlayerStandStill = true;
                                }
                            }
                        }
                    }
                }
            }

            //isAnyAnimationPlaying = parentRb.checkIfAnyAnimationPlaying(animator, 1.0f);
            //bool isRunTurnAnimationPlaying = parentRb.checkIfAnyAnimationPlayingContain(
            //                     parentRb.RunAnimationsNames, animator, 0.99f, "3D_run_");
            if (preShotActive ||
                isShotActive ||
                 //(!isAnyAnimationPlaying &&
                 // !isRunTurnAnimationPlaying &&
                 (!parentRb.isAnimatorPlaying(animator) &&
                   parentRb.isRotationPossible(animator,
                                               lastGkAnimName,
                                               lastTimeGkAnimPlayed)))
            {
                /* if (preShotActive && 
                      !isShotActive && 
                      !isExtraShotActive && 
                      !isChanceToShoot)
                  {*/
                //look at the middle of the goal during preparation of the shoot
                /*    parentRb.RblookAt(cpuPlayerRb,
                                      onBall,
                                      towardsNewPos,
                                      animator,
                                      preShotActive,
                                      new Vector3(0f,0f, -parentRb.PITCH_HEIGHT_HALF), //look at the middle
                                      true,
                                     "3D_shot_right_foot");

                    if (preShotActive || isShotActive)
                        print("#DBG136 before shot VERSION1 preShot " + preShotActive + " shotActive " + isShotActive);
                }
                else
                {*/
                if (preShotActive &&
                    parentRb.isAnimatorPlaying(animator) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= shotRotationDelay)
                {
                    float lookSide = parentRb.getGoalSizePlr1().x;
                    if (endPosOrg.x > 0)
                        lookSide = -lookSide;

                    parentRb.RblookAt(cpuPlayerRb,
                                      onBall,
                                      towardsNewPos,
                                      animator,
                                      preShotActive,
                                      new Vector3(lookSide, 0f, -parentRb.PITCH_HEIGHT_HALF), //look at the middle
                                      true,
                                      "3D_shot_right_foot");
                }
                else
                {

                    if (isPlayerStandStill && (
                        parentRb.powersScript.getIsPlayerDownInvisible() ||
                        parentRb.powersScript.getIsFlareUpEnable()))
                    {

                        parentRb.RblookAt(cpuPlayerRb,
                                     onBall,
                                     new Vector3(UnityEngine.Random.Range(-12f, 12f), 0, -parentRb.PITCH_HEIGHT_HALF),
                                     animator,
                                     preShotActive,
                                     endPosOrg,
                                     true,
                                     "3D_shot_right_foot");
                    }
                    else
                    {
                        parentRb.RblookAt(cpuPlayerRb,
                                         onBall,
                                         towardsNewPos,
                                         animator,
                                         preShotActive,
                                         endPosOrg,
                                         true,
                                         "3D_shot_right_foot");
                    }
                    //if (preShotActive || isShotActive)
                    //    print("#DBG136 before shot VERSION2 preShot " + " shotRotationDelay " + shotRotationDelay 
                    //        + preShotActive + " shotActive " + isShotActive + " " +
                    //        "endPosOrgPred " + endPosOrgPred + " isExtraShotActive " + isExtraShotActive);
                    //}
                }

                //print("#DBG139 before shot endPosOrg " + " shotRotationDelay " + shotRotationDelay + " "  + endPosOrg + " endPosOrgPred " + endPosOrgPred + " " +
                //    " preShotActive " + preShotActive + " isShotActive " + isShotActive);
            }

            //print("ISSHOTACTIVE " + isShotActive);

            if (isShotActive)
            {
                shot();
            }

            //lastBallPos = ball.transform.position;  
            lastBallPos = ballRb[activeBall].transform.position;


        }
        public string getShotType()
        {
            return shotType;
        }

        public Vector3 getEndPosOrg()
        {
            return endPosOrg;
        }

        public Vector3 getOutShotStart()
        {
            return outShotStart;
        }

        public Vector3 getOutShotMid()
        {
            return outShotMid;
        }

        public Vector3 getBallInit()
        {
            return ballInitPos;
        }

        public Vector3 getOutShotEnd()
        {
            return outShotEnd;
        }

        public int getActiveBall()
        {
            return activeBall;
        }

        private void initMultiplayerSettings()
        {
            if (!Globals.isMultiplayer)
                return;

            isGoToPosDelayActive = false;
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                goToPosDelay = true;
            }
            else
            {
                goToPosDelay = false;
            }

            goToPosDelay = true;

            isGoToPosAfterOut = false;
            if (UnityEngine.Random.Range(0, 4) <= 2)
            {
                goToPosAfterOut = true;
            }
            else
            {
                goToPosAfterOut = false;
            }
        }

        private bool isLobPossible(Vector3 opponentPos,
                                   Vector3 playerPos)
        {
            if (Mathf.Abs(playerPos.z) > 5.3f &&
                Mathf.Abs(opponentPos.z) < 3.2f)
                return true;

            return false;
        }

        private bool isLobPossible(Vector3 opponentPos,
                                   Vector3 playerPos,
                                   float minPlayerZ)
        {
            //if (Mathf.Abs(opponentPos.z) > 5.5f ||
            if (Mathf.Abs(playerPos.z) > minPlayerZ &&
                Mathf.Abs(opponentPos.z) < 5.5f)
                return true;

            return false;
        }

        /*This function check whether player is in wrong position, so we can shoot immediately*/
        /*private bool opponentWrongPosition(Vector3 pos)
        {
            if (Mathf.Abs(pos.z) > 3.0f &&
                Mathf.Abs(pos.x) > 8.0f)
                return true;

            return false;
        }*/
        public float getLastTimePlayerOnBall()
        {
            return lastTimePlayerOnBall;
        }

        public bool getIsPlayerOnTheBallNow()
        {
            return isPlayerNowOnBall;
        }

        public float getTimeOfBallFly()
        {
            return timeOfShot;
        }

        public float getPassedTime()
        {
            return passedTime;
        }

        public SHOTVARIANT getShotVariant()
        {
            return shotvariant;
        }

        private Vector3 getPointInsideShotZone(Rect shotZone)
        {
            float xRand = parentRb.getRandFloat(shotZone.xMin, shotZone.xMax);
            float yRand = parentRb.getRandFloat(shotZone.yMin, shotZone.yMax);

            if (parentRb.powersScript.getIsPlayerUpInvisible())
            {
                xRand = parentRb.getRandFloat(-(parentRb.PITCH_WIDTH_HALF - 3f), parentRb.PITCH_WIDTH_HALF - 3f);    
                yRand = parentRb.getRandFloat(3f, 7f);
                for (int i = 0; i < 10; i++)
                {
                    float dist = Vector2.Distance(new Vector2(cpuPlayerRb.transform.position.x,
                                                              cpuPlayerRb.transform.position.z),
                                                  new Vector2(xRand, 
                                                              yRand));
                    if (dist > 10f)
                    {
                        break;
                    }
                }
            }

            /*if (Globals.isMultiplayer)
            {
                xRand = parentRb.getRandFloat(-(parentRb.PITCH_WIDTH_HALF - 4f), parentRb.PITCH_WIDTH_HALF - 4f);
                yRand = parentRb.getRandFloat(3f, 12f);
            }*/

            return new Vector3(xRand, 0.0f, yRand);
        }

        public void setExtraGoals(bool val, Vector3 size)
        {
            isExtraGoals = val;
            recalculateCornerPoints(size);
        }

        public void recalculateCornerPoints(Vector3 sizes)
        {
            goalSizesCpuTakeAction = new Vector3(parentRb.goalSizesCpu.x,
                                                        parentRb.goalSizesCpu.y,
                                                        parentRb.goalSizesCpu.z);

            float levelInter = parentRb.getLevelInterpolationReverse();

            /*LEVEL DEPENDENT*/
            goalSizesCpuTakeAction.x *= (1.5f + levelInter);
            goalSizesCpuTakeAction.y *= (1.3f + levelInter);

            gkCornerPoints = new Vector3(
                goalSizesCpuTakeAction.x, 0f, parentRb.PITCH_HEIGHT_HALF);
        }

        private void prepareShotCpuOptions()
        {
            Vector3 opponentPos = parentRb.getPlayerPosition();

            /*LEVEL DEPENDENT*/
            float levelNorm =
                parentRb.getLevelInterpolationReverse();
            //float skillsNorm = 
            //    parentRb.getSkillInterpolationReverse(Globals.teamBAttackStrength);
            float skillsNorm =
                parentRb.getSkillInterpolationReverse(parentRb.attackSkillsCpu);

            float avg = (levelNorm + skillsNorm) / 2.0f;
            float startInter = Mathf.Lerp(100f, 250f, avg);
            float endInter = Mathf.Lerp(650f, 300f, avg);
            float timeOfShotRangeLeft = parentRb.ShotSpeedMin + startInter;
            float timeOfShotRangeRight = parentRb.ShotSpeedMax - endInter;


            if (Globals.level >= 4)
            {
                timeOfShotRangeLeft = 480f;
                timeOfShotRangeRight = 481f +
                    (skillsNorm * 60f * (parentRb.LEVEL_MAX - (Globals.level - 1)));
            }

            if (Globals.level == 3)
            {
                timeOfShotRangeRight = 800 - Mathf.Lerp(150f, 0f, avg);
                //print("LEFTRIGHT " + timeOfShotRangeLeft + " timeOfShotRangeRight " + timeOfShotRangeRight);
            }

            if (timeOfShotRangeRight < timeOfShotRangeLeft)
                timeOfShotRangeRight = timeOfShotRangeLeft + 30f;

            /*its only for curve shots*/
            timeOfShot = parentRb.getRandFloat(timeOfShotRangeLeft,
                                               timeOfShotRangeRight);

            //print("####timeOfShot " + timeOfShot);

            //timeOfShot = 480f;

            /*print("timeOfShot " + timeOfShot 
                + " timeOfShotRangeLeft " 
                + timeOfShotRangeLeft
                + " RIGHT "
                + timeOfShotRangeRight  
                + " avg " + avg
                + " skillsNorm " + skillsNorm
                + " Globals.teamBAttackStrength " 
                + Globals.teamBAttackStrength);
            */

            /* 0-1 normal 2 curve */
            typeOfShot = UnityEngine.Random.Range(0, 11);
            if (typeOfShot > 5)
                typeOfShot = 2;
            else
                typeOfShot = 0;

            /*TODELETE*/
            //typeOfShot = 2;
            //typeOfShot = 0;

            /*print("DEBUGSHOTCURVE SHOT SPEED " +
                " timeOfShotRangeLeft " + timeOfShotRangeLeft
                + " timeOfShotRangeRight " + timeOfShotRangeRight 
                + " timeOfShot " + timeOfShot + " typeOfShot " + typeOfShot);*/


            /* + timeOfShot + " startInter " + 
             startInter + " ENDINTER " 
             + endInter
             + " typeOfShot " + typeOfShot);*/


            /*TODELETE*/
            //typeOfShot = 2;
            curveShotDirection = UnityEngine.Random.Range(0, 2);
            if (curveShotDirection == 0)
                curveShotDirection = -1;

            shotPreperationActive = true;
            onBall = PlayerOnBall.ONBALL;

            if (!Globals.isTrainingActive &&
                !parentRb.isBonusActive)
            {

                bool notLeadingInTheEndOfMatch = false;
                if ((Globals.score1 >= Globals.score2))
                {
                    if ((Globals.score2 + 2) < Globals.score1)
                        notLeadingInTheEndOfMatch = true;

                    if (((Globals.score2 + 1) < Globals.score1) && (parentRb.getMatchTimeMinute() > 50))
                        notLeadingInTheEndOfMatch = true;
                }

                int randTime = UnityEngine.Random.Range(45, 70);
                if ((parentRb.getMatchTimeMinute() > randTime) ||
                    (UnityEngine.Random.Range(0, 4) == 0))
                    notLeadingInTheEndOfMatch = true;

                if (notLeadingInTheEndOfMatch)
                {
                    int rand = UnityEngine.Random.Range(0, 2);

                    if (parentRb.getShotActive() &&
                        (rand == 0 ||
                        (Globals.stadiumNumber != 0)))
                    {
                            if ((randPower[(int)POWER.CUT_GOAL_BY_HALF] == 5)) {
                                parentRb.powersScript.cutGoalByHalf(true,
                                                        (int) POWER.CUT_GOAL_BY_HALF,
                                                        new Vector3(1.48f, 1.44f / 1.5f, 1f),
                                                        new Vector3(5.25f, 3.5f / 1.5f, 14.0f));
                            }

                            if (randPower[(int) POWER.GOAL_WALL] == 5)
                            {
                                parentRb.powersScript.goalObstacles(true,
                                                                (int)POWER.GOAL_WALL,
                                                                Vector3.zero,
                                                                Vector3.zero);
                            }
                        //print("POWERDBG resize opponent goal");
                    }
                    else
                    {
                        //print("POWERDBG extra goals");
                        if (preShotActive || isShotActive)
                        {
                                if ((randPower[(int)POWER.TWO_EXTRA_GOAL] == 5) &&
                                    (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.TWO_EXTRA_GOAL])) {
                                    parentRb.powersScript.twoExtraGoals(true,
                                                                   (int)POWER.TWO_EXTRA_GOAL,
                                                                    Vector3.zero,
                                                                    Vector3.zero);
                                }
                 
                                if (Globals.level >= 3)
                                {
                                    if ((randPower[(int) POWER.SHAKE_CAMERA] == 5) &&
                                        (parentRb.getMatchTimeMinute() > randPowerMatchTime[(int)POWER.SHAKE_CAMERA]))
                                    {
                                        parentRb.powersScript.shakeCamera(true,
                                                                      (int)POWER.SHAKE_CAMERA,
                                                                      Vector3.zero,
                                                                      Vector3.zero);
                                    }
                                }
                        }
                    }
                }
            }

            //print("SHOTACTIVECPU ");
            //float goalDownX = 0.0f;
            //print("RANDOMX VALUE " + goalDownX);
            //goalDownX = parentRb.getRandFloat(-3.0f, 3.0f);

            /*LEVEL DEPENDENT*/
            /*skills are not taken here at all!? */
            //float xOffsetInter = parentRb.getLevelInterpolationReverse();           
            float goalEndMax = 5.15f;
            if (Globals.level >= 3)
            {
                goalEndMax += (skillsNorm / 1.5f);
            }

            //print("goalEndMax " + goalEndMax + " skillsNORM " + skillsNorm + " (skillsNorm / 2f) " + (skillsNorm / 2f));

            float leftGoalDownX =
                parentRb.getRandFloat(-(goalEndMax + (levelNorm * 1.5f)), -4.0f);
            float rightGoalDownX =
                parentRb.getRandFloat(4.0f, goalEndMax + (levelNorm * 1.5f));
            float middleGoalDownX =
                parentRb.getRandFloat(-2.5f, 2.5f);


            //int randomSide = UnityEngine.Random.Range(0, 2);
            //goalDownX = leftGoalDownX;
            //if (randomSide == 1)
            //{
            //goalDownX = rightGoalDownX;
            //}

            /*LEVEL DEPENDENT*/
            float heightMaxInter = parentRb.getLevelInterpolationReverse();
            float goalDownHeight = parentRb.getRandFloat(0.0f, 3.0f + heightMaxInter);

            initPreShot = false;
            initVolleyShot = false;
            passedTime = 0.0f;
            shotRet = false;
            endPosOrg = new Vector3(leftGoalDownX, goalDownHeight, -parentRb.PITCH_HEIGHT_HALF);

            /*draw whether an extra shot */
            //if (Globals.level >= 3)
            //    randomSide = UnityEngine.Random.Range(1, 101);

            //bool isExtraShot = false;
            //if (randomSide > (100 - (Globals.level * 3)))
            //{
            //    randomSide = UnityEngine.Random.Range(0, 2);                
            //endPosOrg = extraShotVec;
            //if (randomSide == 0)
            //    endPosOrg.x *= -1;
            //}

            isExtraShotActive = false;
            endPosOrg = new Vector3(0, 0, -parentRb.PITCH_HEIGHT_HALF);
            if (preShotActive &&
                endPosOrgPred != parentRb.INCORRECT_VECTOR)
            {
                if (endPosOrgPred.x < 0f)
                    endPosOrg.x = leftGoalDownX;
                else if (endPosOrgPred.x > 0f)
                {
                    endPosOrg.x = rightGoalDownX;
                }
                else
                {
                    endPosOrg.x = middleGoalDownX;
                }

                //print("DEBUGCHANCETOSHOOT UPDATE PREDEF " + endPosOrgPred + " ENDPOS " + endPosOrg);

                typeOfShot = 0;
            } else
            {
                if (parentRb.isExtraShot(ref endPosOrg))
                    isExtraShotActive = true;
            }

            //else
            //{

            /*Vector3 theBestEndGoalPos = parentRb.getTheBestgoalEnd(
                                                  parentRb.getPlayerRb(),
                                                  ballRb[activeBall].transform.position,
                                                  parentRb.getGkCornerPoints(),
                                                  parentRb.getRunSpeed(),
                                                  parentRb.getGoalSizePlr1(),
                                                  ref typeOfShot,
                                                  ref endPosOrg,
                                                  true);


            if (theBestEndGoalPos != parentRb.INCORRECT_VECTOR)
            {
                if (Mathf.Abs(theBestEndGoalPos.x) < parentRb.getRandFloat(4f, 6.5f))
                {
                    int randomSide = UnityEngine.Random.Range(0, 2);
                    if (randomSide == 1)
                        endPosOrg.x *= -1;
                }
                else
                {
                    if (theBestEndGoalPos.x < 0f)
                        endPosOrg.x = leftGoalDownX;
                    else if (theBestEndGoalPos.x > 0f)
                    {
                        endPosOrg.x = rightGoalDownX;
                    }
                    else
                    {
                        endPosOrg.x = middleGoalDownX;
                    }
                }
            }*/
            //}

            /*extra shot*/
            //if (Globals.level >= 3)
            //{
            //    int randomSide = UnityEngine.Random.Range(1, 101);
            //    if (randomSide > (100 - (Globals.level * Globals.level)))
            //    {
            //       if (endPosOrg.x > 0f)
            //           endPosOrg = extraShotVec;
            //       else {
            //           endPosOrg = extraShotVec;
            //           endPosOrg.x *= -1;
            //      }
            //   }
            // }

            /*don't curve when shot across ground */
            if (endPosOrg.y < 0.75f)
                typeOfShot = 0;



            /*TOREMOVE*/
            //endPosOrg = extraShotVec;
            //endPosOrg = new Vector3(3.2f, 3.3f, -14f);
            //typeOfShot = 2;
            //endPosOrg.x *= -1;
            //endPosOrg = parentRb.m_MainCamera.transform.position;
            //Vector3 cameraPos = parentRb.m_MainCamera.transform.position;
            //endPosOrg = new Vector3(cameraPos.x, cameraPos.y + 0.3f, cameraPos.z);
            //endPosOrg.x = -4.3f;
            //endPosOrg.y = 0f;
            //typeOfShot = 2;
            /*TOREMOVE*/
            //endPosOrg = new Vector3(0, 0, -6);
            //timeOfShot = 1800f;
            //typeOfShot = 0;
        }

        public bool isShotCurve()
        {
            if (shotvariant == SHOTVARIANT.CURVE)
                return true;
            return false;
        }

        IEnumerator prepShotDelayCpu(float delay)
        {
            isPrepareShotDelay = true;
            yield return new WaitForSeconds(delay);
            isPrepareShotDelay = false;
        }

        private bool isFinalMovePointReached()
        {

            float dist = Vector3.Distance(cpuPlayerRb.transform.position,
                                          pointInsideShotZone);
            Vector2 playerPosV2 = new Vector2(cpuPlayerRb.transform.position.x,
                                              cpuPlayerRb.transform.position.z);

            if (parentRb.checkIfAnyAnimationPlayingContain(
                             parentRb.RunAnimationsNames, animator, 0.99f, "3D_run_"))
            {
                //print("MOVE CHANGE DIRECTION BEFORE ANIM PLAYING DIST " + dist);
                return false;
            }

            /*if ((dist < randDistanceShotZone) && 
                (moveChangeDirectionCounter > 0 || shotZone.Contains(playerPosV2))) { */

            if ((dist < randDistanceShotZone) &&
                 (shotZone.Contains(playerPosV2) ||
                  moveChangeDirectionCounter > 0))
            {
                moveChangeDirectionCounter++;
                if (moveChangeDirectionCounter != maxMoveChangePos)
                {
                    Vector3 pointInOpponentGoal =
                        new Vector3(
                            parentRb.getRandFloat(-parentRb.PITCH_WIDTH_HALF, parentRb.PITCH_WIDTH_HALF),
                                                   0f,
                             parentRb.getRandFloat(
                                 //1.0f,
                                 -5.0f,
                                 cpuPlayerRb.transform.position.z));
                    //Math.Max(1.1f, Mathf.Max(cpuPlayerRb.transform.position.z)
                    //Math.Max(1.1f, Mathf.Max(cpuPlayerRb.transform.position.z))));

                    float distToMove = parentRb.getRandFloat(speed / 2f, (speed / 2f) + 1f);
                    Vector3 lookDirection =
                        (pointInOpponentGoal - cpuPlayerRb.transform.position).normalized *
                            distToMove;

                    Vector3 pointZoneWorld =
                        cpuPlayerRb.transform.position + lookDirection;

                    if (!parentRb.isTurnAnimPossible(pointZoneWorld))
                    {
                        pointZoneWorld =
                            cpuPlayerRb.transform.position;
                    }

                    /*print("DEBUG2345ANIMPLAY CHANGEPOS COUNTER  "
                        + distToMove + " speed " + speed
                        + " moveChangeDirectionCounter "
                        + moveChangeDirectionCounter
                        + " maxMoveChangePos "
                        + maxMoveChangePos
                        + " pointZoneWorld "
                        + pointZoneWorld
                        + " PLAYERPOS "
                        + cpuPlayerRb.transform.position);*/

                    pointInsideShotZone = new Vector3(
                        pointZoneWorld.x,
                        0,
                        pointZoneWorld.z);
                    pointInsideShotZoneDone = false;
                    randDistanceShotZone = 0.30f;
                }
                else
                {
                    if (Mathf.Abs(parentRb.getPlayerPosition().z) < 5.5f &&
                        Mathf.Abs(cpuPlayer.transform.position.z) < 5.5f)
                    {
                        moveChangeDirectionCounter--;
                        pointInsideShotZone = new Vector3(
                            parentRb.getRandFloat(-6f, 6f),
                            0f,
                            parentRb.getRandFloat(shotZone.yMin + 2f, shotZone.yMax - 1f));
                        pointInsideShotZoneDone = false;
                        /*print("DEBUGCHANCETOSHOOT NEW POINT CALC "
                            + pointInsideShotZone
                            + " shotZone.xMin " + shotZone.xMin
                            + " shotZone.xMax " + shotZone.xMax
                            + " shotZone.yMin " 
                            + shotZone.yMin
                            + " shotZone.yMax " 
                            + shotZone.yMax
                            + " SHOTZONE " + shotZone);*/
                        randDistanceShotZone = 0.15f;
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public void playerFreeze(bool freezePlayer)
        {
            isPlayerFreeze = freezePlayer;
        }

        public void clearPreShotVariables()
        {
            preShotActive = false;
            isShotActive = false;
            initPreShot = false;
            initVolleyShot = false;
            passedTime = 0.0f;
            prepareCpuOptions = false;
            isBallTrailRendererInit = false;
            drawGKHelperTime = 0.0f;
            gkPositionReach = false;
            pointInsideShotZone = getPointInsideShotZone(shotZone);
            pointInsideShotZoneDone = false;
            //moveChangeDirectionCounter = UnityEngine.Random.Range(1, 3);
            moveChangeDirectionCounter = 0;
            /*TODELETE*/
            //maxMoveChangePos = 4;
            maxMoveChangePos = UnityEngine.Random.Range(3, 5);

            //print("MOVE CHANGE DIRECTION COUNTER RANDOM " + maxMoveChangePos);


            /*LEVEL DEPENDENT*/
            float levelInter = parentRb.getLevelInterpolationReverse();
            float randDistInter = Mathf.Lerp(0, 15.0f, levelInter);
            randDistanceShotZone = parentRb.getRandFloat(1.0f, 3.0f + randDistInter);
            //print("DEBUGZGGG123744XYZ  " + randDistInter + " RANDDISTSHOTZONE " + randDistanceShotZone);

            initCpuAdjustAnimSpeed = false;
            initGkDeleyLevel = false;
            levelDelay = 0.0f;
            gkLobPointReached = false;
            gkRunPosReached = false;
            initDistX = -1;

            activeBallFound = false;
        }

        private void move()
        {

        }

        public float getGkLastDistXCord()
        {
            return lastGkDistX;
        }

        private void shot()
        {

            bool isLobActive = false;

            //if (passedTime <= timeOfShot && !shotRet) {
            if (!shotRet)
            {
                shotRet = parentRb.shot3New(outShotStart,
                                            outShotMid,
                                            outShotEnd,
                                            outShotBallVelocity,
                                            ref lastBallVelocity,
                                            shotvariant,
                                            Mathf.InverseLerp(0.0f, timeOfShot, passedTime));
            }
            //}
            curveShotFlyPercent = Mathf.InverseLerp(0.0f, timeOfShot, passedTime);

            float shotDistanceToTravel = parentRb.calcShotDistance(outShotStart,
                                                                   outShotMid,
                                                                   outShotEnd,
                                                                   shotvariant);

            //print("CPUTIMEOFSHOT " + timeOfShot);

            if (!isBallTrailRendererInit)
            {
                //if (timeOfShotOrg < 550f)
                if (parentRb.powersScript.getGoalDownHandical() != 0)
                {
                    parentRb.ball[1].ballTrailRendererInit();
                    //isBallTrailRendererInit = true;
                    parentRb.audioManager.PlayNoCheck("ballFastSpeed");
                    //parentRb.audioManager.Play("kick2");
                }
                else
                {
                    parentRb.audioManager.PlayNoCheck("kick2");
                }
                isBallTrailRendererInit = true;
            }
            passedTime = passedTime + (Time.deltaTime * 1000.0f);
        }

        /*NOT USED*/
        /*
        private bool isOnBall()
        {
            float distance = Vector3.Distance(cpuPlayerRb.transform.position,
                                             (ballRb[activeBall].transform.position));

            return (distance < parentRb.minDistToBallShot && ballRb[activeBall].position.y < 0.3f && onBall != PlayerOnBall.ISGK) ? true : false;
        }*/

        /*private void interruptedShotClean()
        {
            isShotActive = false;
            preShotActive = false;
            isBallTrailRendererInit = false;
            parentRb.ball.ballTrailRendererInit();
        }*/

        public float getCurveShotFlyPercent() {
            return curveShotFlyPercent;
        }

        private Vector3 moveWithBall(bool isAnyAnimationPlaying)
        {
            float dist = Vector3.Distance(cpuPlayerRb.transform.position,
                                          pointInsideShotZone);

            if (!isAnyAnimationPlaying)
            {
                towardsNewPos =
                        (pointInsideShotZone - cpuPlayerRb.transform.position).normalized;
                towardsNewPos.y = 0f;

                bool isRunTurnAnimationPlaying = parentRb.checkIfAnyAnimationPlayingContain(
                        parentRb.RunAnimationsNames, animator, 0.99f, "3D_run_");
                bool isRunAnimationPlaying = parentRb.isPlaying(animator, "3D_run", 0.95f);

                float runSpeed = Mathf.Max(Mathf.Abs(cpuPlayerRb.velocity.x),
                                 Mathf.Abs(cpuPlayerRb.velocity.z));

                //print("DEBUG2345ANIMPLAY  RUNSPEED " + runSpeed);
                runSpeed = Mathf.InverseLerp(0.0f, MAX_RB_CPU_VELOCITY, runSpeed);


                if (parentRb.powersScript.isPlayerUpSlowDown())
                    runSpeed /= 2f;

               animator.SetFloat("3d_run_turn_speed", 1.2f + (runSpeed / 1.8f));


                if (!isRunTurnAnimationPlaying)
                {
                    float angle = Vector2.SignedAngle(new Vector2(towardsNewPos.x,
                                                                  towardsNewPos.z),
                                                      new Vector2(cpuPlayerRb.transform.forward.x,
                                                                  cpuPlayerRb.transform.forward.z));
                    string animName =
                            parentRb.convertAngleToAnimNameRunOnBall(cpuPlayerRb, angle, true);

                    if (Globals.isMultiplayer &&
                        !animName.Equals("3D_run"))
                    {
                        if (UnityEngine.Random.Range(0, 3) <= 1)
                            animName = "3D_run";
                    }

                    //if (!animName.Equals("3D_run"))
                    //    cpuPlayerRb.velocity = Vector3.zero;

                        //if (!animName.Equals("3D_run_cpu") || !isRunAnimationPlaying)
                        if ((!animName.Equals("3D_run") ||
                            !isRunAnimationPlaying))
                    {
                        /*don't execute turn too often and when you are close to border*/
                        if (!animName.Equals("3D_run") &&
                           ((Time.time - lastTimeTurnAnimPlayed) < 1.5f) ||
                            !parentRb.isTurnAnimPossible(cpuPlayerRb.transform.position))
                        //(Mathf.Abs(cpuPlayerRb.transform.position.x) > (parentRb.PITCH_WIDTH_HALF - 2.5f)) ||
                        //(Mathf.Abs(cpuPlayerRb.transform.position.z) > (parentRb.PITCH_HEIGHT_HALF - 2.5f)) ||
                        //(Mathf.Abs(cpuPlayerRb.transform.position.z) < 2.5f))
                        {
                            animName = "3D_run";
                        }

                        //print("ANIMPLAYED CPUANIMPLAYEDNAME " + animName);
                        animator.Play(animName, 0, 0.0f);
                        if (!animName.Equals("3D_run"))
                        {
                            lastTimeTurnAnimPlayed = Time.time;
                            isRunTurnAnimationPlaying = true;
                            //lastTimeTurnCounter++;
                        }
                    }

                    /*print("CPUANIMPLAYEDNAME " + animName + " ANGLE " + angle 
                        + " cpuPlayerRb.transform.forward " + cpuPlayerRb.transform.forward
                        + " towardsNewPos " + towardsNewPos + " pointInsideShotZone " + pointInsideShotZone);*/
                    animator.Update(0f);
                }

                parentRb.interruptSideAnimation(animator, cpuPlayerRb);

                //if (isRunAnimationPlaying)
                cpuPlayerRb.velocity =
                           towardsNewPos * parentRb.getRandFloat(speed - 1.5f, speed);

                if (isRunTurnAnimationPlaying)
                    cpuPlayerRb.velocity /= 1.3f;

                float distToFinalPoint = Vector3.Distance(cpuPlayerRb.transform.position,
                                                          pointInsideShotZone);

                /*print("DEBUG2345ANIMPLAY DISTTOFINALPOINT "
                    + distToFinalPoint + " randDistanceShotZone "
                    + randDistanceShotZone + " playerSpeed " + speed
                    + " TURNSPEED " + (1.2f + (runSpeed / 1.8f)));*/
                if (distToFinalPoint < randDistanceShotZone &&
                    randDistanceShotZone < 0.20f &&
                    isRunTurnAnimationPlaying)
                {
                    //print("DEBUG2345ANIMPLAY ZEROS VELOCITY PLAYER SPEED " 
                    //    + speed + " 1.2f + (runSpeed / 1.8f) " + (1.2f + (runSpeed / 1.8f)));
                    cpuPlayerRb.velocity = Vector3.zero;
                }

                isUpdateBallPosActive = true;
                updateBallPosName = "bodyMain";
                //updateBallPos = towardsNewPos / 1.4f;
            }

            return towardsNewPos;

        }

        private void goalieSave()
        {
            parentRb.gkMoves(animator,
                             cpuPlayerRb,
                             true,
                             ref lastGkAnimName,
                             ref lastTimeGkAnimPlayed,
                             ref lastGkDistX,
                             ref gkStartPos,
                             ref gkStartTransform,
                             ref gkTimeToCorrectPos,
                             ref initCpuAdjustAnimSpeed,
                             ref initGkDeleyLevel,
                             ref levelDelay,
                             ref initAnimName,
                             ref cpuGkAnimAdjustSpeed,
                             ref gkAction,
                             ref gkTimeLastCatch,
                             parentRb.isLobShotActive(),
                             ref stepSideAnimOffset,
                             ref gkLobPointReached,
                             ref gkRunPosReached,
                             ref initDistX,
                             parentRb.getShotVariant(),
                             parentRb.getOutShotStart(),
                             parentRb.getOutShotMid(),
                             parentRb.getOutShotEnd(),
                             parentRb.getEndPosOrg(),
                             parentRb.getTimeOfBallFly(),
                             parentRb.getPassedTime(),
                             ref gkLock,
                             ref rotatedRbToBall,
                             gkCornerPoints,
                             isExtraGoals);
            onBall = PlayerOnBall.ISGK;
        }




        public void goToPos(Vector3 towardsNewPos)
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

            parentRb.interruptSideAnimation(animator, cpuPlayerRb);            
            cpuPlayerRb.velocity = towardsNewPos * speed;


            //print("GKDEBUG7 INTERRUPT IN GO TO POSITION VELOCITY BEFORE " + cpuPlayerRb.velocity);

            //print("GKDEBUG7 INTERRUPT IN GO TO POSITION VELOCITY AFTER " + cpuPlayerRb.velocity);

            //print("TOWARD NEW POS 4 " + cpuPlayerRb.velocity);

            //print("PLAYANIMATION BEFORE");
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("3D_run_cpu") ||
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("3D_run") ||
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            {
                //animator.Play("3D_run_cpu", 0, 0.0f);
                animator.Play("3D_run", 0, 0.0f);
                //print("DEBUGFLASHINGGKDEBUG800 MOVE WITH BALL 2");
            }

            onBall = PlayerOnBall.NEUTRAL;
        }

        public Vector3 getPlayerPosition()
        {
            return cpuPlayer.transform.position;
        }

        private bool isGoToBallPossible(
                        Vector3 ballPos,
                        Rigidbody rb,
                        bool isShotActive,
                        bool isLobActive,
                        ref bool comeToBallReached,
                        bool isCpu)
        {
            Vector3 ballInLocalRb = parentRb.InverseTransformPointUnscaled(
                                                     rb.transform, ballPos);

            float ballDist = parentRb.dist2Dvector3(ballPos,
                                                    cpuPlayerRb.transform.position);


            bool isSlowVel = parentRb.isShotSpeedBelow(28f);
            bool isVerySlowVel = false;
            bool ballOnYourHalf = false;


            /*print("DFAXCEBUG2345ANIMPLAY DIST BEFORE " + parentRb.dist2Dvector3(ballPos,
                                                     cpuPlayerRb.transform.position));*/

            if (!parentRb.isBallReachable(ballRb[activeBall].transform.position,
                                          isCpu))
                return false;

            /*if (ballPos.z < parentRb.BALL_RADIUS)
            {
                ballOnYourHalf = true;
            }*/

            float ballRadius = parentRb.BALL_RADIUS;
            Vector2 ballRbV2 = new Vector2(ballRb[activeBall].transform.position.x,
                                           ballRb[activeBall].transform.position.z);

            //print("DEBUG2345ANIMPLAY 1 " + ballRb[activeBall].transform.position + " ballVel " +
            //    ballRb[activeBall].velocity + " ballPos " + ballPos + " comeToBallReached " + comeToBallReached);

            /*if (ballDist < 1.5f &&
                ballPos.y > ballRadius)
                return false;*/

            if (!isLobActive &&
                endPosOrg.y < 1.4f &&
                isShotActive &&
                parentRb.goAndTakeActiveShotBall(28f,
                                                 goalSizesCpuTakeAction / 1.2f,
                                                 parentRb.getEndPosOrg()))
            {
                return true;
            }

            /*if (isShotActive &&               
                ballPos.y < ballRadius &&
                isVerySlowVel)
            {
                return true;
            }*/

            /*print("DFAXCEBUG2345ANIMPLAY 1 ballInLocalRb don't touch zone "
            + dontTouchBallZone.Contains(ballRb[activeBall]V2)
            + " CPUPLAYER EULER ANGLES " + Mathf.Abs(cpuPlayerRb.transform.eulerAngles.y)
            + " ballPos.y " + ballPos.y + " BALLRADIUS " + ballRadius);*/

            if (dontTouchBallZone.Contains(ballRbV2) &&
                ballPos.y > ballRadius &&
               (!(Mathf.Abs(cpuPlayerRb.transform.eulerAngles.y) > 120f &&
                  Mathf.Abs(cpuPlayerRb.transform.eulerAngles.y) < 240f)))
                return false;

            //print("DEBUG2345ANIMPLAY cpuPlayerRb.transform.eulerAngles.y " + cpuPlayerRb.transform.eulerAngles.y);

            if (((Time.time - parentRb.ball[1].getHeadLastTimeCollisionPlayerUp()) < 0.5f) &&
                ballPos.y > 2.1f)
            {
                //print("DEBUG2345ANIMPLAY 2 JEAD COLLISION " + ballPos.y);
                return false;
            }

            //if (ballOnYourHalf ||
            if (isShotActive && (Mathf.Abs(ballRb[activeBall].velocity.x) > 10.0f ||
                                  Mathf.Abs(ballRb[activeBall].velocity.z) > 10.0f))
            {
                /*print("DFAXCEBUG2345ANIMPLAY 3 JEAD COLLISION " + ballPos.z + " shotvel " +
                    (isShotActive && (Mathf.Abs(ballRb[activeBall].velocity.x) > 10.0f ||
                Mathf.Abs(ballRb[activeBall].velocity.z) > 10.0f)));*/

                return false;
            }

            /*print("DFAXCEBUG2345ANIMPLAY 4  DIST " + parentRb.dist2Dvector3(ballPos,
                                                       cpuPlayerRb.transform.position));*/


            //print("DEBUG2345ANIMPLAY 2.5 ballDist " + ballDist + " comeToBallReached " + comeToBallReached);

            if (ballDist > 1.5f &&
               !comeToBallReached)
            {
                return true;
            }
            else
            {
                comeToBallReached = true;
            }

            /*print("DFAXCEBUG2345ANIMPLAY 4.5 ballInLocalRb " + ball.transform.position.y.ToString("F6")
               + " prevBallPosTest1 " + lastBallPos.y.ToString("F6")
               + " BALLZ " + ballInLocalRb.z
               + " BALLRADIUS " + ballRadius
               + " RES1 " + (ballPos.y < lastBallPos.y)
               + " RES2 " + (ballInLocalRb.z > ballRadius));*/

            if (ballPos.y < ballRadius)
            {
                //print("DEBUG2345ANIMPLAY 5 " + ballPos.y);
                return true;
            }
            else
            {
                /*print("DFAXCEBUG2345ANIMPLAY 6 ballInLocalRb " + ball.transform.position.y.ToString("F6")
                    + " prevBallPosTest1 " + lastBallPos.y.ToString("F6")
                    + " BALLZ " + ballInLocalRb.z
                    + " BALLRADIUS " + ballRadius
                    + " RES1 " + (ballPos.y < lastBallPos.y)
                    + " RES2 " + (ballInLocalRb.z > ballRadius));*/

                if ((ballPos.y < lastBallPos.y) &&
                    (ballInLocalRb.z > ballRadius) &&
                    //ballDist < 1.5f
                    ballPos.y < 1.2f)
                    return true;
            }

            //print("DEBUG2345ANIMPLAY 7 ballInLocalRb " + ballInLocalRb);

            return false;
        }

        public void setShotActive(bool value)
        {
            //print("SETSHOTACTIVECPU " + preShotActive);
            //isBallTrailRendererInit = false;
            //parentRb.ball.ballTrailRendererStop();
            //isShotActive = value;
            //preShotActive = value;
            //parentRb.eraseGkHelperImage();
            clearPreShotVariables();

            /*LEVEL DEPENDENT */
            goalieBasePointChanged = false;

            calcNewGoalieBasePoint();

            //float levelInter = parentRb.getLevelInterpolation();
            //float zStart = Mathf.Lerp(2.0f, 8.0f, levelInter);
            //float zEnd = Mathf.Lerp(8.0f, 12.5f, levelInter);

            //print("DEBUG125125 START GO TO POINT " + zStart);
            //goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.getRandFloat(zStart, zEnd));
            //if (parentRb.trainingScript.isShotTraining())
            //    goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.PITCH_HEIGHT_HALF - 3.0f);
        }

        private void calcNewGoalieBasePoint()
        {
            float levelInter = parentRb.getLevelInterpolation();
            float zStart = Mathf.Lerp(3.5f, 8.0f, levelInter);
            float zEnd = Mathf.Lerp(8.0f, 12.5f, levelInter);

            if (Globals.level >= 4)
            {
                zStart = Mathf.Lerp(8.0f, 10f, levelInter);
                zEnd = Mathf.Lerp(10f, 12.0f, levelInter);
            }

            if (Globals.level == 2)
            {
                zEnd = 11f;
            }

            goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.getRandFloat(zStart, zEnd));
            if (parentRb.trainingScript.isShotTraining())
                goalieBasePoint = new Vector3(0.0f, 0.0f, parentRb.PITCH_HEIGHT_HALF - 3.0f);

            if (Globals.isMultiplayer)
            {
                goalieBasePoint = new Vector3(UnityEngine.Random.Range(-3f, 3f),
                                              0.0f,
                                              parentRb.PITCH_HEIGHT_HALF - UnityEngine.Random.Range(2, 6));
            }
        }

        public float getPlayerSpeed()
        {
            return speed;
        }

        public string getLastGkAnimPlayed()
        {
            return lastGkAnimName;
        }

        public bool getShotActive()
        {
            return isShotActive;
        }

        public bool getPreShotActive()
        {
            return preShotActive;
        }

        public bool isLobShotActive()
        {
            return isLobActive;
        }

        public bool getShotPreparationActive()
        {
            return shotPreperationActive;
        }

        public Animator getAnimator()
        {
            return animator;
        }

        public Rigidbody getPlayerRb()
        {
            return cpuPlayerRb;
        }

        public Vector3 getRbPosition()
        {
            return cpuPlayerRb.transform.position;
        }

        public void setRbPosition(Vector3 pos)
        {
            cpuPlayerRb.transform.position = pos;
        }

        public Transform getRbTransform()
        {
            return cpuPlayerRb.transform;
        }

        public void setAnimatorSpeed(float speed)
        {
            animator.speed = speed;
        }

        public bool isOnBall()
        {
            return parentRb.isPlayerOnBall
                (rbLeftToeBase, rbRightToeBase, ballRb[activeBall], cpuPlayerRb, "move", true, ref activeBall);
        }

        public void lateUpdate()
        {
            if (isShotActive &&
                !shotRet &&
                lastBallVelocity != Vector3.zero
                && parentRb.isBallinGame())
            {
                ballRb[activeBall].velocity = lastBallVelocity;
            }

            parentRb.correctWhenOutOfPitch(animator,
                                  preShotActive,
                                  isShotActive,
                                  cpuPlayerRb,
                                  rbLeftToeBase,
                                  rbRightToeBase,
                                  ballRb[activeBall],
                                  shotType,
                                  ref prevRbPos,
                                  true);

            parentRb.corectBallPositionOnBall(cpuPlayerRb,
                                              animator,
                                              rbRightToeBase,
                                              rbRightFoot,
                                              ref isUpdateBallPosActive,
                                              updateBallPos,
                                              updateBallPosName,
                                              true);


            if (isShotActive &&
                ballRb[activeBall].velocity != Vector3.zero &&
                parentRb.isBallinGame() &&
                parentRb.getGkHelperImageVal())
            {
                //print("DEBUGLASTTOUCHLUCKXYU UPDATE ROTATED " + ballInitPos);

               /* parentRb.drawGkHelperCircle(
                    parentRb.getRotatedRbToBall(ballInitPos,
                                                parentRb.getPlayerRb(),
                                                ref parentRb.getRotatedRbToBallRef(),
                                                parentRb.getGkCornerPoints()),
                                                shotvariant,
                                                outShotStart,
                                                outShotMid,
                                                outShotEnd);
                                                */
               drawHelperImage();
            }

            if (cpuPlayerRb.transform.position.y < 0.03f)
                cpuPlayerRb.transform.position =
                    new Vector3(cpuPlayerRb.transform.position.x, 0.03f, cpuPlayerRb.transform.position.z);
        }

        void drawHelperImage() {
              parentRb.drawGkHelperCircle(
                    parentRb.getRotatedRbToBall(ballInitPos,
                                                parentRb.getPlayerRb(),
                                                ref parentRb.getRotatedRbToBallRef(),
                                                parentRb.getGkCornerPoints()),
                                                shotvariant,
                                                outShotStart,
                                                outShotMid,
                                                outShotEnd);
        }


        public void onAniatorIk()
        {
            parentRb.onAniatorIk(animator,
                                 parentRb.isShotActive(),
                                 parentRb.isLobShotActive(),
                                 lastGkDistX,
                                 cpuPlayerRb,
                                 leftHand,
                                 rightHand,
                                 lastGkAnimName, true);
        }

        IEnumerator goToBallDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            goToPosDelay = false;
        }
        IEnumerator goToPosAfterBallOut(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            goToPosAfterOut = false;
        }

    }
}