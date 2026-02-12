using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.UI
{
    public class GameplayHUD : HUD
    {
        [SerializeField]
        private PauseMenu _pauseMenu;
        
        private void Awake()
        {
            if (_pauseMenu == null)
            {
                Debug.LogError("Pause Menu reference is missing in GameplayHUD.");
                return;
            }

            _pauseMenu.gameObject.SetActive(false);
        }
    }
}