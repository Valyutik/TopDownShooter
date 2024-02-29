using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bonuses
{
    public sealed class HealthBonus : Bonus
    {
        [SerializeField] private int health = 50;

        private CharacterHealth _healingCharacterHealth;

        protected override bool CheckTriggeredObject(Collider other)
        {
            _healingCharacterHealth = other.GetComponentInParent<PlayerHealth>();

            return _healingCharacterHealth != null;
        }
        
        protected override void ApplyBonus()
        {
            _healingCharacterHealth.AddHealthPoints(health);
        }
    }
}