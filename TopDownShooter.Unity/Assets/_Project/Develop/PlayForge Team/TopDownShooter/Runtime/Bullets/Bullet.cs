using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine.Pool;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 30f;
        [SerializeField] private float lifeTime = 2f;
        private ParticleSpawner _particleSpawner;
        private ObjectPool<Bullet> _pool;
        private int _damage;
        private float _currentLifeTime;

        private void OnEnable()
        {
            _currentLifeTime = lifeTime;
        }

        private void Update()
        {
            ReduceLifeTime();
            CheckHit();
            Move();
        }
        
        public void SetDamage(int value)
        {
            _damage = value;
        }

        public void SetParticleSpawner(ParticleSpawner spawner)
        {
            _particleSpawner = spawner;
        }

        public void SetPool(ObjectPool<Bullet> pool)
        {
            _pool = pool;
        }

        private void ReduceLifeTime()
        {
            _currentLifeTime -= Time.deltaTime;

            if (_currentLifeTime <= 0)
            {
                DestroyBullet();
            }
        }

        private void CheckHit()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, speed * Time.deltaTime)
                && !hit.collider.isTrigger)
            {
                Hit(hit);
            }
        }

        private void Move()
        {
            var tr = transform;
            tr.position += tr.forward * (speed * Time.deltaTime);
        }
        
        private void Hit(RaycastHit hit)
        {
            CheckCharacterHit(hit);

            var hitParticle = _particleSpawner.Pool.Get();
            var tr = transform;
            hitParticle.transform.position = hit.point;
            hitParticle.transform.rotation = Quaternion.LookRotation(-tr.up, -tr.forward);
            hitParticle.Play();
            
            DestroyBullet();
        }
        
        private void CheckCharacterHit(RaycastHit hit)
        {
            var hitHealth = hit.collider.GetComponentInChildren<CharacterHealth>();

            if (hitHealth)
            {
                hitHealth.AddHealthPoints(-_damage);
            }
        }
        
        private void DestroyBullet()
        {
            _pool.Release(this);
        }
    }
}