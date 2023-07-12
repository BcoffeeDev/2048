using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace EasyGames
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPage : MonoBehaviour
    {
        [SerializeField] private bool hideOnAwake;
        
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
            if (!hideOnAwake) return;
            Hide(true);
        }

        public void Show(bool instant)
        {
            var duration = instant ? 0 : .1f;
            _canvasGroup
                .DOFade(1, duration)
                .OnStart(() =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                });
        }

        public void Hide(bool instant)
        {
            var duration = instant ? 0 : .1f;
            _canvasGroup
                .DOFade(0, duration)
                .OnStart(() =>
                {
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                });
        }
    }
}
