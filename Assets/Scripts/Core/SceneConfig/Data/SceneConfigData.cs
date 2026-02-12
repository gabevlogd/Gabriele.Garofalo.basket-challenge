using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "SceneConfigData", menuName = "SceneConfigData", order = 0)]
    public class SceneConfigData : ScriptableObject
    {
        public GameModeBase gameModePrefab;
        
        public bool IsValid()
        {
            return gameModePrefab != null;
        }
    }
}