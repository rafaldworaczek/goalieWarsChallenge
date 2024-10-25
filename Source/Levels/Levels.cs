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
    public GameObject infoCanvas;

    void Awake()
    {
        infoCanvas.SetActive(false);

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
        //level of the game, skills
        setLevel();
        setTeamStrength();
        setStadium();
        setTeams();
        setLevelSpecificSettings();
    }

    private void setStadium()
    {
        if (levelNumber == 1 || levelNumber % 2 == 0 || levelNumber % 7 == 0)
            Globals.stadiumNumber = 0;
        else if (levelNumber % 5 == 0 || levelNumber % 9 == 0)
            Globals.stadiumNumber = 1;
        else
        {
            Globals.stadiumNumber = 2;
        }

        //leave first stadium for now
        //Globals.stadiumNumber = 2;
    }

    private void setLevel()
    {
        int rand_score = levelNumber % 6;
        if (levelNumber % 3 == 0)
            rand_score = 2;
        else if (levelNumber % 4 == 0)
            rand_score = 5;
        else if (levelNumber % 5 == 0)
            rand_score = 1;
        else if (levelNumber % 6 == 0)
            rand_score = 4;
        else if (levelNumber % 7 == 0)
            rand_score = 3;
        
        Globals.score1 = rand_score;
        Globals.score2 = rand_score;

        int relativeLevelNumber = levelNumber % 10;
        if (relativeLevelNumber >= 5)
        {
            Globals.score2++;
        }
        
        if (levelNumber < 5)
        {
            Globals.level = 1;
        }
        else if (levelNumber < 10)
        {
            Globals.level = 2;
        }
        else if (levelNumber < 20)
        {
            Globals.level = 3;
        }
        else if (levelNumber < 30)
        {
            Globals.level = 4;
        }
        else if (levelNumber < 40)
        {
            Globals.level = 5;
        }        
        else
        {

        }     
    }

    private void setTeamStrength()
    {
        int relativeLevelNumber = levelNumber  % 10;
        if (relativeLevelNumber == 0)
            relativeLevelNumber = 1;

        int teamASteangth = 85 - (relativeLevelNumber * 5);
        int teamBSteangth = 40 + (relativeLevelNumber * 4) + (Globals.level * 6);

        Globals.teamAGkStrength = Mathf.Clamp(teamASteangth, 40, 97);
        Globals.teamAAttackStrength = Mathf.Clamp(teamASteangth, 40, 97);
        Globals.teamAcumulativeStrength = Globals.teamAGkStrength + Globals.teamAAttackStrength;

        Globals.teamBGkStrength = Mathf.Clamp(teamBSteangth, 40, 97);
        Globals.teamBAttackStrength = Mathf.Clamp(teamBSteangth, 40, 97);
        Globals.teamBcumulativeStrength = Globals.teamBGkStrength + Globals.teamBAttackStrength;
    }


    private string[] getTeam(int levelNumber, int team_idx)
    {
        string[] leagueNames = { "NATIONALS", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "POLAND", "OTHERS" };
        string leagueName = leagueNames[levelNumber % leagueNames.Length];
        Teams teams = new Teams(leagueName);

        int teamRandomInt = levelNumber + 1;
        if (team_idx == 1)
            teamRandomInt = levelNumber + 8;

        if (teamRandomInt >= teams.getMaxTeams())
            teamRandomInt = levelNumber % teams.getMaxTeams();

        if (team_idx == 1)
        {
            Globals.teamBid = teamRandomInt;
            Globals.teamBleague = leagueName;
        } else
        {
            Globals.teamAid = teamRandomInt;
            Globals.teamAleague = leagueName;
        }

        Globals.leagueName = leagueName;

        return teams.getTeamByIndex(teamRandomInt);            
    }

    private void setTeams()
    {

        for (int i = 0; i < 2; i++)
        {  
            string[] randTeam = getTeam(levelNumber, i);

            if (i == 0)
            {
                Globals.teamAname = randTeam[0];
                //Globals.teamAid = teamRandomInt;
                Globals.stadiumColorTeamA = randTeam[5];
                Globals.playerADesc = randTeam[12].Split('|')[0];
            }
            else
            {              
                Globals.teamBname = randTeam[0];
                //Globals.teamBid = teamRandomInt;
                Globals.stadiumColorTeamB = randTeam[5];
                Globals.playerBDesc = randTeam[12].Split('|')[0];
            }

            //teams.swapElements(
            //    teamRandomInt, teams.getMaxActiveTeams() - 1);
        }    
    }

    private void setLevelSpecificSettings()
    {
        /*Traning hardcoded values*/
        //Globals.teamAcumulativeStrength = 160;
        //Globals.teamBcumulativeStrength = 140;

        //Globals.teamAGkStrength = 90;
        //Globals.teamBGkStrength = 70;

        //Globals.teamAAttackStrength = 70;
        //Globals.teamBAttackStrength = 70;

        Globals.matchTime = "60 SECONDS";

        Globals.maxTimeToShotStr = "7";
        //IT's in seconds
        Globals.levelModeTimeOffset = 30.0f + (levelNumber % 8);
    }

    private void setMainSettings()
    {
        Globals.playerPlayAway = false;
        Globals.isMultiplayer = false;
        Globals.isLevelMode = true;
        Globals.isTrainingActive = false;
        Globals.onlyTrainingActive = false;
        Globals.wonFinal = false;
        if (levelNumber > 5)
            Globals.PRO_MODE = true;

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

    public void onClickShowInfoCanvas()
    {
        infoCanvas.SetActive(true);
    }

    public void onClickCloseInfoCanvas()
    {
        infoCanvas.SetActive(false);
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
