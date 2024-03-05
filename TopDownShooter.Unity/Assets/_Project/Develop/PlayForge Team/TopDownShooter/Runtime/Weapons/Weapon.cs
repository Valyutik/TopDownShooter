using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract WeaponIdentity Id { get; }
        
        [SerializeField] private int damage = 10;

        public int Damage => damage;
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}