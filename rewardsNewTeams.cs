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
using AudioManagerNS;
using LANGUAGE_NS;

public class rewardsNewTeams : MonoBehaviour
{
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    //private NationalTeams teams = new NationalTeams();
    private Teams teams = new Teams("NATIONALS");
    private string teamName;
    private List<int> unlockedTeamsList;
    private int[] numberOfListElements;
    private RawImage teamFlagImage;
    private TextMeshProUGUI teamNameText;
    private TextMeshProUGUI teamDefenseSkillText;
    private TextMeshProUGUI teamAttackSkillText;
    private Button rewardNextCanvasButton;
    private StringCommon strCommon;
    private int activePrize = 0;
    private int prizeMax;
    private string[] prizeNames;

    public GameObject teamFlagImageGameObj;
    public GameObject rewardTeamFlagBackground;
    public GameObject teamNameTextGameObj;
    public GameObject teamDefenseSkillBarBackGameObj;
    public GameObject teamAttackSkillBarBackGameObj;
    public GameObject teamDefenseSkillBarGameObj;
    public GameObject teamAttackSkillBarGameObj;
    public GameObject teamDefenseSkillTextGameObj;
    public GameObject teamAttackSkillTextGameObj;
    public GameObject rewardNextCanvas;
    private int currentIdx = 0;
    private GameObject model;
    private GameObject modelHair;
    private GraphicsCommon graphics;
    private AudioManager audioManager;
    public GameObject[] playerModelCanvas;
    public GameObject playerModel;
    public GameObject playerModelHair;
    public string[][] playerModelItems;
    public TextMeshProUGUI prizeHeaderText;
    public GameObject unlockItemCard;
    string[] playerItems = new string[]
            { "tshirt", 
              "tshirt", 
              "shorts", 
              "shoes", 
              "gloves", 
              "socks", 
              "socks",
              "socks",
              "skin", 
              "skin", 
              "skin", 
              "skin",
              "skin"};

    void Start()
    {
        graphics = new GraphicsCommon();
        strCommon = new StringCommon();
        Time.timeScale = 1f;

        admobGameObject = GameObject.Find("admobAdsGameObject");
        if (admobGameObject != null)
            admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();

        model = GameObject.Find("modelMain");
        modelHair = GameObject.Find("modelHairMain");

        initPlayerTexture();
        initReferences();
        initModelCanvas();
        initModelItems();

        graphics.setAllShaderByName(playerModel, "UI/Lit/Transparent");

        int numberOfPrizes = 1;
        //if (UnityEngine.Random.Range(0, 8) == 1)
        //    numberOfPrizes = 1;

        prizeNames = new string[numberOfPrizes];
        //for (int i = 0; i < prizeNames.Length; i++)
        //{
        prizeNames[0] = playerItems[UnityEngine.Random.Range(0, playerItems.Length)];
        if (!isPrizeAvaiable(prizeNames[0]))
                numberOfPrizes--;
        //}

        numberOfListElements = new int[2];

  
        /*take first unlocked team*/
        numberOfListElements[0] = teams.getJustUnlockedTeamsIndexes().Count;

        unlockedTeamsList = teams.getJustUnlockedTeamsIndexes();
        teams.saveJustUnlockedTeamsToPrefabs();

        if (unlockedTeamsList.Count > 0)
        {
            activePrize = 0;

            if (numberOfPrizes == 0)
            {
                prizeMax = 1;
            }
            else
            {
                numberOfListElements[1] = numberOfPrizes;
                prizeMax = 2;
            }
            StartCoroutine(showTeam());
        }
        else
        {
            if (numberOfPrizes == 0)
            {
                disableCanvasElements();        
                model.SetActive(false);
                modelHair.SetActive(false);
                prizeHeaderText.text = "";
                loadNextScene();
                return;
            }

            activePrize = 0;
            prizeMax = 1;
            numberOfListElements[0] = numberOfPrizes;

            disableCanvasElements();
            StartCoroutine(showPrize());
        }

        showBanner();
    }

