using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public const float HeightTolleranceForDespawn = 5f;

    public bool DirRight;

    public bool DirLeft;

    public bool IsLanded = false;

    public GlobalEvents GlobalEvents;
    public BasicEvent PerformLerp;
    public BasicEvent PlatformClaimed;
    public BasicEvent OnLanded;
    public BasicEvent OnLeft;
    public ScoreSystem ScoreSystem;

    public SOPool Pool;
    public BasicEvent PlatformRecycled;
    public GameObject Poolable;

    public bool IsVisible { get; private set; }

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
    private void Update()
    {
        if (!IsVisible)
        {
            OnBecameInvisible();
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
    private void OnBecameVisible()
    {
        IsVisible = true;
    }
    private void OnBecameInvisible()
    {
        IsVisible = false;
        if (!Poolable)
        {
            Poolable = gameObject;
        }
        Start();
        if (squiddy && Pool && squiddy.position.y > transform.position.y + HeightTolleranceForDespawn && CurrentPlatformForSquiddy.CurrentPlatform != this)
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
        IsLanded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        //is squiddy 
        //TODO: rework for this oncollisionstay, for some reason it allocates memory for contacts
#if UNITY_EDITOR
        if (!IsLanded)
        {
            Debug.LogFormat("{0} oncollisionstay called after the oncollisionexit");
        }
#endif
        if (collision.gameObject.layer == 8 && IsLanded)
        {
            if (GlobalEvents.ParentToTarget(transform.root, collision.transform.root))
            {
                if (OnLanded)
                {
                    OnLanded.Raise();
                }
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
                if (PerformLerp)
                {
                    PerformLerp.Raise();
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
            if (OnLeft)
            {
                OnLeft.Raise();
            }
        }
    }
}
