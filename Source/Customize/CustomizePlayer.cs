using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using MenuCustomNS;
using UnityEngine.UI;
using TMPro;
using graphicsCommonNS;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using System.IO;
using MenuCustomNS;
using LANGUAGE_NS;
using AudioManagerNS;

public class CustomizePlayer : MonoBehaviour
{
    private int MAIN_MENU_MAX_BUTTONS = 7;
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
    private Image[] playerCardPlayerStarImg;
    private RawImage[] playerCardPlayerImg;
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

    private Dictionary<string, string> playerSellOfferHash;
    private int numOfUnlockedPlayers = 0;

    public GameObject mainModel3D;
    public GameObject mainModelHair3D;
    public GameObject shirtPrevNextButtons;
    public GameObject shortsPrevNextButtons;
    public GameObject sockPrevNextButtons;
    public GameObject shoePrevNextButtons;
    public GameObject glovesPrevNextButtons;
    public GameObject skinPrevNextButtons;

    private int currentShirtIdx = 0;
    private int currentShortsIdx = 0;
    private int currentSockIdx = 0;
    private int currentShoeIdx = 0;
    private int currentSGlovesIdx = 0;
    private int currentSHairIdx = 0;
    private int currentSkinIdx = 0;
    private int currATeamIdx = 0;
    private int flagsColorIdx = 0;
    private int fansColorIdx = 0;

    public GameObject nationalityChoosePanel; 
    public TextMeshProUGUI teamNationalityName;
    public RawImage nationalityMainFlag;
    public RawImage playerCardFlag;
    public GameObject treasuePanel;

    public InputField inputTeamName;
    public InputField inputPlayerName;
    public GameObject[] mainButtonsFocus;
    public RawImage playerCardImage;
    public TextMeshProUGUI priceText;
    public GameObject saveButtonsGameObject;
    public Button saveButton;
    private int PRICE_PER_PRODUCT = 2000;
    public GameObject shopNotification;

    private int teamAFlagPrevActive = 0;
    private float teamAFlagPrevLastTimeChanged = 0f;
    private int teamAFlagNextActive = 0;
    private float teamAFlagNextLastTimeChanged = 0f;
    private Image LocalProfileImage;
    public RawImage logoImage;
    private string logoFilePath = "";
    private string logoBackgroundFilePath = "";
    public TextMeshProUGUI currentDiamondsNumberText;
    public TextMeshProUGUI playerCardName;
    public GameObject buyItemPanel;
    public TextMeshProUGUI buyItemButtonText;
    public TextMeshProUGUI buyItemNotification;
    private string currentItemName;
    public Button buyItemButton;
    private StringCommon strCommon;
    public GameObject fansColorPanel;
    public RawImage fansFlagColorImg;
    public RawImage fansColorImg;
    public GameObject leaguesAddedInfoPanel;
    public TextMeshProUGUI bodyTextLeaguesAddedPanelInfo;
    public GameObject oopsPanel;
    public TextMeshProUGUI oopsPanelInfoText;
    public TextMeshProUGUI oopsPanelHeaderText;


    public Text playerNamePlaceHolderText;
    public Text teamNamePlaceHolderText;
    public GameObject yesNoPanel;
    public TextMeshProUGUI yesNoPanelMainText;
    public bool isCustomizeOpenFirstTime;

    private string[] fansFlagColors =
    {
        "black_4","blue_4","blueblack_4","bluered_4","claret_4","claretdarkblue_4","claretorange_4",
        "darkclaretdarkgreen_4","darkgreen_4","green_4","lightblue_4","orange_4","orangeblack_4",
        "purple_4","red_4","redblack_4","redgreen_4","rednavyblue_4","yellowblack_4","yellowblue_4"
    };

    private string[] fansColor =
    {
        "fans_black","fans_blue","fans_claret","fans_darkgreen","fans_green","fans_lightblue","fans_orange",
        "fans_purple","fans_red","fans_white","fans_yellow"
    };

    private string[] flagsFileName =
    {
       "afghanistan","aland","albania","algeria","american samoa","andorra","angola","anguilla",
        "antarctica","antigua and barbuda","argentina","armenia","aruba","australia","austria",
        "azerbaijan","bahrain","bangladesh","barbados","belarus","belgium","belize","benin",
        "bermuda","bhutan","bolivia","bosnia","botswana","brazil","british virgin islands","brunei",
        "bulgaria","burkina faso","burundi","cambodia","cameroon","canada","cape verde",
        "cayman islands","central african republic","chad","chile","china","colombia","comoros",
        "congo","cook islands","costarica","croatia","cuba","curaçao","cyprus no mans area","cyprus",
        "czechia","democratic republic of the congo","denmark","djibouti","domin. republic","dominica",
        "east timor","ecuador","egypt","el salvador","england","equatorial guinea","eritrea",
        "estonia","ethiopia","falkland islands","faroe islands",
        "federated states of micronesia","fiji","finland","france","french polynesia",
        "french southern and antarctic lands","gabon","gambia","georgia","germany","ghana",
        "greece","greenland","grenada","guam","guatemala","guernsey","guinea bissau","guinea",
        "guyana","haiti","honduras","hong kong","hungary","iceland","india","indonesia","iran",
        "iraq","ireland","israel","italy","ivory coast","jamaica","japan","jordan","kazakhstan",
        "kenya","kiribati","kosovo","kuwait","kyrgyzstan","laos","latvia","lebanon","lesotho","liberia",
        "libya","liechtenstein","lithuania","luxembourg","macao","macedonia","madagascar","malawi",
        "malaysia","mali","malta","marshall islands","mauritania","mauritius","mexico","moldova",
        "monaco","mongolia","montenegro","montserrat","morocco","mozambique","myanmar","namibia",
        "nepal","netherlands","new caledonia","new zealand","nicaragua","niger","nigeria","niue",
        "norfolk island","north korea","northern cyprus","northern ireland",
        "northern mariana islands","norway","oman","pakistan","palau","palestine","panama",
        "papua new guinea","paraguay","peru","philippines","poland","portugal",
        "puerto rico","qatar","republic of congo","romania","russia","rwanda","saint kitts and nevis",
        "saint lucia","saint martin","saint vincent and the grenadines","samoa",
        "san marino","sao tome and principe","saudi arabia","scotland","senegal","serbia",
        "seychelles","siachen glacier","sierra leone","singapore","sint maarten","slovakia",
        "slovenia","solomon islands","somalia","somaliland","south africa",
        "south georgia and south sandwich islands","south korea","south sudan","spain",
        "sri lanka","sudan","suriname","swaziland","sweden","switzerland","syria","taiwan",
        "tajikistan","thailand","the bahamas","togo","tonga","trinidad and tobago","tunisia",
        "turkey","turkmenistan","turks and caicos islands","uganda","ukraine",
        "united arab emirates","united republic of tanzania","united states virgin islands",
        "uruguay","usa","uzbekistan","vanuatu","venezuela","vietnam","wales","wallis and futuna",
        "western sahara","yemen","zambia","zimbabwe",
    };

    public static string[] shoesFilesNames = { "shoewhite", 
                                                "shoeblackred", "shoeblack", "shoeredwhite", "shoered", 
                                                "shoeorange", 
                                                "shoebluewhite", 
                                                "shoeyellow", "shoewhiteblack", "shoegreen", 
                                                "shoeblue" };

