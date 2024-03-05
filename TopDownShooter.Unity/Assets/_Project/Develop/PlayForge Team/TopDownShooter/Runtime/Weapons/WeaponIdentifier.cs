using System.Collections.Generic;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public static class WeaponIdentifier
    {
        private static readonly Dictionary<WeaponIdentity, int> WeaponIdentityToAnimationIdPairs = new()
        {
            {WeaponIdentity.Rifle, 0},
            {WeaponIdentity.Pistol, 1},
            {WeaponIdentity.Shotgun, 2}
        };

        public static int GetAnimationIdByWeaponIdentify(WeaponIdentity identity)
        {
            return WeaponIdentityToAnimationIdPairs[identity];
        }
    }

    public enum WeaponIdentity
    {
        Rifle,
        Pistol,
        Shotgun
    }
}