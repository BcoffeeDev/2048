using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace EasyGames
{
    public class TweenButton_Scale : MonoBehaviour, ITweenButton
    {
        public RectTransform rectTransform;

        private Vector3 _pressedScale = new Vector3(.93f, .93f, 1f);
        
        public void PressedTween(Action callback = null)
        {
            rectTransform.DOScale(_pressedScale, .1f).SetEase(Ease.OutCirc).From(Vector3.one).OnComplete(() => callback?.Invoke()).SetAutoKill();
        }

        public void ReleaseTween(Action callback = null)
        {
            rectTransform.DOScale(Vector3.one, .1f).SetEase(Ease.OutCirc).OnComplete(() => callback?.Invoke()).SetAutoKill();
        }
    }
}