    public static string[] glovesFilesNames = { "globewhiteblack",
                                          "glovebluedarkblue", "gloveredwhite", "gloveyellowwhite", 
                                          "gloveyellow",
                                          "gloveblueyellow", "gloveblackorange", "gloveredblack",
                                          "glovebluewhite",
                                          "glovegreenwhite", "gloveblacblue", "gloveblackgreen",
                                          "glovelightbluewhite", "gloveblackwhite", "glovebluered", "gloveyellowblack",
                                          };
    public static string[] skinsFileNames = {
        "f0_s1_b0_t0_hblack1","f0_s1_b0_t0_hblack13","f4_s4_b2_t10_hblack5",
        "f0_s1_b0_t0_hblack4","f0_s1_b0_t0_hblack5","f0_s1_b0_t0_hblack6","f0_s1_b0_t0_hblack7",
        "f0_s1_b0_t0_hblack8","f0_s1_b0_t0_hblack9","f0_s1_b0_t0_hblonde3","f0_s1_b0_t0_hblonde4",
        "f0_s1_b0_t0_hred1","f0_s1_b2_t2_hblack1","f0_s1_b2_t2_hblack10","f0_s1_b2_t2_hblack2",
        "f0_s1_b2_t2_hblack3","f0_s1_b2_t2_hblack4","f0_s1_b2_t2_hblack5","f0_s1_b2_t2_hblack6",
        "f0_s1_b2_t2_hblack7","f0_s1_b2_t2_hblack8","f0_s1_b2_t2_hblack9","f0_s1_b2_t2_hblonde2",
        "f0_s1_b2_t2_hnohair","f0_s2_b0_t0_hblack10","f0_s2_b0_t0_hblack3","f0_s2_b0_t0_hblack4",
        "f0_s2_b0_t0_hblack5","f0_s2_b0_t0_hblack9","f0_s2_b0_t0_hblonde1","f0_s2_b0_t0_hblonde2",
        "f0_s2_b0_t0_hblonde4","f0_s2_b0_t0_hred2","f0_s2_b0_t0_hred3","f0_s2_b0_t0_hred4",
        "f0_s2_b2_t4_hblack15","f0_s2_b2_t4_hblack3","f0_s2_b2_t4_hred2","f0_s2_b2_t4_hred3",
        "f0_s4_b0_t0_hblack13","f0_s4_b0_t0_hblack3","f0_s4_b0_t0_hblack5","f0_s4_b0_t0_hblack7",
        "f0_s4_b2_t5_hblack1","f0_s4_b2_t5_hblack10","f0_s4_b2_t5_hblack3","f0_s4_b2_t5_hblack5",
        "f0_s5_b0_t0_hblack1","f0_s5_b0_t0_hblack14","f0_s5_b0_t0_hblack3","f0_s5_b0_t0_hblack5",
        "f0_s5_b0_t0_hnohair","f0_s5_b2_t6_hblack10","f0_s5_b2_t6_hblack13","f0_s5_b2_t6_hblack14",
        "f0_s5_b2_t6_hblack15","f0_s5_b2_t6_hblack3","f0_s5_b2_t6_hblack5","f0_s5_b2_t6_hblack6",
        "f0_s5_b2_t6_hblack8","f1_s1_b0_t0_hblack1","f1_s1_b0_t0_hblack10","f1_s1_b0_t0_hblack12",
        "f1_s1_b0_t0_hblack13","f1_s1_b0_t0_hblack3","f1_s1_b0_t0_hblack4","f1_s1_b0_t0_hblack5",
        "f1_s1_b0_t0_hblack6","f1_s1_b0_t0_hblack7","f1_s1_b0_t0_hblack8","f1_s1_b0_t0_hblonde1",
        "f1_s1_b0_t0_hblonde2","f1_s1_b0_t0_hblonde4","f1_s1_b0_t0_hred2","f1_s1_b2_t7_hblack10",
        "f1_s1_b2_t7_hblack12","f1_s1_b2_t7_hblack3","f1_s1_b2_t7_hblack5","f1_s1_b2_t7_hblack6",
        "f1_s1_b2_t7_hblack7","f1_s1_b2_t7_hblack8","f1_s1_b2_t7_hblack9","f1_s1_b2_t7_hblonde1",
        "f1_s1_b2_t7_hblonde2","f1_s1_b2_t7_hblonde3","f1_s1_b2_t7_hblonde5","f1_s1_b2_t7_hnohair",
        "f1_s1_b2_t7_hred3","f1_s1_b2_t7_hred4","f1_s2_b0_t0_hblack1","f1_s2_b0_t0_hblack10",
        "f1_s2_b0_t0_hblack4","f1_s2_b0_t0_hblonde3","f1_s2_b0_t0_hblonde6","f1_s2_b0_t0_hred3",
        "f1_s2_b2_t8_hblonde2","f1_s4_b0_t0_hblack1","f1_s4_b0_t0_hblack3","f1_s4_b0_t0_hblack5",
        "f1_s4_b0_t0_hblack8","f1_s4_b0_t0_hblack9","f1_s4_b2_t9_hblack3","f1_s4_b2_t9_hblack5",
        "f1_s5_b0_t0_hblack1","f1_s5_b0_t0_hblack13","f1_s5_b0_t0_hblack14","f1_s5_b0_t0_hblack3",
        "f1_s5_b2_t10_hblack1","f1_s5_b2_t10_hblack14","f1_s5_b2_t10_hblack3","f1_s5_b2_t10_hblack5",
        "f2_s1_b0_t0_hblack1","f2_s1_b0_t0_hblack10","f2_s1_b0_t0_hblack12","f2_s1_b0_t0_hblack3",
        "f2_s1_b0_t0_hblack5","f2_s1_b0_t0_hblack7","f2_s1_b0_t0_hblack8","f2_s1_b0_t0_hblack9",
        "f2_s1_b0_t0_hblonde1","f2_s1_b0_t0_hblonde3","f2_s1_b0_t0_hblonde4","f2_s1_b0_t0_hblonde6",
        "f2_s1_b2_t8_hblack1","f2_s1_b2_t8_hblack10","f2_s1_b2_t8_hblack14","f2_s1_b2_t8_hblack3",
        "f2_s1_b2_t8_hblack4","f2_s1_b2_t8_hblack5","f2_s1_b2_t8_hblack6","f2_s1_b2_t8_hblack8",
        "f2_s1_b2_t8_hblonde1","f2_s1_b2_t8_hblonde6","f2_s2_b0_t0_hblack10","f2_s2_b0_t0_hblack3",
        "f2_s2_b0_t0_hblack4","f2_s2_b0_t0_hblack5","f2_s2_b0_t0_hblack8","f2_s2_b0_t0_hblonde1",
        "f2_s2_b0_t0_hblonde2","f2_s2_b0_t0_hblonde3","f2_s2_b0_t0_hblonde4","f2_s2_b2_t9_hblack3",
        "f2_s4_b0_t0_hblack1","f2_s4_b0_t0_hblack10","f2_s4_b0_t0_hblack13","f2_s4_b0_t0_hblack14",
        "f2_s4_b0_t0_hblack4","f2_s4_b0_t0_hblack5","f2_s4_b0_t0_hblack6","f2_s4_b2_t10_hblack10",
        "f2_s4_b2_t10_hblack3","f2_s4_b2_t10_hblack4","f2_s4_b2_t10_hblack5","f2_s4_b2_t10_hblack9",
        "f2_s4_b2_t10_hnohair","f2_s5_b0_t0_hblack11","f2_s5_b0_t0_hblack3","f2_s5_b0_t0_hblack5",
        "f2_s5_b0_t0_hblack8","f2_s5_b2_t3_hblack1","f2_s5_b2_t3_hblack14","f2_s5_b2_t3_hblack3",
        "f2_s5_b2_t3_hblack5","f2_s5_b2_t3_hnohair","f3_s1_b0_t0_hblack1","f3_s1_b0_t0_hblack10",
        "f3_s1_b0_t0_hblack14","f3_s1_b0_t0_hblack15","f3_s1_b0_t0_hblack2","f3_s1_b0_t0_hblack4",
        "f3_s1_b0_t0_hblack5","f3_s1_b0_t0_hblack6","f3_s1_b0_t0_hblack7","f3_s1_b0_t0_hblack8",
        "f3_s1_b0_t0_hblonde1","f3_s1_b0_t0_hblonde3","f3_s1_b0_t0_hblonde4","f3_s1_b0_t0_hred1",
        "f3_s1_b0_t0_hred3","f3_s1_b2_t4_hblack10","f3_s1_b2_t4_hblack12","f3_s1_b2_t4_hblack15",
        "f3_s1_b2_t4_hblack3","f3_s1_b2_t4_hblack4","f3_s1_b2_t4_hblack5","f3_s1_b2_t4_hblack6",
        "f3_s1_b2_t4_hblack8","f3_s1_b2_t4_hblonde1","f3_s1_b2_t4_hblonde5","f3_s1_b2_t4_hred4",
        "f3_s2_b0_t0_hblack10","f3_s2_b0_t0_hblack3","f3_s2_b0_t0_hblack4","f3_s2_b0_t0_hblack5",
        "f3_s2_b0_t0_hblack6","f3_s2_b0_t0_hblack7","f3_s2_b0_t0_hblonde1","f3_s2_b0_t0_hblonde2",
        "f3_s2_b0_t0_hblonde3","f3_s2_b0_t0_hblonde5","f3_s2_b2_t5_hblack1","f3_s2_b2_t5_hblack10",
        "f3_s2_b2_t5_hblack5","f3_s2_b2_t5_hblack6","f3_s4_b0_t0_hblack1","f3_s4_b0_t0_hblack12",
        "f3_s4_b0_t0_hblack2","f3_s4_b0_t0_hblack6","f3_s4_b0_t0_hblack9","f3_s4_b2_t6_hblack1",
        "f3_s4_b2_t6_hblack10","f3_s4_b2_t6_hblack14","f3_s4_b2_t6_hblack6","f3_s4_b2_t6_hblack8",
        "f3_s5_b0_t0_hblack1","f3_s5_b0_t0_hblack3","f3_s5_b0_t0_hblack5","f3_s5_b2_t7_hblack10",
        "f3_s5_b2_t7_hblack12","f3_s5_b2_t7_hblack14","f3_s5_b2_t7_hblack3","f3_s5_b2_t7_hblack5",
        "f3_s5_b2_t7_hnohair","f4_s1_b0_t0_hblack1","f4_s1_b0_t0_hblack10","f4_s1_b0_t0_hblack12",
        "f4_s1_b0_t0_hblack3","f4_s1_b0_t0_hblack5","f4_s1_b0_t0_hblack7","f4_s1_b0_t0_hblonde1",
        "f4_s1_b0_t0_hblonde2","f4_s1_b0_t0_hblonde4","f4_s1_b2_t8_hblack1","f4_s1_b2_t8_hblack10",
        "f4_s1_b2_t8_hblack11","f4_s1_b2_t8_hblack12","f4_s1_b2_t8_hblack13","f4_s1_b2_t8_hblack3",
        "f4_s1_b2_t8_hblack5","f4_s1_b2_t8_hblack6","f4_s1_b2_t8_hblack7","f4_s1_b2_t8_hblack8",
        "f4_s1_b2_t8_hblack9","f4_s1_b2_t8_hblonde1","f4_s1_b2_t8_hblonde2","f4_s1_b2_t8_hblonde3",
        "f4_s1_b2_t8_hred3","f4_s2_b0_t0_hblack1","f4_s2_b0_t0_hblack10","f4_s2_b0_t0_hblack13",
        "f4_s2_b0_t0_hblack4","f4_s2_b0_t0_hblack5","f4_s2_b0_t0_hblack7","f4_s2_b0_t0_hblonde1",
        "f4_s2_b0_t0_hblonde4","f4_s2_b2_t9_hblack2","f4_s2_b2_t9_hblack3","f4_s2_b2_t9_hblack5",
        "f4_s2_b2_t9_hblonde4","f4_s2_b2_t9_hblonde5","f4_s4_b0_t0_hblack1","f4_s4_b0_t0_hblack2",
        "f4_s4_b0_t0_hblack4","f4_s4_b0_t0_hblack5","f4_s4_b2_t10_hblack13","f4_s4_b2_t10_hblack3",
        "f4_s4_b2_t10_hblack5","f4_s4_b2_t10_hred5","f4_s5_b0_t0_hblack1","f4_s5_b0_t0_hblack3",
        "f4_s5_b0_t0_hblack5","f4_s5_b2_t4_hblack1","f4_s5_b2_t4_hblack14","f4_s5_b2_t4_hblack3",
        "f4_s5_b2_t4_hblack5","f4_s5_b2_t4_hnohair","f5_s1_b0_t0_hblack1","f5_s1_b0_t0_hblack10",
        "f5_s1_b0_t0_hblack11","f5_s1_b0_t0_hblack2","f5_s1_b0_t0_hblack3","f5_s1_b0_t0_hblack4",
        "f5_s1_b0_t0_hblack5","f5_s1_b0_t0_hblack6","f5_s1_b0_t0_hblack7","f5_s1_b0_t0_hblonde2",
        "f5_s1_b0_t0_hblonde3","f5_s1_b0_t0_hred4","f5_s1_b2_t5_hblack10","f5_s1_b2_t5_hblack14",
        "f5_s1_b2_t5_hblack2","f5_s1_b2_t5_hblack3","f5_s1_b2_t5_hblack4","f5_s1_b2_t5_hblack5",
        "f5_s1_b2_t5_hblack6","f5_s1_b2_t5_hblack8","f5_s1_b2_t5_hblack9","f5_s1_b2_t5_hblonde3",
        "f5_s1_b2_t5_hblonde5","f5_s1_b2_t5_hnohair","f5_s1_b2_t5_hred4","f5_s2_b0_t0_hblack10",
        "f5_s2_b0_t0_hblack3","f5_s2_b0_t0_hblack4","f5_s2_b0_t0_hblack5","f5_s2_b0_t0_hblack7",
        "f5_s2_b0_t0_hblonde1","f5_s2_b0_t0_hblonde2","f5_s2_b0_t0_hblonde5","f5_s2_b0_t0_hred2",
        "f5_s2_b2_t6_hblack10","f5_s2_b2_t6_hblack3","f5_s2_b2_t6_hblack8","f5_s2_b2_t6_hblack9",
        "f5_s2_b2_t6_hblonde1","f5_s2_b2_t6_hblonde4","f5_s2_b2_t6_hred6","f5_s4_b0_t0_hblack1",
        "f5_s4_b0_t0_hblack10","f5_s4_b0_t0_hblack15","f5_s4_b0_t0_hblack3","f5_s4_b0_t0_hblack5",
        "f5_s4_b2_t7_hblack1","f5_s4_b2_t7_hblack12","f5_s4_b2_t7_hblack15","f5_s4_b2_t7_hblack2",
        "f5_s4_b2_t7_hblack3","f5_s4_b2_t7_hblack6","f5_s4_b2_t7_hnohair","f5_s5_b0_t0_hblack3",
        "f5_s5_b0_t0_hblack5","f5_s5_b2_t8_hblack11","f5_s5_b2_t8_hblack3","f5_s5_b2_t8_hblack9",
        "f5_s5_b2_t8_hnohair","f6_s1_b0_t0_hblack1","f6_s1_b0_t0_hblack10","f6_s1_b0_t0_hblack13",
        "f6_s1_b0_t0_hblack2","f6_s1_b0_t0_hblack3","f6_s1_b0_t0_hblack4","f6_s1_b0_t0_hblack5",
        "f6_s1_b0_t0_hblack6","f6_s1_b0_t0_hblack8","f6_s1_b0_t0_hblonde1","f6_s1_b0_t0_hblonde3",
        "f6_s1_b0_t0_hblonde5","f6_s1_b0_t0_hred3","f6_s1_b2_t9_hblack10","f6_s1_b2_t9_hblack13",
        "f6_s1_b2_t9_hblack14","f6_s1_b2_t9_hblack15","f6_s1_b2_t9_hblack2","f6_s1_b2_t9_hblack3",
        "f6_s1_b2_t9_hblack4","f6_s1_b2_t9_hblack5","f6_s1_b2_t9_hblack7","f6_s1_b2_t9_hblonde3",
        "f6_s1_b2_t9_hred3","f6_s3_b0_t0_hblack1","f6_s3_b0_t0_hblack10","f6_s3_b0_t0_hblack11",
        "f6_s3_b0_t0_hblack3","f6_s3_b0_t0_hblack8","f6_s3_b0_t0_hblack9","f6_s3_b0_t0_hblonde3",
        "f6_s3_b0_t0_hred3","f6_s3_b2_t10_hblack10","f6_s3_b2_t10_hblack3","f6_s3_b2_t10_hblack4",
        "f6_s3_b2_t10_hblack5","f6_s3_b2_t10_hblack8","f6_s3_b2_t10_hblonde4","f6_s4_b0_t0_hblack10",
        "f6_s4_b0_t0_hblack13","f6_s4_b0_t0_hblack14","f6_s4_b0_t0_hblack3","f6_s4_b0_t0_hblack7",
        "f6_s4_b2_t3_hblack1","f6_s4_b2_t3_hblack10","f6_s4_b2_t3_hblack3","f6_s4_b2_t3_hblack5",
        "f6_s4_b2_t3_hblack6","f6_s5_b0_t0_hblack12","f6_s5_b0_t0_hblack14","f6_s5_b0_t0_hblack3",
        "f6_s5_b0_t0_hblack5","f6_s5_b0_t0_hblack6","f6_s5_b0_t0_hblack8","f6_s5_b2_t4_hblack1",
        "f6_s5_b2_t4_hblack3","f6_s5_b2_t4_hblack5","f6_s5_b2_t4_hnohair","f7_s1_b0_t0_hblack1",
        "f7_s1_b0_t0_hblack10","f7_s1_b0_t0_hblack13","f7_s1_b0_t0_hblack3","f7_s1_b0_t0_hblack4",
        "f7_s1_b0_t0_hblack5","f7_s1_b0_t0_hblack6","f7_s1_b0_t0_hblack8","f7_s1_b0_t0_hblack9",
        "f7_s1_b0_t0_hblonde2","f7_s1_b0_t0_hblonde3","f7_s1_b0_t0_hblonde4","f7_s1_b0_t0_hred3",
        "f7_s1_b0_t0_hred4","f7_s1_b0_t0_hred5","f7_s1_b2_t5_hblack1","f7_s1_b2_t5_hblack10",
        "f7_s1_b2_t5_hblack11","f7_s1_b2_t5_hblack13","f7_s1_b2_t5_hblack3","f7_s1_b2_t5_hblack4",
        "f7_s1_b2_t5_hblack5","f7_s1_b2_t5_hblack7","f7_s1_b2_t5_hblack8","f7_s1_b2_t5_hblack9",
        "f7_s1_b2_t5_hblonde2","f7_s1_b2_t5_hnohair","f7_s1_b2_t5_hred5","f7_s3_b0_t0_hblack3",
        "f7_s3_b0_t0_hblack5","f7_s3_b0_t0_hblack8","f7_s3_b0_t0_hblonde2","f7_s3_b0_t0_hblonde4",
        "f7_s3_b2_t6_hblack1","f7_s3_b2_t6_hblack10","f7_s3_b2_t6_hblack15","f7_s3_b2_t6_hblack3",
        "f7_s3_b2_t6_hblack6","f7_s3_b2_t6_hblack9","f7_s3_b2_t6_hblonde2","f7_s3_b2_t6_hnohair",
        "f7_s3_b2_t6_hred3","f7_s4_b0_t0_hblack4","f7_s4_b0_t0_hblack5","f7_s4_b2_t7_hblack3",
        "f7_s4_b2_t7_hblack5","f7_s5_b0_t0_hblack1","f7_s5_b0_t0_hblack13","f7_s5_b0_t0_hblack3",
        "f7_s5_b0_t0_hblack5","f7_s5_b0_t0_hblack8","f7_s5_b0_t0_hnohair","f7_s5_b2_t8_hblack10",
        "f7_s5_b2_t8_hblack13","f7_s5_b2_t8_hblack3","f7_s5_b2_t8_hblack5","f7_s5_b2_t8_hnohair",
        "f8_s1_b0_t0_hblack1","f8_s1_b0_t0_hblack10","f8_s1_b0_t0_hblack11","f8_s1_b0_t0_hblack2",
        "f8_s1_b0_t0_hblack3","f8_s1_b0_t0_hblack5","f8_s1_b0_t0_hblack6","f8_s1_b0_t0_hblack8",
        "f8_s1_b0_t0_hblonde2","f8_s1_b0_t0_hblonde3","f8_s1_b0_t0_hred4","f8_s1_b2_t9_hblack1",
        "f8_s1_b2_t9_hblack10","f8_s1_b2_t9_hblack13","f8_s1_b2_t9_hblack2","f8_s1_b2_t9_hblack3",
        "f8_s1_b2_t9_hblack5","f8_s1_b2_t9_hblack6","f8_s1_b2_t9_hblack7","f8_s1_b2_t9_hblack8",
        "f8_s1_b2_t9_hblack9","f8_s1_b2_t9_hblonde2","f8_s1_b2_t9_hnohair","f8_s3_b0_t0_hblack10",
        "f8_s3_b0_t0_hblack11","f8_s3_b0_t0_hblack3","f8_s3_b0_t0_hblack5","f8_s3_b0_t0_hblack8",
        "f8_s3_b0_t0_hblonde2","f8_s3_b0_t0_hblonde4","f8_s3_b0_t0_hblonde5","f8_s3_b2_t10_hblack5",
        "f8_s4_b0_t0_hblack3","f8_s4_b0_t0_hblack4","f8_s4_b0_t0_hblack8","f8_s4_b2_t4_hblack1",
        "f8_s4_b2_t4_hblack11","f8_s4_b2_t4_hblack14","f8_s4_b2_t4_hblack3","f8_s4_b2_t4_hblack5",
        "f8_s4_b2_t4_hblack8","f8_s5_b0_t0_hblack10","f8_s5_b0_t0_hblack5","f8_s5_b0_t0_hnohair",
        "f8_s5_b2_t5_hblack1","f8_s5_b2_t5_hblack15","f8_s5_b2_t5_hblack5","f8_s5_b2_t5_hblack7",
        "f9_s1_b0_t0_hblack1","f9_s1_b0_t0_hblack10","f9_s1_b0_t0_hblack14","f9_s1_b0_t0_hblack3",
        "f9_s1_b0_t0_hblack5","f9_s1_b0_t0_hblack7","f9_s1_b0_t0_hblack8","f9_s1_b0_t0_hblonde1",
        "f9_s1_b0_t0_hblonde3","f9_s1_b0_t0_hblonde4","f9_s1_b0_t0_hnohair","f9_s1_b0_t0_hred3",
        "f9_s1_b0_t0_hred4","f9_s1_b2_t6_hblack1","f9_s1_b2_t6_hblack10","f9_s1_b2_t6_hblack11",
        "f9_s1_b2_t6_hblack2","f9_s1_b2_t6_hblack3","f9_s1_b2_t6_hblack4","f9_s1_b2_t6_hblack5",
        "f9_s1_b2_t6_hblack6","f9_s1_b2_t6_hblack7","f9_s1_b2_t6_hblack8","f9_s1_b2_t6_hblack9",
        "f9_s1_b2_t6_hblonde4","f9_s1_b2_t6_hnohair","f9_s1_b2_t6_hred1","f9_s1_b2_t6_hred3",
        "f9_s3_b0_t0_hblack3","f9_s3_b0_t0_hblack4","f9_s3_b0_t0_hblack5","f9_s3_b0_t0_hblack6",
        "f9_s3_b0_t0_hblack7","f9_s3_b0_t0_hblack8","f9_s3_b0_t0_hblack9","f9_s3_b0_t0_hblonde2",
        "f9_s3_b0_t0_hblonde3","f9_s3_b2_t7_hblack10","f9_s3_b2_t7_hblack5","f9_s3_b2_t7_hblonde2",
        "f9_s3_b2_t7_hblonde3","f9_s4_b0_t0_hblack14","f9_s4_b0_t0_hblack5","f9_s4_b0_t0_hblack9",
        "f9_s4_b2_t8_hblack3","f9_s4_b2_t8_hblack4","f9_s4_b2_t8_hblack5","f9_s4_b2_t8_hblack6",
        "f9_s4_b2_t8_hblack9","f9_s4_b2_t8_hblonde1","f9_s5_b0_t0_hblack10","f9_s5_b0_t0_hblack13",
        "f9_s5_b0_t0_hblack14","f9_s5_b0_t0_hblack3","f9_s5_b2_t9_hblack1","f9_s5_b2_t9_hblack10",
        "f9_s5_b2_t9_hblack13","f9_s5_b2_t9_hblack3","f9_s5_b2_t9_hblack5","f9_s5_b2_t9_hblack8",
        "f9_s5_b2_t9_hblonde2","f9_s5_b2_t9_hnohair" };

