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
    private float minY = -0.5f;

    [SerializeField]
    private float maxY = 1f;

    private float initialY;

    private void Start()
    {
        initialY = transform.position.y;
    }
    void Update()
    {
        float t = Mathf.PingPong(Time.time * multiplier, duration) / duration;
        transform.position = new Vector3(transform.position.x, initialY + Mathf.Lerp(minY, maxY, t), transform.position.z);

    }
}
