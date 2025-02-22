﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
    
        private float totalSpinAmount;

        private void Update()
        {
            if (!isActive)
            {
                return;
            }
        
            float spinAddAmount = 360 * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360f)
            {
                ActionComplete();
            }
        }

        /*
    public void Spin(Action onSpinComplete)
    {
        this.OnActionComplete = onSpinComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }
    */
        public override void TakeAction(GridPosition gridPosition,Action onSpinComplete)
        {
            totalSpinAmount = 0f;
            ActionStart(onSpinComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = unit.GetGridPosition();

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }

        public override string GetActionName()
        {
            return "Spin";
        }

        public override int GetActionPointsCost()
        {
            return 1;
        }
    
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction()
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }
    }
}
