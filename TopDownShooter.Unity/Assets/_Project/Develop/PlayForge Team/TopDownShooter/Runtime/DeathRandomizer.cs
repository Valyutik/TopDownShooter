using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime
{
    public sealed class DeathRandomizer : StateMachineBehaviour
    {
        private const int AnimationCount = 3;
        private const string DeathIdKey = "DeathId";
        private static readonly int DeathId = Animator.StringToHash(DeathIdKey);
        
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animator.SetInteger(DeathId, Random.Range(0, AnimationCount));
        }
    }
}