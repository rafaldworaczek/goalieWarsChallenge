using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using GlobalsNS;
using LANGUAGE_NS;

public class gameLoader : MonoBehaviour
{
    private bool loadScene = false;
    private string loadingSceneName;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingPercentageText;
    public Slider sliderBar;
    private int loopCounter = 0;
    private int textLoadingCounter = 0;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    private LeagueBackgroundMusic leagueBackgroundMusic;
    private float delayStart = 0f;
    void Start() {
        if  (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            Globals.commentatorStr = "NO";
        }

        Time.timeScale = 1f;
        loadingSceneName = Globals.loaderBarSceneName;
        textLoadingCounter = 0;
        loopCounter = 0;
        delayStart = Time.time;

        if (!Globals.isMultiplayer)
        {
            leagueBackgroundMusic =
                GameObject.Find("leagueBackgroundMusic").GetComponent<LeagueBackgroundMusic>();
            leagueBackgroundMusic.stop();
        }

        if (Globals.isNewSeason)
        {
            if (MatchTableMain.matchTableInstance != null)
            {
                Destroy(MatchTableMain.matchTableInstance);
            }

            MatchTableMain.matchTableInstance = null;
            //Globals.leagueName = leagueNames[leagueAIdx];
            //Globals.loadSceneWithBarLoader("Leagues");
        }

        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

        if (admobAdsScript != null &&
            admobAdsScript != null)
        admobAdsScript.hideBanner();

        StartCoroutine(LoadSceneAsynchronously(loadingSceneName));
    }

    private bool isGameScene(string sceneName)
    {
        if (sceneName.Equals("gameScene") ||
            sceneName.Equals("gameSceneSportsHall") ||
            sceneName.Equals("gameSceneMultiplayer_Room_2") ||
            sceneName.Equals("multiplayerMenu"))
            return true;
        return false;            
    }


    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        string loadingTextTmp = Languages.getTranslate("Loading");
        print("#DBGgameScene name " + sceneName + " dontCheckOnlineUpdate " + Globals.dontCheckOnlineUpdate);
        if (Globals.isMultiplayer &&
            sceneName.Equals("multiplayerMenu") &&
            !Globals.dontCheckOnlineUpdate)
        {
            loadingPercentageText.text = "30 % ";
            while (true)
            {
                loadingTextTmp = Languages.getTranslate("Checking for updates");

                for (int i = 0; i < (textLoadingCounter % 4); i++)
                {
                    loadingTextTmp += ".";
                }

                //print("LOADING TEXT TMP " + loadingTextTmp + " PROGRESS " 
                //    + progress + " sceneName " + sceneName);
                ///print(" loopCounter " + loopCounter + " textLoadingCounter " + textLoadingCounter);
                if (loopCounter % 20 == 0)
                {
                    loadingText.text = loadingTextTmp;
                    textLoadingCounter++;
                }

                loopCounter++;
                if ((Time.time - delayStart) <= 3f)
                    yield return null;
                else
                    break;
            }
        }

        if (Globals.isMultiplayer &&
            Globals.isMultiplayerUpdate &&
            !Globals.dontCheckOnlineUpdate)
        {
            yield break;
        }

        Globals.dontCheckOnlineUpdate = false;


        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            loadingTextTmp = Languages.getTranslate("Loading");
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            sliderBar.value = progress;
            loadingPercentageText.text = (int) (progress * 100.0f) + " % ";
            if (!isGameScene(loadingSceneName))
            {
                loadingTextTmp = Languages.getTranslate("Preparing");
            }

            /*if (Globals.isMultiplayer &&
                ((int) (progress * 100.0f) > 52) &&
                 sceneName.Equals("multiplayerMenu"))
            {
                loadingTextTmp = Languages.getTranslate("Checking for updates");
            }*/

            for (int i = 0; i < (textLoadingCounter % 4); i++)
            {
                loadingTextTmp += ".";
            }

            //print("LOADING TEXT TMP " + loadingTextTmp + " PROGRESS " 
            //    + progress + " sceneName " + sceneName);

            if (loopCounter % 20 == 0)
            {
                loadingText.text = loadingTextTmp;
                textLoadingCounter++;
            }

            loopCounter++;

            yield return null;
        }
    }
}