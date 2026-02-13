using UnityEditor;
using UnityEngine;

namespace BasketChallenge.Core
{
    [CustomEditor(typeof(HUDClass), true)]
    public class HUDClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HUDClass data = target as HUDClass;
            
            if (data != null && data.hudPrefab != null) return;

            if (GUILayout.Button("Generate Prefab"))
            {
                GeneratePrefab(data);
            }
        }
        
        private void GeneratePrefab(HUDClass data)
        {
            // 1. Create GameObject
            GameObject go = data.CreateHUD().gameObject;

            // 2. Choose path
            string folderPath = "Assets/GeneratedPrefabs";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "GeneratedPrefabs");
            }

            string path = $"{folderPath}/{data.hudName}.prefab";

            // 3. Save as prefab
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(go, path);

            // 4. Cleanup scene instance
            DestroyImmediate(go);
            
            // 5) Assegna reference nello ScriptableObject (con Undo + dirty)
            Undo.RecordObject(data, "Assign Generated Prefab");
            prefabAsset.TryGetComponent(out data.hudPrefab);
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Prefab saved at: {path}");
        }
    }
}