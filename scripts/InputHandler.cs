
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;

public partial class InputHandler 
{
    private Handler _Handler;

    public InputHandler(Handler handler)
    {
        _Handler = handler;
    }

    public void SendAction()
    {
        
    }




    public void ReceiveInputs(HashSet<string> _inputsDown, Vector2 leftStick, Vector2 rightStick, Vector2 mousePos)
    {
        if(_inputsDown.Count > 0)
        {
            _Handler._EngineConsole.Log(_inputsDown.First());
        }
        
    }

    private void TranslateInput(string input)
    {
        //from Mapping
    }


}

//actions
public class Action
{
    public string name;
}

public class Mappings()
{
    
}

//irgendwas wie "pressed Buttons" und dann wie lange die gehalten sind oder eingabe der staerke
