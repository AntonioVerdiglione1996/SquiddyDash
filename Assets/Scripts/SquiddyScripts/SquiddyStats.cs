using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats")]
public class SquiddyStats : ScriptableObject
{
    public bool IsPhoneDebugging;
    public float JumpPower;
    public float LandForce;
    public float BouncyPower;

    public Vector3 topRight = new Vector3(1, 1, 0);
    public Vector3 topLeft = new Vector3(-1, 1, 0);

    public Vector3 LessTopRight = new Vector3(0.5f, 0.87f, 0);
    public Vector3 LessTopLeft = new Vector3(-0.5f, 0.87f, 0);

    public Vector3[] RightDirections;
    public Vector3[] LeftDirections;


}
