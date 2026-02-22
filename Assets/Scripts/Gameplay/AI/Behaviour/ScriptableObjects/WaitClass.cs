using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "Wait", menuName = "ScriptableObjects/AIBehaviours/Wait", order = 0)]
    public class WaitClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<Wait>();
        }
    }
}