    public static string[] shirtsFileNames = {
        "blackwithwhiteshirt", "redblueshirt1", "redwitwhiteshirt", "bluewhireshirt", "whiteredstripeswithredshirt", 
        "bludarkbluestripesshirt", "darkredgreenstripesshirt", "bluedarkbluearmshirt", "whitegreenstripesshirt1", 
        "whiteblackshirt1", "darkredshirt", "whitebluestripeshirt", "tshirt_white_black", "blackbluestripesshirt",
        "redwhitearmshirt", "whitebluestripesshirt", "bluetricolourstripesshirt", "whiteshirt", "darkredwithyellowshirt", 
        "whiteredshirt", "blueredshirt", "yellowbluearmshirt", "darkbluewithblacshirt", "redshirt", 
        "whitewithredstripeshirt", "whiteblackstripesshirt", "blackredstripesshirt2", "redblackstripesshirt", 
        "blueshirt1", "redwhiteblackshirt", "yellowwithgreenshirt", "whitedarkbluestripesshirt", 
        "redblackstripeshirt", "redwithblackshirt", "bluedarkblueshirt", "blackredstripeshorshirt", 
        "whitegreenstripeshirt", "orangeblackstripesshirt", "whitewithblackshirt", "westhamshirt",
        "redwhitearmshirt", "redbluestripesshirt", "whitewithblueshirt", "lightblueshirt", "greenshirt1", 
        "yellowblackshirt", "blacwhiteshirt", "darkgreengoldstripesshirt", "greenwhiteshirt", "whiteredshirt1", 
        "redyellowshirt", "redwhitestripesshirt1", "bluewhitestripesshirt1", "whitewithgreenshirt1",
        "bluewithyellowshirt", "blackbluestripesshirt", "yellowwithstripesshirt", "yellowbluestripesshirt", 
        "whitegreenstripeshorshirt", "whiteblestripevertshirt", "whitebluestripesshirt", "whiteredshirt2", 
        "blueblackstripesshirt", "blackredshirt", "redblueshirt", "blueshirt", "whiteredblackstripesshirt", 
        "lightblueshirt", "greydarkgreystripesshirt", "lightbluewithwhiteshirt", "whitegreyshirt", "shirt", 
        "redwhiteshirt", "blackbluestripesshirt", "redwithwhiteshirt", "whitepurpleshirt", "greenwithwhiteshirt", 
        "blestripesshirt1", "whiteblackshirt", "yellowtshirt", "yellowbluestripeshirt", "bluewithwhiteshirt", 
        "darkblueyellowstripeshirt", "whitewithgreenstripeshirt", "blackredstripesshirt", "greenshirt", 
        "bluredshirt", "darkgreendblackstripesshirt", "redbluearmshirt", "whitewithredshirt", "whiteredstripeshirt", 
        "blackredstripesshirt1", "whiteblackstripeshorshirt", "whitegreenstripesshirt", "darkbluedarkbluestripesshirt",
        "greydarkgreystripesshirt", "bluewhitestripesshirt1", "lightbluewhitestripesshirt", "yellowblackstrpesshirt1", 
        "tshirt_blue_whitestripes", "tshirt_white_green", "purplewithwhiteshirt", "yellowshirt1", 
        "whiteblueshirt", "tshirt_red", "yellowblackstripesshirt", "orangeblackshirt", "croatiashirt",
        "blackredstripesshirt3", "whitewithgreenshirt", "yellowbluestripesshirt1", "bluwithblueshirt"
    };

