using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine;
using System;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterShooting : CharacterPart, IWeaponDependent
    {
        public const float DefaultDamageMultiplier = 1;
        private static readonly int WeaponId = Animator.StringToHash("WeaponId");
        
        public event Action<float> SetDamageMultiplierEvent;
        public event Action<float, float> ChangeDamageTimerEvent;
        
        [SerializeField] protected BulletSpawner bulletSpawner;
        private WeaponIdentity _weaponId;
        
        private float DamageMultiplier { get; set; } = DefaultDamageMultiplier;
        private BulletSpawnPoint _bulletSpawnPoint;
        private Animator _animator;
        private Weapon[] _weapons;
        private Weapon _currentWeapon;
        private float _damageMultiplierTimer;
        private float _damageMultiplierDuration;

        protected override void OnInit()
        {
            _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>();
            _animator = GetComponentInChildren<Animator>();
            _weapons = GetComponentsInChildren<Weapon>(true);

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
        
        public void SetWeapon(WeaponIdentity id)
        {
            _weaponId = id;

            SetCurrentWeapon(_weaponId);
        }
        
        protected void DamageBonus()
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
        
        private void SetCurrentWeapon(WeaponIdentity identity)
        {
            foreach (var weapon in _weapons)
            {
                var isTargetId = weapon.Id == identity;
                weapon.SetActive(isTargetId);

                if (isTargetId)
                {
                    _currentWeapon = weapon;
                }
            }
            
            var id = WeaponIdentifier.GetAnimationIdByWeaponIdentify(identity);

            _animator.SetInteger(WeaponId, id);
        }

        private void InitBullet(Bullet bullet)
        {
            bullet.SetDamage((int)(_currentWeapon.Damage * DamageMultiplier));
            var bulletTransform = bullet.transform;
            var bulletSpawnTransform = _bulletSpawnPoint.transform;
            bulletTransform.position = bulletSpawnTransform.position;
            bulletTransform.rotation = bulletSpawnTransform.rotation;
        }
    }
}