using System.Runtime.CompilerServices;
using Exercise.CreationalPattern;

namespace Exercise.BehavorialPatterns
{
    public interface IMediator
    {
        void Notify(object sender, string eventName, object? data = null);
        void Register(IColleague colleague);
    }

    public interface IColleague
    {
        string Name { get; }
        void Receive(string eventName, object? data);
    }

    public class ChatRoomMediator : IMediator
    {
        private readonly List<IColleague> _participants = [];

        public void Register(IColleague colleague)
        {
            _participants.Add(colleague);
            Console.WriteLine($"[ChatRoom] {colleague.Name} joined the room.");
        }

        public void Notify(object sender, string eventName, object? data = null)
        {
            var senderColleague = sender as IColleague;

            foreach (var participant in _participants)
            {
                if (participant == senderColleague) continue;

                if (eventName == "broadcast")
                {
                    participant.Receive("message", data);
                }
                else if (eventName == "whisper" && data is (string target, string msg))
                {
                    if (participant.Name == target)
                    {
                        participant.Receive("private", msg);
                    }
                }
            }
        }
    }
    public class ChatUser : IColleague
    {
        private readonly IMediator _mediator;
        public string Name { get; }

        public ChatUser(string name, IMediator mediator)
        {
            Name = name;
            _mediator = mediator;
            _mediator.Register(this);
        }

        public void Send(string message)
        {
            Console.WriteLine($"[{Name}] sends: \"{message}\"");
            _mediator.Notify(this, "broadcast", $"[{Name}]: {message}");
        }

        public void Whisper(string targetName, string message)
        {
            Console.WriteLine($"[{Name}] whispers to {targetName}: \"{message}\"");
            _mediator.Notify(this, "whisper", (targetName, message));
        }

        public void Receive(string eventName, object? data)
        {
            var tag = eventName == "private" ? "📩 Private" : "💬";
            Console.WriteLine($"  → [{Name}] {tag}: {data}");
        }
    }


    public static class MediatorPatternDemo
    {
        public static void Run()
        {
            var chatRoom = new ChatRoomMediator();

            var alice = new ChatUser("Alice", chatRoom);
            var bob = new ChatUser("Bob", chatRoom);
            var carol = new ChatUser("Carol", chatRoom);

            Console.WriteLine();
            alice.Send("Hey everyone!");
            Console.WriteLine();
            bob.Whisper("Carol", "Meet me in the other room.");
        }
    }
}
