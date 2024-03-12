namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public sealed class WeaponPistol : Weapon
    {
        public override WeaponIdentity Id => WeaponIdentity.Pistol;
        
        protected override void DoShoot(float damageMultiplier)
        {
            DefaultShoot(damageMultiplier);
        }
    }
}