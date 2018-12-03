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
            GlobalEvents.ParentToTarget(transform.root, collision.transform.root);
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
        }
    }
}
