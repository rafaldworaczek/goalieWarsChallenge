using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using GlobalsNS;
using MenuCustomNS;


namespace MenuCustomNS
{
    public class Teams
    {
        private int activeTeams = 0;
        private Dictionary<string, string[][]> leaguePointerHashMap;
        private string defaultLeague = "BRAZIL";
        private string currentLeagueName = "BRAZIL";
        static private string lastTeamName;
        /*team name 
         * gk 
         * shot 
         * index 
         * coinsNeeded 
         * fans/flag color number
         * thirt color 
         * shorts color
         * sock color
         * thirt number color
         * hair
         * flare color
         */
        private string[][] teams;
     
        public Teams(string leagueName = "BRAZIL")
        {
            leaguePointerHashMap = new Dictionary<string, string[][]>();
            setLeague(leagueName);

            activeTeams = teams.Length;
            if (Globals.isPlayerCardLeague(leagueName))
                customizeTeamFill();

//            Debug.Log("#DBG team flag Globals.customizeTeamName " + Globals.customizeTeamName + " Globals.playerTeamName " 
  //              + Globals.playerTeamName);
   //         Debug.Log("#DBG team flag TEAMSPERGROUPS #DBG34532 Globals.isGameSettingActive " + Globals.isGameSettingActive +
  //              " Globals.playerTeamName " + Globals.playerTeamName
   //             + " Globals.isGameSettingActive " + Globals.isGameSettingActive);
            //and not NATIONALS EURO AMERICA CUP
            
            for (int i = 0; i < teams.Length; i++)
            {
                teams[i][3] = i.ToString();
                //Debug.Log("TEAMSPERGROUPS BEFORE " + i.ToString() + " teamName " + teams[i][0] + " lastTeamName " +
                    //lastTeamName);
            }
            
            this.sortTeams();

            for (int i = 0; i < teams.Length; i++)
            {
                teams[i][3] = i.ToString();
                //Debug.Log("#dbg12rr3 TEAMSPERGROUPS " + i.ToString() + " teamName " + teams[i][0] +
                //    " teamIdx " + teams[i][3] + " lastTeamName " + lastTeamName);
            }

            //Debug.Log("#DBG12 teams " + teams[teams.Length - 1][0]);
        }

        /*public Teams(double levelFactor, string leagueName)
        {
            setLeague(leagueName);

            activeTeams = teams.Length;
            for (int i = 0; i < teams.Length; i++)
                teams[i][3] = i.ToString();

            if (levelFactor == 1.0) return;

            double res;
            for (int i = 0; i < teams.Length; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    if (j == 1)
                        res = ((double)(int.Parse(teams[i][j]))) * levelFactor / 1.5;
                    else
                        res = ((double)(int.Parse(teams[i][j]))) * levelFactor;

                    res = (int)res;
                    teams[i][j] = res.ToString();
                }
            }
        }*/

        public bool swapElements(int row1, int row2) {
            if (row1 >= activeTeams
                || row2 >= activeTeams
                || activeTeams < 2
                || row1 < 0
                || row2 < 0)
                return false;

            //tmpRow = new string[10];
            string[] tmpRow = teams[row2];
            teams[row2] = teams[row1];
            teams[row1] = tmpRow;
            activeTeams--;
            return true;
        }

        public int getMaxTeams()
        {
            return teams.Length;
        }

        public int getMaxActiveTeams()
        {
            return this.activeTeams;
        }

        public static int sortTeamsComparator(string[] team1, string[] team2)
        {
            string time1 = team1[0];
            string time2 = team2[0];


            //WORKAROUND!!
            if (!Globals.isGameSettingActive)
            {
                //Debug.Log("TEAMSPERGROUPS LastTeamName " + lastTeamName);
                if (time1.Equals(lastTeamName) )
                {
                    return 1;
                }

                if (time2.Equals(lastTeamName))
                {
                    return -1;
                }
            }

            return time1.CompareTo(time2);
        }

        private void sortTeams()
        {
            Array.Sort(this.teams, sortTeamsComparator);
        }

        public string[] getTeamByIndex(int index)
        {
            //Debug.Log("teams idx " + index);
            if (PlayerPrefs.HasKey(teams[index][0] + "_defense"))
            {
                int defense = PlayerPrefs.GetInt(teams[index][0] + "_defense");
                teams[index][1] = defense.ToString();
            }

            if (PlayerPrefs.HasKey(teams[index][0] + "_attack"))
            {
                int attack = PlayerPrefs.GetInt(teams[index][0] + "_attack");
                teams[index][2] = attack.ToString();
            }

            if (PlayerPrefs.HasKey(teams[index][0] + "_teamPlayers"))
            {
                string teamPlayers = PlayerPrefs.GetString(teams[index][0] + "_teamPlayers");
                teams[index][12] = teamPlayers;
            }

            return teams[index];
        }

