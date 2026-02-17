using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreReceiver : MonoBehaviour
    {
        public static event Action<Character, int> OnScoreUpdated;
        
        private Character _currentScoreOwner;
        
        public int CurrentScore { get; private set; }

        private void Awake()
        {
            if (!TryGetComponent(out _currentScoreOwner))
            {
                Debug.LogError("ScoreReceiver requires a Character component to function properly.");
            }
        }

        private void OnEnable()
        {
            ScoreDetector.OnScoreDetected += ScoreReceived;
        }

        private void OnDisable()
        {
            ScoreDetector.OnScoreDetected -= ScoreReceived;
        }

        private void ScoreReceived(Character scoreOwner, ThrowOutcome scoreOutcome)
        {
            Debug.Log("Score received: " + scoreOutcome);
            if (scoreOwner != _currentScoreOwner) return;
            
            int scoreAmount = GetScoreAmountForOutcome(scoreOutcome);
            
            CurrentScore += scoreAmount;
            
            OnScoreUpdated?.Invoke(scoreOwner, CurrentScore);
        }

        private int GetScoreAmountForOutcome(ThrowOutcome scoreOutcome)
        {
            return scoreOutcome switch
            {
                ThrowOutcome.Perfect => 3,
                ThrowOutcome.NearRim => 2,
                ThrowOutcome.FarRim => 2,
                ThrowOutcome.BackboardMake => 2,
                _ => 0
            };
        }
    }
}