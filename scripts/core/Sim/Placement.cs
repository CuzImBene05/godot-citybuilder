using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Microsoft.VisualBasic;

public class Placement
{
    public SimBase _Simbase;

    private Grid<int> BuildingGrid;
    private Grid<int> RoadGrid;


    public Placement(SimBase simBase)
    {
        _Simbase = simBase;
    }

    public void InitBuildingGrid(Vector2Int size)
    {
        BuildingGrid = new Grid<int>(
        width: size.x,
        height: size.y,
        offsetX: size.x / 2,
        offsetY: size.y / 2
    );
    }

    public void SetBuilding(Vector2Int pos, string Building_Type)
    {
        if(BuildingGrid == null){ return; }
        if(!_Simbase._Handler._ImportHandler.BuildingsDictionary.ContainsKey(Building_Type)){ return; }
        if (_Simbase._Handler == null || _Simbase._Handler._ImportHandler == null) { return; }

        Building b = new();
        b.id = _Simbase.GetID("b");
        
        //does it fit?  
        BuildingsDTO DTO =  _Simbase._Handler._ImportHandler.BuildingsDictionary[Building_Type];

        Vector2Int size = new Vector2Int(DTO.size_x,DTO.size_y);
        List<Vector2Int> positions = new();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                positions.Add(new Vector2Int(pos.x + x, pos.y + y));
            }
        }
        
        for(int i = 0; i < positions.Count; i++)
        {
            if (!BuildingGrid.InBoundsWorld(positions[i].x, positions[i].y))
            {
                return;
            }

            if (BuildingGrid.GetWorld(positions[i].x, positions[i].y) != 0) 
            {
                return;
            }
        }

        _Simbase.Buildings.Add(b);
        for(int i = 0; i < positions.Count; i++)
        {
            if (!BuildingGrid.InBoundsWorld(positions[i].x, positions[i].y))
            {
                return;
            }

            BuildingGrid.SetWorld(positions[i].x, positions[i].y, b.id);
        }


        _Simbase._Handler._EngineBridge._EngineConsole.Log("success");
    } 

    

    
    


}