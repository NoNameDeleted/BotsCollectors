using UnityEngine;
using TMPro;
using System;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private int _resourceCapacity = 30;

    private TextMeshProUGUI _counter;
    private int _storedResourceCount = 0;

    public event Action<Resource> ResourceStored;

    public bool HasFreeStorageSpace => _storedResourceCount < _resourceCapacity;
    public int StoredResourceCount => _storedResourceCount;

    private void Awake()
    {
        _counter = FindObjectOfType<Counter>().GetComponent<TextMeshProUGUI>();
    }

    private void UpdateCounter()
    {
        _counter.SetText("Resources: " + _storedResourceCount + "/" + _resourceCapacity);
    }

    public void StoreResource(Resource resource)
    {
        if (_storedResourceCount < _resourceCapacity)
        {
            _storedResourceCount += 1;
            ResourceStored?.Invoke(resource);
            UpdateCounter();
        }
    }

    public void SpendResources(int amount)
    {
        if (amount > 0)
        {
            _storedResourceCount -= amount;
            UpdateCounter();
        }
    }
}
