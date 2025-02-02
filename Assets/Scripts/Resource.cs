using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private Vector3 _pickupOffset = new Vector3(0, 0.3f, 0);

    private bool _isFree = true;

    public bool IsFree {get; set;}

    public void PickUpByUnit(Unit unit)
    {
        transform.SetParent(unit.transform);
        transform.Translate(_pickupOffset);
    }
}
