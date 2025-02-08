using UnityEngine;
using TMPro;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter;
    [SerializeField] private ResourceGenerator _generator;
    [SerializeField] private int _resourceCapacity = 30;

    private int _storedResourceCount = 0;

    public bool HasFreeStorageSpace => _storedResourceCount < _resourceCapacity;
    public int StoredResourceCount => _storedResourceCount;

    private void Awake()
    {
        if (_counter == null)
        {
            _counter = FindObjectOfType<Counter>().GetComponent<TextMeshProUGUI>();
        }
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
            _generator.ReleaseResource(resource);
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
