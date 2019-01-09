using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerpBackgroundColor : MonoBehaviour
{
    public Gradient color1;
    public float multiplier = 0.1f;
    //public Gradient color2;
    public float duration = 3.0F;

    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void Update()
    {

        float t = Mathf.PingPong(Time.time * multiplier, duration) / duration;
        cam.backgroundColor = color1.Evaluate(t);
    }
}
