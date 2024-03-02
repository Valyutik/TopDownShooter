using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class Enemy : Character
    {
        private EnemyShooting _shooting;

        public override void Init()
        {
            base.Init();
            _shooting = GetComponent<EnemyShooting>();
        }

        public void SetBulletSpawner(BulletSpawner spawner)
        {
            _shooting.SetBulletSpawner(spawner);
        }
    }
}