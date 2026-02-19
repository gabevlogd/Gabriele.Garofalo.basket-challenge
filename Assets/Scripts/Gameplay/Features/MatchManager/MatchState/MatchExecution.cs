using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MatchExecution : MatchState
    {
        private float _duration;
        
        protected override void OnEnter()
        {
            base.OnEnter();
            MatchManager.StartMatch();
            _duration = MatchManager.matchData.duration;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                MatchManager.UpdateMatchDuration(_duration);
            }
            else
            {
                GoToNextState();
            }
        }
        
        private void GoToNextState()
        {
            if (CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode))
            {
                MatchResult matchResult = gameplayGameMode.MatchResultManager.GetMatchResult();
                if (matchResult.IsDraw)
                {
                    // go to extra time state if there are no losers, otherwise end the match
                    RelativeStateMachine.ChangeState<MatchExtraTime>();
                    return;
                }
            }
            RelativeStateMachine.ChangeState<MatchEnd>();
        }
    }
}