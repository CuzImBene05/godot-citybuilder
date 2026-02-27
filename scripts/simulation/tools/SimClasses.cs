using Godot;

public struct Citizen
{
    public int id;
}

public struct Building
{
    public int id;

    public string zone_type; //RSI-C
    public string building_type; //design 3

    public int density;

    public Vector2I pos;
    public Vector2I size;

    public float satisfaction;
    
}

public struct Node
{
    public int id;
    
}

public struct Road
{
    public int id;

}
