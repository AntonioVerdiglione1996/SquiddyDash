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
    public GameEvent ScoreUpdater;

    public bool IsAlreadyUpdatedScore;
    private void Awake()
    {
        IsAlreadyUpdatedScore = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        //is squiddy 
        if (collision.gameObject.layer == 8)
        {
            IsLanded = true;
            GlobalEvents.ParentToTarget(transform, collision.transform);
            PerformLerp.Raise();
            //ci entra solo per un frame
            if (!IsAlreadyUpdatedScore)
            {
                ScoreUpdater.Raise();
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
            return;
        }
    }
}
