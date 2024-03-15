using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine;
using System;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterWeaponSelector : CharacterPart
    {
        public event Action<WeaponIdentity> OnWeaponSelectedEvent;

        [SerializeField] protected WeaponIdentity weaponId;

        public void RefreshSelectedWeapon()
        {
            OnWeaponSelectedEvent?.Invoke(weaponId);
        }
        
        protected override void OnInit() 
        {
            RefreshSelectedWeapon();
        }
    }
}