namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public sealed class WeaponRifle : Weapon
    {
        public override WeaponIdentity Id => WeaponIdentity.Rifle;
        
        protected override void DoShoot(float damageMultiplier)
        {
            DefaultShoot(damageMultiplier);
        }
    }
}