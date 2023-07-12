using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyGames
{
    public sealed class ThemeHandler_Sprite : ThemeHandler
    {
        private Image _image;
        
        public override void ApplyTheme(object source, bool instant)
        {
            if (_image == null)
                _image = GetComponent<Image>();

            _image.sprite = (Sprite)source;
        }
    }
}
