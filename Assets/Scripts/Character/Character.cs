using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GridEntity
{
    public delegate void OnSetNextTile();
    public event OnSetNextTile onSetNextTile;

    public float speed;

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
        onSetNextTile?.Invoke();
    }

    protected virtual void Update()
    {
        MoveToTile(NextTile, speed);
    }
}
