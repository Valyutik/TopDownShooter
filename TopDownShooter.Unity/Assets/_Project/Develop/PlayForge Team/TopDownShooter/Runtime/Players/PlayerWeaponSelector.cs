using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    [RequireComponent(typeof(PlayerAction))]
    public sealed class PlayerWeaponSelector : CharacterWeaponSelector
    {
        private PlayerAction playerAction;
        
        private readonly Dictionary<int, WeaponIdentity> _weaponKeyToIdentityPairs = new()
        {
            { 1, WeaponIdentity.Rifle },
            { 2, WeaponIdentity.Shotgun },
            { 3, WeaponIdentity.Pistol }
        };

        private void Start()
        {
            playerAction = GetComponent<PlayerAction>();
            playerAction.SwitchWeaponEvent += CheckChangeKey;
        }

        private void OnDestroy()
        {
            playerAction.SwitchWeaponEvent -= CheckChangeKey;
        }

        private void CheckChangeKey(int id)
        {
            foreach (var pair in _weaponKeyToIdentityPairs.Where(pair => pair.Key == id))
            {
                weaponId = pair.Value;
                RefreshSelectedWeapon();
            }
        }
    }
}