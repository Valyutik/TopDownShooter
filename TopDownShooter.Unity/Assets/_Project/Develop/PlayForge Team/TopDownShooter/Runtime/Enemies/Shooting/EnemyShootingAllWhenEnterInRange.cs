namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Shooting
{
    public sealed class EnemyShootingAllWhenEnterInRange : EnemyShooting
    {
        private bool _isInRange;

        protected override void Shoot()
        {
            if (!_isInRange && CheckTargetInRange())
            {
                _isInRange = true;
            }
            
            if (_isInRange)
            {
                if (CheckHasBulletsInRow())
                {
                    base.Shoot();
                }
                else
                {
                    _isInRange = false;
                }
            }
        }
    }
}