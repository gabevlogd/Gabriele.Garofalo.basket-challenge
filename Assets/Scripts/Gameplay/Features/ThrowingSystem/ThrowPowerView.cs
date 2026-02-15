using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class ThrowPowerView : MonoBehaviour
    {
        [SerializeField] 
        private Image _sliderFillerImage;

        [SerializeField] 
        private Image _perfectThrowImage;

        [SerializeField] 
        private Image _backboardThrowImage;

        private void Awake()
        {
            if (_sliderFillerImage == null)
                Debug.LogError("Slider Filler Image is not assigned in the inspector.");

            if (_perfectThrowImage == null)
                Debug.LogError("Perfect Throw Image is not assigned in the inspector.");

            if (_backboardThrowImage == null)
                Debug.LogError("Backboard Throw Image is not assigned in the inspector.");

            UpdateSliderFill(0f);
            SwipeThrowController.OnThrowPowerUpdated += UpdateSliderFill;
            SwipeThrowController.OnThrowCanceled += () => UpdateSliderFill(0f);
        }

        private void UpdateSliderFill(float fillAmount)
        {
            if (_sliderFillerImage == null) return;
            _sliderFillerImage.fillAmount = fillAmount;
        }
    }
}