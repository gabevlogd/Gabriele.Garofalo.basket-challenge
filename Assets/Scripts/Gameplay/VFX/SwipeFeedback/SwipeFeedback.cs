using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(TrailRenderer))]
    public class SwipeFeedback : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        
        private Camera _camera;
        
        private void Awake()
        {
            if (TryGetComponent(out _trailRenderer))
            {
                _trailRenderer.Clear();
                _trailRenderer.emitting = false;
            }
        }

        private void Start()
        {
            if (CoreUtility.TryGetPlayerCameraManager(out GameplayCameraManager cameraManager))
            {
                _camera = cameraManager.PlayerCamera;
            }
            else
            {
                Debug.LogError("SwipeFeedback: Failed to find GameplayCameraManager. Swipe feedback will not work.");
            }
        }

        private void OnEnable()
        {
            SwipeThrowController.OnThrowStarted += ShowSwipeFeedback;
            SwipeThrowController.OnThrowPowerUpdated += UpdateSwipeFeedback;
            SwipeThrowController.OnThrowCompleted += HideSwipeFeedback;
            SwipeThrowController.OnThrowCanceled += HideSwipeFeedback;
        }

        private void OnDisable()
        {
            SwipeThrowController.OnThrowStarted -= ShowSwipeFeedback;
            SwipeThrowController.OnThrowPowerUpdated -= UpdateSwipeFeedback;
            SwipeThrowController.OnThrowCompleted -= HideSwipeFeedback;
            SwipeThrowController.OnThrowCanceled -= HideSwipeFeedback;
        }

        private void ShowSwipeFeedback()
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
            _trailRenderer.transform.position = GetTouchWorldPosition();
        }

        private void UpdateSwipeFeedback(float throwPower)
        {
            _trailRenderer.transform.position = GetTouchWorldPosition();
        }
        
        private void HideSwipeFeedback()
        {
            _trailRenderer.emitting = false;
        }
        
        private void HideSwipeFeedback(float throwPower)
        {
            HideSwipeFeedback();
        }
        
        private Vector3 GetTouchWorldPosition()
        {
            if (Application.isMobilePlatform)
            {
                return _camera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1f));
            }
            return _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        }
    }
}
