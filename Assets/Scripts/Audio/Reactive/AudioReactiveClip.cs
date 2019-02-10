using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public struct ReactiveClipData
{
    public float Timestamp;
    public byte Strenght;
    public ReactiveClipData(float time, byte strenght)
    {
        Timestamp = time;
        Strenght = strenght;
    }
}
[CreateAssetMenu(menuName = "Audio/ReactiveClip")]
public class AudioReactiveClip : ScriptableObject
{

    public AudioClip Clip;
    public ReactiveClipData[] Timestamps;

    public bool IsValid()
    {
        return Clip;
    }
    public void Validate()
    {
        if (Timestamps == null || Timestamps.Length == 0)
        {
            return;
        }

        Timestamps = Timestamps.OrderBy(f => f.Timestamp).ToArray();

        if (!Clip)
        {
            return;
        }
#if UNITY_EDITOR
        if (Timestamps[Timestamps.Length - 1].Timestamp >= Clip.length)
        {
            Debug.LogErrorFormat("{0} contains one or more timestamps that exceed the lenght of {1} audioclip!!", this, Clip);
        }
#endif
        if (Timestamps[0].Timestamp <= 0f)
        {
            for(int i = 0; i < Timestamps.Length; i++)
            {
                if(Timestamps[i].Timestamp < 0f)
                {
                    Timestamps[i].Timestamp = 0f;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
