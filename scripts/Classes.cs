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

public sealed class Grid<T>
{
    public readonly int Width;
    public readonly int Height;
    public readonly int OffsetX;
    public readonly int OffsetY;

    private readonly T[] _data;

    public Grid(int width, int height, int offsetX, int offsetY)
    {
        Width = width;
        Height = height;
        OffsetX = offsetX;
        OffsetY = offsetY;
        _data = new T[width * height];
    }

    private int Idx(int gx, int gy) => gx + gy * Width;

    public bool InBoundsWorld(int wx, int wy)
    {
        int gx = wx + OffsetX;
        int gy = wy + OffsetY;
        return (uint)gx < (uint)Width && (uint)gy < (uint)Height;
    }

    public ref T GetRefWorld(int wx, int wy)
    {
        int gx = wx + OffsetX;
        int gy = wy + OffsetY;
        return ref _data[Idx(gx, gy)];
    }

    public T GetWorld(int wx, int wy)
    {
        int gx = wx + OffsetX;
        int gy = wy + OffsetY;
        return _data[Idx(gx, gy)];
    }

    public void SetWorld(int wx, int wy, T value)
    {
        int gx = wx + OffsetX;
        int gy = wy + OffsetY;
        _data[Idx(gx, gy)] = value;
    }
}





public struct Building
{
    public int id;

    public string Building_Type; 
    public Vector2Int pos;

}

//DTOs
public sealed class BuildingsDTO
{
    public int size_x { get; set; }
    public int size_y { get; set; }
}

public struct RawInputPacket
{
    public HashSet<string> inputsDown;
    public Vector2 leftStick;
    public Vector2 rightStick;
    public Vector2 mousePos;
}

