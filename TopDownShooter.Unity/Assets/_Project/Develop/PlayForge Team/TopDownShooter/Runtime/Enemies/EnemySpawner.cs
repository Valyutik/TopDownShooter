using System;
using System.Collections.Generic;
using System.Linq;
using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        private const float MinViewportPosition = -0.1f;
        private const float MaxViewportPosition = 1.1f;
        
        public event Action<Character> OnSpawnEnemyEvent;
        
        [SerializeField] private Character enemyPrefab;
        [SerializeField] private int enemyCount = 10;
        [SerializeField] private float spawnDelay = 1f;
        
        private EnemySpawnPoint[] _spawnPoints;
        private Camera _mainCamera;
        private int _spawnedEnemyCount;
        private float _spawnTimer;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            // Генерируем новых врагов
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            if (_spawnedEnemyCount >= enemyCount)
            {
                return;
            }
            
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                SpawnEnemy();
                ResetSpawnTimer();
            }
        }
        
        private void SpawnEnemy()
        {
            var spawnPoint = GetRandomSpawnPoint();
            var newEnemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            _spawnedEnemyCount++;
            OnSpawnEnemyEvent?.Invoke(newEnemy);
        }
        
        private EnemySpawnPoint GetRandomSpawnPoint()
        {
            var possiblePoints = GetSpawnPointsOutOfCamera();

            if (possiblePoints.Count > 0)
            {
                return possiblePoints[Random.Range(0, possiblePoints.Count)];
            }
            
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        }
        
        private List<EnemySpawnPoint> GetSpawnPointsOutOfCamera()
        {
            return (from t in _spawnPoints
                let pointViewportPosition = _mainCamera.WorldToViewportPoint(t.transform.position)
                where !(pointViewportPosition.x >= MinViewportPosition) ||
                      !(pointViewportPosition.x <= MaxViewportPosition) ||
                      !(pointViewportPosition.y >= MinViewportPosition) ||
                      !(pointViewportPosition.y <= MaxViewportPosition)
                select t).ToList();
        }
        
        private void ResetSpawnTimer()
        {
            _spawnTimer = spawnDelay;
        }
    }
}