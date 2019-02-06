using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EWallType
{
    None,
    Left,
    Right,
    Up,
    Down,
}
public class Wall : MonoBehaviour
{
    public EWallType Type;
    public BoxCollider Collider;
    public Walls Walls;
    private void OnValidate()
    {
        if(!Walls)
        {
            Walls = GetComponentInParent<Walls>();
        }
        if(!Collider)
        {
            Collider = GetComponent<BoxCollider>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Walls.OnCollisionEntered(collision, this);
    }
    private void OnTriggerStay(Collider other)
    {    
        Walls.OnTriggerStaying(other, this);
    }
}