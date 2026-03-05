using System.Collections.Generic;
using Godot;

using NumVec2 = System.Numerics.Vector2;
using GodotVec2 = Godot.Vector2;

public partial class EngineRendering : Node2D
{   
    private Camera2D cam;

    private MeshInstance2D _meshInstance;
    private ArrayMesh _mesh;
    private ShaderMaterial _mat;

    public override void _EnterTree()
    {
        cam = GetNode<Camera2D>("%cam");

        _meshInstance = new MeshInstance2D();
        _mesh = new ArrayMesh();
        _meshInstance.Mesh = _mesh;

        // Simple sprite-style shader (texture optional for now; vertex color always applies)
        _mat = new ShaderMaterial();

        var shader = new Shader();
        shader.Code = @"
            shader_type canvas_item;

            uniform sampler2D tex : source_color;
            uniform bool use_tex = false;

            void fragment() {
                vec4 base = COLOR;
                if (use_tex) {
                    base *= texture(tex, UV);
                }
                COLOR = base;
            }
            ";
        _mat.Shader = shader;

        _meshInstance.Material = _mat;

        AddChild(_meshInstance);
    }

    private static Color PackedAarrggbbToColor(uint aarrggbb)
    {
        float a = ((aarrggbb >> 24) & 0xFF) / 255f;
        float r = ((aarrggbb >> 16) & 0xFF) / 255f;
        float g = ((aarrggbb >> 8) & 0xFF) / 255f;
        float b = (aarrggbb & 0xFF) / 255f;
        return new Color(r, g, b, a);
    }


    public void DrawStack(DrawStack2D stack)
    {
        if (_mesh == null) { return; }
        if (stack == null || stack.instances == null || stack.instances.Count == 0) { return; }

        // Bind texture (optional)
        if (_mat != null)
        {
            if (stack.texture != null)
            {
                _mat.SetShaderParameter("tex", stack.texture);
                _mat.SetShaderParameter("use_tex", true);
            }
            else
            {
                _mat.SetShaderParameter("use_tex", false);
            }
        }

        int instCount = stack.instances.Count;
        int vCount = instCount * 4;
        int iCount = instCount * 6;

        var positions = new GodotVec2[vCount];
        var uvs = new GodotVec2[vCount];
        var colors = new Color[vCount];
        var indices = new int[iCount];

        float invW = stack.atlasWidth > 0 ? 1f / stack.atlasWidth : 0f;
        float invH = stack.atlasHeight > 0 ? 1f / stack.atlasHeight : 0f;

        int vBase = 0;
        int iBase = 0;

        for (int n = 0; n < instCount; n++)
        {
            var inst = stack.instances[n];

            // Positions (bottom-left anchored)
            float x0 = inst.x;
            float y0 = inst.y;
            float x1 = inst.x + inst.w;
            float y1 = inst.y + inst.h;

            positions[vBase + 0] = new GodotVec2(x0, y0); // BL
            positions[vBase + 1] = new GodotVec2(x1, y0); // BR
            positions[vBase + 2] = new GodotVec2(x1, y1); // TR
            positions[vBase + 3] = new GodotVec2(x0, y1); // TL

            // UV rect in atlas pixels (u0,v0 top-left; u1,v1 bottom-right)
            float u0 = inst.u0 * invW;
            float v0 = inst.v0 * invH;
            float u1 = inst.u1 * invW;
            float v1 = inst.v1 * invH;

            // Base UVs for corners (0=BL,1=BR,2=TR,3=TL)
            // Note: Godot UV origin is top-left for textures in 2D; we keep your rect convention:
            // v0 = top, v1 = bottom.
            var uvBL = new GodotVec2(u0, v1);
            var uvBR = new GodotVec2(u1, v1);
            var uvTR = new GodotVec2(u1, v0);
            var uvTL = new GodotVec2(u0, v0);

            GodotVec2 uv0c = uvBL, uv1c = uvBR, uv2c = uvTR, uv3c = uvTL;

            int r = inst.rot90 & 3;
            if (r == 1)
            {
                // 90° clockwise: new = old shifted right by 1
                uv0c = uvTL; uv1c = uvBL; uv2c = uvBR; uv3c = uvTR;
            }
            else if (r == 2)
            {
                // 180°
                uv0c = uvTR; uv1c = uvTL; uv2c = uvBL; uv3c = uvBR;
            }
            else if (r == 3)
            {
                // 270° clockwise
                uv0c = uvBR; uv1c = uvTR; uv2c = uvTL; uv3c = uvBL;
            }

            uvs[vBase + 0] = uv0c;
            uvs[vBase + 1] = uv1c;
            uvs[vBase + 2] = uv2c;
            uvs[vBase + 3] = uv3c;

            var c = PackedAarrggbbToColor(inst.color);
            colors[vBase + 0] = c;
            colors[vBase + 1] = c;
            colors[vBase + 2] = c;
            colors[vBase + 3] = c;

            // Indices
            indices[iBase + 0] = vBase + 0;
            indices[iBase + 1] = vBase + 1;
            indices[iBase + 2] = vBase + 2;
            indices[iBase + 3] = vBase + 0;
            indices[iBase + 4] = vBase + 2;
            indices[iBase + 5] = vBase + 3;

            vBase += 4;
            iBase += 6;
        }

        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = positions;
        arrays[(int)Mesh.ArrayType.TexUV] = uvs;
        arrays[(int)Mesh.ArrayType.Color] = colors;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        _mesh.ClearSurfaces();
        _mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
    }

    public void TransformCameraPos(NumVec2 pos)
    {
        if (cam == null) { return; }

        cam.Position = new GodotVec2(pos.X, pos.Y);
    }

    public void TransformCameraZoom(NumVec2 zoom)
    {
        if (cam == null) { return; }

        cam.Zoom = new GodotVec2(zoom.X, zoom.Y);
    }


}


public sealed class DrawStack2D //Buildings, Cars, Terrain, Deco
{
    // Godot texture for this stack (one stack = one texture/material)
    public string texture; //Buildings.png / Cars.png / Deco.png

    // Atlas size in pixels (used to convert u/v rect pixels to normalized UVs)
    public int atlasWidth;
    public int atlasHeight;

    public List<DrawInstance2D> instances = new();
}

public struct DrawInstance2D
{
    // Bottom-left anchor in world/batch space
    public float x, y;

    // Size in world/batch space
    public float w, h;

    // UV rect in ATLAS PIXELS (u0,v0 top-left; u1,v1 bottom-right)
    public int u0, v0, u1, v1;

    // 0,1,2,3 = 0/90/180/270 clockwise
    public byte rot90;

    public uint color;
    public byte layer;
}
