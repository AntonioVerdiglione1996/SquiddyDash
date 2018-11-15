using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollidersOnPlatforms : MonoBehaviour
{
    public GameObject PlatformParent;
    private void OnTriggerEnter(Collider collider)
    {
        // 8 = squiddy
        if (collider.gameObject.layer == 8)
        {
            PlatformParent.GetComponent<Collider>().enabled = true;
            //setting current platform 
            CurrentPlatformForSquiddy.CurrentPlatform = PlatformParent.GetComponent<Platform>();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        //8 is squiddy
        if (collider.gameObject.layer == 8)
        {
            PlatformParent.GetComponent<Collider>().enabled = false;
        }
    }
}
