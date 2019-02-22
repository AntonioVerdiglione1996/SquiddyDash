using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : ISOPoolable
{
    public event System.Action OnPickupCollected;
    public bool RecycleOnPickup = true;
    public Collider Collider { get { return myCollider; } }
    [SerializeField]
    private Collider myCollider;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (OnPicked(other))
        {
            if(OnPickupCollected != null)
            {
                OnPickupCollected();
            }
            if (RecycleOnPickup)
            {
                Recycle();
            }
        }
    }
    protected abstract bool OnPicked(Collider other);
}
