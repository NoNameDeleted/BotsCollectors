using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _generator;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _resourceCapacity = 30;

    private List<Resource> _allResources;
    private List<Resource> _freeResources;
    private int _storedResourceCount = 0;

    public bool HasFreeSpace
    {
        get
        {
            return _storedResourceCount < _resourceCapacity;
        }
    }

    private void OnEnable()
    {
        _generator.ResourseGenerated += SetResourceFree;
    }

    private void Awake()
    {
        _allResources = new List<Resource>();
        _freeResources = new List<Resource>();
    }

    public void AddResource(Resource resource)
    {
        if (_allResources.Contains(resource) == false)
            _allResources.Add(resource);
    }

    public bool TryFindFreeResource(out Resource freeResource)
    {
        if (_freeResources.Count > 0)
        {
            freeResource = _freeResources[0];
            _freeResources.Remove(freeResource);
            return true;
        }
        else
        {
            freeResource = null;
            return false;
        }
    }

    public void SetResourceFree(Resource resource)
    {
        _freeResources.Add(resource);
    }

    public void StoreResource(Resource resource)
    {
        if (_storedResourceCount >= _resourceCapacity)
            return;
        
        _storedResourceCount += 1;
        _text.SetText("Resources: " + _storedResourceCount + "/" + _resourceCapacity);
        _generator.ReleaseResource(resource);
    }
}
