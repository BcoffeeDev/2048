using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace EasyGames
{
    [CreateAssetMenu(fileName = "SettingProfile", menuName = "Create/EasyGames/SettingProfile")]
    public class SettingProfile : SerializedScriptableObject
    {
        [Title("Control")]
        [SerializeField] private Dictionary<SupportPlatform, ControlType> ControlTypePerPlatform = new();

        [Title("Theme")] 
        public Dictionary<Theme, CustomizeProfile> ThemeProfile = new();

        private SupportPlatform GetCurrentPlatform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        return SupportPlatform.Android;
                    case RuntimePlatform.IPhonePlayer:
                        return SupportPlatform.IOS;
                    default:
                        return SupportPlatform.Editor;
                }
            }
        }

        public ControlType GetControlTypeByPlatform => ControlTypePerPlatform[GetCurrentPlatform];
    }
}
