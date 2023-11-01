using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using GlobalsNS;
using MenuCustomNS;
using AudioManagerNS;

public class setTextures : MonoBehaviour
{
    private controllerRigid playerMainScript;
    public static int MAX_MODELS_PLAYERS = 2;
    public GameObject[] playerModels;
    public GameObject[] playerModelsHair;
    private GraphicsCommon graphics;
    private StringCommon strCommon;
    //private NationalTeams teams;
    private Teams teams;
    private float currentTime;
    private float lastTimeWallsAdsChanged;
    private int wallsAdsMaterialIdx = 0;
    //private int wallsAdsMaterialIdx = 3;
    private float wallsAdsChangTime = 10f;
    private int MAX_WALLS_ADS = 4;
    private int MAX_GROUNDS = 3;
    private int MAX_FLARES = 3;
    private Color[] wallsAdsTopMaterialColors;
    public ParticleSystem[] flareParticle;
    public ParticleSystem[] snowParticle;
    public ParticleSystem[] rainParticle;
    public int lastScoreFlareStarted = 0;
    private float lastTimeParticleStart;
    private float lastTimeParticleStop;
    private bool isFlarePlaying = false;
    private Material wallsAdsMaterial;
    private Material wallsAdsTopMaterial;
    private Material wallsAdsMaterialTransparent;
    private Material wallsAdsTopMaterialTransparent;
    private Texture2D[] wallsAdsTexture;
    private Texture2D[] groundTexture;
    private bool isTrainingActive = Globals.isTrainingActive;
    private bool isBonusActive = false;
    private AudioManager audioManager;
    private int teamHostID;
    private string[,] groundFilesNames =
        {
         //"Ground/Material/st_040_bg_field",                   
         //"Ground/Material/st_050_bg_field",         
         //"Ground/Material/st_051_bg_field",
         //{ "st_060_bg_field", "#5872E0" },
         //{ "st_060_bg_field", "#5F6FB0" },
         //{ "st_031_bg_field", "#636FA6" },
         /*{ "st_080_bg_field", "#918971" },
         { "st_060_bg_field", "#9B9F9A" },
         { "st_080_bg_field", "#C3A0B2" },
         { "st_080_bg_field", "#9B9F9A" },*/
         { "GrassShader_2", "#AEB03C" },
         { "st_060_bg_field", "#92989A" },
         { "GrassShader_1", "#FFFFFF" },
         { "GrassShader_4", "#FFB9B9" },
         { "GrassShader_3", "#FFB9B9" }
         //{ "st_060_bg_field", "#667779" }    
         //"Ground/Material/st_061_bg_field",        
         //"Ground/Material/st_070_bg_field",
         //"Ground/Material/st_071_bg_field",
         ///{ "st_080_bg_field", "#7E7E7E" }
    };


    void Awake()
    {        
        wallsAdsMaterialIdx = UnityEngine.Random.Range(0, MAX_WALLS_ADS);

        updateTrainingSettings();
        //updateBonusSettings();
        playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
        audioManager = FindObjectOfType<AudioManager>();

        lastTimeParticleStart = 0f;
        lastTimeParticleStop = -1000f;
        graphics = new GraphicsCommon();
        strCommon = new StringCommon();
        teams = new Teams(Globals.leagueName);

        ///it's sport halls
        if (Globals.stadiumNumber != 1 &&
            !Globals.PITCHTYPE.Equals("STREET"))               
        {
            initWallsMaterials();
            int groundNum = 0;
            if ((Globals.numGameOpened > 1) ||
                (Globals.numMatchesInThisSession > 2))//) && Globals.numMatchesInThisSession <= 3)
                groundNum = playWheatherEffect();
            else
            {
                groundNum = 0;
                rainParticle[0].Play();
            }
            intGroundMaterials(groundNum);

            setWallsTopColors();
            setWallsAdsTexture();
            setWallsMaterial(0, 0);
        }

        //print("IndexTEAMA " + Globals.teamAid);
        //print("IndexTEAMB " + Globals.teamBid);

        currentTime = lastTimeWallsAdsChanged = Time.time;
        setPlayersTextures();
    }

    void Start()
    {
        if (Globals.isBonusActive)
        {
            isBonusActive = true;
        }
        else
        {
            isBonusActive = false;
        }

        teamHostID = playerMainScript.getTeamHostId();
    }

