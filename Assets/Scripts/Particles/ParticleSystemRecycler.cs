using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemRecycler : MonoBehaviour
{
    public SOPool Pool;
    public GameObject Root;
    [SerializeField]
    private bool playWithChildren = true;
    [SerializeField]
    private ParticleSystem system;
    [SerializeField]
    private ParticleSystem[] systems;
    // Use this for initialization
    private void OnValidate()
    {
        if (!Root)
        {
            Root = transform.root.gameObject;
        }
        system = GetComponent<ParticleSystem>();
        if (playWithChildren)
        {
            systems = Root.GetComponentsInChildren<ParticleSystem>();
        }
    }
    void OnEnable()
    {
        system.time = 0f;
        if (playWithChildren && systems != null)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].time = 0f;
            }
        }
        system.Play(playWithChildren);
    }

    // Update is called once per frame
    void Update()
    {
        if (!system.IsAlive(playWithChildren))
        {
            Pool.Recycle(Root);
        }
    }
}
