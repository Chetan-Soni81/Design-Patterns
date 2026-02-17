using System;

namespace  Exercise.StructuralPatterns
{
    public interface IPaymentProcessor
    {
        void ProcessPayment(string accountNumber, decimal amount);
        bool ValidateAccount(string accountNumber);
    }

    public class InternalPaymentProcessor : IPaymentProcessor
    {
        public void ProcessPayment(string accountNumber, decimal amount)
        {
            Console.WriteLine($"Internal: Processing ${amount} for account {accountNumber}.");
        }
        
        public bool ValidateAccount(string accountNumber)
        {
            Console.WriteLine($"Internal: Validating account {accountNumber}");
            return true;
        }
    }    

    public class ThirdPartyPaymentSystem
    {
        public void MakePayment(int userId, double amountInCents)
        {
            Console.WriteLine($"Third Party: Charging {amountInCents} cents to user {userId}.");
        }
        
        public bool CheckUserExists(int userId)
        {
            Console.WriteLine($"Third Party: Checking user {userId}");
            return true;
        }
    }

    public class PaymentAdapter : IPaymentProcessor
    {
        private readonly ThirdPartyPaymentSystem _thirdPartySystem;

        public PaymentAdapter(ThirdPartyPaymentSystem thirdPartyPaymentSystem)
        {
            _thirdPartySystem = thirdPartyPaymentSystem;
        }

        public void ProcessPayment(string accountNumber, decimal amount) {
            int userId = int.Parse(accountNumber);

            double amountInCents = (double)(amount * 100);

            _thirdPartySystem.MakePayment(userId, amountInCents);
        }

        public bool ValidateAccount(string accountNumber)
        {
            int userId = int.Parse(accountNumber);

            return _thirdPartySystem.CheckUserExists(userId);
        }
    }

    public class AdapterDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Adapter Pattern Demo ===\n");

            IPaymentProcessor internalProcessor = new InternalPaymentProcessor();
            internalProcessor.ProcessPayment("ACC12345", 100.50m);

            Console.WriteLine();

            ThirdPartyPaymentSystem thirdParty = new ThirdPartyPaymentSystem();
            IPaymentProcessor adapterPayment = new PaymentAdapter(thirdParty);
            adapterPayment.ProcessPayment("54321", 100.50m);
        }
    }
}
