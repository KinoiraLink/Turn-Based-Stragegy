using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebufObjectPrefab;

    public static LevelGrid Instance { get; private set; }

    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        gridSystem = new GridSystem(10,10,2f);

        gridSystem.CreateDebugObjects(gridDebufObjectPrefab);
    }

    public void AddUnitAtGridPositiion(GridPosition gridPosition,Unit unit)
    {
        GridObject gridOject = gridSystem.GetGridObject(gridPosition);
        gridOject.AddUnit(unit);
    }

    public  List<Unit> GetUnitListGridPosition(GridPosition gridPosition)
    {
        GridObject gridOject = gridSystem.GetGridObject(gridPosition);
        return gridOject.GetUnitList();
    }

    public void RemoveUniAtGridPosition(GridPosition gridPosition,Unit unit)
    {
        GridObject gridOject = gridSystem.GetGridObject(gridPosition);
        gridOject.RemoveUnit(unit);

    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPositiong, GridPosition toGridPosition)
    {
        RemoveUniAtGridPosition(fromGridPositiong,unit);
        AddUnitAtGridPositiion(toGridPosition,unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
}
