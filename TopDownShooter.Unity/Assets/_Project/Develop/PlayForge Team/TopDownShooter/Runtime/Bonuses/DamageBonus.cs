using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bonuses
{
    public sealed class DamageBonus : Bonus
    {
        [SerializeField] private float damageMultiplier = 2f;
        [SerializeField] private float duration = 10f;
        private CharacterShooting _bonusCharacterShooting;

        protected override bool CheckTriggeredObject(Collider other)
        {
            _bonusCharacterShooting = other.GetComponentInParent<PlayerShooting>();

            return _bonusCharacterShooting != null;
        }
        
        protected override void ApplyBonus()
        {
            _bonusCharacterShooting.SetDamageMultiplier (damageMultiplier, duration);
        }
    }
}