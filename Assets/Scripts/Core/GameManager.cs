using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasketChallenge.Core
{
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
            GameModeBase gameMode = null;
            
            // First, try to find a GameMode specified in the current scene's SceneMap
            SceneConfig sceneConfig = FindObjectOfType<SceneConfig>();
            if (sceneConfig != null && sceneConfig.sceneConfigData != null && sceneConfig.sceneConfigData.IsValid())
            {
                gameMode = sceneConfig.sceneConfigData.gameModePrefab;
            }
            else
            {
                // If no GameMode is specified in the scene, try to load a default GameMode from resources
                SceneConfigData defaultSceneData = Resources.Load<SceneConfigData>("DefaultSceneData");
                if (defaultSceneData != null && defaultSceneData.IsValid())
                {
                    gameMode = defaultSceneData.gameModePrefab;
                }
            }

            // If we found a valid GameMode, instantiate and initialize it
            if (gameMode != null)
            {
                CurrentGameMode = Instantiate(gameMode);
                return CurrentGameMode.Init();
            }
            
            Debug.LogError("No valid GameMode found for the current scene, and no default GameMode is available.");
            return false;
        }

    }
}
