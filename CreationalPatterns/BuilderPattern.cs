using System;
using System.Text;

namespace Exercise.CreationalPattern
{
    public class Computer
    {
        public string? CPU { get; set; }
        public string? RAM { get; set; }
        public string? Storage { get; set; }
        public string? GPU { get; set; }
        public string? MotherBoard { get; set; }
        public string? PowerSupply { get; set; }
        public string? Case { get; set; }
        public List<string> Peripherals { get; set; } = [];

        public void ShowSpecifications()
        {
            Console.WriteLine("\n💻 Computer Specifications");
            Console.WriteLine($"  CPU: {CPU ?? "Not Specified"}");
            Console.WriteLine($"  RAM: {RAM ?? "Not Specified"}");
            Console.WriteLine($"  Storage: {Storage ?? "Not Specified"}");
            Console.WriteLine($"  GPU: {GPU ?? "Not Specified"}");
            Console.WriteLine($"  MotherBoard: {MotherBoard ?? "Not Specified"}");
            Console.WriteLine($"  Power Supply: {PowerSupply ?? "Not Specified"}");
            Console.WriteLine($"  Case: {Case ?? "Not Specified"}");

            if (Peripherals.Count > 0)
            {
                Console.WriteLine($"  Peripherals:");
                foreach (var peripheral in Peripherals)
                {
                    Console.WriteLine($"    - {peripheral}");
                }
            }
        }

        public decimal CalculatePrice()
        {
            decimal total = 0;
            
            if (CPU?.Contains("i9") == true || CPU?.Contains("Ryzen 9") == true) total += 500;
            else if (CPU?.Contains("i7") == true || CPU?.Contains("Ryzen 7") == true) total += 350;
            else total += 200;

            if (RAM?.Contains("32GB") == true) total += 200;
            else if (RAM?.Contains("16GB") == true) total += 100;
            else total += 50;
            
            if(Storage?.Contains("1TB SSD") == true) total += 150;
            else if (Storage?.Contains("512GB SSD") == true) total += 100;
            else total += 50;

            if (GPU?.Contains("RTX 4090") == true) total += 1600;
            else if (GPU?.Contains("RTX 4070") == true) total += 600;
            else if (GPU != null) total += 300;

            total += Peripherals.Count * 50;

            return total;
        }
    }

    public interface IComputerBuilder
    {
        IComputerBuilder SetCPU(string cpu);
        IComputerBuilder SetRAM(string ram);
        IComputerBuilder SetStorage(string storage);
        IComputerBuilder SetGPU(string gpu);
        IComputerBuilder SetMotherboard(string motherboard);
        IComputerBuilder SetPowerSupply(string powerSupply);
        IComputerBuilder SetCase(string computerCase);
        IComputerBuilder AddPeripheral(string peripheral);
        Computer Build();
    }

    public class ComputerBuilder : IComputerBuilder
    {
        private Computer _computer;
        public ComputerBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _computer = new Computer();
        }

        public IComputerBuilder SetCPU(string cpu)
        {
            _computer.CPU = cpu;
            return this;
        }

        public IComputerBuilder SetRAM(string ram)
        {
            _computer.RAM = ram;
            return this;
        }

        public IComputerBuilder SetStorage(string storage)
        {
            _computer.Storage = storage;
            return this;
        }

        public IComputerBuilder SetGPU(string gpu)
        {
            _computer.GPU = gpu;
            return this;
        }

        public IComputerBuilder SetMotherboard(string motherboard)
        {
            _computer.MotherBoard = motherboard;
            return this;
        }

        public IComputerBuilder SetPowerSupply(string powerSupply)
        {
            _computer.PowerSupply = powerSupply;
            return this;
        }

        public IComputerBuilder SetCase(string computerCase)
        {
            _computer.Case = computerCase;
            return this;
        }

        public IComputerBuilder AddPeripheral(string peripheral)
        {
            _computer.Peripherals.Add(peripheral);
            return this;
        }

