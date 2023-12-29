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
    public GameObject showPromotion;
    private bool isShowPromotion = false;
    private TextMeshProUGUI currentCoinsText;
    private TextMeshProUGUI currentDiamondsText;

    void Awake()
    {
        bool isShowPromotion = showPromotion.activeSelf;
        dailyBonusCanvas.SetActive(false);
        showPromotion.SetActive(false);
    }

    void Start()
    {
        int randBonus = UnityEngine.Random.Range(0, 2);
        int coins = 50;
        int diamond = 50;

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
    }

    public void onClickClose()
    {
        dailyBonusCanvas.SetActive(false);
        if (isShowPromotion)
            showPromotion.SetActive(true);
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
}
