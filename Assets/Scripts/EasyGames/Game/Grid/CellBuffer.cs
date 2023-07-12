using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace EasyGames
{
    public class CellBuffer : MonoBehaviour
    {
        [Title("UI")]
        public TextMeshProUGUI numberText;
        public CanvasGroup canvasGroup;
        
        private RectTransform _rectTransform;
        
        public void SetNumber(int value)
        {
            if (value != 0)
                numberText.SetText($"{value}");
            else
                numberText.SetText(string.Empty);

            canvasGroup.alpha = value == 0 ? 0 : 1;
        }
    }
}