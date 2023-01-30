using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LANGUAGE_NS;
using TMPro;

public class GameFirstTimeStart : MonoBehaviour
{
    public GameObject namePanel;
    public GameObject oopsPanel;
    public InputField inputPlayerName;
    public TextMeshProUGUI oopsPanelInfoText;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("PLAYER_NAME_SET") &&
            (Globals.numGameOpened == 1))           
        {
            namePanel.SetActive(false);
            //namePanel.SetActive(true);
            PlayerPrefs.SetInt("PLAYER_NAME_SET", 1);
            PlayerPrefs.Save();

            Globals.loadSceneWithBarLoader("customize");

            return;
        }
        else
        {
            namePanel.SetActive(false);        
        }
    }

    /*
    void Update()
    {
        
    }
    */

    public void onClickOkButton()
    {
        string inputText = inputPlayerName.text;
        string teamName = PlayerPrefs.GetString("CustomizeTeam_TeamName");

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
       
        Globals.customizePlayerName = inputText;

        PlayerPrefs.SetInt("PLAYER_NAME_SET", 1);
        PlayerPrefs.Save();

        Globals.loadSceneWithBarLoader("customize");

    }
}
