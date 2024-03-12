using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class Player : Character
    {
        [SerializeField] private BulletSpawner spawner;
        
        private void Start()
        {
            Init(spawner);
        }
    }
}