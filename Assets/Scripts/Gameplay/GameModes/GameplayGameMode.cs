using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(MatchResultManager))]
    public class GameplayGameMode : GameModeBase
    {
        public MatchResultManager MatchResultManager => _matchResultManager;
        private MatchResultManager _matchResultManager;
        
        protected override void Awake()
        {
            base.Awake();
            if (TryGetComponent(out MatchResultManager matchResultManager))
            {
                _matchResultManager = matchResultManager;
            }
            else
            {
                Debug.LogError("GameplayGameMode requires a MatchResultManager component.");
            }
        }
    }
}