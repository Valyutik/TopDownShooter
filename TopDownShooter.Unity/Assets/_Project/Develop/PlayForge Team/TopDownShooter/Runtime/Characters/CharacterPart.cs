using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterPart : MonoBehaviour
    {
        protected bool IsActive;

        public void Init()
        {
            IsActive = true;
            OnInit();
        }
        
        public void Stop()
        {
            IsActive = false;
            OnStop();
        }
        
        protected virtual void OnInit() { }
        protected virtual void OnStop() { }
    }
}