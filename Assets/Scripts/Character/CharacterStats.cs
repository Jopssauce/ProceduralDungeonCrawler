using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    //Base Stats
    [SerializeField]
    public Stat Health;
    [SerializeField]
    public Stat Mana;
    [SerializeField]
    public Stat Defense;
    [SerializeField]
    public Stat MagicDefense;
    [SerializeField]
    public Stat Attack;
    [SerializeField]
    public Stat MagicAttack;

    CharacterStats(float health, float mana, float defense, float magicDefense, float attack, float magicAttack)
    {
        Health = new Stat(health, health);
        Mana = new Stat(mana, mana);

        Defense = new Stat(defense);
        MagicDefense = new Stat(magicDefense);
        Attack = new Stat(attack);
        MagicAttack = new Stat(magicAttack);
    }

    CharacterStats()
    {
        Health = new Stat(20, 20);
        Mana = new Stat(15, 15);

        Defense = new Stat(5);
        MagicDefense = new Stat(9);
        Attack = new Stat(14);
        MagicAttack = new Stat(8);
    }
}
