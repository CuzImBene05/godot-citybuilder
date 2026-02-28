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
    // Sounds
    // Camera
    // Render Stacks
    // UI

    public override void _EnterTree()
    {
        _EngineInputs = GetNode<EngineInputs>("%input engine");
        _EngineImport = GetNode<EngineImport>("%engine import");
        _EngineConsole = GetNode<EngineConsole>("%engine console");
        _Handler._EngineBridge = this;
        _Handler._EngineConsole = _EngineConsole;
        _Handler._EngineImport = _EngineImport;
        _Handler._EngineInputs = _EngineInputs;
        
    }

    public override void _Ready()
    {
        _Handler.Start();

        // link load folder
    }

    public override void _Process(double delta)
    {
        _Handler.Update(delta);


        // EngineCamera, EngineSounds, ...
    }
    


}