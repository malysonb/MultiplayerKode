using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerKode
{
    /// <summary>
    /// PACKAGE MANAGER FOR SERVER<br/>
    /// VERSION 1.0
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

        /// <summary>
        /// Gets bytes from a string using a token to separate them
        /// </summary>
        /// <param name="token">token char to separate the message</param>
        /// <param name="args">Arguments to send to another client</param>
        /// <returns>An array of bytes</returns>
        public byte[] GetBytesFromMessage(char token = '|',params object[] args)
        {
            return Encoding.ASCII.GetBytes(GenerateMessage(token,args));
        }

        /// <summary>
        /// Translates an array of bytes to an array of strings.
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="token">Token to separate the message</param>
        /// <returns>An array of strings</returns>
        public string[] Translate(byte[] array, char token = '|')
        {
            return Encoding.ASCII.GetString(array).Split(token);
        }
        /// <summary>
        /// Converts an array of strings inside a string.
        /// </summary>
        /// <param name="msg">String with a hidden string array</param>
        /// <param name="token">Token to separate the message</param>
        /// <returns>An array of strings</returns>
        public string[] Translate(string msg, char token = '|')
        {
            return msg.Split(token);
        }
    }
}
