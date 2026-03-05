
public partial class Handler
{
    public EngineBridge _EngineBridge;

    public InputHandler _InputHandler;
    public ImportHandler _ImportHandler;
    public RenderHandler _RenderHandler;
    public SubscriptionHandler _SubscriptionHandler;


    private SimBase _SimBase;

    
    public string current_scene = "ingame";


    public void Start()
    {
        _InputHandler = new(this);
        _ImportHandler = new(this);
        _SubscriptionHandler = new();
        _RenderHandler = new(this);
        
        _SimBase = new(this);

        _InputHandler.RegisterAction("test action","key:T");

        _ImportHandler.ImportAll();

        //Subscriptions
        _SubscriptionHandler.CreateSubscription("BuildingList");

        _RenderHandler.SetupCamera();

        _SimBase.Start();


        
        _SimBase._BuildingSim.InitBuildingGrid(new Vector2Int(256,256));
        _SimBase._BuildingSim.SetBuilding(new Vector2Int(0,0),"standard");


    }

    public void Update(double delta)
    {
        _InputHandler.ReceiveInputs();
        _RenderHandler.Render();
        
        
        if (_InputHandler.GetActionDown("test action"))
        {
            _SimBase.Tick();
        }
        
    }



    

    
}