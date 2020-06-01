using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    public DungeonManager dungeonManager;
    protected Grid grid;
    public Vector3Int currentTile;

    protected virtual void Start()
    {
        grid = dungeonManager.grid;
        currentTile = grid.WorldToCell(transform.position);
    }
    //TODO: Update tiles that they are occupied by this entity
    /// <summary>
    /// Sets the tile for the Entity to move towards to
    /// </summary>
    /// <returns></returns>
    protected virtual void MoveToTile(Vector3Int nextTile, float speed)
    {
        float distanceToDestination = Vector3.Distance(transform.position, grid.GetCellCenterWorld(nextTile));
        if (distanceToDestination > 0.1)
        {
            transform.position = GridMovement.MoveToTile(transform.position, nextTile, grid, speed * Time.deltaTime);
        }
        else
        {
            currentTile = nextTile;
            dungeonManager.dungeonGenerator.DungeonTerrainTiles[currentTile.x, currentTile.y].gridEntity = this;
            transform.position = grid.GetCellCenterWorld(currentTile);
        }
    }

    /// <summary>
    /// Creates a ray in one direction and checks for possible collision
    /// </summary>
    /// <returns></returns>
    public bool WillCollide(Vector2 direction, float rayDistance)
    {
        RaycastHit2D hit;
        Ray2D ray2D;

        ray2D = new Ray2D(transform.position, direction);
        hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayDistance);
        if (hit.collider && hit.collider.gameObject != this.gameObject)
        {
            return true;
        }
        return false;
    }
}
