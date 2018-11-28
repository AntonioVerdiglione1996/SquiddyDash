using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsModifier : MonoBehaviour
{
    public Vector3 NewMultiplier = new Vector3(0.8f, 0.8f, 0f);
    private Walls walls;
    private Vector3 prevMultiplier;
    // Use this for initialization
    void Awake()
    {
        walls = FindObjectOfType<Walls>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        prevMultiplier = walls.RepulsionMultiplier;
        walls.RepulsionMultiplier = NewMultiplier;
    }
    private void OnDisable()
    {
        walls.RepulsionMultiplier = prevMultiplier;
    }
}
