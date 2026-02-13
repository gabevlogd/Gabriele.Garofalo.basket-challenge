using UnityEngine;

namespace BasketChallenge.Core
{
    public class GameModeFactory<T> where T : GameModeBase
    {
        public T CreateGameMode(string gameModeName)
        {
            GameObject instance = new GameObject(gameModeName);
            T component = instance.AddComponent<T>();
            return component;
        }
    }
}