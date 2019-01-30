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

        int particlesGettedCount = Particles.GetParticles(ParticlesList);

        int particleCount = Mathf.Min(particlesGettedCount, ParticlesList.Length);

        Vector3 velocityChange = Windzone.WindVelocity3 * Time.deltaTime;

        for (int i = 0; i < particleCount; i++)
        {
            ParticlesList[i].velocity = ParticlesList[i].totalVelocity + velocityChange;
        }

        Particles.SetParticles(ParticlesList, particleCount);
    }
}
