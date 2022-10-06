using System.Collections;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AreaFloorBaker : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface _surface;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private float _updateRate = 0.1f;
    [SerializeField]
    private float _movementThreshold = 3f;
    [SerializeField]
    private Vector3 _navMeshSize = new Vector3(20, 20, 20);

    private Vector3 _worldAnchor;
    private NavMeshData _navMeshData;
    private List<NavMeshBuildSource> _sources = new List<NavMeshBuildSource>();

    private void Start()
    {
        _navMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(_navMeshData);
        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());
    }

    private IEnumerator CheckPlayerMovement()
    {
        WaitForSeconds wait = new WaitForSeconds(_updateRate);

        while (true)
        {
            if (Vector3.Distance(_worldAnchor, _player.transform.position) > _movementThreshold)
            {
                BuildNavMesh(true);
                _worldAnchor = _player.transform.position;
            }

            yield return wait;
        }
    }

    private void BuildNavMesh(bool async)
    {
        Bounds navMeshBounds = new Bounds(_player.transform.position, _navMeshSize);
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();

        List<NavMeshModifier> modifiers;
        if (_surface.collectObjects == CollectObjects.Children)
        {
            modifiers = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
        }
        else
        {
            modifiers = NavMeshModifier.activeModifiers;
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            if (((_surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1)
                && modifiers[i].AffectsAgentType(_surface.agentTypeID))
            {
                markups.Add(new NavMeshBuildMarkup()
                {
                    root = modifiers[i].transform,
                    overrideArea = modifiers[i].overrideArea,
                    area = modifiers[i].area,
                    ignoreFromBuild = modifiers[i].ignoreFromBuild
                });
            }
        }

        if (_surface.collectObjects == CollectObjects.Children)
        {
            NavMeshBuilder.CollectSources(transform, _surface.layerMask, _surface.useGeometry, _surface.defaultArea, markups, _sources);
        }
        else
        {
            NavMeshBuilder.CollectSources(navMeshBounds, _surface.layerMask, _surface.useGeometry, _surface.defaultArea, markups, _sources);
        }

        _sources.RemoveAll(source => source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);

        if (async)
        {
            NavMeshBuilder.UpdateNavMeshDataAsync(_navMeshData, _surface.GetBuildSettings(), _sources, new Bounds(_player.transform.position, _navMeshSize));
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(_navMeshData, _surface.GetBuildSettings(), _sources, new Bounds(_player.transform.position, _navMeshSize));
        }
    }
}