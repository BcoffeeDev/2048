using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [Title("Clips")]
        [SerializeField] private AudioClip clickClip;
        [SerializeField] private AudioClip moveClip;
        [SerializeField] private AudioClip mergeClip;

        private SoundPlayer[] _soundPlayers;
        private AudioSource _audioSource;
        private bool _useSound;

        public bool UseSound
        {
            get => _useSound;
            set
            {
                _useSound = value;
                _audioSource.mute = !value;
            }
        }

        private void Awake()
        {
            _soundPlayers = FindObjectsOfType<SoundPlayer>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            for (int i = 0; i < _soundPlayers.Length; i++)
            {
                var soundPlayer = _soundPlayers[i];
                soundPlayer.Initialize(_audioSource, GetAudioClipByType(soundPlayer.soundType));
            }
        }

        private AudioClip GetAudioClipByType(SoundType type)
        {
            switch (type)
            {
                case SoundType.Click:
                    return clickClip;
                case SoundType.Move:
                    return moveClip;
                case SoundType.Merge:
                    return mergeClip;
                default:
                    return null;
            }
        }
    }
}
