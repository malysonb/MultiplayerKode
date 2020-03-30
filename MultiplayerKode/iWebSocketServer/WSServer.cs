using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Fleck;

namespace RadikoNetcode
{
    /// <summary>
    /// Makes your server into a websocket server too.
    /// </summary>
    class WSServer
    {
        WebSocketServer WSS;
        UDPnetKode SocketServer;
        public WSServer(UDPnetKode Translator, int port = 8585)
        {
            SocketServer = Translator;
            WSS = new WebSocketServer("ws://0.0.0.0:" + port);
            WSS.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => Translator.sendDirect(PkgMngr.GetBytes(message[0]), PkgMngr.TrimByteArray(1,message.Length,PkgMngr.GetBytes(message)));
            });
        }
    }
}
