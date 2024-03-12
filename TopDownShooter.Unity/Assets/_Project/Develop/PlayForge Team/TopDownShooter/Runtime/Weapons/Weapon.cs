using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract WeaponIdentity Id { get; }
        
        [SerializeField] private float bulletDelay = 0.05f;
        [SerializeField] private float reloadingDuration = 4f;
        [SerializeField] private int bulletsInRow = 7;
        [SerializeField] private int damage = 10;
        
        private BulletSpawner bulletSpawner;
        private Transform _bulletSpawnPoint;
        private int _currentBulletsInRow;
        private float _bulletTimer;
        private float _reloadingTimer;
        private bool _isShootDelayEnd;
        private bool _isReloading;
        
        public void Init(BulletSpawner spawner)
        {
            bulletSpawner = spawner;
            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
            FillBulletsToRow();
        }
        
        private void Update()
        {
            ShootDelaying();
            Reloading();
        }
        
        public void Shoot(float damageMultiplier)
        {
            if (!_isShootDelayEnd || !CheckHasBulletsInRow())
            {
                return;
            }
            
            _bulletTimer = 0;

            SpawnBullet(damageMultiplier);
            _currentBulletsInRow--;
        }
        
        public void Reload()
        {
            if (_isReloading)
            {
                _reloadingTimer += Time.deltaTime;

                return;
            }
            
            _isReloading = true;
        }
        
        public bool CheckHasBulletsInRow()
        {
            return _currentBulletsInRow > 0;
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        private void SpawnBullet(float damageMultiplier)
        {
            var bullet = bulletSpawner.Pool.Get();
            InitBullet(bullet, damageMultiplier);
        }

        private void InitBullet(Bullet bullet, float damageMultiplier)
        {
            var bulletTransform = bullet.transform;
            var bulletSpawnTransform = _bulletSpawnPoint.transform;
            bulletTransform.position = bulletSpawnTransform.position;
            bulletTransform.rotation = bulletSpawnTransform.rotation;
            bullet.SetDamage((int)(damage * damageMultiplier));
        }
        
        private void ShootDelaying()
        {
            _bulletTimer += Time.deltaTime;
            _isShootDelayEnd = _bulletTimer >= bulletDelay;
        }

        private void Reloading()
        {
            if (_isReloading && _reloadingTimer >= reloadingDuration)
            {
                FillBulletsToRow();
            }
        }
        
        private void FillBulletsToRow()
        {
            _isReloading = false;
            _reloadingTimer = 0;
            _currentBulletsInRow = bulletsInRow;
        }
    }
}