using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private bool isEnemy;

    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    
    
    private GridPosition gridPosition;//网格位置
    private HealthSystem healthSystem;
    
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;


    private BaseAction[] baseActionArray;

    private int actionPoints;
    private void Awake()
    {
        actionPoints = ACTION_POINTS_MAX;
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>(); 

    }

    private void Start()
    {
         gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
         LevelGrid.Instance.AddUnitAtGridPositiion(gridPosition,this);
         TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
         healthSystem.OnDead += HealthSystem_OnDead;
         
         OnAnyUnitSpawned?.Invoke(this,EventArgs.Empty);
    }




    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //如果新网格位置与原网格位置不相同
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this,oldGridPosition,newGridPosition);
            
        }
    }

    public MoveAction GetMoveAction() => moveAction;

    public SpinAction GetSpinAction() => spinAction;

    public ShootAction GetShootAction() => shootAction;


    /// <summary>
    /// 获取单位所在位置
    /// </summary>
    /// <returns>单位所在位置</returns>
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }


    public Vector3 GetWorldPosition()
    {
        return transform.position;
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
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
        }
     
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUniAtGridPosition(gridPosition,this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this,EventArgs.Empty); 
    }

    public float GetHealthNormalized() => healthSystem.GetHealthNormalized();
}