        public string[] getTeamByName(string name)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i][0].Equals(name))
                {
                    return getTeamByIndex(i);
                }
                //return teams[i];
            }

            return null;
        }

        public int getTeamIndexByName(string name)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i][0].Equals(name))
                    return i;
            }

            return -1;
        }

        public void customizeTeamFill()
        {
            for (int index = 0; index < teams.Length; index++)
            {
                if (teams[index][0].Equals("Customize Team"))
                {
                    //if (!Globals.customizeTeamName.Equals("CUSTOMIZE_TEAM_NOT_CREATED")) {
                        teams[index][0] = Globals.customizeTeamName;
                        //Debug.Log("#DBG CHANGE TO " + teams[index][0]);
                    //}

                    //if (!Globals.customizeTeamName.Equals("CUSTOMIZE_PLAYER_NOT_CREATED"))
                    //{
                        //Debug.Log("#DBGPLAYER before " + teams[index][12]);
                        teams[index][12] = Regex.Replace(teams[index][12],
                                                         "Juliano Jr",
                                                         Globals.customizePlayerName);
                        Globals.customizePlayerCardName = teams[index][12].Split(':')[6];
                   

                //Debug.Log("#DBGPLAYER AFTER " + teams[index][12] + " Globals.customizePlayerName " +
                //    Globals.customizePlayerName);

                //}

                //if (!Globals.customizePlayerNationality.Equals("EMPTY"))
                //{
                        teams[index][12] = Regex.Replace(teams[index][12],
                                                         "COUNTRY_TO_CHANGE",
                                                          Globals.customizePlayerNationality);

                        //Debug.Log("#DBG134FANS teams Globals.customizeFansColor " + Globals.customizeFansColor);

                        teams[index][5] = Regex.Replace(teams[index][5],
                                                        "FLAG_COLOR1",
                                                         Globals.customizeFlagColor);

                        teams[index][5] = Regex.Replace(teams[index][5],
                                                        "FLAG_COLOR2",
                                                         Globals.customizeFlagColor);

                        teams[index][5] = Regex.Replace(teams[index][5],
                                                        "FANS_COLOR",
                                                        Globals.customizeFansColor);
                        string flareColor = "red";
                        if (Globals.customizeFansColor.Contains("white"))
                        {
                            flareColor = "white";
                        }
                        else if (Globals.customizeFansColor.Contains("green"))
                        {
                            flareColor = "green";
                        }
                        else if (Globals.customizeFansColor.Contains("blue"))
                        {
                            flareColor = "blue";
                        }
                        else if (Globals.customizeFansColor.Contains("yellow"))
                        {
                            flareColor = "yellow";
                        }

                        teams[index][11] = flareColor;

                        //Debug.Log("#DBGteamsDBG " + teams[index][5]);
                  
                        //Debug.Log("#DBGPLAYER AFTERHRE " + teams[index][12] + " Globals.customizePlayerName " +
                        //Globals.customizePlayerName);
                    //}

                        teams[index][12] = Regex.Replace(teams[index][12],
                                                    "SKIN_HAIR_TO_CHANGE",
                                                     Globals.customizePlayerSkinHair);

                   

                    teams[index][6] = Globals.customizePlayerShirt;
                    teams[index][7] = Globals.customizePlayerShorts;
                    teams[index][8] = Globals.customizePlayerSocks;
                    //teams[index][10] = Globals.customizePlayerHair;
                    //teams[index][9] = Globals.customizePlayerSkin;
                    teams[index][14] = Globals.customizePlayerGloves;
                    teams[index][13] = Globals.customizePlayerShoe;
                }
            }

            activeTeams = teams.Length;
            if (!Globals.isGameSettingActive)
            {
                if (Globals.playerTeamName.Equals(Globals.customizeTeamName))
                {
                    swapElements(activeTeams - 2, activeTeams - 1);
                    //Debug.Log("TEAMSPERGROUPS 1 Customize Team " + activeTeams + " lastTeam " +
                    //    teams[activeTeams - 1][0]);
                }
                else
                {
                    activeTeams--;
                    //Debug.Log("#DBG34532 TEAMSPERGROUPS 2 " + activeTeams);
                }
            }

            lastTeamName = teams[teams.Length - 1][0];
        }

        public string getCurrentLeagueName()
        {
            return currentLeagueName;
        }

        public string getLeagueNameByIndex(int index)
        {
            switch (index)
            {
                case 0: 
                    return "BRAZIL";
                case 1:
                    return "ENGLAND";
                case 2:
                    return "GERMANY";
                case 3:
                    return "ITALY";              
                case 4:
                    return "SPAIN";
                case 5:
                    return "CHAMP CUP";
            }

            return "BRAZIL";
        }

        public string getTeamNameByIndex(int index)
        {
            return teams[index][0];
        }

        public string getTshirtColorByTeamIndex(int index)
        {
            /*if (index > getMaxTeams())
                return false;*/
            return teams[index][6];
        }

        public string getShortsColorByTeamIndex(int index)
        {
            /*if (index > getMaxTeams())
                return false;*/
            return teams[index][7];
        }

        public string getSocksColorByTeamIndex(int index)
        {
            /*if (index > getMaxTeams())
                return false;*/
            return teams[index][8];
        }

        //only for natioanl teams
        public string getSkinColorByTeamIndex(int index)
        {
            return teams[index][9];
        }

        //for leagues with players in the team
        public string getPlayerDescByIndex(int teamIdx, int playerIdx)
        {
            return teams[teamIdx][12].Split('|')[playerIdx];
        }

        public string getHairColorByTeamIndex(int index)
        {
            return teams[index][10];
        }

        public string getFlareColorByTeamIndex(int index)
        {
            return teams[index][11];
        }

        /*Add teams with equals coins to playerPrefabs*/
        public void addTeamtoPrefabs(int coins)
        {
            int coinsNeeded = 0;

            for (int i = 0; i < teams.Length; i++)
            {
                coinsNeeded = int.Parse(teams[i][4]);

                if (coinsNeeded == coins)
                {
                    string teamName = Regex.Replace(teams[i][0], "\\s+", "");
                    //Debug.Log("ADD TEAMS NAME " + teamName);

                    if (!PlayerPrefs.HasKey(teamName))
                    {
                        PlayerPrefs.SetInt(teamName, 1);
                        PlayerPrefs.Save();
                    }
                }
            }
        }

        public List<int> getJustUnlockedTeamsIndexes()
        {
            string teamName = "";
            int coinsNeeded = 0;
            int coinsGathered = Globals.coins;
            List<int> teamsIndexses = new List<int>();

            for (int i = 0; i < teams.Length; i++)
            {
                teamName = Regex.Replace(teams[i][0], "\\s+", "");
                coinsNeeded = int.Parse(teams[i][4]);

                if (!PlayerPrefs.HasKey(teamName) &&
                    coinsGathered >= coinsNeeded)
                {
                    teamsIndexses.Add(i);
                }
            }

            return teamsIndexses;
        }

        public void saveJustUnlockedTeamsToPrefabs()
        {
            List<int> teamIndexses = getJustUnlockedTeamsIndexes();
            foreach (int teamIdx in teamIndexses)
            {
                string teamName = Regex.Replace(teams[teamIdx][0], "\\s+", "");
                PlayerPrefs.SetInt(teamName, 1);
                PlayerPrefs.Save();
            }
        }

        public bool isAnyNewTeamUnclocked()
        {
            return (getJustUnlockedTeamsIndexes().Count > 0 ? true : false);
        }
       
        public void setLeague(string leagueName)
        {
            string[][] teamsArr = null;

            currentLeagueName = leagueName;

            //if (leaguePointerHashMap.TryGetValue(leagueName, out teamsArr))
            //{
            //    //Debug.Log("LEAGUE TAKEN FROM CACHED " + leagueName + " teamsARR " + teamsArr);
            //    teams = teamsArr;
            //     return;
            //} else {

                Debug.Log("leagueName " + leagueName);

                if (leagueName.Equals("BRAZIL"))
                {
                    teams = new LeagueBrazil().getTeams();
                }
                else if (leagueName.Equals("ENGLAND"))
                {
                    teams = new LeagueEngland().getTeams();
                }
                else if (leagueName.Equals("GERMANY"))
                {
                    teams = new LeagueGermany().getTeams();
                }
                else if (leagueName.Equals("ITALY"))
                {
                    teams = new LeagueItaly().getTeams();
                }
                else if (leagueName.Equals("POLAND"))
                {
                    teams = new LeaguePoland().getTeams();
                }
                else if (leagueName.Equals("SPAIN"))
                {
                    teams = new LeagueSpain().getTeams();
                }
                else if (leagueName.Equals("WORLD CUP") ||
                         leagueName.Equals("NATIONAL TEAMS") ||
                         leagueName.Equals("NATIONALS") ||
                         leagueName.Equals("NATIONAL"))
                {
                    teams = new NationalTeams().getTeams();
                }
                else if (leagueName.Equals("CHAMP CUP") ||
                         leagueName.Equals("CHAMPCUP") ||
                         leagueName.Equals("OTHERS") )                         
                {
                    teams = new ChampCupTeams().getTeams();
                }
                else if (leagueName.Equals("EURO CUP"))
                {
                    teams = new EuroCupTeams().getTeams();
                } else if (leagueName.Equals("AMERICA CUP"))
                {
                    teams = new AmericaCupTeams().getTeams();
                }

            //  leaguePointerHashMap.Add(leagueName, teams);
            //}

            if (Globals.isPlayerCardLeague(leagueName))
                customizeTeamFill();
            sortTeams();
            //for (int i = 0; i < teams.Length; i++)
            //    teams[i][3] = i.ToString();
        }

        public string getLeagueName()
        {
            return currentLeagueName;
        }
    }
}
