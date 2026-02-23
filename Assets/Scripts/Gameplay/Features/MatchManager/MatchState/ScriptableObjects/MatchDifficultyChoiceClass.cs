using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchDifficultyChoice", menuName = "ScriptableObjects/States/MatchStates/MatchDifficultyChoice", order = 0)]
    public class MatchDifficultyChoiceClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<MatchDifficultyChoice>();
        }
    }
}