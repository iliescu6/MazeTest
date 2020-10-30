using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 10;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    Pathfinding p;

    private void Start()
    {
        p = new Pathfinding(10, 10);
    }

    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y, 0, "White"));
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFrom = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.cameFrom = currentNode;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

            }
        }

        //out of nodes
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        //Left
        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        }
        //Right
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
        }
        //Down
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if (currentNode.x + 1 < grid.GetHeight())
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int y)
    {
        ;

        Debug.Log(x + "+" + y);
        return grid.gridArray[x, y];
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFrom != null)
        {
            path.Add(currentNode.cameFrom);
            currentNode = currentNode.cameFrom;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost <= lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }
}

