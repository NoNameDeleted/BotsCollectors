using System.Collections.Generic;
using UnityEngine;

public class ResourceDistributor : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _generator;

    private List<Resource> _allResources;
    private List<Resource> _freeResources;


    private void Awake()
    {
        _allResources = new List<Resource>();
        _freeResources = new List<Resource>();

        if (_generator == null)
            _generator = FindObjectOfType<ResourceGenerator>();
        
    }

    private void OnEnable()
    {
        _generator.ResourseGenerated += _freeResources.Add;
    }

    private void OnDisable()
    {
        _generator.ResourseGenerated -= _freeResources.Add;
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

    public void ReserveResource(Resource resource)
    {
        _freeResources.Remove(resource);
    }

    public void Initialize()
    {
        _allResources = new List<Resource>();
        _freeResources = new List<Resource>();
    }
}
