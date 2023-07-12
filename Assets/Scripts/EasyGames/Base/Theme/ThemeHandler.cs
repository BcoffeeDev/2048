using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EasyGames
{
    public abstract class ThemeHandler : MonoBehaviour
    {
        public ThemeType themeType = ThemeType.Color;
        public ThemeTarget themeTarget = ThemeTarget.Background;

        public abstract void ApplyTheme(object source, bool instant);
    }
}