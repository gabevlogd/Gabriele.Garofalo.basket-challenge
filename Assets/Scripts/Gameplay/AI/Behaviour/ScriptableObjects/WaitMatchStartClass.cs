using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "WaitMatchStart", menuName = "ScriptableObjects/AIBehaviours/WaitMatchStart", order = 0)]
    public class WaitMatchStartClass : StateClass
    {
        public override StateBase CreateState()
        {
            return CreateState<WaitMatchStart>();
        }
    }
}