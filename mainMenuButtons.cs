using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;
using UnityEngine.Analytics;
using TMPro;
using Photon.Pun;
using LANGUAGE_NS;

public class mainMenuButtons : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject gameMainCanvas;
    public GameObject gameSettingsCanvas;
    public GameObject friendlyCanvas;
    public GameObject tournamentCanvas;
    public GameObject trainingCanvas;


    public GameObject mainGameSettingButton;
    public GameObject adsCanvas;
    public GameObject leaderBoardReportQuestion;
    public GameObject loadGameCanvas;
    public TextMeshProUGUI menuPointsText;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    private AudioSource audioClip;
    private bool waitingForAddEvent = false;
    public gameSettings gameSettingsScript;
    //public ReviewManagerScript reviewManager;
    private LeagueBackgroundMusic leagueBackgroundMusic;
    public GameObject[] menuButtonsObj;
    public GameObject infoCanvas;
    public TextMeshProUGUI infoHeaderText;
    public TextMeshProUGUI infoDescText;
    public RawImage infoImage;
    private int COINS_NEEDED_TO_PLAY = 10;

    void Awake()
    {
        if (Globals.PITCHTYPE.Equals("STREET")) {
            //menuButtonsObj[0].SetActive(false);
            menuButtonsObj[7].SetActive(false);
        }


        infoCanvas.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = false;

        leagueBackgroundMusic =
                GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
        //reviewManager = GameObject.Find("reviewApiGoogle").GetComponent<ReviewManagerScript>();
        adsCanvas.SetActive(false);
        audioClip = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        //audioClip.volume = 0.1f;

        AudioListener.volume = 1f;
        leagueBackgroundMusic.stop();
    }

    public void Start()
    {       
        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

        //add extra conditions
        if (Globals.numMatchesInThisSession > 0 &&
            Globals.adsEnable)
        {
            admobAdsScript.hideBanner();
            if (admobAdsScript.showInterstitialAd())
            {
                adsCanvas.SetActive(true);
                waitingForAddEvent = true;
            } else
            {
                //showBannerAd();
            }
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
        }
        else
        {
            //showBannerAd();
        }

        menuPointsText.text = Globals.coins.ToString();
        gameSettingsCanvas.SetActive(false);
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainGameSettingButton.SetActive(false);
        leaderBoardReportQuestion.SetActive(false);
        loadGameCanvas.SetActive(false);

        if (!waitingForAddEvent)
            audioClip.Play();

        if (((Globals.numGameOpened >= 10) && (Globals.numGameOpened % 5 == 0)) &&
              PlayerPrefs.GetInt("appReview_DontAskAgainClicked") != 0) {
            //reviewManager.RequestReview();
            if (Globals.isAnalyticsEnable)
            {
                AnalyticsResult analyticsResult =
                    Analytics.CustomEvent("Review_API", new Dictionary<string, object>
                {
                    { "show_review", PlayerPrefs.GetInt("appReview_DontAskAgainClicked")},
                    { "show_review_num_game_opened", Globals.numGameOpened}
                });
            }
        }

        /*will api review showned when user already rated app*/
        /*if (Globals.wonFinal ||
           (Globals.numGameOpened > 4 && Globals.numMatchesInThisSession > 0) ||
           (Globals.numGameOpened >= 6 && ((Globals.numGameOpened % 3) == 0)))*/
        /*if (Globals.wonFinal ||
           (Globals.numGameOpened > 4 && Globals.numMatchesInThisSession > 0) ||
           (Globals.numGameOpened >= 6 && ((Globals.numGameOpened % 3) == 0)))
        {
            if ((Globals.numMainMenuOpened % 2) == 0)
            {
                reviewManager.RequestReview();
                if (Globals.isAnalyticsEnable)
                {
                    AnalyticsResult analyticsResult =
                        Analytics.CustomEvent("Review_API", new Dictionary<string, object>
                    {
                    { "show_review", true},
                    });
                }
            }
            Globals.numMainMenuOpened++;
        }*/
    }

    void Update()
    {
        if (Globals.adsEnable &&
            waitingForAddEvent &&
            (admobAdsScript.getAdsClosed() ||
             admobAdsScript.getAdsFailed()))
        {
            waitingForAddEvent = false;
            adsCanvas.SetActive(false);
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
            audioClip.Play();
            //showBannerAd();
        }
    }

    private void showBannerAd()
    {
        if (admobGameObject != null &&
            admobAdsScript  != null)
        {
            admobAdsScript.showBannerAd();
        }
    }

    public void playFriendly()
    {
        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;

        prepareNewGame();
        Globals.gameType = "FRIENDLY";
        //showBannerAd();
        gameMainCanvas.SetActive(false);
        leaderBoardReportQuestion.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainGameSettingButton.SetActive(false);
        gameSettingsCanvas.SetActive(true);

        Globals.isFriendly = true;
        Globals.isLeague = false;
        Globals.onlyTrainingActive = false;
        Globals.wonFinal = false;
        Globals.isLevelMode = false;
    }

    public void enterTournamentMenu()
    {
        if (checkIfEnoughCoinsToPlay())
            return;

        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;

        prepareNewGame();
        Globals.gameType = "CUP";
        //showBannerAd();
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainGameSettingButton.SetActive(false);
        gameMainCanvas.SetActive(false);
        leaderBoardReportQuestion.SetActive(false);
        //gameSettingsCanvas.SetActive(true);
        loadGameCanvas.SetActive(true);

        gameSettingsScript.initGameSaves();
        //gameSettingsScript.setupGameTypeDefaults();

        //print("DBGTOURNAMENT enterTournamentMenu " + gameSettingsCanvas.activeSelf);
        Globals.isFriendly = false;
        Globals.isLeague = false;
        Globals.onlyTrainingActive = false;
        Globals.wonFinal = false;
    }

    private bool checkIfEnoughCoinsToPlay()
    {
        if (Globals.coins < COINS_NEEDED_TO_PLAY)
        {
            infoCanvas.SetActive(true);
            infoHeaderText.text = "Oops..";
            infoDescText.text =
                Languages.getTranslate("Sorry. You must have at least 10 coins to play in this mode. Play Friendly first");
            infoImage.texture = Resources.Load<Texture2D>("Shop/shopNotificationCoins");
            return true;
        }

        return false;
    }

    public void enterSeasonMenu()
    {
        if (checkIfEnoughCoinsToPlay())
            return;

        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;
        prepareNewGame();
        //showBannerAd();
        Globals.gameType = "LEAGUE";

        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainGameSettingButton.SetActive(false);
        gameMainCanvas.SetActive(false);
        leaderBoardReportQuestion.SetActive(false);
        //gameSettingsCanvas.SetActive(true);

        gameSettingsScript.initGameSaves();
        //gameSettingsScript.setupGameTypeDefaults();

        loadGameCanvas.SetActive(true);

        //print("DBGTOURNAMENT enterTournamentMenu " + gameSettingsCanvas.activeSelf);
        Globals.isFriendly = false;
        Globals.isLeague = true;
        Globals.onlyTrainingActive = false;
        Globals.wonFinal = false;
    }

    public void prepareNewGame()
    {
        Globals.gameEnded = false;
    }

    public void enterShopMenu()
    {  
        admobAdsScript.hideBanner();
        SceneManager.LoadScene("shop");
    }

    public void playTraning()
    {
        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;

        int graphicsSettingsIdx = 2;
        int joystickSideIdx = 0;

        //gameMainCanvas.SetActive(false);
        //leaderBoardReportQuestion.SetActive(false);

        if (PlayerPrefs.HasKey("gameSettingsSave"))
        {
            graphicsSettingsIdx = PlayerPrefs.GetInt("graphicsSettingsIdx");
            joystickSideIdx = PlayerPrefs.GetInt("joystickSideIdx");
        }
   

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            Globals.stadiumNumber = 1;
        }

        Globals.graphicsQuality =
            gameSettingsScript.getGraphicStringByIndex(graphicsSettingsIdx);

        Globals.joystickSide = 
            gameSettingsScript.getJoystickSideStringByIndex(joystickSideIdx);

        Globals.isTrainingActive = true;
        Globals.onlyTrainingActive = true;
        Globals.wonFinal = false;

        admobAdsScript.hideBanner();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            Globals.stadiumNumber = 1;
            Globals.loadSceneWithBarLoader("gameSceneSportsHall");
            return;
        }

        if (Globals.PITCHTYPE.Equals("STREET"))
        {
            Globals.stadiumNumber = 2;
            Globals.commentatorStr = "NO";
            Globals.loadSceneWithBarLoader("gameSceneStreet");
            return;
        }

        Globals.loaderBarSceneName = "gameScene";
        SceneManager.LoadScene("gameLoader");
    }

    public void onClickCustomize()
    {
        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;
        Globals.levelModeTimeOffset = 0;

        admobAdsScript.hideBanner();

        Globals.loadSceneWithBarLoader("customize");
    }

    public void onClickLevelsMenu()
    {
        Globals.isMultiplayer = false;
        Globals.isLevelMode = false;

        admobAdsScript.hideBanner();

        Globals.loadSceneWithBarLoader("LevelsMenu");
    }

    public void onClickOnlineMultiplayer()
    {
        Globals.isMultiplayer = true;
        Globals.isMultiplayerUpdate = false;
        Globals.isLevelMode = false;

        if (Globals.PITCHTYPE.Equals("STREET"))
            Globals.commentatorStr = "NO";
        else
            Globals.commentatorStr = "YES";

        Globals.powersStr = "YES";
        Globals.joystickSide = "LEFT";
        Globals.graphicsQuality = "STANDARD";

        Globals.loadSceneWithBarLoader("multiplayerMenu");
    }

    public void showLeaderBoardOnClick()
    {

        #if UNITY_ANDROID
            PlayGamesPlatform.Activate();
        #endif
        
        //if (!PlayerPrefs.HasKey("leaderBoardPermissionsDone"))
        //{
        //    leaderBoardReportQuestion.SetActive(true);
        //    return;
        //}

        showLeaderBoard();
    }


    public void showLeaderBoard()
    {
        #if UNITY_ANDROID   
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {        
                PlayGamesPlatform.Instance.Authenticate((bool success) => {
                    if (success)
                    {
                        LeaderBoard.OnShowLeaderBoard();
                    } else
                    {
                    }
                });            
        }
        else
        {         
            LeaderBoard.OnShowLeaderBoard();
        }

        #endif

    }

    public static void Login()
    {
        #if UNITY_ANDROID
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success)
                {                   
                }                
            });
        }
        #endif
    }

    IEnumerator showLeaderBoardCoroutine()
    {
        #if UNITY_ANDROID
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            LeaderBoard.Login();           
            for (int j = 0; j < 100; j++)
            {
                if (!PlayGamesPlatform.Instance.localUser.authenticated)
                    yield return new WaitForSeconds(0.8f);
                else
                    break;
            }
        }

        LeaderBoard.OnShowLeaderBoard();
        #else
        yield return new WaitForSeconds(0.1f);  
        #endif
    }

    public void leaderBoardPermissionsYes()
    {
        PlayerPrefs.SetInt("leaderBoardPermissionsDone", 1);
        PlayerPrefs.Save();

        leaderBoardReportQuestion.SetActive(false);
        showLeaderBoard();
    }

    public void leaderBoardPermissionsNo()
    {
        leaderBoardReportQuestion.SetActive(false);
        showLeaderBoard();
    } 

    public void mainGameSettingsButton()
    {
        //showBannerAd();
        gameSettingsCanvas.SetActive(false);
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        gameMainCanvas.SetActive(false);
        leaderBoardReportQuestion.SetActive(false);
        mainGameSettingButton.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void onClickCloseInfoCanvas()
    {
        infoCanvas.SetActive(false);
    }
}
