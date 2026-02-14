using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    public class InputComponent : MonoBehaviour
    {
        private TouchManager _touchManager;
        
        private bool _updateTouchEvents = true;
        
        private void Awake()
        {
            _touchManager = new TouchManager();
        }
        
        private void Update()
        {
            if (TouchManager.AnyTouch())
            {
                UpdatePrimaryTouchEvents();
            }
        }
        
        public void EnableTouchEvents() => _updateTouchEvents = true;
        public void DisableTouchEvents() => _updateTouchEvents = false;

        private void UpdatePrimaryTouchEvents()
        {
            if (!_updateTouchEvents) return;
            _touchManager.UpdateTouchEvents(0);
        }
    }
}
