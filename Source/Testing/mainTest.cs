using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuCustomNS;
using GlobalsNS;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using graphicsCommonNS;
using UnityEngine.UI;

public class mainTest : MonoBehaviour
{
    private Teams teams;
    private GraphicsCommon graphics;
    private StringCommon strCommon;

    private int PLAYER_DESC_COL = 7;

    public static string[] flagsFoldersNames =
            { "NATIONAL", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMPCUP"};

    public static Dictionary<string, string> teamLeagueName =
        new Dictionary<string, string>();

    //Clear all prefs before starting this test
    void Start()
    {
        strCommon = new StringCommon();
        graphics = new GraphicsCommon();

        initTeamLeagueNameHash();
        string[] allLeagues = Globals.allLeaguesNames;
        teams = new Teams();
        //checkPlayerCardCorrectionOfStructure(allLeagues);
        checkPlayerNameDuplications();
        checkPlayersCardCorrectionOfStructure();
        checkTeamsStructureCorrection();

        print("#TESTENDED####");
    }

    void Update()
    {
        
    }

    private void checkPlayerNameDuplications()
    {
        string[] leaguesNames =
            { "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMPCUP"};
        string[] teamDesc;
        string[] playerDesc;
        Dictionary<string, string> playersDict =
                new Dictionary<string, string>();

        Dictionary<string, string> teamAlreadyChecked =
                new Dictionary<string, string>();

        Teams teams = new Teams();
        string leagueName = String.Empty;
        string teamName = String.Empty;

        for (int i = 0; i < leaguesNames.Length; i++)
        {
            teams.setLeague(leaguesNames[i]);
            for (int j = 0; j < teams.getMaxTeams(); j++)
            {
                teamDesc = teams.getTeamByIndex(j);
                teamName = teamDesc[0];
                playerDesc = teamDesc[12].Split('|');
                
                if (teamAlreadyChecked.ContainsKey(teamName))
                    continue;

                print("#DBG_PLAYENAME_DUPLICATION Checking team " + teamName);

                for (int x = 0; x < playerDesc.Length; x++)
                {
                    string playerName = playerDesc[x].Split(':')[0];
                    try
                    {
                        playersDict.Add(playerName, teamName);
                    } catch(ArgumentException e)
                    {
                        string oldTeamName;
                        playersDict.TryGetValue(playerName, out oldTeamName);

                        print("#DBG_ERROR_PLAYERCARD_DUPLICATION " + teamName
                               + " LeagueName " + leaguesNames[i]
                               + " playerName " + playerName
                               + " OldTeam " + oldTeamName);
                    }


                }

                teamAlreadyChecked.Add(teamName, "true");
            }
        }

        print("#DBG_END OF PLAYERS CARD DUPLICATRION TEST");
    }

    private void checkPlayersCardCorrectionOfStructure()
    {
        string[] leaguesNames =
            { "NATIONALS", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMPCUP"};
        string[] teamDesc;
        string[] playersDesc;
        Regex rgx;

        Dictionary<string, string> playersDict =
                new Dictionary<string, string>();

        Dictionary<string, string> teamAlreadyChecked =
                new Dictionary<string, string>();

        Teams teams = new Teams();
        string leagueName = String.Empty;
        string teamName = String.Empty;

        for (int i = 0; i < leaguesNames.Length; i++)
        {
            leagueName = leaguesNames[i];
            teams.setLeague(leagueName);            
            for (int j = 0; j < teams.getMaxTeams(); j++)
            {
                teamDesc = teams.getTeamByIndex(j);
                teamName = teamDesc[0];
                playersDesc = teamDesc[12].Split('|');

                if (teamName.Contains("CUSTOMIZE") ||
                    teamName.Contains("Customize"))
                    continue;

                //if (teamAlreadyChecked.ContainsKey(teamName))
                //    continue;

                print("#DBG_PLAYENAME_DUPLICATION Checking team " + teamName);

                for (int x = 0; x < playersDesc.Length; x++)
                {
                    string[] plrDesc = playersDesc[x].Split(':');
                    string playerName = plrDesc[0];
                    string playerCountry = plrDesc[1];
                    string defense = plrDesc[2];
                    string attack = plrDesc[3];
                    string isLocked = plrDesc[4];
                    string price = plrDesc[5];
                    string skinHairColorName = plrDesc[6];

                    print("#DBGDESCRIPTION " + teamName + "|" + playerName + " " + playerCountry +
                         " " + defense + " " + attack + " "
                         + isLocked
                         + " "
                         + price
                         + " "
                         + skinHairColorName);

                    bool result = playerName.All(c => Char.IsLetter(c) || c == ' ');
                    if (result == false)
                    {
                        print("#DBG_ERROR_PLAYERCARD_PLAYENAME_NOT_ONLY_LETTER " 
                            + playerName + " teamName " + teamName);
                    }

                    if (playerName.Length > 20)
                    {
                        print("#DBG_ERROR_PLAYERCARD_PLAYENAME_TOO_LONG "
                           + playerName + " teamName " + teamName + " League " + leagueName);
                    }

                    rgx = new Regex(@"^[A-Z][a-z]+( [A-Z][a-z]+)?$");
                    if (!rgx.IsMatch(playerName))
                    {
                        print("#DBG_ERROR_PLAYERCARD_PLAYENAME_WRONG_NAME_REGEX "
                                + playerName + " teamName " + teamName + " League " + leagueName);
                    }

                    result = playerCountry.All(c => Char.IsLetter(c) || c == ' ');
                    if (result == false)
                    {
                        print("#DBG_ERROR_PLAYERCARD_COUNTRY_NOT_ONLY_LETTER " 
                            + playerName + " teamName " 
                            + teamName + " playerCountry " + playerCountry + " League " + leagueName);
                    }

                    rgx = new Regex(@"^[A-Z][a-z]+( [A-Z][a-z]+)?$");
                    if (!playerCountry.Equals("USA") && 
                        !rgx.IsMatch(playerCountry))
                    {
                        print("#DBG_ERROR_PLAYERCARD_COUNTRY_WRONG_NAME_REGEX "
                                + playerName + " teamName " + teamName
                                + " playerCountry " + playerCountry
                                + " League " + leagueName);
                    }


                    string plrNationalFlagPath = "Flags/" +
                                   Globals.getFlagPath("NATIONALS", playerCountry);

                    plrNationalFlagPath = Regex.Replace(plrNationalFlagPath, "\\s+", "");
                    if (Resources.Load<Texture2D>(plrNationalFlagPath) == null)
                    {
                        print("#DBG_ERROR_PLAYERCARD_COUNTRY_FLAG_FILE_NOT_FOUND "
                               + playerName + " teamName " + teamName
                               + " playerCountry " + plrNationalFlagPath
                               + " League " + leagueName);
                    }

                    result = defense.All(Char.IsDigit);
                    if (result == false)
                    {
                        print("#DBG_ERROR_PLAYERCARD_DEFENSE_NOT_ONLY_DIGIT "
                            + playerName + " teamName " 
                            + teamName + " Defense " 
                            + defense
                            + " League " 
                            + leagueName);
                    }

                    result = attack.All(Char.IsDigit);
                    if (result == false)
                    {
                        print("#DBG_ERROR_PLAYERCARD_ATTACK_NOT_ONLY_DIGIT "
                            + playerName + " teamName " 
                            + teamName 
                            + " attack " 
                            + attack
                            + " League "
                            + leagueName);
                    }

               
                    if (!int.TryParse(defense, out int convDefToInt))
                    {
                        print("#DBG_ERROR_PLAYERCARD_DEFENSE IS NOT INT "
                            + playerName 
                            + " teamName " 
                            + teamName 
                            + " defense " 
                            + defense
                            + " League "
                            + leagueName);
                    }

                    if (!int.TryParse(attack, out int convAttToInt))
                    {
                        print("#DBG_ERROR_PLAYERCARD_DEFENSE IS NOT INT "
                           + playerName 
                           + " teamName " 
                           + teamName 
                           + " attack " 
                           + attack
                           + " League "
                           + leagueName);
                    }

                    int def = int.Parse(defense);
                    int att = int.Parse(attack);

                    if (def < Globals.MIN_PLAYER_SKILLS || Globals.MAX_PLAYER_SKILLS > 100)
                    {
                        print("#DBG_ERROR_PLAYERCARD_DEFENSE VAL TOO BIG "
                            + playerName + " teamName " + teamName + " attack " + def
                            + " League " + leagueName);
                    }

                    if (att < Globals.MIN_PLAYER_SKILLS || att > Globals.MAX_PLAYER_SKILLS)
                    {
                        print("#DBG_ERROR_PLAYERCARD_DEFENSE VAL TOO BIG "
                            + playerName + " teamName " 
                            + teamName + " attack " 
                            + att
                            + " League " 
                            + leagueName);
                    }

                    if (!isLocked.Equals("U") &&
                        !isLocked.Equals("L"))
                    {
                        print("#DBG_ERROR_PLAYERCARD_ISLOCKED WRONG "
                            + playerName 
                            + " teamName " 
                            + teamName 
                            + " isLocked "
                            + isLocked
                            + " League "
                            + leagueName);
                    }

                    if (!int.TryParse(price, out int convPriceToInt))
                    {
                        print("#DBG_ERROR_PLAYERCARD_PRICE IS NOT INT "
                           + playerName + " teamName "
                           + teamName + " price " 
                           + price
                           + " League " 
                           + leagueName);
                    }

                    if (int.Parse(price) < 0 && int.Parse(price) > 10000)
                    {
                        print("#DBG_ERROR_PLAYERCARD PRICE TOO BIG "
                           + playerName 
                           + " teamName " 
                           + teamName 
                           + " price " 
                           + price
                           + " League "
                           + leagueName);
                    }

                    //junior should be for free and unlocked
                    if (x == 0)
                    {
                        if (isLocked.Equals("L"))
                            print("#DBG_ERROR_PLAYERCARD JUNIOR IS LOCKED "
                                + playerName + " teamName " 
                                + teamName + " isLocked " + isLocked
                                + " league " + leagueName);

                        if (int.Parse(price) != 0)
                        {
                            print("#DBG_ERROR_PLAYERCARD JUNIOR PLAYER WRONG PRICE "
                               + playerName + " teamName " 
                               + teamName + " price " + price
                               + " League "
                               + leagueName);
                        }
                    } else
                    {
                        if (isLocked.Equals("U"))
                        {
                            print("#DBG_ERROR_PLAYERCARD NORMAL PLAYER IS UNLOCKED "
                                + playerName + " teamName " 
                                + teamName + " isLocked " 
                                + isLocked
                                + " League " 
                                + leagueName);
                        }

                        if (int.Parse(price) == 0)
                        {
                            print("#DBG_ERROR_PLAYERCARD NORMAL PLAYER WRONG PRICE "
                               + playerName + " teamName " 
                               + teamName + " price " 
                               + price
                               + " League "
                               + leagueName);
                        }
                    }

                    string skinColorName = "";
                    string hairColorName = "";

                    int delimeterIndex =
                        strCommon.getIndexOfNOccurence(skinHairColorName, '_', 4);

                    skinColorName = "skin_" +
                        Globals.getSkinHairColorName(
                            skinHairColorName.Substring(0, delimeterIndex));

                    hairColorName =
                        skinHairColorName.Substring(delimeterIndex + 1);

                    string skinFullPath = "playerMaterials/skins/" + skinColorName;
                    Material skinMaterial =
                        graphics.getMaterial(skinFullPath);

                    if (skinMaterial == null)
                    {
                        print("#DBG_ERROR_PLAYERCARD SKIN MATERIAL DOES NOT EXISTS "
                           + playerName + " teamName " + teamName + " price " 
                           + skinMaterial 
                           + " skiFullPath " 
                           + skinFullPath
                           + " League "
                           + leagueName);
                    }

                    string hairFullPath =
                        "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName;

                    Material hairMaterial = graphics.getMaterial(hairFullPath);
                    if (!hairFullPath.Contains("hnohair") &&
                        hairMaterial == null)
                    {
                        print("#DBG_ERROR_PLAYERCARD HAIR MATERIAL DOES NOT EXISTS "
                           + playerName + " teamName " 
                           + teamName + " price " 
                           + hairMaterial
                           + " hairFullPath " 
                           + hairFullPath
                           + " League  "
                           + leagueName);
                    }


                    string hairMesh = "playerMaterials/hair/"
                            + hairColorName + "/" + hairColorName;
                    if (!hairMesh.Contains("hnohair") &&
                        Resources.Load<Mesh>(hairMesh) == null)
                    {
                        print("#DBG_ERROR_PLAYERCARD HAIR MESH DOES NOT EXISTS "
                           + playerName + " teamName " 
                           + teamName + " price " + hairFullPath
                           + " League "
                           + leagueName);
                    }

                    string playerCardPath = "playersCard/" + skinHairColorName;
                    if (!playerCardPath.Contains("hnohair") &&
                        Resources.Load<Texture2D>(playerCardPath) == null)
                    {
                        print("#DBG_ERROR_PLAYERCARD CANNOT FIND PLAYER CARD "
                           + playerName + " teamName " 
                           + teamName 
                           + " Card Path " 
                           + playerCardPath
                           + " League "
                           + leagueName);
                    }
                }
            }
        }

        print("#DBG_END OF PLAYERS CARD DUPLICATRION TEST");
    }

    private void checkTeamsStructureCorrection()
    {
        string[] leaguesNames =
            { "NATIONALS", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMPCUP"};
        string[] teamDesc;
        string[] playersDesc;
        Regex rgx;

        Dictionary<string, string> teamDict =
                new Dictionary<string, string>();

        Dictionary<string, string> teamAlreadyChecked =
                new Dictionary<string, string>();

        Teams teams = new Teams();
        string leagueName = String.Empty;
        string teamName = String.Empty;

        for (int i = 0; i < leaguesNames.Length; i++)
        {
            leagueName = leaguesNames[i];
            teams.setLeague(leagueName);
            for (int j = 0; j < teams.getMaxTeams(); j++)
            {
                teamDesc = teams.getTeamByIndex(j);
                teamName = teamDesc[0];


                if (teamName.Contains("CUSTOMIZE") ||
                    teamName.Contains("Customize"))
                    continue;

                string defense = teamDesc[1];
                string attack = teamDesc[2];
                string index = teamDesc[3];
                string price = teamDesc[4];
                string fansFlagsColors = teamDesc[5];
                string tshirtColor = teamDesc[6];
                string shortsColor = teamDesc[7];
                string socksColor = teamDesc[8];
                string flareColor = teamDesc[11];
                string shoes = teamDesc[13];
                string gloves = teamDesc[14];

                bool result = teamName.All(c => Char.IsLetter(c) || c == ' ');
                if (result == false)
                {
                    print("#DBG_ERROR_TEAMS_TEAM_NAME_NOT_ONLY_LETTER "
                        + " teamName "
                        + teamName
                        + " league "
                        + leagueName);
                }

                rgx = new Regex(@"^[A-Z]*[a-z\.]*( [A-Z]+[a-z]*)?( [A-Z]*[a-z]*)?$");
                if (!teamName.Equals("USA") &&
                    !rgx.IsMatch(teamName))
                {
                    print("#DBG_ERROR_TEAMS_TEAM_WRONG_NAME "
                            + " teamName " + teamName
                            + " League " + leagueName);
                }

                string flagPath = 
                    getFlagFullPath(teamName);

                if (Resources.Load<Texture2D>(flagPath) == null)
                {
                    print("#DBG_ERROR_TEAMS_TEAM_FLAG_CANNOT BE LOADED "
                           + " teamName " + teamName
                           + " League " + leagueName
                           + " flagPATH " + flagPath);
                }

                result = defense.All(Char.IsDigit);
                if (result == false)
                {
                    print("#DBG_ERROR_TEAMS_DEFENSE_NOT_ONLY_DIGIT "
                        + teamName +
                        " Defense "
                        + defense
                        + " League "
                        + leagueName);
                }

                result = attack.All(Char.IsDigit);
                if (result == false)
                {
                    print("#DBG_ERROR_TEAMS_ATTACK_NOT_ONLY_DIGIT "
                        + " teamName "
                        + teamName
                        + " attack "
                        + attack
                        + " League "
                        + leagueName);
                }


                if (!int.TryParse(defense, out int convDefToInt))
                {
                    print("#DBG_ERROR_TEAMS_DEFENSE IS NOT INT "
                        + " teamName "
                        + " teamName "
                        + teamName
                        + " defense "
                        + defense
                        + " League "
                        + leagueName);
                }

                if (!int.TryParse(attack, out int convAttToInt))
                {
                    print("#DBG_ERROR_TEAMS_DEFENSE IS NOT INT "
                        + " teamName "
                        + teamName
                        + " attack "
                        + attack
                        + " League "
                        + leagueName);
                }

                int def = int.Parse(defense);
                int att = int.Parse(attack);

                if (def < Globals.MIN_PLAYER_SKILLS || Globals.MAX_PLAYER_SKILLS > 100)
                {
                    print("#DBG_ERROR_TEAMS VAL TOO BIG "
                        + " teamName " 
                        + teamName 
                        + " attack " 
                        + def
                        + " League " 
                        + leagueName);
                }

                if (att < Globals.MIN_PLAYER_SKILLS || att > Globals.MAX_PLAYER_SKILLS)
                {
                    print("#DBG_ERROR_TEAMS VAL TOO BIG "
                        + " teamName "
                        + teamName 
                        + " attack "
                        + att
                        + " League "
                        + leagueName);
                }

                if (!int.TryParse(price, out int convPriceToInt))
                {
                    print("#DBG_ERROR_TEAMS_PRICE IS NOT INT "
                       + " teamName "
                       + teamName + " price "
                       + price
                       + " League "
                       + leagueName);
                }

                if (int.Parse(price) < 0 && int.Parse(price) > 10000)
                {
                    print("#DBG_ERROR_PLAYERCARD PRICE TOO BIG "
                        + " teamName "
                        + teamName
                        + " price "
                        + price
                        + " League "
                        + leagueName);
                }



                rgx = new Regex(@"^[a-z][a-z_1-9]+$");
                if (!rgx.IsMatch(tshirtColor))
                {
                    print("#DBG_ERROR_TEAMS_WRONG_TSHIRT_WRONG_FILE_NAME REGEX "
                            + " teamName " + teamName
                            + " League " + leagueName
                            + " tshirtColor " + tshirtColor);
                }

                rgx = new Regex(@"^[a-z_1-9]+$");
                if (!rgx.IsMatch(shortsColor))
                {
                    print("#DBG_ERROR_TEAMS_WRONG_SHORTS_WRONG_FILE_NAME REGEX "
                            + " teamName " + teamName
                            + " League " + leagueName
                            + " shortsColor " + shortsColor);
                }

                rgx = new Regex(@"^[a-z_1-9]+$");
                if (!rgx.IsMatch(socksColor))
                {
                    print("#DBG_ERROR_TEAMS_WRONG_SOCK_COLOR REGEX"
                            + " teamName " + teamName
                            + " League " + leagueName
                            + " sockColor " + socksColor);
                }

                Material tshirtMaterial =
                    graphics.getMaterial("playerMaterials/" + tshirtColor);
                Material shortsMaterial =
                    graphics.getMaterial("playerMaterials/" + shortsColor);
                Material socksMaterial =
                    graphics.getMaterial("playerMaterials/" + socksColor);

                if (tshirtMaterial == null)
                {
                    print("#DBG_ERROR_TEAMS_TSHIRT MATERIAL FAILED "
                            + " teamName " + teamName
                            + " League " + leagueName);
                }

                if (shortsMaterial == null)
                {
                    print("#DBG_ERROR_TEAMS_TSHIRT MATERIAL FAILED "
                            + " teamName " + teamName
                            + " League " + leagueName);
                }

                if (socksMaterial == null)
                {
                    print("#DBG_ERROR_TEAMS_TSHIRT MATERIAL FAILED "
                            + " teamName " + teamName
                            + " League " + leagueName);
                }

                rgx = new Regex(@"^[a-z]+$");
                if (!rgx.IsMatch(flareColor))
                {
                    print("#DBG_ERROR_TEAMS_WRONG_FLARE_COLOR "
                            + " teamName " + teamName
                            + " League " + leagueName
                            + " flareColor " + flareColor);
                }


                string[] shoe = shoes.Split('|');
                for (int x = 0; x < shoe.Length; x++)
                {
                    rgx = new Regex(@"^[a-z]+$");
                    if (!rgx.IsMatch(shoe[x]))
                    {
                        print("#DBG_ERROR_TEAMS_SHOES WRONG NAME "
                                + " teamName " + teamName
                                + " League " + leagueName
                                + " shoeName " + shoe[x]);
                    }

                    string shoeMaterialPath =
                        "playerMaterials/shoe/" + shoe[x];
                    Material shoeMaterial =
                        graphics.getMaterial(shoeMaterialPath);
                    if (shoeMaterial == null)
                    {
                        print("#DBG_ERROR_TEAMS shoeMaterial cannot be LOADED"
                              + " teamName " + teamName
                              + " League " + leagueName
                              + " shoeName " + shoeMaterialPath);
                    }
                }

                string[] glove = gloves.Split('|');
                for (int x = 0; x < glove.Length; x++)
                {
                    rgx = new Regex(@"^[a-z]+$");
                    if (!rgx.IsMatch(glove[x]))
                    {
                        print("#DBG_ERROR_TEAMS_GLOVE WRONG NAME "
                                + " teamName " + teamName
                                + " League " + leagueName
                                + " gloveName " + glove[x]);
                    }

                    string gloveMaterialPath =
                         "playerMaterials/glove/" + glove[x];
                    Material gloveMaterial =
                        graphics.getMaterial(gloveMaterialPath);
                    if (gloveMaterial == null)
                    {
                        print("#DBG_ERROR_TEAMS gloveMaterial cannot be LOADED"
                              + " teamName " + teamName
                              + " League " + leagueName
                              + " shoeName " + gloveMaterialPath);
                    }
                }

                string[] stadiumColors = fansFlagsColors.Split('|');

                string fansColor = stadiumColors[0];
                string bannerColor = stadiumColors[1];
                string fansFlagName = stadiumColors[2];

                string fansPath =
                    "stadium/fans/" + fansColor + "_1";
                Texture2D texturePeople =
                    graphics.getTexture(fansPath);
                if (texturePeople == null)
                {
                    print("#DBG_ERROR_TEAMS gloveMaterial cannot be LOADED"
                             + " teamName " + teamName
                             + " League " + leagueName
                             + " fansPath " + fansPath);
                }


                for (int num = 1; num <= 4; num++)
                {
                    string bannerPath =
                        "stadium/wallsFlag/banner_" + bannerColor + "_" + num.ToString();
                    Texture2D textureBanner =
                        graphics.getTexture(bannerPath);
                    if (textureBanner == null)
                    {
                        print("#DBG_ERROR_TEAMS banner cannot be LOADED"
                        + " teamName " + teamName
                        + " League " + leagueName
                        + " bannerPath " + bannerPath);
                    }
                }
           
                for (int flagRand = 1; flagRand < 5; flagRand++)
                {
                    string stickFlagsPath =
                        "FlagsFans/" + fansFlagName;

                    Texture2D flagTextureFans = graphics.getTexture(stickFlagsPath);
                        
                    if (Globals.isPlayerCardLeague(leagueName))
                    {
                        stickFlagsPath += "_" + flagRand;
                        flagTextureFans = graphics.getTexture(stickFlagsPath);
                    }

                    if (flagTextureFans == null)
                    {
                        print("#DBG_ERROR_TEAMS stick flag cannot be LOADED"
                                + " teamName " + teamName
                                + " League " + leagueName
                                + " stickFlagsPath " + stickFlagsPath);
                     }
                }
            }
        }
        
        print("#DBG_END OF TEAM STRUCTURE TEST");
    }




    private void checkPlayerCardCorrectionOfStructure(string[] allLeagues)
    {
        string[] teamDesc;
        for (int i = 0; i < allLeagues.Length; i++)
        {
            print("leagus " + allLeagues[i]);
            teams.setLeague(allLeagues[i]);

            for (int j = 0; j < teams.getMaxTeams(); j++)
            {
                teamDesc = teams.getTeamByIndex(j);

                string fullTeamPath = "Flags/" +
                    Globals.getFlagPath(allLeagues[i], teamDesc[0]);
                string flagFileName = Regex.Replace(fullTeamPath, "\\s+", "");

                if (Resources.Load<Texture2D>(flagFileName) == null)
                    print("#DBGTEAMFLAGS NO TEAM FLAG " + flagFileName);

                print("TeamDesc " + teamDesc[0]);
                string playersDescription = teamDesc[12];
                string[] playerDesc = playersDescription.Split('|');
                for (int x = 0; x < playerDesc.Length; x++)
                {
                    string[] plr = playerDesc[x].Split(':');
                    for (int k = 0; k < PLAYER_DESC_COL; k++)
                    {
                        int number;
                        if (k == 2 || k == 3 || k == 5)
                        {
                            bool isNumber = Int32.TryParse(plr[k], out number);
                            if (!isNumber)
                            {
                                print("ERROR " + teamDesc[0] + " K " + k);
                            }
                        }
                        else
                        {
                            //check nation flags of players
                            if (k == 1)
                            {
                                string testStr = plr[k];
                                string plrCountry = plr[1];
                                string plrNationalFlagPath = "Flags/" +
                                    Globals.getFlagPath("NATIONALS", plrCountry);

                                print("NATIONALFLAG " + plrNationalFlagPath);
                                plrNationalFlagPath = Regex.Replace(plrNationalFlagPath, "\\s+", "");
                                if (Resources.Load<Texture2D>(plrNationalFlagPath) == null)
                                    print("#DBGPLAYERFLAGS NO PLAYER NATIONAL FLAG " 
                                        + plrNationalFlagPath 
                                        + " TeamName " + teamDesc[0] 
                                        + " playerName " + plr[0]);
                            }
                        }
                    }
                }
            }
        }
    }

    public string getFlagFullPath(string teamName)
    {
        return "Flags/" + getFlagPath(teamName);
    }

    public string getFlagPath(string teamName)
    {
        string flagPath;

        teamName = teamName.ToLower();
        teamName = Regex.Replace(teamName, "\\s+", "");

        teamLeagueName.TryGetValue(teamName, out flagPath);

        if (flagPath == null)
            return "";

        //Debug.Log("#DBGTRANDFLAGS " + (flagPath + "/" + teamName));

        return flagPath + "/" + teamName;
    }

    public static void initTeamLeagueNameHash()
    {
        Teams teams = new Teams();
        string leagueName = String.Empty;
        string teamName = String.Empty;

        for (int i = 0; i < flagsFoldersNames.Length; i++)
        {
            leagueName = flagsFoldersNames[i];
            teams.setLeague(leagueName);
            for (int j = 0; j < teams.getMaxTeams(); j++)
            {
                teamName = teams.getTeamNameByIndex(j);
                teamName = teamName.ToLower();
                teamName = Regex.Replace(teamName, "\\s+", "");
                leagueName = leagueName.ToLower();

                if (teamLeagueName.ContainsKey(teamName))
                    continue;

                teamLeagueName.Add(teamName, leagueName);
                //Debug.Log("#ADDTOTEMALEAGUE " + teamName + " LEAGUENAME " + leagueName);
            }
        }
    }

}
