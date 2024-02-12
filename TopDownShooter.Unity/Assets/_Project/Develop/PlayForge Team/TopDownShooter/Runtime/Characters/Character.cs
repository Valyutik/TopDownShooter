using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class Character : MonoBehaviour
    {
        private CharacterMovement _movement;
        private CharacterAiming _aiming;
        private CharacterPart[] _parts;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _movement = GetComponent<CharacterMovement>();
            _aiming = GetComponent<CharacterAiming>();

            _parts = new CharacterPart[]
            {
                _movement,

                _aiming
            };

            foreach (var t in _parts)
            {
                if (t)
                {
                    t.Init();
                }
            }
        }
    }
}