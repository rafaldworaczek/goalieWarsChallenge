using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using System.Text.RegularExpressions;
using MenuCustomNS;
using UnityEngine.UI;
using TMPro;
using graphicsCommonNS;
using UnityEngine.SceneManagement;
using System;
using LANGUAGE_NS;
using AudioManagerNS;
using UnityEngine.Localization.Components;

public class teamManagement : MonoBehaviour
{
    private int MAX_ENERGY = 100;
    private int MAX_TRANSFERS_ROWS = 7;
    private int MAX_PLAYERS_CARDS = 10;

    private int MIN_PLAYERS_PER_TEAM = 4;
    private AudioManager audioManager;

    private TextMeshProUGUI diamondsNumText;
    private TextMeshProUGUI coinsNumText;

    private GameObject transfersPanel;
    private GameObject buyPlayerPanel;
    private GameObject playerCardBigPanel;
    private GameObject sellPanelConfirm;
    private GameObject infoPanel;
    public GameObject shopPanel;
    private GameObject selectionPlrHelperPanel;

    public Shop shopScript;
    private TextMeshProUGUI infoPanelText;

    private int teamId;
    private Teams orgTeams;
    private GraphicsCommon graphics;
    private Dictionary<string, Texture2D> flagsHashMap;   
    private Teams[] leagues;
    private List<string> transferList;

    private TextMeshProUGUI playerCardBigNameText;
    private RawImage playerCardBigCountryFlag;
    private RawImage playerCardBigCardClubFlag;
    private Image playerCardBigDefenseSkillsBar;
    private Image playerCardBigAttackSkillsBar;
    private Image playerCardBigSkillsEnergyBar;
    private TextMeshProUGUI playerCardBigSkillsDefenseText;
    private TextMeshProUGUI playerCardBigSkillsAttackText;
    private TextMeshProUGUI playerCardBigSkillsEnergyText;
    private TextMeshProUGUI playerCardBigClubNameText;

    private TextMeshProUGUI buyPlayerHeaderText;
    private TextMeshProUGUI buyPlayerNameText;
    private TextMeshProUGUI buyPlayerCardNotificationText;
    private RawImage buyPlayerCardPlayerImage;
    private RawImage buyPlayerCardCountryFlag;
    private RawImage buyPlayerCardClubFlag;
    private GameObject buyPlayerCardClubFlagGameObj;
    private GameObject buyPlayerCardDiamondGameObj;
    private Image buyPlayerCardDefenseSkillsBar;
    private Image buyPlayerCardAttackSkillsBar;
    private TextMeshProUGUI buyPlayerCardSkillsDefenseText;
    private TextMeshProUGUI buyPlayerCardSkillsAttackText;
    private TextMeshProUGUI buyPlayerClubNameText;
    private TextMeshProUGUI buyPlayerClubDiamondPriceText;
    private Button buyPlayerCardYesButton;
    private TextMeshProUGUI buyPlayerCardYesButtonText;

    private int buyPlayerCardListIdx = 0;

    private string playerTeamName;
    private string playerLeagueName;
    private string saveFileName;

    private GameObject[] playerCard;
    private Button[] playerCardButton;
    private TextMeshProUGUI[] playerCardName;
    private Image[] playerCardPlayerStarImg;
    private RawImage[] playerCardPlayerImg;
    private RawImage[] playerCardFlag;
    private Image[] playerEnergySkillsBar;
    private TextMeshProUGUI[] playerCardSkills;

    private GameObject[] playerCardSelectionButtonGO;
    private Button[] playerCardSelectionButton;
    private RawImage[] playerCardSelectionImg;
    private TextMeshProUGUI[] playerCardPrice;

    private Image selectedPlayerCard1;
    private RawImage selectedPlayerCardCountryFlag1;
    private RawImage selectedPlayerCardImage1;
    private TextMeshProUGUI selectedPlayerCardSkills1;
    private TextMeshProUGUI selectedPlayerCardName1;
    private Image selectedPlayerCardEnergyBar1;

    private bool selectedPlayerCardFillActive = false;
    private string selectedPlayerName;

    private GameObject unlockedPlayerCardPanel;
    private GameObject unlockedPlayerCardPanelSmaller;
    private TextMeshProUGUI unlockedPlayerCardNameText;
    private TextMeshProUGUI unlockedPlayerCardSkillsText;
    private TextMeshProUGUI unlockedPlayerClubNameText;
    private RawImage unlockedPlayerCardCountryFlag;
    private RawImage unlockedPlayerCardClubFlag;
    private RawImage unlockedPlayerCardPlayerImage;

    private GameObject[] transferListRow;
    public RawImage extraPrizePlayerImg;
    public TextMeshProUGUI extraPrizeText;
    public GameObject extraPrizePanel;

    private Dictionary<string, string> playerSellOfferHash;
    private int numOfUnlockedPlayers = 0;
    public GameObject refillEnergyButtonGO;
    public GameObject refillEnergyPanel;

    void Start()
    {


        //Debug.Log("Globals.isTrainingActive team Management " + Globals.isTrainingActive);
  /*      print("#DBG1334 team management #### 1 " + Globals.teamAleague + " Globals.playerADesc "
 +
     Globals.playerADesc + " Globals.teamAname " + Globals.teamAname
    + " Globals team B " + Globals.teamBleague
    + " teamB Name " + Globals.teamBname
    + " temBIDx " + Globals.teamBid);*/

        playerSellOfferHash = new Dictionary<string, string>();
        audioManager = FindObjectOfType<AudioManager>();
        saveFileName = Globals.savedFileName;

        //if (Globals.isNewSeason)
        //{
        //    updatePlayersEnergy(true);
        //}

        playerLeagueName = Globals.leagueName;

        if (Globals.playerPlayAway)
            teamId = Globals.teamBid;
        else
            teamId = Globals.teamAid;

        Globals.shopTeamIdx = teamId;

        flagsHashMap = new Dictionary<string, Texture2D>();

        //print("DBGTRANSFER334 TEAMID " + teamId + " leaguName " + playerLeagueName);

        transfersPanel = GameObject.Find("transfersPanel");
        buyPlayerPanel = GameObject.Find("buyPlayerPanel");
        playerCardBigPanel = GameObject.Find("playerCardBigPanel");
        sellPanelConfirm = GameObject.Find("sellPanelConfirm");
        infoPanel = GameObject.Find("infoPanel");
        infoPanelText = GameObject.Find("infoPanelText").GetComponent<TextMeshProUGUI>();
        selectionPlrHelperPanel = GameObject.Find("selectionPlrHelperPanel");

        sellPanelConfirm.SetActive(false);
        infoPanel.SetActive(false);
        selectionPlrHelperPanel.SetActive(false);
        refillEnergyPanel.SetActive(false);

        orgTeams = new Teams(playerLeagueName);
        playerTeamName = orgTeams.getTeamByIndex(teamId)[0];

        //print("DBGTEAM1 PLAYERNAME " + playerTeamName + " Globals.leagueName " + Globals.leagueName
        //     + " teamId " + teamId);

        graphics = new GraphicsCommon();

        showExtraPrize();
        initReferenceTextObject();
        initPlayerCardsReferences();
        updateTeamGlobals();
        initPlayerCardBigReferences();
        initBuyPlayerCardReferences();
        initLeagues();
        initTransferList();
        setupDefaults();
    }

    public void updateCoinsText()
    {
        coinsNumText.text = (Globals.coins).ToString();
        diamondsNumText.text = (Globals.diamonds).ToString();
    }

    private void setupDefaults()
    {       
        string playerSelected = "";
        if (PlayerPrefs.HasKey(playerTeamName + "_lastSelectedPlayer"))
        {
            playerSelected =
                PlayerPrefs.GetString(playerTeamName + "_lastSelectedPlayer").Split(':')[0];
        }
        else
        {
            playerSelected =
                orgTeams.getTeamByIndex(teamId)[12].Split('|')[0].Split(':')[0];
        }

        int selectedPlrIdx = 
            initTeamPlayersCard(playerSelected, saveFileName);

        //print("#DBGSELECTED DEFAULT selected exist? " + PlayerPrefs.HasKey(playerTeamName + "_lastSelectedPlayer") +
//" playerSelected " + playerSelected + " idx " + selectedPlrIdx + "selectedPlayerName " + selectedPlayerName);
        dimPlayerSelectionApartFrom(selectedPlrIdx);

        updateCoinsText();
        if (Globals.isMultiplayer)
        {
            //LocalizeStringEvent localizedStringEvent = 
            //    GameObject.Find("playGameText").GetComponent<LocalizeStringEvent>();
            //localizedStringEvent.StringReference.SetReference("str/PLAY", "str/OK");
            GameObject.Find("playGameText").GetComponent<TextMeshProUGUI>().text = "OK";
        }
    }

