using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PowerUp : MonoBehaviour
{
    public Transform[] SpawnPoints;

    public PowerUpLogic[] PowerUps;

    //audio source is needed for playing sound of trigger of each PU
    private AudioSource aSourceForTrigger;
    //audio source is needed for playing sound of vocal  saying name of pu
    private AudioSource aSourceForVocal;

    public GameObject ParticleTrigger;

    public int TriggerAudioSourceIndex = 0;

    public int VocalAudioSourceIndex = 1;

    [SerializeField]
    private MeshRenderer powRenderer;
    [SerializeField]
    private MeshFilter powMeshFilter;

    private PowerUpLogic currentLogic;

    private GameObject powerupInstantiatedObject;

    private void Awake()
    {
        AudioSourceFinder finder = FindObjectOfType<AudioSourceFinder>();
        if (finder)
        {
            aSourceForTrigger = finder.Sources[TriggerAudioSourceIndex];
            aSourceForVocal = finder.Sources[VocalAudioSourceIndex];
        }
    }
    private void OnDisable()
    {
        if (currentLogic)
        {
            currentLogic.ResetPowerup(powerupInstantiatedObject);
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
    }
    private void OnValidate()
    {
        if (powMeshFilter == null)
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
    public void OnEnable()
    {
        if (!currentLogic)
        {
            OnDisable();
        }
        if (currentLogic)
        {
            powerupInstantiatedObject = currentLogic.InitPowerup(this);
        }
        if(powRenderer)
        {
            powRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //squiddy layer
        if (other.gameObject.layer == 8)
        {
            if (currentLogic)
            {
                currentLogic.PowerUpCollected(other, this);
                //instantiate tigger particle after squiddy pick up PU
                int nullObj;
                if (currentLogic.ParticleAfterTriggerPool != null)
                    currentLogic.ParticleAfterTriggerPool.Get(null, transform.position, Quaternion.identity, out nullObj, true);
            }
            //activate sound
            if (currentLogic.TriggerSound != null)
            {
                //trigger sound
                currentLogic.TriggerSound.Play(aSourceForTrigger);
                //vocal trigger sound
                currentLogic.VocalSound.Play(aSourceForVocal);
            }

            if(powerupInstantiatedObject)
            {
                powerupInstantiatedObject.SetActive(false);
            }
            if(powRenderer)
            {
                powRenderer.enabled = false;
            }
        }
    }
}
