using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    public class TouchManager 
    {
        public static event Action OnTouchBeganEvent;
        public static event Action OnTouchMovedEvent;
        public static event Action OnTouchEndedEvent;
        
        public void UpdateTouchEvents(int index)
        {
            if (!AnyTouch()) return;
            
            switch (Input.GetTouch(index).phase)
            {
                case TouchPhase.Began:
                    OnTouchBeganEvent?.Invoke();
                    break;
                case TouchPhase.Moved:
                    OnTouchMovedEvent?.Invoke();
                    break;
                case TouchPhase.Ended:
                    OnTouchEndedEvent?.Invoke();
                    break;
            }
        }
        
        public static bool AnyTouch() => Input.touchCount > 0;
        
        public static Vector2 GetSwipeVelocity(int index)
        {
            return Input.GetTouch(index).deltaTime != 0 ? 
                Input.GetTouch(index).deltaPosition / Input.GetTouch(index).deltaTime : Vector2.zero;
        }
    }
}
