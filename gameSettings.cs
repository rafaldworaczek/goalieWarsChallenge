using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuCustomNS;
using UnityEngine.UI;
﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GlobalsNS;
using graphicsCommonNS;
using TMPro;
using System;
using System.IO;
using LANGUAGE_NS;
using AudioManagerMultiNS;

public class gameSettings : MonoBehaviour
{
    public GameObject stadiumRewardAds;
    public Button stadiumChooseRewardAdsButton;
    public bool isMultiplayer = false;
    private int MAX_PLAYERS_CARDS = 2;
    //public NationalTeams nationalTeams;
    public Teams teamsA;
    public Teams teamsB;

    public MultiplayerMenu multiplayerMenuScript;

    private GraphicsCommon graphicsStandard;
    private int currATeamIdx = 0;
    private int currBTeamIdx = 10;
    /*This reference is updated dynamically using changeTeamReferences function */
    private TextMeshProUGUI teamAname;
    public TextMeshProUGUI teamBname;
    private TextMeshProUGUI leagueAName;
    public TextMeshProUGUI leagueBName;
    public TextMeshProUGUI time;
    public TextMeshProUGUI level;
    public TextMeshProUGUI graphicsText;
    public TextMeshProUGUI trainingMode;
    public TextMeshProUGUI maxTimeToShotText;
    public TextMeshProUGUI powersText;
    public TextMeshProUGUI commentatorText;

    public TextMeshProUGUI joystickSideText;
    public TextMeshProUGUI timeMain;
    public TextMeshProUGUI levelMain;
    public TextMeshProUGUI graphicsTextMain;
    public TextMeshProUGUI trainingModeMain;
    public TextMeshProUGUI joystickSideTextMain;
    public TextMeshProUGUI maxTimeToShotTextMain;
    public TextMeshProUGUI powersTextMain;
    public TextMeshProUGUI commentatorTextMain;

    private InternetConnection internetConnectivity;

    /*This reference is updated dynamically using changeTeamReferences function */
    private RawImage teamAflag;
    public TextMeshProUGUI[] teamNameMultiplayer;
    public RawImage[] multiplayerMenuTeamFlag;
    private bool updateMultiTexture = false;
    public GameObject teamBSpinnSer;

    public RawImage teamBflag;
    public RawImage mainFlag;
    public GameObject mainMenuCanvas;
    public GameObject trainingCanvas;
    public GameObject gameSettingsCanvas;
    public GameObject mainGameSettingsCanvas;
    public GameObject friendlyCanvas;
    public GameObject tournamentCanvas;
    public GameObject popUpNoCoins;
    public GameObject noInternetCanvas;
    public GameObject loadGameCanvas;
    public GameObject shopPromotionCanvas;
    public Text shopPromotionBuyButtonText;
    public GameObject yesNoMenuPanel;
    public GameObject chooseStadiumCanvas;
    public GameObject chooseStadiumCanvasIndoor;
    public GameObject chooseStadiumCanvasStreet;
    public GameObject chooseStadiumCanvasPaid;
    public Button yesNoMenuNoAnswer;
    public Button yesNoMenuYesAnswer;
    public TextMeshProUGUI yesNoMenuHeaderText;

    private float inernetCheckTime = 0f;
    private Image playerSkillsDefenseBar;
    private TextMeshProUGUI playerSkillsDefenseText;
    private Image playerSkillsAttackBar;
    private TextMeshProUGUI playerSkillsAttackText;
    private TextMeshProUGUI playerTeamALockedCoins;

    public Image cpuSkillsGkBar;
    public TextMeshProUGUI cpuSkillsGkText;
    public Image cpuSkillsAttackBar;
    public TextMeshProUGUI cpuSkillsAttackText;
    public TextMeshProUGUI cpuTeamBLockedCoins;
    private TextMeshProUGUI currCoinsText;

    private TextMeshProUGUI mainCurrentDiamondsText;
    private TextMeshProUGUI mainCurrentCoinsText;
    private TextMeshProUGUI mainNumOfCupsWonText;
    public GameObject playerBSkillsPanel;
    public Image multiplayerPlayerADefenseSkillsBar;
    public Image multiplayerPlayerAAttackSkillsBar;
    public TextMeshProUGUI multiplayerPlayerADefenseText;
    public TextMeshProUGUI multiplayerPlayerAAttackText;
    public TextMeshProUGUI multiplayerPlayerBDefenseText;
    public TextMeshProUGUI multiplayerPlayerBAttackText;
    public GameObject multiplayerPlayerAStar;
    public GameObject multiplayerPlayerBStar;

    public Image multiplayerPlayerBDefenseSkillsBar;
    public Image multiplayerPlayerBAttackSkillsBar;

    private string[] levels = { "KID", "EASY", "NORMAL", "HARD", "EXPERT" };
    //  private string[] gameTimes = { "30 SECONDS", "1 MINUTE", "2 MINUTES", "3 MINUTES", "4 MINUTES", "5 MINUTES" };
    private string[] gameTimes = { "30 SECONDS", "1 MINUTE", "2 MINUTES" };
    private string[] trainingModes = { "AUTOMATIC", "PROFESSIONAL" };
    private string[] graphics = { "VERY LOW", "LOW", "STANDARD", "HIGH", "VERY HIGH" };
    private string[] joystickSide = { "LEFT", "RIGHT" };

    private string[] maxTimeToShot = { "8 SECONDS", "10 SECONDS", "15 SECONDS", "20 SECONDS" };
    private string[] powers = { "YES", "NO" };
    private string[] commentator = { "YES", "NO" };


    private string[] leagueNames;
    private string[] nonCupNames = { "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "POLAND" };
    private string[] cupNames = { "CHAMP CUP", "WORLD CUP", "AMERICA CUP", "EURO CUP" };
    private string[] allLeaguesNames =
        {  "ENGLAND", "BRAZIL","GERMANY", "ITALY", "SPAIN", "POLAND", "CHAMP CUP", "NATIONALS" };

    private int levelsIdx = 1;
    private int netLostPackages = 0;
    private int gameTimesIdx = 1;

    private int maxTimeToShotIdx = 0;
    private int powersIdx = 0;
    private int commentatorIdx = 0;

    private int trainingModeIdx = 0;
    private int joystickSideIdx = 0;
    private int graphicsSettingsIdx = 2;
    private int leagueAIdx = 0;
    private int leagueBIdx = 2;

    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;

    private int teamAFlagPrevActive = 0;
    private float teamAFlagPrevLastTimeChanged = 0f;
    private int teamAFlagNextActive = 0;
    private float teamAFlagNextLastTimeChanged = 0f;
    private int teamBFlagPrevActive = 0;
    private float teamBFlagPrevLastTimeChanged = 0f;
    private int teamBFlagNextActive = 0;
    private float teamBFlagNextLastTimeChanged = 0f;

    private GameObject friendlyModelTeamA;
    private GameObject friendlyModelTeamAhair;

    private GameObject friendlyModelTeamB;
    private GameObject friendlyModelTeamBhair;

    private GameObject tournamentModelTeamA;
    private GameObject tournamentModelTeamAhair;

    private GameObject[] multiplayerModel;
    private GameObject[] multiplayerModelHair;


    private int gameUpdateLoops = 0;
    private int internetReqTime = 4;
    private int internetFailsNum = 8;

    public TextMeshProUGUI[] loadGameButtonText;
    public TextMeshProUGUI[] loadGameButtonPart2Text;
    public Button[] loadGameButton;
    public Button[] loadDeleteButton;
    public GameObject[] loadGameButtonGO;
    public RawImage[] loadGameFlagsCountryImg;
    public RawImage[] loadGameFlagsClubImg;

    private int gameLoadPageIdx = 0;
    private List<string> gameSavedList;
    private int gameSavesNumRows = 3;
    private string savedGamesFileName;

    public RawImage friendlyShopCoinDiamondIconImg;
    public TextMeshProUGUI friendlyShopCoinDiamondText;
    public RawImage friendlyCoinsDiamondMainImg;
    public TextMeshProUGUI friendlyCoinsDiamondMainText;
    public RawImage tournamentShopCoinDiamondIconImg;
    public TextMeshProUGUI tournamentShopCoinDiamondIconText;
    public RawImage tournamentCoinsDiamondMainImg;
    public TextMeshProUGUI tournamentCoinsDiamondMainText;
    public Shop shopScript;
    public GameObject lockStadiumPanel;
    private AudioManager audioManager;

    private GameObject[] playerCard;
    private TextMeshProUGUI[] playerCardName;
    private GameObject[] playerCardPlayerStarImg;
    private RawImage[] playerCardPlayerImg;
    private RawImage[] playerCardFlag;
    private Image[] playerEnergySkillsBar;
    private TextMeshProUGUI[] playerCardSkills;

    void Awake()
    {

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //print("Audio audioManager " + audioManager);

        if (isMultiplayer)
        {
            initPlayerCardReference();
            currATeamIdx = PlayerPrefs.GetInt("multiplayer_currTeamA", 0);
            leagueAIdx = PlayerPrefs.GetInt("multiplayer_leagueAIdx", 0);

            //print("currentTeamIdx " + currATeamIdx + "leagueAIdx "+ leagueAIdx);

            Globals.isFriendly = false;
            Globals.stadiumNumber = 0;
            playerBSkillsPanel.SetActive(false);
            //multiplayerPlayerAStar.SetActive(false);
            //multiplayerPlayerBStar.SetActive(false);

            //print("currATeamIdx PlayerPrefs. " + PlayerPrefs.HasKey("multiplayer_currTeamA"));


            //print("currATeamIdx " + currATeamIdx);

            multiplayerModel = new GameObject[2];
            multiplayerModelHair = new GameObject[2];
            //Globals.gameType = "FRIENDLY";
        }

        if (!isMultiplayer)
        {
            chooseStadiumCanvas.SetActive(false);
            chooseStadiumCanvasIndoor.SetActive(false);
            chooseStadiumCanvasStreet.SetActive(false);
        }

        Globals.isGameSettingActive = true;
        if (!isMultiplayer)
        {
            initReferences();
        }
        
        init();        
        init3dModels();

        if (isMultiplayer)
        {
            changeTeamReferences(false);
            onClickMultiplayerTeamSettingsSave();
        }
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            /*old phones have a issue with internet connection that doesn't work after game start*/
            int apiLevel =
                int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
            if (apiLevel < 23)
            {
                internetFailsNum = 5;
                internetReqTime = 5;
            }
            else if (apiLevel <= 20)
            {
                internetFailsNum = 7;
                internetReqTime = 8;
            }

            //print("APIANDROIDVERSION " + apiLevel);
        }

        yesNoMenuPanel.SetActive(false);
        noInternetCanvas.SetActive(false);
        //internetConnectivity =
        //    GameObject.Find("checkInternetConnection").GetComponent<InternetConnection>();
        //internetConnectivity.checkInternetConnection(null, internetReqTime);

        if (!isMultiplayer)
        {
            admobGameObject = GameObject.Find("admobAdsGameObject");
            admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();
        }
        
        mainGameSettingsCanvas.SetActive(false);
        setupAllSetttingsToDefault();
        if (!isMultiplayer)
            disable3DModels();

        if (!Globals.is_app_paid)
            showPromotion();

        popUpNoCoins.SetActive(false);


        //print("Globals.teamAAttackStrength Start 1 isMultiplayer 1 " + isMultiplayer);
        if (isMultiplayer)
        {
            ///print("#DBGTEST " + isMultiplayer);
            //leagueAIdx = 0;
            //currATeamIdx = 0;
            leagueNames = allLeaguesNames;
            teamsA.setLeague(leagueNames[leagueAIdx]);

            //print("#DBGTEST2 " + isMultiplayer);
//            print("Globals.teamAAttackStrength Start 1 isMultiplayer 2 " + isMultiplayer);

            saveSelectedTeamsToGlobals(true);

            //print("#DBGTEST3 " + isMultiplayer);

            setupTeamDefaults();

            //print("teamDesc set in gameSettings after team defaults " + Globals.teamAname
            //             + " Globals.teamAGkStrength " + Globals.teamAGkStrength
            //             + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);
        }
    }

