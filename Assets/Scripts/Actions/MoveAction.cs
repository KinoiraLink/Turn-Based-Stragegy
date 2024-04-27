using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField]
    private Animator unitAnimator;

    [SerializeField] private int maxMoveDistance = 4;
    
    private Vector3 targetPosition;//移动目标位置
    
    
    private float moveSpeed = 4f;
    private float stoppingDistance = 0.1f;
    private float rotationSpeed = 10f;
    
    private const string IS_WALKING = "IsWalking";
    

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    private void Start()
    {
        unitAnimator.SetBool(IS_WALKING, false);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        //防止移动后误差异常带来的困扰
        if (Vector3.Distance(transform.position,targetPosition)> stoppingDistance)
        {

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            
           

            unitAnimator.SetBool(IS_WALKING, true);
        }
        else 
        {
            unitAnimator.SetBool (IS_WALKING, false);
            isActive = false;
            OnActionComplete?.Invoke();
        }
        
        //旋转
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
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
    public override void TakeAction(GridPosition targetPositon,Action onMoveComplete)
    {
        this.OnActionComplete = onMoveComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPositon);
        isActive = true;
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
                validGridPositionList.Add(testGridPosition);
                Debug.Log(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    
}
