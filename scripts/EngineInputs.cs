using System.Collections.Generic;
using Godot;

// Type aliases to avoid confusion between Godot.Vector2 and System.Numerics.Vector2
using NumVec2 = System.Numerics.Vector2;
using GodotVec2 = Godot.Vector2;

public partial class EngineInputs : Node2D
{
    private EngineBridge _EngineBridge;
    private ClientBase _ClientBase;

    // Reuse (kein GC pro Frame)
    // Client bekommt NUR Strings. Format:
    // - Keys:  "key:<KeyName>"  (z.B. "key:Space", "key:A")
    // - Mouse: "mouse:<ButtonName>" (z.B. "mouse:Left", "mouse:Xbutton1")
    private readonly HashSet<string> _inputsDown = new();

    public override void _Ready()
    {
        _EngineBridge = GetNode<EngineBridge>("%engine bridge");
        _ClientBase = _EngineBridge._Handler._ClientBase;
    }

    public void ReadInputs()
    {
        _inputsDown.Clear();

        // 1) Keys (Down-State)
        // NOTE: Das ist bewusst "dumm" und simpel (Polling). Optimierung später via _Input(InputEvent).

        // Letters A-Z
        for (int k = (int)Key.A; k <= (int)Key.Z; k++)
        {
            var key = (Key)k;
            if (Input.IsKeyPressed(key))
                _inputsDown.Add($"key:{key}");
        }

        // Number row 0-9
        for (int k = (int)Key.Key0; k <= (int)Key.Key9; k++)
        {
            var key = (Key)k;
            if (Input.IsKeyPressed(key))
                _inputsDown.Add($"key:{key}");
        }

        // Function keys F1-F12
        for (int k = (int)Key.F1; k <= (int)Key.F12; k++)
        {
            var key = (Key)k;
            if (Input.IsKeyPressed(key))
                _inputsDown.Add($"key:{key}");
        }

        // Keypad 0-9
        for (int k = (int)Key.Kp0; k <= (int)Key.Kp9; k++)
        {
            var key = (Key)k;
            if (Input.IsKeyPressed(key))
                _inputsDown.Add($"key:{key}");
        }

        // Common control keys
        Key[] extraKeys =
        {
            Key.Space,
            Key.Enter,
            Key.KpEnter,
            Key.Escape,
            Key.Tab,
            Key.Backspace,
            Key.Delete,
            Key.Insert,
            Key.Home,
            Key.End,
            Key.Pageup,
            Key.Pagedown,
            Key.Left,
            Key.Right,
            Key.Up,
            Key.Down,
            Key.Shift,
            Key.Ctrl,
            Key.Alt,
            Key.Meta
        };

        foreach (var key in extraKeys)
        {
            if (Input.IsKeyPressed(key))
                _inputsDown.Add($"key:{key}");
        }

        // 2) Controller Stick Inputs (device 0 als default)
        // Godot liefert Axis als float [-1..1]
        int device = 0;
        float lx = Input.GetJoyAxis(device, JoyAxis.LeftX);
        float ly = Input.GetJoyAxis(device, JoyAxis.LeftY);
        NumVec2 leftStick = new NumVec2(lx, ly);

        float rx = Input.GetJoyAxis(device, JoyAxis.RightX);
        float ry = Input.GetJoyAxis(device, JoyAxis.RightY);
        NumVec2 rightStick = new NumVec2(rx, ry);

        // 3) Mouse position (Viewport Pixel)
        GodotVec2 mousePosGodot = GetViewport().GetMousePosition();
        NumVec2 mousePos = new NumVec2(mousePosGodot.X, mousePosGodot.Y);

        // 4) Mouse buttons (wie Keyboard-Keys: nur "down" state; Click/JustPressed macht der Client)
        // NOTE: WheelUp/Down sind i.d.R. eher Event-basiert, aber wir nehmen sie hier trotzdem als "pressed" mit.
        MouseButton[] mouseButtons =
        {
            MouseButton.Left,
            MouseButton.Right,
            MouseButton.Middle,
            MouseButton.Xbutton1,
            MouseButton.Xbutton2,
            MouseButton.WheelUp,
            MouseButton.WheelDown
        };

        foreach (var mb in mouseButtons)
        {
            if (Input.IsMouseButtonPressed(mb))
                _inputsDown.Add($"mouse:{mb}");
        }

        // Send to client (Client macht Mapping + Hold/JustPressed Logik + Umrechnungen)
        _ClientBase.ReceiveInputs(_inputsDown, leftStick, rightStick, mousePos);
    }


}