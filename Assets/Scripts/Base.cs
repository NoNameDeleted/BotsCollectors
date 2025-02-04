using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private UnitsManager _unitsManager;

    private void Start()
    {
        _scanner.StartScanning();
    }

    private void OnEnable()
    {
        _unitsManager.UnitUnloadResource += _resourceManager.StoreResource;
    }

    private void OnDisable()
    {
        _unitsManager.UnitUnloadResource -= _resourceManager.StoreResource;
    }

    private void Update()
    {
        if (_resourceManager.HasFreeSpace == true)
        {
            if (_resourceManager.TryFindFreeResource(out Resource freeResource))
            {
                _unitsManager.SendFreeUnitToGathering(freeResource);
            }
                
        }
    }
}
