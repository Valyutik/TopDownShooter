using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject hitPrefab;
        [SerializeField] private float speed = 30f;
        [SerializeField] private float lifeTime = 2f;
        private int _damage;
        
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

        private void ReduceLifeTime()
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
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

            var tr = transform;
            Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(-tr.up, -tr.forward));
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
            Destroy(gameObject);
        }
    }
}