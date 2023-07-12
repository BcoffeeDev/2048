using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    public class Storage : MonoBehaviour
    {
        #region Save

        public void Save(StorageData_Game data)
        {
            StorageUtility.Save(data, StoragePath.Game);
        }

        public void Save(StorageData_Leaderboard data)
        {
            StorageUtility.Save(data, StoragePath.Leaderboard);
        }

        public void Save(StorageData_Setting data)
        {
            StorageUtility.Save(data, StoragePath.Setting);
        }

        #endregion

        #region Load

        public void Load(out StorageData_Game gameData)
        {
            if (!StorageUtility.Exist(StoragePath.Game))
            {
                gameData = new StorageData_Game(new List<GridCell>(), 0);
                return;
            }
            gameData = StorageUtility.Load<StorageData_Game>(StoragePath.Game);
        }

        public void Load(out StorageData_Leaderboard leaderboardData)
        {
            if (!StorageUtility.Exist(StoragePath.Leaderboard))
            {
                leaderboardData = new StorageData_Leaderboard();
                return;
            }
            leaderboardData = StorageUtility.Load<StorageData_Leaderboard>(StoragePath.Leaderboard);
        }

        public void Load(out StorageData_Setting settingData)
        {
            if (!StorageUtility.Exist(StoragePath.Setting))
            {
                settingData = new StorageData_Setting();
                return;
            }
            settingData = StorageUtility.Load<StorageData_Setting>(StoragePath.Setting);
        }

        #endregion

        [Button]
        public void Delete(StoragePath storagePath)
        {
            StorageUtility.Delete(storagePath);
        }
    }
}