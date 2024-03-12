using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemyShooting : CharacterShooting
    {
        [SerializeField] private float shootingRange = 10f;

        private Transform _targetTransform;
        
        protected override void OnInit()
        {
            base.OnInit();
            _targetTransform = FindAnyObjectByType<Player>().transform;
        }
        
        protected override void Shooting()
        {
            if (CheckTargetInRange() && CheckHasBulletsInRow())
            {
                Shoot();
            }
        }
        
        protected override void Reloading()
        {
            if (!CheckHasBulletsInRow())
            {
                Reload();
            }
        }
        
        private bool CheckTargetInRange()
        {
            return (_targetTransform.position - transform.position).magnitude <= shootingRange;
        }
    }
}