using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerShooting : CharacterShooting
    {
        [SerializeField] private bool autoReloading = true;
        private PlayerAction _playerAction;
        
        protected override void OnInit()
        {
            base.OnInit();

            _playerAction = GetComponent<PlayerAction>();
            _playerAction.ShootEvent += Shooting;
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
        
        protected override void Reloading()
        {
            if ((!CheckHasBulletsInRow() && Input.GetMouseButton(0)) || Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
        
        private void AutoReloading()
        {
            if (!autoReloading)
            {
                return;
            }
            
            if (!CheckHasBulletsInRow())
            {
                Reload();
            }
        }

        protected override void Shooting()
        {
            Shoot();
            AutoReloading();
        }
    }
}