using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Directional Projectile Skill", menuName = "ScriptableObjects/Default Skills/Directional Projectile Skill")]
public class DirectionalProjectileSkill : DirectionalSkill
{
    [SerializeField]
    SkillProjectile skillProjectilePrefab = null;
    public int arrows = 1;

    public override void CastSkill(Character caster, DungeonManager dungeonManager)
    {
        base.CastSkill(caster, dungeonManager);

        Vector2Int direction = GetSkillDirection(caster);
        caster.StartCoroutine(Fire(caster, direction, dungeonManager));
    }

    public virtual IEnumerator Fire(Character caster, Vector2Int direction, DungeonManager dungeonManager)
    {
        int index = 0;
        while (index < arrows)
        {
            Vector3 spawnPosition = caster.transform.position + new Vector3(direction.x, direction.y) * castRange;
            GameObject projectile = Instantiate(skillProjectilePrefab.gameObject, spawnPosition, skillProjectilePrefab.gameObject.transform.rotation);
            SkillProjectile skillProjectile = projectile.GetComponent<SkillProjectile>();
            skillProjectile.Initialize(caster, this, dungeonManager.dungeonGenerator.DungeonTerrainTiles, dungeonManager.dungeonGenerator.grid);
            
            Grid grid = dungeonManager.dungeonGenerator.grid;
            Vector3 target = caster.transform.position + new Vector3Int(direction.x, direction.y, 0) * (int)skillProjectile.range;
            skillProjectile.SetNextTile(grid.WorldToCell(target));

            index++;
            yield return new WaitForSeconds(0.1f);
        }

    }

    public override void ApplySkill(Character caster, Character target)
    {
        base.ApplySkill(caster, target);
        target.stats.DeductHealth((caster.stats.attack * 2) - target.stats.defense);
    }
}
