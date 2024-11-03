using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using MenuCustomNS;
using graphicsCommonNS;
using GlobalsNS;
using System;
using TMPro;
using UnityEngine.Analytics;
using System.IO;
using LANGUAGE_NS;

public enum MATCHTYPE
{
    LEAGUE = 1,
    CHAMPCUP = 2,
    LEAGUECUP = 3,
}

[Serializable]
public class MatchTableMain : MonoBehaviour
{
    private int MAX_COMPETITIONS = 3;
    private int numCompetitions = 3;
    private int MAX_CHAMP_LEAGUE_MATCHES = 10;
    private int MAX_LEAGUE_CUP_MATCHES = 9;
    private int MAX_WORLD_CUP_MATCHES = 7;
    private int plrTeamCupId = 7777;
    private int numButtonsUp = 3;
    private int MAX_BUTTONS_UP = 3;
    private int activeGroup = 1;
    private int stage = 1;
    public Button playButton;
    public Button leagueButton;
    public Button leagueCupButton;
    public Button champLeagueButton;
    private Text playMatchText;
    private GameObject admobCanvas;
    private GameObject panelBigBackground;

    private LeagueBackgroundMusic leagueBackgroundMusic;

    private GameObject infoPanel;
    private TextMeshProUGUI infoPanelHeaderText;
    private TextMeshProUGUI infoText;
    private TextMeshProUGUI infoPanelExitButtonText;
    private GameObject infoPanelExitButtonGameObj;
    private Button infoPanelExitButton;
    private GameObject cannotLoadSavePanel;

    private GameObject infoPanelNoBigBackground;
    private TextMeshProUGUI infoPanelNoBackgroundHeaderText;
    private TextMeshProUGUI infoPanelNoBackgroundText;

    public int[] teamsPerGroup;
    private int numGroups = 3;
    private int tableColumns = 11;
    //private bool NumUpdateLastResult = false;
    private int END_CURRENT_GAME = 3;
    private int GAME_FINISH = 1000;
    // number team_name nMatches pkt goals main_id Strength speed 
    private int GROUP_MATCH = 3;
    private string[][][] groups;
    //public List<string> groupsSerialize;
    //public List groupsSerialize;
    private string[][][] sortedGroups;
    private TextMeshProUGUI[,] textTableGroup;
    private GameObject[] groupResultRow;
    private TextMeshProUGUI[,] textLastResult;
    private TextMeshProUGUI[,] textNextMatches;
    private RawImage[] groupResultFlag;
    private RawImage[,] lastResultFlag;
    private RawImage[,] nextMatchesFlag;
    private int groupTableNrCols = 8;
    private int groupTableNrRows = 20;
    //private int lastResultsNrRows = 4;
    private int lastResultsNrRows = 10;
    private int lastResultsNrCols = 4;
    //private int nextMatchesNrRows = 2;
    private int nextMatchesNrRows = 10;
    private int nextMatchesNrCols = 2;
    private int group, teamA, teamB;
    //private string[,] games;
    public List<string>[] games;
    //public List<string> gamesSerialize;
    //private int[] gamesIdx;
    //private int[] gamesElementN;
    private System.Random rand;
    private int currIdxMatch = 0;
    private int currIdxGroup = 1;
    //private NationalTeams teams;
    //private NationalTeams orgTeams;
    private Teams[] teams;
    private Teams[] orgTeams;
    private bool initPlrGroupStage = false;
    private int matchesInGroup = 0;
    private int nMatches = 0;
    private bool isKnockOutStage = false;
    private Button playNextMatch;
    private Button mainMenu;
    //private int headLastResult = 0;
    //private int tailLastResult = 0;
    //private string[] lastResults;
    private string[,] lastResults;
    public int[] lastResultsNumElem;
    public int[] headLastResult;
    public int[] tailLastResult;
    public bool[] numUpdateLastResult;

    private TextMeshProUGUI playerNextMatchText;
    private TextMeshProUGUI seasonNumText;
    private int leagueWeek = 1;
    private int leagueSeason = 20;

    private int MAX_LAST_RESULTS = 20;
    //private TextView[][] resTableCol;
    //private TextView[][] lastNextEvents;
    private int numRounds = 4;
    private bool lose = false;
    private bool endOfTheGame = false;
    //private TextView lastResultText;
    //private TextView nextMatchesText;
    //private TextView stage;
    //private int maxCupRounds;
    //private GameSounds sounds;
    private int width, height;
    private float widthRate, heightRate;
    private static int baseWidthRes = 1080;
    private static int baseHeightRes = 1920;
    private Dictionary<string, Texture2D> flagsHashMap;
    private string[] playerTeamDesc;

    /*TOCHANGE*/
    private Dictionary<string, string> groupResultsMap;
    private string player1TeamName;
    private int Player1PosinGroup = -1;
    private int playerTeamID;
    private int[] playerPosInGroup;
    private string level;
    private string timeOfGame;
    private int TEAM1_ID;
    private int TEAM2_ID;
    private string playerGameSaved;
    private GraphicsCommon graphics;
    private bool playNextMatchActive;
    private GameObject pauseCanvas;
    private MatchTableMainSerialize matchTableSerialize;
    private int[] teamQualifiedNum;
    private string[] achievement;
    private string[] gameCategory;
    private int isNewSeason = 0;

    private string[] cupStages = {
              "Round of 64",
              "Round of 32",
              "Round of 16",
              "Quarter-final",
              "Semi-final",
              "Final",
              };

    private string[] leaguesNames = {
              "",
              "League",
              "Champ Cup",
              "League Cup",
              };

    //private string[] leaguesGlobalNames = {
    //          "",
    //          "",
    //          "Champ Cup",
    //          "",
    //          };


    private int stageID = 0;
    public static MatchTableMain matchTableInstance = null;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    private bool waitingForAddEvent = false;
    private AudioSource audioClip;
    private bool initAds = false;
    private bool finalWon = false;

    /*SHOP*/
    //public Button shopOfferButton;
    public Button goalEnlargeBuyButton;
    public Button shopCloseButton;
    public GameObject shopPanel;
    public GameObject shopNotificationCanvas;
    public Button shopNotificationCloseButton;
    public TextMeshProUGUI enlargeGoalSizePriceTextFree;
    public Text enlargeGoalSizePriceText;
    private string leagueName;
    private string savedFileName = "";
    private int nMatchesLeague = 0;

    private int[] numMatches;
    private int[] numMatchesInGroup;
    private int[] nMatchesPlayed;

    private List<MATCHTYPE> matchesSchedule;
    private bool[] isLeagueActive;
    private ScrollRect resultScroll;

    private GameObject[] mainButtonsUpGameObj;
    private GameObject[] mainButtonsFocus;
    private TextMeshProUGUI[] mainButtonText;

    private string savedGamesFileName;
    private bool dontUpdateScore = false;

    private bool isChampCupActive = true;
    private bool nextSeasonChampCup = true;

    void Awake()
    {

        //print("groupisGameSettingActive " + Globals.isGameSettingActive);

        admobGameObject = GameObject.Find("admobAdsGameObject");
        print("#DBG1354 admob " +  GameObject.Find("admobAdsGameObject"));

        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

        /*TOREMOVE*/
        //updateTemporarySettings();

        //print("matchTableInstance " + matchTableInstance);

        if (matchTableInstance == null)
        {
            matchTableInstance = this;      
            DontDestroyOnLoad(gameObject);

            playButton.onClick.AddListener(matchTableInstance.playOnClickEvent);
            leagueButton.onClick.AddListener(matchTableInstance.onClickLeague);
            leagueCupButton.onClick.AddListener(matchTableInstance.onClickLeagueCup);
            champLeagueButton.onClick.AddListener(matchTableInstance.onClickChampLeague);

            //shopOfferButton.onClick.AddListener(matchTableInstance.OnClickEventShowGoalResizePanel);
            goalEnlargeBuyButton.onClick.AddListener(matchTableInstance.OnClickGoalEnlargeBuyButton);
            shopNotificationCloseButton.onClick.AddListener(matchTableInstance.onClickShopNotificationClose);
            shopCloseButton.onClick.AddListener(matchTableInstance.onClickEventCloseShopPanel);

            matchTableInstance.initialization();
              
            //string json = JsonUtility.ToJson(this);
            //print("JSON init saved " + json);
        }
        else if (matchTableInstance != this)
        {
            playButton.onClick.AddListener(matchTableInstance.playOnClickEvent);
            leagueButton.onClick.AddListener(matchTableInstance.onClickLeague);
            leagueCupButton.onClick.AddListener(matchTableInstance.onClickLeagueCup);
            champLeagueButton.onClick.AddListener(matchTableInstance.onClickChampLeague);

            //shopOfferButton.onClick.AddListener(matchTableInstance.OnClickEventShowGoalResizePanel);
            goalEnlargeBuyButton.onClick.AddListener(matchTableInstance.OnClickGoalEnlargeBuyButton);
            shopNotificationCloseButton.onClick.AddListener(matchTableInstance.onClickShopNotificationClose);
            shopCloseButton.onClick.AddListener(matchTableInstance.onClickEventCloseShopPanel);

            Destroy(this.gameObject);

            matchTableInstance.initAdsCanvas();
            matchTableInstance.setReferenceToCanvasTextObjects();
            matchTableInstance.setReferenceToCanvasGameObject();
            matchTableInstance.setReferenceToCanvasRawImageObjects();
            matchTableInstance.setReferenceToScrollsObject();
            matchTableInstance.setReferenceOthers();
            //matchTableInstance.setShopActivitySettings();
            matchTableInstance.clearResTable();

            //matchTableInstance.initCupPanels();
            matchTableInstance.initPauseCanvas();
            matchTableInstance.updateButtonUpText();
            matchTableInstance.prepareGameSettings();
            matchTableInstance.onActivityResult();
            matchTableInstance.initAds = false;

            /*if (!GroupTableInstance.isKnockOutStage ||
                (GroupTableInstance.isKnockOutStage &&
                 GroupTableInstance.currIdxMatch <= GroupTableInstance.nMatches))
            {
                GroupTableInstance.displayAds();
            }

            if (!waitingForAddEvent)
                GroupTableInstance.initAudioClip();*/
            //matchTableInstance.disableUpButtonFocusApartFrom(0);
            matchTableInstance.saveGameState();

            return;
        }
    }

    void Update()
    {
        //print("UPDATE GROUPTABLE SceneManager.GetActiveScene().name " + SceneManager.GetActiveScene().name);
        if (!SceneManager.GetActiveScene().name.Equals("Leagues"))
            return;

        if (!matchTableInstance.initAds &&
            !dontUpdateScore)
        {
            //if (!matchTableInstance.isKnockOutStage ||
            //  (matchTableInstance.currIdxMatch <= matchTableInstance.nMatches
            //    && matchTableInstance.finalWon == false))
            //      matchTableInstance.displayAds();

                if (matchTableInstance.finalWon == false)
                {
                    matchTableInstance.displayAds();
                }

                //print("#DBGINIT AUDIO FIRED WAITING FOR ADD EVENT " + waitingForAddEvent);
            if (!waitingForAddEvent)
                matchTableInstance.initAudioClip();
            matchTableInstance.initAds = true;
        }

        if (Globals.adsEnable &&
            waitingForAddEvent &&
            (admobAdsScript.getAdsClosed() ||
             admobAdsScript.getAdsFailed()))
        {
            waitingForAddEvent = false;
            matchTableInstance.admobCanvas.SetActive(false);
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
            initPauseCanvas();
            matchTableInstance.initAudioClip();
        }

        /*if (Globals.purchasesQueue.Count > 0 &&
            (shopNotificationCanvas != null &&
            !shopNotificationCanvas.activeSelf))
        {
            shopNotificationCanvas.SetActive(true);
            showNotification(
                Globals.purchasesQueue.Dequeue());
            initPauseCanvas();
        }*/
    }

    private void prepareGameSettings()
    {
        if (leagueName.Contains("CUP"))
        {
            numCompetitions = 1;
            numButtonsUp = 1;
            numGroups = 1;
            savedGamesFileName = "savedGames_CUP";
        } else
        {
            savedGamesFileName = "savedGames_LEAGUES";
        }

        mainButtonsUpGameObj = new GameObject[MAX_BUTTONS_UP];
        for (int i = 1; i <= MAX_BUTTONS_UP; i++)
        {
            mainButtonsUpGameObj[i - 1] =
                GameObject.Find("buttonUp" + i.ToString());
        }

        //print("mainButtonsUpGameObj " + numButtonsUp);
        for (int i = numButtonsUp; i < MAX_COMPETITIONS; i++)
        {
            mainButtonsUpGameObj[i].SetActive(false);
            //print("DBG DISABLEUPBUTTON " + i);
        }
    }

    private void hideBannerAds()
    {
        if (admobGameObject != null &&
            admobAdsScript != null)
        {
            admobAdsScript.hideBanner();
        }
    }

    private void displayAds()
    {
        if (admobGameObject != null &&
            admobAdsScript != null &&
            Globals.adsEnable)
            //Globals.numMatchesInThisSession > 0)
        {
            if (admobAdsScript.showInterstitialAd())
            {
                waitingForAddEvent = true;
                admobCanvas.SetActive(true);
            }
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
        }

        admobAdsScript.hideBanner();
        //print("Globals.numMatchesInThisSession GROUPTABLE " + Globals.numMatchesInThisSession);
    }

    private void initAudioClip()
    {
        leagueBackgroundMusic =
            GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
        leagueBackgroundMusic.play();

        //print("#DBGINIT AUDIO FIRED ");
        /*
        audioClip = GameObject.Find("leagueBackgroundMusic").GetComponent<AudioSource>();
        audioClip.volume = 0.5f;
        if (Globals.audioMute)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = 1f;
        audioClip.Play();*/
    }

    //private void initCupPanels()
    //{
        //cupPanel = GameObject.Find("cupPanel");
    //    panelBigBackground = GameObject.Find("panelBigBackground");
    //    playMatchText = GameObject.Find("nextMatch").GetComponent<Text>();
        //cupPanelCoinsRewarded = GameObject.Find("cupPanelCoinsRewarded").GetComponent<Text>();
        //cupPanel.SetActive(false);
    //}

