using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    [field: SerializeField]
    public float BaseValue { get; private set; }
    [field: SerializeField]
    public float MaxBaseValue { get; private set; }

    public float FinalValue
    {
        get
        {
            if (isModified)
            {
                isModified = false;
                return CalculateBasevalueModifiers();
            }
            else
            {
                return BaseValue;
            }
        }
    }
    public float finalMaxValue;

    delegate float DeductStatDelegate(Stat stats);
    delegate float AddStatDelegate(Stat stats);

    public List<StatModifier> Modifiers { get; private set; }
    bool isModified;

    public Stat(float value)
    {
        this.BaseValue = value;
        Modifiers = new List<StatModifier>();
    }

    public Stat(float value, float maxValue)
    {
        this.BaseValue = value;
        this.MaxBaseValue = maxValue;
        Modifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier modifier)
    {
        isModified = true;
        Modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        isModified = true;
        Modifiers.Remove(modifier);
    }

    public void RemoveSourceModifiers(object source)
    {
        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            if (Modifiers[i].source == source)
            {
                isModified = true;
                Modifiers.RemoveAt(i);
            }
        }
    }

    float CalculateBasevalueModifiers()
    {
        float totalPercentage = 0;
        float modifiedValue = BaseValue;
        Modifiers.Sort((a, b) => a.order.CompareTo(b.order));

        for (int i = 0; i < Modifiers.Count; i++)
        {
            StatModifier modifier = Modifiers[i];

            if (modifier.type == StatModifier.Type.Flat)
            {
                modifiedValue += modifier.value;
            }
            else if (modifier.type == StatModifier.Type.Percentage)
            {
                totalPercentage += modifier.value;
            }
        }

        if (totalPercentage > 0)
        {
            modifiedValue *= 1 + totalPercentage;
        }
        isModified = true;
        return modifiedValue;
    }

    //Base Values 
    public void AddBase(float amount)
    {
        if (MaxBaseValue <= 0)
        {
            BaseValue = AddStat(stat => stat.BaseValue, amount);
        }
        else
        {
            BaseValue = AddStat(stat => stat.BaseValue, MaxBaseValue, amount);
        }
    }

    public void DeductBase(float amount)
    {
        BaseValue = DeductStat(stat => stat.BaseValue, amount);
    }

    public void AddMaxBase(float amount)
    {
        MaxBaseValue = AddStat(stat => stat.MaxBaseValue, amount);
    }

    public void DeductMaxBase(float amount)
    {
        MaxBaseValue = DeductStat(stat => stat.MaxBaseValue, amount);
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

    float AddStat(AddStatDelegate addStatDelegate, float amount)
    {
        float stat = addStatDelegate(this);
        stat += amount;
        return stat;
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
}
