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
            //WSclient wsc = new WSclient("ws://127.0.0.1:8080",Server);
            while (Server.alive == true)
            {
                Thread.Sleep(500);
            }
            Environment.Exit(0);
        }
    }
}

