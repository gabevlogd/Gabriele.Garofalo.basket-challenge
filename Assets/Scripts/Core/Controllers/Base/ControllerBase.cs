using UnityEngine;

namespace BasketChallenge.Core
{
    public class ControllerBase : MonoBehaviour
    {
        [HideInInspector]
        public ControllableGameObject controlledGameObject;
        
        public void Possess(ControllableGameObject newGameObject)
        {
            if (newGameObject == null)
            {
                Debug.LogWarning("Attempting to possess a null game object.");
                return;
            }
            
            if (newGameObject == controlledGameObject) return;
            
            if (controlledGameObject != null)
            {
                controlledGameObject.OnUnpossess(this);
            }
            controlledGameObject = newGameObject;
            controlledGameObject.OnPossess(this);
        }
    }
}
