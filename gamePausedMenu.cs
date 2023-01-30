using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GlobalsNS;
using Photon.Pun;

public class gamePausedMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject pausePanel;
    public GameObject pauseMainMenuPanel;
    private bool gamePaused = false;
    private bool audioEnabled = true;
    //public RawImage pausePanelAudioImage;
    // Start is called before the first frame update

    void Start()
    {
        //pauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.isMultiplayer)
        {
            return;
        }

        /*PLATFORMDEPENDENT*/
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
                //print("PAUSE GAME EXECUTED");
            }                        
        }   
    }

    public void pauseGame()
    {
        gamePaused = true;
        pauseCanvas.SetActive(true);
        pausePanel.SetActive(true);
        pauseMainMenuPanel.SetActive(false);
        Time.timeScale = 0f;
        AudioListener.volume = 0f;
    }

    public void ResumeButton()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        if (Globals.audioMute)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = 1f;
        gamePaused = false;
    }

    public void MainMenuExit()
    {
        pauseMainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void MainMenuButtonYes()
    {
        Time.timeScale = 1f;
        //AudioListener.volume = 1f;
        Globals.recoverOriginalResolution();
        if (Globals.isMultiplayer)
        {
            //PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("multiplayerMenu");
        }
        else
            SceneManager.LoadScene("menu");
    }

    public void MainMenuButtonNo()
    {
        pauseMainMenuPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    /*not used for the time being*/
    /*public void audioButton()
    {
        audioEnabled = !audioEnabled;
        if (audioEnabled)
        {
            pausePanelAudioImage.texture =
               Resources.Load<Texture2D>("others/audioON");
            AudioListener.volume = 1f;
        } else
        {
            pausePanelAudioImage.texture =
                Resources.Load<Texture2D>("others/audioOFF");
            AudioListener.volume = 0f;
        }
    }*/

    void OnApplicationPause(bool pauseStatus)
    {
        //print("ONAPPLCATIONPAUSE EXECUTED " + pauseStatus);
        if (pauseStatus)
            pauseGame();    
    }

    public bool isGamePaused()
    {
        return gamePaused;
    }

    public bool isAudioEnabled()
    {        return audioEnabled;

    }
}
