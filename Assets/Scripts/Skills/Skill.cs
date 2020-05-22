using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skill : ScriptableObject
{
    public enum SkillType
    {
        SingleTarget,
        AOE,
        RoomWide
    }

    public string skillName;
    public SkillType type;
    public int castRange = 1;
    public int mpCost = 2;

    //How the skill will be casted
    //Mainly used to setup the skill and aquire targets
    //Cast Skill without caster
    public virtual void CastSkill(DungeonManager dungeonManager) { }

    //How the caster will cast the skill
    //Mainly used to setup the skill and aquire targets
    //Cast Skill with caster
    public virtual void CastSkill(Character caster, DungeonManager dungeonManager) { }

    //Apply on single target
    public virtual void ApplySkill(Character target) { }

    //Apply on multiple targets
    public virtual void ApplySkill(Character[] targets) { }

    //Apply on single target
    public virtual void ApplySkill(Character caster, Character target) { }

    //Apply on multiple targets
    public virtual void ApplySkill(Character caster, Character[] targets) { }
}
