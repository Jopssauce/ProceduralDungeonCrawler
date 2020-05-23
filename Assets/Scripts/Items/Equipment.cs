using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public virtual void ApplyModifier(CharacterStats characterStats) { }

    public virtual void RemoveModifier() { }
}
