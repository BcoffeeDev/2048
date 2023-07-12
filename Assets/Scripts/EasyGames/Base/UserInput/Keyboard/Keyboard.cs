using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    public class Keyboard : MonoBehaviour
    {
        #region Actions

        public static Action SpaceKeyPressed; // Tap
        // todo doubleTap
        // todo tripleTap
        public static Action UpKeyPressed;
        public static Action DownKeyPressed;
        public static Action LeftKeyPressed;
        public static Action RightKeyPressed;

        private void OnSpaceKeyPressed()
        {
            SpaceKeyPressed?.Invoke();
        }

        private void OnUpKeyPressed()
        {
            UpKeyPressed?.Invoke();
        }

        private void OnDownKeyPressed()
        {
            DownKeyPressed?.Invoke();
        }

        private void OnLeftKeyPressed()
        {
            LeftKeyPressed?.Invoke();
        }

        private void OnRightKeyPressed()
        {
            RightKeyPressed?.Invoke();
        }

        #endregion

        private void Update()
        {
            if (!gameObject.activeSelf)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
                OnSpaceKeyPressed();
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                OnUpKeyPressed();
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                OnDownKeyPressed();
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                OnLeftKeyPressed();
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                OnRightKeyPressed();
        }
    }
}
