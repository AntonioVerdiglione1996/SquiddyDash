using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicEvent), true)]
public class BasicEventEditor : Editor
{
    private BasicEvent obj;
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
        obj = target as BasicEvent;
    }
}
