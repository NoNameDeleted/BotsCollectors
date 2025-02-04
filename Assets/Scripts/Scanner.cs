using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 15.0f;
    [SerializeField] private float _scanRate = 1.0f;
    [SerializeField] private ResourceManager _resourceManager;

    public void StartScanning()
    {
        InvokeRepeating(nameof(Scan), 0.0f, _scanRate);
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<Resource>(out Resource resource))
                _resourceManager.AddResource(resource); 
        }
    }
}
