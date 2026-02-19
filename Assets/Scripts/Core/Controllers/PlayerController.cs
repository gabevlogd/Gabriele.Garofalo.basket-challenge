using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    [RequireComponent(typeof(InputComponent))]
    public class PlayerController : Controller
    {
        public InputComponent InputComponent => _inputComponent;
        private InputComponent _inputComponent;

        public PlayerCameraManager CameraManager => _cameraManager;
        [SerializeField] private PlayerCameraManagerClass playerCameraManager; 
        private PlayerCameraManager _cameraManager;

        protected virtual void Awake()
        {
            if (!TryGetComponent(out _inputComponent))
            {
                Debug.LogError("InputComponent is missing on PlayerController GameObject.");
            }
        }

        public override void SetConfig(ControllerClass config)
        {
            base.SetConfig(config);
            if (config is PlayerControllerClass playerControllerConfig)
            {
                playerCameraManager = playerControllerConfig.playerCameraManager;
            }
        }

        protected void EnableTouchEvents() => _inputComponent?.EnableTouchEvents();
        protected void DisableTouchEvents() => _inputComponent?.DisableTouchEvents();
        protected void EnableMouseEvents() => _inputComponent?.EnableMouseEvents();
        protected void DisableMouseEvents() => _inputComponent?.DisableMouseEvents();

        public virtual void InitCameraManager()
        {
            if (!playerCameraManager)
            {
                Debug.LogWarning("PlayerCameraManager is missing on PlayerController GameObject.");
                return;
            }
            
            _cameraManager = playerCameraManager.CreatePlayerCameraManager();
            _cameraManager.transform.parent = transform;
            
            if (!ControllableObject || !_cameraManager) return;

            Camera playerCamera = null;
            
            // Search for an existing Camera component in the ControllableObject's children
            Transform[] allChildTransforms = ControllableObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildTransforms)
            {
                if (child == ControllableObject.transform)
                    continue; 
                
                if (child.TryGetComponent(out Camera foundCamera))
                {
                    playerCamera = foundCamera;
                    break;
                }
            }
            
            // If no Camera component is found, create a new one and parent it to the ControllableObject
            if (!playerCamera)
            {
                GameObject cameraGameObject = new GameObject("PlayerCamera");
                cameraGameObject.transform.SetParent(ControllableObject.transform, false);
                playerCamera = cameraGameObject.AddComponent<Camera>();
            }
            
            _cameraManager.Init(playerCamera);
        }

        public void SetNewCameraViewpoint(Transform newViewpoint, bool keepWorldPosition = true,
            bool keepWorldRotation = true)
        {
            if (_cameraManager == null)
            {
                Debug.LogWarning("PlayerCameraManager is not initialized. Cannot set new camera viewpoint.");
                return;
            }

            _cameraManager.SetNewCameraViewpoint(newViewpoint, keepWorldPosition, keepWorldRotation);
        }
    }
}