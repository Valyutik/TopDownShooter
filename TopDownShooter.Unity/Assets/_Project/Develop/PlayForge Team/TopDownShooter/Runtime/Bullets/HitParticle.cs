using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class HitParticle : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private ObjectPool<HitParticle> _pool;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
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