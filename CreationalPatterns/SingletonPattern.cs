using System;

namespace Exercise.CreationalPattern
{
    public sealed class DatabaseConnection
    {
        private static DatabaseConnection _instance = null;
        private static readonly object _lock = new object();
        private string _connectionString;
        private bool _isConnected;
        private DatabaseConnection()
        {
            _connectionString = "Server=localhost;Database=MyDB;";
            _isConnected = false;
            Console.WriteLine("🔧 DatabaseConnection instance created");
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Connect()
        {
            if(!_isConnected)
            {
                Console.WriteLine($"🔌 Connecting to database: {_connectionString}");
                System.Threading.Thread.Sleep(500);
                _isConnected = true;
                Console.WriteLine($"✅  Connected to database");
            }
            else
            {
                Console.WriteLine($"ℹ️  Already connected");
            }
        }

        public void ExecuteQuery(string query)
        {
            if (_isConnected)
            {
                Console.WriteLine($"📊 Executing: {query}");
            }
            else
            {
                Console.WriteLine("❌ Not connected to database");
            }
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                Console.WriteLine($"🔌 Disconnecting from database");
                _isConnected = false;
                Console.WriteLine($"✅ Disconnected");
            }
        }
    }

    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

        private List<string> _logs;

        private Logger()
        {
            _logs = new();

            Console.WriteLine("📝 Logger instance created");
        }

        public static Logger Instance => _instance.Value;

        public void Log(string message)
        {
            string timestampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            _logs.Add(timestampedMessage);
            Console.WriteLine($"📝 LOG: {timestampedMessage}");
        }

        public void ShowLogs()
        {
            Console.WriteLine($"\n📋 All Logs:");
            foreach(var log in _logs)
            {
                Console.WriteLine($"  {log}");
            }
        }

        public int GetLogCount() => _logs.Count;
    }

    public sealed class ConfigurationManager
    {
        private static readonly ConfigurationManager _instance = new();
        private Dictionary<string,string> _settings;
        static ConfigurationManager()
        {
            
        }

        private ConfigurationManager()
        {
            _settings = new()
            {
                {"AppName", "MyApplication"},
                {"Version", "1.0.0"},
                {"Enviorment", "Production"},
                {"MaxConnections", "100"}
            };
            Console.WriteLine($"⚙️ ConfigurationManager instance created");
        }

        public static ConfigurationManager Instance => _instance;

        public string  GetSetting(string key)
        {
            return _settings.ContainsKey(key) ? _settings[key] : null;
        }

        public void SetSetting(string key, string value)
        {
            _settings[key] = value;
            Console.WriteLine($"⚙️  Setting updated: {key} = {value}");
        }

        public void ShowAllSettings()
        {
            Console.WriteLine($"\n⚙️ All Settings:");
            foreach (var setting in _settings)
            {
                Console.WriteLine($"  {setting.Key}: {setting.Value}");
            }
        }
    }

    public class SingletonDemo
    {
        public static void Run()
        {
            Console.WriteLine($"=== Singleton Pattern Demo ===\n");

            Console.WriteLine($"--- Database Connection Singleton ---\n");

            var db1 = DatabaseConnection.Instance;
            var db2 = DatabaseConnection.Instance;

            Console.WriteLine($"Are db1 and db2 the same instance? {ReferenceEquals(db1, db2)}");

            db1.Connect();
            db1.ExecuteQuery("Select * FROM USERS");
            db2.ExecuteQuery("INSERT INTO USERS value ('Jhon')");
            db1.Disconnect();

            Console.WriteLine("\n\n--- Logger Singleton ---\n");

            var logger1 = Logger.Instance;
            var logger2 = Logger.Instance;

            logger1.Log("Application Started");
            logger2.Log("User Logged in");
            logger1.Log("Processing request");

            logger1.ShowLogs();
            Console.WriteLine($"Total Logs: {logger2.GetLogCount()}");

            Console.WriteLine("\n\n--- Configuration Manager Singleton ---\n");

            var config1 = ConfigurationManager.Instance;
            var config2 = ConfigurationManager.Instance;

            Console.WriteLine($"Are config1 and config2 the same instance? {ReferenceEquals(config1, config2)}");

            config1.ShowAllSettings();
            config2.SetSetting("Theme", "Dark");
            config2.ShowAllSettings();

            Console.WriteLine("\n\n--- Thread Safety Test ---\n");
            TestThreadSafety();
        }

        public static void TestThreadSafety()
        {
            const int threadCount = 5;
            var threads = new System.Threading.Thread[threadCount];
            var instances = new Logger[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                threads[i] = new System.Threading.Thread(() =>
                {
                    instances[index] = Logger.Instance;
                    instances[index].Log($"Thread {index} accessed singleton");
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("\n🔍 Checking if all thread got the same instance:");
            bool allSame = true;

            for (int i = 1; i < threadCount; i++)
            {
                if(!ReferenceEquals(threads[0], threads[i]))
                {
                    allSame = false;
                    break;
                }
            }

            Console.WriteLine(allSame ? "✅ All thread got the same instance (Thread-safe!)" : "❌ Different instances created (Not-Thread-safe!)");
        }
    }
}