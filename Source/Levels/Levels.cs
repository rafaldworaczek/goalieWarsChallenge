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

    void Awake()
    {
        levelNumberText.text = Globals.levelNumber;
        Time.timeScale = 1f;
        Globals.isMultiplayer = false;
        Globals.isBonusActive = false;
        Globals.isTrainingActive = false;
        Globals.isLevelMode = true;

        if (Globals.PITCHTYPE.Equals("STREET"))
            Globals.commentatorStr = "NO";

        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        #if !UNITY_EDITOR
        adInit();
        showInterstitialAd();
        #endif

        updateTreasure();

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
