using UnityEngine;
using UnityEngine.Serialization;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "PlayerCameraManager", menuName = "ScriptableObjects/CameraManagers/PlayerCameraManager", order = 0)]
    public class PlayerCameraManagerClass : ScriptableObject
    {
        public string cameraManagerName = "PlayerCameraManager";
        
        public bool useCameraViewpointPosition = true;
        public bool useCameraViewpointRotation;
        public bool enableCameraLag;
        public bool enableCameraRotationLag;
        
        public float cameraLagSpeed = 1f;
        public float cameraRotationLagSpeed = 1f;
        
        
        public virtual PlayerCameraManager CreatePlayerCameraManager()
        {
            return CreatePlayerCameraManager<PlayerCameraManager>();
        }
        
        protected T CreatePlayerCameraManager<T>() where T : PlayerCameraManager
        {
            PlayerCameraManagerFactory<T> factory = new PlayerCameraManagerFactory<T>();
            return factory.CreatePlayerCameraManager(this);
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(cameraManagerName);
        }
    }
}