using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchExtraTime", menuName = "States/MatchStates/MatchExtraTime", order = 0)]
    public class MatchExtraTimeClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchExtraTime>();
        }
    }
}