    void Update()
    {
        ///print("UPDATE #### texture!!! ## teamBID " + Globals.teamBid + " isMultiplayer " + isMultiplayer
        //    + " updateMultiTexture " + updateMultiTexture);

        if (isMultiplayer && 
           (Globals.teamBid != -1) &&
            !updateMultiTexture)
        {

            print("Globals.teamBid " + Globals.teamBid);
            updateMultiTexture = true;
            audioManager.PlayNoCheck("elementAppear");
            saveSelectedTeamsToGlobals(true);
            setupTeamDefaults();

            ///print("teamDesc set in gameSettings energy " + Globals.teamAname
            //              + " Globals.teamAGkStrength " + Globals.teamAGkStrength
            //              + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);

            if (Globals.isPlayerCardLeague(Globals.teamAleague))
                teamManagement.updatePlayersEnergy(Globals.multiplayerSaveName, Globals.playerADesc);

            //print("teamDesc set in gameSettings energy " + Globals.teamAname
            //            + " Globals.teamAGkStrength " + Globals.teamAGkStrength
            //            + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);

            //print("UPDATE #### texture!!! ## ");
        }

        /*inernetCheckTime += Time.deltaTime;
        if (inernetCheckTime > ((float)internetReqTime + 0.5f))
        {
            int netStatus =
                internetConnectivity.getInternetConnectionStatus();

 

            if (netStatus != 1)
            {
                netLostPackages++;
                if (netLostPackages == internetFailsNum)
                {
                    noInternetCanvas.SetActive(true);
                    netLostPackages = 0;
                }
            }
            else
            {
                netLostPackages = 0;
                noInternetCanvas.SetActive(false);
            }

            inernetCheckTime = 0f;
            if (internetReqTime == 2)
                internetReqTime = 3;
            else
            {
                internetReqTime = 2;
            }

            internetConnectivity.checkInternetConnection(null, internetReqTime);
        }
        */
        updateFlagButtons();
    }

    private void updateFlagButtons()
    {
        float minTime = 0.15f;

        if (teamAFlagPrevActive != 0)
        {
            //print("#DBGTIMEDIFF PREV " + (Time.time - teamAFlagPrevLastTimeChanged));
            if ((Time.time - teamAFlagPrevLastTimeChanged) > minTime)
            {
                teamAFlagPrevLastTimeChanged = Time.time;
                teamAFlagPrev();
            }

            if (teamAFlagPrevActive == -1)
                teamAFlagPrevActive = 0;
        }

        if (teamAFlagNextActive != 0)
        {
            //print("#DBGTIMEDIFF NEXT " + (Time.time - teamAFlagNextLastTimeChanged));
            if ((Time.time - teamAFlagNextLastTimeChanged) > minTime)
            {
                teamAFlagNextLastTimeChanged = Time.time;
                teamAFlagNext();
            }

            if (teamAFlagNextActive == -1)
                teamAFlagNextActive = 0;
        }

        if (teamBFlagPrevActive != 0)
        {
            if ((Time.time - teamBFlagPrevLastTimeChanged) > minTime)
            {
                teamBFlagPrevLastTimeChanged = Time.time;
                teamBFlagPrev();
            }

            if (teamBFlagPrevActive == -1)
                teamBFlagPrevActive = 0;
        }

        if (teamBFlagNextActive != 0)
        {
            if ((Time.time - teamBFlagNextLastTimeChanged) > minTime)
            {
                teamBFlagNextLastTimeChanged = Time.time;
                teamBFlagNext();
            }

            if (teamBFlagNextActive == -1)
                teamBFlagNextActive = 0;
        }
    }

    private void clearFlagButtonsVars(bool teamA, bool teamB)
    {
        //print("#DBGCLERFLAG " + teamA + " teamB " + teamB);

        if (teamA)
        {
            teamAFlagPrevActive = 0;
            teamAFlagPrevLastTimeChanged = 0f;
            teamAFlagNextActive = 0;
            teamAFlagNextLastTimeChanged = 0f;
        }

        if (teamB)
        {
            teamBFlagPrevActive = 0;
            teamBFlagPrevLastTimeChanged = 0f;

            teamBFlagNextActive = 0;
            teamBFlagNextLastTimeChanged = 0f;
        }
    }

    private void showBanner()
    {
        if (admobGameObject != null &&
            admobAdsScript != null)
        {
            admobAdsScript.showBannerAd();
        }
    }

    private void init()
    {
        graphicsStandard = new GraphicsCommon();
        //setupGameTypeDefaults();
        //set default. it will be change later
        if (!isMultiplayer)
        {
            leagueNames = nonCupNames;
            leagueAIdx = 0;
            currATeamIdx = 1;
        }
        else
        {
            leagueNames = allLeaguesNames;
        }

        leagueBIdx = 5;
        currBTeamIdx = 3;
         
        //print("#DBGINITA LEAGUE NAME " + leagueNames[leagueAIdx]);
        //print("#DBGINITB LEAGUE NAME " + leagueNames[leagueBIdx]);

        teamsA = new Teams(leagueNames[leagueAIdx]);


        //for (int i = 0; i < teamsA.getMaxTeams(); i++)
        //{
        //    Debug.Log("#DBG123 teamName " + teamsA.getTeamByIndex(i)[0]);
        //}


        teamsB = new Teams(leagueNames[leagueBIdx]);

        if (!isMultiplayer)
        {
            initGameSaves();
            updateTreasuresText();
        }
    }

    private void initReferences()
    {
        mainCurrentDiamondsText =
            GameObject.Find("mainCurrentDiamondsText").GetComponent<TextMeshProUGUI>();
        mainCurrentCoinsText =
            GameObject.Find("mainCurrentCoinsText").GetComponent<TextMeshProUGUI>();
        mainNumOfCupsWonText = 
            GameObject.Find("mainNumOfCupsWonText").GetComponent<TextMeshProUGUI>();
    }
  
    public void updateTreasuresText()
    {
        //print("#UPDATETREASURE " + Globals.diamonds);
        if (isMultiplayer)
            return;
               
        mainCurrentDiamondsText.text = 
            Globals.diamonds.ToString();
        mainCurrentCoinsText.text =
            Globals.coins.ToString();
        mainNumOfCupsWonText.text =
            Globals.cupsWon.ToString();
    }

    public void initGameSaves()
    {
        if (Globals.gameType.Equals("CUP"))
            savedGamesFileName = "savedGames_CUP";
        else
        {
            savedGamesFileName = "savedGames_LEAGUES";
        }

        //print("DBGLOADSAVED fileName " + savedGamesFileName +
        //    " PlayerPrefs.HasKey(savedGamesFileName)" + PlayerPrefs.HasKey(savedGamesFileName));

        if (gameSavedList == null)
            gameSavedList = new List<string>();
        else
            gameSavedList.Clear();

        //string savedGames =
        //           PlayerPrefs.GetString(savedGamesFileName);
        //print("DBGSAVES SAVEDGAMES " + savedGames);
        if (PlayerPrefs.HasKey(savedGamesFileName))           
        {
            string savedGames =
                   PlayerPrefs.GetString(savedGamesFileName);
            string[] gameSaved = savedGames.Split('|');
            //print("DBGLOADSAVED savedGames gameSettings " + savedGames + " LEN " + gameSaved.Length);

            //for (int i = 0; i < gameSaved.Length; i++) {
            for (int i = gameSaved.Length - 1; i >= 0; i--)
            {
                gameSavedList.Add(gameSaved[i]);
                //print("#DBGSAVES CURRENT SAVES " + gameSaved[i]);
            }

            printSavedGames(0, gameSavedList);
        }
        else
        {
            for (int i = 0; i < gameSavesNumRows; i++)
            {
                loadGameButtonGO[i].SetActive(false);
            }
        }

        for (int i = 0; i < gameSavesNumRows; i++)
        {
            int tmpIdx = i;
            //print("DBGLOADSAVED123 add listener " + gameSavesNumRows + " TMPIDX " + tmpIdx);

            loadGameButton[i].onClick.RemoveAllListeners();
            loadDeleteButton[i].onClick.RemoveAllListeners();

            loadGameButton[i].onClick.AddListener(
                delegate { loadFromSaveOnClick(tmpIdx, savedGamesFileName); });
            loadDeleteButton[i].onClick.AddListener(
                delegate { deleteGameSave(tmpIdx, savedGamesFileName); });
        }
    }

    private void setupDefaultSettings()
    {
        graphicsText.text = Languages.getTranslate(graphics[graphicsSettingsIdx]);
        graphicsTextMain.text = Languages.getTranslate(graphics[graphicsSettingsIdx]);
        time.text = Languages.getTranslate(gameTimes[gameTimesIdx]);
        timeMain.text = Languages.getTranslate(gameTimes[gameTimesIdx]);
        trainingMode.text = Languages.getTranslate(trainingModes[trainingModeIdx]);
        trainingModeMain.text = Languages.getTranslate(trainingModes[trainingModeIdx]);
        joystickSideText.text = Languages.getTranslate(joystickSide[joystickSideIdx]);
        joystickSideTextMain.text = Languages.getTranslate(joystickSide[joystickSideIdx]);

        level.text = Languages.getTranslate(levels[levelsIdx]);
        levelMain.text = Languages.getTranslate(levels[levelsIdx]);


        maxTimeToShotText.text = Languages.getTranslate(maxTimeToShot[maxTimeToShotIdx]);
        maxTimeToShotTextMain.text = Languages.getTranslate(maxTimeToShot[maxTimeToShotIdx]);

        powersText.text = Languages.getTranslate(powers[powersIdx]);
        powersTextMain.text = Languages.getTranslate(powers[powersIdx]);
    
        commentatorText.text = Languages.getTranslate(commentator[commentatorIdx]);
        commentatorTextMain.text = Languages.getTranslate(commentator[commentatorIdx]);
    }

    public void setupTeamDefaults()
    {
        updateTreasuresText();

        if (teamAname == null)
            return;

        string[] team = teamsA.getTeamByIndex(currATeamIdx);
        string playerSkinHairDescA =
            teamsA.getPlayerDescByIndex(currATeamIdx, 0);

        if (PlayerPrefs.HasKey(team[0] + "_lastSelectedPlayer"))
        {
            Globals.playerADesc =
                    PlayerPrefs.GetString(team[0] + "_lastSelectedPlayer");
            playerSkinHairDescA = Globals.playerADesc;
        }   

        string playerSkinHairDescB = 
            teamsB.getPlayerDescByIndex(currBTeamIdx, 0);

        setTeamName(teamAname, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));

        if (isMultiplayer)
            setTeamName(teamNameMultiplayer[0], team[0], int.Parse(team[4]));

        //string fullTeamPath =
        //       Globals.getFlagPath(leagueNames[leagueAIdx], team[0]);

        //string fullTeamPath =
        //        Globals.getFlagPath(leagueNames[leagueAIdx], team[0]);

        string fullTeamPath =
                graphicsStandard.getFlagPath(team[0]);
        
        ///print("#DBG34 TEAM PATH default " + fullTeamPath + " TEAMNAME " + team[0]);

        setFlagImage(teamAflag, fullTeamPath, int.Parse(team[4]));

        if (isMultiplayer)
        {
            setFlagImage(multiplayerMenuTeamFlag[0], fullTeamPath, 0);

            //Vector2 teamSkills = 
             //   Globals.getTeamSkillsAverage(team, Globals.teamAleague);

            //print("Globals.teamAGkStrength " + Globals.teamAGkStrength + " Globals.playerADesc " +
            //      Globals.playerADesc
            //     + " team[12] " + team[12]);

    
            //{
            //    multiplayerPlayerAStar.SetActive(true);
                //multiplayerMenuScript.setBottomButtonInteractable(0, true, 1f);
            //}
            //else
           // {
           //     multiplayerPlayerAStar.SetActive(false);
                //multiplayerMenuScript.setBottomButtonInteractable(0, false, 0.33f);
           // }
            if (updateMultiTexture)
            {
                if (!Globals.teamBCustomize)
                {
                    //print("#DBBoutput " + Globals.teamBname);
                    fullTeamPath = graphicsStandard.getFlagPath(Globals.teamBname);
                    setFlagImage(multiplayerMenuTeamFlag[1], fullTeamPath, 0);
                }
                else
                {
                    multiplayerMenuTeamFlag[1].texture = Resources.Load<Texture2D>(
                            "others/logoFile");
                }
         
                teamNameMultiplayer[1].text = (Globals.teamBname.ToUpper());            

                playerBSkillsPanel.SetActive(true);
                multiplayerPlayerBDefenseSkillsBar.fillAmount = Globals.teamBGkStrength / 100.0f;
                multiplayerPlayerBDefenseText.text =
                    Languages.getTranslate("Defense: " + Globals.teamBGkStrength.ToString());

                multiplayerPlayerBAttackSkillsBar.fillAmount = Globals.teamBAttackStrength / 100.0f;
                multiplayerPlayerBAttackText.text =
                    Languages.getTranslate("Attack: " + Globals.teamBAttackStrength.ToString());
            }
        }

