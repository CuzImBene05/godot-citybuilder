using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class PlacementTools : Node2D
{

	private SimBase _SimBase;

	public override void _EnterTree()
	{
		_SimBase  = GetNode<SimBase>("%sim base");
	}
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Test1"))
		{
			PlaceBuilding("r","s",new Vector2I(5,5));
		}

	}

	//API

	public void PlaceBuilding(string zone_type, string building_type, Vector2I pos) 
	{
		Building b = new();

		b.zone_type = zone_type;
		b.building_type = building_type;
		b.id = _SimBase.GetNewID("building");

		GD.Print(b.id);

		//are any of the squares occupied?

		_SimBase.Buildings.Add(b);
	}

	public void RemoveBuilding(int id) 
	{
		
	}





	public void PlaceNode(){}
	public void ConnectNodes(){}

	public bool IsSquareOccupied(Vector2I pos)
	{
		return false;
	}

}

