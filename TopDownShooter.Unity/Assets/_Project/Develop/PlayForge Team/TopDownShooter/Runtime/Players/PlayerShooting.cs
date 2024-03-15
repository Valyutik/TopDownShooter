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
            _playerAction.ShootEvent += Shoot;
            _playerAction.ReloadEvent += Reload;
        }

        private void OnDestroy()
        {
            _playerAction.ShootEvent -= Shoot;
            _playerAction.ReloadEvent -= Reload;
        }
        
        private void AutoReload()
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

        protected override void Shoot()
        {
            base.Shoot();
            AutoReload();
        }
    }
}