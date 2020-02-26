using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerKode
{
    /// <summary>
    /// PACKAGE MANAGER FOR SERVER
    /// </summary>
    class PackageManager
    {
        /// <summary>
        /// Generate a package to send to a client.
        /// </summary>
        /// <param name="args">Parameters to send</param>
        /// <param name="token">token for the division</param>
        /// <returns>a complete string token at a character</returns>
        public string GenerateMessage(char token = '|',params object[] args)
        {
            string complete = "";
            foreach (var arg in args)
            {
                complete += arg;
                complete += token;
            }
            return complete;
        }

        /// <summary>
        /// Translates a byte array to an ASCII string.
        /// </summary>
        /// <param name="bytearray">Byte array</param>
        /// <returns>A string</returns>
        public string Translate(byte[] bytearray)
        {
            return Encoding.ASCII.GetString(bytearray);
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="message">string to be translated to a byte array</param>
        /// <returns>byte array</returns>
        public byte[] GetBytes(string _string)
        {
            return Encoding.ASCII.GetBytes(_string);
        }

        public byte[] GetBytesFromMessage(char token = '|',params object[] args)
        {
            return Encoding.ASCII.GetBytes(GenerateMessage(token,args));
        }
    }
}
