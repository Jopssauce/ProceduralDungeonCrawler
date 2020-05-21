using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Directional Damage Skill", menuName = "ScriptableObjects/Default Skills/Directional Damage Skill")]
public class DirectionalDamageSkill : Skill
{
    public int aoeRange;
    public enum SkillDirection
    {
        Front,
        Back,
        Left,
        Right
    }
    public SkillDirection skillDirection = SkillDirection.Front;

    public override void CastSkill(Character caster, DungeonManager dungeonManager)
    {
        base.CastSkill(caster, dungeonManager);
        List<Character> targets = new List<Character>();

        Vector2Int facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Front);

        if (facingDirection == Vector2Int.zero)
        {
            return;
        }

        switch (this.skillDirection)
        {
            case SkillDirection.Front:
                facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Front);
                break;
            case SkillDirection.Back:
                facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Back);
                break;
            case SkillDirection.Left:
                facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Left);
                break;
            case SkillDirection.Right:
                facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Right);
                break;
            default:
                break;
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

            Debug.Log(newPostion);

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

    protected override void ApplySkill(Character caster, Character[] targets)
    {
        base.ApplySkill(caster, targets);

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].stats.DeductHealth((caster.stats.attack * 2) - targets[i].stats.defense);
        }
    }
}
