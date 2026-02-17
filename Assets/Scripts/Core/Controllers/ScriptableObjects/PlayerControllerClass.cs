using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "PlayerControllerClass", menuName = "Controllers/PlayerControllerClass", order = 0)]
    public class PlayerControllerClass : ControllerClass
    {
        public PlayerCameraManagerClass playerCameraManager;
        
        public override Controller CreateController()
        {
            return CreateController<PlayerController>();
        }
    }
}