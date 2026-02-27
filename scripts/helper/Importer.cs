using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Importer : Node2D
{	






	public override void _EnterTree()
	{
		Reload();
	}


	private void Reload()
	{
		GD.Print("reloading assets ...");
		//unload everything loaded

		//import everything
	}

	private void Unload()
	{
		//1. Roads 
	}

	private void Load()
	{
		//1. Roads
	}


	// Fetching

	public void FetchBuilding()
	{
		
	}
}
