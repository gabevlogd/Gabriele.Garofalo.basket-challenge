using System;
using UnityEngine;
using BasketChallenge.Core;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(ThrowerComponent))]
    public class PlayerCharacter : Character
    {
        // Temporary target position for testing purposes
        public Transform tempTargetPosition;
        
        [SerializeField]
        private Transform ballSocket;
        
        private BasketBall _currentBall;
        
        
        private ThrowerComponent _throwerComponent;
        public ThrowerComponent ThrowerComponent => _throwerComponent;
        
        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _throwerComponent))
            {
                Debug.LogError("PlayerCharacter requires a ThrowerComponent to function properly.");
            }
            _currentBall = FindObjectOfType<BasketBall>();
            tempTargetPosition = GameObject.Find("PerfectThrow").transform; // Ensure this GameObject exists in the scene
        }

        private void Start()
        {
            Temp();
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
            if (_currentBall == null)
            {
                Debug.LogWarning("No ball to throw.");
                return;
            }
            _currentBall.transform.position = ballSocket.position;
            _throwerComponent.Throw(_currentBall.Rigidbody, tempTargetPosition.position, powerAmount);
        }

        [ContextMenu("Temp")]
        private void Temp()
        {
            _throwerComponent.UpdatePerfectPower(tempTargetPosition.position, ballSocket.position);
        }
        
        
    }
}