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
            
            if (skinnedMeshComponent.SkinnedMesh.TryGetComponent(out CharacterAnimEvents animEvents))
            {
                animEvents.OnThrowAnimationEvent += () => SoundManager.Play("ThrowBall", false);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SwipeThrowController.OnThrowCompleted += StartThrowing;
            MatchManager.OnMatchStart += HandleMatchStart;
            MatchManager.OnMatchTimeExpired += DisableSwipeThrowAbility;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            SwipeThrowController.OnThrowCompleted -= StartThrowing;
            MatchManager.OnMatchStart -= HandleMatchStart;
            MatchManager.OnMatchTimeExpired -= DisableSwipeThrowAbility;
        }
        
        private void StartThrowing(float powerAmount)
        {
            StartThrowing(ThrowPositionsHandler.GetPerfectThrowPosition(), powerAmount);
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
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition());
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
            ThrowerComponent.UpdatePerfectPower(ThrowPositionsHandler.GetPerfectThrowPosition());
        }
        
        protected override void HandleMatchEnd()
        {
            base.HandleMatchEnd();
            
            // Set the camera to a predefined end game viewpoint with smooth transitions
            if (CoreUtility.TryGetPlayerCameraManager(out GameplayCameraManager cameraManager))
            {
                Transform endGameCameraViewpoint = EndGameTransformsHandler.Instance.CameraTrs;
                cameraManager.SetNewCameraViewpoint(endGameCameraViewpoint);
                cameraManager.useCameraViewpointPosition = true;
                cameraManager.enableCameraLag = true;
                cameraManager.cameraLagSpeed = 3f;
                cameraManager.useCameraViewpointRotation = true;
                cameraManager.enableCameraRotationLag = true;
                cameraManager.cameraRotationLagSpeed = 2f;
                cameraManager.PlayerCamera.transform.parent = null;
            }
            
            // Move the character to a predefined end game position
            Transform endGamePosition = EndGameTransformsHandler.Instance.PlayerTrs;
            transform.position = endGamePosition.position;
            transform.rotation = endGamePosition.rotation;

            // Save the player's score and update high score if necessary
            PlayerPrefs.SetInt("LastMatchScore", GetParticipantScore());
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (GetParticipantScore() > highScore)
            {
                PlayerPrefs.SetInt("HighScore", GetParticipantScore());
            }
        }
    }
}