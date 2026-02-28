using System.Security.Cryptography;

public partial class Handler
{
    public EngineBridge _EngineBridge;
    public EngineConsole _EngineConsole;
    public EngineImport _EngineImport;
    public EngineInputs _EngineInputs;

    public InputHandler _InputHandler;
    


    private SimBase _SimBase = new();


    

    public string current_scene = "ingame";

    public void Start()
    {
        _InputHandler = new(this);
        
        _EngineImport.Modify();

        _InputHandler.RegisterAction("test action","key:T");
        
    }

    public void Update(double delta)
    {
        _EngineInputs.ReadInputs();
        if (_InputHandler.GetActionDown("test action"))
        {
            _EngineConsole.Log("pass");
        }
        
    }



    

    
}