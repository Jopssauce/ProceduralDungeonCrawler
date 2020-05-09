using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    public Grid grid;
    public Vector3Int currentTile;

    protected virtual void Start()
    {
        currentTile = grid.WorldToCell(transform.position);
    }

    /// <summary>
    /// Sets the tile for the Entity to move towards to
    /// </summary>
    /// <returns></returns>
    protected virtual void MoveToTile(Vector3Int nextTile, float speed)
    {
        float distanceToDestination = Vector2.Distance(transform.position, grid.GetCellCenterWorld(nextTile));

        if (distanceToDestination > 0.1)
        {
            transform.position = GridMovement.MoveToTile(transform.position, nextTile, grid, speed * Time.deltaTime);
        }
        else
        {
            currentTile = nextTile;
            transform.position = grid.GetCellCenterWorld(currentTile);
        }
    }

    /// <summary>
    /// Creates a ray in one direction and checks for possible collision
    /// </summary>
    /// <returns></returns>
    public bool WillCollide(Vector3 position, Vector2 direction, float rayDistance)
    {
        RaycastHit2D hit;
        Ray2D ray2D;

        ray2D = new Ray2D(position, direction);
        hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayDistance);
        if (hit.collider)
        {
            return true;
        }
        return false;
    }
}