    private IEnumerator showTeam()
    {
        prizeHeaderText.text =
            Languages.getTranslate("Congratulations! New National Team Unlocked");
      
        updateTeamUI(unlockedTeamsList[currentIdx]);

        string playerSkinHairDesc =
           teams.getPlayerDescByIndex(unlockedTeamsList[currentIdx], 0);

        graphics.setPlayerTextures(
            model, 
            modelHair, 
            unlockedTeamsList[currentIdx],
            "NATIONALS",
            playerSkinHairDesc,            
            true,
            true,
            null);

        currentIdx++;
        disableCanvasElements();

        //yield return new WaitForSeconds(0.5f);
        //audioManager.Play("elementAppear");
        model.SetActive(true);
        modelHair.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        audioManager.Play("elementAppear");
        setCanvasElements(true);
        yield return new WaitForSeconds(1.0f);
        rewardNextCanvas.SetActive(true);
    }

    private void initPlayerTexture()
    {
      /*  Globals.customizePlayerShirt = "blackwithwhiteshirt";
        Globals.customizePlayerShorts = "whitegoldstripesshort";
        Globals.customizePlayerSocks = "sock_white";
        Globals.customizePlayerGloves = "globewhiteblack";
        Globals.customizePlayerSkinHair = "f0_s1_b0_t0_hblack1";
        Globals.customizePlayerShoe = "shoewhite";
        Globals.customizeFansColor = "blue";
        Globals.customizeFlagColor = "blue";*/

        string teamDesc =
                     Globals.customizePlayerShirt + "|" +
                     Globals.customizePlayerShorts + "|" +
                     Globals.customizePlayerSocks + "|" +
                     Globals.customizePlayerSkinHair + "|" +
                     Globals.customizePlayerGloves + "|" +
                     Globals.customizePlayerShoe + "|" +
                     Globals.customizeFansColor + "|" +
                     Globals.customizeFlagColor;

        graphics.setPlayerTextures(
                            model,
                            modelHair,
                            "null",
                            teamDesc);
    }

    private IEnumerator showPrize()
    {
        //toRemove
        prizeHeaderText.text =
           Languages.getTranslate("Congratulations! New prize unlocked");

        initPlayerTexture();

        disableCanvasElements();

        model.SetActive(true);
        modelHair.SetActive(true);

        currentIdx++;

        //prizeHeaderText.text =
        //       Languages.getTranslate("Congratulations! New prize unlocked");


        /*graphics.setMaterialByName(playerModel, 
                                   "playerMaterials/" + playerModelItems[4][2], 
                                   new int[] { 1, 2, 3 });

        graphics.setShaderByName(playerModel, "Standard", new int[] { 1 });
        graphics.setShaderByName(playerModel, "DecalUV2", new int[] { 2, 3 });
        disableAllPlayerModelCanvasBut(4);*/



        /*graphics.setMaterialByName(playerModel, 
                                   "playerMaterials/" + playerModelItems[4][2], 
                                   new int[] { 1, 2, 3 });

        graphics.setShaderByName(playerModel, "Standard", new int[] { 1 });
        graphics.setShaderByName(playerModel, "DecalUV2", new int[] { 2, 3 });
        disableAllPlayerModelCanvasBut(4);*/

        /*string skinName = playerModelItems[5][2];
        print("skinName " + skinName);

        int delimeterIndex =
                 strCommon.getIndexOfNOccurence(skinName, '_', 4);

        string skinFileName = "skin_" +
                        Globals.getSkinHairColorName(
                            skinName.Substring(0, delimeterIndex));
        string hairColorName =
                         skinName.Substring(delimeterIndex + 1);
    
        graphics.setMaterialByName(
              playerModel,
              "playerMaterials/skins/" + skinFileName,
              0);
        graphics.setPlayerTexturesHair(
           playerModelHair,
           hairColorName);

        graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 0 });
        disableAllPlayerModelCanvasBut(5);

        string teamDesc =
                             Globals.customizePlayerShirt + "|" +
                             Globals.customizePlayerShorts + "|" +
                             Globals.customizePlayerSocks + "|" +
                             Globals.customizePlayerSkinHair + "|" +
                             Globals.customizePlayerGloves + "|" +
                             Globals.customizePlayerShoe + "|" +
                             Globals.customizeFansColor + "|" +
                             Globals.customizeFlagColor;

        Debug.Log("teamDesc " + teamDesc);

        graphics.setPlayerTextures(
                   model,
                   modelHair,
                   "null",
                   teamDesc);

        Debug.Log("teamDesc " + teamDesc);
        */

        yield return new WaitForSeconds(1.0f);

        unlockItemCard.SetActive(true);

        audioManager.Play("elementAppear");
        displayRandomPrize();
        //setCanvasElements(true);
        yield return new WaitForSeconds(1.0f);
        rewardNextCanvas.SetActive(true);

        //disableCanvasElements();

        yield break;
        //yield return new WaitForSeconds(0.5f);
        //audioManager.Play("elementAppear");
        /*model.SetActive(true);
        modelHair.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        audioManager.Play("elementAppear");
        setCanvasElements(true);
        yield return new WaitForSeconds(1.0f);
        disableCanvasElements();
        rewardNextCanvas.SetActive(true);*/
    }

