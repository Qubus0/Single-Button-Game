using Assets;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BulletPatternLoader))]
    public class BulletPatternLoaderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = (BulletPatternLoader) target;
            if (GUILayout.Button("Load All Bullet Patterns"))
                script.LoadBulletPatterns();
        }
    }
}

