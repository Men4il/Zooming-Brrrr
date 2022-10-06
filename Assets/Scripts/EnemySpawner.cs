using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private float _minSpawnRange;
    [SerializeField] private float _maxSpawnRange;
    [SerializeField] private float _maxEnemiesCount;
    [SerializeField] private DifficultyManager _currentDifficulty;
    


    private ObjectPool<Enemy> _enemies;
    private float _currentCooldown;
    private Transform _playerTransform;

    private void Awake()
    {
        _enemies = new ObjectPool<Enemy>(CreateEnemy, OnEnemyGet, OnEnemyRelease, null, 
            true, 5, 250);
        _playerTransform = gameObject.transform;
    }

    private float GenerateRandomNumber()
    {
        float rnd = Random.Range(-1 * _maxSpawnRange, _maxSpawnRange);
        if (rnd >= 0 && math.abs(rnd) < _minSpawnRange)
        {
            return rnd + 10;
        }

        return rnd;
    }

    private void Update()
    {
        if (_enemies.CountActive < _maxEnemiesCount)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        if (_currentCooldown <= 0)
        {
            var rand_pos = new Vector3(GenerateRandomNumber(), 1, GenerateRandomNumber());
            var rand_rot = Quaternion.Euler(0, GenerateRandomNumber(), 0);
            var enemy = _enemies.Get();
            enemy.SetHealth(enemy.GetHealth() * _currentDifficulty.GetDifficulty());
            //Debug.Log($"Health: {enemy.GetHealth()}, Difficulty: {_currentDifficulty.GetDifficulty()}");
            enemy.transform.SetPositionAndRotation(_playerTransform.position + rand_pos, rand_rot);
            _currentCooldown = _spawnCooldown;
        }

        _currentCooldown -= Time.deltaTime;
    }

    private Enemy CreateEnemy()
    {
        return Instantiate(_enemyPrefab,
             _playerTransform.position + new Vector3(GenerateRandomNumber(), 0, GenerateRandomNumber()),
            Quaternion.Euler(0, GenerateRandomNumber(), 0)).GetComponent<Enemy>();
    }
    
    private void OnEnemyGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.OnDeath.AddListener(ReleaseEnemy);
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.OnDeath.RemoveListener(ReleaseEnemy);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _enemies.Release(enemy);
    }
}
