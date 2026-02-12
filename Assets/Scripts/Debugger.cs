using BasketChallenge.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : PersistentSingleton<Debugger>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitDebugger()
    {
        GameObject debuggerObject = new GameObject("Debugger");
        debuggerObject.AddComponent<Debugger>();
    }
    
    [ContextMenu("Load Reward Scene")]
    private void LoadGameplay()
    {
        SceneManager.LoadScene("Reward");
    }
}