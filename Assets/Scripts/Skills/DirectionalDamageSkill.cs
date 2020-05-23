using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Directional Damage Skill", menuName = "ScriptableObjects/Default Skills/Directional Damage Skill")]
public class DirectionalDamageSkill : DirectionalSkill
{
    public int aoeRange;
    public override void CastSkill(Character caster, DungeonManager dungeonManager)
    {
        base.CastSkill(caster, dungeonManager);
        List<Character> targets = new List<Character>();

        Vector2Int facingDirection = GetSkillDirection(caster);

        if (facingDirection == Vector2Int.zero)
        {
            return;
        }
        
        for (int i = -aoeRange; i <= aoeRange; i++)
        {
            Vector3 newPostion = Vector3.zero;
            if (facingDirection.x != 0)
            {
                newPostion = new Vector3(caster.transform.position.x + castRange * facingDirection.x, caster.transform.position.y + i, 0);
            }
            else if (facingDirection.y != 0)
            {

                newPostion = new Vector3(caster.transform.position.x + i, caster.transform.position.y + castRange * facingDirection.y, 0);
            }

            Vector3Int tile = dungeonManager.grid.WorldToCell(newPostion);
            GridEntity gridEntity = dungeonManager.dungeonGenerator.DungeonTerrainTiles[tile.x, tile.y].gridEntity;
            if (gridEntity != null)
            {
                Character character = gridEntity.GetComponent<Character>();
                PartyMember partyMember = gridEntity.GetComponent<PartyMember>();
                if (character && !partyMember)
                {
                    targets.Add(character);
                }
            }
        }

        ApplySkill(caster, targets.ToArray());

    }

    public override void ApplySkill(Character caster, Character[] targets)
    {
        base.ApplySkill(caster, targets);

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].stats.Health.DeductBase((caster.stats.Attack.FinalValue * 2) - targets[i].stats.Defense.FinalValue);
        }
    }
}
