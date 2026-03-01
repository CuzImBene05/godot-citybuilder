using System.Security.Cryptography;

public partial class Handler
{
    public EngineBridge _EngineBridge;

    public InputHandler _InputHandler;
    
    public ImportHandler _ImportHandler;

    private SimBase _SimBase;

    
    public string current_scene = "ingame";


    public void Start()
    {
        _InputHandler = new(this);
        _ImportHandler = new(this);
        
        _SimBase = new(this);

        _InputHandler.RegisterAction("test action","key:T");

        _ImportHandler.ImportAll();

        _SimBase.Start();
        
        _SimBase._Placement.InitBuildingGrid(new Vector2Int(256,256));
        _SimBase._Placement.SetBuilding(new Vector2Int(0,0),"standard");
        _SimBase._Placement.SetBuilding(new Vector2Int(0,0),"standard");
    }

    public void Update(double delta)
    {
        _EngineBridge._EngineInputs.ReadInputs();
        if (_InputHandler.GetActionDown("test action"))
        {
            _SimBase.Tick();
        }
        
    }



    

    
}