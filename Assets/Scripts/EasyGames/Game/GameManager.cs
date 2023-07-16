using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    public class GameManager : MonoBehaviour
    {
        [Title("Module")] 
        public GridBoard gridBoard;
        public Gesture gesture;
        public Leaderboard leaderboard;
        public Dialog dialog;
        public Keyboard keyboard;
        public Setting setting;
        public Storage storage;
        public UIPageManager uIPageManager;
        public SoundManager soundManager;
        public VibrationManager vibrationManager;

        [Title("Sound")]
        public SoundPlayer clickSound;
        public SoundPlayer moveSound;
        public SoundPlayer mergeSound;

        [Title("Vibrate")]
        public VibrationPlayer clickVibrate;
        public VibrationPlayer moveVibrate;
        public VibrationPlayer mergeVibrate;

        [Title("Setting")]
        public GameObject resumeText;
        public GameObject mainMenuText;

        private bool _isCanNewGame;

        private void Start()
        {
            Connect();

            // Load setting
            storage.Load(out StorageData_Setting settingData);
            setting.theme = settingData.theme;
            setting.ApplyTheme(true, true);
            setting.SetSound(settingData.useSound);
            setting.SetVibrate(settingData.useVibrate);

            // Load score
            storage.Load(out StorageData_Leaderboard leaderboardData);
            leaderboard.LeaderboardScore = leaderboardData.LeaderboardScore;
            
            // Can resume
            CheckCanResume();
        }

        public void NewGame()
        {
            storage.Delete(StoragePath.Game);
            Initialize();
            OpenGame();
        }

        public void Resume()
        {
            Initialize();
            OpenGame();
        }

        public void Back()
        {
            if (uIPageManager.LastPageType is UIPageType.Game)
                OpenGame();
            if (uIPageManager.LastPageType is UIPageType.MainMenu)
                OpenMenu();
        }

        public void MainMenu()
        {
            OpenMenu();
        }

        private void Initialize()
        {
            // Load game
            storage.Load(out StorageData_Game gameFile);
            if (gameFile == null || gameFile.GridCells.Count <= 0)
                CreateGame(true);
            else
            {
                gridBoard.GridCells = gameFile.GridCells;
                leaderboard.Score = gameFile.Score;

                gridBoard.isActive = true;
                dialog.Hide();
                _isCanNewGame = false;
            }

            setting.ApplyTheme(true, true);
        }

        #region Game

        public void CreateGame()
        {
            CreateGame(false);
        }
        
        private void CreateGame(bool isLoadGame)
        {
            if (!isLoadGame && !_isCanNewGame)
                return;
            
            gridBoard.Clear();
            gridBoard.CreateGrid();
            gridBoard.isActive = true;
            
            leaderboard.Score = 0;
            
            dialog.Hide();
            
            // SaveGame();
            
            setting.ApplyTheme(true, true);

            _isCanNewGame = false;
        }

        private void GameOver()
        {
            dialog.Show();
            leaderboard.ScoreToLeaderboard();
            SaveLeaderboard();
            
            storage.Delete(StoragePath.Game);
            
            _isCanNewGame = true;
        }

        #endregion

        #region Save

        private void SaveGame()
        {
            var gameFile = new StorageData_Game(gridBoard.GridCells, leaderboard.Score);
            storage.Save(gameFile);
        }

        private void SaveLeaderboard()
        {
            var leaderboardFile = new StorageData_Leaderboard();
            leaderboardFile.LeaderboardScore = leaderboard.LeaderboardScore;
            storage.Save(leaderboardFile);
        }

        private void SaveSetting()
        {
            var settingFile = new StorageData_Setting();
            settingFile.useSound = setting.useSound;
            settingFile.useVibrate = setting.useVibrate;
            settingFile.theme = setting.theme;
            storage.Save(settingFile);
        }

        #endregion

        #region ConnectHandler
        
        private void Connect()
        {
            // Control
            gesture.gameObject.SetActive(setting.settingProfile.GetControlTypeByPlatform is ControlType.Gesture);
            keyboard.gameObject.SetActive(setting.settingProfile.GetControlTypeByPlatform is ControlType.Keyboard);
            
            if (setting.settingProfile.GetControlTypeByPlatform is ControlType.Gesture)
            {
                Gesture.OnSwipeUp += gridBoard.MoveUp;
                Gesture.OnSwipeDown += gridBoard.MoveDown;
                Gesture.OnSwipeLeft += gridBoard.MoveLeft;
                Gesture.OnSwipeRight += gridBoard.MoveRight;
            }
            else
            {
                Keyboard.UpKeyPressed += gridBoard.MoveUp;
                Keyboard.DownKeyPressed += gridBoard.MoveDown;
                Keyboard.LeftKeyPressed += gridBoard.MoveLeft;
                Keyboard.RightKeyPressed += gridBoard.MoveRight;
            }
            
            // AddScore
            GridBoard.OnMergeGrid += leaderboard.AddScore;
            
            // GameOver
            GridBoard.OnGridCannotMove += GameOver;
            
            // Move
            GridBoard.OnGridIsMove += SaveGame;
            
            // Setting
            Setting.OnSettingChange += SaveSetting;
            Setting.OnSoundChange += useSound => soundManager.UseSound = useSound;
            Setting.OnVibrateChange += useVibrate => vibrationManager.UseVibrate = useVibrate;
            
            // Feedbacks
            GridBoard.OnMergeGrid += _ => mergeSound.Play();
            GridBoard.OnMergeGrid += _ => mergeVibrate.Play();
            GridBoard.OnGridIsMove += moveSound.Play;
            GridBoard.OnGridIsMove += moveVibrate.Play;
            Setting.OnSettingChange += () =>
            {
                clickSound.Play();
                clickVibrate.Play();
            };
            
            // Effect
            GridBoard.OnCreateGridEffect += setting.ApplyTheme;
        }

        private void Disconnect()
        {
            // Control
            Gesture.OnSwipeUp -= gridBoard.MoveUp;
            Gesture.OnSwipeDown -= gridBoard.MoveDown;
            Gesture.OnSwipeLeft -= gridBoard.MoveLeft;
            Gesture.OnSwipeRight -= gridBoard.MoveRight;
            
            // AddScore
            GridBoard.OnMergeGrid -= leaderboard.AddScore;
            
            // GameOver
            GridBoard.OnGridCannotMove -= GameOver;
            
            // Move
            GridBoard.OnGridIsMove -= SaveGame;
            
            // Setting
            Setting.OnSettingChange -= SaveSetting;
            Setting.OnSoundChange -= useSound => soundManager.UseSound = useSound;
            Setting.OnVibrateChange -= useVibrate => vibrationManager.UseVibrate = useVibrate;
            
            // Feedbacks
            GridBoard.OnMergeGrid -= _ => mergeSound.Play();
            GridBoard.OnMergeGrid -= _ => mergeVibrate.Play();
            GridBoard.OnGridIsMove -= moveSound.Play;
            GridBoard.OnGridIsMove -= moveVibrate.Play;
            Setting.OnSettingChange -= () =>
            {
                clickSound.Play();
                clickVibrate.Play();
            };
            
            // Effect
            GridBoard.OnCreateGridEffect -= setting.ApplyTheme;
        }

        #endregion

        #region Page

        public void OpenMenu()
        {
            clickSound.Play();
            clickVibrate.Play();
            uIPageManager.ShowPageFocus(UIPageType.MainMenu, false);

            gesture.enabled = false;
            keyboard.enabled = false;
            
            CheckCanResume();
        }
        
        public void OpenGame()
        {
            clickSound.Play();
            clickVibrate.Play();
            uIPageManager.ShowPageFocus(UIPageType.Game, false);

            gesture.enabled = true;
            keyboard.enabled = true;
        }
        
        public void OpenSetting()
        {
            clickSound.Play();
            clickVibrate.Play();
            uIPageManager.ShowPageFocus(UIPageType.Setting, false);
            mainMenuText.SetActive(uIPageManager.LastPageType is UIPageType.Game);

            gesture.enabled = false;
            keyboard.enabled = false;
        }

        public void OpenLeaderboard()
        {
            clickSound.Play();
            clickVibrate.Play();
            uIPageManager.ShowPageFocus(UIPageType.Leaderboard, false);
            mainMenuText.SetActive(uIPageManager.LastPageType is UIPageType.Game);

            gesture.enabled = false;
            keyboard.enabled = false;
        }

        #endregion

        private void CheckCanResume()
        {
            resumeText.SetActive(StorageUtility.Exist(StoragePath.Game));
        }
    }
}
