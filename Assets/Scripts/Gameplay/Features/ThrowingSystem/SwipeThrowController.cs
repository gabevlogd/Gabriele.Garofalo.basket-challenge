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
        
        [SerializeField]
        private float minPowerAmountToThrow = 0.1f;
        
        [SerializeField]
        private float maxSwipeVerticalDistance = 1000f;
        
        [SerializeField]
        private float maxThrowDuration = 0.2f; 

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
        
        private void UpdatePowerAmount()
        {
            Vector2 currentTouchPosition = GameplayPlayerController.GetPointerPosition();
            
            // Prevent power amount from decreasing if the user swipes down
            if (_lastTouchPosition.y > currentTouchPosition.y)
            { 
                return; 
            }
            
            float swipeVerticalDistance = Mathf.Abs(currentTouchPosition.y - _startTouchPosition.y);
            float currentPowerAmount = Mathf.Clamp01(swipeVerticalDistance / maxSwipeVerticalDistance);
            OnThrowPowerUpdated?.Invoke(currentPowerAmount);
            _lastTouchPosition = currentTouchPosition;
            _lastPowerAmount = currentPowerAmount;
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
        