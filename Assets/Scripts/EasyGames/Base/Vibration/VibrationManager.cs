using System;
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    public class VibrationManager : MonoBehaviour
    {
        public MMF_Player feedbackPlayer;
        
        private VibrationPlayer[] _vibrationPlayers;
        private bool _useVibrate;

        public bool UseVibrate
        {
            get => _useVibrate;
            set
            {
                _useVibrate = value;
                feedbackPlayer.CanPlay = value;
            }
        }

        private void Awake()
        {
            _vibrationPlayers = FindObjectsOfType<VibrationPlayer>();
        }

        private void Start()
        {
            for (int i = 0; i < _vibrationPlayers.Length; i++)
            {
                var vibrationPlayer = _vibrationPlayers[i];
                vibrationPlayer.Initialize(feedbackPlayer);
            }
        }
    }
}