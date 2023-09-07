using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    [System.Serializable]
    public class StorageData_Setting
    {
        public bool useSound;
        public bool useVibrate;
        public bool usePowerSaver;
        public Theme theme;

        public StorageData_Setting()
        {
            useSound = true;
            useVibrate = true;
            usePowerSaver = false;
            theme = Theme.Light;
        }
    }
}
