using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent), typeof(ScoreReceiver))]
    public class ShootingCharacter : Character, IMatchParticipant
    {
        public BasketBall CurrentBall { get; protected set; }
        
        protected ThrowerComponent ThrowerComponent;
        
        protected ThrowOutcome LastThrowOutcome;
        
        [SerializeField]
        protected Transform ballSocket;
        
        [SerializeField]
        private float resetDelayAfterThrow = 2.5f;
        
        private Coroutine _resetThrowCoroutine;
        
        private readonly Guid _id = Guid.NewGuid();

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

        protected virtual void OnEnable()
        {
            MatchManager.OnMatchEnd += ClearResetThrowCoroutine;
            
            if (CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode))
            {
                gameplayGameMode.MatchResultManager.RegisterParticipant(this);
            }
        }

        protected virtual void OnDisable()
        {
            MatchManager.OnMatchEnd -= ClearResetThrowCoroutine;
            
            if (CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode))
            {
                gameplayGameMode.MatchResultManager.UnregisterParticipant(this);
            }
        }

        protected virtual void OnThrowReset()
        {
            // CurrentBall.DisablePhysics();
            CurrentBall.OnBallReset();
            CurrentBall.transform.position = ballSocket.position;
            CurrentBall.transform.parent = ballSocket.transform;
        }

        protected ThrowOutcome ThrowBall(Vector3 targetPosition, float powerAmount)
        {
            // CurrentBall.EnablePhysics();
            // CurrentBall.transform.parent = null;
            LastThrowOutcome = ThrowerComponent.Throw(CurrentBall.Rigidbody, targetPosition, powerAmount);
            CurrentBall.OnBallThrown(LastThrowOutcome);
            _resetThrowCoroutine = StartCoroutine(ResetThrowAfterDelay());
            return LastThrowOutcome;
        }

        private IEnumerator ResetThrowAfterDelay()
        {
            yield return new WaitForSeconds(resetDelayAfterThrow);
            OnThrowReset();
        }

        private void ClearResetThrowCoroutine()
        {
            if (_resetThrowCoroutine != null)
            {
                StopCoroutine(_resetThrowCoroutine);
                _resetThrowCoroutine = null;
            }
        }

        public Guid GetParticipantId()
        {
            return _id;
        }

        public string GetParticipantName()
        {
            return gameObject.name;
        }

        public int GetParticipantScore()
        {
            return TryGetComponent(out ScoreReceiver scoreReceiver) ? scoreReceiver.CurrentScore : 0;
        }
    }
}