using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ScoreReceiver), typeof(SwipeThrowController))]
    public class PlayerCharacter : ShootingCharacter
    {
        [SerializeField]
        private Transform cameraHolder;
        
        private SwipeThrowController _swipeThrowController;
        
        public static event Action OnThrowResetEvent;
        
        protected override void Awake()
        {
            base.Awake();
            
            if (!TryGetComponent(out _swipeThrowController))
            {
                Debug.LogError("PlayerCharacter requires a SwipeThrowController component to function properly.");
            }
        }

        private void Start()
        {
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition(), ballSocket.position);
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
            if (!CurrentBall)
            {
                Debug.LogWarning("No ball to throw.");
                return;
            }
            ThrowBall(ThrowPositionsHandler.GetPerfectThrowPosition(), powerAmount);
            DisableSwipeThrowAbility();
        }

        protected override void OnThrowReset()
        {
            base.OnThrowReset();
            
            if (LastThrowOutcome is ThrowOutcome.BackboardMake or ThrowOutcome.FarRim or ThrowOutcome.NearRim or ThrowOutcome.Perfect)
            {
                SetNewShootingPosition();
            }
            
            ResetCameraPosition();
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition(), ballSocket.position);
            EnableSwipeThrowAbility();
            OnThrowResetEvent?.Invoke();
        }

        private void SetNewShootingPosition()
        {
            Transform newShootingPosition = ShootingPositionsHandler.GetRandomShootingPosition();
            transform.position = newShootingPosition.position;
            transform.rotation = newShootingPosition.rotation;
        }
        
        private void ResetCameraPosition()
        {
            if (!CoreUtility.TryGetPlayerController(out PlayerController playerController)) return;
            playerController.SetNewCameraViewpoint(cameraHolder, false);
        }
        
        private void EnableSwipeThrowAbility()
        {
            _swipeThrowController.enabled = true;
        }

        private void DisableSwipeThrowAbility()
        {
            _swipeThrowController.enabled = false;
        }
    }
}