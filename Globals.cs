using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuCustomNS;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
using Photon.Pun;

namespace GlobalsNS
{
    public class Globals
    {
        public static bool isPhotoModeEnable = false;
        public static int MAX_POWERS = 10;
        public static int MAX_SELECTED = 3;
        public static bool adsEnable = false;

        //GRASS, INDOOR, STREET
        //public static string PITCHTYPE = "GRASS";
        public static string PITCHTYPE = "GRASS";
        public static bool promotionShowed = false;

        public static bool isAnalyticsEnable = true;
        public static bool isMultiplayer = false;
        public static bool isMultiplayerUpdate = false;
        public static bool isMultiplayerFriendConActive = false;
        //VERYSMALL, SMALL, STANDARD, MEDIUM, LARGE, VERYLARGE
        public static string cpuGoalSize = "STANDARD";
        public static bool teamBlocked = true;
        public static bool audioMute = false;
        
        public static string multiplayerSaveName = "multiplayer";
        public static float BALL_NEW_RADIUS = 0.2585f;
        public static float stoppageTime = 0f;
        public static bool isMultiplayerMatchNotFound = false;
        public static playerControllerMultiplayer player1MainScript = null;
        public static playerControllerMultiplayer player2MainScript = null;

        public static Vector3 INCORRECT_VECTOR = new Vector3(float.MaxValue / 3f,
                                                             float.MaxValue / 3f,
                                                             float.MaxValue / 3f);


        public static Vector3 INCORRECT_VECTOR_2 = new Vector3(float.MaxValue / 4f,
                                                               float.MaxValue / 4f,
                                                               float.MaxValue / 4f);

    
        public static int MAX_PLAYERS = 2;
        public static int peersReady = 0;
        public static playerControllerMultiplayer peerPlayerMine = null;
        public static playerControllerMultiplayer peerPlayerNotMine = null;
        public static float FIXEDUPDATE_TIME = 0.02f;
        public static float FRAME_RATE = 30f;


        public static string[] powerFileNames = { "powers/powerTwoSmallGoals",
                                                  "powers/powerCutGoalHalf",
                                                  "powers/powerEnlargeGoal",
                                                  "powers/powersilverBallx2",
                                                  "powers/powerCameraShaking",
                                                  "powers/powerGoalWall",
                                                  "powers/powerInvisiblePlayer",
                                                  "powers/powerBadConditions",
                                                  "powers/powerEnableFlares",
                                                  "powers/powerGoldenBallx3"};

        public static string[] powerNames = { "Extra Goals",
                                              "Cut Goal Back",
                                              "Enlarge Goal",
                                              "Silver Ball",
                                              "Earth quake",
                                              "Goal wall",
                                              "Invisibility",
                                              "Bad Weather",
                                              "Flares",
                                              "Golden Ball"};

        public static int[] powerPrices = { 0,
                                            0,
                                            0,
                                            5200,
                                            4500,
                                            6500,
                                            7000,
                                            6350,
                                            5000,
                                            10000 };
                                            

        //NON-REALESE
        /*public static bool adsEnable = false;
        public static bool isAnalyticsEnable = true;    
        //VERYSMALL, SMALL, STANDARD, MEDIUM, LARGE, VERYLARGE
        public static string cpuGoalSize = "STANDARD";
        public static bool teamBlocked = true;*/
        /*NONRELEASE*/
        /*BEFORE RELEASE SECTION*/
        /*CHANGE TO TRUE BEFORE RELEASE*/

        /*Level*/
        public static int numGameOpened = 0;
        public static int level = 3;
        public static int MIN_LEVEL = 1;
        public static int MAX_LEVEL = 5;
        public static int MAX_PLAYER_SKILLS = 100;
        public static int MIN_PLAYER_SKILLS = 40;
        public static int MAX_ENERGY = 100;
        public static string outstandingEnergyName = "EMPTY";
        public static int outstandingEnergyVal = 100;
        public static int MAX_LEAGUES_NUM = 5;
        //MAX_LEAGUES_NUM + 1 (CHAMP CUP) 
        public static int MAX_PLAYERS_CARD_LEAGUES = 6;
        public static bool wonFinal = false;
        /*STANDARD*/
        public static bool reviewDisplayed = false;
        public static bool isNewSeason = false;
        public static int leagueSeason = 20;
        public static string customizePlayerDesc = "";

        public static int numMatchesInThisSession = 0;
        public static int numMainMenuOpened = 0;

        public static string joystickSide = "Left";
        public static int rewardAdDefaultCoins = 30;
       
