using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Godot;

using NumVec2 = System.Numerics.Vector2;
using GodotVec2 = Godot.Vector2;

public partial class EngineRendering : Node2D
{   


    public void Drawbatch(DrawBatch2D db)
    {
        
    }

    public void SpawnCamera(int id)
    {
        
    }

    public void TransformCamera(int id, NumVec2 pos, NumVec2 rot)
    {
        
    }


}

public struct BatchVertex2D
{
    public short x, y;
    public short u, v;
    public uint color;
    public byte layer;
    public byte pad0, pad1, pad2; // alignment
}

public sealed class DrawBatch2D
{
    public int textureId;                 // atlas index
    public List<BatchVertex2D> vertices;  // or arrays for perf
    public List<ushort> indices;          // 16-bit index (3DS-friendly)
}