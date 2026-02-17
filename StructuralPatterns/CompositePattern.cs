using System;

namespace Exercise.StructuralPatterns
{
    public abstract class FileSystemComponent
    {
        protected string _name;

        public FileSystemComponent(string name)
        {
            _name = name;
        }

        public abstract void Display(int depth);
        public abstract long GetSize();

        public virtual void Add(FileSystemComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(FileSystemComponent component)
        {
            throw new  NotImplementedException();
        }

        public virtual FileSystemComponent GetChild(int index)
        {
            throw new NotImplementedException();
        }
    }

    public class File : FileSystemComponent
    {
        private long _size;
        
        public File(string name, long size) : base(name)
        {
            _size = size;
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string(' ', depth + 2) + $"- {_name} ({_size} bytes)");
        }

        public override long GetSize()
        {
            return _size;
        }
    }

    public class Directory : FileSystemComponent
    {
        private List<FileSystemComponent> _children;

        public Directory(string name) : base(name)
        {
            _children = new List<FileSystemComponent>();
        }

        public override void Add(FileSystemComponent component){
            _children.Add(component);
        } 

        public override void Remove(FileSystemComponent component)
        {
            _children.Remove(component);
        }

        public override FileSystemComponent GetChild(int index)
        {
            return _children[index];
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string(' ', depth + 2) + $"+ {_name}/ ({GetSize()} bytes total)");

            foreach(var child in _children)
            {
                child.Display(depth + 1);
            }
        }

        public override long GetSize()
        {
            long totalSize = 0;
            foreach (var child in _children)
            {
                totalSize += child.GetSize();
            }

            return totalSize;
        }

        public int GetFileCount()
        {
            int count = 0;

            foreach(var child in _children)
            {
                if (child is File)
                {
                    count++;
                }
                else if (child is Directory directory)
                {
                    count += directory.GetFileCount();
                }
            }

            return count;
        }
    }

    public abstract class Employee
    {
        protected string _name;
        protected string _position;
        protected decimal _salary;

        public Employee(string name, string position, decimal salary)
        {
            _name = name;
            _position = position;
            _salary = salary;
        }

        public abstract void ShowDetails(int indent);
        public abstract decimal GetTotalSalary();

        public virtual void Add(Employee employee)
        {
            throw new NotSupportedException("Cannot add to a leaf employee");
        }

        public virtual void Remove(Employee employee)
        {
            throw new NotSupportedException("Cannot remove form a leaf employee");
        }
    }

    public class Developer : Employee
    {
        public Developer(string name, decimal salary) : base(name, "Developer", salary) {}

        public override void ShowDetails(int indent)
        {
            Console.WriteLine(new string(' ', indent + 2) + $"Developer: {_name} (Salary ${_salary})");
        }

        public override decimal GetTotalSalary()
        {
            return _salary;
        }
    }

    public class Designer : Employee
    {
        public Designer(string name, decimal salary) : base (name, "Designer", salary) {}

        public override void ShowDetails(int indent)
        {
            Console.WriteLine($"{new string (' ', indent + 2)}Designer: {_name} (Salary ${_salary})");
        }

        public override decimal GetTotalSalary()
        {
            return _salary;
        }
    }

    public class Manager : Employee
    {
        private List<Employee> _team;

        public Manager(string name, decimal salary) : base (name, "Manager", salary)
        {
            _team = new List<Employee>();
        }

        public override void Add(Employee employee)
        {
            _team.Add(employee);
        }

        public override void Remove(Employee employee)
        {
            _team.Remove(employee);
        }

        public override void ShowDetails(int indent)
        {
            Console.WriteLine($"{new string(' ', indent + 2)}Manager {_name} (Salary ${_salary}, Team Budget: {GetTotalSalary():N0})");

            foreach(var employee in _team)
            {
                employee.ShowDetails(indent + 1);
            }
        }

        public override decimal GetTotalSalary()
        {
            decimal totalSalary = _salary;
            foreach (var employee in _team)
            {
                totalSalary += employee.GetTotalSalary();
            }
            return totalSalary;
        }

        public int GetTeamSize()
        {
            int count = _team.Count;
            foreach (var employee in _team)
            {
                if (employee is Manager manager)
                {
                    count += manager.GetTeamSize();
                }
            }
            return count;
        }
    }

    public class CompositeDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Composite Pattern Demo ===\n");


            //File System Example
            Directory root = new Directory("root");
            Directory documents = new ("Documents");
            Directory pictures = new ("Pictures");
            Directory music = new ("Music");

            File file1 = new ("resume.pdf", 45000);
            File file2 = new ("cover_letter.docx", 32000);
            File file3 = new ("photo1.jpg", 2048000);
            File file4 = new ("photo2.jpg", 1536000);
            File file5 = new ("song1.mp3", 5242880);

            documents.Add(file1);
            documents.Add(file2);
            pictures.Add(file3);
            pictures.Add(file4);
            music.Add(file5);

            root.Add(documents);
            root.Add(pictures);
            root.Add(music);

            root.Display(0);
            Console.WriteLine($"\nTotal files in root: {root.GetFileCount()}");

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            // Organization Example
            Console.WriteLine("=== Organization Heirarchy ===\n");

            Developer developer1 = new("Alice Jhonson", 80000);
            Developer developer2 = new("Bom Smith", 75000);
            Developer developer3 = new("Carol White", 85000);
            Designer designer1 = new("David Brown", 70000);
            Designer designer2 = new("Emma Davis", 72000);

            Manager engineeringManager = new ("Fraklin Miller", 120000);
            engineeringManager.Add(developer1);
            engineeringManager.Add(developer2);

            Manager productManager = new ("Grace Wilson", 115000);
            productManager.Add(developer3);
            productManager.Add(designer1);
            productManager.Add(designer2);

            Manager cto = new ("Henry Tylor", 180000);
            cto.Add(engineeringManager);
            cto.Add(productManager);

            cto.ShowDetails(0);

            Console.WriteLine($"\nTotal team size under CTO: {cto.GetTeamSize()} people");
            Console.WriteLine($"Total salary budget: {cto.GetTotalSalary():N0}");
        }
    }
}