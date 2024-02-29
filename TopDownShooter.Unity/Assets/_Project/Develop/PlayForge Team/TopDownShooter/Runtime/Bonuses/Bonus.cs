using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Bonuses
{
    public abstract class Bonus : MonoBehaviour
    {
        protected abstract bool CheckTriggeredObject(Collider other);

        protected abstract void ApplyBonus();

        private void OnTriggerEnter(Collider other)
        {
            if (CheckTriggeredObject(other))
            {
                ApplyBonus();
                DestroyBonus();
            }
        }
        
        private void DestroyBonus()
        {
            Destroy(gameObject);
        }
    }
}