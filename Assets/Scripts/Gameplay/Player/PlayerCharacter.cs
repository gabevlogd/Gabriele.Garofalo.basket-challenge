using System;
using UnityEngine;
using BasketChallenge.Core;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent))]
    public class PlayerCharacter : Character
    {
        
        [SerializeField]
        private Transform ballSocket;
        
        private BasketBall _currentBall;
        
        
        private ThrowerComponent _throwerComponent;
        public ThrowerComponent ThrowerComponent => _throwerComponent;
        
        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _throwerComponent))
            {
                Debug.LogError("PlayerCharacter requires a ThrowerComponent to function properly.");
            }
            _currentBall = FindObjectOfType<BasketBall>();
            _currentBall.BallOwner = this;
        }

        private void Start()
        {
            Temp();
        }

        private void OnEnable()
        {
            SwipeThrowController.OnThrowCompleted += ThrowBall;
        }
        
        private void OnDisable()
        {
            SwipeThrowController.OnThrowCompleted -= ThrowBall;
        }

        private void ThrowBall(float powerAmount)
        {
            if (_currentBall == null)
            {
                Debug.LogWarning("No ball to throw.");
                return;
            }
            _currentBall.transform.position = ballSocket.position;
            ThrowOutcome throwOutcome = _throwerComponent.Throw(_currentBall.Rigidbody, BasketBackboard.GetPerfectShotPosition(), powerAmount);
            _currentBall.OnBallThrown(throwOutcome);
        }

        [ContextMenu("Temp")]
        private void Temp()
        {
            _throwerComponent.UpdatePerfectPower(BasketBackboard.GetPerfectShotPosition(), ballSocket.position);
        }
        
        
    }
}