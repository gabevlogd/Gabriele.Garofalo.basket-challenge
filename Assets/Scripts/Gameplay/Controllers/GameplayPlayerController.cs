using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayPlayerController : PlayerController
    {
        public static event Action OnThrowInputBeganEvent;
        public static event Action OnThrowInputMovedEvent;
        public static event Action OnThrowInputEndedEvent;
        
        private void OnEnable()
        {
            PauseMenu.OnPauseMenuOpened += DisableThrowInput;
            PauseMenu.OnPauseMenuClosed += EnableThrowInput;
            
            TouchManager.OnTouchBeganEvent += ThrowInputBegan;
            TouchManager.OnTouchMovedEvent += ThrowInputMoved;
            TouchManager.OnTouchEndedEvent += ThrowInputEnded;
            
            MouseManager.OnMouseLeftDownEvent += ThrowInputBegan;
            MouseManager.OnMouseMovedEvent += ThrowInputMoved;
            MouseManager.OnMouseLeftUpEvent += ThrowInputEnded;
            
            SwipeThrowController.OnThrowCompleted += DisableThrowInput;
            PlayerCharacter.OnThrowResetEvent += EnableThrowInput;
        }
        
        private void OnDisable()
        {
            PauseMenu.OnPauseMenuOpened -= DisableThrowInput;
            PauseMenu.OnPauseMenuClosed -= EnableThrowInput;
            
            TouchManager.OnTouchBeganEvent -= ThrowInputBegan;
            TouchManager.OnTouchMovedEvent -= ThrowInputMoved;
            TouchManager.OnTouchEndedEvent -= ThrowInputEnded;
            
            MouseManager.OnMouseLeftDownEvent -= ThrowInputBegan;
            MouseManager.OnMouseMovedEvent -= ThrowInputMoved;
            MouseManager.OnMouseLeftUpEvent -= ThrowInputEnded;
            
            SwipeThrowController.OnThrowCompleted -= DisableThrowInput;
            PlayerCharacter.OnThrowResetEvent -= EnableThrowInput;
        }
        
        private void ThrowInputBegan() => OnThrowInputBeganEvent?.Invoke();
        private void ThrowInputMoved() => OnThrowInputMovedEvent?.Invoke();
        private void ThrowInputEnded() => OnThrowInputEndedEvent?.Invoke();

        public static Vector3 GetPointerPosition()
        {
            if (Application.isMobilePlatform)
            {
                return TouchManager.AnyTouch() ? Input.GetTouch(0).position : Vector3.zero;
            }
            return Input.mousePosition;
        }

        private void DisableThrowInput(float f)
        {
            DisableThrowInput();
        }

        private void DisableThrowInput()
        {
            DisableTouchEvents();
            DisableMouseEvents();
        }
        
        private void EnableThrowInput()
        {
            EnableTouchEvents();
            EnableMouseEvents();
        }
    }
}
