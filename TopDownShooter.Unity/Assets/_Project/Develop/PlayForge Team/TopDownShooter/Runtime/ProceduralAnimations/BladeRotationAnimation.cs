using DG.Tweening;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.ProceduralAnimations
{
    public sealed class BladeRotationAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 directionRotation = new Vector3(0, 0, 360f);
        [SerializeField] private float rotationSpeed = 5f;
        
        private void Start()
        {
            transform.DORotate(directionRotation, rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.Linear);
        }
    }
}
