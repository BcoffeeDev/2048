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
        public TextMeshProUGUI powerSaverText;

        [Title("BaseSetting")]
        public SettingProfile settingProfile;
        
        [Title("Setting")]
        public Theme theme = Theme.Light;
        public bool useSound = true;
        public bool useVibrate = true;
        public bool usePowerSaver = false;

        private CustomizeSource[] _customizeSources;

        #region Const

        private const string _soundOnString = "Sound : ON";
        private const string _soundOffString = "Sound : OFF";
        private const string _vibrateOnString = "Vibrate : ON";
        private const string _vibrateOffString = "Vibrate : OFF";
        private const string _lightThemeString = "Light Mode";
        private const string _darkThemeString = "Dark Mode";
        private const string _powerSaverOnString = "Power Saver : ON";
        private const string _powerSaverOffString = "Power Saver : OFF";

        #endregion
        
        private void Awake()
        {
            _customizeSources = FindObjectsOfType<CustomizeSource>(true);
        }

        #region Action

        public static Action OnSettingChange;
        public static Action<bool> OnSoundChange;
        public static Action<bool> OnVibrateChange;
        public static Action<bool> OnPowerSaverChange;

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
                _customizeSources = FindObjectsOfType<CustomizeSource>(true);
            }

            for (int i = 0; i < _customizeSources.Length; i++)
            {
                var source = _customizeSources[i];
                var profile = settingProfile.ThemeProfile[theme];
                source.SetProfile(profile);
                source.Customize(instant);
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

        public void TogglePowerSaver()
        {
            usePowerSaver = !usePowerSaver;
            powerSaverText.SetText(usePowerSaver ? _powerSaverOnString : _powerSaverOffString);
            OnPowerSaverChange?.Invoke(usePowerSaver);
            OnSettingChange?.Invoke();
        }

        public void SetPowerSaver(bool value)
        {
            usePowerSaver = value;
            powerSaverText.SetText(value ? _powerSaverOnString : _powerSaverOffString);
            OnPowerSaverChange?.Invoke(value);
        }
    }
}
