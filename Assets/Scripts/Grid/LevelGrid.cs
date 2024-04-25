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
    
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);


    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    /// <summary>
    /// 网格位置是否合法
    /// </summary>
    /// <param name="gridPosition">网格位置</param>
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    /// <summary>
    /// 网格位置上是否存在单位
    /// </summary>
    /// <param name="gridPosition">网格位置</param>
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);

        return gridObject.HasAnyUnit();
    }
}
