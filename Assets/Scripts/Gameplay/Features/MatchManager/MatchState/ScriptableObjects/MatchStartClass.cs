using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchStart", menuName = "ScriptableObjects/States/MatchStates/MatchStart", order = 0)]
    public class MatchStartClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchStart>();
        }
    }
}