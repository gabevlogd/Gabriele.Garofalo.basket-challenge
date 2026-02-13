using UnityEngine;

namespace BasketChallenge.Core
{
    public abstract class ControllerBase : MonoBehaviour
    {
        [HideInInspector]
        public ControllableBase controllableObject;
        
        public void Possess(ControllableBase newBase)
        {
            if (newBase == null)
            {
                Debug.LogWarning("Attempting to possess a null game object.");
                return;
            }
            
            if (newBase == controllableObject) return;
            
            if (controllableObject != null)
            {
                controllableObject.OnUnpossess(this);
            }
            controllableObject = newBase;
            controllableObject.OnPossess(this);
        }
    }
}
