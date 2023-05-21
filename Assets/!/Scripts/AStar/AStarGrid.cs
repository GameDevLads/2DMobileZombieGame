using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.AStar
{
    public class AStarGrid : MonoBehaviour
    {
        [Tooltip("The size of the grid in world units centered in the middle of the grid. Should be set to a reasonable size as it will affect performance.")]
        public Vector2 gridWorldSize;
        [Tooltip("Whether diagonal nodes are included in the pathfinding.")]
        public bool allowDiagonals = true;

        [Tooltip("For debugging purposes, shows the A* grid when gizmos are enabled.")]
        public bool showGrid;

        //The node map
        AStarNode[,] nodes;

        private int gridX, gridY;
        private readonly float offset = 0.5f;
        public static AStarGrid instance;
        [Tooltip("The GameObject containing the colliders that will be used to build the grid.")]
        public GameObject Colliders;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
                Destroy(gameObject);
        }
        void Start()
        {
            //set gridX, gridY, offset
            gridX = Mathf.RoundToInt(gridWorldSize.x);
            gridY = Mathf.RoundToInt(gridWorldSize.y);

            //start building the grid
            BuildGrid();
        }

        void BuildGrid()
        {
            //create the map array
            nodes = new AStarNode[gridX, gridY];
            //Bottom left corner of bottom left tile
            Vector3 startPos = transform.position - new Vector3(gridWorldSize.x / 2, gridWorldSize.y / 2, 0);
            Collider2D[] colliders = Colliders.GetComponents<Collider2D>();
            //iterate across the map space
            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    //check the middle of each node
                    Vector3 checkPos = startPos + new Vector3(x + offset, y + offset, 0);
                    bool isSolid = false;

                    //if there is a collider at checkPos, then mark the node as solid
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.OverlapPoint(checkPos))
                        {
                            isSolid = true;
                            break;
                        }
                    }

                    //update the node map
                    nodes[x, y] = new AStarNode(isSolid, x, y);
                }
            }
        }

        public List<AStarNode> GetNeighbouringNodes(AStarNode currentNode)
        {
            List<AStarNode> neighboringNodes = new List<AStarNode>();

            int currentNodeX = currentNode.gridPosition.x;
            int currentNodeY = currentNode.gridPosition.y;

            // Check for horizontal and vertical neighboring nodes
            if (currentNodeX != 0)
            {
                neighboringNodes.Add(nodes[currentNodeX - 1, currentNodeY]);
            }
            if (currentNodeX != gridX - 1)
            {
                neighboringNodes.Add(nodes[currentNodeX + 1, currentNodeY]);
            }
            if (currentNodeY != 0)
            {
                neighboringNodes.Add(nodes[currentNodeX, currentNodeY - 1]);
            }
            if (currentNodeY != gridY - 1)
            {
                neighboringNodes.Add(nodes[currentNodeX, currentNodeY + 1]);
            }

            // Check for diagonal neighboring nodes
            if (allowDiagonals)
            {
                if (currentNodeX != 0 && currentNodeY != 0)
                {
                    if (!nodes[currentNodeX - 1, currentNodeY].isSolid && !nodes[currentNodeX, currentNodeY - 1].isSolid)
                    {
                        neighboringNodes.Add(nodes[currentNodeX - 1, currentNodeY - 1]);
                    }
                }
                if (currentNodeX != 0 && currentNodeY != gridY - 1)
                {
                    if (!nodes[currentNodeX - 1, currentNodeY].isSolid && !nodes[currentNodeX, currentNodeY + 1].isSolid)
                    {
                        neighboringNodes.Add(nodes[currentNodeX - 1, currentNodeY + 1]);
                    }
                }
                if (currentNodeX != gridX - 1 && currentNodeY != 0)
                {
                    if (!nodes[currentNodeX + 1, currentNodeY].isSolid && !nodes[currentNodeX, currentNodeY - 1].isSolid)
                    {
                        neighboringNodes.Add(nodes[currentNodeX + 1, currentNodeY - 1]);
                    }
                }
                if (currentNodeX != gridX - 1 && currentNodeY != gridY - 1)
                {
                    if (!nodes[currentNodeX + 1, currentNodeY].isSolid && !nodes[currentNodeX, currentNodeY + 1].isSolid)
                    {
                        neighboringNodes.Add(nodes[currentNodeX + 1, currentNodeY + 1]);
                    }
                }
            }
            return neighboringNodes;
        }


        /// <summary>
        /// Returns the grid node at the given world position
        /// </summary>
        public AStarNode GetNodeByCoords(Vector3 worldPos)
        {

            // Find the position relative to the grid's center
            Vector3 centered = worldPos - transform.position;

            // Get the position as a percentage of the grid's width and height
            // + 0.5 to ensure that the final value is between 0 and 1 because the grid's center is at (0.5,0.5) rather than (0,0)
            float percentX = centered.x / gridWorldSize.x + 0.5f;
            float percentY = centered.y / gridWorldSize.y + 0.5f;

            // Clamp between 0 and 1 to avoid out of bounds errors if actors step out of the grid 
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt(percentX * (gridX - 1));
            int y = Mathf.RoundToInt(percentY * (gridY - 1));

            if (nodes.Length == 0)
            {
                return null;
            }
            // Return the node at the calculated position
            return nodes[x, y];
        }

        /// <summary>
        /// Returns the world position of the given node.
        /// </summary>
        public Vector3 WorldPointFromNode(AStarNode node)
        {
            //Center on grid position
            Vector3 pos = transform.position;

            pos.x = pos.x - (gridWorldSize.x / 2) + (node.gridPosition.x + 0.5f);
            pos.y = pos.y - (gridWorldSize.y / 2) + (node.gridPosition.y + 0.5f);

            return pos;
        }

        /// <summary>
        /// Returns the nearest walkable node to the given node. Keep in mind that this is a very expensive operation.
        /// </summary>
        public AStarNode GetNearestWalkableNode(AStarNode node, Vector3? exactPos = null)
        {
            if (!node.isSolid)
            {
                return node;
            }
            else
            {
                List<AStarNode> neighbors = GetNeighbouringNodes(node);
                AStarNode closestNode = null;
                float closestDist = float.MaxValue;
                foreach (AStarNode n in neighbors)
                {
                    if (!n.isSolid)
                    {
                        if (exactPos == null)
                        {
                            return n;
                        }
                        float dist = Vector3.Distance(exactPos.Value, WorldPointFromNode(n));
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestNode = n;
                        }
                    }
                }
                return closestNode;
            }
        }

        private void OnDrawGizmos()
        {
            //if debugging
            if (showGrid && nodes != null && nodes.Length > 0)
            {
                //color solid nodes yellow, moveable nodes white
                foreach (AStarNode n in nodes)
                {
                    if (n.isSolid)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1));
                }
            }
        }
    }
}