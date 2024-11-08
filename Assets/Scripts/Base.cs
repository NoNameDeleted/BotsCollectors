using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private int _resourceCapacity = 30;
    [SerializeField] private int _resourseCount = 0;
    [SerializeField] private float _scanRadius = 15.0f;
    [SerializeField] private float _scanRate = 1.0f;
    private int _freeUnitsCount = 0;
    private int _nearbyResourceCount = 0;
    private List<Resource> _resources;

    private void Start()
    {
        InvokeRepeating(nameof(Scann), 0.0f, _scanRate);
    }

    private void Update()
    {
        if ((_resourseCount < _resourceCapacity) & (_freeUnitsCount > 0))
        {
            SendUnit();
        }
    }

    private void Scann()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<Resource>(out Resource resource))
                _resources.Add(resource);
        }
    }

    private void SendUnit()
    {

    }
}
