using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayCameraManager : PlayerCameraManager
    {
        private GameObject _ballViewpoint;
        
        private float _defaultCameraLagSpeed;
        [SerializeField] private float followBallSpeed;
        [SerializeField] private float blendOutFollowSpeed;
        
        private bool _blendOutFollowBall;
        private bool _followBall;

        private Vector3 _basketPosition;

        private void Start()
        {
            _ballViewpoint = new GameObject("BallViewpoint");
            _basketPosition = ThrowPositionsHandler.GetPerfectThrowPosition() + Vector3.up * 3f;
            _defaultCameraLagSpeed = cameraLagSpeed;
        }

        private void OnEnable()
        {
            SwipeThrowController.OnThrowCompleted += CameraStartFollowBall;
            PlayerCharacter.OnThrowResetEvent += StopCameraFollowBall;
        }

        private void OnDisable()
        {
            SwipeThrowController.OnThrowCompleted -= CameraStartFollowBall;
            PlayerCharacter.OnThrowResetEvent -= StopCameraFollowBall;
        }

        private void Update()
        {
            if (_followBall)
            {
                _ballViewpoint.transform.position = Vector3.Lerp(_ballViewpoint.transform.position, _basketPosition, Time.deltaTime * followBallSpeed);
            }
            
            if (_blendOutFollowBall)
            {
                cameraLagSpeed = Mathf.Lerp(cameraLagSpeed, 0, Time.deltaTime * blendOutFollowSpeed);
            }
        }

        public override void SetConfig(PlayerCameraManagerClass config)
        {
            base.SetConfig(config);
            if (config is GameplayCameraManagerClass gameplayConfig)
            {
                followBallSpeed = gameplayConfig.followBallSpeed;
                blendOutFollowSpeed = gameplayConfig.blendOutFollowSpeed;
            }
            
        }

        private void CameraStartFollowBall(float powerAmount)
        {
            if (!CoreUtility.TryGetPlayerControlledObject(out PlayerCharacter playerCharacter)) return;
            if (!playerCharacter.CurrentBall) return;
            
            _followBall = true;
            _ballViewpoint.transform.position = playerCharacter.CurrentBall.transform.position;
            
            SetNewCameraViewpoint(_ballViewpoint.transform);
            
            playerCharacter.CurrentBall.OnBallCollisionEnter -= BlendOutCameraFollowBall;
            playerCharacter.CurrentBall.OnBallCollisionEnter += BlendOutCameraFollowBall;
        }

        private void BlendOutCameraFollowBall(Collision collision)
        {
            _followBall = false;
            _blendOutFollowBall = true;
        }

        private void StopCameraFollowBall()
        {
            _blendOutFollowBall = false;
            cameraLagSpeed = _defaultCameraLagSpeed;
        }
    }
}