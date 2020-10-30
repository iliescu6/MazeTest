//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using Leguar.TotalJSON;

//public class LevelEditor : EditorWindow
//{
//    // Constants
//    public const int GRID_WIDTH = 10;
//    public const int GRID_HEIGHT = 10;
//    public const int EMPTY_TILE_ID = 255;
//    public const int PATCH_SIZE = 10;
//    public const float TILE_SIZE = 1;

//    public static string FOLDER_PATH_EXPANSIONS;
//    public static string FILE_PATH_GRID_PREFAB;
//    Grid<string> grid;
//    GameObject gridGameObject;

//    Color expColor = Color.white;
//    static int selectedExpansionIndex = 0;
//    static GUIStyle labelStyle;

//    // Contains Ids for expansions;
//    static NodeInfo[,] expansionsGrid;
//    static SerializableGrid gridList=new SerializableGrid();


//    // Caches of color to repopulate our grid.
//    static Color[] colorCache;
//    static GameObject gridMesh;
//    static GameObject gridQuad;

//    // Coordinates used to paint the grid.
//    static Coordinate currentCoordinates;
//    static Coordinate selectionStartCoordinate;
//    static Coordinate lastSelectionCoordinate;

//    private static Color highlightColor = new Color(1, 1, 1, 0.3f);
//    private static Coordinate PreviousCoordinates;
//    public static NodeInfo editorNode=new NodeInfo();
//    BlockType bt;
//    public static ColorsEnum colorEnum;

//    [MenuItem("Tools/Create Colors Asset")]
//    static void CreateColorsAsser()
//    {
//        Colors c= new Colors();
//        AssetDatabase.CreateAsset(c, "Assets/MyMaterial.asset");
//    }
//    //[CreateAssetMenu(fileName = "My new asset**.myasset**", menuName = "Create My New Asset")]

//    // This is where we draw the editor GUI.
//    private void OnGUI()
//    {
//        if (currentCoordinates != null)
//        {
//            GUILayout.Label(string.Format("Mouse Coordinates: {0} - {1}", currentCoordinates.x, currentCoordinates.y));
//        }
//        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  //#########################################

//        //if (showGrid != GUILayout.Toggle(showGrid, "Show white grid"))
//        //{
//        //    showGrid = !showGrid;
//        //    gridQuad.SetActive(showGrid);
//        //}
//        //if (showPaintedGrid != GUILayout.Toggle(showPaintedGrid, "Show painted Expansions"))
//        //{
//        //    showPaintedGrid = !showPaintedGrid;
//        //    gridMesh.SetActive(showPaintedGrid);
//        //}

//        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  //#########################################
//        GUILayout.BeginHorizontal(new GUILayoutOption[] {
//                GUILayout.ExpandWidth(true),
//                GUILayout.ExpandHeight(false)
//            });
//        GUILayout.Label("Exp Name: ", GUILayout.Width(100));
//        bt = (BlockType)EditorGUI.EnumPopup(
//            new Rect(3, 310, position.width - 6, 15),
//            "Show:",
//            bt);
//        colorEnum = (ColorsEnum)EditorGUI.EnumPopup(new Rect(3,20, position.width - 6, 15), "Show:", colorEnum);

//        switch (colorEnum)
//        {
//            case ColorsEnum.White:
//                expColor = new Color(1,1,1,.3f);
//                  expColor = EditorGUILayout.ColorField(expColor, GUILayout.Width(100));
//                break;
//            case ColorsEnum.Black:
//                expColor = new Color(0, 0, 0, .3f);
//                expColor = EditorGUILayout.ColorField(expColor, GUILayout.Width(100));
//                break;
//            case ColorsEnum.Red:
//                expColor = new Color(1, 0, 0, .3f);
//                expColor = EditorGUILayout.ColorField(expColor, GUILayout.Width(100));
//                break;
//            case ColorsEnum.Yellow:
//                expColor = new Color(0, 0, 0, .3f);
//                expColor = EditorGUILayout.ColorField(expColor, GUILayout.Width(100));
//                break;
//            case ColorsEnum.Orange:
//                expColor = new Color(0, 0, 0, .3f);
//                expColor = EditorGUILayout.ColorField(expColor, GUILayout.Width(100));
//                break;
//        }

