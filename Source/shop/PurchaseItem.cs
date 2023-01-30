using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalsNS
{
    public class PurchaseItem
    {
        public string name = string.Empty;
        public string goalSize = string.Empty;
        public int coins = 0;
        public int diamonds = 0;
        public int attackVal = 0;
        public int defenseVal = 0;
        public int purchaseTeamId = -1;
        public string teamName;
        public string playerDesc;
        public string leagueName;

        public PurchaseItem(string eventName)
        {
            this.name = eventName;
        }

        public PurchaseItem(string eventName, string gSize)
        {
            this.name = eventName;
            this.goalSize = goalSize;
        }

        public PurchaseItem(string eventName, int val)
        {
            this.name = eventName;
            if (eventName.Contains("coin"))
                this.coins = val;
            
            if (eventName.Contains("diamond"))
                this.diamonds = val;
        }

        public PurchaseItem(string eventName, int coinsVal, int purchTeamId)
        {
            this.name = eventName;
            this.coins = coinsVal;
            this.purchaseTeamId = purchTeamId;
        }

        public PurchaseItem(string eventName, int coinsVal, int attackVal, int defenseVal)
        {
            this.name = eventName;
            this.coins = coinsVal;
            this.attackVal = attackVal;
            this.defenseVal = defenseVal;
        }

        public PurchaseItem(string eventName, string teamName, string playerDesc, string leagueName)
        {
            this.name = eventName;
            this.teamName = teamName;
            this.playerDesc = playerDesc;
            this.leagueName = leagueName;
        }
    }
}
