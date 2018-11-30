using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class Character_Orbit_Mobile : MonoBehaviour
{
    public Transform GuiLimitYPos;

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 2.0f;
    public float ySpeed = 2.0f;
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

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
    private Vector3 GetTarget()
    {
        Transform t = target;
        Vector3 dest = t.position + Offset;
        return dest;
    }
    void LateUpdate()
    {
        Vector3 tgt = GetTarget();
        if (Input.touchCount > 0 && Input.touches[0].position.y > GuiLimitYPos.position.y)
        {
            x += Input.GetTouch(0).deltaPosition.x * xSpeed/* * distance */ * 0.04f;
            y -= Input.GetTouch(0).deltaPosition.y * ySpeed * 0.02f;
        }
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