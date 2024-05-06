using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using Assets;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    

    [SerializeField] private int maxMoveDistance = 4;
    private int pathfindingDistanceMultiplier = 10;
    private List<Vector3> positionList;//移动目标位置

    private int currentPositionIndex;
   
    
    private float moveSpeed = 4f;
    private float stoppingDistance = 0.1f;
    private float rotationSpeed = 10f;
    
    

    protected override void Awake()
    {
        base.Awake();
    }

    public override string GetActionName()
    {
        return "Move";
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        //旋转
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        
        //防止移动后误差异常带来的困扰
        if (Vector3.Distance(transform.position,targetPosition)> stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this,EventArgs.Empty);
                ActionComplete();
            }
            
        }
        
       
    }

    /// <summary>
    /// 获取目标位置
    /// </summary>
    /// <param name="targetPositon"></param>
    /*
    public void Move(GridPosition targetPositon,Action onMoveComplete)
    {
        this.OnActionComplete = onMoveComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPositon);
        isActive = true;
    }
    */
    /*
     * 这里我的想法是不改动Move 而是 新建实现方法 TakeAction
     */
    public override void TakeAction(GridPosition targetPositon,Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetPositon,out int pathLength);
        currentPositionIndex = 0;
        this.positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        
        OnStartMoving?.Invoke(this,EventArgs.Empty);
        
        ActionStart(onActionComplete);

    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x < maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z < maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //网格非法
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //网格站着自己
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //网格站着其他单位
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    //网格上存在障碍
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    //障碍不可达
                    continue;
                }

                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    //太远不可达
                    continue;
                }
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
