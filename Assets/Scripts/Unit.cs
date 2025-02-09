using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Vector3 _pickupOffset = new Vector3(0, 0.3f, 0);
    private bool _isFree = true;
    private Resource _pickedResourse;

    public bool IsFree => _isFree;

    public event Action<Resource> ResourceUnloaded;
    public event Action<Unit> TaskCompleted;

    public void StartMoveToResourse(Resource resource)
    {
        StartCoroutine(nameof(MoveToResourse), resource);
    }

    private IEnumerator MoveToResourse(Resource resource)
    {
        _isFree = false;

        while (transform.position != resource.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, resource.transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        _pickedResourse = resource;
        StartCoroutine(MoveToBase(resource));
    }

    private IEnumerator MoveToBase(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = transform.position + _pickupOffset;

        while (transform.position != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, _speed * Time.deltaTime);
            yield return null;
        }

        UnloadResource(_pickedResourse);
    }

    private void UnloadResource(Resource resource)
    {
        transform.DetachChildren();
        ResourceUnloaded?.Invoke(resource);
        TaskCompleted?.Invoke(this);
        _pickedResourse = null;
    }
}
