using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using GlobalsNS;
using LANGUAGE_NS;
using AudioManagerMultiNS;

public class MultiplayerErrorsInfo : MonoBehaviour
{
    public GameObject infoCanvas;
    public TextMeshProUGUI infoHeaderText;
    public TextMeshProUGUI infoDescText;
    public RawImage infoImage;
    private playerControllerMultiplayer playerMainScript;
    private AudioManager audioManager;
    private bool endOfTheGame = false;
    private float sceneLoadStartTime;
    private float maxTimeWithoutMesage = 1.5f;
    private float startScaleTime = 0f;
    private bool coinsUpdated = false;

    void Start()   
    {
        if (!Globals.isMultiplayerMatchNotFound)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        
        sceneLoadStartTime = Time.time;
        playerMainScript = Globals.player1MainScript;
        infoImage.texture = Resources.Load<Texture2D>("error/error");
        infoCanvas.SetActive(false);
        startScaleTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.isMultiplayer &&
            Globals.isMultiplayerMatchNotFound &&
            !PhotonNetwork.IsConnected)
        {
            infoHeaderText.text = "Oops..";
            infoDescText.text =
                Languages.getTranslate("Sorry. Cannot connect to server. Please check your Internet connection");
            Time.timeScale = 0f;
            PhotonNetwork.AutomaticallySyncScene = false;
            Globals.commentatorStr = "NO";
            //audioManager.Play("music2");
            infoCanvas.SetActive(true);

            return;
        }

        if ((playerMainScript == null) ||
            playerMainScript.doesGameEnded())
            return;

        if (Time.timeScale > 0f)
        {
            startScaleTime = Time.time;
        }

        if (!PhotonNetwork.IsConnected)
        {
            infoHeaderText.text = "Oops..";
            infoDescText.text =
                Languages.getTranslate("Sorry. Cannot connect to server. Please check your Internet connection");
            endOfTheGame = true;
        }

        if ((playerMainScript.getMatchTimeMinute() >= 88) ||
            (playerMainScript.getMatchTimeMinute() <= 5))
        {
            maxTimeWithoutMesage = 4.5f;
        }

        ///print("#DBG1024 Time.time - playerMainScript.getLastTimeUpdate() " +
        ///    (Time.time - playerMainScript.getLastTimeUpdate()));

        if (((Time.time - startScaleTime) > 1f) ||
            (playerMainScript.doesGameStarted() && 
             !playerMainScript.arePeersPlayerSet() &&
             (playerMainScript.getMatchTimeMinute() < 88) &&
             (playerMainScript.getMatchTimeMinute() > 5)) ||            
            (!playerMainScript.doesGameStarted() && (Time.time - sceneLoadStartTime > 35f)) ||
            (playerMainScript.doesGameStarted() && 
            ((Time.time - playerMainScript.getLastTimeUpdate()) > maxTimeWithoutMesage)))
        {
            if ((Time.time - sceneLoadStartTime) > 35f)
                Time.timeScale = 0f;
            PhotonNetwork.AutomaticallySyncScene = false;
            infoHeaderText.text = "Oops..";
            infoDescText.text =
                Languages.getTranslate("Your opponent left the game");
            endOfTheGame = true;
        }

        if (endOfTheGame)
        {
            Time.timeScale = 0f;
            PhotonNetwork.AutomaticallySyncScene = false;
            Globals.commentatorStr = "NO";
            audioManager.Play("music2");
            //if you are connected that means that you peer disconnected
            if (PhotonNetwork.IsConnected && !coinsUpdated)
            {
                playerMainScript.addCoins();
                coinsUpdated = true;
            }
            infoCanvas.SetActive(true);
        }
    }

    public void matchEndedOnClick()
    {
 
        Globals.recoverOriginalResolution();
      
        Globals.dontCheckOnlineUpdate = false;
        Globals.loadSceneWithBarLoader("multiplayerMenu");
        //SceneManager.LoadScene("multiplayerMenu");
        return;

        /*if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return;
        }

        if (teams.isAnyNewTeamUnclocked())
        {
            SceneManager.LoadScene("rewardNewTeam");
        }
        else
        {
            if (Globals.isFriendly)
            {
                SceneManager.LoadScene("menu");
            }
            else
            {
                //SceneManager.LoadScene("Leagues");               
                Globals.loadSceneWithBarLoader("Leagues");
            }
        }*/
    }
}
