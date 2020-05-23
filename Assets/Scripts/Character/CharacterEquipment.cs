using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterEquipment
{
    public Equipment weapon;
    public Equipment armor;

    public void ApplyEquipmentModifiers(CharacterStats characterStats)
    {
        if(weapon != null) weapon.ApplyModifier(characterStats);
        if(armor != null) armor.ApplyModifier(characterStats);
    }
}
