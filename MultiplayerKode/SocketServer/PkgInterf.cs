using System;
using System.Collections.Generic;
using System.Text;

namespace RadikoNetcode
{
    /// <summary>
    /// INTERFACE FOR RADIKO MULTIPLAYER NETCODE<br/>
    /// VERSION: 1.1
    /// </summary>
    static class PkgInterf
    {
        public const byte PING = 0;
        public const byte SYNC = 1;
        public const byte CHAT = 2;
        public const byte INPUT = 3;
        public const byte HELLO = 4;
        public const byte GOODBYE = 5;
        public const byte JOIN = 6;
        public const byte HANDSHAKE = 7;
        public const byte ALERT = 8;
        public const byte ERROR = 255;
    }
}
