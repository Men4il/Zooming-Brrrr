using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDataBase", menuName = "Custom/Booster Data Base", order = 0)]

public class BoosterDataBase : ScriptableObject
{
    [SerializeField] private List<Booster> _boosters;

    private void SetBoosterId(Booster booster, byte id)
    {
        booster.SetID(id);
    }

    private void OnValidate()
    {
        if (_boosters is { Count: > 0 })
        {
            for (int i = 0; i < _boosters.Count; i++)
            {
                SetBoosterId(_boosters[i].GetComponent<Booster>(), (byte)i);
            }
        }
    }

    public Booster PickBoosterById(byte id)
    {
        return _boosters[id];
    }
}