using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using PlayForge_Team.TopDownShooter.Runtime.Sounds;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public event Action<int, int> OnBulletsInRowChangeEvent;
        public event Action OnAutoReloadEvent, OnEndReloadingEvent;
        
        public abstract WeaponIdentity Id { get; }
        
        [SerializeField] private float bulletDelay = 0.05f;
        [SerializeField] private float reloadingDuration = 4f;
        [SerializeField] private float spreadAngle = 5f;
        [SerializeField] private int bulletsInRow = 7;
        [SerializeField] private int damage = 10;
        
        private BulletSpawner bulletSpawner;
        private WeaponSound _weaponSound;
        private Transform _bulletSpawnPoint;
        private float _reloadingTimer;
        private float _bulletTimer;
        private bool _isShootDelayEnd;
        private bool _isReloading;
        private int _currentBulletsInRow;

        public bool IsReloading => _isReloading;
        private int CurrentBulletsInRow
        {
            get => _currentBulletsInRow;
            set
            {
                _currentBulletsInRow = value;
                if (_currentBulletsInRow <= 1)
                {
                    OnAutoReloadEvent?.Invoke();
                }
            }
        }
        
        public void Init(BulletSpawner spawner)
        {
            bulletSpawner = spawner;
            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
            FillBulletsToRow();
            
            _weaponSound = GetComponentInChildren<WeaponSound>();
            _weaponSound.Init();
        }
        
        private void Update()
        {
            ShootDelaying();
            Reloading();
        }
        
        public void Shoot(float damageMultiplier)
        {
            if (!_isShootDelayEnd || !CheckHasBulletsInRow() || _isReloading)
            {
                return;
            }
            _bulletTimer = 0;

            DoShoot(damageMultiplier);
            CurrentBulletsInRow--;
            OnBulletsInRowChangeEvent?.Invoke(CurrentBulletsInRow, bulletsInRow);
            
            _weaponSound.PlaySound(SoundType.Shoot);
        }
        
        public void Reload()
        {
            _isReloading = true;
            _weaponSound.PlaySound(SoundType.Reload);
        }
        
        public bool CheckHasBulletsInRow()
        {
            return CurrentBulletsInRow > 0;
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            OnBulletsInRowChangeEvent?.Invoke(CurrentBulletsInRow, bulletsInRow);
            
            if (value)
            {
                _weaponSound.PlaySound(SoundType.Switch);
            }
        }
        
        protected abstract void DoShoot(float damageMultiplier);

        protected void DefaultShoot(float damageMultiplier)
        {
            SpawnBullet(damageMultiplier);
        }
        
        private void SpawnBullet(float damageMultiplier)
        {
            var bullet = bulletSpawner.Pool.Get();
            
            InitBullet(bullet, damageMultiplier);
            
            var bulletEulerAngles = bullet.transform.eulerAngles;
            bulletEulerAngles.x += Random.Range(-spreadAngle, spreadAngle);
            bulletEulerAngles.y += Random.Range(-spreadAngle, spreadAngle);
            bullet.transform.eulerAngles = bulletEulerAngles;
            
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
            if (_isReloading)
            {
                _reloadingTimer += Time.deltaTime;

                if (_reloadingTimer >= reloadingDuration)
                {
                    FillBulletsToRow();
                    OnEndReloadingEvent?.Invoke();
                }
            }
        }
        
        private void FillBulletsToRow()
        {
            _isReloading = false;
            _reloadingTimer = 0;
            CurrentBulletsInRow = bulletsInRow;
            OnBulletsInRowChangeEvent?.Invoke(CurrentBulletsInRow, bulletsInRow);
        }
    }
}