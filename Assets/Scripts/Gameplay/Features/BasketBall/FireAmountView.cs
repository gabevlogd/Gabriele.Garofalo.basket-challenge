using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class FireAmountView : MonoBehaviour
    {
        [SerializeField]
        private Image currentFireAmountImage;
        
        [SerializeField]
        private Color onFireColor;
        
        [SerializeField]
        private Color offFireColor;
        
        private Coroutine _barAnimationCoroutine;

        private void Awake()
        {
            if (currentFireAmountImage == null)
            {
                Debug.LogError("FireAmountView requires a reference to the current fire amount Image.");
            }
            else
            {
                currentFireAmountImage.fillAmount = 0f;
                currentFireAmountImage.color = offFireColor;
            }
        }

        private void OnEnable()
        {
            if (CoreUtility.TryGetPlayerControlledObject(out ShootingCharacter player))
            {
                player.CurrentBall.FireBallComponent.OnFireAmountChanged += UpdateFireAmountView;
                player.CurrentBall.FireBallComponent.OnBallIgnited += OnReachedMaxFireAmount;
                player.CurrentBall.FireBallComponent.OnBallExtinguished += OnFireAmountDepleted;
            }
        }

        private void OnDisable()
        {
            if (CoreUtility.TryGetPlayerControlledObject(out ShootingCharacter player))
            {
                player.CurrentBall.FireBallComponent.OnFireAmountChanged -= UpdateFireAmountView;
                player.CurrentBall.FireBallComponent.OnBallIgnited -= OnReachedMaxFireAmount;
                player.CurrentBall.FireBallComponent.OnBallExtinguished -= OnFireAmountDepleted;
            }
            
            if (_barAnimationCoroutine != null)
            {
                StopCoroutine(_barAnimationCoroutine);
            }
        }

        private void UpdateFireAmountView(float currentFireAmount)
        {
            if (!currentFireAmountImage) return;
            
            //currentFireAmountImage.fillAmount = currentFireAmount;
            if (_barAnimationCoroutine != null)
            {
                StopCoroutine(_barAnimationCoroutine);
            }
            _barAnimationCoroutine = StartCoroutine(FireAmountBarAnimation(currentFireAmount));
        }
        
        private IEnumerator FireAmountBarAnimation(float targetFillAmount)
        {
            if (!currentFireAmountImage) yield break;

            float initialFillAmount = currentFireAmountImage.fillAmount;
            float elapsedTime = 0f;
            float animationDuration = 0.2f; 

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                currentFireAmountImage.fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, elapsedTime / animationDuration);
                yield return null;
            }

            currentFireAmountImage.fillAmount = targetFillAmount;
        }
        
        private void OnReachedMaxFireAmount()
        {
            if (!currentFireAmountImage) return;
            
            currentFireAmountImage.color = onFireColor;
        }

        private void OnFireAmountDepleted()
        {
            if (!currentFireAmountImage) return;

            currentFireAmountImage.color = offFireColor;
        }
    }
}