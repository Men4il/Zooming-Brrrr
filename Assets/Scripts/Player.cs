using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private float _currentHealth = 100;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _regenValue = 1;
    [SerializeField] private float _damage;
    [SerializeField] private byte _numberOfProjectiles;
    [SerializeField] private float _fireCooldown;
    [SerializeField] private float _explosionRadius = 0f;
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    [SerializeField] private float _explosionDuration;
    [SerializeField] private int _chainAmount;
    [SerializeField] private float _chainDamageMultiplier;
    
    private byte _id;

    public UnityEvent<Player> OnDeath;
    public event Action<float> OnHealthChanged;

    private void Start()
    {
        OnDeath.AddListener(PlayerDied);
        _id = 0;
    }

    private void Update()
    {
        RegenerateHealthPoints();
    }

    public byte ID
    {
        get => _id;
        set => _id = value;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        HealthChange();
        
        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke(this);
        }
    }

    private void RegenerateHealthPoints()
    {
        if (_currentHealth < _maxHealth)
        {
            _currentHealth += _regenValue * Time.deltaTime;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            HealthChange();
        }
    }

    private void HealthChange()
    {
        float currentHealthPercentage = _currentHealth / _maxHealth;
        OnHealthChanged?.Invoke(currentHealthPercentage);
    }
    
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    
    public void SetCurrentHealth(float health)
    {
        _currentHealth = health;
    }

    public void SetMaxHealth(float health)
    {
        _maxHealth = health;
    }

    public byte GetProjectilesNumber()
    {
        return _numberOfProjectiles;
    }

    public void SetProjectilesNumber(byte projectilesNum)
    {
        if (projectilesNum > 126)
        {
            _numberOfProjectiles = 127;
        }
        else
        {
            _numberOfProjectiles = projectilesNum;
        }
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    public float GetFireCooldown()
    {
        return _fireCooldown;
    }
    
    public void SetFireCooldown(float cooldown)
    {
        _fireCooldown = cooldown;
    }

    public float GetRegen()
    {
        return _regenValue;
    }

    public void SetRegen(float regen)
    {
        _regenValue = regen;
    }

    public float GetBulletsExplosionRadius()
    {
        return _explosionRadius;
    }

    public void SetBulletsExplosionRadius(float radius)
    {
        _explosionRadius = radius;
    }

    public float GetBoostersMagnetRadius()
    {
        return _radius;
    }

    public void SetBoostersMagnetRadius(float radius)
    {
        _radius = radius;
    }
    
    public float GetMovementSpeed()
    {
        return _speed;
    }

    public void SetMovementSpeed(float speed)
    {
        _speed = speed;
    }
    
    public void SetExplosionTime(float explosionTime)
    {
        _explosionDuration = explosionTime;
    }

    public float GetExplosionTime()
    {
        return _explosionDuration;
    }

    public int GetChainAmount()
    {
        return _chainAmount;
    }

    public void SetChainAmount(int chainAmount)
    {
        _chainAmount = chainAmount;
    }

    public float GetChainDamageMultiplier()
    {
        return _chainDamageMultiplier;
    }

    public void SetChainDamageMultiplier(float chainDamageMultiplier)
    {
        _chainDamageMultiplier = chainDamageMultiplier;
    }

    private void PlayerDied(Player player)
    {
        Application.LoadLevel(0); // Player died situation (temporary).
    }
}