    private string lastItemName;
    private int lastItemIdx;
    private bool displayRandomPrize()
    {
        /*string name = playerItems[UnityEngine.Random.Range(0, playerItems.Length)];
        if (Globals.numGameOpen == 0 &&
            (Globals.numMatchesInThisSession <= 3))
        {
            if (Globals.numMatchesInThisSession == 1)
                name = "tshirt";
            else if (Globals.numMatchesInThisSession == 2)
            {
                name = "shoes";
            } else if (Globals.numMatchesInThisSession == 3)
            {
                name = "shorts";
            }
        }*/
        //todo
        name = prizeNames[0];

        if (name.Equals("tshirt"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[4].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[4][i]))
                {
                    continue;
                } else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

            graphics.setMaterialByName(playerModel,
                                       "playerMaterials/" + playerModelItems[4][itemFoundIdx],
                                       new int[] { 1, 2, 3, 4 });

            graphics.setMaterialByName(model,
                                       "playerMaterials/" + playerModelItems[4][itemFoundIdx],
                                        new int[] { 1, 2, 3, 4 });

            graphics.setShaderByName(playerModel, "Standard", new int[] { 1 });
            graphics.setShaderByName(playerModel, "DecalUV2", new int[] { 2, 3, 4 });

            lastItemName = playerModelItems[4][itemFoundIdx];
            lastItemIdx = itemFoundIdx;
            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[4][itemFoundIdx], 1);
            PlayerPrefs.Save();

