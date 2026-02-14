using System;
using System.Collections;
using System.Collections.Generic;
using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class SwipeThrowPowerView : MonoBehaviour
    {
        [SerializeField] 
        private Image _sliderFillerImage;

        [SerializeField] 
        private Image _perfectThrowImage;

        [SerializeField] 
        private Image _backboardThrowImage;

        [SerializeField]
        private float _maxSwipeVerticalDistance = 1000;

        private Vector2 _startTouchPosition;
        private Vector2 _lastTouchPosition;
        private float _lastFillAmount;

        private void Awake()
        {
            if (_sliderFillerImage == null)
                Debug.LogError("Slider Filler Image is not assigned in the inspector.");

            if (_perfectThrowImage == null)
                Debug.LogError("Perfect Throw Image is not assigned in the inspector.");

            if (_backboardThrowImage == null)
                Debug.LogError("Backboard Throw Image is not assigned in the inspector.");

            UpdateSliderFill(0f);

            TouchManager.OnTouchBeganEvent += OnTouchBegan;
            TouchManager.OnTouchMovedEvent += OnTouchMoved;
        }

        private void OnTouchBegan()
        {
            _startTouchPosition = Input.GetTouch(0).position;
            _lastFillAmount = 0f;
            _lastTouchPosition = Vector2.zero;
            UpdateSliderFill(0f);
        }

        private void OnTouchMoved()
        {
            float newFillAmount = CalculateFillAmount();
            UpdateSliderFill(newFillAmount);
            _lastFillAmount = newFillAmount;
        }

        private float CalculateFillAmount()
        {
            Vector2 currentTouchPosition = Input.GetTouch(0).position;
            
            if (_lastTouchPosition.y > currentTouchPosition.y)
            {
                // Prevent fill amount from decreasing if the user swipes down
                return _lastFillAmount; 
            }
            
            _lastTouchPosition = currentTouchPosition;
            float swipeVerticalDistance = Mathf.Abs(currentTouchPosition.y - _startTouchPosition.y);
            return Mathf.Clamp01(swipeVerticalDistance / _maxSwipeVerticalDistance);
        }

        public void UpdateSliderFill(float fillAmount)
        {
            if (_sliderFillerImage == null) return;
            _sliderFillerImage.fillAmount = fillAmount;
        }
    }
}