    void Update() {
        //it's sport hall
        if (Globals.stadiumNumber == 1 ||
            Globals.PITCHTYPE.Equals("STREET"))
        {
            return;
        }


        if ((Time.time - lastTimeWallsAdsChanged > wallsAdsChangTime) &&
            !isTrainingActive &&
            !isBonusActive)
        {
            setWallsMaterial(wallsAdsMaterialIdx, 0);
            lastTimeWallsAdsChanged = Time.time;
            wallsAdsMaterialIdx++;
            wallsAdsMaterialIdx = wallsAdsMaterialIdx % MAX_WALLS_ADS;
        }

        if (isBonusActive)
        {
            setWallsMaterial(wallsAdsMaterialIdx, 2);
        }

        if (!(Globals.graphicsQuality.Equals("Low") ||
              Globals.graphicsQuality.Equals("Very low")) &&
            !isTrainingActive &&
            !isBonusActive &&
            !isFlarePlaying &&
            ((Time.time - lastTimeParticleStop) > 320f) &&
             hasScoreChanged(teamHostID) &&
            (playerMainScript.isLeadingByHighScore(teamHostID)))
        {
            setFlarePlay();
        }

        if (isFlarePlaying && 
           (Time.time - lastTimeParticleStart) > 35f)
        {
            setFlareStop();
        }
    }

    private int playWheatherEffect()
    {
        int randWheather = UnityEngine.Random.Range(0, 5);
        if (randWheather <= 1) {
            if (randWheather == 0)
                return 3;
            return 2;
        } else if ((randWheather == 3) ||
                   (randWheather == 2))
        {
            rainParticle[0].Play();
            return 0;
        } else
        {
            snowParticle[0].Play();
            return 1;
        }

        return 2;
    }

    private bool hasScoreChanged(int teamHostID)
    {
        if (teamHostID == 1 && lastScoreFlareStarted != Globals.score1)
            return true;

        if (teamHostID == 2 && lastScoreFlareStarted != Globals.score2)
            return true;

        return false;
    }

    private void setFlarePlay() {
        int randFlare = UnityEngine.Random.Range(1, 4);
        
        for (int i = 0; i < flareParticle.Length; i++)
        {
            flareParticle[i].Play();
            setFlareParticleColor(flareParticle[i], i);
            for (int j = 1; j <= 30; j++)
                Invoke("flarePlaySound", UnityEngine.Random.Range(3.0f, 6.5f));
        }

        if (teamHostID == 1)
            lastScoreFlareStarted = Globals.score1;
        else
            lastScoreFlareStarted = Globals.score2;

        lastTimeParticleStart = Time.time;
        isFlarePlaying = true;
    }

    private void flarePlaySound()
    {
        audioManager.PlayAtTheSameTime("fireworks1");
    }

    private void setFlareStop() {
        for (int i = 0; i < flareParticle.Length; i++)
            flareParticle[i].Stop();

        lastTimeParticleStop = Time.time;
        isFlarePlaying = false;
    }

    private void setFlareParticleColor(ParticleSystem particle, int idx)
    {
        string colorsName = teams.getFlareColorByTeamIndex(Globals.teamAid);
        if (teamHostID == 2)
            colorsName = teams.getFlareColorByTeamIndex(Globals.teamBid);

        string[] teamsColor = colorsName.Split('_');
        int colorIdx = idx % teamsColor.Length;
        particle.startColor = convertFlareColorNameToHex(teamsColor[colorIdx]);
    }

    public bool isFlareEnable()
    {
        return isFlarePlaying;
    }

    private Color convertFlareColorNameToHex(string colorName)
    {
        Color color;
        switch (colorName)
        {
            case "white":
                ColorUtility.TryParseHtmlString("#968D75", out color);
                return color;
            case "green":
                ColorUtility.TryParseHtmlString("#7ACA55", out color);
                return color;
            case "blue":
                ColorUtility.TryParseHtmlString("#719AD7", out color);
                return color;
            case "red":
                ColorUtility.TryParseHtmlString("#F35752", out color);
                return color;
            case "yellow":
                ColorUtility.TryParseHtmlString("#FFD300", out color);
                return color;
            default:
                ColorUtility.TryParseHtmlString("#968D75", out color);
                return color;
        }
    }

    private void initWallsMaterials()
    {
        //walls = new GameObject[16];
        //wallsTop = new GameObject[6];
        wallsAdsTopMaterialColors = new Color[MAX_WALLS_ADS];
        wallsAdsTexture = new Texture2D[MAX_WALLS_ADS];

        wallsAdsTopMaterial = graphics.getMaterial(
           "wallsAds/wallsAdsColorMaterial");

        wallsAdsMaterial = graphics.getMaterial(
               "wallsAds/wallsAdsMaterial");

        wallsAdsMaterialTransparent = graphics.getMaterial(
               "wallsAds/wallsAdsMaterialTransparent");

        wallsAdsTopMaterialTransparent = graphics.getMaterial(
               "wallsAds/wallsAdsColorMaterialTransparent");
    }