    public static string[] shortsFileNames = {
        "whitegoldstripesshort", "darkredshort", "yellowredstripesshort","darkblueshort", "redblackstripesshort",
        "darggreengoldstripesshort", "blackbluestripesshort", "darkblueshort1", "whitebluestripeshort", "lightblueshort", 
        "whiteredstripehorshort", "whiteshort", "darkblueyellowstripeshort", "blackwhitestripesshort",
        "darkredshort", "darggreengoldstripesshort", "shorts_blue_white", "redwhitestripesshort", "whitewithgreenshort", 
        "darkblueshort11", "whiteblackstripeshort", "redshort", "whitegoldstripesshort", "whiteredstripesshort", 
        "blackorangestripesshort", "whitepurpleshort", "greenwhitestripesshort", "greenshorts", "yellowbluestripesshort",
        "blackyellowshort", "blueshort", "blackbluestripesshort", "blueredstripehorshort", "yellowshort", 
        "whitepurplestripesshort", "blacredstripeshort", "darkblueyellowstripeshort", "oragngeshort", "darkredshort", 
        "blueyellowstripesshort", "blackwhitestripeshorwertshort", "bluebluestripesshort", "whiteredstripeshort",
        "whitegreenstripesshort", "redyellowstripeshort", "shorts_blue_white", "shorts_red_whitestripes", 
        "greenwhitestripesshort", "shorts_blue_whitestripes"
    };

