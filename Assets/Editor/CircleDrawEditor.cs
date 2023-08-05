using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CircleDraw))]
    public class CircleDrawEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = (CircleDraw) target;
            if (GUILayout.Button("Draw Circle"))
                script.Draw();
        }
    }
}

