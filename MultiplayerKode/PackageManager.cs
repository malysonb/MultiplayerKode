using System;
using System.Text;

namespace RadikoNetcode
{
    /// <summary>
    /// PACKAGE MANAGER FOR SERVER<br/>
    /// VERSION 2.0
    /// </summary>
    /// 
    class PackageManager
    {
        /// <summary>
        /// New PackageSender
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public byte[] GenerateMessage(byte signal, params byte[][] args)
        {
            int length = 0;
            for(int i = 0; i < args.Length; i++)
            {
                length += args[i].Length;
            }
            byte[] Package = new byte[length+1];
            Package[0] = signal;
            for (int i = 0; i < args.Length; i++)
            {
                for(int j = 1; j < Package.Length; j++)
                {
                    Package[j] = args[i][j];
                }
            }
            return Package;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public byte[] TrimByteArray(int start, int end, byte[] byteArray)
        {
            byte[] result = new byte[start - end];
            for(int i = start; i < end; i++)
            {
                result[i] = byteArray[i];
            }
            return result;
        }

    }
}
