using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _generator;
    [SerializeField] private TextMeshProUGUI _counter;
    [SerializeField] private int _resourceCapacity = 30;

    private List<Resource> _allResources;
    private List<Resource> _freeResources;
    private int _storedResourceCount = 0;

    public bool HasFreeStorageSpace => _storedResourceCount < _resourceCapacity;

    public int StoredResourceCount => _storedResourceCount;

    private void Awake()
    {
        if (_generator == null)
        {
            _generator = FindObjectOfType<ResourceGenerator>();
        }

        if (_counter == null)
        {
            _counter = FindFirstObjectByType<Counter>().GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnEnable()
    {
        _generator.ResourseGenerated += SetResourceFree;
    }

    private void OnDisable()
    {
        _generator.ResourseGenerated -= SetResourceFree;
    }

    private void Start()
    {
        _allResources = new List<Resource>();
        _freeResources = new List<Resource>();
    }

    private void UpdateCounter()
    {
        _counter.SetText("Resources: " + _storedResourceCount + "/" + _resourceCapacity);
    }

    public void AddResource(Resource resource)
    {
        if (resource == null) 
            return;

        if (_allResources == null) 
            _allResources = new List<Resource>();

        if (_allResources.Contains(resource) == false)
            _allResources.Add(resource);
    }

    public bool TryFindFreeResource(out Resource freeResource)
    {
        if (_freeResources == null) 
            _freeResources = new List<Resource>();

        if (_freeResources.Count > 0)
        {
            freeResource = _freeResources[0];
            return true;
        }
        else
        {
            freeResource = null;
            return false;
        }
    }

    public void OccupyResource(Resource resource)
    {
        _freeResources.Remove(resource);
    }

    public void SetResourceFree(Resource resource)
    {
        if (_freeResources == null) 
            _freeResources = new List<Resource>();

        _freeResources.Add(resource);
    }

    public void StoreResource(Resource resource)
    {
        if (_storedResourceCount >= _resourceCapacity)
            return;
        
        _storedResourceCount += 1;
        UpdateCounter();
        _generator.ReleaseResource(resource);
    }

    public void SpendResources(int amount)
    {
        if (amount > 0)
        {
            _storedResourceCount -= amount;
        }
            
        UpdateCounter();
    }

    public void Initialize()
    {
        _allResources = new List<Resource>();
        _freeResources = new List<Resource>();
    }
}
