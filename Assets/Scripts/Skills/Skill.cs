using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillType
    {
        SingleTarget,
        AOE,
        RoomWide
    }

    public string skillName;
    public SkillType type;
    public float castRange;

    //How the skill will be casted
    //Mainly used to setup the skill and aquire targets
    public virtual void CastSkill(Grid grid) { }

    //How the caster will cast the skill
    //Mainly used to setup the skill and aquire targets
    public virtual void CastSkill(Character caster, Grid grid) { }

    //Apply on single target
    protected virtual void ApplySkill(Character target) { }

    //Apply on multiple targets
    protected virtual void ApplySkill(Character[] targets) { }

    //Apply on single target
    protected virtual void ApplySkill(Character caster, Character target) { }

    //Apply on multiple targets
    protected virtual void ApplySkill(Character caster, Character[] targets) { }
}
