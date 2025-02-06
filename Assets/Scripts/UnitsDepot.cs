using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsDepot : MonoBehaviour
{
    [SerializeField] private List<Unit> _allUnits;

    private List<Unit> _freeUnits;

    public event Action<Resource> UnitUnloadResource;
    public event Action<Resource> UnitAimedAtResource;

    public bool HasFreeUnits
    {
        get
        {
            return _freeUnits.Count > 0;
        }
    }

    private void OnEnable()
    {
        _freeUnits = new List<Unit>();

        foreach (Unit unit in _allUnits)
        {
            unit.MissionCompleted += _freeUnits.Add;
            unit.ResourceUnloaded += UnloadResource;
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.MissionCompleted -= _freeUnits.Add;
            unit.ResourceUnloaded -= UnloadResource;
        }
    }

    private void Start()
    {
        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }
    }

    public void SendFreeUnitToGathering(Resource resource)
    {
        if (_freeUnits.Count > 0)
        {
            Unit unit = _freeUnits[0];
            unit.StartMoveToResourse(resource);
            UnitAimedAtResource?.Invoke(resource);
            _freeUnits.Remove(unit);
        }
    }

    private void UnloadResource(Resource resource)
    {
        UnitUnloadResource?.Invoke(resource);
    }

    public void AddUnit(Unit unit)
    {
        _allUnits.Add(unit);
        _freeUnits.Add(unit);
    }
}
