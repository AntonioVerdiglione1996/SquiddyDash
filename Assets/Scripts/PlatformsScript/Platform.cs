using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool DirRight;

    public bool DirLeft;

    public bool IsLanded = false;

    public GlobalEvents GlobalEvents;
    public BasicEvent PerformLerp;
    public BasicEvent PlatformClaimed;
    public ScoreSystem ScoreSystem;

    public SOPool Pool;
    public BasicEvent PlatformRecycled;
    public GameObject Poolable;

    public int ScoreValue = 1;

    public bool IsAlreadyUpdatedScore = false;

    public Collider PlatCollider;

    public Renderer[] ModifiableRenderers;

    private Transform squiddy;

    public void SetMaterial(Material NewMat)
    {
        if (ModifiableRenderers != null)
        {
            for (int i = 0; i < ModifiableRenderers.Length; i++)
            {
                ModifiableRenderers[i].material = NewMat;
            }
        }
    }
    private void Start()
    {
        if (!squiddy)
        {
            SquiddyController controller = FindObjectOfType<SquiddyController>();
            if (controller)
            {
                squiddy = controller.transform;
            }
        }
    }
    private void OnDisable()
    {
        IsAlreadyUpdatedScore = false;
    }
    public void ActivateCollisions()
    {
        if (PlatCollider)
        {
            PlatCollider.enabled = true;
        }
    }
    public void DeactivateCollisions()
    {
        if (!IsLanded && PlatCollider)
        {
            PlatCollider.enabled = false;
        }
    }
    private void OnBecameInvisible()
    {
        if (!Poolable)
        {
            Poolable = gameObject;
        }
        Start();
        if (squiddy && Pool && squiddy.position.y > transform.position.y)
        {
            Pool.Recycle(Poolable);
            if (PlatformRecycled)
            {
                PlatformRecycled.Raise();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (PerformLerp)
        {
            PerformLerp.Raise();
        }
        IsLanded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        //is squiddy 
#if UNITY_EDITOR
        if (!IsLanded)
        {
            Debug.LogFormat("{0} oncollisionstay called after the oncollisionexit");
        }
#endif
        if (collision.gameObject.layer == 8 && IsLanded)
        {
            //TODO: rework for deparenting maybe
            if (GlobalEvents.ParentToTarget(transform.root, collision.transform.root))
            {
                CurrentPlatformForSquiddy.CurrentPlatform = this;
            }

            //ci entra solo per un frame
            if (!IsAlreadyUpdatedScore)
            {
                ScoreSystem.UpdateScore(ScoreValue);
                IsAlreadyUpdatedScore = true;
                if (PlatformClaimed)
                {
                    PlatformClaimed.Raise();
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            IsLanded = false;
            GlobalEvents.ParentToTarget(null, collision.transform);
            CurrentPlatformForSquiddy.CurrentPlatform = null;
            DeactivateCollisions();
        }
    }
}
