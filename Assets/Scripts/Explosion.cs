using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _playerMultiplier = 0.6f;
    
    private bool isCollided;
    private Player _player;
    
    public UnityEvent<Explosion> OnDeath;
    public UnityEvent<Explosion, IDamagable> OnEnemyHit;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        StartCoroutine(ExplosionLifeTime());
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        var enemy = collider.gameObject.GetComponent<Enemy>();
        Debug.Log($"{collider.gameObject.name} has entered the trigger!");
        if (enemy != null && enemy.ID == 1 && !enemy.IsExploded)
        {
            isCollided = true;
            OnEnemyHit?.Invoke(this, enemy);
            
            enemy.IsExploded = true;
            enemy.SetExplosionCooldown(_player.GetExplosionTime());
            
            var explosion = ExplosionCreator.Instance.GetExplosion();
            explosion.transform.position = collider.transform.position;
        }
    }
    
    private IEnumerator ExplosionLifeTime()
    {
        var currentLifeSpan = 0f;
        while (currentLifeSpan < _player.GetExplosionTime())
        {
            currentLifeSpan += Time.deltaTime;

            yield return null;
        }
        
        OnDeath?.Invoke(this);
    }

    public float GetDamageMultiplier()
    {
        return _playerMultiplier;
    }
}