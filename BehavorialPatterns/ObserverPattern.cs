using System;
using System.Linq;

namespace Exercise.BehavorialPatterns
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public class Stock : ISubject
    {
        private List<IObserver> _observers = new();
        private string _symbol;
        private decimal _price;
        private decimal _previousPrice;

        public Stock(string symbol, decimal initialPrice)
        {
            _symbol = symbol;
            _price = initialPrice;
            _previousPrice = initialPrice;
        }

        public string Symbol => _symbol;
        public decimal Price => _price;
        public decimal PreviousPrice => _previousPrice;
        public decimal ChangePercent => _previousPrice == 0 ? 0 : ((_price - _previousPrice) / _previousPrice) * 100;
        public void SetPrice(decimal newPrice)
        {
            if (_price != newPrice)
            {
                _previousPrice = _price;
                _price = newPrice;
                Notify();
            }
        }

        public void Attach(IObserver observer)
        {
            Console.WriteLine($"Stock {_symbol}: Observer attached");
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            Console.WriteLine($"Stock {_symbol}: Observer detached");
            _observers.Remove(observer);
        }

        public void Notify()
        {
            Console.WriteLine($"\nStock {_symbol}: Notifying {_observers.Count} observers...");
            foreach(var observer in _observers)
            {
                observer.Update(this);
            }
        }
    }

    public class InvestorObserver : IObserver
    {
        private string _name;
        private decimal _priceThreshold;

        public InvestorObserver(string name, decimal priceThreshold)
        {
            _name = name;
            _priceThreshold = priceThreshold;
        }
        public void Update(ISubject subject)
        {
            if (subject is Stock stock)
            {
                Console.WriteLine($"[Investor {_name}] {stock.Symbol} is now ${stock.Price:F2}");

                if (stock.Price >= _priceThreshold)
                {
                    Console.WriteLine($"[Investor {_name}] ALERT: Price reached threshold ${_priceThreshold}!");
                }
            }
        }
    }

    public class DisplayObserver : IObserver
    {
        public void Update(ISubject subject)
        {
            if (subject is Stock stock)
            {
                string trend = stock.ChangePercent > 0 ? "📈" :
                    stock.ChangePercent < 0 ? "📉" : "➡️";

                Console.WriteLine($"[Display] {stock.Symbol}: ${stock.Price:F2} $({stock.ChangePercent:+0.00;-0.00;0}%) {trend}");
            }
        }
    }

    public class AnalyticsObserver : IObserver
    {
        private Dictionary<string, List<decimal>> _priceHistory = new();

        public void Update(ISubject subject)
        {
            if (subject is Stock stock)
            {
                if (!_priceHistory.ContainsKey(stock.Symbol))
                {
                    _priceHistory[stock.Symbol] = new List<decimal>();
                }

                _priceHistory[stock.Symbol].Add(stock.Price);

                var history = _priceHistory[stock.Symbol];

                if(history.Count >= 3)
                {
                    decimal avg  = history.TakeLast(3).Average();
                    Console.WriteLine($"[Analytics] {stock.Symbol} 3-period moving average: ${avg:F2}");
                }
            }
        }
    }

    public class StockWithEvents
    {
        public event EventHandler<StockPriceChangedEventArgs> PriceChanged;
        private string _symbol;
        private decimal _price;

        public StockWithEvents(string symbol, decimal price)
        {
            _symbol = symbol;
            _price = price;
        }

        public string Symbol => _symbol;
        public decimal Price => _price;

        public void SetPrice(decimal newPrice)
        {
            if (Price != newPrice)
            {
                var oldPrice = _price;
                _price = newPrice;
                OnPriceChanged(new StockPriceChangedEventArgs(oldPrice, newPrice));
            }
        }

        protected virtual void OnPriceChanged(StockPriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }
    }

    public class StockPriceChangedEventArgs : EventArgs
    {
        public decimal OldPrice {get;}
        public decimal NewPrice {get;}
        public decimal Change => NewPrice - OldPrice;
        public decimal ChangePercent => OldPrice == 0 ? 0 : (Change/OldPrice) * 100;

        public StockPriceChangedEventArgs(decimal oldPrice, decimal newPrice)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    public class ObserverDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Observer Pattern Demo ===\n");

            // Traditional Observer Pattern
            Console.WriteLine("--- Traditional Observer Pattern ---\n");

            Stock appleStock = new("AAPL", 150.00m);
            
            InvestorObserver investor1 = new("Jhon", 155.00m);
            InvestorObserver investor2 = new("Sarah", 160.00m);
            DisplayObserver display = new();
            AnalyticsObserver analytics = new();

            appleStock.Attach(investor1);
            appleStock.Attach(investor2);
            appleStock.Attach(display);
            appleStock.Attach(analytics);

            Console.WriteLine("\n--- Price Update ---");
            appleStock.SetPrice(152.50m);
            appleStock.SetPrice(156.00m);
            appleStock.SetPrice(161.00m);

            Console.WriteLine("\n--- Detaching Investor John ---");
            appleStock.Detach(investor1);
            appleStock.SetPrice(158.50m);

            Console.WriteLine("\n\n--- C# Event-Based Observer ---\n");

            StockWithEvents msftStock = new("MSFT", 300.00m);

            msftStock.PriceChanged += (sender, e) =>
            {
                var stock = sender as StockWithEvents;
                Console.WriteLine($"[Event Subscriber 1] {stock.Symbol}: ${e.OldPrice:F2} -> ${e.NewPrice:F2}");
            };

            msftStock.PriceChanged += (sender, e) =>
            {
              Console.WriteLine($"[Event Subscriber 2] Change {e.ChangePercent:+0.00;-0.00}%");
            };

            msftStock.SetPrice(305.50m);
            msftStock.SetPrice(298.50m);
        }
    }
}