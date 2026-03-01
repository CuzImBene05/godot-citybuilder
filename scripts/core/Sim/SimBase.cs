using System.Collections.Generic;
using Godot;

public partial class SimBase
{
    public Handler _Handler;

    public Placement _Placement;

    public List<Building> Buildings = new();


    public void Start()
    {
        _Placement = new(this);

        
    }
    
    
    public SimBase(Handler handler)
    {
        _Handler = handler;
    }

    public void Tick()
    {
        _Handler._EngineBridge._EngineConsole.Log($"gugugaga");
    }

    public int GetID(string type)
    {
        if(type == "b")
        {
            if(Buildings.Count == 0)
            {
                return 1;
            }
            else
            {
                return Buildings[Buildings.Count-1].id;
            }
        }

        return -1;
    }

    
}