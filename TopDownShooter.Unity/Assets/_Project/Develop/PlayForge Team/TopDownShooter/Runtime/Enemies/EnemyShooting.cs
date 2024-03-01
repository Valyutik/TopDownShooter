using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemyShooting : CharacterShooting
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float bulletDelay = 0.05f;
        [SerializeField] private float shootingRange = 10f;
        [SerializeField] private int bulletsInRow = 7;
        [SerializeField] private float reloadingDuration = 4f;
        private Transform _bulletSpawnPoint;
        private Transform _targetTransform;
        private float _bulletTimer;
        private int _currentBulletsInRow;
        
        protected override void OnInit()
        {
            base.OnInit();

            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
            _targetTransform = FindAnyObjectByType<Player>().transform;
            
            Reload();
        }

        private void Reload()
        {
            _bulletTimer = 0;
            _currentBulletsInRow = bulletsInRow;
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            _bulletTimer += Time.deltaTime;
            
            Shooting();
            Reloading();
            DamageBonusing();
        }

        private void Shooting()
        {
            if (CheckTargetInRange() && CheckHasBulletsInRow() && _bulletTimer >= bulletDelay)
            {
                Shoot();
            }
        }

        private void Reloading()
        {
            if (!CheckHasBulletsInRow() && _bulletTimer >= reloadingDuration)
            {
                Reload();
            }
        }
        
        private bool CheckTargetInRange()
        {
            return (_targetTransform.position - transform.position).magnitude <= shootingRange;
        }

        private bool CheckHasBulletsInRow()
        {
            return _currentBulletsInRow > 0;
        }
        
        private void Shoot()
        {
            _bulletTimer = 0;
            SpawnBullet(bulletPrefab, _bulletSpawnPoint);
            _currentBulletsInRow--;
        }

        private void SpawnBullet()
        {
            Instantiate(bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        }
    }
}