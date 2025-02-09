using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceDistributor _resourceDistibutor;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private UnitsDepot _unitsDepot;
    [SerializeField] private UnitsSpawner _unitSpawner;
    [SerializeField] private Builder _builder;
    [SerializeField] private bool _unitsSpawnActivated;

    private ResourceGenerator _generator;

    private enum Priority
    {
        CreateUnits,
        BuildBase
    }

    private Priority _priorityState;

    private void Awake()
    {
        _generator = FindObjectOfType<ResourceGenerator>();
    }

    private void Start()
    {
        _scanner.ResorceFounded += GatherResouses;
        _priorityState = Priority.CreateUnits;
    }

    private void OnEnable()
    {
        _unitsDepot.UnitUnloadedResource += _resourceStorage.StoreResource;
        _unitsDepot.UnitReservedResource += _resourceDistibutor.ReserveResource;
        _unitsDepot.FreedUpForNewTask += GatherResouses;
        _scanner.StartScanning();
        _builder.FlagPlaced += SetBuildPriority;
        _builder.BaseBuilt += SetCreateUnitsPriority;
        _generator.ResourseGenerated += _resourceDistibutor.SetResourceFree;
        _resourceStorage.ResourceStored += _generator.ReleaseResource;
    }

    private void OnDisable()
    {
        _unitsDepot.UnitUnloadedResource -= _resourceStorage.StoreResource;
        _unitsDepot.UnitReservedResource -= _resourceDistibutor.ReserveResource;
        _unitsDepot.FreedUpForNewTask += GatherResouses;
        _scanner.ResorceFounded += GatherResouses;
        _builder.FlagPlaced -= SetBuildPriority;
        _builder.BaseBuilt -= SetCreateUnitsPriority;
        _generator.ResourseGenerated -= _resourceDistibutor.SetResourceFree;
        _resourceStorage.ResourceStored -= _generator.ReleaseResource;
    }

    private void Update()
    {
        if (_priorityState == Priority.CreateUnits)
        {
            SpawnUnits();
        }
        else if (_priorityState == Priority.BuildBase)
        {
            BuildBase();
        }
    }

    private void GatherResouses()
    {
        if (_resourceStorage.HasFreeStorageSpace)
        {
            if (_resourceDistibutor.TryFindFreeResource(out Resource freeResource))
            {
                _unitsDepot.SendFreeUnitToGathering(freeResource);
            }
        }
    }

    private void SpawnUnits()
    {
        if (_resourceStorage.StoredResourceCount >= _unitSpawner.UnitCost && _unitsSpawnActivated == true)
        {
            _resourceStorage.SpendResources(_unitSpawner.UnitCost);
            _unitsDepot.AddUnit(_unitSpawner.SpawnUnit());
        }
    }

    private void BuildBase()
    {
        if (_resourceStorage.StoredResourceCount >= _builder.BaseCost)
        {
            _resourceStorage.SpendResources(_builder.BaseCost);
            _builder.BuildBase();
        }
    }

    private void SetBuildPriority() => _priorityState = Priority.BuildBase;

    private void SetCreateUnitsPriority() => _priorityState = Priority.CreateUnits;
}