        public static bool champCupPromotedNextSeason = true;
        public static bool loadGameFromSave = true;

        /*Match time*/
        public static string matchTime = "";
        public static string graphicsQuality = "";

        public static string powersStr = "";
        public static string maxTimeToShotStr = "";
        public static string commentatorStr = "";
        public static int energyPlayerA = 100;
        public static int energyPlayerB = 100;


        /*Traning mode*/
        public static bool isTrainingActive = false;
        public static bool isBonusActive = false;
        public static bool isTraningDone = false;
        public static bool onlyTrainingActive = false;
        //public static bool onlySettings = false;

        public static bool isFriendly = true;
        public static bool isLeague = false;
        /*CUP or LEAGUE*/
        public static string gameType = "CUP";
        /*might be BRAZIL, ENGLAND etc, but also WORLDCUP*/
        public static string leagueName = "WORLD CUP";

        public static bool isNewGame = false;
        public static string savedFileName = "";

        public static int cupsWon = 0;
        public static string wonCupName = "League Cup";

        /*Score of team 1 */
        public static int score1;
        /*Score of team 2*/
        public static int score2;
        public static bool isGameSettingActive = true;

        public static string teamAname;
        /*ID of A team in sorted array*/
        public static int teamAid;
        public static string teamBname;
        /*ID of B team in sorted array*/
        public static int teamBid;

        public static int groundNum;
        public static byte effectNumber;

        public static bool showBetaVersionOfMultiplayer = false;

        public static int playerTeamId;
        public static string playerTeamName = "";
        public static string customizeTeamName = "CUSTOMIZE_TEAM_NOT_CREATED";
        public static string customizePlayerName = "CUSTOMIZE_PLAYER_NOT_CREATED";
        public static string logoFilePath = "";
        public static string customizePlayerNationality = "EMPTY";
        public static string customizePlayerShirt;
        public static string customizePlayerShorts;
        public static string customizePlayerSocks;
        public static string customizePlayerSkinHair = "";
        public static string customizePlayerGloves = "";
        public static string customizePlayerShoe = "";
        public static string customizePlayer = "";
        public static string customizeFansColor = "";
        public static string customizeFlagColor = "";
        public static string customizePlayerCardName = "";
        public static int stadiumNumber = 1;
        public static bool dontCheckOnlineUpdate = false;

        public static int shopTeamIdx = 0;

        public static string teamAleague;
        public static string teamBleague;

        public static string playerADesc = "";
        public static string playerBDesc = "";
        public static string teamADesc = "";
        public static string teamBDesc = "";
        public static bool teamBCustomize;

        public static string selectedPlrCardDesc = "";

        public static GameObject leagueBackground = null;
        public static bool isAdmobInit = false;
        public static bool isAdmobGameObjectCreated = false;
        public static int teamAcumulativeStrength;
        public static int teamBcumulativeStrength;
        public static int teamAGkStrength;
        public static int teamAAttackStrength;
        public static int teamBGkStrength;
        public static int teamBAttackStrength;
        public static bool playerPlayAway = false;
        public static int playerStarMinSkills = 178;
        public static int playerReadyToUpdate = -1;
        /*ORG SCREEN SIZE*/
        public static int originalScreenWidth = 1560;
        public static int orignalScreenHeight = 720;

        public static int nativeResWidth;
        public static int nativeResHeight;
        //public static int stadiumColorTeamA = 0;
        //public static int stadiumColorTeamB = 1;
        public static string stadiumColorTeamA = "red|red";
        public static string stadiumColorTeamB = "green|green";

        public static int levelIdx;
        public static int gameTimesIdx;
        public static int joystickSideIdx;
        public static int trainingModeIdx;
        public static int graphicsSettingsIdx;

        public static int powersIdx;
        public static int maxTimeToShotIdx;
        public static int commentatorIdx;

        public static int numGameOpen = 0;
        public static bool gameEnded = false;

        public static int coins = 0;
        public static int diamonds = 0;
        public static bool gameInGroup = true;
        public static bool playGamesActivated = false;

        public static int purchaseTeamIdx = -1;
        public static string purchaseTeamName;
        public static string purchaseLeagueName;
        public static string purchasePlayerDesc;

        public static string loaderBarSceneName = "";
        public static int numSelectedPlayerCardHelperShow;

        private static int MAX_PLAYERCARD_PRICE = 10000;
        private static int BASE_PLAYER_CARD_PRICE = 1000;

