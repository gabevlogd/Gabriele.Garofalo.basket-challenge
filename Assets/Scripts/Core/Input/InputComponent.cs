using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    public class InputComponent : MonoBehaviour
    {
        private TouchManager _touchManager;
        private MouseManager _mouseManager;
        
        private bool _updateTouchEvents = true;
        private bool _updateMouseEvents = true;
        
        private void Awake()
        {
            _touchManager = new TouchManager();
            _mouseManager = new MouseManager();
        }
        
        private void Update()
        {
            UpdatePrimaryTouchEvents();
            UpdateMouseLeftEvents();
        }
        
        public void EnableTouchEvents() => _updateTouchEvents = true;
        public void DisableTouchEvents() => _updateTouchEvents = false;

        private void UpdatePrimaryTouchEvents()
        {
            if (!_updateTouchEvents) return;
            _touchManager.UpdateTouchEvents(0);
        }
        
        public void EnableMouseEvents() => _updateMouseEvents = true;
        public void DisableMouseEvents() => _updateMouseEvents = false;

        private void UpdateMouseLeftEvents()
        {
            if (!_updateMouseEvents) return;
            _mouseManager.UpdateMouseLeftEvents();
        }
        
    }
}
