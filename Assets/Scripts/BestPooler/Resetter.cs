using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    public float LifeTime = 1f;

    private float t;
    private void Start()
    {
        t = LifeTime;
    }
    private void Update()
    {
        t -= Time.deltaTime;
        if (t < 0f)
        {
            disactive();
            t = LifeTime;
        }
    }
    private void disactive()
    {
        ObjectPooler.Recycle(gameObject);
    }
   
}
