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
    [SerializeField]
    private Collider coll;

    public List<Upgrade> Upgrades = new List<Upgrade>();

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
            bool isPowerupOverrided = false;

            if (Upgrades != null)
            {
                for (int i = Upgrades.Count - 1; i > 0; i--)
                {
                    Upgrade up = Upgrades[i];
                    if (up)
                    {
                        isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                        up.ResetPowerup(this, currentLogic);
                    }
                    else
                    {
                        Upgrades.RemoveAt(i);
                    }
                }
            }

            if (!isPowerupOverrided)
            {
                currentLogic.ResetPowerup(powerupInstantiatedObject);
            }
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
        if (!coll)
        {
            coll = GetComponent<Collider>();
        }
#if UNITY_EDITOR
        if (Upgrades == null || Upgrades.Count == 0)
        {
            return;
        }
        List<Upgrade> overriders = new List<Upgrade>();
        for (int i = 0; i < Upgrades.Count; i++)
        {
            Upgrade up = Upgrades[i];
            if (up && up.OverrideSkill)
            {
                overriders.Add(up);
            }
        }
        if (overriders.Count > 1)
        {
            Debug.LogErrorFormat("{0} contains {1} overriding upgrades, this may be an undesired state", this, overriders.Count);
        }
#endif
    }
    public void OnEnable()
    {
        if (!currentLogic)
        {
            OnDisable();
        }
        if (currentLogic)
        {
            bool isPowerupOverrided = false;

            if (Upgrades != null)
            {
                for (int i = Upgrades.Count - 1; i > 0; i--)
                {
                    Upgrade up = Upgrades[i];
                    if (up)
                    {
                        isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                        up.InitPowerup(this, currentLogic);
                    }
                    else
                    {
                        Upgrades.RemoveAt(i);
                    }
                }
            }

            if (!isPowerupOverrided)
            {
                powerupInstantiatedObject = currentLogic.InitPowerup(this);
            }
        }
        if (powRenderer)
        {
            powRenderer.enabled = true;
        }
        if (coll)
        {
            coll.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //squiddy layer
        if (other.gameObject.layer == 8)
        {
            if (currentLogic)
            {
                bool isPowerupOverrided = false;

                if (Upgrades != null)
                {
                    for (int i = Upgrades.Count - 1; i > 0; i--)
                    {
                        Upgrade up = Upgrades[i];
                        if (up)
                        {
                            isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                            up.PowerUpCollected(other, this, currentLogic);
                        }
                        else
                        {
                            Upgrades.RemoveAt(i);
                        }
                    }
                }

                if (!isPowerupOverrided)
                {
                    currentLogic.PowerUpCollected(other, this);
                }
                //instantiate tigger particle after squiddy pick up PU
                int nullObj;
                if (currentLogic.ParticleAfterTriggerPool != null)
                    currentLogic.ParticleAfterTriggerPool.Get(null, transform.position, Quaternion.identity, out nullObj, true);
                //activate sound
                if (currentLogic.TriggerSound != null)
                {
                    //trigger sound
                    currentLogic.TriggerSound.Play(aSourceForTrigger);
                    //vocal trigger sound
                    currentLogic.VocalSound.Play(aSourceForVocal);
                }
            }

            if (powerupInstantiatedObject)
            {
                powerupInstantiatedObject.SetActive(false);
            }
            if (powRenderer)
            {
                powRenderer.enabled = false;
            }
            if (coll)
            {
                coll.enabled = false;
            }
        }
    }
}
