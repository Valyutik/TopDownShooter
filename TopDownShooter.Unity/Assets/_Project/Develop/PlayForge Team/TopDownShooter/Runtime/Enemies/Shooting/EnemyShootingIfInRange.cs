namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Shooting
{
    public sealed class EnemyShootingIfInRange : EnemyShooting
    {
        protected override void Shoot()
        {
            if (CheckTargetInRange() && CheckHasBulletsInRow())
            {
                base.Shoot();
            }
        }
    }
}