using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GlobalsNS;
using MenuCustomNS;
using System.IO;
using System;
using LANGUAGE_NS;

public class loadMainMenu : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isShowBall = false;
    private RectTransform ballImageTransform;
    private float ballRotationAngle = 1.0f;
    private GameObject adsGameObject;
    //private InternetConnection internetConnectivity;
    private float currentTime = 0;
    public GameObject noInternetCanvas;
    public Button retryButton;
    public GameObject langMenuPanel;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("LANG_SETTING_DONE"))
        {
            langMenuPanel.SetActive(true);
            return;
        }

        langMenuPanel.SetActive(false);
        Languages.initLangs();

        /*internetConnectivity =  
            GameObject.Find("checkInternetConnection").GetComponent<InternetConnection>();

        noInternetCanvas.SetActive(false);
        internetConnectivity.checkInternetConnection("http://google.com");*/

        /*add ads*/
        if(Globals.isAdmobGameObjectCreated == false) {
            GameObject adsGameObject = new GameObject("admobAdsGameObject");
            adsGameObject.AddComponent<admobAdsScript>(); 
            DontDestroyOnLoad(adsGameObject);
            Globals.isAdmobGameObjectCreated = true;
        } 
        
        //else
        //{
        //    GameObject.Find("admobAdsGameObject") = leagueBackMusicDontDestroy;
        //}

        Invoke("showBall", 1.3f);
        Invoke("goToMainMenu", 2f);

        /*void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime > 1.5f)
            
                if (internetConnectivity.getInternetConnectionStatus() != 1)
                {
                    noInternetCanvas.SetActive(true);
                    internetRetry();
                    currentTime = 0;
                }
                else
                {
                    noInternetCanvas.SetActive(false);
                    goToMainMenu();
                }
            }
        }*/

        //public void internetRetry()
        //{
        //retryButton.interactable = false;
        //    internetConnectivity.checkInternetConnection("http://google.com");
        //}



        // Start is called before the first frame update
        //void Awake()
        //{
        //NationalTeams teams = new NationalTeams();
        Teams teams = new Teams("NATIONALS");
        Globals.initTeamLeagueNameHash();

        ballImageTransform = GameObject.Find("ballImage").GetComponent<RectTransform>();
        if (PlayerPrefs.HasKey("numGameOpen"))
        {
            int nGameOpen = PlayerPrefs.GetInt("numGameOpen") + 1;
            Globals.coins = PlayerPrefs.GetInt("coins");

            PlayerPrefs.SetInt("numGameOpen", nGameOpen);
            PlayerPrefs.Save();

            teams.addTeamtoPrefabs(0);           
        }
        else /*Game open first time*/
        {
            /*Add teams with zero coins needed to prefabs*/
            teams.addTeamtoPrefabs(0);

            PlayerPrefs.SetInt("numGameOpen", 1);
            PlayerPrefs.Save();

            //Globals.customizeTeamName = "CUSTOMIZE_TEAM_NOT_CREATED";
            //Globals.customizePlayerName = "CUSTOMIZE_PLAYER_NOT_CREATED";

            Globals.coins = 0;
            PlayerPrefs.SetInt("coins", 0);
            PlayerPrefs.Save();            
        }

        if (!PlayerPrefs.HasKey("ONLINE_USERNAME"))
        {
            PlayerPrefs.SetString("ONLINE_USERNAME", Globals.getRandomStr(8));
            PlayerPrefs.Save();
        }


        if (PlayerPrefs.HasKey("audioMute"))
        {
            int audioMute = PlayerPrefs.GetInt("audioMute");
            if (audioMute == 0)
            {
                Globals.audioMute = false;
                AudioListener.volume = 1f;
            }
            else
            {
                Globals.audioMute = true;
                AudioListener.volume = 0f;
            }
        } else
        {
            PlayerPrefs.SetInt("audioMute", 0);
            PlayerPrefs.Save();
            Globals.audioMute = false;
        }

        if (PlayerPrefs.HasKey("diamonds"))
        {
            Globals.diamonds = 
                PlayerPrefs.GetInt("diamonds");
        }
        else
        {
            Globals.diamonds = 2000;
            PlayerPrefs.SetInt("diamonds", Globals.diamonds);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("numSelectedPlayerCardHelper"))
        {
            Globals.numSelectedPlayerCardHelperShow =
                PlayerPrefs.GetInt("numSelectedPlayerCardHelper");
        } else
        {
            PlayerPrefs.SetInt("numSelectedPlayerCardHelperShow", 0);
            PlayerPrefs.Save();

            Globals.numSelectedPlayerCardHelperShow = 0;
        }
      
        if (PlayerPrefs.HasKey("cupsWon"))
        {
            Globals.cupsWon =
                PlayerPrefs.GetInt("cupsWon");
        }
        else
        {
            Globals.cupsWon = 0;
            PlayerPrefs.SetInt("cupsWon", Globals.cupsWon);
            PlayerPrefs.Save();
        }

        Globals.numGameOpened = PlayerPrefs.GetInt("numGameOpen");
        if (!PlayerPrefs.HasKey("enlargeGoal_MEDIUM_CREDITS"))
            {
            PlayerPrefs.SetInt("enlargeGoal_MEDIUM_CREDITS", 3);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("CustomizeTeam_TeamName"))
            Globals.customizeTeamName =
                PlayerPrefs.GetString("CustomizeTeam_TeamName");
        else
        {
            Globals.customizeTeamName = "Your team";
            PlayerPrefs.SetString("CustomizeTeam_TeamName", "Your team");
        }

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerDesc"))
        {
            Globals.customizePlayerDesc = 
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerDesc");
        }

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerCardName"))
        {
            PlayerPrefs.SetInt("PLAYER_NAME_SET", 1);
            PlayerPrefs.Save();

            Globals.customizePlayerCardName =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerCardName");
        } 

        if (PlayerPrefs.HasKey("CustomizeTeam_PlayerName"))
            Globals.customizePlayerName =
                PlayerPrefs.GetString("CustomizeTeam_PlayerName");
        else
            Globals.customizePlayerName = "Player name";

        if (PlayerPrefs.HasKey("customizePlayerNationality"))
            Globals.customizePlayerNationality =
                PlayerPrefs.GetString("customizePlayerNationality");
        else
            Globals.customizePlayerNationality = "afghanistan";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerShirt"))
            Globals.customizePlayerShirt =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerShirt");
        else
            Globals.customizePlayerShirt = "blackwithwhiteshirt";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerShorts"))
            Globals.customizePlayerShorts =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerShorts");
        else
            Globals.customizePlayerShorts = "whitegoldstripesshort";


        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerSock"))
            Globals.customizePlayerSocks =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerSock");
        else
            Globals.customizePlayerSocks = "sock_white";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerGloves"))
            Globals.customizePlayerGloves =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerGloves");
        else
            Globals.customizePlayerGloves = "globewhiteblack";


        if (PlayerPrefs.HasKey("CustomizeTeam_customizeFansColor"))
            Globals.customizeFansColor =
                PlayerPrefs.GetString("CustomizeTeam_customizeFansColor");
        else
            Globals.customizeFansColor = "red";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizeFlagColor"))
            Globals.customizeFlagColor =
                PlayerPrefs.GetString("CustomizeTeam_customizeFlagColor");
        else
            Globals.customizeFlagColor = "red";


        Globals.logoFilePath =
            Application.persistentDataPath + "/logoFile.png";
        //print("#DBG134FANS " + Globals.customizeFansColor);
        //print("#DBG134FLAG " + Globals.customizeFlagColor);


        //if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerHair"))
        //    Globals.customizePlayerHair =
        //        PlayerPrefs.GetString("CustomizeTeam_customizePlayerHair");
        //else
        //    Globals.customizePlayerHair = "hblack4";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerSkinHair"))
            Globals.customizePlayerSkinHair =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerSkinHair");
        else
            Globals.customizePlayerSkinHair = "f0_s1_b0_t0_hblack1";

        if (PlayerPrefs.HasKey("CustomizeTeam_customizePlayerShoe"))
            Globals.customizePlayerShoe =
                PlayerPrefs.GetString("CustomizeTeam_customizePlayerShoe");
        else
            Globals.customizePlayerShoe = "shoewhite";


        //if (!PlayerPrefs.HasKey("CustomizeTeam"))
        //{
            PlayerPrefs.SetInt("CustomizeTeamItem_shoewhite", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_shoeblackred", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_shoeblack", 1);

        
            PlayerPrefs.SetInt("CustomizeTeamItem_globewhiteblack", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_glovebluedarkblue", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_gloveredwhite", 1);

            PlayerPrefs.SetInt("CustomizeTeamItem_f0_s1_b0_t0_hblack1", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_f0_s1_b0_t0_hblack13", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_f4_s4_b2_t10_hblack5", 1);
        
            PlayerPrefs.SetInt("CustomizeTeamItem_blackwithwhiteshirt", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_redblueshirt1", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_redwitwhiteshirt", 1);

            PlayerPrefs.SetInt("CustomizeTeamItem_whitegoldstripesshort", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_darkredshort", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_yellowredstripesshort", 1);


            PlayerPrefs.SetInt("CustomizeTeamItem_sock_white", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_whitegreysocks", 1);
            PlayerPrefs.SetInt("CustomizeTeamItem_redwhitestripesocks1", 1);


         if (!PlayerPrefs.HasKey("POWERS_UNLOCKED"))
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

         PlayerPrefs.Save();
       //}



       /*    PlayerPrefs.SetInt("CustomizeTeamItem_darkredshort", 1);
           PlayerPrefs.SetInt("CustomizeTeamItem_yellowredstripesshort", 1);
           PlayerPrefs.Save();*/

        /*if (!PlayerPrefs.HasKey("userID"))
        {
            PlayerPrefs.SetInt("userID", System.Guid.NewGuid());
            PlayerPrefs.Save();
        }

        int userID = PlayerPrefs.GetInt("userID");
        print("USERID " + userID);
        AnalyticsResult analyticsResult = Analytics.CustomEvent("Game_Started", new Dictionary<string, object>
        {
            { userID, true},           
        });*/

        //print("numGameOpened " + Globals.numGameOpened);
    }

    private void showBall()
    {
        Image ballImage = GameObject.Find("ballImage").GetComponent<Image>();
        ballImage.enabled = true;
        /*Image osystemsImage = GameObject.Find("osystemsImage").GetComponent<Image>();
        osystemsImage.enabled = true;*/

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        isShowBall = true;
    }

    private void goToMainMenu()
    {
        //print("#DBGIDX SceneManager.GetActiveScene().buildIndex " +
        //    SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("menu");
    }
}
