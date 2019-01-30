using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialChanger))]
public class MaterialChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Change"))
        {
            ((MaterialChanger)target).ChangeMaterial();
        }
        EditorGUI.EndDisabledGroup();
    }

}
