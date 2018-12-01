using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityLimiter : MonoBehaviour
{

    public Rigidbody Body;
    public float MaxVelocity = 30f;
    public ForceMode SlowingForceMode = ForceMode.VelocityChange;

    private void OnValidate()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        if (Body == null)
        {
            Body = GetComponentInChildren<Rigidbody>(true);
        }
    }
    void FixedUpdate()
    {
        Vector3 velocity = Body.velocity;
        float velocityMagn = velocity.magnitude;
        if (velocityMagn > MaxVelocity)
        {
            Body.AddForce(-velocity.normalized * (velocityMagn - MaxVelocity), SlowingForceMode);
        }
    }
}
