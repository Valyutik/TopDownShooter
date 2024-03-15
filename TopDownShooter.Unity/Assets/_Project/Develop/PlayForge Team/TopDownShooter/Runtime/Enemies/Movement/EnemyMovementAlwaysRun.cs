namespace PlayForge_Team.TopDownShooter.Runtime.Enemies.Movement
{
    public sealed class EnemyMovementAlwaysRun : EnemyMovement
    {
        protected override void Movement()
        {
            MoveToPlayer();
        }
    }
}