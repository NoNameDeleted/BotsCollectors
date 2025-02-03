using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ResourceGenerator _generator;
    [SerializeField] private List<Unit> _allUnits;
    [SerializeField] private int _resourceCapacity = 30;
    [SerializeField] private float _scanRadius = 15.0f;
    [SerializeField] private float _scanRate = 1.0f;

    private int _storedResourceCount = 0;
    private List<Resource> _freeResources;
    private List<Unit> _freeUnits;

    private void Awake()
    {
        _freeResources = new List<Resource>();
        _freeUnits = new List<Unit>();
    }

    private void Start()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.ResourceStored += AddResource;
        }

        foreach (Unit unit in _allUnits)
        {
            _freeUnits.Add(unit);
        }

        InvokeRepeating(nameof(Scann), 0.0f, _scanRate);
    }

    private void Update()
    {
        if ((_storedResourceCount >= _resourceCapacity) || (_freeUnits.Count <= 0) || (_freeResources.Count == 0))
            return;

        foreach (Unit unit in _allUnits)
        {
            if (unit.IsFree == false)
                continue;

            foreach (Resource resource in _freeResources)
            {
                Debug.Log(resource.IsFree);
                if (resource.IsFree == true)
                {
                    unit.StartMoveToResourse(resource);
                    _freeUnits.Remove(unit);
                    return;
                }
            }
        }
    }

    private void Scann()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);

        _freeResources.Clear();

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<Resource>(out Resource resource))
                _freeResources.Add(resource);  
        }
    }

    public void AddResource(Resource resource)
    {
        if (_storedResourceCount >= _resourceCapacity)
            return;
        
        _storedResourceCount += 1;
        _text.SetText("Resources: " + _storedResourceCount + "/" + _resourceCapacity);

        _freeUnits.Clear();

        foreach (Unit unit in _allUnits)
        {
            if (unit.IsFree == true)
            {
                _freeUnits.Add(unit);
            }
        }

        _generator.ReleaseResource(resource);
        resource.IsFree = true;
    }
}
