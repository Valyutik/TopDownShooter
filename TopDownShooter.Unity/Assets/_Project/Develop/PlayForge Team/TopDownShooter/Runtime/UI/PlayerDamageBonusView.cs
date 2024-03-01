using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace PlayForge_Team.TopDownShooter.Runtime.UI
{
    public sealed class PlayerDamageBonusView : MonoBehaviour
    {
        [SerializeField] private Image percentsImage;
        [SerializeField] private TextMeshProUGUI multiplierText;
        private CharacterShooting _characterShooting;
        private bool _isActive;
        
        private void Start()
        {
            CharacterShooting characterShooting = FindAnyObjectByType<PlayerShooting>();
            Init(characterShooting);
        }

        private void Init(CharacterShooting characterShooting)
        {
            _characterShooting = characterShooting;
            characterShooting.SetDamageMultiplierEvent += RefreshText;
            characterShooting.ChangeDamageTimerEvent += RefreshPercents;
            SetActive(false);
        }
        
        private void OnDestroy()
        {
            if (_characterShooting)
            {
                _characterShooting.SetDamageMultiplierEvent -= RefreshText;
                _characterShooting.ChangeDamageTimerEvent -= RefreshPercents;
            }
        }
        
        private void RefreshText(float multiplier)
        {
            RefreshActivityByMultiplier(multiplier);
            multiplierText.text = $"x{(int)multiplier}";
        }

        private void RefreshActivityByMultiplier(float multiplier)
        {
            SetActive(!Mathf.Approximately(multiplier, CharacterShooting.DefaultDamageMultiplier));
        }

        private void RefreshPercents(float timer, float duration)
        {
            if (!_isActive)
            {
                return;
            }
            
            if (timer >= duration)
            {
                SetActive(false);
            }
            else
            {
                percentsImage.fillAmount = 1 - timer / duration;
            }
        }
        
        private void SetActive(bool value)
        {
            gameObject.SetActive(value);
            _isActive = value;
        }
    }
}