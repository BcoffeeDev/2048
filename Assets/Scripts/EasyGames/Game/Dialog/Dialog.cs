using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    public class Dialog : MonoBehaviour
    {
        public GameObject gameOverObject;
        
        public void Show(DialogType dialogType)
        {
            gameOverObject.SetActive(dialogType == DialogType.GameOver);
        }

        public void Hide()
        {
            gameOverObject.SetActive(false);
        }
    }
}