    private void intGroundMaterials(int groundNum)
    {      
        int randGround = 
            UnityEngine.Random.Range(0, groundFilesNames.GetLength(0));


        if (Globals.numGameOpened <= 1 &&
            Globals.numMatchesInThisSession < 2)
            randGround = 0;
        //if c)
        //    randGround = 2;
        //TOREMOVE
        //randGround = 2;
        //if (groundNum != -1)
        //    randGround = groundNum;

        //print("RANDGROUND " + randGround + " Globals.numGameOpened " +
        //    Globals.numGameOpened + " Globals.numMatchesInThisSession " + Globals.numMatchesInThisSession);
        GameObject stadium = GameObject.Find("stadium_grass");

        Material groundMaterial = 
            graphics.getMaterial("Ground/Material/" + groundFilesNames[randGround, 0]);

        Texture2D groundTexture =
            graphics.getTexture("Ground/Texture/" + groundFilesNames[randGround, 0]);
       
        //print("GROUNDMATERIAL " + groundTexture + 
        //    " groundFilesNames[randGround, 0] " + groundFilesNames[randGround, 0]);

        graphics.setMaterialElement(stadium, groundMaterial, 0);

        //Color color;
        //ColorUtility.TryParseHtmlString(groundFilesNames[randGround, 1], out color);
        //groundMaterial.SetColor("_Color", color);

        //TODO
        //Material[] materials = stadium.GetComponent<Renderer>().materials;
        //materials[0].mainTexture = groundTexture;
        //materials[0].SetColor("_Color", color);
        //stadium.GetComponent<Renderer>().materials = materials;
    }

    private void setWallsTopColors()
    {
        Color color;
        ColorUtility.TryParseHtmlString("#cd1b1b", out color);
        //color.a = 0.7f;
        wallsAdsTopMaterialColors[0] = color;

        ColorUtility.TryParseHtmlString("#0A174B", out color);
        //color.a = 0.7f;
        wallsAdsTopMaterialColors[1] = color;

        ColorUtility.TryParseHtmlString("#cd1b1b", out color);
        //color.a = 0.7f;
        wallsAdsTopMaterialColors[2] = color;

        ColorUtility.TryParseHtmlString("#dfb60a", out color);
        //color.a = 0.7f;
        wallsAdsTopMaterialColors[3] = color;
    }

    private void setWallsAdsTexture()
    {
        /*wallsAdsTexture[0] = graphics.getTexture(
                "wallsAds/bilboardLogo");
        wallsAdsTexture[1] = graphics.getTexture(
            "wallsAds/bilboardRespect");
        wallsAdsTexture[2] = graphics.getTexture(
            "wallsAds/bilboardOSystems");
        wallsAdsTexture[3] = graphics.getTexture(
            "wallsAds/bilboardEnjoy");*/

        wallsAdsTexture[0] = graphics.getTexture(
            "wallsAds/banner1");
        wallsAdsTexture[1] = graphics.getTexture(
            "wallsAds/banner2");
        wallsAdsTexture[2] = graphics.getTexture(
            "wallsAds/banner3");
        wallsAdsTexture[3] = graphics.getTexture(
            "wallsAds/banner4");
    }

    private void setPlayersTextures()
    {
        int teamId = Globals.teamAid;
        string playerDesc = Globals.playerADesc;
        string leagueName = Globals.teamAleague;

        //print("DBG set player texture " +
        //    " A " + Globals.playerADesc +
        //    " B " + Globals.playerBDesc);

        //print("DBGFRIENDLY Globals.teamBid setTextures " + Globals.teamBid);
        for (int i = 0; i < MAX_MODELS_PLAYERS; i++)
        {
            {
                if (i == 0 && Globals.playerPlayAway)
                {
                    teamId = Globals.teamBid;
                    playerDesc = Globals.playerBDesc;
                    leagueName = Globals.teamBleague;
                }

                if (i == 1) {
                    if (Globals.playerPlayAway)
                    {
                        teamId = Globals.teamAid;
                        playerDesc = Globals.playerADesc;
                        leagueName = Globals.teamAleague;
                    }
                    else
                    {
                        teamId = Globals.teamBid;
                        playerDesc = Globals.playerBDesc;
                        leagueName = Globals.teamBleague;
                    }
                }

                //print("DBG set player " + playerDesc + " I " + i);

                ///if (!isTrainingActive)
                if (!Globals.onlyTrainingActive)
                {
                    graphics.setPlayerTextures(
                        playerModels[i],
                        playerModelsHair[i],
                        teamId,
                        leagueName,
                        playerDesc,
                        true,
                        true,
                        null);

                    //print("#DBGFRIENDLY " + teamId + " leagueNme " + leagueName + " playerDesc " + playerDesc);

                } else
                {
                    graphics.setPlayerTextures(
                     playerModels[i],
                     playerModelsHair[i],
                     teamId,
                     "NATIONALS",
                     playerDesc,
                     true,
                     true,
                     null);
                }          
            }
        }
    }

