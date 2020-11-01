using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using TMPro;

public class Grid<Template>
{
    private int width;
    public int GetWidth()
    {
        return width;
    }
    private int height;
    public int GetHeight()
    {
        return height;
    }
    private float cellSize;
    private Vector3 originPostion;
    public Template[,] gridArray;


    private TextMeshPro[,] debugGrid;

    public Grid(int width, int height, float cellSize, Transform originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPostion = originPostion;
        gridArray = new Template[width, height];
    }

    public Grid(int width, int height, GameObject prefab, float cellSize, Transform originPosition, Func<Grid<Template>, int, int, Template> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPostion = originPostion;
        gridArray = new Template[width, height];
        
        debugGrid = new TextMeshPro[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);//default(Template);
                GameObject.Instantiate(prefab, GetWorldPosition(x, y), Quaternion.identity);
                debugGrid[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + new Vector3(0, 0, -2) + originPostion;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPostion).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPostion).y / cellSize);
    }
    public void SetValue(int x, int y, Template value)
    {
        if (x >= 0 && y >= 0 && x <= width && y <= height)
        {
            gridArray[x, y] = value;
            debugGrid[x, y].text = "pp";
        }
    }

    public void SetValue(Vector3 worldPosition, Template value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public Template GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= width && y <= height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(Template);
        }
    }

    public Template GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public Template GetGridObject(int x, int y)
    {
        return gridArray[x, y];
    }
}
