using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public static ParticleManager _instance;

    [System.Serializable]
    public class Particle
    {
        public ParticleTag tag;

        public ParticleSystem particleSystem;
    }

    public List<Particle> particleList;

    public Dictionary<ParticleTag, ParticleSystem> particles;

    public enum ParticleTag
    {
        dash = 1
    }

    public override void Awake()
    {
        base.Awake();
        _instance = this;
        particles = new Dictionary<ParticleTag, ParticleSystem>();
        foreach (Particle particle in particleList)
        {
            ParticleSystem system =
                Instantiate(particle.particleSystem,
                Vector3.zero,
                particle.particleSystem.transform.rotation);
            particles.Add(particle.tag, system);
        }
    }

    public void PlayParticle(ParticleTag tag, Vector3 position)
    {
        particles[tag].transform.position = position;
        particles[tag].Play();
    }
}
