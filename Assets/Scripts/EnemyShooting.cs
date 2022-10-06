using UnityEngine;
using UnityEngine.Pool;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private float _arrowForce = 20f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _globalCooldown;

    private ObjectPool<Bullet> _arrows;
    private float _currentCooldown;
    private Transform _playerTransform;
    private float _nearDistance;
    private byte _id;
    private Enemy _enemy;
    private DifficultyManager _difficultyManager;
    private float _currentDamage;
    
    void Awake()
    {
        _arrows = new ObjectPool<Bullet>(CreateBullet, OnBulletGet, OnBulletRelease);
        _enemy = gameObject.GetComponent<Enemy>();
    }

    private void Start()
    {
        _playerTransform = GetComponent<EnemyAI>().GetPlayerTransform();
        _nearDistance = GetComponent<EnemyAI>().GetNearDistance();
        _id = gameObject.GetComponent<IDamagable>().ID;
        _difficultyManager = _playerTransform.gameObject.GetComponent<DifficultyManager>();
    }

    void Update()
    {
        Shoot();
    }
    
    private void Shoot()
    {
        if (_currentCooldown <= 0)
        {
            _currentCooldown = 0;
            if (Vector3.Distance(transform.position, _playerTransform.position) >= _nearDistance)
            {
                _currentDamage = _enemy.GetDamage();
                var bullet = _arrows.Get();
                var rb = bullet.Rigidbody;
                _currentDamage *= _difficultyManager.GetDifficulty();
                //Debug.Log(_currentDamage);
                bullet.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
                bullet.Rigidbody.velocity = Vector3.zero;
                rb.AddForce(_firePoint.forward * _arrowForce, ForceMode.Impulse);

                _currentCooldown = _globalCooldown;
            }
        }

        _currentCooldown -= Time.deltaTime;
    }
    
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>();
        bullet.ID = _id;
        return bullet;
    }

    public void EnemyHit(Bullet bullet, IDamagable player)
    {
        player.TakeDamage(_currentDamage);
    }

    private void OnBulletGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.OnDeath.AddListener(ReleaseBullet);
        bullet.OnEnemyHit.AddListener(EnemyHit);
    }

    private void OnBulletRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.OnDeath.RemoveListener(ReleaseBullet);
        bullet.OnEnemyHit.RemoveListener(EnemyHit);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        _arrows.Release(bullet);
    }
}
