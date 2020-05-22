using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSkill : Skill
{
    public enum SkillDirection
    {
        Front,
        Back,
        Left,
        Right
    }
    public SkillDirection skillDirection = SkillDirection.Front;

    public virtual Vector2Int GetSkillDirection(Character caster)
    {
        Vector2Int facingDirection = caster.characterDirection.GetOrientation(CharacterDirection2D.Orientation.Front);

        if (facingDirection == Vector2Int.zero)
        {
            return Vector2Int.zero;
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

        return facingDirection;
    }
}
