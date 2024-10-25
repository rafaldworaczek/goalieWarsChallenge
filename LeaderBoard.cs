using UnityEngine;
using System;
using System.Collections;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;
using GlobalsNS;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour
{
    //public static string defaultLeaderboardID = "CgkI9MXf1t8UEAIQAA";
    public static string defaultLeaderboardID = "CgkI9MXf1t8UEAIQAg";
    public static bool loginFinished = false;
    public static bool initFinished = false;

    /*Leaderbords is more intended for a games that have some levels 
     * and udpate score after finish a game. Score is checked agains all time, this week, and daily the best 
     * scores. if it's higher then leaderboard is entry is updated. In this case all time, 
     * this week and daily scores are all the same since it cannot be done differently? To have
     * all time, this week and daily score we need 3 leaderboards? 
     */

    void Start()
    {    
        if (Globals.isAnalyticsEnable)
        {
            if (Globals.isAnalyticsEnable)
            {
                AnalyticsResult analyticsResult = 
                    Analytics.CustomEvent("LEADERBOARD_EVENT", new Dictionary<string, object>
                {
                    { "LEADERBOARD_ENTERED", true},
                });
            }
        }

        //if (!PlayerPrefs.HasKey("leaderBoardPermissionsDone"))
        //{
        //    return;
        //}


        #if UNITY_ANDROID

        PlayGamesPlatform.Activate();

        if (!PlayGamesPlatform.Instance.localUser.authenticated)           
        {           
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success)
                {
                   
                }
                else
                {
                    
                }
            }, true);
        }
        else
        {
        }
        #endif
    }

    public static void Login()
    {
       #if UNITY_ANDROID

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success)
                {

                    //LeaderBoard.ReportScore(Globals.coins);
                    //print("#LEADEBOARDDBG WAS SIGNEIN BEFORE ");
                }
                //else
                //{
                /// Not signed in. We'll want to show a sign in button
                //    print("#LEADEBOARDDBG NOT SIGNEIN BEFORE DO NOTHING");
                //}
            });
        }

        #endif
    }

    public static void OnShowLeaderBoard()
    {
        #if UNITY_ANDROID
        OnShowLeaderBoard(defaultLeaderboardID);
        #endif
    }

    public static void OnShowLeaderBoard(string leaderboardID)
    {
        #if UNITY_ANDROID
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
        }             
        #endif
    }

    public static void ReportScore(long score)
    {
        //if (!PlayerPrefs.HasKey("leaderBoardPermissionsDone"))
        //    return;

        Debug.Log("##addscore " + score);

        #if UNITY_ANDROID
        ReportScore(score, defaultLeaderboardID);
        #endif
    }

    public static void ReportScore(long score, string leaderboardID)
    {
        //if (!PlayerPrefs.HasKey("leaderBoardPermissionsDone"))
        //    return;

        #if UNITY_ANDROID

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardID, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("#LEADEBOARDDBG Update Score Success");
                }
                else
                {
                }
            });
        }
        else
        {
        }
        #endif
    }

    public static string getDefaultLeaderBoardID()
    {
        return defaultLeaderboardID;
    }

    public static bool getLoginFinished()
    {
        return loginFinished;
    }

 
    public static bool getInitFinished()
    {
        return initFinished;
    }


    public static void OnLogOut()
    {
        #if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
        #endif
    }
}
