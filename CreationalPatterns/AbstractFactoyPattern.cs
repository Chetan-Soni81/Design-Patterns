using System;
using System.ComponentModel;

namespace Exercise.CreationalPattern
{
    public interface IButton
    {
        void Render();
        void Click();
    }

    public interface ICheckBox
    {
        void Render();
        void Check();
    }

    public interface ITextBox
    {
        void Render();
        void Input(string text);
    }

    public class WindowsButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Windows-style button (Flat, Blue)");
        }

        public void Click()
        {
            Console.WriteLine("🖱️  Windows button clicked - Playing windows sound");
        }
    }

    public class WindowsCheckBox : ICheckBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Windows-style checkbox (Square)");
        }

        public void Check()
        {
            Console.WriteLine("☑️  Windows checkbox checked");
        }
    }

    public class WindowsTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Windows-style textbox (Segoe UI font)");
        }

        public void Input(string text)
        {
            Console.WriteLine($"⌨️  Input in Windows textbox: {text}");
        }
    }

    public class MacButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Mac-style button (Rounded, Gray)");
        }

        public void Click()
        {
            Console.WriteLine("🖱️  Mac button clicked - Playing Mac sound");
        }
    }

    public class MacCheckBox : ICheckBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Mac-style checkbox (Rounded)");
        }

        public void Check()
        {
            Console.WriteLine("☑️  Mac checkbox checked");
        }
    }

    public class MacTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Mac-style textbox (San Francisco font)");
        }

        public void Input(string text)
        {
            Console.WriteLine($"⌨️  Input in Mac textbox: {text}");
        }
    }

    public class LinuxButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Linux-style button (GTK theme)");
        }

        public void Click()
        {
            Console.WriteLine("🖱️  Linux button clicked");
        }
    }

    public class LinuxCheckBox : ICheckBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Linux-style checkbox (GTK style)");
        }

        public void Check()
        {
            Console.WriteLine("☑️  Linux checkbox checked");
        }
    }

    public class LinuxTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("🖼️  Rendering Linux-style textbox (Ubuntu font)");
        }

        public void Input(string text)
        {
            Console.WriteLine($"⌨️  Input in Linux textbox: {text}");
        }
    }

    public interface IUIFactory
    {
        IButton CreateButton();
        ICheckBox CreateCheckbox();
        ITextBox CreateTextbox();
        string GetThemeName();
    }

    public class WindowsFactory : IUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
        public ICheckBox CreateCheckbox() => new WindowsCheckBox();
        public ITextBox CreateTextbox() => new WindowsTextBox();
        public string GetThemeName() => "Windows Theme";
    }

    public class MacFactory : IUIFactory
    {
        public IButton CreateButton() => new MacButton();
        public ICheckBox CreateCheckbox() => new MacCheckBox();
        public ITextBox CreateTextbox() => new MacTextBox();
        public string GetThemeName() => "Mac Theme";
    }

    public class LinuxFactory : IUIFactory
    {
        public IButton CreateButton() => new LinuxButton();
        public ICheckBox CreateCheckbox() => new LinuxCheckBox();
        public ITextBox CreateTextbox() => new LinuxTextBox();
        public string GetThemeName() => "Linux Theme";
    }

    public class Application
    {
        private IButton _button;
        private ICheckBox _checkbox;
        private ITextBox _textbox;

        public Application(IUIFactory factory)
        {
            Console.WriteLine($"\n🎨 Creating application with {factory.GetThemeName()}");
            _button = factory.CreateButton();
            _checkbox = factory.CreateCheckbox();
            _textbox = factory.CreateTextbox();
        }

        public void RenderUI()
        {
            Console.WriteLine("\n📱 Rendering UI components:");
            _button.Render();
            _checkbox.Render();
            _textbox.Render();
        }

        public void InteractWithUI()
        {
            Console.WriteLine("\n👆 User interactions:");
            _button.Click();
            _checkbox.Check();
            _textbox.Input("Hello, World!");
        }
    }

    public interface IChair
    {
        void SitOn();
        string GetStyle();
    }

    public interface ISofa
    {
        void LieOn();
        string GetStyle();
    }

    public interface ITable
    {
        void PlaceItems();
        string GetStyle();
    }

    public class ModernChair : IChair
    {
        public void SitOn()
        {
            Console.WriteLine("🪑 Sitting on modern ergonomic chair");
        }

        public string GetStyle() => "Modern";
    }

    public class ModernSofa : ISofa
    {
        public void LieOn()
        {
            Console.WriteLine("🛋️ Lying on minimalist modern style");
        }

        public string GetStyle() => "Modern";
    }

    public class ModernTable : ITable
    {
        public void PlaceItems()
        {
            Console.WriteLine("📦 Placing items on sleek glass table");
        }

        public string GetStyle() => "Modern";
    }

    public class VictorianChair : IChair
    {
        public void SitOn()
        {
            Console.WriteLine("🪑 Sitting on ornate Victorian chair");
        }

        public string GetStyle() => "Victorian";
    }

    public class VictorianSofa : ISofa
    {
        public void LieOn()
        {
            Console.WriteLine("🛋️ Lying on luxurious Victorian sofa");
        }

        public string GetStyle() => "Victorian";
    }

    public class VictorianTable : ITable
    {
        public void PlaceItems()
        {
            Console.WriteLine("📦 Placing items on carved wooden table");
        }

        public string GetStyle() => "Victorian";
    }

    public interface IFurnitureFactory
    {
        IChair CreateChair();
        ISofa CreateSofa();
        ITable CreateTable();
        string GetCollectionName();
    }

    public class ModernFurnitureFactory : IFurnitureFactory
    {
        public IChair CreateChair() => new ModernChair();
        public ISofa CreateSofa() => new ModernSofa();
        public ITable CreateTable() => new ModernTable();
        public string GetCollectionName() => "Modern Collection";
    }

    public class VictorianFurnitureFactory : IFurnitureFactory
    {
        public IChair CreateChair() => new VictorianChair();
        public ISofa CreateSofa() => new VictorianSofa();
        public ITable CreateTable() => new VictorianTable();
        public string GetCollectionName() => "Victorian Collection";
    }

    public class FurnitureShowroom
    {
        private IChair _chair;
        private ISofa _sofa;
        private ITable _table;

        public FurnitureShowroom(IFurnitureFactory factory)
        {
            Console.WriteLine($"\n🏠 Setting up showroom with {factory.GetCollectionName()}");
            _chair = factory.CreateChair();
            _sofa = factory.CreateSofa();
            _table = factory.CreateTable();
        }

        public void DisplayFurniture()
        {
            Console.WriteLine("\n🛋️ Funiture on display:");
            Console.WriteLine($"  - Chair: {_chair.GetStyle()} style");
            Console.WriteLine($"  - Sofa: {_sofa.GetStyle()} style");
            Console.WriteLine($"  - Table: {_table.GetStyle()} style");
        }

        public void TestFurniture()
        {
            Console.WriteLine("\n🧪 Testing Furniture");
            _chair.SitOn();
            _sofa.LieOn();
            _table.PlaceItems();
        }
    }

    public class AbstractFactoryPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Abstract Factory Pattern Demo ===\n");

            Console.WriteLine("--- Cross-Platform UI System ---");

            string os = "Windows";
            IUIFactory uiFactory = GetUIFactory(os);

            Application app = new Application(uiFactory);
            app.RenderUI();
            app.InteractWithUI();

            Console.WriteLine($"\n{new string('-',60)}");

            Application macApp = new(new MacFactory());
            macApp.RenderUI();
            macApp.InteractWithUI();

            Console.WriteLine($"\n{new string('-',60)}");

            Application linuxApp = new(new LinuxFactory());
            linuxApp.RenderUI();
            linuxApp.InteractWithUI();

            Console.WriteLine("\n\n--- Furniture Shop System ---");

            List<IFurnitureFactory> furnitureFactories = [
                new ModernFurnitureFactory(),
                new VictorianFurnitureFactory()
            ];

            foreach (var factory in furnitureFactories)
            {
                FurnitureShowroom showroom = new(factory);
                showroom.DisplayFurniture();
                showroom.TestFurniture();
                Console.WriteLine($"\n{new string('-',60)}");
            }
        }

        private static IUIFactory GetUIFactory(string os)
        {
            return os.ToLower() switch
            {
                "windows" => new WindowsFactory(),
                "mac" => new MacFactory(),
                "linux" => new LinuxFactory(),
                _ => new WindowsFactory()
            };
        }
    }
}