    private void initLeagues()
    {
        int playerCardLeaguesNum = Globals.MAX_PLAYERS_CARD_LEAGUES;
        leagues = new Teams[playerCardLeaguesNum];

        print("Globals.isGameSettingActive teamMan " + Globals.isGameSettingActive);

        for (int i = 0; i < Globals.playerCardLeagues.Length; i++)
        {
            leagues[i] = 
                new Teams(Globals.playerCardLeagues[i]);
        }

        //leagues = new Teams[Globals.MAX_LEAGUES_NUM];
        //leagues[0] = new Teams("BRAZIL");
        //leagues[1] = new Teams("ENGLAND");
        //leagues[2] = new Teams("GERMANY");
        //leagues[3] = new Teams("ITALY");
        //leagues[4] = new Teams("SPAIN");
        //leagues[5] = new Teams("CHAMP CUP");
    }

    private void initReferenceTextObject()
    {
        diamondsNumText = GameObject.Find("diamondsNumText").GetComponent<TextMeshProUGUI>();
        coinsNumText = GameObject.Find("coinsNumText").GetComponent<TextMeshProUGUI>();
    }

    private void initTransferList()
    {
        transferList = new List<string>();

        for (int i = 0; i < MAX_TRANSFERS_ROWS; i++) {
            int randomLeague = UnityEngine.Random.Range(0, Globals.MAX_PLAYERS_CARD_LEAGUES);

            /*print("#DBGRANDOMLEAGUE " + randomLeague + " leagues[randomLeague].getMaxActiveTeams() " 
                + leagues[randomLeague].getMaxActiveTeams()
                + " Globals.isGameSettingActive " + Globals.isGameSettingActive);*/

            if (leagues[randomLeague].getMaxActiveTeams() > 0) {
                int randTeam =
                     UnityEngine.Random.Range(0, leagues[randomLeague].getMaxActiveTeams());
                string[] team = 
                    leagues[randomLeague].getTeamByIndex(randTeam);

                /*don't transfer player to the same team*/
                //if (team[i][0].Equals(playerTeamName))
                //print("#DBGTRANSFERLIST INIT TRANSFER " + team[0]);
                string[] players = team[12].Split('|');
                if (team[0].Equals(playerTeamName) ||
                    Globals.isTeamCustomize(team[0]) ||
                    players.Length <= MIN_PLAYERS_PER_TEAM)
                    continue;
               
                int randPlayer =
                    UnityEngine.Random.Range(1, players.Length);
              
                //print("DBGRANDOMLEAGUE players[randPlayer] len " + players.Length
               //     + " teamName " + team[0]
               //     + " randTeam " + randTeam
               //     + " randPlayer  " + randPlayer
               //     + " randomLeague " + randomLeague);

                if (int.Parse(players[randPlayer].Split(':')[5]) == 0 ||
                    players.Length <= MIN_PLAYERS_PER_TEAM)
                    continue;

                string playerToAdd =
                    team[0] + "|" + players[randPlayer] + "|"
                        + orgTeams.getLeagueNameByIndex(randomLeague);

                if (checkIfOnTheList(transferList, playerToAdd))
                    continue;
                
                transferList.Add(playerToAdd);
                leagues[randomLeague].swapElements(randTeam,
                                                   leagues[randomLeague].getMaxActiveTeams() - 1);
                //print("player added " + team[0] + "|" + players[randPlayer]);
            }
        }
    }
    
