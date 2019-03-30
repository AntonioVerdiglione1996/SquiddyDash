using System.Collections;
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
    protected override IDescriber GetOnCollectedInfo()
    {
        return currentLogic ? currentLogic.Describer : base.GetOnCollectedInfo();
    }
    private void OnDisable()
    {
        if (currentLogic)
        {
            bool isPowerupOverrided = false;

            if (currentLogic.AvailableUpgrades != null)
            {
                for (int i = currentLogic.AvailableUpgrades.Count - 1; i >= 0; i--)
                {
                    Upgrade up = currentLogic.AvailableUpgrades[i];
                    if (up)
                    {
                        if (up.IsPurchased)
                        {
                            isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                            up.ResetPowerup(this, currentLogic);
                        }
                    }
                    else
                    {
                        currentLogic.AvailableUpgrades.RemoveAt(i);
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
        FindRigidbody = false;
        base.OnValidate();
#if UNITY_EDITOR
        DebugPowerupMinIndex = Mathf.Clamp(DebugPowerupMinIndex, 0, PowerUps != null && PowerUps.Length != 0 ? PowerUps.Length : 0);
        DebugPowerupMaxIndex = Mathf.Clamp(DebugPowerupMaxIndex, DebugPowerupMinIndex, PowerUps != null && PowerUps.Length != 0 ? PowerUps.Length : 0);
        for (int j = 0; j < PowerUps.Length; j++)
        {
            PowerUpLogic currentLogic = PowerUps[j];
            if (!currentLogic || currentLogic.AvailableUpgrades == null || currentLogic.AvailableUpgrades.Count == 0)
            {
                continue;
            }
            List<Upgrade> overriders = new List<Upgrade>();
            for (int i = 0; i < currentLogic.AvailableUpgrades.Count; i++)
            {
                Upgrade up = currentLogic.AvailableUpgrades[i];
                if (up && up.OverrideSkill)
                {
                    overriders.Add(up);
                }
            }
            if (overriders.Count > 1)
            {
                Debug.LogErrorFormat("{0} contains {1} overriding upgrades, this may be an undesired state", currentLogic, overriders.Count);
            }
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

            if (currentLogic.AvailableUpgrades != null)
            {
                for (int i = currentLogic.AvailableUpgrades.Count - 1; i >= 0; i--)
                {
                    Upgrade up = currentLogic.AvailableUpgrades[i];
                    if (up)
                    {
                        if (up.IsPurchased)
                        {
                            isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                            up.InitPowerup(this, currentLogic);
                        }
                    }
                    else
                    {
                        currentLogic.AvailableUpgrades.RemoveAt(i);
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

                if (currentLogic.AvailableUpgrades != null)
                {
                    for (int i = currentLogic.AvailableUpgrades.Count - 1; i >= 0; i--)
                    {
                        Upgrade up = currentLogic.AvailableUpgrades[i];
                        if (up)
                        {
                            if (up.IsPurchased)
                            {
                                isPowerupOverrided = isPowerupOverrided || up.OverridePowerup;
                                up.PowerUpCollected(other, this, currentLogic);
                            }
                        }
                        else
                        {
                            currentLogic.AvailableUpgrades.RemoveAt(i);
                        }
                    }
                }

                if (!isPowerupOverrided)
                {
                    currentLogic.PowerUpCollected(other, this);
                }
                //instantiate tigger particle after squiddy pick up PU
                if (currentLogic.ParticleAfterTriggerPrefab != null)
                {
                    Spawner.SpawnPrefab(null, currentLogic.ParticleAfterTriggerPrefab, null, false, transform.position, Quaternion.identity);
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
