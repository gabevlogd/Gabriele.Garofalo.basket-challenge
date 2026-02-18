using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class BackboardBonusView : MonoBehaviour
    {
        [SerializeField]
        private Canvas bonusCanvas;
        
        [SerializeField]
        private TextMeshProUGUI bonusText;
        
        [SerializeField]
        private Image bonusBorderImage;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            BackboardBonusManager.OnBonusTriggered += EnableBonusView;
            BackboardBonusManager.OnBonusCleared += DisableBonusView;
        }

        private void OnDisable()
        {
            BackboardBonusManager.OnBonusTriggered -= EnableBonusView;
            BackboardBonusManager.OnBonusCleared -= DisableBonusView;
        }

        private void EnableBonusView(BackboardBonus bonus)
        {
            if (bonusText && bonusCanvas && bonusBorderImage)
            {
                string bonusValueText = bonus.extraPoints.ToString();
                bonusText.text = $"Bonus +{bonusValueText}";
                bonusText.color = bonus.color;
                bonusBorderImage.color = bonus.color;
                bonusCanvas.enabled = true;
            }
        }
        
        private void DisableBonusView()
        {
            if (bonusCanvas)
            {
                bonusCanvas.enabled = false;
            }
        }

        private void Init()
        {
            if (bonusText == null)
            {
                Debug.LogError("BackboardBonusView requires a reference to a TextMeshProUGUI component to function properly.");
            }
            
            if (bonusBorderImage == null)
            {
                Debug.LogError("BackboardBonusView requires a reference to an Image component for the border to function properly.");
            }
            
            if (bonusCanvas == null)
            {
                Debug.LogError("BackboardBonusView requires a reference to a Canvas component to function properly.");
            }
            else
            {
                bonusCanvas.enabled = false; 
            }
        }
    }
}