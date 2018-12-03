using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent), true)]
public class GameEventEditor : Editor
{
    private GameEvent obj;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Raise"))
        {
            obj.Raise();
        }
        if (GUILayout.Button(obj.LocalDebugActive ? "Local debug active" : "Local debug not active"))
        {
            obj.LocalDebugActive = !obj.LocalDebugActive;
        }
        if (GUILayout.Button(GameEvent.AllDebugActive ? "Debug active" : "Debug not active"))
        {
            GameEvent.AllDebugActive = !GameEvent.AllDebugActive;
        }
    }
    private void OnEnable()
    {
        obj = target as GameEvent;
    }
}
