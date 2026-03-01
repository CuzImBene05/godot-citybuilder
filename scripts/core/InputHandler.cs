
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;

public partial class InputHandler 
{
    private Handler _Handler;

    public Dictionary<string, string> Mappings = new();

    public HashSet<string> Actions = new();
    public HashSet<string> ActionsDown = new();  
    public HashSet<string> ActionsUp = new();

    public InputHandler(Handler handler)
    {
        _Handler = handler;
    }

    public void RegisterAction(string name, string key)
    {
        Mappings[name] = key;
        _Handler._EngineBridge._EngineConsole.Log($"registered {name} for {key}");
    }

    public bool GetActionDown(string name)
    {
        if (ActionsDown.Contains(name))
        {
            return true;
        }
        return false;
    }

    public bool GetAction(string name)
    {
        if (Actions.Contains(name))
        {
            return true;
        }
        return false;
    }

    public bool GetActionUp(string name)
    {
        if (ActionsUp.Contains(name))
        {
            return true;
        }
        return false;
    }

    public void ReceiveInputs(HashSet<string> _inputsDown, Vector2 leftStick, Vector2 rightStick, Vector2 mousePos)
    {
        // Build the set of actions that are currently held based on the raw input tokens.
        // NOTE: We intentionally keep this simple: mapping + hold/down/up sets.

        // Compute next actions set
        HashSet<string> nextActions = new();
        foreach (var kv in Mappings)
        {
            // kv.Key  = action name
            // kv.Value = raw input token string, e.g. "key:Space" or "mouse:Left"
            if (_inputsDown != null && _inputsDown.Contains(kv.Value))
            {
                nextActions.Add(kv.Key);
            }
        }

        // Down = in next but not in current
        ActionsDown.Clear();
        foreach (var a in nextActions)
        {
            if (!Actions.Contains(a))
                ActionsDown.Add(a);
        }

        // Up = in current but not in next
        ActionsUp.Clear();
        foreach (var a in Actions)
        {
            if (!nextActions.Contains(a))
                ActionsUp.Add(a);
        }

        // Held = next
        Actions.Clear();
        foreach (var a in nextActions)
            Actions.Add(a);

        // Sticks/mousePos are provided for higher-level logic (cursor, camera, etc.).
        // This method currently focuses only on action state.
    }

    private void TranslateInput(string input)
    {
        //from Mapping
    }


}