        public Computer Build()
        {
            var result = _computer;
            Reset();
            return result;
        }
    }

    public class ComputerDirector
    {
        private IComputerBuilder _builder;

        public ComputerDirector(IComputerBuilder builder)
        {
            _builder = builder;
        }

        public Computer BuildGamingPC()
        {
            Console.WriteLine("🎮 Building Gaming PC...");
            return _builder
                .SetCPU("Imtel Core i9-13900K")
                .SetRAM("32GB DDR5")
                .SetStorage("1TB NVMe SSD")
                .SetGPU("NVIDIA RTX 4090")
                .SetMotherboard("ASUS ROG Maximus Z790")
                .SetPowerSupply("1000W 80+ Gold")
                .SetCase("NZXT H710i RGB")
                .AddPeripheral("Gaming Keyboard (RGB)")
                .AddPeripheral("Gaming Mouse (16000 DPI)")
                .AddPeripheral("Gaming Headset")
                .Build();
        }

        public Computer BuildOfficePC()
        {
            Console.WriteLine($"💼 Building Office PC...");
            return _builder
                .SetCPU("Intel Core i5-12400")
                .SetRAM("16GB DDR4")
                .SetStorage("512GB SSD")
                .SetMotherboard("ASUS Prime B660")
                .SetPowerSupply("500W 80+ Bronze")
                .SetCase("Corsair 4000D")
                .AddPeripheral("Wireless Keyboard")
                .AddPeripheral("Wireless Mouse")
                .Build();
        }

        public Computer BuildWorkstationPC()
        {
            Console.WriteLine($"🖥️ Building Workstation PC...");
            return _builder
                .SetCPU("AMD Ryzen 9 7950X")
                .SetRAM("64GB DDR5")
                .SetStorage("2TB NVMe SSD")
                .SetGPU("NVIDIA RTX A5000")
                .SetMotherboard("ASUS Pro WS X670E")
                .SetPowerSupply("850W 80+ Platinum")
                .SetCase("Fractal Design Define 7")
                .AddPeripheral("Professional Keyboard")
                .AddPeripheral("Graphics Tablet")
                .AddPeripheral("Studio Monitor Speakers")
                .Build();
        }
    }

    public class Meal
    {
        public string MainCourse { get; set; }
        public string SideDish { get; set; }
        public string Drink { get; set; }
        public string Dessert { get; set; }
        public List<string> Extras { get; set; } = [];

        public void Display()
        {
            Console.WriteLine("\n🍽️  Meal Order:");
            Console.WriteLine($"   Main: {MainCourse ?? "None"}");
            Console.WriteLine($"   Side: {SideDish ?? "None"}");
            Console.WriteLine($"   Drink: {Drink ?? "None"}");
            Console.WriteLine($"   Dessert: {Dessert ?? "Dessert"}");

            if (Extras.Count > 0)
            {
                Console.WriteLine($"   Extras: {string.Join(", ", Extras)}");
            }
        }

        public decimal CalculateTotal()
        {
            decimal total = 0;

            if (MainCourse != null) total += 12.99m;
            if (SideDish != null) total += 4.99m;
            if (Drink != null) total += 2.99m;
            if (Dessert != null) total += 5.99m;

            total += Extras.Count * 1.99m;

            return total;
        }
    }

    public class MealBuilder
    {
        private Meal _meal;

        public MealBuilder()
        {
            _meal = new();
        }

        public MealBuilder AddMainCourse(string mainCourse)
        {
            _meal.MainCourse = mainCourse;
            return this;
        }

        public MealBuilder AddSideDish(string side)
        {
            _meal.SideDish = side;
            return this;
        }

        public MealBuilder AddDrink(string drink)
        {
            _meal.Drink = drink;
            return this;
        }

        public MealBuilder AddDessert(string dessert)
        {
            _meal.Dessert = dessert;
            return this;
        }

        public MealBuilder AddExtra(string extra)
        {
            _meal.Extras.Add(extra);
            return this;
        }

        public Meal Build()
        {
            Meal result = _meal;
            _meal = new Meal();
            return result;
        }
    }

    public class SQLQuery
    {
        public string SelectClause { get; set; }
        public string FromClause { get; set; }
        public List<string> JoinClauses { get; set; } = [];
        public string WhereClause { get; set; }
        public string OrderByClause { get; set; }
        public int? LimitValue { get; set; }

        public override string ToString()
        {
            StringBuilder query = new();

            query.AppendLine($"SELECT {SelectClause}");
            query.AppendLine($"FROM {FromClause}");

            foreach (var join in JoinClauses)
            {
                query.AppendLine(join);
            }

            if(!string.IsNullOrEmpty(WhereClause))
            {
                query.AppendLine($"WHERE {WhereClause}");
            }

            if(!string.IsNullOrEmpty(OrderByClause))
            {
                query.AppendLine($"ORDER BY {OrderByClause}");
            }

            if (LimitValue.HasValue)
            {
                query.AppendLine($"LIMIT {LimitValue}");
            }

            return query.ToString().Trim();
        }
    }

    public class SqlQueryBuilder
    {
        private SQLQuery _query;
        public SqlQueryBuilder()
        {
            _query = new SQLQuery();
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            _query.SelectClause = string.Join(", ", columns);
            return this;
        }

        public SqlQueryBuilder From(string table)
        {
            _query.FromClause = table;
            return this;
        }

        public SqlQueryBuilder Join(string table, string condition)
        {
            _query.JoinClauses.Add($"JOIN {table} ON {condition}");
            return this;
        }

        public SqlQueryBuilder Where(string condition)
        {
            _query.WhereClause = condition;
            return this;
        }

        public SqlQueryBuilder OrderBy(string column, bool ascending = true)
        {
            _query.OrderByClause = $"{column} {(ascending ? "ASC" : "DESC")}";
            return this;
        }

        public SqlQueryBuilder Limit(int limit)
        {
            _query.LimitValue = limit;
            return this;
        }

        public SQLQuery Build()
        {
            var result = _query;
            _query = new SQLQuery();
            return result;
        }
    }

    public class BuilderDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Builder Pattern Demo ===\n");

            Console.WriteLine("--- Computer Builder with Director ---");

            IComputerBuilder builder = new ComputerBuilder();
            ComputerDirector director = new(builder);

            Computer gamingPC = director.BuildGamingPC();
            gamingPC.ShowSpecifications();
            Console.WriteLine($"💰 Total Price: ${gamingPC.CalculatePrice():F2}");

            Console.WriteLine("\n"+new string('-', 60));

            Computer workstationPC = director.BuildWorkstationPC();
            workstationPC.ShowSpecifications();
            Console.WriteLine($"💰 Total Price: ${workstationPC.CalculatePrice():F2}");

            Console.WriteLine("\n\n--- Custom Computer Build ---");

            Computer customPC = new ComputerBuilder()
                .SetCPU("AMD Ryzen 7 5800X")
                .SetRAM("32GB DDR4")
                .SetStorage("1TB SSD")
                .SetGPU("NVIDIA RTX 4070")
                .SetCase("Custom RGB Case")
                .AddPeripheral("Mechnical Keyboard")
                .AddPeripheral("Ultrawide Monitor")
                .Build();

            customPC.ShowSpecifications();
            Console.WriteLine($"💰 Total Price: ${customPC.CalculatePrice():F2}");

            Console.WriteLine("\n\n--- Restaurant Meal Builder ---");

            MealBuilder mealBuilder = new MealBuilder();

            Meal combo1 = mealBuilder
                .AddMainCourse("Grilled Chicken")
                .AddSideDish("French Fries")
                .AddDrink("Coca cola")
                .AddDessert("Chocolate cake")
                .Build();

            combo1.Display();
            Console.WriteLine($"💰 Total Price: ${combo1.CalculateTotal():F2}");

            Console.WriteLine("\n" + new string('-', 60));

            Meal combo2 = mealBuilder
                .AddMainCourse("Beef Burger")
                .AddSideDish("Onion Rings")
                .AddDrink("Lemonade")
                .AddExtra("Extra Cheese")
                .AddExtra("Bacon")
                .Build();

            combo2.Display();
            Console.WriteLine($"💰 Total Price: ${combo2.CalculateTotal():F2}");

            Console.WriteLine("\n\n--- SQL Query Builder ---");

            SqlQueryBuilder queryBuilder = new();

            SQLQuery query1 = queryBuilder
                .Select("id", "name", "email")
                .From("users")
                .Where("age > 18")
                .OrderBy("name", true)
                .Limit(10)
                .Build();

            Console.WriteLine("📝 Query 1:");
            Console.WriteLine(query1.ToString());

            Console.WriteLine("\n" + new string('-', 60));

            SQLQuery query2 = queryBuilder
                .Select("o.id", "o.total", "u.name")
                .From("orders o")
                .Join("users u", "o.user_id = u.id")
                .Where("o.status = 'completed'")
                .OrderBy("o.total", false)
                .Build();
            
            Console.WriteLine("📝 Query 2:");
            Console.WriteLine(query2.ToString());
        }
    }
}