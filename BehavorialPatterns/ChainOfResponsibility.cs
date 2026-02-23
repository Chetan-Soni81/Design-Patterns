using System;
using System.ComponentModel;

namespace Exercise.BehavorialPatterns
{
    public abstract class SupportHandler
    {
        protected SupportHandler _nextHandler;
        public void SetNext(SupportHandler handler)
        {
            _nextHandler = handler;
        }

        public abstract void HandleRequest(SupportTicket ticket);
    }

    public class SupportTicket
    {
        public int Id {get; set; }
        public string CustomerName { get; set; }
        public string Issue { get; set; }
        public Priority Priority {get; set; }
        public TicketType Type { get; set; }
        public bool IsResolved { get; set; }

        public SupportTicket(int id, string customerName, string issue, Priority priority, TicketType type)
        {
            Id = id;
            CustomerName = customerName;
            Issue = issue;
            Priority = priority;
            Type = type;
            IsResolved = false;
        }

        public void Resolve()
        {
            IsResolved = true;
        }
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TicketType
    {
        Technical,
        Billing,
        Account,
        General
    }

    public class Level1SupportHandler : SupportHandler
    {
        public override void HandleRequest(SupportTicket ticket)
        {
            Console.WriteLine($"\n[Level 1 Support] Checking Ticket #{ticket.Id}...");

            if (ticket.Priority == Priority.Low && ticket.Type == TicketType.General)
            {
                Console.WriteLine($"✅ [Level 1 Support] Resolved: {ticket.Issue}");
                Console.WriteLine($"    Customer: {ticket.CustomerName}");
                Console.WriteLine($"    Action: Provided FAQ link and basic trobleshooting");
                ticket.Resolve();
            }
            else
            {
                Console.WriteLine($"⚠️ [Level 1 Support] Cannot handle - escalating...");
                if (_nextHandler != null)
                {
                    _nextHandler.HandleRequest(ticket);
                }
            }
        }
    }

    public class Level2SupportHandler : SupportHandler
    {
        public override void HandleRequest(SupportTicket ticket)
        {
            Console.WriteLine($"\n[Level 2 Support] Checking ticket #{ticket.Id}...");

            if((ticket.Priority == Priority.Low || ticket.Priority == Priority.Medium) && ticket.Type == TicketType.Technical)
            {
                Console.WriteLine($"✅ [Level 2 Support] Resolved: {ticket.Issue}");
                Console.WriteLine($"    Customer: {ticket.CustomerName}");
                Console.WriteLine($"    Action: Perform System diagnostics and applied fix");
                ticket.Resolve();
            }
            else
            {
                Console.WriteLine($"⚠️ [Level 2 Support] Cannot handle - escalating...");

                if (_nextHandler != null)
                {
                    _nextHandler.HandleRequest(ticket);
                }
            }
        }
    }

    public class Level3SupportHandler : SupportHandler
    {
        public override void HandleRequest(SupportTicket ticket)
        {
            Console.WriteLine($"\n[Level 3 Support] Checking ticket #{ticket.Id}...");

            if(ticket.Priority == Priority.High)
            {
                Console.WriteLine($"✅ [Level 3 Support] Resolved: {ticket.Issue}");
                Console.WriteLine($"    Customer: {ticket.CustomerName}");
                Console.WriteLine($"    Action: Deep technical analysis and custom solution deployed");
                ticket.Resolve();
            }
            else
            {
                Console.WriteLine($"⚠️ [Level 3 Support] Cannot handle - escalating...");
                if (_nextHandler != null)
                {
                    _nextHandler.HandleRequest(ticket);
                }
            }
        }
    }

    public class ManagerHandler : SupportHandler
    {
        public override void HandleRequest(SupportTicket ticket)
        {
            Console.WriteLine($"\n[Manager] Checking ticket #{ticket.Id}");

            if (ticket.Priority == Priority.Critical)
            {
                Console.WriteLine($"✅ [Manager] Taking ownership: {ticket.Issue}");
                Console.WriteLine($"    Customer: {ticket.CustomerName}");
                Console.WriteLine($"    Action: Personal intervention, emergency response team activated");
                ticket.Resolve();
            }
            else
            {
                Console.WriteLine($"❌ [Manager] Unable to handle this ticket type");
            }
        }
    }

