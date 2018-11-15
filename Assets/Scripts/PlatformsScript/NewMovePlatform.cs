using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platform))]
public class NewMovePlatform : MonoBehaviour
{
    public float Speed;

    private Camera MainCamera;

    private Vector3[] possibleDirections;
    private Platform platform;

    Vector3 dir;
    private void Awake()
    {
        MainCamera = Camera.main;
        platform = GetComponent<Platform>();
        possibleDirections = new Vector3[] { Vector3.right, -Vector3.right };
        dir = DirectionVector();
        Speed = Random.Range(5f, 9f);
    }


    private void Update()
    {

        transform.position += dir * Speed * Time.deltaTime;
        //la piattaforma sta toccando con il lato destro il muro destro
        if (transform.position.x + (transform.localScale.x * 0.5f) >= MainCamera.orthographicSize * MainCamera.aspect)
        {
            dir = -Vector3.right;

            platform.DirRight = false;
            platform.DirLeft = true;

        }
        //la piattaforma sta toccando con il lato sinistro il muro sinistro
        if (transform.position.x - (transform.localScale.x * 0.5f) <= -MainCamera.orthographicSize * MainCamera.aspect)
        {
            dir = Vector3.right;

            platform.DirRight = true;
            platform.DirLeft = false;
        }
    }
    private Vector3 DirectionVector()
    {
        return possibleDirections[(int)Random.Range(0, 2)];
    }
}
