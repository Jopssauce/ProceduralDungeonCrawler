using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    public float health;
    public float mana;
    public float defense;
    public float magicDefense;
    public float attack;
    public float magicAttack;

    CharacterStats(float health, float mana, float defense, float magicDefense, float attack, float magicAttack)
    {
        this.health = health;
        this.mana = mana;
        this.defense = defense;
        this.magicDefense = magicDefense;
        this.attack = attack;
        this.magicAttack = magicAttack;
    }

    CharacterStats()
    {
        this.health = 100;
        this.mana = 100;
        this.defense = 10;
        this.magicDefense = 10;
        this.attack = 5;
        this.magicAttack = 5;
    }
}
