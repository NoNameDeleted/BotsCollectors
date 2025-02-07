using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRate = 1.0f;
    [SerializeField] private ResourceStorage _resourceManager;

    private SphereCollider _sphere;

    public event Action ResorceFounded;

    private void Awake()
    {
        _sphere = GetComponent<SphereCollider>();
    }

    public void StartScanning()
    {
        InvokeRepeating(nameof(Scan), 0.0f, _scanRate);
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _sphere.radius);
        
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<Resource>(out Resource resource))
                _resourceManager.AddResource(resource); 
        }

        ResorceFounded?.Invoke();
    }
}
