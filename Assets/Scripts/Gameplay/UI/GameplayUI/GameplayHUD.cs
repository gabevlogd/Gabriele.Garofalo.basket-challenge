using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayHUD : HUD
    {
        [SerializeField]
        private SwipeThrowPowerView _swipeThrowPowerView;
        
        [SerializeField]
        private PauseMenu _pauseMenu;
        
        private void Awake()
        {
            if (_pauseMenu == null)
            {
                Debug.LogError("Pause Menu reference is missing in GameplayHUD.");
                return;
            }
            
            if (_swipeThrowPowerView == null)
            {
                Debug.LogError("Swipe Throw Power View reference is missing in GameplayHUD.");
                return;
            }

            _pauseMenu.gameObject.SetActive(false);
        }
    }
}