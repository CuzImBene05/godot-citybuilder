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
    // Sounds
    // Camera
    // Render Stacks
    // UI

    public override void _EnterTree()
    {
       _EngineInputs = GetNode<EngineInputs>("%input engine");


    }

    public override void _Ready()
    {
        _Handler.Start();
    }

    public override void _Process(double delta)
    {
        _EngineInputs.ReadInputs();
        _Handler.Update();

        // EngineCamera, EngineSounds, ...
    }


}