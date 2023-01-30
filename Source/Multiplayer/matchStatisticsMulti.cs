using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;

namespace gameStatisticsNS
{
    public class MatchStatisticsMulti
    {
        public playerControllerMultiplayer playerMainScript;
        private int shotsA = 0;
        private int shotsB = 0;
        private int shotsOnTargetA = 0;
        private int shotsOnTargetB = 0;
        private int savesA = 0;
        private int savesB = 0;
        private float ballPossessionAtime = 0.0f;
        private float ballPossessionBtime = 0.0f;

        public MatchStatisticsMulti()
        {
            playerMainScript = Globals.player1MainScript;
        }

        public void setShot(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                shotsA++;
            }
            else
            {
                shotsB++;
            }
        }

        public void setShotOnTarget(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                shotsOnTargetA++;
            }
            else
            {
                shotsOnTargetB++;
            }
        }

        public void decSaves(string teamName, int val)
        {
            if (teamName.Equals("teamA"))
            {
                savesA -= val;
            }
            else
            {
                savesB -= val;
            }
        }

        public bool setSaves(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                if (playerMainScript.isShotOnTarget(
                    playerMainScript.peerPlayer.getEndPosOrg(),
                    playerMainScript.getGoalSizePlr1()))
                {
                    savesA++;
                    return true;
                }
            }
            else {
                if (playerMainScript.isShotOnTarget(
                    playerMainScript.getEndPosOrg(),
                    playerMainScript.getGoalSizePlr2()))
                {
                    savesB++;
                    return true;
                }
            }

            return false;
        }

        public void setBallPossession(string teamName, float time)
        {
            if (teamName.Equals("teamA"))
            {
                ballPossessionAtime += time;
            }
            else
            {
                ballPossessionBtime += time;
            }
        }

        public int getShot(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                return shotsA;
            }
            else
            {
                return shotsB;
            }
        }

        public int getShotOnTarget(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                return shotsOnTargetA;
            }
            else
            {
                return shotsOnTargetB;
            }
        }

        public int getSaves(string teamName)
        {
            if (teamName.Equals("teamA"))
            {
                return savesA;
            }
            else
            {
                return savesB;
            }
        }

        /*After reading one value of possession another one should be calculated 
         * 100 - first value to get 100% */

        public int getBallPossessionPercentage(string teamName)
        {            
            if (teamName.Equals("teamA"))
            {
                return (int) (Mathf.Ceil(
                    ballPossessionAtime / (ballPossessionAtime + ballPossessionBtime) * 100.0f));
            }
            else
            {
                return (int) (Mathf.Ceil(
                    ballPossessionBtime / (ballPossessionAtime + ballPossessionBtime) * 100.0f));
            }
        }
    }
}
