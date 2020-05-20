using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;

public class DungeonGenerator : MonoBehaviour
{
    public Grid grid;
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject redTilePrefab;

    public int size = 100;
    public int attempts = 100;
    public Vector2Int roomSize = new Vector2Int(5, 13);
    public int roomSpacing = 1;

    public bool instantiateDebugTiles = false;
    public bool showMST = false;
    public bool showDelaunay = false;
    public bool generateExtraEdges = false;

    public DungeonTerrainTile[,] DungeonTerrainTiles { get; private set; }
    public GameObject[,] InstancedTiles { get; private set; }
    public List<Room> Rooms { get; private set; }

    public delegate void OnComplete();
    public event OnComplete onComplete;

    TriangleNet.Mesh mesh;
    //Data saved for gizmos
    List<MST.STEdge> spanningTree = new List<MST.STEdge>();
    List<Vector3Int> pathGridTiles = new List<Vector3Int>();
    List<Vector3> worldMidpoints = new List<Vector3>();
    List<Vector3> worldCornerPoints = new List<Vector3>();

    //Center Tiles Converted to World Position
    List<Vector3> centerWorldTiles = new List<Vector3>();

    public class DungeonTerrainTile
    {
        public GridEntity gridEntity;
        public DungeonTerrainTile(TileType type)
        {
            tileType = type;
        }
        public enum TileType
        {
            Wall,
            Floor
        }
        public TileType tileType;
    }

    public struct Room
    {
        public Vector3Int centerGridTile;
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;
    }

    public void Generate()
    {
        InstancedTiles = new GameObject[size, size];
        DungeonTerrainTiles = new DungeonTerrainTile[size, size];
        Rooms = new List<Room>();

        for (int i = 0; i < attempts; i++)
        {
            CreateRoom(roomSize.x, roomSize.y);
        }

        mesh = DelaunayTriangulation.TriangulatePoints(centerWorldTiles);
        spanningTree.AddRange(MST.FormTree(mesh, centerWorldTiles.Count, generateExtraEdges));

        for (int i = 0; i < spanningTree.Count; i++)
        {
            CreatePaths(spanningTree[i].edge);
        }
        onComplete?.Invoke();

        if (instantiateDebugTiles)
        {
            InstantiateTiles();
        }
    }

