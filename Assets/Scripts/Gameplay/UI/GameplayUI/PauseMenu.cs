using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        public static event Action OnPauseMenuOpened;
        public static event Action OnPauseMenuClosed;
        
        [SerializeField]
        private Button _exitButton;
        
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            OnPauseMenuOpened?.Invoke();
        }
        
        private void OnDisable()
        {
            OnPauseMenuClosed?.Invoke();
        }

        private void Init()
        {
            if (_exitButton == null)
            {
                Debug.LogError("Exit Button reference is missing in PauseMenu Prefab.");
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
