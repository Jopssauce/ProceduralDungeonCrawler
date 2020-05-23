using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GridEntity
{
    public delegate void OnSetNextTile();
    public event OnSetNextTile onSetNextTile;

    public float speed = 6;
    public CharacterStats stats;
    public CharacterSkills skills;
    public CharacterDirection2D characterDirection;
    public CharacterEquipment characterEquipment;

    Vector3Int nextTile;
    public Vector3Int NextTile
    {
        get
        {
            return nextTile;
        }
        protected set
        {
            nextTile = value;
            onSetNextTile?.Invoke();
        }

    }

    protected override void Start()
    {
        base.Start();
        NextTile = currentTile;
        characterEquipment.ApplyEquipmentModifiers(stats);
    }

    public virtual void SetNextTile(Vector3Int target)
    {
        dungeonManager.dungeonGenerator.DungeonTerrainTiles[currentTile.x, currentTile.y].gridEntity = null;
        NextTile = target;
        Vector3Int d = nextTile - currentTile;
        characterDirection.SetFront((Vector2Int)d);
        onSetNextTile?.Invoke();
    }

    protected virtual void Update()
    {
        MoveToTile(NextTile, speed);
    }
}
