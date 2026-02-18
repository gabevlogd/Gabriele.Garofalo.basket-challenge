using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "StateMachine", menuName = "StateMachine", order = 0)]
    public class StateMachineData : ScriptableObject
    {
        public StateClass entryState;
        public List<StateClass> states;
    }
}