using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Audio/ReactiveClip")]
public class AudioReactiveClip : ScriptableObject
{

    public AudioClip Clip;
    public float[] Timestamps;

    public bool IsValid()
    {
        return Clip;
    }
    public void OnValidate()
    {
        if (Timestamps == null || Timestamps.Length == 0)
        {
            return;
        }

        Timestamps = Timestamps.OrderBy(f => f).ToArray();

        if (!Clip)
        {
            return;
        }
#if UNITY_EDITOR
        if (Timestamps[Timestamps.Length - 1] >= Clip.length)
        {
            Debug.LogErrorFormat("{0} contains one or more timestamps that exceed the lenght of {1} audioclip!!", this, Clip);
        }
#endif
        if (Timestamps[0] <= 0f)
        {
            for(int i = 0; i < Timestamps.Length; i++)
            {
                if(Timestamps[i] < 0f)
                {
                    Timestamps[i] = 0f;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
