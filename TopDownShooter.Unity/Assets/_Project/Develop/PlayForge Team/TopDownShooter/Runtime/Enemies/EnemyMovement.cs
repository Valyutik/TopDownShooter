using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;
using UnityEngine.AI;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemyMovement : CharacterMovement
    {
        private const string MovementHorizontalKey = "Horizontal";
        private const string MovementVerticalKey = "Vertical";
        
        private static readonly int Horizontal = Animator.StringToHash(MovementHorizontalKey);
        private static readonly int Vertical = Animator.StringToHash(MovementVerticalKey);
        
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private Transform _playerTransform;
        private Vector3 _prevPosition;

        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerTransform = FindAnyObjectByType<Player>().transform;
            _prevPosition = transform.position;
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            SetTargetPosition(_playerTransform.position);
            RefreshAnimation();
        }
        
        protected override void OnStop()
        {
            _navMeshAgent.enabled = false;
            RefreshAnimation();
        }

        private void SetTargetPosition(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
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