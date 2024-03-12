using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public sealed class WeaponShotgun : Weapon
    {
        [SerializeField] private int bulletsInOneShoot = 10;
        
        public override WeaponIdentity Id => WeaponIdentity.Shotgun;
        
        protected override void DoShoot(float damageMultiplier)
        {
            for (var i = 0; i < bulletsInOneShoot; i++)
            {
                DefaultShoot(damageMultiplier);
            }
        }
    }
}