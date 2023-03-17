using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace Assets.Scripts.AStar
{
    public class Algorithm : MonoBehaviour
    {
        public AStarGrid grid;
        void Start()
        {
            grid = AStarGrid.instance;
        }
        public List<AStarNode> FindPath(Vector3 start, Vector3 end)
        {
            AStarNode startNode = grid.GetNodeByCoords(start);
            AStarNode endNode = grid.GetNodeByCoords(end);
            if (startNode == null || endNode == null)
            {
                return new List<AStarNode>();
            }
            if (startNode.isSolid)
            {
                startNode = grid.GetNearestWalkableNode(startNode);
                if (startNode == null)
                {
                    return new List<AStarNode>();
                }
            }
            if (endNode.isSolid)
            {
                endNode = grid.GetNearestWalkableNode(endNode, end);
                if (endNode == null)
                {
                    return new List<AStarNode>();
                }
            }


            SimplePriorityQueue<AStarNode> openList = new SimplePriorityQueue<AStarNode>(); // neighbouring nodes to be evaluated
            HashSet<AStarNode> closedList = new HashSet<AStarNode>(); // nodes already evaluated

            openList.Enqueue(startNode, Mathf.Infinity);

            while (openList.Count > 0)
            {
                AStarNode currentNode = openList.Dequeue();
                closedList.Add(currentNode);

                // If the current node is the end node, we have found the path
                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                // Check the neighbours of the current node
                foreach (AStarNode neighbour in grid.GetNeighbouringNodes(currentNode))
                {
                    // If the neighbour is not walkable or is already in the closed list, skip it
                    if (neighbour.isSolid || closedList.Contains(neighbour))
                    {
                        continue;
                    }
                    // Update costs and parent of the neighbour to generate the path
                    int newGCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newGCost < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        neighbour.gCost = newGCost;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;

                        if (!openList.Contains(neighbour)) 
                            openList.Enqueue(neighbour, neighbour.fCost);
                    }
                }
            }
            return new List<AStarNode>();
        }
        int GetDistance(AStarNode nodeA, AStarNode nodeB)
        {
            if (nodeA == null || nodeB == null)
            {
                return 0;
            }
            int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY); // 14 is the cost of a diagonal move, 10 is the cost of a horizontal or vertical move
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }

        List<AStarNode> RetracePath(AStarNode start, AStarNode end)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentNode = end;

            while (currentNode != start)
            {
                currentNode.worldPosition = grid.WorldPointFromNode(currentNode);
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

    }
}