    //N complexity - use only for short Lists
    private bool checkIfOnTheList(List<string> list, 
                                  string desc)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(desc))
                return true;
        }

        return false;
    }

    IEnumerator selectionFillAmountAnimation(Image img)
    {
        selectedPlayerCardFillActive = false;
        for (int i = 1; i <= 100; i += 3) {
            if (selectedPlayerCardFillActive)
                break;
            img.fillAmount = (float) i / 100f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void initPlayerCardsReferences()
    {
        playerCard = new GameObject[MAX_PLAYERS_CARDS];
        playerCardButton = new Button[MAX_PLAYERS_CARDS];
        playerCardName = new TextMeshProUGUI[MAX_PLAYERS_CARDS];
        playerCardFlag = new RawImage[MAX_PLAYERS_CARDS];
        playerEnergySkillsBar = new Image[MAX_PLAYERS_CARDS];
        playerCardSkills = new TextMeshProUGUI[MAX_PLAYERS_CARDS];
        playerCardPlayerImg = new RawImage[MAX_PLAYERS_CARDS];
        playerCardPlayerStarImg = new Image[MAX_PLAYERS_CARDS];
        playerCardSelectionButtonGO = new GameObject[MAX_PLAYERS_CARDS];
        playerCardSelectionButton = new Button[MAX_PLAYERS_CARDS];
        playerCardSelectionImg = new RawImage[MAX_PLAYERS_CARDS];
        playerCardPrice = new TextMeshProUGUI[MAX_PLAYERS_CARDS];
        transferListRow = new GameObject[MAX_TRANSFERS_ROWS];

        for (int i = 0; i < MAX_PLAYERS_CARDS; i++)
        {
            playerCard[i] = GameObject.Find(
        "playerCard" + (i + 1).ToString());
            playerCardButton[i] =
                GameObject.Find(
        "playerCard" + (i + 1).ToString()).GetComponent<Button>();
            playerCardName[i] = GameObject.Find(
        "playerCardName" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();
            playerCardFlag[i] = GameObject.Find(
        "playerCardFlag" + (i + 1).ToString()).GetComponent<RawImage>();
            playerEnergySkillsBar[i] = GameObject.Find(
        "playerEnergySkillsBar" + (i + 1).ToString()).GetComponent<Image>();
            playerCardSkills[i] = GameObject.Find(
        "playerCardSkills" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();

            playerCardPlayerStarImg[i] = GameObject.Find(
        "playerCardPlayerStar" + (i + 1).ToString()).GetComponent<Image>();
            playerCardPlayerImg[i] = GameObject.Find(
        "playerCardPlayerImg" + (i + 1).ToString()).GetComponent<RawImage>();

            playerCardSelectionImg[i] = GameObject.Find(
        "playerCardSelectionImg" + (i + 1).ToString()).GetComponent<RawImage>();
            playerCardSelectionButtonGO[i] = GameObject.Find(
       "playerCardSelectionButton" + (i + 1).ToString());
            playerCardSelectionButton[i] = GameObject.Find(
        "playerCardSelectionButton" + (i + 1).ToString()).GetComponent<Button>();
            playerCardPrice[i] = GameObject.Find(
        "playerCardPrice" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();
        }

        selectedPlayerCardCountryFlag1 =
             GameObject.Find(
                "selectedPlayerCardCountryFlag1").GetComponent<RawImage>();
        selectedPlayerCardImage1 =
            GameObject.Find(
                "selectedPlayerCardImage1").GetComponent<RawImage>();
        selectedPlayerCardSkills1 = 
            GameObject.Find(
                "selectedPlayerCardSkills1").GetComponent<TextMeshProUGUI>();
        selectedPlayerCardName1 =
           GameObject.Find(
                "selectedPlayerCardName1").GetComponent<TextMeshProUGUI>();
        selectedPlayerCardEnergyBar1 =
            GameObject.Find(
                "selectedPlayerCardEnergyBar1").GetComponent<Image>();
        selectedPlayerCard1 =
            GameObject.Find(
                "selectedPlayerCard1").GetComponent<Image>();

        for (int i = 0; i < MAX_TRANSFERS_ROWS; i++)
            transferListRow[i] = 
                GameObject.Find("transferListRow" + (i + 1).ToString());
        transfersPanel.SetActive(false);

        initUnlockedPlayerCardReferences();
    }

    private void initBuyPlayerCardReferences()
    {
        buyPlayerHeaderText =
            GameObject.Find("buyPlayerHeaderText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardYesButton =
            GameObject.Find("buyPlayerCardYesButton").GetComponent<Button>();
        buyPlayerCardYesButtonText =
            GameObject.Find("buyPlayerCardYesButtonText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardNotificationText =
            GameObject.Find("buyPlayerCardNotificationText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardPlayerImage =
            GameObject.Find("buyPlayerCardPlayerImage").GetComponent<RawImage>();
        buyPlayerNameText =
            GameObject.Find("buyPlayerNameText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardCountryFlag = 
            GameObject.Find("buyPlayerCardCountryFlag").GetComponent<RawImage>();
        buyPlayerCardClubFlag = 
            GameObject.Find("buyPlayerCardClubFlag").GetComponent<RawImage>();
        buyPlayerCardClubFlagGameObj =
            GameObject.Find("buyPlayerCardClubFlag");
        buyPlayerCardDiamondGameObj =
            GameObject.Find("buyPlayerCardDiamondsImg");
        buyPlayerCardDefenseSkillsBar = 
            GameObject.Find("buyPlayerCardDefenseSkillsBar").GetComponent<Image>();
        buyPlayerCardAttackSkillsBar = 
            GameObject.Find("buyPlayerCardAttackSkillsBar").GetComponent<Image>();
        buyPlayerCardSkillsDefenseText =
            GameObject.Find("buyPlayerCardSkillsDefenseText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardSkillsAttackText =
            GameObject.Find("buyPlayerCardSkillsAttackText").GetComponent<TextMeshProUGUI>();
        buyPlayerClubNameText =
            GameObject.Find("buyPlayerClubNameText").GetComponent<TextMeshProUGUI>();
        buyPlayerClubDiamondPriceText =
            GameObject.Find("buyPlayerClubDiamondPriceText").GetComponent<TextMeshProUGUI>();

        buyPlayerPanel.SetActive(false);
    }

    private void initPlayerCardBigReferences()
    {
        playerCardBigNameText =
            GameObject.Find("playerCardBigNameText").GetComponent<TextMeshProUGUI>();
        playerCardBigCountryFlag =
            GameObject.Find("playerCardBigCountryFlag").GetComponent<RawImage>();
        playerCardBigCardClubFlag =
            GameObject.Find("playerCardBigCardClubFlag").GetComponent<RawImage>();
        playerCardBigDefenseSkillsBar =
            GameObject.Find("playerCardBigDefenseSkillsBar").GetComponent<Image>();
        playerCardBigAttackSkillsBar =
            GameObject.Find("playerCardBigAttackSkillsBar").GetComponent<Image>();
        playerCardBigSkillsEnergyBar =
            GameObject.Find("playerCardBigSkillsEnergyBar").GetComponent<Image>();
        playerCardBigSkillsDefenseText =
            GameObject.Find("playerCardBigSkillsDefenseText").GetComponent<TextMeshProUGUI>();
        playerCardBigSkillsAttackText =
            GameObject.Find("playerCardBigSkillsAttackText").GetComponent<TextMeshProUGUI>();
        playerCardBigSkillsEnergyText =
            GameObject.Find("playerCardBigSkillsEnergyText").GetComponent<TextMeshProUGUI>();

        playerCardBigPanel.SetActive(false);
    }

    private void updateTeamGlobals()
    {
        if (!Globals.playerPlayAway)
        {  
            teamId = Globals.teamAid;
        }
        else
        {            
            teamId = Globals.teamBid;
        }
    }

    private int getNumberPlrInTeam(int teamId)
    {
       return orgTeams.getTeamByIndex(teamId)[12].Split('|').Length;
    }

    private int getNumberUnlockedPlrInTeam(int teamId)
    {
        string[] players = orgTeams.getTeamByIndex(teamId)[12].Split('|');
        string[] player;
        int numUnlockedPlr = 0;

        for (int i = 0; i < players.Length; i++)
        {
            player = players[i].Split(':');
            if (player[4].Equals("U"))
            {
                numUnlockedPlr++;
            }
        }

        return numUnlockedPlr;
    }

    private int initTeamPlayersCard(string selectedPlayerName,
                                    string saveName)
    {  
        string[] player = orgTeams.getTeamByIndex(teamId)[12].Split('|');

        print("#DBGPLAYR player len " + player.Length + " teamId " + teamId);

        int selectedIdx = -1;
        //print("player descriptigo long " + orgTeams.getTeamByIndex(teamId)[12]);

        numOfUnlockedPlayers = 0;
        for (int i = 0; i < player.Length; i++) {
            //print("DBG323 PLAYER DESC " + player[i]);

            string[] playerDesc = player[i].Split(':');
            print("DBGPLAYR playerDesc " + player[i]);
            //print("initTeamPlayersCard " + playerDesc[0] + " selected player name " + selectedPlayerName);
            if (playerDesc[0].Equals(selectedPlayerName))
                selectedIdx = i + 1;

            //print("PlayerDESCINIT " + playerDesc[0] + " desc1 "
            //    + playerDesc[1] 
            //    + " playerDesc[4] " + playerDesc[4]
            //    + " playerDesc[5] " + playerDesc[5]
            //    + " I " + i
            //    + " playerCardPrice " + playerCardPrice[i]);

            playerCard[i].SetActive(true);

            string tmpPlayerDesc = player[i];
            playerCardButton[i].onClick.RemoveAllListeners();
            playerCardButton[i].onClick.AddListener(                
                       delegate { onClickShowPlayerCardToBuy(playerTeamName, 
                                                             playerLeagueName,
                                                             tmpPlayerDesc); });

            playerCardName[i].text = playerDesc[0];
            graphics.setFlagRawImage(playerCardFlag[i], "national/"+ playerDesc[1]);

            playerCardPlayerImg[i].texture =
                Resources.Load<Texture2D>(
                    "playersCard/" + playerDesc[6]);

            int skillsCumulative =
                int.Parse(playerDesc[2]) + int.Parse(playerDesc[3]);

            //print("star IMG " + playerCardPlayerStarImg[i] + " I " + i);

            var tempColor = playerCardPlayerStarImg[i].color;
            if (skillsCumulative > Globals.playerStarMinSkills)
            {
                tempColor.a = 1f;
            }
            else
            {
                tempColor.a = 0f;
            }
            playerCardPlayerStarImg[i].color = tempColor;

            //playerCardDefense[i].text = playerDesc[2];
            playerCardSkills[i].text = 
            graphics.getSkillsStr(playerDesc[2],
                                  playerDesc[3]);

            if (playerDesc[4].Equals("L"))
            {
                playerCardPrice[i].text =
                            Globals.getPlayerCardPrice(player[i]);
                //Globals.getPlayerCardPrice(playerDesc[2],
                //                           playerDesc[3]);



                playerEnergySkillsBar[i].fillAmount = 1f;
                Color colorDiamond;
                ColorUtility.TryParseHtmlString("#FFFFFFFF", out colorDiamond);
                //colorDiamond.a = 1f;

                playerCardSelectionButton[i].interactable = false;
                playerCardSelectionImg[i].color = colorDiamond;
                playerCardSelectionImg[i].texture =
                    Resources.Load<Texture2D>("others/diamonds");
                //playerCardSelectionButton[i].SetActive(false);
            } else
            {
                Color colorSelection;
                var tempColorSelect = playerCardSelectionImg[i].color;
                ColorUtility.TryParseHtmlString("#FFEA2D", out colorSelection);
                colorSelection.a = tempColorSelect.a;
                playerCardSelectionImg[i].color = colorSelection;
                playerCardSelectionButton[i].interactable = true;
                playerCardSelectionImg[i].texture = 
                    Resources.Load<Texture2D>("others/teamSelected");

                playerCardPrice[i].text = "";

                int energy = 100;
                if (!Globals.isFriendly)
                {
                    if (PlayerPrefs.HasKey(playerDesc[0] + "_energy_" + saveName))
                    {
                        energy =
                            PlayerPrefs.GetInt(playerDesc[0] + "_energy_" + saveName);
                    }
                }

                playerEnergySkillsBar[i].fillAmount = energy / 100f;
                numOfUnlockedPlayers++;
                //playerCardSelectionButtonGO[i].SetActive(true);
            }
        }

        for (int i = player.Length; i < MAX_PLAYERS_CARDS; i++)
            playerCard[i].SetActive(false);

        return selectedIdx;
        //dimPlayerSelectionApartFrom(1);
    }

    private void initUnlockedPlayerCardReferences()
    {
        unlockedPlayerCardPanel =
            GameObject.Find("unlockedPlayerCardPanel");
        unlockedPlayerCardPanelSmaller =
            GameObject.Find("unlockedPlayerCardPanelSmaller");
        unlockedPlayerCardNameText =
            GameObject.Find("unlockedPlayerCardNameText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerCardSkillsText =
            GameObject.Find("unlockedPlayerCardSkillsText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerClubNameText =
            GameObject.Find("unlockedPlayerClubNameText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerCardCountryFlag =
            GameObject.Find("unlockedPlayerCardCountryFlag").GetComponent<RawImage>();
        unlockedPlayerCardClubFlag =
            GameObject.Find("unlockedPlayerCardClubFlag").GetComponent<RawImage>();
        unlockedPlayerCardPlayerImage =
            GameObject.Find("unlockedPlayerCardPlayerImage").GetComponent<RawImage>();

        unlockedPlayerCardPanel.SetActive(false);
    }

    public void fillTransferList()
    {
        /*TODO REMOVE EMPTY ROWS IN THE END */
        int transferListNum = 0;

        for (int i = 0; i < transferList.Count; i++)
        {
            string[] transferItem = transferList[i].Split('|');
            string teamName = transferItem[0];
   
            string[] playerItem = transferItem[1].Split(':');

            if (playerItem.Length < 3)
                continue;

            string playerName = playerItem[0];
            string countryName = playerItem[1];
            string defenseSkills = playerItem[2];
            string attackSkills = playerItem[3];
            string price = Globals.getPlayerCardPrice(transferItem[1]);

            //string playerImg = playerItem[6];

            //print("Splitted " + playerName + " teamName " + teamName 
            //    + " countryName " + countryName
            //    + " defenseSkills " + defenseSkills
            //    + " attackSkills " + attackSkills);

            transferListRow[i].SetActive(true);
            GameObject row = GameObject.Find("transferListRow" + (i + 1).ToString());

            transferListNum++;
            foreach (Transform obj in row.GetComponentsInChildren<Transform>())
            {
                if (obj.name == "countryFlag")
                {
                    RawImage countryFlagImg =
                        obj.gameObject.GetComponent<RawImage>();
                    graphics.setFlagRawImage(countryFlagImg, "national/" + countryName);
                }

                if (obj.name == "playerName")
                {
                    obj.gameObject.GetComponent<TextMeshProUGUI>().text = playerName;
                }

                if (obj.name == "clubName")
                {
                    obj.gameObject.GetComponent<TextMeshProUGUI>().text = teamName;
                }

                if (obj.name == "clubNameImg")
                {
                    RawImage clubFlagImg =
                        obj.gameObject.GetComponent<RawImage>();


                    graphics.setFlagRawImage(clubFlagImg,
                                             graphics.getFlagPath(teamName));
                }

                if (obj.name == "defenseSkillsText")
                {
                    obj.gameObject.GetComponent<TextMeshProUGUI>().text = defenseSkills;
                }

                if (obj.name == "attackSkillsText")
                {
                    obj.gameObject.GetComponent<TextMeshProUGUI>().text = attackSkills;
                }

                if (obj.name == "priceText")
                {
                    obj.gameObject.GetComponent<TextMeshProUGUI>().text = price;
                }
              
                if (obj.name == "buyButton")
                {
                    int tmpIdx = i;
                    string leagueName = transferItem[2];
                    string playerDesc = transferItem[1];

                    obj.GetComponent<Button>().onClick.RemoveAllListeners();
                    obj.GetComponent<Button>().onClick.AddListener(
                        delegate { onClickShowPlayerCardToBuy(teamName, 
                                                              leagueName,
                                                              playerDesc); });
                }
            }
        }

        for (int i = transferListNum; i < MAX_TRANSFERS_ROWS; i++)
            transferListRow[i].SetActive(false);
    }

    public void setSelectedPlayerCard(Image playerCard,
                                       RawImage countryFlagImg,
                                       RawImage playerCardImg,
                                       TextMeshProUGUI skillsText,
                                       TextMeshProUGUI playerName,
                                       Image energyBar,
                                       string playerStr,
                                       string saveName)
    {
        string[] playerDesc = playerStr.Split(':');
     
        graphics.setFlagRawImage(countryFlagImg, "national/" + playerDesc[1]);
        playerCardImg.texture =
             Resources.Load<Texture2D>("playersCard/" + 
                    playerDesc[6]);
        skillsText.text = 
            graphics.getSkillsStr(playerDesc[2], playerDesc[3]);
        playerName.text = playerDesc[0];

        int energy = 100;
        refillEnergyButtonGO.SetActive(false);

        if (!Globals.isFriendly)
        {
            if (PlayerPrefs.HasKey(playerDesc[0] + "_energy_" + saveName))
            {
                energy =
                    PlayerPrefs.GetInt(playerDesc[0] + "_energy_" + saveName);
                if (energy < 100)
                {
                    refillEnergyButtonGO.SetActive(true);
                }
            }
        }

        selectedPlayerCardEnergyBar1.fillAmount = energy / 100f;

        StartCoroutine(
            selectionFillAmountAnimation(playerCard));

        selectedPlayerName = playerDesc[0];
        Globals.selectedPlrCardDesc = playerStr;

        if (Globals.playerPlayAway)
        {
            Globals.playerBDesc = playerStr;
        } else
        {
            Globals.playerADesc = playerStr;
        }
        
        string selectedPlayerPrefabs = 
            playerTeamName + "_lastSelectedPlayer";
        PlayerPrefs.SetString(selectedPlayerPrefabs, playerStr);
        PlayerPrefs.Save();           
    }

    static public void fillPlayerCard(                                            
                                      RawImage countryFlagImg,
                                      RawImage playerCardImg,
                                      GameObject playerCardPlayerStar,
                                      TextMeshProUGUI skillsText,
                                      TextMeshProUGUI playerName,
                                      Image energyBar,
                                      string playerStr,
                                      string saveName,
                                      int defense,
                                      int attack,
                                      int energy)
    {
        string[] playerDesc = playerStr.Split(':');

        GraphicsCommon graphics = new GraphicsCommon();

        graphics.setFlagRawImage(countryFlagImg, "national/" + playerDesc[1]);
        playerCardImg.texture =
             Resources.Load<Texture2D>("playersCard/" +
                    playerDesc[6]);
        skillsText.text =
            graphics.getSkillsStr(playerDesc[2], playerDesc[3]);
        playerName.text = playerDesc[0];

        energyBar.fillAmount = energy / 100f;

        if (Globals.isPlayerCardStar(int.Parse(playerDesc[2]), int.Parse(playerDesc[3])))
        {
            playerCardPlayerStar.SetActive(true);
        } else
        {
            playerCardPlayerStar.SetActive(false);
        }
    }


    private void transferPlayer(string playerDesc,
                                string leagueNameFrom, 
                                string teamNameFrom,                                
                                string leagueNameTo, 
                                string teamNameTo,
                                bool sell,
                                string saveName)
    {
        /*TODO use regular expression*/
        string[] playerStr = playerDesc.Split(':');
   
        if (!sell)
        {
            if (!Globals.isFriendly)
            {
                PlayerPrefs.SetInt(playerStr[0] + "_energy_" + saveName, MAX_ENERGY);
                PlayerPrefs.Save();
            }
        }
                 
        //print("DBGTRANSFER334 AFTER " + playerDesc);
       // print("DBGTRANSFER334 playerDesc " + playerDesc +
       //     " LeagueNameFrom " + leagueNameFrom
       //     + " teamNameFrom " + teamNameFrom
       //     + " leagueNameTo " + leagueNameTo
       //     + " teamNameTo " + teamNameTo);

        Teams leagueFrom = new Teams(leagueNameFrom);
        Teams leagueTo = new Teams(leagueNameTo);

        int teamFromIdx = leagueFrom.getTeamIndexByName(teamNameFrom);
        int teamToIdx = leagueTo.getTeamIndexByName(teamNameTo);

        string[] teamFrom = leagueFrom.getTeamByIndex(teamFromIdx);
        string[] teamTo = leagueTo.getTeamByIndex(teamToIdx);

        string teamFromPlayerRem = teamFrom[12];

        /*if (teamFromPlayerRem.Contains(playerDesc + "|"))
        {
            teamFromPlayerRem =
                teamFromPlayerRem.Replace(playerDesc + "|", "");
        }
        if (teamFromPlayerRem.Contains(playerDesc))
        {
            teamFromPlayerRem =
                teamFromPlayerRem.Replace(playerDesc, "");
        }

        if (teamFromPlayerRem.EndsWith("|"))
        {
            teamFromPlayerRem =
               teamFromPlayerRem.Remove(teamFromPlayerRem.Length - 1);
        }*/

        teamFromPlayerRem = 
            Globals.removeStrFromString(teamFromPlayerRem, playerDesc, "|");

        //print("DBGTRANSFER334 TAKEFROM " + teamFromPlayerRem + " teamFROM " +
        //    teamFrom[0] + "_teamPlayers");

        string teamToWithNewPlayer = "";
        if (sell)
        {
            playerDesc = Globals.togglePlayerCardLockVal(playerDesc, "L");
            teamToWithNewPlayer = Globals.addPlayerCardToTail(teamTo[12], playerDesc, "|");
        }
        else
        {
            playerDesc = Globals.togglePlayerCardLockVal(playerDesc, "U");
            teamToWithNewPlayer = Globals.addPlayerCardToHead(teamTo[12], playerDesc, "|");
        }

        //print("DBGTEAM1 PLAYERDESC " + playerDesc);
        //print("DBGTRANSFER334 ADDTO " + teamToWithNewPlayer
        //    + " stringName " + teamTo[0] + "_teamPlayers");

        PlayerPrefs.SetString(teamTo[0] + "_teamPlayers", teamToWithNewPlayer);
        PlayerPrefs.SetString(teamFrom[0] + "_teamPlayers", teamFromPlayerRem);
        PlayerPrefs.Save();
    }

    public void dimPlayerSelectionApartFrom(int idx)
    {
        if (idx == -1) 
            idx = 1;

        for (int i = 0; i < MAX_PLAYERS_CARDS; i++) {
            if (!playerCardSelectionButton[i].interactable)
                continue;
        
            var tempColor = playerCardSelectionImg[i].color;

            //if ((idx - 1) != i)
            if ((idx - 1) != i)
            {
                tempColor.a = 0.1f;
            } else
            {
                tempColor.a = 1f;

                string[] player = 
                    orgTeams.getTeamByIndex(teamId)[12].Split('|');
                setSelectedPlayerCard(selectedPlayerCard1,
                                      selectedPlayerCardCountryFlag1,
                                      selectedPlayerCardImage1,
                                      selectedPlayerCardSkills1,
                                      selectedPlayerCardName1,
                                      selectedPlayerCardEnergyBar1,
                                      player[idx - 1],
                                      saveFileName);
            }
            
            playerCardSelectionImg[i].color = tempColor;
        }
    }

    public void onClickPlay()
    {
        /*print("Globals.isMultiplayer " + Globals.isMultiplayer);


        print("#DBG1334 set management here texture SAVE TEAMALEAGUE " + Globals.teamAleague + " Globals.playerADesc "
   +
     Globals.playerADesc + " Globals.teamAname " + Globals.teamAname
    + " Globals team B " + Globals.teamBleague
    + " teamB Name " + Globals.teamBname
    + " temBIDx " + Globals.teamBid);*/



        //print("DBGPLAYONCLICK TEAM MANAGEMENT 1 CLICK PLAY");
        Globals.recoverOriginalResolution();
        if (!Globals.isMultiplayer)
        {
            updatePlayersEnergy(saveFileName);
        }
        updateCustomizePlayerMatches();

        if (Globals.isMultiplayer)
        {
            Globals.dontCheckOnlineUpdate = true;
            Globals.loadSceneWithBarLoader("multiplayerMenu");
        }
        else
        {
            if (!Globals.is_app_paid && ((Globals.numMatchesInThisSession % 2 == 0) || !Globals.PITCHTYPE.Equals("GRASS")))
                Globals.loadSceneWithBarLoader("specialShopOffers");
            else
                Globals.loadSceneWithBarLoader("extraPowers");
        }
    }

    private void updateCustomizePlayerMatches()
    {
        if (selectedPlayerName.Equals(Globals.customizePlayerName))
        {
            int numMatches = 0;
            if (PlayerPrefs.HasKey("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES"))
            {
                numMatches =
                    PlayerPrefs.GetInt("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES");
            }

            if (numMatches > 14)
                numMatches = 0;

            PlayerPrefs.SetInt("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES", numMatches + 1);
            PlayerPrefs.Save();
        }       
    }

    //it's begining of the season
    public static void initPlayerEnergy(string playersDesc, 
                                        string saveName)
    {
        string[] player = playersDesc.Split('|');
        string[] playerDesc;

        for (int i = 0; i < player.Length; i++)
        {
            playerDesc = player[i].Split(':');
            if (playerDesc[4] == "L")
                continue;

            if (!Globals.isFriendly)
            {
                PlayerPrefs.SetInt(playerDesc[0] + "_energy_" + saveName, 100);
                PlayerPrefs.Save();
            }
        }
    }

    private void updatePlayersEnergy(string saveName)
    {
        print("Globals.teamAAttackStrength updatePlayers energy!!");
        string[] player = orgTeams.getTeamByIndex(teamId)[12].Split('|');
        string[] playerDesc;
        string playerName;
        int skillsAvg;
        int energy = 100;

        for (int i = 0; i < player.Length; i++)
        {
            playerDesc = player[i].Split(':');

            if (playerDesc[4] == "L")
                continue;

            playerName = playerDesc[0];
           
            skillsAvg =
                (int.Parse(playerDesc[2]) + int.Parse(playerDesc[3])) / 2;

            if (!Globals.isFriendly &&
                 PlayerPrefs.HasKey(playerName + "_energy_" + saveName))
            {
                energy =
                    PlayerPrefs.GetInt(playerName + "_energy_" + saveName);
            }
            else
            {
                energy = 100;
            }

            int tiredVal = 4;
            int restVal = 2;
            if (skillsAvg > 80)
            {
                restVal = 3;
                tiredVal = 2;
            }
            else if (skillsAvg > 60)
            {
                restVal = 3;
                tiredVal = 3;
            }

            //print("#DbgSelected DBGENERGY RESTVAL " + restVal + " skillval "
            //    + " tired val " + tiredVal
            //    + skillsAvg + " selectdPlayerName " +
            //    selectedPlayerName + " PlayerName " + playerName);

            if (selectedPlayerName.Equals(playerName))
            {
                energy -= tiredVal;

                int defenseSkills = (int) (float.Parse(playerDesc[2]) * 
                    ((float) energy / (float) MAX_ENERGY));
                int attackSkills = (int) (float.Parse(playerDesc[3]) *
                    ((float) energy / (float) MAX_ENERGY));

                if (Globals.playerPlayAway)
                {
                    Globals.teamBGkStrength =
                        Mathf.Clamp(defenseSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamBAttackStrength =
                        Mathf.Clamp(attackSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamBcumulativeStrength =
                        Globals.teamBGkStrength + Globals.teamBAttackStrength;
                } else
                {
                    Globals.teamAGkStrength =
                       Mathf.Clamp(defenseSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    //print("Globals.teamAGkStrength energy " + Globals.teamAGkStrength);
                    Globals.teamAAttackStrength =
                        Mathf.Clamp(attackSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamAcumulativeStrength =
                        Globals.teamAGkStrength + Globals.teamAAttackStrength;
                }

                //print("#DbgSelected " + defenseSkills + " attackskills " + attackSkills
                //    + " Globals.teamAGkStrength " + Globals.teamAGkStrength
                //    +" Globals.teamAAttackStrength " + Globals.teamAAttackStrength
                //    + " Globals.teamAcumulativeStrength " + Globals.teamAcumulativeStrength 
                //    + " Globals.teamBGkStrength " + Globals.teamBGkStrength
                //    + " Globals.teamBAttackStrength " + Globals.teamBAttackStrength
                //    + " Globals.teamBcumulativeStrength " + Globals.teamBcumulativeStrength
                //    + " playerAway " + Globals.playerPlayAway);
            }
            else
            {
                energy += restVal;
            }

            energy = Mathf.Clamp(energy, 0, 100);

           /* if (Globals.isMultiplayer)
            {
                Globals.outstandingEnergyName = playerName + "_energy_" + saveName;
                Globals.outstandingEnergyVal = energy;
                return;
            }*/

            if (!Globals.isFriendly)
            {
                PlayerPrefs.SetInt(playerName + "_energy_" + saveName, energy);
                PlayerPrefs.Save();
            }

        }
    }

    //used by multiplayer
    static public void updatePlayersEnergy(string saveName, string players)
    {
        string[] player = players.Split('|');
        string[] playerDesc;
        string playerName;
        int skillsAvg;
        int energy = 100;
        string selectedPlayerName = players.Split('|')[0].Split(':')[0];

        for (int i = 0; i < player.Length; i++)
        {
            playerDesc = player[i].Split(':');

            if (playerDesc[4] == "L")
                continue;

            playerName = playerDesc[0];

            skillsAvg =
                (int.Parse(playerDesc[2]) + int.Parse(playerDesc[3])) / 2;
            
            if (PlayerPrefs.HasKey(playerName + "_energy_" + saveName))
            {
                energy =
                    PlayerPrefs.GetInt(playerName + "_energy_" + saveName);
            }
            else
            {
                energy = 100;
            }

            int tiredVal = 4;
            int restVal = 2;
            if (skillsAvg > 80)
            {
                restVal = 3;
                tiredVal = 2;
            }
            else if (skillsAvg > 60)
            {
                restVal = 3;
                tiredVal = 3;
            }

            //print("#DbgSelected DBGENERGY RESTVAL " + restVal + " skillval "
            //    + " tired val " + tiredVal
            //    + skillsAvg + " selectdPlayerName " +
            //    selectedPlayerName + " PlayerName " + playerName);

            print("DBGDEFENSESKILLS skillaAvg skillsAvg " + skillsAvg + " playerName " + playerName
                + " selectedPlayerName " + selectedPlayerName + " energy " + energy);
                
            if (selectedPlayerName.Equals(playerName))
            {
                energy -= tiredVal;

                int defenseSkills = (int)(float.Parse(playerDesc[2]) *
                    ((float)energy / (float) Globals.MAX_ENERGY));
                int attackSkills = (int)(float.Parse(playerDesc[3]) *
                    ((float)energy / (float) Globals.MAX_ENERGY));

                if (Globals.playerPlayAway)
                {
                    Globals.teamBGkStrength =
                        Mathf.Clamp(defenseSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamBAttackStrength =
                        Mathf.Clamp(attackSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamBcumulativeStrength =
                        Globals.teamBGkStrength + Globals.teamBAttackStrength;
                }
                else
                {
                    Globals.teamAGkStrength =
                       Mathf.Clamp(defenseSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    //print("Globals.teamAGkStrength energy " + Globals.teamAGkStrength);
                    Globals.teamAAttackStrength =
                        Mathf.Clamp(attackSkills, Globals.MIN_PLAYER_SKILLS, Globals.MAX_PLAYER_SKILLS);
                    Globals.teamAcumulativeStrength =
                        Globals.teamAGkStrength + Globals.teamAAttackStrength;
                }

                //print("#DbgSelected " + defenseSkills + " attackskills " + attackSkills
                //    + " Globals.teamAGkStrength " + Globals.teamAGkStrength
                //    +" Globals.teamAAttackStrength " + Globals.teamAAttackStrength
                //    + " Globals.teamAcumulativeStrength " + Globals.teamAcumulativeStrength 
                //    + " Globals.teamBGkStrength " + Globals.teamBGkStrength
                //    + " Globals.teamBAttackStrength " + Globals.teamBAttackStrength
                //    + " Globals.teamBcumulativeStrength " + Globals.teamBcumulativeStrength
                //    + " playerAway " + Globals.playerPlayAway);
            }
            else
            {
                energy += restVal;
            }

            energy = Mathf.Clamp(energy, 0, 100);

            /*if (Globals.isMultiplayer)
            {
                Globals.outstandingEnergyName = playerName + "_energy_" + saveName;
                Globals.outstandingEnergyVal = energy;
                return;
            }*/

            if (!Globals.isFriendly)
            {
                PlayerPrefs.SetInt(playerName + "_energy_" + saveName, energy);
                PlayerPrefs.Save();
            }

        }
    }


    public void onClickTransferButton()
    {
        if (getNumberPlrInTeam(teamId) >= MAX_PLAYERS_CARDS)
        {
            infoPanel.SetActive(true);
            infoPanelText.text = Languages.getTranslate(
                "You can not have more than " 
                + MAX_PLAYERS_CARDS.ToString() + " player's card.\n" +
                "Sell player by clicking on a card",
                new List<string>() { MAX_PLAYERS_CARDS.ToString() });
            return;
        }

        transfersPanel.SetActive(true);
        fillTransferList();
    }

    public void onClickCloseTransferPanel()
    {
        transfersPanel.SetActive(false);
    }
   
    //This is used for buying and selling player 
    public void onClickShowPlayerCardToBuy(string teamName,
                                           string leagueName,
                                           string playerStr)
    {
        //string[] transferItem = transferList[idx].Split('|');
        //string clubName = transferItem[0];
        string[] playerItem = playerStr.Split(':');
        string playerName = playerItem[0];
        string countryName = playerItem[1];
        string defenseSkills = playerItem[2];
        string attackSkills = playerItem[3];
        string isLocked = playerItem[4];
        //string price = playerItem[5];
        string price = Globals.getPlayerCardPrice(playerStr);
        string playerSkinHair = playerItem[6];

        //print("#DBGTRANSFER334 playerDESC " + playerStr);

        buyPlayerNameText.text = playerName;
        graphics.setFlagRawImage(buyPlayerCardCountryFlag, "national/" + countryName);
        graphics.setFlagRawImage(buyPlayerCardClubFlag,
                                 graphics.getFlagPath(teamName));

        //print("GETFULLPATH TEAM MANAGEMENT " +
        //     graphics.getFlagPath(teamName));

        buyPlayerCardPlayerImage.texture =
              Resources.Load<Texture2D>("playersCard/" +
                    playerSkinHair);


        buyPlayerCardDefenseSkillsBar.fillAmount = float.Parse(defenseSkills) / 100.0f;
        buyPlayerCardAttackSkillsBar.fillAmount = float.Parse(attackSkills) / 100.0f;
        buyPlayerCardSkillsDefenseText.text = 
            Languages.getTranslate(
                "Defense: " + defenseSkills,
                new List<string>() { defenseSkills });
        buyPlayerCardSkillsAttackText.text = 
            Languages.getTranslate(
            "Attack: " + attackSkills,
            new List<string>() { attackSkills });

        buyPlayerClubNameText.text = teamName;
        buyPlayerClubDiamondPriceText.text = price;
 
        buyPlayerCardYesButton.onClick.RemoveAllListeners();
        if (isLocked.Equals("L"))
        {
            buyPlayerHeaderText.text = 
                Languages.getTranslate("Buy player's card");
            buyPlayerCardClubFlagGameObj.SetActive(true);
            buyPlayerCardDiamondGameObj.SetActive(true);

            if (int.Parse(price) > Globals.diamonds)
            {
                buyPlayerCardNotificationText.text =
                     Languages.getTranslate("You don't have enough diamonds to buy this card.\n" +
                    "Go to the shop to buy diamonds");
                buyPlayerCardYesButtonText.text = 
                    Languages.getTranslate("SHOP");

                buyPlayerCardYesButton.onClick.AddListener(
                              delegate {
                                  shopScript.showDiamondsPanel();
                              });

                /*show shop canvas*/
            } else
            {
                buyPlayerCardYesButton.onClick.AddListener(
                               delegate {
                                   onClickBuyButtonYes(teamName,
                                                       leagueName,
                                                       playerStr);
                               });
                buyPlayerCardNotificationText.text =
                    Languages.getTranslate("Buy player's card for " + price + " diamonds",
                                            new List<string>() { price });

                if (teamName.Equals(playerTeamName))
                {
                    buyPlayerHeaderText.text =
                        Languages.getTranslate("Unlock team's player");
                    buyPlayerCardYesButtonText.text =
                        Languages.getTranslate("UNLOCK");
                }
                else
                {
                    buyPlayerHeaderText.text =
                        Languages.getTranslate("Buy player's card");
                    buyPlayerCardYesButtonText.text =
                        Languages.getTranslate("BUY");
                }
            }
        }
        else
        {
            /*sell player*/                     
            buyPlayerHeaderText.text =
                Languages.getTranslate("Sell player");
            /*if (numOfUnlockedPlayers > 1)
                buyPlayerCardYesButtonText.text = "SELL PLAYER";
            else
                buyPlayerCardYesButtonText.text = "OK";*/

            string teamOffer;
            playerSellOfferHash.TryGetValue(playerStr, out teamOffer);         
            if (teamOffer == null)
            {
                teamOffer = findTeamWantToBuyPlayer(teamName,
                                                    defenseSkills,
                                                    attackSkills,
                                                    price);
                //print("DBGSELLPLAYR1 offered NEW " + teamOffer);
                playerSellOfferHash.Add(playerStr, teamOffer);
            } 
            //else
            //{
            //    print("DBGSELLPLAYR1 offered FROM CACHE " + teamOffer);
            //}

            //print("numOfUnlockedPlayers " + numOfUnlockedPlayers);

            buyPlayerClubNameText.text = "";
            if (teamOffer.Equals("NOOFFER") ||
                numOfUnlockedPlayers <= 1)
            {
                buyPlayerClubDiamondPriceText.text = "";
                buyPlayerCardClubFlagGameObj.SetActive(false);
                buyPlayerCardDiamondGameObj.SetActive(false);
                if (numOfUnlockedPlayers == 1)
                    buyPlayerCardNotificationText.text =
                        Languages.getTranslate("You can not sell only player");
                else
                    buyPlayerCardNotificationText.text =
                        Languages.getTranslate("No offer of buying this player");

                buyPlayerCardYesButton.onClick.AddListener(
                    delegate {
                        onClickClosePanel(buyPlayerPanel);
                    });
                buyPlayerCardYesButtonText.text = "OK";
            } else
            {
                buyPlayerClubDiamondPriceText.text = teamOffer.Split('|')[2];
                buyPlayerCardClubFlagGameObj.SetActive(true);
                buyPlayerCardDiamondGameObj.SetActive(true);

                buyPlayerCardYesButtonText.text =
                        Languages.getTranslate(" SELL PLAYER ");
                buyPlayerCardNotificationText.text =
                        Languages.getTranslate(
                      teamOffer.Split('|')[0] + " would like to buy this player \n for " +
                      teamOffer.Split('|')[2] + " diamonds",
                      new List<string>() { teamOffer.Split('|')[0], teamOffer.Split('|')[2] } );

                buyPlayerCardYesButton.onClick.AddListener(
                             delegate {
                                 onClickSellPlayer(teamName,
                                                   leagueName,
                                                   playerStr,
                                                   teamOffer);
                             });
            }
        }

        //buyPlayerCardYesButton.onClick.RemoveAllListeners();
        //buyPlayerCardYesButton.onClick.AddListener(
        //     delegate { onClickShowPlayerCardToBuy(tmpIdx); });

        buyPlayerPanel.SetActive(true);
        //buyPlayerCardListIdx = idx;
    }

    public void refillSelectedPlayerEnergy()
    {
        PlayerPrefs.SetInt(selectedPlayerName + "_energy_" + saveFileName, MAX_ENERGY);
        PlayerPrefs.Save();

        int selectedPlrIdx =
            initTeamPlayersCard(selectedPlayerName, saveFileName);
        dimPlayerSelectionApartFrom(selectedPlrIdx);
        refillEnergyPanel.SetActive(false);
    }

    private string findTeamWantToBuyPlayer(string playerTeamName, 
                                           string defense, 
                                           string attack,
                                           string orgPrice)
    {
        int playerSkills = int.Parse(defense.ToString()) +
                           int.Parse(attack.ToString());
     
        string theBestFitTeam = "";
        int theBestFitTeamDiffVal = 100000;        
        float pricePerc = (float) UnityEngine.Random.Range(80, 101) / 100f;
        int offerPrice = (int)(
            (float) int.Parse(orgPrice) * pricePerc);

        //print("#DBGSELLPLAYERPRICE " + offerPrice + " orgPrice " + orgPrice);

        if (offerPrice == 0 ||
            orgPrice.Equals("0"))
        {
            return "NOOFFER";
        }

        //print("DBGSELLPLAYR1 ORGPRICE " + orgPrice + " OFFERPRICe " + offerPrice.ToString()
        //    + " pricePerc " + pricePerc);

        for (int i = 0; i < 30; i++)
        {
            int randomLeague = UnityEngine.Random.Range(0, Globals.MAX_PLAYERS_CARD_LEAGUES);
            if (leagues[randomLeague].getMaxActiveTeams() > 0)
            {
                int randTeam =
                     UnityEngine.Random.Range(0, leagues[randomLeague].getMaxActiveTeams());
                string leagueNameOffer =
                    leagues[randomLeague].getCurrentLeagueName();
                string[] teamOffer =
                    leagues[randomLeague].getTeamByIndex(randTeam);

                string[] players = teamOffer[12].Split('|');

                //print("DBGSELLPLAYR1 CHECK TEAM team[0] " + teamOffer[0]);
                /*don't transfer player to the same team*/
                if (teamOffer[0].Equals(playerTeamName) ||
                    players.Length >= MAX_PLAYERS_CARDS)
                    continue;

                /*int cumulativeSkills = 0;
                for (int j = 0; j < players.Length; j++)
                {
                    string[] playerDesc = players[j].Split(':');
                    cumulativeSkills +=
                        int.Parse(playerDesc[2]) + int.Parse(playerDesc[3]);                                        
                }
             
                cumulativeSkills /= players.Length;*/
                
                Vector2 avgTeamSkills =
                    Globals.getTeamSkillsAverage(teamOffer,
                                                 leagues[randomLeague].getLeagueName());
                
                int cumulativeSkills = (int) avgTeamSkills.x + (int) avgTeamSkills.y;

                int skillsDiff = Math.Abs(cumulativeSkills - playerSkills);
                //print("#DBGPLAYERSKILLS skills diff " + skillsDiff);

                //print("DBGSELLPLAYR1 CHECK TEAM team[0] " + teamOffer[0]
                //    + " CUMULATIVE SKILLS " + avgTeamSkills + " playerLENGT " + players.Length
                //    + " skillsDiff " + skillsDiff
                //    + " Playerskills " + playerSkills);

                if (skillsDiff < 10 && players.Length < 5)
                {
                    return teamOffer[0] + "|" +
                           leagueNameOffer + "|" +
                           offerPrice.ToString();
                    break;
                } else {
                    if (skillsDiff < theBestFitTeamDiffVal)
                    {
                        theBestFitTeam =
                            teamOffer[0] + "|" +
                            leagueNameOffer + "|" +
                            offerPrice.ToString();
                        theBestFitTeamDiffVal = skillsDiff;
                    }
                }
            }
        }


        //print("#DBGPLAYERSKILLS " + playerSkills + " theBestFitTeamDiffVal " + theBestFitTeamDiffVal);
        //dont tranfser week player to much weeker team
        if (theBestFitTeamDiffVal > 10)
            return "NOOFFER";

        return theBestFitTeam;
    }
  
    public void onClickSellPlayer(string teamName,
                                  string leagueName,
                                  string playerDesc,
                                  string teamOffer)
    {
        sellPanelConfirm.SetActive(true);

        Button sellPanelConfirmYes = 
            GameObject.Find("sellPanelConfirmYes").GetComponent<Button>();
        Button sellPanelConfirmNo =
            GameObject.Find("sellPanelConfirmNo").GetComponent<Button>();

        sellPanelConfirmYes.onClick.RemoveAllListeners();
        sellPanelConfirmYes.onClick.AddListener(
                     delegate {
                         onClickSellYesButton(playerTeamName,
                                              playerLeagueName,
                                              playerDesc,
                                              teamOffer);
                     });

        sellPanelConfirmNo.onClick.RemoveAllListeners();
        sellPanelConfirmNo.onClick.AddListener(
             delegate {
                 onClickSellNoButton();
             });
    }

    public void onClickCloseSelectionHelperPanel()
    {
        selectionPlrHelperPanel.SetActive(false);
    }

    private void onClickSellNoButton()
    {
        sellPanelConfirm.SetActive(false);
    }

    private void onClickClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    private void onClickCloseBuyPlayerPanel()
    {
        buyPlayerPanel.SetActive(false);
    }

    private void onClickSellYesButton(string playerTeamName,
                                      string playerLeagueName,
                                      string playerDesc,
                                      string teamOffer)
    {
        string[] teamOfferDesc = teamOffer.Split('|');
        int priceOffered = int.Parse(teamOfferDesc[2]);

        //print("TrANSFER PLAYER " +
        //    " playerDesc " + playerDesc
        //    + " playerLeagueName " + playerLeagueName
        //    + " playerTeamName " + playerTeamName
        //    + " teamOfferDesc[1] " + teamOfferDesc[1]
        //    + "  teamOfferDesc[0] " + teamOfferDesc[0]
        //    + " priceOffered "+ priceOffered);

        transferPlayer(playerDesc,
                       playerLeagueName,
                       playerTeamName,
                       teamOfferDesc[1],
                       teamOfferDesc[0],
                       true,
                       saveFileName);

        string player = orgTeams.getTeamByIndex(teamId)[12];
        //print("TrANSFER PLAYER AFTER " + player + "##");

        Globals.addDiamonds(priceOffered);   
        sellPanelConfirm.SetActive(false);
        audioManager.Play("elementAppear");

        int selectedPlrIdx = 
            initTeamPlayersCard(selectedPlayerName, saveFileName); 
        dimPlayerSelectionApartFrom(selectedPlrIdx);

        //dimPlayerSelectionApartFrom(1);
        updateCoinsText();

        playerCardBigPanel.SetActive(false);
        transfersPanel.SetActive(false);
        buyPlayerPanel.SetActive(false);
    }

    //used by external scripts
    public void onClickExitShopPanel()
    {
    //    int selectedPlrIdx = 
    //        initTeamPlayersCard(selectedPlayerName, saveFileName);

       /// dimPlayerSelectionApartFrom(selectedPlrIdx);

        shopPanel.SetActive(false);
    }

    private void showExtraPrize()
    {
        extraPrizePanel.SetActive(false);

        //PlayerPrefs.SetString("CustomizeTeam_customizePlayerDesc",
        //                      Globals.customizePlayerCardName);
        //PlayerPrefs.SetString("CustomizeTeam_customizePlayerCardName",
        //                      Globals.customizePlayerCardName);
        //PlayerPrefs.Save();

        if (!Globals.isTeamCustomize(playerTeamName))
        {
            return;
        }

        int numMatchesByCustomizePlayer = 0;
        if (PlayerPrefs.HasKey("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES"))
            numMatchesByCustomizePlayer = 
                PlayerPrefs.GetInt("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES");

        if (numMatchesByCustomizePlayer != 1)
            return;

        PlayerPrefs.SetInt("CUSTOMIZE_PLAYER_NAME_NUM_MATCHES", numMatchesByCustomizePlayer + 1);
        PlayerPrefs.Save();

        string teamPlayers = orgTeams.getTeamByIndex(teamId)[12];
        string playerDesc = "";
        string[] players = teamPlayers.Split('|');

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Split(':')[0].Equals(Globals.customizePlayerName))
            {
                Globals.customizePlayerDesc = players[i];
                Globals.customizePlayerCardName = players[i].Split(':')[6];
                playerDesc = Globals.customizePlayerDesc;
            }
        }

        if (!playerDesc.Contains(Globals.customizePlayerName) ||
            String.IsNullOrEmpty(playerDesc))
            return;

        //string playerDesc = Globals.customizePlayerDesc;
        ///print("#DBG1 playerDEsc " + playerDesc);
        int attackSkills = Globals.getPlayerCardAttackSkills(playerDesc);
        int defenseSkills = Globals.getPlayerCardDefenseSkills(playerDesc);

        if ((attackSkills >= 100) &&
            (defenseSkills >= 100))
            return;

        extraPrizePanel.SetActive(true);
        Texture texture = null;
        texture = Resources.Load<Texture2D>(
                  "playersCard/" + Globals.customizePlayerCardName);

        if (texture != null)
            extraPrizePlayerImg.texture = texture;

        int rand = UnityEngine.Random.Range(0, 2);
        string text = "";

        if (attackSkills >= 100)
            rand = 0;
        if (defenseSkills >= 100)
            rand = 1;

        //print("#DBG1 teamsPlayers Org " + teamPlayers);

        string teamNewPlayers =
            Globals.removeStrFromString(teamPlayers, playerDesc, "|");

        string skillsImprovVal = "1";
        if (rand == 0)
        {
            playerDesc = Globals.incPlayerCardAttackSkills(playerDesc, 1);
            text = Languages.getTranslate("Excellent! " + skillsImprovVal + " to defense skills awarded!",
                                          new List<string>() { skillsImprovVal });
        }
        else
        {
            playerDesc = Globals.incPlayerCardDefenseSkills(playerDesc, 1);            
     
            text = Languages.getTranslate("Excellent! " + skillsImprovVal + " to attack skills awarded!",
                                   new List<string>() { skillsImprovVal });
        }

        //print("#DBG1 playerDEsc after skills " + playerDesc);
  
        //print("#DBG1 teamsPlayers remove playerDesc  " + teamNewPlayers);

        text = Regex.Replace(text, ": ", "");

        teamNewPlayers = Globals.addPlayerCardToHead(teamNewPlayers, playerDesc, "|");
        //print("#DBG1 teamsPlayers add back  " + teamNewPlayers);
        PlayerPrefs.SetString(playerTeamName + "_teamPlayers", teamNewPlayers);
        PlayerPrefs.Save();

        extraPrizeText.text = text;
        audioManager.Play("elementAppear");
    }

    public void onClickCloseExtraPrizePanel()
    {
        extraPrizePanel.SetActive(false);
    }

    public void onClickBuyButtonYes(string teamName,
                                    string leagueName,
                                    string playerDesc)
    {
        //string[] transferListPlayer = 
        //    transferList[buyPlayerCardListIdx].Split('|');

        /*transferPlayer(transferListPlayer[1],
                       transferListPlayer[2],
                       transferListPlayer[0],
                       playerLeagueName,
                       playerTeamName);*/

        //print("DBGTRANSFER334 TEMNAME " + teamName 
        //    + " playerTeamName " + playerTeamName
        //    + " playerDesc " + playerDesc);

        if (teamName.Equals(playerTeamName))
        {
            //print("DBGTRANSFER334 UNLOCKED GLOBAL");
            Globals.unlockedPlayerCard(teamId, 
                                       playerLeagueName, 
                                       playerTeamName, 
                                       playerDesc);
        }
        else
        {
            //print("DBGTRANSFER334 2 transferPLAYER");
            transferPlayer(playerDesc,
                           leagueName,
                           teamName,
                           playerLeagueName,
                           playerTeamName,
                           false,
                           saveFileName);
        }

        //print("#DBGTRANSFER334 orgTeams.getTeamByIndex(teamId)[12] " +
        //    orgTeams.getTeamByIndex(teamId)[12]);

        int price = int.Parse(Globals.getPlayerCardPrice(playerDesc));
            //int.Parse(playerDesc.Split(':')[5]);
        Globals.addDiamonds(-price);

        int selectedPlrIdx = 
            initTeamPlayersCard(selectedPlayerName,
                                saveFileName);
        //dimPlayerSelectionApartFrom(selectedPlrIdx);
        dimPlayerSelectionApartFrom(1);
        initTransferList();
        updateCoinsText();

        playerCardBigPanel.SetActive(false);
        transfersPanel.SetActive(false);
        buyPlayerPanel.SetActive(false);

        StartCoroutine(showUnlockedCard(teamName,
                                        leagueName,
                                        playerDesc));       
    }

    public void onClickShowRefillEnergyPanel()
    {
#if UNITY_EDITOR
            refillSelectedPlayerEnergy();
            return;
#endif

        if (Globals.is_app_paid)
        {
            refillSelectedPlayerEnergy();
            return;
        }


        refillEnergyPanel.SetActive(true);
    }

    public void onClickCloseRefillEnergyPanel()
    {
        refillEnergyPanel.SetActive(false);
    }

    IEnumerator showUnlockedCard(string teamName,
                                 string leagueName,
                                 string playerStr)
    {
        string[] playerDesc = playerStr.Split(':');

        unlockedPlayerCardPanel.SetActive(true);
        unlockedPlayerCardPanelSmaller.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        audioManager.Play("elementAppear");
        unlockedPlayerCardPanelSmaller.SetActive(true);

        //unlockedPlayerCardClubFlag.texture =
        //     Resources.Load<Texture2D>(teamName);

        unlockedPlayerCardNameText.text =
            playerDesc[0];
        unlockedPlayerCardSkillsText.text =
            graphics.getSkillsStr(playerDesc[2],
                                  playerDesc[3]);

        graphics.setFlagRawImage(unlockedPlayerCardCountryFlag, "national/" + playerDesc[1]);
        graphics.setFlagRawImage(unlockedPlayerCardClubFlag,
                                 graphics.getFlagPath(teamName));

        unlockedPlayerCardPlayerImage.texture =
            Resources.Load<Texture2D>("playersCard/" + 
                    playerDesc[6]);
        unlockedPlayerClubNameText.text = teamName;

        yield return new WaitForSeconds(3f);
        unlockedPlayerCardPanel.SetActive(false);

        //print("#DBGPLAYERS NUM UNLOCKED " +
        //    getNumberUnlockedPlrInTeam(teamId) +
        //    " NUMSELECTED " + Globals.numSelectedPlayerCardHelperShow);

        if (getNumberUnlockedPlrInTeam(teamId) > 1 &&
            Globals.numSelectedPlayerCardHelperShow < 4)
        {
            selectionPlrHelperPanel.SetActive(true);
            Globals.incNumSelectedPlayerCardHelperShow(1);
        }
    }

    public void updateAfterBuying(string type)
    {
        updateCoinsText();

        if (type.Equals("unlockedplayercard"))           
        {
            initTeamPlayersCard(selectedPlayerName, saveFileName);
            dimPlayerSelectionApartFrom(1);
        }
    }

    public void onClickBuyButtonNo()
    {
        buyPlayerPanel.SetActive(false);
    }

    public void onClickInfoPanelClose()
    {
        infoPanel.SetActive(false);
    }
}
