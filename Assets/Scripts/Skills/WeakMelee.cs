using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weak Melee Skill", menuName = "ScriptableObjects/Default Skills/Weak Melee Skill")]
public class WeakMelee : Skill
{
    public override void CastSkill(Character caster, DungeonManager dungeonManager)
    {
        base.CastSkill(caster, dungeonManager);
        Character target = null;
        RaycastHit2D hit;
        Ray2D ray2D;

        ray2D = new Ray2D(caster.transform.position, caster.facingDirection);
        hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, castRange);

        if (hit)
        {
            if (hit.collider.GetComponent<Character>())
            {
                target = hit.collider.GetComponent<Character>();
                ApplySkill(caster, target);
            }
        }
    }

    protected override void ApplySkill(Character caster, Character target)
    {
        base.ApplySkill(caster, target);
        target.stats.DeductHealth((caster.stats.attack * 2) - target.stats.defense);
    }
}
