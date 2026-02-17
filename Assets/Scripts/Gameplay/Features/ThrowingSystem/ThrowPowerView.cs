using System;
using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class ThrowPowerView : MonoBehaviour
    {
        [SerializeField] private Image sliderFillerImage;

        [SerializeField] private Image perfectThrowImage;

        [SerializeField] private Image backboardThrowImage;

        [SerializeField] private ThrowPowerProcessor processor = new ThrowPowerProcessor();

        private void Awake()
        {
            if (sliderFillerImage == null)
                Debug.LogError("Slider Filler Image is not assigned in the inspector.");

            if (perfectThrowImage == null)
                Debug.LogError("Perfect Throw Image is not assigned in the inspector.");

            if (backboardThrowImage == null)
                Debug.LogError("Backboard Throw Image is not assigned in the inspector.");
        }

        private void OnEnable()
        {
            UpdateSliderFill(0f);
            SwipeThrowController.OnThrowPowerUpdated += UpdateSliderFill;
            SwipeThrowController.OnThrowCanceled += ResetSliderFill;

            if (CoreUtility.TryGetPlayerControlledObject(out PlayerCharacter playerCharacter))
            {
                if (playerCharacter.TryGetComponent(out ThrowerComponent thrower))
                {
                    thrower.OnPerfectPowerUpdated -= OnPerfectPowerUpdated;
                    thrower.OnPerfectPowerUpdated += OnPerfectPowerUpdated;
                }
            }
        }
        
        private void OnDisable()
        {
            SwipeThrowController.OnThrowPowerUpdated -= UpdateSliderFill;
            SwipeThrowController.OnThrowCanceled -= ResetSliderFill;
        }

        private void UpdateSliderFill(float fillAmount)
        {
            if (sliderFillerImage == null) return;
            sliderFillerImage.fillAmount = fillAmount;
        }

        private void ResetSliderFill()
        {
            UpdateSliderFill(0f);
        }
        
        private void OnPerfectPowerUpdated(float perfectPower)
        {
            perfectPower = Mathf.Clamp01(perfectPower);
            RedrawRanges(perfectPower);
        }

        private void RedrawRanges(float perfectPower)
        {
            if (processor == null) return;

            ThrowPowerRangesRuntime r = processor.CalculateRanges(perfectPower);

            SetRangeRect(perfectThrowImage, r.PerfectMin, r.PerfectMax);
            SetRangeRect(backboardThrowImage, r.BackboardMin, r.BackboardMax);
        }
        
        private static void SetRangeRect(Image img, float min01, float max01)
        {
            if (img == null) return;

            min01 = Mathf.Clamp01(min01);
            max01 = Mathf.Clamp01(max01);
            if (max01 < min01) (min01, max01) = (max01, min01);

            var rt = img.rectTransform;
            rt.anchorMin = new Vector2(0f, min01);
            rt.anchorMax = new Vector2(1f, max01);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}