using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class Character : MonoBehaviour
    {
        private CharacterPart[] _parts;

        public void Init(BulletSpawner spawner)
        {
            _parts = GetComponents<CharacterPart>();
            GetComponent<CharacterShooting>().spawner = spawner;

            foreach (var characterPart in _parts)
            {
                characterPart.Init();
            }
            InitDeath();
            InitWeaponSelection();
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
        
        private void InitWeaponSelection()
        {
            foreach (var t in _parts)
            {
                if (t is CharacterWeaponSelector weaponSelector)
                {
                    weaponSelector.OnWeaponSelected += SelectWeapon;
                    weaponSelector.RefreshSelectedWeapon();
                }
            }
        }
        
        private void SelectWeapon(WeaponIdentity id)
        {
            foreach (var t in _parts)
            {
                if (t is IWeaponDependent weaponDependent)
                {
                    weaponDependent.SetWeapon(id);
                }
            }
        }
    }
}