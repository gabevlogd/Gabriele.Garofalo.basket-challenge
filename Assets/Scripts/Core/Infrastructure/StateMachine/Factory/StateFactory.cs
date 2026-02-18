using UnityEngine;

namespace BasketChallenge.Core
{
    public class StateFactory<T> where T : StateBase, new()
    {
        public T CreateState()
        {
            T newState = new();
            return newState;
        }
    }
    
}