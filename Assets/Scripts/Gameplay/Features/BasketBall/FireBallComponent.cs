using System;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(BasketBall))]
    public class FireBallComponent : MonoBehaviour
    {
        [SerializeField]
        private FireBallData data;
        
        private BasketBall _ball;

        private float _currentFireAmount;
        
        private float _fireRemainingTime;
        
        public bool OnFire { get; private set; }
        
        public event Action OnBallIgnited;
        public event Action OnBallExtinguished;
        public event Action<float> OnFireAmountChanged;

        private void Awake()
        {
            if (!TryGetComponent(out _ball))
            {
                Debug.LogError("FireBallComponent requires a BasketBall component to function properly.");
            }
            
            if (data == null)
            {
                Debug.LogError("FireBallComponent requires a reference to FireBallData.");
            }
        }

        private void OnEnable()
        {
            ScoreReceiver.OnScoreUpdated += HandleScoreUpdated;

            if (_ball)
            {
                _ball.OnBallResetEvent += HandleBallReset;
            }
        }

        private void OnDisable()
        {
            ScoreReceiver.OnScoreUpdated -= HandleScoreUpdated;
            
            if (_ball)
            {
                _ball.OnBallResetEvent -= HandleBallReset;
            }
        }

        private void Update()
        {
            UpdateFireAmount();
        }

        private void UpdateFireAmount()
        {
            if (OnFire)
            {
                if (_fireRemainingTime > 0f)
                {
                    _fireRemainingTime -= Time.deltaTime;
                    SetFireAmount(_fireRemainingTime / data.fireDuration);
                }
                else
                {
                    ExtinguishBall();
                }
            }
            else if (_currentFireAmount > 0f)
            {
                RemoveFireAmount(data.fireAmountDecraseSpeed * Time.deltaTime);
            }
        }

        private void HandleScoreUpdated(ShootingCharacter scoreOwner, int currentScore)
        {
            // Only react to scores made by the owner of this ball sice ScoreDetector.OnScoreDetected is a static event and can be triggered by any ball in the scene
            if (scoreOwner.CurrentBall != _ball) return;

            switch (_ball.LastThrowOutcome)
            {
                case ThrowOutcome.Perfect:
                case ThrowOutcome.NearRim:
                case ThrowOutcome.FarRim:
                case ThrowOutcome.BackboardMake:
                    // Increase the fire amount for each successful score only if the ball is not already on fire
                    if (OnFire) break;
                    AddFireAmount(data.fireAmountPerScore);
                    break;
                default:
                    // For any other score outcome (misses), reset the fire amount
                    // (this is just for eventual error handling, since the ScoreReceiver should only trigger for successful scores, but it's good to be safe)
                    SetFireAmount(0f);
                    break;
            }
            
            if (_currentFireAmount >= 1f)
            {
                IgniteBall();
            }
        }

        private void HandleBallReset()
        {
            if (!_ball) return;

            switch (_ball.LastThrowOutcome)
            {
                case ThrowOutcome.BackboardMiss:
                case ThrowOutcome.ShortMiss:
                case ThrowOutcome.LongMiss:
                    // This is the real reset case for a missed throw, so we want to make sure the fire amount is reset in this case
                    if (OnFire) ExtinguishBall();
                    else SetFireAmount(0f);
                    break;
            }
        }

        private void IgniteBall()
        {
            OnFire = true;
            _fireRemainingTime = data.fireDuration;
            OnBallIgnited?.Invoke();
        }
        
        private void ExtinguishBall()
        {
            OnFire = false;
            _fireRemainingTime = 0f;
            SetFireAmount(0f);
            OnBallExtinguished?.Invoke();
        }
        
        private void AddFireAmount(float amount)
        {
            SetFireAmount(_currentFireAmount + Mathf.Abs(amount));
        }
        
        private void RemoveFireAmount(float amount)
        {
            SetFireAmount(_currentFireAmount - Mathf.Abs(amount));
        }
        
        private void SetFireAmount(float newFireAmount)
        {
            _currentFireAmount = Mathf.Clamp(newFireAmount, 0f, 1f);
            OnFireAmountChanged?.Invoke(_currentFireAmount);
        }
        
    }
}