    private void initPauseCanvas()
    {
        pauseCanvas = GameObject.Find("pauseCanvas");
        Time.timeScale = 1f;
        if (Globals.audioMute)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = 1f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    private void initAdsCanvas()
    {
        admobCanvas = GameObject.Find("admobCanvas");
        admobCanvas.SetActive(false);
    }

    private void fillReferencesTextOnCanvas(TextMeshProUGUI[,] arr, 
                                            string name,
                                            int row, 
                                            int col)
    {

        /* for (int i = 0; i < row; i++)
         {
             for (int j = 0; j < col; j++)
             {
                 string textName = name + i.ToString() + "_" + j.ToString();
                 print("NAME OF TEXT " + textName);
                 arr[i, j] = GameObject.Find(textName).GetComponent<Text>();
             }
         }*/

        for (int i = 0; i < row; i++)
        {     
            GameObject rowObj = GameObject.Find(name + i.ToString());
            //print("objName " + name + i.ToString());

            foreach (Transform obj in rowObj.GetComponentsInChildren<Transform>())
            {
                if (obj.name.Contains("Flag"))
                {
                    continue;
                }

                TextMeshProUGUI text =
                        obj.gameObject.GetComponent<TextMeshProUGUI>();
                //print("###ROW " + i + " " + text);
                               
                switch (obj.name)
                {
                    case "index":
                    case "goalTeamALast":
                    case "teamANameNext":
                        arr[i, 0] = text;
                        //print("set index " + arr[i, 0]);
                        break;

                    case "teamANameLast":
                    case "teamNameRes":
                    case "teamBNameNext":
                        arr[i, 1] = text;
                        break;

                    case "teamBNameLast":
                    case "numMatches":
                        arr[i, 2] = text;
                        break;

                    case "goalTeamBLast":
                    case "numPoints":
                        arr[i, 3] = text;
                        break;

                    case "goalDiff":
                        arr[i, 4] = text;
                        break;
                    case "nWins":
                        arr[i, 5] = text;
                        break;            
                    case "nDraw":
                        arr[i, 6] = text;
                        break;
                    case "nLost":
                        arr[i, 7] = text;
                        break;
                }

                //if (obj.name == "countryFlag")
                //{
                //    arr[i, j] 
                    //RawImage clubFlag =
                    //    obj.gameObject.GetComponent<RawImage>();
                    //graphics.setFlagRawImage(clubFlag, countryName);
                //}
            }
        }
    }

    private void fillReferencesFlagOnCanvas(RawImage[,] arr, 
                                            string name, 
                                            int row, 
                                            int col)
    {

        for (int i = 0; i < row; i++)
        {
            GameObject rowObj = GameObject.Find(name + i.ToString());
            foreach (Transform obj in rowObj.GetComponentsInChildren<Transform>())
            {
                if (!obj.name.Contains("Flag"))
                    continue;

                RawImage flagImg = 
                    obj.gameObject.GetComponent<RawImage>();

                switch (obj.name)
                {
                    case "teamFlag":
                    case "teamAFlag":     
                        arr[i, 0] = flagImg;
                        break;

                    case "teamBFlag":
                        arr[i, 1] = flagImg;
                        break;                
                }
            }
        }

        //for (int i = 0; i < row; i++)
        //{
        //    for (int j = 0; j < col; j++)
        //    {
        //        string textName = name + i.ToString() + "_" + j.ToString();
        //        arr[i, j] = GameObject.Find(textName).GetComponent<RawImage>();
        //    }
        //}
    }

    private void setReferenceToCanvasGameObject()
    {
        for (int i = 0; i <= groupTableNrRows; i++)
        {
            groupResultRow[i] = 
                GameObject.Find("groupResultRow" + i.ToString());            
        }
    }

    private void setReferenceToCanvasTextObjects()
    {
        //fillReferencesTextOnCanvas(textTableGroup, "groupResultText_", teamsPerGroup + 1, groupTableNrCols);
        //fillReferencesTextOnCanvas
        //    (textTableGroup, "groupResultText_", groupTableNrRows + 1, groupTableNrCols);
        fillReferencesTextOnCanvas
            (textTableGroup, "groupResultRow", groupTableNrRows + 1, groupTableNrCols);
        //fillReferencesTextOnCanvas
        //    (textLastResult, "LastResultsText_", lastResultsNrRows, lastResultsNrCols);
        fillReferencesTextOnCanvas
            (textLastResult, "groupLastTextRow", lastResultsNrRows, lastResultsNrCols);
        //fillReferencesTextOnCanvas
        //    (textNextMatches, "NextMatchText_", nextMatchesNrRows, nextMatchesNrCols);
        fillReferencesTextOnCanvas
            (textNextMatches, "groupNextTextRow", nextMatchesNrRows, nextMatchesNrCols);
        
        playerNextMatchText = GameObject.Find("playerNextMatchText").GetComponent<TextMeshProUGUI>();
        seasonNumText = GameObject.Find("seasonNumText").GetComponent<TextMeshProUGUI>();

        for (int i = 1; i <= numButtonsUp; i++)
        {
            mainButtonsFocus[i - 1] =
                GameObject.Find("mainButtonFocus" + i.ToString());
            mainButtonsUpGameObj[i - 1] =
                GameObject.Find("buttonUp" + i.ToString());
            mainButtonText[i - 1] =
                GameObject.Find("mainButtonText" + i.ToString()).GetComponent<TextMeshProUGUI>();       
        }
    }

    private void setReferenceToScrollsObject()
    {
        resultScroll =
            GameObject.Find("scrollResult").GetComponent<ScrollRect>();
    }

    private void setReferenceOthers()
    {
        infoPanel = 
            GameObject.Find("infoPanel");
        infoText = 
            GameObject.Find("infoText").GetComponent<TextMeshProUGUI>();
        infoPanelHeaderText =
            GameObject.Find("infoPanelHeaderText").GetComponent<TextMeshProUGUI>();
        infoPanelExitButtonGameObj =
            GameObject.Find("infoPanelExitButton");
        infoPanelExitButton =
            infoPanelExitButtonGameObj.GetComponent<Button>();
        infoPanelExitButtonText =
            GameObject.Find("infoPanelExitButtonText").GetComponent<TextMeshProUGUI>();

        cannotLoadSavePanel = 
            GameObject.Find("cannotLoadSavePanel");
        cannotLoadSavePanel.SetActive(false);

        infoPanelNoBigBackground =
            GameObject.Find("infoPanelNoBigBackground");
        infoPanelNoBackgroundHeaderText =
            GameObject.Find("infoPanelNoBackgroundHeaderText").GetComponent<TextMeshProUGUI>();
        infoPanelNoBackgroundText =
            GameObject.Find("infoPanelNoBackgroundText").GetComponent<TextMeshProUGUI>();
    
        infoPanel.SetActive(false);
        infoPanelNoBigBackground.SetActive(false);
    }

    private void setReferenceToCanvasRawImageObjects()
    {
        //for (int row = 1; row <= teamsPerGroup; row++)
        for (int row = 1; row <= groupTableNrRows; row++)
        {
            GameObject rowObj = GameObject.Find("groupResultRow" + row.ToString());
            foreach (Transform obj in rowObj.GetComponentsInChildren<Transform>())
            {
                if (obj.name.Equals("teamFlag"))
                    groupResultFlag[row] = obj.gameObject.GetComponent<RawImage>();
            }

           //     string rawImageName = "groupResultFlag_" + row.ToString();         
            //groupResultFlag[row] = 
            //    GameObject.Find(rawImageName).GetComponent<RawImage>();         
        }

        //fillReferencesFlagOnCanvas(lastResultFlag, "LastResultsFlag_", lastResultsNrRows, 2);
        //fillReferencesFlagOnCanvas(nextMatchesFlag, "NextMatchesFlag_", nextMatchesNrRows, 2);

        fillReferencesFlagOnCanvas(lastResultFlag, "groupLastTextRow", lastResultsNrRows, 2);
        fillReferencesFlagOnCanvas(nextMatchesFlag, "groupNextTextRow", nextMatchesNrRows, 2);

    }

    /*private void setShopActivitySettings()
    {   
        shopPanel = GameObject.Find("shopPanel");
        shopNotificationCanvas =
            GameObject.Find("shopNotificationCanvas");
        enlargeGoalSizePriceTextFree =
            GameObject.Find("enlargeGoalSizePriceTextFree").GetComponent<TextMeshProUGUI>();
        enlargeGoalSizePriceText =
            GameObject.Find("enlargeGoalSizePriceText").GetComponent<Text>();

        int enalrgeGoalMediumCreditsNum =
            PlayerPrefs.GetInt("enlargeGoal_MEDIUM_CREDITS");

        if ((PlayerPrefs.HasKey("enlargeGoal_MEDIUM_CREDITS") &&
            enalrgeGoalMediumCreditsNum > 0))
        {
            enlargeGoalSizePriceTextFree.text = "CREDITS: " + enalrgeGoalMediumCreditsNum.ToString() + "\nFREE";
        }
        else
        {
            enlargeGoalSizePriceText.text =
            IAPManager.instance.getPriceByHash("enlargegoalsize_medium");
        }

        shopPanel.SetActive(false);
        shopNotificationCanvas.SetActive(false);
    }*/

    private int printPlayerNextMatch()
    {
        //print("NEXTMATCH__PLAYER ENTERED " + matchesSchedule.Count);

        for (int i = 0; i < matchesSchedule.Count; i++)
        {
            int gameType = (int) matchesSchedule[i];
            if (!isLeagueActive[gameType])
                continue;

            for (int j = 0; j < games[gameType].Count; j++)
            {
                string eventLine = games[gameType][j];
                //string[] teamsIdx = games[gameType, i].Split('-');
                string[] match = eventLine.Split('|');
                string[] teamsIdx = match[0].Split('-');
                string matchType = match[1];
                //print("NEXT MATCH EVENT LINE " + eventLine + " GAMETYPE " + gameType);

                if (match[0].Equals("0-0"))
                {
                    break;
                }

                int teamAIdx = int.Parse(teamsIdx[0]);
                int teamBIdx = int.Parse(teamsIdx[1]);

                string matchTypeName = " - " + match[2];
                if (String.IsNullOrEmpty(match[2]))
                {
                    matchTypeName = "";
                }

                if (teamAIdx == 1 ||
                    teamBIdx == 1 ||
                    teamAIdx == plrTeamCupId ||
                    teamBIdx == plrTeamCupId)
                {
                    string teamAname = "";
                    string teamBname = "";

                    getTeamsNamesByIdx(gameType, 
                                       eventLine, 
                                       ref teamAname, 
                                       ref teamBname);

                    //print("NEXTMATCH__PLAYER " + teamAname + " vs " + teamBname + " # ");

                    string leagueWeekDesc = Languages.getTranslate("Week "
                                                                   + leagueWeek.ToString()
                                                                   + " - ",
                                                                   new List<string>() { leagueWeek.ToString() });

                    print("DBGLANG leagueWeek " + leagueWeekDesc);

                    if (numCompetitions == 1)
                        leagueWeekDesc = "";

                    playerNextMatchText.text = 
                        leagueWeekDesc
                        + Languages.getTranslate(leaguesNames[gameType])
                        + " "
                        + matchTypeName
                        + "\n"
                        + teamAname 
                        + " vs "
                        + teamBname;

                        seasonNumText.text = 
                        leagueSeason.ToString() + "/" + (leagueSeason + 1).ToString();
                    return gameType;
                }
            }
        }

        return 1;
    }

    private void getTeamsNamesByIdx(int group,
                                    string eventLine,
                                    ref string teamAname, 
                                    ref string teamBname)
    {
        //string teamAname;
        //string teamBname;

        string[] match = eventLine.Split('|');
        string[] teamsIdx = match[0].Split('-');
        string matchType = match[1];

        if (match[0].Equals("0-0"))
        {
            return;
        }

        string[] teams = match[0].Split('-');
        int teamAIdx = int.Parse(teams[0]);
        int teamBIdx = int.Parse(teams[1]);

        if (matchType.Equals("C"))
        {
            if (teamAIdx == plrTeamCupId)
            {
                teamAname = player1TeamName;
                teamBname =
                        orgTeams[group].getTeamByIndex(teamBIdx)[0];
            }
            else if (teamBIdx == plrTeamCupId)
            {
                teamBname = player1TeamName;
                teamAname =
                   orgTeams[group].getTeamByIndex(teamAIdx)[0];
            } else
            {
                teamAname = orgTeams[group].getTeamByIndex(teamAIdx)[0];
                teamBname = orgTeams[group].getTeamByIndex(teamAIdx)[0];
            }
        }
        else
        {
            teamAname = groups[group][teamAIdx][1];
            teamBname = groups[group][teamBIdx][1];
        }
    }

    private void initTeams()
    {
        matchesSchedule = new List<MATCHTYPE>();
        teamsPerGroup = new int[numCompetitions + 1];
        isLeagueActive = new bool[numCompetitions + 1];
        playerPosInGroup = new int[numCompetitions + 1];

        for (int i = 0; i < isLeagueActive.Length; i++)
            isLeagueActive[i] = true;

        if (!isChampCupActive)
        {
            isLeagueActive[(int) MATCHTYPE.CHAMPCUP] = false;
        }

        //teams = new Teams[MAX_COMPETITIONS + 1];
        //orgTeams = new Teams[MAX_COMPETITIONS + 1];
        teams = new Teams[numCompetitions + 1];
        orgTeams = new Teams[numCompetitions + 1];

        for (int i = 1; i <= numCompetitions; i++) {
            if (i == 2) {
                teams[i] = new Teams("CHAMP CUP");
                orgTeams[i] = new Teams("CHAMP CUP");
            } else
            {
                teams[i] = new Teams(leagueName);
                orgTeams[i] = new Teams(leagueName);

                //print("CHAMP CUP ID " + leagueName);
            }
        }

        print("NUMCOMP " + numCompetitions + " leagueName " + leagueName);

        /*this is league cup*/
        //teams[3] = new Teams(leagueName);
        //orgTeams[3] = new Teams(leagueName);

        for (int i = 1; i <= numCompetitions; i++)
        {
            if (i > (int) MATCHTYPE.LEAGUE ||
                numCompetitions == 1)
            {
                teamsPerGroup[i] = 4;
                lastResultsNumElem[i] = 5;
            }
            else
            {
                teamsPerGroup[i] = teams[i].getMaxActiveTeams();
                print("TEAMSPERGROUPS MATCHTABLE " + teamsPerGroup[i]);
                //print("LastyresultnumEl " + i + " lastResultsNumElem " + lastResultsNumElem[i]);
                lastResultsNumElem[i] = (teams[i].getMaxActiveTeams() / 2) + 1;
            }
            //print("TEAMSPERGROUP " + i);
        }

        //remove player team from all group if exists
        //print("REMOVE TEAM NAME " + player1TeamName);
        //for (int group = 1; group <= numCompetitions; group++)
        //{
        //    int idx = teams[group].getTeamIndexByName(player1TeamName);
        //    if (idx != -1)
         //   {
         //       teams[group].swapElements(idx, teams[group].getMaxActiveTeams() - 1);
         //   }
        //}
    }

    private void initialization()
    {
        isChampCupActive = Globals.champCupPromotedNextSeason;
        nextSeasonChampCup = true;
        leagueName = Globals.leagueName;
        player1TeamName = Globals.playerTeamName;
        playerTeamID = Globals.playerTeamId;
        leagueSeason = Globals.leagueSeason;

        prepareGameSettings();

        if (Globals.isNewGame)
        {
            savedFileName = findNameOfSaved(leagueName);
            Globals.savedFileName = savedFileName;
        }
        else
        {
            savedFileName = Globals.savedFileName;
        }
      
        if (PlayerPrefs.HasKey(savedFileName + "_isNewSeason"))
        {
            isNewSeason = PlayerPrefs.GetInt(savedFileName + "_isNewSeason");
        }


            /*print("#DBGPLAYER1TEAMNAME_TEST1 " + player1TeamName
               + " TEAMID " + playerTeamID + " leagueName " + leagueName +
               " leagueSeason " + leagueSeason +
               " isChampCupActive " + isChampCupActive
               + " isNewSeason " + isNewSeason
               + " doesHasExist " + PlayerPrefs.HasKey(savedFileName + "_isNewSeason"));*/

        matchTableSerialize = new MatchTableMainSerialize(savedFileName,
                                                          numCompetitions);

        initAds = false;
        lastResultsNumElem = new int[numCompetitions + 1];

        initTeams();
               
        graphics = new GraphicsCommon();
        flagsHashMap = new Dictionary<string, Texture2D>();
        groupResultsMap = new Dictionary<string, string>();

        mainButtonsFocus = new GameObject[numButtonsUp];
        mainButtonText = new TextMeshProUGUI[numButtonsUp];

        //print("TEAMCHOOSEN# " + Globals.teamAname + " ID " + Globals.teamAid);
  
        initAudioClip();
        hideBannerAds();

        //textTableGroup = new Text[teamsPerGroup + 1, groupTableNrCols];
        textTableGroup = new TextMeshProUGUI[groupTableNrRows + 1, groupTableNrCols];
        groupResultRow = new GameObject[groupTableNrRows + 1];
        textLastResult = new TextMeshProUGUI[lastResultsNrRows, lastResultsNrCols];
        textNextMatches = new TextMeshProUGUI[nextMatchesNrRows, nextMatchesNrCols];

        //groupResultFlag = new RawImage[teamsPerGroup + 1];
        groupResultFlag = new RawImage[groupTableNrRows + 1];
        lastResultFlag = new RawImage[lastResultsNrRows, 2];
        nextMatchesFlag = new RawImage[nextMatchesNrRows, 2];

        setReferenceToCanvasGameObject();
        setReferenceToCanvasTextObjects();
        setReferenceToCanvasRawImageObjects();
        setReferenceToScrollsObject();
        setReferenceOthers();
        //setShopActivitySettings();
        clearResTable();
        disableUpButtonFocusApartFrom(0);

        initPauseCanvas();
        initAdsCanvas();

        //maxCupRounds = 5;
        numRounds = 4;
        matchesInGroup = 3;
        lose = false;
        endOfTheGame = false;
        stageID = 0;
        isKnockOutStage = false;
        //bool nationals = getIntent().getboolExtra("NATIONAL_TEAMS", true);

        //teamsPerGroup = 4;
        //teams = new NationalTeams();
        //orgTeams = new NationalTeams();
        //teams = new Teams("Spanish");
        //orgTeams = new Teams("Spanish");
        //teamsPerGroup = teams.getMaxTeams();
        //teamsPerGroup = 4;
        create3dGroupArray(numCompetitions);
        ////sortedGroups = create2DArray<string>(teamsPerGroup, tableColumns);

        //lastResults = new string[MAX_LAST_RESULTS];
        lastResults = new string[numCompetitions + 1, MAX_LAST_RESULTS];
        headLastResult = new int[numCompetitions + 1];
        tailLastResult = new int[numCompetitions + 1];
        numUpdateLastResult = new bool[numCompetitions + 1];
        teamQualifiedNum = new int[numCompetitions + 1];
        achievement = new string[numCompetitions + 1];
        gameCategory = new string[numCompetitions + 1];

        teamQualifiedNum[1] = 3;
        if (numCompetitions == 1)
        {
            teamQualifiedNum[1] = 2;
            gameCategory[1] = "CUP";
        }

        if (numCompetitions > 1)
        {
            gameCategory[1] = "LEAGUE";
            gameCategory[2] = gameCategory[3] = "CUP";
            teamQualifiedNum[2] = 2;
            teamQualifiedNum[3] = 2;
        }

        for (int i = 0; i < numCompetitions + 1; i++)
        {
            numUpdateLastResult[i] = false;
            headLastResult[i] = 0;
            tailLastResult[i] = 0;
        }
        //NumUpdateLastResult = false;

        //if (teamsPerGroup * numGroups > teams.getMaxTeams())
        //    print("Not enough teams to create a groups");

        fillHeader();
        /*TOCHECK*/
        //printGroupCanvasHeader();
        fillGroups();
        //printStage();

        //printResTable((2 * teamsPerGroup) + 2, tableColumns - 2);
        
        currIdxMatch = 0;
        currIdxGroup = 1;

        //float fontSizeLevelTimeOfGame = 50.0f / this.heightRate;
        //nMatches = teamsPerGroup * (teamsPerGroup - 1) / 2;
        scheduleMatches();

        if (!Globals.isNewGame &&
            isNewSeason != 1)
            //!Globals.isNewSeason)
        {
            matchTableInstance.loadGameFromSave();
            //player1TeamName = Globals.teamAname;
        }
        else
        {
            Globals.isNewSeason = false;
            matchTableInstance.saveGameState();
            isNewSeason = 0;
            PlayerPrefs.SetInt(savedFileName + "_isNewSeason", 0);
            PlayerPrefs.Save();
        }

        printResTable((2 * teamsPerGroup[activeGroup]) + 2, tableColumns - 2, activeGroup);
        
        printNextMatches(activeGroup);
        printLastRes(activeGroup);

       
        //disableUpButtonFocusApartFrom(0);

        updateButtonUpText();

        int currentGroupMatch = printPlayerNextMatch();
        //disableUpButtonFocusApartFrom(currentGroupMatch - 1);
        disableUpButtonFocusApartFrom(0);

        //GameObject.Find("scrollResult").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        //GameObject.Find("scrollResult").GetComponent<ScrollRect>().normalizedPosition = 
        //    new Vector2(0,0.5f);
        //GameObject.Find("scrollResult").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;                  
    }

    private T[][] create2DArray<T>(int rows, int cols)
    {
        T[][] array = new T[rows][];
        for (int i = 0; i < array.GetLength(0); i++)
            array[i] = new T[cols];

        return array;
    }

    private void create3dGroupArray(int numGroups)
    {
        /*TODO CREATE GROUPS FOR ALL*/
        //groups = new String[numGroups + 1][teamsPerGroup + maxCupRounds][tableColumns];
        groups = new string[numGroups + 1][][];
        for (int i = 0; i < numGroups + 1; i++)
        {
            //print("CREATE GROUP " + teamsPerGroup);
            //string[][] row = new string[teamsPerGroup[i] + maxCupRounds][];
            string[][] row = new string[teamsPerGroup[i] + 1][];
            //for (int j = 0; j < tableColumns; j++)
            for (int j = 0; j <= teamsPerGroup[i]; j++)
            {
                string[] col = new string[tableColumns];
                // fill values for block
                row[j] = col;
            }
            groups[i] = row;
        }
     
        sortedGroups = new string[numGroups + 1][][];
        for (int i = 0; i < numGroups + 1; i++)
        {
            string[][] row = new string[teamsPerGroup[i]][];
            //for (int j = 0; j < tableColumns; j++)
            for (int j = 0; j < teamsPerGroup[i]; j++)
            {
                string[] col = new string[tableColumns];
                // fill values for block
                row[j] = col;
            }
            sortedGroups[i] = row;
        }
    }

    private void scheduleMatches()
    {
        int idxMatches = 0;
        int nRoundLeague = 2;

        numMatches = new int[numCompetitions + 1];
        numMatchesInGroup = new int[numCompetitions + 1];
        nMatchesPlayed = new int[numCompetitions + 1];

        for (int i = 0; i < nMatchesPlayed.Length; i++)
            nMatchesPlayed[i] = 0;

        if (numCompetitions == 1) {
            if (!leagueName.Equals("CHAMP CUP"))
            {
                nRoundLeague = 1;
            }
        }

        games = new List<string>[numCompetitions + 1];
        for (int i = 0; i < games.Length; i++)
        {
            games[i] = new List<string>();
        }

        ScheduleMatchesNew matchesLeague =
                 new ScheduleMatchesNew(teamsPerGroup[1], nRoundLeague);
        nMatchesLeague = (teamsPerGroup[1] - 1) * nRoundLeague;
        numMatches[1] = nMatchesLeague;
        numMatchesInGroup[1] = nMatchesLeague;

        List<string> schedule =
          matchesLeague.getListOfMatches();

        foreach (string match in schedule)
        {
            if (gameCategory[1].Equals("CUP"))
                games[1].Add(match + "|G|Group stage");
            else
                games[1].Add(match + "|G|");
        }

        //only cup then
        if (numCompetitions == 1)
        {
            drawCup("Round of 16", 1, MATCHTYPE.LEAGUE);
            numMatches[1] += 4;
            //leaguesGlobalNames[1] = leagueName;
            mainButtonText[0].text = leagueName;
        }

        if (numCompetitions > 1)
        {
            if (isChampCupActive)
            {
                ScheduleMatchesNew matchesChampCup =
                    new ScheduleMatchesNew(teamsPerGroup[2], 2);

                schedule = matchesChampCup.getListOfMatches();
                foreach (string match in schedule)
                {
                    games[2].Add(match + "|G|Group stage");
                }
                numMatches[2] = MAX_CHAMP_LEAGUE_MATCHES;
                numMatchesInGroup[2] = (teamsPerGroup[2] - 1) * 2;
                drawCup("Round of 16", 2, MATCHTYPE.CHAMPCUP);
            }

            //print("NMATCHES LEAGUE " + nMatchesLeague);
            ScheduleMatchesNew matchesCup =
               new ScheduleMatchesNew(teamsPerGroup[3], 1);
            schedule = matchesCup.getListOfMatches();
            foreach (string match in schedule)
            {
                games[3].Add(match + "|G|Group stage");
            }
            drawCup("Round of 64", 3, MATCHTYPE.LEAGUECUP);
            //numMatches[3] = (teamsPerGroup[3] - 1) + 4;
            numMatches[3] = MAX_LEAGUE_CUP_MATCHES;
            numMatchesInGroup[3] = teamsPerGroup[3] - 1;
        }

        //print("#DBGSCHED MATCHSCHEDULE START");
        for (int i = 1; i <= numCompetitions; i++)
        {
            for (int j = 0; j < games[i].Count; j++)
            {
                string teamAName = "", teamBName = "";
                getTeamsNamesByIdx(i,
                                   games[i][j],
                                   ref teamAName,
                                   ref teamBName);

                /*print("#DBGSCHED I " + i + "| " 
                    + teamAName + " vs " + teamBName + " Type: " 
                    + games[i][j].Split('|')[1] + " : " +
                      games[i][j].Split('|')[2]);*/
            }
        }
        //print("#DBGSCHED MATCHSCHEDULE END");

        /*champ league*/
        int champCupMod = nMatchesLeague / MAX_CHAMP_LEAGUE_MATCHES;
        if (champCupMod == 0) champCupMod = 1;

        int leagueCupMod = nMatchesLeague / MAX_LEAGUE_CUP_MATCHES;
        if (leagueCupMod == 0) leagueCupMod = 1;

        //it works only when nMatchesLeague is higher than 
        //    MAX_CHAMP_LEAGUE_MATCHES &&
        //    MAX_LEAGUE_CUP_MATCHES

        int[] nMatchesScheduled = new int[MAX_COMPETITIONS + 1];
        nMatchesScheduled[(int) MATCHTYPE.CHAMPCUP] = 
            MAX_CHAMP_LEAGUE_MATCHES;
        nMatchesScheduled[(int) MATCHTYPE.LEAGUECUP] = 
            MAX_LEAGUE_CUP_MATCHES;

        //match league should always be first
        matchesSchedule.Add(MATCHTYPE.LEAGUE);
        for (int i = 1; i < nMatchesLeague; i++)
        {
            /*add champ league match*/
            if (isChampCupActive &&
                (i % champCupMod) == 0 && 
                numCompetitions > 1 &&
                nMatchesScheduled[(int) MATCHTYPE.CHAMPCUP] > 0)
            {
                matchesSchedule.Add(MATCHTYPE.CHAMPCUP);
                nMatchesScheduled[(int) MATCHTYPE.CHAMPCUP]--;
            }

            matchesSchedule.Add(MATCHTYPE.LEAGUE);

            if ((i % leagueCupMod) == 0 && 
                numCompetitions > 1 &&
                nMatchesScheduled[(int)MATCHTYPE.LEAGUECUP] > 0)
            {
                matchesSchedule.Add(MATCHTYPE.LEAGUECUP);
                nMatchesScheduled[(int)MATCHTYPE.LEAGUECUP]--;
            }
        }

        //if some matches remains in champ cup add them
        if (numCompetitions > 1)
        {
            for (int i = 0; i < nMatchesScheduled[(int) MATCHTYPE.CHAMPCUP]; i++)
            {
                matchesSchedule.Add(MATCHTYPE.CHAMPCUP);
            }

            for (int i = 0; i < nMatchesScheduled[(int) MATCHTYPE.LEAGUECUP]; i++)
            {
                matchesSchedule.Add(MATCHTYPE.LEAGUECUP);
            }
        } else
        {
            int nMatchesLeft = 0;            
            if (leagueName.Equals("CHAMP CUP"))
                nMatchesLeft = MAX_CHAMP_LEAGUE_MATCHES - numMatchesInGroup[1];
            else
            {
                nMatchesLeft = MAX_WORLD_CUP_MATCHES - numMatchesInGroup[1];
            }

            //print("NEXT " + nMatchesLeft + " LEAGUENAME " + leagueName);
            for (int i = 1; i <= nMatchesLeft; i++)
            {
                matchesSchedule.Add(MATCHTYPE.LEAGUE);
            }
        }

        //foreach (MATCHTYPE matchVal in matchesSchedule)
        //{
        //    print("Schedule match " + matchVal);
        //}

        /*print games*/
        //for (int i = 1; i <= 3; i++)
        //{
        //    print("###GROUP\n" + i);
        //    for (int j = 0; j < games[i].Count; j++)
        //    {
        //        print(games[i][j] + "\n");
        //    }
        //}           
    }

    private void updateButtonUpText()
    {
        if (numCompetitions == 1)
        {
            leaguesNames[1] = leagueName;
            //Languages.getTranslate(leagueName);
            mainButtonText[0].text = leagueName;
                    Languages.getTranslate(leagueName);
        }
    }

    private void playOnClickEvent()
    {
        bool entered = false;

        //if (endOfTheGame)
        // {
        //    Globals.recoverOriginalResolution();
        //    SceneManager.LoadScene("menu");
        //   return;
        //}

        //print("#DBGPLAYONCLICK1 ");
      
        int score1, score2;
        bool playerMatchFound = false;
        int teamASaved = 0, teamBSaved = 0, savedGroupNum = 0;
        bool exitMatchSchedule = false;

        //for (int k = 0; k < matchesSchedule.Count; k++)
        while (matchesSchedule.Count != 0)
        {            
            int group = (int) matchesSchedule[0];
            matchesSchedule.RemoveAt(0);
            if (!isLeagueActive[group])
                continue;

            //print("#DBGPLAYONCLICK2 ");

            //for (int mIdx = gamesIdx[group]; mIdx < gamesElementN[group]; mIdx++)
            while (games[group].Count != 0)
            {
                //string match = games[group, gamesIdx[group]];
                string eventLine = games[group][0];
                games[group].RemoveAt(0);             
                //print("MATCHRESULT1 FETCH " + eventLine + " GROUP + " + group);
                string[] match = eventLine.Split('|');
                string matchType = match[1];
                if (playerMatchFound &&
                    match[0].Equals("0-0"))
                {
                    exitMatchSchedule = true;
                    break;
                }

                string[] teamsIdx = match[0].Split('-');
                teamA = int.Parse(teamsIdx[0]);
                teamB = int.Parse(teamsIdx[1]);

                if ((teamA == 1 || 
                    teamB == 1) || 
                    teamA == plrTeamCupId ||
                    teamB == plrTeamCupId)
                {
                    playerGameSaved = eventLine;
                    teamASaved = teamA;
                    teamBSaved = teamB;
                    savedGroupNum = group;
                    playerMatchFound = true;
                    continue;
                }
  
                // in percent 
                int team1ChanceOfWinning = 35;
                int team1DrawTeam2 = 30;
                int team2ChanceOfWinning = 35;

                int teamAidx = int.Parse(groups[group][teamA][tableColumns - 3]);
                int teamBidx = int.Parse(groups[group][teamB][tableColumns - 3]);
                int[] bothTeamsId = { teamAidx, teamBidx };

                 int[] avg = new int[2];
                 for (int i = 0; i < avg.Length; i++)
                 {
                    int team = bothTeamsId[i];
                    string[] skills = orgTeams[group].getTeamByIndex(team);
                    //add strength and speed and make average 
                    //avg[i] = (int.Parse(skills[1]) +
                    //          int.Parse(skills[2])) / 2;

                    Vector2 avgSkills = 
                        Globals.getTeamSkillsAverage(skills, leagueName);
                    //print("#DBGTEAMSKILLSXY " + skills[0] + " avgSkills " + avgSkills + " index " + i);

                    avg[i] = (int) ((avgSkills.x + avgSkills.y) / 2f);
                 }

                 double diff = Math.Abs(avg[0] - avg[1]) / 65.0;
                 if (diff > 1)
                    diff = 1;


                 if (avg[0] > avg[1])
                 {
                    team1ChanceOfWinning += (int)(((double) team1DrawTeam2 * diff) +
                                            ((double) team2ChanceOfWinning * diff));

                    team1DrawTeam2 -= (int)((double) team1DrawTeam2 * diff);
                    team2ChanceOfWinning = 100 - team1ChanceOfWinning - team1DrawTeam2;
                 }
                 else
                 {
                    team2ChanceOfWinning += (int)(((double) team1DrawTeam2 * diff) +
                                            ((double) team1ChanceOfWinning * diff));

                    team1DrawTeam2 -= (int)((double)team1DrawTeam2 * diff);
                    team1ChanceOfWinning = 100 - team2ChanceOfWinning - team1DrawTeam2;
                 }

                team1DrawTeam2 = 100 - team1ChanceOfWinning - team2ChanceOfWinning;

                /*print("#DBGTEAMSKILLSXY team1chance: " + team1ChanceOfWinning 
                     + " draw: " + team1DrawTeam2
                     + " team2chance: " + team2ChanceOfWinning + " diff " + diff +
                     " team1DrawTeam2 " + " relative diff " + Math.Abs(avg[0] - avg[1]));*/

                 // draw a number from 1 to 100 
                 int num = rand.Next(1, 100);
                 int tmpscore;

                 if (Math.Abs(avg[0] - avg[1]) <= 20)
                 {
                    score1 = rand.Next(4);
                    score2 = rand.Next(4);
                 }
                 else
                 {
                    score1 = rand.Next(10);
                    score2 = rand.Next(10);
                 }

                 int maxGoalDiff = 4 - (Math.Abs(team1ChanceOfWinning - team2ChanceOfWinning) / 33);

                 if (maxGoalDiff < 0) maxGoalDiff = 1;

                 int goalDiff = rand.Next(maxGoalDiff) + 1;

                 if (score1 == score2) score1++;

                 // First team wins 
                 if (num <= team1ChanceOfWinning)
                 {
                        if (score2 > score1)
                        {
                            tmpscore = score1;
                            score1 = score2;
                            score2 = tmpscore;
                        }

                        if (team2ChanceOfWinning > team1ChanceOfWinning)
                        {
                            if ((score1 - goalDiff) < 0)
                                score2 = 0;
                            else
                                {
                                    score2 = score1 - goalDiff;
                                }
                        }
                 } else
                 {
                    // Draw 
                    if (num <= (team1ChanceOfWinning + team1DrawTeam2))
                    {
                        score1 = score2;
                    }
                    else
                    {
                         // Second team wins 
                        if (score1 > score2)
                        {
                            tmpscore = score1;
                            score1 = score2;
                            score2 = tmpscore;
                        }

                        if (team1ChanceOfWinning > team2ChanceOfWinning)
                        {
                            if ((score2 - goalDiff) < 0)
                                score1 = 0;
                            else
                                score1 = score2 - goalDiff;
                        }
                    }
                 }


                //print("#DBGPLAYONCLICK3 ");


                updateLastResults(teamA, teamB, score1, score2, eventLine, group);
                 //if (!isKnockOutStage)
                // {
                 updateTable(teamA, teamB, score1, score2, group);
                 /*print("MATCHRESULT1 UPDATESCORE " + teamA + " teamA " + " teamB " 
                       + teamB + " Result " + score1 + ":"  
                       + score2 + " currIdxGroup " + group
                       );*/
                 //}
            }

            //matchesScheduleIdx++;
            if (exitMatchSchedule)
            {
                break;
            }
        }

        if (playerMatchFound)
        {
            TEAM1_ID = teamASaved;
            TEAM2_ID = teamBSaved;
               
            //Prepare to game
            Globals.score1 = -1;
            Globals.score2 = -1;

            //to do implementation of cup
            string[] team1;
            string[] team2;

            if (teamASaved == plrTeamCupId ||
                teamASaved == 1)
            {
                team1 = playerTeamDesc;
                if (teamASaved == plrTeamCupId)
                {
                    team2 = orgTeams[savedGroupNum].getTeamByIndex(teamBSaved);
                } else
                {
                    team2 = orgTeams[savedGroupNum].getTeamByIndex(
                        int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]));
                }

                Globals.playerPlayAway = false;
            }
            else
            {
                //team2 is playing away
                team2 = playerTeamDesc;
                if (teamBSaved == plrTeamCupId)
                {
                    team1 = orgTeams[savedGroupNum].getTeamByIndex(teamASaved);
                }
                else
                {
                    team1 = orgTeams[savedGroupNum].getTeamByIndex(
                        int.Parse(groups[savedGroupNum][teamASaved][tableColumns - 3]));
                }
                Globals.playerPlayAway = true;
            }

            /*print("NEXTMATCH REAL " + teamASaved 
                + " teamBSaved " + teamBSaved + " Globals " +
                Globals.playerPlayAway);*/


            //string[] team2 = orgTeams[savedGroupNum].getTeamByIndex(team2ID);

            //Globals.playerPlayAway = false;
            //if (teamBSaved == 1 || teamBSaved == plrTeamCupId)
            //{
            //    team2 = playerTeamDesc;
            //    team1 = orgTeams[savedGroupNum].getTeamByIndex(
            //              int.Parse(groups[savedGroupNum][teamASaved][tableColumns - 3]));
            //    Globals.playerPlayAway = true;
            //}

            Vector2 skillAvgA =
                Globals.getTeamSkillsAverage(team1, Globals.leagueName);

            Vector2 skillsAvgB =
               Globals.getTeamSkillsAverage(team2, Globals.leagueName);

            //Globals.teamAGkStrength = int.Parse(team1[1]);
            //Globals.teamAAttackStrength = int.Parse(team1[2]);
            //Globals.teamAcumulativeStrength = int.Parse(team1[1]) + int.Parse(team1[2]);
            Globals.teamAGkStrength = (int) skillAvgA.x;
            Globals.teamAAttackStrength = (int) skillAvgA.y;
            Globals.teamAcumulativeStrength =
                Globals.teamAGkStrength + Globals.teamAAttackStrength;


            //Globals.stadiumColorTeamA = int.Parse(team1[5]);
            Globals.stadiumColorTeamA = team1[5];

            Globals.teamAleague = leagueName;
            Globals.teamBleague = leagueName;

            if (savedGroupNum == 2)
            {
                if (Globals.playerPlayAway)
                {
                    Globals.teamAleague = "CHAMP CUP";
                } else
                {
                    Globals.teamBleague = "CHAMP CUP";
                }
            }

            Globals.teamAid = int.Parse(team1[3]);

            int randPlayer = 0;
            if (Globals.playerPlayAway)
            {
                randPlayer = 
                    UnityEngine.Random.Range(0, team1[12].Split('|').Length);
            }

            Globals.playerADesc = team1[12].Split('|')[randPlayer];
            Globals.teamAname = team1[0];

            //Globals.teamBGkStrength = int.Parse(team2[1]);
            //Globals.teamBAttackStrength = int.Parse(team2[2]);
            //Globals.teamBcumulativeStrength = int.Parse(team2[1]) + int.Parse(team2[2]);
            Globals.teamBGkStrength = (int) skillsAvgB.x;
            Globals.teamBAttackStrength = (int) skillsAvgB.y;
            Globals.teamBcumulativeStrength = 
                Globals.teamBGkStrength + Globals.teamBAttackStrength;

            //Globals.stadiumColorTeamB = int.Parse(team2[5]);
            Globals.stadiumColorTeamB = team2[5];
            Globals.teamBid = int.Parse(team2[3]);

            randPlayer = 0;
            if (!Globals.playerPlayAway)
            {
                randPlayer =
                    UnityEngine.Random.Range(0, team2[12].Split('|').Length);
            }
            Globals.playerBDesc = team2[12].Split('|')[randPlayer];
            Globals.teamBname = team2[0];

            /*print("DBGTRANSFER334 TEAM GLOBALS ID MATCH TABLE " +
                " Globals.teamAid " + Globals.teamAid +
                " Globals.teamBid " + Globals.teamBid 
                + " TEMANAM " 
                + 
                Globals.teamAname + " teamBName " 
                + Globals.teamBname
                + " playerADESC " + Globals.playerADesc
                + " playerBDESC " + Globals.playerBDesc);*/

            /*  Globals.teamBname =
                  orgTeams[savedGroupNum].getTeamByIndex(
                      int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[0];

              Globals.teamBGkStrength = int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[1]);

              Globals.teamBAttackStrength = int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[2]);

              Globals.teamBcumulativeStrength =
                  int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[1]) +
                  int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[2]);

              Globals.stadiumColorTeamB = int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[5]);

              Globals.teamBid = int.Parse(
                      orgTeams[savedGroupNum].getTeamByIndex(
                          int.Parse(groups[savedGroupNum][teamBSaved][tableColumns - 3]))[3]);

              */


            //if (isKnockOutStage)
            ///     Globals.gameInGroup = false;


            string[] match = playerGameSaved.Split('|');
            if (match[1].Equals("C"))
                Globals.gameInGroup = false;
            else
                Globals.gameInGroup = true;

            Globals.recoverOriginalResolution();
            //SceneManager.LoadScene("specialShopOffers");
            //SceneManager.LoadScene("gameScene");

            //Globals.matchTime = "1 SECONDS";
        
            activeGroup = savedGroupNum;
            if (activeGroup == 1)
                leagueWeek++;

            if (leagueName.Contains("CUP") &&
               !leagueName.Equals("CHAMP CUP"))
            {
                //SceneManager.LoadScene("specialShopOffers");
                Globals.loadSceneWithBarLoader("specialShopOffers");
            }
            else
            {
                Globals.loadSceneWithBarLoaderNoSettings("teamManagement");
                //Globals.loadSceneWithBarLoader("teamManagement");
                //SceneManager.LoadScene("");
            }

            //print("#DBGPLAYONCLICK4 ");

            //print("NEXTMATCH " + Globals.teamAname + "vs" + Globals.teamBname);
        }
    }

    public void onClickLeague()
    {
        infoPanelNoBigBackground.SetActive(false);

        //print("ONCLICK LEAGUE");
        clearResTable();

        int gameType = (int) MATCHTYPE.LEAGUE;
        printResTable(
            (2 * teamsPerGroup[activeGroup]) + 2, tableColumns - 2, gameType);
        printLastRes(gameType);
        printNextMatches(gameType);
        disableUpButtonFocusApartFrom(0);
    }

    public void onClickLeagueCup()
    {
        infoPanelNoBigBackground.SetActive(false);
        //print("onClickLeagueCup");
        clearResTable();

        int gameType = (int) MATCHTYPE.LEAGUECUP;
        printResTable(
            (2 * teamsPerGroup[activeGroup]) + 2, tableColumns - 2, gameType);
        printLastRes(gameType);
        printNextMatches(gameType);
        disableUpButtonFocusApartFrom(2);
    }

    public void onClickChampLeague()
    {
        //print("onClickChampLeague");
        clearResTable();

        //print("#DBGPLAYER1TEAMNAME_TEST1 ONCLICKCHAMP " + isChampCupActive);

        int gameType = (int) MATCHTYPE.CHAMPCUP;
        if (isChampCupActive)
        {
            printResTable(
                (2 * teamsPerGroup[activeGroup]) + 2, tableColumns - 2, gameType);           
        } 

        printLastRes(gameType);
        printNextMatches(gameType);

        disableUpButtonFocusApartFrom(1);

        if (!isChampCupActive)
        {
            infoPanel.SetActive(false);
            infoPanelNoBigBackground.SetActive(true);
            infoPanelNoBackgroundHeaderText.text = 
                Languages.getTranslate("Champ Cup");
            infoPanelNoBackgroundText.text =
                Languages.getTranslate(
                    player1TeamName + " was not qualified to Champ Cup in the last season. " +
                    "\n Only first " + teamQualifiedNum[1].ToString() + " teams in the league get qualified",
                     new List<string>() { player1TeamName, teamQualifiedNum[1].ToString() });
        }
    }

    //This prints group table 
    private void printResTable(int nRow, int nCol, int groupNum)
    {
        //For the time being I support only one group - group 1 
        //int groupNum = 1;
        string player1teamName = "";
        //if (isKnockOutStage)
        //    player1teamName = savedGroups[1][1][1];
        //else
        //{
        player1teamName = groups[1][1][1];
        //}
        //print("#DBGGROUP recoverd 1 " + player1teamName);

        for (int j = 0; j < teamsPerGroup[groupNum]; j++)
        {
            for (int k = 0; k < tableColumns; k++)
            {
                //if (!isKnockOutStage)
                    sortedGroups[groupNum][j][k] = groups[groupNum][j + 1][k];
                //else
                //    sortedGroups[groupNum][j][k] = savedGroups[groupNum][j + 1][k];

                //print("SORTED " + sortedGroups[groupNum][j][k] + " " + " J " + j + " k " + k);
            }
            //print("\n");
        }

        //print("#DBGGROUP recoverd 2 " + player1teamName);

        //print("sortedGroups[groupNum] " + sortedGroups[groupNum] + " groupNum " + groupNum);
        Array.Sort(sortedGroups[groupNum], sortedGroupsComp);

        printGroupCanvasHeader(teamsPerGroup[groupNum]);

        //print("#DBGGROUP recoverd 3 " + player1teamName);
        /*Set Group Canvas flag and color of row*/
        for (int row = 1; row <= teamsPerGroup[groupNum]; row++)
        {
            //print("#DBGGROUP recoverd 3.1 " + player1teamName + " row " + row + " groupNum " + groupNum);

            var tempColor = groupResultFlag[row].color;
            tempColor.a = 1f;
            groupResultFlag[row].color = tempColor;
            groupResultFlag[row].texture = 
                getFlag(sortedGroups[groupNum][row - 1][1], groupNum);

            /*just first 4 rows*/
            int promotedTeams = 
                teamQualifiedNum[groupNum];
            //print("PROMOTEDTEAMS " + groupNum);

            if (row <= 4)
            {
                Color outRowColor;

                if (row <= promotedTeams)
                {
                    ColorUtility.TryParseHtmlString("#00446A", out outRowColor);
                    outRowColor.a = 0.94f;
                }
                else
                {
                    ColorUtility.TryParseHtmlString("#052357", out outRowColor);
                    outRowColor.a = 0.313f;
                }

                groupResultRow[row].GetComponent<Image>().color = outRowColor;
            }
        }

        //print("#DBGGROUP before SORTED TEMA NAME 4 " + player1TeamName);
        for (int j = 0; j < teamsPerGroup[groupNum]; j++)
        {
            if (player1TeamName.Equals(sortedGroups[groupNum][j][1]))
            {
                float scrollVal = 1;
                Player1PosinGroup = j + 1;

                //print("#DBGGROUP POS " + Player1PosinGroup);

                playerPosInGroup[groupNum] = Player1PosinGroup;
                //resultScroll.verticalNormalizedPosition = 0.12f;

                //TOREMOVE
                //Player1PosinGroup = 11;

                if (Player1PosinGroup <= 9)
                {
                    //resultScroll.verticalNormalizedPosition = 1f;
                    scrollVal = 1;
                }
                else if (Player1PosinGroup < 12)
                {
                    //resultScroll.verticalNormalizedPosition = 0.5f;
                    scrollVal = 0.5f;
                }
                else if (Player1PosinGroup < 14)
                {
                    //resultScroll.verticalNormalizedPosition = 0.32f;
                    scrollVal = 0.32f;
                }
                else if (Player1PosinGroup < 16)
                {
                    //resultScroll.verticalNormalizedPosition = 0.12f;
                    scrollVal = 0.12f;
                }
                else
                {
                    //resultScroll.verticalNormalizedPosition = 0f;
                    scrollVal = 0f;
                    //print("#DBGVERT SET ZERO " + resultScroll.verticalNormalizedPosition);
                }


                StartCoroutine(resTableScroll(scrollVal));
                //print("#DBGVERT " + resultScroll.verticalNormalizedPosition);
            }
        }

        /*Set up canvas text */
        for (int i = 1; i < teamsPerGroup[groupNum] + 1; i++)
        {
            for (int j = 1; j < groupTableNrCols; j++)
            {
                Color textColor;

                if (Player1PosinGroup == i)
                    ColorUtility.TryParseHtmlString("#79BDFF", out textColor);
                else
                    ColorUtility.TryParseHtmlString("#FFFFFF", out textColor);

                textTableGroup[i, j].text = sortedGroups[groupNum][i - 1][j];
                textTableGroup[i, j].color = textColor;
            }
        }
    }

    private IEnumerator resTableScroll(float scrollVal)
    {
        //print("#DBGVERTDBG BEFORE " + resultScroll.verticalNormalizedPosition + " VAL " + scrollVal);

        yield return new WaitForEndOfFrame();
        resultScroll.verticalNormalizedPosition = scrollVal;

        //print("#DBGVERTDBG AFTER " + resultScroll.verticalNormalizedPosition);
    }

    private int sortedGroupsComp(string[] first, string[] second)
    {

        //print("inside comp name " + first[1] + " second " + second[1]);

//        print("inside comp " + int.Parse(first[3]) + " second " + int.Parse(second[3]));
//        print("inside comp 2 " + int.Parse(first[4]) + " second " + int.Parse(second[4]));

        if (int.Parse(first[3]) > int.Parse(second[3]))
            return -1;

        if (int.Parse(first[3]) < int.Parse(second[3]))
            return 1;

        //point are the same compare goals 
        string[] plr1Goals = first[4].Split(':');
        string[] plr2Goals = second[4].Split(':');

        int diff1 = int.Parse(plr1Goals[0]) - int.Parse(plr1Goals[1]);
        int diff2 = int.Parse(plr2Goals[0]) - int.Parse(plr2Goals[1]);

        if (diff1 > diff2)
            return -1;

        if (diff1 < diff2)
            return 1;

        if (diff1 == diff2)
        {
            if (int.Parse(plr1Goals[0]) > int.Parse(plr2Goals[0]))
                return -1;

            if (int.Parse(plr1Goals[0]) < int.Parse(plr2Goals[0]))
                return 1;
        }

        // if points and goals are the same - compare direct matches 
        //string result = groupResultsMap[first[1] + "-" + second[1]];
        string result;
        groupResultsMap.TryGetValue(first[1] + "-" + second[1], out result);

        bool teamsSwaped = false;
        if (result == null)
        {
            //result = groupResultsMap[second[1] + "-" + first[1]];
            groupResultsMap.TryGetValue(second[1] + "-" + first[1], out result);
            teamsSwaped = true;
        }

        if (result != null)
        {
            string[] scores = result.Split('-');
            int score1 = int.Parse(scores[0]);
            int score2 = int.Parse(scores[1]);

            if (teamsSwaped)
            {
                score2 = score1;
                score1 = int.Parse(scores[1]);
            }

            if (score1 > score2) return -1;
            if (score2 > score1) return 1;
        }

        return 0;
    }

    //private void clearResTable(TextView resTableCol[][], int nRow, int nCol)
    private void clearResTable()
    {

        for (int i = 1; i <= groupTableNrRows; i++) {
            for (int j = 0; j < groupTableNrCols; j++) {
                //resTableCol[i][j].setBackgroundColor(Color.TRANSPARENT);
                //resTableCol[i][j].setBackground(null);
                //print("CLEAR RES TABLE I " + i + " J " + j + " textTableGroup[i, j] " + textTableGroup[i, j]);
                textTableGroup[i, j].text = "";
                var tempColor = groupResultFlag[i].color;
                tempColor.a = 0f;
                groupResultFlag[i].color = tempColor;               
            }
        }
    }

    private void insertResultHashMap(string key, string value)
    {
        groupResultsMap.Add(key, value);
    }

    private void insertFlagToHashMap(string key, float w, float h)
    {
        String teamName = Regex.Replace(key, "\\s+", "");
    }

    private Texture2D getFlag(string teamName, int groupNum)
    {
        Texture2D flag;

        if (Globals.isTeamCustomize(teamName))
        {

            if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
            {
                return graphics.loadTexture(Globals.logoFilePath);
            }
            else
            {
                return Resources.Load<Texture2D>(
                            "others/logoFile");
            }
        }

        String flagPath = 
            graphics.getFlagFullPath(teamName);
              
        if (flagsHashMap.TryGetValue(flagPath, out flag))
        {
            return flag;
        }
        else
        {
            Texture2D texture = graphics.getTexture(flagPath);
            if (texture != null)
            {
                flagsHashMap.Add(flagPath, texture);
            }
            return texture;
        }

        return null;
    }

    private void setTeamFlagForResultTable(string teamName, int row, int col, float w, float h)
    {

        teamName = Regex.Replace(teamName, "\\s+", "");
    }

    private void fillHeader()
    {
        groups[1][0][0] = " ";
        groups[1][0][1] = "Team";
        groups[1][0][2] = "M";
        groups[1][0][3] = "P";
        groups[1][0][4] = "G";
    }

    private void printGroupCanvasHeader(int teamsPerGroup)
    {
        int row = 0;
        int col;

        /*print header TEAM|M|P|G*/   
        /*fill indexes of teams 1|2|3...*/
        col = 0;
        for (row = 1; row <= teamsPerGroup; row++)
        {
            //print("ROW COL " + row + " col  " + col);
            textTableGroup[row, col].text = row.ToString();
        }

    }

    private void fillGroups()
    {
        int drawTeamIdx;

        int plr1TeamId = playerTeamID;
        int maxSkillsTeamIdx;
        int maxSkills, minSkills;

        playerTeamDesc = teams[1].getTeamByIndex(playerTeamID);
   
        /*TOCHECK plr1TeamIDX for cup*/
        if (!initPlrGroupStage)
        {
            string[] player1Team = teams[1].getTeamByIndex(playerTeamID);

            for (int group = 1; group <= numGroups; group++)
            {

                if (!isLeagueActive[group])
                    continue;

                groups[group][1][0] = "0";
                groups[group][1][1] = player1Team[0];
                //this.player1TeamName = player1Team[0];
                //attack
                groups[group][1][tableColumns - 1] = player1Team[2];
                //defense
                groups[group][1][tableColumns - 2] = player1Team[1];
                //Team main id
                groups[group][1][tableColumns - 3] = playerTeamID.ToString();
                groups[group][1][2] = "0";
                groups[group][1][3] = "0";
                groups[group][1][4] = "0:0";
                groups[group][1][5] = "0";
                groups[group][1][6] = "0";
                groups[group][1][7] = "0";

                teams[group].swapElements(playerTeamID, teams[group].getMaxActiveTeams() - 1);
                initPlrGroupStage = true;
            }
        }

            //rand = new System.Random(System.currentTimeMillis());
            rand = new System.Random();

            string[] drawTeam;

            for (int group = 1; group <= numGroups; group++)
            {
                if (!isLeagueActive[group])
                    continue;

                for (int index = 2; index <= teamsPerGroup[group]; index++)
                {
                    //if (group == 1 && index == 1) continue;
                    //if (index == 1) continue;
                    drawTeamIdx = rand.Next(teams[group].getMaxActiveTeams() - 1);
                    drawTeam = teams[group].getTeamByIndex(drawTeamIdx);
                    Vector2 skills =
                        Globals.getTeamSkillsAverage(drawTeam, leagueName);
                    maxSkills = minSkills = (int) (skills.x + skills.y);
                    //maxSkills = minSkills = int.Parse(drawTeam[1]) + int.Parse(drawTeam[2]);

                    int tmpSkills;
                    string[] tmpdrawTeam;
                    int tmpDrawTeamIdx;

                    //Last team in the group is with lowest skills 
                    //if (group == 1 && index == teamsPerGroup[group] && !isKnockOutStage)
                    if (index == teamsPerGroup[group])
                    {
                        //for (int j = 1; j <= 4; j++)
                        for (int j = 1; j <= 2; j++)
                            {
                                tmpDrawTeamIdx = rand.Next(teams[group].getMaxActiveTeams() - 1);
                                tmpdrawTeam = teams[group].getTeamByIndex(tmpDrawTeamIdx);
                                Vector2 skillsGLast =
                                    Globals.getTeamSkillsAverage(tmpdrawTeam, leagueName);
                                tmpSkills = (int) (skills.x + skills.y);
                                //tmpSkills = int.Parse(tmpdrawTeam[1]) + int.Parse(tmpdrawTeam[2]);

                                if (tmpSkills < minSkills)
                                {
                                    minSkills = tmpSkills;
                                    drawTeamIdx = tmpDrawTeamIdx;
                                    drawTeam = tmpdrawTeam;
                                }
                            }
                        }

                    //if (isKnockOutStage == true || (group == 1 && index == 2))
                        if (index == 2)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                tmpDrawTeamIdx = rand.Next(teams[group].getMaxActiveTeams() - 1);
                                tmpdrawTeam = teams[group].getTeamByIndex(tmpDrawTeamIdx);

                                Vector2 skillsSecond =
                                    Globals.getTeamSkillsAverage(tmpdrawTeam, leagueName);  
                                tmpSkills = (int)(skills.x + skills.y);
                        //tmpSkills = int.Parse(tmpdrawTeam[1]) + int.Parse(tmpdrawTeam[2]);

                            if (tmpSkills > maxSkills)
                                {
                                    maxSkills = tmpSkills;
                                    drawTeamIdx = tmpDrawTeamIdx;
                                    drawTeam = tmpdrawTeam;
                                }
                            }
                        }

                        string[] choose = teams[group].getTeamByIndex(drawTeamIdx);

                    //print("GET TEAM BY INDEX " + index + " drawTeam " + drawTeam[0]);
                        groups[group][index][0] = "0";
                        groups[group][index][1] = drawTeam[0];
                        groups[group][index][2] = "0";
                        groups[group][index][3] = "0";
                        groups[group][index][4] = "0:0";
                        groups[group][index][5] = "0";
                        groups[group][index][6] = "0";
                        groups[group][index][7] = "0";
                        groups[group][index][tableColumns - 1] = drawTeam[2];
                        groups[group][index][tableColumns - 2] = drawTeam[1];
                        groups[group][index][tableColumns - 3] = drawTeam[3];
                        teams[group].swapElements(drawTeamIdx, teams[group].getMaxActiveTeams() - 1);
                }
            }
    }

    private void drawCup(string startRound, int group, MATCHTYPE gameType)
    {
        int startIdx = 0;
        for (int i = 0; i < cupStages.Length; i++) {
            if (cupStages[i].Equals(startRound))
            {
                startIdx = i;
                break;
            }
        }

        int nStages = cupStages.Length - startIdx;
        int drawMaxTeamsDelta = 0;
        string match;

        //print("GROUP START IDX " + group + " startIDX " + startIdx);

        for (int i = startIdx; i < cupStages.Length; i++)
        {
            if (gameType == MATCHTYPE.CHAMPCUP ||
                gameType == MATCHTYPE.LEAGUE)
            {
                match = drawOneTeam(2 + drawMaxTeamsDelta, true, group);
            } else
            {
                if ((cupStages.Length - i) < 2)
                {
                    match = drawOneTeam(1 + drawMaxTeamsDelta, true, group);
                } else {
                    match = drawOneTeam(Mathf.Max(1, 3 - drawMaxTeamsDelta), false, group);
                }
            }

            drawMaxTeamsDelta++;
            games[group].Add(match + "|C|" + cupStages[i]);
            games[group].Add("0-0|E|E");
        }
    }

    private string drawOneTeam(int maxTeams, bool isTheStrongest, int group)
    {
        int tmpSkills;
        string[] tmpdrawTeam;
        int drawTeamIdx = -1, tmpDrawTeamIdx = -1;
        int orgDrawTeamIdx = -1;
        int maxSkills = 0, minSkills = int.MaxValue;
        string[] drawTeam;

        for (int j = 1; j <= maxTeams; j++)
        {
            tmpDrawTeamIdx = rand.Next(teams[group].getMaxActiveTeams() - 1);
            tmpdrawTeam = teams[group].getTeamByIndex(tmpDrawTeamIdx);

            //print("TMPDRAWTEM " + tmpdrawTeam[0] + " group " + group + " Idx " + tmpDrawTeamIdx);
            //tmpSkills = int.Parse(tmpdrawTeam[1]) + int.Parse(tmpdrawTeam[2]);
            Vector2 skills =
                Globals.getTeamSkillsAverage(tmpdrawTeam, leagueName);
            tmpSkills = (int)(skills.x + skills.y);

            if (isTheStrongest)
            {
                if (tmpSkills > maxSkills)
                {
                    maxSkills = tmpSkills;
                    drawTeamIdx = tmpDrawTeamIdx;
                    orgDrawTeamIdx = int.Parse(tmpdrawTeam[3]);
                    drawTeam = tmpdrawTeam;
                }
            } else
            {
                if (tmpSkills < minSkills)
                {
                    minSkills = tmpSkills;
                    drawTeamIdx = tmpDrawTeamIdx;
                    orgDrawTeamIdx = int.Parse(tmpdrawTeam[3]);
                    drawTeam = tmpdrawTeam;
                }
            }           
        }

        teams[group].swapElements(drawTeamIdx, teams[group].getMaxActiveTeams() - 1);
        return plrTeamCupId.ToString() + "-" + orgDrawTeamIdx.ToString();
    }


    private void printGroup(int group)
    {
        print("GROUP " + group);
        for (int j = 1; j <= teamsPerGroup[activeGroup]; j++)
        {
            print(groups[group][j][1]);
        }
    }

    private void addValue(int group, int row, int col, int val)
    {
        int readV = int.Parse(groups[group][row][col]) + val;
        groups[group][row][col] = readV.ToString();
    }

    private void addGoalstoTable(int group, int row, int score1, int score2)
    {

        string[] playerGoals = groups[group][row][4].Split(':');
        int currentScored = int.Parse(playerGoals[0]) + score1;
        int currentLost = int.Parse(playerGoals[1]) + score2;

        groups[group][row][4] = currentScored.ToString() + ":" + currentLost.ToString();
    }

    private void addWinDrawLost(int group, int row, int score1, int score2)
    {
        int col = 6;
        if (score1 > score2)
            col = 5;
        else if (score2 > score1)
        {
            col = 7;
        }

        groups[group][row][col] = (int.Parse(groups[group][row][col]) + 1).ToString();
    }

    private void updateTable(int teamA, int teamB, int score1, int score2, int groupId)
    {

        addValue(groupId, teamA, 2, 1);
        addValue(groupId, teamB, 2, 1);
        addGoalstoTable(groupId, teamA, score1, score2);
        addGoalstoTable(groupId, teamB, score2, score1);
        addWinDrawLost(groupId, teamA, score1, score2);
        addWinDrawLost(groupId, teamB, score2, score1);

        if (score1 == score2)
        {
            addValue(groupId, teamA, 3, 1);
            addValue(groupId, teamB, 3, 1);
        }
        else
        {
            if (score1 > score2)
            {
                addValue(groupId, teamA, 3, 3);
            }
            else
            {
                addValue(groupId, teamB, 3, 3);
            }
        }

        string match = groups[groupId][teamA][1] + "-" + groups[groupId][teamB][1];
        string result = score1.ToString() + "-" + score2.ToString();

        //print("MATCHRESULT2 " + match + " RESULT " + result + " GROUP " + groupId);
        insertResultHashMap(match, result);
    }

    private void updateLastResults(int teamA, 
                                   int teamB, 
                                   int score1, 
                                   int score2, 
                                   string eventLine, 
                                   int gameType)
    {

        if ((tailLastResult[gameType] % lastResultsNumElem[gameType]) == 0)
        {
            tailLastResult[gameType] = 0;
            if (numUpdateLastResult[gameType])
            {
                if (tailLastResult[gameType] == headLastResult[gameType])
                    headLastResult[gameType] = 
                        (headLastResult[gameType] + 1) % lastResultsNumElem[gameType];
            }
        }

        string teamAname = "";
        string teamBName = "";

        getTeamsNamesByIdx(gameType,
                           eventLine,
                           ref teamAname,
                           ref teamBName);

        lastResults[gameType, tailLastResult[gameType]] =
                    score1.ToString() + "-" +
                    //groups[gameType][teamA][1] + "-" +
                    //groups[gameType][teamB][1] + "-" +
                    teamAname + "-" +
                    teamBName + "-" +
                    score2.ToString();

        tailLastResult[gameType] = (tailLastResult[gameType] + 1) % lastResultsNumElem[gameType];

        if ((headLastResult[gameType] == tailLastResult[gameType]))
        {
            headLastResult[gameType] = (headLastResult[gameType] + 1) % lastResultsNumElem[gameType];
        }

        numUpdateLastResult[gameType] = true;
    }

    private void printLastRes(int gameType)
    {       
        int row = 1;
        int idx = 0;

        for (int i = 0; i < lastResultsNrRows; i++)
        {
            Color color = new Color(1, 1, 1, 0);
            lastResultFlag[i, 0].color = color;
            lastResultFlag[i, 1].color = color;
            for (int j = 0; j < lastResultsNrCols; j++)
            {
                //print("TEXTLASRES " + textLastResult[i, j] + " I " + i + " J " + j);
                textLastResult[i, j].text = "";                
            }
        }
            
        if (tailLastResult[gameType] != headLastResult[gameType])
        {
            string tmpLastResults = "";
            int first = headLastResult[gameType];
            int last = tailLastResult[gameType];

            while (first != last)
            {
                string[] teamsRes = lastResults[gameType, first].Split('-');
                // Leave a space - col 2 and 3 for flags 
                string teamA = teamsRes[1];
                string teamB = teamsRes[2];

                string currTeam = "";

                int generalIdx = 0;

                for (int col = 0; col < lastResultsNrCols; col++)
                {
                    //print("last result idx|col " + idx + " col " + col
                    //    + " lastResultsNrCols " + lastResultsNrCols);
                    textLastResult[idx, col].text = teamsRes[col];
                }

                Color color = new Color(1, 1, 1, 1);
                lastResultFlag[idx, 0].color = color;
                lastResultFlag[idx, 1].color = color;
                lastResultFlag[idx, 0].texture = getFlag(teamA, gameType);
                lastResultFlag[idx, 1].texture = getFlag(teamB, gameType);

                first = (first + 1) % lastResultsNumElem[gameType];
                idx++;
            }
        }
    }

    public void printNextMatches(int gameType)
    {
        //int printMatches;
        //int startRow = 9;
        //int row = startRow;
        //int maxRows = 4;
        int idx = 0;
        //string tmpNextMatches = "";

        for (int i = 0; i < nextMatchesNrRows; i++)
        {
            Color color = new Color(1, 1, 1, 0);

            nextMatchesFlag[i, 0].color = color;
            nextMatchesFlag[i, 1].color = color;

            for (int j = 0; j < nextMatchesNrCols; j++)
                textNextMatches[i, j].text = "";
        }

        //if (isKnockOutStage == true)
        //    printMatches = 1;
        //else
        //    printMatches = 2;

        //if (gamesIdx[gameType] >= games.GetLength(1))
        //    return;
        if (games[gameType].Count == 0 ||
            isLeagueActive[gameType] == false)
            return;

        //for (int i = gamesIdx[gameType]; i < games.GetLength(1); i++) {
        //     if (games[gameType, i].Equals("0-0"))
        //        break;

        for (int i = 0; i < games[gameType].Count; i++) {
            string eventLine = games[gameType][i];

            //print("EVENTLINE " + eventLine);

            //string[] teamsIdx = games[gameType, i].Split('-');
            string[] match = eventLine.Split('|');
            if (match[0].Equals("0-0"))
                break;

            string[] teams = match[0].Split('-');
            int teamAIdx = int.Parse(teams[0]);
            int teamBIdx = int.Parse(teams[1]);

            /*if (match[1].Equals("C"))
            {
                //it's player team
                if (teamA == plrTeamCupId)
                {
                    teamsIdx[0] = player1TeamName;
                    teamsIdx[1] = 
                            orgTeams[gameType].getTeamByIndex(teamB)[0];
                }
                else if (teamB == plrTeamCupId)
                {
                    teamsIdx[1] = player1TeamName;
                    teamsIdx[0] = 
                        orgTeams[gameType].getTeamByIndex(teamA)[0];
                } 
                //orgTeams[gameType].getTeamByIndex(teamA)[0];
                //teamsIdx[1] =
                //    orgTeams[gameType].getTeamByIndex(teamB)[0];
            }
            else
            {
                teamsIdx[0] = groups[gameType][teamA][1];
                teamsIdx[1] = groups[gameType][teamB][1];
            }*/

            getTeamsNamesByIdx(gameType, 
                               eventLine, 
                               ref teams[0], 
                               ref teams[1]);

            //print("NEXTMATCH " + teams[0] + " vs " + teams[1] + " # ");

            for (int col = 0; col < nextMatchesNrCols; col++)
            {
                textNextMatches[idx, col].text = teams[col];
            }

            Color color = new Color(1, 1, 1, 1);
            nextMatchesFlag[idx, 0].color = color;
            nextMatchesFlag[idx, 1].color = color;
            nextMatchesFlag[idx, 0].texture = getFlag(teams[0], gameType);
            nextMatchesFlag[idx, 1].texture = getFlag(teams[1], gameType);

            idx++;
            //string teamAName;
        }
        //}
    }

    public void onClickLoadMainMenu()
    {
        SceneManager.LoadScene("menu");
        return;
    }

    private void onClickInfoExitButton(string eventType)
    {
        if (eventType.Equals("gameEnd"))
        {
            Globals.recoverOriginalResolution();
            SceneManager.LoadScene("menu");

            //print("LOAD MAIN MENU");

        } else if (eventType.Equals("closeInfo"))
        {
            infoPanel.SetActive(false);
        } else if (eventType.Equals("newSeason"))
        {           
            Globals.loadSceneWithBarLoader("Leagues");
        }
    }

    private void setNewSeasonPrefs()
    {
        //leagueSeason++;
        //Globals.leagueSeason = leagueSeason;
        //Globals.isNewSeason = true;
        //isChampCupActive = Globals.champCupPromotedNextSeason;
        //leagueName = Globals.leagueName;
        //player1TeamName = Globals.playerTeamName;
        //playerTeamID = Globals.playerTeamId;
        //leagueSeason = Globals.leagueSeason;

        //leagueSeason++;
        //PlayerPrefs.SetInt(savedFileName + "_season", leagueSeason);
        //PlayerPrefs.SetInt(savedFileName + "_isChampCupActive", champCupPromotedNextSeason);
        //PlayerPrefs.SetInt(savedFileName + "_playerTeamID", playerTeamID);
        //PlayerPrefs.SetInt(savedFileName + "_player1TeamName", player1TeamName);
        PlayerPrefs.SetInt(savedFileName + "_isNewSeason", 1);
        //PlayerPrefs.SetString(savedFileName + "_leagueName", leagueName);
        PlayerPrefs.Save();
    }
    
    private void onActivityResult()
    {
        //print("NEXTMATCH ONACTIVITY START");

        int teamA = TEAM1_ID;
        int teamB = TEAM2_ID;

        int score1 = Globals.score1;
        int score2 = Globals.score2;

        //fake score TO DELETE
        //if (Globals.playerPlayAway)
        //{
        //    score2 = 2;
        //} else
        //{
        //    score1 = 2;
        //}

        //print("NEXTMATCH SCORE " + score1 + " : " + score2);
        //print("DEBUGONACTIVITY onActivityResult 1");

        //if (score1 == -1 || score2 == -1)
        //    /*TODO EXCEPTION*/
        //    print("WRONG SCORE ERROR");
        
        int groupIdx = 1;
        nMatchesPlayed[activeGroup]++;
    
        string[] eventLine = playerGameSaved.Split('|');
        string matchType = eventLine[1];
        string matchTypeDesc = eventLine[2];

        if (matchType.Equals("G")) {
            updateTable(teamA, teamB, score1, score2, activeGroup);
        }

        //print("ONACTIVITYRESULT PRINTRESTABLE");
        printResTable((2 * teamsPerGroup[activeGroup]) + 2, tableColumns - 2, activeGroup);

        //print("DEBUG PLAYER POST IN GROUP " + Player1PosinGroup);
        if (!dontUpdateScore)
            updateLastResults(teamA, teamB, score1, score2, playerGameSaved, activeGroup);
        printLastRes(activeGroup);

        //bool isAnyActive = false;
        //for (int i = 1; i <= numCompetitions; i++)
        //{
        //    if (isLeagueActive[group])
        //        isAnyActive = true;
        //}
 
        string eventType = "NO_EVENT";
        if (!dontUpdateScore)
        {
            if (gameCategory[activeGroup].Equals("CUP"))
            {
                if ((matchType.Equals("C") && Globals.didPlayerLose(score1, score2)) ||
                    (matchType.Equals("G") &&
                     numMatchesInGroup[activeGroup] == nMatchesPlayed[activeGroup] &&
                     playerPosInGroup[activeGroup] > teamQualifiedNum[activeGroup]))
                {                
                    string cupName =
                        getCupName(numCompetitions, activeGroup);

                    infoPanelHeaderText.text = 
                        Languages.getTranslate(cupName + " Summary");
                    infoText.text = 
                        Languages.getTranslate(
                            player1TeamName + " was knocked out of the " +
                            cupName +
                            " in the " + matchTypeDesc,
                            new List<string>() { player1TeamName, cupName, matchTypeDesc });

                    achievement[activeGroup] = matchTypeDesc;
                    isLeagueActive[activeGroup] = false;

                    eventType = "cupLose";
                }
                else
                {
                    if (matchTypeDesc.Equals("Final"))
                    {
                        achievement[activeGroup] = "WON_FINAL";
                  
                        if (numCompetitions == 1)
                        {
                            Globals.gameEnded = true;
                            Globals.wonCupName = leagueName;
                            //if that's cup that's end of the game - cleanup
                            Globals.deleteGameSave(savedFileName, 
                                                   savedGamesFileName,
                                                   leagueName,
                                                   player1TeamName);
                        } else
                        {
                            if (activeGroup == 2)
                            {
                                Globals.wonCupName = "CHAMP CUP";
                            } else if (activeGroup == 3)
                            {
                                Globals.wonCupName = "LEAGUE CUP";
                            }
                        }

                        dontUpdateScore = true;
                        Globals.addWonCup(1);
                        finalWon = true;
                        SceneManager.LoadScene("fireworks");                      
                        return;
                    }
                }
            }
            else
            {
                if (numMatches[activeGroup] == nMatchesPlayed[activeGroup])
                {
                    if (playerPosInGroup[activeGroup] <= teamQualifiedNum[activeGroup])
                    {
                 
                        achievement[activeGroup] = "YES_LEAGUE_QUALIFIED";
                        Globals.champCupPromotedNextSeason = true;
                        nextSeasonChampCup = true;
                        //isChampCupActive = true;

                        //you won the league
                        if (playerPosInGroup[activeGroup] == 1)
                        {
                            Globals.addWonCup(1);
                            Globals.wonCupName = "LEAGUE";
                            dontUpdateScore = true;
                            SceneManager.LoadScene("fireworks");
                            return;
                        }
                    }
                    else
                    {
                        achievement[activeGroup] = "NO_LEAGUE_QUALIFIED";
                        nextSeasonChampCup = false;
                        Globals.champCupPromotedNextSeason = false;
                        //isChampCupActive = false;
                    }
                }
            }
        }

        infoPanelExitButton.onClick.RemoveAllListeners();
        if (eventType.Equals("cupLose") &&
            numCompetitions == 1)
        {
            infoPanel.SetActive(true);
            Globals.deleteGameSave(savedFileName, 
                                   savedGamesFileName, 
                                   leagueName,
                                   player1TeamName);
            infoPanelExitButtonText.text = 
                Languages.getTranslate("Main Menu");
            infoPanelExitButton.onClick.AddListener(
                        delegate
                        {
                            onClickInfoExitButton("gameEnd");
                        });
        }
        else
        {
            if (isEndOfGame())
            {
                infoPanel.SetActive(true);
                infoText.text = getEndSeasonInfoStr();
                infoPanelExitButtonText.text = 
                    Languages.getTranslate("New Season");

                Globals.isNewSeason = true;
                leagueSeason++;
                Globals.leagueSeason = leagueSeason;
                leagueWeek = 1;
                isChampCupActive = nextSeasonChampCup;

                setSavePrefs();
                setNewSeasonPrefs();

                infoPanelExitButton.onClick.AddListener(
                        delegate
                        {                           
                            onClickInfoExitButton("newSeason");
                        });
            }
            else if (!eventType.Equals("NO_EVENT"))
            {
                infoPanel.SetActive(true);
                infoPanelExitButtonText.text = "OK";
                infoPanelExitButton.onClick.AddListener(
                delegate
                {
                    onClickInfoExitButton("closeInfo");
                });
            }
        }
        
        //print("NEXTMATCH BEFORE");
        printNextMatches(activeGroup);

        //print("NEXTMATCH AFTER");
        printPlayerNextMatch();
        disableUpButtonFocusApartFrom(activeGroup-1);

        dontUpdateScore = false;  
        return;       
    }

    private string getCupName(int numCompetitions, int group)
    {
        if (numCompetitions == 1) {
            return leagueName;
        }

        if (group == 2)
            return "CHAMP CUP";

        if (group == 3)
            return "LEAGUE CUP";

        return "CUP";
    }

    private string getEndSeasonInfoStr()
    {
        string info = 
            Languages.getTranslate("League: " + player1TeamName + " has been ",
                                    new List<string>() { player1TeamName });
        infoPanelHeaderText.text = 
            Languages.getTranslate("Season summary");

        if (achievement[1].Equals("NO_LEAGUE_QUALIFIED"))
        {
            info = Languages.getTranslate(
                "League: " + player1TeamName + " has not been ",
                new List<string>() { player1TeamName });
        }
        info += Languages.getTranslate(
            "qualified to champ cup! ");
        info += Languages.getTranslate(
            player1TeamName + " finished at " + playerPosInGroup[1].ToString() + " position",
            new List<string>() { player1TeamName, playerPosInGroup[1].ToString() });
        info += "\n";

        if (isChampCupActive) {
            info += 
                Languages.getTranslate("Champ Cup: " + player1TeamName);
            if (achievement[2].Equals("WON_FINAL")) {
                info += 
                    Languages.getTranslate(" won a champ cup!");
            } else
            {
                info += 
                    Languages.getTranslate(" was knockout in " + achievement[2]);
            }
            info += "\n";
        }
        info += 
            Languages.getTranslate("League Cup: " + player1TeamName);
        if (achievement[3].Equals("WON_FINAL"))
        {
            info += Languages.getTranslate(" won a League Cup");
        }
        else
        {
            info += Languages.getTranslate(" was knockout in " + achievement[3]);
        }

        return info;
    }

    private bool isEndOfGame()
    {  
        for (int group = 1; group <= numCompetitions; group++) {
            if (!isLeagueActive[group])
                continue;

            if (nMatchesPlayed[group] < numMatches[group])
                return false;
        }
    
        return true;
    }

    private void OnClickEventShowGoalResizePanel()
    {
        if (!Globals.cpuGoalSize.Equals("STANDARD"))
            return;

        shopPanel.SetActive(true);
    }

    private void OnClickGoalEnlargeBuyButton()
    {
        int enalrgeGoalMediumCreditsNum =
            PlayerPrefs.GetInt("enlargeGoal_MEDIUM_CREDITS");

        if ((PlayerPrefs.HasKey("enlargeGoal_MEDIUM_CREDITS") &&
             enalrgeGoalMediumCreditsNum > 0))
        {
            //Globals.enlargeGoalSize("MEDIUM");
            //Globals.purchasesQueue.Enqueue(new PurchaseItem("enlargegoalsize_medium", "MEDIUM"));
            enalrgeGoalMediumCreditsNum--;
            PlayerPrefs.SetInt("enlargeGoal_MEDIUM_CREDITS", enalrgeGoalMediumCreditsNum);
            PlayerPrefs.Save();
        }
        else
        {
            //IAPManager.instance.buyEnlargegoalsizeMedium();
        }

        shopPanel.SetActive(false);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("ENLARGEGOAL_BUTTON_CLICK_EVENT", new Dictionary<string, object>
            {
                    { "enlargeButtonCLICKED", true},
            });
        }
    }

    private void disableUpButtonFocusApartFrom(int idx)
    {
        for (int i = 0; i < numButtonsUp; i++)
        {
            if (i == idx)
            {
                mainButtonText[i].color = new Color32(241, 234, 203, 255);
                mainButtonsFocus[i].SetActive(true);
                continue;
            }

            mainButtonText[i].color = new Color32(74, 172, 247, 255);
            //print("DISABLE FOCUS FROM " + i + " Index " + idx);
            mainButtonsFocus[i].SetActive(false);
        }
    }

    private void onClickEventCloseShopPanel()
    {
        shopPanel.SetActive(false);
    }

    private void onClickShopNotificationClose()
    {
        shopPanel.SetActive(false);
        shopNotificationCanvas.SetActive(false);
    }

    /*public void showNotification(PurchaseItem item)
    {
        string type = item.name;

        if (type.Contains("enlargegoalsize_medium"))
        {
            shopNotificationCanvas.SetActive(true);
        }
    */
        //{
        //notificationText.text =
        //         "Excellent! Ads disabled!";
        //     notificationImage.texture =
        //         Resources.Load<Texture2D>("Shop/adsRemovedIcon");
        //}

        //print("TYPE ITEM " + item.name);
        //notificationHeaderText.text = "Congratulations!";
        //audioManager.Play("elementAppear");

        //if (type.Contains("enlargegoalsize_medium"))
        //{
        //notificationText.text =
        //         "Excellent! Ads disabled!";
        //     notificationImage.texture =
        //         Resources.Load<Texture2D>("Shop/adsRemovedIcon");
        //}
    //}

    /*REMOVEIT*/
    private void updateTemporarySettings()
    {
        Globals.matchTime = "1 SECONDS";

        /*for (int i = 0; i < 2; i++)
        {    
            if (i == 0)
            {
                Globals.teamAname = "Argentina";
                Globals.teamAid = 1;
                Globals.stadiumColorTeamA = 1;
            }
            else
            {
                Globals.teamBname = "Argentina";
                Globals.teamBid = 1;
                Globals.stadiumColorTeamB = 1;
            }
        }

        Globals.teamAcumulativeStrength = 180;
        Globals.teamBcumulativeStrength = 180;

        Globals.teamAGkStrength = 90;
        Globals.teamBGkStrength = 80;

        Globals.teamAAttackStrength = 90;
        Globals.teamBAttackStrength = 90;
        Globals.matchTime = "3 SECONDS";
        Globals.joystickSide = "RIGHT";
        Globals.isFriendly = false;
        Globals.isLeague = true;
        Globals.level = 3;*/
    }

    private void loadGameFromSave()
    {
        //print("#DBG Application.persistentDataPath " + Application.persistentDataPath);

        //string json = File.ReadAllText(Path + fileName);
        //string json = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
        //print("JSON LOAD FROM FILE " + json);
        //classRef = JsonUtility.FromJson<MatchTableMain>(json);

        MatchTableMainSerialize matchSerialized;
        try
        {
             matchSerialized =
                matchTableSerialize.loadSave();
        }
        catch (Exception e)
        {
            cannotLoadSavePanel.SetActive(true);
            return;
        }

        leagueWeek = matchSerialized.leagueWeek;
        leagueSeason = matchSerialized.leagueSeason;
        numCompetitions = matchSerialized.numCompetitions;
        numButtonsUp = matchSerialized.numButtonsUp;
        //Globals.isNewSeason = matchSerialized.isNewSeason;
        //isNewSeason = matchSerialized.isNewSeason;
        playerTeamID = matchSerialized.playerTeamID;
        player1TeamName = matchSerialized.player1TeamName;
        isChampCupActive = matchSerialized.isChampCupActive;
        leagueName = matchSerialized.leagueName;
        //dontUpdateScore = matchSerialized.dontUpdateScore;


        if (PlayerPrefs.HasKey(savedFileName + "_stadiumNumber"))
        {
            Globals.stadiumNumber =
                PlayerPrefs.GetInt(savedFileName + "_stadiumNumber");
        }
        else
        {
            Globals.stadiumNumber = 0;
        }

        for (int i = 1; i <= numCompetitions; i++)
        {
            games[i].Clear();
        }

        //print("DESERIALIZE teamsPerGroup" + matchSerialized.teamsPerGroup);
        //print("DESERIALIZE BEFORE ");
        //foreach (string eventLine in matchSerialized.games)
        //{
        //    print("DESERIALIZE GAMES " + eventLine);
        //}
        //print("DESERIALIZE AFTER ");

        foreach (string eventLine in matchSerialized.games)
        {
            string[] match = eventLine.Split('|');
            int group = int.Parse(match[0]);

            //print("DESERIALIZE GAMES ## group " + group + " match " 
            //    + eventLine.Substring(eventLine.IndexOf('|') + 1));
            games[group].Add(
                eventLine.Substring(eventLine.IndexOf('|') + 1));
            //gamesSerialize.Add(i.ToString + "|" + match);
        }

        matchesSchedule.Clear();
        foreach (MATCHTYPE matchSchedule in matchSerialized.matchesSchedule)
        {
            matchesSchedule.Add(matchSchedule);
        }

        /*GAMES RECOVERED*/
        //for (int i = 1; i <= numCompetitions; i++)
       // {
        //    print("DESERIALIZE GAMES BEFORE " + i.ToString());
        //    foreach (string eventLine in games[i])
        //    {
        //        print(eventLine);                
         //   }
         //   print("DESERIALIZE GAMES AFTER " + i.ToString());
       //}

        int[] groupRowIdx = new int[numCompetitions + 1];
        for (int i = 1; i <= numCompetitions; i++)
            groupRowIdx[i] = 1;

        foreach (string teamLine in matchSerialized.groups)
        {
            string[] team = teamLine.Split('|');
            int group = int.Parse(team[0]);
            //print("DESERIALIZE GROUPS " + group + " teamLINE " + team);

            for (int col = 1; col < team.Length; col++)
            {
                //print("DESERIALIZE TEAM COL " + col + " team[col] " + team[col]);
                groups[group][groupRowIdx[group]][col - 1] = team[col];
            }
            groupRowIdx[group]++;
        }

        //for (int group = 1; group <= numCompetitions; group++) {
        //    print("SERIALIZATION_LOAD GROUP " + group.ToString());
        //    for (int row = 1; row <= teamsPerGroup[group]; row++) {
        //        for (int col = 0; col < tableColumns; col++) {
        //            print(groups[group][row][col]);
        //        }
        //        print("\n");
        //    }
        //}

        for (int i = 1; i <= numCompetitions; i++)
            groupRowIdx[i] = 0;
        foreach (string lastResultLine in matchSerialized.lastResults)
        {
            string[] match = lastResultLine.Split('|');
            if (!String.IsNullOrEmpty(match[1]))
            {
                int group = int.Parse(match[0]);
                lastResults[group, groupRowIdx[group]++] = match[1];
                //print("SERIALIZATION_LOAD LAST RESULT " + group.ToString() + " res " +
                //    lastResults[group, groupRowIdx[group] - 1]);
            }
        }

        matchSerialized.teamsPerGroup.CopyTo(teamsPerGroup, 0);
        //for (int i = 1; i < teamsPerGroup.Length; i++)
        //    print("TeamsPerGroup " + teamsPerGroup[i]);

        matchSerialized.lastResultsNumElem.CopyTo(lastResultsNumElem, 0);
        matchSerialized.headLastResult.CopyTo(headLastResult, 0);
        matchSerialized.tailLastResult.CopyTo(tailLastResult, 0);
        matchSerialized.numUpdateLastResult.CopyTo(numUpdateLastResult, 0);
        matchSerialized.isLeagueActive.CopyTo(isLeagueActive, 0);
        matchSerialized.nMatchesPlayed.CopyTo(nMatchesPlayed, 0);
        matchSerialized.numMatches.CopyTo(numMatches, 0);
        matchSerialized.numMatchesInGroup.CopyTo(numMatchesInGroup, 0);
        matchSerialized.achievement.CopyTo(achievement, 0);

        foreach (string matchLine in matchSerialized.groupResultsMap)
        {
            string[] match = matchLine.Split('|');
            string key = match[0];
            string value = match[1];
            //print("DESERIALIZE ADD KEY: " + key + " VAL: " + value);
            insertResultHashMap(key, value);
        }
   
        string savedGamesList =
                 PlayerPrefs.GetString(savedGamesFileName);
        //print("DBGLOADSAVED add to end " + savedFileName + " list " + savedGamesList);

        //savedGamesList = orgTeams[1].rearrangeToEndList(savedGamesList,
        //                                                savedFileName,
        //                                                "|");
        savedGamesList = Globals.rearrangeToEndList(savedGamesList,
                                                    savedFileName,
                                                    "|");

        //print("DBGLOADSAVED after exchanged " + savedFileName + " list " + savedGamesList);
        PlayerPrefs.SetString(savedGamesFileName, savedGamesList);
        PlayerPrefs.Save();

        playerTeamDesc = orgTeams[1].getTeamByIndex(playerTeamID);
    }

    private void saveGameState()
    {
        //serialize games list
        matchTableSerialize.numCompetitions = numCompetitions;
        matchTableSerialize.numButtonsUp = numButtonsUp;
        matchTableSerialize.leagueWeek = leagueWeek;
        matchTableSerialize.leagueSeason = leagueSeason;
        //matchTableSerialize.isNewSeason = Globals.isNewSeason;
        matchTableSerialize.playerTeamID = playerTeamID;
        matchTableSerialize.player1TeamName = player1TeamName;
        matchTableSerialize.isChampCupActive = isChampCupActive;
        matchTableSerialize.leagueName = leagueName;
        //matchTableSerialize.dontUpdateScore = dontUpdateScore;

        matchTableSerialize.games.Clear();
        for (int i = 1; i <= numCompetitions; i++) {
            foreach (string match in games[i])
            {
                matchTableSerialize.games.Add(i.ToString() + "|" + match);
            }
        }

        matchTableSerialize.matchesSchedule.Clear();        
        foreach (MATCHTYPE matchSchedule in matchesSchedule)
        {
                matchTableSerialize.matchesSchedule.Add(matchSchedule);
        }

        //foreach (string el in matchTableSerialize.games)
        //{
        //    print("SERIALIZATION_SAVE LIST GAMES " + el);
        //}

        //Serialize group
        matchTableSerialize.groups.Clear();
        for (int group = 1; group <= numCompetitions; group++)
        {
            for (int row = 1; row <= teamsPerGroup[group]; row++)
            {
                string fullRow = group.ToString() + "|";
                for (int col = 0; col < tableColumns; col++)
                {
                    fullRow += groups[group][row][col] + "|";
                }

                fullRow = fullRow.Remove(fullRow.Length - 1);
                matchTableSerialize.groups.Add(fullRow);
            }
        }

        matchTableSerialize.lastResults.Clear();

        //print("DBG123# NUMCOMPETITIONS " + numCompetitions);
        for (int group = 1; group <= numCompetitions; group++)
        {
            //print("DBG123# lastResultsNumElem[group] " + lastResultsNumElem[group]);
            for (int j = 0; j < lastResultsNumElem[group]; j++)
            {
                //print("DBG123# lastResults[group, j] " + lastResults[group, j]);
                matchTableSerialize.lastResults.Add(
                    group.ToString() + "|" + lastResults[group, j]);
            }
        }

        teamsPerGroup.CopyTo(matchTableSerialize.teamsPerGroup, 0);
        lastResultsNumElem.CopyTo(matchTableSerialize.lastResultsNumElem, 0);
        headLastResult.CopyTo(matchTableSerialize.headLastResult, 0);
        tailLastResult.CopyTo(matchTableSerialize.tailLastResult, 0);
        numUpdateLastResult.CopyTo(matchTableSerialize.numUpdateLastResult, 0);
        isLeagueActive.CopyTo(matchTableSerialize.isLeagueActive, 0);
        nMatchesPlayed.CopyTo(matchTableSerialize.nMatchesPlayed, 0);
        numMatchesInGroup.CopyTo(matchTableSerialize.numMatchesInGroup, 0);
        numMatches.CopyTo(matchTableSerialize.numMatches, 0);
        achievement.CopyTo(matchTableSerialize.achievement, 0);

        //foreach (string el in matchTableSerialize.groups)
        //{
        //    print("SERIALIZATION_SAVE LIST GROUP " + el);
        //}

        groupResultsMap.Clear();
        foreach (var match in groupResultsMap)
        {
            matchTableSerialize.groupResultsMap.Add(match.Key + "|" + match.Value);
        }

        matchTableSerialize.saveGame();

        string savedGamesList = "";
        if (Globals.isNewGame)
        {
            if (PlayerPrefs.HasKey(savedGamesFileName))
            {
                savedGamesList =
                    PlayerPrefs.GetString(savedGamesFileName);
                savedGamesList += "|" + savedFileName;
            } else
            {
                savedGamesList = savedFileName;
            }

            //print("DBGLOADSAVED savedGamesList NEW " + savedGamesList);
            //PlayerPrefs.SetString("savedGameList", savedGamesList);           
            //PlayerPrefs.Save();

            leagueSeason = Globals.leagueSeason;
            PlayerPrefs.SetString(savedGamesFileName, savedGamesList);

            //print("#DBGLEVEL Saved file name level" + (savedFileName + "_levelsIdx")
            //    + " globals.Level " + (Globals.level - 1));

            //if (!PlayerPrefs.HasKey(savedFileName + "_levelsIdx"))

            PlayerPrefs.SetInt(savedFileName + "_levelsIdx", Globals.levelIdx);
            PlayerPrefs.SetInt(savedFileName + "_gameTimesIdx", Globals.gameTimesIdx);
            PlayerPrefs.SetInt(savedFileName + "_trainingModeIdx", Globals.trainingModeIdx);
            PlayerPrefs.SetInt(savedFileName + "_graphicsSettingsIdx", Globals.graphicsSettingsIdx);
            PlayerPrefs.SetInt(savedFileName + "_joystickSideIdx", Globals.joystickSideIdx);
            PlayerPrefs.SetInt(savedFileName + "_stadiumNumber", Globals.stadiumNumber);

            PlayerPrefs.Save();

            //TOCHECK GLOBALS.teamAid

        }

        //print("#DBGPLAYER1TEAMNAME_TEST1 " + Globals.isNewGame +
        //    " isNewSeason " + isNewSeason);

        if (Globals.isNewGame ||
            isNewSeason == 1) {
            //Globals.isNewSeason) {
                if (Globals.isPlayerCardLeague(leagueName))
                {
                    //print("#DBGPLAYER1TEAMNAME_TEST1 ENERGY " +
                    //    "ORRGTEMS " + Globals.teamAid + " orgTeams[1] " + orgTeams[1].getMaxTeams());
                    //teamManagement.initPlayerEnergy(
                       // orgTeams[1].getTeamByIndex(Globals.teamAid)[12], savedFileName);
                    teamManagement.initPlayerEnergy(
                         orgTeams[1].getTeamByIndex(playerTeamID)[12], savedFileName);
                }
        }

        Globals.isNewGame = false;
        setSavePrefs();

    }

    private void setSavePrefs()
    {
        string entryValue = savedFileName + "|" +
                            player1TeamName + "|" +
                            leagueWeek + "|" +
                            leagueSeason + "|" +
                            playerTeamID + "|" +
                            isChampCupActive.ToString();
                            //Globals.champCupPromotedNextSeason.ToString();

        //print("DBGLOADSAVEDENTRY saved entry value " + entryValue);

        PlayerPrefs.SetString(savedFileName, entryValue);
        PlayerPrefs.Save();
    }

    private string findNameOfSaved(string leagueName)
    {
        int leagueID = 1;
        string keyName = leagueName + "_savedLastID";

        if (PlayerPrefs.HasKey(keyName))
        {
            leagueID = PlayerPrefs.GetInt(keyName);
            leagueID++;
        }

        //print("DBGLOADSAVED leagueID " + leagueID + " leagueName " + keyName
        //    + " FILENAME " + leagueName + "_" + leagueID.ToString());

        PlayerPrefs.SetInt(keyName, leagueID);
        PlayerPrefs.Save();

        return leagueName + "_" + leagueID.ToString();
    }
}

