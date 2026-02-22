using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "StateMachine", menuName = "ScriptableObjects/StateMachine", order = 0)]
    public class StateMachineData : ScriptableObject
    {
        public StateClass entryState;
        public List<StateClass> states;
    }
}