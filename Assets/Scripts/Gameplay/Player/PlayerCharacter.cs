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

        public BasketBall CurrentBall { get; private set; }

        private ThrowerComponent _throwerComponent;
        public ThrowerComponent ThrowerComponent => _throwerComponent;
        
        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _throwerComponent))
            {
                Debug.LogError("PlayerCharacter requires a ThrowerComponent to function properly.");
            }
            CurrentBall = FindObjectOfType<BasketBall>();
            CurrentBall.BallOwner = this;
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
            if (CurrentBall == null)
            {
                Debug.LogWarning("No ball to throw.");
                return;
            }
            CurrentBall.transform.position = ballSocket.position;
            ThrowOutcome throwOutcome = _throwerComponent.Throw(CurrentBall.Rigidbody, BasketBackboard.GetPerfectShotPosition(), powerAmount);
            CurrentBall.OnBallThrown(throwOutcome);
        }

        [ContextMenu("Temp")]
        private void Temp()
        {
            _throwerComponent.UpdatePerfectPower(BasketBackboard.GetPerfectShotPosition(), ballSocket.position);
        }
        
        
    }
}