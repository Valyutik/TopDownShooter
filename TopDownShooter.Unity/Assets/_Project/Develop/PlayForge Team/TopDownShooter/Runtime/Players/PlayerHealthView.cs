using PlayForge_Team.TopDownShooter.Runtime.Characters;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerHealthView : CharacterHealthView
    {
        private void Start()
        {
            CharacterHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
            Init(playerHealth);
        }
    }
}