//        editorNode.blockType = (int)bt;
//        editorNode.color = colorEnum.ToString();
//        editorNode.color = EditorGUILayout.TextField("Color name:", editorNode.color);
//        GUILayout.EndHorizontal();
//        GUILayout.BeginHorizontal();
//        GUILayout.Label("Color: ", GUILayout.Width(100));
        
//        GUILayout.EndHorizontal();

//        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //#########################################

//        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //#########################################

//        GUILayout.BeginHorizontal();
//        if (GUILayout.Button("Save", GUILayout.Width(150)))
//        {
//            //string path = string.IsNullOrEmpty(openExpansionFile) ? FILE_PATH_DEFAULT_EXPANSIONS : openExpansionFile;
//            string path = FOLDER_PATH_EXPANSIONS = Application.dataPath + "/Editor/plm";
//            ExportMaze(path);
//        }
//        if (GUILayout.Button("Save As", GUILayout.Width(150)))
//        {
//            string path = EditorUtility.SaveFilePanel("Expansion json file", FOLDER_PATH_EXPANSIONS, "expansions", "json");
//            ExportMaze(path);
//        }
//        GUILayout.EndHorizontal();

//        if (GUILayout.Button("Load", GUILayout.Width(150)))
//        {
//            string path = EditorUtility.OpenFilePanel("Expansion json file", FOLDER_PATH_EXPANSIONS, "json");
//            //LoadExpansions(path);
//        }

//        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //#########################################
//    }

//    [MenuItem("Tools/Level Editor")]
//    static void OpenSceneEditor()
//    {
//        FOLDER_PATH_EXPANSIONS = Application.dataPath + "/Editor/ExpansionsEditor/";
//        FILE_PATH_GRID_PREFAB = "Assets/Editor/GridQuad.prefab";


//        // Setup our camera to be top down, ortographic.
//        SceneView.lastActiveSceneView.orthographic = true;
//        SceneView.lastActiveSceneView.rotation = Quaternion.Euler(0, 0, 0);
//        SceneView.lastActiveSceneView.pivot = new Vector3(0, 0, 0);
//        SceneView.lastActiveSceneView.isRotationLocked = true;

//        // Open our expansion editor window.
//        EditorWindow.GetWindow<LevelEditor>();

//        // Make sure we are not already subscribed.
//        SceneView.duringSceneGui -= OnSceneGUI;

//        // Subscribe to the scene GUI (required to get mouse position)
//        SceneView.duringSceneGui += OnSceneGUI;
//        labelStyle = new GUIStyle();
//        Texture2D tex2t = new Texture2D(1, 1);
//        Color color = new Color(0, 0, 0, 0.5f);
//        tex2t.SetPixel(0, 0, color);
//        tex2t.Apply();
//        labelStyle.normal.background = tex2t;
//        labelStyle.padding = new RectOffset(5, 5, 5, 1);

//        //Initialize grid
//        expansionsGrid = new NodeInfo[GRID_WIDTH, GRID_HEIGHT];
//        for (int x = 0; x < GRID_WIDTH; x++)
//        {
//            for (int y = 0; y < GRID_HEIGHT; y++)
//            {
//                expansionsGrid[x, y] = new NodeInfo();
//            }
//        }

//        // Generate the mesh we will be painting the expanions on.
//        GenerateGridMesh();

//        // Instantiate our visual grid
//        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(FILE_PATH_GRID_PREFAB);
//        gridQuad = Instantiate(go);
//    }

//    public static void GenerateGridMesh()
//    {
//        gridMesh = GenerateGridMesh(new Vector2(0, 0), new Vector2(PATCH_SIZE, PATCH_SIZE), TILE_SIZE);
//        gridMesh.transform.position = new Vector3(0, 0, -5);
//    }

//    // Creates a mesh with one quad per tile.
//    public static GameObject GenerateGridMesh(Vector2 startCoords, Vector2 size, float tileSize)
//    {
//        int sizeX = (int)size.x;
//        int sizeY = (int)size.y;

//        int numberOfTiles = sizeX * sizeY;

//        //Create our map gameObject
//        GameObject grid = new GameObject("Patch" + startCoords);

//        //Add a MeshRenderer and a MeshFilter component
//        MeshRenderer meshRenderer = grid.AddComponent<MeshRenderer>();
//        MeshFilter mf = grid.AddComponent<MeshFilter>();

