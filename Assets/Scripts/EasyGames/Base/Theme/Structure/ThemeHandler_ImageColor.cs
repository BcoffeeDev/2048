using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace EasyGames
{
    [RequireComponent(typeof(Image))]
    public sealed class ThemeHandler_ImageColor : ThemeHandler
    {
        private Image _image;
        
        public override void ApplyTheme(object source, bool instant)
        {
            if (_image == null)
                _image = GetComponent<Image>();
            
            if (instant)
                _image.color = (Color)source;
            else
                _image.DOColor((Color)source, .18f);
        }
    }
}
