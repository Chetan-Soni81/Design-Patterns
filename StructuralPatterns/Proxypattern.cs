using System;

namespace Exercise.StructuralPatterns
{
    public interface IImage
    {
        void Display();
        string GetImageInfo();
    }

    public class RealImage : IImage
    {
        private string _filename;
        private byte[] _imageData;

        public RealImage(string filename)
        {
            _filename = filename;
            LoadImageFromDisk();
        }

        private void LoadImageFromDisk()
        {
            Console.WriteLine($"Loading image form disk: {_filename}");

            System.Threading.Thread.Sleep(1000);
            _imageData = new byte[1024 * 1024];

            Console.WriteLine($"Image Loaded: {_filename}");
        }

        public void Display()
        {
            Console.WriteLine($"Displaying Image: {_filename}");
        }

        public string GetImageInfo()
        {
            return $"Image: {_filename}, Size: {_imageData.Length} bytes";
        }
    }

    public class ImageProxy : IImage
    {
        private string _filename;
        private RealImage _realImage;

        public ImageProxy(string filename)
        {
            _filename = filename;
        }

        public void Display()
        {
            if (_realImage == null)
            {
                _realImage = new RealImage(_filename);
            }
            _realImage.Display();
        }

        public string GetImageInfo()
        {
            if(_realImage == null)
            {
                return $"Image: {_filename} (not loaded yet)";
            }
            return _realImage.GetImageInfo();
        }
    }

    public interface IDocument
    {
        void View();
        void Edit(string content);
        void Delete();
    }

    public class Document : IDocument
    {
        private string _content;
        private string _name;

        public Document(string name, string content)
        {
            _name = name;
            _content = content;
        }

        public void View()
        {
            Console.WriteLine($"Viewing document: {_name}");
            Console.WriteLine($"Content: {_content}");
        }

        public void Edit(string content) {
            Console.WriteLine($"Editing document: {_name}");
            _content = content;
        }

        public void Delete()
        {
            Console.WriteLine($"Deleting document: {_name}");
            _content = null;
        }
    }

    public enum UserRole
    {
        Viewer,
        Editor,
        Admin
    }

    public class DocumentProxy : IDocument
    {
        private Document _document;
        private UserRole _userRole;

        public DocumentProxy(Document document, UserRole userRole)
        {
            _document = document;
            _userRole = userRole;
        }

        public void View()
        {
            _document.View();
        }

        public void Edit(string content)
        {
            if (_userRole == UserRole.Editor || _userRole == UserRole.Admin)
            {
                _document.Edit(content);
            }
            else
            {
                Console.WriteLine("Access Denied: You don't have permission to edit.");
            }
        }

        public void Delete()
        {
            if (_userRole == UserRole.Admin)
            {
                _document.Delete();
            }
            else
            {
                Console.WriteLine("Access Denied: Only admins can delete documents.");
            }
        }
    }

    public interface IDataService
    {
        string GetData(string query);
    }

    public class DatabaseService : IDataService
    {
        public string GetData(string query)
        {
            Console.WriteLine($"Fetching data from database for query: {query}");

            System.Threading.Thread.Sleep(500);
            return $"Data for '{query}' from database";
        }
    }

    public class CachingProxy : IDataService
    {
        private DatabaseService _databaseService;
        private Dictionary<string, string> _cache;

        public CachingProxy(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _cache = new Dictionary<string, string>();
        }

        public string GetData(string query)
        {
            if(_cache.ContainsKey(query))
            {
                Console.WriteLine($"Returning cached data for query: {query}");
                return _cache[query];
            }

            string data = _databaseService.GetData(query);
            _cache[query] = data;
            return data;
        }
    }

    public class ProxyDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Proxy Pattern Demo ===\n");

            Console.WriteLine("--- Virtual Proxy (Lazy Loading) ---");
            IImage image1 = new ImageProxy("image1.jpg");
            Console.WriteLine("Image proxy created, but image on loaded yet");
            Console.WriteLine(image1.GetImageInfo());
            Console.WriteLine("\nNow Displaying image:");
            image1.Display();
            Console.WriteLine("\nDisplaying again:");
            image1.Display();

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            Console.WriteLine("--- Protection Proxy (Access Control) ---");
            Document doc = new Document("Cofidential Report", "Sensitive data...");
            
            Console.WriteLine("\nViewer Role:");
            IDocument viewerProxy = new DocumentProxy(doc, UserRole.Viewer);
            viewerProxy.View();
            viewerProxy.Edit("new content");
            viewerProxy.Delete();

            Console.WriteLine("\nEditor Role:");
            IDocument editorProxy = new DocumentProxy(doc, UserRole.Editor);
            editorProxy.View();
            editorProxy.Edit("Updated Content");
            editorProxy.Delete();

            Console.WriteLine("\nAdmin Role:");
            IDocument adminProxy = new DocumentProxy(doc, UserRole.Admin);
            adminProxy.View();
            adminProxy.Edit("Admin Content");
            adminProxy.Delete();

            Console.WriteLine("--- Caching Proxy ---");
            DatabaseService dbService = new DatabaseService();
            IDataService cachedService = new CachingProxy(dbService);

            Console.WriteLine("First call:");
            string data1 = cachedService.GetData("user123");
            Console.WriteLine($"Result: {data1}\n");

            Console.WriteLine("Second call (same query):");
            string data2 = cachedService.GetData("user123");
            Console.WriteLine($"Result: {data2}\n");

            Console.WriteLine("Third call (different query):");
            string data3 = cachedService.GetData("order456");
            Console.WriteLine($"Result: {data3}");
        }
    }
}