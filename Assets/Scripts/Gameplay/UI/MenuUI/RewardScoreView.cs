using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class RewardScoreView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI lastMatchScoreText;
        
        [SerializeField]
        private TextMeshProUGUI bestScoreText;

        private void Awake()
        {
            if (lastMatchScoreText == null)
            {
                Debug.LogError("Last Match Score Text is not assigned in the inspector.");
            }
            
            if (bestScoreText == null)
            {
                Debug.LogError("Best Score Text is not assigned in the inspector.");
            }
        }

        private void OnEnable()
        {
            if (lastMatchScoreText != null)
            {
                int lastMatchScore = PlayerPrefs.GetInt("LastMatchScore", 0);
                lastMatchScoreText.text = $"Last Score: {lastMatchScore}";
            }
            
            if (bestScoreText != null)
            {
                int highScore = PlayerPrefs.GetInt("HighScore", 0);
                bestScoreText.text = $"Best Score: {highScore}";
            }
        }
    }
}
