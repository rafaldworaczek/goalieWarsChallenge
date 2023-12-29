using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GlobalsNS;
using LANGUAGE_NS;

public class dailyBonus : MonoBehaviour
{
    public GameObject dailyBonusCanvas;
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI notificationTextHeader;
    public RawImage notificationImage;
    private TextMeshProUGUI currentCoinsText;
    private TextMeshProUGUI currentDiamondsText;

    void Awake()
    {
        dailyBonusCanvas.SetActive(false);
    }

    void Start()
    {
        int randBonus = UnityEngine.Random.Range(0, 2);
        int coins = 50;
        int diamond = 50;

        if (!checkIfEnoughTimePassed())
            return;

        notificationTextHeader.text = Languages.getTranslate("Daily Bonus");
        if (randBonus == 0)
        {
            notificationText.text =
                Languages.getTranslate("Excellent! +" + coins.ToString() + " coins awarded!",
                                    new List<string>() { coins.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/shownotificationCoins");
            Globals.addCoins(coins);
        }
        else if (randBonus == 1)
        {
            notificationText.text =
                   Languages.getTranslate("Excellent! +" + diamond.ToString() + " diamonds awarded!",
                                          new List<string>() { diamond.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/showNotificationDiamonds");
            Globals.addDiamonds(diamond);
        }

        dailyBonusCanvas.SetActive(true);
        updateGlobalCoinsText();
        updateGlobalDiamondText();

        Globals.dailyBonusShowed = true;
    }

    public void onClickClose()
    {
        dailyBonusCanvas.SetActive(false);  
    }

    private void updateGlobalCoinsText()
    {
        currentCoinsText = GameObject.Find("mainCurrentCoinsText").GetComponent<TextMeshProUGUI>();
        if (currentCoinsText != null ) 
            currentCoinsText.text = Globals.coins.ToString();
    }

    private void updateGlobalDiamondText()
    {
        currentDiamondsText = GameObject.Find("mainCurrentDiamondsText").GetComponent<TextMeshProUGUI>();
        if (currentDiamondsText != null )
            currentDiamondsText.text = Globals.diamonds.ToString();
    }

    private bool checkIfEnoughTimePassed()
    {
        if (Globals.dailyBonusShowed == true ||
            Globals.numGameOpened <= 1 ||
            !PlayerPrefs.HasKey("dailyBonus_epochTime"))            
            return false;

        //Show daily bonus every 8 hours
        int prevTimeSeconds = PlayerPrefs.GetInt("dailyBonus_epochTime");
        int currTimeSeconds = Globals.getEpochTimeInSeconds();

        Debug.Log("#DBGtime prevTimeSeconds " + prevTimeSeconds);
        Debug.Log("#DBGtime currTimeSeconds " + currTimeSeconds);

        //every 8 hours
        if (currTimeSeconds - prevTimeSeconds > 28800)
        {
            PlayerPrefs.SetInt("dailyBonus_epochTime", currTimeSeconds);
            return true;
        }

        return false;
    }


}
