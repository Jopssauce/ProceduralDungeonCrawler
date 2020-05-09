using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridMovement
{
    //Change to move only in one axis
    /// <summary>
    /// Returns a world position between the current world position and target grid tile
    /// </summary>
    /// <returns></returns>
    public static Vector3 MoveToTile(Vector3 currentPosition, Vector3Int targetTile, Grid grid, float maxDistanceDelta)
    {
        Vector3 targetTileWorld = grid.GetCellCenterWorld(targetTile); 
        Vector3 newPos = Vector3.MoveTowards(currentPosition, targetTileWorld, maxDistanceDelta);
        return newPos;
    }
    
}
