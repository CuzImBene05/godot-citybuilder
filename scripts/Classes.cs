using System.Collections.Generic;
using System.Text.Json;
using System.Linq.Expressions;
using System.Numerics;


public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString() => $"({x}, {y})";
}






