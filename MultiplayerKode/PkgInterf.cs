using System;
using System.Collections.Generic;
using System.Text;

namespace RadikoNetcode
{
    interface IPkgInterf
    {
        public interface IByte
        {
            const byte PING = (byte)0;
            const byte SYNC = (byte)1;
            const byte CHAT = (byte)2;
            const byte INPUT = (byte)3;
            const byte HELLO = (byte)4;
            const byte GOODBYE = (byte)5;
            const byte JOIN = (byte)6;
            const byte HANDSHAKE = (byte)7;
            const byte ALERT = (byte)8;
        }
    }
}
