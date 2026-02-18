using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreReceiver : MonoBehaviour
    {
        public static event Action<ShootingCharacter, int> OnScoreUpdated;
        
        private ShootingCharacter _currentScoreOwner;
        
        public int CurrentScore { get; private set; }

        private void Awake()
        {
            if (!TryGetComponent(out _currentScoreOwner))
            {
                Debug.LogError("ScoreReceiver requires a ShootingCharacter component to function properly.");
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

        private void ScoreReceived(ShootingCharacter scoreOwner, ThrowOutcome scoreOutcome)
        {
            Debug.Log("Score received: " + scoreOutcome);
            if (scoreOwner != _currentScoreOwner) return;
            
            int scoreAmount = GetScoreAmountForOutcome(scoreOutcome);
            
            scoreAmount += GetBonusScoreAmount(scoreOwner.CurrentBall);
            
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

        private int GetBonusScoreAmount(BasketBall ball)
        {
            int bonusScore = 0;
            
            if (ball.lastBackboardBonus != null)
            {
                bonusScore += ball.lastBackboardBonus.extraPoints;
            }
            
            if (ball.OnFire)
            {
                bonusScore *= 2; 
            }

            return bonusScore;
        }
    }
}