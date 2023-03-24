using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GlobalsNS;
using UnityEngine.Analytics;
using MenuCustomNS;
using TMPro;
using System.Text.RegularExpressions;
using graphicsCommonNS;
using System.IO;
using LANGUAGE_NS;
using AudioManagerNS;

public class Shop : MonoBehaviour
{
    public GameObject shopPromotionCanvas;
    private int MAIN_MENU_MAX_BUTTONS = 4;
    private int COINS_MAX_BUTTONS = 10;
    private int DIAMONDS_MAX_BUTTONS = 10;
    private int SKILLS_MAX_BUTTONS = 4;

    private Dictionary<string, string> promotionCodesDict;
    private bool waitingForAdsEvent = false;
    private admobAdsScript admobAdsScript = null;
    private int rewardAdsCoinsNumber = Globals.rewardAdDefaultCoins;
    private GameObject admobGameObject = null;
    private Text numCoinsRewardText;
    private string rewardAdsEventName;

    private GameObject promotionPanel;
    private InputField inputCodeValue;
    private Text coinsNumAwardedText;

    private RawImage notificationImage;

    private RawImage[] skillsButtonPlayerCardImg;
    private GameObject[] playerCardSkillsStarGameObj;
    private TextMeshProUGUI[] playerCardSkillsText;
    private RawImage[] playerCardSkillsCountryFlag;
    private RawImage[] playerCardSkillsDiamondImg;

    private TextMeshProUGUI notificationText;
    private GameObject admobCanvas;
    private GameObject treasureAwardedImage;
    private TextMeshProUGUI currentCoinsText;
    private TextMeshProUGUI currentDiamondsText;

    private AudioManager audioManager;

    private GameObject shopCanvas;
    private GameObject coinsPanel;
    private GameObject diamondsPanel;
    private GameObject promoPanel;

    private GameObject[] mainButtonsFocus;
    private TextMeshProUGUI[] mainButtonText;
    private TextMeshProUGUI notificationHeaderText;
    //private TextMeshProUGUI[] buttonCoinsPrice;
    private Text[] buttonCoinsPrice;
    private Text[] buttonDiamondsPrice;

    private GameObject teamSkillsPanel;
    private TextMeshProUGUI shopHeaderText;
    private GameObject[] skillsButtonGO;
    private Button[] skillsButton;
    private RawImage[] skillsButtonImage;
    private TextMeshProUGUI[] skillsButtonHeaderText;
    private Text[] skillsButtonPriceText;

    private TextMeshProUGUI skillsTeamAnameText;
  
    private int currATeamIdx = 0;
    //private NationalTeams nationalTeams;
    private Teams nationalTeams;
    private Image playerSkillsDefenseBar;
    private Text playerSkillsDefenseText;
    private Image playerSkillsAttackBar;
    private Text playerSkillsAttackText;
    private Text playerTeamALockedCoins;
    private RawImage skillsTeamAflagRawImage;
    private int teamAFlagPrevActive = 0;
    private float teamAFlagPrevLastTimeChanged = 0f;
    private int teamAFlagNextActive = 0;
    private float teamAFlagNextLastTimeChanged = 0f;

    private int MAX_SKILLS_GK_OFFER = 10;
    private int MAX_SKILS_ATTACK_OFFER = 10;

    private GameObject skillsModel;
    private GameObject skillsModelHair;

    private int[] coinsValues = 
        new int[] { 100, 200, 500, 1000, 2000, 5000, 8000, 10000 };

    private int[] diamondsValues =
      new int[] { 100, 200, 500, 1000, 2000, 5000, 8000, 10000 };

    private GraphicsCommon graphics;
    private string waitingForBuyProductEvent = string.Empty;

    private GameObject notificationCanvas;
    private gameSettings gSettings;
    private teamManagement teamManage;

    public bool IS_ADS_MODULE_ENABLED = true;
    public bool IS_SKILLS_MODULE_ENABLED = true;
    public bool IS_PROMOTION_CODES_ENABLED = true;
    //public bool IS_UPDATE_RELATED_SCRIPT_ENABLED = true;
    public bool IS_GAME_SETTINGS_UPDATE_ENABLED = false;
    public bool IS_TEAM_MANAGEMENT_UPDATE_ENABLED = false;

