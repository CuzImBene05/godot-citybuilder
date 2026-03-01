using System.Collections.Generic;
using System.Text.Json;

public partial class ImportHandler
{
    private Handler _Handler;

    public Dictionary<string, BuildingsDTO> BuildingsDictionary;

    public ImportHandler(Handler handler)
    {
        _Handler = handler;
    }




    public void ImportAll()
    {
        string json;
        Dictionary<string, BuildingsDTO> data;

        //Buildings
        json = _Handler._EngineBridge._EngineImport.ReadJsonRaw("res://load/Buildings.json");
        data = JsonSerializer.Deserialize<Dictionary<string, BuildingsDTO>>(json);
        BuildingsDictionary = data ?? new Dictionary<string, BuildingsDTO>();

        //


    }




}