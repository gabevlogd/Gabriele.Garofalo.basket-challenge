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
        
        protected float CurrentThrowPower;
        
        protected Vector3 CurrentThrowTarget;
        
        [SerializeField]
        protected Transform ballSocket;
        
        [SerializeField]
        private float resetDelayAfterThrow = 2.5f;
        
        private Coroutine _resetThrowCoroutine;
        
        private readonly Guid _id = Guid.NewGuid();

        private static readonly int IsThrowing = Animator.StringToHash("IsThrowing");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Lose = Animator.StringToHash("Lose");
        
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

            if (!skinnedMeshComponent.SkinnedMesh.TryGetComponent(out CharacterAnimEvents animEvents)) return;
            animEvents.OnThrowAnimationEvent += ThrowBall;
            animEvents.OnThrowCompleteAnimationEvent += StopThrowAnimation;
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
            MatchManager.OnMatchEnd += HandleMatchEnd;
        }

        protected virtual void OnDisable()
        {
            MatchManager.OnMatchTimeExpired -= ClearResetThrowCoroutine;
            MatchManager.OnMatchEnd -= HandleMatchEnd;
        }

        protected virtual void OnThrowReset()
        {
            CurrentBall.OnBallReset();
            CurrentBall.transform.position = ballSocket.position;
            CurrentBall.transform.parent = ballSocket.transform;
            CurrentThrowPower = 0f;
            CurrentThrowTarget = Vector3.zero;
        }

        /// <summary>
        /// This method caches the target position and power for the throw, and triggers the throw animation.
        /// The actual throwing of the ball will be handled by the ThrowBall method, which is called by an animation event during the throw animation.
        /// This separation allows for better synchronization between the animation and the gameplay mechanics of throwing the ball.
        /// </summary>
        protected void StartThrowing(Vector3 targetPosition, float powerAmount)
        {
            CurrentThrowPower = powerAmount;
            CurrentThrowTarget = targetPosition;
            skinnedMeshComponent.Animator.SetBool(IsThrowing, true);
        }
        
        private void StopThrowAnimation()
        {
            skinnedMeshComponent.Animator.SetBool(IsThrowing, false);
        }

        private void ThrowBall()
        {
            ThrowBall(CurrentThrowTarget, CurrentThrowPower);
        }

        private void ThrowBall(Vector3 targetPosition, float powerAmount)
        {
            if (CurrentBall == null)
            {
                Debug.LogError("No ball available to throw.");
                return;
            }
            LastThrowOutcome = ThrowerComponent.Throw(CurrentBall.Rigidbody, targetPosition, powerAmount);
            CurrentBall.OnBallThrown(LastThrowOutcome);
            _resetThrowCoroutine = StartCoroutine(ResetThrowAfterDelay());
            Debug.Log(name + " performed a throw with outcome: " + LastThrowOutcome);
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

        protected virtual void HandleMatchEnd()
        {
            ClearResetThrowCoroutine();
            
            // Set the appropriate win/lose animation based on the match result
            if (!CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode)) return;
            MatchResult result = gameplayGameMode.MatchResultManager.GetMatchResult();
            skinnedMeshComponent.Animator.SetBool(result.Winners.Contains(this) ? Win : Lose, true);
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