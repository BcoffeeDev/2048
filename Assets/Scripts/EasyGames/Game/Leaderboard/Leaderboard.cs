using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace EasyGames
{
    public class Leaderboard : MonoBehaviour
    {
        [Title("Text")]
        public TextMeshProUGUI scoreText;

        [Title("Leaderboard")] 
        public TextMeshProUGUI[] leaderboardText;
        
        private int _score;
        private List<int> _leaderboardScore = new(5);
        
        private const string _emptyLeaderboardString = "-";

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                scoreText.SetText($"{value}");
            }
        }

        public List<int> LeaderboardScore
        {
            get => _leaderboardScore;
            set
            {
                _leaderboardScore = value;
                while (_leaderboardScore.Count < 5)
                {
                    _leaderboardScore.Add(0);
                }
                CheckLeaderboard();
            }
        }

        public void AddScore(int value)
        {
            Score += value;
        }

        public void ScoreToLeaderboard()
        {
            for (int i = 0; i < _leaderboardScore.Count; i++)
            {
                var highScore = _leaderboardScore[i];
                if (Score <= highScore)
                    continue;
                if (i == _leaderboardScore.Count - 1)
                    _leaderboardScore[i] = Score;
                else
                {
                    _leaderboardScore.Insert(i, Score);
                    _leaderboardScore.RemoveAt(_leaderboardScore.Count - 1);
                }
                    
                break;
            }
            
            CheckLeaderboard();
        }

        private void CheckLeaderboard()
        {
            for (int i = 0; i < leaderboardText.Length; i++)
            {
                var text = leaderboardText[i];
                    
                if (_leaderboardScore[i] <= 0)
                {
                    text.SetText(_emptyLeaderboardString);
                    continue;
                }
                    
                text.SetText($"{_leaderboardScore[i]}");
            }
        }
    }
}
