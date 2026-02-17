using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BasketChallenge.Core
{
    public class PlayerCameraManager : MonoBehaviour
    {
        public Transform CameraViewpoint { get; protected set; }
        public Camera PlayerCamera { get; protected set; }

        public bool useCameraViewpointPosition = true;
        public bool useCameraViewpointRotation = false;
        public bool enableCameraLag = true;
        public bool enableCameraRotationLag = false;
        
        public float cameraLagSpeed = 1f;
        public float cameraRotationLagSpeed = 1f;

        private void LateUpdate()
        {
            if (CameraViewpoint == null || PlayerCamera == null) return;
            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        public void Init(Camera playerCamera)
        {
            if (playerCamera == null)
            {
                Debug.LogError("PlayerCameraManager requires a valid Camera reference.");
                return;
            }
            PlayerCamera = playerCamera;
            CameraViewpoint = playerCamera.transform;
        }
        
        public virtual void SetConfig(PlayerCameraManagerClass config)
        {
            useCameraViewpointPosition = config.useCameraViewpointPosition;
            useCameraViewpointRotation = config.useCameraViewpointRotation;
            enableCameraLag = config.enableCameraLag;
            enableCameraRotationLag = config.enableCameraRotationLag;
            cameraLagSpeed = config.cameraLagSpeed;
            cameraRotationLagSpeed = config.cameraRotationLagSpeed;
        }
        
        public void SetNewCameraViewpoint(Transform newViewpoint, bool keepWorldPosition = true, bool keepWorldRotation = true)
        {
            if (newViewpoint == null)
            {
                Debug.LogWarning("Attempting to set a null camera viewpoint.");
                return;
            }
            CameraViewpoint = newViewpoint;
            
            if (!keepWorldPosition)
            {
                PlayerCamera.transform.position = newViewpoint.position;
            }
            
            if (!keepWorldRotation)
            {
                PlayerCamera.transform.rotation = newViewpoint.rotation;
            }
        }

        private void UpdateCameraPosition()
        {
            if (!useCameraViewpointPosition) return;
            if (enableCameraLag)
            {
                PlayerCamera.transform.position = Vector3.Lerp(PlayerCamera.transform.position, CameraViewpoint.position, Time.deltaTime * cameraLagSpeed);
            }
            else
            {
                PlayerCamera.transform.position = CameraViewpoint.position;
            }
        }
        
        private void UpdateCameraRotation()
        {
            if (!useCameraViewpointRotation) return;
            if (enableCameraRotationLag)
            {
                PlayerCamera.transform.rotation = Quaternion.Slerp(PlayerCamera.transform.rotation, CameraViewpoint.rotation, Time.deltaTime * cameraRotationLagSpeed);
            }
            else
            {
                PlayerCamera.transform.rotation = CameraViewpoint.rotation;
            }
        }
    }
}