using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    public class AICharacter : ShootingCharacter
    {
        public AIBehaviourData behaviourData;
        
        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _stateMachine))
            {
                Debug.LogError("AICharacter requires a StateMachine to function properly.");
            }
            
            if (behaviourData == null)
            {
                Debug.LogError("AIBehaviourData is not assigned on AICharacter.");
            }
        }

        protected override void Start()
        {
            base.Start();
            _stateMachine.StartStateMachine(); 
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MatchManager.OnMatchTimeExpired += StopPlaying;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MatchManager.OnMatchTimeExpired -= StopPlaying;
        }

        protected override void OnThrowReset()
        {
            base.OnThrowReset();
            if (LastThrowOutcome is ThrowOutcome.BackboardMake or ThrowOutcome.FarRim or ThrowOutcome.NearRim or ThrowOutcome.Perfect)
            {
                Transform newShootingPosition = ShootingPositionsHandler.GetRandomShootingPosition();
                transform.position = newShootingPosition.position;
                transform.rotation = newShootingPosition.rotation;
            }
            _stateMachine.ChangeState<ThrowBall>();
        }

        public void ThrowBall(float powerAmount)
        {
            ThrowBall(ThrowPositionsHandler.GetPerfectThrowPosition(), powerAmount);
        }

        public void UpdatePerfectPower()
        {
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition(), ballSocket.position);
        }

        private void StopPlaying()
        {
            _stateMachine.ChangeState<Wait>();
        }
    }
}
