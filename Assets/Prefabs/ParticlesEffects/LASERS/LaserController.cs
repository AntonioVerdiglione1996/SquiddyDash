using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    public GameObject LaserToCotrol;
    public float MaxTime;
    private float t;
    private void Awake()
    {
        t = MaxTime;
    }
    private void Update()
    {
        t -= Time.deltaTime;
        if (t < 0)
        {
            if (LaserToCotrol != null)
            {
                LaserToCotrol.SetActive(!LaserToCotrol.activeSelf);
            }
            t = MaxTime;
        }
    }
}
