using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GlobalsNS;
using System;

public class admobAdsScript : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private bool adsClosed = false;
    private bool adsFailedLoad = false;
    private bool adsRewardEarn = false;
    /*CHANGEIT TO TEST*/

    private bool isAdTestEnable = false;
    private bool DEBUG_ENABLE = true;
    private float lastTimeInterstetialDisplay;


    public void Start()
    {      
        if (Globals.isAdRemoved())
        {
            Globals.adsEnable = false;
        }

        lastTimeInterstetialDisplay = 0f;

        init();
        RequestRewardedAd();

        if (!Globals.adsEnable)
        {
            return;
        }
        RequestInterstitial();
    }

    public void init()
    {
        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            if (!isAdTestEnable)
                MobileAds.Initialize("ca-app-pub-4281391536440718~6866202819");
            else
                MobileAds.Initialize("ca-app-pub-3940256099942544~3347511713");
        }
        else
        {
            if (!isAdTestEnable)
                MobileAds.Initialize("ca-app-pub-4281391536440718~7400929053");
            else
                MobileAds.Initialize("ca-app-pub-3940256099942544~3347511713");
        }        
    }

    private void RequestBanner()
    {    
        //print("DEBUGADMONB1 banner request");
        string adUnitId  = "";
     

        if (Globals.PITCHTYPE.Equals("INDOOR")) {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-4281391536440718/4501207002";
                #elif UNITY_IPHONE
                    adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                    adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }

        } else {
           if (!isAdTestEnable)
           {
                #if UNITY_ANDROID
                    adUnitId = "ca-app-pub-4281391536440718/9627918321";
                #elif UNITY_IPHONE
                    adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                    adUnitId = "unexpected_platform";
                #endif
           }
           else
           {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
            #else
                adUnitId = "unexpected_platform";
            #endif
            }   
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    private void RequestBanner(AdPosition pos)
    {
        //print("DEBUGADMONB1 banner request");
        string adUnitId = "";

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/4501207002";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
        }
        else
        {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/9627918321";
                #elif UNITY_IPHONE
                    adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                    adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
                #else
                    adUnitId = "unexpected_platform";
                #endif
            }
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, pos);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        string adUnitId;


        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/2897971983";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/1033173712";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
        }
        else
        {

            if (!isAdTestEnable)
            {
#if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/4659512731";
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
                adUnitId = "unexpected_platform";
#endif
            }
            else
            {
#if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
                adUnitId = "unexpected_platform";
#endif
            }
        }
        // Initialize an InterstitialAd.
        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        this.interstitial = new InterstitialAd(adUnitId);
        this.interstitial.OnAdFailedToLoad += genericHandleOnAdFailedToLoad;
        this.interstitial.OnAdClosed += genericHandleAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
     
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void RequestRewardedAd()
    {
        string adUnitId;

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/9254646718";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
        }
        else
        {
            if (!isAdTestEnable)
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-4281391536440718/8670059876";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
            else
            {
                #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
                #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
                #else
                adUnitId = "unexpected_platform";
                #endif
            }
        }

        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public bool showBannerAd()
    {
        if (!Globals.adsEnable)
            return false;

        RequestBanner();

        return true;
    }

    public bool showBannerAd(AdPosition pos)
    {
        if (!Globals.adsEnable)
            return false;

        RequestBanner(pos);

        return true;
    }

    public bool showInterstitialAd()
    {
        //was for to 05.01.2023
        if (!Globals.adsEnable ||
            ((Time.time - lastTimeInterstetialDisplay) < 7f))
            return false;

        //print("ADSINTERSTITAL " + this.interstitial.IsLoaded());

        if (this.interstitial.IsLoaded())
        {
            adsClosed = false;
            this.interstitial.Show();
            lastTimeInterstetialDisplay = Time.time;
            RequestInterstitial();


        } else
        {
            return false;
        }

        return true;
    }

    public bool showRewardAd()
    {
        //if (!Globals.adsEnable)
        //    return false;

        if (this.rewardedAd.IsLoaded())
        {
            adsClosed = false;
            this.rewardedAd.Show();
            RequestRewardedAd();
        }
        else
        {
            return false;
        }

        return true;
    }

    private bool destroyBanner()
    {
        if (!Globals.adsEnable)
            return false;

        bannerView.Destroy();
        return true;
    }

    public bool hideBanner()
    {
        //if (!Globals.adsEnable)
        // return false;

        if (bannerView != null)
            bannerView.Hide();

        return true;
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        adsFailedLoad = true;
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        adsFailedLoad = true;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        adsClosed = true;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        adsRewardEarn = true;
    }

    public void genericHandleAdClosed(object sender, EventArgs args)
    {
        adsClosed = true;
    }

    public void genericHandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        adsFailedLoad = true;
        print("DBGADS1 INTERSTIAL FAILED TO LOAD");
    }

    public void setAdsRewardEarn(bool val)
    {
        adsRewardEarn = val;
    }

    public bool getAdsRewardEarn()
    {
        return adsRewardEarn;
    }

    public void setAdsClosed(bool val)
    {
        adsClosed = val;
    }

    public void setAdsFailed(bool val)
    {
        adsFailedLoad = val;
    }

    public bool getAdsClosed()
    {
        return adsClosed;
    }

    public bool getAdsFailed()
    {
        return adsFailedLoad;
    }

}
