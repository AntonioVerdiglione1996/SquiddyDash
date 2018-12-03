using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class PowerUp : MonoBehaviour
{
    public Transform[] SpawnPoints;

    public PowerUpLogic[] PowerUps;

    [SerializeField]
    private MeshRenderer powRenderer;
    [SerializeField]
    private MeshFilter powMeshFilter;

    private PowerUpLogic currentLogic;
    private void OnValidate()
    {
        if(powMeshFilter == null)
        {
            powMeshFilter = GetComponent<MeshFilter>();
            powMeshFilter.mesh = null;
        }
        if (powRenderer == null)
        {
            powRenderer = GetComponent<MeshRenderer>();
            powRenderer.materials = null;
        }
    }
    public void ResetState()
    {
        if (currentLogic)
        {
            currentLogic.ResetPowerup();
        }
        if (SpawnPoints != null && SpawnPoints.Length > 0)
        {
            int random = Random.Range(0, SpawnPoints.Length);
            transform.position = SpawnPoints[random].position;
        }
        if (PowerUps != null && PowerUps.Length > 0)
        {
            int random = Random.Range(0, PowerUps.Length);
            currentLogic = PowerUps[random];
            powMeshFilter.mesh = currentLogic.Mesh;
            powRenderer.materials = currentLogic.Materials;
        }
        if (currentLogic)
        {
            currentLogic.InitPowerup(this);
        }
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        //squiddy layer
        if (other.gameObject.layer == 8)
        {
            if (currentLogic)
            {
                currentLogic.PowerUpCollected(other, this);
            }

            gameObject.SetActive(false);
        }
    }
}
