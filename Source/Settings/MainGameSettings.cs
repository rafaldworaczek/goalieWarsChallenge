using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GlobalsNS;
using LANGUAGE_NS;

public class MainGameSettings : MonoBehaviour
{
    public GameObject settingsPanel;
    private string savedFileName;
    private int gameTimesIdx = 1;
    private int trainingModeIdx = 0;
    private int joystickSideIdx = 0;
    private int graphicsSettingsIdx = 2;
    private int levelsIdx = 2;

    private int maxTimeToShotIdx = 0;
    private int powersIdx = 0;
    private int commentatorIdx = 0;

    public TextMeshProUGUI time;
    public TextMeshProUGUI level;
    public TextMeshProUGUI graphicsText;
    public TextMeshProUGUI trainingMode;
    public TextMeshProUGUI joystickSideText;

    public TextMeshProUGUI maxTimeToShotText;
    public TextMeshProUGUI powersText;
    public TextMeshProUGUI commentatorText;

    private string[] levels = { "KID", "EASY", "NORMAL", "HARD", "EXPERT" };
    private string[] gameTimes = { "30 SECONDS", "1 MINUTE", "2 MINUTES", "3 MINUTES", "4 MINUTES", "5 MINUTES" };
    private string[] trainingModes = { "NO ", "YES" };
    private string[] graphics = { "VERY LOW", "LOW", "STANDARD", "HIGH", "VERY HIGH" };
    private string[] joystickSide = { "LEFT", "RIGHT" };
    private string[] maxTimeToShot = { "8 SECONDS", "10 SECONDS", "15 SECONDS", "20 SECONDS" };
    private string[] powers = { "YES", "NO" };
    private string[] commentator = { "YES", "NO" };

    void Start()
    {
        savedFileName = Globals.savedFileName;

        recoverPrefabGameSettings();
        setupDefaultSettings();
        settingsPanel.SetActive(false);
    }

    //void Update()
    //{

    //}

    private void setupDefaultSettings()
    {
        graphicsText.text = Languages.getTranslate(graphics[graphicsSettingsIdx]);
        time.text = Languages.getTranslate(gameTimes[gameTimesIdx]);
        trainingMode.text = Languages.getTranslate(trainingModes[trainingModeIdx]);
        joystickSideText.text = Languages.getTranslate(joystickSide[joystickSideIdx]);
        level.text = Languages.getTranslate(levels[levelsIdx]);
        maxTimeToShotText.text = Languages.getTranslate(maxTimeToShot[maxTimeToShotIdx]);
        powersText.text = Languages.getTranslate(powers[powersIdx]);
        commentatorText.text = Languages.getTranslate(commentator[commentatorIdx]);
    }

    private void recoverPrefabGameSettings()
    {
        //if (PlayerPrefs.HasKey(savedFileName + "_levelsIdx"))
            levelsIdx =
                PlayerPrefs.GetInt(savedFileName + "_levelsIdx");

        //if (PlayerPrefs.HasKey(savedFileName + "_gameSettingsSave"))
        //{
            //Level cannot be change after starting a game
            //levelsIdx = PlayerPrefs.GetInt(savedFileName + "_levelsIdx");
            gameTimesIdx = PlayerPrefs.GetInt(savedFileName + "_gameTimesIdx");
            //trainingModeIdx = PlayerPrefs.GetInt(savedFileName + "_trainingModeIdx");
            trainingModeIdx = 0;
            graphicsSettingsIdx = PlayerPrefs.GetInt(savedFileName + "_graphicsSettingsIdx");
            joystickSideIdx = PlayerPrefs.GetInt(savedFileName + "_joystickSideIdx");

            powersIdx = PlayerPrefs.GetInt(savedFileName + "_powersIdx");
            commentatorIdx = PlayerPrefs.GetInt(savedFileName + "_commentatorIdx");
            maxTimeToShotIdx = PlayerPrefs.GetInt(savedFileName + "_maxTimeToShotIdx");

            saveSettingsToGlobals();
    }

    private void saveSettingsToGlobals()
    {
        Globals.level = levelsIdx + 1;
        Globals.matchTime = gameTimes[gameTimesIdx];
        Globals.graphicsQuality = graphics[graphicsSettingsIdx];
        Globals.isTrainingActive = true;
        Globals.joystickSide = joystickSide[joystickSideIdx];
        Globals.powersStr = powers[powersIdx];
        Globals.maxTimeToShotStr = maxTimeToShot[maxTimeToShotIdx];
        Globals.commentatorStr = commentator[commentatorIdx];

        if (trainingModeIdx == 0)
        {
            Globals.isTrainingActive = false;
        }
    }

