using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int gridPosition;
    public Vector3 worldPosition;
    public int gCost, hCost;
    public int fCost 
    {
        get
        {
            return gCost + hCost;
        }
    }
    public bool isSolid = false;
    // parent is used to generate the path back to the starting node
    public Node parent;
    public List<Node> neighbours;
    public Node(bool solid, int x, int y)
    {
        this.gridPosition = new Vector2Int(x,y);
        this.isSolid = solid;
    }

}