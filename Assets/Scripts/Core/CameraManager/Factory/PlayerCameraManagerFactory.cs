using UnityEngine;

namespace BasketChallenge.Core
{
    public class PlayerCameraManagerFactory<T> where T : PlayerCameraManager
    {
        public T CreatePlayerCameraManager(PlayerCameraManagerClass config) 
        {
            GameObject instance = new GameObject(config.cameraManagerName);
            T component = instance.AddComponent<T>();
            component.SetConfig(config);
            return component;
        }
    }
}