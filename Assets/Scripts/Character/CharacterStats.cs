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
    [field: SerializeField]
    public float MaxHealth { get; private set; }
    [field: SerializeField]
    public float MaxMana { get; private set; }
    public float defense;
    public float magicDefense;
    public float attack;
    public float magicAttack;

    delegate float DeductStatDelegate(CharacterStats stats);
    delegate float AddStatDelegate(CharacterStats stats);

    CharacterStats(float health, float mana, float defense, float magicDefense, float attack, float magicAttack)
    {
        this.Health = health;
        this.MaxHealth = health;
        this.Mana = mana;
        this.MaxMana = mana;
        this.defense = defense;
        this.magicDefense = magicDefense;
        this.attack = attack;
        this.magicAttack = magicAttack;
    }

    CharacterStats()
    {
        this.Health = 20;
        this.MaxHealth = Health;
        this.Mana = 15;
        this.MaxMana = Mana;

        this.defense = 5;
        this.magicDefense = 9;
        this.attack = 14;
        this.magicAttack = 8;
    }

    public void DeductHealth(float amount)
    {
        Health = DeductStat(stat => stat.Health, amount);
    }

    public void AddHealth(float amount)
    {
        Health = AddStat(stat => stat.Health, MaxHealth, amount);
    }

    public void DeductMana(float amount)
    {
        Mana = DeductStat(stat => stat.Mana, amount);
    }

    public void AddMana(float amount)
    {
        Mana = AddStat(stat => stat.Mana, MaxMana, amount);
    }

    float DeductStat(DeductStatDelegate deductStatDelegate, float amount)
    {
        float stat = deductStatDelegate(this);
        stat -= amount;

        if (stat <= 0)
        {
            stat = 0;
        }

        return stat;
    }

    float AddStat(AddStatDelegate addStatDelegate, float max, float amount)
    {
        float stat = addStatDelegate(this);
        stat += amount;

        if (stat >= max)
        {
            stat = max;
        }

        return stat;
    }
}
