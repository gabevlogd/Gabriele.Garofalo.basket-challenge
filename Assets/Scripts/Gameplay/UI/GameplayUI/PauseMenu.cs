using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private Button _exitButton;
        
        private void Awake()
        {
            if (_exitButton == null)
            {
                Debug.LogError("Exit Button reference is missing in GameplayHUD.");
                return;
            }
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
