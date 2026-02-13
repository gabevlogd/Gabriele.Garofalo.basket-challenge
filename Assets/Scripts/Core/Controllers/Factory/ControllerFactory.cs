using UnityEngine;

namespace BasketChallenge.Core
{
    public class ControllerFactory<T> where T : Controller
    {
        public T CreateController(string controllerName)
        {
            GameObject instance = new GameObject(controllerName);
            T component = instance.AddComponent<T>();
            return component;
        }
    }
}