        public static string[] nonPlayerCardLeagues =
            { "WORLD CUP", "AMERICA CUP", "EURO CUP", "NATIONALS", "NATIONAL TEAMS" };

        public static string[] playerCardLeagues =
           { "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMP CUP"};

        public static string[] allLeaguesNames =
           { "NATIONALS", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "OTHERS"};

        public static string[] flagsFoldersNames =
            { "NATIONAL", "BRAZIL", "ENGLAND", "GERMANY", "ITALY", "SPAIN", "CHAMPCUP"};

        public static Queue<PurchaseItem> purchasesQueue = new Queue<PurchaseItem>();
        public static Dictionary<string, string> teamLeagueName =
            new Dictionary<string, string>();

        private static Teams nationalTeams = new Teams("NATIONALS");

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

        public static bool checkIfTeamExitsByName(string teamName)
        {
            teamName = teamName.ToLower();
            teamName = Regex.Replace(teamName, "\\s+", "");

            Debug.Log("#DBG12 " + teamName
                + " teamLeagueName.ContainsKey(teamName) " + teamLeagueName.ContainsKey(teamName));

            if (teamLeagueName.ContainsKey(teamName))
                return true;
            else
                return false;
        }

        public static void adsRemove()
        {
            if (!PlayerPrefs.HasKey("adsRemoved"))
            {
                Globals.adsEnable = false;
                PlayerPrefs.SetInt("adsRemoved", 1);
                PlayerPrefs.Save();
            }
        }

        public static bool isAdRemoved()
        {
            if (PlayerPrefs.HasKey("adsRemoved"))
            {
                Globals.adsEnable = false;
                return true;
            }
            else
                return false;
        }

        public static void addCoins(int value)
        {
            int coinsNum = Globals.coins + value;
            PlayerPrefs.SetInt("coins", coinsNum);
            PlayerPrefs.Save();
            /*update global coins value */
            Globals.coins = coinsNum;
            LeaderBoard.ReportScore(Globals.coins);
        }

        public static void addDiamonds(int value)
        {
            int diamondsNum = Globals.diamonds + value;
            PlayerPrefs.SetInt("diamonds", diamondsNum);
            PlayerPrefs.Save();

            /*update global diamonds value */
            Globals.diamonds = diamondsNum;
            //LeaderBoard.ReportScore(Globals.coins);
        }

        public static void addWonCup(int value)
        {
            Globals.cupsWon += value;
            PlayerPrefs.SetInt("cupsWon", Globals.cupsWon);
            PlayerPrefs.Save();
        }

        public static void unlockedTeam(int teamIdx)
        {
            string[] team = nationalTeams.getTeamByIndex(teamIdx);

            PlayerPrefs.SetInt(team[0], 1);
            PlayerPrefs.SetInt(team[0].ToLower(), 1);

            //Debug.Log("DBGUNLOCKEDTEAM " + team[0] + " Lower " + team[0].ToLower());
            PlayerPrefs.Save();
        }

        public static string getSkinHairColorName(string desc)
        {
            return Regex.Replace(desc, "[^0-9_]", "");
        }

        public static string getPlayerCardFileName(string skinName,
                                                   string hairName)
        {
            skinName = Regex.Replace(skinName, "[^0-9_]", "");
            string[] skinNames = skinName.Split('_');

            string playerCardName = "";
            for (int i = 1; i < skinNames.Length; i++)
            {
                if (i == 1)
                {
                    playerCardName += "f";
                }

                if (i == 2)
                {
                    playerCardName += "s";
                }

                if (i == 3)
                {
                    playerCardName += "b";
                }

                if (i == 4)
                {
                    playerCardName += "t";
                }

                //Debug.Log("character skinName " + skinNames[i]);

                playerCardName += skinNames[i] + "_";
            }

            return playerCardName + hairName;
        }

        public static Vector2 getTeamSkillsAverage(string[] players, string leagueName)
        {
            if (isPlayerCardLeague(leagueName))
            {
                string[] playersDesc = players[12].Split('|');
                int playersNum = playersDesc.Length;
                int defenseCumulative = 0;
                int attackCumulative = 0;

                for (int i = 0; i < playersNum; i++)
                {
                    //dont take as an average junior skills
                    if (playersNum > 1 && i == 0)
                        continue;

                    string[] player = playersDesc[i].Split(':');
                    defenseCumulative += int.Parse(player[2]);
                    attackCumulative += int.Parse(player[3]);
                }

                //Debug.Log("DBGSKILLS " + defenseCumulative + " ATTACK " + attackCumulative);
                if (playersNum > 1)
                    playersNum = playersNum - 1;

                return new Vector2(
                    (float)defenseCumulative / playersNum,
                    (float)attackCumulative / playersNum);
            }
            else
            {
                return new Vector2(float.Parse(players[1]),
                                   float.Parse(players[2]));
            }
        }

