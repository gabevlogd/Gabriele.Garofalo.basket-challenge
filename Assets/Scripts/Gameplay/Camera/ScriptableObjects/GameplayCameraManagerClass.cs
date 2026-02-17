using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayCameraManager", menuName = "CameraManagers/GameplayCameraManager", order = 0)]
    public class GameplayCameraManagerClass : PlayerCameraManagerClass
    {
        public override PlayerCameraManager CreatePlayerCameraManager()
        {
            return CreatePlayerCameraManager<GameplayCameraManager>();
        }
    }
}