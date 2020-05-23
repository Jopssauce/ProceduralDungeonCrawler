using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "ScriptableObjects/Items/Equipment/Sword")]
public class Sword : Equipment
{
    [SerializeField]
    StatModifier attackModifer = null;

    public override void ApplyModifier(CharacterStats characterStats)
    {
        base.ApplyModifier(characterStats);
        characterStats.Attack.AddModifier(attackModifer);
    }
}