    public class ExpenseRequest
    {
        public int Id { get; set; }
        public string EmployeeName {get;set;}
        public decimal Amount {get;set;}
        public string Description {get;set;}
        public bool IsApproved {get;set;}
        public string ApprovedBy {get;set;}

        public ExpenseRequest(int id, string employeeName, decimal amount, string description)
        {
            Id = id;
            EmployeeName = employeeName;
            Amount = amount;
            Description = description;
            IsApproved = false;
        }

        public void Approve(string approver)
        {
            IsApproved = true;
            ApprovedBy = approver;
        }
    }

    public abstract class ExpenseApprover
    {
        protected ExpenseApprover _nextApprover;
        protected string _name;
        protected decimal _approvalLimit;

        public ExpenseApprover(string name, decimal approvalLimit)
        {
            _name = name;
            _approvalLimit = approvalLimit;
        }

        public void SetNext(ExpenseApprover approver)
        {
            _nextApprover = approver;
        }

        public abstract void ProcessRequest(ExpenseRequest request);
    }

    public class TeamLeadApprover : ExpenseApprover
    {
        public TeamLeadApprover(string name) : base(name, 500m) {}

        public override void ProcessRequest(ExpenseRequest request)
        {
            Console.WriteLine($"\n[Team Lead {_name}] Reviewing expense #{request.Id}...");

            if(request.Amount <= _approvalLimit)
            {
                Console.WriteLine($"✅ Approved by {_name}");
                Console.WriteLine($"   Employee: {request.EmployeeName}");
                Console.WriteLine($"   Amount: ${request.Amount:F2}");
                Console.WriteLine($"   Description: {request.Description}");
            }
            else
            {
                Console.WriteLine($"⚠️ Amount ${request.Amount:F2} exceeds limit ${_approvalLimit:F2}");
                Console.WriteLine($"   Forwarding to next approver...");
                _nextApprover?.ProcessRequest(request);
            }
        }
    }

    public class ManagerApprover : ExpenseApprover
    {
        public ManagerApprover(string name) : base(name, 2000m) {}

        public override void ProcessRequest(ExpenseRequest request)
        {
            Console.WriteLine($"\n[Manager {_name}]  Reviewing expense #{request.Id}...");

            if (request.Amount <= _approvalLimit)
            {
                Console.WriteLine($"✅ Approved by {_name}");
                Console.WriteLine($"   Employee: {request.EmployeeName}");
                Console.WriteLine($"   Amount: ${request.Amount:F2}");
                Console.WriteLine($"   Description: {request.Description}");
                request.Approve(_name);
            }
            else
            {
                Console.WriteLine($"⚠️ Amount ${request.Amount:F2} exceeds limit ${_approvalLimit:F2}");
                Console.WriteLine($"   Forwaring to next approver...");
                _nextApprover?.ProcessRequest(request);
            }
        }
    }

    public class DirectorApprover : ExpenseApprover
    {
        public DirectorApprover(string name) : base(name, 10000m) {}

        public override void ProcessRequest(ExpenseRequest request)
        {
            Console.WriteLine($"\n[Director {_name}] Reviewing expense #{request.Id}...");

            if(request.Amount <= _approvalLimit)
            {
                Console.WriteLine($"✅ Approved by {_name}");
                Console.WriteLine($"   Employee: {request.EmployeeName}");
                Console.WriteLine($"   Amount: ${request.Amount:F2}");
                Console.WriteLine($"   Description: {request.Description}");
                request.Approve(_name);
            }
            else
            {
                Console.WriteLine($"⚠️ Amount ${request.Amount:F2} exceeds limit ${_approvalLimit:F2}");
                Console.WriteLine($"   Forwarding to CEO...");
                _nextApprover?.ProcessRequest(request);
            }
        }
    }

