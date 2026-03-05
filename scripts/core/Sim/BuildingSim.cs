using System.Collections.Generic;

public class BuildingSim
{
    public SimBase _Simbase;

    public Grid<int> BuildingGrid;
    public List<Building> Buildings = new();

    // Subscription
    private const string SUB_BUILDING_LIST = "BuildingList";
    private const int CHANGE_BUILDING_ADDED = 1;
    private const int CHANGE_BUILDING_REMOVED = 2;


    public BuildingSim(SimBase simBase)
    {
        _Simbase = simBase;
    }


    public void Tick()
    {
        // TODO
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
        if (BuildingGrid == null) { return; }
        if (_Simbase._Handler == null || _Simbase._Handler._ImportHandler == null) { return; }

        var dict = _Simbase._Handler._ImportHandler.BuildingsDictionary;
        if (dict == null) { return; }
        if (!dict.TryGetValue(Building_Type, out BuildingsDTO DTO)) { return; }

        Building b = new();
        b.id = GetNewID();

        // does it fit? (pos = down-left corner)
        Vector2Int size = new Vector2Int(DTO.size_x, DTO.size_y);
        List<Vector2Int> positions = new();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                positions.Add(new Vector2Int(pos.x + x, pos.y + y));
            }
        }

        // Validate empty (your GridHandler decides)
        if (!_Simbase._GridHandler.IsGridPosEmpty(positions)) { return; }

        // Commit: add to list
        Buildings.Add(b);

        // Commit: write to grid
        for (int i = 0; i < positions.Count; i++)
        {
            int wx = positions[i].x;
            int wy = positions[i].y;

            if (!BuildingGrid.InBoundsWorld(wx, wy))
            {
                // safety: if out of bounds, rollback list add and return
                Buildings.RemoveAt(Buildings.Count - 1);
                return;
            }

            BuildingGrid.SetWorld(wx, wy, b.id);
        }

        // Log change
        if (_Simbase._Handler._SubscriptionHandler != null)
        {
            Change c = new();
            c.kind = CHANGE_BUILDING_ADDED;
            c.a = b.id;
            c.descr = "building added";
            _Simbase._Handler._SubscriptionHandler.Log(SUB_BUILDING_LIST, c);
        }

        _Simbase._Handler._EngineBridge._EngineConsole.Log("success");
    }

    public void RemoveBuilding(int id)
    {
        if (BuildingGrid == null) { return; }
        if (_Simbase._Handler == null) { return; }
        if (id <= 0) { return; }

        int removeId = id;

        // Remove from list (if present)
        int idx = GetIndexByID(removeId);
        if (idx >= 0)
        {
            Buildings.RemoveAt(idx);
        }

        // Clear all grid cells that reference this id (simple + safe for now)
        // Iterate in grid space and convert to world coords.
        for (int gy = 0; gy < BuildingGrid.Height; gy++)
        {
            for (int gx = 0; gx < BuildingGrid.Width; gx++)
            {
                int wx = gx - BuildingGrid.OffsetX;
                int wy = gy - BuildingGrid.OffsetY;

                if (BuildingGrid.GetWorld(wx, wy) == removeId)
                {
                    BuildingGrid.SetWorld(wx, wy, 0);
                }
            }
        }

        // Log change
        if (_Simbase._Handler._SubscriptionHandler != null)
        {
            Change c = new();
            c.kind = CHANGE_BUILDING_REMOVED;
            c.a = removeId;
            c.descr = "building removed";
            _Simbase._Handler._SubscriptionHandler.Log(SUB_BUILDING_LIST, c);
        }
    }

    public void RemoveBuilding(Vector2Int pos)
    {
        if (BuildingGrid == null) { return; }

        int wx = pos.x;
        int wy = pos.y;
        if (!BuildingGrid.InBoundsWorld(wx, wy)) { return; }

        int id = BuildingGrid.GetWorld(wx, wy);
        if (id == 0) { return; }

        RemoveBuilding(id);
    }

    public int GetIndexByID(int id)
    {
        int left = 0;
        int right = Buildings.Count - 1;

        while (left <= right)
        {
            int mid = left + ((right - left) / 2);
            int midId = Buildings[mid].id;

            if (midId == id)
            {
                return mid;
            }

            if (midId < id)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return -1;
    }

    public int GetNewID()
    {
        int maxId = 0;
        for (int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings[i].id > maxId) maxId = Buildings[i].id;
        }

        return maxId + 1;
    }
}