using UnityEngine;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float _bulletForce = 20f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _projectilesSpread;
    [SerializeField] private LayerMask _chainLayer;
    [SerializeField] private VFXChainHandler _vfxChainHandler;
    
    private PlayerControlsInitializer _initializer;
    private ObjectPool<Bullet> _bullets;
    private float _currentCooldown;
    private byte _id;
    private Player _player;

    private void Awake()
    {
        _initializer = GetComponent<PlayerControlsInitializer>();
        _bullets = new ObjectPool<Bullet>(CreateBullet, OnBulletGet, OnBulletRelease);
        _id = gameObject.GetComponent<IDamagable>().ID;
        _player = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        Shoot();
    }
    
    private void Shoot()
    {
        if (_initializer.IsShooting && _currentCooldown <= 0)
        {
            int projAmount = _player.GetProjectilesNumber();
            
            for (int i = 0; i < projAmount; i++)
            {
                Vector3 startingAngle = Quaternion.Euler(0, -1 * _projectilesSpread / 2,0) * _firePoint.transform.forward;
                Vector3 shootDirection = Quaternion.Euler(0,
                                             projAmount < 2
                                                 ? _projectilesSpread / 2
                                                 : i * (_projectilesSpread / (projAmount - 1)), 0) * startingAngle;
                
                var bullet = _bullets.Get();
                bullet.transform.SetPositionAndRotation(_firePoint.position, Quaternion.LookRotation(shootDirection));

                if (CheckBulletExplosions())
                {
                    bullet.SetIsExplosive(true);
                }
                
                var rb = bullet.Rigidbody;
                bullet.Rigidbody.velocity = Vector3.zero;
                rb.AddForce(shootDirection * _bulletForce, ForceMode.Impulse);
                
                _currentCooldown = _player.GetFireCooldown();
            }
        }

        _currentCooldown -= Time.deltaTime;
    }

    private bool CheckBulletExplosions()
    {
        return _player.GetBulletsExplosionRadius() > 0f;
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>();
        bullet.SetChainLayer(_chainLayer);
        bullet.SetVFXChainHandler(_vfxChainHandler);
        bullet.ID = _id;
        return bullet;
    }

    private void OnBulletEnemyHit(Bullet bullet, IDamagable enemy)
    {
        enemy.TakeDamage(_player.GetDamage());
    }

    private void OnBulletGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.OnDeath.AddListener(ReleaseBullet);
        bullet.OnEnemyHit.AddListener(OnBulletEnemyHit);
        bullet.SetDamage(_player.GetDamage());
        bullet.SetChainCount(_player.GetChainAmount());
        bullet.SetChainDamageMultiplier(_player.GetChainDamageMultiplier());
    }

    private void OnBulletRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.OnDeath.RemoveListener(ReleaseBullet);
        bullet.OnEnemyHit.RemoveListener(OnBulletEnemyHit);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        _bullets.Release(bullet);
    }
}
