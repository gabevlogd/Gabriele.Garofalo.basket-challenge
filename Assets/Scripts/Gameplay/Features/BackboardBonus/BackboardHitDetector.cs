using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    public class BackboardHitDetector : MonoBehaviour
    {
        public event Action<BasketBall> OnBackboardHit;
        
        private BoxCollider _boxCollider;
        
        private void Awake()
        {
            if (!TryGetComponent(out _boxCollider))
            {
                Debug.LogError("BackboardHitDetector requires a BoxCollider component to function properly.");
            }
            else
            {
                _boxCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BasketBall ball))
            {
                OnBackboardHit?.Invoke(ball);
                SoundManager.Play("Backboard", false);
            }
        }
    }
}