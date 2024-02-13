using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemyPhysicBound : CharacterPhysicBounds
    {
        private Collider _bodyCollider;

        protected override void OnInit()
        {
            _bodyCollider = GetComponentInChildren<Collider>();
        }
        
        protected override void OnStop()
        {
            _bodyCollider.enabled = false;
        }
    }
}