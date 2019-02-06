using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float speed = 1f;
    Vector3 newPos;
    void Update()
    {
        newPos = Vector3.up * speed * Time.deltaTime;
        if (transform.position.y > 15f)
        {
            transform.position = new Vector3(transform.position.x,0, transform.position.z);
        }
        transform.position += newPos;
    }
}
