using CodeMonkey.Utils;
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
       // grid = new Grid<PathNode>(10, 10, cellSize, new Vector3(0, 0, 0));
        //pathfinding = new Pathfinding(10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
