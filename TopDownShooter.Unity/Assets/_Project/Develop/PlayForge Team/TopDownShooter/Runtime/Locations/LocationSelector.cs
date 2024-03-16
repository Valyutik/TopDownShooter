using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Locations
{
    public sealed class LocationSelector : MonoBehaviour
    {
        [SerializeField] private Location[] locations;

        private void Awake()
        {
            Init();
        }
        
        private void Init()
        {
            SelectRandomLocation(GameStateChanger.Level);
        }
        
        private void SelectRandomLocation(int seed)
        {
            var random = new System.Random(seed);

            var selectedId = random.Next(locations.Length);

            for (var i = 0; i < locations.Length; i++)
            {
                locations[i].SetActive(i == selectedId);
            }
        }
    }
}