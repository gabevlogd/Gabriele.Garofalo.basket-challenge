using System;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(FireBallComponent))]
    public class BasketBall : MonoBehaviour
    {
        public event Action<Collision> OnBallCollisionEnter;
        public event Action OnBallResetEvent;
        
        public ShootingCharacter BallOwner { get; set; }
        
        public Rigidbody Rigidbody => _rigidbody;
        private Rigidbody _rigidbody;
     
        public FireBallComponent FireBallComponent => _fireBallComponent;
        private FireBallComponent _fireBallComponent;

        public BackboardBonus lastBackboardBonus;

        public ThrowOutcome LastThrowOutcome { get; private set; }
        
        [SerializeField]
        private ParticleSystem fireBallParticleSystemPrefab;
        private ParticleSystem _fireBallParticleSystemInstance;

        public bool HasScored { get; set; }
        public bool OnFire => _fireBallComponent && _fireBallComponent.OnFire;

        private int _collisionCount;
        
        protected void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("BasketBall requires a Rigidbody component to function properly.");
            }
            
            if (!TryGetComponent(out _fireBallComponent))
            {
                Debug.LogError("BasketBall requires a FireBallComponent to function properly.");
            }
        }

        private void OnEnable()
        {
            if (_fireBallComponent && fireBallParticleSystemPrefab)
            {
                SpawFireBallParticles();
                _fireBallComponent.OnBallIgnited += BallIgnited;
                _fireBallComponent.OnBallExtinguished += BallExtinguished;
            }
        }

        private void OnDisable()
        {
            if (_fireBallComponent)
            {
                _fireBallComponent.OnBallIgnited -= BallIgnited;
                _fireBallComponent.OnBallExtinguished -= BallExtinguished;
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
            HasScored = false;
            lastBackboardBonus = null;
            EnablePhysics();
            transform.parent = null;
        }

        public void OnBallReset()
        {
            DisablePhysics();
            OnBallResetEvent?.Invoke();
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
                    ApplyVelocityCorrectionToNegateScore(collision);
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
            Vector3 correction = ThrowPositionsHandler.GetPerfectThrowPosition() - transform.position;
            Rigidbody.velocity = correction * speed;
        }

        private void ApplyVelocityCorrectionToNegateScore(Collision collision)
        {
            float speed = Rigidbody.velocity.magnitude;
            Rigidbody.velocity = collision.GetContact(0).normal * speed;
        }
        
        public void EnablePhysics()
        {
            Rigidbody.isKinematic = false;
        }
        
        public void DisablePhysics()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.isKinematic = true;
        }
        
        private void BallIgnited()
        {
            if (_fireBallParticleSystemInstance)
            {
                _fireBallParticleSystemInstance.Play();
            }
        }

        private void BallExtinguished()
        {
            if (_fireBallParticleSystemInstance)
            {
                _fireBallParticleSystemInstance.Stop();
            }
        }

        private void SpawFireBallParticles()
        {
            _fireBallParticleSystemInstance = Instantiate(fireBallParticleSystemPrefab, transform);
            _fireBallParticleSystemInstance.transform.localPosition = Vector3.zero;
            _fireBallParticleSystemInstance.Stop();
        }
    }
}