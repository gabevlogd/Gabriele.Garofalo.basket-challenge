using UnityEngine;

namespace BasketChallenge.Core
{
    public class ControllerFactory<T> where T : Controller
    {
        public T CreateController(ControllerClass config) 
        {
            GameObject instance = new GameObject(config.controllerName);
            T component = instance.AddComponent<T>();
            component.SetConfig(config);
            return component;
        }
    }
}