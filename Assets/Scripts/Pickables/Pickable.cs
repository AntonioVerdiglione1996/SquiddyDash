using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : ISOPoolable
{
    public event System.Action OnPickupCollected;
    public bool RecycleOnPickup = true;
    public bool RecycleOnSafetyLayer = true;
    public int SafetyLayerRecycler = 9;
    public Collider Collider { get { return myCollider; } }
    public Rigidbody Body { get { return body; } }
    public bool FindRigidbody = true;
    [SerializeField]
    private Collider myCollider;
    [SerializeField]
    private Rigidbody body;
    protected override void OnValidate()
    {
        base.OnValidate();
        if (!myCollider)
        {
            myCollider = GetComponent<Collider>();
            if (!myCollider)
            {
                myCollider = GetComponentInChildren<Collider>(true);
            }
        }
        if (FindRigidbody && !body)
        {
            body = GetComponent<Rigidbody>();
            if (!body)
            {
                body = GetComponentInChildren<Rigidbody>(true);
            }
        }
        if (myCollider)
        {
            myCollider.isTrigger = true;
        }
        if (body)
        {
            body.isKinematic = true;
            body.useGravity = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        bool recycle = RecycleOnSafetyLayer && other.gameObject.layer == SafetyLayerRecycler;
        if (OnPicked(other))
        {
            if (OnPickupCollected != null)
            {
                OnPickupCollected();
            }
            recycle = recycle || RecycleOnPickup;
        }
        if (recycle)
        {
            Recycle();
        }
    }
    protected abstract bool OnPicked(Collider other);
}
