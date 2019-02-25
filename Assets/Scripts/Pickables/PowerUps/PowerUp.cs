﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerUp : Pickable
{
    public Transform[] SpawnPoints;

    public PowerUpLogic[] PowerUps;

    public int DebugPowerupMinIndex = 0;
    public int DebugPowerupMaxIndex = int.MaxValue;

    //audio source is needed for playing sound of trigger of each PU
    private AudioSource aSourceForTrigger;
    //audio source is needed for playing sound of vocal  saying name of pu
    private AudioSource aSourceForVocal;

    public int TriggerAudioSourceIndex = 0;

    public int VocalAudioSourceIndex = 1;

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
                for (int i = Upgrades.Count - 1; i >= 0; i--)
                {
                    Upgrade up = Upgrades[i];
                    if (up)
                    {
                        if (up.IsPowerupUpgradable(currentLogic.GetType(), currentLogic))
                        {
                            isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                            up.ResetPowerup(this, currentLogic);
                        }
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
#if UNITY_EDITOR
            DebugPowerupMinIndex = Mathf.Clamp(DebugPowerupMinIndex, 0, PowerUps.Length);
            DebugPowerupMaxIndex = Mathf.Clamp(DebugPowerupMaxIndex, DebugPowerupMinIndex, PowerUps.Length);
            int random = Random.Range(DebugPowerupMinIndex, DebugPowerupMaxIndex);
#else
            int random = Random.Range(0, PowerUps.Length);
#endif
            currentLogic = PowerUps[random];
        }
    }
    protected override void OnValidate()
    {
        base.OnValidate();
#if UNITY_EDITOR
        DebugPowerupMinIndex = Mathf.Clamp(DebugPowerupMinIndex, 0, PowerUps != null && PowerUps.Length != 0 ? PowerUps.Length : 0);
        DebugPowerupMaxIndex = Mathf.Clamp(DebugPowerupMaxIndex, DebugPowerupMinIndex, PowerUps != null && PowerUps.Length != 0 ? PowerUps.Length : 0);

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
                for (int i = Upgrades.Count - 1; i >= 0; i--)
                {
                    Upgrade up = Upgrades[i];
                    if (up)
                    {
                        if (up.IsPowerupUpgradable(currentLogic.GetType(), currentLogic))
                        {
                            isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                            up.InitPowerup(this, currentLogic);
                        }
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
        if (Collider)
        {
            Collider.enabled = true;
        }
    }

    protected override bool OnPicked(Collider other)
    {
        //squiddy layer
        if (other.gameObject.layer == 8)
        {
            if (currentLogic)
            {
                bool isPowerupOverrided = false;

                if (Upgrades != null)
                {
                    for (int i = Upgrades.Count - 1; i >= 0; i--)
                    {
                        Upgrade up = Upgrades[i];
                        if (up)
                        {
                            if (up.IsPowerupUpgradable(currentLogic.GetType(), currentLogic))
                            {
                                isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                                up.PowerUpCollected(other, this, currentLogic);
                            }
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
                if (currentLogic.ParticleAfterTriggerPool != null)
                {
                    Spawner.SpawnPrefab(null, currentLogic.ParticleAfterTriggerPool.Pool, null, false, transform.position, Quaternion.identity);
                }
                //activate sound
                if (currentLogic.TriggerSound)
                {
                    currentLogic.TriggerSound.Play(aSourceForTrigger);
                }
                if (currentLogic.VocalSound)
                {
                    currentLogic.VocalSound.Play(aSourceForVocal);
                }
            }

            if (powerupInstantiatedObject)
            {
                powerupInstantiatedObject.SetActive(false);
            }
            if (Collider)
            {
                Collider.enabled = false;
            }
            return true;
        }
        return false;
    }
}
