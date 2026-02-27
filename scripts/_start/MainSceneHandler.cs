using Godot;

public partial class MainSceneHandler : Node2D
{
    private SimBase _SimBase;

    public override void _EnterTree()
    {
        // In Tool-Scripts kann CurrentScene null sein, daher eher Root/CurrentScene fallback
        _SimBase  = GetNode<SimBase>("%sim base");

    }

    public override void _Ready()
    {
        
    }
}