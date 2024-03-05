using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public abstract class WeaponAiming : MonoBehaviour
    {
        public abstract WeaponIdentity Id { get; }
        private MultiAimConstraint[] _constraints;
        
        public void Init(Transform aim)
        {
            WeightedTransformArray constraintSourceObject = CreateConstraintSourceObject(aim);
            _constraints = GetComponentsInChildren<MultiAimConstraint>(true);

            foreach (var constraint in _constraints)
            {
                constraint.data.sourceObjects = constraintSourceObject;
            }
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        private WeightedTransformArray CreateConstraintSourceObject(Transform aim)
        {
            var constraintAimArray = new WeightedTransformArray(1)
            {
                [0] = new(aim, 1)
            };

            return constraintAimArray;
        }
    }
}