    public static string[] sockFileNames =
    {
        "sock_white", "whitegreysocks", "redwhitestripesocks1", "sock_001_21", "greenwhitesocks", "sock_004_06", "sock_003_34", "sock_008_06", 
        "sock_002_30", "sock_003_41", "lightbluewhirestripessocks", 
        "blackwhitestripeupsocks", "sock_002_09", "sock_002_36", "sock_005_13", "sock_008_10", "sock_003_27", 
        "whiteblackstripedownsocks", "blacwhitestripessocks", "whiteredstripesverticalsocks", "sock_000_07", 
        "sock_004_04", "sock_002_20", "sock_001_08", "sock_005_00", "sock_darkgreen", "sock_005_12", 
        "blackredstripesocks", "sock_red", "sock_008_02", "sock_003_33", "whitegoldsocks", "sock_004_05", 
        "sock_003_42", "bluewhitestripesverticalsocks", "sock_002_24", "sock_orange", "redbluestripessocks", 
        "sock_004_11", "sock_002_12", "sock_003_32", "sock_004_13", "sock_004_19", "sock_001_10", "sock_008_08", 
        "sock_003_19", "sock_005_14", "sock_008_00", "sock_006_09", "sock_001_16", "darkredgreystripesocks", 
        "sock_003_16", "sock_007_01", "sock_003_23", "sock_001_14", "redblackstripessocks", "sock_003_09", 
        "sock_008_04", "sock_002_28", "whiteredstripesocks1", "sock_003_21", "sock_008_07", "sock_003_38", 
        "sock_007_03", "bluebluestripesocks", "sock_002_19", "sock_001_05", "sock", "sock_003_39", "sock_006_08", 
        "sock_003_12", "purplewhitesocks", "sock_005_02", "sock_002_25", "bluewhitestripedownsocks", "sock_003_29",
        "whiteblacstripessocks", "sock_002_29", "sock_003_28", "orangesocks", "sock_004_14", "sock_002_32", 
        "darkbluesocks1", "whitedarkgreenstripeupsocks", "sock_005_11", "sock_006_10", "redwhitestripesocks", 
        "blueyellowstripesocks", "sock_008_05", "sock_005_04", "sock_003_35", "whiteredbluestripessocks", "sock_002_18",
        "blackredstripessocks1", "sock_002_31", "sock_005_15", "lightblueredstripesocks", "sock_000_15", 
        "sock_002_11", "sock_002_38", "westhamsocks", "sock_004_18", "sock_000_16", "sock_002_34", "sock_000_06",
        "blueredstripedownsocks", "sock_002_07", "sock_002_21", "darkgreengoldstripessocks", "sock_001_19", 
        "sock_000_17", "bluewhitestripesocks", "whitebluesocks", "sock_002_05", "redblackstripesocks1", "sock_003_24", 
        "blackwhitestripesverticalsocks", "sock_008_12", "sock_001_07", "sock_004_00", "sock_blue", 
        "blueblackstripesocks", "sock_002_03", "sock_008_09", "yellowsocks", "sock_003_31", "sock_003_40",
        "sock_003_20", "sock_000_18", "sock_005_03", "yellowbluestripecsocks", "sock_002_39", "sock_007_04", 
        "sock_006_07", "sock_003_22", "sock_002_41", "redwhitesocks", "sock_003_30", "blackbluestripessocks",
        "sock_000_14", "sock_black", "blacredstripessocks", "greenwhitestripessocks", "sock_002_13", "sock_002_27", 
        "sock_004_17", "sock_004_02", "sock_005_17", "sock_003_03", "sock_005_08", "sock_008_03", "sock_002_26", 
        "sock_darkred", "sock_001_15", "sock_002_23", "sock_000_03", "sock_005_07", "sock_002_15", 
        "redyellowstripessocks", "sock_004_01", "whitebluestripesocks", "sock_004_09", "blacksocks", "sock_006_04",
        "sock_000_12", "sock_001_13", "sock_002_06", "sock_002_17", "whiteblackstripeupsocks", "sock_003_00", 
        "sock_002_10", "blueblackstripeupsocks", "sock_002_40", "sock_002_02", "whitelightbluestripesocks",
        "sock_002_01", "socks", "sock_004_20", "whitegreenstripedownsocks", "sock_003_25", "whitegreensocks",
        "darkbluewhitestripesocks", "sock_002_00", "sock_003_05", "whiteblueblackstripessocks", "sock_007_00",
        "sock_003_07", "blackredstripessocks", "sock_003_13", "sock_darkblue", "sock_003_26", "sock_001_06", 
        "sock_005_06", "sock_001_17", "sock_008_11", "whiteredstripessocks1", "sock_005_16", "sock_yellow", 
        "sock_004_21", "sock_006_02", "sock_002_08", "sock_002_33", "sock_001_04", "yellowblackstripessocks",
        "sock_004_08", "darkbluewhitestripessocks", "sock_007_02", "greenwhitestripesocks", 
        "whiteblustripesocks", "sock_001_01", "sock_003_11", "sock_001_12", "sock_005_01", "blubluestripesocks",
        "blackwhitestripessocks", "yellowgreenstripesocks", "sock_003_02", "sock_003_43", "greensocks", 
        "darkblueyellowstripessocks", "sock_000_01", "liverpoolsocks", "sock_006_01", "sock_006_05", "sock_004_16",
        "sock_002_14", "sock_008_13", "orangeblackstripesocks", "sock_002_16", "whiteredsocks", "sock_white_black"
    };

    void Update()
    {    
        updateFlagButtons();
    }

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        strCommon = new StringCommon();
        graphics = new GraphicsCommon();
        logoFilePath = 
            Application.persistentDataPath + "/logoFile.png";
        logoBackgroundFilePath = "others/logoFileBackground";
           
        shopNotification.SetActive(false);
        updateDiamondsNumber();
        restoreCurrentClothIdx();

        //print("Current sock idx " + currentSockIdx);
        restoreCurrentCloth();
        deactivateAllPrevNextButtons();
        initPlayerCardImage(skinsFileNames[currentSkinIdx]);
        initTeamLogo();
        updatePrice(shirtsFileNames[currentShirtIdx]);
        disableFocusApartFrom(0);
        shirtPrevNextButtons.SetActive(true);
        nationalityChoosePanel.SetActive(false);
        buyItemPanel.SetActive(false);
        fansColorPanel.SetActive(false);
        leaguesAddedInfoPanel.SetActive(false);
        oopsPanel.SetActive(false);
        yesNoPanel.SetActive(false);
        initDefaultNationalityFlag();
        setFlagsColor(fansFlagColors[flagsColorIdx]);
        setFansColor(fansColor[fansColorIdx]);

        updateInputText();
        currentItemName = shirtsFileNames[currentShirtIdx];

        if (PlayerPrefs.HasKey("numOfCustomizeOpen"))
        {
            isCustomizeOpenFirstTime = false;
        }
        else
        {
            treasuePanel.SetActive(false);
            isCustomizeOpenFirstTime = true;
            oopsPanelHeaderText.text = Languages.getTranslate("CUSTOMIZE");
            oopsPanelInfoText.text =
                Languages.getTranslate(
                    "Welcome to our football game! We hope you will enjoy it. You can create your own player card and team in this menu");
            oopsPanel.SetActive(true);

            Debug.Log("oopsPanel");

            PlayerPrefs.SetInt("numOfCustomizeOpen", 1);
            PlayerPrefs.Save();
        }