        public static int getPlayerCardAttackSkills(string playerDesc)
        {
            return int.Parse(playerDesc.Split(':')[2]);
        }

        public static int getPlayerCardDefenseSkills(string playerDesc)
        {
            return int.Parse(playerDesc.Split(':')[3]);
        }


        /*public static string getPlayerCardPrice(string defense,
                                                string attack)
        {
            int price = getPlayerCardPrice(int.Parse(defense),
                                           int.Parse(attack));

            return price.ToString();
        }*/

        public static string getPlayerCardPrice(string playerDesc)
        {
            string[] player = playerDesc.Split(':');
            int defense = int.Parse(player[2]);
            int attack = int.Parse(player[3]);
            int price = int.Parse(player[5]);

            if (price == 0)
                return "0";

            return getPlayerCardPrice(defense, attack).ToString();
        }

        public static int getPlayerCardPrice(int defense, int attack)
        {
            int cumulativeSkills =
                Mathf.Max(MIN_PLAYER_SKILLS * 2, defense + attack);
            float perc =
                Mathf.InverseLerp(MIN_PLAYER_SKILLS * 2, MAX_PLAYER_SKILLS * 2, cumulativeSkills);

            //Debug.Log("#DBGDYNAMICPRICE VALUE " + " 0: "
            //        + (MAX_PLAYER_SKILLS * 2) + " cumulative " + cumulativeSkills
            //        + " PERC " + perc
            //        + " finalPrice " + ((int)(MAX_PLAYERCARD_PRICE * perc)));

            return BASE_PLAYER_CARD_PRICE +
                   ((int)((MAX_PLAYERCARD_PRICE - BASE_PLAYER_CARD_PRICE) * perc));
        }

        public static bool isPlayerCardLeague(string leagueName)
        {
            for (int i = 0; i < nonPlayerCardLeagues.Length; i++)
            {
                if (leagueName.Equals(nonPlayerCardLeagues[i]))
                {
                    return false;
                }
            }

            return true;
        }

        //shoud be moved to graphics
        public static string getFlagPath(string leagueName, string teamName)
        {
            string dirName = "";

            if (Globals.isPlayerCardLeague(leagueName))
            {
                if (leagueName.Equals("OTHERS"))
                    leagueName = "champcup";

                dirName = leagueName.ToLower();
            }
            else
            {
                dirName = "national";
            }

            return dirName + "/" + teamName;
        }

        public static void unlockedPlayerCard(int teamIdx,
                                              string leagueName,
                                              string teamName,
                                              string playerDesc)
        {
            string teamPlayers = "";
            if (PlayerPrefs.HasKey(teamName + "_teamPlayers"))
            {
                teamPlayers =
                    PlayerPrefs.GetString(teamName + "_teamPlayers");
            }
            else
            {
                Teams teams = new Teams(leagueName);
                string team = teams.getTeamByIndex(teamIdx)[12];
                teamPlayers = team;
            }

            /*string playerNewDesc = "";

            string[] playerStr = playerDesc.Split(':');
            playerStr[4] = "U";
            for (int i = 0; i < playerStr.Length; i++)
            {
                playerNewDesc += playerStr[i] + ":";
            }
            playerNewDesc =
               playerNewDesc.Remove(playerNewDesc.Length - 1);
            */

            //teamPlayers = teamPlayers.Replace(playerDesc, playerNewDesc);
            //Remove player from player's list
            //Debug.Log("#DBGUNLOCKPLAYER PLAYER BEFORE UNLOCKED "
            //    + teamPlayers + " PlayerDesc " + playerDesc);
            //teamPlayers = teamPlayers.Replace(playerDesc, "");
            //teamPlayers = teamPlayers.Replace("||", "|");
            //if (teamPlayers.EndsWith("|"))
            //    teamPlayers = teamPlayers.Substring(0, teamPlayers.Length - 1);

            teamPlayers = removeStrFromString(teamPlayers, playerDesc, "|");

            /*if (teamPlayers.Contains(playerDesc + "|"))
            {
                teamPlayers = teamPlayers.Replace(playerDesc + "|", "");
            }
            else if (teamPlayers.Contains(playerDesc))
            {
                teamPlayers = teamPlayers.Replace(playerDesc, "");
            }*/

            playerDesc = togglePlayerCardLockVal(playerDesc, "U");

            if (!String.IsNullOrEmpty(teamPlayers))
                teamPlayers = playerDesc + "|" + teamPlayers;
            else
                teamPlayers = playerDesc;

            //Debug.Log("#DBGUNLOCKPLAYER PLAYER AFTER UNLOCKED " + teamPlayers);

            PlayerPrefs.SetString(teamName + "_teamPlayers", teamPlayers);
            PlayerPrefs.Save();
        }