    void CreatePaths(Edge edge)
    {
            Vertex v0 = mesh.vertices[edge.P0];
            Vertex v1 = mesh.vertices[edge.P1];

            Vector3Int a = new Vector3Int( (int)v0.x, (int)v0.y, 0);
            Vector3Int b = new Vector3Int( (int)v1.x, (int)v1.y, 0);

            Room roomA = GetRoom(a);
            Room roomB = GetRoom(b);

            Vector3 midpointWorld;
            midpointWorld = GetMidpoint(roomA.centerGridTile, roomB.centerGridTile);
            Vector3Int midPointGrid = grid.WorldToCell(midpointWorld);

            bool hasLine = false;
            //Range makes sure that there is enough space for a 3 wide path
            int range = 1;

            //Check Horizontal Line
            //If midpoint is within vertical range add horizontal line between the two rooms
            if (midPointGrid.y >= roomA.minY + range && midPointGrid.y <= roomA.maxY - range)
            {
                if (midPointGrid.y >= roomB.minY + range && midPointGrid.y <= roomB.maxY - range)
                {
                    hasLine = true;
                    AddStraightLine(roomA.centerGridTile.x, roomB.centerGridTile.x, midPointGrid, new Vector3Int(1, 0, 0));
                }
            }
            //Check Vertical Line
            //If midpoint is within horizontal range add vertical line between the two rooms
            else if (midPointGrid.x >= roomA.minX + range && midPointGrid.x <= roomA.maxX - range)
                {
                    if (midPointGrid.x >= roomB.minX + range && midPointGrid.x <= roomB.maxX - range)
                    {
                        hasLine = true;
                        AddStraightLine(roomA.centerGridTile.y, roomB.centerGridTile.y, midPointGrid, new Vector3Int(0, 1, 0));
                    }
                }

            //Check for L line
            if(hasLine == false)
            {
                Vector3Int cornerPointA = Vector3Int.zero;
                Vector3Int cornerPointB = Vector3Int.zero;
                cornerPointA = new Vector3Int(roomA.centerGridTile.x, roomB.centerGridTile.y, 0);
                cornerPointB = new Vector3Int(roomB.centerGridTile.x, roomA.centerGridTile.y, 0);

                worldCornerPoints.Add(grid.GetCellCenterWorld(cornerPointA));
                worldCornerPoints.Add(grid.GetCellCenterWorld(cornerPointB));

                if (DungeonTerrainTiles[cornerPointA.x, cornerPointA.y] == null || DungeonTerrainTiles[cornerPointA.x, cornerPointA.y].tileType == DungeonTerrainTile.TileType.Wall
                || DungeonTerrainTiles[cornerPointA.x, cornerPointA.y].tileType == DungeonTerrainTile.TileType.Floor)
                {
                    AddCornerLines(cornerPointA, roomA, roomB);
                }
                else if (DungeonTerrainTiles[cornerPointB.x, cornerPointB.y] == null || DungeonTerrainTiles[cornerPointB.x, cornerPointB.y].tileType != DungeonTerrainTile.TileType.Wall
                || DungeonTerrainTiles[cornerPointB.x, cornerPointB.y].tileType != DungeonTerrainTile.TileType.Floor)
                {
                    AddCornerLines(cornerPointB, roomB, roomA);
                }

            }

        //Create paths
        for (int w = 0; w < pathGridTiles.Count; w++)
        {
            DungeonTerrainTiles[pathGridTiles[w].x, pathGridTiles[w].y] = new DungeonTerrainTile(DungeonTerrainTile.TileType.Floor);
        }

        //Create paths walls
        //TODO create walls as path is created to avoid another loop
        for (int w = 0; w < pathGridTiles.Count; w++)
        {
            List<Vector3Int> neighbors = GetNeighbors(pathGridTiles[w]);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (DungeonTerrainTiles[neighbors[i].x, neighbors[i].y] == null)
                {
                    DungeonTerrainTiles[neighbors[i].x, neighbors[i].y] = new DungeonTerrainTile(DungeonTerrainTile.TileType.Wall);
                }
            }

        }
    }

    void CreateRoom(int min, int max)
    {
        Vector2Int randomTile = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
        Room room = new Room();

        int roomSizeX = Random.Range(min, max);
        int roomSizeY = Random.Range(min, max);

        //If room goes over beyod grid return
        if (roomSizeX + randomTile.x > size || roomSizeY + randomTile.y > size)
        {
            return;
        }

        int minX = randomTile.x - roomSpacing;
        int minY = randomTile.y - roomSpacing;

        int maxX = randomTile.x + roomSizeX + roomSpacing;
        int maxY = randomTile.y + roomSizeY + roomSpacing;

        Vector2Int centerTile = new Vector2Int((randomTile.x + roomSizeX / 2), (randomTile.y + roomSizeY / 2));

        //Checks if room boundaries are inside the grid
        if (minX < 0 || minY < 0 || maxX > size || maxY > size)
        {
            return;
        }

        //Checks if there are any tiles in room boundaries
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if (DungeonTerrainTiles[x, y] != null)
                {
                    return;
                }
            }
        }

        centerWorldTiles.Add(grid.GetCellCenterWorld(new Vector3Int(centerTile.x, centerTile.y, 0)));

        //Room Walls
        for (int x = randomTile.x; x <= randomTile.x + roomSizeX; x++)
        {
            for (int y = randomTile.y; y <= randomTile.y + roomSizeY; y++)
            {
                if (x == randomTile.x || y == randomTile.y || x == randomTile.x + roomSizeX || y == randomTile.y + roomSizeY)
                {
                    DungeonTerrainTiles[x, y] = new DungeonTerrainTile(DungeonTerrainTile.TileType.Wall);
                }
            }
        }

        //Room Floor
        for (int x = randomTile.x + 1; x <= randomTile.x + roomSizeX - 1; x++)
        {
            for (int y = randomTile.y + 1; y <= randomTile.y + roomSizeY - 1; y++)
            {
                DungeonTerrainTiles[x, y] = new DungeonTerrainTile(DungeonTerrainTile.TileType.Floor);
            }
        }

        room.centerGridTile = new Vector3Int(centerTile.x, centerTile.y, 0);
        room.minX = randomTile.x;
        room.maxX = randomTile.x + roomSizeX;

        room.minY = randomTile.y;
        room.maxY = randomTile.y + roomSizeY;

        Rooms.Add(room);

        return;
    }

    void InstantiateTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (DungeonTerrainTiles[x, y] == null)
                {
                    continue;
                }
                if (DungeonTerrainTiles[x, y].tileType == DungeonTerrainTile.TileType.Wall)
                {
                    GameObject tile = Instantiate(wallPrefab, grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), wallPrefab.transform.rotation);
                    InstancedTiles[x, y] = tile;
                }
                if (DungeonTerrainTiles[x, y].tileType == DungeonTerrainTile.TileType.Floor)
                {
                    GameObject tile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), tilePrefab.transform.rotation);
                    InstancedTiles[x, y] = tile;
                }

            }
        }
    }

    Vector3 GetMidpoint(Vector3 a, Vector3 b)
    {
        Vector3 midpoint = Vector3.zero;

        midpoint.x = (a.x + b.x) / 2;
        midpoint.y = (a.y + b.y) / 2;

        midpoint = grid.GetCellCenterWorld(new Vector3Int((int)midpoint.x, (int)midpoint.y, 0));
        worldMidpoints.Add(midpoint);
        return midpoint;
    }

    List<Vector3Int> GetNeighbors(Vector3Int current)
    {
        List<Vector3Int> Neighbors = new List<Vector3Int>();

        //Checks around current node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int xPos = current.x + x;
                int yPos = current.y + y;
                Vector3Int tile = new Vector3Int(xPos, yPos, 0);

                if (IsNodeInsideGrid(tile)) Neighbors.Add(tile);

            }
        }
        return Neighbors;
    }

    public bool IsNodeInsideGrid(Vector3Int tile)
    {
        if (tile.x >= 0 && tile.x < size && tile.y >= 0 && tile.y < size)
            return true;
        else
            return false;
    }

    void RemoveInstancedTile(Vector3Int tile)
    {
        //Removes tile gameobject
        Destroy(InstancedTiles[tile.x, tile.y]);
        InstancedTiles[tile.x, tile.y] = null;
    }

    void AddPathTiles(Vector3Int tile)
    {
        //If tile is empty add it to path
        if (DungeonTerrainTiles[tile.x, tile.y] == null)
        {
            pathGridTiles.Add(tile);
        }
        //If tile is a wall remove wall and add path
        else if (DungeonTerrainTiles[tile.x, tile.y].tileType == DungeonTerrainTile.TileType.Wall)
        {
            RemoveInstancedTile(tile);
            pathGridTiles.Add(tile);
        }
    }

    void AddCornerLines(Vector3Int cornerPoint, Room roomA, Room roomB)
    {
        if (cornerPoint.y < roomA.centerGridTile.y)
        {
            //roomA is above
            for (int y = cornerPoint.y; y < roomA.centerGridTile.y; y++)
            {
                AddPathTiles(new Vector3Int(cornerPoint.x, y, 0));
            }
        }
        if (cornerPoint.y > roomA.centerGridTile.y)
        {
            //roomA is below
            for (int y = cornerPoint.y; y > roomA.centerGridTile.y; y--)
            {
                AddPathTiles(new Vector3Int(cornerPoint.x, y, 0));
            }
        }
        if (cornerPoint.x < roomB.centerGridTile.x)
        {
            //roomB is right
            for (int x = cornerPoint.x; x < roomB.centerGridTile.x; x++)
            {
                AddPathTiles(new Vector3Int(x, cornerPoint.y, 0));
            }
        }
        if (cornerPoint.x > roomB.centerGridTile.x)
        {
            //roomB is left
            for (int x = cornerPoint.x; x > roomB.centerGridTile.x; x--)
            {
                AddPathTiles(new Vector3Int(x, cornerPoint.y, 0));
            }
        }
    }

    void AddStraightLine(int a, int b, Vector3Int midPointGrid, Vector3Int axis)
    {
        Vector3Int tile = Vector3Int.zero;

        //If A is less then B. Then B is either right or above A
        //Then we add points from A to B
        if (a < b)
        {
            for (int i = a; i <= b; i++)
            {
                //Move in the x axis
                if (axis.x > 0)
                {
                    tile = new Vector3Int(i, midPointGrid.y, 0);
                }
                //Move in the y axis 
                else
                {
                    tile = new Vector3Int(midPointGrid.x, i, 0);
                }

                AddPathTiles(tile);
            }
        }

        //Else B is either left or below A
        //Then we subract points from A to B
        else
        {
            for (int i = a; i >= b; i--)
            {
                //Move in the x axis
                if (axis.x > 0)
                {
                    tile = new Vector3Int(i, midPointGrid.y, 0);
                }
                //Move in the y axis 
                else
                {
                    tile = new Vector3Int(midPointGrid.x, i, 0);
                }

                AddPathTiles(tile);
            }
        }
    }

    Room GetRoom(Vector3Int center)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            if (Rooms[i].centerGridTile.x == center.x && Rooms[i].centerGridTile.y == center.y)
            {
                return Rooms[i];
            }
        }
        return Rooms[0];
    }

    public void Clear()
    {
        if (instantiateDebugTiles)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (InstancedTiles[x, y] != null)
                    {
                        Destroy(InstancedTiles[x, y].gameObject);
                    }
                }
            }
            System.Array.Clear(InstancedTiles, 0, InstancedTiles.Length);
        }

        System.Array.Clear(DungeonTerrainTiles, 0, DungeonTerrainTiles.Length);
        spanningTree.Clear();
        Rooms.Clear();
        pathGridTiles.Clear();
        worldMidpoints.Clear();
        worldCornerPoints.Clear();
        centerWorldTiles.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        for (int i = 0; i < centerWorldTiles.Count; i++)
        {
            Gizmos.DrawCube(new Vector3(centerWorldTiles[i].x, centerWorldTiles[i].y, 0), new Vector3(1, 1, 1));
        }
        Gizmos.color = Color.yellow;
        for (int i = 0; i < worldMidpoints.Count; i++)
        {
            Gizmos.DrawCube(new Vector3(worldMidpoints[i].x, worldMidpoints[i].y, 0), new Vector3(1, 1, 1));
        }

        Gizmos.color = Color.black;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (y == size - 1 || x == size - 1 || y == 0 || x == 0)
                {
                    Gizmos.DrawCube(grid.GetCellCenterWorld(new Vector3Int(x,y,0)), new Vector3(1, 1, 1));
                }
            }
        }

        Gizmos.color = Color.green;
        for (int x = 0; x < size; x++)
        {
            for (int i = 0; i < worldCornerPoints.Count; i++)
            {
                Gizmos.DrawCube(worldCornerPoints[i], new Vector3(1, 1, 1));
            }
        }

        //Delaunay Triangulation
        Gizmos.color = Color.red;
        if (mesh != null && showDelaunay == true)
        {
            foreach (Edge edge in mesh.Edges)
            {
                Vertex v0 = mesh.vertices[edge.P0];
                Vertex v1 = mesh.vertices[edge.P1];
                Vector3 p0 = new Vector3((float)v0.x, (float)v0.y, 0.0f);
                Vector3 p1 = new Vector3((float)v1.x, (float)v1.y, 0.0f);
                Gizmos.DrawLine(p0, p1);
            }
        }
        
        //Minimum Spanning Tree
        Gizmos.color = Color.blue;
        if (mesh != null && showMST == true)
        {
            for (int i = 0; i < spanningTree.Count; i++)
            {
                Vertex v0 = mesh.vertices[spanningTree[i].edge.P0];
                Vertex v1 = mesh.vertices[spanningTree[i].edge.P1];
                Vector3 p0 = new Vector3((float)v0.x, (float)v0.y, 0.0f);
                Vector3 p1 = new Vector3((float)v1.x, (float)v1.y, 0.0f);
                Gizmos.DrawLine(p0, p1);
            }
        }

    }
}
