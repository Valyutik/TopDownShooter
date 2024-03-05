using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerShooting : CharacterShooting
    {
        [SerializeField] private float bulletDelay = 0.05f;
        private PlayerAction _playerAction;
        private float _bulletTimer;
        
        protected override void OnInit()
        {
            base.OnInit();

            _playerAction = GetComponent<PlayerAction>();
            _playerAction.ShootEvent += Shooting;
            _bulletTimer = 0;
        }

        private void OnDestroy()
        {
            _playerAction.ShootEvent -= Shooting;
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
            DamageBonus();
        }

        private void Shooting()
        {
            _bulletTimer += Time.deltaTime;

            if (_bulletTimer >= bulletDelay)
            {
                _bulletTimer = 0;
                SpawnBullet();
            }
        }
    }
}