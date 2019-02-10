using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWindParticleAffector : MonoBehaviour
{
    public ExtendedWindZone Windzone;
    public ParticleSystem Particles;
    public uint MaxNumberOfParticlesAffected = 100;
    public float SingleParticleMass = 2f;

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
        if (!Particles || !Windzone || ParticlesList == null || Mathf.Approximately(Windzone.WindVelocity.sqrMagnitude, 0.0f))
        {
            return;
        }

        int particlesGettedCount = Particles.GetParticles(ParticlesList);

        int particleCount = Mathf.Min(particlesGettedCount, ParticlesList.Length);

        float massMultiplier = SingleParticleMass <= 0f ? 0f : 1f / SingleParticleMass;

        Vector3 velocityChange = Windzone.WindVelocity * Time.deltaTime * massMultiplier;

        for (int i = 0; i < particleCount; i++)
        {
            ParticlesList[i].velocity = velocityChange;
        }

        Particles.SetParticles(ParticlesList, particleCount);
    }
}
