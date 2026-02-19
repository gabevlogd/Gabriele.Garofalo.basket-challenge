using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchExecution", menuName = "States/MatchStates/MatchExecution", order = 0)]
    public class MatchExecutionClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchExecution>();
        }
    }
}