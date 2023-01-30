using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class LanguageMenu : MonoBehaviour
{
    private int currentIdx = 0;
    public RawImage flagImg;
    public GameObject langMenuCanvas;

    private string[] langsFlags =
    {
            "united kingdom", 
            "poland",
            "spain",
            "brazil"
            //"portugal",
            //"spain"    
    };

    void Awake()
    {
        LocalizationSettings.InitializationOperation.WaitForCompletion();
        //for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        //    Debug.Log("#DBGLOCAL " + LocalizationSettings.AvailableLocales.Locales[i]);
        initFlag();
    }

    public void initFlag()
    {
        if (PlayerPrefs.HasKey("LANG_FLAG_IDX"))
            currentIdx = PlayerPrefs.GetInt("LANG_FLAG_IDX");
        else
            currentIdx = 0;

        string countryName = langsFlags[currentIdx];
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        flagImg.texture = Resources.Load<Texture2D>("Flags/national/" + countryName);

        LocalizationSettings.InitializationOperation.WaitForCompletion();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentIdx];
    }

    public void onClickPrev()
    {
        if (currentIdx > 0)
        {
            currentIdx--;
        }
        else
        {
            if (currentIdx == 0)
            {
                currentIdx = langsFlags.Length - 1;
            }
        }

        string countryName = langsFlags[currentIdx];
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        flagImg.texture = Resources.Load<Texture2D>("Flags/national/" + countryName);
    }

    public void onClickNext()
    {
        if ((currentIdx + 1) < langsFlags.Length)
        {
            currentIdx++;
        }
        else
        {
            //if (currATeamIdx == (nationalTeams.getMaxTeams() - 1))
            if (currentIdx == (langsFlags.Length - 1))
            {
                currentIdx = 0;
            }
        }

        string countryName = langsFlags[currentIdx];
        countryName = Regex.Replace(countryName, "\\s+", "").ToLower();
        flagImg.texture = Resources.Load<Texture2D>("Flags/national/" + countryName);
    }

    public void onClickOk()
    {
        LocalizationSettings.InitializationOperation.WaitForCompletion();        
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentIdx];

        PlayerPrefs.SetInt("LANG_FLAG_IDX", currentIdx);
        PlayerPrefs.SetInt("LANG_SETTING_DONE", 1);
        PlayerPrefs.Save();
       
        SceneManager.LoadScene("logoScene");
    }

    public void onClickShowCanvas()
    {
        langMenuCanvas.SetActive(true);
    }

    public void onClickCloseCanvas()
    {
        langMenuCanvas.SetActive(false);
    }
}
