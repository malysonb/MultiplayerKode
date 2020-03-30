using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using WebSocket4Net;

namespace RadikoNetcode
{
    class WSclient
    {
        WebSocket wsc;
        public WSclient(string Address)
        {
            wsc = new WebSocket(Address);
            wsc.Opened += Wsc_Opened;
            wsc.Open();
        }

        private void Wsc_Opened(object sender, EventArgs e)
        {
            wsc.Send("It works");
        }
    }
}
