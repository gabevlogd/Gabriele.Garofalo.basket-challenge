using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class WaitMatchStart : AIBehaviour
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            MatchManager.OnMatchStart += HandleMatchStart;
        }

        protected override void OnExit(StateBase nextState)
        {
            base.OnExit(nextState);
            MatchManager.OnMatchStart -= HandleMatchStart;
        }

        private void HandleMatchStart()
        {
            RelativeStateMachine.ChangeState<ThrowBall>();
        }
    }
}
