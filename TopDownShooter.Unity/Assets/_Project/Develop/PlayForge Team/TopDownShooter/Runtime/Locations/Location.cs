using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Locations
{
    public sealed class Location : MonoBehaviour
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}