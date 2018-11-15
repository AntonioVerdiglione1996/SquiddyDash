using UnityEngine;
using System.Collections;
public class randomParticleRotation_Reworked : MonoBehaviour
{
    ParticleSystem system;
    ParticleSystem.Particle[] particlesToRotate;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
        particlesToRotate = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(particlesToRotate);
    }
    private void Update()
    {
        for (int i = 0; i < particlesToRotate.Length; i++)
        {
            particlesToRotate[i].rotation = Random.Range(0f, 360f);
        }

    }
}