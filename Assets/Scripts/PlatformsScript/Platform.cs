using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ISOPoolable
{
    public const float HeightTolleranceForDespawn = 5f;
    public bool DebugEnabled = false;
    public bool DirRight;

    public bool DirLeft;

    public bool IsLanded = false;

    public LinkedListNode<Platform> Node;

    public GlobalEvents GlobalEvents;
    public BasicEvent PerformLerp;
    public BasicEvent PlatformClaimed;
    public BasicEvent OnLanded;
    public BasicEvent OnLeft;
    public ScoreSystem ScoreSystem;

    public BasicEvent PlatformRecycled;

    public bool IsVisible { get; private set; }

    public int ScoreValue = 1;

    public bool IsAlreadyUpdatedScore = false;

    public Collider PlatCollider;

    public Renderer[] ModifiableRenderers;

    public CheckVisibility Visibility;

    private Transform squiddy;

    private NewMovePlatform mover;

    public Bounds GetBounds()
    {
        return !mover ? new Bounds(transform.position, transform.lossyScale) : mover.CollisionBounds;
    }
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
    private void OnValidate()
    {
        if (!Visibility)
        {
            Visibility = GetComponent<CheckVisibility>();
            if (!Visibility)
            {
                Visibility = GetComponentInChildren<CheckVisibility>(true);
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
        if (!mover)
        {
            mover = GetComponentInChildren<NewMovePlatform>(true);
        }
    }
    private void Update()
    {
        if (!IsVisible)
        {
            OnInvisible();
        }
    }
    private void OnEnable()
    {
        Visibility.OnVisible += OnVisible;
        Visibility.OnInvisible += OnInvisible;
    }
    private void OnDisable()
    {
        if (PlatCollider)
        {
            PlatCollider.enabled = false;
        }
        IsAlreadyUpdatedScore = false;
        Visibility.OnVisible -= OnVisible;
        Visibility.OnInvisible -= OnInvisible;
        if (Node != null && Node.List != null)
        {
            Node.List.Remove(Node);
        }
        Node = null;
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
    private void OnVisible()
    {
        IsVisible = true;
    }
    private void OnInvisible()
    {
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogWarningFormat("{0} in invisible", this);
        }
#endif
        IsVisible = false;
        Start();
        if (squiddy && Pool && squiddy.position.y > transform.position.y + HeightTolleranceForDespawn && CurrentPlatformForSquiddy.CurrentPlatform != this)
        {
            Recycle();
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
            Debug.LogFormat("{0} oncollisionstay called after the oncollisionexit", this);
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
