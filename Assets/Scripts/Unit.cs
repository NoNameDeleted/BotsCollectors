using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    private bool _isFree = true;
    private Resource _pickedResourse;

    public bool IsFree => _isFree;

    public event Action<Resource> ResourceStored;

    public void StartMoveToResourse(Resource resource)
    {
        StartCoroutine(nameof(MoveToResourse), resource);
    }

    public IEnumerator MoveToResourse(Resource resource)
    {
        _isFree = false;
        resource.IsFree = false;

        while (transform.position != resource.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, resource.transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        _pickedResourse = resource;
        resource.PickUpByUnit(this);
        StartCoroutine(nameof(MoveToBase));
    }

    public IEnumerator MoveToBase()
    {
        while (transform.position != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, _speed * Time.deltaTime);
            yield return null;
        }

        StoreResource(_pickedResourse);
    }

    public void StoreResource(Resource resource)
    {
        transform.DetachChildren();
        _isFree = true;
        ResourceStored?.Invoke(resource);
        _pickedResourse = null;
    }
}
