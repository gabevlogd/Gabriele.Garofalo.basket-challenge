using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "SceneConfigData", menuName = "ScriptableObjects/SceneConfigData", order = 0)]
    public class SceneConfigData : ScriptableObject
    {
        //public GameModeBase gameModePrefab;
        public GameModeClass gameModeClass;
        
        public bool IsValid()
        {
            return gameModeClass != null;
        }
    }
}