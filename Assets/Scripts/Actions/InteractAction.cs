﻿using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;

namespace Actions
{
    public class InteractAction : BaseAction
    {

        private int maxInteractDistance = 1;

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

        }

        public override string GetActionName()
        {
            return "Interact";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            IInteractable interactable =  LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
            interactable.Interact(OnInteractComplete);   
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();

            for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
            {
                for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    IInteractable interactable =  LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                    if (interactable == null)
                    {   
                        //无interactable可互动
                        continue;
                    }
                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction()
            {
                gridPosition = gridPosition,
                actionValue = 0
            };
        }


        private void OnInteractComplete()
        {
            ActionComplete();
        }
    }
}