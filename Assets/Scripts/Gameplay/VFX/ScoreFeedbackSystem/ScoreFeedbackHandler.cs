using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [System.Serializable]
    public class ScoreFeedback
    {
        public ThrowOutcome associatedOutcome;
        
        public ParticleSystem particleSystemPrefab;
        
        [HideInInspector]
        public ParticleSystem particleSystemInstance;
        
        public void PlayFeedback()
        {
            if (particleSystemInstance != null)
            {
                particleSystemInstance.Play();
            }
        }
    }
    
    public class ScoreFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private ScoreFeedbacks data;
        
        private Dictionary<ThrowOutcome, ScoreFeedback> _feedbackMap;

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            ScoreDetector.OnScoreDetected += HandleScoreDetected;
        }

        private void OnDisable()
        {
            ScoreDetector.OnScoreDetected -= HandleScoreDetected;
        }

        private void Init()
        {
            _feedbackMap = new Dictionary<ThrowOutcome, ScoreFeedback>();
            foreach (ScoreFeedback feedback in data.scoreFeedbackList)
            {
                if (!_feedbackMap.ContainsKey(feedback.associatedOutcome))
                {
                    if (feedback.particleSystemPrefab == null)
                    {
                        Debug.LogWarning($"Score feedback for outcome {feedback.associatedOutcome} has no particle system assigned.");
                        continue;
                    }
                    _feedbackMap.Add(feedback.associatedOutcome, feedback);
                    ParticleSystem system = Instantiate(feedback.particleSystemPrefab, transform);
                    system.transform.position = ThrowPositionsHandler.GetPerfectThrowPosition() + Vector3.down * 0.1f;
                    feedback.particleSystemInstance = system;
                }
                else
                {
                    Debug.LogWarning($"Duplicate feedback for outcome {feedback.associatedOutcome} detected. Only the first one will be used.");
                }
            }
        }
        
        private void HandleScoreDetected(ShootingCharacter scoreOwner, ThrowOutcome scoreOutcome)
        {
            if (_feedbackMap.TryGetValue(scoreOutcome, out ScoreFeedback feedback))
            {
                feedback.PlayFeedback();
            }
            else
            {
                Debug.LogWarning($"No feedback found for outcome {scoreOutcome}");
            }
        }
    }
}
