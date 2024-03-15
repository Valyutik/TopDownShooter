using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Movement
{
    [RequireComponent(typeof(Collider))]
    public sealed class Obstacle : MonoBehaviour
    {
        [SerializeField] private float extraOffset = 0.7f;
        private Collider _collider;
        
        private void Start()
        {
            _collider = GetComponentInChildren<Collider>();
        }
        
        public Vector3 GetOppositePosition(Vector3 targetPosition)
        {
            var position = transform.position;
            var delta = targetPosition - position;
            var oppositePosition = position - delta;
            oppositePosition = _collider.bounds.ClosestPoint(oppositePosition);
            oppositePosition -= delta.normalized * extraOffset;
            
            return oppositePosition;
        }
    }
}