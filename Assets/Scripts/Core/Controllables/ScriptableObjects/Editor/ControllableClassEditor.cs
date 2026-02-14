using UnityEditor;
using UnityEngine;

namespace BasketChallenge.Core
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ControllableClass), true)]
    public class ControllableClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ControllableClass data = target as ControllableClass;
            
            if (data != null && data.controllablePrefab != null) return;

            if (GUILayout.Button("Generate Prefab"))
            {
                GeneratePrefab(data);
            }
        }
        
        private void GeneratePrefab(ControllableClass data)
        {
            //Create GameObject
            GameObject go = data.CreateControllable().gameObject;

            //Choose path (ensure folder exists)
            string folderPath = "Assets/GeneratedPrefabs";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "GeneratedPrefabs");
            }
            string path = $"{folderPath}/{data.controllableName}.prefab";

            //Save as prefab
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(go, path);

            //Cleanup scene instance
            DestroyImmediate(go);
            
            //Assign reference in ScriptableObject (with Undo + dirty)
            Undo.RecordObject(data, "Assign Generated Prefab");
            prefabAsset.TryGetComponent(out data.controllablePrefab);
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Prefab saved at: {path}");
        }
    }
#endif
}