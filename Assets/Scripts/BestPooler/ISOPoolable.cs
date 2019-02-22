using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISOPoolable : MonoBehaviour
{
    public SOPool Pool;
    public GameObject Root;
    protected virtual void OnValidate()
    {
        if (!Root)
        {
            Root = transform.root.gameObject;
        }
    }
    public void Recycle()
    {
        if (!Root)
        {
#if UNITY_EDITOR
            Debug.LogException(new System.NullReferenceException("ISOPoolable requires a valid reference to a Root obj"));
#endif
            return;
        }
        if (!Pool)
        {
#if UNITY_EDITOR
            Debug.LogException(new System.NullReferenceException("ISOPoolable requires a valid reference to a Pool obj"));
#endif
            return;
        }
        Pool.Recycle(Root);
    }
}
