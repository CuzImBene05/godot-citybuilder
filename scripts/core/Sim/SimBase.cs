using System.Collections.Generic;

public partial class SimBase
{
    public Handler _Handler;

    public BuildingSim _BuildingSim;

    public GridHandler _GridHandler;


    public void Start()
    {
        _BuildingSim = new(this);
        _GridHandler = new(this);

    }       
    
    
    public SimBase(Handler handler)
    {
        _Handler = handler;
    }

    public void Tick()
    {
        _Handler._EngineBridge._EngineConsole.Log($"gugugaga");

        _BuildingSim.Tick();


    }


}