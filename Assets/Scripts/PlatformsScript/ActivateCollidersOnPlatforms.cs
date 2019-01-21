using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollidersOnPlatforms : MonoBehaviour
{
    public Platform Platform;
    private Transform myTransform;
    private void Awake()
    {
        myTransform = transform;
    }
    private void OnTriggerStay(Collider collider)
    {
        OnTriggerEnter(collider);
    }
    private void OnTriggerEnter(Collider collider)
    {
        // 8 = squiddy
        if (collider.gameObject.layer == 8 && Vector3.Dot(collider.attachedRigidbody.velocity.normalized, myTransform.up) <= 0f)
        {
            Platform.ActivateCollisions();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        //8 is squiddy
        if (collider.gameObject.layer == 8)
        {
            Platform.DeactivateCollisions();
        }
    }
}
