using System;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private int _baseCost = 5;
    private bool _isFlagSetMode = false;
    private bool _isFlagOnGround;
    private Flag _flag;
    private Camera _camera;

    public int BaseCost => _baseCost;
    public event Action FlagPlaced;
    public event Action BaseBuilt;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnMouseUp()
    {
        _isFlagSetMode = true; 
    }

    private void Update()
    {
        if (_isFlagSetMode == true)
            SetFlag();
    }

    private void SetFlag()
    {
        if (Input.GetMouseButtonDown(0) == false)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) == false)
            return;

        if (raycastHit.collider.gameObject.TryGetComponent<Ground>(out Ground ground) == false)
            return;

        if (_isFlagOnGround == false)
        {
            _flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            FlagPlaced?.Invoke();
        }
        else
        {
            _flag.transform.position = raycastHit.point;
        }

        _flag.transform.LookAt(transform);
        
        _isFlagOnGround = true;
        _isFlagSetMode = false;
    }

    public void BuildBase()
    {
        if (_isFlagOnGround && _flag != null)
        {
            Base _base = Instantiate(_basePrefab, _flag.transform.position, Quaternion.identity);
            _base.GetComponent<ResourceDistributor>().Initialize();
            Destroy(_flag.gameObject);
            _isFlagOnGround = false;
            BaseBuilt?.Invoke();
        }
    }
}
