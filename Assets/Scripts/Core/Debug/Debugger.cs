using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasketChallenge.Core
{
    public class Debugger : PersistentSingleton<Debugger>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitDebugger()
        {
            GameObject debuggerObject = new GameObject("Debugger");
            debuggerObject.AddComponent<Debugger>();
        }

        #region Draw Debug Methods
    
        private List<DrawDebug> _activeDraws = new List<DrawDebug>();
        private List<DrawDebug> _expiredDraws = new List<DrawDebug>();

        private void OnDrawGizmos()
        {
            foreach (DrawDebug debug in _activeDraws)
            {
                debug.OnDrawGizmos();
                if (debug.IsExpired)
                {
                    _expiredDraws.Add(debug);
                }
            }
        
            foreach (DrawDebug expired in _expiredDraws)
            {
                _activeDraws.Remove(expired);
            }
            _expiredDraws.Clear();
        }
    
        public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration = 0f)
        {
            Instance._activeDraws.Add(new DrawDebugSphere(position, radius, color, duration));
        }
    
        #endregion
    }
}