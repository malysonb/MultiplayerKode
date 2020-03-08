using System;
using System.Collections.Generic;
using System.Text;

namespace RadikoNetcode
{
    /// <summary>
    /// Interface version 1.0
    /// </summary>
    interface IPkgInterf
    {
        public interface IByte
        {
            const byte PING = 0;
            const byte SYNC = 1;
            const byte CHAT = 2;
            const byte INPUT = 3;
            const byte HELLO = 4;
            const byte GOODBYE = 5;
            const byte JOIN = 6;
            const byte HANDSHAKE = 7;
            const byte ALERT = 8;
            const byte ERROR = 255;
        }
    }
}
