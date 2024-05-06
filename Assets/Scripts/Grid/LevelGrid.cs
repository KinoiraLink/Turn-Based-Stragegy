using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebufObjectPrefab;
    [SerializeField] private int width;
    [SerializeField]private int height;
    [SerializeField] private float cellSize;
    public static LevelGrid Instance { get; private set; }
    public event EventHandler OnAnyUnitMoveGridPosition;
 
    
    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        gridSystem = new GridSystem<GridObject>(width,height,cellSize,
            ((GridSystem<GridObject> g,GridPosition gridPosition) => new GridObject(g,gridPosition)));

       // gridSystem.CreateDebugObjects(gridDebufObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width,height,cellSize);
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
        OnAnyUnitMoveGridPosition?.Invoke(this,EventArgs.Empty);
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

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
