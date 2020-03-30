using System;
using System.Threading;

namespace RadikoNetcode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Starting Radiko Multiplayer Netcode!]");
            UDPnetKode Server = new UDPnetKode(8484);
            WSServer Wss = new WSServer(Server);
            while (Server.alive == true)
            {
                Thread.Sleep(500);
            }
            Environment.Exit(0);
        }
    }
}

