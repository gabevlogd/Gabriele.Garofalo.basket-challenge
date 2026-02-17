using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayCameraManager : PlayerCameraManager
    {
        private GameObject _ballViewpoint;
        
        private float _defaultCameraLagSpeed;
        
        private bool _stopFollowBall;
        private bool _followBall;

        private Vector3 _basketPosition;

        private void Start()
        {
            _ballViewpoint = new GameObject("BallViewpoint");
            _basketPosition = BasketBackboard.GetPerfectShotPosition() + Vector3.up * 3f;
            _defaultCameraLagSpeed = cameraLagSpeed;
        }

        private void OnEnable()
        {
            SwipeThrowController.OnThrowCompleted += CameraStartFollowBall;
        }

        private void OnDisable()
        {
            SwipeThrowController.OnThrowCompleted -= CameraStartFollowBall;
        }

        private void Update()
        {
            if (_followBall)
            {
                _ballViewpoint.transform.position = Vector3.Lerp(_ballViewpoint.transform.position, _basketPosition, Time.deltaTime * 3f);
            }
            
            if (_stopFollowBall)
            {
                cameraLagSpeed = Mathf.Lerp(cameraLagSpeed, 0, Time.deltaTime * 2f);
                if (cameraLagSpeed <= 0.08f)
                {
                    cameraLagSpeed = _defaultCameraLagSpeed;
                    _stopFollowBall = false;
                    SetNewCameraViewpoint(PlayerCamera.transform);
                }
            }
        }

        private void CameraStartFollowBall(float powerAmount)
        {
            if (!CoreUtility.TryGetPlayerControlledObject(out PlayerCharacter playerCharacter)) return;
            if (!playerCharacter.CurrentBall) return;
            
            _followBall = true;
            _ballViewpoint.transform.position = playerCharacter.CurrentBall.transform.position;
            
            SetNewCameraViewpoint(_ballViewpoint.transform);
            
            playerCharacter.CurrentBall.OnBallCollisionEnter -= CameraStopFollowBall;
            playerCharacter.CurrentBall.OnBallCollisionEnter += CameraStopFollowBall;
        }

        private void CameraStopFollowBall(Collision collision)
        {
            _followBall = false;
            _stopFollowBall = true;
        }
    }
}