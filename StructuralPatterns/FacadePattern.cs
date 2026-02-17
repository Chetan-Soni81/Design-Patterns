using System;

namespace Exercise.StructuralPatterns
{
    public class DVDPlayer
    {
        public void On()
        {
            Console.WriteLine("DVD Player: Turning on.");
        }

        public void Play(string movie)
        {
            Console.WriteLine($"DVD Player: Playing {movie}");
        }

        public void Stop()
        {
            Console.WriteLine("DVD Player: Stopping playback");
        }

        public void Off()
        {
            Console.WriteLine("DVD Player: Turning off.");
        }
    }

    public class Projector
    {
        public void On()
        {
            Console.WriteLine("Projector: Turning on");
        }

        public void SetInput(string input)
        {
            Console.WriteLine($"Projector: Setting input to {input}");
        }

        public void WideScreenMode()
        {
            Console.WriteLine("Projector: Setting wide screen mode (16:9)");
        }

        public void Off()
        {
            Console.WriteLine("Projector: Turning off");
        }
    }

    public class SoundSystem
    {
        public void On()
        {
            Console.WriteLine("Sound System: Turning on");
        }

        public void SetVolume(int level)
        {
            Console.WriteLine($"Sound System: Setting volumne to {level}");
        }

        public void SetSurroundSound()
        {
            Console.WriteLine($"Sound System: Setting 5.1 surround sound");
        }

        public void Off()
        {
            Console.WriteLine("Sound System: Turning off");
        }
    }

    public class Lights
    {
        public void Dim(int level)
        {
            Console.WriteLine($"Lights: Dimming to {level}%");
        }

        public void On()
        {
            Console.WriteLine("Lights: Turing on to full brightness");
        }
    }

    public class PopcornMaker
    {
        public void On()
        {
            Console.WriteLine("Popcorn Maker: Turning on");
        }

        public void Pop()
        {
            Console.WriteLine("Popcorn Maker: Popping popcorn!");
        }

        public void Off()
        {
            Console.WriteLine("Popcorn Maker: Turing off");
        }
    }

    public class HomeTheaterFacade
    {
        private DVDPlayer _dvd;
        private Projector _projector;
        private SoundSystem _soundSystem;
        private Lights _lights;
        private PopcornMaker _popcornMaker;

        public HomeTheaterFacade(DVDPlayer dvd, Projector projector, SoundSystem soundSystem, Lights lights, PopcornMaker popcornMaker)
        {
            _dvd = dvd;
            _projector = projector;
            _soundSystem = soundSystem;
            _lights = lights;
            _popcornMaker = popcornMaker;
        }

        public void WatchMovie(string movie)
        {
            Console.WriteLine("Get ready to watch a movie...\n");

            _popcornMaker.On();
            _popcornMaker.Pop();
            _lights.Dim(10);
            _projector.On();
            _projector.WideScreenMode();
            _projector.SetInput("DVD");
            _soundSystem.On();
            _soundSystem.SetVolume(5);
            _soundSystem.SetSurroundSound();
            _dvd.On();
            _dvd.Play(movie);

            Console.WriteLine("\nMovie is now playing!");
        }

        public void EndMovie()
        {
            Console.WriteLine("\nShutting down the theater...\n");

            _popcornMaker.Off();
            _lights.On();
            _dvd.Stop();
            _dvd.Off();
            _soundSystem.Off();
            _projector.Off();

            Console.WriteLine("\nTheater is shut down!");
        }
    }

    public class FacadeDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Facade Pattern Demo ===\n");

            DVDPlayer dvd = new DVDPlayer();
            Projector projector = new Projector();
            SoundSystem soundSystem = new SoundSystem();
            Lights lights = new Lights();
            PopcornMaker popcornMaker = new PopcornMaker();

            HomeTheaterFacade homeTheaterFacade = new HomeTheaterFacade(dvd, projector, soundSystem, lights, popcornMaker);

            homeTheaterFacade.WatchMovie("Inception");

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            homeTheaterFacade.EndMovie();
        }
    }
}