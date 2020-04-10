using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using WebSocket4Net;

namespace RadikoNetcode
{
    /// <summary>
    /// Can't port forward your server? use a relay server made of node.js and host somewhere.<br/>
    /// Theres a lot of sites to host that kind of servers, like on glitch.io or Heroku.
    /// </summary>
    class WSclient
    {
        WebSocket wsc;
        UDPnetKode server;
        public WSclient(string Address, UDPnetKode server)
        {
            wsc = new WebSocket(Address);
            server.Wsc = this;
            this.server = server;
            wsc.Opened += Wsc_Opened;
            wsc.Closed += Wsc_Closed;
            wsc.DataReceived += Wsc_DataReceived;
            wsc.Error += Wsc_Error;
            wsc.MessageReceived += Wsc_MessageReceived;
            wsc.Open();
        }

        private void Wsc_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Relay: " + e.Message);
            byte[] pkg = PkgMngr.GetBytes(e.Message);
            try
            {
                switch (pkg[0])
                {
                    case PkgInterf.HELLO:

                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Trying Communication:" + ex.Message);
            }
        }

        private void Wsc_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new Exception("Relay server closed");
        }

        private void Wsc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        private void Wsc_Closed(object sender, EventArgs e)
        {
            throw new Exception("Relay server closed");
        }

        private void Wsc_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to relay server");
            wsc.Send("It works");
        }
    }
}
