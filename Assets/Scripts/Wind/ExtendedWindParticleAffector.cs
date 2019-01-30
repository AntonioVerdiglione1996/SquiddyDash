using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWindParticleAffector : MonoBehaviour
{
    public ExtendedWindZone Windzone;
    public ParticleSystem Particles;
    public uint MaxNumberOfParticlesAffected = 100;

    public ParticleSystem.Particle[] ParticlesList { get; private set; }
    private void Awake()
    {
        SetMaxNumberOfParticlesAffected(MaxNumberOfParticlesAffected);
    }
    public void SetMaxNumberOfParticlesAffected(uint newMaxNumberOfParticlesAffected)
    {
        if (ParticlesList == null || ParticlesList.Length != newMaxNumberOfParticlesAffected)
        {
            MaxNumberOfParticlesAffected = newMaxNumberOfParticlesAffected;

            ParticlesList = new ParticleSystem.Particle[MaxNumberOfParticlesAffected];
        }
    }

    void Update()
    {
        if (!Particles || !Windzone || ParticlesList == null || Mathf.Approximately(Windzone.WindVelocity3.sqrMagnitude, 0.0f))
        {
            return;
        }

        Particles.GetParticles(ParticlesList);

        int particleCount = Mathf.Min(Particles.particleCount, ParticlesList.Length);

        Vector3 velocityChange = Windzone.WindVelocity3 * Time.deltaTime;

        for (int i = 0; i < particleCount; i++)
        {
#if UNITY_EDITOR
            Vector3 velocity = ParticlesList[i].totalVelocity;
#endif
            ParticlesList[i].velocity = ParticlesList[i].totalVelocity + velocityChange;
#if UNITY_EDITOR
            Debug.LogFormat("Particle prev velocity: {0}, new velocity: {1}, velocity change expected: {2}. Velocity correct? {3}.", velocity, ParticlesList[i].totalVelocity, velocityChange, ParticlesList[i].totalVelocity.Approximately(velocity + velocityChange));
#endif
        }

        Particles.SetParticles(ParticlesList, particleCount);
    }
}
