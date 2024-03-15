using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Weapons;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace PlayForge_Team.TopDownShooter.Runtime.UI
{
    public sealed class PlayerCurrentWeaponView : MonoBehaviour
    {
        [SerializeField] private CharacterWeaponSelector playerWeaponSelector;
        [SerializeField] private CharacterShooting playerShooting;
        [SerializeField] private TMP_Text bulletsText;
        [SerializeField] private Sprite rifleSprite;
        [SerializeField] private Sprite pistolSprite;
        [SerializeField] private Sprite shotgunSprite;
        [SerializeField] private Image iconImage;
        
        private Dictionary<WeaponIdentity, Sprite> _weaponToSpritePairs;
        private Weapon _playerWeapon;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _weaponToSpritePairs = new Dictionary<WeaponIdentity, Sprite>()
            {
                { WeaponIdentity.Rifle, rifleSprite },
                { WeaponIdentity.Pistol, pistolSprite },
                { WeaponIdentity.Shotgun, shotgunSprite }
            };
            
            playerWeaponSelector.OnWeaponSelectedEvent += SetIconByType;
            playerShooting.OnSetCurrentWeaponEvent += SubscribeForBullets;
        }
        
        private void SubscribeForBullets(Weapon weapon)
        {
            weapon.OnBulletsInRowChangeEvent += SetBulletText;

            if (_playerWeapon)
            {
                _playerWeapon.OnBulletsInRowChangeEvent -= SetBulletText;
            }
            
            _playerWeapon = weapon;
        }
        
        private void SetIconByType(WeaponIdentity weaponId)
        {
            iconImage.sprite = _weaponToSpritePairs[weaponId];
        }

        private void SetBulletText(int currentBulletsInRow, int bulletsInRow)
        {
            bulletsText.text = $"{currentBulletsInRow}/{bulletsInRow}";
        }

        private void OnDestroy()
        {
            if (playerWeaponSelector)
            {
                playerWeaponSelector.OnWeaponSelectedEvent -= SetIconByType;
            }
            
            if (playerShooting)
            {
                playerShooting.OnSetCurrentWeaponEvent -= SubscribeForBullets;
            }
            
            if (_playerWeapon)
            {
                _playerWeapon.OnBulletsInRowChangeEvent -= SetBulletText;
            }
        }
    }
}