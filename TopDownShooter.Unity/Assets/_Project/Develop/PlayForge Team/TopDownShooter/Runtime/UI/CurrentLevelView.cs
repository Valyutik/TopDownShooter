using UnityEngine;
using TMPro;

namespace PlayForge_Team.TopDownShooter.Runtime.UI
{
    public sealed class CurrentLevelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;

        private void OnEnable()
        {
            SetLevelText(GameStateChanger.Level);
        }
        
        private void SetLevelText(int level)
        {
            levelText.text = $"Уровень {level}";
        }
    }
}