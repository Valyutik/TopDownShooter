using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerShooting : CharacterShooting
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float bulletDelay = 0.05f;
        private Transform _bulletSpawnPoint;
        private float _bulletTimer;
        
        protected override void OnInit()
        {
            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
            _bulletTimer = 0;
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            Shooting();
        }

        private void Shooting()
        {
            if (Input.GetMouseButton(0))
            {
                _bulletTimer += Time.deltaTime;

                if (_bulletTimer >= bulletDelay)
                {
                    _bulletTimer = 0;
                    SpawnBullet();
                }
            }
        }

        private void SpawnBullet()
        {
            Instantiate(bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        }
    }
}