        //print("#DBGPATHDATA " + Application.persistentDataPath);
    }

    //used by external scripts
    public void onClickExitShopPanel()
    {
    //    int selectedPlrIdx = 
    //        initTeamPlayersCard(selectedPlayerName, saveFileName);

       /// dimPlayerSelectionApartFrom(selectedPlrIdx);

        shopPanel.SetActive(false);
    }

    private void initPlayerCardImage(string fileName)
    {     
        playerCardImage.texture =
            Resources.Load<Texture2D>(
            "playersCard/" + fileName);
    }

    public void initTeamLogo()
    {
        if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
            logoImage.texture = 
                graphics.loadTexture(logoFilePath);
        else
            logoImage.texture = Resources.Load<Texture2D>(
                "others/logoFile");

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
    }

    private void clearFlagButtonsVars(bool teamA, bool teamB)
    {
        if (teamA)
        {
            teamAFlagPrevActive = 0;
            teamAFlagPrevLastTimeChanged = 0f;
            teamAFlagNextActive = 0;
            teamAFlagNextLastTimeChanged = 0f;
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

    public void teamAFlagPrev()
    {
        if (currATeamIdx > 0)
        {
            currATeamIdx--;
        }
        else
        {
            if (currATeamIdx == 0)
            {
                currATeamIdx = flagsFileName.Length - 1;
            }
        }

        string countryName = flagsFileName[currATeamIdx];
        teamNationalityName.text = countryName.ToUpper();
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        nationalityMainFlag.texture = Resources.Load<Texture2D>("Flags/national/" + countryName);
    }

    public void teamAFlagNext()
    {
        //if ((currATeamIdx + 1) < nationalTeams.getMaxTeams())
        if ((currATeamIdx + 1) < flagsFileName.Length)
        {
            currATeamIdx++;
        }
        else
        {
            //if (currATeamIdx == (nationalTeams.getMaxTeams() - 1))
            if (currATeamIdx == (flagsFileName.Length - 1))
            {
                currATeamIdx = 0;
            }
        }

        string countryName = flagsFileName[currATeamIdx];
        teamNationalityName.text = countryName.ToUpper();
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        nationalityMainFlag.texture = Resources.Load<Texture2D>("Flags/national/" + countryName);
    }

    public void saveNationalityFlag(string countryName)
    {
        teamNationalityName.text = countryName.ToUpper();
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();

        nationalityMainFlag.texture =
            Resources.Load<Texture2D>("Flags/national/" + countryName);
        playerCardFlag.texture = 
            Resources.Load<Texture2D>("Flags/national/" + countryName);


        saveCurrentClothIdx();
        //print("#DBG country " + ("Flags/national/" + countryName));
    }

    public void initDefaultNationalityFlag()
    {
        string countryName = flagsFileName[currATeamIdx];
        teamNationalityName.text = countryName.ToUpper();

        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        nationalityMainFlag.texture = 
            Resources.Load<Texture2D>("Flags/national/" + countryName);
        playerCardFlag.texture =
            Resources.Load<Texture2D>("Flags/national/" + countryName);
        //print("####CountryName init flag " + ("Flags/national/" + countryName));
    }
    
    public void onClickShirt()
    {
        //print("onClick shirt");
        updatePrice(shirtsFileNames[currentShirtIdx]);

        disableFocusApartFrom(0);
        deactivateAllPrevNextButtons();
        shirtPrevNextButtons.SetActive(true);
        currentItemName = shirtsFileNames[currentShirtIdx];
    }

    public void onClickShirtNext()
    {
        int maxIdx = shirtsFileNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        //print("#DBG shirtsFileNames " + shirtsFileNames[currentShirtIdx]);

        if (currentShirtIdx < (maxIdx - 1))
        {
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/" + shirtsFileNames[++currentShirtIdx],
                  1);
            graphics.setMaterialByName(
               mainModel3D,
               "playerMaterials/" + shirtsFileNames[currentShirtIdx],
               2);
            graphics.setMaterialByName(
               mainModel3D,
               "playerMaterials/" + shirtsFileNames[currentShirtIdx],
               3);
            updatePrice(shirtsFileNames[currentShirtIdx]);
            currentItemName = shirtsFileNames[currentShirtIdx];
        }
    }

    public void onClickShirtPrev()
    {
        if (currentShirtIdx > 0)
        {
            graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shirtsFileNames[--currentShirtIdx],
                1);
            graphics.setMaterialByName(
               mainModel3D,
               "playerMaterials/" + shirtsFileNames[currentShirtIdx],
               2);
            graphics.setMaterialByName(
               mainModel3D,
               "playerMaterials/" + shirtsFileNames[currentShirtIdx],
               3);
            updatePrice(shirtsFileNames[currentShirtIdx]);
            currentItemName = shirtsFileNames[currentShirtIdx];
        }
    }

    public void onClickShorts()
    {
        updatePrice(shortsFileNames[currentShortsIdx]);


        disableFocusApartFrom(1);
        deactivateAllPrevNextButtons();
        shortsPrevNextButtons.SetActive(true);
        currentItemName = shortsFileNames[currentShortsIdx];
    }

    public void onClickShortsPrev()
    {
        if (currentShortsIdx > 0)
        {
            graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shortsFileNames[--currentShortsIdx],
                5);
            updatePrice(shortsFileNames[currentShortsIdx]);
            currentItemName = shortsFileNames[currentShortsIdx];
        }
    }

    public void onClickShortsNext()
    {
        int maxIdx = shortsFileNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        if (currentShortsIdx < (maxIdx - 1))
        {
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/" + shortsFileNames[++currentShortsIdx],
                  5);
            updatePrice(shortsFileNames[currentShortsIdx]);
            currentItemName = shortsFileNames[currentShortsIdx];
        }
    }

    public void onClickSock()
    {
        updatePrice(sockFileNames[currentSockIdx]);

        disableFocusApartFrom(2);
        deactivateAllPrevNextButtons();
        sockPrevNextButtons.SetActive(true);
        currentItemName = sockFileNames[currentSockIdx];
    }

    public void onClickSockPrev()
    {
        if (currentSockIdx > 0)
        {
            graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + sockFileNames[--currentSockIdx],
                7);
            updatePrice(sockFileNames[currentSockIdx]);
            currentItemName = sockFileNames[currentSockIdx];
        }
    }

    public void onClickSockNext()
    {
        int maxIdx = sockFileNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        if (currentSockIdx < (maxIdx - 1))
        {
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/" + sockFileNames[++currentSockIdx],
                  7);
            updatePrice(sockFileNames[currentSockIdx]);
            currentItemName = sockFileNames[currentSockIdx];
        }
    }

    public void onClickShoe()
    {
        updatePrice(shoesFilesNames[currentShoeIdx]);
        disableFocusApartFrom(6);
        deactivateAllPrevNextButtons();
        shoePrevNextButtons.SetActive(true);
        currentItemName = shoesFilesNames[currentShoeIdx];
    }

    public void onClickShoePrev()
    {
        if (currentShoeIdx > 0)
        {
            graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/shoe/" + shoesFilesNames[--currentShoeIdx],
                6);
            updatePrice(shoesFilesNames[currentShoeIdx]);
            currentItemName = shoesFilesNames[currentShoeIdx];
        }
    }

    public void onClickShoeNext()
    {
        int maxIdx = shoesFilesNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        if (currentShoeIdx < (maxIdx - 1))
        {
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/shoe/" + shoesFilesNames[++currentShoeIdx],
                  6);
            updatePrice(shoesFilesNames[currentShoeIdx]);
            currentItemName = shoesFilesNames[currentShoeIdx];
        }
    }

    public void onClickSkin()
    {
        updatePrice(skinsFileNames[currentSkinIdx]);
        disableFocusApartFrom(5);
        deactivateAllPrevNextButtons();
        skinPrevNextButtons.SetActive(true);
        currentItemName = skinsFileNames[currentSkinIdx];
    }

    public void onClickSkinPrev()
    {
        if (currentSkinIdx > 0)
        {
            currentSkinIdx--;
            int delimeterIndex =
                  strCommon.getIndexOfNOccurence(skinsFileNames[currentSkinIdx], '_', 4);

            string skinFileName = "skin_" +
                            Globals.getSkinHairColorName(
                                skinsFileNames[currentSkinIdx].Substring(0, delimeterIndex));
            string hairColorName =
                             skinsFileNames[currentSkinIdx].Substring(delimeterIndex + 1);

            //print("#DBGSKIN " + skinFileName + " hairColorName " + hairColorName);
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/skins/" + skinFileName,
                  0);
            graphics.setPlayerTexturesHair(
               mainModelHair3D,
               hairColorName);
            updatePrice(skinsFileNames[currentSkinIdx]);
            initPlayerCardImage(skinsFileNames[currentSkinIdx]);
            currentItemName = skinsFileNames[currentSkinIdx];
        }
    }

    public void onClickSkinNext()
    {
        int maxIdx = skinsFileNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        if (currentSkinIdx < (maxIdx - 1))
        {
            currentSkinIdx++;
            int delimeterIndex =
                  strCommon.getIndexOfNOccurence(skinsFileNames[currentSkinIdx], '_', 4);

            string skinFileName = "skin_" +
                            Globals.getSkinHairColorName(
                                skinsFileNames[currentSkinIdx].Substring(0, delimeterIndex));
            string hairColorName =
                             skinsFileNames[currentSkinIdx].Substring(delimeterIndex + 1);

            //print("#DBGSKIN " + skinFileName + " hairColorName " + hairColorName);

            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/skins/" + skinFileName,
                  0); 
            graphics.setPlayerTexturesHair(
               mainModelHair3D,
               hairColorName);
            updatePrice(skinsFileNames[currentSkinIdx]);
            initPlayerCardImage(skinsFileNames[currentSkinIdx]);
            currentItemName = skinsFileNames[currentSkinIdx];
        }
    }

    public void onClickFansFlagColorPanel()
    {
        disableFocusApartFrom(4);
        deactivateAllPrevNextButtons();
        fansColorPanel.SetActive(true);
    }

    public void onClickFlagsColorPrev()
    {
        if (flagsColorIdx > 0)
        {
            setFlagsColor(fansFlagColors[--flagsColorIdx]);
        }
    }

    public void onClickFlagsColorNext()
    {
        if (flagsColorIdx < (fansFlagColors.Length - 1)) {            
            setFlagsColor(fansFlagColors[++flagsColorIdx]);
        }       
    }

    public void onClickFansColorPrev()
    {
        if (fansColorIdx > 0)
        {
            setFansColor(fansColor[--fansColorIdx]);
        }
    }

    public void onClickFansColorNext()
    {
        if (fansColorIdx < (fansColor.Length - 1))
        {
            setFansColor(fansColor[++fansColorIdx]);
        }
    }

    public void onClickGloves()
    {
        updatePrice(glovesFilesNames[currentSGlovesIdx]);
        disableFocusApartFrom(3);
        deactivateAllPrevNextButtons();
        glovesPrevNextButtons.SetActive(true);
    }

    public void onClickGlovePrev()
    {
        if (currentSGlovesIdx > 0)
        {
            graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/glove/" + glovesFilesNames[--currentSGlovesIdx],
                8);
            updatePrice(glovesFilesNames[currentSGlovesIdx]);
            currentItemName = glovesFilesNames[currentSGlovesIdx];
        }
    }

    public void onClickGloveNext()
    {
        int maxIdx = glovesFilesNames.Length;
        if (isCustomizeOpenFirstTime)
            maxIdx = 3;

        if (currentSGlovesIdx < (maxIdx - 1))
        {
            graphics.setMaterialByName(
                  mainModel3D,
                  "playerMaterials/glove/" + glovesFilesNames[++currentSGlovesIdx],
                  8);
            updatePrice(glovesFilesNames[currentSGlovesIdx]);
            currentItemName = glovesFilesNames[currentSGlovesIdx];
        }
    }

    public void deactivateAllPrevNextButtons()
    {
        glovesPrevNextButtons.SetActive(false);
        skinPrevNextButtons.SetActive(false);
        shoePrevNextButtons.SetActive(false);
        shortsPrevNextButtons.SetActive(false);
        shirtPrevNextButtons.SetActive(false);
        sockPrevNextButtons.SetActive(false);
    }

    public void onClickSave()
    {
        PlayerPrefs.SetInt("customizeTeamActive", 1);
        PlayerPrefs.Save();
    }

    public void onClickTeamNameChanged()
    {
        string inputText = inputTeamName.text;
        oopsPanelHeaderText.text = "Oops..";

        if (string.IsNullOrEmpty(inputText))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text = 
                Languages.getTranslate("Team name cannot be empty");
            return;
        }

        if (!Regex.IsMatch(inputText, "^[a-zA-Z ]*$"))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text =
              Languages.getTranslate("Sorry. Only English characters are allowed");
            return;
        }

        if (Globals.checkIfTeamExitsByName(inputText))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text = 
                Languages.getTranslate("The team with such name already exists");
            return;
        }

        string prevTeamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");
        List<string> savesList = Globals.getNamesSavesToDelete(prevTeamName);
        if (savesList.Count != 0)
        {
            string savesNameStr = "";
            for (int i = 0; i < savesList.Count; i++)
            {

                savesNameStr += savesList[i] + "\n";
                if (i > 7) {
                    savesNameStr += "....\n";
                    break;
                }
            }

            yesNoPanel.SetActive(true);
            yesNoPanelMainText.text =
                Languages.getTranslate("Following saves will be deleted") + "\n" + savesNameStr;

            return;
        } 

        //in the case we change the team name
        if (PlayerPrefs.HasKey(prevTeamName + "_lastSelectedPlayer"))
        {
            string selectedPlayer = 
                PlayerPrefs.GetString(prevTeamName + "_lastSelectedPlayer");
            PlayerPrefs.SetString(inputText + "_lastSelectedPlayer", selectedPlayer);
            PlayerPrefs.DeleteKey(prevTeamName + "_lastSelectedPlayer");
        }

        //copy players from our team to the team with the new name
        if (PlayerPrefs.HasKey(prevTeamName + "_teamPlayers"))
        {
            string teamPlayers =
                PlayerPrefs.GetString(prevTeamName + "_teamPlayers");
            PlayerPrefs.SetString(inputText + "_teamPlayers", teamPlayers);
            PlayerPrefs.DeleteKey(prevTeamName + "_teamPlayers");
        }

        PlayerPrefs.SetString("CustomizeTeam_TeamName", inputText);
        PlayerPrefs.Save();

        Globals.customizeTeamName = inputText;
    }
  
    public void onClickChangeTeamNameDeleteSavesYes()
    {
        string prevTeamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");
        Globals.deleteAllTeamSaves(Globals.customizeTeamName);

        string inputText = inputTeamName.text;

        PlayerPrefs.SetString("CustomizeTeam_TeamName", inputText);
        PlayerPrefs.Save();

        Globals.customizeTeamName = inputText;
        yesNoPanel.SetActive(false);
    }

    public void onClickChangeTeamNameDeleteSavesNo()
    {
        yesNoPanel.SetActive(false);
        inputTeamName.text = Globals.customizeTeamName;
    }

    public void onClickCloseOnlyEnglishCharactersInfoPanel()
    {
        oopsPanel.SetActive(false);
    }

    public void onClickPlayerNameChanged()
    {
        string inputText = inputPlayerName.text;
        string teamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");

        oopsPanelHeaderText.text = "Oops..";

        if (string.IsNullOrEmpty(inputText))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text = 
                Languages.getTranslate("Player name cannot be empty");
            return;
        }

        if (!Regex.IsMatch(inputText, "^[a-zA-Z ]*$"))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text =                                
               Languages.getTranslate("Sorry. Only English characters are allowed");
            return;
        }

        string teamPlayers =
                PlayerPrefs.GetString(teamName + "_teamPlayers");
        print("TEamPlayers " + teamPlayers);
        if (!string.IsNullOrEmpty(teamPlayers))
        {
            string prevPlayerName =
                PlayerPrefs.GetString("CustomizeTeam_PlayerName");
            teamPlayers = Regex.Replace(teamPlayers,
                                        prevPlayerName,
                                        inputText);
            PlayerPrefs.SetString(teamName + "_teamPlayers", teamPlayers);
        }

        PlayerPrefs.SetString("CustomizeTeam_PlayerName", inputText);
        PlayerPrefs.Save();

        Globals.customizePlayerName = inputText;
        playerCardName.text = inputText;
    }
   
    private void disableFocusApartFrom(int idx)
    {
        for (int i = 0; i < MAIN_MENU_MAX_BUTTONS; i++)
        {
            if (i == idx)
            {
                mainButtonsFocus[i].SetActive(true);
                continue;
            }
            mainButtonsFocus[i].SetActive(false);
        }
    }

    public void onClickExitInfo()
    {
        //Debug.Log("#DBG103 tem Name " + Globals.customizeTeamName);
        if (Globals.customizeTeamName.Equals("Your team"))
        {
            //Debug.Log("#DBG103 tem Name onClick exit#DBG1234 entered");
            onClickExitCustomize();
        }
        else
        {
            leaguesAddedInfoPanel.SetActive(true);
            bodyTextLeaguesAddedPanelInfo.text =
                Languages.getTranslate(
                "Team " + Globals.customizeTeamName +
                " has been added to the following leageus:\n" +
                "Brazil\n" +
                "England\n" +
                "Italy\n" +
                "Germany\n" +
                "Spain\n" +
                "Champ Cup\n",
                new List<string>() { Globals.customizeTeamName});
        }
    }

    public void onClickCloseLeaguesAddedInfoPanel()
    {
        leaguesAddedInfoPanel.SetActive(false);
    }

    public void onClickExitCustomize()
    {
        SceneManager.LoadScene("menu");
    }

    public void updatePrice(string itemName)
    {
        if (PlayerPrefs.HasKey("CustomizeTeamItem_" + itemName))
        {
            priceText.text = "FREE"; 
        }
        else
        {
            priceText.text = 
                Languages.getTranslate("2000 diamonds");
        }
    }
      
    private void saveCurrentClothIdx()
    {
        PlayerPrefs.SetInt("CustomizeTeam_currentShirtIdx", currentShirtIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentShortsIdx", currentShortsIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentSockIdx", currentSockIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentShoeIdx", currentShoeIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentSGlovesIdx", currentSGlovesIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentSHairIdx", currentSHairIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currentSkinIdx", currentSkinIdx);
        PlayerPrefs.SetInt("CustomizeTeam_currATeamIdx", currATeamIdx);
        PlayerPrefs.SetInt("CustomizeTeam_fansFlagColorIdx", flagsColorIdx);
        PlayerPrefs.SetInt("CustomizeTeam_fansColorIdx", fansColorIdx);
        PlayerPrefs.Save();

        //print("#DBG currATeamIdx save " + currATeamIdx);
    }

    private void restoreCurrentClothIdx()
    {
        if (PlayerPrefs.HasKey("CustomizeTeam_currentShirtIdx"))
            currentShirtIdx = PlayerPrefs.GetInt("CustomizeTeam_currentShirtIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentShortsIdx"))
           currentShortsIdx = PlayerPrefs.GetInt("CustomizeTeam_currentShortsIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentSockIdx"))
            currentSockIdx = PlayerPrefs.GetInt("CustomizeTeam_currentSockIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentShoeIdx"))
            currentShoeIdx = PlayerPrefs.GetInt("CustomizeTeam_currentShoeIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentSGlovesIdx"))
            currentSGlovesIdx = PlayerPrefs.GetInt("CustomizeTeam_currentSGlovesIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentSHairIdx"))
            currentSHairIdx = PlayerPrefs.GetInt("CustomizeTeam_currentSHairIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currentSkinIdx"))
            currentSkinIdx = PlayerPrefs.GetInt("CustomizeTeam_currentSkinIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_currATeamIdx"))
            currATeamIdx = PlayerPrefs.GetInt("CustomizeTeam_currATeamIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_fansFlagColorIdx"))
            flagsColorIdx = PlayerPrefs.GetInt("CustomizeTeam_fansFlagColorIdx");
        if (PlayerPrefs.HasKey("CustomizeTeam_fansColorIdx"))
            fansColorIdx = PlayerPrefs.GetInt("CustomizeTeam_fansColorIdx");

        //print("currATeamIdx " + currATeamIdx);
        //print("#DBG currentSockIdx restore " + currentSockIdx);
    }

    private void restoreCurrentCloth()
    {
        graphics.setMaterialByName(
                 mainModel3D,
                 "playerMaterials/shoe/" + shoesFilesNames[currentShoeIdx],
                 6);
 
        int delimeterIndex =
                  strCommon.getIndexOfNOccurence(skinsFileNames[currentSkinIdx], '_', 4);

        string skinFileName = "skin_" +
                        Globals.getSkinHairColorName(
                            skinsFileNames[currentSkinIdx].Substring(0, delimeterIndex));
        string hairColorName =
                         skinsFileNames[currentSkinIdx].Substring(delimeterIndex + 1);

        //print("#DBGSKIN " + skinFileName + " hairColorName " + hairColorName);
        graphics.setMaterialByName(
              mainModel3D,
              "playerMaterials/skins/" + skinFileName,
              0);
        graphics.setPlayerTexturesHair(
           mainModelHair3D,
           hairColorName);

        /*graphics.setMaterialByName(
          mainModel3D,
          "playerMaterials/shoe/" + hairFileNames[currentSHairIdx],
          6);*/

        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/glove/" + glovesFilesNames[currentSGlovesIdx],
                8);

        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + sockFileNames[currentSockIdx],
                7);

        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shirtsFileNames[currentShirtIdx],
                1);
        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shirtsFileNames[currentShirtIdx],
                2);
        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shirtsFileNames[currentShirtIdx],
                3);

        graphics.setMaterialByName(
                mainModel3D,
                "playerMaterials/" + shortsFileNames[currentShortsIdx],
                5);
    }

    private void updateDiamondsNumber()
    {
        currentDiamondsNumberText.text = 
            Globals.diamonds.ToString();
    }

    public void onClickCloseShopNotification()
    {
        shopNotification.SetActive(false);
    }

    public void onClickSavePlayerNationality()
    {
        string teamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");
        string playerName = PlayerPrefs.GetString("CustomizeTeam_PlayerName");
        string prevNationality = PlayerPrefs.GetString("customizePlayerNationality");

        nationalityChoosePanel.SetActive(false);
        saveNationalityFlag(flagsFileName[currATeamIdx]);

        string nationality = 
            Regex.Replace(flagsFileName[currATeamIdx], "\\s+", "").ToLower();


        string teamPlayers =
               PlayerPrefs.GetString(teamName + "_teamPlayers");
        print("TEamPlayers " + teamPlayers);
        if (!string.IsNullOrEmpty(teamPlayers))
        {
            teamPlayers = Regex.Replace(teamPlayers,
                                        playerName + ":[a-zA-Z ]+:",
                                        playerName + ":" + flagsFileName[currATeamIdx] + ":");
            PlayerPrefs.SetString(teamName + "_teamPlayers", teamPlayers);
        }

        print("TEamPlayers AFTER " + teamPlayers + " prevNationality " + prevNationality);

        PlayerPrefs.SetString("customizePlayerNationality", flagsFileName[currATeamIdx]);        
        PlayerPrefs.Save();

        Globals.customizePlayerNationality = flagsFileName[currATeamIdx];
    }

    public void onClickClosePlayerNationalityPlayer()
    {
        nationalityChoosePanel.SetActive(false);
    }

    public void onClickShowPlayerNationalityPlayer()
    {
        nationalityChoosePanel.SetActive(true);
    }

    /*public void ShowMediaPicker()
    {
        if (Application.isEditor)
        {
        }
        else
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                UploadNewProfileImage(path);
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                LocalProfileImage.sprite = 
                    Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            });
        }
    }*/

    public void onClickUploadImage()
    {
        int maxSize = 512;
        oopsPanelHeaderText.text = "Oops..";
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
        if (path != null)
        {
            Texture2D foregroundTexture =
                NativeGallery.LoadImageAtPath(path, maxSize, false, false);
            if (foregroundTexture == null)
            {
                return;
            }

            if (foregroundTexture.width > 400 ||
                foregroundTexture.height > 256) {
                oopsPanel.SetActive(true);
                oopsPanelInfoText.text =
                   Languages.getTranslate(
                    "Image can not have width bigger than 400 and height bigger than 256. " +
                    "It must be in png format");
                return;
            }

                Texture2D backgroundTexture =
                            Resources.Load<Texture2D>(
                            logoBackgroundFilePath);

                Texture2D outLogoFile =
                    graphics.combine2DTextures(backgroundTexture, foregroundTexture, 400, 256);

                graphics.saveTextureToPng(outLogoFile, logoFilePath);

                PlayerPrefs.SetInt("CustomizeTeam_logoUploaded", 1);
                PlayerPrefs.Save();
                logoImage.texture = outLogoFile;
            }
        }, "Select a PNG image", "image/png");

        //Debug.Log("Permission result: " + permission);
    }

    public void onClickSaveItem(string itemType)
    {
        //print("currentItemName " + currentItemName);
        if (!PlayerPrefs.HasKey("CustomizeTeamItem_" + currentItemName))
        {
            if (Globals.diamonds >= PRICE_PER_PRODUCT)
            {
               
                buyItemButtonText.text = 
                    Languages.getTranslate("BUY");
                buyItemNotification.text =
                    Languages.getTranslate("PRICE\n2000 diamonds");
                buyItemButton.onClick.RemoveAllListeners();
                buyItemButton.onClick.AddListener(
                                   delegate
                                   {
                                       onClickBuyButton(itemType);
                                   });
            } else
            {
                buyItemNotification.text =
                    Languages.getTranslate(
                    "You don't have enough diamonds to buy this card.\n" +
                    "Go to the shop to buy diamonds");
                buyItemButtonText.text = Languages.getTranslate("SHOP");
                //buyItemHeaderText.text = "BUY ITEM";
                buyItemButton.onClick.RemoveAllListeners();
                buyItemButton.onClick.AddListener(
                                delegate
                                {
                                    onClickBuyButton("SHOP");
                                });
            }

            buyItemPanel.SetActive(true);
        }
        else
        {
            //print("#DBG element selected");
            PlayerPrefs.SetInt("CustomizeTeamItem_" + currentItemName, 1);
            PlayerPrefs.Save();
            saveItemsGlobals(itemType);
            saveCurrentClothIdx();
            updatePrice(currentItemName);
            audioManager.Play("elementSelected");
        }
    }

    private void setFansColor(string fansColor)
    {
        fansColorImg.texture =
            Resources.Load<Texture2D>("stadium/fansColor/" + fansColor);
    }

    private void setFlagsColor(string fansFlagColor)
    {
        fansFlagColorImg.texture =
            Resources.Load<Texture2D>("stadium/wallsFlag/banner_" + fansFlagColor);
    }


    public void onClickBuyButton(string itemType)
    {
        if (itemType.Equals("SHOP"))
        {
            SceneManager.LoadScene("menu");
            return;
        }

        PlayerPrefs.SetInt("CustomizeTeamItem_" + currentItemName, 1);
        PlayerPrefs.Save();

        saveItemsGlobals(itemType);
        saveCurrentClothIdx();
        Globals.addDiamonds(-PRICE_PER_PRODUCT);
        updateDiamondsNumber();
        updatePrice(currentItemName);
        audioManager.Play("elementSelected");
        buyItemPanel.SetActive(false);
    }

    public void onClicksaveFansFlagsColorSettings()
    {
        PlayerPrefs.SetString(
            "CustomizeTeam_customizeFlagColor", fansFlagColors[flagsColorIdx].Split('_')[0]);
        PlayerPrefs.SetString(
            "CustomizeTeam_customizeFansColor", fansColor[fansColorIdx].Split('_')[1]);

        Globals.customizeFlagColor =
              PlayerPrefs.GetString("CustomizeTeam_customizeFlagColor");
        Globals.customizeFansColor =
              PlayerPrefs.GetString("CustomizeTeam_customizeFansColor");

        PlayerPrefs.SetInt("CustomizeTeam_fansFlagColorIdx", flagsColorIdx);
        PlayerPrefs.SetInt("CustomizeTeam_fansColorIdx", fansColorIdx);

        PlayerPrefs.Save();

        fansColorPanel.SetActive(false);
    }

    public void saveItemsGlobals(string typeName)
    {
        switch (typeName)
        {
            case "shirt":
                PlayerPrefs.SetString("CustomizeTeam_customizePlayerShirt", currentItemName);
                Globals.customizePlayerShirt = currentItemName;
                break;
            case "shorts":
                PlayerPrefs.SetString("CustomizeTeam_customizePlayerShorts", currentItemName);
                Globals.customizePlayerShorts = currentItemName;
                break;
            case "sock":
                    PlayerPrefs.SetString("CustomizeTeam_customizePlayerSock", currentItemName);
                Globals.customizePlayerSocks = currentItemName;
                break;
            case "gloves":
                PlayerPrefs.SetString("CustomizeTeam_customizePlayerGloves",currentItemName);
                Globals.customizePlayerGloves = currentItemName;
                break;
            //case "hair":
            //    PlayerPrefs.SetString("CustomizeTeam_customizePlayerHair", currentItemName);
            //    Globals.customizePlayerHair = currentItemName;
            //    break;
            case "skin":

                string teamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");
                string teamPlayers =
                        PlayerPrefs.GetString(teamName + "_teamPlayers");
                print("#DBGSKIN BEFORE " + teamPlayers);

                if (!string.IsNullOrEmpty(teamPlayers))
                {
                    int idx = -1;
                    string prevSkinName =
                        PlayerPrefs.GetString("CustomizeTeam_customizePlayerSkinHair");
                    string[] players = teamPlayers.Split('|');

                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].Split(':')[0].Equals(Globals.customizePlayerName))
                        {
                            idx = i;
                            break;
                        }
                    }

                    string replaceSkin =
                         Regex.Replace(players[idx],
                                       prevSkinName,
                                       currentItemName);
                    print("DBGSKIN replaceSkin " + replaceSkin + " Prev " + prevSkinName + " CURRENT " + currentItemName +
                        " players[idx] " + players[idx] + " idx " + idx);

                    teamPlayers = Regex.Replace(teamPlayers,
                                                players[idx],
                                                replaceSkin);
                    print("#DBGSKIN after " + teamPlayers);
 
                    PlayerPrefs.SetString(teamName + "_teamPlayers", teamPlayers);
                }

                print("#DBGSKIN AFTER " + teamPlayers);


                PlayerPrefs.SetString("CustomizeTeam_customizePlayerSkinHair", currentItemName);
                Globals.customizePlayerSkinHair = currentItemName;
                break;
            case "shoe":
                PlayerPrefs.SetString("CustomizeTeam_customizePlayerShoe", currentItemName);
                Globals.customizePlayerShoe = currentItemName;
                break;
        }

        PlayerPrefs.Save();
    }

    public void updateInputText()
    {
        playerNamePlaceHolderText.text = Globals.customizePlayerName;
        teamNamePlaceHolderText.text = Globals.customizeTeamName;
        playerCardName.text = Languages.getTranslate(Globals.customizePlayerName);
    }

    public void onClickCloseBuyItemPanel()
    {
        buyItemPanel.SetActive(false);
    }

    public void onClickCloseFlagsFansColorPanel()
    {
        fansColorPanel.SetActive(false);
    }
}


