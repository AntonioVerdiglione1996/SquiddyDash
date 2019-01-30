using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePowerUp : MonoBehaviour
{

    public float X;
    public float Y;
    public float Z;

    public float RotationSpeed;

    private void Update()
    {
        transform.Rotate(new Vector3(X, Y, Z) * RotationSpeed * Time.deltaTime);
    }
}
