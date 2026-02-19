using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "PlayerControllerClass", menuName = "ScriptableObjects/Controllers/PlayerControllerClass", order = 0)]
    public class PlayerControllerClass : ControllerClass
    {
        public PlayerCameraManagerClass playerCameraManager;
        
        public override Controller CreateController()
        {
            return CreateController<PlayerController>();
        }
    }
}