using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chris_FlingPad))]
public class FlingPadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Chris_FlingPad flingPad = (Chris_FlingPad)target;

        if (GUILayout.Button("Fling") && Application.isPlaying)
        {
            flingPad.Fling();
        }
    }
}
