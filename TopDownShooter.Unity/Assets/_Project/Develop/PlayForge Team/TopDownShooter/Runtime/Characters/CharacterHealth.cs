using System;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterHealth : CharacterPart
    {
        private const string DeathKey = "Death";
        private static readonly int Death = Animator.StringToHash(DeathKey);
        
        public event Action OnDieEvent;
        public event Action<CharacterHealth> OnDieWithObject;
        public event Action OnAddHealthPoints;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private int startHealthPoints = 100;
        private Animator _animator;
        private int _healthPoints;
        private bool _isDead;

        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            _healthPoints = startHealthPoints;
            _isDead = false;
        }
        
        public void AddHealthPoints(int value)
        {
            if (_isDead)
            {
                return;
            }

            _healthPoints += value;
            _healthPoints = Mathf.Clamp(_healthPoints, 0, startHealthPoints);

            OnAddHealthPoints?.Invoke();

            if (_healthPoints <= 0)
            {
                OnDie();
            }
        }
        
        public int GetStartHealthPoints()
        {
            return startHealthPoints;
        }

        public int GetHealthPoints()
        {
            return _healthPoints;
        }
        
        private void OnDie()
        {
            _isDead = true;
            _animator.SetTrigger(Death);
            OnDieEvent?.Invoke();
            OnDieWithObject?.Invoke(this);
            audioSource.PlayOneShot(deathClip);
        }
    }
}