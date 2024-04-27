using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    
    private GridPosition gridPosition;//网格位置
    
    private MoveAction moveAction;
    private SpinAction spinAction;

    private BaseAction[] baseActionArray;

    private int actionPoints;
    private void Awake()
    {
        actionPoints = ACTION_POINTS_MAX;
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
         gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
         LevelGrid.Instance.AddUnitAtGridPositiion(gridPosition,this);
         TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }



    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //如果新网格位置与原网格位置不相同
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition,newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    /// <summary>
    /// 获取单位所在位置
    /// </summary>
    /// <returns>单位所在位置</returns>
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => 
        actionPoints >= baseAction.GetActionPointsCost();

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
    }

    public int GetActionPoints() => actionPoints;
    
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        actionPoints = ACTION_POINTS_MAX;
        OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);

    }
}


