using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(StateMachine))]
    public class MatchManager : MonoBehaviour
    {
        public static event Action OnMatchStart;
        public static event Action<float> OnMatchStartDelayUpdate;
        public static event Action<float> OnMatchDurationUpdate; 
        public static event Action OnMatchEnd;
        
        public MatchData matchData;
        private StateMachine _stateMachine;

        private void Awake()
        {
            if (TryGetComponent(out StateMachine stateMachine))
            {
                _stateMachine = stateMachine;
            }

            if (!matchData)
            {
                Debug.LogError("MatchManager requires a MatchData asset to function properly.");
            }
        }
        
        private void Start()
        {
            _stateMachine.StartStateMachine();
        }

        public void StartMatch()
        {
            OnMatchStart?.Invoke();
        }
        
        public void EndMatch()
        {
            OnMatchEnd?.Invoke();
        }
        
        public void UpdateMatchStartDelay(float delay)
        {
            OnMatchStartDelayUpdate?.Invoke(delay);
        }
        
        public void UpdateMatchDuration(float duration)
        {
            OnMatchDurationUpdate?.Invoke(duration);
        }
    }
}
