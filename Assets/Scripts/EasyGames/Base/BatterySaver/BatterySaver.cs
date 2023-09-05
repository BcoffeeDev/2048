using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyGames.Base.BatterySaver
{
    public class BatterySaver : MonoBehaviour
    {
        public int fpsWhenBatterySaver = 10;
        public int timeForWait = 5;
        public TextMeshProUGUI fpsText;

        private GameInput _gameInput;

        public bool IsBatterySaverActivated { get; private set; }
        
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
            Invoke(nameof(StartBatterySaver), timeForWait);
        }

        private void OnFingerDown(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(StartBatterySaver));
            StopBatterySaver();
        }

        private void OnFingerUp(InputAction.CallbackContext context)
        {
            Invoke(nameof(StartBatterySaver), timeForWait);
        }

        private void OnMultiTap(InputAction.CallbackContext context)
        {
            var active = fpsText.gameObject.activeSelf;
            active = !active;
            fpsText.gameObject.SetActive(active);
        }

        private void StartBatterySaver()
        {
#if UNITY_EDITOR
            print("BatterySaver: On");
#endif
            Application.targetFrameRate = fpsWhenBatterySaver;
            IsBatterySaverActivated = true;
        }

        private void StopBatterySaver()
        {
#if UNITY_EDITOR
            print("BatterySaver: Off");
#endif
            Application.targetFrameRate = 120;
            IsBatterySaverActivated = false;
        }

        private void LateUpdate()
        {
            fpsText.SetText($"{1f / Time.unscaledDeltaTime:0}");
        }
    }
}
