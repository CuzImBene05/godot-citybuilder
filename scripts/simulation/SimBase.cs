using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class SimBase : Node2D
{
	public List<Citizen> Citizens = new();
	public List<Building> Buildings = new();
	

	 
	public override void _EnterTree()
	{

	}
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Tick"))
		{
			Tick();
		}

		
	}


	public int GetNewID(string type)
	{
		if(type == "building")
		{
			if(Buildings.Count <= 0)
			{
				return 1;
			}
			else
			{
				int last_id = Buildings[Buildings.Count-1].id;
				return last_id + 1;
			}
		}



		return 0;
	}


	public void Tick() //
	{
		GD.Print("tick");
	}

}
