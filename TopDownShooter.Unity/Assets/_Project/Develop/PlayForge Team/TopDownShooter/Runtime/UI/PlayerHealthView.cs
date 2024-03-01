using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;

namespace PlayForge_Team.TopDownShooter.Runtime.UI
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