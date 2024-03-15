using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerAction : MonoBehaviour
    {
        public event Action JumpEvent, ShootEvent;
        public event Action<int> SwitchWeaponEvent;
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public float ZoomCamera { get; private set; }
        public float RotateCameraDirection { get; private set; }

        private PlayerInput _input;
        private InputAction _shoot;

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _shoot = _input.actions["Shoot"];
        }

        private void Update()
        {
            if (_shoot.inProgress)
            {
                ShootEvent?.Invoke();
            }
        }

        public void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        public void OnJump()
        {
            JumpEvent?.Invoke();
        }

        public void OnRotateCamera(InputValue value)
        {
            RotateCameraDirection = value.Get<float>();
        }

        public void OnAim(InputValue value)
        {
            LookDirection = value.Get<Vector2>();
        }

        public void OnZoomCamera(InputValue value)
        {
            ZoomCamera = value.Get<float>();
        }

        public void OnSwitchWeaponsToRifle()
        {
            SwitchWeaponEvent?.Invoke(1);
        }
        
        public void OnSwitchWeaponsToShotgun()
        {
            SwitchWeaponEvent?.Invoke(2);
        }
        
        public void OnSwitchWeaponsToPistol()
        {
            SwitchWeaponEvent?.Invoke(3);
        }
    }
}