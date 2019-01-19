using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AudioReactiveClip))]
public class AudioReactiveClipEditor : Editor {

    AudioReactiveClip clip;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Validate data"))
        {
            clip.Validate();
        }
    }
    private void OnEnable()
    {
        clip = target as AudioReactiveClip;
    }
}
