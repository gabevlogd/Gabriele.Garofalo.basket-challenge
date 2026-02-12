using UnityEngine;

namespace BasketChallenge.Core
{
    public class GameModeBase : Singleton<GameModeBase>
    {
        public PlayerController PlayerController { get; private set; }
        public PlayerController playerControllerPrefab;
        
        public HUD HUD { get; private set; }
        public HUD hudPrefab;
        
        public ControllableGameObject PlayerControlledGameObject { get; private set; }
        public ControllableGameObject playerControlledGameObjectPrefab;
        
        public virtual bool Init()
        {
            if (!CreatePlayerController()) return false;
            if (!CreateHUD()) return false;
            SpawnPlayerControlledGameObject();
            PlayerController.Possess(PlayerControlledGameObject);
            return true;
        }
        
        private bool CreatePlayerController()
        {
            if (playerControllerPrefab == null)
            {
                Debug.LogError("PlayerController prefab is not assigned.");
                return false;
            }
            PlayerController = Instantiate(playerControllerPrefab);
            return true;
        }

        private bool CreateHUD()
        {
            if (hudPrefab == null)
            {
                Debug.LogError("HUD prefab is not assigned.");
                return false;
            }
            HUD = Instantiate(hudPrefab);
            return true;
        }

        private void SpawnPlayerControlledGameObject()
        {
            // Find a PlayerStart in the scene to determine spawn position and rotation
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;
            PlayerStart playerStart = FindObjectOfType<PlayerStart>();
            if (playerStart != null)
            {
                spawnPosition = playerStart.transform.position;
                spawnRotation = playerStart.transform.rotation;
            }
            
            if (playerControlledGameObjectPrefab == null)
            {
                return;
            }
            PlayerControlledGameObject = Instantiate(playerControlledGameObjectPrefab, spawnPosition, spawnRotation);
        }
    }
}
