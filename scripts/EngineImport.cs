using Godot;

public partial class EngineImport : Node
{
    public string ReadJsonRaw(string path)
    {
        return FileAccess.GetFileAsString(path); // reines weiterreichen
    }
}