    public void setWallsMaterial(int materialNameIdx, int elementIdx)
    {

        wallsAdsMaterial.mainTexture = wallsAdsTexture[materialNameIdx];
        wallsAdsMaterialTransparent.mainTexture = wallsAdsTexture[materialNameIdx];

        wallsAdsTopMaterialColors[materialNameIdx].a = 1f;
        graphics.setMaterialColor(wallsAdsTopMaterial,
                                  wallsAdsTopMaterialColors[materialNameIdx]);

        //wallsAdsTopMaterialColors[materialNameIdx].a = 0.7f;
        wallsAdsTopMaterialColors[materialNameIdx].a = 0.55f;
        graphics.setMaterialColor(wallsAdsTopMaterialTransparent,
                                  wallsAdsTopMaterialColors[materialNameIdx]);
    }

    private void updateTrainingSettings()
    {
        if (!Globals.onlyTrainingActive)
            return;

        Teams teams = new Teams("NATIONALS");

        for (int i = 0; i < 2; i++)
        {
            int teamRandomInt = 
                UnityEngine.Random.Range(0, teams.getMaxTeams());
            string[] randTeam = 
                teams.getTeamByIndex(teamRandomInt);
            
            if (i == 0)
            {
                Globals.teamAname = randTeam[0];
                //Globals.teamAid = teamRandomInt;
                Globals.teamAid = teamRandomInt;
                Globals.stadiumColorTeamA = randTeam[5];
                Globals.playerADesc = randTeam[12].Split('|')[0];
            }
            else
            {
                Globals.teamBname = randTeam[0];
                Globals.teamBid = teamRandomInt;
                Globals.stadiumColorTeamB = randTeam[5];
                Globals.playerBDesc = randTeam[12].Split('|')[0];
            }

            teams.swapElements(
                teamRandomInt, teams.getMaxActiveTeams() - 1);
        }

        /*Traning hardcoded values*/
        Globals.teamAcumulativeStrength = 160;
        Globals.teamBcumulativeStrength = 140;

        Globals.teamAGkStrength = 90;
        Globals.teamBGkStrength = 70;

        Globals.teamAAttackStrength = 70;
        Globals.teamBAttackStrength = 70;

        Globals.matchTime = "2000 minutes";
        Globals.maxTimeToShotStr = "5000";

        Globals.level = 3;
    }

    private void updateBonusSettings()
    {
        if (isBonusActive)
            return;

        Teams teams = new Teams("NATIONALS");

        for (int i = 0; i < 2; i++)
        {
            int teamRandomInt =
                UnityEngine.Random.Range(0, teams.getMaxTeams());
            string[] randTeam =
                teams.getTeamByIndex(teamRandomInt);

            if (i == 0)
            {
                Globals.teamAname = randTeam[0];
                //Globals.teamAid = teamRandomInt;
                Globals.teamAid = teamRandomInt;
                Globals.stadiumColorTeamA = randTeam[5];
                Globals.playerADesc = randTeam[12].Split('|')[0];
            }
            else
            {
                Globals.teamBname = randTeam[0];
                Globals.teamBid = teamRandomInt;
                Globals.stadiumColorTeamB = randTeam[5];
                Globals.playerBDesc = randTeam[12].Split('|')[0];
            }

            teams.swapElements(
                teamRandomInt, teams.getMaxActiveTeams() - 1);
        }

        /*Traning hardcoded values*/
        Globals.teamAcumulativeStrength = 180;
        Globals.teamBcumulativeStrength = 180;

        Globals.teamAGkStrength = 90;
        Globals.teamBGkStrength = 70;

        Globals.teamAAttackStrength = 85;
        Globals.teamBAttackStrength = 85;
        Globals.maxTimeToShotStr = "5000";
        Globals.matchTime = "2000 minutes";
        Globals.level = 3;
    }

}

