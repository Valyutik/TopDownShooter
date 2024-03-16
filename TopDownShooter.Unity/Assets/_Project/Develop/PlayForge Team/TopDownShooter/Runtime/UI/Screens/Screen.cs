using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.UI.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}