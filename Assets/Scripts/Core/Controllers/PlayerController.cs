using UnityEngine;

namespace BasketChallenge.Core
{
    [RequireComponent(typeof(InputComponent))]
    public class PlayerController : Controller
    {
        private InputComponent _inputComponent;

        protected virtual void Awake()
        {
            if (!TryGetComponent(out _inputComponent))
            {
                Debug.LogError("InputComponent is missing on PlayerController GameObject.");
            }
        }

        protected void EnableTouchEvents() => _inputComponent?.EnableTouchEvents();
        protected void DisableTouchEvents() => _inputComponent?.DisableTouchEvents();
        protected void EnableMouseEvents() => _inputComponent?.EnableMouseEvents();
        protected void DisableMouseEvents() => _inputComponent?.DisableMouseEvents();
    }
}