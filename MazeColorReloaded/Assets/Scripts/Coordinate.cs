using System;
using UnityEngine;

[Serializable]
public class Coordinate
{
    public int x, y;

    public Coordinate()
    {
        x = 0;
        y = 0;
    }

    public Coordinate(Vector2 vector)
    {
        x = Mathf.RoundToInt(vector.x);
        y = Mathf.RoundToInt(vector.y);
    }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public override string ToString()
    {
        return "(" + x + " , " + y + ")";
    }

    public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.x + b.x, a.y + b.y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new Coordinate(a.x - b.x, a.y - b.y);
    public static bool operator ==(Coordinate a, Coordinate b)
    {
        if (object.ReferenceEquals(a, null))
        {
            return object.ReferenceEquals(b, null);
        }
        return a.Equals(b);
    }

    public static bool operator !=(Coordinate a, Coordinate b)
    {
        if (object.ReferenceEquals(a, null))
        {
            return !object.ReferenceEquals(b, null);
        }
        return !a.Equals(b);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var coord = (Coordinate)obj;
        return (coord.x == x && coord.y == y);
    }

    public override int GetHashCode()
    {
        return x ^ y;

    }
}