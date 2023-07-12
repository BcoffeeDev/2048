using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    public class SoundPlayer : MonoBehaviour
    {
        public SoundType soundType;

        private AudioSource _audioSource;
        private AudioClip _audioClip;

        public void Initialize(AudioSource audioSource, AudioClip audioClip)
        {
            _audioSource = audioSource;
            _audioClip = audioClip;
        }
        
        public void Play()
        {
            if (_audioSource.isPlaying && _audioSource.time < .15f) return; 
            _audioSource.Stop();
            _audioSource.clip = _audioClip;
            if (_audioSource.clip == null) return;
            _audioSource.Play();
        }
    }
}