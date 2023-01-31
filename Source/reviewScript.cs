using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using UnityEngine.Analytics;

public class reviewScript : MonoBehaviour
{
    public GameObject panel;

    void Awake()
    {
        panel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //print("DBG1245 Globals.numGameOpened " + Globals.numGameOpened);
        //print("DBG1245 Globals.numGameOpened  " + Globals.numGameOpened);

        if (Globals.reviewDisplayed ||
            Globals.numGameOpened < 5 ||
            Globals.numMatchesInThisSession != 1 ||
            (PlayerPrefs.GetInt("appReview_ButtonYesClicked") != 0) ||
            (PlayerPrefs.GetInt("appReview_DontAskAgainClicked") != 0))
        {
            return;
        }

        if (PlayerPrefs.HasKey("appReview_notNow_numGameOpened"))
        {
            int notNowNumGameOpened =
                 PlayerPrefs.GetInt("appReview_notNow_numGameOpened");
            if ((Globals.numGameOpened - notNowNumGameOpened) < 3)
            {
                return;
            }
        }

        Globals.reviewDisplayed = true;
        panel.SetActive(true);               
    }
  
    public void dontAskAgain()
    {
        PlayerPrefs.SetInt("appReview_DontAskAgainClicked", 1);
        PlayerPrefs.Save();
        panel.SetActive(false);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("REVIEW_GOOGLE_PLAY_RATE", new Dictionary<string, object>
            {
                    { "REVIEW_GOOGLE_PLAY_RATE_DONT_ASK_AGAIN", Globals.numGameOpened},
            });
        }
    }

    public void notNow()
    {
        PlayerPrefs.SetInt("appReview_notNow_numGameOpened", Globals.numGameOpened);
        PlayerPrefs.Save();
        panel.SetActive(false);

        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("REVIEW_GOOGLE_PLAY_RATE", new Dictionary<string, object>
            {
                    { "REVIEW_GOOGLE_PLAY_RATE_NOT_NOW", Globals.numGameOpened},
            });
        }
    }

    public void yesSure()
    {
        PlayerPrefs.SetInt("appReview_ButtonYesClicked", 1);
        PlayerPrefs.Save();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.OSystems.GoalieWarsFootbalIndoor");
        } else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.OSystems.GoalieWarsFootbalIndoor");
        }
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.OSystems.GoalieStrikerFootball");
        }

        panel.SetActive(false);


        if (Globals.isAnalyticsEnable)
        {
            AnalyticsResult analyticsResult =
                Analytics.CustomEvent("REVIEW_GOOGLE_PLAY_RATE", new Dictionary<string, object>
            {
                    { "REVIEW_GOOGLE_PLAY_RATE_YES_SURE", Globals.numGameOpened},
            });
        }
        //Application.OpenURL("market://details?id=com.OSystems.GoalieStrikerFootball");
    }

    public void closePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
