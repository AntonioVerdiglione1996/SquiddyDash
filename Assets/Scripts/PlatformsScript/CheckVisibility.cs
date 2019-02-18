using System;
using UnityEngine;

public class CheckVisibility : MonoBehaviour {
    public event Action OnInvisible;
    public event Action OnVisible;
    private void OnBecameInvisible()
    {
        if(OnInvisible != null)
        {
            OnInvisible();
        }
    }
    private void OnBecameVisible()
    {
        if (OnVisible != null)
        {
            OnVisible();
        }
    }
}
