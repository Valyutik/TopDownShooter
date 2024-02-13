using System;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public sealed class CharacterHealth : CharacterPart
    {
        private const string DeathKey = "Death";
        private static readonly int Death = Animator.StringToHash(DeathKey);
        
        public event Action OnDieEvent;
        
        [SerializeField] private int startHealthPoints = 100;
        private Animator _animator;
        private int _healthPoints;
        private bool _isDead;

        public void AddHealthPoints(int value)
        {
            if (_isDead)
            {
                return;
            }
            _healthPoints += value;

            if (_healthPoints <= 0)
            {
                OnDie();
            }
        }
        
        protected override void OnInit()
        {
            _animator = GetComponentInChildren<Animator>();
            _healthPoints = startHealthPoints;
            _isDead = false;
        }
        
        private void OnDie()
        {
            _isDead = true;
            _animator.SetTrigger(Death);
            OnDieEvent?.Invoke();
        }
    }
}