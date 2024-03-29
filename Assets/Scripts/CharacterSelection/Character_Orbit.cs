﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Orbit with zoom")]
public class Character_Orbit : MonoBehaviour
{
    public Transform ylimit;
    public GameObject Informer;

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float xSpeedMobile = 120.0f;
    public float ySpeedMobile = 120.0f;
    public float scrollrate = 3.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public Vector3 Offset;

    public bool Clip;

    private GameObject TargetGO;

    public float x = 0.0f;
    float y = 0.0f;

    private Vector3 DefaultPosition;
    private Quaternion DefaultRotation;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        DefaultPosition = transform.position;
        DefaultRotation = transform.rotation;
    }
    private Vector3 GetTarget()
    {
        Transform t = target;
        Vector3 dest = t.position + Offset;
        return dest;
    }
    void LateUpdate()
    {
        if(!Informer.activeSelf)
        {
            transform.position = DefaultPosition;
            transform.rotation = DefaultRotation;
            return;
        }
        Vector3 tgt = GetTarget();
#if UNITY_STANDALONE
        if (Input.GetMouseButton(0) && Input.mousePosition.y > ylimit.position.y)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        }
#elif (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0 && Input.GetTouch(0).position.y > ylimit.position.y)
        {
            x += Input.GetTouch(0).deltaPosition.x * xSpeedMobile * Time.deltaTime;
            y -= Input.GetTouch(0).deltaPosition.y * ySpeedMobile * Time.deltaTime;
        }
#else
        throw new UnityException("Input not handled for current platform");
#endif
        y = ClampAngle(y, yMinLimit, yMaxLimit);


        Quaternion rotation = Quaternion.Euler(y, x, 0);

        //distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * scrollrate, distanceMin, distanceMax);

        if (Clip)
        {
            RaycastHit hit;
            if (Physics.Linecast(tgt, transform.position, out hit))
            {
                distance -= hit.distance;
            }
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + tgt;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
