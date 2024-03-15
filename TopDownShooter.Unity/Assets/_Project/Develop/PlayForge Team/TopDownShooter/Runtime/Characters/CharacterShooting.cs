using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterShooting : CharacterPart, IWeaponDependent
    {
        public const float DefaultDamageMultiplier = 1;
        private static readonly int WeaponId = Animator.StringToHash("WeaponId");
        
        public event Action<float> SetDamageMultiplierEvent;
        public event Action<float, float> ChangeDamageTimerEvent;
        
        public event Action<Weapon> OnSetCurrentWeaponEvent;

        [HideInInspector] public BulletSpawner spawner;
        private WeaponIdentity _weaponId;
        private float DamageMultiplier { get; set; } = DefaultDamageMultiplier;
        private Animator _animator;
        private Weapon[] _weapons;
        private Weapon _currentWeapon;
        private float _damageMultiplierTimer;
        private float _damageMultiplierDuration;

        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            _weapons = GetComponentsInChildren<Weapon>(true);
            InitWeapons(_weapons);

            SetDefaultDamageMultiplier();
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            
            Shooting();
            Reloading();
            DamageBonus();
        }
        
        protected abstract void Shooting();

        protected abstract void Reloading();
        
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
        
        private void SetDefaultDamageMultiplier()
        {
            SetDamageMultiplier(DefaultDamageMultiplier, 0);
        }
        
        private void SetCurrentWeapon(WeaponIdentity identity)
        {
            foreach (var weapon in _weapons)
            {
                var isTargetId = weapon.Id == identity;

                if (isTargetId)
                {
                    _currentWeapon = weapon;

                    OnSetCurrentWeaponEvent?.Invoke(weapon);
                }
                weapon.SetActive(isTargetId);
            }

            var id = WeaponIdentifier.GetAnimationIdByWeaponIdentify(identity);
            _animator.SetInteger(WeaponId, id);
        }
        
        protected void Shoot()
        {
            _currentWeapon.Shoot(DamageMultiplier);
        }

        protected bool CheckHasBulletsInRow()
        {
            return _currentWeapon.CheckHasBulletsInRow();
        }

        protected void Reload()
        {
            _currentWeapon.Reload();
        }

        private void InitWeapons(IEnumerable<Weapon> weapons)
        {
            foreach (var t in weapons)
            {
                t.Init(spawner);
            }
        }
    }
}