using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class Character : MonoBehaviour
    {
        private CharacterPart[] _parts;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _parts = GetComponents<CharacterPart>();

            foreach (var characterPart in _parts)
            {
                characterPart.Init();
            }
            InitDeath();
        }

        private void OnDestroy()
        {
            foreach (var characterPart in _parts)
            {
                if (characterPart is CharacterHealth health)
                {
                    health.OnDieEvent -= Stop;
                }
            }
        }

        private void InitDeath()
        {
            foreach (var characterPart in _parts)
            {
                if (characterPart is CharacterHealth health)
                {
                    health.OnDieEvent += Stop;
                }
            }
        }

        private void Stop()
        {
            foreach (var characterPart in _parts)
            {
                characterPart.Stop();
            }
        }
    }
}