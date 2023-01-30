using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using GlobalsNS;
using LANGUAGE_NS;

public class gameLoaderNoSettings : MonoBehaviour
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

    void Start() {
        loadingSceneName = Globals.loaderBarSceneName;
        textLoadingCounter = 0;
        loopCounter = 0;
              
        StartCoroutine(LoadSceneAsynchronously(loadingSceneName));
    }

    private bool isGameScene(string sceneName)
    {
        if (sceneName.Equals("gameScene"))
            return true;
        return false;            
    }

    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            sliderBar.value = progress;
            loadingPercentageText.text = (int) (progress * 100.0f) + " % ";
            string loadingTextTmp = Languages.getTranslate("Loading");
            if (!isGameScene(loadingSceneName))
            {
                loadingTextTmp = Languages.getTranslate("Preparing");
            }

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