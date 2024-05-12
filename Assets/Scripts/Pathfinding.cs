using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Transform gridDebugObjectPrefab;
        [SerializeField] private LayerMask obstaclesLayerMask;
        public static Pathfinding Instance { get; private set; }


        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private GridSystem<PathNode> gridSystem;

        private int width;
        private int height;
        private float cellSize;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one Pathfinding" + transform + " - " + Instance);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;


        }

        public void Setup(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridSystem = new GridSystem<PathNode>(width, height, cellSize,
                ((g, gridPosition) => new PathNode(gridPosition)));

            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);


            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                    float raycastOffsetDistance = 5f;
                    if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                            Vector3.up,
                            raycastOffsetDistance * 2,
                            obstaclesLayerMask))
                    {
                        GetNode(x, z).SetIsWalkable(false);
                    }
                }
            }

        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition,out int pathLength)
        {
            List<PathNode> openList = new();
            List<PathNode> closedList = new();

            PathNode startNode = gridSystem.GetGridObject(startGridPosition);
            PathNode endNode = gridSystem.GetGridObject(endGridPosition);
            openList.Add(startNode);


            //c初始化所有节点
            for (int x = 0; x < gridSystem.GetWidth(); x++)
            {
                for (int z = 0; z < gridSystem.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
            }

            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
            startNode.CalculateFCost();

            //寻点
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    //寻点结束
                    pathLength = endNode.GetFCost();
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    if (!neighbourNode.IsWalkable())
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GetGCost() +
                                         CalculateDistance(currentNode.GetGridPosition(),
                                             neighbourNode.GetGridPosition());

                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetCameFromPathNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(),
                            endGridPosition));
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            //无路可寻
            pathLength = 0;
            return null;
        }



        //计算两个网格之间的费用 
        public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
        {
            GridPosition gridPositionDistance = gridPositionA - gridPositionB;

            int xDistance = Mathf.Abs(gridPositionDistance.x);
            int zDistance = Mathf.Abs(gridPositionDistance.z);
            int remaining = Mathf.Abs(xDistance - zDistance);
            //由于可以斜着走，歇着走一格比直走两个要更省费用
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }


        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostPathNode = pathNodeList[0];

            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNodeList[i];
                }
            }

            return lowestFCostPathNode;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            GridPosition gridPosition = currentNode.GetGridPosition();
            if (gridPosition.x - 1 >= 0)
            {
                //Left
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    //Left Down
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    //Left Up
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + +1));
                }
            }

            if (gridPosition.x + 1 < gridSystem.GetWidth())
            {
                //Right
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    //Right Down
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    //Right Up
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }

            }

            if (gridPosition.z - 1 >= 0)
            {
                //Down
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Up
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
            }

            return neighbourList;
        }

        private PathNode GetNode(int x, int z)
        {
            return gridSystem.GetGridObject(new GridPosition(x, z));
        }

        /// <summary>
        /// 计算路径
        /// </summary>
        /// <param name="endNode">终结点</param>
        /// <returns>路径点</returns>
        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodeList = new List<PathNode>();
            pathNodeList.Add(endNode);

            PathNode currentNode = endNode;
            //从后往前获取点
            while (currentNode.GetCameFromPathNode() != null)
            {
                pathNodeList.Add(currentNode.GetCameFromPathNode());
                currentNode = currentNode.GetCameFromPathNode();
            }

            //倒置
            pathNodeList.Reverse();
            //获取点的位置
            List<GridPosition> gridPositionList = new List<GridPosition>();
            foreach (PathNode pathNode in pathNodeList)
            {
                gridPositionList.Add(pathNode.GetGridPosition());
            }

            return gridPositionList;
        }

        public bool IsWalkableGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).IsWalkable();
        }

        public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            return FindPath(startGridPosition, endGridPosition,out int pathLength) != null;
        }

        public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            FindPath(startGridPosition, endGridPosition,out int pathLength);
            return pathLength;
        }

        public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
        {
            gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
        }
        
    }

}