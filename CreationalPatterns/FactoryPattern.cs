using System;

namespace Exercise.CreationalPattern
{
    public interface IDocument
    {
        void Open();
        void Save();
        void Close();
        string GetDocumentType();
    }

    public class WordDocument : IDocument
    {
        private string _content;

        public WordDocument()
        {
            _content = "";
            Console.WriteLine("📄 Word document created");
        }

        public void Open()
        {
            Console.WriteLine("📂 Opening Word document in Microsoft Word");
        }

        public void Save()
        {
            Console.WriteLine("💾 Saving Word document (.docx)");
        }

        public void Close()
        {
            Console.WriteLine("❌ Closing Word document");
        }

        public string GetDocumentType() => "Word document";
    }

    public class PDFDocument : IDocument
    {
        public PDFDocument()
        {
            Console.WriteLine("📕 PDF document created");
        }

        public void Open()
        {
            Console.WriteLine("📂 Opening PDF in Adobe Reader");
        }

        public void Save()
        {
            Console.WriteLine("💾 Saving PDF document (.pdf)");
        }

        public void Close()
        {
            Console.WriteLine("❌ Closing PDF document");
        }

        public string GetDocumentType() =>  "PDF Document";
    }

    public class ExcelDocument : IDocument
    {
        public ExcelDocument()
        {
            Console.WriteLine("📊 Excel document created");
        }

        public void Open()
        {
            Console.WriteLine("📂 Opening Excel spreadsheet in Microsoft Excel");
        }

        public void Save()
        {
            Console.WriteLine("💾 Saving Excel document (.xlsx)");
        }

        public void Close()
        {
            Console.WriteLine("❌ Closing Excel document");
        }

        public string GetDocumentType() => "Excel Document";
    }

    public abstract class DocumentCreator
    {
        public abstract IDocument CreateDocument();

        public void ProcessDocument()
        {
            Console.WriteLine($"\n--- Processing with {GetCreatorType()} ---");
            IDocument doc = CreateDocument();

            doc.Open();
            Console.WriteLine($"✏️ Working with {doc.GetDocumentType()}");
            doc.Save();
            doc.Close();
        }

        protected abstract string GetCreatorType();
    }

    public class WordDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument()
        {
            return new WordDocument();
        }

        protected override string GetCreatorType() => "Word Creator";
    }

    public class PDFDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument()
        {
            return new PDFDocument();
        }

        protected override string GetCreatorType() => "PDF Creator";
    }

    public class ExcelDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument()
        {
            return new ExcelDocument();
        }

        protected override string GetCreatorType() => "Excel Creator";
    }

    public interface IVehicle
    {
        void Manufacture();
        void Assemble();
        void Test();
        string GetVehicleType();
    }

    public class Car : IVehicle
    {
        private string _model;
        public Car(string model)
        {
            _model = model;
        }

        public void Manufacture()
        {
            Console.WriteLine($"🏭 Manufacturing car: {_model}");
        }

        public void Assemble()
        {
            Console.WriteLine("🔧 Assembling car components");
        }

        public void Test()
        {
            Console.WriteLine("✅ Car quality test passed");
        }

        public string GetVehicleType() => $"Car ({_model})";
    }

    public class Truck : IVehicle
    {
        private string _model;
        public Truck(string model)
        {
            _model = model;
        }

        public void Manufacture()
        {
            Console.WriteLine($"🏭 Manufacturing Truck: {_model}");
        }

        public void Assemble()
        {
            Console.WriteLine($"🔧 Assembling heavy-duty truck components");
        }

        public void Test()
        {
            Console.WriteLine("✅ Truck load capacity test passed");
        }

        public string GetVehicleType() => $"Truck ({_model})";
    }

    public class Motorcycle : IVehicle
    {
        private string _model;
        public Motorcycle(string model)
        {
            _model = model;
        }

        public void Manufacture()
        {
            Console.WriteLine($"🏭 Manufacturing motorcycle: {_model}");
        }

        public void Assemble()
        {
            Console.WriteLine($"🔧 Assembling motorcycle parts");
        }

        public void Test()
        {
            Console.WriteLine("✅ Motorcycle safety test passed");
        }

        public string GetVehicleType() => $"Motorcycle ({_model})";
    }

    public abstract class VehicleFactory
    {
        public abstract IVehicle CreateVehicle(string model);

        public void OrderVehicle(string model)
        {
            Console.WriteLine($"\n--- Processing order via {GetFactoryName()} ---\n");
            IVehicle vehicle = CreateVehicle(model);

            vehicle.Manufacture();
            vehicle.Assemble();
            vehicle.Test();

            Console.WriteLine($"✅ {vehicle.GetVehicleType()} is ready for delivery!");
        }

        protected abstract string GetFactoryName();
    }

    public class CarFactory : VehicleFactory
    {
        public override IVehicle CreateVehicle(string model)
        {
            return new Car(model);
        }

        protected override string GetFactoryName() => "Car Factory";
    }

    public  class TruckFactory : VehicleFactory
    {
        public override IVehicle CreateVehicle(string model)
        {
            return new Truck(model);
        }

        protected override string GetFactoryName() => "Truck Factory";
    }

    public  class MotorcycleFactory : VehicleFactory
    {
        public override IVehicle CreateVehicle(string model)
        {
            return new Motorcycle(model);
        }

        protected override string GetFactoryName() => "Motorcycle Factory";
    }

    public class FactoryPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Factory Method Pattern Demo ===\n");

            Console.WriteLine("--- Document Creation System ---");

            List<DocumentCreator> creators =
            [
                new WordDocumentCreator(),
                new PDFDocumentCreator(),
                new ExcelDocumentCreator()
            ];

            foreach(var creator in creators)
            {
                creator.ProcessDocument();
            }

            Console.WriteLine("\n\n--- Vehicle Manufacting System ---");

            List<VehicleFactory> factories = [
                new CarFactory(),
                new TruckFactory(),
                new MotorcycleFactory()
            ];

            List<string> models =
            [
                "Sedan 2026",
                "Heavy Duty 4000",
                "Cruise bike 650"
            ];

            for (int i = 0; i < factories.Count; i++)
            {
                factories[i].OrderVehicle(models[i]);
            }

            Console.WriteLine("\n\n--- Runtime Factory Selection ---");

            string userChoice = "car";

            VehicleFactory? selectedFactory = GetFactory(userChoice);
            selectedFactory?.OrderVehicle("Custom Model");
        }

        private static VehicleFactory? GetFactory(string type)
        {
            return type.ToLower() switch
            {
                "car" => new CarFactory(),
                "truck" => new TruckFactory(),
                "motercycle" => new MotorcycleFactory(),
                _ => null
            };
        }
    }
}