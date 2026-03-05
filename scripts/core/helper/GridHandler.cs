using System.Collections.Generic;

public class GridHandler
{
    public SimBase _Simbase;



    public GridHandler(SimBase simBase)
    {
        _Simbase = simBase;
    }


    public bool IsGridPosEmpty(Vector2Int pos)
    {
        //Buildings
        if (!_Simbase._BuildingSim.BuildingGrid.InBoundsWorld(pos.x, pos.y))
        {
            return false;
        }

        if (_Simbase._BuildingSim.BuildingGrid.GetWorld(pos.x, pos.y) != 0) 
        {
            return false;
        }



        return true;
    }

    public bool IsGridPosEmpty(List<Vector2Int> positions)
    {
        for(int i = 0; i < positions.Count; i++)
        {
            if (!IsGridPosEmpty(positions[i]))
            {
                return false;
            }     
        }
        return true;
    }
}