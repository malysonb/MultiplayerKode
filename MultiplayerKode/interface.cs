using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerKode
{
    interface @interface
    {
        public interface ISignal
        {
            const byte PING = 0;
            const byte SYNC = 1;
            const byte CHAT = 2;
            const byte INPUT = 3;
            const byte HELLO = 4;
            const byte GOODBYE = 5;
            const byte JOIN = 6;
        }
    }
}
