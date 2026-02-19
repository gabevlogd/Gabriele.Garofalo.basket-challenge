using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayCameraManager", menuName = "ScriptableObjects/CameraManagers/GameplayCameraManager", order = 0)]
    public class GameplayCameraManagerClass : PlayerCameraManagerClass
    {
        public float followBallSpeed = 3f;
        public float blendOutFollowSpeed = 2f;
        
        public override PlayerCameraManager CreatePlayerCameraManager()
        {
            return CreatePlayerCameraManager<GameplayCameraManager>();
        }
    }
}