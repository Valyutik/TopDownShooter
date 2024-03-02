using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSpawner particleSpawner;
        [SerializeField] private Bullet bulletPrefab;
        public ObjectPool<Bullet> Pool { get; private set; }

        private void Start()
        {
            Pool = new ObjectPool<Bullet>(CreateBullet, OnTakeBulletFromPool, 
                OnReturnBulletToPoo, OnDestroyBullet,
                true, 1000);
        }

        private Bullet CreateBullet()
        {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetPool(Pool);
            bullet.SetParticleSpawner(particleSpawner);
            return bullet;
        }

        private void OnTakeBulletFromPool(Component bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnReturnBulletToPoo(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Object bullet)
        {
            Destroy(bullet);
        }
    }
}