using UnityEngine;

namespace BasketChallenge.Core
{
    public class StateClass : ScriptableObject
    {
        public virtual StateBase CreateState()
        {
            return null;
        }

        protected T CreateState<T>() where T : StateBase, new()
        {
            StateFactory<T> factory = new StateFactory<T>();
            return factory.CreateState();
        }
    }
}