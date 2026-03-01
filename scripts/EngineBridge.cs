using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class EngineBridge : Node2D
{
    public Handler _Handler = new();

    //APIs
    public EngineInputs _EngineInputs;
    public EngineImport _EngineImport;
    public EngineConsole _EngineConsole;
    public EngineRendering _EngineRendering;
    // Sounds
    // Camera
    // UI

    public override void _EnterTree()
    {
        _EngineInputs = GetNode<EngineInputs>("%input engine");
        _EngineImport = GetNode<EngineImport>("%engine import");
        _EngineConsole = GetNode<EngineConsole>("%engine console");
        _EngineRendering = GetNode<EngineRendering>("%engine rendering");
        _Handler._EngineBridge = this;
        
    }

    public override void _Ready()
    {
        _Handler.Start();

    }

    public override void _Process(double delta)
    {
        _Handler.Update(delta);
    }
    
    


}