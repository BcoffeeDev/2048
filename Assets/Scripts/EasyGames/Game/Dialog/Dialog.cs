using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace EasyGames
{
    public class Dialog : MonoBehaviour
    {
        public GameObject gameOverObject;
        
        public void Show()
        {
            gameOverObject.SetActive(true);
            gameOverObject.transform.DOScale(Vector3.one, 0.15f).From(Vector3.zero).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            gameOverObject.SetActive(false);
        }
    }
}
