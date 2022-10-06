using System;
using UnityEngine;
using UnityEngine.Pool;

public class ExplosionCreator : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _explosionPrefab;

    private ObjectPool<Explosion> _explosionsOnEnemies;

    public static ExplosionCreator Instance { get; private set; }

    private void Awake()
    {
        _explosionsOnEnemies = new ObjectPool<Explosion>(CreateEnemyExplosion, OnEnemyExplosionGet, OnEnemyExplosionRelease);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Explosion CreateEnemyExplosion()
    {
        Explosion explosion = Instantiate(_explosionPrefab, transform).GetComponent<Explosion>();
        explosion.gameObject.transform.localScale *= _player.GetBulletsExplosionRadius();
        return explosion;
    }
    
    private void OnEnemyExplosionHit(Explosion explosion, IDamagable enemy)
    {
        enemy.TakeDamage(_player.GetDamage() * explosion.GetDamageMultiplier());
    }

    private void OnEnemyExplosionGet(Explosion explosion)
    {
        if (Math.Abs(_player.GetBulletsExplosionRadius() - explosion.gameObject.transform.localScale.x) > 0f)
        {
            explosion.gameObject.transform.localScale = new Vector3(1, 1, 1) * _player.GetBulletsExplosionRadius();
        }
        explosion.gameObject.SetActive(true);
        explosion.OnDeath.AddListener(ReleaseEnemyExplosion);
        explosion.OnEnemyHit.AddListener(OnEnemyExplosionHit);
    }

    private void OnEnemyExplosionRelease(Explosion explosion)
    {
        explosion.gameObject.SetActive(false);
        explosion.OnDeath.RemoveListener(ReleaseEnemyExplosion);
        explosion.OnEnemyHit.RemoveListener(OnEnemyExplosionHit);
    }

    private void ReleaseEnemyExplosion(Explosion explosion)
    {
        _explosionsOnEnemies.Release(explosion);
    }

    public Explosion GetExplosion()
    {
        return _explosionsOnEnemies.Get();
    }
}