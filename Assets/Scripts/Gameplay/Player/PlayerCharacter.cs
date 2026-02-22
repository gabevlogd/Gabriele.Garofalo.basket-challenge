using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(SwipeThrowController))]
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

        protected override void Start()
        {
            base.Start();
            DisableSwipeThrowAbility();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SwipeThrowController.OnThrowCompleted += ThrowBall;
            MatchManager.OnMatchStart += HandleMatchStart;
            MatchManager.OnMatchTimeExpired += DisableSwipeThrowAbility;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            SwipeThrowController.OnThrowCompleted -= ThrowBall;
            MatchManager.OnMatchStart -= HandleMatchStart;
            MatchManager.OnMatchTimeExpired -= DisableSwipeThrowAbility;
        }
        
        private void ThrowBall(float powerAmount)
        {
            ThrowBall(ThrowPositionsHandler.GetPerfectThrowPosition(), powerAmount);
            SoundManager.Play("ThrowBall", false);
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
        
        private void HandleMatchStart()
        {
            EnableSwipeThrowAbility();
            if (CoreUtility.TryGetHUD(out GameplayHUD gameplayHUD))
            {
                gameplayHUD.ShowHUD();
            }
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition(), ballSocket.position);
        }
    }
}