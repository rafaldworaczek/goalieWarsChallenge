using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using LANGUAGE_NS;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Com.Osystems.GoalieStrikerFootball;
using Photon.Pun;
using System.Globalization;

public class MultiplayerMenu : MonoBehaviour
{
    private int COINS_NEEDED_TO_PLAY = 10;
    private int MAX_PLAYERS_CARDS = 2;
    public GameObject chooseTeamCanvas;
    private LeagueBackgroundMusic leagueBackgroundMusic;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    public GameObject admobCanvas;
    private bool waitingForInterstitialAddEvent = false;
    public GameObject blockingCanvas;
    public GameObject betaVersionCanvas;    
    public TextMeshProUGUI currentCoinsText;
    public TextMeshProUGUI currentDiamondsText;
    public GameObject shopCanvas;
    public GameObject infoCanvas;
    public TextMeshProUGUI infoHeaderText;
    public TextMeshProUGUI infoDescText;
    public RawImage infoImage;
    public RawImage[] buttonsBottom;
    public Image[] focusBottom;
    public Button[] bottomButtons;
    public Launcher launcher;
    public GameObject invitePartyPanel;

    void Awake()
    {
        /*Vector3 ballPos = new Vector3(0.2f, 0.3f, 0.5f);
        string currBallPosStr =
        ballPos.x.ToString("F3", CultureInfo.InvariantCulture);
        Debug.Log("parse " + float.Parse(currBallPosStr, CultureInfo.InvariantCulture)
            + " currBallPosStr " + currBallPosStr);*/

       
        Time.timeScale = 1f;
        Globals.isMultiplayer = true;
        Globals.savedFileName = Globals.multiplayerSaveName;
        Globals.isBonusActive = false;
        Globals.isTrainingActive = false;
        invitePartyPanel.SetActive(false);
        if (Globals.PITCHTYPE.Equals("STREET"))
            Globals.commentatorStr = "NO";

        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        adInit();
        showInterstitialAd();

        updateTreasure();
        //#if !UNITY_EDITOR
        if (Globals.coins < COINS_NEEDED_TO_PLAY)
        {
            infoCanvas.SetActive(true);
            infoHeaderText.text = "Oops..";
            infoDescText.text = 
                Languages.getTranslate("Sorry. You must have at least 10 coins to play online. Play Friendly, Season or Tournament first");
            infoImage.texture = Resources.Load<Texture2D>("Shop/shopNotificationCoins");
            return;
        }
        //#endif

        if (!Globals.showBetaVersionOfMultiplayer)
        {
            betaVersionCanvas.SetActive(true);
            Globals.showBetaVersionOfMultiplayer = true;
        } else
        {
            betaVersionCanvas.SetActive(false);
        }

        //TODELETE
        //Languages.initLangs();
        //TODELETE
        //Globals.initTeamLeagueNameHash();
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

    public void onClickShowChooseTeamMenu()
    {
        chooseTeamCanvas.SetActive(true);
    }

    public void onClickCloseChooseTeamMenu()
    {
        chooseTeamCanvas.SetActive(false);
    }

    public void onClickChooseTeamSave()
    {
        chooseTeamCanvas.SetActive(false);
    }

    public void onClickTeamManagementMenu()
    {
        if (Globals.isPlayerCardLeague(Globals.leagueName))
        {
            Globals.isGameSettingActive = true;
            Globals.loadSceneWithBarLoader("teamManagement");
        }
    }

    public void onClickMainMenu()
    {
        Globals.loadSceneWithBarLoader("menu");
    }
  
    public void onClickExit()
    {
        PhotonNetwork.LeaveRoom();
        Globals.loadSceneWithBarLoader("menu");
    }

    private void initAudioClip()
    {
        leagueBackgroundMusic =
            GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
        leagueBackgroundMusic.play();
    }

    public void onClickCloseBetaCanvas()
    {
        betaVersionCanvas.SetActive(false);
    }

    public void onClickCloseShopCanvas()
    {
        updateTreasure();
        shopCanvas.SetActive(false);
    }

    public void setBottomButtonsAlpha(int idx, float alpha)
    {
        Color tempColor;
  
            tempColor = buttonsBottom[idx].color;
            tempColor.a = alpha;
            buttonsBottom[idx].color = tempColor;

            tempColor = focusBottom[idx].color;
            tempColor.a = alpha;
            focusBottom[idx].color = tempColor;
    }

    public void setBottomButtonsAlpha(float alpha)
    {
        Color tempColor;
        for (int i = 0; i < buttonsBottom.Length; i++)
        {
            tempColor = buttonsBottom[i].color;
            tempColor.a = alpha;
            buttonsBottom[i].color = tempColor;

            //workaround to last button invite party
            if (i != (buttonsBottom.Length - 1))
            {
                tempColor = focusBottom[i].color;
                tempColor.a = alpha;
                focusBottom[i].color = tempColor;
            }
        }
    }

    public void setBottomButtonInteractable(int idx, bool val, float alpha)
    {        
        bottomButtons[idx].interactable = val;
        setBottomButtonsAlpha(idx, alpha);
    }

    public void setBottomButtonInteractable(bool val)
    {
        for (int i = 0; i < bottomButtons.Length; i++)
        {
            bottomButtons[i].interactable = val;
        }
    }

    public void onClickPlay()
    {
        setBottomButtonInteractable(false);
        setBottomButtonsAlpha(0.33f);
        launcher.Connect("random", null);
    }

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
}
