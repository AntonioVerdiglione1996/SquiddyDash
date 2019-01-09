using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreSystem), true)]
public class ScoreSystemEditor : Editor
{
    private ScoreSystem obj;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Raise"))
        {
            obj.Raise();
        }
        if (GUILayout.Button("Increase Score"))
        {
            obj.UpdateScore(1);
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
        obj = target as ScoreSystem;
    }
}
