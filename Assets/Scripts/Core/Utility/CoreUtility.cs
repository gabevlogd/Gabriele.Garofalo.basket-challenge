
namespace BasketChallenge.Core
{
    public static class CoreUtility 
    {
        public static bool TryGetGameMode<T>(out T gameMode) where T : GameModeBase
        {
            gameMode = null;
            return GameModeBase.Instance && GameModeBase.Instance.TryGetComponent(out gameMode);
        }
        
        public static bool TryGetPlayerController<T>(out T playerController) where T : PlayerController
        {
            playerController = null;
            if (GameModeBase.Instance == null)
            {
                return false;
            }

            if (GameModeBase.Instance.PlayerController == null)
            {
                return false;
            }

            return GameModeBase.Instance.PlayerController.TryGetComponent(out playerController);
        }

        public static bool TryGetPlayerControlledObject<T>(out T playerControlledObject) where T : ControllableBase
        {
            playerControlledObject = null;
            if (GameModeBase.Instance == null)
            {
                return false;
            }

            if (GameModeBase.Instance.PlayerControllableObject == null)
            {
                return false;
            }

            return GameModeBase.Instance.PlayerControllableObject.TryGetComponent(out playerControlledObject);
        }
        
        public static bool TryGetHUD<T>(out T hud) where T : HUD
        {
            hud = null;
            if (GameModeBase.Instance == null)
            {
                return false;
            }

            if (GameModeBase.Instance.HUD == null)
            {
                return false;
            }

            return GameModeBase.Instance.HUD.TryGetComponent(out hud);
        }
    }
}