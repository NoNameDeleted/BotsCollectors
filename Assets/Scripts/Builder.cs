using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _baseCost = 5;
    private bool _isFlagSetMode = false;
    private bool _isFlagOnGround;
    private Flag _flag;

    public int BaseCost => _baseCost;
    public event Action FlagPlaced;

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

    }
}
