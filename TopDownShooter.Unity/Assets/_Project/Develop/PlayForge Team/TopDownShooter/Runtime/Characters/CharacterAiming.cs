using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterAiming : CharacterPart
    {
        private WeaponAiming[] _weaponAimings;
        private RigBuilder _rigBuilder;
        private WeaponIdentity _weaponId;

        public void SetWeapon(WeaponIdentity id)
        {
            _weaponId = id;
            SetCurrentWeapon(_weaponId);
        }
        
        protected override void OnInit()
        {
            _weaponAimings = GetComponentsInChildren<WeaponAiming>(true);
            _rigBuilder = GetComponentInChildren<RigBuilder>();
        }
        
        protected void InitWeaponAimings(Transform aim)
        {
            foreach (var t in _weaponAimings)
            {
                t.Init(aim);
            }

            _rigBuilder.Build();
        }
        
        private void SetCurrentWeapon(WeaponIdentity identity)
        {
            foreach (var weaponAiming in _weaponAimings)
            {
                var isTargetId = weaponAiming.Id == identity;
                weaponAiming.SetActive(isTargetId);
            }
        }
    }
}