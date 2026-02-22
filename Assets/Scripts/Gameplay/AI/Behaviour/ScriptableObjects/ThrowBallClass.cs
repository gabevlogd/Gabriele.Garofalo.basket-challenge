using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "ThrowBall", menuName = "ScriptableObjects/AIBehaviours/ThrowBall", order = 0)]
    public class ThrowBallClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<ThrowBall>();
        }
    }
}