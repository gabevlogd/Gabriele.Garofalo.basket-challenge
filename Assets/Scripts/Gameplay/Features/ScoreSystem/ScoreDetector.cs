using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreDetector : MonoBehaviour
    {
        public static event Action<Character, ThrowOutcome> OnScoreDetected;
        
        private Collider _ballChecker;

        private void Awake()
        {
            if (!TryGetComponent(out _ballChecker))
            {
                Debug.LogError("ScoreDetector requires a Collider component to function properly.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BasketBall ball))
            {
                if (ball.LastThrowOutcome is ThrowOutcome.None or ThrowOutcome.LongMiss or ThrowOutcome.ShortMiss or ThrowOutcome.BackboardMiss)
                {
                    Debug.Log("Score detection ignored due to throw outcome: " + ball.LastThrowOutcome);
                    return;
                }

                // Prevent multiple score detections for the same throw
                if (ball.hasScored) return;
                
                ball.hasScored = true;
                OnScoreDetected?.Invoke(ball.BallOwner, ball.LastThrowOutcome);
            }
        }
    }
}
