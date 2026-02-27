using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class TrafficSim : Node2D
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
		
	}

}
