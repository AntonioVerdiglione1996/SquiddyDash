using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField]
    private float multiplier = 1f;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private float minY = 0f;

    [SerializeField]
    private float maxY = 2f;
    void Update()
    {
        float t = Mathf.PingPong(Time.time * multiplier, duration) / duration;
        transform.position = new Vector3(0, Mathf.Lerp(minY, maxY, t), 0);

    }
}
