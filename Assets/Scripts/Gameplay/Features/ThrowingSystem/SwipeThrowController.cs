using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class SwipeThrowController : MonoBehaviour
    {
        public static event Action OnThrowStarted;
        public static event Action OnSwipeThrowStarted; 
        public static event Action<float> OnThrowPowerUpdated;
        public static event Action<float> OnThrowCompleted;
        public static event Action OnThrowCanceled;

        private Vector2 _startTouchPosition;
        private Vector2 _lastTouchPosition;

        private float _lastPowerAmount;
        
        [SerializeField] private float minPowerAmountToThrow = 0.1f;
        
        [SerializeField] private float maxSwipeVerticalDistance = 1000f;
        
        [SerializeField] private float maxThrowDuration = 0.2f; 
        
        [SerializeField] private float deadzonePixels = 2f;
        
        [SerializeField] private float smoothTime = 0.06f; 

        private Coroutine _throwTimeoutCoroutine;
        
        private void OnEnable()
        {
            GameplayPlayerController.OnThrowInputBeganEvent += ThrowInputBegan;
            GameplayPlayerController.OnThrowInputEndedEvent += ThrowInputEnded;
        }
        
        private void OnDisable()
        {
            GameplayPlayerController.OnThrowInputBeganEvent -= ThrowInputBegan;
            GameplayPlayerController.OnThrowInputEndedEvent -= ThrowInputEnded;
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowInputMoved;
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowStartCheck;
        }

        private void ThrowInputBegan()
        {
            _startTouchPosition = GameplayPlayerController.GetPointerPosition();
            _lastPowerAmount = 0f;
            _lastTouchPosition = _startTouchPosition;
            _targetPower = 0f;
            _displayPower = 0f;
            _maxPowerReached = 0f;
            _powerVel = 0f;
            
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowInputMoved;
            GameplayPlayerController.OnThrowInputMovedEvent += ThrowInputMoved;
        }
        
        private void ThrowInputMoved()
        {
            // Check if the user has just started swiping
            if (_lastTouchPosition == _startTouchPosition)
            {
                SwipeThrowStart();
            }
            UpdatePowerAmount();
        }
        
        private void SwipeThrowStart()
        {
            // Start checking for throw start conditions as soon as the user starts swiping
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowStartCheck;
            GameplayPlayerController.OnThrowInputMovedEvent += ThrowStartCheck;
            OnSwipeThrowStarted?.Invoke();
        }
        
        private float _targetPower;     // 0..1 (raw/target)
        private float _displayPower;    // 0..1 (smoothed)
        private float _powerVel;        // SmoothDamp velocity
        private float _maxPowerReached; // monotonic clamp

        private void UpdatePowerAmount()
        {
            Vector2 p = GameplayPlayerController.GetPointerPosition();

            // vertical distance from start touch position (in pixels)
            float dy = p.y - _startTouchPosition.y;

            // deadzone for avoiding noise when the user is just tapping or making very small movements
            if (dy < deadzonePixels)
                dy = 0f;

            // normalize to 0..1 based on maxSwipeVerticalDistance, and clamp to 1
            float raw = Mathf.Clamp01(dy / maxSwipeVerticalDistance);

            // monotonic clamp to ensure the power only increases as the user swipes up, and doesn't drop if they move their finger down a bit while swiping
            _maxPowerReached = Mathf.Max(_maxPowerReached, raw);
            _targetPower = _maxPowerReached;

            // smoothing towards the target power for better feel (frame-rate independent)
            _displayPower = Mathf.SmoothDamp(_displayPower, _targetPower, ref _powerVel, smoothTime);

            OnThrowPowerUpdated?.Invoke(_displayPower);
            _lastTouchPosition = p;
            _lastPowerAmount = _displayPower;
        }

        private void ThrowStartCheck()
        {
            // Check if the power amount is sufficient to start the throw
            if (_lastPowerAmount >= minPowerAmountToThrow)
            {
                GameplayPlayerController.OnThrowInputMovedEvent -= ThrowStartCheck;
                ThrowStart();
            }
        }
        
        private void ThrowStart()
        {
            // Start a timeout coroutine to ensure the throw completes
            // even if the user doesn't lift their finger
            _throwTimeoutCoroutine = StartCoroutine(ThrowTimeout());
            OnThrowStarted?.Invoke();
        }

        private IEnumerator ThrowTimeout()
        {
            yield return new WaitForSeconds(maxThrowDuration); 
            ThrowCompleted(_lastPowerAmount);
        }
        
        private void ThrowCompleted(float powerAmount)
        {
            if (!enabled) return;
            OnThrowCompleted?.Invoke(powerAmount);
            ResetThrow();
        }
        
        private void ThrowInputEnded()
        {
            if (_lastPowerAmount >= minPowerAmountToThrow)
            {
                ThrowCompleted(_lastPowerAmount);
            }
            else if (_lastPowerAmount > 0f)
            {
                ThrowCanceled();
            }
        }

        private void ThrowCanceled()
        {
            ResetThrow();
            OnThrowCanceled?.Invoke();
        }

        private void ResetThrow()
        {
            _lastPowerAmount = 0f;
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowInputMoved;
            GameplayPlayerController.OnThrowInputMovedEvent -= ThrowStartCheck;
            if (_throwTimeoutCoroutine != null)
            {
                StopCoroutine(_throwTimeoutCoroutine);
                _throwTimeoutCoroutine = null;
            }
        }
    }
}
        