using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent), typeof(ScoreReceiver))]
    public class ShootingCharacter : Character, IMatchParticipant
    {
        [SerializeField]
        private BasketBall ballPrefab; 
        
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
            
            if (ballPrefab)
            {
                CurrentBall = Instantiate(ballPrefab, ballSocket.transform, true);
                CurrentBall.BallOwner = this;
                CurrentBall.OnBallReset();
                CurrentBall.transform.position = ballSocket.position;
            }
            else 
            {
                Debug.LogError("Ball Prefab is not assigned on ShootingCharacter.");
            }
        }

        protected virtual void Start()
        {
            if (CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode))
            {
                gameplayGameMode.MatchResultManager.RegisterParticipant(this);
            }
        }

        protected virtual void OnEnable()
        {
            MatchManager.OnMatchTimeExpired += ClearResetThrowCoroutine;
        }

        protected virtual void OnDisable()
        {
            MatchManager.OnMatchTimeExpired -= ClearResetThrowCoroutine;
        }

        protected virtual void OnThrowReset()
        {
            CurrentBall.OnBallReset();
            CurrentBall.transform.position = ballSocket.position;
            CurrentBall.transform.parent = ballSocket.transform;
        }

        protected ThrowOutcome ThrowBall(Vector3 targetPosition, float powerAmount)
        {
            if (CurrentBall == null)
            {
                Debug.LogError("No ball available to throw.");
                return ThrowOutcome.None;
            }
            LastThrowOutcome = ThrowerComponent.Throw(CurrentBall.Rigidbody, targetPosition, powerAmount);
            CurrentBall.OnBallThrown(LastThrowOutcome);
            _resetThrowCoroutine = StartCoroutine(ResetThrowAfterDelay());
            Debug.Log(name + " performed a throw with outcome: " + LastThrowOutcome);
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