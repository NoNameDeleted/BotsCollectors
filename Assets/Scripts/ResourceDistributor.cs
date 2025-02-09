using System.Collections.Generic;
using UnityEngine;

public class ResourceDistributor : MonoBehaviour
{
    private List<Resource> _allResources;
    private List<Resource> _freeResources;


    public void Awake()
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
        }
        else
        {
            freeResource = null;
        }

        return _freeResources.Count > 0;
    }

    public void SetResourceFree(Resource resource) => _freeResources.Add(resource);

    public void ReserveResource(Resource resource) => _freeResources.Remove(resource);
}
