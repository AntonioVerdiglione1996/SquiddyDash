using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool DirRight;

    public bool DirLeft;

    public bool IsLanded;

    public GlobalEvents GlobalEvents;
    public GameEvent PerformLerp;
    public ScoreSystem ScoreSystem;

    public int ScoreValue = 1;

    public bool IsAlreadyUpdatedScore = false;

    public Collider PlatCollider;

    public void ActivateCollisions()
    {
        PlatCollider.enabled = true;
    }
    public void DeactivateCollisions()
    {
        PlatCollider.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        PerformLerp.Raise();
    }
    private void OnCollisionStay(Collision collision)
    {
        //is squiddy 
        if (collision.gameObject.layer == 8)
        {
            IsLanded = true;
            //TODO: rework for deparenting
            GlobalEvents.ParentToTarget(transform.root, collision.transform.root);
            CurrentPlatformForSquiddy.CurrentPlatform = this;

            //ci entra solo per un frame
            if (!IsAlreadyUpdatedScore)
            {
                ScoreSystem.UpdateScore(ScoreValue);
                IsAlreadyUpdatedScore = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            IsLanded = false;
            GlobalEvents.ParentToTarget(null, collision.transform);
            CurrentPlatformForSquiddy.CurrentPlatform =null;
        }
    }
}
