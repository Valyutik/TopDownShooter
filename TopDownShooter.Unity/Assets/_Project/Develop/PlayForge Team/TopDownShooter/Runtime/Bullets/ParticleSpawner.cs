using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private HitParticle particlePrefab;
        public ObjectPool<HitParticle> Pool { get; private set; }
        
        private void Start()
        {
            Pool = new ObjectPool<HitParticle>(CreateParticle, OnTakeParticleFromPool, 
                OnReturnParticleToPoo, OnDestroyParticle,
                true, 100);
        }

        private HitParticle CreateParticle()
        {
            var particle = Instantiate(particlePrefab, transform);
            particle.SetPool(Pool);
            return particle;
        }

        private void OnTakeParticleFromPool(HitParticle particle)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }

        private void OnReturnParticleToPoo(HitParticle particle)
        {
            particle.gameObject.SetActive(false);
        }

        private void OnDestroyParticle(Object particle)
        {
            Destroy(particle);
        }
    }
}