        public static string removeStrFromString(string str,
                                                 string strToRemove,
                                                 string delimiter)
        {
            //TODO should return whole string
            if (!str.Contains(strToRemove))
                return String.Empty;

            if (str.Contains(strToRemove + delimiter))
            {
                str = str.Replace(strToRemove + delimiter, "");
            }
            else if (str.Contains(delimiter + strToRemove))
            {
                str = str.Replace(delimiter + strToRemove, "");
            }
            else
                str = str.Replace(strToRemove, "");

            return str;
        }

        public static string removeStrFromStr(string str,
                                              string strToRemove,
                                              string delimiter)
        {
            //TODO should return whole string
            if (!str.Contains(strToRemove))
                return str;

            if (str.Contains(strToRemove + delimiter))
            {
                str = str.Replace(strToRemove + delimiter, "");
            }
            else if (str.Contains(delimiter + strToRemove))
            {
                str = str.Replace(delimiter + strToRemove, "");
            }
            else
                str = str.Replace(strToRemove, "");

            return str;
        }


        public static string rearrangeToEndList(string str,
                                                string toReplace,
                                                string delimiter)
        {
            //Debug.Log("#DBG_REARANGE before " + str + " saveName " + toReplace);

            string resultStr =
                removeStrFromString(str, toReplace, delimiter);

            //if (str.Contains(toReplace + delimiter))
            //    str = str.Replace(toReplace + delimiter, "");

            //if (str.Contains(toReplace))
            //    str = str.Replace(toReplace, "");

            if (String.IsNullOrEmpty(resultStr))
                resultStr = toReplace;
            else
                resultStr += "|" + toReplace;

            //Debug.Log("#DBG_REARANGE after " + resultStr + " saveName " + toReplace);


            return resultStr;
        }

        public static string addPlayerCardToTail(string orgPlayers,
                                                 string newPlayer,
                                                 string delimiter)
        {
            string outputString = "";
            if (String.IsNullOrEmpty(orgPlayers))
            {
                outputString = newPlayer;
            }
            else
            {
                outputString = orgPlayers + delimiter + newPlayer;
            }

            return outputString;
        }

        public static string addPlayerCardToHead(string orgPlayers,
                                                 string newPlayer,
                                                 string delimiter)
        {
            string outputString = "";
            if (String.IsNullOrEmpty(orgPlayers))
            {
                outputString = newPlayer;
            }
            else
            {
                outputString = newPlayer + delimiter + orgPlayers;
            }

            return outputString;
        }

        public static string incPlayerCardAttackSkills(string playerDesc,
                                                       int incVal)
        {
            string playerNewDesc = "";
            string[] playerStr = playerDesc.Split(':');
            int newVal = int.Parse(playerStr[2]) + incVal;
            newVal = Mathf.Clamp(newVal, MIN_PLAYER_SKILLS, MAX_PLAYER_SKILLS);
            playerStr[2] = newVal.ToString();
            for (int i = 0; i < playerStr.Length; i++)
            {
                playerNewDesc += playerStr[i] + ":";
            }
            playerNewDesc =
               playerNewDesc.Remove(playerNewDesc.Length - 1);

            return playerNewDesc;
        }

        public static string incPlayerCardDefenseSkills(string playerDesc,
                                                        int incVal)
        {
            string playerNewDesc = "";
            string[] playerStr = playerDesc.Split(':');
            int newVal = int.Parse(playerStr[3]) + incVal;
            newVal = Mathf.Clamp(newVal, MIN_PLAYER_SKILLS, MAX_PLAYER_SKILLS);

            playerStr[3] = newVal.ToString();
            for (int i = 0; i < playerStr.Length; i++)
            {
                playerNewDesc += playerStr[i] + ":";
            }

            playerNewDesc =
               playerNewDesc.Remove(playerNewDesc.Length - 1);

            return playerNewDesc;
        }

