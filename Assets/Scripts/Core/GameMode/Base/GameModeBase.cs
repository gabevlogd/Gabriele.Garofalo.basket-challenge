using UnityEngine;

namespace BasketChallenge.Core
{
    /// <summary>
    /// The GameModeBase class is responsible for managing the overall game state,
    /// including player controllers, HUD, and player-controlled game objects.
    /// It serves as a base class for specific game modes that can be implemented by inheriting from it.
    /// The GameModeBase class initializes the player controller, HUD, and spawns the player-controlled game object at the start of the game.
    /// It also handles the possession of the player-controlled game object by the player controller.
    /// </summary>
    public class GameModeBase : Singleton<GameModeBase>
    {
        public PlayerController PlayerController { get; private set; }
        
        public HUD HUD { get; private set; }
        
        public ControllableBase PlayerControllableObject { get; private set; }
        
        public virtual bool Init(GameModeClass gameModeClass)
        {
            if (!CreatePlayerController(gameModeClass.playerControllerClass)) return false;
            SpawnPlayerControlledGameObject(gameModeClass.playerControllableClass);
            PlayerController.Possess(PlayerControllableObject);
            if (!CreateHUD(gameModeClass.hudClass)) return false;
            return true;
        }
        
        private bool CreatePlayerController(PlayerControllerClass controllerClass)
        {
            if (controllerClass == null || !controllerClass.IsValid())
            {
                Debug.LogError("PlayerControllerClass is not valid.");
                return false;
            }

            PlayerController = controllerClass.CreateController() as PlayerController;
            return true;
        }

        private bool CreateHUD(HUDClass hudClass)
        {
            if (hudClass == null || !hudClass.IsValid())
            {
                Debug.LogError("HUD prefab is not assigned.");
                return false;
            }
            HUD = Instantiate(hudClass.hudPrefab);
            return true;
        }

        private void SpawnPlayerControlledGameObject(ControllableClass playerControllableClass)
        {
            if (playerControllableClass == null || !playerControllableClass.IsValid())
            {
                Debug.LogWarning("PlayerControllableClass is not valid.");
                return;
            }
            
            // Find a PlayerStart in the scene to determine spawn position and rotation
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;
            PlayerStart playerStart = FindObjectOfType<PlayerStart>();
            if (playerStart != null)
            {
                spawnPosition = playerStart.transform.position;
                spawnRotation = playerStart.transform.rotation;
            }
            
            if (playerControllableClass == null)
            {
                return;
            }
            PlayerControllableObject = Instantiate(playerControllableClass.controllablePrefab, spawnPosition, spawnRotation);
        }
    }
}
