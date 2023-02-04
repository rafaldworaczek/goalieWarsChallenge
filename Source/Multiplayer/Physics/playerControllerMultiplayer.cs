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
using Photon.Pun;
using AudioManagerMultiNS;
using System.Globalization;

/*TOCOMMENT*/
//using UnityEngine.Profiling;

public enum SHOTVARIANT2
{
    STRAIGHT = 1,
    CURVE = 2
}


public enum RPC_ACK
{
    GK_PREDICT_CONFIRM = 0,
    GOAL_CONFIRM = 1,
    BALL_IS_OUT = 2,
    BALL_POS_UPDATE = 3,
    PLAYER_ON_BALL = 4,
    BALL_IS_OUT_NEW_POS = 5
}

public class playerControllerMultiplayer : MonoBehaviour
{
    private bool GK_DEBUG = false;
    private bool GK_DEBUG_INIT = true;
    private bool SIMULATE_SHOT = false;
    //public delegate void sendAndACK(int x, int y);
    float RPC_delayAfterGoal = 2.0f;
    float RPC_afterGoalLag = 0f;
    private bool animatorIKExecuted = false;

    private IDictionary<string, float> animationShotAnimSpeed;

    public playerControllerMultiplayer peerPlayer = null;
    public PhotonView photonView;
    private bool isPeerReady = false;
    private bool isMaster = false;
    public float lastTimeSaveAupdate;
    public float lastTimeSaveBupdate;
    //B - buffered, O - onBall, S - shotActive, P - out of pitch, A - another half
    private string mainUpdateType = "M";
    public bool mainUpdateTypeActive = false;
    private bool rpc_shotActive = false;
    private bool rpc_preShotCalc = false;
    private bool rpc_playerOnBallActive = false;
    private bool isLateUpdateBallVelocity = false;
    private bool isLateUpdateBallPos = false;
    private Vector3 lateUpdateBallVelocity;
    private Quaternion lateUpdateBallRot;
    private Vector3 lateUpdateBallPos;
    private Vector3 rpc_rbPrevPos;
    public bool playerOnBall = false;
    private bool prepareShotRpcSend = false;
    private float rpc_mainLag;
    private Vector3 rpc_rbPos;
    private int rpc_joystickExtraButtonsIdx = -1;
    float rpc_runningSpeed;
    public Vector3 rpc_ballPos;
    public float rpcMain_updateTime;
    public Vector3 rpc_ballVelocity;
    public Vector3 rpc_ballAngularVelocity;
    Vector3 rpc_playerDirection;
    Vector3 rpc_rbVelocity;
    Vector3 rpc_rbAngularVelocity;
    Quaternion rpc_rbPlayerRotation;
    private float rpc_prepareShotDelay = 0;
    private float prepareShotDelay = 0.35f;
    private float prepareShotDelay2 = 0.1f;
    private bool predictGkCollisionActive = false;
    GameObject rbTmpGameObj;
    Rigidbody rbTmp;
    GameObject rotatedRbToBallTmp;
    float curvePercentHit = -1f;
    string predictedAnimName = "";
    float gkStartSeq;
    Vector3 localGkStartPos;
    bool updateBallDuringShot = false;
    bool isUpdateBallVelDuringShot = false;
    Vector3 updateBallDuringShotPos;
    Vector3 updateBallDuringShotVel;
    bool preShotPositionActive = false;
    Vector3 preShotPositionVal;

    public struct RPC_blocking
    {
        public Vector3 ballPosBeforeShot;

        public RPC_blocking(Vector3 ballPosBefShot)
        {
            this.ballPosBeforeShot = ballPosBefShot;
        }
    }
    RPC_blocking RPC_locks;
    private Quaternion rpc_rbRotation;
    bool rpc_isShotActive;
    bool rpc_isBallOut = false;
    Quaternion rpc_ballRotation;
    private double preShotCalc_time;
    private PhotonView peerPhotonView = null;
    public float playerPrevPosTime;
    private Vector3 gkLastCollisionVel = Globals.INCORRECT_VECTOR;
    private Vector3 gkLastCollisionAngVel = new Vector3(24.0f, 24.0f, 24.0f);
    private float gkPercentCollisionTime = 0f;
    private float gkStartSequenceTime = -1f;
    private float gkCollisionPackageArriveTime = 0f;
    private float shotPercent;
    private bool initPreShotRPC;
    private Vector3[,] bufferedBallPos = new Vector3[1100, 5];
    private int bufferedBallPosCurrIdxPush = 0;
    private int bufferedBallPosCurrIdxPop = 0;
    private int bufferedBallPosMax = 1100;
    private int mainUpdatePacketId = 0;
    int gkRotationLoops = 0;
    Vector3 gkRbRotationPos;
    Vector3 gkRbRotationRot;
    string gkStepSideAnimName = "";
    float gkStepAnimPercent;
    string gkSideAnimName;
    float gkSideAnimSpeed;
    float gkSideAnimDelayBefore = 0f;
    float gkSideAnimDelayBeforeStart = -1f;
    private Vector3 gkSideAnimStartPos;
    private Vector3 gkSideAnimStartRot;
    int gkSideOffsetListCurrIdx = 0;
    int gkSideOffsetListMax = 0;
    List<Vector3> stepSideAnimOffsetList = new List<Vector3>();
    private bool isPrepareShotDelay = false;
    private bool isPrepareShotDelay2 = false;

    private bool RPC_gkPacketProcessed = false;
    private bool RPC_gkPredictionActive = false;
    private bool rpc_gkMovementSend = false;
    public static float FIXEDUPDATE_TIME = 0.02f;
    private bool initGkMoves = false;
    private bool initGKPreparation = false;
    private Vector3 prevBallPos;
    private bool isCollisionActive = false;
    private string gkOperations = "EMPTY";
    string gkAnimNameCache;
    float gkAnimSpeedCache;
    private int fixedUpdateCount = 0;
    GameObject tmpRbGameObject;
    Rigidbody tmpRigidbody;
    private Vector3 rpc_prevPosStored;
    private bool lockCollision = true;
    private Rigidbody rb;
    public GameObject playerUp;
    public setTexturesMulti setTextureScript;
    private gamePausedMenu gamePausedScript;
    protected Rigidbody[] ballRb;
    protected GameObject[] ballRbLeftSide;
    protected GameObject[] ballRbRightSide;
    public Vector3[,] prepareShotPos;
    private int prepareShotPosIdx = 0;
    public int prepareShotMaxIdx = 0;
    private List<Collider> wallsCollider;
    private List<Collider> playersCollider;
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
    private Vector2 specialPowersScreenOffset;
    public Powers powersScript;

    public buttonVolleyMulti volleyButton;
    public buttonLobMulti lobButton;
    public buttonCameraMulti cameraButton;
    private GameObject volleyButtonTextGameObject;
    private GameObject lobButtonTextGameObject;
    private GameObject cameraButtonTextGameObject;
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
    public float BALL_NEW_RADIUS = 0.2585f;
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
    //private Vector3 extraShotVec = Vector3.zero;
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
    private bool[] RPC_confirmation;
    public short[] RPC_sequenceNumber;
    //private short[] RPC_expectedSequenceNumber;
    private float[] RPC_lastUpdateTime;
    private LeagueBackgroundMusic leagueBackgroundMusic;

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
    public BallMovementMultiplayer[] ball;
    private bool goalJustScored;
    private bool gkAnimPlayed = false;
    private List<string> AllAnimationsNames;
    private List<string> RunAnimationsNames;
    private List<string> JoystickButtonAnimNames;
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
    public GameObject networkInfoPanel;
    public Text networkPing;
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
    //private float ShotSpeedMax = 1300.0f;
    private float ShotSpeedMax = 900.0f;
    private float ShotSpeedMin = 480.0f;
    private Vector3 ballPosAfterOut = Vector3.zero;
    /*private float ShotCurveSpeedMinTime = 650.0f;
    private float ShotCurveSpeedMaxTime = 1500.0f;*/
    //private float MIN_SHOT_SPEED = 50.0f;
    private float MIN_SHOT_SPEED = 65.0f;
    private float MAX_SHOT_SPEED = 120f;
    //private float MAX_SHOT_SPEED_UNITY_UNITS = 34.5f;
    private float MAX_SHOT_SPEED_UNITY_UNITS = 32.5f;
    private bool isLobButtonPressed = false;
    private bool isLobActive = false;
    //private float animationOffsetTime;
    private GameObject rbRightFoot;
    private GameObject rbRightToeBase;
    private GameObject rbLeftToeBase;
    private GameObject rbHead;
    private float ballUpStartYVelocity = 0.14f;
    private IDictionary<string, float> animationOffsetTime;
    private bool gkNotClickedLocked = false;
    private bool gkMovesActive = false;


    private bool initPreShot;
    private bool initVolleyShot;
    private bool isBallOut = false;
    private Vector3 outShotStart, outShotMid, outShotEnd, outShotBallVelocity;
    public SHOTVARIANT2 shotvariant;
    private int minuteOfMatch = 0;
    private static bool wallCollierTest = false;
    private bool playerColliderCollierTest = false;
    private GameObject playerDownLeftToeBaseCollider;

    private float CURVE_SHOT_MAX_DIST = 4.0f;
    private float SKILLS_MAX_VALUE = 100.0f;
    private float LEVEL_MIN = 1f;
    private float LEVEL_MAX = 5f;
    private float SPEED_RUN_MIN = 2.0f;
    private float SPEED_RUN_MAX = 10.0f;
    private float MAX_PLAYER_SPEED = 12.0f;
    private float MIN_ANIM_GK_PLAYER_SPEED = 3.1f;
    private float MAX_ANIM_GK_PLAYER_SPEED = 3.5f;
    private float GK_SIDE_MOVE_MIN_TIME = 0.3f;
    public Vector3 BALL_MIN_VAL =
        new Vector3(0f, 0.270f, 0f);

    /*this is time the maximum time is needed to GK 
     * with slowest animation execution to save a ball*/
    private float MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE = 0.270f;
    private bool shotRet = false;
    private float cpuGkLastMovement = 0.0f;
    private int counter = 0;
    private string lastGkAnimName = "";
    private string lastGkAnimNameVirtual = "";
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
    public GameObject playerTextureGO;
    public GameObject hairPlayerDownGO;

    private float lastGkDistX;
    bool matchTargetActive1 = false;
    bool matchTargetActive2 = false;
    private Vector3 gkStartPos;
    private Transform gkStartTransform;
    private float gkTimeToCorrectPos;
    private float gkSideAnimPlayOffset;
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
    private float maxTimeToShot = 9f;
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
    private Vector3 goalSizesCpu = new Vector3(4.7f, 3.1f, -14.0f);

    //private Vector3 goalSizesCpu = new Vector3(5.25f, 3.5f, 14.0f);

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

    private MatchStatisticsMulti matchStatistics;
    private Vector3 ballPrevPosition;

    private int winCoinsRewarded;
    private int tieCoinsRewarded;

    private GraphicsCommon graphics = new GraphicsCommon();
    private GeometryCommon geometry = new GeometryCommon();
    private float MAX_RB_VELOCITY = 10.0f;

    private GameObject[] fansFlag;
    private GameObject[] fansFlagSticks;
    private List<RectTransform> joystickButtons = new List<RectTransform>();
    private int FANS_FLAG_MAX = 3;
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
    public float MIN_DIST_REAL_CLICKED = 1.0f;
    public float gkDistRealClicked = 0;
    private float INCORRECT_DIST = float.MinValue;
    private GameObject rotatedRbToBall;
    private GameObject tmpRotatedRbToBall;

    private bool isUpdateBallPosActive = false;
    private Vector3 updateBallPos;
    private string updateBallPosName = "";

    private bool timeToShotExceeded = false;
    private bool rpc_timeToShotExceeded = false;
    public bool rpc_isBallOutActive = false;
    private GameObject dummyTouchRotatedGO;
    private RectTransform cameraRectTrans;
    /*not main camera. it's position of camera to change game view angles*/
    private Vector3 cameraPos;
    private Canvas UICanvas;

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

    private bool isBonusActive = false;
    private float shotRotationDelay = 0f;
    private bool initIntroNewCameraPos = false;

    public float ballShotVelocity = 20f;

    public Vector3 playerPrevPos;
    public float playerVelocity = 0f;
    //private bool gkHelperImageErased = true;
    //private GameObject MarkerBasic;
    public audienceReactions audienceReactionsScript;
    private float curveShotFlyPercent = 0f;

    private Vector3 prepareShotPlayerInitPos = Vector3.zero;
    private Vector3 prepareShotJustBeforeShotPos = Vector3.zero;
    private Quaternion prepareShotJustBeforeShotRot;
    private Quaternion prepareShotPlayerInitRot;
    //private Vector3 prepareShotPlayerInitVel;
    private bool updateTextureDone = false;
    private bool updateTextureConfirm = false;
    private bool isGoalJustScored = false;

    [PunRPC]
    void RPC_goalUpdate(int goalNum, 
                        int scoreNum)
    {

        peerPlayer.RPC_setLastTimeUpdate();

        photonView.RPC("RPC_PACKET_ACK",
                        RpcTarget.Others,
                        (byte) RPC_ACK.GOAL_CONFIRM);

        ////////print("##RECEIVED RPC_goalUpdate " + goalNum + " scoreNum " + scoreNum);
        //if (!isWaitGoalActive && 
        //    (Globals.score1 < goalNum))
        //{
        if (Globals.score1 < goalNum)
        {
            isWaitGoalActive = true;
            isWaitGoalActiveNewPos = true;
            goalNewScore = goalNum;
        }

            //StartCoroutine(wait_isGoal(goalNum,
            //                           scoreNum));is
        //}
    }

    public bool isWaitGoalActive = false;
    public bool isWaitGoalActiveNewPos = false;
    public int goalNewScore = 0;
    /*
    IEnumerator wait_isGoal(int goalNum, int scoreNum)
    {
        isWaitGoalActive = true;
        for (int i = 0; i <= 100; i++)
        {

            if ((Mathf.Abs(ballRb[activeBall].transform.position.z) >= 14f) ||
                (peerPlayer.getShotPercent() >= 1f))
            {
               
                    Globals.score1 = goalNum;
                    peerPlayer.setIsGoalJustScored(true);
                    //TOCHECK
                    peerPlayer.setScoresText();
                    ball[1].playFlares(scoreNum);
                    isWaitGoalActive = false;
                    yield break;              
            }

            yield return 0;
        }
        isWaitGoalActive = false;
    }
    */
    /*
    [PunRPC]
    void RPC_prepareShot(string shotType,
                         Vector3 playerRbPos,
                         Quaternion playerRbRot,
                         Vector3 playerRbVel,
                         PhotonMessageInfo info)
    {
        print("rpc_rbPredictedPos  RBBALLPOS RPC_PREPARESHOT arrived");

        rpc_shotActive = true;
        passedShotFlyTime = 0f;

        rpc_playerOnBallActive = true;

        setWallColliders(false);
        peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), true);

        preShotActive = true;
        this.shotType = shotType;
        isUpdateBallPosActive = false;
        this.rpc_prepareShotDelay = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));

        setShotAnimSpeed(animator, shotType, rpc_prepareShotDelay);      
        prepareShotPlayerInitPos = playerRbPos;
        prepareShotPlayerInitRot = playerRbRot;      
    }*/

    [PunRPC]
    void RPC_playerOnBAll()
    {
        peerPlayer.RPC_setLastTimeUpdate();

        rpc_playerOnBallActive = true;
        //print("DBG#135 RBBALLPOS RPC_PLAYERONBALL");
    }

    [PunRPC]
    void RPC_ballOutNewPos(Vector3 ballAfterOut,                     
                           PhotonMessageInfo info)
    {
        peerPlayer.RPC_setLastTimeUpdate();

        peerPlayer.ballPosAfterOut = ballAfterOut;
        photonView.RPC("RPC_PACKET_ACK",
                        RpcTarget.Others,
                        (byte) RPC_ACK.BALL_IS_OUT_NEW_POS);
    }


    float lastUpdateBallOut = 0;
    [PunRPC]
    void RPC_ballOut(Vector3 ballAfterOut,
                     bool timeToShotExceeded,
                     bool isGoalJustScored,
                     int score,
                     PhotonMessageInfo info)
    {
        peerPlayer.RPC_setLastTimeUpdate();

        //400 ms + lag 
        photonView.RPC("RPC_PACKET_ACK",
                        RpcTarget.Others,
                        (byte) RPC_ACK.BALL_IS_OUT);

        if ((Time.time - lastUpdateBallOut) < 3f)
            return;

        if (isGoalJustScored)
        {
            if (Globals.score1 < score)
            {
                isWaitGoalActive = true;
                goalNewScore = score;
                photonView.RPC("RPC_PACKET_ACK",
                                RpcTarget.Others,
                                (byte) RPC_ACK.GOAL_CONFIRM);
            }
        }

        peerPlayer.rpc_isBallOutActive = true;
        peerPlayer.rpcSetTimeToShotExceeded(timeToShotExceeded);

        lastUpdateBallOut = Time.time;
        //peerPlayer.ballPosAfterOut = ballAfterOut;
        float RPC_afterGoalLag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp)) + 0.4f;
        //print("#DBGBALLCHANGEPOS get RPC_afterGoalLag #### " + RPC_afterGoalLag + " ballAfterOut " + ballAfterOut);
        StartCoroutine(wait_isBallOut(RPC_afterGoalLag, true, timeToShotExceeded, isGoalJustScored));
    }


    IEnumerator wait_isBallOut(float delay, 
                               bool isBallOut,
                               bool timeToShotExceeded, 
                               bool isGoalJustScored)
    {

        yield return new WaitForSeconds(delay);

        //for (int i = 0; i < 30; i++)
        //{
        //    if (!isBallOutOfPitch() &&
        //        !ball[activeBall].getBallGoalCollisionStatus())
        //    {
        //    }
        //}

        peerPlayer.rpc_isBallOutActive = false;
        peerPlayer.setTimeToShotExceeded(timeToShotExceeded);
        peerPlayer.setIsGoalJustScored(isGoalJustScored);
        peerPlayer.setIsBallOut(true);

    }

    public void setTimeToShotExceeded(bool val) 
    {
        timeToShotExceeded = val;
    }

    public void rpcSetTimeToShotExceeded(bool val)
    {
        rpc_timeToShotExceeded = val;
    }

    public bool getRpc_TimeToShotExceeded()
    {
        return rpc_timeToShotExceeded;
    }

    public void setIsBallOut(bool val)
    {
        isBallOut = val;
    }

    public bool getIsBallOut()
    {
        return isBallOut;
    }

    //[PunRPC]
    //void RPC_justBeforeShot(Vector3 ballPos,
    //                        PhotonMessageInfo info)
    // {
    //     RPC_locks.ballPosBeforeShot = ballPos;
    //     this.rpc_prepareShotDelay = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
    //    Time.timeScale = 1f;
    //    shotActive = true;
    //}
    [PunRPC]
    void RPC_PACKET_ACK(byte idx)
    {
        peerPlayer.RPC_setLastTimeUpdate();

        RPC_confirmation[idx] = true;
        //Debug.Log("DBG342344COL get ack confirm idx " + idx + " Time " + Time.time);
    }


    [PunRPC]
    void RPC_GK_COLLISION_PREDICT(Vector3 collision,
                                  float collisionPercentTime,
                                  Vector3 ballPosWhenCollision,
                                  float gkSequenceStart,
                                  float gkSideAnimPlayOffset,
                                  string gkOp,
                                  Vector3 playerRbPos,
                                  //Quaternion playerRbRot,
                                  Vector3 playerRbRot,
                                  bool isCollisionWithPlayer,
                                  //Quaternion rotatedRbRot)
                                  Vector3 rotatedRbPos,
                                  Vector3 rotRbToBall,
                                  short packageSeqNumber,
                                  PhotonMessageInfo info)
    //GameObject rotRbToBall)

    {
        peerPlayer.RPC_setLastTimeUpdate();

        if (RPC_sequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM] == packageSeqNumber)
            return;

        RPC_gkPredictionActive = true;
        RPC_sequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM] = packageSeqNumber;
        //if ((Time.time - RPC_lastUpdateTime[(int) RPC_ACK.GK_PREDICT_CONFIRM]) <= 3.0f)
        //{
        //    photonView.RPC("RPC_PACKET_ACK",
        //           RpcTarget.Others,
        //           (byte)RPC_ACK.GK_PREDICT_CONFIRM);
        //    return;
        //
        //}

        photonView.RPC("RPC_PACKET_ACK",
                     RpcTarget.Others,
                     (byte)RPC_ACK.GK_PREDICT_CONFIRM);

        RPC_lastUpdateTime[(int) RPC_ACK.GK_PREDICT_CONFIRM] = Time.time;

        gkOperations = gkOp;
        gkLastCollisionVel = collision;
        gkPercentCollisionTime = collisionPercentTime;
        gkStartSequenceTime = gkSequenceStart;
        this.ballPosWhenCollision = ballPosWhenCollision;
        gkCollisionPackageArriveTime = Time.time;
        //rotatedRbToBall = rotRbToBall;
        this.isCollisionWithPlayer = isCollisionWithPlayer;
        this.gkSideAnimPlayOffset = gkSideAnimPlayOffset;
        rb.transform.position = playerRbPos;
        //print("dbgrbposition PACKET ARRIVE rb.transform.position " + rb.transform.position);

        rotatedRbToBall.transform.position = rotatedRbPos;

        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));

        ///print("#DBGEULERANGLES rbRot " + playerRbRot + " rbtmpRot " + rotRbToBall);
        rb.transform.eulerAngles = playerRbRot;
        //+ (180f * Vector3.up);
        rotatedRbToBall.transform.eulerAngles = rotRbToBall;
        //+ (180f * Vector3.up);
        ///print("#DBGEULERANGLES rbRot AFTER ## " + rb.transform.eulerAngles +
        ////    " rbtmpRot " + rotatedRbToBall.transform.eulerAngles);

        //rb.transform.rotation = playerRbRot;
        //rotatedRbToBall.transform.eulerAngles = rotRbToBall;
        //+ (180f * Vector3.up);

        /*print("DBGSHOT12 DBGPREDICTCOLLISON inside RPC gkLastCollision ARRIVED ### " +
            gkLastCollisionVel
            + " gkPercentCollisionTime " + gkPercentCollisionTime
            + " currentShotTime " + peerPlayer.getCurveShotFlyPercent()
            + " isShotActive " + peerPlayer.getShotActive()
            +"  ballPosition " + ballRb[activeBall].transform.position
            + " lag " + lag
            + " gkCollisionPackageArriveTime " + gkCollisionPackageArriveTime
            + " gkOperations " + gkOperations
            + " gkStartSequenceTime " + gkStartSequenceTime
            + " ballRb " + ballRb[activeBall].transform.position
            + " rb.transform.position " + rb.transform.position);*/

        RPC_gkPacketProcessed = true;
        /*print("DBGSTEPSIDE_123 gkOperations ARRIVED ##########" + gkOperations +
             " gkStartSequence " + gkStartSequenceTime
             + " gkPercentCollisionTime " + gkPercentCollisionTime);*/


        /*print("#DBGCOLLISIONCALC1024D collisionOutput ARRIVED " + gkLastCollisionVel
           + " gkPercentCollisionTime " + gkPercentCollisionTime + " gkStartSeq " + gkStartSequenceTime);

        print("DBGCOLLISIONCALC1024D PACKAGE_ARRIVED Time " + Time.time + " peerPlayer.getShotActive() " +
            peerPlayer.getShotActive() + " gkOperations " + gkOperations
            + " gkPercentCollisionTime " + gkPercentCollisionTime
            + " gkStartSequence " + gkStartSequenceTime
            + " ballRb.transform " + ballRb[activeBall].transform.position
            + " getShot " + peerPlayer.getShotPercent()
            + " LAG " + lag);

        if (peerPlayer.getShotActive())
        {
            print("DBGSTEPSIDE_123 ARRIVED SHOT PERCENT ##########" + peerPlayer.getShotPercent());
        }*/
    }

    [PunRPC]
    void RPC_preShotCalc(float shotSpeed,
                         bool isLobActive,
                         Vector3 outShotBallVelocity,
                         Vector3 outShotStart,
                         Vector3 outShotMid,
                         Vector3 outShotEnd,
                         Vector2 touchEndPos,
                         SHOTVARIANT2 shotvariant,
                         float timeOfBallFlyShot,
                         string ballPositions,
                         Vector3 prepareShotJustBeforeShotPos,
                         Quaternion prepareShotJustBeforeShotRot,
                         Vector3 playerRbPos,
                         Quaternion playerRbRot,
                         string shotType,
                         float shotRotationDelay,
                         PhotonMessageInfo info)
    {
        //print("SHOTACTIVE getPreShotCalc " + ballPositions);
        peerPlayer.RPC_setLastTimeUpdate();

        /*print("DBGSHOT12## RBBALLPOS RPC_PREPARESHOT_CALC ARRIVED "
            + " outShotStart " + outShotStart
            + " outShotMid " + outShotMid
            + " outShotEnd " + outShotEnd
            + " shotvariant " + shotvariant
            + " timeOfBallFly " + timeOfBallFlyShot
            + " LAG " + Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp))
            + " ARRIVEDTIME " + PhotonNetwork.Time
            + " shotRotationDelay " + shotRotationDelay);*/

        rpc_shotActive = true;
        passedShotFlyTime = 0f;

        //TOREMOVE
        rpc_playerOnBallActive = true;

        setWallColliders(false);
        peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), true);

        preShotActive = true;
        //print("DBGSHOT preShotActive true");
        this.shotType = shotType;

        //        print("#DBGK1024_SIMULATE_DBG2435_CREATE RPCEXECUTED RPC_prepareShot RECEIVED");

        //gkTouchDone = false;
        //touchCount = 0;
        /*TOCHECK*/
        isUpdateBallPosActive = false;
        this.rpc_prepareShotDelay = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));

        setShotAnimSpeed(animator, shotType, rpc_prepareShotDelay);

        //print("#PREPARESHOTSTARTTIME get RPC_PREPARE_SHOT DELAY "
        //    + rpc_prepareShotDelay + " Time " + PhotonNetwork.Time);
        //ballRb.transform.position = ballPos;
        prepareShotPlayerInitPos = playerRbPos;
        prepareShotPlayerInitRot = playerRbRot;
        //prepareShotPlayerInitVel = playerRbVel;

        this.shotRotationDelay = shotRotationDelay;
        this.shotSpeed = shotSpeed;
        this.isLobActive = isLobActive;
        this.outShotStart = outShotStart;
        this.outShotMid = outShotMid;
        this.outShotEnd = outShotEnd;
        this.shotvariant = shotvariant;
        this.outShotBallVelocity = outShotBallVelocity;
        this.timeofBallFly = timeOfBallFlyShot;
        this.preShotCalc_time = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
        this.endPos = touchEndPos;
        this.prepareShotJustBeforeShotPos = prepareShotJustBeforeShotPos;
        this.prepareShotJustBeforeShotRot = prepareShotJustBeforeShotRot;

        string[] ballPos = ballPositions.Split('|');
        //print("ballPos converted ballPositions " + ballPositions);
        CultureInfo culture = 
            CultureInfo.GetCultureInfo("en-US");

        for (int i = 0; i < ballPos.Length; i++)
        {
            string[] ballVec = ballPos[i].Split(':');
            //print("ballPos " + ballPos[i] + " ballVec[0] " +
            //    ballVec[0] + " ballVec[1] + " + ballVec[1] + " ballVec[2] " + ballVec[2]
            //    + " ballVec[3] " + ballVec[3] + " index " + i);


            /*prepareShotPos[i, 0].x = float.Parse(ballVec[0], CultureInfo.InvariantCulture);
            prepareShotPos[i, 0].y = float.Parse(ballVec[1], CultureInfo.InvariantCulture);
            prepareShotPos[i, 0].z = float.Parse(ballVec[2], CultureInfo.InvariantCulture);
            prepareShotPos[i, 1].x = float.Parse(ballVec[3], CultureInfo.InvariantCulture);*/
            prepareShotPos[i, 0].x = float.Parse(ballVec[0], CultureInfo.InvariantCulture);
            prepareShotPos[i, 0].y = float.Parse(ballVec[1], CultureInfo.InvariantCulture);
            prepareShotPos[i, 0].z = float.Parse(ballVec[2], CultureInfo.InvariantCulture);
            prepareShotPos[i, 1].x = float.Parse(ballVec[3], CultureInfo.InvariantCulture);

            //print("ballPos converted " + prepareShotPos[i, 0] + " idx " + prepareShotPos[i, 1].x);
        }

        prepareShotPosIdx = 0;
        prepareShotMaxIdx = ballPos.Length;
         
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
        ballInitPos = outShotStart;
        /*print("SHOT3432X RPCSHOTDBG RPC preShotCalc shotSpeed " + this.shotSpeed
            + " isLobActive " + this.isLobActive
            + " outShotStart " + this.outShotStart
            + " outShotMid " + this.outShotMid
            + " outShotEnd " + this.outShotEnd
            + " shotvariant " + this.shotvariant
            + " this.outShotBallVelocity " + this.outShotBallVelocity
            + " this.timeofBallFly " + this.timeofBallFly
            + " LAGTIME " + lag);

        print("DBG WAITING FOR A SHOT LAGTIME " + lag);*/
        rpc_preShotCalc = true;
        //shotActive = true;

        RPC_locks.ballPosBeforeShot = outShotStart;
        //this.rpc_prepareShotDelay = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
        Time.timeScale = 1f;
        //shotActive = true;
    }

    private int lastPacketId = 0;
    private float lastTimeUpdate = 0f;

    public void RPC_setLastTimeUpdate()
    {
        lastTimeUpdate = Time.time;
    }

    public float getLastTimeUpdate()
    {
        return lastTimeUpdate;
    }

    [PunRPC]
    void RPC_mainUpdate(Vector3 rbPos,
                        Vector3 rbVelocity,
                        Vector3 rbAngularVelocity,
                        Quaternion rbRotation,
                        float runningSpeedRpc,
                        Vector3 playerDirection,
                        Vector3 ballPos,
                        Vector3 ballVelocity,
                        Vector3 ballAngularVelocity,
                        Quaternion ballRotation,
                        bool isShotActive,
                        bool collisionActive,
                        int packetId,
                        short packageSeqNumber,
                        string mainUpdateType,
                        int joystickExtraButtons,
                        bool isBallOut,
                        bool isOnBall,
                        PhotonMessageInfo info)
    {
        if ((Time.time - peerPlayer.getLastTimeUpdate()) > 0.250f)
        {
            peerPlayer.networkInfoPanel.SetActive(true);
            peerPlayer.networkPing.text = (rpc_mainLag * 1000f).ToString("F0") + " ms";
        }
        else
            peerPlayer.networkInfoPanel.SetActive(false);

        peerPlayer.RPC_setLastTimeUpdate();

        rpc_mainLag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));

        ///print("DBGSHOT RPC_mAIN UPDATE rpc_rbPos " + rpc_mainLag + " rpc_mainLag ms " + (rpc_mainLag * 100f));
  
        ///print("rpc_mainLag main log " + rpc_mainLag);
        rpc_rbPos = rbPos;
        rpc_rbVelocity = rbVelocity;
        rpc_rbAngularVelocity = rbAngularVelocity;
        rpc_runningSpeed = runningSpeed;
        rpc_rbRotation = rbRotation;
        rpc_playerDirection = playerDirection;
        rpc_ballRotation = ballRotation;
        rpc_isShotActive = isShotActive;
        rpc_joystickExtraButtonsIdx = joystickExtraButtons;
        rpc_isBallOut = isBallOut;
        this.mainUpdateType = mainUpdateType;

        if ((packetId <= lastPacketId))
            return;

        lastPacketId = packetId;

        if (!isBallOnYourHalf(getPlayerPosition(), ballRb[activeBall].transform.position))
            clearBufferBallPos();

        if (!isBallOnYourHalf(getPlayerPosition(), ballPos))
        {
            return;
        }

        rpcMain_updateTime = Time.time;

        if (RPC_sequenceNumber[(int) RPC_ACK.BALL_POS_UPDATE] == packageSeqNumber) {
            //if (!peerPlayer.getIsBallOut())
            //{
            //print("#DBGBALLPOSITION packageSeqNumber received seq number " + packageSeqNumber + 
            //    " ballPos " + ballPos);
            rpc_ballPos = ballPos;
            rpc_ballVelocity = ballVelocity;
            rpc_ballAngularVelocity = ballAngularVelocity;
            rpc_isBallOut = isBallOut;
        }

        //print("DBGBUFFER rpc_ballPos " + rpc_ballPos
        //     + " rpc_ballVelocity " + rpc_ballVelocity
        //     + " rpc_ballAngularVelocity " + bufferedBallPosCurrIdxPush
        //     + " mainUpdateType " + mainUpdateType);

        if (mainUpdateType.Equals("B"))
            //isBallOnYourHalf(getPlayerPosition(), rpc_ballPos))
        {
            if (bufferedBallPosCurrIdxPush == bufferedBallPos.GetLength(0))
                bufferedBallPosCurrIdxPush = 0;

            bufferedBallPos[bufferedBallPosCurrIdxPush, 0] = rpc_ballPos;
            bufferedBallPos[bufferedBallPosCurrIdxPush, 1] = rpc_ballVelocity;
            bufferedBallPos[bufferedBallPosCurrIdxPush, 2] = rpc_ballAngularVelocity;
            bufferedBallPos[bufferedBallPosCurrIdxPush, 3].x = (float) packetId;

            if (rpc_isBallOut)
            {
                bufferedBallPos[bufferedBallPosCurrIdxPush, 4].x = 1;
            } else
            {
                bufferedBallPos[bufferedBallPosCurrIdxPush, 4].x = 0;
            }

            bufferedBallPosCurrIdxPush++;
        } else
        {
            if (bufferedBallPosCurrIdxPush != 0)
            {
                bufferedBallPosMax = bufferedBallPosCurrIdxPush;
                ///print("DBGBUFFER setBufferedBallMax " + bufferedBallPosMax);
            }
        }

        if (!isBallOnYourHalf(getPlayerPosition(), rpc_ballPos))
        {
            /*print("DBGBUFFER clear variable bufferedBallPosCurrIdxPush " + bufferedBallPosCurrIdxPush
                + " bufferedBallPosCurrIdxPop " + bufferedBallPosCurrIdxPop
                + " bufferedBallPosMax " + bufferedBallPosMax);*/
            clearBufferBallPos();
        }

            /*else
            {
                bufferedBallPosCurrIdxPush = 0;
                bufferedBallPosCurrIdxPop = 0;
            }*/



        //isCollisionActive = collisionActive;
        //peerPlayer.setCollisionOccur(isCollisionActive);
        /*if (peerPlayer.getShotActive() &&
            collisionActive)
        {
            peerPlayer.setShotActive(false);
            rpc_playerOnBallActive = false;
            rpc_playerOnBallActive = false;
            rpc_shotActive = false;
            peerPlayer.clearAfterBallCollision();
            rpc_isShotActive = false;
        }*/

        /*print("#DBGPLA LAG " + rpc_mainLag + " shotActive " + shotActive);
        print("#DBGPLA rbPos " + rbPos + " rbVelocity " + rbVelocity
            + " rbAngularVelocity " + rbAngularVelocity
            + " playerDirection " + playerDirection
            + " ballPos " + ballPos
            + " ballVelocity " + ballVelocity
            + " ballAngularVelocity " + ballAngularVelocity
            + " isShotActive " + isShotActive
            + " !checkIfAnyAnimationPlaying " + !checkIfAnyAnimationPlaying(animator, 1.0f)
            + " lag " + rpc_mainLag
            + " photonView.ISMINE " + photonView.IsMine);*/

        //if (!checkIfAnyAnimationPlaying(animator, 1.0f))
        //{
        //print("#DBGPLA RBPOS " + (rbPos + (rbVelocity * rpc_mainLag)));
        //rb.transform.position = rbPos;
        //rb.velocity = rbVelocity * runningSpeed;
        //rb.angularVelocity = ballAngularVelocity;
        //RbLookAt(playerDirection, rb, isMaster);
        //rb.transform.rotation = rbRotation;
        //}

        //if (!isShotActive &&
        //    !isBallOnYourHalf(isMaster))
        //{
        //print("DBGPLA BALLOIPOS " + (ballPos + (ballVelocity * lag)));
        ///ballRb.transform.position = ballPos;
        //ballRb.velocity = ballVelocity;
        //ballRb.angularVelocity = ballAngularVelocity;
        //ballRb.transform.rotation = ballRotation;
        //}

        //ballSyncQueue.Enqueue(
        //       new RPCballSyncPacket(pos, velocity, angularVelocity, rotation, true, nameOfcollider));
    }

    private void clearBufferBallPos()
    {
        bufferedBallPosCurrIdxPush = 0;
        bufferedBallPosCurrIdxPop = 0;
        bufferedBallPosMax = bufferedBallPos.GetLength(0);
        mainUpdateTypeActive = false;
        //print("DBGBUFFER clear buffer positions #####");
    }

    void setWallColliders(bool enable)
    {
        enable = true;
        //TOCHECK
        //return;

        if (playerControllerMultiplayer.wallCollierTest == !enable)
        {
            return;
        }

        //if (GameObject.Find("goalDownPostColliderLeft").GetComponent<Collider>().isTrigger == enable)
        //    return;

        for (int i = 0; i < wallsCollider.Count; i++)
        {
            wallsCollider[i].isTrigger = !enable;
            //print("#DBGWALLSCOLLIDER " + wallsCollider[i] + " set " + !enable);
        }

        /*foreach (var wallsColliders in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (wallsColliders.name.Contains("wall") ||
                wallsColliders.name.Contains("goalDownPost") ||
                wallsColliders.name.Contains("goalDownCross") ||
                wallsColliders.name.Contains("goalUpPost") ||
                wallsColliders.name.Contains("goalUpcollisionOpCross"))
            {
                print("DBG342344COL setCollider for " + wallsColliders.name + " val " + !enable 
                    + " ISMINE" +  photonView.IsMine);
                wallsColliders.GetComponent<Collider>().isTrigger = !enable;
                         
            }
        }*/
        playerControllerMultiplayer.wallCollierTest = !enable;
    }

    void setPlayersColliders(GameObject parentGameObj, bool value)
    {
        ///print("DBG342344COL setPlayerColliders val " + value);
        //TOREMOVE
        //if (value == false)
        //    return;

        if (playerColliderCollierTest == value)
            return;

        GameObject gameObject = null;
        //TOCHECK

        for (int i = 0; i < playersCollider.Count; i++)
        {
            playersCollider[i].isTrigger = value;
            //print("#DBGWALLSCOLLIDER " + wallsCollider[i] + " set " + !enable);
        }

        playerColliderCollierTest = value;

        /*gameObject = getChildWithName(parentGameObj, "playerDownLeftToeBaseCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftFootCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftUpLegCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightToeBaseCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightFootCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightUpLegCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "Spine");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftHandCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftHandCollider2");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftForeArmCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownLeftShoulderCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownHeadCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightHandCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightHandCollider2");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightForeArmCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;

        gameObject = getChildWithName(parentGameObj, "playerDownRightShoulderCollider");
        gameObject.GetComponent<Collider>().isTrigger = value;*/
        //}


        /* if (photonView.IsMine)
         {
             foreach (var wallsColliders in FindObjectsOfType(typeof(GameObject)) as GameObject[])
             {
                 if ((isMaster && (wallsColliders.name.Contains("wallUp") ||
                                   wallsColliders.name.Contains("wallLeft1") ||
                                   wallsColliders.name.Contains("wallLeft2") ||
                                   wallsColliders.name.Contains("wallRight1") ||
                                   wallsColliders.name.Contains("wallRight2") ||
                                   wallsColliders.name.Contains("goalUp")))
                                   ||
                    (!isMaster && (wallsColliders.name.Contains("wallDown") ||
                                   wallsColliders.name.Contains("wallLeft3") ||
                                   wallsColliders.name.Contains("wallLeft4") ||
                                   wallsColliders.name.Contains("wallRight3") ||
                                   wallsColliders.name.Contains("wallRight4") ||
                                   wallsColliders.name.Contains("goalDown"))))
                 {
                     if (wallsColliders.GetComponent<Collider>() != null)
                         wallsColliders.GetComponent<Collider>().isTrigger = true;
                 }
             }
         }*/
    }

    public bool stepSidePredicted(Animator animator,
                                 Rigidbody rb,
                                 string animName,
                                 ref Vector3 stepSideAnimOffset)
    {

        //if (!isPlaying(animator, animName, 1.0f))
        if (!checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset"))
        {
            animator.Play(animName, 0, 0.0f);
            animator.Update(0f);
            //print("#DBGSTEPSIDE_123433 ##### PLAY gkStepSideAnimName " + animName + " Time " + Time.time);
            //print("STEP SIDE " + animName);
            ///TODO
            //print("#DBGSTEPSIDE_123 stepOffset " + stepSideAnimOffset + " idx " + (gkSideOffsetListCurrIdx)
            //    + " gkSideOffsetListMax " + gkSideOffsetListMax);
            stepSideAnimOffset = stepSideAnimOffsetList[gkSideOffsetListCurrIdx++];
            ///print("DBG342344COL play stepSide " + gkSideOffsetListCurrIdx);
            /*photonView.RPC("RPC_GKoffsetanimPlay",
                            RpcTarget.Others,
                            animName,
                            gkDistRealClicked,
                            stepSideAnimOffset,
                            animator.speed);*/


            //rb.position = TransformPointUnscaled(rb.transform, locPos);
            return true;
        }

        return false;
    }

    public float getShotPercent()
    {
        return shotPercent;
    }

    private bool RblookAtDirectionGK(Rigidbody rb,
                                    GameObject rotatedRbToBall,
                                    float rotationSpeed)

    {
        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation,
                             rotatedRbToBall.transform.rotation,
                             Time.deltaTime * rotationSpeed);

        return true;
    }


    private bool RblookAtDirectionGK(Rigidbody rb,
                                     GameObject rotatedRbToBall,
                                    //Vector3 lookPoint,
                                     float stopAngle,
                                     float rotationAngle)

    {
        float angle = Quaternion.Angle(rb.transform.rotation, rotatedRbToBall.transform.rotation);
        //print("DBGK1024_SIMULATE_DBG2435 ROATAION ANGLE " + angle + " stopAngle " + stopAngle);
        if (angle < stopAngle)
            return false;

        float angleSpeed = rotationAngle / angle;

        rb.transform.rotation =
            Quaternion.Slerp(rb.transform.rotation,
                             rotatedRbToBall.transform.rotation,
                             Time.deltaTime * angleSpeed);

        return true;
    }

    int predictGkCollisionStartI = 0;
    int predictGkCollisionStartJ = 0;
    bool initialization = false;
    bool isCollisionWithPlayer = false;
    Vector3 ballPosWhenCollision;
    void predictGkCollision(Vector3 outShotStart,
                            Vector3 outShotMid,
                            Vector3 outShotEnd,
                            //Rigidbody ballRb,
                            Rigidbody playerRb,
                            GameObject rotatedRbToBallTmp,
                            string gkOperations,
                            string animName,
                            float animOffset,
                            float gkSideAnimPlayOffset,
                            float timeOfBallFly,
                            ref float curvePercentHit,
                            float lastGkDist,
                            float gkStartSeq,
                            ref Vector3 localGkStartPos)
    {
        Vector3 collisionOutput= INCORRECT_VECTOR;

        //bool initialization = false;
        int max_test = 3;
        int max_type_test = 1;
       
        //print("#DBGPREDICTCOLLISON PREDICTION STARTED outShotStart " + outShotStart + " " +
        //    " outShotMid " + outShotMid + " outShotEnd " + outShotEnd);

        //print("DBG342344COL predict collision set walls true isMine " + photonView.IsMine);
        setWallColliders(true);

        int iter = 0;
        if (predictGkCollisionStartI == max_type_test)

        {
            predictGkCollisionActive = false;
            predictGkCollisionStartI = 0;
            predictGkCollisionStartJ = 0;
            PredictCollision.setOnAnimatorAcitve(false);
            PredictCollision.setBallVelocitySet(false);
            return;
        }

        //for (int i = predictGkCollisionStartI; i < 2; i++)g
        //{
        if (predictGkCollisionStartJ == 0)
        {
            initialization = false;
        }

        /*if ((predictGkCollisionStartI == 1) &&
            (predictGkCollisionStartJ == 0))
        {
            collisionOutput = INCORRECT_VECTOR;
            curvePercentHit = 1f;          
        }*/

        //for (int j = predictGkCollisionStartJ; j < max_test; j++)
        //{

        /*print("#DBG342344COL_NEWDBG34 BEFORE collisionOutput  wallsIsTrigger " + collisionOutput + " j " + predictGkCollisionStartJ + " i " + predictGkCollisionStartI
          + " curvePercentHit " + curvePercentHit + " ballRB " + ballRb[activeBall].transform.position
            ///+ " wallsCollidersTrigger " + GameObject.Find("wallRight3").GetComponent<Collider>().isTrigger
            + " isMine " + photonView.IsMine
          + " isFixedUpdate " + isFixedUpdate);*/
          //+ " wallsIsTrigger " + GameObject.Find("wallDownRight2").GetComponent<Collider>().isTrigger);
        collisionOutput = PredictCollision.predictionGkCollisionOutput(outShotStart,
                                                                       outShotMid,
                                                                       outShotEnd,
                                                                       playerRb,
                                                                       rotatedRbToBallTmp,
                                                                       ref initialization,
                                                                       animName,
                                                                       animOffset,
                                                                       gkSideAnimPlayOffset,
                                                                       timeOfBallFly,
                                                                       ref curvePercentHit,
                                                                       lastGkDist,
                                                                       ref localGkStartPos,
                                                                       peerPlayer.prepareShotPos,
                                                                       peerPlayer.prepareShotMaxIdx,
                                                                       predictGkCollisionStartJ,
                                                                       ref isCollisionWithPlayer,
                                                                       ref ballPosWhenCollision);


        //print("DBGWALL collisionOutput " + collisionOutput);
        /*print("DEBUGGK1045 collisionOutput ######## " + collisionOutput + " j " + predictGkCollisionStartJ + " i " + predictGkCollisionStartI
            + " curvePercentHit " + curvePercentHit + " ballRB " + ballRb[activeBall].transform.position
            ///+ " wallsCollidersTrigger " + GameObject.Find("wallRight3").GetComponent<Collider>().isTrigger
            + " isMine " + photonView.IsMine
            + " isFixedUpdate " + isFixedUpdate
            + "  rb.transform.position " + playerRb.transform.position
            + " wallsIsTrigger " + GameObject.Find("wallDownLeft2").GetComponent<Collider>().isTrigger);*/

        predictGkCollisionStartJ++;
        if (predictGkCollisionStartJ == max_test)
        {
            predictGkCollisionStartJ = 0;
            predictGkCollisionStartI++;
        }

        //it was the last test
        if (!((predictGkCollisionStartI == max_type_test) &&
              (predictGkCollisionStartJ == 0)))
        {
            if (collisionOutput == INCORRECT_VECTOR)
            {
                curvePercentHit = Mathf.InverseLerp(0.0f,
                        timeOfBallFly,
                        (timeOfBallFly * curvePercentHit) + (Time.deltaTime * 1000f));
                //print("DBG342344COL_NEWDBG34 return ");
                return;
            }
        }

        ///print("DBG342344COL_NEWDBG34 too far!!");
        //iter = i;
        ///break;
        //}


        //if (collisionOutput != INCORRECT_VECTOR)
        //break;on
        //}
        if (collisionOutput == Globals.INCORRECT_VECTOR)
        {
            gkLastCollisionVel = Globals.INCORRECT_VECTOR;
            gkPercentCollisionTime = -1f;
            curvePercentHit = -1f;
            gkCollisionPackageArriveTime = Time.time;
        }

        //in the case the goal
        else if (collisionOutput == Globals.INCORRECT_VECTOR_2)
        {
            gkLastCollisionVel = Globals.INCORRECT_VECTOR_2;
            gkPercentCollisionTime = 1f;
            curvePercentHit = 1f;
            gkCollisionPackageArriveTime = Time.time;
        }
        else
        {
            gkCollisionPackageArriveTime = Time.time;
            gkLastCollisionVel = collisionOutput;
            gkPercentCollisionTime = curvePercentHit;
        }

        //print("#DBGPREDICTCOLLISON FINAL PREDICTION " + collisionOutput +
        //     " time " + Time.time + " curvePercentHit " + curvePercentHit);
        setWallColliders(false);
        ///print("DBG342344COL predict collision set walls false");

        //print("#DBGCOLLISIONCALC1024DB collisionOutput " + collisionOutput
        //    + " gkPercentCollisionTime " + curvePercentHit + " gkStartSeq " + gkStartSeq + " I " + iter);

        predictGkCollisionActive = false;
        predictGkCollisionStartI = 0;
        predictGkCollisionStartJ = 0;
        PredictCollision.setOnAnimatorAcitve(false);
        PredictCollision.setBallVelocitySet(false);

        //if (gkStartSeq != -1)
        RPC_sequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM]++;

        RPC_sendGK_Predict(
                           collisionOutput,
                           curvePercentHit,
                           ballPosWhenCollision,
                           gkStartSeq,
                           gkSideAnimPlayOffset,
                           gkOperations,
                           isCollisionWithPlayer,
                           playerRb.transform.position,
                           playerRb.transform.eulerAngles,
                           rotatedRbToBallTmp.transform.position,
                           rotatedRbToBallTmp.transform.eulerAngles,
                           RPC_sequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM]);

        StartCoroutine(sendAndACK(0,
                                  collisionOutput,
                                  curvePercentHit,
                                  ballPosWhenCollision,
                                  gkStartSeq,
                                  gkSideAnimPlayOffset,
                                  gkOperations,
                                  playerRb.transform.position,
                                  playerRb.transform.eulerAngles,
                                  rotatedRbToBallTmp.transform.position,
                                  rotatedRbToBallTmp.transform.eulerAngles,
                                  RPC_sequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM]));
    }

    public IEnumerator sendAndACK(
                                  int idx, 
                                  Vector3 collisionOutput,
                                  float collisionPercentTime,
                                  Vector3 ballPosWhenCollision,
                                  float gkSequenceStart,
                                  float gkSideAnimPlayOffset,
                                  string gkOp,
                                  Vector3 playerRbPos,
                                  Vector3 playerRbRot,
                                  Vector3 rotatedRbPos,
                                  Vector3 rotRbToBall,
                                  short packageSeqNumber)
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                RPC_sendGK_Predict(
                          collisionOutput,
                          collisionPercentTime,
                          ballPosWhenCollision,
                          gkSequenceStart,
                          gkSideAnimPlayOffset,
                          gkOp,
                          isCollisionWithPlayer,
                          playerRbPos,
                          playerRbRot,
                          rotatedRbPos,
                          rotRbToBall,
                          packageSeqNumber);
                yield return null;
            }
            else
            {
                break;
            }
        }

        RPC_confirmation[idx] = false;
    }

    public IEnumerable rpc_playerOnBall_resend(int idx)
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                ///print("Send RPC_goalUpdate score Ienumerator ");
                photonView.RPC("RPC_playerOnBAll",
                                RpcTarget.Others);
                yield return null;
            }
            else
            {
                break;
            }
        }

        RPC_confirmation[idx] = true;
    }

    public IEnumerator sendAndACKGoal(
                                      int idx,
                                      int goalNum,
                                      int scoreNum)                             
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                ///print("Send RPC_goalUpdate score Ienumerator ");
                photonView.RPC("RPC_goalUpdate",
                                RpcTarget.Others,
                                goalNum, 
                                scoreNum);
                yield return null;
            }
            else
            {
                break;
            }
        }

        RPC_confirmation[idx] = false;
    }

    public IEnumerator sendAndACKBallOut(int idx,
                                         Vector3 ballPosAfterOut,
                                         bool timeToShotExceeded,
                                         bool isGoalJustScored,
                                         int score)
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                photonView.RPC("RPC_ballOut",
                                RpcTarget.Others,
                                ballPosAfterOut,
                                timeToShotExceeded,
                                isGoalJustScored,
                                score);
                yield return null;
            }
            else
            {
                break;
            }
        }

        RPC_confirmation[idx] = false;
    }

    public IEnumerator sendAndACKBallOutNewPos(int idx,
                                               Vector3 ballPosAfterOut)
                                              
    {
        for (int i = 0; i <= 50; i++)
        {
            if (!RPC_confirmation[idx])
            {
                photonView.RPC("RPC_ballOutNewPos",
                                RpcTarget.Others,
                                ballPosAfterOut);
                         
                yield return null;
            }
            else
            {
                break;
            }
        }

        RPC_confirmation[idx] = false;
    }



    private void RPC_sendGK_Predict(Vector3 collisionOutput,
                                    float collisionPercentTime,
                                    Vector3 ballPosWhenCollision,
                                    float gkSequenceStart,
                                    float gkSideAnimPlayOffset,
                                    string gkOp,
                                    bool isCollisionWithPlayer,
                                    Vector3 playerRbPos,
                                    Vector3 playerRbRot,
                                    Vector3 rotatedRbPos,
                                    Vector3 rotRbToBall,
                                    short packageSeqNumber)
    {
        ///print("DBG342344COL RPC_GK_COLLISION_PREDICT sent ### time " + Time.time);

        photonView.RPC("RPC_GK_COLLISION_PREDICT",
                        RpcTarget.Others,
                        collisionOutput,
                        collisionPercentTime,
                        ballPosWhenCollision,
                        //gkShotSimulateStart
                        gkSequenceStart,
                        gkSideAnimPlayOffset,
                        //peerPlayer.getShotPercent(),
                        gkOp,
                        playerRbPos,
                        //playerRb.transform.rotation,
                        playerRbRot,
                        isCollisionWithPlayer,
                        rotatedRbPos,
                        rotRbToBall,
                        packageSeqNumber);                        
    }

    private void gkAnimationPlayPrediction(Rigidbody rbTmp,
                                           GameObject rotatedRbToBall,
                                           Rigidbody ballRb,
                                           Vector3 outShotStart,
                                           Vector3 outShotMid,
                                           Vector3 outShotEnd,
                                           Vector3 endPosOrg,
                                           float timeofBallFly,
                                           ref float shotPassedTime,
                                           Vector3 localSpace,
                                           bool isLobActive,
                                           float timeToHitZ,
                                           SHOTVARIANT2 shotvariant,
                                           ref float lastGkDistX,
                                           ref float gkTimeToCorrectPos,
                                           ref float gkSideAnimPlayOffset,
                                           ref Vector3 localGkStartPos,
                                           ref string lastAnimName,
                                           ref string predictedAnimName,
                                           ref float predictedAnimDelay,
                                           ref float predictedAnimSpeed,
                                           ref float curvePercentHit,
                                           Vector3[,] prepareShotPos,
                                           int prepareShotMaxIdx)
    {

        /*getRotatedRbToBall(peerPlayer.getBallInit(),
                       rbTmp,
                       ref getRotatedRbToBallRef(),
                       getGkCornerPoints());

        gkHitPointCalc(rbTmp,
                       shotvariant,
                       outShotStart,
                       outShotMid,
                       outShotEnd,
                       endPosOrg,
                       timeofBallFly,
                       shotPassedTime,
                       rotatedRbToBall,
                       ref localSpace,
                       ref timeToHitZ,
                       ref curvePercentHit);*/

        /*print("DEBUGGK1045 gkAnimationPlayPrediction BEGIN " + " localSpace " +
            localSpace + " timeToHitZ " + timeToHitZ + " rb.transform " + rbTmp.transform.position
            + " rotatedRbToBall " + rotatedRbToBall.transform.position
            + " ballRb " + ballRb.transform.position);*/


        //print("DBGCOLLISIONCALC1024D localSpacelocalSpace beforeCorrect " + localSpace);
        correctLocalOffsetMax(ref localSpace, shotvariant, false);

        float distX = Mathf.Abs(localSpace.x);
        float distY = Mathf.Abs(localSpace.y);
        float maxTimingCorrection = 0.230f;

        //print("DBGCOLLISIONCALC1024D localSpacelocalSpace distY " + distY + " localSpace " + localSpace);
        //print("DBGK1024_SIMULATE PLAY PREDICTION ");

        string animName = "";

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


        //print("DBGCOLLISIONCALC1024D localSpacelocalSpace distY " + distY + " localSpace " + localSpace + " AnimName " + animName);

        /* when player clicked to late, adjust 
         * cpuAnimAdjustSpeed until MAX_ANIM_GK_PLAYER_SPEED*/

        float calcAnimSpeed = calcGkAnimSpeedBaseOnTimeToHit(ref animName,
                                                             timeToHitZ,
                                                             ballRb,
                                                             ref initGkDeleyLevel,
                                                             ref levelDelay,
                                                             ref calculatedTimeToStartPos,
                                                             localSpace,
                                                             false,
                                                             0.2f,
                                                             MIN_ANIM_GK_PLAYER_SPEED,
                                                             MAX_ANIM_GK_PLAYER_SPEED,
                                                             shotvariant);
        //print("DEBUGGK1045 calcAnimSpeed " + calcAnimSpeed + " maxTimingCorrection " + maxTimingCorrection);
        //print("PLAYER ANIMSPEED " + cpuAnimAdjustSpeed);
        //}

        //initAnimName = animName;
        //initCpuAdjustAnimSpeed = true;
        //print("GKDEBUG800 CPUADMINADJUST " + cpuAnimAdjustSpeed + " isCpuPlayer " + isCpuPlayer);
        //if (!isCpuPlayer)
        //    print("TESTPOINT121 CPU ANIM SPEED CALCULATED " + cpuAnimAdjustSpeed  + " VEL " + ballRb.velocity.z);
        //}

        predictedAnimDelay = 0f;
        while (true)
        {
            if (calculateTimeToGkAction(ref animName,
                                        timeToHitZ,
                                        ballRb,
                                        calcAnimSpeed,
                                        ref initGkDeleyLevel,
                                        ref levelDelay,
                                        ref calculatedTimeToStartPos,
                                        localSpace,
                                        shotvariant,
                                        false) &&
                (Mathf.Abs(calculatedTimeToStartPos - timeToHitZ) < maxTimingCorrection) &&
                 calculatedTimeToStartPos < timeToHitZ)
            {
                //print("CALCULATED TIME NOCPU " + calculatedTimeToStartPos + " REALTIMETOHIT "
                //     + timeToHitZ + " LOCAL " + localSpace + " cpuAnimAdjustSpeed " + cpuAnimAdjustSpeed);
                predictedAnimDelay += Globals.FIXEDUPDATE_TIME;

                getRotatedRbToBall(//peerPlayer.getBallInit(),
                          prepareShotPos[0,0],
                          rbTmp,
                          ref rotatedRbToBall,
                          //ref getRotatedRbToBallRef(),
                          getGkCornerPoints());

                gkHitPointCalc(rbTmp,
                               shotvariant,
                               outShotStart,
                               outShotMid,
                               outShotEnd,
                               endPosOrg,
                               timeofBallFly,
                               shotPassedTime,
                               rotatedRbToBall,
                               ref localSpace,
                               ref timeToHitZ,
                               ref curvePercentHit,
                               prepareShotPos,
                               prepareShotMaxIdx);

                /*Vector3 realHitPlaceLocal3 = bezierCurvePlaneInterPoint(0.0f,
                                                      1.0f,
                                                      rotatedRbToBall,
                                                      outShotStart,
                                                      outShotMid,
                                                      outShotEnd,
                                                      true);*/

                Vector3 realHitPlaceLocal3 =  bezierCurvePlaneInterPoint(
                                                      rotatedRbToBall,
                                                      prepareShotPos,
                                                      0,
                                                      prepareShotMaxIdx - 1,
                                                      true);

                shotPassedTime = shotPassedTime + (Globals.FIXEDUPDATE_TIME * 1000.0f);

                float timeToHitZ2 = ((realHitPlaceLocal3.z * timeofBallFly) - shotPassedTime) / 1000f;
                /*print("DEBUGGK1045 delay calculating... timeToHitZ " + timeToHitZ2
                    + " delay " + predictedAnimDelay + " calculatedTimeToStartPos " +
                    calculatedTimeToStartPos
                    + " calcAnimSpeed " + calcAnimSpeed);*/

                continue;
            }
            else
            {
                break;
            }
        }

        //print("DEBUGGK1045 predictedAnimDelay " + predictedAnimDelay);

         //print("#DEBUGGK1045 gkPredict localSpace before OFFSET" + localSpace + " timeToHitZ " + timeToHitZ + " predictedAnimName " 
         //   + predictedAnimName);

        /*Overwrite to default value */
        calculatedTimeToStartPos = 0.227f;
        float offset;

        offset = 1.15f;
        if (animName.Contains("up"))
        {
            offset = 1.50f;
            if (animName.Contains("punch"))
                offset = 2.0f;
        }
        else if (animName.Contains("mid"))
        {
            offset = 1.10f;
            //it was 2.30 before
            if (animName.Contains("punch"))
                offset = 2.40f;
        }
        else if (animName.Contains("down"))
        {
            offset = 1.0f;
            if (animName.Contains("punch"))
                offset = 2.20f;
        }

        lastGkDistX = localSpace.x;

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

        //print("DBGCOLLISIONCALC1024D localSpace " + localSpace);

        gkTimeToCorrectPos = calculateMatchParentGkTimePos(animName);

        ///print("DEBUGGK1045 gkTimeToCorrectPos " + gkTimeToCorrectPos);
        //print("DBGK1024_SIMULATE_DBG2435 offset " + offset);
        //interruptSideAnimation(animator, rb);

        predictedAnimName = "3D_GK_sidecatch_" + animName;
        predictedAnimSpeed = calcAnimSpeed;

        //print("#DEBUGGK1045 gkPredict localSpace dbg PLAYMAIN ### " + localSpace + " timeToHitZ " + timeToHitZ + " predictedAnimName " 
        //    + predictedAnimName);

        float timeLeft = timeToHitZ;
        if (timeToHitZ < gkTimeToCorrectPos)
        {
            localSpace.x = localSpace.x * Mathf.InverseLerp(0f, timeToHitZ, gkTimeToCorrectPos);
            /*print("DEBUGGK1045 localSpace corrected " + localSpace + 
                " Mathf.InverseLerp(0f, timeToHitZ, gkTimeToCorrectPos) "
                + Mathf.InverseLerp(0f, timeToHitZ, gkTimeToCorrectPos)
                + " gkTimeToCorrectPos " + gkTimeToCorrectPos
                + " timeToHitZ " + timeToHitZ);*/
        }
       
        if (predictedAnimName.Contains("3D_GK_sidecatch_straight_"))
            localSpace.x = 0f;

        Vector3 localGkStartPosTmp = Vector3.zero;
        // InverseTransformPointUnscaled(rb.transform, rb.transform.position);
        localGkStartPosTmp.x += localSpace.x;
        localGkStartPos = TransformPointUnscaled(rbTmp.transform, localGkStartPosTmp);

        int numbOfFrames = getnumberOfFramesOfAnim(predictedAnimName);
        //Percent of animation executed
        gkSideAnimPlayOffset =
            Mathf.InverseLerp(0, numbOfFrames, (Globals.FRAME_RATE * calcAnimSpeed) * timeLeft);
        //print("DEBUGGK1045 gkSideAnimPlayOffset " + gkSideAnimPlayOffset + " calcAnimSpeed " + calcAnimSpeed);
        //print("DEBUGGK1045 localGkStartPos(Global) " + localGkStartPos + "  " +
        //    " predictedAnimName " + predictedAnimName
        //    + " localSpace " + localSpace);

        /*print("DEBUGGK1045 gkPredict MAIN PLAY ****** localGkStartPos " + localGkStartPos + " rb.transform.position " + rbTmp.transform.position
            + " localSpace " + localSpace + " gkTimeToCorrectPos " + gkTimeToCorrectPos
            + " animName " + predictedAnimName 
            + " numbOfFrames " + numbOfFrames
            + " gkSideAnimPlayOffset " + gkSideAnimPlayOffset
            + " localGkStartPosTmp " + localGkStartPosTmp);*/
        //lastGkAnimName = animName;
        lastAnimName = animName;
    }

    private void gkHitPointCalc(
                           Rigidbody rb,
                           SHOTVARIANT2 shotvariant,
                           Vector3 outShotStart,
                           Vector3 outShotMid,
                           Vector3 outShotEnd,
                           Vector3 endPosOrg,
                           float timeofBallFly,
                           float passedShotFlyTime,
                           GameObject rotatedRbToBall,
                           ref Vector3 localSpace,
                           ref float timeToHitZ,
                           ref float curvePercentHit,
                           Vector3[,] prepareShotPos,
                           int prepareShotMaxIdx)
    {
        Vector3 clickedRbRotatedLS = InverseTransformPointUnscaled(
                                                                   rotatedRbToBall.transform,
                                                                   gkTouchPosRotatedRbWS);

        Vector3 clickedRbLS = InverseTransformPointUnscaled(
                                                rb.transform,
                                                gkTouchPosRbWS);

        /*is that correct*/
        clickedRbRotatedLS.z = clickedRbLS.z = 0.0f;

        //print("DEBUGLASTTOUCH IN SPACE GK MOVES " + gkTouchPosRotatedRbWS);
        Vector3 realHitPlaceLocal = Vector3.zero;

        ////print("DBGCOLLISIONCALC1024DB shotvariant " + shotvariant);

        //if (shotvariant == SHOTVARIANT2.CURVE)
        //{
        /*realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rotatedRbToBall,
                                                       outShotStart,
                                                       outShotMid,
                                                       outShotEnd,
                                                       true);*/

        realHitPlaceLocal = bezierCurvePlaneInterPoint(
                                                       rotatedRbToBall,
                                                       prepareShotPos,
                                                       0,
                                                       prepareShotMaxIdx - 1,
                                                       true);


        curvePercentHit = realHitPlaceLocal.z;
        timeToHitZ = ((realHitPlaceLocal.z * timeofBallFly) - passedShotFlyTime) / 1000.0f;

        /*print("DBGCOLLISIONCALC1024DBGK realHitPlaceLocal "
            + realHitPlaceLocal
            + " timeToHitZ "
            + timeToHitZ
            + " timeofBallFly " + timeofBallFly
            + " passedShotFlyTime " + passedShotFlyTime
            + " rotatedRbToBall " + rotatedRbToBall.transform.position
            + " rb.transform " + rb.transform.position
            + " gkTouchPosRbWS " + gkTouchPosRbWS
            + " gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
            + " outShotStart " + outShotStart
            + " outShotMid " + outShotMid
            + " outShotEnd " + outShotEnd);*/
        /*now don't overwritte z */
        realHitPlaceLocal = bezierCurvePlaneInterPoint(
                                               rotatedRbToBall,
                                               prepareShotPos,
                                               0,
                                               prepareShotMaxIdx - 1,
                                               false);


        /*print("#DEBUGGK1045 START realHitPlaceLocal " + realHitPlaceLocal);
        for (int i = 0; i < prepareShotMaxIdx; i++)
        {
            print("MANUALCALCULATION I: " + i + " global " + prepareShotPos[i, 0]
                + " local " + InverseTransformPointUnscaled(rotatedRbToBall.transform, prepareShotPos[i, 0])
                + " curvePercent " + prepareShotPos[i, 1]);
        }*/
        //print("#DEBUGGK1045  END realHitPlaceLocal " + realHitPlaceLocal);

        //print("#DBGK1024_SIMULATE realHitPlaceLocal " + realHitPlaceLocal + " curvePercentHit " + curvePercentHit);
        //print("GKDEBUG1X timeToHitZ bezier AFTER " + timeToHitZ
        //+ " rotatedRBToBall " + rotatedRbToBall.transform.position
        //+ " rotatedtoBall.EulerAngles " + rotatedRbToBall.transform.eulerAngles
        //+ " realHitPlaceLocal " + realHitPlaceLocal);
        
        //if (Mathf.Abs(realHitPlaceLocal.z) > 0.5f)
        /*if (Mathf.Abs(realHitPlaceLocal.z) > 0.7f)
        {
            realHitPlaceLocal = INCORRECT_VECTOR;
            timeToHitZ = 0f;
        }*/

        //print("DEBUGLASTTOUCH CURVED HIT POINT " + TransformPointUnscaled(rotatedRbToBall.transform, 
        //                                                                  realHitPlaceLocal)); 

        gkDistRealClicked = Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal);
        //print("DBGCOLLISIONCALC1024DBGK gkDistRealClicked " + gkDistRealClicked);

        if (gkDistRealClicked <= MIN_DIST_REAL_CLICKED &&
            realHitPlaceLocal != INCORRECT_VECTOR)
        {
            localSpace = realHitPlaceLocal;
            //print("DEBUGGK1045 localSpace click CORRECT## localSpace " + localSpace + " clickedRbRotatedLS  "
            //     + clickedRbRotatedLS + " gkTouchPosRotatedRbWS  " + gkTouchPosRotatedRbWS);
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

            //print("DEBUGGK1045 localSpace click NOT CORRECT## " + localSpace + " clickedRbRotatedLS  " 
            //    + clickedRbRotatedLS + " gkTouchPosRotatedRbWS  " + gkTouchPosRotatedRbWS);

        }

        //print("#DBGK1024_SIMULATE localSpace " + realHitPlaceLocal + " clickedRbRotatedLS " + clickedRbRotatedLS
        //    + " gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
         //   + " TransformPointUnscaled global " + TransformPointUnscaled(rotatedRbToBall.transform, localSpace));

        localSpace.z = 0;
        //}
    }

    public void RbLookAt(Vector3 playerDirection,
                         Rigidbody rb,
                         bool isMaster)
    {
        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        if (playerDirection == Vector3.zero)
        {
            if (isMaster)
                playerDirection = new Vector3(0f, 0f, 1f);
            else
                playerDirection = new Vector3(0f, 0f, -1f);
        }

        lookOnLook = Quaternion.LookRotation(playerDirection);

        rb.transform.rotation =
                  Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);
    }

    private void rpc_UpdatePos()
    {
        //if (!photonView.IsMine)
        //{

      
        //print("rpc_rbPredictedPos gkRotationLoops " 
        //    + gkRotationLoops + " ANIM " + !checkIfAnyAnimationPlaying(animator, 1.0f));

        if (!photonView.IsMine &&
            !checkIfAnyAnimationPlaying(animator, 1.0f) &&
            //(gkStartSequenceTime == -1))
            gkRotationLoops < 1)
        {

            //rb.transform.position = rpc_rbPlayerPos;
            //print("#RBPOSDIFF CORRECT " + rpc_rbPos + " now " + rb.transform.position);

            if ((rpc_joystickExtraButtonsIdx != -1) &&
                 !isPlaying(animator, JoystickButtonAnimNames[rpc_joystickExtraButtonsIdx], 1f)) {
                    animator.Play(JoystickButtonAnimNames[rpc_joystickExtraButtonsIdx]);
                } else {
                  //  if (checkIfJoystickExtraButtonsPlaying(animator, 1f) == -1)
                  //  {
                        Vector3 direction =
                            rpc_rbPos - rb.transform.position;

                        Vector3 rpc_rbPredictedPos =
                        new Vector3(rpc_rbPos.x, 0f, rpc_rbPos.z);
                        rpc_rbPredictedPos += rpc_rbVelocity * rpc_mainLag;



                        //print("rpc_rbPredictedPos " + rpc_rbPredictedPos + " rpc_rbVelocity " + rpc_rbVelocity
                        //    + " rpc_mainLag " + rpc_mainLag + " rpc_rbPos " + rpc_rbPos);

                        float dist = Vector2.Distance(new Vector2(rb.transform.position.x, rb.transform.position.z),
                                              new Vector2(rpc_rbPredictedPos.x, rpc_rbPredictedPos.z));

                        rb.velocity = Vector3.zero;
                        rb.transform.position =
                        Vector3.MoveTowards(rb.transform.position,
                                            rpc_rbPredictedPos,
                                            9f * Time.deltaTime);

                if (checkIfJoystickExtraButtonsPlaying(animator, 1f) == -1)
                    RbLookAt(rpc_playerDirection, rb, isMaster);

                //print("RPCVELOCITY " + rpc_rbVelocity.ToString("F10") + "rpc_rbRotation "+ rpc_rbRotation);
                if (rpc_rbVelocity == Vector3.zero)
                {
                    rb.transform.rotation = rpc_rbRotation;
                }
                    //}
            }
            //rpc_runningSpeed * Time.deltaTime);

            //print("DBGPLAYERPOS rpc updat " + rb.transform.position);
            //dist * Time.deltaTime);

            //print("#DBGPOS " + rb.transform.position + " lag " + rpc_mainLag + " " +
            //    Time.deltaTime " + Time.deltaTime);

            /*if (!isPlaying(animator, "3D_run", 1f) &&
                rpc_rbVelocity != Vector3.zero)
                animator.Play("3D_run", 0, 0.0f);*/

            //setupRunningAnimSpeed();
            /* if (dist < 0.03f)
                 rb.velocity = Vector3.zero;
             else
                 //rb.velocity = rpc_rbVelocity * rpc_runningSpeed;
                 rb.velocity = direction * rpc_mainLag;
             */
            //rb.angularVelocity = rpc_rbAngularVelocity;
            //rb.transform.rotation =
            //     Quaternion.Slerp(
            //         rb.transform.rotation, Quaternion.LookRotation(rpc_playerDirection), Time.deltaTime * 10.0f);
            //rb.transform.rotation = rpc_rbPlayerRotation;
        }
        //}
    }

    void Awake()
    {
        prepareShotPos = new Vector3[100, 2];
        //Globals.teamBid = -1;
        leagueBackgroundMusic =
                GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
        leagueBackgroundMusic.stop();

        RPC_initConfirm();
        initColliders();

        tmpRbGameObject = new GameObject();
        tmpRigidbody = tmpRbGameObject.AddComponent<Rigidbody>();

        //TOREMOVE
        ///Globals.leagueName = "NATIONAL TEAMS";
        //Globals.teamAname = "Argentina";
        //Globals.teamBname = "England";
        //TOREMOVE
        Globals.playerPlayAway = false;
        Globals.commentatorStr = "YES";
        Globals.powersStr = "YES";
        Globals.joystickSide = "LEFT";
        Globals.graphicsQuality = "STANDARD";
        /*

        Teams nationalTeams = new Teams("NATIONAL TEAMS");
        for (int i = 0; i < 2; i++)
        {
            int teamRandomInt = UnityEngine.Random.Range(0, nationalTeams.getMaxTeams());
            string[] randTeam = nationalTeams.getTeamByIndex(teamRandomInt);
            if (i == 0)
            {
                Globals.teamAname = randTeam[0];

                Globals.teamAid = teamRandomInt;
                Globals.stadiumColorTeamA = 
                    randTeam[5];
            }
            else
            {
                Globals.teamBname = randTeam[0];

                Globals.teamBid = teamRandomInt;
                Globals.stadiumColorTeamB =
                               randTeam[5];
            }

            nationalTeams.swapElements(
                teamRandomInt, nationalTeams.getMaxActiveTeams() - 1);
        }

        */


        //TODO
        /*Traning hardcoded values*/
        /*Globals.teamAcumulativeStrength = 180;
        Globals.teamBcumulativeStrength = 180;

        Globals.teamAGkStrength = 90;
        Globals.teamBGkStrength = 80;

        Globals.teamAAttackStrength = 90;
        Globals.teamBAttackStrength = 90;
        */
        Globals.matchTime = "90 SECONDS";
        Globals.level = 3;

        //Globals.graphicsQuality = "normal";
        //Globals.maxTimeToShotStr = "10000 SECONDS";
        Globals.maxTimeToShotStr = "9 SECONDS";
        Globals.level = 3;

        fansFlagAngles = new Vector3[FANS_FLAG_MAX];
        fansFlagDirections = new Vector3[FANS_FLAG_MAX];
        isFansFlagActive = new bool[FANS_FLAG_MAX];

        //initBonuses();
        initPowers();
        leagueName = Globals.leagueName;
        Globals.initSkills(ref attackSkillsPlayer,
                           ref attackSkillsCpu,
                           ref defenseSkillsPlayer,
                           ref defenseSkillsCpu,
                           ref cumulativeStrengthPlayer,
                           ref cumulativeStrengthCpu,
                           Globals.playerPlayAway);

        //if (leagueName.Equals("WORLDCUP") ||
        //    leagueName.Equals("EUROCUP") ||
        //    leagueName.Equals("COPAAMERICA"))
        //{
        //    teamHostID = UnityEngine.Random.Range(1, 3);
        //}
        //else
        //{
        //    teamHostID = 1;
        //}

        ballRb = new Rigidbody[NUMBER_OF_BALLS + 1];
        ballRbLeftSide = new GameObject[NUMBER_OF_BALLS + 1];
        ballRbRightSide = new GameObject[NUMBER_OF_BALLS + 1];

        ball = new BallMovementMultiplayer[NUMBER_OF_BALLS + 1];

        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {
            ballRb[i] = GameObject.Find("ball" + i.ToString()).GetComponent<Rigidbody>();
            ball[i] = GameObject.Find("ball" + i.ToString()).GetComponent<BallMovementMultiplayer>();
            ballRbLeftSide[i] = GameObject.Find("ball" + i.ToString() + "LeftSide");
            ballRbRightSide[i] = GameObject.Find("ball" + i.ToString() + "RightSide");
        }

        for (int i = NUMBER_OF_BALLS + 1; i <= MAX_NUMBER_OF_BALLS; i++)
        {
            GameObject.Find("ball" + i.ToString()).SetActive(false);
        }

        cpuLeftPalm = GameObject.FindWithTag("playerUpLeftPalm");
        cpuRightPalm = GameObject.FindWithTag("playerUpRightPalm");

        RPC_locks = new RPC_blocking(INCORRECT_VECTOR);

    }

    void Start()
    {
        //print("DBG12345 Globals.level " + Globals.level);


        if (Globals.player1MainScript != this)
            Globals.player2MainScript = this;

        photonView = GetComponent<PhotonView>();
        //print("DBGINTRO#### photonView " + photonView  + " ISmINE" + photonView.IsMine);

        //if (photonView.IsMine)
        //{
        //Globals.peerPhotonView = photonView;
        //    Globals.peerPlayerMine = Globals.player2MainScript;
        /*print("DBGPHOTONVIEW1 SAVE PHOTON VIEW "
            + Globals.peerPhotonView + " photonView "
            + photonView
            + "gameObject.name " + gameObject.name);*/
        //}
        //elsesetWallColliders
        // {
        //Globals.peerPhotonView = null;
        ///    Globals.peerPlayerNotMine = this;
        //}

        if (PhotonNetwork.IsMasterClient)
            isMaster = true;
        else
            isMaster = false;

        if (photonView.IsMine == false)
            isMaster = !isMaster;


        if (isMaster)
            teamHostID = 1;
        else
            teamHostID = 2;

        if (photonView.IsMine)
        {
            Material goalNonTransparent = graphics.getMaterial("Goals/Material/goalNonTransparent");
            Material goalTransparent = graphics.getMaterial("Goals/Material/goalTransparent");
            Material goalNonTransparentNet = graphics.getMaterial("Goals/Material/goalNonTransparentNet");
            Material goalTransparentNet = graphics.getMaterial("Goals/Material/goalTransparentNet");
            Material wallObstaclesTransparent = 
                graphics.getMaterial("powers/obstaclesMaterialTransparent");
            Material wallObstaclesNonTransparent = 
                graphics.getMaterial("powers/obstaclesMaterialNonTransparent");

            if (!PhotonNetwork.IsMasterClient)
            {
                GameObject standsfences = null;
                if (!Globals.PITCHTYPE.Equals("STREET"))
                    standsfences = GameObject.Find("standsFences");
                GameObject stadiumObjects = GameObject.Find("stadiumObjects");
                GameObject pitchBorders = GameObject.Find("pitchBorders");

                //stadiumObjects.transform.position = new Vector3(16.81576f, stadiumObjects.transform.position.y, -21.9f);
                if (!Globals.PITCHTYPE.Equals("STREET"))
                {
                    stadiumObjects.transform.position = new Vector3(4.6f, 7.99f, 0.49f);
                    standsfences.transform.position = 
                        new Vector3(-4.9f, standsfences.transform.position.y, -0.58f);
                    standsfences.transform.eulerAngles = 
                        stadiumObjects.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    pitchBorders.transform.eulerAngles = stadiumObjects.transform.eulerAngles =
                        new Vector3(0f, 180f, 0f);
                } else {
                    stadiumObjects.transform.eulerAngles = new Vector3(0f, 270f, 0f);
                    pitchBorders.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    GameObject.Find("groundLight").transform.eulerAngles = new Vector3(50f, 100, 0f);
                }

                graphics.setMaterialElement(
                            GameObject.Find("goalDown_PostCrossbar"),
                            goalNonTransparent,
                            0);
                graphics.setMaterialElement(
                GameObject.Find("goalUp_PostCrossbar"),
                goalTransparent,
                0);

                if (!Globals.PITCHTYPE.Equals("STREET"))
                {
                    GameObject.Find("DirectionalLight1").transform.eulerAngles = new Vector3(50f, -150f, 0f);
                    GameObject.Find("LineGoalDown").SetActive(false);
                    
                } else
                {
                    graphics.setMaterialElement(
                    GameObject.Find("goalDownNet"),
                    goalNonTransparentNet,
                    0);
                    graphics.setMaterialElement(
                    GameObject.Find("goalUpNet"),
                    goalTransparentNet,
                    0);
                    GameObject.Find("DirectionalLight1").transform.eulerAngles = new Vector3(50f, 200f, 0f);
                }






                if (!Globals.PITCHTYPE.Equals("STREET"))
                    GameObject.Find("goalDownBallCrossLine").SetActive(false);

                graphics.setMaterialElement(
                   GameObject.Find("wallGoalUpObstacle1"),
                   wallObstaclesTransparent,
                   0);
                graphics.setMaterialElement(
                    GameObject.Find("wallGoalDownObstacle1"),
                    wallObstaclesNonTransparent,
                    0);
            }
            else
            {
                if (!Globals.PITCHTYPE.Equals("STREET"))
                {
                    GameObject.Find("goalUpBallCrossLine").SetActive(false);
                    GameObject.Find("LineGoalUp").SetActive(false);
                }
            }

            UICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            GameObject.Find("wallGoalUpObstacle1").SetActive(false);
            GameObject.Find("wallGoalDownObstacle1").SetActive(false);

            ///gamePausedScript = GameObject.Find("pauseGame").GetComponent<gamePausedMenu>();
            cameraButton = GameObject.Find("camera_button").GetComponent<buttonCameraMulti>();

            volleyButton = GameObject.Find("volley_button").GetComponent<buttonVolleyMulti>();
            lobButton = GameObject.Find("lob_button").GetComponent<buttonLobMulti>();
            audienceReactionsScript = GameObject.Find("fans").GetComponent<audienceReactions>();
            joystick = GameObject.Find("joystickBG").GetComponent<joystick1>();

            if (!Globals.PITCHTYPE.Equals("STREET"))
            {
                fansFlag = new GameObject[FANS_FLAG_MAX];
                fansFlagSticks = new GameObject[FANS_FLAG_MAX];

                for (int i = 0; i < FANS_FLAG_MAX; i++)
                {
                    fansFlag[i] = GameObject.Find("flag" + (i + 4).ToString());
                    fansFlagSticks[i] = GameObject.Find("flag" + (i + 4).ToString() + "Stick");
                }
            }
        }

        rbTmpGameObj = new GameObject();
        rbTmp = rbTmpGameObj.AddComponent<Rigidbody>();

        rotatedRbToBallTmp = new GameObject();

        //playerControllerMultiplayer.wallCollierTest = GameObject.Find("wallUpLeft2").GetComponent<Collider>().isTrigger;
        //print("DBG342344COL onColliderTest START " + playerControllerMultiplayer.wallCollierTest);
        playerDownLeftToeBaseCollider = getChildWithName(gameObject, "playerDownLeftToeBaseCollider");
        playerColliderCollierTest = playerDownLeftToeBaseCollider.GetComponent<Collider>().isTrigger;
        playerTextureGO = getChildWithName(gameObject, "model_002");
        hairPlayerDownGO = getChildWithName(gameObject, "hair_playerDown");

        //print("playerTextureGO " + playerTextureGO);

        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0;
        gkCollisionPackageArriveTime = Time.time;

        ballInitialization();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rpc_rbPrevPos = rb.transform.position;
        rpc_prevPosStored = rb.transform.position;

        if (PhotonNetwork.IsMasterClient)
            gkCornerPoints = new Vector3(PITCH_WIDTH_HALF, 0f, -PITCH_HEIGHT_HALF);
        else
            gkCornerPoints = new Vector3(PITCH_WIDTH_HALF, 0f, PITCH_HEIGHT_HALF);

        maxTimeToShot =
            float.Parse(Regex.Replace(Globals.maxTimeToShotStr, @"[^\d]", ""));

        ///if (Globals.level == Globals.MIN_LEVEL)
        //    maxTimeToShot = 20;

        goalResize(true);

        //extraShotVec = new Vector3(
        //    UnityEngine.Random.Range(4.3f, 4.4f),
        //    UnityEngine.Random.Range(2.7f, 2.8f),
        //    -PITCH_HEIGHT_HALF);

        //Application.targetFrameRate = 30;
        //QualitySettings.vSyncCount = 0;
        //QualitySettings.antiAliasing = 0;

        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        dummyTouchRotatedGO = new GameObject("touchRotatedGO");

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

        if (photonView.IsMine)
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

        if (photonView.IsMine)
        {
            gkHelperImageGameObject = GameObject.Find("gkHelper");
            gkClickHelperGameObject = GameObject.Find("gkClickHelper");

            gkHelperImage = GameObject.Find("gkHelper").GetComponent<Image>();
            gkHelperRectTransform = GameObject.Find("gkHelper").GetComponent<RectTransform>();
            gkHelperImageWidth = Screen.width * 0.055f;
            gkHelpeImageHeight = gkHelperImageWidth;
            gkHelperRectTransform.sizeDelta = new Vector2(gkHelperImageWidth,
                                                      gkHelpeImageHeight);
            setupCanvasGameObjects();
            setJoystickPosition();
            setSpecialButtonsPosition();
        }

        rotatedRbToBall = new GameObject();
        tmpRotatedRbToBall = new GameObject();

        Globals.score1 = 0;
        Globals.score2 = 0;

        if (photonView.IsMine)
        {
            score1Text = GameObject.Find("scoreText_1").GetComponent<TextMeshProUGUI>();
            score2Text = GameObject.Find("scoreText_2").GetComponent<TextMeshProUGUI>();

            mainTimeText = GameObject.Find("mainTimeText").GetComponent<TextMeshProUGUI>();
            timeToShotText = GameObject.Find("timeToShotText").GetComponent<TextMeshProUGUI>();
            networkPing = GameObject.Find("networkPing").GetComponent<Text>();
            networkInfoPanel = GameObject.Find("networkInfoPanel");
            networkInfoPanel.SetActive(false);
            shotBar = GameObject.Find("shotBar").GetComponent<Image>();
            speedShotText = GameObject.Find("speedShotText").GetComponent<TextMeshProUGUI>();
        }
        /* rbRightFoot = GameObject.FindWithTag("playerDownRightLeg");
         rbRightToeBase = GameObject.FindWithTag("playerDownRightToeBase");
         rbLeftToeBase = GameObject.FindWithTag("playerDownLeftToeBase");
         rbHead = GameObject.FindWithTag("playerDownHead");


         leftPalm = GameObject.FindWithTag("playerDownLeftPalm");
         rightPalm = GameObject.FindWithTag("playerDownRightPalm");

         leftHand = GameObject.FindWithTag("playerDownLeftHand");
         rightHand = GameObject.FindWithTag("playerDownRightHand");

         cpuLeftHand = GameObject.FindWithTag("playerUpLeftHand");
         cpuRightHand = GameObject.FindWithTag("playerUpRightHand");
         */

        rbRightFoot = getChildWithName(gameObject, "playerDownRightLeg");
        rbRightToeBase = getChildWithName(gameObject, "playerDownRightToeBase");
        rbLeftToeBase = getChildWithName(gameObject, "playerDownLeftToeBase");
        rbHead = getChildWithName(gameObject, "playerDownHead");

        cpuLeftPalm = getChildWithName(gameObject, "playerUpLeftPalm");
        cpuRightPalm = getChildWithName(gameObject, "playerUpRightPalm");
        leftPalm = getChildWithName(gameObject, "playerDownLeftPalm");
        rightPalm = getChildWithName(gameObject, "playerDownRightPalm");
        leftHand = getChildWithName(gameObject, "playerDownLeftHand");
        rightHand = getChildWithName(gameObject, "playerDownRightHand");


        setTextureScript = GameObject.Find("MainCamera").GetComponent<setTexturesMulti>();

        setPlayersColliders(gameObject, true);

        matchStatistics = new MatchStatisticsMulti();

        if (photonView.IsMine)
        {
            string[] flagsNames = new string[] { Globals.teamAname,
                                                 Globals.teamBname};
            //TOUNCOMMENTMULTi
            RawImage flagGameImage;
            for (int i = 0; i < flagsNames.Length; i++)
            {

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

            calculateAndSetWinTieMatchIntroValues(cumulativeStrengthPlayer,
                                                  cumulativeStrengthCpu);


            //print("DEGUBX1 teamA name cumulative " + Globals.teamAname + " value " + Globals.teamAcumulativeStrength);
            //print("DEBUGX1 teamB name cumulative " + Globals.teamBname + " value " + Globals.teamBcumulativeStrength);


            graphics.setFlagRawImage(teamAflagStatisticsImage,
                                     graphics.getFlagPath(Globals.teamAname));

            graphics.setFlagRawImage(teamBflagStatisticsImage,
                                     graphics.getFlagPath(Globals.teamBname));

            teamAStatisticsText.text = Globals.teamAname;
            teamBStatisticsText.text = Globals.teamBname;

            if (!Globals.PITCHTYPE.Equals("STREET"))
            {
                updateStadiumTextures();
                initFlagPositions();
            }

            setScoresText();
            setTimesText();
        }

        //trailRenderer = GameObject.Find("shotDrawLine").GetComponent<TrailRenderer>();
        //trail = GameObject.Find("shotDrawLine");

        animationOffsetTime = new Dictionary<string, float>();
        animationShotAnimSpeed = new Dictionary<string, float>();

        animationOffsetTime.Add("3D_shot_left_foot", 0.33f);
        animationOffsetTime.Add("3D_shot_right_foot", 0.33f);
        //animationOffsetTime.Add("3D_volley", 0.43f);

        animationOffsetTime.Add("3D_volley", 0.45f);

        if (photonView.IsMine)
        {
            animationShotAnimSpeed.Add("3d_normal_shot_speed", 0.8f);
            //animationShotAnimSpeed.Add("3d_volley_shot_speed", 0.6f);
            animationShotAnimSpeed.Add("3D_shot_left_foot", 0.8f);
            animationShotAnimSpeed.Add("3D_shot_right_foot", 0.8f);
            animationShotAnimSpeed.Add("3D_volley_before_speed", 0.8f);
        }
        else
        {
            animationShotAnimSpeed.Add("3d_normal_shot_speed", 0.9f);
            //animationShotAnimSpeed.Add("3d_volley_shot_speed", 0.9f);
            animationShotAnimSpeed.Add("3D_shot_left_foot", 0.9f);
            animationShotAnimSpeed.Add("3D_shot_right_foot", 0.9f);
            animationShotAnimSpeed.Add("3D_volley_before_speed", 0.9f);
        }

        animator.SetFloat("3d_normal_shot_speed", animationShotAnimSpeed["3d_normal_shot_speed"]);
        animator.SetFloat("3d_volley_shot_speed", animationShotAnimSpeed["3D_volley_before_speed"]);

        initPreShot = false;
        initPreShotRPC = false;
        initVolleyShot = false;


        initPreShot = false;
        initVolleyShot = false;

        rb = GetComponent<Rigidbody>();
        touchLine = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        playerPrevPosTime = Time.time;
        playerPrevPos = rb.transform.position;

        if (photonView.IsMine)
        {
            gkClickHelper = GameObject.Find("gkClickHelper").GetComponent<Image>();
            rectTransformGkClickHelper = gkClickHelper.GetComponent<RectTransform>();
        }

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
            timeFactor = 60.0f;
        }

        string timeOfGame = Regex.Replace(Globals.matchTime, "[^0-9]", "");
        timeOfGameInSec = float.Parse(timeOfGame) * timeFactor;

        stoppageTime = Globals.stoppageTime;

            //UnityEngine.Random.Range(4, 10);


        //timeOfGameInSec = 1000f;
        //print("TIMEOFGAME " + timeOfGameInSec);
        //animator = GameObject.Find("mainPlayer1").GetComponent<Animator>();
        animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;


        if (photonView.IsMine)
        {
            if (Globals.stadiumNumber == 0)
                m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                              5.5f,
                                                              m_MainCamera.transform.position.z);

            if (!PhotonNetwork.IsMasterClient)
            {
                m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                              m_MainCamera.transform.position.y,
                                                              -m_MainCamera.transform.position.z);
                m_MainCamera.transform.eulerAngles = new Vector3(m_MainCamera.transform.eulerAngles.x,
                                                                 m_MainCamera.transform.eulerAngles.y + 180f,
                                                                 m_MainCamera.transform.eulerAngles.z);
            }
        }

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
        JoystickButtonAnimNames = new List<string>();
        shotTypesNames = new List<string>();
        gkTimeToCorrectPos = 0.119f;

        initAnimationList();
        initRunAnimationList();
        initShotsList();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ///FindObjectOfType<AudioManager>();
        touchLocked = false;

        //int("audioManager " + audioManager);
        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
            ballRb[i].maxAngularVelocity = 1000.0f;

        cameraStartPos = m_MainCamera.transform.rotation;

        if (photonView.IsMine) {
            gkHelperImage.enabled = false;
            gkClickHelper.enabled = false;
        }
        prevZballPos = ballRb[activeBall].transform.position.z;
        ballPrevPosition = ballRb[activeBall].transform.position;

        //transform.parent = GameObject.Find("mainPlayer1").transform;

        //DrawLine(new Vector3(-22.0f, 0.01f, -1.0f), new Vector3(22.0f, 0.01f, -1.0f), Color.white, 300000.0f, 0.01f);
        /*POSSIBLE TO REMOVE*/

        //DRAW HELPER LINE//        
        //drawHelperGrid();

        //print("ISBONUSACTIVE " + isBonusActive);

        //if (isTrainingActive || isBonusActive)
        //{
        //    audioManager.Play("training1");
        //}
        //else
        //{

        if (Globals.PITCHTYPE.Equals("STREET"))
            audioManager.PlayNoCheck("training1");
        else
            audioManager.Play("fanschantBackground2", 0.3f);


        //if (Globals.stadiumNumber == 1)
        if (photonView.IsMine)
            audienceReactionsScript.playApplause1();
        ///}

        if (photonView.IsMine)
            deactivateCanvasElements();

        /*Draw who starts */
        int whoStarts = UnityEngine.Random.Range(0, 2);
        if (whoStarts == 0)
        {
            ballRb[activeBall].transform.position = new Vector3(0, 0, 2);
            ball[1].setwhoTouchBallLast(2);
        }
        else
        {
            ballRb[activeBall].transform.position = new Vector3(0, 0, -2);
            ball[1].setwhoTouchBallLast(1);
        }

        ///if (isTrainingActive || isBonusActive)
        ///{
        ///    gameStarted = true;
        ///    initCameraMatchStartPos();
        ///    activateCanvasElements();
        ///    ballRb[activeBall].transform.position = new Vector3(0, 0, -4);
        ///}

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rpc_rbPos = rb.transform.position;
        rpc_rbVelocity = Vector3.zero;
        rpc_runningSpeed = 0f;

        prevRbPos = rb.transform.position;
        matchSavePos = rb.transform.position;
        matchInitSavePos = false;
        //print("maxTimeToShotDBG2 START" + maxTimeToShot);


        if (photonView.IsMine &&
            !isMaster &&
            SIMULATE_SHOT)
        {
            shotActive = true;
        }        
        else
        {
            ballRb[activeBall].transform.position = new Vector3(0f, 0.3f, -2f);
            ballRb[activeBall].velocity = Vector3.zero;
            ballRb[activeBall].angularVelocity = Vector3.zero;
            preShotActive = false;
            setWallColliders(true);
            shotActive = false;
        }

        //TOREMOVE
        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;
        ballRb[activeBall].transform.position = new Vector3(0f, 0.3f, -2f);
        //ball[1].setwhoTouchBallLast(1);
        ballPrevPosition = ballRb[activeBall].transform.position;        
        rpc_ballPos = ballRb[activeBall].transform.position;
        rpc_ballVelocity = Vector3.zero;
        rpc_ballAngularVelocity = Vector3.zero;
        rpc_playerDirection = Vector3.zero;
        rpc_isShotActive = false;
        rpc_mainLag = 0f;

   
        /*Collider[] hitColliders = Physics.OverlapSphere(
                    new Vector3(-10f, 0f, -14f),
                    BALL_NEW_RADIUS,
                    Physics.AllLayers); ;

        if (PredictCollision.doesItCollideWithPlayer(hitColliders))
            print("#DBG collide " + hitColliders.Length);
        else
            print("#DBG collide " + hitColliders.Length);
        */


        //print("###BallPos " + ballRb[activeBall].transform.position);
        //if (photonView.IsMine)
        //    print("#DBGPLAYERPOS START " + rb.transform.position);

        /*if (photonView.IsMine) {
            if (isMaster)
            {
                rb.transform.position = new Vector3(0, 0, -10f);
            }
            else {
                rb.transform.position = new Vector3(0, 0, 10f);
            }
        }*/

        //print("cameraIntro pos " + m_MainCamera.transform.position +
        //    " rot " + m_MainCamera.transform.eulerAngles);


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


    //void OnDrawGizmos() {
    //        Gizmos.DrawSphere(new Vector3(0f, 0.5f, 0f), BALL_NEW_RADIUS);
    //}


    //int deltaIterator = 1;
    void Update()
    {

        matchTarget(animator,
            rb,
            ref gkStartPos,
            gkTimeToCorrectPos,
            stepSideAnimOffset,
            ref matchSavePos,
            ref matchInitSavePos,
            false);

        //if (photonView.IsMine)
        //{


        if (!photonView.IsMine ||
            //(Globals.peersReady < Globals.MAX_PLAYERS) ||
            (!arePeersPlayerSet() && !gameEnded) ||
             isGamePaused())
        {
             return;
        }


        //print("#DBGINTRO photonView update " + photonView + " photonView.IsMine " + photonView.IsMine
        //       + " isGamePaused " + isGamePaused()
        //       + " (Globals.peerPhotonView " + Globals.peersReady + " END ");
        //}
        updatePlayersTextures();

        //print("DEBUGCAMERA UPDATE DELTA TIME " + deltaIterator + " Time.deltaTime " + Time.deltaTime);

        //print("maxTimeToShotDBG2 FIXEDUPDATE" + maxTimeToShot);

        /*in traning mode start game immediately */
        //if (isTrainingActive)
        //    gameStarted = true;
        clearGeneralInformtionText();

        if (!isTrainingActive && !isBonusActive && !Globals.PITCHTYPE.Equals("STREET"))
            updateFlagsPositions();

        realTime += Time.deltaTime;
        if (gameEnded)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            rb.velocity = Vector3.zero;
            displayStatisticsPanel();
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

        if (photonView.IsMine)
        {
            //print("#DBGINTRO " + photonView.IsMine + " gameStarted " + gameStarted + " realTime " + realTime);
            if (!gameStarted)
            {
                //if (realTime > 1f)
                //{
                cameraMovementIntro();
                //}

                //print("#DBGINTRO inside " + photonView.IsMine + " gameStarted " + gameStarted);

                if (realTime > 11.5f)
                {
                    audioManager.PlayNoCheck("whislestart1");
                    int RandWhistleCom = UnityEngine.Random.Range(1, 3);
                    if (!Globals.commentatorStr.Equals("NO"))
                        audioManager.Play("com_firstwhistle" + RandWhistleCom.ToString());
                    gameStarted = true;
    

                    //print("DBGINTRO gameStarted ### " + gameStarted);
                    lastTimeBallWasOut = Time.time;
                }

                float matchIntroPanelFillAmount = (realTime - 1.0f) / 2.0f;
                //print("#DBGINTRO before matchIntroPanel " + photonView.IsMine + "gameStarted " + gameStarted +
                //    " realTime " + realTime);

                if (realTime >= 1.0f && realTime < 3.0f)
                {
                    int chantRandom = UnityEngine.Random.Range(3, 5);
                    if (!gameStartedInit)
                    {
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
                    //print("#DBGINTRO matchIntroPanel " + photonView.IsMine + "gameStarted " + gameStarted);
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
                    //rb.transform.position = new Vector3(0f, 0.03f, -12.5f);
                    animator.speed = 1f;
                }

                return;
            }
        }

        /*if updateGameTime is true - begining of match end */
        /*if (!isTrainingActive && !isBonusActive)
        {*/


            if (photonView.IsMine &&
                updateGameTime())
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
        //}*/

        timeLoops++;




        /*matchTarget(animator,
                    rb,
                    ref gkStartPos,
                    gkTimeToCorrectPos,
                    stepSideAnimOffset,
                    ref matchSavePos,
                    ref matchInitSavePos,
                    false);*/

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

        if ((isPlayerOnBall() || peerPlayer.getShotActive()) &&
            !isTouchPaused())
        {
            updateTouch();
        }

        clearTouch();

        /*        if (peerPlayer.getShotActive() && gkTouchDone == true && !shotActive && !preShotActive)
                {

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
                            ref initGKPreparation,
                            ref initAnimName,
                            ref levelDelay,
                            ref cpuGkAnimAdjustSpeed,
                            ref gkAction,
                            ref gkTimeLastCatch,
                            peerPlayer.isLobShotActive(),
                            ref stepSideAnimOffset,
                            ref gkLobPointReached,
                            ref gkRunPosReached,
                            ref initDistX,                    
                            peerPlayer.getShotVariant(),
                            peerPlayer.getOutShotStart(),
                            peerPlayer.getOutShotMid(),
                            peerPlayer.getOutShotEnd(),
                            peerPlayer.getEndPosOrg(),
                            peerPlayer.getTimeOfBallFly(),
                            peerPlayer.getPassedTime(),
                            ref gkLock,
                            ref rotatedRbToBall,
                            gkCornerPoints,
                            isExtraGoals);
                }*/
    }

    void FixedUpdate()
    {
        //if (photonView.IsMine)
        //    print("DBG342344COL counter isFixedUpdate " + isFixedUpdate);




        //print("#DBGLOG Globals.player1MainScript " + Globals.player1MainScript +
        //    " Globals.player2MainScript " + Globals.player2MainScript);

        ///if (!photonView.IsMine)
        //{
        //    print("#DBGLOG eulerAngles " + rb.transform.eulerAngles + " gameStared " + gameStarted);
        ///}


        //print("ActiveBall " + activeBall);
        //print("ballRb[activeBall]POSITION " + ballRb[activeBall].transform.position.ToString("F6"));

        //print("RBVEL " + rb.velocity);
        //if (isGamePaused())
        //    return;
        if ((!gameStarted ||
              gameEnded) && photonView.IsMine)
            RPC_setLastTimeUpdate();
        recoverAnimatorSpeed();

        if (!isPeerReady)
        {
            Globals.peersReady++;
            isPeerReady = true;
        }

        if ((Globals.peersReady < Globals.MAX_PLAYERS) ||
            isGamePaused())
        {

            return;
        }
        else
        {
            //peerPhotonView = Globals.peerPhotonView;
            if (photonView.IsMine)
                peerPlayer = Globals.player2MainScript;
            else
                peerPlayer = Globals.player1MainScript;
        }

        if (!arePeersPlayerSet() ||
            (photonView.IsMine && !gameStarted) ||
            (!photonView.IsMine && !peerPlayer.gameStarted))
             return;
    
        //print("#DBGPLAYER photonView "
        //    + photonView.IsMine + " peerPlayer " + peerPlayer + " Globals.peersReady " + Globals.peersReady
        //    + " Globals.player1MainScript " + Globals.player1MainScript
        //    + "  Globals.player2MainScript " + Globals.player2MainScript);

        isFixedUpdate++;

        //if (photonView.IsMine &&
        //    !isMaster)
        //{
        //    print("#DBGLOCALBALL " + InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position)
        //        + " rb.transform " + rb.transform 
        //        + " ballPos "  + ballRb[activeBall].transform.position);
        //}

        updatePlayerVelocity();
        if (!photonView.IsMine)
        {
            if (!peerPlayer.getGameEnded())
                RPCstateMachine();

            //print("DBGWALL #### ballPos " + ballRb[activeBall].transform.position + " ballVel "
            //    + ballRb[activeBall].velocity);

            return;
        }

        if (photonView.IsMine)
        {

            bool isOnBall = isPlayerOnBall(
                   rbLeftToeBase,
                   rbRightToeBase,
                   ballRb[activeBall],
                   rb,
                   "move",
                   ref activeBall,
                   isMaster);
            if (!isBallOut &&
                isOnBall &&
                !peerPlayer.getShotActive() &&
                !isAnimatorPlaying(animator))
            {
                playerOnBall = true;
                ball[1].setwhoTouchBallLast(1);
                peerPlayer.clearAfterBallCollision();
            }

            if (playerOnBall)
            {
                isUpdateBallPosActive = true;
                updateBallPosName = "bodyMain";
            }

            if (preShotActive ||
                shotActive ||
                isBallOut)
                playerOnBall = false;

            if (ball[activeBall].getBallCollided() &&
                !peerPlayer.getShotActive() &&
                lockCollision == false)
            {
                //print("#DBG342344COL getBallCollided ballPos " + ball[activeBall].transform.position);
                clearAfterBallCollision();
                isCollisionActive = true;
                ball[activeBall].setBallCollided(false);
            }
        }

        //print("isBallInGame " + isBallInGame + " isballOUt " + isBallOut);
        string name = nameAnimationPlaying(animator, 1.0f);
        bool isBackRunPlaying = isPlaying(animator, "3D_back_run_cpu", 1.0f);
        bool isRunPlaying = isPlaying(animator, "3D_run", 1.00f);
        /*if (!photonView.IsMine)
        {
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
                if (isRunPlaying)
                    name = "3D_back_run_cpu";

                if (shotActive || preShotActive)
                {
                    print("rpc_rbPredictedPos PLAYINGANIMATIONOW " + name + "  "
                        + animator.GetCurrentAnimatorStateInfo(0).IsName(name) + " ANIMNORMTIME "
                        + animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                        + " BALLPOS " + ballRb[activeBall].transform.position
                        + " RB POS " + rb.transform.position
                        + " preShotActive " + preShotActive
                        + " isShotActive " + shotActive);
                        //+ " cpuPlayer.getRbPosition() " + peerPlayer.getRbPosition());
                }
            }
        }*/

        //print("###BallPos fixedUpdate 1 " + ballRb[activeBall].transform.position + " photonView " + photonView.IsMine);


        //print("ENTERPOSITION CLEAN END OF FIXEDUPATE START RB POS " 
        //    + rb.transform.position + " isUpdateBallPosActive " + isUpdateBallPosActive);

        /*this should stay here because fixedUpdate might execute more than once?? */
        if (photonView.IsMine)
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
                                  isMaster);
        }

        ///rpc_UpdatePos();

        corectBallPositionOnBall(rb,
                                 animator,
                                 rbRightToeBase,
                                 rbRightFoot,
                                 ref isUpdateBallPosActive,
                                 updateBallPos,
                                 updateBallPosName,
                                 false);

        //print("###BallPos fixedUpdate 2 " + ballRb[activeBall].transform.position + " photonView " + photonView.IsMine);


        Vector2 ballPos = new Vector2(ballRb[activeBall].transform.position.x, ballRb[activeBall].transform.position.z);
        //Vector2 cpuPos = new Vector2(peerPlayer.getRbPosition().x,
        //                             peerPlayer.getRbPosition().z);

        if (!peerPlayer.getShotActive())
            gkLock = false;

        if (!gameStarted)
        {
            return;
        }

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

        if (photonView.IsMine)
        {
            if (!gameEnded)
                //|| (gameEnded && !gameEndedAnimations))
                /*TODELETE*/
                if (!gkLock)
                    playerRun();
        }

        if (gameEnded && photonView.IsMine)
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
                             isMaster);

            return;
        }

        if (photonView.IsMine)
        {
            if ((!isTrainingActive && !isBonusActive) &&
                !timeToShotExceeded)
                playMissGoalSound();
        }

        if (photonView.IsMine)
        {
            if (!peerPlayer.getShotActive())
            {
                gkNotClickedLocked = false;
                gkMovesActive = false;
            }

            //if (fixedUpdateCount == 1 &&
            if (!gkNotClickedLocked &&
                !gkMovesActive &&
                peerPlayer.getShotActive() &&
                IsItTooLateToGkDive(peerPlayer.prepareShotPos, 
                                    peerPlayer.prepareShotMaxIdx))
            {

                //GameObject rbTmpGameObj = new GameObject();
                //Rigidbody rbTmp = rbTmpGameObj.AddComponent<Rigidbody>();
                //GameObject rotatedRbToBallTmp = new GameObject();
                Vector3 localSpaceHit = Vector3.zero;
                float timeToHitZ = -1f;
                curvePercentHit = -1f;
                copyRbTransform(ref rbTmp, rb.transform);
                copyRbTransform(ref rotatedRbToBallTmp, rb.transform);
                rbTmp.velocity = Vector3.zero;

                gkNotClickedLocked = true;
                gkHitPointCalc(rbTmp,
                               peerPlayer.getShotVariant(),
                               peerPlayer.getOutShotStart(),
                               peerPlayer.getOutShotMid(),
                               peerPlayer.getOutShotEnd(),
                               peerPlayer.getEndPosOrg(),
                               peerPlayer.getTimeOfBallFly(),
                               peerPlayer.passedShotFlyTime,
                               rotatedRbToBallTmp,
                               ref localSpaceHit,
                               ref timeToHitZ,
                               ref curvePercentHit,
                               peerPlayer.prepareShotPos,
                               peerPlayer.prepareShotMaxIdx);

                /*print("DBGCOLLISIONCALC1024DB CCCC SEND #### notClicked!! ballPos" + ballRb[activeBall].transform.position
                    + " curvePercentHit " + curvePercentHit
                    + " localSpaceHit " + localSpaceHit
                    + " rbTmp " + rbTmp.transform.position
                    + " rb.transform " + rb.transform.position);*/
                predictGkCollisionActive = true;
                predictedAnimName = "EMPTY";
                gkStartSeq = -1;
                //print("DBG342344COL NOTCLICKED PredictCollision.getOnAnimatorAcitve() " + PredictCollision.getOnAnimatorAcitve());


                //StartCoroutine(predictGkCollision(
                /* predictGkCollision(
                               peerPlayer.getOutShotStart(),
                               peerPlayer.getOutShotMid(),
                               peerPlayer.getOutShotEnd(),
                               rbTmp,
                               rotatedRbToBallTmp,
                               "EMTPY",
                               "EMPTY",
                               //("3D_GK_sidecatch_" + animName),
                               -1f,
                               peerPlayer.getTimeOfBallFly(),
                               curvePercentHit,
                               -1f,
                               -1f,
                               Vector3.zero);*/
                //pretend that something has been touch
                //touchCount = 2;
            }
            else
            {
                if (
                    peerPlayer.getShotActive() &&
                    (gkTouchDone == true || GK_DEBUG == true) &&
                    !shotActive &&
                    !preShotActive &&
                    !gkNotClickedLocked)
                {
                    if (GK_DEBUG)
                        gkTouchDone = true;

                    gkMovesActive = true;
                    //print("DBGCOLLISIONCALC1024DB CCCC SEND #### CLICKED ### ballInit " +
                    //    peerPlayer.getBallInit());
                    //print("RPCEXECUTED gk in update exec  uted IsMine " + photonView.IsMine);
                    if (!rpc_gkMovementSend)
                    {

                        /*BUG SOLVED RACE Condition beetween fixed update and update*/
                        getRotatedRbToBall(peerPlayer.prepareShotPos[0,0],
                                           //peerPlayer.getBallInit(),
                                           getPlayerRb(),
                                           ref getRotatedRbToBallRef(),
                                           getGkCornerPoints());


                        /*print("GKDEBUG1X ROTATION SENT "
                            + rotatedRbToBall.transform.eulerAngles
                            + " ROTPOS " + rotatedRbToBall.transform.position);*/

                        /*photonView.RPC("RPC_gkMovement",
                                        RpcTarget.Others,
                                        rb.transform.position,
                                        rb.transform.rotation,
                                        rotatedRbToBall.transform.position,
                                        rotatedRbToBall.transform.rotation,
                                        peerPlayer.getPassedTime(),
                                        gkTouchPosRotatedRbWS);
                        PhotonNetwork.SendAllOutgoingCommands();
                        rpc_gkMovementSend = true;*/
                    }

                    /* if (!GK_DEBUG_INIT)
                     {
                         getRotatedRbToBall(peerPlayer.getBallInit(),
                                        getPlayerRb(),
                                        ref rotatedRbToBall,
                                        getGkCornerPoints());
                         rpc_gkMovementSend = true;

                         Vector3 realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                                1.0f,
                                                                rotatedRbToBall,
                                                                peerPlayer.getOutShotStart(),
                                                                peerPlayer.getOutShotMid(),
                                                                peerPlayer.getOutShotEnd(),
                                                                true);

                         realHitPlaceLocal.z = 0f;
                         gkTouchPosRotatedRbWS = TransformPointUnscaled(rotatedRbToBall.transform, realHitPlaceLocal);
                         print("#DBGK1024_SIMULATE_DBG2435_CREATE gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
                             + " realHitPlaceLocal " + realHitPlaceLocal);
                         GK_DEBUG_INIT = true;
                     }*/

                    //print("DBGCOLLISIONCALC CALC GK #### gkTouchPosRotatedRbWS "
                    //    + gkTouchPosRotatedRbWS + " gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
                    //    + " gkTouchPosRbWS " + gkTouchPosRbWS);

                    gkMoves(animator,
                            rb,
                            false,
                            ref lastGkAnimName,
                            ref lastTimeGkAnimPlayed,
                            ref lastGkDistX,
                            ref gkStartPos,
                            ref gkStartTransform,
                            ref gkTimeToCorrectPos,
                            ref gkSideAnimPlayOffset,
                            ref initCpuAdjustAnimSpeed,
                            ref initGkDeleyLevel,
                            ref initGKPreparation,
                            ref initAnimName,
                            ref levelDelay,
                            ref cpuGkAnimAdjustSpeed,
                            ref gkAction,
                            ref gkTimeLastCatch,
                            peerPlayer.isLobShotActive(),
                            ref stepSideAnimOffset,
                            ref gkLobPointReached,
                            ref gkRunPosReached,
                            ref initDistX,
                            peerPlayer.getShotVariant(),
                            peerPlayer.getOutShotStart(),
                            peerPlayer.getOutShotMid(),
                            peerPlayer.getOutShotEnd(),
                            peerPlayer.getEndPosOrg(),
                            peerPlayer.getTimeOfBallFly(),
                            peerPlayer.getPassedTime(),
                            ref gkLock,
                            ref rotatedRbToBall,
                            gkCornerPoints,
                            isExtraGoals,
                            peerPlayer.prepareShotPos,
                            peerPlayer.prepareShotMaxIdx);
                }
            }
        }

        if (photonView.IsMine && predictGkCollisionActive)
        {

            predictGkCollision(
                                peerPlayer.getOutShotStart(),
                                peerPlayer.getOutShotMid(),
                                peerPlayer.getOutShotEnd(),
                                //rbTmp,
                                rb,
                                rotatedRbToBallTmp,
                                gkOperations,
                                predictedAnimName,
                                gkTimeToCorrectPos,
                                gkSideAnimPlayOffset,
                                peerPlayer.getTimeOfBallFly(),
                                ref curvePercentHit,
                                lastGkDistX,
                                gkStartSeq,
                                ref gkStartPos);
            /*print("DBG342344COL ANALYZE rbTmp " + rbTmp.transform.position + " predictedAnimName " + predictedAnimName
                + "rbTmp.transform.rotation " + rbTmp.transform.eulerAngles
                + " rbTmp " + rbTmp.velocity

                + " gkOPperations " + gkOperations
                + " gkStartSeq " + gkStartSeq
                + " gkStartPos " + gkStartPos
                + " outShotStart" + outShotStart
                + " outShotMid " + outShotMid
                + " outShotEnd " + outShotEnd
                + " timeofBallFly " + timeofBallFly
                + " curvePercentHit " + curvePercentHit
                + " gkTimeToCorrectPos " + gkTimeToCorrectPos
                );*/
        }

        ////print("###BallPos fixedUpdate 3 " + ballRb[activeBall].transform.position + " photonView " + photonView.IsMine);


        if (photonView.IsMine)
        {
            timeToShotExceeded = updateTimeToShot(ref prevZballPos, ref timeToShot);
            if (!isMaster)
                timeToShotExceeded = false;
            ///print("timeToShotExceeded " + timeToShotExceeded);


            if (isMaster)
            {
                if (timeToShotExceeded ||
                    isBallOutOfPitch() ||
                    ///ball[activeBall].getBallGoalCollisionStatus() ||
                    isGoalJustScored)
                {

                    //print("DBGCOLLISIONCALC1024D isBAllOut " + isBallOut);
                    //print("DBGCOLLISIONCALC1024D ballOutOfPitch");
                    if (!isBallOut)
                    {
                        //ballPosAfterOut = getBallPosAfterOut(timeToShotExceeded, isGoalJustScored);
                        photonView.RPC("RPC_ballOut",
                                        RpcTarget.Others,
                                        ballPosAfterOut,
                                        timeToShotExceeded,
                                        isGoalJustScored,
                                        Globals.score2);
                        StartCoroutine(
                            sendAndACKBallOut((int) RPC_ACK.BALL_IS_OUT,
                                                    ballPosAfterOut,
                                                    timeToShotExceeded,
                                                    isGoalJustScored,
                                                    Globals.score2));
                    }
                    isBallOut = true;
                }
            }

            if (preShotActive ||
                shotActive)
                mainUpdateType = "S";

            if (playerOnBall)
                mainUpdateType = "O";

            if (!isBallOnYourHalf(rb.transform.position))
                mainUpdateType = "A";

            if (isMaster && isBallOut)
                mainUpdateType = "P";

            //if (mainUpdateType.Equals("B"))
            // {
            //     print("BUFFEREDPOSITION send ball pos " + ballRb[activeBall].transform.position
            //         + " mainUpdatePacketId " + mainUpdatePacketId
            //         + " vel " + ballRb[activeBall].velocity 
            //         );
            // }

            photonView.RPC("RPC_mainUpdate",
                            RpcTarget.Others,
                            rb.transform.position,
                            rb.velocity,
                            rb.angularVelocity,
                            rb.rotation,
                            runningSpeed,
                            playerDirection,
                            ballRb[activeBall].transform.position,
                            ballRb[activeBall].velocity,
                            ballRb[activeBall].angularVelocity,
                            ballRb[activeBall].rotation,
                            shotActive,
                            isCollisionActive,
                            mainUpdatePacketId++,
                            RPC_sequenceNumber[(int) RPC_ACK.BALL_POS_UPDATE],
                            mainUpdateType,                    
                            checkIfJoystickExtraButtonsPlaying(animator, 1f),
                            isBallOut,
                            playerOnBall);

            PhotonNetwork.SendAllOutgoingCommands();

            //print("DBGPLA sent request ballRb "  + ballRb.transform.position);
        }

        /*Second condition will help when you shot and ball will not cross half*/

        /* for (int i = 1; i <= NUMBER_OF_BALLS; i++)
         {

             if (ball[i].getBallCollided() ||
                 (shotActive &&
                 (Mathf.Abs(ballRb[activeBall].transform.position.y) < 0.45f) &&
                 (Mathf.Abs(ballRb[activeBall].velocity.x) < 0.05f) &&
                 (Mathf.Abs(ballRb[activeBall].velocity.y) < 0.05f) &&
                 (Mathf.Abs(ballRb[activeBall].velocity.z) < 0.05f)))
             {

                 clearAfterBallCollision();
                 ball[i].setBallCollided(false);

                 //print("GKDEBUG1TEST1 SHOTACTIVE " + shotActive + " PRESHOT " + preShotActive);

                 if (ball[i].getBallCpuPlayerStatus() == false)
                 {
                     peerPlayer.setShotActive(false);
                 }
                 else
                 {
                     ball[i].setBallCpuPlayerCollision(false);
                 }
             }
         }*/

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY BEFORE CPU FIXED UPDATE " + ballRb[activeBall].velocity + " SHOT " + shotActive);

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER CPU FIXED UPDATE " + ballRb[activeBall].velocity + " SHOT " + shotActive);

        //prepareFrameClean();

        //print("UNLOCK TOUCH COUNT " + touchCount);

        //if (!isBonusActive)

        //if (!isBonusActive &&
        //     (isBallOutOfPitch() ||
        //      ball[1].getBallGoalCollisionStatus() ||
        //      (timeToShotExceeded && !isTrainingActive)))
        //      isBallOut = true;

        /*print("Executed setBallPositionFlash AFTER DELAY IS BALL OUT " 
            + isBallOutOfPitch() + " BALLCOLLISION " + ball.getBallGoalCollisionStatus());*/
        if (photonView.IsMine) {

            /*if (peerPlayer.getShotActive())
            {
                print("DBG342344COL shotACtive### " +
                    " gkLastCollisionVel " + gkLastCollisionVel
                    + " gkLastCollisionAngVel " + gkLastCollisionAngVel
                    + " ballRb.transform.position " + ballRb[activeBall].transform.position
                    + " gkPercentCollisionTime " + gkPercentCollisionTime
                    + " (Time.time - gkCollisionPackageArriveTime " + (Time.time - gkCollisionPackageArriveTime)
                    + " peerPlayer.getShotPercent() " + peerPlayer.getShotPercent());
            }*/

            /*print("#DBGSHOT333 peerPlayer.getShotActive() " + peerPlayer.getShotActive()
                + " isCollisionWithPlayer " + isCollisionWithPlayer
                + " ball[activeBall].getIsPlayerDownCollided() " + ball[activeBall].getIsPlayerDownCollided()
                + "  (!Globals.hasTheSameSign(rb.transform.position.z, ballRb[activeBall].transform.position.z)) "
                + (!Globals.hasTheSameSign(rb.transform.position.z, ballRb[activeBall].transform.position.z))
                + " ballRb.transform.position  " + ballRb[activeBall].transform.position
                + " rb.transform.position " + rb.transform.position);*/


            if (peerPlayer.getShotActive() &&
                //isCollisionWithPlayer &&
                ball[activeBall].getIsPlayerDownCollided() &&
                //(prepareShotPosIdx < 2))
                (!Globals.hasTheSameSign(rb.transform.position.z, ballRb[activeBall].transform.position.z) ||
                  (prepareShotPosIdx <= 1)))
            {
                ball[activeBall].setIsPlayerDownCollided(false);
                //print("DBGSHOT ballCollidedCOLISSSION DBG342344COL DBG342344COL DBGSHOT333 APPLY ISMINE LATEUPADTE set setIsPlayerDownCollided falsE "
                //    + " ballPos " + ballRb[activeBall].transform.position);
            }

            if (peerPlayer.getShotActive() &&
               (gkLastCollisionVel != INCORRECT_VECTOR) &&
               (gkPercentCollisionTime != -1) &&
               (
                (peerPlayer.getShotPercent() >= gkPercentCollisionTime) ||                
                (isCollisionWithPlayer &&
                 (ball[activeBall].getIsPlayerDownCollided() ||
                  Mathf.Abs(InverseTransformPointUnscaled(rb.transform, 
                  ballRb[activeBall].transform.position).z) < 0.5f)) 
                 ||
                 (!isCollisionWithPlayer &&
                  ball[activeBall].getIsWallCollided())
                  //||
                  //(Mathf.Abs(ballRb[activeBall].transform.position.z) >= PITCH_HEIGHT_HALF - 0.6f)
                  )                                                                                      
                  )
                //&&
                //((Time.time - gkCollisionPackageArriveTime) < 2.0f))
            /*if (peerPlayer.getShotActive() &&
           (gkLastCollisionVel != INCORRECT_VECTOR) &&
           (peerPlayer.getShotPercent() >= gkPercentCollisionTime) &&
           ((Time.time - gkCollisionPackageArriveTime) < 2.0f))*/
            {

                /*print("DBGWALL ballCollidedCOLISSSION DBG342344COL APPLY ISMINE LATEUPADTE gkLastCollisionVel " + gkLastCollisionVel
                    + " gkLastCollisionAngVel " + gkLastCollisionAngVel
                    + " ballRb.transform.position " + ballRb[activeBall].transform.position
                    + " gkPercentCollisionTime " + gkPercentCollisionTime
                    + " peerPlayer.getShotPercent() " + peerPlayer.getShotPercent()
                    + " (Time.time - gkCollisionPackageArriveTime " + (Time.time - gkCollisionPackageArriveTime)
                    + " Mathf.Abs(InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position).z) " +
                    Mathf.Abs(InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position).z) +
                     " ball[activeBall].getIsPlayerDownCollided() " +
                     ball[activeBall].getIsPlayerDownCollided() 
                     + " isCollisionWithPlayer " +
                     isCollisionWithPlayer
                     + " ballPosWhenCollisin " + ballPosWhenCollision
                     + " ball[activeBall].getIsWallCollided() " +
                     ball[activeBall].getIsWallCollided());*/

                if (isCollisionWithPlayer)
                {
                    //only applicable for master
                    ball[1].setwhoTouchBallLast(1);
                    float distX = getGkLastDistXCord();
                    if (isMaster)
                        ball[activeBall].setPlayerDownLastGkCollision(Time.time);
                    else
                        ball[activeBall].setPlayerUpLastGkCollision(Time.time);

                    audioManager.PlayNoCheck("gksave1");      
                    audioManager.Commentator_PlayRandomSave(getGkLastDistXCord());
                    if (distX > 4.0f)
                    {
                        if (!audioManager.isPlayingByName("crowdOvation1Short") &&
                            !isTrainingActive &&
                            !isBonusActive)
                        {
                            audioManager.Play("crowdOvation1Short");
                        }
                    }
                }

                if (!isCollisionWithPlayer)
                {
                    //ballRb[activeBall].transform.position = ballPosWhenCollision;
                    isLateUpdateBallPos = true;
                }

                /*if (!isCollisionWithPlayer)
                {
                    if (ballRb[activeBall].transform.position.z > 0)
                    {
                        ballRb[activeBall].transform.position =
                            new Vector3(ballRb[activeBall].transform.position.x,
                            ballRb[activeBall].transform.position.y,
                            PITCH_HEIGHT_HALF - BALL_NEW_RADIUS);
                    }
                    else
                    {
                        ballRb[activeBall].transform.position =
                           new Vector3(ballRb[activeBall].transform.position.x,
                           ballRb[activeBall].transform.position.y,
                           -(PITCH_HEIGHT_HALF - BALL_NEW_RADIUS));
                    }
                }*/

                mainUpdateType = "B";
                peerPlayer.setShotActive(false);
                ball[activeBall].setIsPlayerDownCollided(false);
                setPlayersColliders(gameObject, false);
                peerPlayer.RPCclearPreShotVariables();
                clearAfterBallCollision();
                isCollisionWithPlayer = false;
                //print("DBG342344COL try to set setGkHelperImageVal");
                setGkHelperImageVal(false);
                lockCollision = false;
                setShotSaveStatistics("teamA");
                lastTimeSaveAupdate = Time.time;
                /*print("DBGPREDICTCOLLISON APPLY PREDICTION " + gkLastCollisionVel + " gkPercentCollisionTime " +
                    gkPercentCollisionTime + " peerPlayer.getShotPercent() " + peerPlayer.getShotPercent());*/


                if (gkLastCollisionVel != Globals.INCORRECT_VECTOR_2)
                {
                    ballRb[activeBall].velocity = gkLastCollisionVel;
                    ballRb[activeBall].angularVelocity = gkLastCollisionAngVel;
                }
                else
                {
                    ///print("DBGCOLLISIONCALC1024D Globals.INCORRECT_VECTOR_2 "
                    // + gkLastCollisionVel + " ballVel " + ballRb[activeBall].velocity);
                }

                //print("DBGWALL APPLY VEL " + ballRb[activeBall].velocity + " " +
                //   " ballRb[activeBall].angularVelocity " + ballRb[activeBall].angularVelocity
                //    + " BALLpOS " + ballRb[activeBall].transform.position);

                isLateUpdateBallVelocity = true;
                isLateUpdateBallPos = true;
                lateUpdateBallVelocity = gkLastCollisionVel;
                lateUpdateBallPos = ballRb[activeBall].transform.position;
                lateUpdateBallRot = ballRb[activeBall].rotation;

                gkLastCollisionVel = INCORRECT_VECTOR;
            }

            if (
                peerPlayer.getShotActive() &&
                (gkLastCollisionVel == INCORRECT_VECTOR) &&
                (peerPlayer.getShotPercent() >= 1f))
            {
                gkPercentCollisionTime = -1f;

                //it's a goal
               // if (gkLastCollisionVel == Globals.INCORRECT_VECTOR_2)
               // {
               //     setIsGoalJustScored(true);           

                mainUpdateType = "B";

                peerPlayer.setShotActive(false);
                setPlayersColliders(gameObject, false);
                //TOCHECK
                peerPlayer.RPCclearPreShotVariables();
                clearAfterBallCollision();
                //print("DBGSHOT ballCollidedCOLISSSION SHOTACTIVEdbg ballRb velocity apply try to set setGkHelperImageVal");
                setGkHelperImageVal(false);

                rpc_playerOnBallActive = false;
                rpc_shotActive = false;            
            }
        }

        if (photonView.IsMine)
        {
            if (isMaster) 
                RPC_afterGoalLag = 0f;

            if (isBallOut)
            {
               mainUpdateType = "P";

               if (delayAfterGoal <= (RPC_delayAfterGoal - RPC_afterGoalLag))
                //if (delayAfterGoal <= 2.5f)
               {
                    if (peerPlayer.isWaitGoalActive)
                        Globals.score1 = peerPlayer.goalNewScore;

                    delayAfterGoal += Time.fixedDeltaTime;
                    if (!initDisplayEventInfo && (delayAfterGoal >= 0.400f))
                    {
                        if (isMaster) {
                            ballPosAfterOut = getBallPosAfterOut(timeToShotExceeded, isGoalJustScored);
                            photonView.RPC("RPC_ballOutNewPos",
                                            RpcTarget.Others,
                                            ballPosAfterOut);
                                        
                            StartCoroutine(
                                sendAndACKBallOutNewPos((int)RPC_ACK.BALL_IS_OUT_NEW_POS,
                                                         ballPosAfterOut));
                                                       
                        }
                        displayEventInfo();
                        initDisplayEventInfo = true;
                    }

                    playerOnBall = false;
                    peerPlayer.playerOnBall = false;
                    isBallInGame = false;
                    goalJustScored = false;
                    onBall = PlayerOnBall.NEUTRAL;
                    setScoresText();
                    setGkHelperImageVal(false);
                    enableShotButtons();
                    isUpdateBallPosActive = false;
                    //preShotActive = false;
                    //shotActive = false;
                    setIsGoalJustScored(false);

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
                                //TOcheck
                                //RblookAtWSPoint(rb, endPosOrg);
                            }
                            else
                            {
                                //RblookAtWSPoint(rb, new Vector3(0f, 0f, PITCH_HEIGHT_HALF));
                                if (isMaster)
                                    RblookAtWSPoint(rb, new Vector3(0f, 0f, PITCH_HEIGHT_HALF));
                                else
                                    RblookAtWSPoint(rb, new Vector3(0f, 0f, -PITCH_HEIGHT_HALF));
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
                               isMaster);

                    //playerOutOfPitch(animator, rb, ref prevRbPos, rbLeftToeBase, rbRightToeBase, isPlayerOnBall(), false);
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
                        timeToShot += RPC_afterGoalLag;
                            //0.0f;
                    }
                  
                    if (!ballPositionLock)
                            StartCoroutine(setBallPositionFlash(0.01f));

                    preShotActive = false;
                    shotActive = false;
                    setWallColliders(true);

                    peerPlayer.setShotActive(false);
                    setPlayersColliders(gameObject, false);
                    peerPlayer.RPCclearPreShotVariables();
                    clearAfterBallCollision();
                    setGkHelperImageVal(false);
                    setIsGoalJustScored(false);

                    initDisplayEventInfo = false;
                }

                return;
            }
        }

        if (!photonView.IsMine)
            return;

        updateBallPossessionMatchStatistics();

        //print("Executed setBallPositionFlash AFTER");
        float distance = Vector3.Distance(rb.transform.position,
                                         (ballRb[activeBall].transform.position));

        //print("GKDEBUG1 DISTANCEHERE " + distance + " preShotActive " + preShotActive + " ShoTACTIVE " + shotActive + 
        //    " RBTRANSFOMR " + rb.transform.position);

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (!preShotActive && !shotActive && !isAnyAnimationPlaying && photonView.IsMine)
        {
            //if (distance < minDistToBallShot && !cpuPlayer.getShotActive() && !shotActive && !preShotActive)

            bool isOnBall =
                isPlayerOnBall(rbLeftToeBase, rbRightToeBase, ballRb[activeBall], rb, "move", ref activeBall, isMaster);

            //print("ISONBALL " + isOnBall + " activeBall " + activeBall);

            //print("CPUMOVEDEBUG123X_NOCPU PLAYERONBALL " + isOnBall);
            if (isOnBall &&
               !peerPlayer.getShotActive())
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
                    rpc_gkMovementSend = false;
                    initGkMoves = false;

                    StartCoroutine(prepShotDelay(prepareShotDelay));
                    StartCoroutine(prepShotDelay2(prepareShotDelay2));

                    /*TOCHECK*/
                    isUpdateBallPosActive = false;

                    //print("GKDEBUG1TEST1 PRESHOT REACXTIVE " + preShotActive + " SHOT " 
                    //    + shotActive + " isAnyAnimationPlaying " + isAnyAnimationPlaying);
                    // print("GKDEBUG1 REFRESH!!");

                }

                onBall = PlayerOnBall.ONBALL;
                gkTouchDone = false;
                rpc_gkMovementSend = false;
                initGkMoves = false;
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

            if (!isOnBall && !peerPlayer.getShotActive())
            {
                //print("ONBALL NEUTRAL ");
                onBall = PlayerOnBall.NEUTRAL;
            }
        }

        //print("GKDEBUG7 ballRb[activeBall]VELOCITY VELOCITY AFTER CPU TOUCH " + ballRb[activeBall].velocity + " SHOTACTIVE " + shotActive);

        if (preShotActive && !shotActive && photonView.IsMine && !SIMULATE_SHOT)
        {
            shotActive = prepareShot(animator,
                                     ref shotType,
                                     rb,
                                     ballRb[activeBall],
                                     rbRightFoot,
                                     rbRightToeBase,
                                     rbLeftToeBase,
                                     //goalUpPlane,
                                     ref initPreShot,
                                     ref initPreShotRPC,
                                     ref initVolleyShot,
                                     //endPos,
                                     //endPosOrg,
                                     ref isUpdateBallPosActive,
                                     ref updateBallPos,
                                     ref updateBallPosName,
                                     ref shotRotationDelay,
                                     isMaster,
                                     isPrepareShotDelay,
                                     isPrepareShotDelay2);

            //if (shotActive == true)
            //{
            //    if (isShotOnTarget(endPosOrg, goalSizes))
            //    {
            //        matchStatistics.setShotOnTarget("teamA");
            //    }
            //}
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
        if (!isPrepareShotDelay) {
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
                         (peerPlayer.getShotActive() || (isMaster && ballRb[activeBall].transform.position.z > 0.0f) ||
                         (!isMaster && ballRb[activeBall].transform.position.z <= 0.0f)))
                    //(peerPlayer.getShotActive() || ballRb[activeBall].transform.position.z > 0.0f))
                    {
                        Vector3 lookPoint =
                                calcGkCorrectRotationToBall(
                                    peerPlayer.getBallInit(),
                                    rb,
                                    ref rotatedRbToBall,
                                    gkCornerPoints);

                        if (peerPlayer.getShotActive())
                        {
                            RblookAtWSPoint(rb, lookPoint);
                            //print("#DBGLOOKPOINT 1 gameStarted " + doesGameStarted());
                        }
                        else
                        {
                            //if (ballRb[activeBall].transform.position.z > 0.0f)
                            if ((isMaster && (ballRb[activeBall].transform.position.z > 0.0f)) ||
                                (!isMaster && (ballRb[activeBall].transform.position.z <= 0.0f)))
                            {
                                lookPoint =
                                calcGkCorrectRotationToBall(
                                    ball[1].transform.position,
                                    rb,
                                    ref rotatedRbToBall,
                                    gkCornerPoints);
                                RblookAtWSPoint(rb, lookPoint);
                                //print("#DBGLOOKPOINT 2 gameStarted " + doesGameStarted() + " ball[1].transform.position, " +
                                //    ball[1].transform.position);
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
                            //if (!bonusScript.isRotationLocked())
                            //{

                            if (isAnimatorPlaying(animator) &&
                                animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= shotRotationDelay)
                            {
                                Vector3 shotEndPosOrg = outShotEnd;
                                float lookSide = 0f;
                                if (!isMaster)
                                    lookSide = getGoalSizePlr1().x;
                                else
                                    lookSide = getGoalSizePlr2().x;

                                if (endPosOrg.x > 0)
                                    lookSide = -lookSide;

                                if (!isMaster)
                                    shotEndPosOrg = new Vector3(lookSide, 0f, -PITCH_HEIGHT_HALF);
                                else
                                    shotEndPosOrg = new Vector3(lookSide, 0f, PITCH_HEIGHT_HALF);
                                shotDirection3D = (shotEndPosOrg - rb.transform.position).normalized;
                                shotDirection3D.y = 0.0f;
                                lookOnLook = Quaternion.LookRotation(shotDirection3D);
                                rb.transform.rotation =
                                    Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);


                              //  print("#DBGLOOKPOINT 3 gameStarted " + doesGameStarted() 
                              //      + " ball[1].transform.position, " +
                              //      shotDirection3D);

                            }
                            else
                            {
                                RblookAt(rb,
                                         onBall,
                                         playerDirection,
                                         animator,
                                         preShotActive | shotActive,
                                         Vector3.zero,
                                         isMaster,
                                         shotType);

                                //print("#DBGLOOKPOINT 4 gameStarted " + doesGameStarted() + " playerDirection " +
                                //    playerDirection);

                            }
                            //}
                            //}
                        }
                    }
                }
            }
        }

        if (shotActive && photonView.IsMine && !SIMULATE_SHOT)
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
            /*if (!initShot)
            {
                float height = drawDistance / 80.0f;
                ballInitPos = ballRb[activeBall].transform.position;
                initShot = true;
                float deltaTime = drawTimeEnd - drawTimeStart;
                timeofBallFly = (drawDistance / deltaTime) / 0.85f;         
                timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
                timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);
                if (isLobActive)
                {
                    timeofBallFly = UnityEngine.Random.Range(550.0f, 630.0f);
                }
                shotSpeed = MIN_SHOT_SPEED +
                    Mathf.InverseLerp(ShotSpeedMin, ShotSpeedMax, timeofBallFly) * speedMultiplayer;               
                passedShotFlyTime = 0.0f;               
                matchStatistics.setShot("teamA");
                timeofBallFly = ShotSpeedMin + (ShotSpeedMax - timeofBallFly);                
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
                float extraTimeOfBallFly =
                    Mathf.Lerp(100f, 0, Mathf.InverseLerp(MIN_SHOT_SPEED, MAX_SHOT_SPEED, shotSpeed));
                timeofBallFly = timeofBallFly + (timeofBallFly * extraTimeOfBallFly) / (ShotSpeedMin - 20f);
                timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
                timeofBallFly = Mathf.Max(ShotSpeedMin - 20f, timeofBallFly);
                float speedPerc = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly);
                setBallShotVel(speedPerc * MAX_SHOT_SPEED_UNITY_UNITS);
              
            }*/

            //print("POSITIONVECTOR" + startPos + " MID " + midPos + " END " + endPos);

            /*TOCHECK*/
            //print("GKDEBUG7 GKDEBUG5 SHOTRET BEFORE " + shotRet);
            if (!shotRet)
            {
                //float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);
                //curveShotFlyPercent = normalizeTime;
                //shotPercent = normalizeTime;

                //print("factorTimeFlyNorm " + timeofBallFly + " TIME " + passedShotFlyTime + " NORMALIZE TIME " + normalizeTime);
                //print("NORMALIZE TIME " + normalizeTime);
                //shotRet = shot3New(outShotStart,
                //                   outShotMid,
                //                   outShotEnd,
                //                   outShotBallVelocity,
                //                   ref lastBallVelocity,
                //                   shotvariant,
                //                   normalizeTime);

                float ballVelocity = 0f;
                /*if (prepareShotPosIdx > 0) {
                    ballVelocity = Vector3.Distance
                                     (prepareShotPos[prepareShotPosIdx, 0],
                                      prepareShotPos[prepareShotPosIdx - 1, 0]) / Time.deltaTime;
                    ballRb[activeBall].velocity =
                    (prepareShotPos[prepareShotPosIdx - 1, 0] - prepareShotPos[prepareShotPosIdx, 0]).normalized * ballVelocity;
                    updateBallDuringShot = true;
                }*/

                ballRb[activeBall].transform.position =
                    prepareShotPos[prepareShotPosIdx, 0];
                ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
                ballRb[activeBall].velocity = Vector3.zero;

                curveShotFlyPercent = shotPercent =
                    prepareShotPos[prepareShotPosIdx, 1].x;

                ////print("DBGSHOT prepareShotPosIdx " + prepareShotPosIdx + " ballPos " 
               ////     + ballRb[activeBall].transform.position);
                prepareShotPosIdx++;

                updateBallDuringShotPos = ballRb[activeBall].transform.position;

                if (prepareShotPosIdx == prepareShotMaxIdx)
                {
                    //print("SHOTACTIVEdbg end ### " + shotRet);
                    shotRet = true;
                    ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
                    float velocity =
                        Vector3.Distance(prepareShotPos[prepareShotMaxIdx - 1, 0],
                                         prepareShotPos[prepareShotMaxIdx - 2, 0]) / Time.deltaTime;
                    ballRb[activeBall].velocity =
                (prepareShotPos[prepareShotMaxIdx - 1, 0] - prepareShotPos[prepareShotMaxIdx - 2, 0]).normalized * velocity;

                    isUpdateBallVelDuringShot = true;
                    updateBallDuringShot = false;
                    updateBallDuringShotVel = ballRb[activeBall].velocity;
                    //print("#DBGSHOT updateBallDuringShotVel " + updateBallDuringShotVel);

                    /*print("SHOTACTIVEdbg ballRb velocity apply " + ballRb[activeBall].velocity + " velocity " + velocity
                        + " (prepareShotPos[prepareShotMaxIdx - 1, 0] " + prepareShotPos[prepareShotMaxIdx - 1, 0]
                        + " prepareShotPos[prepareShotMaxIdx - 2, 0] " + prepareShotPos[prepareShotMaxIdx - 2, 0]
                        + " prepareShotPos[prepareShotMaxIdx - 3, 0] " + prepareShotPos[prepareShotMaxIdx - 3, 0]
                        + " Distance " + Vector3.Distance(prepareShotPos[prepareShotMaxIdx - 1, 0],
                                         prepareShotPos[prepareShotMaxIdx - 2, 0]));*/
                    //TOCHECK                    
                    //ballRb[activeBall].velocity = 
                }
                else {
                    shotRet = false;
                    updateBallDuringShot = true;
                }

                //print("SHOTACTIVEdbg ballRb " + ballRb[activeBall].transform.position
                //    + " curveShotFlyPercent " + curveShotFlyPercent + " photonView " + photonView.IsMine + " shotPercent " +
                //    shotPercent + " ball velocity " + ballRb[activeBall].velocity);

                /*print("DBGSHOT12 shoot normalizeTime " + normalizeTime  + 
                    " passedShotFlyTime " + passedShotFlyTime
                    + " timeofBallFly " + timeofBallFly
                    + " shotPercent " + shotPercent
                    + " outShotStart " + outShotStart
                    + " outShotMid " + outShotMid
                    + " outShotEnd " + outShotEnd);*/

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
                                                            SHOTVARIANT2);*/

                ///if (//timeOfBallFlyBasedOnPositionReverse(
                //     rb.transform.position, timeofBallFly, shotDistanceToTravel) <
                //(ShotSpeedMin + 50.0f))
                if (!isBallTrailRendererInit)
                {
                    if (timeofBallFlyOrg < 550f)
                    {
                        //ball[1].ballTrailRendererInit();
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

            //if (isFixedUpdate == 1)          
            passedShotFlyTime = passedShotFlyTime + (Time.deltaTime * 1000.0f);
            //print("TIMEEXEC " + passedShotFlyTime);
        }

    }

    public void setIsGoalJustScored(bool val)
    {
        isGoalJustScored = val;
    }
    public bool isBallOutPlayingArea()
    {
        return isBallOut;
    }


    private bool IsItTooLateToGkDive(Vector3[,] prepareShotPos,
                                     int prepareShotMaxIdx)
    {

        //float maxTimeToGkAction = 0.100f;
        float maxTimeToGkAction = 0.060f;

        Vector3 realHitPlaceLocal = Vector3.zero;
        float timeToHitZ = 0f;

        if (!peerPlayer.getShotActive())
            return false;

        /*print("DBGCOLLISIONCALC1024D IsItToLateToGkDive executed startPos " +
            peerPlayer.getOutShotStart() + " midPos " + peerPlayer.getOutShotMid() +
            " endPos " + peerPlayer.getOutShotEnd() + " isMaster " + isMaster
            + " ballRb.transform.position " + ballRb[activeBall].transform.position
            + " rb.transform.position " + rb.transform.position
            + " timeOfBallFlhy " + peerPlayer.getTimeOfBallFly()
            + " passedTime " + peerPlayer.getPassedTime());*/

        /*realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rb,
                                                       peerPlayer.getOutShotStart(),
                                                       peerPlayer.getOutShotMid(),
                                                       peerPlayer.getOutShotEnd(),
                                                       true);*/

        getRotatedRbToBall(peerPlayer.prepareShotPos[0, 0],
                           getPlayerRb(),
                           ref getRotatedRbToBallRef(),
                           getGkCornerPoints());

        realHitPlaceLocal = bezierCurvePlaneInterPoint(
                                                        //gameObject,
                                                        rotatedRbToBall,
                                                        prepareShotPos,
                                                        0,
                                                        prepareShotMaxIdx - 1,
                                                        true);


        timeToHitZ = ((realHitPlaceLocal.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
        /*print("#DBG1024 timeToHitZ " + timeToHitZ + " peerPlayer.getTimeOfBallFly() " +
            peerPlayer.getTimeOfBallFly() 
            + " peerPlayer.getPassedTime() "+ peerPlayer.getPassedTime()
            + " realHitPlaceLocal.z " + realHitPlaceLocal.z
            + " prepareShotPos Last IDX " + prepareShotPos[prepareShotMaxIdx - 1, 0]
            + " rb.transform.position " + rb.transform.position
            + "rotatedRb "+ rotatedRbToBall.transform.position
            + " eulerAngles " + rotatedRbToBall.transform.eulerAngles);

        Vector3 timeToHitZold = bezierCurvePlaneInterPoint(0.0f,
                                                       1.0f,
                                                       rb,
                                                       peerPlayer.getOutShotStart(),
                                                       peerPlayer.getOutShotMid(),
                                                       peerPlayer.getOutShotEnd(),
                                                       true);
        float timeToHitZZ = ((timeToHitZold.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
        print("#DBG1024 timeToHitZ old " + timeToHitZZ + " peerPlayer.getTimeOfBallFly() " +
            peerPlayer.getTimeOfBallFly()
            + " peerPlayer.getPassedTime() " + peerPlayer.getPassedTime());

        ///print("DEBUGGK1045 collision to player IsItTooLateToGkDive timeToHitZ " + timeToHitZ
        ///    + " realHitPlaceLocal " + realHitPlaceLocal + " IsItTooLateToGkDive "
        ///    + " ballRb " + ballRb[activeBall].transform.position
        ///   + " rb.transform.position " + rb.transform.position);*/

        if (timeToHitZ < maxTimeToGkAction)
            return true;
        else
            return false;

        if ((timeToHitZ < maxTimeToGkAction) &&
           (Mathf.Abs(realHitPlaceLocal.x) <= 2f) &&
           (Mathf.Abs(realHitPlaceLocal.y) <= 3f))
        {
            //print("DBGCOLLISIONCALC1024D collision to player");
            return true;
        }
        else
        {
            //print("DBGCOLLISIONCALC1024D left wall before ##");

            tmpRigidbody.transform.position = new Vector3(-PITCH_WIDTH_HALF, 0f, 0f);
            tmpRigidbody.transform.eulerAngles = new Vector3(0f, 90f, 0f);

            realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                           1.0f,
                                                           tmpRigidbody,
                                                           peerPlayer.getOutShotStart(),
                                                           peerPlayer.getOutShotMid(),
                                                           peerPlayer.getOutShotEnd(),
                                                           true);

            timeToHitZ = ((realHitPlaceLocal.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;

            if ((peerPlayer.getOutShotEnd().x <= -PITCH_WIDTH_HALF) &&
                (timeToHitZ < maxTimeToGkAction))
            {
                //print("DBGCOLLISIONCALC1024D left wall  ## timeToi" + timeToHitZ);
                return true;
            }
            else
            {

                tmpRigidbody.transform.position = new Vector3(PITCH_WIDTH_HALF, 0f, 0f);
                tmpRigidbody.transform.eulerAngles = new Vector3(0f, -90f, 0f);

                realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                           1.0f,
                                                           tmpRigidbody,
                                                           peerPlayer.getOutShotStart(),
                                                           peerPlayer.getOutShotMid(),
                                                           peerPlayer.getOutShotEnd(),
                                                           true);

                timeToHitZ = ((realHitPlaceLocal.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
                //print("DBGCOLLISIONCALC1024D right wall before ## timeToHitZ " + timeToHitZ);

                if ((peerPlayer.getOutShotEnd().x >= PITCH_WIDTH_HALF) &&
                    (timeToHitZ < maxTimeToGkAction))
                {
                    //print("DBGCOLLISIONCALC1024D right wall ##");
                    return true;
                }
                else
                {
                    if (peerPlayer.getOutShotEnd().z < 0f)
                    {

                        tmpRigidbody.transform.position = new Vector3(0f, 0f, -PITCH_HEIGHT_HALF + 0.1f);
                        tmpRigidbody.transform.eulerAngles = Vector3.zero;

                        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                                       1.0f,
                                                                       tmpRigidbody,
                                                                       peerPlayer.getOutShotStart(),
                                                                       peerPlayer.getOutShotMid(),
                                                                       peerPlayer.getOutShotEnd(),
                                                                       true);
                        timeToHitZ = ((realHitPlaceLocal.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
                        //print("DBGCOLLISIONCALC1024D down wall ## master before timeToHitZ " + timeToHitZ
                        //    + " tmpRigidbody.transform.position " + tmpRigidbody.transform.position);

                        if (timeToHitZ < maxTimeToGkAction)
                        {
                            //print("DBGCOLLISIONCALC1024D dooown wall ## master");
                            return true;
                        }
                    }
                    else
                    {
                        tmpRigidbody.transform.position = new Vector3(0f, 0f, PITCH_HEIGHT_HALF - 0.1f);
                        tmpRigidbody.transform.eulerAngles = new Vector3(0f, 180f, 0f);

                        realHitPlaceLocal = bezierCurvePlaneInterPoint(0.0f,
                                                                      1.0f,
                                                                      tmpRigidbody,
                                                                      peerPlayer.getOutShotStart(),
                                                                      peerPlayer.getOutShotMid(),
                                                                      peerPlayer.getOutShotEnd(),
                                                                      true);
                        timeToHitZ = ((realHitPlaceLocal.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
                        //print("DBGCOLLISIONCALC1024D up wall ## master before timeToHitZ " + timeToHitZ
                        //    + " tmpRigidbody.transform.position " + tmpRigidbody.transform.position);

                        if (timeToHitZ < maxTimeToGkAction)
                        {
                            //print("DBGCOLLISIONCALC1024D up wall ## master");
                            return true;
                        }
                    }
                }
            }
        }


        return false;
    }

    private void RPCshot()
    {
        if (preShotActive && !shotActive)
        {
            shotActive = prepareShot(animator,
                                     ref shotType,
                                     rb,
                                     ballRb[activeBall],
                                     rbRightFoot,
                                     rbRightToeBase,
                                     rbLeftToeBase,
                                     ref initPreShot,
                                     ref initPreShotRPC,
                                     ref initVolleyShot,
                                     ref isUpdateBallPosActive,
                                     ref updateBallPos,
                                     ref updateBallPosName,
                                     ref shotRotationDelay,
                                     isMaster,
                                     false,
                                     false);
            //print("DBGSHOT12## shotActive ret " + shotActive + " isMine " + photonView.IsMine);
            if (shotActive)
            {
                passedShotFlyTime = 0f;
                outShotStart = ballRb[activeBall].transform.position;
                peerPlayer.matchStatistics.setShot("teamB");
                if (peerPlayer.isShotOnTarget(outShotEnd, peerPlayer.goalSizes))
                {
                    peerPlayer.matchStatistics.setShotOnTarget("teamB");
                }
                //print("#DBGK1024_SIMULATE_DBG2435_CREATE shot starting...." + getShotActive());
            }

            //print("RPCEXECUTED preShotActive prepare  " + preShotActive + " shotActive " + shotActive);

            //print("RPCSHOTDBG prepareShot isshotActive " + shotActive);
        }

        //print("RPCSHOTDBG BEFORE shotActive " + shotActive + " RPC_PRESHOTCALC " + rpc_preShotCalc);

        //if (!rpc_preShotCalc && shotActive)
        //{
        //passedShotFlyTime = 0;
        //outShotStart = ballRb[activeBall].transform.position;
        //print("GKDEBUG1X_DGX DBG WAITING FOR A SHOT");
        //}

        isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        if (preShotActive ||
            shotActive ||
            (!isAnyAnimationPlaying &&
             !checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 1.00f, "3D_run_")))
        {
            if (!gkLock && rpc_preShotCalc)
            {

                //TOCHECK
                ///outShotStart = ballRb[activeBall].transform.position;

                /*volley has a separeate rotation*/
                /*if (!isPlaying(animator, "3D_volley", 1.0f) &&
                    !isPlaying(animator, "3D_volley_before", 1.0f))
                {
                    //print("DBG234X RBLOOKAT END");
                    Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
                    Vector3 shotDirection3D = Vector3.zero;
                    Vector3 shotEndPosOrg = outShotEnd;
                    if (isAnimatorPlaying(animator) &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= shotRotationDelay)
                    {
                        float lookSide = 0f;
                        if (!isMaster)
                            lookSide = getGoalSizePlr1().x;
                        else
                            lookSide = getGoalSizePlr2().x;

                        if (endPosOrg.x > 0)
                            lookSide = -lookSide;

                        if (!isMaster)
                            shotEndPosOrg = new Vector3(lookSide, 0f, -PITCH_HEIGHT_HALF);
                        else
                            shotEndPosOrg = new Vector3(lookSide, 0f, PITCH_HEIGHT_HALF);
                    }

                    shotDirection3D = (shotEndPosOrg - rb.transform.position).normalized;
                    shotDirection3D.y = 0.0f;
                    lookOnLook = Quaternion.LookRotation(shotDirection3D);
                    rb.transform.rotation =
                        Quaternion.Slerp(rb.transform.rotation, lookOnLook, Time.deltaTime * 10.0f);
                }*/
            }
        }

        if (shotActive && rpc_preShotCalc)
        {
            /*print("DBGSHOT12## startshooting isMine " + photonView.IsMine
                + " shotRet " + shotRet
                + " prepareShotMaxIdx " + prepareShotMaxIdx
                + " prepareShotPosIdx " + prepareShotPosIdx);*/
            if (!shotRet)
            {
                float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);
                //print("factorTimeFlyNorm " + timeofBallFly + " TIME " + passedShotFlyTime + " NORMALIZE TIME " + normalizeTime);
                //print("NORMALIZE TIME " + normalizeTime);
                shotPercent = normalizeTime;
                //print("GKDEBUG1X_DGX SHOT FLYING " + ballRb.transform.position);
                

                /*print("GKDEBUG1X_DGX ## BALLPOS " + ballRb.transform.position
                    + " normtime " + normalizeTime
                    + " passedTime " + passedShotFlyTime
                    + " timeOfBallFly " + timeofBallFly
                    + " OUTSTART "
                    + outShotStart
                        + " MID " + outShotMid + " END " + outShotEnd);*/


                //shotRet = shot3New(outShotStart,
                //                   outShotMid,
                //                   outShotEnd,
                //                   outShotBallVelocity,
                //                   ref lastBallVelocity,
                //                   shotvariant,
                //                   normalizeTime);
                float ballVelocity = 0f;
                //if (prepareShotPosIdx > 0)
                //{
                //    ballVelocity = Vector3.Distance
                //                     (prepareShotPos[prepareShotPosIdx, 0],
                //                      prepareShotPos[prepareShotPosIdx - 1, 0]) / Time.deltaTime;
                //    ballRb[activeBall].velocity =
                 //   (prepareShotPos[prepareShotPosIdx - 1, 0] - prepareShotPos[prepareShotPosIdx, 0]).normalized * ballVelocity;
                 //   updateBallDuringShot = true;
                //}


                ballRb[activeBall].transform.position =
                    prepareShotPos[prepareShotPosIdx, 0];
                ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
                ballRb[activeBall].velocity = Vector3.zero;

                curveShotFlyPercent = shotPercent =
                    prepareShotPos[prepareShotPosIdx, 1].x;

                updateBallDuringShotPos = ballRb[activeBall].transform.position;
                ////print("DBGSHOT prepareShotPosIdx " + prepareShotPosIdx + " ballPos " + ballRb[activeBall].transform.position);
                prepareShotPosIdx++;
                if (prepareShotPosIdx == prepareShotMaxIdx)
                {
                    //print("SHOTACTIVEdbg end ### " + shotRet);
                    shotRet = true;
                    ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
                    float velocity =
                        Vector3.Distance(prepareShotPos[prepareShotMaxIdx - 2, 0],
                                         prepareShotPos[prepareShotMaxIdx - 3, 0]) / Time.deltaTime;
                    ballRb[activeBall].velocity =
                (prepareShotPos[prepareShotMaxIdx - 1, 0] - prepareShotPos[prepareShotMaxIdx - 2, 0]).normalized * velocity;
                    isUpdateBallVelDuringShot = true;
                    updateBallDuringShot = false;
                    updateBallDuringShotVel = ballRb[activeBall].velocity;
                    //print("#DBGSHOT updateBallDuringShotVel " + updateBallDuringShotVel);
                    /*print("SHOTACTIVEdbg ballRb velocity apply state machine " + ballRb[activeBall].velocity + " velocity " + velocity
                        + " (prepareShotPos[prepareShotMaxIdx - 1, 0] " + prepareShotPos[prepareShotMaxIdx - 1, 0]
                        + " prepareShotPos[prepareShotMaxIdx - 2, 0] " + prepareShotPos[prepareShotMaxIdx - 2, 0]
                        + " prepareShotPos[prepareShotMaxIdx - 3, 0] " + prepareShotPos[prepareShotMaxIdx - 3, 0]
                        + " Distance " + Vector3.Distance(prepareShotPos[prepareShotMaxIdx - 2, 0],
                                         prepareShotPos[prepareShotMaxIdx - 3, 0]));*/
                    //TOCHECK                    
                    //ballRb[activeBall].velocity = 
                }
                else
                {
                    shotRet = false;
                    updateBallDuringShot = true;
                }

                /*print("SHOTACTIVEdbg ballRb " + ballRb[activeBall].transform.position
                   + " curveShotFlyPercent " + curveShotFlyPercent + " photonView " + photonView.IsMine
                   + " prepareShotPosIdx " + prepareShotPosIdx
                   + " prepareShotMaxIdx " + prepareShotMaxIdx
                   + " isFixedUpdate " + isFixedUpdate
                   + " Time.deltaTime " + Time.deltaTime);*/

                if (!isBallTrailRendererInit)
                //&& shotSpeed > 72.0f)
                {
                    //print("SHOTSPEED " + shotSpeed);

                    float shotDistanceToTravel = calcShotDistance(outShotStart,
                                                                  outShotMid,
                                                                  outShotEnd,
                                                                  shotvariant);


                    if (timeOfBallFlyBasedOnPositionReverse(
                            rb.transform.position, timeofBallFly, shotDistanceToTravel) <
                       (ShotSpeedMin + 50.0f))
                    {
                        //ball.ballTrailRendererInit();
                        audioManager.PlayNoCheck("ballFastSpeed");
                    }

                    //else {
                    audioManager.PlayNoCheck("kick2");
                    //}

                    isBallTrailRendererInit = true;
                }

                //if (isFixedUpdate == 1)               
            }

            passedShotFlyTime = passedShotFlyTime + (Time.deltaTime * 1000.0f);
        }
    }

    public IEnumerator prepShotDelay(float delay)
    {
        isPrepareShotDelay = true;
        yield return new WaitForSeconds(delay);
        isPrepareShotDelay = false;
    }

    public IEnumerator prepShotDelay2(float delay)
    {
        isPrepareShotDelay2 = true;
        yield return new WaitForSeconds(delay);
        isPrepareShotDelay2 = false;
    }

    //public IEnumerator prepShotDelay(float delay, ref bool delayVar)
    //{
    //    delayVar = true;
    //   yield return new WaitForSeconds(delay);
    //   delayVar = false;
    //}

    private bool rpcRotationInit = false;
    private void RPCstateMachine()
    {
        if (!rpcRotationInit)
        {
            if (isMaster)
            {
                rpc_rbRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                rpc_rbRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            rpcRotationInit = true;
        }
  
        /*GK simulation*/
        //if (RPCgkMovesPacket != null && 
        //if (!RPCgkMovesPacket.packetProccesed &&prepareShot
        /*print("DBG342344COL analyze peerPlayer.getShotPercent() " + peerPlayer.getShotPercent() 
            + " gkPercentCollisionTime " 
            + gkPercentCollisionTime + " gkLastCollisionVel " + gkLastCollisionVel
                     + " gkLastCollisionAngVel " + gkLastCollisionAngVel
                     + " gkPercentCollisionTime " + gkPercentCollisionTime
                     + " RPC_gkPredictionActive " + RPC_gkPredictionActive
                     + " peerPlayer.getSotActive() " + peerPlayer.getShotActive());*/

        if (peerPlayer.getShotActive() &&
            //isCollisionWithPlayer &&
            ball[activeBall].getIsPlayerDownCollided() &&
            (!Globals.hasTheSameSign(rb.transform.position.z, ballRb[activeBall].transform.position.z) 
            ||
            (prepareShotPosIdx <= 1)))
        {
            ////print("DBGSHOT setIsPlayerDownCollided faslse prepareShotIdx " + prepareShotPosIdx
            ////    + " ballRb.transform.position " + ballRb[activeBall].transform.position);
            ball[activeBall].setIsPlayerDownCollided(false);
        }

            if (peerPlayer.getShotActive() &&
               (gkLastCollisionVel != INCORRECT_VECTOR) &&
               (gkPercentCollisionTime != -1) &&
               ((peerPlayer.getShotPercent() >= gkPercentCollisionTime) ||
               ((isCollisionWithPlayer && 
               (ball[activeBall].getIsPlayerDownCollided() ||
                Mathf.Abs(InverseTransformPointUnscaled(rb.transform, ballRb[activeBall].transform.position).z) < 0.5f))) ||
               (!isCollisionWithPlayer &&
                ball[activeBall].getIsWallCollided() 
                //||
              //  (Mathf.Abs(ballRb[activeBall].transform.position.z) >= PITCH_HEIGHT_HALF - 0.6f)
               )                                                               
                )
                && RPC_gkPredictionActive)
           //((Time.time - gkCollisionPackageArriveTime) < 2.0f))
        {

            if (isCollisionWithPlayer)
            {
                float distX = getGkLastDistXCord();
                audioManager.PlayNoCheck("gksave1");

                ball[1].setwhoTouchBallLast(2);

                if (isMaster)
                    ball[activeBall].setPlayerDownLastGkCollision(Time.time);
                else
                    ball[activeBall].setPlayerUpLastGkCollision(Time.time);

                audioManager.Commentator_PlayRandomSave(getGkLastDistXCord());
                if (distX > 4.0f)
                {
                    if (!audioManager.isPlayingByName("crowdOvation1Short") &&
                        !isTrainingActive &&
                        !isBonusActive)
                    {
                        audioManager.Play("crowdOvation1Short");
                    }
                }
            }

            mainUpdateTypeActive = true;
            //mainUpdateType = "B";
            peerPlayer.setShotActive(false);
            peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), false);
            ball[activeBall].setIsPlayerDownCollided(false);
            ball[activeBall].setIsWallCollided(false);

            rpc_playerOnBallActive = false;
            rpc_shotActive = false;
            isCollisionWithPlayer = false;
            peerPlayer.setShotSaveStatistics("teamB");
            lastTimeSaveBupdate = Time.time;


            /* if (!isCollisionWithPlayer)
             {
                 if (ballRb[activeBall].transform.position.z > 0)
                 {
                     ballRb[activeBall].transform.position =
                         new Vector3(ballRb[activeBall].transform.position.x,
                         ballRb[activeBall].transform.position.y,
                         PITCH_HEIGHT_HALF - BALL_NEW_RADIUS);
                 }
                 else
                 {
                     ballRb[activeBall].transform.position =
                        new Vector3(ballRb[activeBall].transform.position.x,
                        ballRb[activeBall].transform.position.y,
                        -(PITCH_HEIGHT_HALF - BALL_NEW_RADIUS));
                 }
             }*/

            if (!isCollisionWithPlayer)
            {
                ///ballRb[activeBall].transform.position = ballPosWhenCollision;
                isLateUpdateBallPos = true;
            }

            //print("DBGWALL rpc_rbPredictedPos  APPLY peerPlayer.getShotPercent() " + peerPlayer.getShotPercent() + " " +
            //       gkPercentCollisionTime + " gkLastCollisionVel " + gkLastCollisionVel
            //        + " gkLastCollisionAngVel " + gkLastCollisionAngVel
            //    + " diff " + (Time.time - gkCollisionPackageArriveTime)
            //     + " ballPos " + ballRb[activeBall].transform.position);

            //print("DBGCOLLISIONCALC1024D goalDownPosts isTrigger "
            //       + GameObject.Find("goalDownPosts").GetComponent<Collider>().isTrigger);

            //print("DBGCOLLISIONCALC1024D playerDownLeftToeBaseCollider isTrigger "
            //    + GameObject.Find("playerDownLeftToeBaseCollider").GetComponent<Collider>().isTrigger);

            peerPlayer.clearAfterBallCollision();
            rpc_isShotActive = false;

            if (gkLastCollisionVel != Globals.INCORRECT_VECTOR_2)
            {
                ballRb[activeBall].velocity = gkLastCollisionVel;
                ballRb[activeBall].angularVelocity = gkLastCollisionAngVel;
            }
            else
            {
                //print("DBGCOLLISIONCALC1024D Globals.INCORRECT_VECTOR_2 "
                //    + gkLastCollisionVel + " ballVel " + ballRb[activeBall].velocity);
            }

            isLateUpdateBallVelocity = true;
            isLateUpdateBallPos = true;
            lateUpdateBallVelocity = gkLastCollisionVel;
            lateUpdateBallPos = ballRb[activeBall].transform.position;
            lateUpdateBallRot = ballRb[activeBall].rotation;

            gkLastCollisionVel = INCORRECT_VECTOR;
            gkPercentCollisionTime = -1f;
            RPC_gkPredictionActive = false;
        }

        if (isWaitGoalActive &&
            ((peerPlayer.getShotPercent() >= 1f) ||
             (!peerPlayer.getShotActive() && (Mathf.Abs(ballRb[activeBall].transform.position.z) > 14f))))
        {
            Globals.score1 = goalNewScore;
            //peerPlayer.setIsGoalJustScored(true);
            //peerPlayer.isBallOut = true;
            audioManager.Commentator_PlayRandomGoal(false);
            StartCoroutine(audioManager.Commentator_AfterGoal(1.3f, 2f));
            ball[1].playFlares(1);
            isWaitGoalActive = false;
            rpc_playerOnBallActive = false;
            if ((Time.time - lastTimeSaveBupdate) < 3.0f)
                peerPlayer.decSavesStatistics("teamB", 1);
        }

        //no collision with anything
        if (RPC_gkPredictionActive &&
            peerPlayer.getShotActive() &&
           (gkLastCollisionVel == INCORRECT_VECTOR) &&
           (peerPlayer.getShotPercent() >= 1f))
        {
            gkPercentCollisionTime = -1f;
            mainUpdateTypeActive = true;
            //print("DBG342344COL rpc_rbPredictedPos APPLY peerPlayer.getShotPercent() " + peerPlayer.getShotPercent() + " " +
            //gkPercentCollisionTime + " gkLastCollisionVel " + gkLastCollisionVel
            //+ " gkLastCollisionAngVel " + gkLastCollisionAngVel
            //+ " diff " + (Time.time - gkCollisionPackageArriveTime));
            //peerPlayer.setShotActive(false);
            peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), false);

            rpc_playerOnBallActive = false;
            rpc_shotActive = false;

            //peerPlayer.clearAfterBallCollision();
            rpc_isShotActive = false;
            RPC_gkPredictionActive = false;
        }

        if (//!RPC_gkPacketProcessed
            peerPlayer.getShotActive() &&
            RPC_gkPredictionActive &&
            (gkStartSequenceTime != -1f) &&
            (peerPlayer.getShotPercent() > gkStartSequenceTime)
             //&&
             )
        //&&
        //peerPlayer.getPassedTime() >= RPCgkMovesPacket.shotTime)
        {

            //print("#DBGCOLLISIONCALC1024DB gkStartSequenceTime ## " + gkStartSequenceTime + " peerPlayer.getShotPercent() "
            //    + peerPlayer.getShotPercent());
            /*print("GKDEBUG1X SHOTPASSEDTIME "
                + peerPlayer.getPassedTime()
                + " RPCgkMovesPacket.shotTime " +
                RPCgkMovesPacket.shotTime
                + " RPCSHOTACTIVE " + peerPlayer.rpc_shotActive
                + " RPPRESHOTCALC " + peerPlayer.rpc_preShotCalc);*/
            /*if (!rpc_shotActive ||
                !rpc_preShotCalc)
                print("GKDEBUG1X ERROR #### rpc_shotActive " + peerPlayer.rpc_shotActive
                    + " RPPRESHOTCALC " + peerPlayer.rpc_preShotCalc);*/

            gkTouchDone = true;
            if (!initGkMoves)
            {
                //fillGkFromRPCPackage();
                initGkMoves = true;
            }

            //TODO


            //print("RPCEXECUTED gkExecuted gkMoves in RCP");
            gkMoves(animator,
                    rb,
                    false,
                    ref lastGkAnimName,
                    ref lastTimeGkAnimPlayed,
                    ref lastGkDistX,
                    ref gkStartPos,
                    ref gkStartTransform,
                    ref gkTimeToCorrectPos,
                    ref gkSideAnimPlayOffset,
                    ref initCpuAdjustAnimSpeed,
                    ref initGkDeleyLevel,
                    ref initGKPreparation,
                    ref initAnimName,
                    ref levelDelay,
                    ref cpuGkAnimAdjustSpeed,
                    ref gkAction,
                    ref gkTimeLastCatch,
                    peerPlayer.getLobActive(),
                    ref stepSideAnimOffset,
                    ref gkLobPointReached,
                    ref gkRunPosReached,
                    ref initDistX,
                    peerPlayer.getShotVariant(),
                    peerPlayer.getOutShotStart(),
                    peerPlayer.getOutShotMid(),
                    peerPlayer.getOutShotEnd(),
                    peerPlayer.getEndPosOrg(),
                    peerPlayer.getTimeOfBallFly(),
                    peerPlayer.getPassedTime(),
                    ref gkLock,
                    ref rotatedRbToBall,
                    gkCornerPoints,
                    isExtraGoals,
                    peerPlayer.prepareShotPos,
                    peerPlayer.prepareShotMaxIdx);

            /*TOCHECK*/
            //RPCgkMovesPacket.packetProccesed = true;
            return;
        }


        if (rpc_shotActive)
        {
            RPCshot();
            return;
        }

        //TODo
        //if (rpc_playerOnBallActive)
        //{
            //float rbDist = Vector2.Distance(
            ///    new Vector2(rb.transform.position.x, rb.transform.position.z),
           ///     new Vector2(rpc_rbPrevPos.x, rpc_rbPrevPos.z));

            //print("DBGPREVRB " + rpc_rbPrevPos + " rb " + rb.transform.position);

            //if (rpc_shotActive)
            //{
                //peerPlayer.setCollisionOccur(false);
                //print("DBGSHOT rpc shot started");

                ///print("DBGSHOT12## shot active");
            //    RPCshot();
            //    return;
           // }

            //print("#DBGDIST " + rbDist);
            /*if (rpc_rbVelocity != Vector3.zero &&
                !isPlaying(animator, "3D_run", 1f) &&
                !checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 0.99f, "3D_run_"))
            {
                animator.Play("3D_run", 0, 0.0f);
            }*/


            //isUpdateBallPosActive = true;
            //updateBallPosName = "bodyMain";
            bool isAnyTurnAnimPlaying =
                checkIfAnyAnimationPlayingContain(RunAnimationsNames, animator, 1f, "3D_run_");

            //print("#DBG233 "+ (Time.time - peerPlayer.getLastTimeUpdate()));
            if ((Time.time - peerPlayer.getLastTimeUpdate()) < 0.700f)
            {   
                if (!isAnyTurnAnimPlaying)
                    rpc_UpdatePos();
                playerRun();
            }
            //return;
        //}

        //rpc_UpdatePos();
        //playerRun();

        rpc_rbPrevPos = rb.transform.position;
        //print("#DBGACTION isMaster " + isMaster + " yourHalf " + isBallOnYourHalf(isMaster) + " ballPos " +
        //    ballRb.transform.position + " isMine " + photonView.IsMine);

      

        if (!peerPlayer.getShotActive() &&
            //!isBallOnYourHalf(!isMaster) &&
            !isBallOnYourHalf(peerPlayer.getPlayerPosition()) &&
            !rpc_playerOnBallActive &&
            !rpc_shotActive &&
            !peerPlayer.isBallOutPlayingArea() &&
            !peerPlayer.getIsBallOut())
        {
            //print("DBG342344COL RPC_BALL_UPDATE " + !isBallOnYourHalf(peerPlayer.getPlayerPosition()) +
            ///    " rb.transform.position " + rb.transform.position + " ballRb[activeBall].transform.position " +
             //   ballRb[activeBall].transform.position);
            isUpdateBallPosActive = true;
            updateBallPosName = "rpcBallUpdate";
            return;
        }

        if (!rpc_playerOnBallActive)
            isUpdateBallPosActive = false;

        //if (rpc_playerRun)
        //{
        //    if (!isPlaying(animator, "3D_run", 1f))
        //    {
        //         animator.Play("3D_run", 0, 0.0f);
        //      }

        //     return;
        //}
    }


    //main half is one  with .z < 0
    private bool isPlayerOnMainHalf(Vector3 playerPos)
    {
        if (playerPos.z < 0)
            return true;
        return false;
    }

    private void ballInitialization()
    {

        //ball = GameObject.Find("ball").GetComponent<ballMovement>();
        //ball = GameManager.ball.GetComponent<ballMovement>();
        //ballRb = ball.GetComponent<Rigidbody>();

        //print("DBG123 ball " + ball + " BALLRB " + ballRb);

        ballRb[activeBall].maxAngularVelocity = 1000.0f;
        prevZballPos = ballRb[activeBall].transform.position.z;
        ballPrevPosition = ballRb[activeBall].transform.position;
        /*Draw who starts */
        int whoStarts = UnityEngine.Random.Range(0, 2);
        //TODO
        //if (whoStarts == 0)
        //{
        //    ballRb.transform.position = new Vector3(0, 0, 2);
       // 
        ////else
        //{

        //TODO
        ballRb[activeBall].transform.position = new Vector3(0, 0, 6);
        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;
        //}

        prevBallPos = ballRb[activeBall].transform.position;
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
        if (ball[1].getBallGoalCollisionStatus() ||
            peerPlayer.isWaitGoalActive ||
            isGoalJustScored)            
        {
            ////peerPlayer.isWaitGoalActive = false;

            //if (ball[1].whoScored() == 2)
            //{
            //if (((Time.time - gkTimeLastCatch) < 1.0f))
            //    printGameEventsInfo(gkAction);
            //   printGameEventsInfo("GOAL!");
            // }
            //else
            //{
            printGameEventsInfo("GOAL!");
            //}
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

        peerPlayer.rpc_ballPos = ballRb[activeBall].transform.position;
        peerPlayer.rpc_ballVelocity = Vector3.zero;
        peerPlayer.rpc_ballAngularVelocity = Vector3.zero;
        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;
        isBallOut = false;
        //print("DBGBALLPOSITION isBallOut setBAll #### position " + peerPlayer.rpc_ballPos
        //    + " ballPosition " + ballRb[activeBall].transform.position);
        RPC_sequenceNumber[(int) RPC_ACK.BALL_POS_UPDATE]++;
        peerPlayer.RPC_sequenceNumber[(int) RPC_ACK.BALL_POS_UPDATE]++;
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

    public bool getShotActive()
    {
        return shotActive;
    }

    public bool getPreShotActive()
    {
        return preShotActive;
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
        float horizontalMovement = 0f;
        float verticalMovement = 0f;

        if (photonView.IsMine)
        {
             horizontalMovement = joystick.Horizontal();
             verticalMovement = joystick.Vertical();
        }

        if (isRunPaused())
        {           
                horizontalMovement = 0f;
                verticalMovement = 0f;
        }

        /* setup speed of Run animation depending on a joystick position */
        //float runSpeed = Mathf.Max(Mathf.Abs(horizontalMovement),
        //                           Mathf.Abs(verticalMovement));

        playerDirection = new Vector3(horizontalMovement, 0.0f, verticalMovement); 
        if (!PhotonNetwork.IsMasterClient)
            playerDirection *= -1;

        if (!photonView.IsMine)
        {
            playerDirection = rpc_playerDirection;
            runningSpeed = rpc_runningSpeed;
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
                          ref activeBall,
                          isMaster);

            if (!photonView.IsMine)
            {
                isOnBall = rpc_playerOnBallActive;
                if (isOnBall) ball[1].setwhoTouchBallLast(2);
            }

            if (peerPlayer.getShotActive())
                isOnBall = false;

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

            //print("#DBGrun isOnBall " + isOnBall + " playerDirection " + playerDirection
            //    + " runningSpeed " + runningSpeed + " photonView.IsMine " + photonView.IsMine);

            playerOnBallMoves(rb,
                              isOnBall,
                              ref playerDirection,
                              runningSpeed);
        }
    }

    private void sendPrepareShotRpc(string shotType, 
                                    Vector3 playerPos, 
                                    Quaternion playerRot,
                                    float shotRotationDelay)
    {
        touchLocked = true;
        updateShotPos();

        /*float height = drawDistance / 80.0f;
        initShot = true;
        float deltaTime = drawTimeEnd - drawTimeStart;
        timeofBallFly = (drawDistance / deltaTime) / 1.25f;
        timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
        timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);
        if (isLobActive)
        {
            timeofBallFly = UnityEngine.Random.Range(500.0f, 700.0f);
        }
        shotSpeed = MIN_SHOT_SPEED + Mathf.InverseLerp(ShotSpeedMin, ShotSpeedMax, timeofBallFly) * speedMultiplayer;
        shotBar.fillAmount = 1.0f - Mathf.InverseLerp(120f, 0f, shotSpeed);

        speedShotText.text = ((int)shotSpeed).ToString() + " km/h";
        passedShotFlyTime = 0.0f;
        timeofBallFly = ShotSpeedMin + (ShotSpeedMax - timeofBallFly);

        timeofBallFly =
            timeOfBallFlyBasedOnPosition(rb.transform.position, timeofBallFly); 
        ballInitPos = ballRb[activeBall].transform.position;
        */

        initShot = true;
        float height = drawDistance / 80.0f;
        ballInitPos = ballRb[activeBall].transform.position;
        float deltaTime = drawTimeEnd - drawTimeStart;
        timeofBallFly = (drawDistance / deltaTime) / 0.85f;
        timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
        timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);


        shotSpeed = MIN_SHOT_SPEED +
            Mathf.InverseLerp(ShotSpeedMin, ShotSpeedMax, timeofBallFly) * speedMultiplayer;
        shotSpeed = Mathf.Min(118f, shotSpeed);

        passedShotFlyTime = 0.0f;
        matchStatistics.setShot("teamA");
  
        timeofBallFly = ShotSpeedMin + (ShotSpeedMax - timeofBallFly);

        if (isLobActive)
        {
            //timeofBallFly = UnityEngine.Random.Range(550.0f, 630.0f);
            timeofBallFly = UnityEngine.Random.Range(900.0f, 1000.0f);
        }

        //print("#DBGshotspeed " + shotSpeed + " timeofBallFly " + timeofBallFly + "speedMultiplayer " +
        //    speedMultiplayer);

        /*preShotCalc(curveStartPos3,
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
                    false);*/

        outShotStart = ShotSimulate.predictShotPos(playerPos,
                                                   playerRot,
                                                   shotType,
                                                   updateEndShotPos(),
                                                   ref prepareShotJustBeforeShotPos,
                                                   ref prepareShotJustBeforeShotRot);

        ///print("DBGSHOT prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos + " rb.transform " 
        ///    + rb.transform.position + " ballposition " + ballRb[activeBall].transform.position);
        ///    + rb.transform.position + " ballposition " + ballRb[activeBall].transform.position);
        preShotCalc(curveStartPos3,
             curveMidPos3,
             curveEndPos3,
             endPosOrg,
             height,
             outShotStart,
             shotSpeed,
             isLobActive,
             ref outShotBallVelocity,
             ref outShotStart,
             ref outShotMid,
             ref outShotEnd,
             ref shotvariant,
             isMaster);

        if (isShotOnTarget(outShotEnd, goalSizes))
        {
            matchStatistics.setShotOnTarget("teamA");
        }

        float shotDistanceToTravel = calcShotDistance(outShotStart,
                                                      outShotMid,
                                                      outShotEnd,
                                                      SHOTVARIANT2.CURVE);
        //timeofBallFly -= 20f;
        timeofBallFlyOrg = timeofBallFly;
        timeofBallFly =
            timeOfBallFlyBasedOnPosition(rb.transform.position, timeofBallFly, shotDistanceToTravel);
        float extraTimeOfBallFly =
            Mathf.Lerp(100f, 0, Mathf.InverseLerp(MIN_SHOT_SPEED, MAX_SHOT_SPEED, shotSpeed));
        //timeofBallFly = timeofBallFly + (timeofBallFly * extraTimeOfBallFly) / (ShotSpeedMin - 20f);
        timeofBallFly = timeofBallFly + (timeofBallFly * extraTimeOfBallFly) / (ShotSpeedMin);
        timeofBallFly = Mathf.Max(ShotSpeedMin, timeofBallFly);

        if (!isLobActive)
        {
            timeofBallFly = Mathf.Min(ShotSpeedMax, timeofBallFly);
        } else
        {
            timeofBallFly = Mathf.Min(1400f, timeofBallFly);
        }

        ///print("#DBG timeOfBallFly " + timeofBallFly + " timeofBallFlyOrg " + timeofBallFlyOrg);
        //float speedPerc = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly);
        //setBallShotVel(speedPerc * MAX_SHOT_SPEED_UNITY_UNITS);

        //float speedPerc = Mathf.InverseLerp(ShotSpeedMax, ShotSpeedMin, timeofBallFly);
        //setBallShotVel(speedPerc * MAX_SHOT_SPEED_UNITY_UNITS);

        ///print("DBGPREPXAGDS endPosOrg " + endPosOrg + " OUT " + outShotBallVelocity);

        /*print("DBGSHOT12## AFTERPRESHOTCALC curveStartPos3 " + curveStartPos3 + " curveMidPos3 "
            + curveMidPos3 + " curveEndPos3 " + curveEndPos3
            + " height " + height
            + " outShotStart " + outShotStart
            + " outShotMid " + outShotMid
            + " outShotEnd " + outShotEnd
            + " outShotBallVelocity " + outShotBallVelocity
            + " shotvariant " + shotvariant
            + " endPosOrg " + endPosOrg
            + " HEIGHT " + height);*/

        string ballPositions = "";

        //prepareShotJustBeforeShotPos = ShotSimulate.predictShotPos(rb.transform.position,
        //                                                           shotType, 
        //                                                           outShotEnd,
        //                                                           ref prepareShotJustBeforeShotRot);
        //print("prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos);



        //print("DBGSHOTPOS outShotStart calculated " + outShotStart);

        shotBallPosPreCalc(outShotStart,
                           outShotMid,
                           outShotEnd,
                           timeofBallFly,
                           ref ballPositions);

        photonView.RPC("RPC_preShotCalc",
                        RpcTarget.Others,
                        shotSpeed,
                        isLobActive,
                        outShotBallVelocity,
                        outShotStart,
                        outShotMid,
                        outShotEnd,
                        endPos,
                        shotvariant,
                        timeofBallFly,
                        ballPositions,                     
                        prepareShotJustBeforeShotPos,
                        prepareShotJustBeforeShotRot,
                        playerPos,
                        playerRot,
                        shotType,
                        shotRotationDelay);

        PhotonNetwork.SendAllOutgoingCommands();

    }

    private void shotBallPosPreCalc(Vector3 outShotStart,
                                    Vector3 outShotMid,
                                    Vector3 outShotEnd,
                                    float timeofBallFly,
                                    ref string ballPosStr)
    {
        float passedShotFlyTime = 0f;
        short cIdx = 0;
        Vector3 ballPos = Vector3.zero;
        bool ret = false;
        //curveShotFlyPercent = normalizeTime;
        //shotPercent = normalizeTime;
        prepareShotPosIdx = 0;

        while (ret != true)
        {
            float normalizeTime = Mathf.InverseLerp(0.0f, timeofBallFly, passedShotFlyTime);
            ret = shot3Simulation(outShotStart,
                                  outShotMid,
                                  outShotEnd,
                                  ref ballPos,
                                  SHOTVARIANT2.CURVE,
                                  normalizeTime);

            string currBallPosStr =
                  ballPos.x.ToString("F3", CultureInfo.InvariantCulture) + ":" 
                + ballPos.y.ToString("F3", CultureInfo.InvariantCulture) + ":" 
                + ballPos.z.ToString("F3", CultureInfo.InvariantCulture);
            string curShotPercent = normalizeTime.ToString("F5", CultureInfo.InvariantCulture);

            prepareShotPos[cIdx, 0] = 
                new Vector3(float.Parse(ballPos.x.ToString("F3", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                            float.Parse(ballPos.y.ToString("F3", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                            float.Parse(ballPos.z.ToString("F3", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));

            prepareShotPos[cIdx, 1].x = float.Parse(curShotPercent, CultureInfo.InvariantCulture);

            if (String.IsNullOrEmpty(ballPosStr))
                ballPosStr = currBallPosStr + ":" + curShotPercent;
            else
                ballPosStr += "|" + currBallPosStr + ":" + curShotPercent;


            cIdx++;
            passedShotFlyTime = passedShotFlyTime + (0.02f * 1000.0f);
        }

        prepareShotMaxIdx = cIdx;
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
                //print("DBGrun play run");
                audioManager.PlayNoCheck("run2");
            }

            float ballRbDist =
                Vector2.Distance(new Vector2(ballRb[activeBall].transform.position.x, ballRb[activeBall].transform.position.z),
                                 new Vector2(rb.transform.position.x, rb.transform.position.z));

            if (photonView.IsMine)
            {
                if (runDirection != Vector3.zero &&
                    isBallInGame &&
                    ballRbDist < 2f &&
                    ballRb[activeBall].transform.position.y < 1.0f)
                {
                    runDirection = (ballRb[activeBall].transform.position - rb.transform.position).normalized;
                    runDirection.y = 0;
                }

                rb.velocity = runDirection * runSpeed;
            }

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
            if (photonView.IsMine)
                rb.velocity = prevRbVel;

            if (!isPlaying(animator, "3D_run", 1.0f) &&
                ((!photonView.IsMine && runDirection != Vector3.zero) || (prevRbVel != Vector3.zero)) &&
                !isAnyTurnAnimPlaying)
            {
                animator.Play("3D_run", 0, 0.0f);
                audioManager.PlayNoCheck("run2");
            }

            isUpdateBallPosActive = true;
            updateBallPosName = "bodyMain";
            if (photonView.IsMine)
            {
                photonView.RPC("RPC_playerOnBAll",
                                RpcTarget.Others);
                //StartCoroutine(rpc_playerOnBall_resend((int) RPC_ACK.PLAYER_ON_BALL));
            }
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

            animName =
                convertAngleToAnimNameRunOnBall(rb, angle, false);

            if (animName.Contains("3D_run_"))           
            {
                lastAnimTurnPlayed = animName;
                animator.Play(animName, 0, 0.0f);
                setupTurnSpeedAnim();
                isAnyTurnAnimPlaying = true;
                rb.velocity = Vector3.zero;
                animator.Update(0f);
                audioManager.PlayNoCheck("run2");
                lastPlayerMovePosHead = 0;
                lastPlayerMovePosTail = 0;
            }   
        }

        if (isAnyTurnAnimPlaying)
        {
       
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
        photonView.RPC("RPC_playerOnBAll",
                        RpcTarget.Others);

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

                if (photonView.IsMine)
                {
                    rb.velocity = vel * runSpeed;
                    rb.velocity /= 1.15f;
                }
                setupRunningAnimSpeed();

                prevRbVel = rb.velocity;

                lastPlayerMovePosHead++;
                lastPlayerMovePosHead %= lastPlayerMovePos.GetLength(0);
            }
        } 
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
        teamAflagIntro = GameObject.Find("teamAflagIntroImage").GetComponent<RawImage>();
        teamBflagIntro = GameObject.Find("teamBflagIntroImage").GetComponent<RawImage>();
        teamAIntroText = GameObject.Find("teamATeamNameText").GetComponent<TextMeshProUGUI>();
        teamBIntroText = GameObject.Find("teamBTeamNameText").GetComponent<TextMeshProUGUI>();


        teamAflagStatisticsImage = GameObject.Find("teamAflagStatisticsImage").GetComponent<RawImage>();
        teamBflagStatisticsImage = GameObject.Find("teamBflagStatisticsImage").GetComponent<RawImage>();

        teamAStatisticsText = GameObject.Find("teamAStatisticsText").GetComponent<TextMeshProUGUI>();
        teamBStatisticsText = GameObject.Find("teamBStatisticsText").GetComponent<TextMeshProUGUI>();

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

        //traningPanel = GameObject.Find("traningPanel");
        //shotBarGameObject = GameObject.Find("shotBar");
        //shotBarIconGameObject = GameObject.Find("shotBarIcon");
    }

    public Vector3 getGoalSizePlr1()
    {
        return goalSizes;
    }

    public Vector3 getGoalSizePlr2()
    {
        return goalSizes;
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

        if (!isPowerEnable ||
            //TODO
            Globals.PITCHTYPE.Equals("STREET"))
        {
            powerButton1GameObject.SetActive(false);
            powerButton2GameObject.SetActive(false);
            powerButton3GameObject.SetActive(false);
        } else
        {
            powerButton1GameObject.SetActive(true);
            powerButton2GameObject.SetActive(true);
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
        //if (isTrainingActive)
        //    traningPanel.SetActive(true);
    }

    public void deactivateCanvasElements()
    {
        //TODOMULTI
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
        pauseCanvas.SetActive(false);
        /* if (isTrainingActive ||
             isBonusActive)
         {
             //stadiumPeople.SetActive(false);
             foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
             {
                 if (allStadiumPeople.name.Contains("crowdAnimated"))
                 {
                     allStadiumPeople.SetActive(false);
                 }
             }
         }*/
    }

    public void addCoins()
    {
        //if ((Globals.score1 > Globals.score2 && isMaster) ||
        //    (Globals.score2 > Globals.score1 && !isMaster))
        winCoinsRewarded = 20;
        tieCoinsRewarded = 10;

        ///print("addCoins coins " + Globals.coins);

        if (Globals.score1 > Globals.score2)
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
                                ref int activeBall,
                                bool isMaster)
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
        if (!isBallReachable(ballRb[i].transform.position, isMaster))
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
                minDistance += (BALL_NEW_RADIUS + 0.15f);
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

    private bool isBallReachable(Vector3 ballPos, bool isMaster)
    {
        if (!isMaster)
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
                rpc_gkMovementSend = false;
                initGkMoves = false;
            }
        }

        if (isPlaying(animator, "3D_volley", 1.0f))
        {
            touchCount = 0;
            gkTouchDone = false;
            rpc_gkMovementSend = false;
            initGkMoves = false;
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
    }

    public GameObject getPlayerGameObject()
    {
        return gameObject;
    }

    public bool isPlayerOnBall()
    {
        return (onBall == PlayerOnBall.ONBALL) ? true : false;
    }

    void OnAnimatorIK(int layerIndex)
    {

        if (///(Globals.peersReady < Globals.MAX_PLAYERS) ||
             !arePeersPlayerSet())
             ///!photonView.IsMine)
        {
            return;
        }
        animatorIKExecuted = true;
        ///PredictCollision.onAnimIK();

        //print("ONANIMATOROIK peerPlayer " + peerPlayer);
        onAniatorIk(animator,
                    PredictCollision.ballAnimatorStartPos,
                    //ballRb[activeBall].transform.position,
                    peerPlayer.getShotActive(),
                    peerPlayer.isLobShotActive(),
                    lastGkDistX, 
                    rb, 
                    leftHand,
                    rightHand,
                    gkSideAnimName,
                    false,
                    false);
    }

    private float lastTimeShotActivePlayer;
    private float lastTimeShotActiveCpu;

    public void onAniatorIk(Animator animator,
                            Vector3 ballRbPos,
                            bool shotActive,
                            bool isLobActive,
                            float distX,
                            Rigidbody rb,
                            GameObject leftHand,
                            GameObject rightHand,
                            string anim,
                            bool isCpu,
                            bool isSimulation)
    {

        if (!isSimulation && (gkDistRealClicked > (MIN_DIST_REAL_CLICKED + 1.0f)))
        {

            //  if (checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch") &&
            //      (shotActive))
            //          print("#DBGANIMATOR gkDistRealClicked dist too big ####");
            return;
        }

        Vector3 ballInLocalRb =
            InverseTransformPointUnscaled(rb.transform, ballRbPos);
        if ((ballInLocalRb.z < 0.0f) &&
             !isSimulation) 
            return;

        bool isGktStraightPlaying =
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "straight");

        if (isGktStraightPlaying &&
            checkIfAnyAnimationPlayingContain(animator, 1.0f, "chest"))
            return;

        if (isGktStraightPlaying &&
            ballRbPos.y > 0.8f)
        {
            float reach = 1.0f;

            Vector3 leftHandLocalPos = rb.transform.InverseTransformPoint(leftPalm.transform.position);
            Vector3 rightHandLocalPos = rb.transform.InverseTransformPoint(rightPalm.transform.position);
            //Vector3 ballInLocalRb = rb.transform.InverseTransformPoint(ballRb[activeBall].transform.position);

            //if (ballInLocalRb.z < 0.0f) return;

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
            animator.SetIKPosition(AvatarIKGoal.RightHand, ballRbPos);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, ballRbPos);

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

            Vector3 ballPos = ballRbPos;
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
                    Quaternion.LookRotation(ballRbPos - rightHand.transform.position);

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
                    Quaternion.LookRotation(ballRbPos - leftHand.transform.position);
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
        
        float shotSkillsInter =
            getSkillInterpolationReverse(attackSkillsPlayer);

        float offset;
        offset = offsetBase +
            (Mathf.InverseLerp(0f, SKILLS_MAX_VALUE, defenseSkillsPlayer) * 3.0f);

        MAX_GK_OFFSET = offset;
        MAX_GK_OFFSET = Mathf.Clamp(MAX_GK_OFFSET, 4.2f, 6.0f);
  
        runningSpeed = calcRunLevelSpeed(Globals.level,
                                         cumulativeStrengthPlayer,
                                         false) + 2.0f;
        speedMultiplayer = 70.0f - Mathf.Lerp(0, 30f, shotSkillsInter);

        //print("#DBGDEFENSESKILLS " + defenseSkillsPlayer + " attack " + attackSkillsPlayer + " runningSpeed " +
        //       runningSpeed + "gk " + MAX_GK_OFFSET);

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

        if (animator == null)
            return;

        if (!checkIfAnyAnimationPlaying(animator, 1.0f) &&
            !isPlaying(animator, animName, 1.0f))
        {
            animator.Play(animName, 0, 0.0f);
        }

        if (peerPlayer != null)
        {
            Animator cpuAnimator = peerPlayer.getAnimator();
            if (!checkIfAnyAnimationPlaying(cpuAnimator, 1.0f) &&
                !isPlaying(cpuAnimator, cpuAnimName, 1.0f))
            {
                cpuAnimator.Play(cpuAnimName, 0, 0.0f);
            }
        }
    }

    private void recoverAnimatorSpeed()
    {
        if (!checkIfAnyAnimationPlayingContain(animator, 1.0f, "3D_GK_sidecatch_"))
        {
            animator.speed = 1.0f;
        }
    }

    public string getLastGkAnimPlayed()
    {
        return lastGkAnimName;
    }

    void LateUpdate()
    {
        if (!arePeersPlayerSet())
            // Globals.peersReady < Globals.MAX_PLAYERS)
        {
            return;
        }

        //if (!ball[1].getBallCollided() &&
        //    shotActive &&
        //    !shotRet &&
        //    lastBallVelocity != Vector3.zero &&
        //    isBallInGame)
        //{
        //    ballRb[activeBall].velocity = lastBallVelocity;
        //}

        if ((photonView.IsMine ||
            (checkIfJoystickExtraButtonsPlaying(animator, 1.0f) != -1)) &&
            !preShotPositionActive)
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
                        isMaster);
        }

        corectBallPositionOnBall(rb,
                                 animator,
                                 rbRightToeBase,
                                 rbRightFoot,
                                 ref isUpdateBallPosActive,
                                 updateBallPos,
                                 updateBallPosName,
                                 false);

        if (photonView.IsMine)
        {
            /*print("DBGPREDICTCOLLISON peerPlayer.getShotPercent() " + peerPlayer.getShotPercent()
                  + " gkLastCollisionVel  " + gkLastCollisionVel
                  + " peerPlayer.getShotPercent()  " + peerPlayer.getShotPercent()
                  + " gkPercentCollisionTime  " + gkPercentCollisionTime
                  + " (Time.time - gkCollisionPackageArriveTime) "
                  + (Time.time - gkCollisionPackageArriveTime));*/

               if (!peerPlayer.getShotActive() &&
                   playerOnBall)
            {
                //TOUNCOMENT
                //peerPlayer.setShotActive(false);
                //print("DBG342344COL isPlayerOnball lateUpdate");
                setPlayersColliders(gameObject, false);
                peerPlayer.RPCclearPreShotVariables();
            }
        }

        /*if (photonView.IsMine)
        {           
            if (peerPlayer.getShotActive() &&
               (gkLastCollisionVel != INCORRECT_VECTOR) &&
               ((gkPercentCollisionTime != -1) &&
                (peerPlayer.getShotPercent() >= gkPercentCollisionTime)) &&
                ((Time.time - gkCollisionPackageArriveTime) < 2.0f))
            {

            
                peerPlayer.setShotActive(false);
                setPlayersColliders(gameObject, false);
                peerPlayer.RPCclearPreShotVariables();

                if (gkLastCollisionVel != Globals.INCORRECT_VECTOR_2)
                {
                    ballRb[activeBall].velocity = gkLastCollisionVel;
                    ballRb[activeBall].angularVelocity = gkLastCollisionAngVel;
                }
                else
                {
                   

                }
                gkLastCollisionVel = INCORRECT_VECTOR;
            }
        }*/


        if (updateBallDuringShot)            
        {
            //print("DBGWALL updateballPos " + updateBallDuringShotPos);
            ballRb[activeBall].transform.position = updateBallDuringShotPos;
            ballRb[activeBall].angularVelocity = new Vector3(24f, 24f, 24f);
            ballRb[activeBall].velocity = Vector3.zero;

            updateBallDuringShot = false;
        }

        if (isUpdateBallVelDuringShot)           
        {
            //print("DBGWALL updateballvel " + updateBallDuringShotVel);

            ballRb[activeBall].angularVelocity = new Vector3(24f, 24f, 24f);
            ballRb[activeBall].velocity = updateBallDuringShotVel;
            isUpdateBallVelDuringShot = false;
        }

        if (preShotPositionActive)
        {
            rb.transform.position = preShotPositionVal;
            preShotPositionActive = false;
        }

        /*if (isLateUpdateBallVelocity) {
            ballRb[activeBall].velocity = lateUpdateBallVelocity;
            ballRb[activeBall].rotation = lateUpdateBallRot;
            if (isLateUpdateBallPos)
            {
               print("DBGWALL isLateUpdaeBallPos before pos " + ballRb[activeBall].transform.position
                    + " ballVel "  + ballRb[activeBall].velocity);
                //ballRb[activeBall].transform.position = lateUpdateBallPos;
                isLateUpdateBallPos = false;
                ///print("DBGWALL isLateUpdaeBallPos after pos " + ballRb[activeBall].transform.position);
            }
            isLateUpdateBallVelocity = false;
        }*/
    
        if (photonView.IsMine)
        {
            if (peerPlayer.getShotActive() &&
                ballRb[activeBall].velocity != Vector3.zero &&
                isBallinGame() &&
                getGkHelperImageVal())
            {
                //print("DEBUGLASTTOUCHLUCKXYU UPDATE ROTATED " + ballInitPos + " " +
                //    "peerPlayer.prepareShotPos[0,0] " + peerPlayer.prepareShotPos[0, 0]
                //    + " ballRb " + ballRb[activeBall].transform.position);

                drawGkHelperCircle(
                      getRotatedRbToBall(peerPlayer.prepareShotPos[0, 0],
                                         getPlayerRb(),
                                         ref getRotatedRbToBallRef(),
                                         getGkCornerPoints()),
                      peerPlayer.getShotVariant(),
                      peerPlayer.getOutShotStart(),
                      peerPlayer.getOutShotMid(),
                      peerPlayer.getOutShotEnd(),
                      peerPlayer.prepareShotPos,
                      peerPlayer.prepareShotMaxIdx);



                ///print("DRAWHELPERCIRLCE");
            }
        }


        //print("LATE RB POSITION " + rb.position);
        /*don't allow to goes down over floor after some time */
        if (rb.transform.position.y < 0.03f)
            rb.transform.position =
                new Vector3(rb.transform.position.x, 0.03f, rb.transform.position.z);


        if (photonView.IsMine)
        {
            for (int i = 0; i < isFixedUpdate; i++)
            {
                cameraMovement(false, -1);
            }
        }

        isFixedUpdate = 0;
        //print("ENTERPOSITION CLEAN END OF LATEUPDATE RB POS " + rb.transform.position);
    }

    private void RPCclearPreShotVariables()
    {
        //print("GKDEBUG1X_DGX RPCclearPreShotVariables 2");

        //rint("DBGprepare1234 RPCclearPreShotVariables here " + preShotActive);

        rpc_preShotCalc = false;
        rpc_shotActive = false;
        rpc_playerOnBallActive = false;
        prepareShotRpcSend = false;

        preShotActive = false;
        shotActive = false;
        setWallColliders(true);
        initPreShot = false;
        initPreShotRPC = false;
        initVolleyShot = false;
        passedShotFlyTime = 0.0f;
        //prepareCpuOptions = false;
        isBallTrailRendererInit = false;
        //drawGkHelperCircle = 0.0f;
        initCpuAdjustAnimSpeed = false;
        initGkDeleyLevel = false;
        initGKPreparation = false;

        gkRotationLoops = 0;
        gkSideAnimDelayBefore = 0f;
        gkSideAnimDelayBeforeStart = -1f;
        GK_DEBUG_INIT = false;


        gkSideOffsetListCurrIdx = 0;
        gkSideOffsetListMax = 0;
        stepSideAnimOffsetList.Clear();

        levelDelay = 0.0f;
        initDistX = -1;
        shotRet = false;
        setGkHelperImageVal(false);
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
        if (!photonView.IsMine)
        {
            rbRoundVector = 
                (rb.transform.position - playerPrevPos).normalized * playerVelocity;
            rbRoundVector = new Vector3(
                                           Mathf.Round(rbRoundVector.x),
                                           Mathf.Round(rbRoundVector.y),
                                           Mathf.Round(rbRoundVector.z));
            //print("#DBGBALLROTATTE " + rbRoundVector + " playerVelocity " + playerVelocity);
        }

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


    public Vector3 getBallPosAfterOut(bool timeToShotExceeded, bool ballGoal)
    {
        //bool ballGoal = ball[1].getBallGoalCollisionStatus();
        Vector3 ballPosPlayer2 = new Vector3(UnityEngine.Random.Range(-10, 10),
                                             BALL_NEW_RADIUS,
                                             UnityEngine.Random.Range(5, 10));
        Vector3 ballPosPlayer1 = ballPosPlayer2;
        ballPosPlayer1.z *= -1;

        if (timeToShotExceeded)
        {
            if (ball[1].transform.position.z <= 0)
                ball[1].setwhoTouchBallLast(1);
            else
                ball[1].setwhoTouchBallLast(2);
        }

        if (ballGoal || peerPlayer.isWaitGoalActiveNewPos)
        {
            peerPlayer.isWaitGoalActiveNewPos = false;

            if (ballGoal)
            {
                return ballPosPlayer1;
            } else
            {
                return ballPosPlayer2;
            }

            //if (ball[1].whoScored() == 1)
            //{
            //   return ballPosPlayer2;
            // }
            // else
            //{
            return ballPosPlayer1;
            //}
        }

        if (ball[1].whoTouchBallLast() == 1)
        {
            //ballRb[activeBall].position = new Vector3(0, 0, 4);
            ////ball[1].setwhoTouchBallLast(2);
            return ballPosPlayer2;
        }
        else
        {
            ////ball[1].setwhoTouchBallLast(1);
            return ballPosPlayer1;
        }
    }


    public void setBallPosition()
    {
        //if (ball.getBallGoalCollisionStatus())
        //{
        //if (Globals.isTrainingActive ||
        //    isBonusActive)
       // {
            /*if (trainingScript.isGkTraining())
            {
                ballRb[activeBall].position =
                    new Vector3(UnityEngine.Random.Range(-13, 13f),
                                0f,
                                UnityEngine.Random.Range(3, 8f));
            }*/

       //     return;
       // }

        //ballRb[activeBall].position = new Vector3(PITCH_WIDTH_HALF - 0.23f, 0, -(PITCH_HEIGHT_HALF - 0.23f));
        //return;


        ballRb[activeBall].transform.position = ballPosAfterOut;
        peerPlayer.clearBufferBallPos();
        isUpdateBallPosActive = false;

        if (ballPosAfterOut.z > 0)
        {
            ball[activeBall].setwhoTouchBallLast(1);
        }
        else
        {
            ball[activeBall].setwhoTouchBallLast(2);
        }


        //print("DBGTOUCHLAST setballPosition ######### to " + ballPosAfterOut);
        /////bool ballGoal = ball[1].getBallGoalCollisionStatus();
        ////Vector3 ballPosPlayer2 = new Vector3(UnityEngine.Random.Range(-10, 10),
        ////                                     ballRadius,
        ////                                     UnityEngine.Random.Range(5, 10));
        ///Vector3 ballPosPlayer1 = ballPosPlayer2;
        ////ballPosPlayer1.z *= -1;

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

        /*print("#DBGTOUCHLAST setBallPosition whoTouchLast ballGoal " + ballGoal + " whotouchlast " +
            ball[1].whoTouchBallLast());

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
        }*/

        //print("whoTouchBallLastSETBALL " + ball.whoScored() + " WHOTOUCHLAST " + ball.whoTouchBallLast());
        /*if (ball[1].whoTouchBallLast() == 1)
        {
            //ballRb[activeBall].position = new Vector3(0, 0, 4);
            ballRb[activeBall].transform.position = ballPosPlayer2;
            ball[1].setwhoTouchBallLast(2);
            return;
        }
        else
        {
            ballRb[activeBall].transform.position = ballPosPlayer1;
            ball[1].setwhoTouchBallLast(1);
            return;
        }*/

        /*shouldn't be reach */
        //ballRb[activeBall].transform.position = ballPosPlayer1;

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

    public void showBalls(int num)
    {
        for (int i = 1; i <= NUMBER_OF_BALLS; i++)
        {
            ballRb[i] = GameObject.Find("ball" + i.ToString()).GetComponent<Rigidbody>();
            ball[i] = GameObject.Find("ball" + i.ToString()).GetComponent<BallMovementMultiplayer>();
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

    private bool prepareShot(Animator animator,
                             ref string shotType,
                             Rigidbody playerRb,
                             Rigidbody ballRb,
                             GameObject rbRightFoot,
                             GameObject rbRightToeBase,
                             GameObject rbLeftToeBase,
                             //Plane plane,
                             ref bool initShot,
                             ref bool initShotRpc,
                             ref bool initVolleyShot,
                          //Vector2 endPos,
                          //Vector3 endPosOrg,
                             ref bool isUpdateBallPosActive,
                             ref Vector3 updateBallPos,
                             ref string updateBallPosName,
                             ref float shotRotationDelay,
                             bool isMaster,
                             bool isDelay,
                             bool isDelay2)
    {
        bool shotActive = false;
        float clipOffsetTime = 1f;
        bool isCpu = false;

        if (photonView.IsMine && !initShotRpc)
        {
            /*Don't block touch here */
            /*animator.Play(shotType, 0, 0.0f);
            animator.Update(0.0f);
            initShot = true;
            shotRotationDelay = 0f;
            if (shotType.Equals("3D_volley_before"))
            {
                shotType = "3D_volley";
            }*/

            ///print("DBGSHOT12# init IsMine " + photonView.IsMine);
            animator.SetFloat("3d_normal_shot_speed", 0.8f);
            rb.velocity = playerRb.velocity = Vector3.zero;

            setWallColliders(false);
            peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), true);

            //photonView.RPC("RPC_prepareShot",
            //                RpcTarget.Others,
            //                shotType,
            //                playerRb.transform.position,
            //                playerRb.transform.rotation,
            //                playerRb.velocity);

            ///print("rpc_rbPredictedPos prepareShot RPC SEND ###");
            //print("#PREPARESHOTSTARTTIME SEND ANIM START " + PhotonNetwork.Time);
            //print("BEZIERDBG123 PREPARESHOT SENT rbPOS "
            //    + playerRb.transform.position + " ROT " + playerRb.transform.rotation
            //    + " VEL " + playerRb.velocity);
            prepareShotPlayerInitPos = playerRb.transform.position;
            prepareShotPlayerInitRot = playerRb.transform.rotation;
            initShotRpc = true;
        }

        if (!photonView.IsMine && !initShotRpc)
        {

            //print("DBGSHOT12# init IsMine " + photonView.IsMine);
            rb.transform.position = playerRb.transform.position = prepareShotPlayerInitPos;
            rb.transform.rotation = playerRb.transform.rotation = prepareShotPlayerInitRot;
            //rb.velocity = playerRb.velocity = prepareShotPlayerInitVel;
            rb.velocity = playerRb.velocity = Vector3.zero;

            peerPlayer.setPlayersColliders(peerPlayer.getPlayerGameObject(), true);

            //print("BEZIERDBG123 PREPARESHOT GET RB POS " + rb.transform.position
            //    + " VELOCITY " + playerRb.velocity);
            initShotRpc = true;
        }

        //print("DBGSHOT12# isDelay " + isDelay + " IsMine " +  photonView.IsMine);

        //delay to draw shot curve
        if (isDelay)
            return false;

        if (!initShot && photonView.IsMine)
        {
            if (!shotType.Contains("volley") && UnityEngine.Random.Range(0, 2) == 1)
                shotRotationDelay = animationOffsetTime[shotType] * UnityEngine.Random.Range(0.3f, 0.38f);
        }

        if (photonView.IsMine &&
            !prepareShotRpcSend)
        {
            //(clipOffsetTime - 0.15f))) {
            //print("SHOTDBDDDAG1 NORTIME " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            sendPrepareShotRpc(shotType, 
                               playerRb.transform.position, 
                               playerRb.transform.rotation,
                               shotRotationDelay);
            prepareShotRpcSend = true;
            touchLocked = true;
            touchCount = 0;
            isTouchBegin = false;
            //print("DBGprepare1234 normTime");
        }

        //if (isDelay2)
        //    return false;
        
        if (!initShot)
        {
            /*Don't block touch here */
          
            playerRb.transform.position = prepareShotPlayerInitPos;
            playerRb.transform.rotation = prepareShotPlayerInitRot;
            playerRb.velocity = Vector3.zero;

            //print("#PREPARESHOTSTARTTIME ANIM START " + PhotonNetwork.Time);

            //print("BEZIERDBG123 PREPARE SHOT START POS #### "
            //   + playerRb.transform.position + " angle " +
            //   playerRb.transform.eulerAngles);

            animator.Play(shotType, 0, 0.0f);
            animator.Update(0.0f);
            initShot = true;
            if (shotType.Equals("3D_volley_before"))
            {
                shotType = "3D_volley";
            }
            
            //playerRb.velocity = Vector3.zero;
        }

        clipOffsetTime = animationOffsetTime[shotType];

        //print("DBGSHOT12## " + shotType + " OFFSET TIME " + clipOffsetTime + " noramlized time " +
        //    animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " VALUE " 
        //    + animator.GetCurrentAnimatorStateInfo(0).IsName(shotType)
        //    + " isMine " + photonView.IsMine);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(shotType) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > clipOffsetTime)
        {
            bool onBall = isPlayerOnBall(rbLeftToeBase,
                                         rbRightToeBase,
                                         ballRb,
                                         playerRb,
                                         shotType,
                                         ref activeBall,
                                         isMaster);


            //print("DBGSHOT12# onBall true isMine " + photonView.IsMine + " onBall  " + onBall);

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
                                        ref activeBall,
                                        isMaster);

                //todo
                onBall = true;

                //if (!isCpu)
                //    print("CPUMOVEDEBUG123X_NOCPU ONBALL RETURN " + onBall);
                //else
                //    print("CPUMOVEDEBUG123X_CPU " + onBall);
            }

            if (!photonView.IsMine)
            {
                onBall = true;
            }

            if (onBall)
            {
                if (photonView.IsMine)
                {
                    //photonView.RPC("RPC_justBeforeShot",
                    //                RpcTarget.Others,
                    //                ballRb.transform.position);
                }
                else
                {
                    if (RPC_locks.ballPosBeforeShot == INCORRECT_VECTOR)
                    {
                        ///print("NOWPOSITION RB BEFORE GKDEBUG1X BALL PoS before shot ERROR DIDN' COME ???????");
                        //print("DBGSHOT12## LOCKED");

                        Time.timeScale = 0f;
                        return false;

                        //print("BEZIERDBG12 RESUMED " + Time.timeScale);
                    }


                    //print("DBGSHOT12## NOT LOCKED");

                    //else
                    //{

                    /*print("GKDEBUG1X ballPos CORRECTED FROM " + ballRb.transform.position + " TO "
                         + RPC_locks.ballPosBeforeShot + " DIST " +
                         Vector3.Distance(ballRb.transform.position, RPC_locks.ballPosBeforeShot));*/

                    //ballRb.transform.position = RPC_locks.ballPosBeforeShot;
                    RPC_locks.ballPosBeforeShot = INCORRECT_VECTOR;
                    //}*/
                }

                /*                if (!shotType.Contains("volley"))
                                {
                                    print("NOWPOSITION RB #### " + rb.transform.position + " prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos +
                                        " prepareShotPlayerInitPos " + prepareShotPlayerInitPos);
                                    rb.transform.position = prepareShotJustBeforeShotPos;
                                    print("NOWPOSITION RB ### " + rb.transform.position + " rotation before " + rb.transform.eulerAngles);
                                    rb.rotation = prepareShotJustBeforeShotRot;
                                    print("NOWPOSITION RB ### " + rb.transform.position + " rotation after " + rb.transform.eulerAngles);
                                } else
                                {*/
                /*print("DBGSHOT rb pos  player was " + rb.transform.position +
                    " beforePos  " + prepareShotJustBeforeShotPos + " eulerAngle before " +
                    rb.transform.eulerAngles);

                    rb.transform.position = prepareShotJustBeforeShotPos;                                                 
                    rb.transform.rotation = prepareShotJustBeforeShotRot;*/
                //}

                /*print("DBGSHOT rb pos  player was " + rb.transform.position +
                    " beforePos  " + prepareShotJustBeforeShotPos + " eulerAngle after " +
                    rb.transform.eulerAngles);*/
                /*  GameObject toDelete = new GameObject();

                  toDelete.transform.position = prepareShotPlayerInitPos;
                  toDelete.transform.rotation = prepareShotPlayerInitRot;
                  rotateGameObjectTowardPoint(ref toDelete, endPosOrg, 1f);
                  */
                //toDelete.transform.rotation = prepareShotPlayerInitRot;
                /*print("DBGSHOT " 
                    + " shotType " + shotType + " outShotStart " + outShotStart
                    + " ballRb.Transform " + ballRb.transform.position
                    + " Vector3.Distance " + Vector3.Distance(ballRb.transform.position, outShotStart)
                    + " prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos
                    + " rb.transform.position " + rb.transform.position);*/

                /*Start shoting */

                //if (!PhotonNetwork.IsMasterClient)
                //{
                    /*print("DBGSHOT " 
                    + " shotType " + shotType + " outShotStart " + outShotStart
                    + " ballRb.Transform " + ballRb.transform.position
                    + " Vector3.Distance " + Vector3.Distance(ballRb.transform.position, outShotStart)
                    + " prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos
                    + " rb.transform.position " + rb.transform.position);*/
                //}

                    shotActive = true;



                //RPC_expectedSequenceNumber[(int) RPC_ACK.GK_PREDICT_CONFIRM]++;
                initShot = false;
                initVolleyShot = false;
                prepareShotRpcSend = false;
                playerOnBall = false;

                /*print("DBGSHOT startShooting ballPos " + ballRb.transform.position + " rb.transform.position " +
                        rb.transform.position 
                        + " outShotStart " + outShotStart);*/

                //outShotStart = ballRb.transform.position;
                ball[activeBall].setIsPlayerDownCollided(false);
                ball[activeBall].setIsWallCollided(false);

                ball[activeBall].setIsWallCollided(false);

                peerPlayer.setGkHelperImageVal(true);
                //print("DBG342344COL try to set setGkHelperImageVal set true");
                //if (isMaster)
                //{
                if (photonView.IsMine)
                    setShotButtonsInactive();
      
                //if (shotType.Contains("volley"))
                //{
                //    print("#VOLLEYDBG ballPos " + ballRb.transform.position +
                //     " ballLocalPos " + InverseTransformPointUnscaled(rb.transform, ballRb.transform.position)
                //     + " rb.transform.position " + rb.transform.position);
                //}

                //   updateShotPos();
                //print("DBGSHOT12## start shooting");


                //}

                return true;
            }
            else
            {
                //if (!isCpu)
                //{
                clearPreShotVariables();
                isTouchBegin = false;
                //}
                //else
                // {
                //    peerPlayer.clearPreShotVariables();
                //}

                return false;
            }

            //print("ENTERED ISMOVING STRAING");
        }
        else
        {
            //print("NOWPOSITION RB BEFORE " + rb.transform.position + " prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos +
            //   " prepareShotPlayerInitPos " + prepareShotPlayerInitPos + " animator.GetCurrentAnimatorStateInfo(0).normalizedTime " +
            //   animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            //if (!shotType.Contains("volley"))
            //{
            /*print("DBGSHOT rb.transform.position  " + rb.transform.position + " prepareShotJustBeforeShotPos "
                    + prepareShotJustBeforeShotPos
                    + " rotationRB " + rbTmpGameObj.transform.eulerAngles);*/
            
            rb.transform.position = Vector3.MoveTowards(rb.transform.position,
                                                        prepareShotJustBeforeShotPos,
                                                        4.0f * Time.deltaTime);
            if (!shotType.Contains("volley") &&
                (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > shotRotationDelay))
            {
                rbTmpGameObj.transform.position = rb.transform.position;
                rbTmpGameObj.transform.rotation = prepareShotJustBeforeShotRot;

                RblookAtDirectionGK(rb,
                                    rbTmpGameObj,
                                    15.0f);
            }

            preShotPositionActive = true;
            preShotPositionVal = rb.transform.position;
            //}

            ///print("NOWPOSITION RB " + rb.transform.position + " prepareShotJustBeforeShotPos " + prepareShotJustBeforeShotPos +
           ///      " prepareShotPlayerInitPos " + prepareShotPlayerInitPos + " animator.GetCurrentAnimatorStateInfo(0).normalizedTime " +
            //     animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            ///rb.transform.position = prepareShotJustBeforeShotPos;
           // print("NOWPOSITION RB " + rb.transform.position + " rotation before " + rb.transform.eulerAngles);
            //rb.rotation = prepareShotJustBeforeShotRot;
           // print("NOWPOSITION RB " + rb.transform.position + " rotation after " + rb.transform.eulerAngles);

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
                        //if (!isCpu)
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
            //print("BALLENDPOS volley start");
            Vector3 shotGoalEndPos = updateEndShotPos();
            if (photonView.IsMine)
                RblookAtWSPoint(rb, shotGoalEndPos);
            else
                RblookAtWSPoint(rb, outShotEnd);

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
            rbLeftToeBase, rbRightToeBase, ballRb[activeBall], rb, "move", ref activeBall, isMaster);
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
                                  bool isMaster)

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
        //if (isMaster)
        //{
        //    goalXOff = goalXOffsetCpu;
        //}

        float minXDistance = minDistToOnBAll - 0.1f;
        float minZDistance = minDistToOnBAll - 0.1f;
        ///TODO
        //float minZDistance = minDistToOnBAll + 0.2f;

        /*ball should never be on higher position than PITCH_WIDTH - BALL_RADIUS */
        //if (!isMaster)
        //{
        minXDistance += (BALL_NEW_RADIUS - 0.1f);
        //}

        if (Mathf.Abs(rb.transform.position.z) <= 1.5f)
            minZDistance += BALL_NEW_RADIUS + 0.15f;

        if (isAnimPlaying && !isPlaying(animator, "3D_volley", 1f))
        {
            minXDistance += 0.42f;
            //minZDistance += 0.42f;
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


        //print("#DBGPLAYERPOS " + rb.transform.position + " isPlayerOut " + isPlayerOut
        //    + " minZDistance " + minZDistance
        //    + " isMaster " + isMaster);

        //if (!isCpu)
        //print("DEBUG2345ANIMPLAY CLEAN CALC VALUES " + isPlayerOut
        //    + " PITCH_HEIGHT_HALF - minZDistance " + (PITCH_HEIGHT_HALF - minZDistance).ToString("F4")
        //    + " MINDISTANCe " + minZDistance);

        //if ((isMaster && (rb.transform.position.z <= minZDistance)) ||
        ///   (!isMaster && (rb.transform.position.z >= -minZDistance)) ||
      //            (isPlayerOut && (!isMaster || isAnimPlaying)))

        if ((Mathf.Abs(rb.transform.position.z) <= minZDistance) ||
             isPlayerOut)
        {
            //if (!isMaster)
            //{

            ///print("DBGPLAYERPOS entered");
            if (isMaster)
            {
                if (rb.transform.position.z < -(PITCH_HEIGHT_HALF - minZDistance))
                {
                    rb.transform.position =
                        new Vector3(rb.transform.position.x, 0f, -(PITCH_HEIGHT_HALF - minZDistance));
               //     print("DBGPLAYERPOS correct pos " + rb.transform.position);
                }
                else
                {
                    if (rb.transform.position.z > -minZDistance)
                    {
                        //print("#DBGrB rb.transform.position " + rb.transform.position + " minZDistance " + -minZDistance);

                        rb.transform.position =
                            new Vector3(rb.transform.position.x, 0f, -minZDistance);
                    }
                }
            } else
            {
                if (rb.transform.position.z > (PITCH_HEIGHT_HALF - minZDistance))
                {
                    rb.transform.position =
                        new Vector3(rb.transform.position.x, 0f, (PITCH_HEIGHT_HALF - minZDistance));
                }
                else
                {
                    if (rb.transform.position.z < minZDistance)
                    {
                        rb.transform.position =
                            new Vector3(rb.transform.position.x, 0f, minZDistance);

                        //print("#DBGrB rb.transform.position " + rb.transform.position + " minZDistance " + minZDistance);

                    }
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
            //print("DBGPLAYERPOS oUT of pitch 1" + rb.transform.position);
            //}
            //else
            //{
            //    print("#DBGPLAYERPOS outOfPitch2 " + prevRbPos);
            //   rb.transform.position = prevRbPos;
            //}
        }

        prevRbPos = rb.transform.position;
    }

    private void calculateAndSetWinTieMatchIntroValues(int teamAstrength, int teamBstrength)
    {
        /*float levelFactor = 0;
        int winPoints = 50;
        int tiePoints = 20;
        float level = (float)Globals.level;

        if (Globals.level <= 2)
            levelFactor = 0.05f + (level * 0.07f);
        else
        {
            if (Globals.level == 3)
            {
                levelFactor = 0.27f;
            }
            else if (Globals.level == 4)
            {
                levelFactor = 0.50f;
            }
            else if (Globals.level == 5)
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

        winPoints = (int)((float)winPoints * levelFactor);
        tiePoints = (int)((float)tiePoints * levelFactor);

        /*values should be beetween 0 and 100 */
        /*winPoints = Mathf.Clamp(winPoints, 7, 130);
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
            */

        //for beta multiplayer version
        int winPoints = 20;
        int tiePoints = 10;

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
        if (photonView.IsMine)
            setShotButtonsInactive();
    }

    private void setShotButtonsInactive()
    {
        volleyButton.setButtonState(false);
        lobButton.setButtonState(false);
    }

    public void gkMoveUpAnim()
    {
        if ((isBallOnYourHalf(rb.transform.position) && !peerPlayer.getShotActive()) ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            peerPlayer.getShotActive())
            return;

        if (isAnimatorPlaying(animator)) return;

        //stepUpDown(animator, rb, 0.05f, "3D_run");
        //if (!isPlaying(animator, "3D_walk", 1.0f))
        //{0
        animator.Play("3D_walk", 0, 0.0f);
        //}

    }

    public void gkMoveDownAnim()
    {
        if ((isBallOnYourHalf(rb.transform.position) && !peerPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            peerPlayer.getShotActive())
            return;

        if (isAnimatorPlaying(animator)) return;

        //isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);
        //if (isAnyAnimationPlaying) return;

        animator.Play("3D_back_run", 0, 0.0f);
    }

    public void gkSideLeftAnim()
    {
        if ((isBallOnYourHalf(rb.transform.position) && !peerPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            peerPlayer.getShotActive())
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
        if ((isBallOnYourHalf(rb.transform.position) && !peerPlayer.getShotActive()) ||
            Mathf.Abs(rb.transform.position.z) > PITCH_HEIGHT_HALF ||
            joystick.getPointerId() != -1 ||
            !gameStarted ||
            isGamePaused() ||
            (!isGkTrainingActive() && (Globals.isTrainingActive || isBonusActive)) ||
            peerPlayer.getShotActive())
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
        return false;
    }

    public bool isRunPaused()
    {
        return false;
    }

    public bool isGamePaused()
    {
        //multiplayer cannot be pause
        return false;

        //if (isTrainingActive &&
        //    trainingScript.isTraningPaused())
        //    return true;
        if (gamePausedScript.isGamePaused())
            return true;

        return false;
    }

    public bool isShotTrainingActive()
    {
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
        return false;
    }

    public Vector3 updateEndShotPos()
    {
        float dist = 0.0f;
        RaycastHit hit;
        Vector3 hitPoint = new Vector3(0.0f, 0.0f, 14.0f);
        //Vector3 endShotPos = Vector3.zero;
        Vector3 endShotPos = INCORRECT_VECTOR;
        Plane plane = goalUpPlane;

        if (!PhotonNetwork.IsMasterClient)
        {
            hitPoint = new Vector3(0.0f, 0.0f, -PITCH_HEIGHT_HALF);
            plane = goalDownPlane;
        }

        //if (!photonView.IsMine)
        //    ballEndPos = new Vector2(outShotEnd.x, outShotEnd.z);


        ///print("BALLENDPOS outShotEnd " + outShotEnd + " endPos " + endPos);

        Ray ray = m_MainCamera.ScreenPointToRay(endPos);
        if (plane.Raycast(ray, out dist))
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

        //if (!photonView.IsMine)
        //{
        //    rb.velocity = (rb.transform.position - playerPrevPos).normalized * playerVelocity;
        //}

        playerPrevPos = rb.transform.position;
        playerPrevPosTime = Time.time;    
    }

    private void updateShotPos()
    {
        float dist = 0.0f;
        RaycastHit hit;
        Vector3 hitPoint = new Vector3(0.0f, 0.0f, PITCH_HEIGHT_HALF);
        Plane plane = goalUpPlane;

        if (!PhotonNetwork.IsMasterClient)
        {
            hitPoint = new Vector3(0.0f, 0.0f, -PITCH_HEIGHT_HALF);
            plane = goalDownPlane;
        }

        //print("DEBUG1 STARTHRNEW UPDATE SHOT START " + startPos + " MID " + midPos + " END " + endPos);

        midPos = updateMidTouchPos(startPos, endPos);
        //print("DEBUG12345XA MIDPOS " + midPos);
        startPos3 = ballRb[activeBall].transform.position;
        endPosOrg = Vector3.zero;
        midPos3 = Vector3.zero;

        Ray ray = m_MainCamera.ScreenPointToRay(startPos);
        if (plane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            //if (hitPoint.y < 0)
            //    hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POS " + hitPoint);
            curveStartPos3 = hitPoint;
        }

        ray = m_MainCamera.ScreenPointToRay(midPos);
        if (plane.Raycast(ray, out dist))
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
        if (plane.Raycast(ray, out dist))
        {
            hitPoint = ray.GetPoint(dist);
            //if (hitPoint.y < 0)
            //    hitPoint.y = 0.0f;

            //print("SHOTVECTOR3POS " + hitPoint);
            curveEndPos3 = hitPoint;
        }

        ray = m_MainCamera.ScreenPointToRay(endPos);
        if (plane.Raycast(ray, out dist))
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

        ///print("timeToShot " + timeToShot + " maxTimeToShot " + maxTimeToShot);

        if (timeToShot > (maxTimeToShot - 1f))
        {
            /*add if not training!!*/
            /*Do something with ball*/
            //printGameEventsInfo("Time to shoot up");
            /*if shot already started then it's fine :-) */
            if ((touchCount > 0) ||
                 preShotActive ||
                 shotActive ||
                 peerPlayer.getShotActive() ||
                 peerPlayer.getPreShotActive())
            return false;   

            /*if ((ballRb[activeBall].transform.position.z > 0.0f))
            {
                if (peerPlayer.getShotActive() ||
                    peerPlayer.getPreShotActive())
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
            }*/

            if (timeToShot > maxTimeToShot)
                return true;
        }

        timeToShot += Time.deltaTime;
        prevZballPos = ballRb[activeBall].transform.position.z;

        //get one second to synchronize 
        //if (timeToShot > (maxTimeToShot - 1f))
        //    timeToShot = maxTimeToShot - 1f;

        //print("timeToShot " + timeToShot);

        if (timeToShot > (maxTimeToShot - 1f))
            timeToShotText.text = ((int) maxTimeToShot - 1f).ToString();
        else
            timeToShotText.text = ((int) timeToShot).ToString();

        if ((maxTimeToShot - timeToShot) <= 4.0f)
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


        if (isBallOnYourHalf(rb.transform.position, ballRb[activeBall].transform.position))
        {
            matchStatistics.setBallPossession("teamA", deltaTime);
        } else
        {
            matchStatistics.setBallPossession("teamB", deltaTime);
        }

        /*if (ballRb[activeBall].transform.position.z < 0.0f && ballPrevPosition.z < 0.0f)
            matchStatistics.setBallPossession("teamA", deltaTime);
        else
        {
            if (ballRb[activeBall].transform.position.z > 0.0f && ballPrevPosition.z > 0.0f)
            {
                matchStatistics.setBallPossession("teamB", deltaTime);
            }
        }

        ballPrevPosition = ballRb[activeBall].transform.position;*/
    }

    public int getMatchTimeMinute()
    {
        return minuteOfMatch;
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
        minuteOfMatch = minutes;
        currentTimeOfGame += Time.deltaTime;

        //print("CURRENT TIME OF GAME " + currentTimeOfGame + " MAX " + timeOfGameInSec + 
        //   " virtualTimeSeconds " + virtualTimeSeconds);

        if (isShotActive() ||
            isPreShotActive() ||
            peerPlayer.getShotActive() ||
            peerPlayer.getPreShotActive())
            return false;

        if (currentTimeOfGame >= (timeOfGameInSec + stoppageTime))
        {
            if (Globals.gameInGroup ||
                Globals.isFriendly ||
                Globals.isMultiplayer ||
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
                                       bool isMaster)
    {
        bool isOnBall = false;
        if (preShotActive || shotActive)
        {
            /*isOnBall = isPlayerOnBall(
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           rb,
                           shotType,
                           ref activeBall,
                           isMaster);*/
            isOnBall = true;
        }
        else
        {
            if (playerOnBall)
                isOnBall = true;


            /*isOnBall = isPlayerOnBall(
                           rbLeftToeBase,
                           rbRightToeBase,
                           ballRb,
                           rb,
                           "move",
                           ref activeBall,
                           isMaster);*/
        }

        playerOutOfPitch(animator,
                         rb,
                         ref prevRbPos,
                         rbLeftToeBase,
                         rbRightToeBase,
                         isOnBall,
                         isMaster);
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

        if (!actionName.Equals("rpcBallUpdate"))
            setBallAngularVelocity(rb);

        switch (actionName)
        {
            case "rpcBallUpdate":
                //ballRb.transform.position = lastRpcBallPos;
                //ballRb.transform.position += velocity * lag;
                /*print("DBGPLA rpcBallUpdate BALL RPC DIST "
                    + Vector3.Distance(ballRb.transform.position, lastRpcBallPos)
                    + " BALLRB TRANSFORM " + ballRb.transform.position
                    + " rpc_ballPos " + rpc_ballPos
                    + " GKPOS " + rb.transform.position
                    + " peerShotStart " + peerPlayer.getOutShotStart()
                    + " peerShotMid " + peerPlayer.getOutShotMid()
                    + " peerShotEnd " + peerPlayer.getOutShotEnd()
                    + " peerEndPosOrg " + peerPlayer.getEndPosOrg());*/

                Vector3 rpc_ballPredictedPos =
                        new Vector3(rpc_ballPos.x, rpc_ballPos.y, rpc_ballPos.z);
                if ((bufferedBallPosCurrIdxPop == bufferedBallPosMax) ||
                    (bufferedBallPos[bufferedBallPosCurrIdxPop, 4].x > 0))
                {
                    bufferedBallPosCurrIdxPush = 0;
                    bufferedBallPosCurrIdxPop = 0;
                    bufferedBallPosMax = bufferedBallPos.GetLength(0);
                    mainUpdateTypeActive = false;
                }

                /*print("#DBGBUFFER bufferPop before ## " + ballRb[activeBall].velocity
                       + " bufferedBallPosMax " + bufferedBallPosMax);*/

                if ((bufferedBallPosCurrIdxPush != 0) &&
                     mainUpdateTypeActive)
                {                
                    mainUpdateTypeActive = true;

                    if (bufferedBallPosCurrIdxPush != bufferedBallPosCurrIdxPop)
                    {
                        ballRb[activeBall].transform.position = bufferedBallPos[bufferedBallPosCurrIdxPop, 0];
                        //print("BUFFEREDPOSITION " + bufferedBallPos[bufferedBallPosCurrIdxPop, 0] + " idx " +
                        //    bufferedBallPos[bufferedBallPosCurrIdxPop, 3].x
                        //+ " ballRb " + ballRb[activeBall].transform.position);

                        ballRb[activeBall].velocity = bufferedBallPos[bufferedBallPosCurrIdxPop, 1];
                        ballRb[activeBall].angularVelocity = bufferedBallPos[bufferedBallPosCurrIdxPop, 2];
                        bufferedBallPosCurrIdxPop++;
                    }

                    //print("#DBGBUFFER bufferPop " + ballRb[activeBall].velocity
                    //    + " bufferedBallPosCurrIdxPop " + bufferedBallPosCurrIdxPop
                    //    + " bufferedBallPosCurrIdxPush " + bufferedBallPosCurrIdxPush
                    //    + " bufferedBallPosMax " + bufferedBallPosMax
                    //    + " bufferedBallPosCurrIdxPop " + bufferedBallPosCurrIdxPop);

                    //the queue is empty
                    /* if (bufferedBallPosCurrIdxPush == bufferedBallPosCurrIdxPop)
                     {
                         bufferedBallPosCurrIdxPush = 0;
                         bufferedBallPosCurrIdxPop = 0;
                         print("DBGBUFFER clear buffer ####");
                         mainUpdateTypeActive = false;
                     }*/
                }
                else
                {
                    if (!rpc_isBallOut &&
                       ((Time.time - rpcMain_updateTime) < 1.0f))
                    {
                        rpc_ballPredictedPos = rpc_ballPredictedPos + (rpc_ballVelocity * rpc_mainLag);
                        ballRb[activeBall].transform.position = Vector3.MoveTowards(ballRb[activeBall].transform.position,
                                                                                    rpc_ballPredictedPos,
                                                                                    6f * Time.deltaTime);
                        ballRb[activeBall].angularVelocity = rpc_ballAngularVelocity;
                    }
                }
                //add ball velocity 0?


                //3f * Time.deltaTime);
                //print("DBGBALLPOSITION rpc_ballPredictedPos " + rpc_ballPredictedPos + " rpc_ballVelocity " +
                //    rpc_ballVelocity + " rpc_mainLag " + rpc_mainLag
                //    + " rpc_ballPos " + rpc_ballPos
                //    + " ballRb[activeBall].transform.position " + ballRb[activeBall].transform.position
                //    + " isBallOut " + isBallOut);
                break;
            case "volleyShot":
                if (!isPlaying(animator, "3D_volley", 1.0f))
                    volleyShotUpdateBallPos(rb, rbRightToeBase, rbRightFoot, isCpu);
                break;
            case "3D_shot_left_foot":
            case "3D_shot_right_foot":
                normalShotUpdateBallPos(rb, actionName, animator);
                break;
            case "bodyMain":
                //print("DBGTOUCHLAST bodyMain make!");
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

        if (PhotonNetwork.IsMasterClient)
        {
            m_MainCamera.transform.eulerAngles =
                  new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
        } else
        {
            m_MainCamera.transform.eulerAngles =
                new Vector3(cameraSettings[cameraIdx][2], 180.0f, 0.0f);
        }

        m_MainCamera.GetComponent<Camera>().fieldOfView =
               cameraSettings[cameraIdx][0];
        cameraMovement(noLerpMove, -1);
    }

    public void cameraChanged(bool noLerpMove, int cameraIdx)
    {
        m_MainCamera.transform.eulerAngles =
              new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
        m_MainCamera.GetComponent<Camera>().fieldOfView =
               cameraSettings[cameraIdx][0];
        cameraMovement(noLerpMove, cameraIdx);
    }

    ///Vector3[] cameraVel = new Vector3[10];

    Vector3 cameraVel = Vector3.zero;

    public void cameraMovement(bool noLerpMove, int camIdx)
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
            if (!PhotonNetwork.IsMasterClient)
            {
                yDist = Mathf.InverseLerp(0f, 10f, rb.transform.position.z);
            }

            yDist = yDist - ((1f - yDist) * 0.85f);
            //print("#DBGYDIST " + yDist + " rb.transform.position.z " + rb.transform.position.z);
            yDist = Mathf.Clamp(yDist, 0f, 1f);

            if (PhotonNetwork.IsMasterClient)
            {
                m_MainCamera.transform.eulerAngles =
                    new Vector3(
                        cameraSettings[cameraIdx][2] - (Mathf.InverseLerp(-10, 0f, rb.transform.position.z) * 4f), 0.0f, 0.0f);
            } else
            {
                m_MainCamera.transform.eulerAngles =
                    new Vector3(
                        cameraSettings[cameraIdx][2] - (Mathf.InverseLerp(10, 0f, rb.transform.position.z) * 4f), 180.0f, 0.0f);
            }

            if (Mathf.Abs(rb.transform.position.z) > 10f)
            {
                yDist = 1f;
                if (PhotonNetwork.IsMasterClient)
                {
                    m_MainCamera.transform.eulerAngles =
                        new Vector3(cameraSettings[cameraIdx][2], 0.0f, 0.0f);
                } else
                {
                    m_MainCamera.transform.eulerAngles =
                       new Vector3(cameraSettings[cameraIdx][2], 180.0f, 0.0f);
                }
            }

            m_MainCamera.GetComponent<Camera>().fieldOfView =
                Mathf.Min(47f, 41f + (Mathf.Abs(rb.transform.position.z)));
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
            if (PhotonNetwork.IsMasterClient)
            {
                m_MainCamera.transform.position =
                new Vector3(
                        rb.transform.position.x,
                        yDist,
                        rb.transform.position.z - zDist);
            } else
            {
                m_MainCamera.transform.position =
                     new Vector3(
                        rb.transform.position.x,
                        yDist,
                       rb.transform.position.z + zDist);
            }
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

            Vector3 newCameraPos = Vector3.zero;
            if (PhotonNetwork.IsMasterClient)
            {
                newCameraPos = m_MainCamera.transform.position =
                     new Vector3(
                 Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f),
                            Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
                            Mathf.Lerp(m_MainCamera.transform.position.z,
                            rb.transform.position.z - zDist,
                            0.2f));
            }
            else
            {
                newCameraPos = m_MainCamera.transform.position =
                  new Vector3(
                    Mathf.Lerp(m_MainCamera.transform.position.x, rb.transform.position.x, 0.2f),
                         Mathf.Lerp(m_MainCamera.transform.position.y, yDist, 0.1f),
                         Mathf.Lerp(m_MainCamera.transform.position.z,
                         rb.transform.position.z + zDist,
                         0.2f));
            }

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
        if (Mathf.Abs(m_MainCamera.transform.position.z) >= Mathf.Abs(cameraSettings[cameraIdx][5]))
        {

            if (PhotonNetwork.IsMasterClient)
            {
                m_MainCamera.transform.position =
                    new Vector3(m_MainCamera.transform.position.x,
                                m_MainCamera.transform.position.y,
                                cameraSettings[cameraIdx][5]);
            }
            else
            {
                m_MainCamera.transform.position =
                      new Vector3(m_MainCamera.transform.position.x,
                                  m_MainCamera.transform.position.y,
                                  Mathf.Abs(cameraSettings[cameraIdx][5]));
            }
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
        float cameraSpeed = 0.010f;

        //print("cameraIntro " + m_MainCamera.transform.eulerAngles + " transform.position " +
        //    m_MainCamera.transform.position +  " photonView " + photonView.IsMine);

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
                                                          m_MainCamera.transform.position.y + (cameraSpeed / 3.4f),
                                                          m_MainCamera.transform.position.z - 0.001f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.01f, 0.0f);

            return;
        }


        if (realTime > 2.0f && realTime < 7f)
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y + (cameraSpeed / 3.4f),
                                                          m_MainCamera.transform.position.z - 0.006f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.04f, 0.0f);
        }
        else if (realTime > 7f)
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                        m_MainCamera.transform.position.y + (cameraSpeed / 3.4f),
                                                        m_MainCamera.transform.position.z - 0.008f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.06f, 0.0f);
        }
        else
        {
            m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x,
                                                          m_MainCamera.transform.position.y + (cameraSpeed / 3.4f),
                                                          m_MainCamera.transform.position.z - 0.004f);
            m_MainCamera.transform.eulerAngles = m_MainCamera.transform.eulerAngles - new Vector3(0.0f, 0.02f, 0.0f);
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

    public void rotateGameObjectTowardPoint(ref GameObject gObject,
                                             Vector3 lookPoint,
                                             float slerpT)
    {
        Vector3 lookDirection = Vector3.zero;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.up);

        ///if (!isMaster)
        //{
        //    gObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        //}

        lookDirection = (lookPoint - gObject.transform.position).normalized;
        lookDirection.y = 0.0f;

        //print("DBGCOLLISIONCALC1024D updateLastGk updateLastGkTouchPos lookDirection lookDirection " + lookDirection
        //    + " lookPoint " + lookPoint
        //    + " gObject.transform.position " + gObject.transform.position
        //    + " Vector3.forward " + Vector3.forward);

        //if (isMaster)
        lookAt = Quaternion.LookRotation(lookDirection);
        //else
        //    lookAt = Quaternion.LookRotation(Vector3.forward, lookDirection);



        gObject.transform.rotation =
               Quaternion.Slerp(gObject.transform.rotation, lookAt, slerpT);
        //if (!isMaster)
        //{
        //}


        //if (!isMaster)
        //{
        //    print("DBG342344COL ROTATION updateLastGk updateLastGkTouchPos lookDirection gObject.transform.rotation " + gObject.transform.eulerAngles
        //       + " lookPoint " + lookPoint
        //       + " gObject.transform.position " + gObject.transform.position);
        //}
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
                          bool isMaster,
                          string shotType)
    {
        bool isAnyAnimationPlaying = checkIfAnyAnimationPlaying(animator, 1.0f);

        Quaternion lookOnLook = Quaternion.LookRotation(Vector3.up);
        Vector3 shotDirection3D = Vector3.zero;

        if (((!preShotActive || onBAll != PlayerOnBall.ONBALL) &&
            !isAnyAnimationPlaying) ||
            (!isMaster && isPlaying(animator, "3D_run", 1.0f)))
        {
            //if (!isCpu)
            if (playerDirection == Vector3.zero)
                if (isMaster)
                    playerDirection = new Vector3(0f, 0f, 1f);
                else
                    playerDirection = new Vector3(0f, 0f, -1f);
            //if (playerDirection == Vector3.zero)
            //        playerDirection = new Vector3(0f, 0f, 1f);

            lookOnLook = Quaternion.LookRotation(playerDirection);

            ///print("#DBGLOOKPOINT 5 gameStarted " + doesGameStarted()
            //                    + " ball[1].transform.position, " +
            //                    playerDirection);
        }
        else
        {
            //if (!isMaster)
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


//            print("#DBGLOOKPOINT 6 gameStarted " + doesGameStarted()
//                                + " ball[1].transform.position, " +
//                                playerDirection);        
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

        Vector3 tmpBall = ballPos;

        ///if (!isMaster)
        //    print("DBGCOLLISIONCALC1024D updateLastGk tmpBall " + tmpBall);
        Vector3 cornerPointLeft = new Vector3(-pointX, 0.0f, pointZ);
        Vector3 cornerPointRight = new Vector3(pointX, 0.0f, pointZ);

        //if (!isMaster)
        //{
        //    cornerPointLeft = new Vector3(pointX, 0.0f, pointZ);
        //    cornerPointRight = new Vector3(-pointX, 0.0f, pointZ);
            //  print("DBGCOLLISIONCALC1024D updateLastGk updateLastGk updateLastGkTouchPos corentPoint Left " + cornerPointLeft + " cornerPointRight " +
            //      cornerPointRight);
        //}

        rotatedRbToBall.transform.position = rb.transform.position;
        //print("DBGCOLLISIONCALC1024D updateLastGk updateLastGkTouchPos rotatedRbToBall.transform.position init " + rotatedRbToBall.transform.position);
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
                    //print("DBGCOLLISIONCALC1024D updateLastGk updateLastGkTouchPos tmpBall out1 " + tmpBall + " rotatedRbToBall " +
                    //    rotatedRbToBall.transform.eulerAngles  + " step " + step);
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
                    //print("DBGCOLLISIONCALC1024D updateLastGk updateLastGkTouchPos tmpBall out2 " + tmpBall + "  rotatedRbToBall " +
                    //    rotatedRbToBall.transform.eulerAngles + " step " + step);
                    return tmpBall;
                }
            }

            step += 1.0f;
            if (Mathf.Abs(ballPos.x - step) > PITCH_WIDTH_HALF &&
                Mathf.Abs(ballPos.x + step) > PITCH_WIDTH_HALF)
            {
                //print("DBGCOLLISIONCALC1024D updateLastGk updateLastGkTouchPos tmpBall notfound " + tmpBall);

                //print("DEBUGLASTTOUCHLUCKXYU NOT FOUND!!!!!!!");
                /*if no correct towards ball rotation 
                 * was not found (what may happen when you are close to goal) don't rotate them at all*/
                rotatedRbToBall.transform.eulerAngles = Vector3.zero;
                if (!isMaster)
                    rotatedRbToBall.transform.eulerAngles = new Vector3(0, 180f, 0f);

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
                             ref SHOTVARIANT2 type,
                             bool isMaster)
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

        if (distMidFromLine <= 1.0f)
            distMidFromLine = 0f;

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
        //if (!isCpu)
        //{
        //    lastDistFromMidLine = distMidFromLine;
        // }
        /*if (!isCpu)
            print("DEBUG2112XXF STARTHRNEW " + startPos + " MIDPOS " + midPos + " ENDPOS " + endPos + " lineSTARTEND " 
                + lineStartEnd + " DISTMIDFROMLINE " + distMidFromLine + " midPosV2 " + midPosV2
                + " LOCAL3SHOT " + localMidPos3);*/

        if (endPosOrg.y < ballRadius)
            endPosOrg.y = ballRadius;

        /* Shot straight */ /*CHANGE TO 0.5f back */
        /*if (distMidFromLine <= 1.0f && !isLobActive)
        {
           /* Vector3 shotDirection3D = (endPosOrg - ballInitPos).normalized;pre
            float velocity = 25.0f;
            if (shotSpeed != 0.0f)
                velocity = Mathf.Min(shotSpeed / 3.5f, 34.5f);

            ballShotVelocity = velocity;
            ballVelocity = shotDirection3D * velocity;
            outStartPos = ballInitPos;
            outEndPos = endPosOrg;
            outMidPos = outStartPos + ((outEndPos - outStartPos) / 2f);
            type = SHOTVARIANT2.STRAIGHT;
            return true;
        }
        else
        {*/
        string side = "left";
        ///if (isCpu)
        //{
        //     distMidFromLine = UnityEngine.Random.Range(3, CURVE_SHOT_MAX_DIST + 1);
        //}

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
        if (isMaster) {
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
        } else
        {
            if (midPos3Local.x < 0f)
            {
                side = "right";
            }
            else
            {
                if (midPos3Local.x > 0f)
                {
                    side = "left";
                }
                else
                {
                    side = "center";
                }
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
        if (!isMaster)
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
        //if (!isCpu)
        //{
            if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.3f))
            {
                minDiv = 0.78f;
            }
            else if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.5f))
            {
                minDiv = 0.65f;
            }
        //}
        //}
        //else
        // {
        //     if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.3f))
        //     {
        //         minDiv = 0.70f;
        //     }
        //     else if (distanceToGoal < (PITCH_HEIGHT_HALF * 1.4f))
        //     {
        //         minDiv = 0.65f;
        //   }
        // }

        outMidPos.y = endPosOrg.y / UnityEngine.Random.Range(minDiv, 0.85f);

        /*if (isCpu &&
            UnityEngine.Random.Range(0, 4) > 2)
        {
            outMidPos.y = endPosOrg.y / 2f;
        }*/

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
                outMidPos.y = 8.0f;
                float playersDistZ = Mathf.Abs(getPlayerPosition().z) +
                                     Mathf.Abs(peerPlayer.getPlayerPosition().z);

                if (playersDistZ < 9f)
                {
                    outMidPos.y += 1.5f;
                    if (playersDistZ < 6f)
                        outMidPos.y += 1f;
                }            
        }

        outStartPos = ballInitPos;

        if (endPosOrg.x > PITCH_WIDTH_HALF)
            endPosOrg.x = PITCH_WIDTH_HALF;

        if (endPosOrg.x < -PITCH_WIDTH_HALF)
            endPosOrg.x = -PITCH_WIDTH_HALF;

        outEndPos = endPosOrg;

        type = SHOTVARIANT2.CURVE;
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
        type = SHOTVARIANT2.CURVE;
        */

        //print("DEBUG12345XA preSHOTCALCFINALVALUES OUTBALLVEL SHOT VARIANT CURVE DIST " + distMidFromLine + " OUTSTART " 
        //   + outStartPos + " OUTMID " + outMidPos + " outEndPos " + outEndPos);


        //return false;  
        //}

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

    public Vector3 getPosLocal(GameObject gObject,
                                Vector3 pos)                  
    {
     
        return InverseTransformPointUnscaled(gObject.transform, pos);
    }

    public Vector3 getPosLocal(Rigidbody gObject,
                                Vector3 pos)
    {

        return InverseTransformPointUnscaled(gObject.transform, pos);
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

    private Vector3 getCurveShotPosLocal(Rigidbody gObject,
                                         Vector3 startPos,
                                         Vector3 endPos,
                                         float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, endPos, currentTime);

        return InverseTransformPointUnscaled(gObject.transform, m1);
    }

    private Vector3 getCurveShotPosLocal(GameObject gObject,
                                     Vector3 startPos,
                                     Vector3 endPos,
                                     float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, endPos, currentTime);

        return InverseTransformPointUnscaled(gObject.transform, m1);
    }


    /*Shot using vector 3*/
    private float calcShotDistance(Vector3 startPos,
                                   Vector3 midPos,
                                   Vector3 endPos,
                                   SHOTVARIANT2 type)
    {
        if (type == SHOTVARIANT2.STRAIGHT)
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
    Vector3 shotBallPrevPos = Vector3.zero;
    private bool shot3New(Vector3 startPos,
                          Vector3 midPos,
                          Vector3 endPos,
                          Vector3 ballVelocity,
                          ref Vector3 lastBallVelocity,
                          SHOTVARIANT2 type,
                          float currentTime)
    {
        if (type == SHOTVARIANT2.STRAIGHT)
        {
            ballRb[activeBall].velocity = ballVelocity;
            //print("DEBUG111X ballRb[activeBall]VELOCITY STRIAGHT VELOCITY SETACTIVE " + ballRb[activeBall].velocity);
            lastBallVelocity = ballRb[activeBall].velocity;

            ///print("CURRENTIME STRAIGHT");
            return false;
        }
        else
        {
            //print("GKDEBUG7 CURRENTIME ENTERED " + currentTime);
            //TOCHECK
            if (currentTime > 1.0f)
            {
                //print("DEBUGBALLVEL > 1.0f VEL: " + ballRb[activeBall].velocity + " mine " +
                //    photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
                 //   + " isFixedUpdate " + isFixedUpdate);
                return true;
            }

            /*if (currentTime >= 0.9f)
            {
                Vector3 shotCurrBallPos = BezierCuve.bezierCurve3DGetPoint(startPos,
                                                                           midPos,
                                                                           endPos,
                                                                           currentTime,
                                                                           true);
                ballRb[activeBall].velocity = new Vector3((shotCurrBallPos.x - shotBallPrevPos.x) / Time.deltaTime,
                                                          (shotCurrBallPos.y - shotBallPrevPos.y) / Time.deltaTime,
                                                          (shotCurrBallPos.z - shotBallPrevPos.z) / Time.deltaTime);
                ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

                ///print("DEBUGBALLVEL > 0.9f VEL: " + ballRb[activeBall].velocity + " mine " +
                //    photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
                //    + " isFixedUpdate "  + isFixedUpdate);
                //return false; ;
            //}
            */

            //print("DBGSHOT12 shot3New ");
            //Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
            //Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
            //Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

            float delta = 1.0f - currentTime;

            /*ballRb[activeBall].velocity = new Vector3((currPos.x - ballRb[activeBall].transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb[activeBall].transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb[activeBall].transform.position.z) / Time.deltaTime);*/

            ballRb[activeBall].transform.position = BezierCuve.bezierCurve3DGetPoint(startPos,
                                                                                     midPos,
                                                                                     endPos,
                                                                                     currentTime,
                                                                                     true);
            shotBallPrevPos = ballRb[activeBall].transform.position;
            ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            lastBallVelocity = ballRb[activeBall].velocity;

            //print("DEBUGBALLVEL < 0.9f VEL: " + ballRb[activeBall].velocity + " mine " +
            //    photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
            //    + " isFixedUpdate " + isFixedUpdate);
            //print("currentTime " + currentTime);
            return false;
        }
    }


    private bool shot3Simulation(Vector3 startPos,
                                 Vector3 midPos,
                                 Vector3 endPos,
                                 ref Vector3 ballPos,
                                 SHOTVARIANT2 type,
                                 float currentTime)
    {
        if (type == SHOTVARIANT2.STRAIGHT)
        {
            //ballRb[activeBall].velocity = ballVelocity;
            //print("DEBUG111X ballRb[activeBall]VELOCITY STRIAGHT VELOCITY SETACTIVE " + ballRb[activeBall].velocity);
            lastBallVelocity = ballRb[activeBall].velocity;

            //print("CURRENTIME STRAIGHT");
            return false;
        }
        else
        {
            //print("GKDEBUG7 CURRENTIME ENTERED " + currentTime);
            //TOCHECK
            //if (currentTime > 1.0f)
           // {
            //    print("DEBUGBALLVEL > 1.0f VEL: " + ballRb[activeBall].velocity + " mine " +
            //        photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
            //        + " isFixedUpdate " + isFixedUpdate);
            //    return true;
            //}

            if (currentTime >= 1f)
            {
                Vector3 shotCurrBallPos = BezierCuve.bezierCurve3DGetPoint(startPos,
                                                                           midPos,
                                                                           endPos,
                                                                           currentTime,
                                                                           false);

                /*UballRb[activeBall].velocity = new Vector3((shotCurrBallPos.x - shotBallPrevPos.x) / Time.deltaTime,
                                                          (shotCurrBallPos.y - shotBallPrevPos.y) / Time.deltaTime,
                                                          (shotCurrBallPos.z - shotBallPrevPos.z) / Time.deltaTime);*/
                ///ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);

                //ballRb[activeBall].transform.position = shotCurrBallPos;
                //ballPos = ballRb[activeBall].transform.position;
                ballPos = shotCurrBallPos;

                //print("DEBUGBALLVEL > 0.9f VEL: " + ballRb[activeBall].velocity + " mine " +
                //    photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
                //    + " isFixedUpdate " + isFixedUpdate);
                return true; ;
            }

            //print("DBGSHOT12 shot3New ");
            //Vector3 m1 = Vector3.Lerp(startPos, midPos, currentTime);
            //Vector3 m2 = Vector3.Lerp(midPos, endPos, currentTime);
            //Vector3 currPos = Vector3.Lerp(m1, m2, currentTime);

            float delta = 1.0f - currentTime;

            /*ballRb[activeBall].velocity = new Vector3((currPos.x - ballRb[activeBall].transform.position.x) / Time.deltaTime,
                                          (currPos.y - ballRb[activeBall].transform.position.y) / Time.deltaTime,
                                          (currPos.z - ballRb[activeBall].transform.position.z) / Time.deltaTime);*/

            /*ballRb[activeBall].transform.position = BezierCuve.bezierCurve3DGetPoint(startPos,
                                                                                     midPos,
                                                                                     endPos,
                                                                                     currentTime,                                        
                                                                                     false);*/
            ballPos = BezierCuve.bezierCurve3DGetPoint(startPos,
                                                       midPos,
                                                       endPos,
                                                       currentTime,
                                                       false);

            shotBallPrevPos = ballRb[activeBall].transform.position;
            ///ballRb[activeBall].angularVelocity = new Vector3(24.0f, 24.0f, 24.0f);
            lastBallVelocity = ballRb[activeBall].velocity;
            //ballPos = ballRb[activeBall].transform.position;

            //print("DEBUGBALLVEL < 0.9f VEL: " + ballRb[activeBall].velocity + " mine " +
            //    photonView.IsMine + " ballPosition " + ballRb[activeBall].transform.position + " curr " + currentTime
            //    + " isFixedUpdate " + isFixedUpdate);
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

    private bool isBallOnYourHalf(Vector3 playerPos, Vector3 ballPos)
    {

        if (Globals.hasTheSameSign(playerPos.z, ballPos.z))
            return true;

        //if (Mathf.Sign(playerPos.z) == Mathf.Sign(ballRb[activeBall].transform.position.z))
        //    return true;

        return false;
    }


    private bool isBallOnYourHalf(Vector3 playerPos)
    {

        if (Globals.hasTheSameSign(playerPos.z, ballRb[activeBall].transform.position.z))
            return true;

        //if (Mathf.Sign(playerPos.z) == Mathf.Sign(ballRb[activeBall].transform.position.z))
        //    return true;

        return false;
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

        //print("#DEBUGSTADIUM 1 ");
        //int teamColorChoosen = Globals.stadiumColorTeamA;
        string teamColorChoosen = Globals.stadiumColorTeamA;
        //print("teamColorChoosen " + teamColorChoosen);
        //if (teamHostID == 2)
        //   teamColorChoosen = Globals.stadiumColorTeamB;
        //teamColorChoosen = Globals.stadiumColorTeamB;

        //print("#DBGFANSCOLOR " + teamColorChoosen);

        string[] stadiumColors = teamColorChoosen.Split('|');

        string fansColor = stadiumColors[0];
        string bannerColor = stadiumColors[1];
        string fansFlagName = stadiumColors[2];

        /*print("#DEBUGSTADIUM 1 stadiumColors " + stadiumColors[0]
            + " Globals.stadiumNumber " + Globals.stadiumNumber
            + " fansColor " + fansColor
            + " bannerColor "  + bannerColor
            + " bannerColor " + fansFlagName);*/

        if (Globals.stadiumNumber == 1 ||
            Globals.stadiumNumber == 0)
        {
            int FANS_FLAG_MAX = 3;
            int numOfFansActive = 32;
            int currentFansActive = 0;
            if (Globals.graphicsQuality.Equals("LOW"))
            {
                numOfFansActive = 23;
            }
            else if (Globals.graphicsQuality.Equals("VERY LOW"))
            {
                numOfFansActive = 10;
            }

            /*print("DEBUGSTADIUM numOfFansActive " + numOfFansActive + " " +
                " Globals.graphicsQuality "  + Globals.graphicsQuality
                + " isBonusActive " + isBonusActive
                + " isTrainingActive " + isTrainingActive);*/

            foreach (var allStadiumPeople in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (allStadiumPeople.name.Contains("fan_"))
                {
                    if (currentFansActive >= numOfFansActive ||
                        isBonusActive ||
                        isTrainingActive)
                    {
                        allStadiumPeople.transform.parent.gameObject.SetActive(false);
                        //print("DEBUGSTADIUM disable fan 1 " + currentFansActive
                        //    + " numOfFansActive " + numOfFansActive);
                        continue;
                    }

                    if (Globals.graphicsQuality.Equals("LOW"))
                    {
                        int randFans = UnityEngine.Random.Range(0, 3);
                        if (randFans == 0)
                        {
                            allStadiumPeople.transform.parent.gameObject.SetActive(false);
                            //print("DEBUGSTADIUM disable fan 2 " + currentFansActive
                           //+ " numOfFansActive " + numOfFansActive);
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
                            //print("DEBUGSTADIUM disable fan 3 " + currentFansActive
                //+ " numOfFansActive " + numOfFansActive);
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

                    //print("texture people " + texturePeople + "fansColor " + fansColor);

                    allStadiumPeople.GetComponent<Renderer>().material = fansMaterial;
                    allStadiumPeople.GetComponent<Renderer>().material.SetTexture("_MainTex", texturePeople);                    
                }

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
                    if (allStadiumPeople.name.Contains("flagStands"))
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
                    string leagueName = Globals.leagueName;
                    if (!PhotonNetwork.IsMasterClient)
                        leagueName = Globals.teamBleague;

                    if (Globals.isPlayerCardLeague(leagueName))
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

        JoystickButtonAnimNames.Add("3D_GK_step_left");
        JoystickButtonAnimNames.Add("3D_GK_step_right");
        JoystickButtonAnimNames.Add("3D_back_run");
        JoystickButtonAnimNames.Add("3D_walk");
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

    public bool getIsMaster()
    {
        return isMaster;
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



    private Vector3 bezierCurvePlaneInterPoint(
                                               GameObject gObject,
                                               Vector3[,] arr,
                                               int s,
                                               int e,
                                               bool overwritteZ)
    {
        int mid = (s + e) / 2;
        Vector3 midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[mid, 0]);

        if ((e <= s))
        /// ((midPoint.z < 0.5f) && (midPoint.z >= 0f)))
        {
            //if (overwritteZ)
            //    midPoint.z = arr[mid, 1].x;

            /*Use z vector cord as time */
            //print("#DBGBINARYSEARCH MANUALCALCULATION globalPoint " + arr[mid, 0] + " localPoint " + midPoint);
            if (Mathf.Abs(midPoint.z) < 0.1f)
            {
                if (overwritteZ)
                    midPoint.z = arr[mid, 1].x;
                return midPoint;
            }
            else
            {
                if (!overwritteZ)
                {
                    return bezierCurvePlaneInterPoint(
                                    0f,
                                    1f,
                                    gObject,
                                    arr[mid, 0],
                                    arr[mid + 1, 0],
                                    false);
                }
                else
                {
                    midPoint = bezierCurvePlaneInterPoint(
                            0f,
                            1f,
                            gObject,
                            arr[mid, 0],
                            arr[mid + 1, 0],
                            true);

                    midPoint.z = arr[mid, 1].x;
                    return midPoint;
                }
            }
        }

        if ((e - s) <= 1)
        {
            if (e > s)
            {
                if (Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]).z < 0)
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);

                        //print("#DBGBINARYSEARCH MANUALCALCULATION 1 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);
                        midPoint.z = arr[s, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            true);
                            midPoint.z = arr[s, 1].x;
                            return midPoint;
                        }
                    }
                }
                else
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        //print("#DBGBINARYSEARCH MANUALCALCULATION 2 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        midPoint.z = arr[e, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            true);
                            midPoint.z = arr[e, 1].x;
                            return midPoint;
                        }

                        //midPoint.z = arr[e, 1].x;               
                    }
                }
            }
        }

        if (midPoint.z >= 0f)
            return bezierCurvePlaneInterPoint(gObject, arr, mid, e, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(gObject, arr, s, mid, overwritteZ);
    }

    private Vector3 bezierCurvePlaneInterPoint(
                                               Rigidbody gObject,
                                               Vector3[,] arr,
                                               int s,
                                               int e,
                                               bool overwritteZ)
    {
        int mid = (s + e) / 2;
        Vector3 midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[mid, 0]);

        if ((e <= s))
        /// ((midPoint.z < 0.5f) && (midPoint.z >= 0f)))
        {
            //if (overwritteZ)
            //    midPoint.z = arr[mid, 1].x;

            /*Use z vector cord as time */
            //print("#DBGBINARYSEARCH MANUALCALCULATION globalPoint " + arr[mid, 0] + " localPoint " + midPoint);
            if (Mathf.Abs(midPoint.z) < 0.1f)
            {
                if (overwritteZ)
                    midPoint.z = arr[mid, 1].x;
                return midPoint;
            }
            else
            {
                if (!overwritteZ)
                {
                    return bezierCurvePlaneInterPoint(
                                    0f,
                                    1f,
                                    gObject,
                                    arr[mid, 0],
                                    arr[mid + 1, 0],
                                    false);
                }
                else
                {
                    midPoint = bezierCurvePlaneInterPoint(
                            0f,
                            1f,
                            gObject,
                            arr[mid, 0],
                            arr[mid + 1, 0],
                            true);

                    midPoint.z = arr[mid, 1].x;
                    return midPoint;
                }
            }
        }

        if ((e - s) <= 1)
        {
            if (e > s)
            {
                if (Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]).z < 0)
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);

                        //print("#DBGBINARYSEARCH MANUALCALCULATION 1 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);
                        midPoint.z = arr[s, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            true);
                            midPoint.z = arr[s, 1].x;
                            return midPoint;
                        }
                    }
                }
                else
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        //print("#DBGBINARYSEARCH MANUALCALCULATION 2 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        midPoint.z = arr[e, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            true);
                            midPoint.z = arr[e, 1].x;
                            return midPoint;
                        }

                        //midPoint.z = arr[e, 1].x;               
                    }
                }
            }
        }

        if (midPoint.z >= 0f)
            return bezierCurvePlaneInterPoint(gObject, arr, mid, e, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(gObject, arr, s, mid, overwritteZ);
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
    }

    private Vector3 bezierCurvePlaneInterPoint(float s,
                                               float e,
                                               Rigidbody rb,
                                               Vector3 startPos,
                                               Vector3 endPos,
                                               bool overwritteZ)
    {
        float time = (s + e) / 2.0f;
        Vector3 midPoint = getCurveShotPosLocal(rb, startPos, endPos, time);

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
            return bezierCurvePlaneInterPoint(time, e, rb, startPos, endPos, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(s, time, rb, startPos, endPos, overwritteZ);
    }

    private Vector3 bezierCurvePlaneInterPoint(float s,
                                               float e,
                                               GameObject rb,
                                               Vector3 startPos,
                                               Vector3 endPos,
                                               bool overwritteZ)
    {
        float time = (s + e) / 2.0f;
        Vector3 midPoint = getCurveShotPosLocal(rb, startPos, endPos, time);

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
            return bezierCurvePlaneInterPoint(time, e, rb, startPos, endPos, overwritteZ);
        else
            return bezierCurvePlaneInterPoint(s, time, rb, startPos, endPos, overwritteZ);
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
                         ref float gkSideAnimPlayOffset,
                         ref bool initCpuAdjustAnimSpeed,
                         ref bool initGkDeleyLevel,
                         ref bool initGKPreparation,
                         ref string initAnimName,
                         ref float levelDelay,
                         ref float cpuAnimAdjustSpeed,
                         ref string gkAction,
                         ref float gkTimeLastCatch,
                         bool isLobActive,
                         ref Vector3 stepSideAnimOffset,
                         ref bool gkLobPointReached,
                         ref bool gkRunPosReached,
                         ref float initDistX,
                         SHOTVARIANT2 shotVariant,
                         Vector3 outShotStart,
                         Vector3 outShotMid,
                         Vector3 outShotEnd,
                         Vector3 endPosOrg,
                         float timeofBallFly,
                         float passedShotFlyTime,
                         ref bool gkLock,
                         ref GameObject rotatedRbToBall,
                         Vector3 cornerPoints,
                         bool isExtraGoals,
                         Vector3[,] prepareShotPos,
                         int prepareShotMaxIdx)
    {
        bool negativeRun = false;
        float timeToHitX = float.MaxValue;
        float timeToHitZ = float.MaxValue;
        float timeToHitY = float.MaxValue;


        Vector3 realHitPlaceLocal2333 = bezierCurvePlaneInterPoint(
                                                  rotatedRbToBall,
                                                  prepareShotPos,
                                                  0,
                                                  prepareShotMaxIdx - 1,
                                                  true);


        float timeToHitZAA = 
            ((realHitPlaceLocal2333.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;

        //print("#DEBUGGK1045 gkMoves #### " + timeToHitZAA + " ballRb.transform " +
        //    ballRb[activeBall].transform.position + " gk position " + rb.transform.position
        //    + " posLocal " + getPosLocal(rb, ballRb[activeBall].transform.position));

        if (gkTouchDone == false)
        {
            //print("DBG342344COL gkMoves gkTouchDone " + gkTouchDone);
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
            //print("DBG342344COL gkMoves gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS);

            //print("DEBUGLASTTOUCHLAKI LASTOUCH INCORRECT NO HIT");
            return;
        }

        Vector3 realHitPlaceLocal = Vector3.zero;

        if (!isCpuPlayer)
        {
            if (!isCpuPlayer)
            {
                //print("#DBGK1024 rotatedRbToBall.transform.eulerAngles " +
                //rotatedRbToBall.transform.eulerAngles);
                ///print("DBGK1024_SIMULATE_DBG2435_CREATE initGKPreparation " + initGKPreparation);
                if (!initGKPreparation)
                {

                    //  print("#DBGK1024 cache filled localSpace " + localSpace + " timeToHitZ " + timeToHitZ
                    //                    + " curvePercentHit " + curvePercentHit);

                    //              print("#DBGBPRIV rotated before " + rotatedRbToBall.transform.position);
                    if (photonView.IsMine)
                    {
                        //print("DBGK1024_SIMULATE_DBG2435_CREATE gkPredictCalc executed***");
                        gkPredictCalc(rb,
                                      rotatedRbToBall,
                                      shotvariant,
                                      outShotStart,
                                      outShotMid,
                                      outShotEnd,
                                      endPosOrg,
                                      timeofBallFly,
                                      passedShotFlyTime,
                                      timeToHitZ,
                                      ref stepSideAnimOffset,
                                      ref lastGkDistX,
                                      ref gkStartPos,
                                      ref lastAnimName,
                                      ref animName,
                                      ref gkTimeToCorrectPos,
                                      ref gkSideAnimPlayOffset,
                                      ref cpuAnimAdjustSpeed,
                                      ref gkOperations,
                                      prepareShotPos,
                                      prepareShotMaxIdx);
                    }
                    //print("#DBGBPRIV rotated after " + rotatedRbToBall.transform.position);

                    initGKPreparation = true;

                    gkSimulatePrepare(gkOperations,
                                      ref stepSideAnimOffset,
                                      ref gkTimeToCorrectPos,
                                      ref gkStartPos,
                                      ref lastGkDistX,
                                      prepareShotPos,
                                      prepareShotMaxIdx);

                    //gkAnimNameCache = lastAnimName;
                    //gkAnimSpeedCache = cpuAnimAdjustSpeed;
                    //if (timeToHitZ != 0f)
                }
                else
                {
                    /*   print("DBGK1024 cache hit");
                       localSpace = localSpaceCache;
                       timeToHitZ = timeToHitZCache;
                       curvePercentHit = curvePercentHitCache;
                       animName = gkAnimNameCache;
                       cpuAnimAdjustSpeed = gkAnimSpeedCache;*/
                }



                //if (photonView.IsMine)
                //{
                //    print("TIMESTAMP " + Time.time.ToString("F3") + " count " + fixedUpdateCount + " delta " +
                //        Time.deltaTime);
                //}

                gkSimulate(animator,
                           rotatedRbToBall,
                           rb,
                           ref stepSideAnimOffset,
                           prepareShotPos,
                           prepareShotMaxIdx);

                //print("#DBGK1024 localSpace " + localSpace + " timeToHitZ " + timeToHitZ
                //     + " curvePercentHit " + curvePercentHit);

                return;
                /*used by lob */
                //if (userHitCorrectPoint)
                // hitPointWorld = TransformPointUnscaled(rotatedRbToBall.transform, localSpace);
                /*print("DEBUGLASTTOUCHLAKI DISTANCE GK CURVED " + 
                    Vector3.Distance(clickedRbRotatedLS, realHitPlaceLocal) + " LOCALSPACECLICKED "
                   + clickedRbRotatedLS + " REALHITPLACE " + realHitPlaceLocal
                   + " LOCALSPACE " + localSpace + " gkTouchPosRbWS " + gkTouchPosRbWS
                    + " gkTouchPosRotated " + gkTouchPosRotatedRbWS);  */
            }
            return;
        }
    }

    private void updatePlayersTextures()
    {
        if (updateTextureDone ||
           (Globals.teamBid == -1))
            return;


        /*if (photonView.IsMine)
        {
            print("TeamId DESC" + Globals.teamAid
                + " Globals.teamAname " + Globals.teamAname
                + " Globals.teamAleague " + Globals.teamAleague
                + " Globals.playerADesc " + Globals.playerADesc
                + " Globals.teamBid " + Globals.teamBid
                + " Globals.teamBname " + Globals.teamBname
                + " Globals.teamBleague " + Globals.teamBleague);
        }*/


        setTextureScript.setPlayersTextures(new GameObject[] { playerTextureGO, peerPlayer.playerTextureGO },
                                            new GameObject[] { hairPlayerDownGO, peerPlayer.hairPlayerDownGO });

        updateTextureDone = true;
    }

    public bool getLobActive()
    {
        return isLobActive;
    }

    private void gkSimulatePrepare(string gkOperations,
                                   ref Vector3 stepSideAnimOffset,
                                   ref float gkTimeToCorrectPos,
                                   ref Vector3 localGkStartPos,
                                   ref float lastGkDistX,
                                   Vector3[,] prepareShotPos,
                                   int prepareShotMaxIdx)
    {

        string[] operations = gkOperations.Split(':');

        //print("#DBGK1024 simulate prepare " + gkOperations + " operations[0].Split('|') " + operations[0].Split('|'));

        string[] rotationOperations =
            operations[0].Split('|');

        gkRotationLoops = int.Parse(rotationOperations[1]);

        //print("#DBGK1024 float1 " + rotationOperations[5] + " float2 " + rotationOperations[6] + " float3 " +
        //    rotationOperations[7]);
        gkRbRotationPos = new Vector3(float.Parse(rotationOperations[2], CultureInfo.InvariantCulture),
                                      float.Parse(rotationOperations[3], CultureInfo.InvariantCulture),
                                      float.Parse(rotationOperations[4], CultureInfo.InvariantCulture));

        gkRbRotationRot = new Vector3(float.Parse(rotationOperations[5], CultureInfo.InvariantCulture),
                                      float.Parse(rotationOperations[6], CultureInfo.InvariantCulture),
                                      float.Parse(rotationOperations[7], CultureInfo.InvariantCulture));

        string[] stepSideOperations =
            operations[1].Split('|');

        if (!stepSideOperations[1].Equals("NUL"))
        {
            gkStepSideAnimName = stepSideOperations[1];
            gkStepAnimPercent = float.Parse(stepSideOperations[2], CultureInfo.InvariantCulture);
            gkSideOffsetListMax = int.Parse(stepSideOperations[3]);
            //print("DBGLISTMAX " + gkSideOffsetListMax);
            gkSideOffsetListCurrIdx = 0;
            stepSideAnimOffsetList.Clear();
            //print("#DBG23 gkOperations " + gkOperations);
            for (int i = 0; i < gkSideOffsetListMax; i++)
            {
                stepSideAnimOffsetList.Add(
                    new Vector3(float.Parse(stepSideOperations[4 + (i * 3)], CultureInfo.InvariantCulture),
                                float.Parse(stepSideOperations[5 + (i * 3)], CultureInfo.InvariantCulture),
                                float.Parse(stepSideOperations[6 + (i * 3)], CultureInfo.InvariantCulture)));
                //  print("stepSideAnimOffsetListvalues " + stepSideAnimOffsetList[i]);
            }
        }
        else
        {
            gkStepSideAnimName = "NUL";
            stepSideAnimOffsetList.Clear();
        }

        string[] sideOperations =
          operations[2].Split('|');

        gkSideAnimName = sideOperations[1];
        gkSideAnimSpeed = float.Parse(sideOperations[2], CultureInfo.InvariantCulture);
        gkSideAnimDelayBefore = float.Parse(sideOperations[3], CultureInfo.InvariantCulture);
        gkTimeToCorrectPos = float.Parse(sideOperations[4], CultureInfo.InvariantCulture);
        localGkStartPos = new Vector3(float.Parse(sideOperations[5], CultureInfo.InvariantCulture),
                                      float.Parse(sideOperations[6], CultureInfo.InvariantCulture),
                                      float.Parse(sideOperations[7], CultureInfo.InvariantCulture));

        gkSideAnimStartPos = new Vector3(float.Parse(sideOperations[9], CultureInfo.InvariantCulture),
                                          float.Parse(sideOperations[10], CultureInfo.InvariantCulture),
                                          float.Parse(sideOperations[11], CultureInfo.InvariantCulture));

        gkSideAnimStartRot = new Vector3(float.Parse(sideOperations[12], CultureInfo.InvariantCulture),
                                         float.Parse(sideOperations[13], CultureInfo.InvariantCulture),
                                         float.Parse(sideOperations[14], CultureInfo.InvariantCulture));

        lastGkDistX = float.Parse(sideOperations[8], CultureInfo.InvariantCulture);

        /*print("#DEBUGGK1045  gkPrepare simulation Prepare gkRotationLoops " + gkRotationLoops
              + " gkRbRotationPos " + gkRbRotationPos
              + " gkRbRotationRot " + gkRbRotationRot
              + " gkStepSideAnimName " + gkStepSideAnimName
              + " gkStepAnimPercent " + gkStepAnimPercent
              + " stepSideAnimOffset " + stepSideAnimOffset
              + " gkSideAnimName " + gkSideAnimName
              + " gkSideAnimSpeed " + gkSideAnimSpeed
              + " gkSideAnimDelayBefore " + gkSideAnimDelayBefore
              + " gkTimeToCorrectPos " + gkTimeToCorrectPos
              + " localGkStartPos " + localGkStartPos
              + " lastGkDistX " + lastGkDistX
              + " Time.time " + Time.time);*/
    }


    string[] gkExcludedAnim = new string[] { "3D_back_run_cpu",
                                             "3D_GK_step_left_no_offset_nocpu",
                                             "3D_GK_step_right_no_offset_nocpu" };


    private void gkSimulate(Animator animator,
                            GameObject rotatedRbToBall,
                            Rigidbody rb,
                            ref Vector3 stepSideAnimOffset,
                            Vector3[,] prepareShotPos,
                            int prepareShotMaxIdx)
    {

        /*Vector3 realHitPlaceLocal2 = bezierCurvePlaneInterPoint(0.0f,
                                                                1.0f,
                                                                rotatedRbToBall,
                                                                peerPlayer.getOutShotStart(),
                                                                peerPlayer.getOutShotMid(),
                                                                peerPlayer.getOutShotEnd(),
                                                                true);
                                                                */
       Vector3 realHitPlaceLocal2 = bezierCurvePlaneInterPoint(
                                               rotatedRbToBall,
                                               prepareShotPos,
                                               0,
                                               prepareShotMaxIdx - 1,
                                               true);

        float timeToHitPlayer = ((realHitPlaceLocal2.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
        //print("DBGSTEPSIDE_123433 gkSimulate timetoHit/////// " + timeToHitZ33 + " Time " + Time.time
        //    + " peerPlayer.getPassedTime() " + peerPlayer.getPassedTime()
        //    + " getShotPercent " + peerPlayer.getShotPercent());

        //print("GKDEBUG800 isAnimationPlaying" + (checkIfAnyAnimationPlaying(animator, 1.0f, excluded)));
        if (checkIfAnyAnimationPlaying(animator, 1.0f, gkExcludedAnim))
        {
            //if (checkIfAnyAnimationPlaying(animator, 1.0f)) {   
            //print("#DBGSTEPSIDE_123 blocked!!");
            return;
        }

        /*print("##DEBUGGK1045 gkSimulate playerRb.transform.position gkSimulate MAIN BEFORE ### " + gkSideAnimName 
            + " gkSideAnimSpeed " + gkSideAnimSpeed
            + " GKENDPOS## " + gkStartPos
            + " gkTimeToCorrectPos " + gkTimeToCorrectPos
            + " animator.GetCurrentAnimatorStateInfo(0).normalizedTime " +
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime
            + " rb.transform.position " + rb.transform.position
            + " Time " + Time.time
            + " getShotPercent " + peerPlayer.getShotPercent()
            + " timeToHitZ33 " + timeToHitPlayer);*/


        //if (!photonView.IsMine)
        //    print("#DBGK1024_SIMULATE_DBG2435_CREATE peerPlayer.getPassedTime() " + peerPlayer.getPassedTime());

        //        print("#DBGK1024_SIMULATE_DBG2435_CREATE peerPlayer.getPassedTime() " + peerPlayer.getPassedTime());


        //print("#dbgrbposition before " + rb.transform.position);

        if (!photonView.IsMine)
            rb.transform.position = gkRbRotationPos;

        //print("#dbgrbposition after " + rb.transform.position);

        if (RblookAtDirectionGK(rb,
                                rotatedRbToBall,
                                2.0f,
                                25.0f) &&
            gkRotationLoops > 0)
        {
            gkRotationLoops--;

            /*Vector3 realHitPlaceLocal233 = bezierCurvePlaneInterPoint(0.0f,
                                 1.0f,
                                 rotatedRbToBall,
                                 peerPlayer.getOutShotStart(),
                                 peerPlayer.getOutShotMid(),
                                 peerPlayer.getOutShotEnd(),
                                 true);*/

            Vector3 realHitPlaceLocal233 = bezierCurvePlaneInterPoint(
                                        rotatedRbToBall,
                                        prepareShotPos,
                                        0,
                                        prepareShotMaxIdx - 1,
                                        true);


            float timeToHitZ = ((realHitPlaceLocal233.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
            ///print("#DEBUGGK1045 gkSimulate STEPDIDE STEPSIDE ROTATION LOOPS " + gkRotationLoops + " TimeToHitZ " + timeToHitZ);

            //print("DBGK1024_SIMULATE_DBG2435 STEPDIDE ROTATION realHitPlaceLocal " + realHitPlaceLocal2 + " rb.transform " +
            //     rb.transform.position + " timeToHitZ " + timeToHitZ + " ballRb.transofmr " + ballRb.transform.position
            //     + " passedTime " + peerPlayer.getPassedTime());

            return;
        }

        if (!photonView.IsMine)
            rb.transform.eulerAngles = gkRbRotationRot;

        //print("#DEBUGGK1045 gkSimulate gkSimulate ater loops position " + rb.transform.position + " rotation " +
        //        rb.transform.eulerAngles);

        //what if two animations are executed
        //print("#DBGK1024_SIMULATE_DBG2435 gkStepSideAnimName " + gkStepSideAnimName);
        //print("DEBUGGK1045 timeToHitPlayer " + timeToHitPlayer);
        if (!gkStepSideAnimName.Equals("NUL") &&
           (timeToHitPlayer > MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE))
        {
            //  print("##DBGK1024_SIMULATE_DBG2435_CREATE gkStepSideAnimName " + gkStepSideAnimName);
            if (gkSideOffsetListCurrIdx != gkSideOffsetListMax)
            {
                stepSidePredicted(animator,
                                  rb,
                                  gkStepSideAnimName,
                                  ref stepSideAnimOffset);
                    /*print("#DEBUGGK1045 STEPDIDE normalizeTime " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "" +
                        " gkStepAnimPercent " + gkStepAnimPercent + " stepSideAnimOffset " + stepSideAnimOffset
                      + " gkStepSideAnimName " + gkStepSideAnimName
                      + " rb.transform.position " + rb.transform.position);*/

        /*Vector3 realHitPlaceLocal3 = bezierCurvePlaneInterPoint(0.0f,
                                                 1.0f,
                                                 rotatedRbToBall,
                                                 peerPlayer.getOutShotStart(),
                                                 peerPlayer.getOutShotMid(),
                                                 peerPlayer.getOutShotEnd(),
                                                 true);*/


                Vector3 realHitPlaceLocal3 = bezierCurvePlaneInterPoint(
                                                       rotatedRbToBall,
                                                       prepareShotPos,
                                                       0,
                                                       prepareShotMaxIdx - 1,
                                                       true);

                float timeToHitZ2 = ((realHitPlaceLocal3.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
                /*print("#DEBUGGK1045 gkSimulate STEPDIDE STEPSIDE ANIM PROGRESS #### STEPANIM realHitPlaceLocal " + realHitPlaceLocal3 + " rb.transform " +
                    rb.transform.position + " timeToHitZ " + timeToHitZ2 + " ballRb.transofmr " + ballRb[activeBall].transform.position
                    + " Time.time " + Time.time
                    + " getShotPercent " + peerPlayer.getShotPercent()
                    + " isFixedUpdate  " + isFixedUpdate
                    + " stepSideAnimOffset " + stepSideAnimOffset);*/

                return;
            }
            else
            {
                //bool isStepSideAnimPlaying = checkIfAnyAnimationPlayingContain(animator, 1.0f, "no_offset");
                //if (isStepSideAnimPlaying &&
                /*print("#DBGK1024_SIMULATE_DBG2435_CREATE STEPDIDE normalizeTimeXXX " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "" +
                  " gkStepAnimPercent " + gkStepAnimPercent + " stepSideAnimOffset " + stepSideAnimOffset
                + " gkStepSideAnimName " + gkStepSideAnimName
                + " rb.transform.position " + rb.transform.position
                + " ballRb.transform " + ballRb.transform.position);*/


                /*Vector3 realHitPlaceLocal4 = bezierCurvePlaneInterPoint(0.0f,
                                   1.0f,
                                   rotatedRbToBall,
                                   peerPlayer.getOutShotStart(),
                                   peerPlayer.getOutShotMid(),
                                   peerPlayer.getOutShotEnd(),
                                   true);*/

                Vector3 realHitPlaceLocal4 = bezierCurvePlaneInterPoint(
                                       rotatedRbToBall,
                                       prepareShotPos,
                                       0,
                                       prepareShotMaxIdx - 1,
                                       true);



                float timeToHitZ3 = ((realHitPlaceLocal4.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
                /*print("##DEBUGGK1045 gkSimulate STEPDIDE STEPSIDE realHitPlaceLocal " + realHitPlaceLocal4 + " rb.transform " +
                    rb.transform.position + " timeToHitZ " + timeToHitZ3 + " ballRb.transofmr " + ballRb[activeBall].transform.position
                    + " gkStepAnimPercent " + gkStepAnimPercent
                    + " animator.GetCurrentAnimatorStateInfo(0).normalizedTime " +
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime +
                    " gkSideOffsetListCurrIdx " + gkSideOffsetListCurrIdx
                    + " gkSideOffsetListMax " + gkSideOffsetListMax
                    + " Time.time " + Time.time
                    + " stepPefect " + (gkStepAnimPercent % 1)
                    + " isFixedUpdate + " + isFixedUpdate
                    + " stepSideAnimOffset " + stepSideAnimOffset);*/


                if (
                   (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= (gkStepAnimPercent % 1)))
                {
                    gkStepSideAnimName = "NUL";
                    //print("#DEBUGGK1045 gkSimulate STEPSIDE SIMULATIoutN END--------------"
                    //    + " timeToHitZ3 " + timeToHitZ3 + " Time " + Time.time);

                }
                else
                    return;
            }

            //TODO
            //if (isStepSideAnimPlaying &&
            //   (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f))
            //{
            //    gkStepAnimPercent -= 1.0f;
            //   print("#DBGK1024_SIMULATE STEPDIDE gkStepAnimPercent DECREASED " + gkStepAnimPercent);
            //}

            //if ((isStepSideAnimPlaying &&
            //   (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= gkStepAnimPercent)))
            // {
            //   return;
            //}
        }


        if (gkSideAnimDelayBeforeStart == -1)
        {
            //print("#DBGK1024_SIMULATE_DBG2435_CREATE init start before ");
            gkSideAnimDelayBeforeStart = Time.time;
        }

        /*print("#DBGK1024_SIMULATE_DBG2435_CREATE gkSideAnimDelayBefore " + gkSideAnimDelayBefore + " " +
            "gkSideAnimDelayBeforeStart " + gkSideAnimDelayBeforeStart
            + " Time.time - gkSideAnimDelayBeforeStart diff " + (Time.time - gkSideAnimDelayBeforeStart));*/

        if ((Time.time - gkSideAnimDelayBeforeStart) < gkSideAnimDelayBefore)
        {
            //print("#DEBUGGK1045 gkSimulate delay " + (Time.time - gkSideAnimDelayBeforeStart) + " gkSideAnimDelayBefore " +
            //    gkSideAnimDelayBefore);
            return;
        }

        //print("timeToHitZ33 gkSimulate ater steps position " + rb.transform.position + " rotation " +
        //    rb.transform.eulerAngles);

       /* Vector3 realHitPlaceLocal5 = bezierCurvePlaneInterPoint(0.0f,
                                                    1.0f,
                                                    rotatedRbToBall,
                                                    peerPlayer.getOutShotStart(),
                                                    peerPlayer.getOutShotMid(),
                                                    peerPlayer.getOutShotEnd(),
                                                    true);*/

        Vector3 realHitPlaceLocal5 = bezierCurvePlaneInterPoint(
                                       rotatedRbToBall,
                                       prepareShotPos,
                                       0,
                                       prepareShotMaxIdx - 1,
                                       true);


        float timeToHitZ_5 = ((realHitPlaceLocal5.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
        /*print("#DEBUGGK1045 STEPDIDE STEPSIDE PLAY realHitPlaceLocal " + realHitPlaceLocal5 + " rb.transform " +
            rb.transform.position + " timeToHitZ " + timeToHitZ_5 + " ballRb.transofmr " 
            + ballRb[activeBall].transform.position
            + " stepSideAnimOffsetList.Count " + stepSideAnimOffsetList.Count
            //+ " LASTeLEMENT " + stepSideAnimOffsetList[stepSideAnimOffsetList.Count - 1]
            + " stepSideAnimOffset " + stepSideAnimOffset
            + " gkSideAnimStartPos " + gkSideAnimStartPos);*/

        if (stepSideAnimOffsetList.Count > 0)
        {
            rb.transform.position = stepSideAnimOffsetList[stepSideAnimOffsetList.Count - 1];
            //print("DEBUGGK1045 last " + stepSideAnimOffsetList[stepSideAnimOffsetList.Count - 1]);
        }
        //stepSideAnimOffset;
        else
        {
            rb.transform.position = gkSideAnimStartPos;
            //rb.transform.eulerAngles = gkSideAnimStartRot;
        }
        ///else
        //    rb.transform.position = 

//        print("DEBUGGK1045 gkSimulate ##PLAYMAIN " + gkSideAnimName + " gkSideAnimSpeed " +
//            gkSideAnimSpeed + " timeToHitZ_5 " + timeToHitZ_5
//             + " rb.transform.position " + rb.transform.position
//             + " rb.rotation " + rb.transform.eulerAngles);

        interruptSideAnimation(animator, rb);
        animator.speed = gkSideAnimSpeed;
        animator.Play(gkSideAnimName, 0, 0.0f);
        animator.Update(0f);

        /*Vector3 realHitPlaceLocal44 = bezierCurvePlaneInterPoint(0.0f,
                                  1.0f,
                                  rotatedRbToBall,
                                  peerPlayer.getOutShotStart(),
                                  peerPlayer.getOutShotMid(),
                                  peerPlayer.getOutShotEnd(),
                                  true);*/

        Vector3 realHitPlaceLocal44 = bezierCurvePlaneInterPoint(
                               rotatedRbToBall,
                               prepareShotPos,
                               0,
                               prepareShotMaxIdx - 1,
                               true);


        float timeToHitZ333 = ((realHitPlaceLocal44.z * peerPlayer.getTimeOfBallFly()) - peerPlayer.getPassedTime()) / 1000f;
        //print("#DEBUGGK1045 gkSimulate PLAYANIM MAIN #### position " + rb.transform.position + " rotation " +
        //    rb.transform.eulerAngles + " timeToHitZ " + timeToHitZ333
        //    + " gkStartPos " + gkStartPos);

        // if (!photonView.IsMine)
        //{
        /*print("#DBG342344COL PLAYANIM MAIN |## " + gkSideAnimName + " gkSideAnimSpeed " + gkSideAnimSpeed
               + " GKENDPOS## " + gkStartPos
               + " gkTimeToCorrectPos " + gkTimeToCorrectPos
               + " animator.GetCurrentAnimatorStateInfo(0).normalizedTime " +
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime
               + " rb.transform.position " + rb.transform.position
               + " timeToHitz " + timeToHitZ_5
               + " Time " + Time.time);*/
        //}

        //RPC_gkPacketProcessed = true;
        RPC_gkPacketProcessed = false;
        //RPC_gkPredictionActive = false;
        touchLocked = true;
        touchCount = 0;
        animationPlaying = true;
        gkTouchDone = false;
        initGkMoves = false;
        rpc_gkMovementSend = false;
        gkStartSequenceTime = -1;
        //RPC_gkPacketProcessed = true;

        gkTimeLastCatch = Time.time;
        gkLobPointReached = false;
        gkRunPosReached = false;
        initDistX = -1;
        stepSideAnimOffsetList.Clear();
    }

    public void copyRbTransform(ref Rigidbody rb, Transform transform)
    {
        rb.transform.position = transform.position;
        rb.transform.rotation = transform.rotation;
        rb.transform.localScale = transform.localScale;
    }

    public void copyRbTransform(ref GameObject rb, Transform transform)
    {
        rb.transform.position = transform.position;
        rb.transform.rotation = transform.rotation;
        rb.transform.localScale = transform.localScale;
    }

    public bool arePeersPlayerSet() 
    {
        if (peerPlayer == null)
            return false;

        if (peerPlayer.peerPlayer == false)
            return false;

        return true;
    }


    private string gkPredictCalc(Rigidbody rb,
                                 GameObject rotatedRbToBall,
                                 SHOTVARIANT2 shotvariant,
                                 Vector3 outShotStart,
                                 Vector3 outShotMid,
                                 Vector3 outShotEnd,
                                 Vector3 endPosOrg,
                                 float timeofBallFly,
                                 float passedShotFlyTime,
                                 float timeToHitZ,
                                 ref Vector3 stepSideAnimOffset,
                                 ref float lastGkDistX,
                                 ref Vector3 localGkStartPos,
                                 ref string lastGkAnimName,
                                 ref string lastAnimName,
                                 ref float gkTimeToCorrectPos,
                                 ref float gkSideAnimPlayOffset,
                                 ref float predictedAnimSpeed,
                                 ref string gkOperations,
                                 Vector3[,] prepareShotPos,
                                 int prepareShotMaxIdx)

    {
        int rotationLoops = 0;
        gkOperations = "";
        float shotPassedTime = peerPlayer.getPassedTime();
        gkStartSeq = shotPassedTime / peerPlayer.getTimeOfBallFly();

        lockCollision = true;

        //print("#DBGK1024_SIMULATE_DBG2435_CREATE shotPassedTime init " + shotPassedTime);
        //0f;
        float stepSideNumFrames = 33f;
        //in ms
        float sidesteppingExecutionTime = stepSideNumFrames / 54f;
        float percentOfSideSteppingExecuted = 0f;
        curvePercentHit = 0f;
        Vector3 localSpace = Vector3.zero;

        //GameObject rbTmpGameObj = new GameObject();
        //Rigidbody rbTmp = rbTmpGameObj.AddComponent<Rigidbody>();
        //GameObject rotatedRbToBallTmp = new GameObject();

        //rbTmp.transform.position = rb.transform.position;
        //rbTmp.tranform.rotation = rb.transform.rotation;
        //rbTmp.transform.localScale

        copyRbTransform(ref rbTmp, rb.transform);
        copyRbTransform(ref rotatedRbToBallTmp, rotatedRbToBall.transform);
        rbTmp.velocity = Vector3.zero;

        /*print("DBG342344COL gkPredictCalc eulerAngles " +
            rotatedRbToBall.transform.eulerAngles
            + " rotatedRbToBall.transform.position "
            + rotatedRbToBall.transform.position
            + " peerPlayer.getBallInit() " + peerPlayer.getBallInit()
            + " rbTmp " + rbTmp.transform.position);*/
        //print("DBGK1024_SIMULATE PREDICT CALC");
        //print("#DBGK1024 PREDICT rotatedRB pos " + rotatedRbToBall.transform.position +
        //    " rotatedEulerAngles " + rotatedRbToBall.transform.eulerAngles
        //    + " rbTmp  " + rbTmp.transform.position
        //   + " rbTmp.transform.eulerAngles " + rbTmp.transform.eulerAngles);

        while (true)
        {
            if (RblookAtDirectionGK(rbTmp,
                                    rotatedRbToBallTmp,
                                    2.0f,
                                    25.0f))
            {
                rotationLoops++;
                shotPassedTime = shotPassedTime + (Globals.FIXEDUPDATE_TIME * 1000.0f);
                /*print("#DBGCOLLISIONCALC1024DB  ROTATION shotPassedTime  " + shotPassedTime);
                print("DBG342344COL ROTATION loop " + rbTmp.transform.position + " rotation " +
                        rbTmp.transform.eulerAngles
                    + " lastGkDistX " + lastGkDistX
                    + " localGkStartPos " + localGkStartPos
                    + " lastGkAnimName " + lastGkAnimName);*/
                continue;
            }

            break;
        }

        gkOperations = "R|" + rotationLoops.ToString()
                            + "|"
                            + rbTmp.transform.position.x.ToString(CultureInfo.InvariantCulture)
                            + "|"
                            + rbTmp.transform.position.y.ToString(CultureInfo.InvariantCulture)
                            + "|"
                            + rbTmp.transform.position.z.ToString(CultureInfo.InvariantCulture)
                            + "|"
                            + rbTmp.transform.eulerAngles.x.ToString(CultureInfo.InvariantCulture)
                            + "|"
                            + rbTmp.transform.eulerAngles.y.ToString(CultureInfo.InvariantCulture)
                            + "|"
                            + rbTmp.transform.eulerAngles.z.ToString(CultureInfo.InvariantCulture);


        /*print("DBG342344COL gk prediction loop " + rbTmp.transform.position + " rotation " +
                    rbTmp.transform.eulerAngles
                    + " lastGkDistX " + lastGkDistX
                    + " localGkStartPos " + localGkStartPos
                    + " lastGkAnimName " + lastGkAnimName);*/


        gkOperations += ":S|";
        float timeLeftForSideStepping = 0;

        //shotPassedTime = ((Globals.FIXEDUPDATE_TIME * 1000.0f)) * rotationLoops;

        /*print("DBGK1024_SIMULATE_DBG2435_CREATE BEFORE gkHitPointCalc rbTmp " + rbTmp.transform.position 
            + " shotPassedTime " + shotPassedTime
            + " rotatedRbToBallTmp " + rotatedRbToBallTmp.transform.position);*/

        //print("DBGCOLLISIONCALC1024DB inside simulation ballInit " + peerPlayer.getBallInit());
        ////getRotatedRbToBall(peerPlayer.getBallInit(),
        getRotatedRbToBall(prepareShotPos[0, 0],
                           rbTmp,
                           ref rotatedRbToBallTmp,
                           //ref getRotatedRbToBallRef(),
                           getGkCornerPoints());

        gkHitPointCalc(rbTmp,
                       shotvariant,
                       outShotStart,
                       outShotMid,
                       outShotEnd,
                       endPosOrg,
                       timeofBallFly,
                       shotPassedTime,
                       //rotatedRbToBall,
                       rotatedRbToBallTmp,
                       ref localSpace,
                       ref timeToHitZ,
                       ref curvePercentHit,
                       prepareShotPos,
                       prepareShotMaxIdx);

        //timeToHitZ -= (Globals.FIXEDUPDATE_TIME * rotationLoops);


        /*print("#DBG342344COL ### INIT after rotation 1 timeToHitz " + timeToHitZ + " localSpace " + localSpace
            + " curvePercentHit " + curvePercentHit + " rbTmp.transform.position " + rbTmp.transform.position
            + " shotPassedTime " + shotPassedTime
            + " outShotStart " + outShotStart
            + " outShotMid " + outShotMid
            + " outShotEnd " + outShotEnd
            + " endPosOrg " + endPosOrg);*/

        //print("#DEBUGGK1045 localSpace before " + localSpace);
        

        int ballPoMainIdx = BezierCuve.bezierCurvePlaneInterPoint(
                                       rbTmp,
                                       peerPlayer.prepareShotPos,
                                       0,
                                       prepareShotMaxIdx - 1);

        /*print("#DEBUGGK1045 localSpace before difference " +
                                       bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            rbTmp,
                                            prepareShotPos[ballPoMainIdx, 0],
                                            prepareShotPos[ballPoMainIdx + 1, 0],
                                            false)
                                      + " prepareShotPos[ballPoMainIdx, 0] " + prepareShotPos[ballPoMainIdx, 0]
                                      + " prepareShotPos[ballPoMainIdx, 0] " + prepareShotPos[ballPoMainIdx + 1, 0]);

        */
        /*ctor3 bezierCurvePlaneInterPoint(float s,
                                               float e,
                                               Rigidbody rb,
                                               Vector3 startPos,
                                               Vector3 endPos,
                                               bool overwritteZ)*/


        /*print("DBGK1024_SIMULATE_DBG2435_CREATE localSpace "
            + localSpace + " timeToHit " + timeToHitZ
            + " shotPassedTime " + shotPassedTime);
        print("#DBGK1024_SIMULATE_EARLY_STAGE gkPredictCalc localSpace " + localSpace + " timeToHitZ " + timeToHitZ + " curvePercentHit " + curvePercentHit
            + " outShotStart " + outShotStart + " outShotMid " + outShotMid + " outShotEnd " + outShotEnd
            + " endPosOrg " + endPosOrg 
            + " timeofBallFly " + timeofBallFly
            + " passedShotFlyTime " + shotPassedTime
            + " rotatedRbToBall " + rotatedRbToBallTmp.transform.position
            + " timeToHitZ " + timeToHitZ
            + " curvePercentHit " + curvePercentHit
            + " rotationLoops " + rotationLoops
            + " rbTmp " + rbTmp.transform.position
            + " shotvariat " + shotvariant
            + " ballRb.transform.position " + ballRb.transform.position
            + " shotPassedTime " + shotPassedTime);
            */

        //format
        //S_animName_percentAnimExecuted_stepSideOffset_distX_newPosX_newPosY_newPosZ
        float stepSideDelay = 0;
        float timeToHitZStart = timeToHitZ;
        float distX = Mathf.Abs(localSpace.x);

        //print("DBGK1024_SIMULATE_DBG2435_CREATE timeZ before step " + timeToHitZ);
        if (timeToHitZ > MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
        {
            Vector3 locPos = InverseTransformPointUnscaled(rbTmp.transform, rbTmp.transform.position);

            //animator.Play(animName, 0, 0.0f);
            //print("STEP SIDE " + animName);

            timeLeftForSideStepping = timeToHitZ - MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE;
            //print("#DBGCOLLISIONCALC1024DB INIT timeLeftForSideStepping " + timeLeftForSideStepping);
            if (distX >= 0.25f)
            {
                if (localSpace.x < 0.0f)
                {
                    //   distX = Mathf.Max(-2.4f, localSpace.x);
                    gkOperations += "3D_GK_step_left_no_offset_nocpu";
                }
                else
                {
                    // distX = Mathf.Min(2.4f, localSpace.x);
                    gkOperations += "3D_GK_step_right_no_offset_nocpu";
                }
            }
            else
            {
                timeLeftForSideStepping = 0f;
                gkOperations += "NUL";
            }

            //print("#DBGK1024_SIMULATE_EARLY timeLeftForSideStepping " + timeLeftForSideStepping);

            percentOfSideSteppingExecuted =
                    timeLeftForSideStepping / sidesteppingExecutionTime;

            //locPos.x += distX;
            //percentOfSideSteppingExecuted);
            //stepSideAnimOffset = TransformPointUnscaled(rbTmp.transform, locPos);

            float tmpPercentOfSideSteppingExecuted = percentOfSideSteppingExecuted;

            /*print("#DBGK1024_SIMULATE_DBG2435_CREATE gkPredictCalc PREDICT step before rb.position " + rb.transform.position + " photonView.IsMine " +
                photonView.IsMine + " shotPassedTime " + shotPassedTime + " percentOfSideSteppingExecuted " + percentOfSideSteppingExecuted
               + " timeofBallFly " + timeofBallFly);*/

            while (true)
            {
                //if (timeLeftForSideStepping >= timeLeftForSideStepping)
                //    break;
                if (tmpPercentOfSideSteppingExecuted <= 0f)
                    break;

                //print("#DBGK1024_SIMULATE_DBG2435_CREATE distX " + distX + " localSpace " + localSpace);
                //if (timeToHitZ < MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE)
                //    break;

                if (distX < 0.25f)
                {
                    timeLeftForSideStepping = timeToHitZStart - timeToHitZ;
                    percentOfSideSteppingExecuted =
                        timeLeftForSideStepping / sidesteppingExecutionTime;
                    //print("#DBGK1024_SIMULATE_DBG2435_CREATE stepSideDelay " + stepSideDelay
                    //    + " timeLeftForSideStepping " + timeLeftForSideStepping
                    //    + " percentOfSideSteppingExecuted " + percentOfSideSteppingExecuted);
                    break;
                }

                //getRotatedRbToBall(peerPlayer.getBallInit(),
                getRotatedRbToBall(prepareShotPos[0,0],
                                   rbTmp,
                                   ref rotatedRbToBallTmp,
                                   //ref getRotatedRbToBallRef(),
                                   getGkCornerPoints());

                gkHitPointCalc(rbTmp,
                               shotvariant,
                               outShotStart,
                               outShotMid,
                               outShotEnd,
                               endPosOrg,
                               timeofBallFly,
                               shotPassedTime,
                               //rotatedRbToBall,
                               rotatedRbToBallTmp,
                               ref localSpace,
                               ref timeToHitZ,
                               ref curvePercentHit,
                               prepareShotPos,
                               prepareShotMaxIdx);


                if (localSpace.x < 0.0f)
                {
                    distX = Mathf.Max(-2.4f, localSpace.x);
                }
                else
                {
                    distX = Mathf.Min(2.4f, localSpace.x);
                }

                /*print("#DBGK1024_SIMULATE_DBG2435_CREATE distX " + distX + " localSpace " + localSpace.x +
                     "  tmpPercentOfSideSteppingExecuted " + tmpPercentOfSideSteppingExecuted
                     + " rbTmp.transform.position " + rbTmp.transform.position
                     + " distX " + distX
                     + " timeToHitZ " + timeToHitZ);*/
                Vector3 rbTmplocPos = Vector3.zero;
                if (tmpPercentOfSideSteppingExecuted > 1)
                {
                    rbTmplocPos.x = distX;
                }
                else
                {
                    rbTmplocPos.x = (distX * tmpPercentOfSideSteppingExecuted);
                }

                rbTmp.transform.position = TransformPointUnscaled(rbTmp.transform, rbTmplocPos);

                //print("DEBUGGK1045  rbTmp.transform " + rbTmp.transform.position + " rotated transform " +
                //     " rotatedRbToBall " + rotatedRbToBall.transform.position
                //     + " percentOfSideSteppingExecuted " + percentOfSideSteppingExecuted
                //     + " sidesteppingExecutionTime " + sidesteppingExecutionTime);

                stepSideAnimOffsetList.Add(rbTmp.transform.position);
                //print("DBG342344COL gk step side " + rbTmp.transform.position + " rotation " +
                //     rbTmp.transform.eulerAngles);

                if (percentOfSideSteppingExecuted >= 1)
                {
                    //while step animation executed
                    shotPassedTime = shotPassedTime + (sidesteppingExecutionTime * 1000f);
                }
                else
                {
                    //part of animation executed
                    shotPassedTime = shotPassedTime + (percentOfSideSteppingExecuted * sidesteppingExecutionTime * 1000f);
                }

                tmpPercentOfSideSteppingExecuted -= 1f;

                //TODO it's not executed in one frame
                // shotPassedTime = shotPassedTime + (Globals.FIXEDUPDATE_TIME * 1000.0f);
                //print("#DBGCOLLISIONCALC1024DB INIT PREDICT LIST_ADD step rb.position " + rbTmp.transform.position + " photonView " + photonView.IsMine
                //    + " rbTmplocPos " + rbTmplocPos + " shotPassedTime " + shotPassedTime + " localSpace " + localSpace
                //    + " rotatedRbToBall " + rotatedRbToBallTmp.transform.position + " timeToHitZ " + timeToHitZ
                //    + " shotPassedTime " + shotPassedTime);
            }


            /*print("#DBGK1024_SIMULATE_DBG2435_CREATE shotPassedTime after step side ##" + shotPassedTime
                + " percentOfSideSteppingExecuted " + percentOfSideSteppingExecuted
                + " sidesteppingExecutionTime " + sidesteppingExecutionTime
                + " plus side stepp shotPassedTime " +
                (percentOfSideSteppingExecuted * sidesteppingExecutionTime * 1000f) +
                " rbTmp.transform " + rb.transform.position);*/
            //is it neccessary?
            /*Rigidbody rbTmp = new Rigidbody();
            rbTmp = rb.transform;
            rbTmp.rotation = rb.rotation;

            rbTmp.transform.position = TransformPointUnscaled(rbTmp.transform, locPos);
            calcGkCorrectRotationToBall(                   
                              //peerPlayer.getBallInit(),
                              ballRb.transform.position,
                              rbTmp,
                              ref rotatedRbToBall,
                              gkCornerPoints);

            while (true)
            {
                if (RblookAtDirectionGK(rb,
                                        rotatedRbToBall,
                                        2.0f,
                                        25.0f))
                {
                    rotationLoops++;
                    continue;
                }
                break;
            }*/

            //stepSideAnimOffset
            gkOperations += "|" + percentOfSideSteppingExecuted.ToString(CultureInfo.InvariantCulture)
                         + "|" + stepSideAnimOffsetList.Count.ToString(CultureInfo.InvariantCulture);

            for (int i = 0; i < stepSideAnimOffsetList.Count; i++)
            {
                gkOperations += "|" + stepSideAnimOffsetList[i].x.ToString(CultureInfo.InvariantCulture)
                              + "|" + stepSideAnimOffsetList[i].y.ToString(CultureInfo.InvariantCulture)
                              + "|" + stepSideAnimOffsetList[i].z.ToString(CultureInfo.InvariantCulture);
            }

            /*   + "_" + rbTmp.transform.position.x.ToString()
               + "_" + rbTmp.transform.position.y.ToString()
               + "_" + rbTmp.transform.position.z.ToString();*/
        }
        else
        {
            timeLeftForSideStepping = 0f;
            gkOperations += "NUL";
        }

        /*print("#DBGK1024_SIMULATE_DBG2435_CREATE before main anim gkHitPointCalc gkPredictCalc PREDICT rbTmp.transform.position " + rbTmp.transform.position
            + " shotPassedTime " + shotPassedTime
            + " timeToHitZ " + timeToHitZ);
        */
        //getRotatedRbToBall(peerPlayer.getBallInit(),
                getRotatedRbToBall(prepareShotPos[0, 0],
                                   rbTmp,
                                   ref rotatedRbToBallTmp,
                           //ref getRotatedRbToBallRef(),
                                   getGkCornerPoints());

        gkHitPointCalc(rbTmp,
                       shotvariant,
                       outShotStart,
                       outShotMid,
                       outShotEnd,
                       endPosOrg,
                       timeofBallFly,
                       shotPassedTime,
                       //rotatedRbToBall,
                       rotatedRbToBallTmp,
                       ref localSpace,
                       ref timeToHitZ,
                       ref curvePercentHit,
                       prepareShotPos,
                       prepareShotMaxIdx);

        correctLocalOffsetMax(ref localSpace, shotvariant, false);
        //add step side delay
        /////stepSideDelay = Mathf.Max(0, timeToHitZ - MAX_TIME_GK_PLAYER_NEED_TO_ANIMATE);

        //print("#DBG342344COL delay after main anim gkHitPointCalc gkPredictCalc PREDICT rbTmp.transform.position " + rbTmp.transform.position
        //    + " shotPassedTime " + shotPassedTime
        //    + " timeToHitZ " + timeToHitZ
        //    + " localSpace " + localSpace
        //    + " stepSideDelay " + stepSideDelay
        //    + " gkOperations " + gkOperations);


        //print("#DEBUGGK1045 gkHitPointCalc after after stepping side PREDICT rbTmp.transform.position " + rbTmp.transform.position
        //    + " timeToHitZ before " + timeToHitZ + " shotPassedTime " + shotPassedTime);            

        //timeToHitZ = timeToHitZ - ((Globals.FIXEDUPDATE_TIME * rotationLoops) +
        //                          (sidesteppingExecutionTime * percentOfSideSteppingExecuted));

        //print("#DBGK1024_SIMULATE_EARLY_STEPPING gkPredictCalc PREDICT AFTER SIDE STEP timeToHitZ " + timeToHitZ + " percentOfSideSteppingExecuted " + percentOfSideSteppingExecuted
        //    + " sidesteppingExecutionTime " + sidesteppingExecutionTime + " timeToHitZ  " + timeToHitZ);

        predictedAnimName = "";
        float predictedDelay = 0f;

        //print("DBGK1024_SIMULATE_EARLY_DBG1 ###################");

        //print("#DBGBPRIV rotated 5 " + rotatedRbToBall.transform.position);

        gkAnimationPlayPrediction(rbTmp,
                                  //rotatedRbToBall,
                                  rotatedRbToBallTmp,
                                  ballRb[activeBall],
                                  outShotStart,
                                  outShotMid,
                                  outShotEnd,
                                  endPosOrg,
                                  timeofBallFly,
                                  ref shotPassedTime,
                                  localSpace,
                                  //TODO
                                  //isLobActive, 
                                  false,
                                  timeToHitZ,
                                  shotvariant,
                                  ref lastGkDistX,
                                  ref gkTimeToCorrectPos,
                                  ref gkSideAnimPlayOffset,
                                  ref localGkStartPos,
                                  ref lastGkAnimName,
                                  ref predictedAnimName,
                                  ref predictedDelay,
                                  ref predictedAnimSpeed,
                                  ref curvePercentHit,
                                  prepareShotPos,
                                  prepareShotMaxIdx);
        ///predictedDelay += stepSideDelay;

        //print("#DBGCOLLISIONCALC1024DB predictedDelay calculated " + predictedDelay + " timeToHitZ " + timeToHitZ);

        //print("#DEBUGGK1045 gkPredictCalc PREDICTED START POS " + localGkStartPos
        //    + " predictedDelay " + predictedDelay
        //    );
        lastGkAnimNameVirtual = predictedAnimName;

        gkOperations += ":A|" + predictedAnimName
                     + "|"
                     + predictedAnimSpeed.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + predictedDelay.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + gkTimeToCorrectPos.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + localGkStartPos.x.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + localGkStartPos.y.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + localGkStartPos.z.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + lastGkDistX.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.position.x.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.position.y.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.position.z.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.eulerAngles.x.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.eulerAngles.y.ToString(CultureInfo.InvariantCulture)
                     + "|"
                     + rbTmp.transform.eulerAngles.z.ToString(CultureInfo.InvariantCulture);


        //print("DEBUGGK1045 gk prediction play main ### " + rbTmp.transform.position + " rotation " +
        //          rbTmp.transform.eulerAngles + " localGkStartPos " + localGkStartPos +
        //          " predictedAnimName " + predictedAnimName +
        //          " localSpace " + localSpace);

        //        print("#DBGK1024_SIMULATE_DBG2435_CREATE gkOperations calculated " + gkOperations);

        if (photonView.IsMine)
        {
            //          print("#DBGK1024_SIMULATE_DBG2435_CREATE gkPredictCalc " + gkOperations + " rb.transform.pos " + rbTmp.transform.position);
            //print("DBG342344COL CLICKED PredictCollision.getOnAnimatorAcitve() " + PredictCollision.getOnAnimatorAcitve()
            //    + " rotatedRbToBall.transform.rotation " + rotatedRbToBall.transform.eulerAngles);
            predictGkCollisionActive = true;
            ///rbTmp.transform.rotation = rotatedRbToBall.transform.rotation;

            //StartCoroutine(predictGkCollision(
            /*     predictGkCollision(
                 outShotStart,
                 outShotMid,
                 outShotEnd,
                 rbTmp,
                 rotatedRbToBallTmp,
                 gkOperations,
                 predictedAnimName,
                 gkTimeToCorrectPos,
                 timeofBallFly,
                 curvePercentHit,
                 lastGkDistX,
                 gkStartSeq,
                 localGkStartPos);*/
        }

        //print("#DBGBPRIV rotated 7 " + rotatedRbToBall.transform.position);


        //print("#DBGGKUPDATEVAL ");
        return gkOperations;
    }

    public string getLastGkAnimNameVirtual()
    {
        return lastGkAnimNameVirtual;
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
                               ballRb[activeBall].transform.position, shotEndPos, 1.2f);

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
    private void getGkShotIntersection(SHOTVARIANT2 SHOTVARIANT2,
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
        if (shotvariant == SHOTVARIANT2.CURVE)
            return true;
        return false;
    }

    public SHOTVARIANT2 getShotVariant()
    {
        return SHOTVARIANT2.CURVE;
        //return shotvariant;
    }

    public float getGkLastDistXCord()
    {
        return lastGkDistX;
    }

    private void correctLocalOffsetMax(ref Vector3 localPos, SHOTVARIANT2 SHOTVARIANT2, bool isCpu)
    {
        /*LEVEL DEPENDENT*/
        float maxOffset = MAX_GK_OFFSET;
        if (isCpu)
        {
            maxOffset = MAX_GK_OFFSET_CPU;
            /*TOCHANGE*/
            if (SHOTVARIANT2 == SHOTVARIANT2.CURVE &&
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
                     SHOTVARIANT2 SHOTVARIANT2)
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
                                 ref timeToStartPos,
                                 localSpace,
                                 SHOTVARIANT2,
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
                                                SHOTVARIANT2 SHOTVARIANT2,
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

        if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
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

    private void RPC_initConfirm()
    {
        RPC_confirmation = new bool[Enum.GetNames(typeof(RPC_ACK)).Length];
        RPC_lastUpdateTime = new float[Enum.GetNames(typeof(RPC_ACK)).Length];
        RPC_sequenceNumber = new short[Enum.GetNames(typeof(RPC_ACK)).Length];
        //RPC_expectedSequenceNumber = new short[Enum.GetNames(typeof(RPC_ACK)).Length];

        for (int i = 0; i < RPC_confirmation.Length; i++)
        {
            RPC_confirmation[i] = false;
            RPC_lastUpdateTime[i] = Time.time;
            RPC_sequenceNumber[i] = 0;
            //C_expectedSequenceNumber[i] = 0;
        }
    }

    private void initColliders()
    {
        wallsCollider = new List<Collider>();
        playersCollider = new List<Collider>();

        foreach (var collider in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (collider.name.Contains("wall") ||
                collider.name.Contains("goalDownPost") ||
                collider.name.Contains("goalDownCross") ||
                collider.name.Contains("goalUpPost") ||
                collider.name.Contains("goalUpCross"))
            {
                wallsCollider.Add(collider.GetComponent<Collider>());
            }

            if (collider.name.Equals("Spine") ||
                (collider.name.Contains("playerDown") &&
                 collider.name.Contains("Collider")))
            {
                playersCollider.Add(collider.GetComponent<Collider>());
            }
        }
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
                                         SHOTVARIANT2 SHOTVARIANT2)
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
                if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
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
                if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
                {
                    prob = 50;
                    minOffset = 4.0f;
                }
                delayTime = 0.12f;*/
                minVel = UnityEngine.Random.Range(16.0f, 19f);
                minOffset = UnityEngine.Random.Range(1.5f, 2.1f);
                prob = UnityEngine.Random.Range(70, 85);

                if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
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
                if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
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
                if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
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
                                         SHOTVARIANT2 SHOTVARIANT2,
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
                                                    SHOTVARIANT2);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
                //print("DEBUGGK1045 timeToStartPOS " + timeToStartPos);
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
        if (isShotActive() || peerPlayer.getShotActive())
        {
            float goalXOff = goalXOffset + 0.3f;
            if (peerPlayer.getShotActive())
                goalXOff = goalXOffset + 0.3f;

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

       /* if (PhotonNetwork.IsMasterClient)
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
        {*/
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
        //}
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
        }

        if (realTime > 7.0)
            matchStatisticsNext.SetActive(true);

        if (realTime > 9.0f)
        {
            audioManager.Stop("fanschantBackground2");
            audioManager.Play("music2");
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

    private void setShotAnimSpeed(Animator animator,
                                  string shotType,
                                  float lagDelay)
    {
        //normal shot - frame 12
        float frame = 13;
        float framesPerSeconds = 30;

        //TODO
        //error handling
        //what if 0.666f - lagDelay is below zero or zero!!
        //float speed = frame / (0.666f - lagDelay) / framesPerSeconds;
        //float speed = frame / (0.466f - lagDelay) / framesPerSeconds;
        //gain 200 ms
        //With speed 0.6f, 13 frames executes 0.541 ms
        float speed = frame / (0.441f - lagDelay) / framesPerSeconds;
        if ((0.441f - lagDelay) < 0.150f)
            speed = 1.3f;

        if (shotType.Contains("volley"))
        {
            frame = 19;
            //it takes 633 with speed 0.7 to execute volley_before anim
            speed = frame / (0.633f - lagDelay) / framesPerSeconds;
            if ((0.633f - lagDelay) < 0.150f)
                speed = 1.3f;
        }

        speed = Mathf.Clamp(speed, 0.9f, 1.3f);

        //print("#DBGANIM SPEED " + speed);
        if (!shotType.Contains("volley"))
        {
            animator.SetFloat("3d_normal_shot_speed", speed);
            //print("#DBGSHOT12## 3d_normal_shot_speed " + speed);
            //print("SetAnim Speed 3d_normal_shot_speed " + speed);
        }
        else {
            animator.SetFloat("3D_volley_before_speed", speed);
            //print("SetAnim Speed 3D_volley_before_speed " + speed);
        }


        //print("DBGSTEPSIDE_123433 shotSpeed " + speed + " lagDelay " + lagDelay);
        //float standardAnimDelay = 0.166f + lagDelay;

        //add volley
        //animator.SetFloat("3d_normal_shot_speed", animationShotAnimSpeed["3d_normal_shot_speed"]);
        //animator.SetFloat("3d_volley_shot_speed", animationShotAnimSpeed["3d_volley_shot_speed"]);

    }

    public void setShotActive(bool active)
    {
        //print("DBGSHOT setShotActive executed" + active);
        shotActive = active;
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

                ///if (animator.isMatchingTarget || animator.IsInTransition(0))
                //{
                   //print("DEBUGGK1045 in transition ### matchTarget prep normTime " + normTime + " gkEndPos " + gkEndPos +
                   //      " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position
                   //     + " timeToCorrectPos " + timeToCorrectPos); 
               // }

                animator.MatchTarget(new Vector3(gkEndPos.x, rb.transform.position.y, gkEndPos.z),
                                                 Quaternion.identity, AvatarTarget.Root,
                                                 new MatchTargetWeightMask(Vector3.one, 0f), 0.0f, timeToCorrectPos);
                //print("DBGCOLLISIONCALC1024DB STAart### matchTarget prep normTime " + normTime + " gkEndPos " + gkEndPos +
                //    " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);
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
                    //print("DEBUGGK1045 matchSave POS " + rb.transform.position);
                    matchInitSavePos = true;
                }

                //rb.transform.position = matchSavePos;
                rb.transform.position = new Vector3(gkEndPos.x, rb.transform.position.y, gkEndPos.z);
                //print("DEBUGGK1045 matchTarget SET SAVED POS prep normTime " + normTime + " gkEndPos " + gkEndPos +
                //     " timeToCorrectPos " + timeToCorrectPos + " rb.transf" + rb.transform.position);
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
            {
                //print("DBGCOLLISIONCALC1024DB stepOffset matchTarget IsInTransition " + stepSideAnimOffset);
                return;
            }

            animator.MatchTarget(stepSideAnimOffset,
                                 Quaternion.identity,
                                 AvatarTarget.Root,
                                 new MatchTargetWeightMask(Vector3.one, 0f),
                                 0.0f,
                                 1.0f);
            //print("DBGCOLLISIONCALC1024DB stepOffset matchTarget " + stepSideAnimOffset);

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
        bool isCpuShotActive = peerPlayer.getShotActive();

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

                if (isTouchInsidePowerButtons(touch.position))
                    continue;

                touch = Input.GetTouch(i);
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
            }
            else
            {
                gkTouchDone = false;
                rpc_gkMovementSend = false;
                initGkMoves = false;
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
            //trailShootRenderer.sortingOrder = 1;
            //trailShoot.transform.rotation = Quaternion.identity;
            //trailShoot.transform.position = Vector3.zero;

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

        ///print("DBG342344COL ROTATION updateLastGk gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS
        ///    + " peerPlayer.getBallInit() " + peerPlayer.getBallInit());
        getRotatedRbToBall(peerPlayer.getBallInit(),
                           rb,
                           ref rotatedRbToBall,
                           //ref getRotatedRbToBallRef(),
                           getGkCornerPoints());

        Plane rbRotatedPlane = new Plane(
            rotatedRbToBall.transform.forward,
            rotatedRbToBall.transform.position);

        //if (!isMaster)
        //{
        //    rbRotatedPlane = new Plane(
        //        rotatedRbToBall.transform.forward * -1f,
        //        rotatedRbToBall.transform.position);
        //}

        //print("DBGCOLLISIONCALC1024D updateLastGkTouchPos rotatedRbToBall " + rotatedRbToBall.transform.position +
        //    " rotatedRbToBall.transform.forward " + rotatedRbToBall.transform.forward + " rotatedRbToBall " +
        //    rotatedRbToBall.transform.eulerAngles + " peerPlayer.getBallInit()) " +
        //    peerPlayer.getBallInit());

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
            ///print("DBGCOLLISIONCALC1024D updateLastGkTouchPos gkTouchPosRotatedRbWS " + gkTouchPosRotatedRbWS);
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

    private int checkIfJoystickExtraButtonsPlaying(Animator animator, float end)
    {
        //animator.Update(0f);

        for (int i = 0; i < JoystickButtonAnimNames.Count; i++)
        {
            if (isPlaying(animator, JoystickButtonAnimNames[i], end))
                return i;
        }

        return -1;
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

    public bool checkIfAnyAnimationPlayingContain(List<string> list,
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


    public bool checkIfAnyAnimationPlayingContain(Animator animator, float end, string contain)
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
       /* if (isMaster (((ballRb[activeBall].transform.position.z) < -15f) ||
                          (Mathf.Abs(ballRb[activeBall].transform.position.x) &&
                          ballRb[activeBall].transform.position.z) < 0f))
            return true;*/
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

        //print("DBG342344COL clearVariables()");
        gkStartSequenceTime = -1f;
        //joystick.setDefaultColorButton();
        peerPlayer.setShotActive(false);

        delayAfterGoal = 0.0f;
        //print("VELOCITY CLEARED HERE 4");

        ballRb[activeBall].velocity = Vector3.zero;
        ballRb[activeBall].angularVelocity = Vector3.zero;

        clearAfterBallCollision();
        isBallInGame = true;
        goalJustScored = false;
        //isBallOut = false;
        shotRet = false;
        onBall = PlayerOnBall.NEUTRAL;

        gkStartSequenceTime = -1f;
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

        updateBallDuringShot = false;
        isUpdateBallVelDuringShot = false;

        //print("DBG342344COL clearAfterBallCollision");
        rpc_preShotCalc = false;
        rpc_shotActive = false;
        rpc_playerOnBallActive = false;
        prepareShotRpcSend = false;
        RPC_gkPacketProcessed = false;

        initPreShot = false;
        initPreShotRPC = false;
        initShot = false;
        prepareShotRpcSend = false;

        if (photonView.IsMine)
        {
            peerPlayer.clearAfterBallCollision();
        }

        passedShotFlyTime = 0.0f;
        setWallColliders(true);

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

        shotRet = false;
        initPreShot = false;
        initPreShotRPC = false;
        initShot = false;
        prepareShotRpcSend = false;
        isLobActive = false;
        isBallTrailRendererInit = false;
        touchCount = 0;
        drawDistance = 0.0f;
        touchLocked = false;
        gkTouchDone = false;
        RPC_gkPacketProcessed = false;
        rpc_gkMovementSend = false;
        initGkMoves = false;
        initCpuAdjustAnimSpeed = false;
        initGKPreparation = false;
        initGkDeleyLevel = false;

        gkRotationLoops = 0;
        gkSideAnimDelayBefore = 0f;
        gkSideAnimDelayBeforeStart = -1f;
        GK_DEBUG_INIT = false;

        gkSideOffsetListCurrIdx = 0;
        gkSideOffsetListMax = 0;
        stepSideAnimOffsetList.Clear();

        levelDelay = 0.0f;

        isTouchBegin = false;
        touchFingerId = -1;
        gkStartSequenceTime = -1f;
        ///if (photonView.IsMine)
        setGkHelperImageVal(false);
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

    public bool isMasterPlayer()
    {
        return PhotonNetwork.IsMasterClient;
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

    private void updateGameSettings()
    {
        Globals.matchTime = "90 SECONDS";
        Globals.isTrainingActive = false;
    }

    private Vector3 getBallHitRbPoint(GameObject gameObjectRbRotated,
                                      SHOTVARIANT2 SHOTVARIANT2,
                                      Vector3 outShotStart,
                                      Vector3 outShotMid,
                                      Vector3 outShotEnd,
                                      Vector3[,] prepareShotPos,
                                      int prepareShotMaxIdx)        
    {
        Vector3 realHitPlace = INCORRECT_VECTOR;

        if (SHOTVARIANT2 == SHOTVARIANT2.CURVE)
        {
            if (Mathf.Abs(gameObjectRbRotated.transform.position.z) <= 12.8f) {
                realHitPlace = bezierCurvePlaneInterPoint(
                                                     gameObjectRbRotated,
                                                     prepareShotPos,
                                                     0,
                                                     prepareShotMaxIdx - 1,
                                                     false);
            } else {
                realHitPlace = bezierCurvePlaneInterPoint(0.0f,
                                                          1.0f,
                                                          gameObjectRbRotated,
                                                          outShotStart,
                                                          outShotMid,
                                                          outShotEnd,
                                                          false);
            }
            //print("DBG1024 localSpace " + realHitPlace);
            //print("DEBUGLASTTOUCHLUCKXYU #### realHitPlace LOCALSPACE "
            //    + realHitPlace + " gameObjectRbRotated " + gameObjectRbRotated.transform.position);

            //if (Mathf.Abs(realHitPlace.z) > 0.5f)
            //    return INCORRECT_VECTOR;

            realHitPlace.z = 0.0f;
            realHitPlace = TransformPointUnscaled(gameObjectRbRotated.transform, realHitPlace);

            //print("DEBUGLASTTOUCHLUCKXYU #### realHitPlace WORLD CURVED " + realHitPlace +
            //  " outShotStart " + outShotStart + " outShotMid " + outShotMid + " outShotEnd " + outShotEnd);
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
                                    SHOTVARIANT2 SHOTVARIANT2,
                                    Vector3 outShotStart,
                                    Vector3 outShotMid,
                                    Vector3 outShotEnd,
                                    Vector3[,] prepareShotPos,
                                    int prepareShotMaxIdx)        
    {
        /*Vector3 direction = endPosOrg - ballRb[activeBall].position;
        Plane playerXLocalPlane = new Plane(
                                            rb.transform.forward,
                                            rb.transform.position);*/

        /*print("DEBUGLASTTOUCHLUCKXYU DRAW HELPER CIRCLE START SHOTV " + SHOTVARIANT2 +
             " OUTSHOTS " + outShotStart + " MID " + outShotMid + " OUTSHOTEND " + outShotEnd);*/
        Vector3 hitPointWS = 
            getBallHitRbPoint(
                gameObjectRbRotated, 
                SHOTVARIANT2, outShotStart, outShotMid, outShotEnd, prepareShotPos,
                prepareShotMaxIdx);
        //print("DEBUGLASTTOUCH HELPER IMAGE " + hitPointWS + " SHOTVARIANT2 " + SHOTVARIANT2);
        if (hitPointWS != INCORRECT_VECTOR)
        {
            gkHelperRectTransform.position = m_MainCamera.WorldToScreenPoint(hitPointWS);
            gkHelperImage.enabled = true;
        }


        /*print("#DBG1024 rb.transform.position " + getPlayerRb().transform.position 
            + " gameObjectRbRotated pos " + gameObjectRbRotated.transform.position
            + " gameObjectRbRotated Rot " + gameObjectRbRotated.transform.eulerAngles
            + " hitPointWS " + hitPointWS
            + " outShotStart " + outShotStart
            + " outShotMid "+ outShotMid
            + " outShotEnd " + outShotEnd
            + " prepareShotMaxIdx "+ prepareShotMaxIdx);*/

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

        if (!photonView.IsMine)
            return;

        //print("DBG342344COL try to set setGkHelperImageVal " + val
        //     + " photonView.IsMine " + photonView.IsMine);

        gkHelperImage.enabled = val;
        if (!val)
            gkClickHelper.enabled = val;

        //MarkerBasic.SetActive(val);
        //if (!val)
        //{
        ///print("DBG342344COL set to setGkHelperImageVal " + val);
        //    gkClickHelper.enabled = val;
        //}
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

    public float getAnimationOffsetTime(string shotType)
    {
        if (shotType.Contains("volley"))
        {
            return 0.45f;
        }

        return animationOffsetTime[shotType];
    }

    private bool isShotSpeedBelow(float speed)
    {
        if (Mathf.Abs(ballRb[activeBall].velocity.x) < speed &&
            Mathf.Abs(ballRb[activeBall].velocity.z) < speed)
            return true;
        return false;
    }

     public GameObject getChildWithName(GameObject obj, string name)
    {
        if (obj == null)
            return null;

        foreach (Transform objTransform in obj.transform.GetComponentsInChildren<Transform>())
        {
            if (objTransform.gameObject.name.Equals(name) ||
                objTransform.gameObject.tag.Equals(name))
            {
                return objTransform.gameObject;
            }
        }

        return null;
    }

    private int getnumberOfFramesOfAnim(string animName)
    {
        int frameNumbers = 0;
        if (animName.Contains("punch"))
        {
            if (animName.Contains("up"))
                frameNumbers = 141;
            if (animName.Contains("mid"))
                frameNumbers = 117;
            if (animName.Contains("down"))
                frameNumbers = 131;
        }  else
        {
            if (animName.Contains("up"))
                frameNumbers = 109;
            if (animName.Contains("mid"))
                frameNumbers = 104;
            if (animName.Contains("down"))
                frameNumbers = 117;
        }

        return frameNumbers;
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
                    backSpeed = Mathf.Max(8.5f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.5f, Mathf.Max(7.6f, speed * 0.8f));
                break;
            case 4:
                if (rand != 0)
                    backSpeed = Mathf.Max(8.0f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(7.0f, Mathf.Max(7.1f, speed * 0.8f));
                break;
            case 3:
                if (rand != 0)
                    backSpeed = Mathf.Max(7.5f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(6.5f, Mathf.Max(6.5f, speed * 0.8f));
                break;
            case 2:
                if (rand != 0)
                    backSpeed = Mathf.Max(6.5f, speed);
                else
                    backSpeed = UnityEngine.Random.Range(6.0f, Mathf.Max(6.1f, speed * 0.8f));
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
   
    public void playerFreeze(bool freezePlayer)
    {
        isPlayerFreeze = freezePlayer;
    }

    public void setExtraGoals(bool val)
    {
        isExtraGoals = val;
    }

    public void setExtraGoals(bool val, Vector3 size)
    {
        isExtraGoals = val;
        recalculateCornerPoints(size);
    }

    public void recalculateCornerPoints(Vector3 sizes)
    {
        Vector3 goalSizesCpuTakeAction = new Vector3(goalSizes.x,
                                                     goalSizes.y,
                                                     goalSizes.z);

        float levelInter = getLevelInterpolationReverse();

        /*LEVEL DEPENDENT*/
        goalSizesCpuTakeAction.x *= (1.5f + levelInter);
        goalSizesCpuTakeAction.y *= (1.3f + levelInter);

        gkCornerPoints = new Vector3(
            goalSizesCpuTakeAction.x, 0f, PITCH_HEIGHT_HALF);
    }
}