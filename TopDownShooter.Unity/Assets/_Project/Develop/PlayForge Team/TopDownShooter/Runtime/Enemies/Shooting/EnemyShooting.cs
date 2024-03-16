using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Shooting
{
    public abstract class EnemyShooting : CharacterShooting
    {
        [SerializeField] private float shootingRange = 10f;

        private Transform _targetTransform;
        
        protected override void OnInit()
        {
            base.OnInit();
            _targetTransform = FindAnyObjectByType<Player>().transform;
        }

        protected override void Update()
        {
            base.Update();
            Shoot();
            Reload();
        }
        
        protected override void Reload()
        {
            if (!CheckHasBulletsInRow())
            {
                base.Reload();
            }
        }
        
        protected bool CheckTargetInRange()
        {
            return (_targetTransform.position - transform.position).magnitude <= shootingRange;
        }
    }
}