        public static string togglePlayerCardLockVal(string playerDesc,
                                                     string lockedVal)
        {
            string playerNewDesc = "";
            string[] playerStr = playerDesc.Split(':');
            playerStr[4] = lockedVal;
            for (int i = 0; i < playerStr.Length; i++)
            {
                playerNewDesc += playerStr[i] + ":";
            }
            playerNewDesc =
               playerNewDesc.Remove(playerNewDesc.Length - 1);

            return playerNewDesc;
        }

        public static void incNumSelectedPlayerCardHelperShow(int val)
        {
            numSelectedPlayerCardHelperShow += val;
            PlayerPrefs.SetInt("numSelectedPlayerCardHelperShow",
                numSelectedPlayerCardHelperShow);
            PlayerPrefs.Save();
        }

        public static void incTeamSkills(int teamIdx, int attack, int defense)
        {
            string[] team = nationalTeams.getTeamByIndex(teamIdx);
            int newDefenseSkill = int.Parse(team[1]) + defense;
            int newAttackSkill = int.Parse(team[2]) + attack;

            if (newDefenseSkill > MAX_PLAYER_SKILLS)
            {
                newDefenseSkill = Mathf.Min(newDefenseSkill, MAX_PLAYER_SKILLS);
            }

            if (newAttackSkill > MAX_PLAYER_SKILLS)
            {
                newAttackSkill = Mathf.Min(newAttackSkill, MAX_PLAYER_SKILLS);
            }

            //Debug.Log("#DBG123 skills TEAM[0] " + team[0]);

            if (attack > 0)
            {
                PlayerPrefs.SetInt(team[0] + "_attack", newAttackSkill);
            }

            if (defense > 0)
            {
                PlayerPrefs.SetInt(team[0] + "_defense", newDefenseSkill);
            }

            PlayerPrefs.Save();
        }

        public static bool isPlayerCardStar(int defense, int attack)
        {
            if ((defense + attack) > playerStarMinSkills)
                return true;

            return false;
        }

        public static void enlargeGoalSize(string goalSize)
        {
            cpuGoalSize = goalSize;
        }

        public static string getAllTeamPlayers(string leagueName, string teamName)
        {
            Teams team = new Teams(leagueName);
            return team.getTeamByName(teamName)[12];
        }

        public static bool deleteGameSave(string saveName,
                                          string savesGroup,
                                          string leagueName,
                                          string teamName)
        {
            if (PlayerPrefs.HasKey(savesGroup))
            {
                string saves =
                    PlayerPrefs.GetString(savesGroup);
               
                saves = removeStrFromString(saves, saveName, "|");            
                if (String.IsNullOrEmpty(saves))
                {
                    PlayerPrefs.DeleteKey(savesGroup);
                }
                else
                {
                    PlayerPrefs.SetString(savesGroup, saves);
                }
                PlayerPrefs.Save();

                string filePath = Application.persistentDataPath + "/" + saveName;
                if (File.Exists(filePath))
                {
                    //Debug.Log("delete file " + filePath);
                    File.Delete(filePath);
                }

                PlayerPrefs.DeleteKey(saveName + "_levelsIdx");
                PlayerPrefs.DeleteKey(saveName + "_gameSettingsSave");
                PlayerPrefs.DeleteKey(saveName + "_gameTimesIdx");
                PlayerPrefs.DeleteKey(saveName + "_trainingModeIdx");
                PlayerPrefs.DeleteKey(saveName + "_graphicsSettingsIdx");
                PlayerPrefs.DeleteKey(saveName + "_joystickSideIdx");
                PlayerPrefs.DeleteKey(saveName + "_powersIdx");
                PlayerPrefs.DeleteKey(saveName + "_commentatorIdx");
                PlayerPrefs.DeleteKey(saveName + "_maxTimeToShotIdx");


                PlayerPrefs.DeleteKey(saveName + "_isNewSeason");
                PlayerPrefs.DeleteKey(saveName + "_stadiumNumber");

                if (isPlayerCardLeague(leagueName))
                {
                    string[] players =
                        getAllTeamPlayers(leagueName, teamName).Split('|');

                    for (int i = 0; i < players.Length; i++)
                    {
                        PlayerPrefs.DeleteKey(
                            players[i].Split(':')[0] + "_energy_" + saveName);
                    }
                }

                PlayerPrefs.Save();

                //Debug.Log("Application.dataPath " + Application.persistentDataPath);
            }

            return false;
        }

