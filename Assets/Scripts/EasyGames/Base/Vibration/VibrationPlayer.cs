using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace EasyGames
{
    public class VibrationPlayer : MonoBehaviour
    {
        public VibrationType vibrationType;

        private MMF_Player _feedbackPlayer;
        
        public void Initialize(MMF_Player feedbackPlayer)
        {
            _feedbackPlayer = feedbackPlayer;
        }

        public void Play()
        {
            _feedbackPlayer.StopFeedbacks();
            
            var feedbacks = _feedbackPlayer.FeedbacksList;
            for (int i = 0; i < feedbacks.Count; i++)
            {
                var f = feedbacks[i];
                f.Active = false;
                if (f.Label != $"{vibrationType} Haptic") continue;
                f.Active = true;
            }
            
            _feedbackPlayer.PlayFeedbacks();
        }
    }
}