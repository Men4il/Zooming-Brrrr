using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")] 
    [SerializeField] private float _speed;
    [SerializeField] private float _nearDistance;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _teleportDistance;
    

    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent.stoppingDistance = _stopDistance;
        _navMeshAgent.speed = _speed;
    }
    
    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _playerTransform.position);
        var dist = Vector3.Distance(transform.position, _playerTransform.position);

        if (dist > _teleportDistance)
        {
            transform.position -= (transform.position - _playerTransform.position) * 1.5f;
        }
        
        if (dist > _stopDistance)
        {
            _navMeshAgent.destination = _playerTransform.position;
        } else if (dist <= _nearDistance)
        {
            Vector3 runTo = transform.position + ((transform.position - _playerTransform.position) * 10);
            _navMeshAgent.SetDestination(runTo);
        }
    }

    public Transform GetPlayerTransform()
    {
        return _playerTransform;
    }

    public float GetStopDistance()
    {
        return _stopDistance;
    }
    
    public float GetNearDistance()
    {
        return _nearDistance;
    }
}