    public void saveGlobalsSettingsToPrefab()
    {
        PlayerPrefs.SetInt(savedFileName + "_gameSettingsSave", 1);
        PlayerPrefs.SetInt(savedFileName + "_gameTimesIdx", gameTimesIdx);
        //PlayerPrefs.SetInt("levelsIdx", levelsIdx);
        PlayerPrefs.SetInt(savedFileName + "_trainingModeIdx", trainingModeIdx);
        PlayerPrefs.SetInt(savedFileName + "_graphicsSettingsIdx", graphicsSettingsIdx);
        PlayerPrefs.SetInt(savedFileName + "_joystickSideIdx", joystickSideIdx);
        PlayerPrefs.SetInt(savedFileName + "_powersIdx", powersIdx);
        PlayerPrefs.SetInt(savedFileName + "_commentatorIdx", commentatorIdx);
        PlayerPrefs.SetInt(savedFileName + "_maxTimeToShotIdx", maxTimeToShotIdx);

        PlayerPrefs.Save();

        saveSettingsToGlobals();
    }

    public void saveSettings()
    {
        saveGlobalsSettingsToPrefab();
        settingsPanel.SetActive(false);
    }

    public void prevGraphics()
    {
        if (graphicsSettingsIdx > 0)
        {
            graphicsText.text =
                Languages.getTranslate(graphics[--graphicsSettingsIdx]);
        }
    }

    public void nextGraphics()
    {
        if (graphicsSettingsIdx < (graphics.Length - 1))
        {
            graphicsText.text =
                Languages.getTranslate(graphics[++graphicsSettingsIdx]);
        }
    }

    public void prevTime()
    {
        if (gameTimesIdx > 0)
        {
            time.text = Languages.getTranslate(gameTimes[--gameTimesIdx]);
        }
    }

    public void nextTime()
    {
        if (gameTimesIdx < (gameTimes.Length - 1))
        {
            time.text = Languages.getTranslate(gameTimes[++gameTimesIdx]);
        }
    }

    public void prevTrainingMode()
    {
        if (trainingModeIdx > 0)
        {
            trainingMode.text = 
                Languages.getTranslate(trainingModes[--trainingModeIdx]);
        }

        if (trainingModeIdx == 1)
            Globals.isTrainingActive = true;
        else
            Globals.isTrainingActive = false;
    }

    public void nextTrainingMode()
    {
        if (trainingModeIdx < (trainingModes.Length - 1))
        {
            trainingMode.text = 
                Languages.getTranslate(trainingModes[++trainingModeIdx]);
        }

        if (trainingModeIdx == 1)
            Globals.isTrainingActive = true;
        else
            Globals.isTrainingActive = false;
    }

    public void prevJoystickSide()
    {
        if (joystickSideIdx > 0)
        {
            joystickSideText.text = 
                Languages.getTranslate(joystickSide[--joystickSideIdx]);
        }
    }

    public void nextJoystickSide()
    {
        if (joystickSideIdx < (joystickSide.Length - 1))
        {
            joystickSideText.text = 
                Languages.getTranslate(joystickSide[++joystickSideIdx]);
        }
    }

    public void prevMaxShotTime()
    {
        if (maxTimeToShotIdx > 0)
        {
            maxTimeToShotText.text = Languages.getTranslate(maxTimeToShot[--maxTimeToShotIdx]);
        }
    }

    public void NextMaxShotTime()
    {
        if (maxTimeToShotIdx < (maxTimeToShot.Length - 1))
        {
            maxTimeToShotText.text = 
                Languages.getTranslate(maxTimeToShot[++maxTimeToShotIdx]);
        }
    }

    public void prevPowers()
    {
        if (powersIdx > 0)
        {
            powersText.text = 
                Languages.getTranslate(powers[--powersIdx]);
        }
    }

    public void nextPowers()
    {
        if (powersIdx < (powers.Length - 1))
        {
            powersText.text = Languages.getTranslate(powers[++powersIdx]);
        }
    }

    public void prevCommentator()
    {
        if (commentatorIdx > 0)
        {
            commentatorText.text = Languages.getTranslate(commentator[--commentatorIdx]);
        }
    }

    public void nextCommentator()
    {
        if (commentatorIdx < (commentator.Length - 1))
        {
            commentatorText.text = Languages.getTranslate(commentator[++commentatorIdx]);
        }
    }

    public void onClickOpenPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void onClickClosePanel()
    {
        saveSettingsToGlobals();
        settingsPanel.SetActive(false);
    }
}
