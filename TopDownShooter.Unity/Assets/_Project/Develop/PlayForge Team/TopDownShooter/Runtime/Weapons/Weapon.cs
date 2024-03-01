using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        public int Damage => damage;
    }
}