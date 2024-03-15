using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Movement
{
    public sealed class EnemyMovementRunThenHide : EnemyMovement
    {
        [SerializeField] private float searchRange = 10f;
        [SerializeField] private LayerMask coverLayerMask;
        
        private Vector3 _hidePosition;
        private bool _isCoverFound;
        
        protected override void Movement()
        {
            if (!_isCoverFound)
            {
                MoveToPlayer();
                FindCover();
            }
            else
            {
                SetTargetPosition(_hidePosition);
            }
        }
        
        private void FindCover()
        {
            var cover = GetRandomCover();

            if (!cover)
            {
                return;
            }
            _isCoverFound = true;

            _hidePosition = cover.GetOppositePosition(PlayerTransform.position);
        }
        
        private Obstacle GetRandomCover()
        {
            var collidersInRange = Physics.OverlapSphere(transform.position, searchRange, coverLayerMask);

            if (collidersInRange == null || collidersInRange.Length == 0)
            {
                return null;
            }
            
            var randomCollider = collidersInRange[Random.Range(0,collidersInRange.Length)];
            var randomCover = randomCollider.GetComponentInParent<Obstacle>();

            return randomCover;
        }
    }
}