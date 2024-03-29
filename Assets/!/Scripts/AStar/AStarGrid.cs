using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.AStar
{
    public class AStarGrid : MonoBehaviour
    {
        //Grid parameters:
        //collidableMap is the Tilemap containing collidable tiles
        //gridWorldSize is the size of the grid in world units centered in the middle of the grid
        //allowDiagonals determines whether diagonal nodes are returned as neighbor

        public Vector2 gridWorldSize;
        public bool allowDiagonals = true;
        public GameObject map;
        private Collider2D[] _colliders;

        //For debugging purposes, shows the A* grid
        public bool showGrid;

        //The node map
        AStarNode[,] nodes;

        private int gridX, gridY;
        private float offset = 0.5f;
        public static AStarGrid instance;

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

            _colliders = map.GetComponents<Collider2D>();
            //start building the grid
            BuildGrid();
        }

        bool isObjectHere(Vector3 position)
        {
            foreach (Collider2D collider in _colliders)
            {
                if (collider.bounds.Contains(position)) return true;
            }

            return false;

        }

        void BuildGrid()
        {
            //create the map array
            nodes = new AStarNode[gridX, gridY];
            //Bottom left corner of bottom left tile
            Vector3 startPos = transform.position - new Vector3(gridWorldSize.x / 2, gridWorldSize.y / 2, 0);
            //iterate across the map space
            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    //check the middle of each node
                    Vector3 checkPos = startPos + new Vector3(x + offset, y + offset, 0);
                    bool isSolid = false;

                    //if there is a collidable tile there, then mark the node as solid
                    if (isObjectHere(checkPos))
                    {
                        isSolid = true;
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



        public Vector3 WorldPointFromNode(AStarNode node)
        {
            //Center on grid position
            Vector3 pos = transform.position;

            pos.x = pos.x - (gridWorldSize.x / 2) + (node.gridPosition.x + 0.5f);
            pos.y = pos.y - (gridWorldSize.y / 2) + (node.gridPosition.y + 0.5f);

            return pos;
        }
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