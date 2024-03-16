using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Bullets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace PlayForge_Team.TopDownShooter.Runtime.Enemies
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        private const float MinViewportPosition = -0.1f;
        private const float MaxViewportPosition = 1.1f;
        
        public event Action<Character> OnSpawnEnemyEvent;
        public event Action OnAllEnemiesDieEvent;

        [SerializeField] private BulletSpawner spawner;
        [SerializeField] private EnemySpawnPoint[] spawnPoints;
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private int enemyCountByLevel = 10;
        [SerializeField] private float spawnDelay = 1f;
        
        private readonly List<CharacterHealth> _enemiesHealth = new();
        private Camera _mainCamera;
        private float _spawnTimer;
        private int _spawnedEnemyCount;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            if (_spawnedEnemyCount >= GameStateChanger.Level * enemyCountByLevel)
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
            var newEnemy = Instantiate(GetRandomEnemyPrefab(), spawnPoint.transform.position, Quaternion.identity, transform);
            newEnemy.Init(spawner);
            _spawnedEnemyCount++;
            OnSpawnEnemyEvent?.Invoke(newEnemy);
            
            var newHealth = newEnemy.GetComponent<CharacterHealth>();
            _enemiesHealth.Add(newHealth);
            newHealth.OnDieWithObject += RemoveEnemy;
        }
        
        private void RemoveEnemy(CharacterHealth health)
        {
            _enemiesHealth.Remove(health);

            health.OnDieWithObject -= RemoveEnemy;

            if (_enemiesHealth.Count <= 0)
            {
                OnAllEnemiesDieEvent?.Invoke();
            }
        }
        
        private Enemy GetRandomEnemyPrefab()
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }
        
        private EnemySpawnPoint GetRandomSpawnPoint()
        {
            var possiblePoints = GetSpawnPointsOutOfCamera();

            return possiblePoints.Count > 0
                ? possiblePoints[Random.Range(0, possiblePoints.Count)]
                : spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        
        private List<EnemySpawnPoint> GetSpawnPointsOutOfCamera()
        {
            return (from t in spawnPoints where t.isActiveAndEnabled
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