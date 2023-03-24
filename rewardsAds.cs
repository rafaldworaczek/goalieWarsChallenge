using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using MenuCustomNS;
using GlobalsNS;
using graphicsCommonNS;
using TMPro;
using UnityEngine.Analytics;
using LANGUAGE_NS;
using GameBench;
using AudioManagerNS;

public class rewardsAds : MonoBehaviour
{
    private Button rewardNextCanvasButton;
    private int currentIdx = 0;
    public GameObject rewardAdsTeamCanvas;
    public GameObject treasureButton;
    public GameObject rewardAdsNextCanvasButtonGameObj;
    public GameObject rewardsAdsPanel;
    public GameObject coinsAwardedPanel;
    public GameObject rewardFailedPanel;
    public GameObject admobCanvas;
    public GameObject shopPanel;

    private GraphicsCommon graphics;
    private AudioManager audioManager;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    private bool waitingForAddEvent = false;
    private bool waitingForInterstitialAddEvent = false;
    private int coinsNumber = Globals.rewardAdDefaultCoins;
    public Text coinsNumRewardText;
    public Text coinsNumAwardedText;

    //public Text enlargeGoalsizeMediumPriceText;
    //public TextMeshProUGUI enlargeGoalSizePriceTextFree;
    public GameObject shopNotificationCanvas;
    public GameObject notificationCanvas;
    public TextMeshProUGUI notificationHeaderText;
    public TextMeshProUGUI notificationText;
    public RawImage notificationImage;
    private string lastRewardAdsEventName = "coinsplusdiamonds";
    public GameObject shopPromotionCanvas;
    public TextMeshProUGUI shopPromotionNotificationText;
    public Text shopPromotionBuyButtonText;
    //private string loadSceneName = "gameScene";
    private string loadSceneName = "extraPowers";


    public UIManager uiManagerWheel;

    void Awake()
    {
        admobCanvas.SetActive(false);
        shopNotificationCanvas.SetActive(false);
        //shopPromotionCanvas.SetActive(false);
        //shopPanel.SetActive(false);
        rewardAdsNextCanvasButtonGameObj.SetActive(false);

        adInit();

        //print("Globals.stadiumNumber " + Globals.stadiumNumber);
        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            if (Globals.stadiumNumber == 0)
                loadSceneName = "gameScene";
        }

        if (Globals.stadiumNumber == 1)
            loadSceneName = "gameSceneSportsHall";

         if (Globals.stadiumNumber == 2)
            loadSceneName ="gameSceneStreet";

        if (Globals.PITCHTYPE.Equals("STREET"))
            loadSceneName = "gameScene";

        //// if (!Globals.cpuGoalSize.Equals("STANDARD"))
        ////  {
        //||
        //!Globals.adsEnable)
        //||
        //(Globals.numMatchesInThisSession % 2) != 0)
        ////     Globals.loadSceneWithBarLoader(loadSceneName);
        // }
        /// else
        ////  {
        if (Globals.adsEnable)
            //&&
          //  !Globals.PITCHTYPE.Equals("STREET"))
          {
                if (admobAdsScript.showInterstitialAd())
                {
                    admobCanvas.SetActive(true);
                    waitingForInterstitialAddEvent = true;
                }
                //else
                //{
                //    rewardAdsNextCanvasButtonGameObj.SetActive(true);
                //}
                rewardAdsNextCanvasButtonGameObj.SetActive(true);
            }
            else
            {
                rewardAdsNextCanvasButtonGameObj.SetActive(true);
            }

            /*if ((Globals.numMatchesInThisSession % 3) == 2)
            {
                if (showPromotion() == false)
                {
                    shopPanel.SetActive(true);
                    rewardAdsNextCanvasButtonGameObj.SetActive(true);
                }
            }
            else
            {
                shopPanel.SetActive(true);
                rewardAdsNextCanvasButtonGameObj.SetActive(true);
            }*/
        ////}
                     
