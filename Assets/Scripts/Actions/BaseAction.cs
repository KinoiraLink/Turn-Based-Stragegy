using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;
    
        protected Unit unit;
        protected bool isActive = false;
        protected Action OnActionComplete ;
        protected virtual void Awake()
        {
            unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidActionGridPositionList();

        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        protected void ActionStart(Action onActionComplete)
        {
            isActive = true;
            this.OnActionComplete = onActionComplete;
        
            OnAnyActionStarted?.Invoke(this,EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            isActive = false;
            OnActionComplete();
            OnAnyActionCompleted?.Invoke(this,EventArgs.Empty);
        }

        public Unit GetUnit() => unit;

        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort(((a, b) => b.actionValue -a.actionValue));
                return enemyAIActionList[0];
            }
            else
            {
                //没有可执行的行动
                return null;
            }
        }

        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    }
}
