using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AStar
{
    public class AStarNode
    {
        public Vector2Int gridPosition;
        public Vector3 worldPosition;
        public int gCost, hCost; // gCost - cost from start to current node, hCost - cost from current node to end node
        public int fCost // fCost - total cost of the node
        {
            get
            {
                return gCost + hCost;
            }
        }
        public bool isSolid = false;
        // parent is used to generate the path back to the starting node
        public AStarNode parent;
        public List<AStarNode> neighbours;
        public AStarNode(bool solid, int x, int y)
        {
            this.gridPosition = new Vector2Int(x, y);
            this.isSolid = solid;
        }

    }
}