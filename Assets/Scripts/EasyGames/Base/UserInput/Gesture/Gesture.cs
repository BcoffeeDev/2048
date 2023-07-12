using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

namespace EasyGames
{
    public class Gesture : MonoBehaviour
    {
        #region Actions

        public static Action OnTap;
        public static Action OnDoubleTap;
        public static Action OnTripleTap;
        public static Action OnSwipeUp;
        public static Action OnSwipeDown;
        public static Action OnSwipeLeft;
        public static Action OnSwipeRight;

        private void Tap()
        {
            OnTap?.Invoke();
        }
        
        private void DoubleTap()
        {
            OnDoubleTap?.Invoke();
        }
        
        private void TripleTap()
        {
            OnTripleTap?.Invoke();
        }
        
        private void SwipeUp()
        {
            OnSwipeUp?.Invoke();
        }
        
        private void SwipeDown()
        {
            OnSwipeDown?.Invoke();
        }
        
        private void SwipeLeft()
        {
            OnSwipeLeft?.Invoke();
        }
        
        private void SwipeRight()
        {
            OnSwipeRight?.Invoke();
        }

        #endregion

        [Range(100, 1000)]
        public float ignoreMagnitude = 100;

        [Range(0, 200)]
        public float avoidPadding = 50;

        private void OnEnable()
        {
            LeanTouch.OnFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerUp -= OnFingerUp;
        }
        
        private void OnFingerUp(LeanFinger finger)
        {
            if (!gameObject.activeSelf)
                return;
            
            var start = finger.StartScreenPosition;
            var end = finger.LastScreenPosition;

            if (!IsGestureCanUse(start))
                return;
            
            var magnitude = (end - start).magnitude;
            if (magnitude <= ignoreMagnitude)
            {
                BeginTap();
                return; // detect as tap
            }

            var distance = end - start;
            distance.x = Mathf.Abs(distance.x);
            distance.y = Mathf.Abs(distance.y);

            if ((int)distance.x == (int)distance.y)
                return; // detect as tap

            var isVertical = distance.y > distance.x;
            
            // Vertical swipe
            if (isVertical)
            {
                var isUp = end.y > start.y;
                if (isUp)
                    SwipeUp();
                else
                    SwipeDown();
                return;
            }

            // Horizontal swipe
            var isRight = end.x > start.x;
            if (isRight)
                SwipeRight();
            else
                SwipeLeft();
        }
        
        private int _tapCount;

        private void BeginTap()
        {
            _tapCount++;
            CancelInvoke(nameof(EndTap));
            if (_tapCount >= 3)
            {
                EndTap();
                return;
            }
            Invoke(nameof(EndTap), 0.3f);
        }

        private void EndTap()
        {
            CancelInvoke(nameof(EndTap));
            
            switch (_tapCount)
            {
                case >= 3:
                    TripleTap();
                    _tapCount = 0;
                    return;
                case 2:
                    DoubleTap();
                    _tapCount = 0;
                    return;
                default:
                    Tap();
                    _tapCount = 0;
                    break;
            }
        }

        private bool IsGestureCanUse(Vector2 position)
        {
            var screenHeight = Screen.currentResolution.height;
            var screenWidth = Screen.currentResolution.width;

            if (position.x < avoidPadding)
                return false;
            if (position.x > screenWidth - avoidPadding)
                return false;
            if (position.y < avoidPadding)
                return false;
            if (position.y > screenHeight - avoidPadding)
                return false;

            return true;
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var screenHeight = Screen.currentResolution.height;
            var screenWidth = Screen.currentResolution.width;

            var topLeft = new Vector2(avoidPadding, screenHeight - avoidPadding);
            var topRight = new Vector2(screenWidth - avoidPadding, screenHeight - avoidPadding);
            var botLeft = new Vector2(avoidPadding, avoidPadding);
            var botRight = new Vector2(screenWidth - avoidPadding, avoidPadding);
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(botLeft, botRight);
            Gizmos.DrawLine(topLeft, botLeft);
            Gizmos.DrawLine(topRight, botRight);
        }

#endif

        #endregion
    }
}