[Serializable]
public class MatchTableMainSerialize
{  
    public int numCompetitions;
    public int numButtonsUp;
    //public bool isNewSeason;
    private string saveName;
    public List<string> games;
    public List<string> groups;
    public List<string> groupResultsMap;
    public int[] teamsPerGroup;
    public List<string> lastResults;
    public int[] lastResultsNumElem;
    public int[] headLastResult;
    public int[] tailLastResult;
    public bool[] numUpdateLastResult;
    public bool[] isLeagueActive;
    public List<MATCHTYPE> matchesSchedule;
    //public bool dontUpdateScore;

    public int[] numMatches;
    public int[] numMatchesInGroup;
    public int[] nMatchesPlayed;

    public int leagueWeek = 1;
    public int leagueSeason = 20;
    public int playerTeamID;
    public string player1TeamName;
    public string leagueName;
    public bool isChampCupActive;

    public string[] achievement;

    public MatchTableMainSerialize(string fileName, 
                                   int numCompetitions)
    {
        //Debug.Log("#DBG1 num competitons " + numCompetitions);
        this.numCompetitions = numCompetitions;

        games = new List<string>();
        groups = new List<string>();
        groupResultsMap = new List<string>();
        lastResultsNumElem = new int[numCompetitions + 1];

        lastResults = new List<string>();
        headLastResult = new int[numCompetitions + 1];
        tailLastResult = new int[numCompetitions + 1];
        numUpdateLastResult = new bool[numCompetitions + 1];
        isLeagueActive = new bool[numCompetitions + 1];

        matchesSchedule = new List<MATCHTYPE>();

        teamsPerGroup = new int[numCompetitions + 1];
        nMatchesPlayed = new int[numCompetitions + 1];
        numMatches = new int[numCompetitions + 1];
        numMatchesInGroup = new int[numCompetitions + 1];
        achievement = new string[numCompetitions + 1];

        saveName = fileName;
    }

    public void saveGame()
    {        
        string classDataSerialize = JsonUtility.ToJson(this);
        //Debug.Log("SAVENAME##FILENAME " + Application.persistentDataPath + "/" + saveName);
        System.IO.File.WriteAllText(
            Application.persistentDataPath + "/" + saveName, classDataSerialize);
        //Debug.Log("SERIALIZATION SAVE " + classDataSerialize);
    }

    public MatchTableMainSerialize loadSave()
    {
        //Debug.Log("Application.persistentDataPath " + Application.persistentDataPath);
        string json = 
            File.ReadAllText(Application.persistentDataPath + "/" + saveName);
        return JsonUtility.FromJson<MatchTableMainSerialize>(json);
        //Debug.Log("SERIALIZATION LOAD " + json);
    }
}

