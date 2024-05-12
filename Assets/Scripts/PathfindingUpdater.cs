using System;
using UnityEngine;

namespace Assets
{
    public class PathfindingUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyDestoryed += DestructibleCrate_AnyDestoryed;
        }

        private void DestructibleCrate_AnyDestoryed(object sender, EventArgs e)
        {
            DestructibleCrate destructibleCrate = sender as DestructibleCrate;
            Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(),true);
        }
    }
}