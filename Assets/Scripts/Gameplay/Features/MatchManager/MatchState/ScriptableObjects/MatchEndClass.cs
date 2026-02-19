using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchEnd", menuName = "States/MatchStates/MatchEnd", order = 0)]
    public class MatchEndClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchEnd>();
        }
    }
}