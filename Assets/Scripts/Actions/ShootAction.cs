using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;

    }

    public class ShootAction : BaseAction
    {
        private enum State
        {
            Aiming,
            Shooting,
            Cooloff,
        }

        public event EventHandler<OnShootEventArgs> OnShoot;

        private State state;
        private int maxShootDistance;
        private float stateTimer;
        private Unit targetUnit;
        private bool canShootBullet;

        protected override void Awake()
        {
            base.Awake();
            maxShootDistance = 2;
            canShootBullet = true;
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;
        
            switch (state)
            {
                case State.Aiming:
                    Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir,
                        Time.deltaTime * rotateSpeed);
                    break;
                case State.Shooting:
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }
                    break;
                case State.Cooloff:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (stateTimer <= 0f)
            {
                NextState();
            }
        }



        private void NextState()
        {
            switch (state)
            {
                case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            Debug.Log(state);
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
        

            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
            state = State.Aiming;
            float aimingStateTime = 1f;
            stateTimer = aimingStateTime;

            canShootBullet = true;
        
            ActionStart(onActionComplete);
        }
        
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = unit.GetGridPosition();
            return GetValidActionGridPositionList(unitGridPosition);
        }

        public  List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            
            for (int x = -maxShootDistance; x < maxShootDistance; x++)
            {
                for (int z = -maxShootDistance; z < maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        //网格非法
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Math.Abs(z);
                    if (testDistance > maxShootDistance)
                    {
                        continue;
                    }
          

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        //网格没有其他单位
                        continue;
                    }

                    targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                        continue;
                    }
                    validGridPositionList.Add(testGridPosition);
                
                }
            }
            return validGridPositionList;
        }

     
        private void Shoot()
        {
            OnShoot?.Invoke(this,new OnShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unit,
            });
            const int damageValue = 40;
            targetUnit.Damage(damageValue);
        }

        public Unit GetTargetUnit() => targetUnit;

        public int GetMaxShootDistance() => maxShootDistance;
        
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
             return new EnemyAIAction()
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1-targetUnit.GetHealthNormalized()) * 100f),
            };
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition) =>
        
            GetValidActionGridPositionList(gridPosition).Count;
        
    }
}