using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.UI
{
    public sealed class EnemyHealthViewsController : MonoBehaviour
    {
        private const float MinViewportPosition = -0.1f;
        private const float MaxViewportPosition = 1.1f;
        private readonly Dictionary<CharacterHealth, CharacterHealthView> _enemyHealthViewPairs = new();
        
        [SerializeField] private CharacterHealthView enemyHealthViewPrefab;
        [SerializeField] private Transform enemyHealthViewsContainer;
        [SerializeField] private Vector3 deltaHealthViewPosition = new(0, 2.2f, 0);
        
        private Camera _mainCamera;
        private EnemySpawner _enemySpawner;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _mainCamera = Camera.main;
            _enemySpawner = FindAnyObjectByType<EnemySpawner>();
            CreateViewsForExistingEnemies();
            SubscribeForFutureEnemies();
        }
        
        private void Update()
        {
            RefreshViewsPositions();
        }
        
        private void CreateViewsForExistingEnemies()
        {
            var enemyHealths = FindObjectsOfType<EnemyHealth>();

            foreach (var enemyHealth in enemyHealths)
            {
                CreateEnemyHealthView(enemyHealth);
            }
        }
        
        private void SubscribeForFutureEnemies()
        {
            _enemySpawner.OnSpawnEnemyEvent += CreateEnemyHealthView;
        }

        private void CreateEnemyHealthView(Character enemy)
        {
            CreateEnemyHealthView(enemy.GetComponent<CharacterHealth>());
        }

        private void RefreshViewsPositions()
        {
            foreach (var pair in _enemyHealthViewPairs)
            {
                var enemyPosition = pair.Key.transform.position + deltaHealthViewPosition;

                if (!CheckPositionVisible(enemyPosition))
                {
                    continue;
                }
                SetHealthViewScreenPosition(pair.Value, enemyPosition);
            }
        }
        
        private bool CheckPositionVisible(Vector3 position)
        {
            var viewportPosition = _mainCamera.WorldToViewportPoint(position);

            return !(viewportPosition.x < MinViewportPosition) && !(viewportPosition.x > MaxViewportPosition) &&
                   !(viewportPosition.y < MinViewportPosition) && !(viewportPosition.y > MaxViewportPosition);
        }
        
        private void CreateEnemyHealthView(CharacterHealth health)
        {
            var characterHealthView = Instantiate(enemyHealthViewPrefab, enemyHealthViewsContainer);
            SetHealthViewScreenPosition(characterHealthView, health.transform.position);
            characterHealthView.Init(health);
            _enemyHealthViewPairs.Add(health, characterHealthView);
            health.OnDieWithObject += DestroyEnemyHealthView;
        }
        
        private void SetHealthViewScreenPosition(Component view,Vector3 worldPosition)
        {
            view.transform.position = _mainCamera.WorldToScreenPoint(worldPosition);
        }
        
        private void DestroyEnemyHealthView(CharacterHealth health)
        {
            var view = _enemyHealthViewPairs[health];
            _enemyHealthViewPairs.Remove(health);
            Destroy(view.gameObject);
            health.OnDieWithObject -= DestroyEnemyHealthView;
        }
    }
}