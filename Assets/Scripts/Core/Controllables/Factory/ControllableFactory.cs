using UnityEngine;

namespace BasketChallenge.Core
{
    public class ControllableFactory<T> where T : ControllableBase
    {
        public T CreateControllable(string controllableName)
        {
            GameObject instance = new GameObject(controllableName);
            T component = instance.AddComponent<T>();
            return component;
        }
    }
}