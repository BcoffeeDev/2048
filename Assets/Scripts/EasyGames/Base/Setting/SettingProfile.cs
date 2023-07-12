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
        [SerializeField] private Dictionary<Theme, Color> FontColorByTheme = new();
        [SerializeField] private Dictionary<Theme, Color> BackgroundColorByTheme = new();
        [SerializeField] private Dictionary<Theme, Sprite> GridSpriteByTheme = new();
        [SerializeField] private Dictionary<Theme, Sprite> CellSpriteByTheme = new();
        [SerializeField] private Dictionary<Theme, Sprite> MenuSpriteByTheme = new();

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

        #region Theme

        public Color GetTextColorByTheme(Theme theme)
        {
            return FontColorByTheme[theme];
        }

        public Color GetBackgroundColorByTheme(Theme theme)
        {
            return BackgroundColorByTheme[theme];
        }

        public Sprite GetGridSpriteByTheme(Theme theme)
        {
            return GridSpriteByTheme[theme];
        }

        public Sprite GetCellSpriteByTheme(Theme theme)
        {
            return CellSpriteByTheme[theme];
        }

        public Sprite GetMenuSpriteByTheme(Theme theme)
        {
            return MenuSpriteByTheme[theme];
        }

        public Color GetThemeColor(Theme theme, ThemeTarget themeTarget)
        {
            switch (themeTarget)
            {
                case ThemeTarget.Background:
                    return BackgroundColorByTheme[theme];
                case ThemeTarget.Text:
                    return FontColorByTheme[theme];
                default:
                    throw new ArgumentOutOfRangeException(nameof(themeTarget), themeTarget, null);
            }
        }
        
        public Sprite GetThemeSprite(Theme theme, ThemeTarget themeTarget)
        {
            switch (themeTarget)
            {
                case ThemeTarget.Grid:
                    return GridSpriteByTheme[theme];
                case ThemeTarget.Cell:
                    return CellSpriteByTheme[theme];
                case ThemeTarget.Menu:
                    return MenuSpriteByTheme[theme];
                default:
                    throw new ArgumentOutOfRangeException(nameof(themeTarget), themeTarget, null);
            }
        }

        #endregion
    }
}
