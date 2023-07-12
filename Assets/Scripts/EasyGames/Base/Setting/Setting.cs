using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace EasyGames
{
    public class Setting : MonoBehaviour
    {
        [Title("Text")] 
        public TextMeshProUGUI soundText;
        public TextMeshProUGUI vibrateText;
        public TextMeshProUGUI themeText;
        public TextMeshProUGUI versionText;

        [Title("BaseSetting")]
        public SettingProfile settingProfile;
        
        [Title("Setting")]
        public Theme theme = Theme.Light;
        public bool useSound = true;
        public bool useVibrate = true;

        private ThemeHandler[] _themeHandlers;

        #region Const

        private const string _soundOnString = "Sound : ON";
        private const string _soundOffString = "Sound : OFF";
        private const string _vibrateOnString = "Vibrate : ON";
        private const string _vibrateOffString = "Vibrate : OFF";
        private const string _lightThemeString = "Light Mode";
        private const string _darkThemeString = "Dark Mode";

        #endregion
        
        private void Awake()
        {
            _themeHandlers = FindObjectsOfType<ThemeHandler>(true);
        }

        #region ReadOnly

        private Color GetTextColor => settingProfile.GetThemeColor(theme, ThemeTarget.Text);
        private Color GetBackgroundColor => settingProfile.GetThemeColor(theme, ThemeTarget.Background);
        private Sprite GetGridSprite => settingProfile.GetThemeSprite(theme, ThemeTarget.Grid);
        private Sprite GetCellSprite => settingProfile.GetThemeSprite(theme, ThemeTarget.Cell);

        private Sprite GetMenuSprite => settingProfile.GetThemeSprite(theme, ThemeTarget.Menu);

        #endregion

        #region Action

        public static Action OnSettingChange;
        public static Action<bool> OnSoundChange;
        public static Action<bool> OnVibrateChange;

        #endregion
        
        private void Start()
        {
            versionText.SetText($"Ver {Application.version}");
            Application.targetFrameRate = 120;
            ApplyTheme(false, true);
        }

        public void ApplyTheme()
        {
            ApplyTheme(true, false);
        }

        public void ApplyTheme(bool forceTheme, bool instant)
        {
            if (forceTheme)
            {
                _themeHandlers = FindObjectsOfType<ThemeHandler>(true);
            }

            for (int i = 0; i < _themeHandlers.Length; i++)
            {
                var handler = _themeHandlers[i];
                
                if (handler.themeType is ThemeType.Color)
                    switch (handler.themeTarget)
                    {
                        case ThemeTarget.Background:
                            handler.ApplyTheme(GetBackgroundColor, instant);
                            continue;
                        case ThemeTarget.Text:
                            handler.ApplyTheme(GetTextColor, instant);
                            continue;
                    }
                
                if (handler.themeType is ThemeType.Sprite)
                    switch (handler.themeTarget)
                    {
                        case ThemeTarget.Grid:
                            handler.ApplyTheme(GetGridSprite, instant);
                            continue;
                        case ThemeTarget.Cell:
                            handler.ApplyTheme(GetCellSprite, instant);
                            continue;
                        case ThemeTarget.Menu:
                            handler.ApplyTheme(GetMenuSprite, instant);
                            continue;
                    }
            }
            
            themeText.SetText(theme is Theme.Light ? _lightThemeString : _darkThemeString);
        }

        public void ToggleTheme()
        {
            if (theme is Theme.Light)
                theme = Theme.Dark;
            else
                theme = Theme.Light;
            
            ApplyTheme();

            OnSettingChange?.Invoke();
        }

        public void ToggleSound()
        {
            useSound = !useSound;
            soundText.SetText(useSound ? _soundOnString : _soundOffString);
            OnSoundChange?.Invoke(useSound);
            OnSettingChange?.Invoke();
        }

        public void SetSound(bool value)
        {
            useSound = value;
            soundText.SetText(value ? _soundOnString : _soundOffString);
            OnSoundChange?.Invoke(value);
        }

        public void ToggleVibrate()
        {
            useVibrate = !useVibrate;
            vibrateText.SetText(useVibrate ? _vibrateOnString : _vibrateOffString);
            OnVibrateChange?.Invoke(useVibrate);
            OnSettingChange?.Invoke();
        }

        public void SetVibrate(bool value)
        {
            useVibrate = value;
            vibrateText.SetText(value ? _vibrateOnString : _vibrateOffString);
            OnVibrateChange?.Invoke(value);
        }
    }
}
