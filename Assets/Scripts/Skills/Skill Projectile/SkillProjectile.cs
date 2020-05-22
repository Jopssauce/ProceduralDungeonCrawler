using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public Vector3Int currentTile;
    public float speed = 10;
    public float range = 4;

    public delegate void OnSetNextTile();
    public event OnSetNextTile onSetNextTile;

    Character caster;
    Skill skill;
    DungeonGenerator.DungeonTerrainTile[,] dungeonTerrainTiles;
    List<Character> hits;
    protected Grid grid;

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

    public void Initialize(Character caster, Skill skill, DungeonGenerator.DungeonTerrainTile[,] dungeonTerrainTiles, Grid grid)
    {
        this.caster = caster;
        this.skill = skill;
        this.dungeonTerrainTiles = dungeonTerrainTiles;
        this.grid = grid;
        hits = new List<Character>();
    }

    protected virtual void Update()
    {
        MoveToTile(NextTile, speed);

        //If projectile is within the dungeon
        if (dungeonTerrainTiles[currentTile.x, currentTile.y] != null)
        {
            Character target = ReturnHit();
            if (target != null)
            {
                HitCharacter(caster, target);
                Destroy(this.gameObject);
            }
            else if (target == null && currentTile == nextTile)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void HitCharacter(Character caster, Character target)
    {   
        skill.ApplySkill(caster, target);
    }

    public virtual Character ReturnHit()
    {
        GridEntity gridEntity = dungeonTerrainTiles[currentTile.x, currentTile.y].gridEntity;
        if (dungeonTerrainTiles[currentTile.x, currentTile.y].gridEntity && gridEntity.GetComponent<Character>() && !gridEntity.GetComponent<PartyMember>())
        {

            return gridEntity.GetComponent<Character>();
        }

        return null;
    }

    public virtual List<Character> ReturnHits()
    {
        GridEntity gridEntity = dungeonTerrainTiles[currentTile.x, currentTile.y].gridEntity;
        Character character = gridEntity.GetComponent<Character>();

        if (gridEntity != null && character)
        {
            hits.Add(character);
        }

        return hits;
    }

    public virtual void SetNextTile(Vector3Int target)
    {
        NextTile = target;
        onSetNextTile?.Invoke();
    }

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
        currentTile = grid.WorldToCell(this.transform.position);
    }
}
