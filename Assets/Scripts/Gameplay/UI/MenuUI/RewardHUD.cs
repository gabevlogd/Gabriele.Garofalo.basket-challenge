using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class RewardHUD : HUD
    {
        [SerializeField] 
        protected Button _playAgainButton;
        
        [SerializeField]
        protected Button _backToMenuButton;
        
        private void Awake()
        {
            if (_playAgainButton == null)
            {
                Debug.LogError("Play Again Button reference is missing in RewardHUD.");
                return;
            }
            
            if (_backToMenuButton == null)
            {
                Debug.LogError("Back To Menu Button reference is missing in RewardHUD.");
                return;
            }
            
            _playAgainButton.onClick.RemoveAllListeners();
            _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
            
            _backToMenuButton.onClick.RemoveAllListeners();
            _backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }

        private void OnBackToMenuClicked()
        {
            SceneManager.LoadScene(0);
        }

        private void OnPlayAgainClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
