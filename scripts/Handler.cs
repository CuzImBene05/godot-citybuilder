public partial class Handler
{
    public InputHandler _InputHandler;

    public EngineBridge _EngineBridge;
    public EngineConsole _EngineConsole;
    

    private SimBase _SimBase = new();

    

    public string current_scene = "ingame";

    public void Start()
    {
        _InputHandler = new(this);
    }

    public void Update(double delta)
    {
        
    }

    

    
}