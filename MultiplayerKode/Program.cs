using System;

namespace MultiplayerKode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Starting Radiko Multiplayer NetKode!]");
            UDPnetKode Server = new UDPnetKode(8484);
            while (Server.alive)
            {
                Console.Write(">");
                string command = Console.ReadLine();
                Server.send(command);
            }
            Environment.Exit(0);
        }
    }
}

