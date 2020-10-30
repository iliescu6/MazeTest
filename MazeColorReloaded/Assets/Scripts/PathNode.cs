using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{ 
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;
    public string color;
    public BlockType blockType;

    public PathNode cameFrom;


    public PathNode(Grid<PathNode> grid, int x, int y, int blockType, string color)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.blockType = (BlockType)blockType;
        this.color = color;
        if (this.blockType == BlockType.ColorWalkable)
        {
            //set color based on dictionary
        }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}

//[System.Serializable]
//public class SerializableGrid
//{
//    public int gridhWidth = 0;
//    public int gridHeight = 0;
//    public List<NodeInfo> nodes = new List<NodeInfo>();
//    public SerializableGrid()
//    {
//        gridhWidth = 0;
//        gridHeight = 0;
//        nodes = new List<NodeInfo>();
//    }
//}

//[System.Serializable]
//public class NodeInfo
//{
//    public int x;
//    public int y;
//    public string color;
//    public int blockType;

//    public NodeInfo()
//    {
//        color = "None";
//        blockType = 0;
//    }

//    public NodeInfo(string c, int bt)
//    {
//        color = c;
//        blockType = bt;
//    }
//}

public enum BlockType { Wall, Walkable, ColorWalkable }
