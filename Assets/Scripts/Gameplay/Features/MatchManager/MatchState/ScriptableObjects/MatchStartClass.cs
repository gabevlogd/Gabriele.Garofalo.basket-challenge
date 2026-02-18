using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MatchStart", menuName = "States/MatchStates/MatchStart", order = 0)]
    public class MatchStartClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchStart>();
        }
    }
}