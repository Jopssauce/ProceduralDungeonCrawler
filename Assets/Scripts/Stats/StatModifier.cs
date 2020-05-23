using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public enum Type
    {
        Flat,
        Percentage
    }
    public Type type;
    public float value;
    public int order;
    public object source { get; private set;}

    public StatModifier(float value, Type type, int order)
    {
        this.value = value;
        this.type = type;
        this.order = order;
    }

    public StatModifier(float value, Type type, int order, object source)
    {
        this.value = value;
        this.type = type;
        this.order = order;
        this.source = source;
    }

    public StatModifier(float value, Type type, object source)
    {
        this.value = value;
        this.type = type;
        this.source = source;
        this.order = (int)type;
    }

    public StatModifier(float value, Type type)
    {
        this.value = value;
        this.type = type;
        this.order = (int)type;
    }
}