        public static bool deleteListItem(string itemName,
                                          string itemFileName)                                 
        {

            if (PlayerPrefs.HasKey(itemFileName))
            {
                string items =
                    PlayerPrefs.GetString(itemFileName);

                string newItems = removeStrFromStr(items, itemName, "|");
                if (String.IsNullOrEmpty(newItems))
                {
                    PlayerPrefs.DeleteKey(itemFileName);
                }
                else
                {
                    PlayerPrefs.SetString(itemFileName, newItems);
                }
                PlayerPrefs.Save();                             
            }

            return false;
        }

        /*public static int numerOfSavesToDelete(string teamName)
        {
            int count = 0;
            if (PlayerPrefs.HasKey("savedGames_CUP"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_CUP").Split('|');
                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');
                    if (teamDesc[1].Equals(teamName))
                    {
                        count++;
                    }
                }
            }

            if (PlayerPrefs.HasKey("savedGames_LEAGUES"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_LEAGUES").Split('|');

                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');

                    if (teamDesc[1].Equals(teamName))
                    {
                        count++;
                    }
                }
            }

            return count;
        }*/

        public static bool compareSign(float val1, float val2)
        {
            return Mathf.Sign(val1) == Mathf.Sign(val2);
        }

        public static List<string> getNamesSavesToDelete(string teamName)
        {
            List<string> savesList = new List<string>();

            if (PlayerPrefs.HasKey("savedGames_CUP"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_CUP").Split('|');
                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');
                    string saveIdx = countryName[1];

                    if (teamDesc[1].Equals(teamName))
                    {
                        savesList.Add(teamName + " : " + cupSaves[i] + ": " +
                              " Week " + teamDesc[2] +
                              " - Season " + teamDesc[3] + "/" +
                              (int.Parse(teamDesc[3]) + 1).ToString());
                    }
                }
            }

            if (PlayerPrefs.HasKey("savedGames_LEAGUES"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_LEAGUES").Split('|');

                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');
                    string saveIdx = countryName[1];


                    if (teamDesc[1].Equals(teamName))
                    {
                        savesList.Add(teamName + " : " + cupSaves[i] + ": " +
                                      " Week " + teamDesc[2] +
                                      " - Season " + teamDesc[3] + "/" +
                                      (int.Parse(teamDesc[3]) + 1).ToString());
                    }
                }
            }

            return savesList;
        }

