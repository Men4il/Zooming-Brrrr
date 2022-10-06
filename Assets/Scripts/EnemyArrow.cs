using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _lifeDuration;

    private bool isCollided;

    public Rigidbody Rigidbody => _rb;
    public UnityEvent<EnemyArrow> OnDeath;
    public UnityEvent<EnemyArrow, Player> OnEnemyHit;

    private void OnEnable()
    {
        StartCoroutine(ArrowLifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCollided = true;
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            OnEnemyHit?.Invoke(this, player);
        }
    }

    private IEnumerator ArrowLifeTime()
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
}
