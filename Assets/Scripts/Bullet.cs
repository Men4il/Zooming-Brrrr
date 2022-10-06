using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _lifeDuration;

    private bool isCollided;
    private bool _isCausingExplosion;
    private float _damage;
    private float _chainDamageMultiplier;
    private int _chainCount;
    private LayerMask _chainLayer;
    private VFXChainHandler _vfxChainHandler;
        
    public Rigidbody Rigidbody => _rb;
    public UnityEvent<Bullet> OnDeath;
    public UnityEvent<Bullet, IDamagable> OnEnemyHit;
    public byte ID;

    private void OnEnable()
    {
        StartCoroutine(BulletLifeTime());
    }

    private void OnTriggerEnter(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamagable>();
        if (damagable != null && ID != damagable.ID)
        {
            isCollided = true;
            OnEnemyHit?.Invoke(this, damagable);

            if (_isCausingExplosion)
            {
                var explosion = ExplosionCreator.Instance.GetExplosion();
                explosion.transform.position = collider.transform.position;
            }

            if (_chainCount > 0)
            {
                var chain = new Chain(collider.transform.position, 15f, _chainCount ,_chainLayer, _vfxChainHandler, enemy => enemy.TakeDamage(_damage * _chainDamageMultiplier));
                chain.Start();
            }
        }
    }

    private IEnumerator BulletLifeTime()
    {
        var currentLifeSpan = 0f;
        while (currentLifeSpan < _lifeDuration)
        {
            currentLifeSpan += Time.deltaTime;
            
            if (isCollided)
            {
                currentLifeSpan = _lifeDuration;
                isCollided = false;
            }
            
            yield return null;
        }
        
        OnDeath?.Invoke(this);
    }

    public void SetIsExplosive(bool isExplosive)
    {
        _isCausingExplosion = isExplosive;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetChainCount(int chainCount)
    {
        _chainCount = chainCount;
    }

    public void SetChainDamageMultiplier(float chainDamageMultiplier)
    {
        _chainDamageMultiplier = chainDamageMultiplier;
    }

    public void SetChainLayer(LayerMask layer)
    {
        _chainLayer = layer;
    }

    public void SetVFXChainHandler(VFXChainHandler vfxChainHandler)
    {
        _vfxChainHandler = vfxChainHandler;
    }
}