        public static int numerOfSavesToDelete(string teamName)
        {
            int count = 0;
            if (PlayerPrefs.HasKey("savedGames_CUP"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_CUP").Split('|');
                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');
                    if (teamDesc[1].Equals(teamName))
                    {
                        count++;
                    }
                }
            }

            if (PlayerPrefs.HasKey("savedGames_LEAGUES"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_LEAGUES").Split('|');

                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');

                    if (teamDesc[1].Equals(teamName))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static void deleteAllTeamSaves(string teamName)
        {
            if (PlayerPrefs.HasKey("savedGames_CUP"))
            {
                string[] cupSaves = 
                    PlayerPrefs.GetString("savedGames_CUP").Split('|');
                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');

                    if (teamDesc[1].Equals(teamName)) {
                        deleteGameSave(cupSaves[i],
                                      "savedGames_CUP",
                                       countryName[0],
                                       teamDesc[1]);
                    }
                }
            }

            if (PlayerPrefs.HasKey("savedGames_LEAGUES"))
            {
                string[] cupSaves =
                    PlayerPrefs.GetString("savedGames_LEAGUES").Split('|');
                
                for (int i = 0; i < cupSaves.Length; i++)
                {
                    string saveDesc = PlayerPrefs.GetString(cupSaves[i]);
                    string[] teamDesc = saveDesc.Split('|');
                    string[] countryName = teamDesc[0].Split('_');

                    Debug.Log("DBG## cupSaves[i] " + cupSaves[i] +
                        " countryName " + countryName[0]
                        + " teamDesc " + teamDesc[1]);


                    if (teamDesc[1].Equals(teamName))
                    {               
                        deleteGameSave(cupSaves[i],
                                       "savedGames_LEAGUES",
                                       countryName[0],
                                       teamDesc[1]);
                    }
                }
            }
        }

        public static string[] getRandTeam()
        {
            Teams[] leagues = new Teams[MAX_PLAYERS_CARD_LEAGUES];

            for (int i = 0; i < playerCardLeagues.Length; i++)
            {
                leagues[i] =
                    new Teams(playerCardLeagues[i]);
            }

            int randomLeague =
                UnityEngine.Random.Range(0, MAX_PLAYERS_CARD_LEAGUES);
            int randTeam =
                     UnityEngine.Random.Range(0, leagues[randomLeague].getMaxActiveTeams());

            return leagues[randomLeague].getTeamByIndex(randTeam);
        }

        public static void initSkills(ref int attackSkillsPlayer,
                                      ref int attackSkillsCpu,
                                      ref int defenseSkillsPlayer,
                                      ref int defenseSkillsCpu,
                                      ref int cumulativeStrengthPlayer,
                                      ref int cumulativeStrengthCpu,
                                      bool isPlayerPlayAway)
        {

            if (!isPlayerPlayAway)
            {
                attackSkillsPlayer =
                    Globals.teamAAttackStrength;
                defenseSkillsPlayer =
                   Globals.teamAGkStrength;
                cumulativeStrengthPlayer =
                    attackSkillsPlayer + defenseSkillsPlayer;

                attackSkillsCpu =
                    Globals.teamBAttackStrength;
                defenseSkillsCpu =
                    Globals.teamBGkStrength;
                cumulativeStrengthCpu =
                    attackSkillsCpu + defenseSkillsCpu;
            }
            else
            {
                attackSkillsPlayer =
                    Globals.teamBAttackStrength;
                defenseSkillsPlayer =
                   Globals.teamBGkStrength;
                cumulativeStrengthPlayer =
                    attackSkillsPlayer + defenseSkillsPlayer;

                attackSkillsCpu =
                    Globals.teamAAttackStrength;
                defenseSkillsCpu =
                    Globals.teamAGkStrength;
                cumulativeStrengthCpu =
                    attackSkillsCpu + defenseSkillsCpu;
            }
        }

        public static void recoverOriginalResolution()
        {
            Screen.SetResolution(Globals.originalScreenWidth,
                                 Globals.orignalScreenHeight,
                                 true);
        }

        public static void loadSceneWithBarLoader(string sceneName)
        {
            loaderBarSceneName = sceneName;
            SceneManager.LoadScene("gameLoader");
        }

        public static void loadSceneWithBarLoaderNoSettings(string sceneName)
        {
            loaderBarSceneName = sceneName;
            SceneManager.LoadScene("gameLoaderNoSettings");
        }

        public static bool didPlayerLose(int score1,
                                         int score2)
        {
            if (score1 < score2 && !Globals.playerPlayAway)
                return true;

            if (score2 < score1 && Globals.playerPlayAway)
                return true;

            return false;
        }

        public static bool didPlayerWon(int score1,
                                        int score2)
        {
            if (score1 > score2 && !Globals.playerPlayAway)
                return true;

            if (score2 > score1 && Globals.playerPlayAway)
                return true;

            return false;
        }

        public static string getRandomStr(int size)
        {
            string digitletters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            string outStr = "";

            for (int i = 0; i < size; i++)
            {
                char c = digitletters[UnityEngine.Random.Range(0, digitletters.Length)];
                outStr += c;
            }

            return outStr;
        }

        public static bool didPlayerDraw(int score1,
                                         int score2)
        {
            if (score1 == score2)
                return true;
            return false;
        }

        public static string firstLetterUpperCase(string str)
        {
            str = str.ToLower();
            string[] text = str.Split(' ');
            string outputStr = "";

            for (int i = 0; i < text.Length; i++)
            {
                outputStr += text[i].Substring(0, 1).ToUpper() + text[i].Substring(1);
                if (i < (text.Length - 1))
                    outputStr += " ";
            }

            return outputStr;
        }

        public static bool isTeamCustomize(string teamName)
        {
            if (teamName.Equals(Globals.customizeTeamName) ||
                teamName.Equals("Customize Team") ||
                teamName.Contains("logoFile"))
            {
                return true;
            }

            return false;
        }

        public static float getSign(float val)
        {
            return val < 0 ? -1 : (val > 0 ? 1 : 0);
        }

        public static bool hasTheSameSign(float num1, float num2)
        {
            if ((num1 < 0 && num2 > 0) ||
                (num1 > 0 && num2 < 0))
                return false;
            return true;
        }

        /*SOURCE https://answers.unity.com/questions/1238142/version-of-transformtransformpoint-which-is-unaffe.html */
        public static Vector3 TransformPointUnscaled(Transform transform, Vector3 position)
        {
            Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            return localToWorldMatrix.MultiplyPoint3x4(position);
        }

        public static Vector3 InverseTransformPointUnscaled(Transform transform, Vector3 position)
        {
            Matrix4x4 worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
            return worldToLocalMatrix.MultiplyPoint3x4(position);
        }
    }
}