        graphics = new GraphicsCommon();
    }

    void Start()
    {        
        initReferences();
        disableCanvasElements();
        //setShopActivitySettings();
        //StartCoroutine(showCoins());
    }

    void Update()
    {
        if (waitingForAddEvent &&
           (admobAdsScript.getAdsClosed() ||
            admobAdsScript.getAdsFailed() ||
            admobAdsScript.getAdsRewardEarn()))
        {
            waitingForAddEvent = false;
            rewardAdsNextCanvasButtonGameObj.SetActive(true);

            if (admobAdsScript.getAdsRewardEarn())
            {

                if (lastRewardAdsEventName.Equals("coinsplusdiamonds"))
                {
                    Globals.addCoins(20);
                    Globals.addDiamonds(20);
                    showNotification(
                        new PurchaseItem("coinplusdiamonds", 20));
                }

                if (lastRewardAdsEventName.Equals("spinroulette"))
                {
                    uiManagerWheel.OnClickPaidSpin();
                }

                /*else if (lastRewardAdsEventName.Equals("enlargeGoal")) {
                    shopNotificationCanvas.SetActive(true);
                    Globals.enlargeGoalSize("MEDIUM");
                    showNotification(
                       new PurchaseItem("enlargegoalsize_medium"));
                }*/
            }
            else
            {
                showNotification(
                  new PurchaseItem("adsfailed"));
            }
    
            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsClosed(false);
            admobAdsScript.setAdsRewardEarn(false);
            admobCanvas.SetActive(false);
        }

        if (waitingForInterstitialAddEvent &&
           (admobAdsScript.getAdsClosed() ||
            admobAdsScript.getAdsFailed()))
        {
            waitingForInterstitialAddEvent = false;
            admobAdsScript.setAdsFailed(false);
            admobAdsScript.setAdsClosed(false);
            admobCanvas.SetActive(false);
            rewardAdsNextCanvasButtonGameObj.SetActive(true);
        }

        if (Globals.purchasesQueue.Count > 0 &&
           !shopNotificationCanvas.activeSelf)
        {          
            shopNotificationCanvas.SetActive(true);
            showNotification(
                Globals.purchasesQueue.Dequeue());
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

    private IEnumerator showCoins()
    {  
        yield return new WaitForSeconds(0.5f);
        audioManager.Play("elementAppear");
        treasureButton.SetActive(true);
    }
   
    public void nextCanvasButton()
    {
        Globals.loadSceneWithBarLoader(loadSceneName);
        //SceneManager.LoadScene("gameLoader");
    }

    public void onClickWatchRewardAdButton(string eventName)
    {
        if (admobAdsScript.showRewardAd()) {
            waitingForAddEvent = true;
            admobCanvas.SetActive(true);
            rewardAdsNextCanvasButtonGameObj.SetActive(false);
            lastRewardAdsEventName = eventName;
        }
        else
        {
            rewardAdsNextCanvasButtonGameObj.SetActive(true);
            waitingForAddEvent = false;
        }

        admobAdsScript.setAdsClosed(false);
        admobAdsScript.setAdsFailed(false);
        admobAdsScript.setAdsRewardEarn(false);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("REWARDS_ADS_ONLCICK_EVENT", new Dictionary<string, object>
            {
                    { "REWARDS_ADS_BUTTON_CLICKED", true},
            });
        }
    }

    private bool showPromotion()
    {        
        string diamond10000Price =
            IAPManager.instance.getPriceByHash("diamond10000");

        if (string.IsNullOrEmpty(diamond10000Price))
        {
            shopPromotionCanvas.SetActive(false);
            return false;
        }

        shopPromotionCanvas.SetActive(true);
        shopPromotionBuyButtonText.text = diamond10000Price;

        return true;
    }

    private void initReferences()
    {
        audioManager = FindObjectOfType<AudioManager>();
        //rewardAdsNextCanvas = rewardAdsNextCanvas.GetComponent<Button>();
    }

    private void disableCanvasElements()
    {
        //treasureButton.SetActive(false);
    }

    /*public void OnClickGoalEnlargeBuyButton()
    {
        if (!Globals.cpuGoalSize.Equals("STANDARD"))
            return;

        int enalrgeGoalMediumCreditsNum =
            PlayerPrefs.GetInt("enlargeGoal_MEDIUM_CREDITS");
        if ((PlayerPrefs.HasKey("enlargeGoal_MEDIUM_CREDITS") &&
             enalrgeGoalMediumCreditsNum > 0))
        {
            Globals.enlargeGoalSize("MEDIUM");
            Globals.purchasesQueue.Enqueue(new PurchaseItem("enlargegoalsize_medium", "MEDIUM"));
            enalrgeGoalMediumCreditsNum--;
            enlargeGoalSizePriceTextFree.text = 
                "CREDITS: " + enalrgeGoalMediumCreditsNum.ToString() + "\nFREE";
            PlayerPrefs.SetInt("enlargeGoal_MEDIUM_CREDITS", enalrgeGoalMediumCreditsNum);
            PlayerPrefs.Save();
        }
        else
        {
            //IAPManager.instance.buyEnlargegoalsizeMedium();
            onClickWatchRewardAdButton("enlargeGoal");
        }

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("ENLARGE_SPECIAL_OFFER_BUTTON_CLICK_EVENT", new Dictionary<string, object>
            {
                    { "ENLARGE_SPECIAL_OFFER_BUTTON_CLICK", true},
            });
        }
        //shopPanel.SetActive(false);
    }*/

    public void onClick10000diamonds()
    {
        IAPManager.instance.buyDiamond10000();
    }

    /*
    private void setShopActivitySettings()
    {

        int enalrgeGoalMediumCreditsNum =
            PlayerPrefs.GetInt("enlargeGoal_MEDIUM_CREDITS");
        if ((PlayerPrefs.HasKey("enlargeGoal_MEDIUM_CREDITS") &&
            enalrgeGoalMediumCreditsNum > 0))
        {
            enlargeGoalSizePriceTextFree.text = "CREDITS: " + enalrgeGoalMediumCreditsNum.ToString() + "\nFREE";
        }
        else
        {
            enlargeGoalSizePriceTextFree.text = "FREE\nWATCH REWARD AD";
        }
    }*/

    public void onClickShopNotificationClose()
    {
        shopNotificationCanvas.SetActive(false);
    }

    public void showNotification(PurchaseItem item)
    {
        string type = item.name;

        if (type.Contains("enlargegoalsize_medium"))
        {
            shopNotificationCanvas.SetActive(true);
            notificationText.text =
               Languages.getTranslate("The opponent's goal will be enlarge for the next match. Good luck!");
            notificationImage.texture =
                Resources.Load<Texture2D>("others/goal4");
        } else if (type.Contains("coinplusdiamonds"))
        {
            shopNotificationCanvas.SetActive(true);
            int coins = item.coins;
            notificationText.text =
                Languages.getTranslate("Excellent! +" + coins.ToString() + " coins and + "
                + coins.ToString() + " diamonds awarded!",
                new List<string>() { coins.ToString(), coins.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/showNotificationCoinsAndDiamonds");
        } else if (type.Contains("coin"))
        {
            shopNotificationCanvas.SetActive(true);
            int coins = item.coins;
            notificationText.text =
                    Languages.getTranslate("Excellent! +" + coins.ToString() + " coins awarded!",
                               new List<string>() { coins.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/shownotificationCoins");
        }
        else if (type.Contains("adsfailed"))
        {
            shopNotificationCanvas.SetActive(true);
            notificationHeaderText.text =
                Languages.getTranslate("Ads failed");
            notificationText.text = "";
            notificationImage.texture =
                     Resources.Load<Texture2D>("Shop/showNotificationAdsFailed");
        }
        else if (type.Contains("diamond"))
        {
            int diamond = item.diamonds;
            shopNotificationCanvas.SetActive(true);
            notificationText.text =
                Languages.getTranslate(
                    "Excellent! +" + diamond.ToString() + " diamonds and 2000 coins awarded!",
                    new List<string>() { diamond.ToString() });
            notificationImage.texture =
                Resources.Load<Texture2D>("Shop/showNotificationCoinsAndDiamonds");
            //extra coins for free
            Globals.addCoins(2000);
        }
    }

    public void onClickShopPromotionClose()
    {
        shopPromotionCanvas.SetActive(false);
        nextCanvasButton();
    }
}
