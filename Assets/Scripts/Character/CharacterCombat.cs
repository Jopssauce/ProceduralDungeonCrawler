using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterCombat : MonoBehaviour
{
    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Attack(Skill skill, DungeonManager dungeonManager)
    {
        skill.CastSkill(character, dungeonManager);
    }
}
