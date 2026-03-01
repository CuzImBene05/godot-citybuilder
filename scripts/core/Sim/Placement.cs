using System.Numerics;

public class Placement
{
    public SimBase _Simbase;

    public Placement(SimBase simBase)
    {
        _Simbase = simBase;
    }

    public void SetBuilding(Vector2 pos)
    {
        Building b = new();
        b.id = _Simbase.GetID("b");


        _Simbase.Buildings.Add(b);
    }


}