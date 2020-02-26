using System;

namespace MultiplayerKode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool keepAlive = true;
            Console.WriteLine("[Starting Radiko Multiplayer NetKode!]");
            UDPnetKode Server = new UDPnetKode();
            while (Server.alive)
            {
                Console.Write(">");
                string command = Console.ReadLine();
                Server.enviar(command);
            }
            Environment.Exit(0);
        }
    }
}

