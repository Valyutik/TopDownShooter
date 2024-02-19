using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Characters
{
    public abstract class CharacterHealthView : MonoBehaviour
    {
        [SerializeField] private Transform percentsImageTransform;
        private CharacterHealth _characterHealth;
        
        public void Init(CharacterHealth characterHealth)
        {
            _characterHealth = characterHealth;
            _characterHealth.OnAddHealthPoints += OnRefresh;
        }
        
        private void OnDestroy()
        {
            _characterHealth.OnAddHealthPoints -= OnRefresh;
        }
        
        private void OnRefresh()
        {
            var percents = (float) _characterHealth.GetHealthPoints() / _characterHealth.GetStartHealthPoints();
            percents = Mathf.Clamp01(percents);
            SetPercents(percents);
        }
        
        private void SetPercents(float value)
        {
            percentsImageTransform.localScale = new Vector3(value, 1, 1);
        }
    }
}