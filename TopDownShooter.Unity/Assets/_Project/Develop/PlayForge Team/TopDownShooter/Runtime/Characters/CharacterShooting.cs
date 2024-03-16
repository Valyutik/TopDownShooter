using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using UnityEngine.Animations.Rigging;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterShooting : CharacterPart, IWeaponDependent
    {
        public const float DefaultDamageMultiplier = 1;
        private static readonly int WeaponId = Animator.StringToHash("WeaponId");
        private static readonly int IsReloading = Animator.StringToHash("IsReloading");
        
        public event Action<float> SetDamageMultiplierEvent;
        public event Action<float, float> ChangeDamageTimerEvent;
        
        public event Action<Weapon> OnSetCurrentWeaponEvent;

        [HideInInspector] public BulletSpawner spawner;
        protected Weapon[] Weapons;
        
        private WeaponIdentity _weaponId;
        private Animator _animator;
        private Weapon _currentWeapon;
        private Rig _rig;
        private float _damageMultiplierTimer;
        private float _damageMultiplierDuration;

        private float DamageMultiplier { get; set; } = DefaultDamageMultiplier;

        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            Weapons = GetComponentsInChildren<Weapon>(true);
            _rig = GetComponentInChildren<Rig>();
            InitWeapons(Weapons);

            SetDefaultDamageMultiplier();
        }
        
        protected virtual void Update()
        {
            if (!IsActive)
            {
                return;
            }
            
            DamageBonus();
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

        private void DamageBonus()
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
            UnsubscribeFromEndReloading();
            foreach (var weapon in Weapons)
            {
                var isTargetId = weapon.Id == identity;

                if (isTargetId)
                {
                    _currentWeapon = weapon;
                    SubscribeToEndReloading();

                    OnSetCurrentWeaponEvent?.Invoke(weapon);
                }
                weapon.SetActive(isTargetId);
            }

            var id = WeaponIdentifier.GetAnimationIdByWeaponIdentify(identity);
            _animator.SetInteger(WeaponId, id);
            
            RefreshReloadingAnimation();
        }
        
        protected virtual void Shoot()
        {
            if (IsActive)
            {
                _currentWeapon.Shoot(DamageMultiplier);
            }
        }

        protected bool CheckHasBulletsInRow()
        {
            return _currentWeapon.CheckHasBulletsInRow();
        }

        protected virtual void Reload()
        {
            _currentWeapon.Reload();
            RefreshReloadingAnimation();
        }

        private void InitWeapons(IEnumerable<Weapon> weapons)
        {
            foreach (var t in weapons)
            {
                t.Init(spawner);
            }
        }
        
        private void RefreshReloadingAnimation()
        {
            _rig.weight = _currentWeapon.IsReloading ? 0 : 1;
            _animator.SetBool(IsReloading, _currentWeapon.IsReloading);
        }
        
        private void SubscribeToEndReloading()
        {
            if (!_currentWeapon)
            {
                return;
            }
            
            _currentWeapon.OnEndReloadingEvent += RefreshReloadingAnimation;
        }
        
        private void UnsubscribeFromEndReloading()
        {
            if (!_currentWeapon)
            {
                return;
            }
            _currentWeapon.OnEndReloadingEvent -= RefreshReloadingAnimation;
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEndReloading();
        }
    }
}