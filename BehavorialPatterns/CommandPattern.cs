using System.Text;

namespace Exercise.BehavorialPatterns
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        string GetDescription();
    }

    public class Light
    {
        private string _location;
        private int _brightness;

        public Light(string location)
        {
            _location = location;
            _brightness = 0;
        }

        public void TurnOn()
        {
            _brightness = 100;
            Console.WriteLine($"💡 {_location} light is ON (brightness: {_brightness}%)");
        }

        public void TurnOff()
        {
            _brightness =0 ;
            Console.WriteLine($"💡 {_location} is OFF");
        }

        public void Dim(int level)
        {
            _brightness = level;
            Console.WriteLine($"💡 {_location} light is dimmed to {_brightness}%");
        }

        public int GetBrightness() => _brightness;
    }

    public class Thermostat
    {
        private int _temperatue;
        public Thermostat()
        {
            _temperatue = 70;
        }

        public void SetTemperature(int temp)
        {
            _temperatue = temp;
            Console.WriteLine($"🌡️ Thermostat set to {_temperatue}°F");
        }

        public int GetTemperature() => _temperatue;
    }

    public class GarageDoor
    {
        private bool _isOpen;
        public void Open()
        {
            _isOpen = true;
            Console.WriteLine($"🚪 Garage door is OPEN");
        }

        public void Close()
        {
            _isOpen = false;
            Console.WriteLine($"🚪 Garage door is CLOSED");
        }

        public bool IsOpen() => _isOpen;
    }

    public class LightOnCommand : ICommand
    {
        private Light _light;

        public  LightOnCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOn();
        }

        public void Undo()
        {
            _light.TurnOff();
        }

        public string GetDescription() => "Turn Light On";
    }

    public class LightOffCommand : ICommand
    {
        private Light _light;

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOff();
        }

        public void Undo()
        {
            _light.TurnOn();
        }

        public string GetDescription() => "Turn Light Off";
    }

    public class DimLightCommand : ICommand
    {
        private Light _light;
        private int _newLevel;
        private int _previousLevel;

        public DimLightCommand(Light light, int level)
        {
            _light = light;
            _newLevel = level;
        }

        public void Execute()
        {
            _previousLevel = _light.GetBrightness();
            _light.Dim(_newLevel);
        }

        public void Undo()
        {
            _light.Dim(_previousLevel);
        }

        public string GetDescription() => $"Dim light to {_newLevel}%";
    }

    public class ThermostatCommand : ICommand
    {
        private Thermostat _thermostat;
        private int _newTemp;
        private int _previoudTemp;

        public ThermostatCommand(Thermostat thermostat, int temperature)
        {
            _thermostat = thermostat;
            _newTemp = temperature;
        }

        public void Execute()
        {
            _previoudTemp = _thermostat.GetTemperature();
            _thermostat.SetTemperature(_newTemp);
        }

        public void Undo()
        {
            _thermostat.SetTemperature(_previoudTemp);
        }

        public string GetDescription() => $"Set Temperature to {_newTemp}°F";
    }

    public class GarageDoorCommand : ICommand
    {
        private GarageDoor _garageDoor;
        private bool _open;
        public GarageDoorCommand(GarageDoor garageDoor, bool open)
        {
            _garageDoor = garageDoor;
            _open = open;
        }

        public void Execute()
        {
            if (_open) 
            _garageDoor.Open();
            else 
            _garageDoor.Close();
        }

        public void Undo()
        {
            if (_open)
                _garageDoor.Close();
            else
                _garageDoor.Open();
        }

        public string GetDescription() => _open ? "Open garage door" : "Close garage door";
    }

    public class MacroCommand : ICommand
    {
        private List<ICommand> _commands;

        public MacroCommand(List<ICommand> commands)
        {
            _commands = commands;
        }

        public void Execute()
        {
            Console.WriteLine("📋 Executing macro command...");
            foreach(var command in _commands)
            {
                command.Execute();
            }
        }

        public void Undo()
        {
            Console.WriteLine($"📋 Undoing macro command...");
            foreach(var command in _commands)
            {
                command.Undo();
            }
        }

        public string  GetDescription() => "Marco command";
    }

    public class RemoteControl
    {
        private Stack<ICommand> _commandHistory;

        public RemoteControl()
        {
            _commandHistory = new();
        }

        public void ExcuteCommand(ICommand command)
        {
            Console.WriteLine($"⚡ Executing {command.GetDescription()}");
            command.Execute();
            _commandHistory.Push(command);
        }

        public void Undo()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand command = _commandHistory.Pop();
                Console.WriteLine($"↩️ Undoing: {command.GetDescription()}");
                command.Undo();
            }
            else
            {
                Console.WriteLine("Nothing to Undo!");
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine($"\n 📜 Command History:");
            if(_commandHistory.Count == 0)
            {
                Console.WriteLine($" (empty)");
                return;
            }

            int i = 1;
            foreach (var command in _commandHistory.Reverse())
            {
                Console.WriteLine($"  {i}. {command.GetDescription()}");
                i++;
            }
        }
    }

    public class TextEditor
    {
        private StringBuilder _content;
        public TextEditor()
        {
            _content = new();
        }

        public void Write(string text)
        {
            _content.Append(text);
        }

        public void Delete(int count)
        {
            if(count > _content.Length)
            {
                count = _content.Length;
            }

            _content.Remove(_content.Length - count, count);
        }

        public string GetContent() => _content.ToString();

        public void Clear()
        {
            _content.Clear();
        }
    }

    public class WriteCommand : ICommand
    {
        private TextEditor _editor;
        private string _text;
        public WriteCommand(TextEditor textEditor, string text)
        {
            _editor = textEditor;
            _text = text;
        }

        public void Execute()
        {
            _editor.Write(_text);
            Console.WriteLine($"✏️ Wrote: \'{_text}\"");
        }

        public void Undo()
        {
            _editor.Delete(_text.Length);
            Console.WriteLine($"↩️ Deleted: \"{_text}\"");
        }

        public string GetDescription() => $"Write \"{_text}\"";
    }

    public class DeleteCommand : ICommand
    {
        private TextEditor _editor;
        private int _count;
        private string _deletedText;

        public DeleteCommand(TextEditor editor, int count)
        {
            _editor = editor;
            _count = count;
        }

        public void Execute()
        {
            string content = _editor.GetContent();
            int deleteCount = Math.Min(_count, content.Length);
            _deletedText = content.Substring(content.Length - deleteCount);
            _editor.Delete(deleteCount);
            Console.WriteLine($"✂️ Delete {deleteCount} characters");
        }

        public void Undo()
        {
            _editor.Write(_deletedText);
            Console.WriteLine($"↩️ Restored: \"{_deletedText}\"");
        }

        public string GetDescription() => $"Delete {_count} characters";
    }

    public class CommandDemo
    {
        public static void Run()
        {
            Console.WriteLine($"=== Command Pattern Demo ===\n");

            Console.WriteLine($"--- Smart Home Automation ---\n");

            Light livingRoomLight = new("Living Room");
            Light bedRoomLight = new("Bed Room");
            Thermostat thermostat = new();
            GarageDoor garageDoor = new();

            RemoteControl remoteControl = new();

            remoteControl.ExcuteCommand(new LightOnCommand(livingRoomLight));
            remoteControl.ExcuteCommand(new DimLightCommand(bedRoomLight, 50));
            remoteControl.ExcuteCommand(new ThermostatCommand(thermostat, 72));
            remoteControl.ExcuteCommand(new GarageDoorCommand(garageDoor, true));

            remoteControl.ShowHistory();

            Console.WriteLine("\n--- Undoing Commands ---");
            remoteControl.Undo();
            remoteControl.Undo();
            remoteControl.Undo();
        
            Console.WriteLine("\n\n--- Macro Command: Good Night Mode ---\n");

            List<ICommand> goodNightCommand = new List<ICommand>
            {
                new LightOffCommand(livingRoomLight),
                new LightOffCommand(bedRoomLight),
                new ThermostatCommand(thermostat, 68),
                new GarageDoorCommand(garageDoor, false)
            };

            MacroCommand macroCommand = new(goodNightCommand);
            remoteControl.ExcuteCommand(macroCommand);

            Console.WriteLine("\n--- Undo Good Night Command ---");
            remoteControl.Undo();

            Console.WriteLine("\n\n--- Text Editor with Undo/Redo ---\n");

            TextEditor editor = new();
            Stack<ICommand> editorHistory = new();

            var cmd1 = new WriteCommand(editor, "Hello ");
            cmd1.Execute();
            editorHistory.Push(cmd1);
            Console.WriteLine($"Content: \"{editor.GetContent()}\"");

            var cmd2 = new WriteCommand(editor, "World!");
            cmd2.Execute();
            editorHistory.Push(cmd2);
            Console.WriteLine($"Content: \"{editor.GetContent()}\"");

            var cmd3 = new WriteCommand(editor, " this a test");
            cmd3.Execute();
            editorHistory.Push(cmd3);

            Console.WriteLine("\n--- Undo Operations ---");
            while(editorHistory.Count > 0)
            {
                var cmd = editorHistory.Pop();
                cmd.Undo();
                Console.WriteLine($"Content: \"{editor.GetContent()}\"");
            }
        }
    }
}