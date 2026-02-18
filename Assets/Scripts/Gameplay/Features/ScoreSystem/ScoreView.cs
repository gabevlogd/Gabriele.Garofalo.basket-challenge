using BasketChallenge.Core;
using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI personalScoreText;
        
        [SerializeField]
        private TextMeshProUGUI opponentScoreText;

        private PlayerCharacter _playerCharacterRef;

        private void Start()
        {
            if (!CoreUtility.TryGetPlayerControlledObject(out _playerCharacterRef))
            {
                Debug.LogError("ScoreView requires a reference to the player character to function properly.");
            }
            SetScoreText(personalScoreText, 0);
            SetScoreText(opponentScoreText, 0);
        }

        private void OnEnable()
        {
            ScoreReceiver.OnScoreUpdated += UpdateScoreDisplay;
        }

        private void OnDisable()
        {
            ScoreReceiver.OnScoreUpdated -= UpdateScoreDisplay;
        }
        
        private void UpdateScoreDisplay(ShootingCharacter scoreOwner, int score)
        {
            SetScoreText(scoreOwner == _playerCharacterRef ? personalScoreText : opponentScoreText, score);
        }
        
        private void SetScoreText(TextMeshProUGUI scoreText, int score)
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
            else
            {
                Debug.LogWarning("ScoreView: Attempted to set score text on a null TextMeshProUGUI reference.");
            }
        }
    }
}