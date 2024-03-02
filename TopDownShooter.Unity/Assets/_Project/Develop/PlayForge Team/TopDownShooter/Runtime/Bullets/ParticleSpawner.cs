using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particlePrefab;
        public ObjectPool<ParticleSystem> Pool { get; private set; }
        
        private void Start()
        {
            Pool = new ObjectPool<ParticleSystem>(CreateParticle, OnTakeParticleFromPool, 
                OnReturnParticleToPoo, OnDestroyParticle,
                true, 1000);
        }

        private ParticleSystem CreateParticle()
        {
            var particle = Instantiate(particlePrefab, transform);
            return particle;
        }

        private void OnTakeParticleFromPool(Component particle)
        {
            particle.gameObject.SetActive(true);
        }

        private void OnReturnParticleToPoo(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }

        private void OnDestroyParticle(Object particle)
        {
            Destroy(particle);
        }
    }
}