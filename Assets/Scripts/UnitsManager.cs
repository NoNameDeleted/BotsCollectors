using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField] private List<Unit> _allUnits;
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _newUnitCost = 3;

    private List<Unit> _freeUnits;

    public event Action<Resource> UnitUnloadResource;

    public bool HasFreeUnits
    {
        get
        {
            return _freeUnits.Count > 0;
        }
    }

    private void OnEnable()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.MissionCompleted += SetUnitFree;
            unit.ResourceUnloaded += UnloadResource;
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.MissionCompleted -= SetUnitFree;
            unit.ResourceUnloaded -= UnloadResource;
        }
    }

    private void Start()
    {
        _freeUnits = new List<Unit>();

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }
    }

    private void SetUnitFree(Unit unit)
    {
        _freeUnits.Add(unit);
    }

    public void SendFreeUnitToGathering(Resource resource)
    {
        if (_freeUnits.Count > 0)
        {
            Unit unit = _freeUnits[0];
            unit.StartMoveToResourse(resource);
            _freeUnits.Remove(unit);
        }
    }

    private void UnloadResource(Resource resource)
    {
        UnitUnloadResource?.Invoke(resource);
    }

    public void CreateNewUnit()
    {
        Unit unit = Instantiate(_unitPrefab, transform);
        _allUnits.Add(unit);
        _freeUnits.Add(unit);
    }
}