//        //Allocate our vertices, triangles and colors arrays.
//        Vector3[] vertices = new Vector3[4 * numberOfTiles];
//        int[] triangles = new int[6 * numberOfTiles];
//        Color[] colors = new Color[4 * numberOfTiles];

//        meshRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
//        Color color = Color.white;
//        color.a = 0f;

//        for (int i = 0; i < sizeX; i++)
//        {
//            for (int j = 0; j < sizeY; j++)
//            {
//                Vector3 tilePosition = new Vector3(i, j) * tileSize;

//                // Change the anchor of the tile
//                float yOffset = tilePosition.y;
//                float xOffset = tilePosition.x;

//                //Create vertices for this tile
//                int startIndex = 4 * (i + j * sizeX);
//                vertices[startIndex] = new Vector3(xOffset, yOffset, 0);
//                vertices[startIndex + 1] = new Vector3(xOffset + tileSize, yOffset, 0);
//                vertices[startIndex + 2] = new Vector3(xOffset + tileSize, yOffset + tileSize, 0);
//                vertices[startIndex + 3] = new Vector3(xOffset, yOffset + tileSize, 0);

//                //Create triangles for this tile.
//                int startIndexTriangles = 6 * (i + j * sizeX);
//                triangles[startIndexTriangles] = startIndex;
//                triangles[startIndexTriangles + 1] = startIndex + 2;
//                triangles[startIndexTriangles + 2] = startIndex + 1;
//                triangles[startIndexTriangles + 3] = startIndex + 2;
//                triangles[startIndexTriangles + 4] = startIndex;
//                triangles[startIndexTriangles + 5] = startIndex + 3;

//                colors[startIndex] = color;
//                colors[startIndex + 1] = color;
//                colors[startIndex + 2] = color;
//                colors[startIndex + 3] = color;
//            }
//        }

//        //Assign mesh name, vertices, triangles and uvs.
//        Mesh mesh = new Mesh();
//        mesh.name = "Grid";
//        mesh.vertices = vertices;
//        mesh.triangles = triangles;
//        mesh.colors = colors;
//        mesh.RecalculateBounds();

//        mf.sharedMesh = mesh;
//        //Set the patch position and cache the Colors
//        colorCache = colors;

//        return grid;
//    }

//    // Saves the expansion definitions and expansions in json format;
//    private static void ExportMaze(string path)
//    {
//        gridList = new SerializableGrid();
//        gridList.gridhWidth = GRID_WIDTH;
//        gridList.gridHeight = GRID_HEIGHT;
//        //List<Expansion> expansions = new List<Expansion>();
//        for (int x = 0; x < GRID_WIDTH; x++)
//        {
//            for (int y = 0; y < GRID_HEIGHT; y++)
//            {
//                gridList.nodes.Add(expansionsGrid[x, y]);
//                gridList.nodes[gridList.nodes.Count - 1].x = x;
//                gridList.nodes[gridList.nodes.Count - 1].y = y;
//            }
//        }

//        //List<ExpansionDefinition> defs = new List<ExpansionDefinition>();
//        //defs.AddRange(expansionDefinitions.Values);
//        //ExpansionsWrapper wrapper = new ExpansionsWrapper()
//        //{
//        //    Expansions = expansions,
//        //    expDefinitions = defs
//        //};

//        string jsonText = JSON.Serialize(gridList).CreateString();//TODO fa o lista de columns cu x,y si celelalte info si cumva fa grid din ele
//        File.WriteAllText(path, jsonText);
//    }

//    // This will draw everything on the scene, and also handle the mouse input.
//    private static void OnSceneGUI(SceneView view)
//    {
//        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
//        Coordinate coordinates = new Coordinate(new Vector2(Mathf.FloorToInt(ray.origin.x / TILE_SIZE), Mathf.FloorToInt(ray.origin.y / TILE_SIZE)));
//        coordinates.x = coordinates.x < 0 ? 0 : coordinates.x;
//        coordinates.y = coordinates.y < 0 ? 0 : coordinates.y;
//        coordinates.x = coordinates.x >= PATCH_SIZE ? PATCH_SIZE - 1 : coordinates.x;
//        coordinates.y = coordinates.y >= PATCH_SIZE ? PATCH_SIZE - 1 : coordinates.y;

