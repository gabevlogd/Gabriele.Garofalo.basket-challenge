using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    public class MouseManager 
    {
        public static event Action OnMouseLeftDownEvent;
        public static event Action OnMouseMovedEvent;
        public static event Action OnMouseLeftUpEvent;
        
        private Vector3 _lastMousePosition;

        public void UpdateMouseLeftEvents()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
                OnMouseLeftDownEvent?.Invoke();
            }
            else if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition == _lastMousePosition) return;
                _lastMousePosition = Input.mousePosition;
                OnMouseMovedEvent?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _lastMousePosition = Input.mousePosition;
                OnMouseLeftUpEvent?.Invoke();
            }
        }
    }
}
