using System;
using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MatchTimerView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI matchTimerText;

        private void Awake()
        {
            if (matchTimerText == null)
            {
                Debug.LogError("MatchTimer: matchTimerText reference is not set.");
            }
        }

        private void OnEnable()
        {
            MatchManager.OnMatchDurationUpdate += UpdateMatchTimerText;
        }

        private void OnDisable()
        {
            MatchManager.OnMatchDurationUpdate -= UpdateMatchTimerText;
        }
        
        private void UpdateMatchTimerText(float remainingTime)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
            matchTimerText.text = $"{timeSpan.Minutes:D1}:{timeSpan.Seconds:D2}";
        }
    }
}