//        if (coordinates != currentCoordinates && selectionStartCoordinate == null) // TODO check boundaries.
//        {
//            ShowMouseMoveHighlight(coordinates.x, coordinates.y);
//            currentCoordinates = coordinates;
//        }

//        // Prevent click through.
//        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

//        // Handling the left mouse click.
//        if (Event.current.button == 0 )//Add color here
//        {
//            if (Event.current.type == EventType.MouseDrag)
//            {
//                // TODO only do this if we moved the mouse.
//                if (selectionStartCoordinate != null && lastSelectionCoordinate != coordinates)
//                {
//                    //if (lastSelectionCoordinate != null)
//                    //{
//                    //    UndoBoxFill(selectionStartCoordinate, lastSelectionCoordinate);
//                    //}
//                    FillBox(selectionStartCoordinate, coordinates, GetColorForExpansion(editorNode),editorNode);
//                    lastSelectionCoordinate = coordinates;
//                }
//            }

//            // Paint over expansions.
//            if (Event.current.type == EventType.MouseDown)
//            {
//                selectionStartCoordinate = coordinates;
//            }

//            // Finish the paint over.
//            if (Event.current.type == EventType.MouseUp)
//            {
//                UpdateExpansionGrid(selectionStartCoordinate, coordinates, new NodeInfo());
//                selectionStartCoordinate = null;
//                lastSelectionCoordinate = null;
//            }
//        }

//        // Handle the right click
//        if (Event.current.button == 1)
//        {
//            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
//            {
//                ChangeTileColor(coordinates, GetColorForExpansion(editorNode));
//                expansionsGrid[coordinates.x, coordinates.y] = new NodeInfo();

//            }
//            Event.current.Use();
//        }

//        // Scene GUI drawing
//        if (selectionStartCoordinate == null || lastSelectionCoordinate == null)
//        {
//            Handles.Label(ray.origin + new Vector3(0, -5, 0), string.Format("Coords: {0} , {1}", Mathf.FloorToInt(coordinates.x), Mathf.FloorToInt(coordinates.y)), labelStyle);
//        }
//        else
//        {
//            Coordinate size = selectionStartCoordinate - lastSelectionCoordinate;
//            Handles.Label(ray.origin + new Vector3(0, -5, 0), string.Format("Coords: {0} , {1} \nWidth: {2} Height {3}",
//                coordinates.x,
//                coordinates.y,
//                Mathf.Abs(size.x) + 1,
//                Mathf.Abs(size.y) + 1), labelStyle);
//        }

//        // Draw expansion labels.
//        //foreach (byte id in expansionLabels.Keys)
//        //{
//        //    Handles.Label(new Vector3(expansionLabels[id].x, expansionLabels[id].y, 0) * TILE_SIZE,
//        //        expansionDefinitions[id].name,
//        //        labelStyle);
//        //}
//    }

//    // Updates a rectangle from the expansion grid with a specific expansion ID;
//    public static void UpdateExpansionGrid(Coordinate startCoord, Coordinate endCoord, NodeInfo nodeInfo)//TODO in loc de byte trebuie color sau ce-mi trebuie mie
//    {
//        Coordinate from = new Coordinate(Mathf.Min(startCoord.x, endCoord.x), Mathf.Min(startCoord.y, endCoord.y));
//        Coordinate to = new Coordinate(Mathf.Max(startCoord.x, endCoord.x), Mathf.Max(startCoord.y, endCoord.y));
//        for (int i = from.x; i <= to.x; i += 1)
//        {
//            for (int j = from.y; j <= to.y; j += 1)
//            {
//                expansionsGrid[i, j] = nodeInfo;
//            }
//        }
//       // RefreshExpansionLabels();
//    }


//    // Paint a rectangle
//    public static void FillBox(Coordinate startCoord, Coordinate endCoord, Color color,NodeInfo ni)
//    {
//        Coordinate from = new Coordinate(Mathf.Min(startCoord.x, endCoord.x), Mathf.Min(startCoord.y, endCoord.y));
//        Coordinate to = new Coordinate(Mathf.Max(startCoord.x, endCoord.x), Mathf.Max(startCoord.y, endCoord.y));
//        for (int i = from.x; i <= to.x; i += 1)
//        {
//            for (int j = from.y; j <= to.y; j += 1)
//            {
//                ChangeCacheColor(new Coordinate(i, j), color,ni);
//            }
//        }
//        ApplyCacheColors();
//    }

