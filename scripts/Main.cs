using Godot;

public partial class Main : Node2D 
{
    public SimBase _SimBase = new();

    


    public override void _EnterTree()
    {
        GD.Print("test");
        
    }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Test1"))
        {
            _SimBase.Tick();
        }
    }
}