    public class CEOApprover : ExpenseApprover
    {
        public CEOApprover(string name) : base(name, Decimal.MaxValue){}

        public override void ProcessRequest(ExpenseRequest request)
        {
            Console.WriteLine($"\n[CEO: {_name}] Reviewing expense #{request.Id}...");
            Console.WriteLine($"✅ Approved by {_name} (Final Authority)");
            Console.WriteLine($"   Employee: {request.EmployeeName}");
            Console.WriteLine($"   Amount: ${request.Amount:F2}");
            Console.WriteLine($"   Description: {request.Description}");
            request.Approve(_name);
        }
    }

    public class ChainOfResponsibilityDemo
    {
        public static void Run()
        {
            Console.WriteLine($"=== Chain of Reponsibility Pattern Demo ===");

            Console.WriteLine($"--- Customer Support Ticket System ---");

            SupportHandler level1 = new Level1SupportHandler();
            SupportHandler level2 = new Level2SupportHandler();
            SupportHandler level3 = new Level3SupportHandler();
            SupportHandler manager = new ManagerHandler();

            level1.SetNext(level2);
            level2.SetNext(level3);
            level3.SetNext(manager);

            List<SupportTicket> tickets = new()
            {
                new SupportTicket(1, "John Doe", "How do I reset my password?", Priority.Low, TicketType.General),
                new SupportTicket(2, "Jane Smith", "Application crashes on startup", Priority.Medium, TicketType.Technical),
                new SupportTicket(3, "Bob Jhonson", "Database connection timeout", Priority.High, TicketType.Technical),
                new SupportTicket(4, "Alice Brown" , "Complete system outage affecting all users", Priority.Critical, TicketType.Technical)
            };

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"\n{new string('=', 60)}");
                Console.WriteLine($"Processig Ticket #{ticket.Id}");
                Console.WriteLine($"Customer: {ticket.CustomerName}");
                Console.WriteLine($"Issue: {ticket.Issue}");
                Console.WriteLine($"Priority: {ticket.Priority} | Type: {ticket.Type}");
                Console.WriteLine(new string('=', 60));

                level1.HandleRequest(ticket);

                if(ticket.IsResolved)
                {
                    Console.WriteLine($"\n✅ Ticket #{ticket.Id} RESOLVED");
                }
                else
                {
                    Console.WriteLine($"\n❌ Ticket #{ticket.Id} UNRESOLVED");
                }
            }

            Console.WriteLine($"\n\n\n--- Expense Approval System ---");

            ExpenseApprover teamLead = new TeamLeadApprover("Mike Peterson");
            ExpenseApprover manager2 = new ManagerApprover("Sarah Williams");
            ExpenseApprover director = new DirectorApprover("David Chen");
            ExpenseApprover ceo = new CEOApprover("Emily Rodriguez");

            teamLead.SetNext(manager2);
            manager2.SetNext(director);
            director.SetNext(ceo);

            List<ExpenseRequest> expenses = new()
            {
                new (101, "Tom Anderson", 350m, "Office supplies and equipment"),
                new (102, "Lisa Martinez", 1500m, "Conference attendence and travel"),
                new (103, "Chris Taylor", 7500m, "New workstations for team"),
                new (104, "Emma Wilson", 25000m, "Enterprise software licenses")
            };

            foreach (var expense in expenses)
            {
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"Processing Expense Request #{expense.Id}");
                Console.WriteLine($"Expense: {expense.EmployeeName}");
                Console.WriteLine($"Amount: ${expense.Amount:F2}");
                Console.WriteLine($"Description: ${expense.Description}");
                Console.WriteLine($"{new string('=', 60)}");

                teamLead.ProcessRequest(expense);

                if (expense.IsApproved)
                {
                    Console.WriteLine($"\n✅ Expense #{expense.Id}  APPROVED by {expense.ApprovedBy}");
                }
                else
                {
                    Console.WriteLine($"\n❌ Expense #{expense.Id} NOT APPROVED");
                }
            }
        }
    }
}