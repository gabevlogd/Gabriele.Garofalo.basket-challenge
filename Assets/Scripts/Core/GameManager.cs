using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasketChallenge.Core
{
    /// <summary>
    /// The GameManager class is responsible for managing the overall game state, handling scene transitions, and providing global access to game-related functionality.
    /// It ensures that there is only one instance of the GameManager throughout the entire game lifecycle, making it a persistent singleton.
    /// The GameManager initializes itself before the first scene is loaded, allowing it to manage game state and handle scene transitions effectively.
    /// It also manages the current GameMode, which defines the rules and mechanics of the game for each scene.
    /// </summary>
    public class GameManager : PersistentSingleton<GameManager>
    {
        public GameModeBase CurrentGameMode { get; private set; }
        
        /// <summary>
        /// Initializes the GameManager instance before any scene is loaded. This ensures that the GameManager is available
        /// throughout the entire game lifecycle, allowing it to manage game state, handle scene transitions,
        /// and provide global access to game-related functionality.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log("Game Manager initializing...");
            
            GameObject gameManagerObject = new GameObject("GameManager");
            gameManagerObject.AddComponent<GameManager>();
            
            SceneManager.sceneLoaded -= Instance.OnSceneLoaded;
            SceneManager.sceneLoaded += Instance.OnSceneLoaded;
            
            SceneManager.sceneUnloaded -= Instance.OnSceneUnloaded;
            SceneManager.sceneUnloaded += Instance.OnSceneUnloaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeGameMode();
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            CurrentGameMode = null;
        }

        private bool InitializeGameMode()
        {
            GameModeClass gameModeClass = null;
            
            // First, try to find a GameMode specified in the scene's SceneConfig
            SceneConfig sceneConfig = FindObjectOfType<SceneConfig>();
            if (sceneConfig != null && sceneConfig.sceneConfigData != null && sceneConfig.sceneConfigData.IsValid())
            {
                gameModeClass = sceneConfig.sceneConfigData.gameModeClass;
            }
            else
            {
                // If no GameMode is specified in the scene, try to load a default GameMode from resources
                GameModeClass defaultGameMode = Resources.Load<GameModeClass>("DefaultGameMode");
                if (defaultGameMode != null && defaultGameMode.IsValid())
                {
                    gameModeClass = defaultGameMode;
                }
            }

            // If we found a valid GameMode, initialize it and set it as the current GameMode
            if (gameModeClass != null)
            {
                CurrentGameMode = gameModeClass.CreateGameMode();
                return true;
            }
            
            Debug.LogError("No valid GameMode found for the current scene, and no default GameMode is available.");
            return false;
        }

    }
}
