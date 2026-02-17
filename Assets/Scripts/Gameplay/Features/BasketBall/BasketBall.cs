using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class BasketBall : MonoBehaviour
    {
        public event Action<Collision> OnBallCollisionEnter;
        
        public Character BallOwner { get; set; }
        
        public Rigidbody Rigidbody => _rigidbody;
        private Rigidbody _rigidbody;

        public ThrowOutcome LastThrowOutcome { get; private set; }

        [HideInInspector]
        public bool hasScored;
        
        private int _collisionCount;
        
        protected void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("BasketBall requires a Rigidbody component to function properly.");
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleFirstCollision(other);
            OnBallCollisionEnter?.Invoke(other);
        }
        
        public void OnBallThrown(ThrowOutcome throwOutcome)
        {
            LastThrowOutcome = throwOutcome;
            _collisionCount = 0;
            hasScored = false;
        }
        
        private void HandleFirstCollision(Collision collision)
        {
            if (_collisionCount > 0)
                return;

            _collisionCount++;

            switch (LastThrowOutcome)
            {
                case ThrowOutcome.Perfect:
                case ThrowOutcome.NearRim:
                case ThrowOutcome.FarRim:
                case ThrowOutcome.BackboardMake:
                    ApplyVelocityCorrectionToScore();
                    break;
                case ThrowOutcome.BackboardMiss:
                    ApplyVelocityCorrectionToNegateScore();
                    break;
                case ThrowOutcome.LongMiss:
                case ThrowOutcome.ShortMiss:
                case ThrowOutcome.None:
                    // Nothing to do here, as the throw was already a miss, so no need to negate score
                    break;
            }
        }

        private void ApplyVelocityCorrectionToScore()
        {
            float speed = Rigidbody.velocity.magnitude;
            Vector3 correction = BasketBackboard.GetPerfectShotPosition() - transform.position;
            Rigidbody.velocity = correction * speed;
        }

        private void ApplyVelocityCorrectionToNegateScore()
        {
            float speed = Rigidbody.velocity.magnitude;
            Vector3 correction = transform.position - BasketBackboard.GetPerfectShotPosition();
            Rigidbody.velocity = correction * speed;
        }
    }
}