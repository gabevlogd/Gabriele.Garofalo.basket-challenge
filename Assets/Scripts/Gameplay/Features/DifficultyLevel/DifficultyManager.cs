using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        [SerializeField] private AIBehaviourData currentDifficultyLevel;
        
        public static event Action<AIBehaviourData> OnDifficultyLevelChanged;

        protected override void Awake()
        {
            base.Awake();
            if (currentDifficultyLevel == null)
            {
                Debug.LogWarning("DifficultyManager: No difficulty level set. Please assign a default difficulty level in the inspector.");
            }
        }

        public static void SetDifficultyLevel(AIBehaviourData newDifficultyLevel)
        {
            if (newDifficultyLevel == null)
            {
                Debug.LogError("DifficultyManager: Cannot set difficulty level to null.");
                return;
            }
            
            Instance.currentDifficultyLevel = newDifficultyLevel;
            OnDifficultyLevelChanged?.Invoke(newDifficultyLevel);
        }
    }
}