using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDirection2D
{
    public enum Orientation
    {
        Front,
        Back,
        Right,
        Left
    }

    private Vector2Int front;

    public Vector2Int GetOrientation(Orientation orientation)
    {
        Vector2Int o = Vector2Int.zero;

        switch (orientation)
        {
            case Orientation.Front:
                o = front;
                break;
            case Orientation.Back:
                o = front * -1;
                break;
            case Orientation.Right:
                o = GetRight();
                break;
            case Orientation.Left:
                o = GetRight() * -1;
                break;
            default:
                break;
        }

        return o;
    }

    public void SetFront(Vector2Int front)
    {
        this.front = front;
    }

    private Vector2Int GetRight()
    {
        Vector2Int right = Vector2Int.zero;

        if (front.y != 0)
        {
            right.x = front.y;
        }
        if (front.x != 0)
        {
            right.y = front.x;
        }

        return right;
    }
}
