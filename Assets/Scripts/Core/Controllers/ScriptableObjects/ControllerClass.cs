using UnityEngine;
using UnityEngine.Serialization;

namespace BasketChallenge.Core
{
    //[CreateAssetMenu(fileName = "ControllerClass", menuName = "Controllers/ControllerClass", order = 0)]
    public class ControllerClass : ScriptableObject
    {
        public string controllerName = "Controller";
        
        public virtual Controller CreateController()
        {
            return CreateController<Controller>();
        }
        
        protected T CreateController<T>() where T : Controller
        {
            ControllerFactory<T> factory = new ControllerFactory<T>();
            return factory.CreateController(this);
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(controllerName);
        }
    }
}