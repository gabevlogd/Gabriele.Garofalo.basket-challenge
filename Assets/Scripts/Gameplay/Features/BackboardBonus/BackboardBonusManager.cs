using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BasketChallenge.Gameplay
{
    public class BackboardBonusManager : MonoBehaviour
    {
        public static event Action<BackboardBonus> OnBonusTriggered;
        public static event Action OnBonusCleared;
        
        [SerializeField] private BackboardHitDetector backboardHitDetector;

        [SerializeField] private BackboardBonuses bonusesData;
        
        private Coroutine _randomBonusCoroutine;
        
        private BackboardBonus _currentBonus;

        private void Awake()
        {
            if (backboardHitDetector == null)
            {
                Debug.LogError("BackboardBonusManager requires a reference to a BackboardHitDetector to function properly.");
            }
        }
        
        private void OnEnable()
        {
            if (backboardHitDetector)
            {
                backboardHitDetector.OnBackboardHit += HandleBackboardHit;
            }
            
            ScoreDetector.OnScoreDetected += HandleScoreDetected;
            MatchManager.OnMatchEnd += HandleMatchEnd;
            
            _randomBonusCoroutine = StartCoroutine(RandomBonusTrigger());
        }

        private void OnDisable()
        {
            if (backboardHitDetector)
            {
                backboardHitDetector.OnBackboardHit -= HandleBackboardHit;
            }
            
            ScoreDetector.OnScoreDetected -= HandleScoreDetected;
            MatchManager.OnMatchEnd -= HandleMatchEnd;
            
            if (_randomBonusCoroutine != null)
            {
                StopCoroutine(_randomBonusCoroutine);
            }
        }

        private void HandleScoreDetected(ShootingCharacter scoreOwner, ThrowOutcome scoreOutcome)
        {
            if (_currentBonus == null) return;

            if (scoreOwner.CurrentBall.lastBackboardBonus == _currentBonus)
            {
                ClearCurrentBonus();
            }
        }
        
        private void HandleBackboardHit(BasketBall ball)
        {
            if (_currentBonus == null) return;
            
            if (ball.LastThrowOutcome == ThrowOutcome.BackboardMake)
            {
                ball.lastBackboardBonus = _currentBonus;
            }
        }
        
        private void HandleMatchEnd()
        {
            if (_randomBonusCoroutine != null)
            {
                StopCoroutine(_randomBonusCoroutine);
            }
            ClearCurrentBonus(false);
        }

        private IEnumerator RandomBonusTrigger()
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            TryTriggerBackboardBonus();
        }
        
        private void TryTriggerBackboardBonus()
        {
            foreach (BackboardBonus bonus in bonusesData.bonusList)
            {
                if (bonus.TryTriggerBonus())
                {
                    TriggerBonus(bonus);
                    return;
                }
            }
            
            _randomBonusCoroutine = StartCoroutine(RandomBonusTrigger());
        }
        
        private void TriggerBonus(BackboardBonus bonus)
        {
            _currentBonus = bonus;
            OnBonusTriggered?.Invoke(bonus);
        }
        
        private void ClearCurrentBonus(bool startNewBonusCoroutine = true)
        {
            _currentBonus = null;
            _randomBonusCoroutine = startNewBonusCoroutine ? StartCoroutine(RandomBonusTrigger()) : null;
            OnBonusCleared?.Invoke();
        }
    }
}
