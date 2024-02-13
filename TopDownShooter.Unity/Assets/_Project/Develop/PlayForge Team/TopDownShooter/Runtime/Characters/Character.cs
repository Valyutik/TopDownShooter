using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class Character : MonoBehaviour
    {
        private CharacterMovement _movement;
        private CharacterAiming _aiming;
        private CharacterShooting _shooting;
        private CharacterPart[] _parts;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _movement = GetComponent<CharacterMovement>();
            _aiming = GetComponent<CharacterAiming>();
            _shooting = GetComponent<CharacterShooting>();

            _parts = new CharacterPart[]
            {
                _movement,
                _aiming,
                _shooting
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