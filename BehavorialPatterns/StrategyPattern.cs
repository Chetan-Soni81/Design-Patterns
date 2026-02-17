using System;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Exercise.BehavorialPatterns
{
    public interface IPaymentStrategy
    {
        bool ProcessPayment(decimal amount);
        string GetPaymentDetails();
    }

    public class CreditCardStrategy : IPaymentStrategy
    {
        private string _cardNumber;
        private string _cvv;
        private string _expiryDate;

        public CreditCardStrategy(string cardNumber, string cvv, string expiryDate)
        {
            _cardNumber = cardNumber;
            _cvv = cvv;
            _expiryDate = expiryDate;
        }

        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount:F2}");
            Console.WriteLine($"Card: ****-****-****-{_cardNumber.Substring(_cardNumber.Length - 4)}");

            if (ValidCard())
            {
                Console.WriteLine($"✅ Payment successful via Credit Card");
                return true;
            }

            Console.WriteLine($"❌ Payment failed - Invalid card");
            return false;
        }

        private bool ValidCard()
        {
            return !string.IsNullOrEmpty(_cardNumber) && _cardNumber.Length == 16;
        }

        public string GetPaymentDetails()
        {
            return $"Credit card ending in {_cardNumber.Substring(_cardNumber.Length - 4)}";
        }
    }

    public class PayPalStrategy : IPaymentStrategy
    {
        private string _email;
        private string _password;
        public PayPalStrategy(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of ${amount:F2}");
            Console.WriteLine($"PayPal account: {_email}");

            if (Authenticate())
            {
                Console.WriteLine($"✅ Payment successful via PayPal");
                return true;
            }

            Console.WriteLine($"❌ Payment failed - Authentication error");
            return false;
        }

        private bool Authenticate()
        {
            return !string.IsNullOrEmpty(_email) && _email.Contains("@");
        }

        public string GetPaymentDetails()
        {
            return $"PayPal account: {_email}";
        }
    }

    public class CryptocurrencyStrategy : IPaymentStrategy
    {
        private string _walletAddress;
        private string _cryptoType;

        public CryptocurrencyStrategy(string walletAddress, string cryptoType)
        {
            _walletAddress = walletAddress;
            _cryptoType = cryptoType;
        }

        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing {_cryptoType} payment of {amount:F2}");
            Console.WriteLine($"Wallet: {_walletAddress.Substring(0, 8)}...{_walletAddress.Substring(_walletAddress.Length - 8)}");

            decimal cryptoAmount = _cryptoType == "BTC" ? amount / 45000 : amount / 2500;
            Console.WriteLine($"Amount in {_cryptoType}: {cryptoAmount:F2}");

            Console.WriteLine($"✅ Payment successful via Cryptocurrency");
            return true;
        }

        public string GetPaymentDetails()
        {
            return $"{_cryptoType} wallet: {_walletAddress.Substring(0, 10)}...";
        }
    }

    public class ShoppingCart
    {
        public List<(string item, decimal price)> _items = new();
        private IPaymentStrategy _paymentStrategy;

        public void AddItem(string item, decimal price)
        {
            _items.Add((item, price));
            Console.WriteLine($"Added {item} (${price:F2}) to cart");
        }

        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
            Console.WriteLine($"\nPayment method set: {strategy.GetPaymentDetails()}");
        }

        public void Checkout()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine($"Cart is empty");
                return;
            }

            if (_paymentStrategy == null)
            {
                Console.WriteLine($"Please select a payment method!");
                return;
            }

            decimal total = _items.Sum(item => item.price);

            Console.WriteLine($"\n--- Checkout ---");
            Console.WriteLine($"Items:");
            foreach (var item in _items)
            {
                Console.WriteLine($" - {item.item}: ${item.price:F2}");
            }
            Console.WriteLine($"Total: ${total:F2}\n");

            if (_paymentStrategy.ProcessPayment(total))
            {
                _items.Clear();
                Console.WriteLine($"\nOrder completed successfully!");
            }
            else
            {
                Console.WriteLine($"Order Failed. Please try another payment method.");
            }
        }
    }


    public interface ISortStrategy
    {
        void Sort(List<int> list);
        string GetName();
    }

    public class BubbleSortStrategy : ISortStrategy
    {
        public void Sort(List<int> list)
        {
            Console.WriteLine($"Sorting using Bubble Sort...");
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public string GetName() => "Bubble Sort";
    }

    public class QuickSortStrategy : ISortStrategy
    {
        public void Sort(List<int> list)
        {
            Console.WriteLine($"Sorting using Quick Sort...");
            QuickSort(list, 0, list.Count - 1);
        }

        private void QuickSort(List<int> list, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(list, low, high);
                QuickSort(list, low, pi - 1);
                QuickSort(list, pi + 1, high);
            }
        }

        private int Partition(List<int> list, int low, int high)
        {
            int pivot = list[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (list[j] < pivot)
                {
                    i++;
                    int temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }

            int temp1 = list[i + 1];
            list[i + 1] = list[high];
            list[high] = temp1;

            return i + 1;
        }

        public string GetName() => "Quick Sort";
    }

    public class SortingContext
    {
        private ISortStrategy _strategy;

        public void SetStrategy(ISortStrategy strategy)
        {
            _strategy = strategy;
        }

        public void PerformSort(List<int> data)
        {
            if (_strategy == null)
            {
                Console.WriteLine("No sorting strategy set");
                return;
            }

            Console.WriteLine($"\nUsing {_strategy.GetName()}");
            Console.WriteLine($"Before: {string.Join(", ", data)}");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _strategy.Sort(data);
            stopwatch.Stop();

            Console.WriteLine($"After: {string.Join(", ", data)}");
            Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    public class StrategyDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Strategy Pattern ===\n");

            Console.WriteLine("--- Payment Processing ---\n");

            ShoppingCart cart = new();
            cart.AddItem("Laptop", 1200m);
            cart.AddItem("Mouse", 25.00m);
            cart.AddItem("Keyboard", 75.00m);

            Console.WriteLine("\n--- Paying with Credit Card ---");
            cart.SetPaymentStrategy(new CreditCardStrategy("4242424242424242", "344", "12/27"));
            cart.Checkout();

            Console.WriteLine("\n\n---  New Order ---\n");
            cart.AddItem("Monitor", 350.00m);
            cart.AddItem("Webcam", 80.00m);

            Console.WriteLine("\n--- Paying with PayPal ---");
            cart.SetPaymentStrategy(new PayPalStrategy("chetan@test.com", "password123"));
            cart.Checkout();

            Console.WriteLine("\n\n--- New Order ---\n");
            cart.AddItem("Headphones", 150.00m);

            Console.WriteLine("\n--- Paying with Cyptocurrency ---");
            cart.SetPaymentStrategy(new CryptocurrencyStrategy("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa", "BTC"));
            cart.Checkout();

            Console.WriteLine("\n\n--- Sorting Strategy ---\n");

            SortingContext sortingContext = new();

            List<int> smallData = new List<int> {64, 34, 25, 12, 22, 11, 90};
            sortingContext.SetStrategy(new BubbleSortStrategy());
            sortingContext.PerformSort(new List<int>(smallData));

            sortingContext.SetStrategy(new QuickSortStrategy());
            sortingContext.PerformSort(new List<int>(smallData));
        }
    }
}