        //print("#DBGINITA LEAGUENAMES setup Defaults "
        //    + leagueNames[leagueAIdx] + " : " + leagueNames + " team[0] " + team[0] + 
        //    " fullTeamPath " + fullTeamPath);

        Vector2 skills =
            Globals.getTeamSkillsAverage(team, leagueNames[leagueAIdx]);

        //playerSkillsDefenseBar.fillAmount = float.Parse(team[1]) / 100.0f;
        //playerSkillsDefenseText.text = "Defense: " + team[1];
        //playerSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        //playerSkillsAttackText.text = "Attack: " + team[2];

        playerSkillsDefenseBar.fillAmount = skills.x / 100.0f;
        playerSkillsDefenseText.text = 
            Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
        playerSkillsAttackBar.fillAmount = skills.y / 100.0f;
        playerSkillsAttackText.text = 
            Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

        print("Skillsplayer " + skills);

        if (isMultiplayer) {
            multiplayerPlayerADefenseSkillsBar.fillAmount = skills.x / 100.0f;
            multiplayerPlayerADefenseText.text =
                Languages.getTranslate("Defense: " + ((int)skills.x).ToString());

            multiplayerPlayerAAttackSkillsBar.fillAmount = skills.y / 100.0f;
            multiplayerPlayerAAttackText.text =
                Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

        }

        //if (Globals.isPlayerCardStar(Globals.teamAGkStrength, Globals.teamAAttackStrength))

        /*set default cpu team*/
        if (!isMultiplayer)
        {
            team = teamsB.getTeamByIndex(currBTeamIdx);
            setTeamName(teamBname, team[0], int.Parse(team[4]));
            //setTeamLocked(cpuTeamBLockedCoins, team[0], int.Parse(team[4]));
            setTeamLocked(cpuTeamBLockedCoins, team[0], 0);

            //fullTeamPath =
            //    Globals.getFlagPath(leagueNames[leagueBIdx], team[0]);
            fullTeamPath =
                    graphicsStandard.getFlagPath(team[0]);

            //setFlagImage(teamBflag, fullTeamPath, int.Parse(team[4]));
            setFlagImage(teamBflag, fullTeamPath, 0);

            skills =
               Globals.getTeamSkillsAverage(team, leagueNames[leagueBIdx]);

            //cpuSkillsGkBar.fillAmount = float.Parse(team[1]) / 100.0f;
            //cpuSkillsGkText.text = "Defense: " + team[1];
            //cpuSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
            //cpuSkillsAttackText.text = "Attack: " + team[2];

            cpuSkillsGkBar.fillAmount = skills.x / 100.0f;
            cpuSkillsGkText.text =
                Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
            cpuSkillsAttackBar.fillAmount = skills.y / 100.0f;
            cpuSkillsAttackText.text =
                Languages.getTranslate("Attack: " + ((int)skills.y).ToString());
        }

        if (!isMultiplayer)
        {
            if (Globals.isFriendly)
            {
                graphicsStandard.setPlayerTextures(
                    friendlyModelTeamA,
                    friendlyModelTeamAhair,
                    currATeamIdx,
                    leagueNames[leagueAIdx],
                    playerSkinHairDescA,
                    false,
                    false,
                    teamsA);

                graphicsStandard.setPlayerTextures(
                  friendlyModelTeamB,
                  friendlyModelTeamBhair,
                  currBTeamIdx,
                  leagueNames[leagueBIdx],
                  playerSkinHairDescB,
                  false,
                  false,
                  teamsB);
            }
            else
            {
                graphicsStandard.setPlayerTextures(
                    tournamentModelTeamA,
                    tournamentModelTeamAhair,
                    currATeamIdx,
                    leagueNames[leagueAIdx],
                    playerSkinHairDescA,
                    false,
                    false,
                    teamsA);
            }
        }

        if (isMultiplayer)
        {
            /*if (PlayerPrefs.HasKey(playerTeamName + "_lastSelectedPlayer") {
                graphicsStandard.setPlayerTextures(
                    multiplayerModel[1],
                    multiplayerModelHair[1],
                    Globals.playerADesc,
                    Globals.teamBDesc);
            }*/

            /*print("set model standard " + leagueNames[leagueAIdx]);

            Debug.Log(" playerCardFlag[0] " + playerCardFlag[0]
                 + " playerCardPlayerImg " + playerCardPlayerImg[0]
                 + " playerCardPlayerStarImg[0] " + playerCardPlayerStarImg[0]
                 + " playerCardSkills[0] " + playerCardSkills[0]
                 + " playerCardName[0] " + playerCardName[0]);*/

            Color tmpColor = multiplayerMenuTeamFlag[0].color;
            if (Globals.isPlayerCardLeague(leagueNames[leagueAIdx]))
            {
                teamManagement.fillPlayerCard(
                                              playerCardFlag[0],
                                              playerCardPlayerImg[0],
                                              playerCardPlayerStarImg[0],
                                              playerCardSkills[0],
                                              playerCardName[0],
                                              playerEnergySkillsBar[0],
                                              Globals.playerADesc,
                                              Globals.multiplayerSaveName,
                                              Globals.teamAGkStrength,
                                              Globals.teamAAttackStrength,
                                              Globals.energyPlayerA);
                print("Globals.energyPlayerA " + Globals.energyPlayerA);
                tmpColor.a = 0f;
                multiplayerMenuTeamFlag[0].color = tmpColor;
                playerCard[0].SetActive(true);
            }
            else
            {
                tmpColor.a = 1f;
                multiplayerMenuTeamFlag[0].color = tmpColor;
                playerCard[0].SetActive(false);
            }

            graphicsStandard.setPlayerTextures(
               multiplayerModel[0],
               multiplayerModelHair[0],
               currATeamIdx,
               leagueNames[leagueAIdx],
               playerSkinHairDescA,
               false,
               false,
               teamsA);

            graphicsStandard.setPlayerTextures(
                 tournamentModelTeamA,
                 tournamentModelTeamAhair,
                 currATeamIdx,
                 leagueNames[leagueAIdx],
                 playerSkinHairDescA,
                 false,
                 false,
                 teamsA);


            if (updateMultiTexture == true) {
                teamsB = new Teams(Globals.teamBleague);
                playerSkinHairDescB = Globals.playerBDesc;

                //print("#DBGGlobals.teamBleague after update " + Globals.teamBleague);
                //if (Globals.teamBCustomize)
               // {
                    graphicsStandard.setPlayerTextures(
                       multiplayerModel[1],
                       multiplayerModelHair[1],
                       Globals.playerBDesc,
                       Globals.teamBDesc);


                playerCard[MAX_PLAYERS_CARDS - 1].SetActive(true);


                if (Globals.isPlayerCardLeague(Globals.teamBleague))
                {
                    teamManagement.fillPlayerCard(
                                      playerCardFlag[1],
                                      playerCardPlayerImg[1],
                                      playerCardPlayerStarImg[1],
                                      playerCardSkills[1],
                                      playerCardName[1],
                                      playerEnergySkillsBar[1],
                                      Globals.playerBDesc,
                                      Globals.multiplayerSaveName,
                                      Globals.teamBGkStrength,
                                      Globals.teamBAttackStrength,
                                      Globals.energyPlayerB);
                    tmpColor.a = 0f;
                    multiplayerMenuTeamFlag[1].color = tmpColor;
                    playerCard[1].SetActive(true);
                }
                else
                {
                    tmpColor.a = 1f;
                    multiplayerMenuTeamFlag[1].color = tmpColor;
                    playerCard[1].SetActive(false);
                }
                /* } else {
                     graphicsStandard.setPlayerTextures(
                       multiplayerModel[1],
                       multiplayerModelHair[1],
                       Globals.teamBid,
                       Globals.teamBleague,
                       playerSkinHairDescB,
                       false,
                       false,
                       teamsB);
                 }*/
            }            
        }

        leagueAName.text = leagueNames[leagueAIdx];
        if (!isMultiplayer)
            leagueBName.text = leagueNames[leagueBIdx];

        Globals.leagueName = leagueNames[leagueAIdx];
        Globals.shopTeamIdx = currATeamIdx;