            disableAllPlayerModelCanvasBut(4);
        }
        else if (name.Equals("skin"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[5].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[5][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

            /*graphics.setMaterialByName(playerModel,
                                       "playerMaterials/" + playerModelItems[5][itemFoundIdx],
                                       0);

            graphics.setMaterialByName(model,
                                       "playerMaterials/" + playerModelItems[5][itemFoundIdx],
                                       0);*/

            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[5][itemFoundIdx], 1);
            PlayerPrefs.Save();

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 0 });

            lastItemName = playerModelItems[5][itemFoundIdx];
            lastItemIdx = itemFoundIdx;


            string skinName = playerModelItems[5][itemFoundIdx];

            int delimeterIndex =
                     strCommon.getIndexOfNOccurence(skinName, '_', 4);

            string skinFileName = "skin_" +
                        Globals.getSkinHairColorName(
                            skinName.Substring(0, delimeterIndex));
            string hairColorName =
                         skinName.Substring(delimeterIndex + 1);

            Debug.Log("skinFileName " + skinFileName + " hairColorName " + hairColorName);

            graphics.setMaterialByName(
                  playerModel,
                  "playerMaterials/skins/" + skinFileName,
                  0);
            graphics.setMaterialByName(
                model,
                "playerMaterials/skins/" + skinFileName,
                0);

            graphics.setPlayerTexturesHair(
                playerModelHair,
                hairColorName);

            graphics.setPlayerTexturesHair(
                modelHair,
                hairColorName);

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 0 });
            disableAllPlayerModelCanvasBut(5);

            /*string teamDesc =
                             Globals.customizePlayerShirt + "|" +
                             Globals.customizePlayerShorts + "|" +
                             Globals.customizePlayerSocks + "|" +
                             Globals.customizePlayerSkinHair + "|" +
                             Globals.customizePlayerGloves + "|" +
                             Globals.customizePlayerShoe + "|" +
                             Globals.customizeFansColor + "|" +
                             Globals.customizeFlagColor;

            graphics.setPlayerTextures(
                       model,
                       modelHair,
                       "null",
                       teamDesc);
            */
            disableAllPlayerModelCanvasBut(5);

        }
        else if (name.Equals("shorts"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[1].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[1][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;


            graphics.setMaterialByName(playerModel,
                           "playerMaterials/" + playerModelItems[1][itemFoundIdx],
                           5);

            graphics.setMaterialByName(model,
                                       "playerMaterials/" + playerModelItems[1][itemFoundIdx],
                                       5);

            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[1][itemFoundIdx], 1);
            PlayerPrefs.Save();

            lastItemName = playerModelItems[1][itemFoundIdx];
            lastItemIdx = itemFoundIdx;

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 5 });
            disableAllPlayerModelCanvasBut(1);
        }
        else if (name.Equals("shoes"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[2].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[2][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

            graphics.setMaterialByName(playerModel,
                           "playerMaterials/shoe/" + playerModelItems[2][itemFoundIdx],
                           6);

            graphics.setMaterialByName(model,
                          "playerMaterials/shoe/" + playerModelItems[2][itemFoundIdx],
                          6);

            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[2][itemFoundIdx], 1);
            PlayerPrefs.Save();

            lastItemName = playerModelItems[2][itemFoundIdx];
            lastItemIdx = itemFoundIdx;

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 6 });
            disableAllPlayerModelCanvasBut(2);
        }
        else if (name.Equals("gloves"))
        {
            print("DBGMODEL " + playerModelItems[3].Length);

            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[3].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[3][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

            print("Gloves name " + playerModelItems[3][itemFoundIdx]);

            graphics.setMaterialByName(playerModel,
                           "playerMaterials/glove/" + playerModelItems[3][itemFoundIdx],
                            8);

            graphics.setMaterialByName(model,
                        "playerMaterials/glove/" + playerModelItems[3][itemFoundIdx],
                         8);

            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[3][itemFoundIdx], 1);
            PlayerPrefs.Save();

            lastItemName = playerModelItems[3][itemFoundIdx];

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", new int[] { 8 });
            disableAllPlayerModelCanvasBut(3);
        } else if (name.Equals("socks"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[0].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[0][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

            graphics.setMaterialByName(playerModel,
                           "playerMaterials/" + playerModelItems[0][itemFoundIdx],
                           7);

            graphics.setMaterialByName(model,
                     "playerMaterials/" + playerModelItems[0][itemFoundIdx],
                     7);

            PlayerPrefs.SetInt("CustomizeTeamItem_" + playerModelItems[0][itemFoundIdx], 1);
            PlayerPrefs.Save();

            lastItemName = playerModelItems[0][itemFoundIdx];
            lastItemIdx = itemFoundIdx;

            graphics.setShaderByName(playerModel, "Legacy Shaders/Diffuse", 7);
            disableAllPlayerModelCanvasBut(0);
        }

        return true;
    }

    private bool isPrizeAvaiable(string name)
    {
        if (name.Equals("tshirt"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[4].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[4][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;           
        }
        else if (name.Equals("skin"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[5].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[5][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;

        }
        else if (name.Equals("shorts"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[1].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[1][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;
          
        }
        else if (name.Equals("shoes"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[2].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[2][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;          
        }
        else if (name.Equals("gloves"))
        {
            print("DBGMODEL " + playerModelItems[3].Length);

            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[3].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[3][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;           
        }
        else if (name.Equals("socks"))
        {
            int itemFoundIdx = -1;
            for (int i = 0; i < playerModelItems[0].Length; i++)
            {
                if (PlayerPrefs.HasKey("CustomizeTeamItem_" + playerModelItems[0][i]))
                {
                    continue;
                }
                else
                {
                    itemFoundIdx = i;
                }
            }

            if (itemFoundIdx == -1)
                return false;
            
        }

        return true;
    }


    private void disableAllPlayerModelCanvasBut(int idx)
    {
        for (int i = 0; i < playerModelCanvas.Length; i++)
        {
            if (i == idx)
                playerModelCanvas[i].SetActive(true);
            else
                playerModelCanvas[i].SetActive(false);
        }
    }

    public void loadNextScene()
    {
        if (admobAdsScript != null) 
            admobAdsScript.hideBanner();

        if (Globals.isMultiplayer)
        {
            Globals.loadSceneWithBarLoader("multiplayerMenu");
        }
        else if (Globals.isFriendly ||
                 Globals.gameEnded)
        {
            //Globals.wonFinal)
            Globals.loadSceneWithBarLoader("menu");
        }
        else
            Globals.loadSceneWithBarLoader("Leagues");
        //SceneManager.LoadScene("Leagues");
    }

    public void nextButton()
    {
        if (currentIdx == numberOfListElements[activePrize])
        {
            activePrize++;
            currentIdx = 0;
        }
        
        if (activePrize == prizeMax) {
            loadNextScene();
        }
        else
        {
            if (activePrize == 0)
                StartCoroutine(showTeam());

            if (activePrize == 1)
                StartCoroutine(showPrize());
        }
    }

    private void updateTeamUI(int idx)
    {
        string[] team = teams.getTeamByIndex(idx);
        string teamName = team[0];
        teamNameText.text = teamName;
        teamName = Regex.Replace(teamName, "\\s+", "");
        teamFlagImage.texture = 
            Resources.Load<Texture2D>("Flags/national/" + teamName.ToLower()); 
        teamDefenseSkillText.text = "Defense: " + team[1];
        teamAttackSkillText.text = "Attack: " + team[2];

        teamDefenseSkillBarGameObj.GetComponent<Image>().fillAmount = 
            float.Parse(team[1]) / 100.0f;
        teamAttackSkillBarGameObj.GetComponent<Image>().fillAmount = 
            float.Parse(team[2]) / 100.0f;
    }

    private void initReferences()
    {
        audioManager = FindObjectOfType<AudioManager>();
        teamFlagImage = teamFlagImageGameObj.GetComponent<RawImage>();
        teamNameText = teamNameTextGameObj.GetComponent<TextMeshProUGUI>();
        teamDefenseSkillText = teamDefenseSkillTextGameObj.GetComponent<TextMeshProUGUI>();
        teamAttackSkillText = teamAttackSkillTextGameObj.GetComponent<TextMeshProUGUI>();
        rewardNextCanvasButton = rewardNextCanvas.GetComponent<Button>();
    }

    private void initModelCanvas()
    {
        for (int i = 0; i < playerModelCanvas.Length; i++)
            playerModelCanvas[i].SetActive(false);
    }

    private void initModelItems()
    {
        unlockItemCard.SetActive(false);

        playerModelItems = new string[playerModelCanvas.Length][];
        playerModelItems[0] = CustomizePlayer.sockFileNames;
        playerModelItems[1] = CustomizePlayer.shortsFileNames;
        playerModelItems[2] = CustomizePlayer.shoesFilesNames;
        playerModelItems[3] = CustomizePlayer.glovesFilesNames;
        playerModelItems[4] = CustomizePlayer.shirtsFileNames;
        playerModelItems[5] = CustomizePlayer.skinsFileNames;

        //for (int i = 0; i < playerModelItems.Length; i++)
        //{
        //    Debug.Log("#DBG size " + i + " val " + playerModelItems[i].Length + " name " +
        //        playerModelItems[i][UnityEngine.Random.Range(0, playerModelItems[i].Length)]);
        //}
    }

  
    private void disableCanvasElements()
    {
        teamFlagImageGameObj.SetActive(false);
        rewardTeamFlagBackground.SetActive(false);
        teamNameTextGameObj.SetActive(false);
        teamDefenseSkillTextGameObj.SetActive(false);
        teamAttackSkillTextGameObj.SetActive(false);
        teamDefenseSkillBarBackGameObj.SetActive(false);
        teamAttackSkillBarBackGameObj.SetActive(false);
        teamDefenseSkillBarGameObj.SetActive(false);
        teamAttackSkillBarGameObj.SetActive(false);
        rewardNextCanvas.SetActive(false);
        //model.SetActive(false);
        //modelHair.SetActive(false);
    }

    private void setCanvasElements(bool value)
    {        
        teamFlagImageGameObj.SetActive(value);
        rewardTeamFlagBackground.SetActive(value);
        teamNameTextGameObj.SetActive(value);
        teamDefenseSkillBarBackGameObj.SetActive(value);
        teamAttackSkillBarBackGameObj.SetActive(value);
        teamDefenseSkillBarGameObj.SetActive(value);
        teamAttackSkillBarGameObj.SetActive(value);
        teamDefenseSkillTextGameObj.SetActive(value);
        teamAttackSkillTextGameObj.SetActive(value);
        rewardNextCanvas.SetActive(value);
        model.SetActive(value);
        modelHair.SetActive(value);
    }

    public void onClickUseItemButton()
    {
        if (lastItemName.Contains("shirt"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerShirt", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentShirtIdx", lastItemIdx);
            Globals.customizePlayerShirt = lastItemName;
        }
        else if (lastItemName.Contains("short"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerShorts", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentShortsIdx", lastItemIdx);
            Globals.customizePlayerShorts = lastItemName;
        }
        else if (lastItemName.Contains("sock"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerSock", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentSockIdx", lastItemIdx);
            Globals.customizePlayerSocks = lastItemName;
        }
        else if (lastItemName.Contains("glove"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerGloves", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentSGlovesIdx", lastItemIdx);

            Globals.customizePlayerGloves = lastItemName;
        } 
        else if (lastItemName.Contains("shoe"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerShoe", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentShoeIdx", lastItemIdx);

            Globals.customizePlayerShoe = lastItemName;
        }
        else if (lastItemName.Contains("_hblack") ||
                lastItemName.Contains("_hblonde") ||
                lastItemName.Contains("_hred") ||
                lastItemName.Contains("_hnohair"))
        {
            audioManager.Play("elementAppear");
            PlayerPrefs.SetString("CustomizeTeam_customizePlayerSkinHair", lastItemName);
            PlayerPrefs.SetInt("CustomizeTeam_currentSkinIdx", lastItemIdx);
            Globals.customizePlayerSkinHair = lastItemName;
        }

        PlayerPrefs.Save();
        nextButton();
    }

    private void showBanner()
    {
        if (admobGameObject != null &&
            admobAdsScript != null)
        {
            admobAdsScript.showBannerAd();
        }
    }
}
