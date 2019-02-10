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
    public bool DebugEnabled = false;
    private void OnValidate()
    {
        if (!Walls)
        {
            Walls = GetComponentInParent<Walls>();
        }
        if (!Collider)
        {
            Collider = GetComponent<BoxCollider>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Walls.OnCollisionEntered(collision, this);
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogWarningFormat("{0} of type {1} called oncollisionenter at {2} with {3}", this, Type, Time.time, collision.gameObject);
        }
#endif
    }
    private void OnTriggerStay(Collider other)
    {
        Walls.OnTriggerStaying(other, this);
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogWarningFormat("{0} of type {1} called ontriggerstay at {2} with {3}", this, Type, Time.time, other.gameObject);
        }
#endif
    }
}