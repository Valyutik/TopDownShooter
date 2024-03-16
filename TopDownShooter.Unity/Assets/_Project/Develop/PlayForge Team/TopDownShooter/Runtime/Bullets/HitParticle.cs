using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class HitParticle : MonoBehaviour
    {
        public AudioSource AudioSource => audioSource;
        private ParticleSystem _particleSystem;
        private AudioSource audioSource;
        private ObjectPool<HitParticle> _pool;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetPool(ObjectPool<HitParticle> pool)
        {
            _pool = pool;
        }

        public void Play()
        {
            _particleSystem.Play();
        }

        private void OnDisable()
        {
            _pool.Release(this);
        }
    }
}