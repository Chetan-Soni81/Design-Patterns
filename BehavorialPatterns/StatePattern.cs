using System;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Exercise.BehavorialPatterns
{
    public interface IVendingMachineState
    {
        void InsertCoin(VendingMachine machine, decimal amount);
        void SelectProduct(VendingMachine machine, string product);
        void DispenseProduct(VendingMachine machine);
        void ReturnMoney(VendingMachine machine);
        string GetStateName();
    }

    public class VendingMachine
    {
        private IVendingMachineState _currentState;
        private decimal _balance;
        private Dictionary<string, (decimal price, int stock)> _inventory;
        private string _selectedProduct;

        public VendingMachine()
        {
            _currentState = new IdleState();
            _balance = 0;
            _inventory = new Dictionary<string, (decimal, int)>
            {
                {"Coke", (1.50m, 10)},
                {"Pepsi", (1.50m, 8)},
                {"Water", (1.00m, 15)},
                {"Chips", (2.00m, 12)},
                {"Candy", (1.25m, 20)}
            };
        }

        public void SetState(IVendingMachineState state)
        {
            Console.WriteLine($"State changed: {_currentState.GetStateName()} -> {state.GetStateName()}");
            _currentState = state;
        }

        public IVendingMachineState GetState() => _currentState;

        public void InsertCoin(decimal amount)
        {
            _currentState.InsertCoin(this, amount);
        }

        public void SelectProduct(string product)
        {
            _currentState.SelectProduct(this, product);
        }

        public void Dispense()
        {
            _currentState.DispenseProduct(this);
        }

        public void ReturnMoney()
        {
            _currentState.ReturnMoney(this);
        }

        public void AddBalance(decimal amount)
        {
            _balance += amount;
            Console.WriteLine($"💰 Balance: ${_balance:F2}");
        }

        public decimal GetBalance() => _balance;

        public void ResetBalance()
        {
            _balance = 0;
        }

        public void SetSelectedProduct(string product)
        {
            _selectedProduct = product;
        }

        public string GetSelectedProduct() => _selectedProduct;

        public bool HasProduct(string product)
        {
            return _inventory.ContainsKey(product);
        }

        public decimal GetProductPrice(string product)
        {
            return _inventory.ContainsKey(product) ? _inventory[product].price : 0;
        }

        public bool IsProductInStock(string product)
        {
            return _inventory.ContainsKey(product) && _inventory[product].stock > 0;
        }

        public void ReductStock(string product)
        {
            if (_inventory.ContainsKey(product))
            {
                var item = _inventory[product];
                _inventory[product] = (item.price, item.stock - 1);
            }
        }

        public void ShowInventory()
        {
            Console.WriteLine($"\n📦 Inventory:");
            foreach(var item in _inventory)
            {
                string status = item.Value.stock > 0 ? "✅" : "❌";
                Console.WriteLine($" {status} {item.Key}: ${item.Value.price:F2} (Stock: {item.Value.stock})");
            }
            Console.WriteLine();
        }
    }

    public class IdleState : IVendingMachineState
    {
        public void InsertCoin(VendingMachine machine, decimal amount)
        {
            Console.WriteLine($"💵 Coin Inserted: ${amount:F2}");
            machine.AddBalance(amount);
            machine.SetState(new HasMoneyState());
        }

        public void SelectProduct(VendingMachine machine, string product)
        {
            Console.WriteLine($"❌ Please insert money first!");
        }

        public void DispenseProduct(VendingMachine machine)
        {
            Console.WriteLine($"❌ Cannot dispense - no product selected!");
        }

        public void ReturnMoney(VendingMachine machine)
        {
            Console.WriteLine($"ℹ️ No money to return!");
        }

        public string GetStateName() => "Idle";
    }

    public class HasMoneyState : IVendingMachineState
    {
        public void InsertCoin(VendingMachine machine, decimal amount)
        {
            Console.WriteLine($"💵 Additional coin inserted: ${amount:F2}");
            machine.AddBalance(amount);
        }

        public void SelectProduct(VendingMachine machine, string product)
        {
            if(!machine.HasProduct(product))
            {
                Console.WriteLine($"❌ Product '{product}' not available!");
                return;
            }

            if(!machine.IsProductInStock(product))
            {
                Console.WriteLine($"❌ Product '{product}' is out of stock!");
                return;
            }

            decimal price = machine.GetProductPrice(product);
            if(machine.GetBalance() < price)
            {
                Console.WriteLine($"❌ Insufficient funds! Need ${price:F2}, have ${machine.GetBalance():F2}");
                return;
            }

            Console.WriteLine($"✅ Selected: {product} (${price:F2})");
            machine.SetSelectedProduct(product);
            machine.SetState(new DispensingState());
            machine.Dispense();
        }

        public void DispenseProduct(VendingMachine machine)
        {
            Console.WriteLine("❌ Please select a product first!");
        }

        public void ReturnMoney(VendingMachine machine)
        {
            Console.WriteLine($"💵 Returning ${machine.GetBalance():F2}");
            machine.ResetBalance();
            machine.SetState(new IdleState());
        }

        public string GetStateName() => "Has Money";
    }

    public class DispensingState : IVendingMachineState
    {
        public void InsertCoin(VendingMachine machine, decimal amount)
        {
            Console.WriteLine("⌛ Please wait, dispensing product...");
        }

        public void SelectProduct(VendingMachine machine, string product)
        {
            Console.WriteLine($"⌛ Already dispensing a product!");
        }

        public void DispenseProduct(VendingMachine machine)
        {
            string product = machine.GetSelectedProduct();
            decimal price = machine.GetProductPrice(product);

            Console.WriteLine($"🎁 Dispensing {product}...");
            System.Threading.Thread.Sleep(1000);

            machine.ReductStock(product);
            decimal change = machine.GetBalance() - price;
            machine.ResetBalance();

            if (change > 0)
            {
                Console.WriteLine($"💵 Returning change: ${change:F2}");
            }

            Console.WriteLine($"✅ Enjoy your product!");
            machine.SetState(new IdleState());
        }

        public void ReturnMoney(VendingMachine machine)
        {
            Console.WriteLine("❌ Cannot return money while dispensing!");
        }

        public string GetStateName() => "Dispensing";
    }

    public enum DocumentStatus
    {
        Draft,
        PendingReview,
        Approved,
        Published,
        Rejected
    }

    public interface IDocumentState
    {
        void Edit(Document document);
        void Submit(Document document);
        void Approve(Document document);
        void Reject(Document document);
        void Publish(Document document);
        string GetStatus();
    }

    public class Document
    {
        private IDocumentState _state;
        private string _content;
        private string _author;
        public Document(string author)
        {
            _author = author;
            _content = "";
            _state = new DraftState();
            Console.WriteLine($"📄 New document created by {_author}");
        }

        public void SetState(IDocumentState state)
        {
            Console.WriteLine($"Status: {_state.GetStatus()} -> {state.GetStatus()}");
            _state = state;
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        public string GetContent() => _content;
        public string GetAuthor() => _author;

        public void Edit(string content)
        {
            _state.Edit(this);
            if(_state is DraftState)
            {
                _content = content;
                Console.WriteLine($"✏️ Content updated: \"{content.Substring(0, Math.Min(50, content.Length))}...\"");
            }
        }

        public void Submit() =>_state.Submit(this);
        public void Approve() => _state.Approve(this);
        public void Reject(string reason) => _state.Reject(this);
        public void Publish() => _state.Publish(this);
        public void GetStatus() => _state.GetStatus();
    }

    public class DraftState : IDocumentState
    {
        public void Edit(Document document)
        {
            Console.WriteLine($"✅ Editing allowed in Draft state");
        }

        public void Submit(Document document)
        {
            Console.WriteLine($"📤 Submitting document for review...");
            document.SetState(new PendingReviewState());
        }

        public void Approve(Document document)
        {
            Console.WriteLine($"❌ Cannot approved - document not submitted yet");
        }

        public void Reject(Document document)
        {
            Console.WriteLine($"❌ Cannot reject - document not submitted yet");
        }

        public void Publish(Document document)
        {
            Console.WriteLine($"❌ Cannot publish - document not approved");
        }

        public string GetStatus() => "Draft";
    }

    public class PendingReviewState : IDocumentState
    {
        public void Edit(Document document)
        {
            Console.WriteLine($"❌ Cannot edit - document is under review");
        }

        public void Submit(Document document)
        {
            Console.WriteLine($"ℹ️ Document already submitted");
        }

        public void Approve(Document document)
        {
            Console.WriteLine($"✅ Document approved!");
            document.SetState(new ApprovedState());
        }

        public void Reject(Document document)
        {
            Console.WriteLine($"❌ Document rejected - returning to draft");
            document.SetState(new RejectedState());
        }

        public void Publish(Document document)
        {
            Console.WriteLine($"❌ Cannot publish - need approval first");
        }

        public string GetStatus() => "Pending Review";
    }

    public class ApprovedState : IDocumentState
    {
        public void Edit(Document document)
        {
            Console.WriteLine($"❌ Cannot edit approved document");
        }

        public void Submit(Document document)
        {
            Console.WriteLine($"ℹ️ Document already approved");
        }

        public void Approve(Document document)
        {
            Console.WriteLine($"ℹ️ Document already approved");
        }

        public void Reject(Document document)
        {
            Console.WriteLine($"⚠️ Rejecting approved document");
            document.SetState(new RejectedState());
        }

        public void Publish(Document document)
        {
            Console.WriteLine($"🚀 Publishing document...");
            document.SetState(new PublishedState());
        }

        public string GetStatus() => "Approved";
    }

    public class PublishedState : IDocumentState
    {
        public void Edit(Document document)
        {
            Console.WriteLine($"❌ Cannot edit published document");
        }

        public void Submit(Document document)
        {
            Console.WriteLine("ℹ️ Document already published");
        }

        public void Approve(Document document)
        {
            Console.WriteLine($"ℹ️ Document already published");
        }

        public void Reject(Document document)
        {
            Console.WriteLine($"❌ Cannot reject already published document");
        }

        public void Publish(Document document)
        {
            Console.WriteLine($"ℹ️ Document already published");
        }

        public string GetStatus() => "Published";
    }

    public class RejectedState : IDocumentState
    {
        public void Edit(Document document)
        {
            Console.WriteLine($"✅ Moving back to Draft for edits");
            document.SetState(new DraftState());
        }

        public void Submit(Document document)
        {
            Console.WriteLine($"📤 Resubmitted document");
            document.SetState(new PendingReviewState());
        }

        public void Approve(Document document)
        {
            Console.WriteLine($"❌ Cannot approve rejected document - needs resubmission");
        }

        public void Reject(Document document)
        {
            Console.WriteLine($"ℹ️ Document already rejected");
        }

        public void Publish(Document document)
        {
            Console.WriteLine($"❌ Cannot publish rejected document");
        }

        public string GetStatus() => "Rejected";
    }

    public class StateDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== State Pattern Demo ===\n");

            // Vending Machine example
            Console.WriteLine("--- Vending Machine ---\n");

            VendingMachine machine = new();
            machine.ShowInventory();

            machine.SelectProduct("Coke");

            machine.InsertCoin(1.00m);
            machine.InsertCoin(0.50m);
            machine.SelectProduct("Coke");

            Console.WriteLine($"\n{new string('-', 50)}\n");

            machine.InsertCoin(2.00m);
            machine.SelectProduct("Chips");

            Console.WriteLine($"\n{new string('-', 50)}\n");

            machine.InsertCoin(5.00m);
            machine.ReturnMoney();

            machine.ShowInventory();

            // Document workflow example
            Console.WriteLine("\n\n--- Document Workflow ---\n");

            Document doc = new("Chetan Soni");

            doc.Edit("This is the initial content of the document.");
            doc.Edit("Updated content with more details.");

            doc.Submit();

            doc.Edit("Cannot edit now");

            doc.Approve();

            doc.Publish();

            doc.Edit("Cannot edit published");

            Console.WriteLine($"\n{new string('-',50)}\n");

            Document doc2 = new("Senorita ji");
            doc2.Edit("Initial draft content");
            doc2.Submit();
            doc2.Reject("Need more detials");
            doc2.Edit("Update with detialed content.");
            doc2.Submit();
            doc2.Approve();
            doc2.Publish();
        }
    }
}