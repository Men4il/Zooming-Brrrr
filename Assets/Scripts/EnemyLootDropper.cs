using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyLootDropper : MonoBehaviour
{
    [SerializeField] private BoosterDataBase _boosterDataBase;
    
    private Enemy _enemy;
    private ObjectPool<Booster> _healthBoosters;
    private ObjectPool<Booster> _damageBoosters;
    private ObjectPool<Booster> _doubleProjectilesBoosters;
    private ObjectPool<Booster> _rateOfFireBoosters;
    private ObjectPool<Booster> _regenBoosters;
    private ObjectPool<Booster> _explosionOnEnemiesBoosters;
    private ObjectPool<Booster> _magnetBoosters;
    private ObjectPool<Booster> _movementSpeedBoosters;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<Enemy>();
        _healthBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(0)), OnBoosterGet, OnBoosterRelease);
        _damageBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(1)), OnBoosterGet, OnBoosterRelease);
        _doubleProjectilesBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(2)), OnBoosterGet, OnBoosterRelease);
        _rateOfFireBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(3)), OnBoosterGet, OnBoosterRelease);
        _regenBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(4)), OnBoosterGet, OnBoosterRelease);
        _explosionOnEnemiesBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(5)), OnBoosterGet, OnBoosterRelease);
        _magnetBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(6)), OnBoosterGet, OnBoosterRelease);
        _movementSpeedBoosters = new ObjectPool<Booster>(() => CreateBooster(_boosterDataBase.PickBoosterById(7)), OnBoosterGet, OnBoosterRelease);
    }

    private void OnEnable()
    {
        _enemy.OnDeath.AddListener(DropLoot);
    }

    private void OnDisable()
    {
        _enemy.OnDeath.RemoveListener(DropLoot);
    }

    private void DropLoot(Enemy enemy)
    {
        int proc_seed = Random.Range(0, 1000);
        
        if (proc_seed >= 0 && proc_seed < 150)
        {
            InitializeBooster(0);
        } else if (proc_seed >= 150 && proc_seed < 300)
        {
            InitializeBooster(1);
        } else if (proc_seed >= 300 && proc_seed < 305)
        {
            InitializeBooster(2);
        } else if (proc_seed >= 305 && proc_seed < 350)
        {
            InitializeBooster(3);
        } else if (proc_seed >= 350 && proc_seed < 450)
        {
            InitializeBooster(4);
        } else if (proc_seed >= 450 && proc_seed < 500)
        {
            InitializeBooster(5);
        } else if (proc_seed >= 500 && proc_seed < 650)
        {
            InitializeBooster(6);
        } else if (proc_seed >= 650 && proc_seed < 700)
        {
            InitializeBooster(7);
        }
    }

    private void InitializeBooster(byte id)
    {
        Booster booster;
        switch (id)
        {
            case 0: booster = _healthBoosters.Get();
                break;
            case 1: booster = _damageBoosters.Get();
                break;
            case 2: booster = _doubleProjectilesBoosters.Get();
                break;
            case 3: booster = _rateOfFireBoosters.Get();
                break;
            case 4: booster = _regenBoosters.Get();
                break;
            case 5: booster = _explosionOnEnemiesBoosters.Get();
                break;
            case 6: booster = _magnetBoosters.Get();
                break;
            case 7: booster = _movementSpeedBoosters.Get();
                break;
            default: booster = _healthBoosters.Get();
                throw new ArgumentOutOfRangeException();
        }
        booster.transform.SetPositionAndRotation(_enemy.transform.position,  Quaternion.Euler(0, 0, 0));
    }

    private Booster CreateBooster(Booster booster)
    {
        return Instantiate(booster, _enemy.transform.position, Quaternion.Euler(0,0,0)).GetComponent<Booster>();
    }

    private void OnBoosterGet(Booster booster)
    {
        booster.gameObject.SetActive(true);
        booster.OnDeath.AddListener(ReleaseBooster);
        booster.OnPickup.AddListener(ReleaseBooster);
    }

    private void OnBoosterRelease(Booster booster)
    {
        booster.gameObject.SetActive(false);
        booster.OnDeath.RemoveListener(ReleaseBooster);
        booster.OnPickup.RemoveListener(ReleaseBooster);
    }

    private void ReleaseBooster(Booster booster)
    {
        try
        {
            switch (booster.GetId())
            {
                case 0:
                    _healthBoosters.Release(booster);
                    break;
                case 1:
                    _damageBoosters.Release(booster);
                    break;
                case 2:
                    _doubleProjectilesBoosters.Release(booster);
                    break;
                case 3:
                    _rateOfFireBoosters.Release(booster);
                    break;
                case 4:
                    _regenBoosters.Release(booster);
                    break;
                case 5:
                    _explosionOnEnemiesBoosters.Release(booster);
                    break;
                case 6:
                    _magnetBoosters.Release(booster);
                    break;
                case 7:
                    _movementSpeedBoosters.Release(booster);
                    break;
            }
        }
        catch (InvalidOperationException)
        {
            Debug.Log("Shlyuha");
        }
    }
}
