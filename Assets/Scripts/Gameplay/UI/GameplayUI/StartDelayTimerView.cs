using System;
using System.Collections;
using System.Collections.Generic;
using BasketChallenge.Core;
using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class StartDelayTimerView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timerText;

        private void Awake()
        {
            if (timerText == null)
            {
                Debug.LogError("StartDelayTimerView: timerText reference is not set.");
            }
        }

        private void OnEnable()
        {
            MatchManager.OnMatchStartDelayUpdate += UpdateStartDelayTimerText;
            MatchManager.OnMatchStart += HideTimer;
        }

        private void OnDisable()
        {
            MatchManager.OnMatchStartDelayUpdate -= UpdateStartDelayTimerText;
            MatchManager.OnMatchStart -= HideTimer;
        }
        
        private void HideTimer()
        {
            if (!timerText) return;
            gameObject.SetActive(false);
        }
        
        private int _lastDisplayedSecond = -1;
        private void UpdateStartDelayTimerText(float remainingDelay)
        {
            if (!timerText) return;
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingDelay);
            int currentSecond = timeSpan.Seconds + 1; // Add 1 to show the correct countdown number
            timerText.text = $"{currentSecond:D1}";
            if (currentSecond != _lastDisplayedSecond)
            {
                SoundManager.Play("Countdown", false);
                _lastDisplayedSecond = currentSecond;
            }
        }
    }
}
