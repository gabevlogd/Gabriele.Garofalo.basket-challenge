using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayHUD : HUD
    {
        [SerializeField]
        private ThrowPowerView throwPowerView;
        
        [SerializeField]
        private PauseMenu pauseMenu;
        
        [SerializeField]
        private MatchResultView matchResultView;
        
        [SerializeField]
        private GameObject onMatchExecutionUI;
        
        private void Awake()
        {
            if (pauseMenu == null)
            {
                Debug.LogError("Pause Menu reference is missing in GameplayHUD.");
                return;
            }
            
            if (throwPowerView == null)
            {
                Debug.LogError("Swipe Throw Power View reference is missing in GameplayHUD.");
                return;
            }
            
            if (onMatchExecutionUI == null)
            {
                Debug.LogError("On Match Execution UI reference is missing in GameplayHUD.");
                return;
            }
            
            if (matchResultView == null)
            {
                Debug.LogError("Match Result View reference is missing in GameplayHUD.");
                return;
            }
            
            pauseMenu.gameObject.SetActive(false);
            matchResultView.gameObject.SetActive(false);
            onMatchExecutionUI.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            MatchManager.OnMatchEnd += HandleMatchEnd;
        }

        private void OnDisable()
        {
            MatchManager.OnMatchEnd -= HandleMatchEnd;
        }

        private void HandleMatchEnd()
        {
            HideHUD();
            matchResultView.gameObject.SetActive(true);
            matchResultView.UpdateMatchResult();
        }
        
        public void ShowHUD()
        {
            onMatchExecutionUI.gameObject.SetActive(true);
        }
        
        private void HideHUD()
        {
            onMatchExecutionUI.gameObject.SetActive(false);
        }

        
    }
}