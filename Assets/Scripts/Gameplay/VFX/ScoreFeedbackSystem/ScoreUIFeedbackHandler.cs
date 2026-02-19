using System;
using System.Collections;
using BasketChallenge.Core;
using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreUIFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;
        
        private PlayerCharacter _playerCharacter;
        private Camera _camera;
        private Coroutine _animationCoroutine;

        private void Awake()
        {
            if (scoreText == null)
            {
                Debug.LogError("ScoreUIFeedbackHandler requires a reference to a TextMeshProUGUI component for displaying the score.");
            }
            
            scoreText.enabled = false;
        }

        private void Start()
        {
            if (!CoreUtility.TryGetPlayerControlledObject(out _playerCharacter))
            {
                Debug.LogError("ScoreUIFeedbackHandler requires a PlayerCharacter in the scene to function properly.");
            }
            
            if (CoreUtility.TryGetPlayerCameraManager(out PlayerCameraManager cameraManager))
            {
                _camera = cameraManager.PlayerCamera;
            }
        }

        private void OnEnable()
        {
            ScoreReceiver.OnScoreAmountCalculated += HandleScoreAmountCalculated;
        }
        
        private void OnDisable()
        {
            ScoreReceiver.OnScoreAmountCalculated -= HandleScoreAmountCalculated;
        }

        private void LateUpdate()
        {
            if (scoreText != null && _camera != null)
            {
                scoreText.transform.rotation = _camera.transform.rotation;
            }
        }

        private void HandleScoreAmountCalculated(ShootingCharacter scoreOwner, int scoreAmount)
        {
            if (scoreText == null || scoreOwner != _playerCharacter)
            {
                return;
            }

            BasketBall currentBall = scoreOwner.CurrentBall;
            Color textColor = Color.white;
            string text = $"+{scoreAmount}";
            if (currentBall.lastBackboardBonus != null)
            {
                textColor = currentBall.lastBackboardBonus.color;
                text = "Bonus! " + text;
            }
            else if (currentBall.OnFire)
            {
                textColor = Color.red; 
                text = "On fire! " + text;
            }
            else if (currentBall.LastThrowOutcome == ThrowOutcome.Perfect)
            {
                textColor = Color.green;
                text = "Perfect! " + text;
            }

            scoreText.text = text;
            scoreText.color = textColor;
            StartAnimation();
        }

        private void StartAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(PerformScoreFeedbackAnimation());
        }
        
        private IEnumerator PerformScoreFeedbackAnimation()
        {
            Vector3 originalPosition = scoreText.transform.position;
            Vector3 targetPosition = originalPosition + Vector3.up * 0.5f; 
            float animationDuration = 1f;
            float elapsedTime = 0f;
            scoreText.enabled = true;

            while (elapsedTime < animationDuration)
            {
                scoreText.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            scoreText.transform.position = originalPosition;
            scoreText.enabled = false;
        }
    }
}
