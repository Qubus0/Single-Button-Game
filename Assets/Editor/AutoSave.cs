using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    // ref: https://www.youtube.com/watch?v=B6FtKwX_RS0

    public class AutoSaveEditor
    {
        static AutoSaveEditor()
        {
            EditorApplication.playModeStateChanged += SaveOnPlay;
        }

        private static void SaveOnPlay(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            Debug.Log ("Auto-saving...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}