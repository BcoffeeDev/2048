using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasyGames
{
    public class CustomizeSource : MonoBehaviour
    {
        public CustomizeType type = CustomizeType.Text;
        public ColorizeLayer colorizeLayer = ColorizeLayer.Cell;

        [ShowIf("@type == CustomizeType.Text"), SerializeField]
        private TextMeshProUGUI text;

        [ShowIf("@type == CustomizeType.Image"), SerializeField]
        private Image image;

        [ShowIf("@type == CustomizeType.Particle"), SerializeField]
        private new ParticleSystem particleSystem;

        private CustomizeProfile _profile;

#if UNITY_EDITOR
        private void OnValidate()
        {
            switch (type)
            {
                case CustomizeType.Text:
                    text = GetComponent<TextMeshProUGUI>();
                    break;
                case CustomizeType.Image:
                    image = GetComponent<Image>();
                    break;
                case CustomizeType.Particle:
                    particleSystem = GetComponent<ParticleSystem>();
                    break;
            }
        }
#endif

        public void SetProfile(CustomizeProfile profile)
        {
            _profile = profile;
        }
        
        public void Customize(bool instant = false)
        {
            if (_profile is null)
                return;

            var color = _profile.ColorizeData[colorizeLayer];
            var duration = instant ? 0 : 0.15f;

            switch (type)
            {
                case CustomizeType.Text:
                    text.DOColor(color, duration);
                    break;
                case CustomizeType.Image:
                    image.DOColor(color, duration);
                    break;
                case CustomizeType.Particle:
                    var main = particleSystem.main;
                    main.startColor = color;
                    break;
            }
        }

        private void OnDestroy()
        {
            switch (type)
            {
                case CustomizeType.Text:
                    text.DOKill();
                    break;
                case CustomizeType.Image:
                    image.DOKill();
                    break;
                case CustomizeType.Particle:
                    break;
            }
        }
    }
}
