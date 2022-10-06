using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMagnetLogic : MonoBehaviour
{
    [SerializeField] private float _spheresCheckerCooldown = 0.3f;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Player _player;

    private Collider[] _boostersColliders = new Collider[250];
    private int _currentFoundBoosters;

    private void Start()
    {
        StartCoroutine(GetBoostersColliders());
    }

    private IEnumerator GetBoostersColliders()
    {
        while (true)
        {
            _currentFoundBoosters = GetBoostersToMagnet();
            if (_currentFoundBoosters > 0)
            {
                SetBoostersMagnetedProperty();
            }

            yield return new WaitForSeconds(_spheresCheckerCooldown);
        }
    }

    private int GetBoostersToMagnet()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, _player.GetBoostersMagnetRadius(), _boostersColliders, _layer, QueryTriggerInteraction.Collide);
    }

    private void SetBoostersMagnetedProperty()
    {
        for (int i = 0; i < _currentFoundBoosters; i++)
        {
            _boostersColliders[i].GetComponent<Booster>().SetIsMagneted(true);
        }
    }
}