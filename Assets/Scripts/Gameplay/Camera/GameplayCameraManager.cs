using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayCameraManager : PlayerCameraManager
    {
        private void OnEnable()
        {
            SwipeThrowController.OnThrowCompleted += CameraStartFollowBall;
        }

        private void OnDisable()
        {
            SwipeThrowController.OnThrowCompleted -= CameraStartFollowBall;
        }

        private void CameraStartFollowBall(float powerAmount)
        {
            if (!CoreUtility.TryGetPlayerControlledObject(out PlayerCharacter playerCharacter)) return;
            if (!playerCharacter.CurrentBall) return;
                
            SetNewCameraViewpoint(playerCharacter.CurrentBall.transform);
            
            playerCharacter.CurrentBall.OnBallCollisionEnter -= CameraStopFollowBall;
            playerCharacter.CurrentBall.OnBallCollisionEnter += CameraStopFollowBall;
        }

        private void CameraStopFollowBall(Collision collision)
        {
            SetNewCameraViewpoint(PlayerCamera.transform);
        }
    }
}