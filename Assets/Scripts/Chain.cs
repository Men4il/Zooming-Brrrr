using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Chain
{
    private Collider[] _enemiesColliders = new Collider[100];
    private List<Enemy> _chainedEnemies;

    private LayerMask _layer;
    private int _currentFoundNearEnemies;
    private Vector3 _startPos;
    private float _radius;
    private int _chainCount;
    private ChainCallback _callback;
    private VFXChainHandler _vfxChainHandler;

    public Chain(Vector3 startingPosition, float radius, int chainCount,LayerMask chainLayer,VFXChainHandler vfxChainHandler, ChainCallback callback)
    {
        _startPos = startingPosition;
        _radius = radius;
        _chainCount = chainCount;
        _layer = chainLayer;
        _callback = callback;
        _vfxChainHandler = vfxChainHandler;

        _chainedEnemies = new List<Enemy>();
    }

    public async void Start()
    {
        var currentStartPos = _startPos;

        for (int i = 0; i < _chainCount; i++)
        {
            var chainLinkData = await ChainLink(currentStartPos);

            if (!chainLinkData.Completed)
            {
                break;
            }
            currentStartPos = chainLinkData.EndingPosition;
        }
    }

    private async Task<ChainLinkData> ChainLink(Vector3 startingPosition)
    {
        var linkData = new ChainLinkData();
        var count = Physics.OverlapSphereNonAlloc(startingPosition, _radius, _enemiesColliders, _layer);

        if (count > 0)
        {
            
            var chainEnemy = FindMinDistanceEnemy(startingPosition, count);
            _chainedEnemies.Add(chainEnemy);
            linkData.EndingPosition = chainEnemy.transform.position;
            await _vfxChainHandler.CreateVFXChain(startingPosition, linkData.EndingPosition);
            
            _callback(chainEnemy);

            linkData.Completed = true;
        }

        return linkData;
    }
    
    private struct ChainLinkData
    {
        public bool Completed;
        public Vector3 EndingPosition;
    }

    private Enemy FindMinDistanceEnemy(Vector3 startingPosition, int count)
    {
        var minDistance = _radius;
        var index = 0;

        for (int i = 0; i < count; i++)
        {
            var collider = _enemiesColliders[i];
            if (_chainedEnemies.Exists(enemy => enemy.gameObject == collider.gameObject)) continue;
            
            var currentDistance = Vector3.Distance(startingPosition, collider.transform.position);

            if (currentDistance < minDistance && currentDistance > 0.5f)
            {
                minDistance = currentDistance;
                index = i;
            }
        }

        return _enemiesColliders[index].GetComponent<Enemy>();
    }

    public delegate void ChainCallback(Enemy enemy);
}