﻿using System.Collections.Generic;
using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerAiming : CharacterAiming
    {
        [SerializeField] private float aimingSpeed = 10f;

        private PlayerAction _playerAction;
        private Transform _aimTransform;
        private RigBuilder _rigBuilder;
        private WeaponAiming[] _weaponAimings;

        private Camera _mainCamera;

        protected override void OnInit()
        {
            _mainCamera = Camera.main;
            _playerAction = GetComponent<PlayerAction>();
            _aimTransform = GetComponentInChildren<PlayerAim>().transform;
            _rigBuilder = GetComponentInChildren<RigBuilder>();
            _weaponAimings = GetComponentsInChildren<WeaponAiming>(true);

            InitWeaponAimings(_weaponAimings, _aimTransform);
        }

        private void InitWeaponAimings(IEnumerable<WeaponAiming> weaponAimings, Transform aim)
        {
            foreach (var weaponAiming in weaponAimings)
            {
                weaponAiming.Init(aim);
            }

            _rigBuilder.Build();
        }

        private void FixedUpdate()
        {
            if (!IsActive)
            {
                return;
            }

            Aiming();
        }

        private void Aiming()
        {
            var mouseScreenPosition = _playerAction.LookDirection;
            var findTargetRay = _mainCamera.ScreenPointToRay(mouseScreenPosition);

            if (Physics.Raycast(findTargetRay, out var hitInfo))
            {
                var lookDirection = (hitInfo.point - transform.position).normalized;
                lookDirection.y = 0;
                var newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, newRotation, aimingSpeed * Time.fixedDeltaTime);

                _aimTransform.position =
                    Vector3.Lerp(_aimTransform.position, hitInfo.point, aimingSpeed * Time.fixedDeltaTime);
            }
        }
    }
}