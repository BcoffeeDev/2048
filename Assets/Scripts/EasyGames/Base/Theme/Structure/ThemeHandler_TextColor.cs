using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace EasyGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class ThemeHandler_TextColor : ThemeHandler
    {
        private TextMeshProUGUI _text;
        
        public override void ApplyTheme(object source, bool instant)
        {
            if (_text == null)
                _text = GetComponent<TextMeshProUGUI>();

            if (instant)
                _text.color = (Color)source;
            else
                _text.DOColor((Color)source, .18f);
        }
    }
}
