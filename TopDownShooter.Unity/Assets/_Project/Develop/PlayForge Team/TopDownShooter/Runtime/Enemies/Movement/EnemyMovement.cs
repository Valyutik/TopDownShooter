using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine.AI;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Movement
{
    public abstract class EnemyMovement : CharacterMovement
    {
        private const string MovementHorizontalKey = "Horizontal";
        private const string MovementVerticalKey = "Vertical";
        
        private static readonly int Horizontal = Animator.StringToHash(MovementHorizontalKey);
        private static readonly int Vertical = Animator.StringToHash(MovementVerticalKey);

        protected Transform PlayerTransform;
        private NavMeshAgent navMeshAgent;
        private Animator _animator;
        private Vector3 _prevPosition;

        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            PlayerTransform = FindAnyObjectByType<Player>().transform;
            _prevPosition = transform.position;
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            Movement();

            RefreshAnimation();
        }
        
        protected abstract void Movement();
        
        protected void MoveToPlayer()
        {
            SetTargetPosition(PlayerTransform.position);
        }
        
        protected override void OnStop()
        {
            navMeshAgent.enabled = false;
            RefreshAnimation();
        }

        protected void SetTargetPosition(Vector3 position)
        {
            navMeshAgent.SetDestination(position);
        }

        private void RefreshAnimation()
        {
            var curPosition = transform.position;
            var deltaMove = curPosition - _prevPosition;
            _prevPosition = curPosition;
            deltaMove.Normalize();
            var relatedX = Vector3.Dot(deltaMove, transform.right);
            var relatedY = Vector3.Dot(deltaMove, transform.forward);
            _animator.SetFloat(Horizontal, relatedX);
            _animator.SetFloat(Vertical, relatedY);
        }
    }
}