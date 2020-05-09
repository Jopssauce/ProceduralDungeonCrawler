using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonTiler : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;
    public Tilemap dungeonBase;
    public TileBase dungeonBaseWallTile;
    public TileBase dungeonFloorTile;

    public delegate void OnComplete();
    public event OnComplete onComplete;

    private void Awake()
    {
        dungeonGenerator.onComplete += TileGrid;
    }

    void TileGrid()
    {
        for (int x = 0; x < dungeonGenerator.size; x++)
        {
            for (int y = 0; y < dungeonGenerator.size; y++)
            {
                if (dungeonGenerator.DungeonTerrainTiles[x, y] == null)
                {
                    continue;
                }
                if (dungeonGenerator.DungeonTerrainTiles[x, y].tileType == DungeonGenerator.DungeonTerrainTile.TileType.Wall)
                {
                    dungeonBase.SetTile(new Vector3Int(x, y, 0), dungeonBaseWallTile);
                }
                if (dungeonGenerator.DungeonTerrainTiles[x, y].tileType == DungeonGenerator.DungeonTerrainTile.TileType.Floor)
                {
                    dungeonBase.SetTile(new Vector3Int(x, y, 0), dungeonBaseWallTile);
                }

            }
        }
        onComplete?.Invoke();
    }

}
