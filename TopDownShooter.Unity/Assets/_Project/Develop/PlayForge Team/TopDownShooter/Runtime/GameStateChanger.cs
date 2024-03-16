using PlayForge_Team.TopDownShooter.Runtime.UI.Screens;
using PlayForge_Team.TopDownShooter.Runtime.Characters;
using PlayForge_Team.TopDownShooter.Runtime.Enemies;
using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine.SceneManagement;
using UnityEngine;
using Screen = PlayForge_Team.TopDownShooter.Runtime.UI.Screens.Screen;

namespace PlayForge_Team.TopDownShooter.Runtime
{
    public sealed class GameStateChanger : MonoBehaviour
    {
        private const string LevelKey = "Level";
        private static int _level;
        
        private CharacterHealth _playerHealth;
        private EnemySpawner _enemySpawner;
        private Screen[] _screens;
        
        public static int Level
        {
            get
            {
                if (_level <= 0)
                {
                    _level = PlayerPrefs.GetInt(LevelKey, 1);
                }
                return _level;
            }
            set
            {
                _level = value;
                PlayerPrefs.SetInt(LevelKey, _level);
            }
        }

        public void NextLevel()
        {
            Level++;
            LoadScene();
        }

        public void RestartLevel()
        {
            LoadScene();
        }

        public void DropLevel()
        {
            Level = 1;
            LoadScene();
        }
        
        private void LoadScene()
        {
            ShowScreen<LoadingScreen>();
            SceneManager.LoadSceneAsync(0);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _enemySpawner = FindObjectOfType<EnemySpawner>();
            _screens = FindObjectsOfType<Screen>(true);
            _playerHealth.OnDieEvent += LoseGame;
            _enemySpawner.OnAllEnemiesDieEvent += WinGame;
            ShowScreen<GameScreen>();
        }
        
        private void LoseGame()
        {
            ShowScreen<GameLoseScreen>();
        }

        private void WinGame()
        {
            ShowScreen<GameWinScreen>();
        }

        private void ShowScreen<T>() where T : Screen
        {
            foreach (var t in _screens)
            {
                t.SetActive(t is T);
            }
        }

        private void OnDestroy()
        {
            if (_playerHealth)
            {
                _playerHealth.OnDieEvent -= LoseGame;
            }
            
            if (_enemySpawner)
            {
                _enemySpawner.OnAllEnemiesDieEvent -= WinGame;
            }
        }
    }
}