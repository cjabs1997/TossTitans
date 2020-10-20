using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameEvent gameEvent = (GameEvent)target;

        if(GUILayout.Button("Raise Event"))
        {
            gameEvent.Raise();
        }
    }
}