    private string[] leagueNames =
            { "NATIONAL TEAMS", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMP CUP" };
    private int leagueNamesIdx = 0;
    private TextMeshProUGUI leagueNameText;
    private Teams teams;

    private GameObject buyPlayerCardPanel;
    private TextMeshProUGUI buyPlayerCardNameText;
    private RawImage buyPlayerCardCountryFlag;
    private RawImage buyPlayerCardClubFlag;
    private Image buyPlayerCardDefenseSkillsBar;
    private Image buyPlayerCardAttackSkillsBar;
    private TextMeshProUGUI buyPlayerCardSkillsDefenseText;
    private TextMeshProUGUI buyPlayerCardSkillsAttackText;
    private TextMeshProUGUI buyPlayerClubNameText;
    private TextMeshProUGUI buyPlayerClubDiamondPriceText;
    private TextMeshProUGUI buyPlayerCardNotificationText;
    private RawImage buyPlayerCardPlayerImage;
    private Button buyPlayerCardYesButton;

    private GameObject unlockedPlayerCardPanel;
    private TextMeshProUGUI unlockedPlayerCardNameText;
    private TextMeshProUGUI unlockedPlayerCardSkillsText;
    private TextMeshProUGUI unlockedPlayerClubNameText;
    private RawImage unlockedPlayerCardCountryFlag;
    private RawImage unlockedPlayerCardClubFlag;
    private RawImage unlockedPlayerCardPlayerImage;

    private GameObject pauseCanvas;

    void Awake()
    {
        init();
        setupReferences();
    }

    void Start()
    {
        setupDefaults(); 
        admobCanvas.SetActive(false);
        promotionCodesDict = new Dictionary<string, string>();
        inputCodeValue.onEndEdit.AddListener(delegate { confirmInputCode(); });
        initPromotionCodes();
        promotionPanel.SetActive(false);
        rewardAdInit();

        AnalyticsResult analyticsResult =
             Analytics.CustomEvent("Shop", new Dictionary<string, object>
        {
                    { "Shop_entered", Globals.numGameOpened},
        });

        return;
    }

    void Update()
    {
        if (Globals.purchasesQueue.Count > 0 &&
            !notificationCanvas.activeSelf)
        {
            notificationCanvas.SetActive(true);
            showNotification(
                Globals.purchasesQueue.Dequeue());            
        }

        if (waitingForAdsEvent &&
           (admobAdsScript.getAdsClosed() ||
            admobAdsScript.getAdsFailed() ||
            admobAdsScript.getAdsRewardEarn() ||
            Application.isEditor))
        {
            waitingForAdsEvent = false;
            notificationCanvas.SetActive(true);

            if (admobAdsScript.getAdsRewardEarn() ||
                Application.isEditor)
            {
                //for the time being use the same coins and diamonds number
                string rewardCoinsNumStr = rewardAdsCoinsNumber.ToString();

                if (rewardAdsEventName.Contains("stadium_sportHall"))
                {
                    if (Globals.PITCHTYPE.Equals("INDOOR"))
                        gSettings.onClickChooseStadium(0);
                    else
                        gSettings.onClickChooseStadium(1);

                    notificationCanvas.SetActive(false);
                } else if (rewardAdsEventName.Contains("stadium_street")) {
                    gSettings.onClickChooseStadium(2);
                    notificationCanvas.SetActive(false);
                } 
                else if (rewardAdsEventName.Equals("refillEnergy"))
                {
                    showNotification(
                        new PurchaseItem(
                            "refillEnergy"));
                } else if (rewardAdsEventName.Equals("coin"))
                {
                    updateCurrentCoins(rewardAdsCoinsNumber);
                    showNotification(
                        new PurchaseItem(
                            "coin" + rewardCoinsNumStr, rewardAdsCoinsNumber));
                }
                else
                {
                    updateCurrentDiamonds(rewardAdsCoinsNumber);
                    showNotification(
                    new PurchaseItem(
                         "diamond" + rewardCoinsNumStr, rewardAdsCoinsNumber));
                }
            }
            else
            {
                showNotification(   
                    new PurchaseItem("adsfailed"));
            }
            
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsRewardEarn(false);

            admobCanvas.SetActive(false);
        }

        updateFlagButtons();
    }

    public void showNotification(PurchaseItem item)
    {
        string type = item.name;

        if (IS_TEAM_MANAGEMENT_UPDATE_ENABLED)
        {
            initPauseCanvas();
        }

        notificationHeaderText.text = 
            Languages.getTranslate("Congratulations!");
        audioManager.Play("elementAppear");

        if (type.Contains("removeAds"))
        {
            notificationText.text =
                Languages.getTranslate("Excellent! Ads disabled!");
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/showNotificationAdsRemoved");
        }
        else if (type.Contains("coin"))
        {
            int coins = item.coins;
            if (coins == 10001)
            {
                coins = 10000;
            } else
            {
                updateGlobalCoinsText();
                updateRelatedScripts("coin");
            }

            notificationText.text =
                Languages.getTranslate("Excellent! +" + coins.ToString() + " coins awarded!", 
                                       new List<string>() { coins.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/shownotificationCoins");           
        }
        else if (type.Contains("diamond"))
        {
            int diamond = item.diamonds;
            //special christmas Promotion
            if (diamond == 20000)
            {
                shopPromotionCanvas.SetActive(false);
                GameObject.Find("mainCurrentCoinsText").GetComponent<TextMeshProUGUI>().text = Globals.coins.ToString();
                GameObject.Find("mainCurrentDiamondsText").GetComponent<TextMeshProUGUI>().text = Globals.diamonds.ToString();                
            } else
            {
                updateGlobalDiamondText();
                updateRelatedScripts("diamond");
            }

            notificationText.text =
                Languages.getTranslate("Excellent! +" + diamond.ToString() + " diamonds awarded!",
                                       new List<string>() { diamond.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/showNotificationDiamonds");
   
        } else if (type.Contains("unlockedplayercard"))
        {
            int teamPurchaseId = item.purchaseTeamId;
            string leagueName = item.leagueName;
            string teamName = item.teamName;
            string[] playerDesc = item.playerDesc.Split(':');

            notificationCanvas.SetActive(false);
            unlockedPlayerCardPanel.SetActive(true);

            unlockedPlayerCardClubFlag.texture =
                 Resources.Load<Texture2D>(teamName);

            unlockedPlayerCardNameText.text =
                playerDesc[0];
            unlockedPlayerCardSkillsText.text = "D: " + playerDesc[2] + "\n" +
                                                "A: " + playerDesc[3];
            string fullTeamPath =
                graphics.getFlagPath("NATIONALS", playerDesc[1]);
            graphics.setFlagRawImage(unlockedPlayerCardCountryFlag, fullTeamPath);

            fullTeamPath =
                graphics.getFlagPath(teamName);
            graphics.setFlagRawImage(unlockedPlayerCardClubFlag, fullTeamPath);
            
            unlockedPlayerCardPlayerImage.texture =
                Resources.Load<Texture2D>("playersCard/" + playerDesc[6]);
            unlockedPlayerClubNameText.text = teamName;

            updateAfterBuying();
            updateRelatedScripts("unlockedplayercard");
        }
        //team unlock
        else if (type.Contains("unlocked"))
        {
            int teamPurchaseId = item.purchaseTeamId;
            string[] team = nationalTeams.getTeamByIndex(teamPurchaseId);
            notificationText.text =
                Languages.getTranslate("Excellent! " + team[0] + " unlocked!",
                                       new List<string>() { team[0] });

            string teamName = Regex.Replace(team[0], "\\s+", "");

            string fullTeamPath =
                graphics.getFlagPath("NATIONALS", teamName);
            graphics.setFlagRawImage(notificationImage, fullTeamPath);

            //notificationImage.texture = 
            //    Resources.Load<Texture2D>("Flags/" + teamName.ToLower());
            updateAfterBuying();
            updateRelatedScripts("teamunlocked");

        }
        else if (type.Contains("attackdefense"))
        {
            int attack = item.attackVal;
            int defense = item.defenseVal;
            notificationText.text =
                Languages.getTranslate("Well done! +" + attack.ToString() + " to attack skills and +" +
                        defense.ToString() + " to defense skills awarded!",
                        new List<string>() { attack.ToString(), defense.ToString() });
            notificationImage.texture =
                 Resources.Load<Texture2D>("others/gloveShot");
            updateAfterBuying();
            updateRelatedScripts("attackdefense");
        }
        else if (type.Contains("defense"))
        {
            int defense = item.defenseVal;
            notificationText.text =
                Languages.getTranslate(
                    "Excellent! +" + defense.ToString() + " to defense skills awarded!",
                     new List<string>() { defense.ToString() });
            notificationImage.texture =
                    Resources.Load<Texture2D>("others/gloves");
            updateAfterBuying();
            updateRelatedScripts("attackdefense");
        }
        else if (type.Contains("attack"))
        {
            int attack = item.attackVal;
            notificationText.text =
                Languages.getTranslate(
                "Excellent! +" + attack.ToString() + " to attack skills awarded!",
                new List<string>() { attack.ToString() });
            notificationImage.texture =
                    Resources.Load<Texture2D>("others/shoot3_wider");
            updateAfterBuying();
            updateRelatedScripts("attack");
        }
        else if (type.Contains("refillEnergy"))
        {
            teamManage.refillSelectedPlayerEnergy();
            notificationText.text =
               Languages.getTranslate("Excellent! player's energy refill!"); 
            notificationImage.texture =
                    Resources.Load<Texture2D>("Shop/adsRemovedIcon2");
            notificationHeaderText.text = Languages.getTranslate("Refill energy");
        }
        else if (type.Contains("adsfailed"))
        {
            notificationText.text =
                Languages.getTranslate("Ads watching failed");
            notificationImage.texture =
                    Resources.Load<Texture2D>("Shop/showNotificationAdsFailed");
            notificationHeaderText.text =
                Languages.getTranslate("Ads failed");
        }

        return;
    }

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

    /*public void showRewardedPanel(int coinsNum, string type)
    {
        string textToDisplay = "";
        if (type.Contains("ads")) {
            textToDisplay = 
                "Excellent! Ads disabled!";
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/adsRemovedIcon");
        } else
        {
            textToDisplay =
                "Excellent! +" + coinsNum.ToString() + " coins awarded!";
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/goldHeapMiddle");
        }
       
        rewardAwardedPanel.SetActive(true);
        treasureAwardedImage.SetActive(false);
        coinsNumAwardedText.text = "";
        StartCoroutine(rewardAwardedPanelCoins(coinsNum, textToDisplay));
    }*/

    /*public void showFailedPanel()
    {
        rewardFailedPanel.SetActive(true);
    }*/

    private IEnumerator rewardAwardedPanelCoins(int coinsNum, string textToDisplay)
    {
        yield return new WaitForSeconds(0.5f);
        audioManager.Play("elementAppear");
        treasureAwardedImage.SetActive(true);
        coinsNumAwardedText.text = textToDisplay;               
    }
   
    private void rewardAdInit()
    {
        //if (Globals.adsEnable)
        //{
            admobGameObject = GameObject.Find("admobAdsGameObject");
            admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsRewardEarn(false);
        //workaround
            //Debug.Log("sceneName " + SceneManager.GetActiveScene().name);
            if (!SceneManager.GetActiveScene().name.Equals("extraPowers"))
                admobAdsScript.hideBanner();
        //}    
    }

    /*public void closeRewardPanel()
    {
        rewardAwardedPanel.SetActive(false);
        rewardFailedPanel.SetActive(false);
    }*/

    public void watchRewardAdButton(string eventName)
    {
        if (Application.isEditor)
        {
            waitingForAdsEvent = true;
            rewardAdsEventName = eventName;
        }
        else
        {
            if (admobAdsScript.showRewardAd())
            {
                waitingForAdsEvent = true;
                admobCanvas.SetActive(true);
                rewardAdsEventName = eventName;
            }
            else
            {
                showNotification(
                       new PurchaseItem("adsfailed"));
                waitingForAdsEvent = false;
            }
        }

        admobAdsScript.setAdsClosed(false);
        admobAdsScript.setAdsFailed(false);
        admobAdsScript.setAdsRewardEarn(false);
    }

    public void shopPrevCanvas()
    {
        SceneManager.LoadScene("menu");
        return;
    }

    public void closeNoificationCanvas()
    {
        notificationCanvas.SetActive(false);
    }

    public void confirmInputCode()
    {
        string inputedCode = inputCodeValue.text;

        //Debug.Log("DBGINPUTCODE12 INPUTED CODE " + inputedCode);
        foreach (KeyValuePair<string, string> entry in promotionCodesDict)
        {
            string key = entry.Key;
            string value = entry.Value;

            if (inputedCode.Equals(key) &&
                !PlayerPrefs.HasKey(inputedCode))
            {
                //print("DBGINPUTCODE12 " + key + " VALUE " + value);
                if (value.Contains("remove_ads"))
                {
                    removeAds();
                    PlayerPrefs.SetInt(inputedCode, 1);
                    PlayerPrefs.Save();
                    inputCodeValue.text = Languages.getTranslate("Ads disabled!");
                    return;
                }
                else if (value.Contains("coins"))
                {
                    string[] productName = value.Split('_');
                    string coinsStr = productName[1];

                    if (int.TryParse(coinsStr, out int coinInt))
                    {
                        updateCurrentCoins(coinInt);
                        updateRelatedScripts("coins");
                        PlayerPrefs.SetInt(inputedCode, 1);
                        PlayerPrefs.Save();
                        inputCodeValue.text = 
                            Languages.getTranslate(coinsStr + " coins added!",
                            new List<string>() { coinsStr.ToString() });
                        return;
                    }
                    else
                    {
                        //print("DBGINPUTCODE12 Wrong CODE 1");
                        inputCodeValue.text = Languages.getTranslate("Wrong code!");
                        return;
                    }
                }
                else if (value.Contains("diamonds"))
                {
                    string[] productName = value.Split('_');
                    string diamondStr = productName[1];

                    if (int.TryParse(diamondStr, out int diamondInt))
                    {
                        updateCurrentDiamonds(diamondInt);
                        updateRelatedScripts("diamonds");

                        PlayerPrefs.SetInt(inputedCode, 1);
                        PlayerPrefs.Save();
                        inputCodeValue.text = 
                            Languages.getTranslate(diamondStr + " diamonds added!",
                                                   new List<string>() { diamondStr });
                        return;
                    }
                    else
                    {
                        //print("DBGINPUTCODE12 Wrong CODE 1");
                        inputCodeValue.text = Languages.getTranslate("Wrong code!");
                        return;
                    }
                } else if (value.Contains("attack") &&
                           value.Contains("defense"))
                {
                    /*TODO*/
                    /*now it always gives 10 attack and 10 defense*/
                    string[] line = value.Split(':');
                    //string[] attack10_defense10 = line.Split("_");
                    string teamName = line[1];

                    int teamIdx = nationalTeams.getTeamIndexByName(teamName);

                    //print("DBGTEAMNAME " + teamName + " teamIdx " + teamIdx);
                    if (teamIdx == -1)
                    {
                        inputCodeValue.text = Languages.getTranslate("Operation failed!");
                        return;
                    }

                    Globals.incTeamSkills(teamIdx, 10, 10);
                    updateRelatedScripts("defense_attack");

                    //PlayerPrefs.SetInt(inputedCode, 1);

                    inputCodeValue.text = Languages.getTranslate("Skills improved!");
                    return;
                } else if (value.Contains("enlargegoalsize"))
                {
                    /*TODO SIZES*/
                    string[] line = value.Split(':');
                    //string[] attack10_defense10 = line.Split("_");
                    string goalSize = line[1];
                    Globals.cpuGoalSize = goalSize;
                    PlayerPrefs.SetInt(inputedCode, 1);
                    PlayerPrefs.Save();

                    inputCodeValue.text = Languages.getTranslate("Goal enlarged!");

                    return;
                }
            }
        }
                
        inputCodeValue.text = Languages.getTranslate("Wrong code!");
    }
 
    public void removeAds()
    {
        Globals.adsRemove();
    }

    public void updateCurrentCoins(int coins)
    {
        Globals.addCoins(coins);
        updateGlobalCoinsText();
    }
    
    public void updateCurrentDiamonds(int diamonds)
    {
        Globals.addDiamonds(diamonds);
        updateGlobalDiamondText();
    }

    private void setupDefaults()
    {
        string[] team = nationalTeams.getTeamByIndex(currATeamIdx);

        updateAfterBuying();
        fillCoinsOffersButton();
        fillSkillsOffersButton(team, leagueNames[leagueNamesIdx]);

        leagueNameText.text = leagueNames[leagueNamesIdx];
        shopHeaderText.text = Languages.getTranslate("Get coins!");
        disableFocusApartFrom(0);

        shopCanvas.SetActive(false);
        notificationCanvas.SetActive(false);
    }

    private void disableFocusApartFrom(int idx)
    {
        for (int i = 0; i < MAIN_MENU_MAX_BUTTONS; i++) {
            if (i == idx)
            {
                //mainButtonText[i].color = new Color32(241, 234, 203, 255);
                mainButtonsFocus[i].SetActive(true);
                continue;
            }
            //mainButtonText[i].color = new Color32(74, 172, 247, 255); 
            mainButtonsFocus[i].SetActive(false);
        }
    }

    private void updateTreasure()
    {
        updateGlobalCoinsText();
        updateGlobalDiamondText();
    }

    private void updateAfterBuying()
    {
        //print("DBGPurchasing UPDATE AFTER BUYING");
        fillTeam(currATeamIdx);
        
        updateGlobalCoinsText();
        updateGlobalDiamondText();
        //updateRelatedScripts("standard");
    }

    private void updateGlobalCoinsText()
    {
        currentCoinsText = GameObject.Find("shopCurrentCoinsText").GetComponent<TextMeshProUGUI>();
        currentCoinsText.text = Globals.coins.ToString();
    }

    private void updateGlobalDiamondText()
    {
        currentDiamondsText = GameObject.Find("shopCurrentDiamondsText").GetComponent<TextMeshProUGUI>();
        currentDiamondsText.text = Globals.diamonds.ToString();
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
                //currATeamIdx = nationalTeams.getMaxTeams() - 1;
                currATeamIdx = teams.getMaxTeams() - 1;

            }
        }

        fillTeam(currATeamIdx);

        /*string[] team = nationalTeams.getTeamByIndex(currATeamIdx);
        setTeamName(skillsTeamAnameText, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));
        setFlagImage(skillsTeamAflagRawImage, team[0], int.Parse(team[4]));

        graphics.setPlayerTextures(
             skillsModel, skillsModelHair, currATeamIdx);

        fillSkillsOffersButton(team);
 
        playerSkillsDefenseBar.fillAmount = float.Parse(team[1]) / 100.0f;
        playerSkillsDefenseText.text = "Defense: " + team[1];
        playerSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        playerSkillsAttackText.text = "Attack: " + team[2];*/
    }

    public void teamAFlagNext()
    {
        //popUpNoCoins.SetActive(false);

        //if ((currATeamIdx + 1) < nationalTeams.getMaxTeams())
        if ((currATeamIdx + 1) < teams.getMaxTeams())
        {
            currATeamIdx++;
        }
        else
        {
            //if (currATeamIdx == (nationalTeams.getMaxTeams() - 1))
            if (currATeamIdx == (teams.getMaxTeams() - 1))
            {
                currATeamIdx = 0;
            }
        }

        fillTeam(currATeamIdx);

       /* string[] team = nationalTeams.getTeamByIndex(currATeamIdx);
        setTeamName(skillsTeamAnameText, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));
        setFlagImage(skillsTeamAflagRawImage, team[0], int.Parse(team[4]));

          graphics.setPlayerTextures(
            skillsModel, skillsModelHair, currATeamIdx);

        fillSkillsOffersButton(team);

        playerSkillsDefenseBar.fillAmount = float.Parse(team[1]) / 100.0f;
        playerSkillsDefenseText.text = "Defense: " + team[1];
        playerSkillsAttackBar.fillAmount = float.Parse(team[2]) / 100.0f;
        playerSkillsAttackText.text = "Attack: " + team[2];*/
    }

    private void fillTeam(int idx)
    {
        string leagueName = leagueNames[leagueNamesIdx];
        string[] team = teams.getTeamByIndex(idx);
        setTeamName(skillsTeamAnameText, team[0], int.Parse(team[4]));
        setTeamLocked(playerTeamALockedCoins, team[0], int.Parse(team[4]));

        string fullTeamPath =
              graphics.getFlagPath(team[0]);
        setFlagImage(skillsTeamAflagRawImage, fullTeamPath, int.Parse(team[4]));

        //print("DBGPurchasing team name update " + team[0]);

        string playerSkinHairDescA =
            teams.getPlayerDescByIndex(idx, 0);

        graphics.setPlayerTextures(
          skillsModel, 
          skillsModelHair, 
          idx,
          leagueName,
          playerSkinHairDescA,
          false,
          false,
          teams);

        fillSkillsOffersButton(team, leagueNames[leagueNamesIdx]);
        Vector2 skills = 
            Globals.getTeamSkillsAverage(team, leagueName);

        //print("DBGPurchasing team name update new skills " + skills);

        playerSkillsDefenseBar.fillAmount = skills.x / 100.0f;
        playerSkillsDefenseText.text = 
            Languages.getTranslate("Defense: " + ((int) skills.x).ToString(),
                                    new List<string>() { ((int)skills.x).ToString() });
        playerSkillsAttackBar.fillAmount = skills.y / 100.0f;
        playerSkillsAttackText.text = 
            Languages.getTranslate("Attack: " + ((int) skills.y).ToString(),
                                    new List<string>() { ((int)skills.y).ToString() });
    }

    public void onClickPrevLeague()
    {
        if (leagueNamesIdx > 0)
        {
            leagueNameText.text = leagueNames[--leagueNamesIdx];
            teams.setLeague(leagueNames[leagueNamesIdx]);
            currATeamIdx = 0;
            fillTeam(0);
            clearFlagButtonsVars();
            ///setupTeamDefaults();
        }
    }

    public void onClickNextLeague()
    {
        if (leagueNamesIdx < (leagueNames.Length - 1))
        {
            //time.text = leagueNames[++leagueIdx];
            //leagueNameText.text = leagueNames[++leagueIdx];
            leagueNameText.text = leagueNames[++leagueNamesIdx];
            teams.setLeague(leagueNames[leagueNamesIdx]);
            currATeamIdx = 0;
            fillTeam(0);
            clearFlagButtonsVars();
            //setupTeamDefaults();           
        }
    }

    public void setSkillsTeam(int leagueNamesIdx, int teamIdx)
    {
        leagueNameText.text = leagueNames[leagueNamesIdx];
        teams.setLeague(leagueNames[leagueNamesIdx]);
        fillTeam(teamIdx);
        clearFlagButtonsVars();
    }

    private void setTeamName(TextMeshProUGUI teamText, string team, int teamNeededCoins)
    {
        teamText.text = team.ToUpper();
    }

    private void setTeamLocked(Text teamText, string teamName, int teamNeededCoins)
    {
        if (teamNeededCoins > Globals.coins &&
            !PlayerPrefs.HasKey(teamName))
        {
            teamText.text = teamNeededCoins.ToString() + " $";
        }
        else
        {
            teamText.text = "";
        }
    }

    private void setFlagImage(RawImage image, string filePath, int teamNeededCoins)
    {
        if (filePath.Contains("logoFile"))
        {
            if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
            {
                image.texture = graphics.loadTexture(Globals.logoFilePath);
            }
            else
            {
                image.texture = Resources.Load<Texture2D>(
                            "others/logoFile");
            }

            return;
        }

        string teamName = Path.GetFileName(filePath);

        //print("TEAMNAME PATH " + teamName);
        if (teamNeededCoins > Globals.coins &&
            !PlayerPrefs.HasKey(teamName))
        {
            image.texture = Resources.Load<Texture2D>("Flags/locked");
        }
        else
        {
            filePath = Regex.Replace(filePath, "\\s+", "");
            image.texture = Resources.Load<Texture2D>("Flags/" + filePath.ToLower());
        }
    }

    private void updateRelatedScripts(string type)
    {
        //if (!IS_UPDATE_RELATED_SCRIPT_ENABLED)
        //    return;

        //print("#UPDATE RELATED#");
        if (IS_GAME_SETTINGS_UPDATE_ENABLED)
            gSettings.setupTeamDefaults();

        if (IS_TEAM_MANAGEMENT_UPDATE_ENABLED)
            teamManage.updateAfterBuying(type);
    }

    private void fillCoinsOffersButton()
    {
        for (int buttonIdx = 0; buttonIdx < COINS_MAX_BUTTONS; buttonIdx++)
        {
            if (buttonIdx == 0)
            {
                buttonCoinsPrice[buttonIdx].text =
                    IAPManager.instance.getPriceByHash("removeAds");
                continue;
            }

            /*FREE Reward ads*/
            if (buttonIdx == 1)
            {
                buttonCoinsPrice[buttonIdx].text = "FREE";
                continue;
            }

            //print("buttonCoinsPrice[buttonIdx] " + buttonIdx + " OBJECT " + buttonCoinsPrice[buttonIdx]);
            buttonCoinsPrice[buttonIdx].text = 
                IAPManager.instance.getPriceByHash("coin" + coinsValues[buttonIdx - 2].ToString());
        }
    }

    private void fillDiamondsOffersButton()
    {
        for (int buttonIdx = 0; buttonIdx < DIAMONDS_MAX_BUTTONS; buttonIdx++)
        {
            if (buttonIdx == 0)
            {
                buttonDiamondsPrice[buttonIdx].text =
                    IAPManager.instance.getPriceByHash("removeAds");
                continue;
            }

            /*FREE Reward ads*/
            if (buttonIdx == 1)
            {
                buttonCoinsPrice[buttonIdx].text = "FREE";
                continue;
            }

            //print("buttonCoinsPrice[buttonIdx] " + buttonIdx + " OBJECT " + buttonCoinsPrice[buttonIdx]);
            buttonDiamondsPrice[buttonIdx].text =
                IAPManager.instance.getPriceByHash("diamond" + diamondsValues[buttonIdx - 2].ToString());
        }
    }

    private void fillSkillsOffersButton(string[] team, string leagueName)
    {        
        string teamName = team[0];
        int gkSkills = int.Parse(team[1]);
        int attackSkills = int.Parse(team[2]);
        int buttonIdx = 0;

        if (leagueName.Equals("NATIONAL TEAMS"))
        {
            int teamNeededCoins = int.Parse(team[4]);

            //only aplicable for players Card
            for (int i = 0; i < SKILLS_MAX_BUTTONS; i++)
                playerCardSkillsStarGameObj[i].SetActive(false);

            if (teamNeededCoins > Globals.coins &&
                !PlayerPrefs.HasKey(teamName))
            {
                skillsButtonGO[buttonIdx].SetActive(true);
                var tempColor = skillsButtonImage[buttonIdx].color;
                tempColor.a = 1f;
                skillsButtonImage[buttonIdx].color = tempColor;

                string teamFlagName = Regex.Replace(teamName, "\\s+", "");
                skillsButtonImage[buttonIdx].texture = 
                    Resources.Load<Texture2D>("Flags/national/" + teamFlagName.ToLower());

                int cost =
                    calcUnlockTeamPrice(teamNeededCoins - Globals.coins);

                skillsButtonHeaderText[buttonIdx].text =
                    Languages.getTranslate("Unlock " + teamName,
                                           new List<string>() { teamName });
                skillsButton[buttonIdx].onClick.RemoveAllListeners();
                skillsButton[buttonIdx].onClick.AddListener(
                    delegate { buyProductByName("unlockedteam" + cost.ToString() + "goaliestriker"); });
                skillsButtonPriceText[buttonIdx].text =
                    IAPManager.instance.getPriceByHash("unlockedteam" + cost.ToString());

                buttonIdx++;
            }
            else
            {
                int attackSkillsDiff = Globals.MAX_PLAYER_SKILLS - attackSkills;
                int gkSkillsDiff = Globals.MAX_PLAYER_SKILLS - gkSkills;
                int incAttackSkills = Mathf.Min(MAX_SKILS_ATTACK_OFFER, attackSkillsDiff);
                int incGkSkills = Mathf.Min(MAX_SKILLS_GK_OFFER, gkSkillsDiff);

                if (incAttackSkills > 0 &&
                    attackSkills < 100)
                {
                    skillsButtonGO[buttonIdx].SetActive(true);
                    var tempColor = skillsButtonImage[buttonIdx].color;
                    tempColor.a = 1f;
                    skillsButtonImage[buttonIdx].color = tempColor;
                    skillsButtonImage[buttonIdx].texture =
                        Resources.Load<Texture2D>("others/shoot3_wider");
                    skillsButtonHeaderText[buttonIdx].text =
                         Languages.getTranslate(
                             "Attack +" + incAttackSkills.ToString(),
                             new List<string>() { incAttackSkills.ToString() });
                    skillsButtonPriceText[buttonIdx].text =
                        IAPManager.instance.getPriceByHash("attack" + incAttackSkills.ToString() + "team");
                    skillsButton[buttonIdx].onClick.RemoveAllListeners();
                    skillsButton[buttonIdx].onClick.AddListener(
                        delegate { buyProductByName("attack" + incAttackSkills.ToString() + "goaliestriker"); });
                    buttonIdx++;
                }

                if (incGkSkills > 0 &&
                    gkSkills < 100)
                {
                    skillsButtonGO[buttonIdx].SetActive(true);
                    var tempColor = skillsButtonImage[buttonIdx].color;
                    tempColor.a = 1f;
                    skillsButtonImage[buttonIdx].color = tempColor;

                    skillsButtonImage[buttonIdx].texture =
                       Resources.Load<Texture2D>("others/gloves");

                    skillsButtonHeaderText[buttonIdx].text =
                           Languages.getTranslate(
                           "Defense +" + incGkSkills.ToString(),
                           new List<string>() { incGkSkills.ToString() });
                    skillsButtonPriceText[buttonIdx].text =
                       IAPManager.instance.getPriceByHash("defense" + incGkSkills.ToString() + "team");
                    skillsButton[buttonIdx].onClick.RemoveAllListeners();
                    skillsButton[buttonIdx].onClick.AddListener(
                        delegate { buyProductByName("defense" + incGkSkills.ToString() + "goaliestriker"); });
                    buttonIdx++;
                }

                if (incAttackSkills >= 10 &&
                    incGkSkills >= 10 &&
                    attackSkills < 100 &&
                    gkSkills < 100)
                {
                    skillsButtonGO[buttonIdx].SetActive(true);
                    var tempColor = skillsButtonImage[buttonIdx].color;
                    tempColor.a = 1f;
                    skillsButtonImage[buttonIdx].color = tempColor;

                    skillsButtonImage[buttonIdx].texture =
                       Resources.Load<Texture2D>("others/gloveShot");
                    skillsButtonHeaderText[buttonIdx].text =
                        Languages.getTranslate(
                          "Defense & Attack +" + incAttackSkills.ToString(),
                          new List<string>() { incAttackSkills.ToString() });
                    skillsButtonPriceText[buttonIdx].text =
                        IAPManager.instance.getPriceByHash("attackdefense" + incGkSkills.ToString() + "team");
                    skillsButton[buttonIdx].onClick.RemoveAllListeners();
                    skillsButton[buttonIdx].onClick.AddListener(
                        delegate { buyProductByName("attackdefense" + incAttackSkills.ToString() + "goaliestriker"); });
                    buttonIdx++;
                }
            }

            //disable player cards images
            for (int i = 0; i < buttonIdx; i++)
            {
                var tempColor = skillsButtonPlayerCardImg[buttonIdx].color;
                tempColor.a = 0f;
                skillsButtonPlayerCardImg[i].color = tempColor;
                tempColor = playerCardSkillsDiamondImg[i].color;
                tempColor.a = 0f;
                playerCardSkillsDiamondImg[i].color = tempColor;
                tempColor = playerCardSkillsCountryFlag[i].color;
                tempColor.a = 0f;
                playerCardSkillsCountryFlag[i].color = tempColor;
                playerCardSkillsText[i].text = "";
            }          
        } else
        {
            //player cards
            string[] players = team[12].Split('|');
            for (int i = 0; i < players.Length; i++)
            {
                string[] playerDesc = players[i].Split(':');

                if (playerDesc[4].Equals("L")) {
                    skillsButtonGO[buttonIdx].SetActive(true);
                    skillsButton[buttonIdx].onClick.RemoveAllListeners();

                    var tempColor = skillsButtonImage[buttonIdx].color;
                    tempColor.a = 0;
                    skillsButtonImage[buttonIdx].color =
                        tempColor;

                    skillsButtonHeaderText[buttonIdx].text =
                        Languages.getTranslate("Unlock " + playerDesc[0],
                                                new List<string>() { playerDesc[0] });

                    tempColor = skillsButtonPlayerCardImg[buttonIdx].color;
                    tempColor.a = 1f;
                    skillsButtonPlayerCardImg[buttonIdx].color = tempColor;

                    skillsButtonPlayerCardImg[buttonIdx].texture =
                        Resources.Load<Texture2D>("playersCard/" + playerDesc[6]);

                    playerCardSkillsText[buttonIdx].text = 
                        "D: " + playerDesc[2] + "\n" + "A: " + playerDesc[3];

                    //print("#DBG start " +
                    //    Globals.isPlayerCardStar(int.Parse(team[1]), int.Parse(team[2])) +
                    //     " defense " + int.Parse(team[1]) 
                    //     + " attack " + int.Parse(team[2])
                    //     + " Player " + playerDesc[0]);

                    if (Globals.isPlayerCardStar(
                        int.Parse(playerDesc[2]), int.Parse(playerDesc[3])))
                        playerCardSkillsStarGameObj[buttonIdx].SetActive(true);
                    else
                        playerCardSkillsStarGameObj[buttonIdx].SetActive(false);

                    tempColor = playerCardSkillsCountryFlag[buttonIdx].color;
                    tempColor.a = 1f;
                    playerCardSkillsCountryFlag[buttonIdx].color = tempColor;

                    string fullTeamPath =
                        graphics.getFlagPath("NATIONALS", playerDesc[1]);

                    graphics.setFlagRawImage(
                        playerCardSkillsCountryFlag[buttonIdx], fullTeamPath);

                    skillsButton[buttonIdx].onClick.RemoveAllListeners();
                    string playerStr = players[i];

                    tempColor = playerCardSkillsDiamondImg[buttonIdx].color;
                    //string price = Globals.getPlayerCardPrice(playerDesc[2],
                    //                                          playerDesc[3]);

                    string price =
                        Globals.getPlayerCardPrice(players[i]);

                    if (Globals.diamonds >= int.Parse(price)) {
                        tempColor.a = 1f;
                        playerCardSkillsDiamondImg[buttonIdx].color = tempColor;
                        skillsButtonPriceText[buttonIdx].text = price;
                                          
                        skillsButton[buttonIdx].onClick.AddListener(
                        delegate {
                        buyPlayerCardForDiamonds(currATeamIdx,
                                                 leagueNames[leagueNamesIdx],
                                                 teamName,
                                                 playerStr
                                                 );
                        });
                    } else
                    {
                        tempColor.a = 0f;
                        playerCardSkillsDiamondImg[buttonIdx].color = tempColor;

                        //it's the same for coins and diamonds
                        int cost =
                            calcUnlockTeamPrice(int.Parse(price));
                        
                        skillsButton[buttonIdx].onClick.AddListener(
                        delegate { buyProductByName(
                            "unlockedplayercard" + cost.ToString() + "goaliestriker",
                            teamName,
                            playerStr,
                            leagueNames[leagueNamesIdx],
                            currATeamIdx); });

                        //print("PLYAERS_EQUAL ADD LISTENER" + players[i]);

                        skillsButtonPriceText[buttonIdx].text =
                          IAPManager.instance.getPriceByHash("unlockedplayercard" + cost.ToString());
                    }

                    buttonIdx++;
                }

                if (buttonIdx >= SKILLS_MAX_BUTTONS)
                    break;
                              
                //skillsButtonGO[i].SetActive(false);
            }        
        }

        for (int i = buttonIdx; i < SKILLS_MAX_BUTTONS; i++)
        {
            skillsButtonGO[i].SetActive(false);
        }
    }

    private void initBuyPlayerCardReferences()
    {
        buyPlayerCardPanel = 
            GameObject.Find("shopBuyPlayerCardPanel");
        buyPlayerCardNameText =
            GameObject.Find("shopBuyPlayerCardNameText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardCountryFlag =
            GameObject.Find("shopBuyPlayerCardCountryFlag").GetComponent<RawImage>();
        buyPlayerCardClubFlag =
            GameObject.Find("shopBuyPlayerCardClubFlag").GetComponent<RawImage>();
        buyPlayerCardDefenseSkillsBar =
            GameObject.Find("shopBuyPlayerCardDefenseSkillsBar").GetComponent<Image>();
        buyPlayerCardAttackSkillsBar =
            GameObject.Find("shopBuyPlayerCardAttackSkillsBar").GetComponent<Image>();
        buyPlayerCardSkillsDefenseText =
            GameObject.Find("shopBuyPlayerCardSkillsDefenseText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardSkillsAttackText =
            GameObject.Find("shopBuyPlayerCardSkillsAttackText").GetComponent<TextMeshProUGUI>();
        buyPlayerClubNameText =
            GameObject.Find("shopBuyPlayerClubNameText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardNotificationText =
            GameObject.Find("shopBuyPlayerCardNotificationText").GetComponent<TextMeshProUGUI>();
        buyPlayerClubDiamondPriceText =
            GameObject.Find("shopBuyPlayerClubDiamondPriceText").GetComponent<TextMeshProUGUI>();
        buyPlayerCardPlayerImage =
            GameObject.Find("shopBuyPlayerCardPlayerImage").GetComponent<RawImage>();
        buyPlayerCardYesButton =
            GameObject.Find("shopBuyPlayerCardYesButton").GetComponent<Button>();

        buyPlayerCardPanel.SetActive(false);
    }

    private void initUnlockedPlayerCardReferences()
    {
        unlockedPlayerCardPanel =
            GameObject.Find("shopUnlockedPlayerCardPanel");
        unlockedPlayerCardNameText =
            GameObject.Find("shopUnlockedPlayerCardNameText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerCardSkillsText =
            GameObject.Find("shopUnlockedPlayerCardSkillsText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerClubNameText =
            GameObject.Find("shopUnlockedPlayerClubNameText").GetComponent<TextMeshProUGUI>();
        unlockedPlayerCardCountryFlag =
            GameObject.Find("shopUnlockedPlayerCardCountryFlag").GetComponent<RawImage>();
        unlockedPlayerCardClubFlag =
            GameObject.Find("shopUnlockedPlayerCardClubFlag").GetComponent<RawImage>();
        unlockedPlayerCardPlayerImage =
            GameObject.Find("shopUnlockedPlayerCardPlayerImage").GetComponent<RawImage>();

        unlockedPlayerCardPanel.SetActive(false);
    }

    private int calcUnlockTeamPrice(int coinsNeeded)
    {        
        for (int i = 1; i < coinsValues.Length; i++) {
            if (coinsNeeded > coinsValues[i])
                continue;
           
            int midVal = (coinsValues[i] - coinsValues[i-1]) / 2;
            if ((coinsValues[i-1] + midVal) >= coinsNeeded)
            {
                return coinsValues[i-1];
            }

            return coinsValues[i];
        }

        return coinsValues[coinsValues.Length - 1];
    }

    private void init()
    {
        //nationalTeams = new NationalTeams();
        nationalTeams = new Teams("NATIONALS");

        buttonCoinsPrice = new Text[COINS_MAX_BUTTONS];
        buttonDiamondsPrice = new Text[DIAMONDS_MAX_BUTTONS];
        skillsButtonGO = new GameObject[SKILLS_MAX_BUTTONS];
        mainButtonsFocus = new GameObject[MAIN_MENU_MAX_BUTTONS];
        mainButtonText = new TextMeshProUGUI[MAIN_MENU_MAX_BUTTONS];
        skillsButton = new Button[SKILLS_MAX_BUTTONS];
        skillsButtonImage = new RawImage[SKILLS_MAX_BUTTONS];
        skillsButtonHeaderText = new TextMeshProUGUI[SKILLS_MAX_BUTTONS];
        skillsButtonPriceText = new Text[SKILLS_MAX_BUTTONS];

        skillsButtonPlayerCardImg = new RawImage[SKILLS_MAX_BUTTONS];
        playerCardSkillsStarGameObj = new GameObject[SKILLS_MAX_BUTTONS];
        playerCardSkillsText = new TextMeshProUGUI[SKILLS_MAX_BUTTONS];
        playerCardSkillsCountryFlag = new RawImage[SKILLS_MAX_BUTTONS];
        playerCardSkillsDiamondImg = new RawImage[SKILLS_MAX_BUTTONS];

        teams = new Teams(leagueNames[leagueNamesIdx]);
        graphics = new GraphicsCommon();
    }

    private void setupReferences()
    {
        if (IS_GAME_SETTINGS_UPDATE_ENABLED)
            gSettings = 
                GameObject.Find("gameSettings").GetComponent<gameSettings>();

        if (IS_TEAM_MANAGEMENT_UPDATE_ENABLED)
            teamManage = 
                GameObject.Find("teamManagement").GetComponent<teamManagement>();

        audioManager = FindObjectOfType<AudioManager>();
        shopCanvas = GameObject.Find("shopCanvas");
        coinsPanel = GameObject.Find("shopCoinsPanel");
        diamondsPanel = GameObject.Find("shopDiamondsPanel");
        promoPanel = GameObject.Find("shopPromoPanel");
        promotionPanel = GameObject.Find("shopPromoPanel");
        notificationHeaderText = GameObject.Find("shopNotificationHeaderText").GetComponent<TextMeshProUGUI>();
        teamSkillsPanel = GameObject.Find("shopSkillsPanel");
        notificationCanvas = GameObject.Find("shopNotificationCanvas");

        notificationImage = GameObject.Find("notificationImageShop").GetComponent<RawImage>();
        notificationText = GameObject.Find("notificationText").GetComponent<TextMeshProUGUI>();

        shopHeaderText = GameObject.Find("shopHeaderText").GetComponent<TextMeshProUGUI>();

        playerSkillsDefenseBar = 
            GameObject.Find("shopSkillsPlayerDefenseSkillsBar").GetComponent<Image>();
        playerSkillsDefenseText =
            GameObject.Find("shopSkillsPlayerSkillsDefenseText").GetComponent<Text>();

        playerSkillsAttackBar =
            GameObject.Find("shopSkillsPlayerAttackSkillsBar").GetComponent<Image>();
        playerSkillsAttackText =
             GameObject.Find("shopSkillsPlayerSkillsAttackText").GetComponent<Text>();

        playerTeamALockedCoins =
            GameObject.Find("shopSkillsTeamALockedCoins").GetComponent<Text>();

        skillsTeamAnameText = 
            GameObject.Find("skillsShopTeamAname").GetComponent<TextMeshProUGUI>();

        skillsTeamAflagRawImage = 
           GameObject.Find("shopSkillsTeamAflagRawImage").GetComponent<RawImage>();

        skillsModel = GameObject.Find("shopSkillsModel");
        skillsModelHair = GameObject.Find("shopSkillsModelHair");
        
        admobCanvas = GameObject.Find("admobCanvas");
        admobCanvas.SetActive(false);

        inputCodeValue = GameObject.Find("promoInputField").GetComponent<InputField>();

        for (int i = 1; i <= COINS_MAX_BUTTONS; i++)
        {            
            buttonCoinsPrice[i-1] = 
                GameObject.Find("buttonCoinsPrice" + i.ToString()).GetComponent<Text>();
        }

        for (int i = 1; i <= DIAMONDS_MAX_BUTTONS; i++)
        {
            //buttonCoinsPrice[i-1] = 
            //    GameObject.Find("buttonCoinsPrice" + i.ToString()).GetComponent<TextMeshProUGUI>();
            buttonDiamondsPrice[i - 1] =
                GameObject.Find("buttonDiamondsPrice" + i.ToString()).GetComponent<Text>();
        }

        for (int i = 1; i <= SKILLS_MAX_BUTTONS; i++)
        {

            skillsButtonGO[i - 1] =
                GameObject.Find("skillsButton" + i.ToString());
            skillsButton[i - 1] =
                skillsButtonGO[i - 1].GetComponent<Button>();
            skillsButtonImage[i-1] = 
                GameObject.Find("skillsButtonImage" + i.ToString()).GetComponent<RawImage>();
            skillsButtonHeaderText[i-1] =
                GameObject.Find("skillsButtonHeaderText" + i.ToString()).GetComponent<TextMeshProUGUI>();           
            skillsButtonPriceText[i-1] =
            GameObject.Find("skillsButtonPriceText" + i.ToString()).GetComponent<Text>();

            skillsButtonPlayerCardImg[i-1] = 
                GameObject.Find("skillsButtonPlayerCardImg" + i.ToString()).GetComponent<RawImage>();

            playerCardSkillsStarGameObj[i-1] =
                 GameObject.Find("playerCardSkillsStar" + i.ToString());
            
            playerCardSkillsText[i-1] = 
                GameObject.Find("playerCardSkillsText" + i.ToString()).GetComponent<TextMeshProUGUI>();
            playerCardSkillsCountryFlag[i-1] =
                 GameObject.Find("playerCardSkillsCountryFlag" + i.ToString()).GetComponent<RawImage>();
            playerCardSkillsDiamondImg[i-1] =
                 GameObject.Find("playerCardSkillsDiamondImg" + i.ToString()).GetComponent<RawImage>(); 
        }


        for (int i = 1; i <= MAIN_MENU_MAX_BUTTONS; i++)
        {
            mainButtonsFocus[i-1] = 
                GameObject.Find("shopMainButtonFocus" + i.ToString());
            mainButtonText[i - 1] =
                GameObject.Find("shopMainButtonText" + i.ToString()).GetComponent<TextMeshProUGUI>();
        }

        leagueNameText = 
            GameObject.Find("shopLeagueNameText").GetComponent<TextMeshProUGUI>();

        initBuyPlayerCardReferences();
        initUnlockedPlayerCardReferences();

        if (Globals.adsEnable)
        {
            admobGameObject = GameObject.Find("admobAdsGameObject");
            admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();
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

    private void updateFlagButtons()
    {
        //print("DBGCLEARVARS TIMEDELTA## " + Time.time + " teamAFlagPrevActive " + teamAFlagPrevActive);

        if (teamAFlagPrevActive != 0)
        {
            //print("DBGCLEARVARS NEXT ACTIVE DIFF " + ((Time.time - teamAFlagPrevLastTimeChanged))  + " Timecurrent " +
            //    Time.time + " lastTimeChange " + teamAFlagPrevLastTimeChanged);

            if ((Time.time - teamAFlagPrevLastTimeChanged) > 0.15f)
            {
                teamAFlagPrevLastTimeChanged = Time.time;
                //print("DBGCLEARVARS SET LAST TIME " + teamAFlagPrevLastTimeChanged);
                teamAFlagPrev();
            }

            if (teamAFlagPrevActive == -1)
                teamAFlagPrevActive = 0;
        }

        if (teamAFlagNextActive != 0)
        {
            //print("DBGCLEARVARS NEXT ACTIVE DIFF " + (Time.time - teamAFlagNextLastTimeChanged));

            if ((Time.time - teamAFlagNextLastTimeChanged) > 0.15f)
            {
                teamAFlagNextLastTimeChanged = Time.time;
                teamAFlagNext();
            }

            if (teamAFlagNextActive == -1)
                teamAFlagNextActive = 0;
        } 
    }

    private void clearFlagButtonsVars()
    {
        //print("#DBGCLEARVARS ");
        teamAFlagPrevActive = 0;
        teamAFlagPrevLastTimeChanged = 0f;

        teamAFlagNextActive = 0;
        teamAFlagNextLastTimeChanged = 0f;
    }

    public void showShopPanel()
    {
        if (admobAdsScript != null)
            admobAdsScript.hideBanner();
        shopCanvas.SetActive(true);
        updateTreasure();
    }

    public void closeShopPanel()
    {
        shopCanvas.SetActive(false);
    }

    public void showCoinsPanel()
    {
        fillCoinsOffersButton();

        disableFocusApartFrom(0);
        if (admobAdsScript != null)
            admobAdsScript.hideBanner();
        showShopPanel();
        coinsPanel.SetActive(true);
        teamSkillsPanel.SetActive(false);
        promoPanel.SetActive(false);
        diamondsPanel.SetActive(false);
        shopHeaderText.text = Languages.getTranslate("Coins");
    }

    public void showDiamondsPanel()
    {
        fillDiamondsOffersButton();

        disableFocusApartFrom(1);
        if (admobAdsScript != null)
            admobAdsScript.hideBanner();
        showShopPanel();
        diamondsPanel.SetActive(true);
        coinsPanel.SetActive(false);
        teamSkillsPanel.SetActive(false);
        promoPanel.SetActive(false);
        shopHeaderText.text = Languages.getTranslate("Diamonds");
    }

    private int convertLeagueNameToIdx(string leagueName)
    {
        switch (leagueName)
        {
            case "WORLD CUP":
            case "AMERICA CUP":
            case "EURO CUP":
                return 0;

            case "BRAZIL":
                return 1;
            case "ENGLAND":
                return 2;
            case "GERMANY":
                return 3;
            case "ITALY":
                return 4;
            case "SPAIN":
                return 5;
            case "CHAMP CUP":
                return 6;                               
        }

        return 0;
    }

    public void showTeamsSkillsPanel()                         
    {
        //fillSkillsOffersButton(nationalTeams.getTeamByIndex(currATeamIdx), 
        //                       leagueNames[leagueNamesIdx]);
          
        leagueNamesIdx = convertLeagueNameToIdx(Globals.leagueName);
        currATeamIdx = Globals.shopTeamIdx;

        //print("#DBGSHOWSKILLS LEAGUENAME " +
        //  Globals.leagueName +
        //  " currTeamIDX " +
        //  Globals.shopTeamIdx + 
        //  " leagueIDX " +
        //  leagueNamesIdx);

        setSkillsTeam(leagueNamesIdx, currATeamIdx);

        string[] team = teams.getTeamByIndex(currATeamIdx);
        fillSkillsOffersButton(team, leagueNames[leagueNamesIdx]);

        disableFocusApartFrom(2);
        if (admobAdsScript != null)
            admobAdsScript.hideBanner();
        showShopPanel();
        coinsPanel.SetActive(false);
        diamondsPanel.SetActive(false);
        promoPanel.SetActive(false);
        teamSkillsPanel.SetActive(true);
        shopHeaderText.text = Languages.getTranslate("Improve your team skills!");
    }

    public void showPromoPanel()
    {
        disableFocusApartFrom(3);
        if (admobAdsScript != null)
            admobAdsScript.hideBanner();
        showShopPanel();
        coinsPanel.SetActive(false);
        teamSkillsPanel.SetActive(false);
        diamondsPanel.SetActive(false);
        promoPanel.SetActive(true);
        shopHeaderText.text = Languages.getTranslate("Promotional codes");
    }

    #region buyProducts   
    public void buyRemoveAds()
    {
        //waitingForBuyProductEvent = "removeAds";
        IAPManager.instance.buyRemoveAds();
    }
    public void buyPromotion()
    {
        //waitingForBuyProductEvent = "removeAds";
        IAPManager.instance.buyEnlargegoalsizeMedium();
    }

    public void buyCoin100()
    {
        IAPManager.instance.buyCoin100();
    }

    public void buyCoin200()
    {
        IAPManager.instance.buyCoin200();
    }

    public void buyCoin500()
    {
        IAPManager.instance.buyCoin500();
    }

    public void buyCoin1000()
    {
        IAPManager.instance.buyCoin1000();
    }

    public void buyCoin2000()
    {
        IAPManager.instance.buyCoin2000();
    }

    public void buyCoin5000()
    {
        IAPManager.instance.buyCoin5000();
    }

    public void buyCoin8000()
    {
        IAPManager.instance.buyCoin8000();
    }

    public void buyCoin10000()
    {
        IAPManager.instance.buyCoin10000();
    }

    public void buyDiamond100()
    {
        IAPManager.instance.buyDiamond100();
    }

    public void buyDiamond200()
    {
        IAPManager.instance.buyDiamond200();
    }

    public void buyDiamond500()
    {
        IAPManager.instance.buyDiamond500();
    }

    public void buyDiamond1000()
    {
        IAPManager.instance.buyDiamond1000();
    }

    public void buyDiamond2000()
    {
        IAPManager.instance.buyDiamond2000();
    }

    public void buyDiamond5000()
    {
        IAPManager.instance.buyDiamond5000();
    }

    public void buyDiamond8000()
    {
        IAPManager.instance.buyDiamond8000();
    }

    public void buyDiamond10000()
    {
        IAPManager.instance.buyDiamond10000();
    }

    //-----------------//
    public void buyProductByName(string name)
    {

        //print("DBGPurchasing BUYPRODUCTBYNAME " + name);
        Globals.purchaseLeagueName = leagueNames[leagueNamesIdx];        
        Globals.purchaseTeamIdx = currATeamIdx;
        IAPManager.instance.buyProductByName(name);
    }

    public void buyPlayerCardForDiamonds(int teamIdx,
                                         string leagueName,
                                         string teamName,
                                         string playerDesc)
                                         
    {
    
        string[] playerItem = playerDesc.Split(':');
        string playerName = playerItem[0];
        string countryName = playerItem[1];
        string defenseSkills = playerItem[2];
        string attackSkills = playerItem[3];
        //string price = playerItem[5];

        string price = Globals.getPlayerCardPrice(playerDesc);

        string playerCardImgName = "playersCard/" + playerItem[6];

        buyPlayerCardNameText.text = playerName;

        string fullTeamPath =
              graphics.getFlagPath("NATIONALS", countryName);
        //graphics.setFlagRawImage(buyPlayerCardCountryFlag, countryName);
        //graphics.setFlagRawImage(buyPlayerCardClubFlag, teamName);
        graphics.setFlagRawImage(buyPlayerCardCountryFlag, fullTeamPath);
        fullTeamPath =
              graphics.getFlagPath(teamName);
        graphics.setFlagRawImage(buyPlayerCardClubFlag, fullTeamPath);

        buyPlayerCardDefenseSkillsBar.fillAmount = float.Parse(defenseSkills) / 100.0f;
        buyPlayerCardAttackSkillsBar.fillAmount = float.Parse(attackSkills) / 100.0f;

        buyPlayerCardSkillsDefenseText.text =
            Languages.getTranslate("Defense: " + defenseSkills,
                                   new List<string>() { defenseSkills });

        buyPlayerCardSkillsAttackText.text =
            Languages.getTranslate("Attack: " + attackSkills,
                                   new List<string>() { attackSkills });

        buyPlayerClubNameText.text = teamName;
        buyPlayerClubDiamondPriceText.text = price;

        buyPlayerCardNotificationText.text =
                Languages.getTranslate("Buy this player's card for " + price + " diamonds",
                                       new List<string>() { price });

        buyPlayerCardPlayerImage.texture = 
            Resources.Load<Texture2D>(playerCardImgName);

        buyPlayerCardYesButton.onClick.RemoveAllListeners();
        buyPlayerCardYesButton.onClick.AddListener(
            delegate { onClickBuyPlayerCard(teamIdx,
                                            leagueName,
                                            teamName, 
                                            playerDesc 
                                            ); });

        buyPlayerCardPanel.SetActive(true);
    }

    public void onClickBuyPlayerCard(int teamIdx,
                                     string leagueName,
                                     string teamName, 
                                     string playerDesc)                                      
    {
        buyPlayerCardPanel.SetActive(false);


        Globals.unlockedPlayerCard(teamIdx,
                                   leagueName,
                                   teamName,
                                   playerDesc);

        showNotification(
            //new PurchaseItem("unlockedplayercardDiamonds",
            new PurchaseItem("unlockedplayercard",
                             teamName,
                             playerDesc,
                             leagueName));

        //use add but with minus 
        Globals.addDiamonds(-int.Parse(playerDesc.Split(':')[5]));
        updateAfterBuying();
    }

    public void onClickClosePlayerCard()
    {
        buyPlayerCardPanel.SetActive(false);
    }

    public void onClickCloseUnlockedPlayerCard()
    {
        unlockedPlayerCardPanel.SetActive(false);
    }

    public void buyProductByName(string productName,
                                 string teamName,                                 
                                 string playerDesc,
                                 string leagueName,
                                 int teamIdx)                                 
    {
        //print("BUYPRODUCTBYNAME " + productName + " teamNAME " + teamName + " " +
        //    " playerDesc " + playerDesc);

        Globals.purchaseTeamName = teamName;
        Globals.purchasePlayerDesc = playerDesc;
        Globals.purchaseLeagueName = leagueName;
        Globals.purchaseTeamIdx = teamIdx;
        IAPManager.instance.buyProductByName(productName);
    }

    public void buyUnlockedteam100()
    {
        IAPManager.instance.buyUnlockedteam100();
    }

    public void buyUnlockedteam200()
    {
        IAPManager.instance.buyUnlockedteam200();
    }

    public void buyUnlockedteam500()
    {
        IAPManager.instance.buyUnlockedteam500();
    }

    public void buyUnlockedteam1000()
    {
        IAPManager.instance.buyUnlockedteam1000();
    }

    public void buyUnlockedteam2000()
    {
        IAPManager.instance.buyUnlockedteam2000();
    }

    public void buyUnlockedteam5000()
    {
        IAPManager.instance.buyUnlockedteam5000();
    }

    public void buyUnlockedteam8000()
    {
        IAPManager.instance.buyUnlockedteam8000();
    }

    public void buyUnlockedteam10000()
    {
        IAPManager.instance.buyUnlockedteam10000();
    }
    
    //---------------------------/
    public void buyAttack1team()
    {
        IAPManager.instance.buyAttack1team();
    }

    public void buyAttack2team()
    {
        IAPManager.instance.buyAttack2team();
    }

    public void buyAttack3team()
    {
        IAPManager.instance.buyAttack3team();
    }

    public void buyAttack4team()
    {
        IAPManager.instance.buyAttack4team();
    }

    public void buyAttack5team()
    {
        IAPManager.instance.buyAttack5team();
    }

    public void buyAttack6team()
    {
        IAPManager.instance.buyAttack6team();
    }

    public void buyAttack7team()
    {
        IAPManager.instance.buyAttack7team();
    }

    public void buyAttack8team()
    {
        IAPManager.instance.buyAttack8team();
    }

    public void buyAttack9team()
    {
        IAPManager.instance.buyAttack9team();
    }

    public void buyAttack10team()
    {
        IAPManager.instance.buyAttack10team();
    }

    //---------------------------/
    public void buyDefense1team()
    {
        IAPManager.instance.buyDefense1team();
    }

    public void buyDefense2team()
    {
        IAPManager.instance.buyDefense2team();
    }

    public void buyDefense3team()
    {
        IAPManager.instance.buyDefense3team();
    }

    public void buyDefense4team()
    {
        IAPManager.instance.buyDefense4team();
    }

    public void buyDefense5team()
    {
        IAPManager.instance.buyDefense5team();
    }

    public void buyDefense6team()
    {
        IAPManager.instance.buyDefense6team();
    }

    public void buyDefense7team()
    {
        IAPManager.instance.buyDefense7team();
    }

    public void buyDefense8team()
    {
        IAPManager.instance.buyDefense8team();
    }

    public void buyDefense9team()
    {
        IAPManager.instance.buyDefense9team();
    }

    public void buyDefense10team()
    {
        IAPManager.instance.buyDefense10team();
    }
    //--------------------------/

    public void buyAttackDefense10team()
    {
        IAPManager.instance.buyAttackDefense10team();
    }
    #endregion
    
    #region promotionCodes
    private void initPromotionCodes()
    {
        promotionCodesDict.Add("Gvk3Mp", "coins_100");
        promotionCodesDict.Add("HZcLVh", "coins_100");
        promotionCodesDict.Add("33EtGc", "coins_100");
        promotionCodesDict.Add("vpDGcc", "coins_100");
        promotionCodesDict.Add("RLGmn1", "coins_100");
        promotionCodesDict.Add("WVTk8m", "coins_100");
        promotionCodesDict.Add("Hol6iS", "coins_100");
        promotionCodesDict.Add("gDXSQx", "coins_100");
        promotionCodesDict.Add("XYgBAW", "coins_100");
        promotionCodesDict.Add("6jgyx5", "coins_100");
        promotionCodesDict.Add("5mptvA", "coins_100");
        promotionCodesDict.Add("09jFxG", "coins_200");
        promotionCodesDict.Add("eCUwT7", "coins_200");
        promotionCodesDict.Add("RY2sSa", "coins_200");  
        promotionCodesDict.Add("5RyFjk", "coins_200");
        promotionCodesDict.Add("CnupbX", "coins_200");
        promotionCodesDict.Add("agftXi", "coins_200");
        promotionCodesDict.Add("C9OHfI", "coins_200");
        promotionCodesDict.Add("IklzzE", "coins_200");
        promotionCodesDict.Add("bxI1VW", "coins_200");
        promotionCodesDict.Add("o1VuvI", "coins_200");
        promotionCodesDict.Add("55QsJ8", "coins_200");
        promotionCodesDict.Add("mINsKP", "coins_500");
        promotionCodesDict.Add("6N9n9N", "coins_500");
        promotionCodesDict.Add("D4lnyU", "coins_500");
        promotionCodesDict.Add("Fici8e", "coins_500");
        promotionCodesDict.Add("U6zUpb", "coins_500");
        promotionCodesDict.Add("cKE6rm", "coins_500");

        promotionCodesDict.Add("sj9NX1", "coins_500");
        promotionCodesDict.Add("tHFzOt", "coins_500");
        promotionCodesDict.Add("rhthj3", "coins_500");
        promotionCodesDict.Add("Htyfz2", "coins_500");
        promotionCodesDict.Add("vKNdtb", "coins_500");
        promotionCodesDict.Add("RQxD37", "coins_1000");
        promotionCodesDict.Add("wdXrc2", "coins_1000");
        promotionCodesDict.Add("fsS7YO", "coins_1000");
        promotionCodesDict.Add("6lks2A", "coins_1000");
        promotionCodesDict.Add("UxZcj8", "coins_1000");
        promotionCodesDict.Add("amdwwM", "coins_1000");
        promotionCodesDict.Add("dsdBe3", "coins_1000");
        promotionCodesDict.Add("7uK0o6", "coins_1000");
        promotionCodesDict.Add("hNh6ot", "coins_1000");
        promotionCodesDict.Add("YiEuDD", "coins_1000");
        promotionCodesDict.Add("jRFeFT", "coins_1000");
        promotionCodesDict.Add("ay7Lxq", "coins_2000");
        promotionCodesDict.Add("mfeSaP", "coins_2000");
        promotionCodesDict.Add("3xUQWK", "coins_2000");
        promotionCodesDict.Add("CjAwMw", "coins_2000");
        promotionCodesDict.Add("P733hq", "coins_2000");
        promotionCodesDict.Add("ptBI3c", "coins_2000");
        promotionCodesDict.Add("09GT6U", "coins_2000");
        promotionCodesDict.Add("fWiR3s", "coins_2000");
        promotionCodesDict.Add("OQN5VJ", "coins_2000");
        promotionCodesDict.Add("UmT6ft", "coins_2000");
        promotionCodesDict.Add("nTKl3z", "coins_5000");
        promotionCodesDict.Add("9fL5YH", "coins_5000");
        promotionCodesDict.Add("yOYgQk", "coins_5000");
        promotionCodesDict.Add("RrUcDt", "coins_5000");
        promotionCodesDict.Add("Xtz4Kb", "coins_5000");
        promotionCodesDict.Add("VnqJC4", "coins_5000");
        promotionCodesDict.Add("Gtydqk", "coins_5000");
        promotionCodesDict.Add("Uzt4XO", "coins_5000");
        promotionCodesDict.Add("gFOPQ0", "coins_5000");
        promotionCodesDict.Add("9kgmN4", "coins_5000");
        promotionCodesDict.Add("3R5aNn", "coins_5000");
        promotionCodesDict.Add("GPJogJ", "coins_8000");
        promotionCodesDict.Add("tGxAmu", "coins_8000");
        promotionCodesDict.Add("eCw1Ap", "coins_8000");
        promotionCodesDict.Add("MAsLzt", "coins_8000");
        promotionCodesDict.Add("L21XUz", "coins_8000");
        promotionCodesDict.Add("MguxOK", "coins_8000");
        promotionCodesDict.Add("ElfVtC", "coins_8000");
        promotionCodesDict.Add("unHbmG", "coins_8000");
        promotionCodesDict.Add("3prqjV", "coins_8000");
        promotionCodesDict.Add("IPQdm5", "coins_8000");
        promotionCodesDict.Add("WeTj0j", "coins_8000");
        promotionCodesDict.Add("ZCVjXO", "coins_10000");
        promotionCodesDict.Add("fmMcSL", "coins_10000");
        promotionCodesDict.Add("HBES7h", "coins_10000");
        promotionCodesDict.Add("9YW4qq", "coins_10000");
        promotionCodesDict.Add("FyALkA", "coins_10000");
        promotionCodesDict.Add("AkVjJs", "coins_10000");
        promotionCodesDict.Add("XvQCSM", "coins_10000");
        promotionCodesDict.Add("2T9Msk", "coins_10000");
        promotionCodesDict.Add("zppIKR", "coins_10000");
        promotionCodesDict.Add("FgarHa", "coins_10000");
        promotionCodesDict.Add("v5vmNt", "coins_10000");
        promotionCodesDict.Add("Gvg3Mp", "diamonds_100");
        promotionCodesDict.Add("HZgLVh", "diamonds_100");
        promotionCodesDict.Add("33gtGc", "diamonds_100");
        promotionCodesDict.Add("vpgGcc", "diamonds_100");
        promotionCodesDict.Add("RLgmn1", "diamonds_100");
        promotionCodesDict.Add("WVgk8m", "diamonds_100");
        promotionCodesDict.Add("Hog6iS", "diamonds_100");
        promotionCodesDict.Add("gDfSQx", "diamonds_100");
        promotionCodesDict.Add("XYaBAW", "diamonds_100");
        promotionCodesDict.Add("6jayx5", "diamonds_100");
        promotionCodesDict.Add("5matvA", "diamonds_100");
        promotionCodesDict.Add("09aFxG", "diamonds_200");
        promotionCodesDict.Add("eCawT7", "diamonds_200");
        promotionCodesDict.Add("RYasSa", "diamonds_200");
        promotionCodesDict.Add("5RaFjk", "diamonds_200");
        promotionCodesDict.Add("CnapbX", "diamonds_200");
        promotionCodesDict.Add("ag6tXi", "diamonds_200");
        promotionCodesDict.Add("C96HfI", "diamonds_200");
        promotionCodesDict.Add("Ik6zzE", "diamonds_200");
        promotionCodesDict.Add("bx61VW", "diamonds_200");
        promotionCodesDict.Add("o1yuvI", "diamonds_200");
        promotionCodesDict.Add("55asJ8", "diamonds_200");
        promotionCodesDict.Add("mIgsKP", "diamonds_500");
        promotionCodesDict.Add("6Ngn9N", "diamonds_500");
        promotionCodesDict.Add("D4gnyU", "diamonds_500");
        promotionCodesDict.Add("Figi8e", "diamonds_500");
        promotionCodesDict.Add("U6gUpb", "diamonds_500");
        promotionCodesDict.Add("cKg6rm", "diamonds_500");
        promotionCodesDict.Add("sjaNX1", "diamonds_500");
        promotionCodesDict.Add("tHazOt", "diamonds_500");
        promotionCodesDict.Add("rhahj3", "diamonds_500");
        promotionCodesDict.Add("Htafz2", "diamonds_500");
        promotionCodesDict.Add("vK5dtb", "diamonds_500");
        promotionCodesDict.Add("qq5j47", "diamonds_1000");
        promotionCodesDict.Add("JA3n2L", "diamonds_1000");
        promotionCodesDict.Add("fs47YO", "diamonds_1000");
        promotionCodesDict.Add("6l3s2A", "diamonds_1000");
        promotionCodesDict.Add("Ux3cj8", "diamonds_1000");
        promotionCodesDict.Add("amawwM", "diamonds_1000");
        promotionCodesDict.Add("ds5Be3", "diamonds_1000");
        promotionCodesDict.Add("7u50o6", "diamonds_1000");
        promotionCodesDict.Add("hNa6ot", "diamonds_1000");
        promotionCodesDict.Add("YiguDD", "diamonds_1000");
        promotionCodesDict.Add("jRgeFT", "diamonds_1000");
        promotionCodesDict.Add("aygLxq", "diamonds_2000");
        promotionCodesDict.Add("mfgSaP", "diamonds_2000");
        promotionCodesDict.Add("3xgQWK", "diamonds_2000");
        promotionCodesDict.Add("CjgwMw", "diamonds_2000");
        promotionCodesDict.Add("P7e3hq", "diamonds_2000");
        promotionCodesDict.Add("ptdI3c", "diamonds_2000");
        promotionCodesDict.Add("09dT6U", "diamonds_2000");
        promotionCodesDict.Add("fWdR3s", "diamonds_2000");
        promotionCodesDict.Add("OQd5VJ", "diamonds_2000");
        promotionCodesDict.Add("Umc6ft", "diamonds_2000");
        promotionCodesDict.Add("nTcl3z", "diamonds_5000");
        promotionCodesDict.Add("9fb5YH", "diamonds_5000");
        promotionCodesDict.Add("yObgQk", "diamonds_5000");
        promotionCodesDict.Add("RracDt", "diamonds_5000");
        promotionCodesDict.Add("Xta4Kb", "diamonds_5000");
        promotionCodesDict.Add("Vn4JC4", "diamonds_5000");
        promotionCodesDict.Add("Gt6dqk", "diamonds_5000");
        promotionCodesDict.Add("Uz64XO", "diamonds_5000");
        promotionCodesDict.Add("gF5PQ0", "diamonds_5000");
        promotionCodesDict.Add("9k4mN4", "diamonds_5000");
        promotionCodesDict.Add("3R3aNn", "diamonds_5000");
        promotionCodesDict.Add("GP3ogJ", "diamonds_8000");
        promotionCodesDict.Add("tG3Amu", "diamonds_8000");
        promotionCodesDict.Add("eC31Ap", "diamonds_8000");
        promotionCodesDict.Add("MAeLzt", "diamonds_8000");
        promotionCodesDict.Add("L2dXUz", "diamonds_8000");
        promotionCodesDict.Add("MgqxOK", "diamonds_8000");
        promotionCodesDict.Add("ElqVtC", "diamonds_8000");
        promotionCodesDict.Add("unqbmG", "diamonds_8000");
        promotionCodesDict.Add("3pqqjV", "diamonds_8000");
        promotionCodesDict.Add("IPqdm5", "diamonds_8000");
        promotionCodesDict.Add("Wetj0j", "diamonds_8000");
        promotionCodesDict.Add("ahghXc", "diamonds_10000");
        promotionCodesDict.Add("fmMgSL", "diamonds_10000");
        promotionCodesDict.Add("HBEg7h", "diamonds_10000");
        promotionCodesDict.Add("9YWgqq", "diamonds_10000");
        promotionCodesDict.Add("FyAgkA", "diamonds_10000");
        promotionCodesDict.Add("AkVaJs", "diamonds_10000");
        promotionCodesDict.Add("XvQdSM", "diamonds_10000");
        promotionCodesDict.Add("2T9hsk", "diamonds_10000");
        promotionCodesDict.Add("zppaKR", "diamonds_10000");
        promotionCodesDict.Add("Fakza3", "diamonds_10000");
        promotionCodesDict.Add("a5cAJt", "diamonds_10000");
        promotionCodesDict.Add("EP8aEC", "remove_ads");
        promotionCodesDict.Add("37f6hN", "remove_ads");
        promotionCodesDict.Add("MLfbQs", "remove_ads");
        promotionCodesDict.Add("lKCGVF", "remove_ads");
        promotionCodesDict.Add("D8vi7g", "remove_ads");
        promotionCodesDict.Add("NBHOs3", "remove_ads");
        promotionCodesDict.Add("2LSCoF", "remove_ads");
        promotionCodesDict.Add("4DggfA", "remove_ads");
        promotionCodesDict.Add("ik2nxg", "remove_ads");
        promotionCodesDict.Add("cFsrbB", "remove_ads");
        promotionCodesDict.Add("bR6UCX", "remove_ads");
        promotionCodesDict.Add("ljUhr5", "remove_ads");
        promotionCodesDict.Add("DbpVnK", "attack10_defense10:Bosnia");
        promotionCodesDict.Add("ApdRLe", "attack10_defense10:Estonia");
        promotionCodesDict.Add("rQDo8v", "attack10_defense10:Georgia");
        promotionCodesDict.Add("M7Ptvy", "attack10_defense10:Iraq");
        promotionCodesDict.Add("5XgvQe", "attack10_defense10:Latvia");
        promotionCodesDict.Add("sTPgfA", "attack10_defense10:Lithuania");
        promotionCodesDict.Add("rgp4xA", "attack10_defense10:Montenegro");
        promotionCodesDict.Add("HpQMbp", "attack10_defense10:Morocco");
        promotionCodesDict.Add("GpUWgQ", "attack10_defense10:Serbia");
        promotionCodesDict.Add("ULdBGu", "attack10_defense10:Slovakia");
        promotionCodesDict.Add("fyMvet", "attack10_defense10:Slovenia");
        promotionCodesDict.Add("v69ky7", "attack10_defense10:Tunisia");
        promotionCodesDict.Add("kGUAUw", "attack10_defense10:Venezuela");
        promotionCodesDict.Add("p2mJp2", "attack10_defense10:Argentina");
        promotionCodesDict.Add("ZHdCJk", "attack10_defense10:Algeria");
        promotionCodesDict.Add("wqhJkW", "attack10_defense10:Brazil");
        promotionCodesDict.Add("BfsbYr", "attack10_defense10:Chile");
        promotionCodesDict.Add("GJx3KR", "attack10_defense10:Colombia");
        promotionCodesDict.Add("XzjJJ7", "attack10_defense10:Austria");
        promotionCodesDict.Add("a9fNcp", "attack10_defense10:Denmark");
        promotionCodesDict.Add("x9efhX", "attack10_defense10:Germany");
        promotionCodesDict.Add("BkCtU3", "attack10_defense10:Greece");
        promotionCodesDict.Add("j2cgtn", "attack10_defense10:France");
        promotionCodesDict.Add("yRnhZv", "attack10_defense10:Finland");
        promotionCodesDict.Add("NA5uNF", "attack10_defense10:Hungary");
        promotionCodesDict.Add("hLeWrW", "attack10_defense10:Iceland");
        promotionCodesDict.Add("ktFom2", "attack10_defense10:Italy");
        promotionCodesDict.Add("Q5ssKm", "attack10_defense10:Netherlands");
        promotionCodesDict.Add("eJCZr4", "attack10_defense10:Poland");
        promotionCodesDict.Add("VE4qyh", "attack10_defense10:Belgium");
        promotionCodesDict.Add("wguaCm", "attack10_defense10:Portugal");
        promotionCodesDict.Add("wodCaY", "attack10_defense10:Croatia");
        promotionCodesDict.Add("4ruVhD", "attack10_defense10:Spain");
        promotionCodesDict.Add("GVNJnD", "attack10_defense10:Uruguay");
        promotionCodesDict.Add("cHJFyG", "attack10_defense10:Switzerland");
        promotionCodesDict.Add("puph5r", "attack10_defense10:Sweden");
        promotionCodesDict.Add("32eF4P", "attack10_defense10:Mexico");
        promotionCodesDict.Add("6wEYSn", "attack10_defense10:Iran");
        promotionCodesDict.Add("4YHg8v", "attack10_defense10:Peru");
        promotionCodesDict.Add("mKCuQB", "attack10_defense10:Senegal");
        promotionCodesDict.Add("BEwqnZ", "attack10_defense10:Ukraine");
        promotionCodesDict.Add("gPrUVU", "attack10_defense10:Romania");
        promotionCodesDict.Add("arReP9", "attack10_defense10:Japan");
        promotionCodesDict.Add("2hx9Y6", "attack10_defense10:USA");
        promotionCodesDict.Add("5Jf8Dw", "attack10_defense10:South");
        promotionCodesDict.Add("9FA5wn", "attack10_defense10:Turkey");
        promotionCodesDict.Add("vcBnxX", "attack10_defense10:Czechia");
        promotionCodesDict.Add("F7NXL4", "attack10_defense10:Australia");
        promotionCodesDict.Add("wcLuKa", "attack10_defense10:Russia");
        promotionCodesDict.Add("YekDLp", "attack10_defense10:Bulgaria");
        promotionCodesDict.Add("zmuzQr", "attack10_defense10:Saudi");
        promotionCodesDict.Add("nK3pxZ", "attack10_defense10:Nigeria");
        promotionCodesDict.Add("LDxZ2H", "attack10_defense10:China");
        promotionCodesDict.Add("tXkDRu", "attack10_defense10:Cameroon");
        promotionCodesDict.Add("ef9Xkm", "attack10_defense10:Costarica");
        promotionCodesDict.Add("Mj6Gjm", "attack10_defense10:Ecuador");
        promotionCodesDict.Add("x6wZUt", "attack10_defense10:Ghana");
        promotionCodesDict.Add("q7wnwX", "attack10_defense10:Honduras");
        promotionCodesDict.Add("vpmetB", "attack10_defense10:Ivory");
        promotionCodesDict.Add("97gHv2", "attack10_defense10:India");
        promotionCodesDict.Add("tHKYPu", "attack10_defense10:Qatar");
        promotionCodesDict.Add("KLYGbk", "attack10_defense10:England");
        promotionCodesDict.Add("z2QwNL", "attack10_defense10:Egypt");
        promotionCodesDict.Add("THADot", "attack10_defense10:Panama");
        promotionCodesDict.Add("EVZn5V", "attack10_defense10:Norway");
        promotionCodesDict.Add("MW4Xnh", "attack10_defense10:Paraguay");
        promotionCodesDict.Add("RwufD5", "attack10_defense10:Jamaica");
        promotionCodesDict.Add("YCwfyJ", "attack10_defense10:Canada");
        promotionCodesDict.Add("rSSrVE", "attack10_defense10:Albania");
        promotionCodesDict.Add("k22JNv", "attack10_defense10:Bolivia");
        promotionCodesDict.Add("62SypE", "attack10_defense10:Belarus");
        promotionCodesDict.Add("fEBamE", "attack10_defense10:Ireland");
        promotionCodesDict.Add("DJGAqF", "attack10_defense10:Israel");
        promotionCodesDict.Add("ht5S7h", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("wd3acw", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("zEoC4P", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("hDufWS", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("Ue33fT", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("KXMegj", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("VcVpLy", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("kLC7N6", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("nwagbx", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("ywb2XR", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("m9fxMV", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("f9xBWJ", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("AFUCd9", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("9Z5HGF", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("EFBg7Z", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("xRycPL", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("xukvwF", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("DxMXzr", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("x8R2gX", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("8n493A", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("zZSyB7", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("Jz2uuX", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("AyUMWT", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("DTsVm9", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("jSUMso", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("MrM6qN", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("ByN3sH", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("qsFJE3", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("4JmV57", "enlargegoalsize_player2:MEDIUM");
        promotionCodesDict.Add("uMqPZg", "enlargegoalsize_player2:MEDIUM");
    }
    #endregion
}
