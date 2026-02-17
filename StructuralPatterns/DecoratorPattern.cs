using System;

namespace Exercise.StructuralPatterns
{
    public interface ICoffee
    {
        string GetDescription();
        decimal GetCost();
    }

    public class SimpleCoffee : ICoffee
    {
        public string GetDescription()
        {
            return "Simple Coffee";
        }

        public decimal GetCost()
        {
            return 2.00m;
        }
    }

    public class Espresso : ICoffee
    {
        public string GetDescription()
        {
            return "Espresso";
        }

        public decimal GetCost()
        {
            return 3.00m;
        }
    }

    public abstract class CoffeeDecorator : ICoffee
    {
        protected ICoffee _coffee;

        public CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }

        public virtual string GetDescription()
        {
            return _coffee.GetDescription();
        }

        public virtual decimal GetCost()
        {
            return _coffee.GetCost();
        }
    }

    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee) : base(coffee) {}

        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Milk";
        }

        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.50m;
        }
    }

    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(ICoffee coffee) : base(coffee) {}

        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Sugar";
        }

        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.25m;
        }
    }

    public class WhipCreamDecorator : CoffeeDecorator
    {
        public WhipCreamDecorator(ICoffee coffee) : base(coffee) {}

        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Whip Cream";
        }

        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.75m;
        }
    }

    public class CaramelDecorator : CoffeeDecorator
    {
        public CaramelDecorator(ICoffee coffee) : base(coffee) {}

        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Caramel";
        }

        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.60m;
        }
    }

    public class DecoratorDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Decorator Pattern Demo ===\n");

            // SimpleCoffee
            ICoffee coffee1 = new SimpleCoffee();
            Console.WriteLine($"{coffee1.GetDescription()} - ${coffee1.GetCost()}");

            // Coffee with milk
            ICoffee coffee2 = new MilkDecorator(new SimpleCoffee());
            Console.WriteLine($"{coffee2.GetDescription()} - ${coffee2.GetCost()}");

            // Coffee with milk and sugar
            ICoffee coffee3 = new SugarDecorator(new MilkDecorator(new SimpleCoffee()));
            Console.WriteLine($"{coffee3.GetDescription()} - ${coffee3.GetCost()}");

            // Espresso with everything
            ICoffee coffee4 = new CaramelDecorator(
                new WhipCreamDecorator(
                    new MilkDecorator(
                        new SugarDecorator(
                            new Espresso()
                        )
                    )
                )
            );
            Console.WriteLine($"{coffee4.GetDescription()} - ${coffee4.GetCost()}");
        }
    }
}