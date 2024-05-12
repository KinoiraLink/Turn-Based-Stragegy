using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class GridObject 
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        this.unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (var unit in this.unitList) 
        {
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit) {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) 
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList() 
    {
        return unitList;
    }
    
    /// <summary>
    /// 格子上是否存在单位
    /// </summary>
    /// <returns></returns>
    public bool HasAnyUnit() => unitList.Count > 0;

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }
}
