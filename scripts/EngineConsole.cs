using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class EngineConsole : Node2D
{
    public void Log(string message)
    {
        GD.Print(message);
    }

}