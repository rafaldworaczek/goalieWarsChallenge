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

public class ExtraPowers : MonoBehaviour
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
    public GameObject shopCanvas;
    //public Shop shopScript;
    private TextMeshProUGUI infoPanelText;

    private Teams orgTeams;
 
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
    private bool[] unlockPower;
    private bool[] selectedPower;
    private int numPowersSelected = 0;
    private string[,] powerTextDesc;
    public GameObject powerDescPanel;
    public TextMeshProUGUI powerDescHeaderText;
    public TextMeshProUGUI powerDescText;
    public RawImage powerDescImg;
    public Button buyPowerInfoButton;
    public GameObject loadingCanvas;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;

    void Awake()
    {
        adInit();
    }

    void Start()
    {
        if (Globals.powersStr.Equals("NO"))
        {
            onClickPlay();
            return;
        }


        powerTextDesc = new string[Globals.MAX_POWERS, 1];
        loadingCanvas.SetActive(false);

        //ToRemove
        //Languages.initLangs();
        initPowerTextDesc();

       /* if (!PlayerPrefs.HasKey("POWERS_UNLOCKED"))
        {
            PlayerPrefs.SetString("POWERS_UNLOCKED", "1_1_1_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_" +
                "0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0");
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("POWERS_SELECTED"))
        {
            PlayerPrefs.SetString("POWERS_SELECTED", "1_1_1_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_" +
                "0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0");
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerSkinHair"))
            Globals.customizePlayerSkinHair =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerSkinHair");
        else
            Globals.customizePlayerSkinHair = "f0_s1_b0_t0_hblack1";

        if (PlayerPrefs.HasKey("customizePlayerNationality"))
            Globals.customizePlayerNationality =
                PlayerPrefs.GetString("customizePlayerNationality");
        else
            Globals.customizePlayerNationality = "afghanistan";
        Globals.diamonds = 60000;
        */
        //end to remove


        initUnlockedPowers();


        //to remove
        //playerSellOfferHash = new Dictionary<string, string>();
        audioManager = FindObjectOfType<AudioManager>();
        //saveFileName = Globals.savedFileName;

        //if (Globals.isNewSeason)
        //{
        //    updatePlayersEnergy(true);
        //}

        //playerLeagueName = Globals.leagueName;

        //if (Globals.playerPlayAway)
        //    teamId = Globals.teamBid;
        //else
        //    teamId = Globals.teamAid;

        //Globals.shopTeamIdx = teamId;


        //print("DBGTRANSFER334 TEAMID " + teamId + " leaguName " + playerLeagueName);

        //transfersPanel = GameObject.Find("transfersPanel");
        buyPlayerPanel = GameObject.Find("buyPlayerPanel");
        playerCardBigPanel = GameObject.Find("playerCardBigPanel");
        //sellPanelConfirm = GameObject.Find("sellPanelConfirm");
        infoPanel = GameObject.Find("infoPanel");
        infoPanelText = GameObject.Find("infoPanelText").GetComponent<TextMeshProUGUI>();
        selectionPlrHelperPanel = GameObject.Find("selectionPlrHelperPanel");

        //sellPanelConfirm.SetActive(false);
        infoPanel.SetActive(false);
        selectionPlrHelperPanel.SetActive(false);
        //refillEnergyPanel.SetActive(false);

        //orgTeams = new Teams(playerLeagueName);


        //orgTeams.getTeamByIndex(teamId)[0];

        //print("DBGTEAM1 PLAYERNAME " + playerTeamName + " Globals.leagueName " + Globals.leagueName
        //     + " teamId " + teamId);


        //showExtraPrize();
        initReferenceTextObject();
        initPlayerCardsReferences();
        initBuyPlayerCardReferences();
        //initTransferList();


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
        playerTeamName = Globals.customizeTeamName;

        //uncomment this to show player card
        /*selectedPlayerCardName1.text = Globals.customizePlayerName;


        print("fileName " + PlayerPrefs.GetString("CustomizeTeam_customizePlayerSkinHair"));
        selectedPlayerCardImage1.texture = Resources.Load<Texture2D>(
                                            "playersCard/" +
                                            Globals.customizePlayerSkinHair);
        selectedPlayerCardCountryFlag1.texture =
                              Resources.Load<Texture2D>("Flags/national/" + Globals.customizePlayerNationality);
                             */

        initPowers();
      
        updateCoinsText();
       // GameObject.Find("playGameText").GetComponent<TextMeshProUGUI>().text = "OK";

    }

    private void initReferenceTextObject()
    {
        diamondsNumText = GameObject.Find("diamondsNumText").GetComponent<TextMeshProUGUI>();
        coinsNumText = GameObject.Find("coinsNumText").GetComponent<TextMeshProUGUI>();
    }

    private void initUnlockedPowers()
    {
        string unlockedPowers = PlayerPrefs.GetString("POWERS_UNLOCKED");
        string[] powerState = unlockedPowers.Split('_');

        string selectedPowers = PlayerPrefs.GetString("POWERS_SELECTED");
        string[] selectedPowerState = selectedPowers.Split('_');

        //print("selectedPowers " + selectedPowers);

        unlockPower = new bool[powerState.Length];
        selectedPower = new bool[selectedPowerState.Length];

        for (int i = 0; i < powerState.Length; i++)
        {
            if (powerState[i].Equals("1"))
            {
                unlockPower[i] = true;
            }
            else
            {
                unlockPower[i] = false;
            }
        }

        for (int i = 0; i < Globals.MAX_POWERS; i++)
        {
            if (selectedPowerState[i].Equals("1"))
            {
                selectedPower[i] = true;
                numPowersSelected++;
            }
            else
            {
                selectedPower[i] = false;
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
            img.fillAmount = (float)i / 100f;
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
            /*playerCardFlag[i] = GameObject.Find(
        "playerCardFlag" + (i + 1).ToString()).GetComponent<RawImage>();*/
            //  playerEnergySkillsBar[i] = GameObject.Find(
            //"playerEnergySkillsBar" + (i + 1).ToString()).GetComponent<Image>();
            //    playerCardSkills[i] = GameObject.Find(
            //"playerCardSkills" + (i + 1).ToString()).GetComponent<TextMeshProUGUI>();

            //playerCardPlayerStarImg[i] = GameObject.Find(
            //"playerCardPlayerStar" + (i + 1).ToString()).GetComponent<Image>();
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

       /* selectedPlayerCardCountryFlag1 =
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
*/

        //selectedPlayerCardEnergyBar1 =
        //    GameObject.Find(
        //        "selectedPlayerCardEnergyBar1").GetComponent<Image>();
     /*   selectedPlayerCard1 =
            GameObject.Find(
                "selectedPlayerCard1").GetComponent<Image>();*/

        ///for (int i = 0; i < MAX_TRANSFERS_ROWS; i++)
        //    transferListRow[i] = 
        //        GameObject.Find("transferListRow" + (i + 1).ToString());
        //transfersPanel.SetActive(false);

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
        //buyPlayerCardCountryFlag =
        //    GameObject.Find("buyPlayerCardCountryFlag").GetComponent<RawImage>();
        //buyPlayerCardClubFlag =
        //    GameObject.Find("buyPlayerCardClubFlag").GetComponent<RawImage>();
        //buyPlayerCardClubFlagGameObj =
        //    GameObject.Find("buyPlayerCardClubFlag");
        buyPlayerCardDiamondGameObj =
            GameObject.Find("buyPlayerCardDiamondsImg");
        //buyPlayerCardDefenseSkillsBar =
        //    GameObject.Find("buyPlayerCardDefenseSkillsBar").GetComponent<Image>();
        //buyPlayerCardAttackSkillsBar =
        //    GameObject.Find("buyPlayerCardAttackSkillsBar").GetComponent<Image>();
        //buyPlayerCardSkillsDefenseText =
        //    GameObject.Find("buyPlayerCardSkillsDefenseText").GetComponent<TextMeshProUGUI>();
        // buyPlayerCardSkillsAttackText =
        //     GameObject.Find("buyPlayerCardSkillsAttackText").GetComponent<TextMeshProUGUI>();
        // buyPlayerClubNameText =
        //     GameObject.Find("buyPlayerClubNameText").GetComponent<TextMeshProUGUI>();
        buyPlayerClubDiamondPriceText =
            GameObject.Find("buyPlayerClubDiamondPriceText").GetComponent<TextMeshProUGUI>();

        buyPlayerPanel.SetActive(false);
    }

    private int getNumberPlrInTeam(int teamId)
    {
        return orgTeams.getTeamByIndex(teamId)[12].Split('|').Length;
    }

    private int initPowers()
    {
        ///string[] player = orgTeams.getTeamByIndex(teamId)[12].Split('|');

        //print("#DBGPLAYR player len " + player.Length + " teamId " + teamId);

        //int selectedIdx = -1;
        //print("player descriptigo long " + orgTeams.getTeamByIndex(teamId)[12]);

        numOfUnlockedPlayers = 0;
        for (int i = 0; i < Globals.MAX_POWERS; i++) {            
            playerCard[i].SetActive(true);

            //string tmpPlayerDesc = player[i];
            playerCardButton[i].onClick.RemoveAllListeners();
            if (!unlockPower[i])
            {
                int index = i;
                playerCardButton[i].onClick.AddListener(
                           delegate { onClickShowPlayerCardToBuy(index); });
            }

            playerCardName[i].text = Languages.getTranslate(Globals.powerNames[i]);

            playerCardPlayerImg[i].texture =
                Resources.Load<Texture2D>(
                    Globals.powerFileNames[i]);

      
            if (!unlockPower[i])
            {
                playerCardPrice[i].text = Globals.powerPrices[i].ToString();

                Color colorDiamond;
                ColorUtility.TryParseHtmlString("#FFFFFFFF", out colorDiamond);

                playerCardSelectionButton[i].interactable = false;
                playerCardSelectionImg[i].color = colorDiamond;
                playerCardSelectionImg[i].texture =
                Resources.Load<Texture2D>("others/diamonds");
            } else
            {
                Color colorSelection;
                ColorUtility.TryParseHtmlString("#FFEA2D", out colorSelection);

                if (selectedPower[i])
                {
                    colorSelection.a = 1f;
                } else
                {
                    colorSelection.a = 0.1f;
                }

                playerCardSelectionImg[i].color = colorSelection;
                playerCardSelectionButton[i].interactable = true;
                playerCardSelectionImg[i].texture =
                    Resources.Load<Texture2D>("others/teamSelected");

                playerCardPrice[i].text = "";
                numOfUnlockedPlayers++;
            }
        }

        for (int j = Globals.MAX_POWERS; j < 0; j++)
            playerCard[j].SetActive(false);

        return 0;
    }

    private void initUnlockedPlayerCardReferences()
    {
        unlockedPlayerCardPanel =
            GameObject.Find("unlockedPlayerCardPanel");
        unlockedPlayerCardPanelSmaller =
            GameObject.Find("unlockedPlayerCardPanelSmaller");
        unlockedPlayerCardNameText =
            GameObject.Find("unlockedPlayerCardNameText").GetComponent<TextMeshProUGUI>();       
        unlockedPlayerCardPlayerImage =
            GameObject.Find("unlockedPlayerCardPlayerImage").GetComponent<RawImage>();

        unlockedPlayerCardPanel.SetActive(false);
    }

    public void selectUnselectPower(int idx)
    {     
        if (!playerCardSelectionButton[idx].interactable)
            return;

        var tempColor = playerCardSelectionImg[idx].color;
        //deslected then
        if (tempColor.a > 0.2f)
        {
            tempColor.a = 0.1f;
            numPowersSelected--;
            //unlockPower[idx] = false;
            selectedPower[idx] = false;
            saveSelectedPowersArr();
        }
        else
        {
            if (numPowersSelected <= 2)
            {
                tempColor.a = 1f;
                numPowersSelected++;
                selectedPower[idx] = true;
                saveSelectedPowersArr();
            }
            else
            {
                //max 3 selected info
                selectionPlrHelperPanel.SetActive(true);
                return;
            }
        }

        playerCardSelectionImg[idx].color = tempColor;     
    }


/*    public void onClickPlay()
    {
        print("Globals.isMultiplayer " + Globals.isMultiplayer);

        Globals.recoverOriginalResolution();

        if (Globals.isMultiplayer)
        {
            Globals.loadSceneWithBarLoader("multiplayerMenu");
        }
        else
        {
            SceneManager.LoadScene("specialShopOffers");
        }
    }*/

    //This is used for buying and selling player 
    public void onClickShowPlayerCardToBuy(int idx)
    {        
        buyPlayerNameText.text = Globals.powerNames[idx];
        
        buyPlayerCardPlayerImage.texture =
              Resources.Load<Texture2D>(Globals.powerFileNames[idx]);
       
        string price = Globals.powerPrices[idx].ToString();
        buyPlayerClubDiamondPriceText.text = price;

        buyPlayerCardYesButton.onClick.RemoveAllListeners();
        buyPowerInfoButton.onClick.RemoveAllListeners();
        buyPowerInfoButton.onClick.AddListener(
            delegate {
                onClickShowPowerTextInfo(idx);
        });


        if (!unlockPower[idx])
        {
            buyPlayerHeaderText.text =
                Languages.getTranslate("Unlock power");
            buyPlayerCardDiamondGameObj.SetActive(true);

            if (int.Parse(price) > Globals.diamonds)
            {
                buyPlayerCardNotificationText.text =
                     Languages.getTranslate("You don't have enough diamonds to buy this power.\n" +
                    "Go to the shop to buy diamonds");
                buyPlayerCardYesButtonText.text =
                    Languages.getTranslate("SHOP");

                //buyPlayerCardYesButton.onClick.AddListener(
                //              delegate {
                //                  shopScript.showDiamondsPanel();
                //              });
                /*show shop canvas*/
            } else
            {
                buyPlayerCardYesButton.onClick.AddListener(
                               delegate {
                                   onClickBuyButtonYes(idx);
                               });
                buyPlayerCardNotificationText.text =
                    Languages.getTranslate("Buy power for " + price + " diamonds",
                                            new List<string>() { price });
                buyPlayerCardYesButtonText.text =
                    Languages.getTranslate("UNLOCK");
            }
        }

        buyPlayerPanel.SetActive(true);
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


    //used by external scripts
    public void onClickExitShopPanel()
    {        
        shopPanel.SetActive(false);
    }

    private void saveUnlockedPowersArr()
    {
        string unlockStr = "";
        for (int i = 0; i < unlockPower.Length; i++)
        {
            if (unlockPower[i])
            {
                unlockStr += "1_";
            } else
            {
                unlockStr += "0_";
            }
        }

        unlockStr = unlockStr.Remove(unlockStr.Length - 1);

        //if (!PlayerPrefs.HasKey("POWERS_UNLOCKED"))
        //{
            PlayerPrefs.SetString("POWERS_UNLOCKED", unlockStr);
            PlayerPrefs.Save();
        //}
    }


    private void saveSelectedPowersArr()
    {
        string selectedStr = "";
        for (int i = 0; i < selectedPower.Length; i++)
        {
            if (selectedPower[i])
            {
                selectedStr += "1_";
            }
            else
            {
                selectedStr += "0_";
            }
        }

        selectedStr = selectedStr.Remove(selectedStr.Length - 1);

        //if (!PlayerPrefs.HasKey("POWERS_SELECTED"))
        //{
            PlayerPrefs.SetString("POWERS_SELECTED", selectedStr);
            PlayerPrefs.Save();
        //}
    }

    public void onClickBuyButtonYes(int idx)
    {       
        unlockPower[idx] = true;
        saveUnlockedPowersArr();
      
        int price = 2000;        
        Globals.addDiamonds(-price);

        initPowers();

       
        updateCoinsText();
        buyPlayerPanel.SetActive(false);

        StartCoroutine(showUnlockedCard(idx));
    }
    IEnumerator showUnlockedCard(int idx)
    {
        unlockedPlayerCardPanel.SetActive(true);
        unlockedPlayerCardPanelSmaller.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        audioManager.Play("elementAppear");
        unlockedPlayerCardPanelSmaller.SetActive(true);

        unlockedPlayerCardNameText.text =
            Globals.powerNames[idx];
       
        unlockedPlayerCardPlayerImage.texture =
            Resources.Load<Texture2D>(Globals.powerFileNames[idx]);

        yield return new WaitForSeconds(3f);
        unlockedPlayerCardPanel.SetActive(false);
        
    }

    public void updateAfterBuying(string type)
    {
        updateCoinsText();

        if (type.Equals("unlockedplayercard"))
        {
            initPowers();
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

    private void initPowerTextDesc()
    {
        powerDescPanel.SetActive(false);

        powerTextDesc[0, 0] = Languages.getTranslate(
            "Power type: Offensive\n\n" +
            "This power adds two additional small goals next to standard one.\n" +
            "You can score to any of 3 goals then\n\n" +
            "When used by opponent you can use:\n" +
            "Bad Weather, Cut Goal Back, Earthquake, Goal Wall, Invisibility, Flares\n\n");
        powerTextDesc[1, 0] = Languages.getTranslate(
            "Type: Defensive\n\n" +
            "This power decreases height of your goal.\n" +
            "It can be used when opponent strikes to minimize chance of scoring goal\n\n" +
            "When used by opponent you can use:\n" +
            "Enlarge Goal, Invisibility, Bad Weather, Earthquake\n\n");
        powerTextDesc[2, 0] = Languages.getTranslate(
            "Power type: Offensive\n\n" +
            "This power enlarge opponent goal.\n" +
            "You can used it when shooting\n\n" +
            "When used by opponent you can use:\n" +
            "Cut Goal Back, Goal Wall, Bad Weather, Flares, Earthquake, Invisibility\n\n");
        powerTextDesc[3, 0] = Languages.getTranslate(
            "Power type: Offensive\n\n" +
            "If you score a goal this power will give you two goals instead of one\n\n" +
            "When used by opponent you can use:\n" +
            "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Earthquake\n\n");
        powerTextDesc[4, 0] = Languages.getTranslate(
            "Power type: Offensive, Defensive\n\n" +
            "This power makes an earthquake.\n" +
            "It's more difficult to attack and defend in such conditions\n\n" +
            "When used by opponent you can use:\n" +
            "Goal Wall, Cut Goal Back, Invisibility, Earthquake, Flares, Bad Weather\n\n");
        powerTextDesc[5, 0] = Languages.getTranslate(
            "Power type: Defensive\n\n" +
            "The power creates a wall that covers standard size goal.\n" +
            "Opponent cannot scores a goal unless Enlarge goal will be used\n\n" +
            "When used by opponent you can use\n" +
            "Enlarge Goal, Extra Goals\n\n");
        powerTextDesc[6, 0] = Languages.getTranslate(
            "Power type: Offensive, Defensive\n\n" +
            "This makes the ball and your player invisible.\n" +
            "You can mislead opponent and strikes from any position on the pitch\n\n" +
            "When used by opponent you can use:\n" +
            "Flares, Goal Wall, Cut Goal Back, Bad Weather, Invisibility, Earthquake\n\n");
        powerTextDesc[7, 0] = Languages.getTranslate(
            "Power type: Offensive, Defensive\n\n" +
            "This power makes extreme weather conditions for your opponent like snow, storm and wind.\n" +
            "It decreases his run speed\n\n" +
            "When used by opponent you can use:\n" +
            "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n");
        powerTextDesc[8, 0] = Languages.getTranslate(
            "Power type: Offensive, Defensive\n\n" +
            "The power enables flare on your opponent half that limits visibility for him\n\n" +
            "When used by opponent you can use:\n" +
            "Goal Wall, Cut Goal Back, Flares, Earthquake\n\n");
        powerTextDesc[9, 0] = Languages.getTranslate(
            "Power type: Offensive\n\n" +
            "This is very powerful power.\n" +
            "If you score a goal this power will give you three goals instead of one.\n" +
            "But you have to score first!\n\n" +
            "When used by opponent you can use:\n" +
            "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Flares, Earthquake\n\n");
    }

    public void onClickShowPowerTextInfo(int idx)
    {
        powerDescPanel.SetActive(true);
        powerDescHeaderText.text = Globals.powerNames[idx];
        powerDescText.text = powerTextDesc[idx, 0];
        powerDescImg.texture = Resources.Load<Texture2D>(
                        Globals.powerFileNames[idx]);        
    }

    public void closePowerTextInfoPanel()
    {
        powerDescPanel.SetActive(false);
    }

    public void onClickPlay()
    {
        hideBanner();
        loadingCanvas.SetActive(true);

        if (Globals.stadiumNumber == 1)
        {
            Globals.loadSceneWithBarLoader("gameSceneSportsHall");
        } else if (Globals.stadiumNumber == 2)
        {
            Globals.loadSceneWithBarLoader("gameSceneStreet");
        }
        else
        {
            Globals.loadSceneWithBarLoader("gameScene");
        }
    }
    
    public void onClickCloseShop()
    {
        setupDefaults();
        shopCanvas.SetActive(false);
        buyPlayerPanel.SetActive(false);
    }

    private void adInit()
    {
        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();
        admobAdsScript.setAdsClosed(false);
        admobAdsScript.setAdsFailed(false);
        admobAdsScript.setAdsRewardEarn(false);
        //showBanner();

        //admobAdsScript.hideBanner();
    }

    private void showBanner()
    {
        //Debug.Log("showBanner 1");
        if (admobGameObject != null &&
            admobAdsScript != null)
        {
            admobAdsScript.showBannerAd();
        }
    }

    private void hideBanner()
    {
        //Debug.Log("hideBanner");
        if (admobGameObject != null &&
            admobAdsScript != null)
        {
            admobAdsScript.hideBanner();
        }
    }
}
