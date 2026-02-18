using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent))]
    public class ShootingCharacter : Character
    {
        protected ThrowerComponent ThrowerComponent;
        
        [SerializeField]
        protected Transform ballSocket;
        
        public BasketBall CurrentBall { get; protected set; }
        
        [SerializeField]
        private float resetDelayAfterThrow = 2.5f;
        
        protected ThrowOutcome LastThrowOutcome;

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out ThrowerComponent))
            {
                Debug.LogError("ShootingCharacter requires a ThrowerComponent to function properly.");
            }
            
            // TODO: handle ball spawning properly, this is just a temporary solution to get things working
            CurrentBall = FindObjectOfType<BasketBall>();
            CurrentBall.BallOwner = this;
            CurrentBall.DisablePhysics();
            CurrentBall.transform.position = ballSocket.position;
        }
        
        protected virtual void OnThrowReset(){}

        protected ThrowOutcome ThrowBall(BasketBall ball, Vector3 targetPosition, float powerAmount)
        {
            ball.EnablePhysics();
            LastThrowOutcome = ThrowerComponent.Throw(ball.Rigidbody, targetPosition, powerAmount);
            ball.OnBallThrown(LastThrowOutcome);
            StartCoroutine(ResetThrowAfterDelay());
            return LastThrowOutcome;
        }

        private IEnumerator ResetThrowAfterDelay()
        {
            yield return new WaitForSeconds(resetDelayAfterThrow);
            OnThrowReset();
        }
    }
}