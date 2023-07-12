using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    public class StorageData_Leaderboard
    {
        public List<int> LeaderboardScore;

        public StorageData_Leaderboard()
        {
            LeaderboardScore = new List<int>();
        }
    }
}
