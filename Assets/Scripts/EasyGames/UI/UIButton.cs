using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EasyGames
{
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool interactable = true;
        
        public UnityEvent pressedEvent;
        public UnityEvent releaseEvent;
        
        private bool _isExit;
        private ITweenButton _tween;

        private void Start()
        {
            _tween = GetComponent<ITweenButton>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
                return;
            
            _isExit = false;
            
            if (_tween != null)
                _tween.PressedTween(() => pressedEvent?.Invoke());
            else
                pressedEvent?.Invoke();
                
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isExit)
                return;
            if (!interactable)
                return;
            
            if (_tween != null)
                _tween.ReleaseTween(() => releaseEvent?.Invoke());
            else
                releaseEvent?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
                return;
            _isExit = true;
            _tween?.ReleaseTween();
        }
    }
}
