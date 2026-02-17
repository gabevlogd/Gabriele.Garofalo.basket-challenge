using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent), typeof(ScoreReceiver))]
    public class PlayerCharacter : ShootingCharacter
    {
        [SerializeField]
        private Transform ballSocket;
        
        [SerializeField]
        private Transform cameraHolder;

        public BasketBall CurrentBall { get; private set; }

        public static event Action OnThrowResetEvent;
        
        protected override void Awake()
        {
            base.Awake();
            
            // TODO: handle ball spawning properly, this is just a temporary solution to get things working
            CurrentBall = FindObjectOfType<BasketBall>();
            CurrentBall.BallOwner = this;
            CurrentBall.DisablePhysics();
            CurrentBall.transform.position = ballSocket.position;
        }

        private void Start()
        {
            ThrowerComponent.UpdatePerfectPower(BasketBackboard.GetPerfectShotPosition(), ballSocket.position);
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
            ThrowBall(CurrentBall, BasketBackboard.GetPerfectShotPosition(), powerAmount);
        }

        protected override void OnThrowReset()
        {
            if (LastThrowOutcome is ThrowOutcome.Perfect or ThrowOutcome.BackboardMake or ThrowOutcome.NearRim
                or ThrowOutcome.FarRim)
            {
                Transform newShootingPosition = ShootingPositionsHandler.GetRandomShootingPosition();
                transform.position = newShootingPosition.position;
                transform.rotation = newShootingPosition.rotation;
            }
            ResetCameraPosition();
            CurrentBall.DisablePhysics();
            CurrentBall.transform.position = ballSocket.position;
            ThrowerComponent.UpdatePerfectPower(BasketBackboard.GetPerfectShotPosition(), ballSocket.position);
            OnThrowResetEvent?.Invoke();
        }
        
        private void ResetCameraPosition()
        {
            if (!CoreUtility.TryGetPlayerController(out PlayerController playerController)) return;
            playerController.SetNewCameraViewpoint(cameraHolder, false);
        }
    }
}