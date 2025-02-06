using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Player : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    private Camera _camera;
    private bool _isBuildMode = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        EnterInBuildMode();
        BuildFlag();
    }

    private void EnterInBuildMode()
    {
        if (Input.GetMouseButtonUp(0) == false)
            return;
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit _raycastHit) == false)
            return;

        if (_raycastHit.collider.gameObject.TryGetComponent<Base>(out Base _base) == false)
            return;

        _isBuildMode = true;
    }

    private void BuildFlag()
    {
        if (Input.GetMouseButtonDown(0) == false)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) == false)
            return;

        if (raycastHit.collider.gameObject.TryGetComponent<Ground>(out Ground ground) == false)
            return;

        Instantiate(_flag, raycastHit.point, Quaternion.identity);
        _isBuildMode = false;
    }
}
