using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platform))]
public class NewMovePlatform : MonoBehaviour
{
    public static float SpeedMultiplier = 1f;

    public float Speed;
    public float InitialSpeed { get; private set; }
    public Bounds CollisionBounds;

    private Camera MainCamera;

    private Vector3[] possibleDirections;
    private Platform platform;

    Transform root;
    Vector3 dir;
    private void Awake()
    {
        root = transform.root;
        MainCamera = Camera.main;
        platform = GetComponent<Platform>();
        possibleDirections = new Vector3[] { Vector3.right, -Vector3.right };
        dir = DirectionVector();
        Speed = Random.Range(5f, 9f);
        InitialSpeed = Speed;
    }

    private void Update()
    {

        root.position += dir * Speed * Time.deltaTime * SpeedMultiplier;
        //la piattaforma sta toccando con il lato destro il muro destro
        Bounds cameraBounds = Utils.GetCameraBounds(MainCamera);
        Bounds platBounds = new Bounds(root.position, CollisionBounds.size);
        if (platBounds.center.x + platBounds.extents.x >= cameraBounds.center.x + cameraBounds.extents.x)
        {
            dir = -Vector3.right;

            platform.DirRight = false;
            platform.DirLeft = true;

        }
        //la piattaforma sta toccando con il lato sinistro il muro sinistro
        if (platBounds.center.x - platBounds.extents.x <= cameraBounds.center.x - cameraBounds.extents.x)
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
