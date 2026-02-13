using UnityEngine;

namespace BasketChallenge.Core
{
    //[CreateAssetMenu(fileName = "ControllerClass", menuName = "Controllers/ControllerClass", order = 0)]
    public class ControllerClass : ScriptableObject
    {
        [SerializeField]
        private string _controllerName = "Controller";
        
        public virtual Controller CreateController()
        {
            return CreateController<Controller>();
        }
        
        protected T CreateController<T>() where T : Controller
        {
            ControllerFactory<T> factory = new ControllerFactory<T>();
            return factory.CreateController(_controllerName);
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(_controllerName);
        }
    }
}