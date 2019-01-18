using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreSystem), true)]
public class ScoreSystemEditor : Editor
{
    private ScoreSystem obj;
    private int increaseAmount = 1;
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
        if (GUILayout.Button(BasicEvent.AllDebugActive ? "Debug active" : "Debug not active"))
        {
            BasicEvent.AllDebugActive = !BasicEvent.AllDebugActive;
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Increase Score"))
        {
            obj.UpdateScore(increaseAmount);
        }
        increaseAmount = EditorGUILayout.IntField(increaseAmount);

        GUILayout.EndHorizontal();
    }
    private void OnEnable()
    {
        obj = target as ScoreSystem;
    }
}
