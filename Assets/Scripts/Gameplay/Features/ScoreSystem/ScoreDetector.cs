using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreDetector : MonoBehaviour
    {
        private static event Action<Character> OnScoreDetected;
        
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
                OnScoreDetected?.Invoke(ball.BallOwner);
                Debug.Log("Score detected");
            }
        }
    }
}
