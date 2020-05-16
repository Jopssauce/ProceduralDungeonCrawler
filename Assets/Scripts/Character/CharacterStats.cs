using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    [field: SerializeField]
    public float Health { get; private set; }
    [field: SerializeField]
    public float Mana { get; private set; }
    public float defense;
    public float magicDefense;
    public float attack;
    public float magicAttack;

    CharacterStats(float health, float mana, float defense, float magicDefense, float attack, float magicAttack)
    {
        this.Health = health;
        this.Mana = mana;
        this.defense = defense;
        this.magicDefense = magicDefense;
        this.attack = attack;
        this.magicAttack = magicAttack;
    }

    CharacterStats()
    {
        this.Health = 20;
        this.Mana = 15;
        this.defense = 5;
        this.magicDefense = 9;
        this.attack = 14;
        this.magicAttack = 8;
    }

    public void DeductHealth(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
        }
    }

    public void AddHealth(float amount)
    {
        Health += amount;
    }
}
