using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bullets
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject hitPrefab;
        [SerializeField] private float speed = 30f;
        [SerializeField] private float lifeTime = 2f;
        
        private void Update()
        {
            ReduceLifeTime();
            CheckHit();
            Move();
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
            if (Physics.Raycast(transform.position, transform.forward, out var hit, speed * Time.deltaTime))
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
            var tr = transform;
            Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(-tr.up, -tr.forward));
            DestroyBullet();
        }
        
        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}