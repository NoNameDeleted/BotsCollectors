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

    private enum Priority
    {
        CreateUnits,
        BuildBase
    }

    private Priority _priorityState;

    private void Start()
    {
        _scanner.ResorceFounded += GatherResouses;
        _priorityState = Priority.CreateUnits;
    }

    private void OnEnable()
    {
        _unitsDepot.UnitUnloadResource += _resourceStorage.StoreResource;
        _unitsDepot.UnitAimedAtResource += _resourceDistibutor.ReserveResource;
        _unitsDepot.ReadyForNewTask += GatherResouses;
        _scanner.StartScanning();
        _builder.FlagPlaced += SetBuildPriority;
        _builder.BaseBuilt += SetCreateUnitsPriority;
    }

    private void OnDisable()
    {
        _unitsDepot.UnitUnloadResource -= _resourceStorage.StoreResource;
        _unitsDepot.UnitAimedAtResource -= _resourceDistibutor.ReserveResource;
        _unitsDepot.ReadyForNewTask += GatherResouses;
        _scanner.ResorceFounded += GatherResouses;
        _builder.FlagPlaced -= SetBuildPriority;
        _builder.BaseBuilt -= SetCreateUnitsPriority;
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
        if (_resourceStorage.HasFreeStorageSpace == true)
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

    private void SetBuildPriority()
    {
        _priorityState = Priority.BuildBase;
    }

    private void SetCreateUnitsPriority()
    {
        _priorityState = Priority.CreateUnits;
    }
}