//    // Undo the paint over a certain area (while holding the mouse click)
//    public static void UndoBoxFill(Coordinate startCoord, Coordinate endCoord)
//    {
//        Coordinate from = new Coordinate(Mathf.Min(startCoord.x, endCoord.x), Mathf.Min(startCoord.y, endCoord.y));
//        Coordinate to = new Coordinate(Mathf.Max(startCoord.x, endCoord.x), Mathf.Max(startCoord.y, endCoord.y));
//        for (int i = from.x; i <= to.x; i += 1)
//        {
//            for (int j = from.y; j <= to.y; j += 1)
//            {
//                Coordinate coord = new Coordinate(i, j);
//                ChangeCacheColor(coord, GetStoredColor(coord));
//            }
//        }
//        ApplyCacheColors();
//    }

//    // Draw the tile highlight of the current mouse position.
//    private static void ShowMouseMoveHighlight(int x, int y)
//    {
//        if (PreviousCoordinates != null)
//        {
//            ChangeTileColor(PreviousCoordinates, GetStoredColor(PreviousCoordinates));
//        }
//        PreviousCoordinates = new Coordinate(x, y);
//        ChangeTileColor(PreviousCoordinates, highlightColor);
//    }

//    private static Color GetStoredColor(Coordinate coord)
//    {
//        return GetColorForExpansion(expansionsGrid[coord.x, coord.y]);
//    }

//    // Retrieves the color of a certain expansion
//    private static Color GetColorForExpansion(NodeInfo expansionId)
//    {
//        ColorsEnum ce = (ColorsEnum)System.Enum.Parse(typeof(ColorsEnum), expansionId.color);
//        switch (ce)
//        {
//            case ColorsEnum.White:
//                return new Color(1, 1, 1, .3f);
//                break;
//            case ColorsEnum.Black:
//                return new Color(0, 0, 0, .3f);
//                break;
//            case ColorsEnum.Red:
//                return new Color(1, 0, 0, .3f);
//                break;
//            case ColorsEnum.Yellow:
//                return new Color(0, 0, 0, .3f);
//                break;
//            case ColorsEnum.Orange:
//                return new Color(0, 0, 0, .3f);
//                break;
//            default:
//                return new Color(0, 0, 0, 0);
//                break;
//        }

//       // Color c = expansionDefinitions[expansionId].color;//TODO replace this with something else
//        //Color color = new Color(.5f,.5f,.5f, 0.55f);
//        //return color;
//    }

//    private void OnDestroy()
//    {
//        // Cleanup our scene.
//        SceneView.duringSceneGui -= OnSceneGUI;
//        if (gridMesh != null)
//        {
//            DestroyImmediate(gridMesh);
//        }
//        if (gridQuad != null)
//        {
//            DestroyImmediate(gridQuad);
//        }
//        expansionsGrid = null;
//        SceneView.lastActiveSceneView.isRotationLocked = false;
//    }

//    // Changes the Color for the tile at the specified coordinates
//    private static void ChangeTileColor(Coordinate coord, Color color)
//    {
//        ChangeCacheColor(coord, color);
//        ApplyCacheColors();
//    }

//    // Change the color in our color cache for a certain tile.
//    private static void ChangeCacheColor(Coordinate coord, Color color,NodeInfo ni=null)
//    {
//        int startIndex = 4 * (coord.x + coord.y * PATCH_SIZE);
//        if (colorCache != null)
//        {
//            colorCache[startIndex] = color;
//            colorCache[startIndex + 1] = color;
//            colorCache[startIndex + 2] = color;
//            colorCache[startIndex + 3] = color;
//        }
//        if (ni != null)
//        {
//            expansionsGrid[coord.x, coord.y] = ni;
//        }
//    }

//    // Apply our colors to the mesh.
//    private static void ApplyCacheColors()
//    {
//        Mesh mesh = gridMesh.GetComponent<MeshFilter>().sharedMesh;
//        mesh.colors = colorCache;

//    }
//}
//public enum ColorsEnum { White,Black,Red,Yellow,Orange,None}