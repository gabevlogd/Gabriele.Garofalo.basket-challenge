using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BasketChallenge.UI
{
    public class MenuHUD : HUD
    {
        [SerializeField] 
        protected Button _playButton;
        
        private void Awake()
        {
            if (_playButton == null)
            {
                Debug.LogError("Play Button reference is missing in MenuHUD.");
                return;
            }
            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
