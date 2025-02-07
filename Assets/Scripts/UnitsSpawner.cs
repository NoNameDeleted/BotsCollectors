using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _unitCost = 3;

    public int UnitCost => _unitCost;

    public Unit SpawnUnit()
    {
        return Instantiate(_unitPrefab, transform);
    }
}