        if (!isMultiplayer)
            updateShopOffer(leagueNames[leagueAIdx]);     
    }

    public void setupGameTypeDefaults()
    {
        leagueAIdx = 0;
        leagueBIdx = 0;
        currATeamIdx = 1;
        currBTeamIdx = 10;

        //print("DBGTOURNAMENT23 GLOBAL GAMETYEP " + Globals.gameType);
        if (Globals.gameType.Equals("FRIENDLY"))
        {
            leagueNames = allLeaguesNames;
            if (Globals.PITCHTYPE.Equals("GRASS"))
            {
                leagueAIdx = 4;
                leagueBIdx = 4;
                currATeamIdx = 1;
                currBTeamIdx = 3;
            }
        } else if (Globals.gameType.Equals("CUP"))
        {
                //Array.Clear(leagueNames, 0, leagueNames.Length);
                //Array.Resize<string>(ref leagueNames, cupNames.Length);
                leagueNames = cupNames;

                //print("DBGTOURNAMENT23 LEAGUESNAMES CUP " + leagueNames + " size " + leagueNames.Length);
         } else {
            //Array.Clear(leagueNames, 0, leagueNames.Length);
            //Array.Resize<string>(ref leagueNames, nonCupNames.Length);
             
                leagueNames = nonCupNames;
                //print("DBGTOURNAMENT23 LEAGUESNAMES NONCUP " + leagueNames + " size " + leagueNames.Length);            
         }
        
          teamsA.setLeague(leagueNames[leagueAIdx]);
          teamsB.setLeague(leagueNames[leagueBIdx]);
    }

    private void changeTeamReferences(bool isFriendly)
    {
        if (!isFriendly)
        {
            teamAname = GameObject.Find("tournamentTeamAname").GetComponent<TextMeshProUGUI>();
            teamAflag = GameObject.Find("tournamentTeamAflag").GetComponent<RawImage>();
            leagueAName = GameObject.Find("tournamentLeagueAnameText").GetComponent<TextMeshProUGUI>();
            playerSkillsDefenseBar = GameObject.Find("tournamentPlayerDefenseSkillsBar").GetComponent<Image>();
            playerSkillsDefenseText = GameObject.Find("tournamentPlayerSkillsDefenseText").GetComponent<TextMeshProUGUI>();
            playerSkillsAttackBar = GameObject.Find("tournametPlayerAttackSkillsBar").GetComponent<Image>();
            playerSkillsAttackText = GameObject.Find("tournamentPlayerSkillsAttackText").GetComponent<TextMeshProUGUI>();
            playerTeamALockedCoins = GameObject.Find("tournamentTeamALockedCoins").GetComponent<TextMeshProUGUI>();
            //print("LEAGUENAME " + leagueAName);
        }
        else
        {
            teamAname = GameObject.Find("teamAname").GetComponent<TextMeshProUGUI>();
            teamAflag = GameObject.Find("teamAflag").GetComponent<RawImage>();
            leagueAName = GameObject.Find("friendlyLeagueAnameText").GetComponent<TextMeshProUGUI>();
            playerSkillsDefenseBar = GameObject.Find("playerDefenseSkillsBar").GetComponent<Image>();
            playerSkillsDefenseText = GameObject.Find("playerSkillsDefenseText").GetComponent<TextMeshProUGUI>();
            playerSkillsAttackBar = GameObject.Find("playerAttackSkillsBar").GetComponent<Image>();
            playerSkillsAttackText = GameObject.Find("playerSkillsAttackText").GetComponent<TextMeshProUGUI>();
            playerTeamALockedCoins = GameObject.Find("playerTeamALockedCoins").GetComponent<TextMeshProUGUI>();
        }
    }

    public void teamAFlagPrevPointerDown()
    {
        teamAFlagPrevActive = 1;
    }

    public void teamAFlagPrevPointerUp()
    {
        teamAFlagPrevActive = -1;
    }

    public void teamAFlagNextPointerDown()
    {
        teamAFlagNextActive = 1;
    }

    public void teamAFlagNextPointerUp()
    {
        teamAFlagNextActive = -1;
    }

    public void teamBFlagPrevPointerDown()
    {
        teamBFlagPrevActive = 1;
    }

    public void teamBFlagPrevPointerUp()
    {
        teamBFlagPrevActive = -1;
    }

    public void teamBFlagNextPointerDown()
    {
        teamBFlagNextActive = 1;
    }

    public void teamBFlagNextPointerUp()
    {
        teamBFlagNextActive = -1;
    }

    public void teamAFlagPrev()
    {
        popUpNoCoins.SetActive(false);

        if (currATeamIdx > 0)
        {
            currATeamIdx--;
        }
        else
        {
            if (currATeamIdx == 0)
            {
                //currATeamIdx = nationalTeams.getMaxTeams() - 1;
                currATeamIdx = teamsA.getMaxTeams() - 1;
            }
        }

        //string[] team = nationalTeams.getTeamByIndex(currATeamIdx);
        string[] team = teamsA.getTeamByIndex(currATeamIdx);

        setTeamName(teamAname, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));

        //string fullTeamPath =
        //    Globals.getFlagPath(leagueNames[leagueAIdx], team[0]);
        string fullTeamPath = 
            graphicsStandard.getFlagPath(team[0]);

        //pint("#DBG34 TEAM PATH " + fullTeamPath + " TEAMNAME " + team[0]);

        setFlagImage(teamAflag, fullTeamPath, int.Parse(team[4]));

        string playerSkinHairDescA =
            teamsA.getPlayerDescByIndex(currATeamIdx, 0);

        if (Globals.isFriendly)
        {
            graphicsStandard.setPlayerTextures(
                friendlyModelTeamA, 
                friendlyModelTeamAhair, 
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
        }
        else
        {
            graphicsStandard.setPlayerTextures(
                tournamentModelTeamA, 
                tournamentModelTeamAhair, 
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
        }

        Vector2 skills =
            Globals.getTeamSkillsAverage(team, leagueNames[leagueAIdx]);

        playerSkillsDefenseBar.fillAmount = skills.x / 100.0f;
        playerSkillsDefenseText.text = 
            Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
        playerSkillsAttackBar.fillAmount = skills.y / 100.0f;
        playerSkillsAttackText.text =
            Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

        Globals.shopTeamIdx = currATeamIdx;

        //playerSkillsDefenseBar.fillAmount = float.Parse(team[1]) / 100.0f;
        //playerSkillsDefenseText.text = "Defense: " + team[1];
        //playerSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        //playerSkillsAttackText.text = "Attack: " + team[2];
    }

    public void teamAFlagNext()
    {

        //print("#DBG team flag next ");
        popUpNoCoins.SetActive(false);

        //if ((currATeamIdx + 1) < nationalTeams.getMaxTeams())
        if ((currATeamIdx + 1) < teamsA.getMaxTeams())
        {
            currATeamIdx++;
        }
        else
        {
            //if (currATeamIdx == (nationalTeams.getMaxTeams() - 1))
            if (currATeamIdx == (teamsA.getMaxTeams() - 1))
            {
                currATeamIdx = 0;
            }
        }




        //for (int i = 0; i < teamsA.getMaxTeams(); i++)
        //{
        //    Debug.Log("#DBG123FLAG teamName " + teamsA.getTeamByIndex(i)[0]);
        //}



        //string[] team = nationalTeams.getTeamByIndex(currATeamIdx);
        string[] team = teamsA.getTeamByIndex(currATeamIdx);
        setTeamName(teamAname, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));

        //print("#DBG team flag next 1 ");

        //string fullTeamPath =
        //    Globals.getFlagPath(leagueNames[leagueAIdx], team[0]);

        string fullTeamPath =
                graphicsStandard.getFlagPath(team[0]);

        //print("#DBG34 TEAM PATH " + fullTeamPath + " TEAMNAME " + team[0]);

        setFlagImage(teamAflag, fullTeamPath, int.Parse(team[4]));

        string playerSkinHairDescA =
            teamsA.getPlayerDescByIndex(currATeamIdx, 0);
        //print("#DBG team flag next 2");


        if (Globals.isFriendly)
        {
            graphicsStandard.setPlayerTextures(
                friendlyModelTeamA, 
                friendlyModelTeamAhair, 
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
        }
        else
        {
            graphicsStandard.setPlayerTextures(
                tournamentModelTeamA, 
                tournamentModelTeamAhair, 
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
        }

        //print("#DBG team flag next 3");


        Vector2 skills =
            Globals.getTeamSkillsAverage(team, leagueNames[leagueAIdx]);

        playerSkillsDefenseBar.fillAmount = skills.x / 100.0f;

        playerSkillsDefenseText.text = 
            Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
        playerSkillsAttackBar.fillAmount = skills.y / 100.0f;
        playerSkillsAttackText.text = 
            Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

        //playerSkillsDefenseBar.fillAmount = float.Parse(team[1]) / 100.0f;
        //playerSkillsDefenseText.text = "Defense: " + team[1];
        //playerSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        //playerSkillsAttackText.text = "Attack: " + team[2];
        //print("#DBG team flag next 4");

        Globals.shopTeamIdx = currATeamIdx;

        //print("#DBG team flag next END ");

    }

    public void teamBFlagPrev()
    {
        popUpNoCoins.SetActive(false);

        if (currBTeamIdx > 0)
        {
            currBTeamIdx--;
        }
        else
        {
            if (currBTeamIdx == 0)
            {
                //currBTeamIdx = nationalTeams.getMaxTeams() - 1;
                currBTeamIdx = teamsB.getMaxTeams() - 1;
            }
        }

        //string[] team = nationalTeams.getTeamByIndex(currBTeamIdx);
        string[] team = teamsB.getTeamByIndex(currBTeamIdx);
        setTeamName(teamBname, team[0], int.Parse(team[4]));
        //setTeamLocked(cpuTeamBLockedCoins, team[0], int.Parse(team[4]));
        /*we unblock team be just by given 0 coinsNeeded*/
        setTeamLocked(cpuTeamBLockedCoins, team[0], 0);

        //string fullTeamPath =
        //       Globals.getFlagPath(leagueNames[leagueBIdx], team[0]);

        string fullTeamPath =
                graphicsStandard.getFlagPath(team[0]);

        //print("#DBG34 TEAM PATH " + fullTeamPath + " TEAMNAME " + team[0]);

        setFlagImage(teamBflag, fullTeamPath, 0);
        string playerSkinHairDescA =
            teamsB.getPlayerDescByIndex(currBTeamIdx, 0);

        graphicsStandard.setPlayerTextures(
            friendlyModelTeamB, 
            friendlyModelTeamBhair, 
            currBTeamIdx,
            leagueNames[leagueBIdx],
            playerSkinHairDescA,
            false,
            false,
            teamsB);

        //cpuSkillsGkBar.fillAmount = float.Parse(team[1]) / 100.0f;
        //cpuSkillsGkText.text = "Defense: " + team[1];
        //cpuSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        //cpuSkillsAttackText.text = "Attack: " + team[2];

        Vector2 skills =
            Globals.getTeamSkillsAverage(team, leagueNames[leagueBIdx]);

        cpuSkillsGkBar.fillAmount = skills.x / 100.0f;
        cpuSkillsGkText.text = 
            Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
        cpuSkillsAttackBar.fillAmount = skills.y / 100.0f;
        cpuSkillsAttackText.text = 
            Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

    }

    public void teamBFlagNext()
    {
        popUpNoCoins.SetActive(false);

        //if ((currBTeamIdx + 1) < nationalTeams.getMaxTeams())
        if ((currBTeamIdx + 1) < teamsB.getMaxTeams())
        {
            currBTeamIdx++;
        }
        else
        {
            //if (currBTeamIdx == (nationalTeams.getMaxTeams() - 1))
            if (currBTeamIdx == (teamsB.getMaxTeams() - 1))
            {
                currBTeamIdx = 0;
            }
        }

        //string[] team = nationalTeams.getTeamByIndex(currBTeamIdx);
        string[] team = teamsB.getTeamByIndex(currBTeamIdx);

        setTeamName(teamBname, team[0], int.Parse(team[4]));
        //setTeamLocked(cpuTeamBLockedCoins, team[0], int.Parse(team[4]));
        /*we unblock team be just by given 0 coinsNeeded*/
        setTeamLocked(cpuTeamBLockedCoins, team[0], 0);
        //string fullTeamPath =
        //       Globals.getFlagPath(leagueNames[leagueBIdx], team[0]);

        string fullTeamPath =
                graphicsStandard.getFlagPath(team[0]);

        //print("#DBG34 TEAM PATH " + fullTeamPath + " TEAMNAME " + team[0]);

        setFlagImage(teamBflag, fullTeamPath, 0);
        string playerSkinHairDescB =
            teamsB.getPlayerDescByIndex(currBTeamIdx, 0);

        graphicsStandard.setPlayerTextures(
                friendlyModelTeamB, 
                friendlyModelTeamBhair, 
                currBTeamIdx,
                leagueNames[leagueBIdx],
                playerSkinHairDescB,
                false,
                false,
                teamsB);

        Vector2 skills =
            Globals.getTeamSkillsAverage(team, leagueNames[leagueBIdx]);

        cpuSkillsGkBar.fillAmount = skills.x / 100.0f;
        cpuSkillsGkText.text = 
            Languages.getTranslate("Defense: " + ((int)skills.x).ToString());
        cpuSkillsAttackBar.fillAmount = skills.y / 100.0f;
        cpuSkillsAttackText.text = 
            Languages.getTranslate("Attack: " + ((int)skills.y).ToString());

        //cpuSkillsGkBar.fillAmount = float.Parse(team[1]) / 100.0f;
        //cpuSkillsGkText.text = "Defense: " + team[1];
        //cpuSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        //cpuSkillsAttackText.text = "Attack: " + team[2];
    }

    public void onClickShopPromotionClose()
    {
        shopPromotionCanvas.SetActive(false);
    }

    public void onClickShopSkillsButton()
    {
        shopScript.showTeamsSkillsPanel();
    }

    public void onClickCoinsDiamondButton()
    {
        if (Globals.isPlayerCardLeague(leagueNames[leagueAIdx]))
            shopScript.showDiamondsPanel();
        else
            shopScript.showCoinsPanel();

    }

    private void updateShopOffer(string leagueName)
    {
        string treasureName = Languages.getTranslate("COINS");
        string treasureNum = Globals.coins.ToString();
        string mainTreasureNameImg = "others/heapSmallGold";
        string shopNameImg = "others/coinsShop";

        if (Globals.isPlayerCardLeague(leagueName))
        {
            treasureName = Languages.getTranslate("DIAMONDS");
            treasureNum = Globals.diamonds.ToString();
            mainTreasureNameImg = "others/heapMiddleDiamond";
            shopNameImg = "others/diamondShop";
        }

        if (Globals.isFriendly)
        {
            friendlyCoinsDiamondMainImg.texture =
                Resources.Load<Texture2D>(mainTreasureNameImg);
            friendlyShopCoinDiamondIconImg.texture =
                Resources.Load<Texture2D>(shopNameImg);

            friendlyCoinsDiamondMainText.text = treasureNum;
            friendlyShopCoinDiamondText.text = treasureName;

            /*friendlyShopCoinsDiamondsButton.onClick.RemoveAllListeners();
            if (treasureName.Equals("DIAMONDS"))
            {
                friendlyShopCoinsDiamondsButton.onClick.AddListener(
                           delegate
                           {
                               shopScript.showDiamondsPanel();
                           });
            }
            else
            {
                friendlyShopCoinsDiamondsButton.onClick.AddListener(
                           delegate
                           {
                               shopScript.showCoinsPanel();
                           });
            }*/
        }
        else
        {
            tournamentCoinsDiamondMainImg.texture =
                 Resources.Load<Texture2D>(mainTreasureNameImg);
            tournamentShopCoinDiamondIconImg.texture =
                 Resources.Load<Texture2D>(shopNameImg);

            tournamentCoinsDiamondMainText.text = treasureNum;
            tournamentShopCoinDiamondIconText.text = treasureName;

            /*if (treasureName.Equals("DIAMONDS"))
            {
                tournamentShopCoinsDiamondsButton.onClick.AddListener(
                           delegate
                           {
                               shopScript.showDiamondsPanel();
                           });
            }
            else
            {
                tournamentShopCoinsDiamondsButton.onClick.AddListener(
                           delegate
                           {
                               shopScript.showCoinsPanel();
                           });
            }*/
        }
    }

    public void ChooseTeamNextCanvas()
    {
        clearFlagButtonsVars(true, true);
        //mainMenuCanvas.SetActive(false);
        //chooseTeamCanvas.SetActive(false);
        //gameSettingsCanvas.SetActive(false);
        //groupTournamentCanvas.SetActive(true);
        saveSelectedTeamsToGlobals(false);

        if (Globals.isTrainingActive)
        {
            //SceneManager.LoadScene("gameLoader");
            Globals.isGameSettingActive = false;
            //training always on grass
            Globals.stadiumNumber = 0;
            Globals.loadSceneWithBarLoader("gameScene");
            return;
        }
        else
        {
            /*check if you have enough coins to choose that teams*/
            if (Globals.teamBlocked == true)
            {
                //string[] team = nationalTeams.getTeamByIndex(currATeamIdx);
                string[] team = teamsA.getTeamByIndex(currATeamIdx);

                int coinsNeeded = int.Parse(team[4]);
                if (Globals.coins < coinsNeeded &&
                    !PlayerPrefs.HasKey(team[0]))
                {
                    popUpNoCoins.SetActive(true);
                    return;
                }

                /*if (Globals.isFriendly)
                {
                    team = nationalTeams.getTeamByIndex(currBTeamIdx);
                    coinsNeeded = int.Parse(team[4]);
                    if (Globals.coins < coinsNeeded)
                    {
                        popUpNoCoins.SetActive(true);
                        return;
                    }
                }*/
            }

            Debug.Log("Globals.numMatchesInThisSession " + Globals.numMatchesInThisSession
                + " Globals.numGameOpened " + Globals.numGameOpened);

            if ((Globals.numGameOpened <= 1) &&
                (Globals.numMatchesInThisSession == 0))
            {
                mainMenuCanvas.SetActive(false);
                friendlyCanvas.SetActive(false);
                tournamentCanvas.SetActive(false);
                gameSettingsCanvas.SetActive(true);

                if ((Globals.numGameOpen <= 1) &&
                    !Globals.isMultiplayer) {
                    trainingYesButton();
                }
                else
                {
                    trainingCanvas.SetActive(true);
                    /////showBanner();
                }
                return;
            }
        }

        //Debug.Log("DBG1334 init stadium before " + Globals.teamBid);
        if (Globals.PITCHTYPE.Equals("STREET"))
        {
            //Globals.stadiumNumber = 0;
            Globals.commentatorStr = "NO";
            //loadGameScene();
            initChooseStadiumCanvas();
        }
        else
            initChooseStadiumCanvas();     
        //loadGameScene();
    }

    public void initChooseStadiumCanvas()
    {

        /////showBanner();
        ///
        if (Globals.PITCHTYPE.Equals("INDOOR")) {
            chooseStadiumCanvasIndoor.SetActive(true);
        } else {
            if (Globals.PITCHTYPE.Equals("GRASS"))
            {
                chooseStadiumCanvas.SetActive(true);
            } else
            {
                chooseStadiumCanvasStreet.SetActive(true);
            }
        }


        if (Globals.is_app_paid == true)
        {
            chooseStadiumCanvasPaid.SetActive(true);
            chooseStadiumCanvas.SetActive(false);
            chooseStadiumCanvasIndoor.SetActive(false);
            chooseStadiumCanvasStreet.SetActive(false);
        }

        /*print("#DBG1334 init choose stadiu Globals.teamAleague " 
    + Globals.playerADesc + " Globals.playerADesc " 
    + Globals.teamAname + " + Globals.teamAname "
    + " Globals team B " + Globals.teamBleague
    + " teamB Name " + Globals.teamBname
    + " temBIDx " + Globals.teamBid);*/

        //if (Globals.coins < 6000)
        //    lockStadiumPanel.SetActive(true);
        //else
        //    lockStadiumPanel.SetActive(false);
    }

    public void ChooseStadiumNextCanvas()
    {
        loadGameScene();
    }

    public void createNewGameOnClick()
    {
        Globals.isNewGame = true;
        Globals.isNewSeason = false;
        Globals.leagueSeason = 20;
        Globals.champCupPromotedNextSeason = true;

        loadGameCanvas.SetActive(false);
        gameSettingsCanvas.SetActive(true);
        cleanupMatchTableMain();
    }

    private void deleteGameSaveYes(string saveName, 
                                   string savedGamesFileName,
                                   string leagueName,
                                   string teamName)
    {
        Globals.deleteGameSave(saveName, 
                               savedGamesFileName,
                               leagueName,
                               teamName);
        initGameSaves();
        yesNoMenuPanel.SetActive(false);
    }

    private void deleteGameSaveNo()
    {
        yesNoMenuPanel.SetActive(false);
    }


    public void deleteGameSave(int buttonIdx, string savedGamesFileName)
    {
        int chosenGameIdx = (gameLoadPageIdx * gameSavesNumRows) + buttonIdx;
        string saveName = gameSavedList[chosenGameIdx];

        yesNoMenuNoAnswer.onClick.RemoveAllListeners();
        yesNoMenuYesAnswer.onClick.RemoveAllListeners();

        if (PlayerPrefs.HasKey(saveName))
        {
            string saveDesc = PlayerPrefs.GetString(saveName);

            string[] teamDesc = saveDesc.Split('|');
            string[] countryName = teamDesc[0].Split('_');
            string saveIdx = countryName[1];

            //print("SAVEDESC " + teamDesc[1] + " saveName " + saveDesc);

            yesNoMenuHeaderText.text = 
                Languages.getTranslate("Confirm deleting save: ") + saveIdx 
                + " "
                + countryName[0]
                + " "
                + teamDesc[1];

            yesNoMenuYesAnswer.onClick.AddListener(
            delegate {
                  deleteGameSaveYes(saveName,
                                    savedGamesFileName,
                                    countryName[0],
                                    teamDesc[1]);
            });
        } else
        {
            yesNoMenuYesAnswer.onClick.AddListener(
            delegate {
              deleteGameSaveYes(saveName,
                                savedGamesFileName,
                                "",
                                "");
            });
        }

        yesNoMenuNoAnswer.onClick.AddListener(
          delegate { deleteGameSaveNo(); });
      

        yesNoMenuPanel.SetActive(true);

        //Globals.deleteGameSave(saveName, savedGamesFileName);
    }
  
    public void loadFromSaveOnClick(int buttonIdx, string savedGamesFileName)
    {
        int chosenGameIdx = (gameLoadPageIdx * gameSavesNumRows) + buttonIdx;
        string[] entryDesc = gameSavedList[chosenGameIdx].Split('_');

        //print("DBGLOADSAVED123 clikc buttonIDX " + buttonIdx);

        cleanupMatchTableMain();

        if (savedGamesFileName.Equals("savedGames_CUP"))
        {
            Globals.gameType = "CUP";
        } else
        {
            Globals.gameType = "LEAGUE";
        }

        Globals.isNewGame = false;
        Globals.isNewSeason = false;
        //Globals.leagueName = entryDesc[0].Split('_')[0];
        Globals.leagueName = entryDesc[0];
        //Globals.savedFileName = entryDesc[0];
        Globals.savedFileName = gameSavedList[chosenGameIdx];

        //Globals.teamAname = entryDesc[1];

        if (PlayerPrefs.HasKey(gameSavedList[chosenGameIdx]))
        {
            string saveDesc = PlayerPrefs.GetString(gameSavedList[chosenGameIdx]);
            string[] teamDesc = saveDesc.Split('|');
            Globals.teamAname = teamDesc[1];
            Globals.playerTeamId = int.Parse(teamDesc[4]);
            Globals.playerTeamName = teamDesc[1];
            Globals.leagueSeason = int.Parse(teamDesc[3]);
            Globals.champCupPromotedNextSeason = Convert.ToBoolean(teamDesc[5]);
        }

        //print("#DBGSAVED gameSavedList[chosenGameIdx] " + gameSavedList[chosenGameIdx] + " " +
        //    " Globasl.savedFile " + Globals.savedFileName
        //    + " Globals.leagueName " + Globals.leagueName
        //    + " Globals.teamAname " + Globals.teamAname
        //    + " Globals.leagueSeason " + Globals.leagueSeason);

        //print("DBGLOADSAVED leagueName " + Globals.leagueName
        //    + " savedFileName " + Globals.savedFileName + "" +
        //    " chosenGameIdx " + chosenGameIdx
        //   + " gameLoadPageIdx " + gameLoadPageIdx);

        //print("#DBG LOAD LEAGUES");
        Globals.isGameSettingActive = false;
        Globals.loadSceneWithBarLoader("Leagues");
        //SceneManager.LoadScene("Leagues");
    }

    public void loadGamePrevCanvas()
    {
        GameSettingsPrevCanvas();
    }

    public void printSavedGames(int gameLoadPageIdx, List<string> list)
    {
        int startIdx = gameLoadPageIdx * 3;
        int currIdx = 0;

        for (int i = startIdx; i < list.Count; i++)
        {
            loadGameButtonGO[currIdx].SetActive(true);

            if (PlayerPrefs.HasKey(list[i]))
            {
                string saveDesc = PlayerPrefs.GetString(list[i]);

                //print("SAVEDESC " + saveDesc);
                string[] teamDesc = saveDesc.Split('|');
                string[] countryName = teamDesc[0].Split('_');
                string saveIdx = countryName[1];

                loadGameButtonText[currIdx].text =
                    saveIdx + ": " +
                    Languages.getTranslate(" Week ") + teamDesc[2] +
                    Languages.getTranslate(" - Season ") + teamDesc[3] + "/" +
                    (int.Parse(teamDesc[3]) + 1).ToString();

                loadGameButtonPart2Text[currIdx].text = 
                    Globals.firstLetterUpperCase(countryName[0]) +
                   // countryName[0].Substring(0,1) + countryName[0].Substring(1).ToLower() +
                    " - " + teamDesc[1];

                //string fullTeamPath = "national/" + countryName[0];
                if (countryName[0].Contains("CUP"))
                {
                    loadGameFlagsCountryImg[currIdx].texture =
                         Resources.Load<Texture2D>("others/cupLong");
                } else {
                    setFlagImage(loadGameFlagsCountryImg[currIdx], "national/" + countryName[0]);
                }

                //fullTeamPath = countryName[0].ToLower() + "/" + teamDesc[1];

                //string fullTeamPath =
                //    Globals.getFlagPath(countryName[0], teamDesc[1]);   

                string fullTeamPath =
                        graphicsStandard.getFlagPath(teamDesc[1]);

                setFlagImage(loadGameFlagsClubImg[currIdx], fullTeamPath);
                currIdx++;
            }

            if (currIdx > 2)
                break;
        }

        /*Leave others empty*/
        for (int i = currIdx; i < gameSavesNumRows; i++)
        {
            loadGameButtonText[i].text = "";
            loadGameButtonGO[i].SetActive(false);
        }
    }

    public void nextLoadGameSavedPage()
    {
        int maxIdx = (Mathf.Max(0, gameSavedList.Count - 1) / gameSavesNumRows);

        //print("GAMESAVEDLISTIDX " + gameLoadPageIdx + " maxIDX " + maxIdx);

        if (gameLoadPageIdx < maxIdx)
        {
            gameLoadPageIdx++;
            printSavedGames(gameLoadPageIdx, gameSavedList);
        }
    }

    public void prevLoadGamePrevPage()
    {
        if (gameLoadPageIdx > 0)
        {
            gameLoadPageIdx--;
            printSavedGames(gameLoadPageIdx, gameSavedList);
        }
    }

    public void onClickPopUpNoCoins()
    {
        popUpNoCoins.SetActive(false);
    }

    private void cleanupMatchTableMain()
    {
        if (MatchTableMain.matchTableInstance != null)
        {
            Destroy(MatchTableMain.matchTableInstance);
        }

        MatchTableMain.matchTableInstance = null;
    }

    public void loadGameScene()
    {
        Globals.leagueName = leagueNames[leagueAIdx];

        /*if (Globals.PITCHTYPE.Equals("STREET"))
        {
            Globals.stadiumNumber = 0;
            Globals.commentatorStr = "NO";
        }*/

        if (Globals.isFriendly)
        {
            print("Globals.isGameSettingActive " + Globals.isGameSettingActive);

            if (Globals.isPlayerCardLeague(Globals.leagueName))
            {
                //Globals.isGameSettingActive = true;
                Globals.loadSceneWithBarLoader("teamManagement");
            }
            //SceneManager.LoadScene("");
            else
            {
                //Globals.isGameSettingActive = false;
                if (!Globals.is_app_paid)
                    SceneManager.LoadScene("specialShopOffers");
                else
                    SceneManager.LoadScene("ExtraPowers");
            }

            Globals.isGameSettingActive = true;
            Globals.playerPlayAway = false;
            //SceneManager.LoadScene("specialShopOffers");
        }
        else
        {
            /*TOCHECK*/
            //if (Globals.isLeague)
            //{
            //if (MatchTableMain.matchTableInstance != null)
            //{
            //    Destroy(MatchTableMain.matchTableInstance);
            //}

            //MatchTableMain.matchTableInstance = null;
            cleanupMatchTableMain();

            Globals.isNewGame = true;
            Globals.isGameSettingActive = false;
            Globals.loadSceneWithBarLoader("Leagues");

            //SceneManager.LoadScene("Leagues");
            //}
            //else
            //{

            //    if (GroupTable.GroupTableInstance != null)
            //    {
            //        Destroy(GroupTable.GroupTableInstance);
            //    }
            //    GroupTable.GroupTableInstance = null;

            //    SceneManager.LoadScene("GroupTableScene");
            //}
        }
    }

    public void ChooseTeamPrevCanvas()
    {
        clearFlagButtonsVars(true, true);
        popUpNoCoins.SetActive(false);

        /////showBanner();
        //saveSelectedTeamsToGlobals();
        mainMenuCanvas.SetActive(false);
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        gameSettingsCanvas.SetActive(true);
    }

    public void prevLevel()
    {
        if (levelsIdx > 0)
        {
            level.text = Languages.getTranslate(levels[--levelsIdx]);
            levelMain.text = Languages.getTranslate(levels[levelsIdx]);
        }
    }

    public void nextLevel()
    {
        if (levelsIdx < (levels.Length - 1))
        {
            level.text = Languages.getTranslate(levels[++levelsIdx]);
            levelMain.text = Languages.getTranslate(levels[levelsIdx]);
        }
    }

    public void prevGraphics()
    {
        if (graphicsSettingsIdx > 0)
        {
            graphicsText.text = Languages.getTranslate(graphics[--graphicsSettingsIdx]);
            graphicsTextMain.text = Languages.getTranslate(graphics[graphicsSettingsIdx]);
        }
    }

    public void nextGraphics()
    {
        if (graphicsSettingsIdx < (graphics.Length - 1))
        {
            graphicsText.text = Languages.getTranslate(graphics[++graphicsSettingsIdx]);
            graphicsTextMain.text = Languages.getTranslate(graphics[graphicsSettingsIdx]);
        }
    }

    public void prevTime()
    {
        if (gameTimesIdx > 0)
        {
            time.text = Languages.getTranslate(gameTimes[--gameTimesIdx]);
            timeMain.text = Languages.getTranslate(gameTimes[gameTimesIdx]);
        }
    }

    public void nextTime()
    {
        if (gameTimesIdx < (gameTimes.Length - 1))
        {
            time.text = Languages.getTranslate(gameTimes[++gameTimesIdx]);
            timeMain.text = Languages.getTranslate(gameTimes[gameTimesIdx]);
        }
    }

    public void prevTrainingMode()
    {
        if (trainingModeIdx > 0)
        {
            trainingMode.text = Languages.getTranslate(trainingModes[--trainingModeIdx]);
            trainingModeMain.text = Languages.getTranslate(trainingModes[trainingModeIdx]);
        }
    }

    public void nextTrainingMode()
    {
        if (trainingModeIdx < (trainingModes.Length - 1))
        {
            trainingMode.text = Languages.getTranslate(trainingModes[++trainingModeIdx]);
            trainingModeMain.text = Languages.getTranslate(trainingModes[trainingModeIdx]);
        }
    }

    public void prevJoystickSide()
    {
        if (joystickSideIdx > 0)
        {
            joystickSideText.text = Languages.getTranslate(joystickSide[--joystickSideIdx]);
            joystickSideTextMain.text = Languages.getTranslate(joystickSide[joystickSideIdx]);
        }
    }

    public void nextJoystickSide()
    {
        if (joystickSideIdx < (joystickSide.Length - 1))
        {
            joystickSideText.text = Languages.getTranslate(joystickSide[++joystickSideIdx]);
            joystickSideTextMain.text = Languages.getTranslate(joystickSide[joystickSideIdx]);
        }
    }


    public void prevMaxShotTime()
    {
        if (maxTimeToShotIdx > 0)
        {
            maxTimeToShotText.text = Languages.getTranslate(maxTimeToShot[--maxTimeToShotIdx]);
            maxTimeToShotTextMain.text = Languages.getTranslate(maxTimeToShot[maxTimeToShotIdx]);
        }
    }

    public void NextMaxShotTime()
    {
        if (maxTimeToShotIdx < (maxTimeToShot.Length - 1))
        {
            maxTimeToShotText.text = Languages.getTranslate(maxTimeToShot[++maxTimeToShotIdx]);
            maxTimeToShotTextMain.text = Languages.getTranslate(maxTimeToShot[maxTimeToShotIdx]);
        }
    }

    public void prevPowers()
    {
        if (powersIdx > 0)
        {
            powersText.text = Languages.getTranslate(powers[--powersIdx]);
            powersTextMain.text = Languages.getTranslate(powers[powersIdx]);
        }
    }

    public void nextPowers()
    {
        if (powersIdx < (powers.Length - 1))
        {
            powersText.text = Languages.getTranslate(powers[++powersIdx]);
            powersTextMain.text = Languages.getTranslate(powers[powersIdx]);
        }
    }

    public void prevCommentator()
    {
        if (commentatorIdx > 0)
        {
            commentatorText.text = Languages.getTranslate(commentator[--commentatorIdx]);
            commentatorTextMain.text = Languages.getTranslate(commentator[commentatorIdx]);
        }
    }

    public void nextCommentator()
    {
        if (commentatorIdx < (commentator.Length - 1))
        {
            commentatorText.text = Languages.getTranslate(commentator[++commentatorIdx]);
            commentatorTextMain.text = Languages.getTranslate(commentator[commentatorIdx]);
        }
    }



    public void prevLeagueA()
    {
        //print("NEXTLEAGUECLICK 1 PREV");

        if (leagueAIdx > 0)
        {
            //time.text = leagueNames[--leagueIdx];
            leagueAName.text = Languages.getTranslate(leagueNames[--leagueAIdx]);
            teamsA.setLeague(leagueNames[leagueAIdx]);
            currATeamIdx = 0;
            setupTeamDefaults();
            clearFlagButtonsVars(true, false);
            //print("NEXTLEAGUECLICK 2 PREV");

            //--leagueAIdx;
            //setFlagImage(mainFlag, leagueNames[leagueAIdx]);
        }

        Globals.leagueName = leagueNames[leagueAIdx];
    }

    public void nextLeagueA()
    {
        //print("NEXTLEAGUECLICK 1");
        if (leagueAIdx < (leagueNames.Length - 1))
        {
            //time.text = leagueNames[++leagueIdx];
            //leagueNameText.text = leagueNames[++leagueIdx];
            leagueAName.text = Languages.getTranslate(leagueNames[++leagueAIdx]);
            teamsA.setLeague(leagueNames[leagueAIdx]);
            currATeamIdx = 0;
            setupTeamDefaults();
            clearFlagButtonsVars(true, false);
            //print("NEXTLEAGUECLICK 2" + leagueAName.text);
            //++leagueIdx;
            //setFlagImage(mainFlag, leagueNames[leagueIdx]);
        }

        Globals.leagueName = leagueNames[leagueAIdx];
    }

    public void prevLeagueB()
    {
        if (leagueBIdx > 0)
        {
            //time.text = leagueNames[--leagueIdx];
            leagueBName.text = Languages.getTranslate(leagueNames[--leagueBIdx]);
            teamsB.setLeague(leagueNames[leagueBIdx]);
            currBTeamIdx = 0;
            setupTeamDefaults();
            clearFlagButtonsVars(false, true);
            //--leagueAIdx;
            //setFlagImage(mainFlag, leagueNames[leagueAIdx]);
        }
    }

    public void nextLeagueB()
    {
        if (leagueBIdx < (leagueNames.Length - 1))
        {
            //time.text = leagueNames[++leagueIdx];
            //leagueNameText.text = leagueNames[++leagueIdx];
            leagueBName.text = Languages.getTranslate(leagueNames[++leagueBIdx]);
            teamsB.setLeague(leagueNames[leagueBIdx]);
            currBTeamIdx = 0;
            setupTeamDefaults();
            clearFlagButtonsVars(false, true);
            //++leagueIdx;
            //setFlagImage(mainFlag, leagueNames[leagueIdx]);
        }
    }

    public void saveGlobalsSettingsToPrefab()
    {
        PlayerPrefs.SetInt("gameTimesIdx", gameTimesIdx);
        PlayerPrefs.SetInt("levelsIdx", levelsIdx);
        PlayerPrefs.SetInt("trainingModeIdx", trainingModeIdx);
        PlayerPrefs.SetInt("graphicsSettingsIdx", graphicsSettingsIdx);
        PlayerPrefs.SetInt("gameSettingsSave", 1);
        PlayerPrefs.SetInt("joystickSideIdx", joystickSideIdx);

        PlayerPrefs.SetInt("powersIdx", powersIdx);
        PlayerPrefs.SetInt("commentatorIdx", commentatorIdx);
        PlayerPrefs.SetInt("maxTimeToShotIdx", maxTimeToShotIdx);
        PlayerPrefs.Save();
    }

    public void recoverPrefabGameSettings()
    {
        //print("SETINGS REC " + PlayerPrefs.HasKey("gameSettingsSave"));
        if (!isMultiplayer)
        {
            if (PlayerPrefs.HasKey("gameSettingsSave"))
            {
                levelsIdx = PlayerPrefs.GetInt("levelsIdx");
                gameTimesIdx = PlayerPrefs.GetInt("gameTimesIdx");
                trainingModeIdx = PlayerPrefs.GetInt("trainingModeIdx");
                graphicsSettingsIdx = PlayerPrefs.GetInt("graphicsSettingsIdx");
                joystickSideIdx = PlayerPrefs.GetInt("joystickSideIdx");

                powersIdx = PlayerPrefs.GetInt("powersIdx", powersIdx);
                commentatorIdx = PlayerPrefs.GetInt("commentatorIdx", commentatorIdx);
                maxTimeToShotIdx = PlayerPrefs.GetInt("maxTimeToShotIdx", maxTimeToShotIdx);
            }

            if (PlayerPrefs.HasKey("goalieMode"))
                trainingModeIdx = PlayerPrefs.GetInt("trainingModeIdx");

        }
    }

    public void GameSettingsNextCanvas()
    {
        clearFlagButtonsVars(true, true);

        enable3DModels();
        //showBanner();
        //admobAdsScript.hideBanner();
        mainMenuCanvas.SetActive(false);
        gameSettingsCanvas.SetActive(false);

        if (Globals.isFriendly)
        {
            friendlyCanvas.SetActive(true);
        }
        else
        {
            tournamentCanvas.SetActive(true);
        }

        //currCoinsText = GameObject.Find("chooseMenuPointsText").GetComponent<TextMeshProUGUI>();
        //currCoinsText.text = Globals.coins.ToString();

        saveSettingsToGlobals();

        changeTeamReferences(Globals.isFriendly);
        setupAllSetttingsToDefault();

        setupGameTypeDefaults();
        setupTeamDefaults();
    }

    public void GameSettingsPrevCanvas()
    {
        ////showBanner();
        gameSettingsCanvas.SetActive(false);
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        saveSettingsToGlobals();
    }

    public string getGraphicStringByIndex(int graphicsSettingsIdx)
    {
        return graphics[graphicsSettingsIdx];
    }

    public string getJoystickSideStringByIndex(int joystickSideIdx)
    {
        return joystickSide[joystickSideIdx];
    }

    public void enable3DModels()
    {
        friendlyModelTeamA.SetActive(true);
        friendlyModelTeamB.SetActive(true);
        friendlyModelTeamAhair.SetActive(true);
        friendlyModelTeamAhair.SetActive(true);
    }

    public void disable3DModels()
    {
        friendlyModelTeamA.SetActive(false);
        friendlyModelTeamB.SetActive(false);
        friendlyModelTeamAhair.SetActive(false);
        friendlyModelTeamAhair.SetActive(false);
    }

    private void saveSettingsToGlobals()
    {
        Globals.level = levelsIdx + 1;
        Globals.matchTime = gameTimes[gameTimesIdx];
        Globals.graphicsQuality = graphics[graphicsSettingsIdx];
        //Globals.isTrainingActive = true;
        Globals.joystickSide = joystickSide[joystickSideIdx];

        Globals.powersStr = powers[powersIdx];
        Globals.maxTimeToShotStr = maxTimeToShot[maxTimeToShotIdx];
        Globals.commentatorStr = commentator[commentatorIdx];


        if ((trainingModeIdx == 0) ||
            Globals.isMultiplayer)
        {
            Globals.isTrainingActive = false;
        }

        Globals.levelIdx = levelsIdx;
        Globals.gameTimesIdx = gameTimesIdx;
        Globals.graphicsSettingsIdx = graphicsSettingsIdx;
        Globals.joystickSideIdx = joystickSideIdx;

        //traingMode was replaced by goalie automatic mode
        Globals.trainingModeIdx = trainingModeIdx;
        if (trainingModeIdx == 1)
            Globals.PRO_MODE = true;
        else
            Globals.PRO_MODE = false;

        Globals.isTrainingActive = false;

        Globals.powersIdx = powersIdx;
        Globals.maxTimeToShotIdx = maxTimeToShotIdx;
        Globals.commentatorIdx = commentatorIdx;
}

    public void getRandomTeamBAndFill()
    {
        int randomLeague = UnityEngine.Random.Range(0, Globals.MAX_PLAYERS_CARD_LEAGUES);
        Globals.teamBleague = Globals.playerCardLeagues[randomLeague];
        Globals.teamBCustomize = false;

        Teams teamsB = new Teams(Globals.playerCardLeagues[randomLeague]);

        int currBTeamIdx = -1;
        for (int i = 0; i < 1000; i++)
        {
            currBTeamIdx =
                   UnityEngine.Random.Range(0, teamsB.getMaxActiveTeams());

            if (!Globals.isTeamCustomize(teamsB.getTeamByIndex(currBTeamIdx)[0]))
                break;         
        }

        /*take an average when playing friendly*/
        //Vector2 avgTeamSkills =
        //      Globals.getTeamSkillsAverage(teamsB.getTeamByIndex(currBTeamIdx),
        //                                   leagueNames[leagueBIdx]);

        int numPlayers = teamsB.getTeamByIndex(currBTeamIdx)[12].Split('|').Length;
        int randPlayer = 
                   UnityEngine.Random.Range(0, numPlayers);
        Globals.playerBDesc = 
            teamsB.getTeamByIndex(currBTeamIdx)[12].Split('|')[randPlayer];

        Globals.teamBGkStrength = int.Parse(Globals.playerBDesc.Split(':')[2]);
        Globals.teamBAttackStrength = int.Parse(Globals.playerBDesc.Split(':')[3]);

        Globals.teamBcumulativeStrength =  
            Globals.teamBGkStrength + Globals.teamBAttackStrength;

        Globals.stadiumColorTeamB =
            teamsB.getTeamByIndex(currBTeamIdx)[5];

        Globals.teamBname = teamsB.getTeamByIndex(currBTeamIdx)[0];

        Globals.teamBid = int.Parse(
            teamsB.getTeamByIndex(currBTeamIdx)[3]);

        Globals.teamBDesc = teamsB.getTeamByIndex(currBTeamIdx)[6] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[7] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[8] + "|" +
                    Globals.playerBDesc.Split('|')[0].Split(':')[6] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[14].Split('|')[0] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[13].Split('|')[0] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[5].Split('|')[0] + "|" +
                    teamsB.getTeamByIndex(currBTeamIdx)[5].Split('|')[1];


        print("UPDATE2 teamBID");

        print("Globals.teamBname " + Globals.teamBname);
    }


    private void saveSelectedTeamsToGlobals(bool isMultiplayer)
    {
        //Globals.teamAname = nationalTeams.getTeamByIndex(currATeamIdx)[0];
        //Globals.teamBname = nationalTeams.getTeamByIndex(currBTeamIdx)[0];
        Globals.teamAname = teamsA.getTeamByIndex(currATeamIdx)[0];
        Globals.playerTeamName = Globals.teamAname;
        //print("Globals.teamAAttackStrength save selected ");

        if (!isMultiplayer)
        {
            Globals.teamBname = teamsB.getTeamByIndex(currBTeamIdx)[0];
        }
     
        Globals.teamAGkStrength =
           int.Parse(teamsA.getTeamByIndex(currATeamIdx)[1]);
        Globals.teamAAttackStrength =
            int.Parse(teamsA.getTeamByIndex(currATeamIdx)[2]);
       
        Globals.teamAcumulativeStrength =
             int.Parse(teamsA.getTeamByIndex(currATeamIdx)[1]) +
             int.Parse(teamsA.getTeamByIndex(currATeamIdx)[2]);

        Globals.stadiumColorTeamA = 
            teamsA.getTeamByIndex(currATeamIdx)[5];
        
        Globals.teamAid = int.Parse(
            teamsA.getTeamByIndex(currATeamIdx)[3]);

        Globals.playerTeamId = Globals.teamAid;

        Globals.playerADesc = teamsA.getTeamByIndex(currATeamIdx)[12].Split('|')[0];

        if (Globals.isMultiplayer && 
            !updateMultiTexture)
        {
            if (Globals.isPlayerCardLeague(leagueNames[leagueAIdx]))
            {
                multiplayerMenuScript.setBottomButtonInteractable(0, true, 1f);
            }
            else
            {
                multiplayerMenuScript.setBottomButtonInteractable(0, false, 0.33f);
            }
        }

        print("leagueNames[leagueAIdx] " + leagueNames[leagueAIdx]);
        if (Globals.isMultiplayer &&
            Globals.isPlayerCardLeague(leagueNames[leagueAIdx]))            
        {

            if (PlayerPrefs.HasKey(Globals.playerTeamName + "_lastSelectedPlayer"))
                Globals.playerADesc = PlayerPrefs.GetString(Globals.playerTeamName + "_lastSelectedPlayer");

            string[] playerDesc = Globals.playerADesc.Split('|');
            int energy = 100;
            /*print("#DBG playerDesc[0].Split(':')[0] " + playerDesc[0].Split(':')[0]+ " Globals.playerTeamName " +
                Globals.playerTeamName + " _lastSelectedPlayer " + PlayerPrefs.HasKey(Globals.playerTeamName + "_lastSelectedPlayer")
                + " Globals.multiplayerSaveName " + Globals.multiplayerSaveName
                + " energyGet " + (PlayerPrefs.GetInt(playerDesc[0].Split(':')[0] + "_energy_" + Globals.multiplayerSaveName)));*/
            if (PlayerPrefs.HasKey(playerDesc[0].Split(':')[0] + "_energy_" + Globals.multiplayerSaveName))
            {
                energy =
                    PlayerPrefs.GetInt(playerDesc[0].Split(':')[0] + "_energy_" + Globals.multiplayerSaveName);
            }

            /*Globals.teamAGkStrength = (int) (float.Parse(playerDesc[0].Split(':')[2]) *
                                        ((float) energy / (float) Globals.MAX_ENERGY));
            Globals.teamAAttackStrength = (int)(float.Parse(playerDesc[0].Split(':')[3]) *
                                        ((float) energy / (float) Globals.MAX_ENERGY));*/

            // Globals.teamAGkStrength = (int)(float.Parse(playerDesc[0].Split(':')[2]));

            // Globals.teamAAttackStrength = (int)(float.Parse(playerDesc[0].Split(':')[3]));

            print("#DBG skills team " + teamsA.getTeamByIndex(currATeamIdx)[0] +
               teamsA.getTeamByIndex(currATeamIdx)[12] + " league " + leagueNames[leagueAIdx]);

            Vector3 skills =
                Globals.getTeamSkillsAverage(teamsA.getTeamByIndex(currATeamIdx), leagueNames[leagueAIdx]);

          
            Globals.teamAGkStrength = (int) skills.x;
            //(int)(float.Parse(playerDesc[0].Split(':')[2]));
            Globals.teamAAttackStrength = (int) skills.y;
            //(int)(float.Parse(playerDesc[0].Split(':')[3]));

            //print("teamDesc set in gameSettings 1 " + Globals.teamAname
            //              + " Globals.teamAGkStrength " + Globals.teamAGkStrength
            //              + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);

            Globals.teamAcumulativeStrength = Globals.teamAGkStrength + Globals.teamAAttackStrength;
            Globals.energyPlayerA = energy;
            //print("energy " + energy);
        }

        if (isMultiplayer)

            //print("teamDesc set in gameSettings 2 " + Globals.teamAname
            //               + " Globals.teamAGkStrength " + Globals.teamAGkStrength
            //               + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);

        {
            if (Globals.isPlayerCardLeague(leagueNames[leagueAIdx]))
            {
                Globals.teamADesc = teamsA.getTeamByIndex(currATeamIdx)[6] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[7] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[8] + "|" +
                    Globals.playerADesc.Split('|')[0].Split(':')[6] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[14].Split('|')[0] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[13].Split('|')[0] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[5].Split('|')[0] + "|" +
                    teamsA.getTeamByIndex(currATeamIdx)[5].Split('|')[1];
            } else
            {
                Globals.teamADesc = 
                   teamsA.getTeamByIndex(currATeamIdx)[6] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[7] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[8] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[12].Split('|')[0].Split(':')[6] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[14].Split('|') + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[13].Split('|')[0] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[5].Split('|')[0] + "|" +
                   teamsA.getTeamByIndex(currATeamIdx)[5].Split('|')[1];
            }
        }


        ///print("#DBGGlobals.teamAGkStrength " + Globals.teamAGkStrength 
         //   + " Globals.multiplayerSaveName " + Globals.multiplayerSaveName);

        if (!Globals.isMultiplayer)
            Globals.playerBDesc = teamsB.getTeamByIndex(currBTeamIdx)[12].Split('|')[0];

        /*if (Globals.isMultiplayer &&
            Globals.isPlayerCardLeague(leagueNames[leagueBIdx]))
        {

            if (PlayerPrefs.HasKey(Globals.teamAname + "_lastSelectedPlayer"))
                Globals.playerBDesc = PlayerPrefs.GetString(Globals.teamAname + "_lastSelectedPlayer");

            string[] playerDesc = Globals.playerBDesc.Split(':');

            Globals.teamBGkStrength = (int)(float.Parse(playerDesc[2]));
            Globals.teamBAttackStrength = (int)(float.Parse(playerDesc[3]));
            Globals.teamBcumulativeStrength = Globals.teamBGkStrength + Globals.teamBAttackStrength;
        }*/

        Globals.teamAleague = leagueNames[leagueAIdx];
        if (!Globals.isMultiplayer)
            Globals.teamBleague = leagueNames[leagueBIdx];

        /*if not friendly this should be fill in GroupTable */
        if (Globals.isFriendly &&
            !Globals.isMultiplayer)
        {
            updateTeamBGlobalsSettings(currBTeamIdx);            
        }

        Globals.teamAid = currATeamIdx;
        Globals.playerTeamId = Globals.teamAid;

        //print("DBGFRIENDLY SAVEID " + currBTeamIdx);
        if (!isMultiplayer)
        {
            Globals.teamBid = currBTeamIdx;
            print("UPDATE3 teamBID");
        }
        Globals.gameInGroup = true;

        /*print("DBG1334 TEAMALEAGUE " + Globals.teamAleague + " Globals.playerADesc " 
            +
              Globals.playerADesc + " Globals.teamAname " + Globals.teamAname
             + " Globals team B " + Globals.teamBleague 
             + " teamB Name " + Globals.teamBname
             + " temBIDx " + Globals.teamBid);*/
    }
    
    private void updateTeamBGlobalsSettings(int teamIdx)
    {
        Teams teams = teamsB;

        //is training then?
        if (!Globals.isFriendly)
        {
            teams = new Teams("NATIONALS");
            teamIdx =
                UnityEngine.Random.Range(0, teams.getMaxTeams());
            Globals.teamBleague = "NATIONALS";
        }

        //Globals.teamBGkStrength = int.Parse(teams.getTeamByIndex(teamIdx)[1]);
        //Globals.teamBAttackStrength = int.Parse(teams.getTeamByIndex(teamIdx)[2]);
        //Globals.teamBcumulativeStrength =
        //   int.Parse(teams.getTeamByIndex(teamIdx)[1]) +
        //   int.Parse(teams.getTeamByIndex(teamIdx)[2]);

       
        if (!Globals.isFriendly)
        {
            /*Traning hardcoded values*/
            Globals.teamAcumulativeStrength = 160;
            Globals.teamBcumulativeStrength = 140;

            Globals.teamAGkStrength = 90;
            Globals.teamBGkStrength = 70;

            Globals.teamAAttackStrength = 70;
            Globals.teamBAttackStrength = 70;
        } else
        {
            /*take an average when playing friendly*/
            Vector2 avgTeamSkills =
                  Globals.getTeamSkillsAverage(teams.getTeamByIndex(teamIdx),
                                               leagueNames[leagueBIdx]);
            Globals.teamBGkStrength = (int) avgTeamSkills.x;
            Globals.teamBAttackStrength = (int) avgTeamSkills.y;           
        }

        Globals.teamBcumulativeStrength =  
            Globals.teamBGkStrength + Globals.teamBAttackStrength;

        Globals.stadiumColorTeamB =
            teams.getTeamByIndex(teamIdx)[5];

        //print("#DBG GET ID " + int.Parse(
        //    teams.getTeamByIndex(teamIdx)[3]) + " TEAMID " + teamIdx
        //    + " Globals.stadiumColorTeamB " + Globals.stadiumColorTeamB);

        print("UPDATE1 teamBID");
        //Globals.teamBid = int.Parse(
        //    teams.getTeamByIndex(teamIdx)[3]);
    }

    public void setupAllSetttingsToDefault()
    {
        if (!isMultiplayer)
        {
            currATeamIdx = 1;
            currBTeamIdx = 1;
        }

        init3dModels();
        recoverPrefabGameSettings();
        if (!isMultiplayer)
            setupDefaultSettings();
    }

    private void init3dModels()
    {
        if (!isMultiplayer)
        {
            friendlyModelTeamA = GameObject.Find("friendlyTeamAModel");
            friendlyModelTeamAhair = GameObject.Find("friendlyTeamAModelHair");
            friendlyModelTeamB = GameObject.Find("friendlyTeamBModel");
            friendlyModelTeamBhair = GameObject.Find("friendlyTeamBModelHair");
        }

        tournamentModelTeamA = GameObject.Find("tournamentTeamAModel");
        tournamentModelTeamAhair = GameObject.Find("tournamentTeamAModelHair");

        if (isMultiplayer)
        {
            multiplayerModel[0] = GameObject.Find("multiTeamAModel");
            multiplayerModel[1] = GameObject.Find("multiTeamBModel");
            multiplayerModelHair[0] = GameObject.Find("multiTeamAModelHair");
            multiplayerModelHair[1] = GameObject.Find("multiTeamBModelHair");
        }

        /*graphicsStandard.setPlayerTextures(
            friendlyModelTeamA, friendlyModelTeamAhair, currATeamIdx);

        graphicsStandard.setPlayerTextures(
               friendlyModelTeamB, friendlyModelTeamBhair, currBTeamIdx);
     
        graphicsStandard.setPlayerTextures(
             tournamentModelTeamA, tournamentModelTeamAhair, currATeamIdx);*/

        string playerSkinHairDescA =
                teamsA.getPlayerDescByIndex(currATeamIdx, 0);
        if (!isMultiplayer)
        {    
            graphicsStandard.setPlayerTextures(
                    friendlyModelTeamA,
                    friendlyModelTeamAhair,
                    currATeamIdx,
                    leagueNames[leagueAIdx],
                    playerSkinHairDescA,
                    false,
                    false,
                    teamsA);

            string playerSkinHairDescB =
                teamsA.getPlayerDescByIndex(currATeamIdx, 0);

            graphicsStandard.setPlayerTextures(
                   friendlyModelTeamB,
                   friendlyModelTeamBhair,
                   currBTeamIdx,
                   leagueNames[leagueBIdx],
                   playerSkinHairDescB,
                   false,
                   false,
                   teamsB);
        }

        graphicsStandard.setPlayerTextures(
                tournamentModelTeamA, 
                tournamentModelTeamAhair, 
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
    }

    private void setFlagImage(RawImage image, string team)
    {
        if (team.Contains("logoFile"))
        {
            if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
            {
                image.texture = graphicsStandard.loadTexture(Globals.logoFilePath);
            }
            else
            {
                image.texture = Resources.Load<Texture2D>(
                            "others/logoFile");
            }
            return;
        }

        team = Regex.Replace(team, "\\s+", "");
        image.texture = Resources.Load<Texture2D>("Flags/" + team.ToLower());
    }

    private void setFlagImage(RawImage image, string team, int teamNeededCoins)
    {
        Debug.Log("DBGUNLOCKEDTEAM teamName " + team);
        if (team.Contains("logoFile"))
        {
            if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
            {
                image.texture = graphicsStandard.loadTexture(Globals.logoFilePath);
            } else
            {
                image.texture = Resources.Load<Texture2D>(
                            "others/logoFile");
            }
            
            return;
        }

        if (teamNeededCoins > Globals.coins &&
            !PlayerPrefs.HasKey(Path.GetFileName(team)))
        {
            image.texture = Resources.Load<Texture2D>("Flags/locked");
        }
        else
        {
            team = Regex.Replace(team, "\\s+", "");


            print("#DGBNAME team Name " + team.ToLower());
            //print("team path " + team);
            image.texture = Resources.Load<Texture2D>("Flags/" + team.ToLower());
        }
    }

    private void setTeamName(TextMeshProUGUI teamText, string team, int teamNeededCoins)
    {
        teamText.text = team.ToUpper();
    }

    private void setTeamLocked(TextMeshProUGUI teamText, string teamName, int teamNeededCoins)
    {
        if (teamNeededCoins > Globals.coins &&
            !PlayerPrefs.HasKey(teamName))
        {
            teamText.text = teamNeededCoins.ToString() + " COINS";
        }
        else
        {
            teamText.text = "";
        }
    }

    private void initPlayerCardReference()
    {
        playerCard = new GameObject[MAX_PLAYERS_CARDS];
        playerCardName = new TextMeshProUGUI[MAX_PLAYERS_CARDS];
        playerCardFlag = new RawImage[MAX_PLAYERS_CARDS];
        playerEnergySkillsBar = new Image[MAX_PLAYERS_CARDS];
        playerCardSkills = new TextMeshProUGUI[MAX_PLAYERS_CARDS];
        playerCardPlayerImg = new RawImage[MAX_PLAYERS_CARDS];
        playerCardPlayerStarImg = new GameObject[MAX_PLAYERS_CARDS];


        for (int i = 0; i < MAX_PLAYERS_CARDS; i++)
        {
            playerCard[i] = GameObject.Find(
                "playerCard" + (i + 1).ToString());            
            playerCardName[i] = GameObject.Find(
                "playerCardName" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();
            playerCardFlag[i] = GameObject.Find(
                "playerCardFlag" + (i + 1).ToString()).GetComponent<RawImage>();
            playerEnergySkillsBar[i] = GameObject.Find(
                "playerEnergySkillsBar" + (i + 1).ToString()).GetComponent<Image>();
            playerCardSkills[i] = GameObject.Find(
                "playerCardSkills" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();
            playerCardPlayerStarImg[i] = GameObject.Find(
                "playerCardPlayerStar" + (i + 1).ToString());
            playerCardPlayerImg[i] = GameObject.Find(
                "playerCardPlayerImg" + (i + 1).ToString()).GetComponent<RawImage>();
        }

        playerCard[MAX_PLAYERS_CARDS - 1].SetActive(false);
    }

    public void trainingYesButton()
    {
        Globals.isTrainingActive = true;        
        updateTeamBGlobalsSettings(currBTeamIdx);
        Globals.isGameSettingActive = false;


        //Debug.Log("Globals.isTrainingActive team Management " + Globals.isTrainingActive);


        if (Globals.PITCHTYPE.Equals("STREET"))
        {
            //Globals.stadiumNumber = 0;
            //Globals.commentatorStr = "NO";
            //loadGameScene();
            initChooseStadiumCanvas();
        }
        else
        {
            initChooseStadiumCanvas();
        }
    }

    public void trainingNoButton()
    {
        Globals.isTrainingActive = false;
        initChooseStadiumCanvas();
        //loadGameScene();
    }

    public void mainGameSettingsButtonPrev()
    {
    }

    public void mainGameSettingsButtonNextSave()
    {
        saveGlobalsSettingsToPrefab();
        showMainMenu();
    }

    public void onClickChooseStadium(int idx)
    {
        ///if (idx == 1 && Globals.coins < 6000)
        //{
        //    popUpNoCoins.SetActive(true);
         //   return;
        //}
     
        Globals.stadiumNumber = idx;

        loadGameScene();
        admobAdsScript.hideBanner();
    }

    public void showMainMenu()
    {
        showBanner();

        gameSettingsCanvas.SetActive(false);
        friendlyCanvas.SetActive(false);
        tournamentCanvas.SetActive(false);
        trainingCanvas.SetActive(false);
        mainGameSettingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    private bool showPromotion()
    {
        if (!Globals.adsEnable)
        {
            shopPromotionCanvas.SetActive(false);
            return false;
        }

        if (Globals.isMultiplayer ||
            (Globals.numGameOpened <= 1 && Globals.numMatchesInThisSession <= 2) ||
            (PlayerPrefs.HasKey("showRemoveAdsPromo") && Globals.numMatchesInThisSession % 3 != 0)) 
        {
        //if (PlayerPrefs.HasKey("showRemoveAdsPromo")) {
        //    if ((Globals.numGameOpened - PlayerPrefs.GetInt("showRemoveAdsPromo")) <= 5)
            //{
                shopPromotionCanvas.SetActive(false);
                return false;
            //}
        }

        PlayerPrefs.SetInt("showRemoveAdsPromo", Globals.numGameOpened);
        PlayerPrefs.Save();

        string removeAdsPrice =
            IAPManager.instance.getPriceByHash("removeAds");

        if (string.IsNullOrEmpty(removeAdsPrice))
        {
            shopPromotionCanvas.SetActive(false);
            return false;
        }

        shopPromotionCanvas.SetActive(true);
        shopPromotionBuyButtonText.text = removeAdsPrice;

        return true;
    }

    public void onClickShowStadiumWatchAd(string eventAction)
    {
        stadiumRewardAds.SetActive(true);

        stadiumChooseRewardAdsButton.onClick.RemoveAllListeners();
        stadiumChooseRewardAdsButton.onClick.AddListener(
                   delegate {
                       shopScript.watchRewardAdButton(eventAction);
                   });
        Debug.Log("button click " + eventAction);
    }

    public void onClickCloseStadiumWatchAds()
    {
        stadiumRewardAds.SetActive(false);
    }
               
    #region testCode
    int playerIdx = 0;

    public void onClickPrevPlayer() {

        if (playerIdx == 0)
            return;

        playerIdx--;

        string playerSkinHairDescA =
            teamsA.getPlayerDescByIndex(currATeamIdx, playerIdx);

        //print("DBGPLAYER IDX " + playerIdx + " DESC " + playerSkinHairDescA);

        graphicsStandard.setPlayerTextures(
                tournamentModelTeamA,
                tournamentModelTeamAhair,
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);
    }

    public void onClickMultiplayerTeamSettingsSave()
    {
        PlayerPrefs.SetInt("multiplayer_currTeamA", currATeamIdx);
        PlayerPrefs.SetInt("multiplayer_leagueAIdx", leagueAIdx);        
        PlayerPrefs.Save();

        print("saved currATeamIdx multiplayer " + currATeamIdx + " read " + PlayerPrefs.GetInt("multiplayer_currTeamA"));

        saveSelectedTeamsToGlobals(true);
        setupTeamDefaults();
        tournamentCanvas.SetActive(false);
    }

    public void onClickNextPlayer()
    {

        int playersNum = teamsA.getTeamByIndex(currATeamIdx)[12].Split('|').Length;
        if (playerIdx + 1 == playersNum)
            return;        

        playerIdx++;
        string playerSkinHairDescA =
            teamsA.getPlayerDescByIndex(currATeamIdx, playerIdx);

        //print("DBGPLAYER IDX " + playerIdx + " DESC " + playerSkinHairDescA);

        graphicsStandard.setPlayerTextures(
                tournamentModelTeamA,
                tournamentModelTeamAhair,
                currATeamIdx,
                leagueNames[leagueAIdx],
                playerSkinHairDescA,
                false,
                false,
                teamsA);

    }
    #endregion
}
