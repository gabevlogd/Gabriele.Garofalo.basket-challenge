using UnityEngine;

namespace BasketChallenge.Core
{
    public abstract class ControllerBase : MonoBehaviour
    {
        public ControllableBase ControllableObject { get; private set; }
        
        public void Possess(ControllableBase newBase)
        {
            if (newBase == null)
            {
                Debug.LogWarning("Attempting to possess a null game object.");
                return;
            }
            
            if (newBase == ControllableObject) return;
            
            if (ControllableObject != null)
            {
                ControllableObject.OnUnpossess(this);
            }
            ControllableObject = newBase;
            ControllableObject.OnPossess(this);
        }

        public virtual void SetConfig(ControllerClass config) 
        {
        }
    }
}
