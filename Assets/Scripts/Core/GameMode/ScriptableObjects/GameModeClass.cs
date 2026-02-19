using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "DefaultGameMode", menuName = "ScriptableObjects/GameModes/DefaultGameMode", order = 0)]
    public class GameModeClass : ScriptableObject
    {
        [SerializeField]
        private string _gameModeName = "DefaultGameMode";
        
        public PlayerControllerClass playerControllerClass;
        
        public HUDClass hudClass;
        
        public ControllableClass playerControllableClass;
        
        public virtual GameModeBase CreateGameMode()
        {
            return CreateGameMode<GameModeBase>();
        }
        
        protected T CreateGameMode<T>() where T : GameModeBase
        {
            GameModeFactory<T> factory = new GameModeFactory<T>();
            T newGameMode = factory.CreateGameMode(_gameModeName);
            newGameMode.Init(this);
            return newGameMode;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(_gameModeName) && 
                   playerControllerClass != null &&
                   hudClass != null &&
                   playerControllableClass != null;
        }
    }
}