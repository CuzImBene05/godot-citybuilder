using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class EngineBridge : Node2D
{
    public Handler _Handler = new();

    //Inputs
    private EngineInputs _EngineInputs;

    //Outputs
    public EngineConsole _EngineConsole;
    // Sounds
    // Camera
    // Render Stacks
    // UI

    public override void _EnterTree()
    {
        _EngineInputs = GetNode<EngineInputs>("%input engine");
        _EngineConsole = GetNode<EngineConsole>("%engine console");
        _Handler._EngineBridge = this;
        _Handler._EngineConsole = _EngineConsole;

    }

    public override void _Ready()
    {
        _Handler.Start();
    }

    public override void _Process(double delta)
    {
        _EngineInputs.ReadInputs();
        _Handler.Update(delta);


        // EngineCamera, EngineSounds, ...
    }
    


}