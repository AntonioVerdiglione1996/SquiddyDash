using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AudioReactiveClip))]
public class AudioReactiveClipEditor : Editor
{

    AudioReactiveClip clip;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Validate data"))
        {
            clip.Validate();
        }
        if (GUILayout.Button("Create debug data"))
        {
            uint size = (uint)clip.Clip.length;
            clip.Timestamps = new ReactiveClipData[size];
            for (int i = 0; i < size; i++)
            {
                clip.Timestamps[i] = new ReactiveClipData(i, (byte)(UnityEngine.Random.Range(byte.MinValue, byte.MaxValue + 1) / 15));
            }
            clip.Validate();
        }

        if (GUILayout.Button("Clear data"))
        {
            clip.Timestamps = new ReactiveClipData[0];
        }
    }
    private void OnEnable()
    {
        clip = target as AudioReactiveClip;
    }
}
