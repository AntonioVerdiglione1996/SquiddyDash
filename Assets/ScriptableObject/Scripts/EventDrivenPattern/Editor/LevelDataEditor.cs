using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    LevelData obj;
    private void OnEnable()
    {
        obj = target as LevelData;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save To File"))
        {
            obj.SaveToFile();
        }
        if (GUILayout.Button("Restore From File"))
        {
            obj.Restore();
        }
    }
}
