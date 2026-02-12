using UnityEngine;

namespace BasketChallenge.Core
{
    public class ControllableGameObject : MonoBehaviour
    {
        public ControllerBase Controller { get; private set; }

        public virtual void OnPossess(ControllerBase controller)
        {
            //Debug.Log($"{gameObject.name} possessed by {controller.gameObject.name}");
            if (controller == null)
            {
                Debug.LogWarning("Attempting to possess with a null controller.");
                return;
            }

            Controller = controller;
        }

        public virtual void OnUnpossess(ControllerBase controller)
        {
            //Debug.Log($"{gameObject.name} unpossessed by {controller.gameObject.name}");
            if (controller == null)
            {
                Debug.LogWarning("Attempting to unpossess with a null controller.");
                return;
            }

            Controller = null;
        }
    }
}