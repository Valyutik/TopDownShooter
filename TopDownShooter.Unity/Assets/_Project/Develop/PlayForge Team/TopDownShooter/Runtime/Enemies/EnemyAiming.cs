using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemyAiming : CharacterAiming
    {
        [SerializeField] private float aimingSpeed = 10f;
        [SerializeField] private Vector3 aimDeltaPosition = Vector3.up;
        [SerializeField] private float aimingRange = 20f;
        
        private Transform _aimTransform;
        private RigBuilder _rigBuilder;
        private WeaponAiming[] _weaponAimings;
        private Transform _targetTransform;
        private bool _isTargetInRange;
        
        protected override void OnInit()
        {
            _aimTransform = CreateAim().transform;
            _rigBuilder = GetComponentInChildren<RigBuilder>();
            _weaponAimings = GetComponentsInChildren<WeaponAiming>(true);

            _targetTransform = FindAnyObjectByType<Player>().transform;
            SetDefaultAimPosition();
            InitWeaponAimings(_weaponAimings, _aimTransform);
        }
        
        private void InitWeaponAimings(IReadOnlyList<WeaponAiming> weaponAimings, Transform aim)
        {
            foreach (var weaponAiming in weaponAimings)
            {
                weaponAiming.Init(aim);
            }
            _rigBuilder.Build();
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            Aiming();
        }

        private void Aiming()
        {
            if (CheckTargetInRange())
            {
                _isTargetInRange = true;
                _aimTransform.position = Vector3.Lerp(_aimTransform.position,
                    _targetTransform.position + aimDeltaPosition, aimingSpeed * Time.deltaTime);
            }
            else
            {
                if (_isTargetInRange)
                {
                    _isTargetInRange = false;
                    SetDefaultAimPosition();
                }
            }
            
            var lookDirection = (_aimTransform.position - transform.position).normalized;
            lookDirection.y = 0;
            var newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, aimingSpeed * Time.deltaTime);
        }
        
        private bool CheckTargetInRange()
        {
            return (_targetTransform.position - transform.position).magnitude <= aimingRange;
        }
        
        private GameObject CreateAim()
        {
            var aim = new GameObject("EnemyAim");
            aim.transform.SetParent(transform);
            return aim;
        }
        
        private void SetDefaultAimPosition()
        {
            var tr = transform;
            _aimTransform.position = tr.position + tr.forward + aimDeltaPosition;
        }
    }
}