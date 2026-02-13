using UnityEngine;

namespace BasketChallenge.Core
{
    public class HUDFactory<T> where T : HUD
    {
        public T CreateHUD(string hudName)
        {
            GameObject instance = new GameObject(hudName);
            T component = instance.AddComponent<T>();
            return component;
        }
    }
}