using BasketChallenge.Core;

namespace BasketChallenge.Gameplay
{
    public class MatchDifficultyChoice : MatchState
    {
        
        protected override void OnEnter()
        {
            base.OnEnter();
            DifficultyManager.OnDifficultyLevelChanged += HandleDifficultyLevelChanged;
        }

        protected override void OnExit(StateBase nextState)
        {
            base.OnExit(nextState);
            DifficultyManager.OnDifficultyLevelChanged -= HandleDifficultyLevelChanged;
        }

        private void HandleDifficultyLevelChanged(AIBehaviourData obj)
        {
            RelativeStateMachine.ChangeState<MatchStart>();
        }
    }
}