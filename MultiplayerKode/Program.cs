using System;
using System.Threading;

namespace RadikoNetcode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Starting Radiko Multiplayer NetKode!]");
            UDPnetKode Server = new UDPnetKode(8484);
            while (Server.alive)
            {
                Thread.Sleep(500);
            }
            Environment.Exit(0);
        }
    }
}

