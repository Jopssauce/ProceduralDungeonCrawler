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
    public Vector2Int facingDirection;

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
    }

    public virtual void SetNextTile(Vector3Int target)
    {
        NextTile = target;
        Vector3Int d = nextTile - currentTile;
        facingDirection = (Vector2Int)d;
        onSetNextTile?.Invoke();
    }

    protected virtual void Update()
    {
        MoveToTile(NextTile, speed);
    }
}
