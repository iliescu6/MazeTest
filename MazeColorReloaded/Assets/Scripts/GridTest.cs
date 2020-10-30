﻿using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField]
    float cellSize;
    Grid<PathNode> grid;

    private Pathfinding pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<PathNode>(10, 10, cellSize, new Vector3(0, 0, 0));
        pathfinding = new Pathfinding(10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // grid.SetValue(UtilsClass.GetMouseWorldPosition(), new NodeInfo());
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0,0,x,y);
            if (path != null)
            {
                for (int i=0;i<path.Count-1;i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y)*10f+Vector3.one*5f,new Vector3(path[i+1].x, path[i+1].y)*10f+Vector3.one*5f,Color.green,100);
                }
            }
        }
    }
}