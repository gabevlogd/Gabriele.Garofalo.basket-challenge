using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MatchStart : MatchState
    {
        private float _startDelay;
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _startDelay = MatchManager.matchData.startDelay;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (_startDelay > 0)
            {
                _startDelay -= Time.deltaTime;
                MatchManager.UpdateMatchStartDelay(_startDelay);
            }
            else
            {
                RelativeStateMachine.ChangeState<MatchExecution>();
            }
        }
    }
}