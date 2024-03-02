using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine;
using System;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterShooting : CharacterPart
    {
        public const float DefaultDamageMultiplier = 1;
        
        public event Action<float> SetDamageMultiplierEvent;
        public event Action<float, float> ChangeDamageTimerEvent;

        [SerializeField] protected BulletSpawner bulletSpawner;
        
        private float DamageMultiplier { get; set; } = DefaultDamageMultiplier;
        private Transform _bulletSpawnPoint;
        private Weapon _weapon;
        private float _damageMultiplierTimer;
        private float _damageMultiplierDuration;
        
        protected override void OnInit()
        {
            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
            _weapon = GetComponentInChildren<Weapon>();
            SetDefaultDamageMultiplier();
        }
        
        public void SetDamageMultiplier(float multiplier, float duration)
        {
            DamageMultiplier = multiplier;
            _damageMultiplierDuration = duration;
            _damageMultiplierTimer = 0;

            SetDamageMultiplierEvent?.Invoke(DamageMultiplier);
            ChangeDamageTimerEvent?.Invoke(_damageMultiplierTimer, _damageMultiplierDuration);
        }
        
        protected void DamageBonusing()
        {
            if (_damageMultiplierDuration <= 0)
            {
                return;
            }
            
            _damageMultiplierTimer += Time.deltaTime;

            ChangeDamageTimerEvent?.Invoke(_damageMultiplierTimer, _damageMultiplierDuration);

            if (_damageMultiplierTimer >= _damageMultiplierDuration)
            {
                SetDefaultDamageMultiplier();
            }
        }

        protected void SpawnBullet()
        {
            var bullet = bulletSpawner.Pool.Get();
            InitBullet(bullet);
        }
        
        private void SetDefaultDamageMultiplier()
        {
            SetDamageMultiplier(DefaultDamageMultiplier, 0);
        }

        private void InitBullet(Bullet bullet)
        {
            bullet.SetDamage((int)(_weapon.Damage * DamageMultiplier));
            var bulletTransform = bullet.transform;
            var bulletSpawnTransform = _bulletSpawnPoint.transform;
            bulletTransform.position = bulletSpawnTransform.position;
            bulletTransform.rotation = bulletSpawnTransform.rotation;
        }
    }
}