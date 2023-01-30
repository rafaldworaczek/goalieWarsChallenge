using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalsNS;
using graphicsCommonNS;
using UnityEngine.SceneManagement;
using MenuCustomNS;
using TMPro;
using LANGUAGE_NS;

public class fireworksScript : MonoBehaviour
{
    public GameObject infoTextGO;
    public TextMeshProUGUI infoText;
    private GraphicsCommon graphics = new GraphicsCommon();
    public GameObject modelGO;
    public GameObject modelHairGO;
    public GameObject coinsImageGO;
    public GameObject diamondsImageGO;
    private float finalCoinsRewarded = 0;
    private Teams team;

    void Awake()
    {
        team = new Teams();
        Globals.wonFinal = true;
        coinsImageGO.SetActive(false);
        diamondsImageGO.SetActive(false);

        team = new Teams(Globals.leagueName);

        int teamID = Globals.teamAid;
        if (Globals.playerPlayAway)
        {
            teamID = Globals.teamBid;
        }

        string playerSkinHairDescA =
          team.getPlayerDescByIndex(teamID, 0);

        graphics.setPlayerTextures(modelGO, 
                                   modelHairGO, 
                                   teamID, 
                                   Globals.leagueName,
                                   playerSkinHairDescA,
                                   true,
                                   true,
                                   team);
    }

    // Start is called before the first frame update
    void Start()
    {
        coinsAward();
        infoTextGO.SetActive(false);        
        StartCoroutine(celebrationTextShow());
    }

    IEnumerator celebrationTextShow() {
        Color color = infoText.color;

        yield return new WaitForSeconds(1.0f);
        infoText.text = Languages.getTranslate("CONGRATULATIONS!");
        infoTextGO.SetActive(true);
        color.a = 1f;
        infoText.color = color;
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(graphics.textFadeOut(infoText, 3f));
        //print("DEBUG321 RETURN " + infoText.color.a);
        yield return new WaitForSeconds(1.5f);

        color.a = 1f;
        infoText.color = color;

        //infoText.text = "YOU ARE THE CHAMPION!";

        infoText.text = 
            Languages.getTranslate(
                "YOU HAVE WON THE " + Globals.wonCupName.ToUpper(),
                new List<string>() { Globals.wonCupName.ToUpper() });
    
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(graphics.textFadeOut(infoText, 3f));
        yield return new WaitForSeconds(1.5f);

        color.a = 1f;
        infoText.color = color;
        coinsImageGO.SetActive(true);
        infoText.text = "+" + ((int) finalCoinsRewarded).ToString() 
            + Languages.getTranslate(" COINS AWARDED!");    
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(graphics.textFadeOut(infoText, 3f));
        coinsImageGO.SetActive(false);
        yield return new WaitForSeconds(1.5f);


        color.a = 1f;
        infoText.color = color;
        diamondsImageGO.SetActive(true);
        infoText.text = "+" + ((int)finalCoinsRewarded).ToString() + 
            Languages.getTranslate(" DIAMONDS AWARDED!");
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(graphics.textFadeOut(infoText, 3f));
        diamondsImageGO.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        if (Globals.level != Globals.MAX_LEVEL)
        {
            infoText.text = 
                Languages.getTranslate("IT IS TIME TO CHALLENGE A NEXT LEVEL NOW");       
            color.a = 1f;
            infoText.color = color;
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(graphics.textFadeOut(infoText, 5f));
            yield return new WaitForSeconds(1.5f);
            infoText.text = Languages.getTranslate("ARE YOU READY?");
            color.a = 1f;
            infoText.color = color;
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(graphics.textFadeOut(infoText, 3f));
            yield return new WaitForSeconds(1.5f);
        }

        infoText.text = "";
    }

    public void coinsAward()
    {
        //finalCoinsRewarded = Mathf.InverseLerp(
        //       Globals.MIN_LEVEL, Globals.MAX_LEVEL, Globals.level) * 1500f;

        finalCoinsRewarded = 100f;
        if (Globals.level == Globals.MIN_LEVEL)
        {
            finalCoinsRewarded = 40f;
        }
        else if (Globals.level == 2)
        {
            finalCoinsRewarded = 80f;
        }
        else if (Globals.level == 3)
        {
            finalCoinsRewarded = 100f;
        }
        else if (Globals.level == 4)
        {
            finalCoinsRewarded = 250f;
        }
        else if (Globals.level == Globals.MAX_LEVEL)
        {
            finalCoinsRewarded = 400f;
        }

        if (Globals.wonCupName.Equals("LEAGUE"))
            finalCoinsRewarded = finalCoinsRewarded * 1.5f;

        Globals.addCoins((int) finalCoinsRewarded);
        Globals.addDiamonds((int) finalCoinsRewarded);
    }

    public void nextCanvas()
    {
        Globals.recoverOriginalResolution();
        Teams teams = new Teams("NATIONALS");
        
        if (teams.isAnyNewTeamUnclocked())
        {         
            SceneManager.LoadScene("rewardNewTeam");
        }
        else
        {
            if (Globals.gameType.Equals("CUP"))
                SceneManager.LoadScene("menu");
            else
                Globals.loadSceneWithBarLoader("Leagues");
        }
    }
}
