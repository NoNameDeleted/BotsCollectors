using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SphereCollider))]
public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _radius = 5;
    [SerializeField] private int _resourseMaxCount = 10;

    private ObjectPool<Resource> _pool;
    private SphereCollider _sphere;

    public event Action<Resource> ResourseGenerated;

    private void Awake()
    {
        _sphere = GetComponent<SphereCollider>();

        _pool = new ObjectPool<Resource>(
            createFunc: () => CreateResource(_prefab),
            actionOnGet: (obj) => GetResource(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => DestroyResource(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void Start()
    {
        StartCoroutine(nameof(GenerateResource));
    }

    public void ReleaseResource(Resource resource)
    {
        if (resource.gameObject.activeSelf == true)
            _pool.Release(resource);
    }

    private void GetResource(Resource resource)
    {
        resource.transform.position = GetRandomPosition();
        resource.gameObject.SetActive(true);
        ResourseGenerated?.Invoke(resource);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 positionInCircle = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
        Vector3 positionNearby = transform.position + positionInCircle * _radius;

        if (Vector3.Distance(positionNearby, transform.position) < _sphere.radius)
        {
            positionNearby = GetRandomPosition();
        }
        else
        {
            return positionNearby;
        }

        return positionNearby;
    }

    private Resource CreateResource(Resource prefab)
    {
        Resource resource = Instantiate(prefab);
        return resource;
    }

    private void DestroyResource(Resource resource)
    {
        Destroy(resource);
    }

    private void GetResource()
    {
        _pool.Get();
    }

    private IEnumerator GenerateResource()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            if (_pool.CountActive <= _resourseMaxCount)
            {
                GetResource();
            }
            
            yield return wait;
        }
    }
}
