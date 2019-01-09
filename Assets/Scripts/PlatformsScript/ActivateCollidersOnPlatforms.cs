using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollidersOnPlatforms : MonoBehaviour
{
    public Platform Platform;
    private void OnTriggerEnter(Collider collider)
    {
        // 8 = squiddy
        if (collider.gameObject.layer == 8)
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
