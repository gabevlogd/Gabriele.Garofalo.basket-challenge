using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "Controllable", menuName = "ScriptableObjects/Controllables/Controllable", order = 0)]
    public class ControllableClass : ScriptableObject
    {
        public string controllableName = "Controllable";
        
        public ControllableBase controllablePrefab;
        
        public virtual ControllableBase CreateControllable()
        {
            return CreateControllable<ControllableBase>();
        }
        
        protected T CreateControllable<T>() where T : ControllableBase
        {
            ControllableFactory<T> factory = new ControllableFactory<T>();
            return factory.CreateControllable(controllableName);
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(controllableName);
        }
    }
}