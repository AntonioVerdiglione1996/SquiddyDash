using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosCancel : MonoBehaviour
{
    public bool EnableDebug = false;
    public Transform Gui;

    private void Update()
    {
#if UNITY_EDITOR
        if (EnableDebug)
        {
            Debug.Log(Gui.position);
            Debug.Log(Screen.width);
            Debug.Log(Screen.height);
        }
#endif
    }
}
