using EasyGames.Pattern;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyGames.Base.BatterySaver
{
    public class BatterySaver : SingletonPattern<BatterySaver>
    {
        public int fpsWhenBatterySaver = 10;
        public int timeForWait = 5;
        public TextMeshProUGUI fpsText;

        private GameInput _gameInput;

        private bool _enablePowerSaver;

        public bool IsBatterySaverActivated { get; private set; }

        public bool EnablePowerSaver
        {
            get => _enablePowerSaver;
            set
            {
                _enablePowerSaver = value;
                SetBatterySaver(value);
            }
        }
        
        private void Awake()
        {
            _gameInput = new GameInput();
        }

        private void OnEnable()
        {
            _gameInput.Default.Enable();
            
            _gameInput.Default.Any.started += OnFingerDown;
            _gameInput.Default.Any.canceled += OnFingerUp;
            _gameInput.Default.MultiTap.performed += OnMultiTap;
        }
        
        private void OnDisable()
        {
            _gameInput.Default.Disable();
            
            _gameInput.Default.Any.started -= OnFingerDown;
            _gameInput.Default.Any.canceled -= OnFingerUp;
            _gameInput.Default.MultiTap.performed -= OnMultiTap;
        }

        private void Start()
        {
            SetBatterySaver(true);
        }

        private void OnFingerDown(InputAction.CallbackContext context)
        {
            SetBatterySaver(false);
        }

        private void OnFingerUp(InputAction.CallbackContext context)
        {
            SetBatterySaver(true);
        }

        private void OnMultiTap(InputAction.CallbackContext context)
        {
            var active = fpsText.gameObject.activeSelf;
            active = !active;
            fpsText.gameObject.SetActive(active);
        }

        private void SetBatterySaver(bool value)
        {
            if (value)
            {
                if (!EnablePowerSaver) return;
                CancelInvoke(nameof(StartBatterySaver));
                Invoke(nameof(StartBatterySaver), timeForWait);
            }
            else
            {
                CancelInvoke(nameof(StartBatterySaver));
                StopBatterySaver();
            }
        }

        private void StartBatterySaver()
        {
            Application.targetFrameRate = fpsWhenBatterySaver;
            IsBatterySaverActivated = true;
        }

        private void StopBatterySaver()
        {
            Application.targetFrameRate = 120;
            IsBatterySaverActivated = false;
        }

        private void LateUpdate()
        {
            if (!fpsText.gameObject.activeSelf)
                return;
            fpsText.SetText($"{1f / Time.unscaledDeltaTime:0}");
        }
    }
}
