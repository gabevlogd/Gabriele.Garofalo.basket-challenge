using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MatchExtraTime : MatchState
    {
        private float _duration;
        
        protected override void OnEnter()
        {
            base.OnEnter();
            MatchManager.StartMatch();
            _duration = MatchManager.matchData.extraTimeDuration;
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
                TryGoToNextState();
            }
        }
        
        private void TryGoToNextState()
        {
            if (CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode))
            {
                MatchResult matchResult = gameplayGameMode.MatchResultManager.GetMatchResult();
                if (matchResult.IsDraw)
                {
                    // reset the duration for another extra time if there are still no losers, otherwise end the match
                    _duration = MatchManager.matchData.extraTimeDuration;
                    return;
                }
            }
            RelativeStateMachine.ChangeState<MatchEnd>();
        }
    }
}