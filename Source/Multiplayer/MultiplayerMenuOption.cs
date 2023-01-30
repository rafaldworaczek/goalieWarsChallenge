using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GlobalsNS;
using UnityEngine.SceneManagement;
using MenuCustomNS;

public class MultiplayerMenuOption : MonoBehaviour
{
    public GameObject waitingForOponnentPanel;
    private playerControllerMultiplayer playerMainScript;


    // Start is called before the first frame update

    void Start()
    {
        playerMainScript = Globals.player1MainScript;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMainScript.doesGameEnded())
        {
            waitingForOponnentPanel.SetActive(false);
            return;
        }
        
        if (!playerMainScript.arePeersPlayerSet())
        {
            waitingForOponnentPanel.SetActive(true);
        } else
        {
            waitingForOponnentPanel.SetActive(false);
        }
    }

    public void matchEndedOnClick()
    {
        //NationalTeams teams = new NationalTeams();
        //Teams teams = new Teams("NATIONALS");
        Teams teams = new Teams("NATIONALS");

        Globals.recoverOriginalResolution();

        if (Globals.onlyTrainingActive)
        {
            SceneManager.LoadScene("menu");
            return;
        }

        if (teams.isAnyNewTeamUnclocked() ||
            ((Globals.numMatchesInThisSession % 4) == 0) ||
            (Globals.numMainMenuOpened == 1 &&
             Globals.numMatchesInThisSession <= 3))
        {
            SceneManager.LoadScene("rewardNewTeam");
            return;
        }

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
