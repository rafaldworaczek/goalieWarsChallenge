using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using GlobalsNS;
using MenuCustomNS;
using AudioManagerNS;
using TMPro;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    public GameObject admobCanvas;
    private bool waitingForInterstitialAddEvent = false;
    public TextMeshProUGUI currentCoinsText;
    public TextMeshProUGUI currentDiamondsText;
    public TextMeshProUGUI levelNumberText;
    private LeagueBackgroundMusic leagueBackgroundMusic;
    public GameObject loadingCanvas;
    private int levelNumber = 1;
    private string[] graphics = { "VERY LOW", "LOW", "STANDARD", "HIGH", "VERY HIGH" };
    private string[] joystickSide = { "LEFT", "RIGHT" };


    void Awake()
    {
        adInit();
        showInterstitialAd();
        
        levelNumberText.text = Globals.levelNumber.ToString();
        levelNumber = Globals.levelNumber;

        setMainSettings();
        setLevelSettings();
        Time.timeScale = 1f;

        Globals.isMultiplayer = false;
        Globals.isBonusActive = false;
        Globals.isTrainingActive = false;
        Globals.isLevelMode = true;

        if (Globals.PITCHTYPE.Equals("STREET"))
            Globals.commentatorStr = "NO";

   

        updateTreasure();
    }

    void Start()
    {
        initAudioClip();       
        //updateTeamSkills();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.adsEnable)
        {
            if (waitingForInterstitialAddEvent &&
               (admobAdsScript.getAdsClosed() ||
                admobAdsScript.getAdsFailed()))
            {
                waitingForInterstitialAddEvent = false;
                admobAdsScript.setAdsFailed(false);
                admobAdsScript.setAdsClosed(false);
                admobCanvas.SetActive(false);
            }           
        }
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

    private void updateTeamSkills()
    {
        string[] playerDesc = Globals.playerADesc.Split(':');
        int defenseSkills = int.Parse(playerDesc[2]);
        int attackSkills = int.Parse(playerDesc[3]);

        ///print("#DBGdefenseSkils " + defenseSkills + " attackSkills " + attackSkills);

        Globals.teamAGkStrength =
            Mathf.Clamp(defenseSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
        Globals.teamAAttackStrength =
           Mathf.Clamp(attackSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
        Globals.teamAcumulativeStrength =
            Globals.teamAGkStrength + Globals.teamAAttackStrength;
    }

   
    public void updateTreasure()
    {
        //print("currentCoinsText " + currentCoinsText);
        //print("currentDiamondsText " + currentDiamondsText);
        currentCoinsText.text = Globals.coins.ToString();
        currentDiamondsText.text = Globals.diamonds.ToString();
    }

    public void showInterstitialAd()
    {
        if (Globals.adsEnable)
        {
            if (admobAdsScript.showInterstitialAd())
            {
                admobCanvas.SetActive(true);
                waitingForInterstitialAddEvent = true;
            }
        }
    }

    public void onClickMainMenu()
    {
        Globals.loadSceneWithBarLoader("menu");
    }
  
    public void onClickExit()
    {
        Globals.loadSceneWithBarLoader("menu");
    }

    private void initAudioClip()
    {
        leagueBackgroundMusic =
            GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
        leagueBackgroundMusic.play();
    }

    public void onClickPlay()
    {
        //hideBanner();
        loadingCanvas.SetActive(true);

        if (Globals.stadiumNumber == 1)
        {
            Globals.loadSceneWithBarLoader("gameSceneSportsHall");
        }
        else if (Globals.stadiumNumber == 2)
        {
            Globals.loadSceneWithBarLoader("gameSceneStreet");
        }
        else
        {
            Globals.loadSceneWithBarLoader("gameScene");
        }
    }

    void setLevelSettings()
    {
        setGlobalLevel(levelNumber);
        setGlobalStadium(levelNumber);


        switch (levelNumber)
        {
            case 1:
                updateSettings();
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    private void setGlobalStadium(int levelNumber)
    {
        if (levelNumber <= 3 || levelNumber % 2 == 0 || levelNumber % 3 == 0)
            Globals.stadiumNumber = 0;
        else if (levelNumber % 5 == 0)
            Globals.stadiumNumber = 1;
        else
        {
            Globals.stadiumNumber = 2;
        }
    }

    private void setGlobalLevel(int levelNumber)
    {
        if (levelNumber < 5)
            Globals.level = 1;
        else if (levelNumber < 15)
            Globals.level = 2;
        else if (levelNumber < 30)
            Globals.level = 3;
        else if (levelNumber < 50)
            Globals.level = 4;
        else
            Globals.level = 5;
    }

    private void updateSettings()
    {
        Teams teams = new Teams("NATIONALS");

        for (int i = 0; i < 2; i++)
        {
            int teamRandomInt =
                UnityEngine.Random.Range(0, teams.getMaxTeams());
            string[] randTeam =
                teams.getTeamByIndex(teamRandomInt);

            if (i == 0)
            {
                Globals.teamAname = randTeam[0];
                //Globals.teamAid = teamRandomInt;
                Globals.teamAid = teamRandomInt;
                Globals.stadiumColorTeamA = randTeam[5];
                Globals.playerADesc = randTeam[12].Split('|')[0];
            }
            else
            {
                Globals.teamBname = randTeam[0];
                Globals.teamBid = teamRandomInt;
                Globals.stadiumColorTeamB = randTeam[5];
                Globals.playerBDesc = randTeam[12].Split('|')[0];
            }

            teams.swapElements(
                teamRandomInt, teams.getMaxActiveTeams() - 1);
        }

        /*Traning hardcoded values*/
        Globals.teamAcumulativeStrength = 160;
        Globals.teamBcumulativeStrength = 140;

        Globals.teamAGkStrength = 90;
        Globals.teamBGkStrength = 70;

        Globals.teamAAttackStrength = 70;
        Globals.teamBAttackStrength = 70;

        Globals.matchTime = "60 SECONDS";

        Globals.level = 3;

        Globals.teamAleague = "WORLD CUP";
        Globals.teamBleague = "WORLD CUP";

        Globals.score1 = 0;
        Globals.score2 = 1;

        Globals.maxTimeToShotStr = "8";
        //IT's in seconds
        Globals.levelModeTimeOffset = 30.0f;
    }

    private void setMainSettings()
    {
        Globals.isMultiplayer = false;
        Globals.isLevelMode = true;
        Globals.isTrainingActive = false;
        Globals.onlyTrainingActive = false;
        Globals.wonFinal = false;

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
            getGraphicStringByIndex(graphicsSettingsIdx);

        Globals.joystickSide =
            getJoystickSideStringByIndex(joystickSideIdx);

 
        admobAdsScript.hideBanner();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            Globals.stadiumNumber = 1;
            Globals.loadSceneWithBarLoader("gameSceneSportsHall");
        }

        if (Globals.PITCHTYPE.Equals("STREET"))
        {
            Globals.stadiumNumber = 2;
            Globals.commentatorStr = "NO";
            Globals.loadSceneWithBarLoader("gameSceneStreet");
        }
    }

    public string getGraphicStringByIndex(int graphicsSettingsIdx)
    {
        return graphics[graphicsSettingsIdx];
    }

    public string getJoystickSideStringByIndex(int joystickSideIdx)
    {
        return joystickSide[joystickSideIdx];
    }

    /*
    public void showInfoCanvas(string headerText, 
                               string descText,
                               string textureName)
    {
        infoHeaderText.text = headerText;
        infoDescText.text = descText;
        infoImage.texture = Resources.Load<Texture2D>(textureName);
        infoCanvas.SetActive(true);
    }

    public void onClickCloseInfoCanvas()
    {
        infoCanvas.SetActive(false);
        if (Globals.coins < COINS_NEEDED_TO_PLAY)
        {
            onClickMainMenu();
        }
    }
    */
}
